"use client"

import { PieChart } from "@/app/components/Charts/pie"
import DataTable from "@/app/components/DataTable"
import { useBuscarMovimentacoes } from "@/hooks/useBuscarMovimentacoes"
import { sortBy, totalizarMovimentacoesPorCategoria } from "@/utils/array"
import { useEffect, useState } from "react"

import ArrowDropDownIcon from '@mui/icons-material/ArrowDropDown';
import ArrowDropUpIcon from '@mui/icons-material/ArrowDropUp';
import dayjs from "dayjs"
import Dropdown from "@/app/components/Dropdown"
import { useBuscarSaldo } from "@/hooks/useBuscarSaldo"
import { Button } from "@/app/components/Button"
import { AdicionarSalarioRequest } from "@/services/salario"
import { useToast } from "@/app/components/Toast/useToast"
import { useBuscarMovimentacoesPendetes } from "@/hooks/useBuscarMovimentacoesPendetes"




export const MovimentacoesDetalhes = () => {

    const { showToast } = useToast()
    const { movimentacoes, buscar, loading } = useBuscarMovimentacoes()
    const { pendentes, buscar: buscarPendentes } = useBuscarMovimentacoesPendetes()
    const { saldo, buscar: buscarSaldo, loading: loadingSaldo } = useBuscarSaldo()
    const [periodo, setPeriodo] = useState<MovimentacoesPeriodo>('MES')
    const movimentacoesAgrupadas = totalizarMovimentacoesPorCategoria(movimentacoes, "SAIDA")


    useEffect(() => {
        buscar(periodo)
        buscarPendentes()
        buscarSaldo("Corrente")
    }, [periodo])


    const tipoTempl = (row: Movimentacao) => {
        return row.tipo == 'SAIDA'
            ? <ArrowDropDownIcon color="warning" />
            : <ArrowDropUpIcon color="success" />
    }

    const dataTmpl = (row: Movimentacao) => {
        return dayjs(row.data).format('DD/MM/YYYY')
    }

    const valorTempl = (row: Movimentacao) => {
        return `R$ ${row.valor.toFixed(2)}`
    }

    const SelectDate: SelectValues[] = [
        { id: 'SEMANA', name: 'Semana' },
        { id: 'MES', name: 'Mês' },
        { id: 'SEIS_MESES', name: 'Ultímos Seis Mêses' },
        { id: 'DURANTE_ANO', name: 'Durante o Ano' },
        { id: 'DOZE_MESES', name: 'Últimos 12 Mêses' }
    ]

    const onAdicionarSalario = async () => {
        try {
            await AdicionarSalarioRequest()
            await buscarSaldo("Corrente")
        }
        catch (err: any) {
            showToast(err.message || "Não foi possível adicionar o salário", "error")
        }
    }

    return (

        <div className="grid grid-cols-1 md:grid-cols-2  flex-1 md:max-h-full gap-4">
            <div className=" flex w-full flex-col gap-4">
                <div className="align-center items-center card flex p-4 rounded-2xl h-[8vh] bg-neutral-800/40 shadow-lg">
                    <div className="flex w-4/5 px-5 justify-between text-lg">Saldo <span>R$ {saldo?.valor.toFixed(2) || 0}</span></div>
                    <Button onClick={onAdicionarSalario} > + Salario </Button>
                </div>
                <div className="card p-4 pt-2 rounded-2xl h-[78.3vh] bg-neutral-800/40 shadow-lg">
                    <div className="block md:flex">
                        <div className=" mb-20 md:mb-0 flex flex-col justify-center w-full ">
                            <div className="m-4 flex font-semibold w-full md:mb-12">
                                <h3 className="mt-2">Resumo</h3>
                                <div className="mx-4 pr-4 font-semibold w-full ">

                                    <Dropdown
                                        onChange={e => setPeriodo(e.target.value as MovimentacoesPeriodo)}
                                        value={periodo}
                                        data={SelectDate}
                                    />
                                </div>
                            </div>
                            <div className="flex justify-center w-full h-">
                                <PieChart loading={loading} data={movimentacoesAgrupadas} />
                            </div>
                            <div className="mt-6 flex flex-col">
                            <div className="flex justify-between p-4 ">
                                <span className="w-4/5">Pendentes</span>
                                <Button> Sim </Button>
                            </div>
                                <div className="h-[30vh] scroll-smooth transparent-scrollbar">
                                    <DataTable.Root data={pendentes}>
                                        <DataTable.Column description="Valor" body={valorTempl} />
                                        <DataTable.Column description="Categoria" field="categoria" />
                                        <DataTable.Column description="Data" body={dataTmpl} />
                                    </DataTable.Root>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div>
                <div className=" p-4 rounded-2xl h-[88vh] bg-neutral-800/40 shadow-lg overflow-y-scroll scroll-smooth transparent-scrollbar">

                    <DataTable.Root data={movimentacoes} >
                        <DataTable.Column description="" body={tipoTempl} />
                        <DataTable.Column description="Valor" body={valorTempl} />
                        <DataTable.Column description="Categoria" field="categoria" />
                        <DataTable.Column description="Data" body={dataTmpl} />
                    </DataTable.Root>

                </div>
            </div>
        </div>
    )
}
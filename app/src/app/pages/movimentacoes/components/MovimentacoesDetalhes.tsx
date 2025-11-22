"use client"

import DataTable from "@/app/components/DataTable"
import { useMovimentacoesHook } from "@/hooks/useBuscarMovimentacoes"
import { sortBy, totalizarMovimentacoesPorCategoria } from "@/utils/array"
import { useEffect, useState } from "react"

import { IoMdArrowDropdown, IoMdArrowDropup } from "react-icons/io";
import { IoMdArrowUp } from "react-icons/io";

import dayjs from "dayjs"
import { useBuscarSaldo } from "@/hooks/useBuscarSaldo"
import { Button } from "@/app/components/Button"
import { AdicionarSalarioRequest } from "@/services/salario"
import { ProcessarMovimentacoesPendentesRequest } from "@/services/movimentacoes"
import { toast } from "sonner"
import { SelectField } from "@/app/components/Inputs/SelectField"
import { PieChart } from "@/app/components/Charts/pie"




export const MovimentacoesDetalhes = () => {

    const { movimentacoes, pendentes, buscar, loading, definirPeriodo, periodo } = useMovimentacoesHook()
    const { saldo, buscar: buscarSaldo, loading: loadingSaldo } = useBuscarSaldo()
    const movimentacoesAgrupadas = totalizarMovimentacoesPorCategoria(movimentacoes, "SAIDA")

    const [pendentesLoading, setPendentesLoadgind] = useState(false)
    const [salarioLoading, setSalarioLoadgind] = useState(false)


    useEffect(() => {
        buscar()
        buscarSaldo("Corrente")
    }, [periodo])

    const processarMovimentacoesPendentes = async () => {
        try {
            setPendentesLoadgind(true)
            await ProcessarMovimentacoesPendentesRequest()
            await buscar()
        }
        catch (err: any) {
            toast(err.message)
        }
        finally {
            setPendentesLoadgind(false)
        }
    }

    const onAdicionarSalario = async () => {
        try {
            setSalarioLoadgind(true)
            await AdicionarSalarioRequest()
            await buscarSaldo("Corrente")
        }
        catch (err: any) {
            toast(err.message)
        }
        finally {
            setSalarioLoadgind(false)
        }
    }

    const tipoTempl = (row: Movimentacao) => {
        return row.tipo == 'SAIDA'
            ? <IoMdArrowDropdown color="warning" className="text-bold text-[#ec9b30]" />
            : <IoMdArrowDropup color="success" className="text-[#39c429]" />
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



    return (

        <div className="grid grid-cols-1 md:grid-cols-2  flex-1 md:max-h-full gap-4">
            <div className=" flex w-full flex-col gap-4">
                <div className="align-center items-center card flex p-4 rounded-2xl h-[8vh] bg-neutral-800/40 shadow-lg">
                    <div className="flex w-4/5 px-5 justify-between text-lg">Saldo <span>R$ {saldo?.valor.toFixed(2) || 0}</span></div>
                    <Button disabled={salarioLoading} onClick={onAdicionarSalario} > + Salario </Button>
                </div>
                <div className="card border-1 border-neutral-600/70 p-4 pt-2 rounded-2xl h-[78.3vh] bg-neutral-800/40 shadow-lg">
                    <div className="block md:flex">
                        <div className=" mb-20 md:mb-0 flex flex-col justify-center w-full ">
                            <div className="m-4 flex font-semibold w-full md:mb-12">
                                <h3 className="mt-2">Saldo</h3>
                                <div className="mx-4 pr-4 font-semibold w-full ">
                                    <SelectField
                                        classname="bg-neutral-700/50 border-none focus:outline-none focus:border-none"
                                        onChange={(e) => definirPeriodo(e as MovimentacoesPeriodo)}
                                        value={periodo}
                                        data={SelectDate}
                                        placeholder="Tipo de Movimentação "

                                    />
                                </div>
                            </div>
                            <div className="flex justify-center w-full h-80">
                                <PieChart loading={loading} data={movimentacoesAgrupadas} />
                            </div>
                            <div className="mt-6 flex flex-col">
                                <div className="flex justify-between p-4 ">
                                    <span className="w-4/5">Pendentes</span>
                                    <Button disabled={pendentesLoading || pendentes.length == 0} onClick={processarMovimentacoesPendentes}> Pagar </Button>
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
                <div className="card p-4 rounded-2xl h-[88vh] bg-neutral-800/40 shadow-lg overflow-y-scroll scroll-smooth transparent-scrollbar">

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
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




export const MovimentacoesDetalhes = () => {

    const { movimentacoes, buscar, loading } = useBuscarMovimentacoes()
    const [periodo, setPeriodo] = useState<MovimentacoesPeriodo>('MES')
    const movimentacoesAgrupadas = totalizarMovimentacoesPorCategoria(movimentacoes)

    useEffect(() => {
        console.log(periodo)
        buscar(periodo)
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

    return (

        <div className="grid grid-cols-1 md:grid-cols-2 h-full gap-4">
            <div className=" flex w-full flex-col gap-4">
                <div className="card p-4 pt-2 rounded-2xl h-[82vh] md:h-1/2 bg-neutral-800/40 shadow-lg">
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
                            <div className="flex justify-center w-full">

                                <PieChart loading={loading} data={movimentacoesAgrupadas} />
                            </div>
                        </div>
                    </div>
                </div>
                <div className="card p-4 rounded-2xl h-full md:h-1/2 bg-neutral-800/40 shadow-lg">
                    {JSON.stringify(sortBy(movimentacoes, 'valor'))}
                </div>
            </div>
            <div>
                <div className="transparent-scrollbar p-4 rounded-2xl h-[88vh] bg-neutral-800/40 shadow-lg overflow-y-scroll scroll-smooth">

                    <DataTable.Root data={movimentacoes} >
                        <DataTable.Column description="asd" body={tipoTempl} />
                        <DataTable.Column description="Valor" body={valorTempl} />
                        <DataTable.Column description="Categoria" field="categoria" />
                        <DataTable.Column description="Data" body={dataTmpl} />
                    </DataTable.Root>

                </div>
            </div>
        </div>
    )
}
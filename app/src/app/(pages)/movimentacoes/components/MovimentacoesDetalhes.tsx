"use client"

import { PieChart } from "@/app/components/Charts/pie"
import DataTable from "@/app/components/DataTable"
import { useBuscarMovimentacoes } from "@/hooks/useBuscarMovimentacoes"
import { groupBy, totalizarAportesPorCategoria, totalizarMovimentacoesPorCategoria } from "@/utils/array"
import { useEffect, useState } from "react"

import ArrowDropDownIcon from '@mui/icons-material/ArrowDropDown';
import ArrowDropUpIcon from '@mui/icons-material/ArrowDropUp';
import dayjs from "dayjs"




export const MovimentacoesDetalhes = () => {

    const { movimentacoes, buscar, loading } = useBuscarMovimentacoes()

    const movimentacoesAgrupadas = totalizarMovimentacoesPorCategoria(movimentacoes)

    useEffect(() => {
        buscar()
    }, [])

    const tipoTempl = (row: Movimentacao) => {
        return row.tipo == 'SAIDA' 
        ? <ArrowDropDownIcon color="warning"/>
        : <ArrowDropUpIcon color="success"/>
    }
    
    const dataTmpl = (row: Movimentacao) => {
        return dayjs(row.data).format('DD/MM/YYYY') 
    }

    return (

        <div className="grid grid-cols-1 md:grid-cols-2 h-full gap-4">
            <div className=" flex w-full flex-col gap-4">
                <div className="card p-4 pt-2 rounded-2xl h-[82vh] md:h-1/2 bg-neutral-800/40 shadow-lg">
                    <div className="block md:flex">
                        <div className=" mb-20 md:mb-0 flex flex-col justify-center w-full ">
                            <div className="m-4 font-semibold w-full md:mb-12">
                                <h3>Resumo</h3>
                            </div>
                            <div className="flex justify-center w-full">
                                <PieChart loading={loading} data={movimentacoesAgrupadas} />
                            </div>
                        </div>
                    </div>
                </div>
                <div className="card p-4 rounded-2xl h-full md:h-1/2 bg-neutral-800/40 shadow-lg">

                </div>
            </div>
            <div>
                <div className="transparent-scrollbar p-4 rounded-2xl h-[88vh] bg-neutral-800/40 shadow-lg overflow-y-scroll scroll-smooth">

                    <DataTable.Root data={movimentacoes} >
                        <DataTable.Column description="asd" body={tipoTempl}/>
                        <DataTable.Column description="Valor" field="valor"/>
                        <DataTable.Column description="Categoria"  field="categoria"/>
                        <DataTable.Column description="Data" body={dataTmpl}/>
                    </DataTable.Root>

                </div>
            </div>
        </div>
    )
}
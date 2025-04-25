"use client"

import { PieChart } from "@/app/components/Charts/pie"
import { CustomDataTable, FieldProps } from "@/app/components/DataTable"
import Dropdown from "@/app/components/Dropdown"
import { useAportesHook } from "@/hooks/UseBuscarAportes"
import { useBuscarMovimentacoes } from "@/hooks/useBuscarMovimentacoes"
import { groupBy } from "@/utils/array"
import { useEffect, useState } from "react"

const fields: FieldProps[] = [
    { field: "identificador", description: "Ticker" },
    { field: "precoMedio", description: "Preço Médio" },
    { field: "precoAtual", description: "Preço Atual" },
    { field: "quantidade", description: "Quantidade" },
]

export const MovimentacoesDetalhes = () => {

    const { movimentacoes, buscar, loading } = useBuscarMovimentacoes()

    const movimentacoesAgrupadas = groupBy([], 'categoria')

    useEffect(() => {
        buscar()
    }, [])

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
                                <PieChart loading={loading} data={[]} />
                            </div>
                        </div>
                    </div>
                </div>
                <div className="card p-4 rounded-2xl h-full md:h-1/2 bg-neutral-800/40 shadow-lg">

                </div>
            </div>
            <div>
                <div className="transparent-scrollbar p-4 rounded-2xl h-[88vh] bg-neutral-800/40 shadow-lg overflow-y-scroll scroll-smooth">

                    <CustomDataTable data={movimentacoes} fields={fields} />

                </div>
            </div>
        </div>
    )
}
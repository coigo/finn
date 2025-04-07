"use client"
import { CustomDataTable, FieldProps } from "@/app/components/DataTable"
import { useAportesHook } from "@/hooks/UseBuscarAportes"
import { groupBy } from "@/utils/array"
import { useEffect } from "react"


const fields: FieldProps[] = [
        {field: "identificador", description: "Ticker"},
        {field: "precoMedio", description: "Preço Médio"},
        {field: "precoAtual", description: "Preço Atual"},
        {field: "quantidade", description: "Quantidade"},
    ]

export const AportesDetalhes = () => {

    const { aportes, buscar, loading } = useAportesHook()
    const aportesAgrupados = groupBy(aportes, "categoria")

    useEffect(() => {
        buscar()
    }, [])

    return (
        
        <div className="grid grid-cols-1 md:grid-cols-2 h-full gap-4">
            <div className=" flex w-full flex-col gap-4">
                <div className="card p-4 rounded-2xl h-[82vh] md:h-1/2 bg-neutral-800/40 shadow-lg">
                </div>
                <div className="card p-4 rounded-2xl h-full md:h-1/2 bg-neutral-800/40 shadow-lg">
                </div>
            </div>
            <div>
                <div className="transparent-scrollbar p-4 rounded-2xl h-[88vh] bg-neutral-800/40 shadow-lg overflow-y-scroll scroll-smooth">

                    { Object.keys(aportesAgrupados).map(a => {
                        return <div className="p-2">
                            <div className="p-2 w-full bg-neutral-500/10 rounded-lg">
                                <h3 className="ml-4 font-semibold">{a}</h3>

                            </div>

                            <CustomDataTable  data={aportesAgrupados[a]} fields={fields}/>
                        </div> 
                    }) }

                </div>
            </div>
        </div>
    )
}
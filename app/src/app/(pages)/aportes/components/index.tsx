"use client"
import { PieChart } from "@/app/components/Charts/pie"
import { CustomDataTable, FieldProps } from "@/app/components/DataTable"
import { useAportesHook } from "@/hooks/UseBuscarAportes"
import { countBy, groupBy } from "@/utils/array"
import { useEffect, useState } from "react"


const fields: FieldProps[] = [
    { field: "identificador", description: "Ticker" },
    { field: "precoMedio", description: "Preço Médio" },
    { field: "precoAtual", description: "Preço Atual" },
    { field: "quantidade", description: "Quantidade" },
]

export const AportesDetalhes = () => {

    const { aportes, buscar, loading } = useAportesHook()
    const [categoria, setCategoria] = useState<string>("")
    
    
    const aportesAgrupados = groupBy(aportes, "categoria")
    const gruposPorcategoria = countBy(aportes, "categoria")


    useEffect(() => {
        buscar()
    }, [])

    useEffect(() => {
        if (aportes.length) {
            setCategoria(aportes[0].categoria)
        }
    }, [aportes])
    
    return (

        <div className="grid grid-cols-1 md:grid-cols-2 h-full gap-4">
            <div className=" flex w-full flex-col gap-4">
                <div className="card p-4 rounded-2xl h-[82vh] md:h-1/2 bg-neutral-800/40 shadow-lg">
                    <div className="block md:flex">
                        <div className="flex flex-col justify-center w-full md:w-1/2">
                            <div className="m-4 font-semibold w-full md:mb-12">
                                <h3>Geral</h3>
                            </div>
                            <div className="flex justify-center w-full">
                                <PieChart loading={loading} data={gruposPorcategoria} />
                            </div>

                        </div>
                        <div className="flex flex-col justify-center w-full md:w-1/2">
                            {JSON.stringify(aportesAgrupados[categoria])}
                            <div className="m-4 font-semibold w-full md:mb-12">
                                <h3>{categoria}</h3>
                            </div>
                            <div className="flex justify-center w-full">
                                {/* <PieChart loading={loading} data={[] } /> */}
                            </div>

                        </div>

                    </div>



                </div>
                <div className="card p-4 rounded-2xl h-full md:h-1/2 bg-neutral-800/40 shadow-lg">

                </div>
            </div>
            <div>
                <div className="transparent-scrollbar p-4 rounded-2xl h-[88vh] bg-neutral-800/40 shadow-lg overflow-y-scroll scroll-smooth">

                    {Object.keys(aportesAgrupados).map((cat, i) => {
                        return <div key={cat + i} className="p-2">
                            <div className="p-2 w-full bg-neutral-500/10 rounded-lg">
                                <h3 className="ml-4 font-semibold">{cat}</h3>

                            </div>

                            <CustomDataTable data={aportesAgrupados[cat]} fields={fields} />
                        </div>
                    })}

                </div>
            </div>
        </div>
    )
}
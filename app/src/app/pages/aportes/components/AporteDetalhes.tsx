"use client"
import { PieChart } from "@/app/components/Charts/pie"
import Datatable from "@/app/components/DataTable"
import { SelectField } from "@/app/components/Inputs/SelectField"
import { useAportesHook } from "@/hooks/UseBuscarAportes"
import { filterBy, groupBy, sortBy, totalizarAportes, totalizarAportesPorCategoria } from "@/utils/array"
import { useEffect, useState } from "react"


export const AportesDetalhes = () => {

    const { aportes, buscar, loading } = useAportesHook()
    const [categoria, setCategoria] = useState<string>("ACAO")


    const aportesAgrupados = groupBy(aportes, "categoria")
    const quantidadeCategoria = totalizarAportesPorCategoria(aportes)
    const categoriaDetalhes = totalizarAportes(filterBy(aportes, "categoria", categoria))

    useEffect(() => {
        buscar()
    }, [])

    useEffect(() => {
        if (aportes.length) {
            setCategoria(aportes[0].categoria)
        }

    }, [aportes])

    const precoMedioTempl = (row:AporteTotalizado) => {
        return `R$ ${row.precoMedio.toFixed(2)}`
    }

    const precoAtualTempl = (row:AporteTotalizado) => {
        return `R$ ${row.precoAtual.toFixed(2)}`
    }

    const totalTempl = (row:AporteTotalizado) => {
        return `R$ ${row.total.toFixed(2)}`
    }

    return (

        <div className="grid grid-cols-1 md:grid-cols-2 h-full gap-4">
            <div className=" flex w-full flex-col gap-4">
                <div className="card p-4 pt-2 rounded-2xl h-[82vh] md:h-1/2 bg-neutral-800/40 shadow-lg">
                    <div className="block md:flex">
                        <div className=" mb-20 md:mb-0 flex flex-col justify-center w-full md:w-1/2">
                            <div className="m-4 font-semibold w-full md:mb-12">
                                <h3>Geral</h3>
                            </div>
                            <div className="flex justify-center w-full">
                                <PieChart loading={loading} data={quantidadeCategoria} />
                            </div>

                        </div>
                        <div className=" mb-20 md:mb-0 flex flex-col justify-center w-full md:w-1/2">
                            <div className="m-4 pr-4 font-semibold w-full md:mb-12">

                                <SelectField
                                    placeholder="Tipo de Aporte" 
                                    onChange={e => setCategoria(e)}
                                    value={categoria}
                                    data={Object.keys(aportesAgrupados).map(c => { return { id: c, name: c } } )}
                                    
                                />

                            </div>
                            <div className="flex justify-center w-full">
                                <PieChart loading={loading} data={categoriaDetalhes} />
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

                            <Datatable.Root data={aportesAgrupados[cat].reverse()} >
                                <Datatable.Column description="Ticker" field="identificador"/>
                                <Datatable.Column description="Total" body={totalTempl}/>
                                <Datatable.Column description="Preço Médio" body={precoMedioTempl}/>
                                {
                                    cat != "CRIPTOMOEDA" 
                                        ? <Datatable.Column description="Preço Atual" body={precoAtualTempl}/> 
                                        : <></>
                                }
                                <Datatable.Column description="Quantidade" field="quantidade"/>

                            </Datatable.Root>
                        </div>
                    })}

                </div>
            </div>
        </div>
    )
}
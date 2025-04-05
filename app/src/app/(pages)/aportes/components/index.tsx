"use client"
import { CustomDataTable, FieldProps } from "@/app/components/DataTable"
import { groupBy } from "@/utils/array"

    const data = [
        {id:1, nome:"oi", grupo: "ACOES"},
        {id:2, nome:"oii", grupo: "ACOES"},
        {id:3, nome:"oiii", grupo: "ACOES"},
        {id:4, nome:"oiiii", grupo: "ACOES"},
        {id:5, nome:"oiiiii", grupo: "FII"},
        {id:6, nome:"oiiiiii", grupo: "FII"},
        {id:6, nome:"oiiiiii", grupo: "FII"},
        {id:6, nome:"oiiiiii", grupo: "ACOES"},
        {id:6, nome:"oiiiiii", grupo: "ACOES"},
        {id:6, nome:"oiiiiii", grupo: "FII"},
        {id:6, nome:"oiiiiii", grupo: "FII"},
        {id:6, nome:"oiiiiii", grupo: "FII"},
        {id:6, nome:"oiiiiii", grupo: "Cripto"},
        {id:6, nome:"oiiiiii", grupo: "ACOES"},
        {id:6, nome:"oiiiiii", grupo: "Cripto"},
        {id:6, nome:"oiiiiii", grupo: "ACOES"},
        {id:6, nome:"oiiiiii", grupo: "FII"},
        {id:6, nome:"oiiiiii", grupo: "FII"},
        {id:6, nome:"oiiiiii", grupo: "FII"},
        {id:6, nome:"oiiiiii", grupo: "FII"},
    ]
    const fields: FieldProps[] = [
        {field: "id", description: "Id"},
        {field: "nome", description: "Nome"},
    ]

export const AportesDetalhes = () => {

    const aportes = groupBy(data, "grupo")
    
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

                    { Object.keys(aportes).map(a => {
                        return <div className="p-2">
                            <div className="p-2 w-full bg-neutral-500/10 rounded-lg">
                                <h3 className="ml-4 font-semibold">{a}</h3>

                            </div>

                            <CustomDataTable  data={aportes[a]} fields={fields}/>
                        </div> 
                    }) }

                </div>
            </div>
        </div>
    )
}
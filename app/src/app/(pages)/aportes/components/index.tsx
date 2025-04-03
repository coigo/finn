import { CustomDataTable, FieldProps } from "@/app/components/DataTable"

    const data = [
        {id:1, nome:"oi"},
        {id:2, nome:"oii"},
        {id:3, nome:"oiii"},
        {id:4, nome:"oiiii"},
        {id:5, nome:"oiiiii"},
        {id:6, nome:"oiiiiii"},
        {id:6, nome:"oiiiiii"},
        {id:6, nome:"oiiiiii"},
        {id:6, nome:"oiiiiii"},
        {id:6, nome:"oiiiiii"},
        {id:6, nome:"oiiiiii"},
        {id:6, nome:"oiiiiii"},
        {id:6, nome:"oiiiiii"},
        {id:6, nome:"oiiiiii"},
    ]
    const fields: FieldProps[] = [
        {field: "id", description: "Id", align: "right"},
        {field: "nome", description: "Nome"},
    ]

export const AportesDetalhes = () => {
    return (
        <div className="grid grid-cols-1 md:grid-cols-2 h-full gap-4">
            <div className="flex w-full  flex-col gap-4">
                <div className="p-4  rounded-2xl h-[82vh] md:h-1/2 bg-neutral-800/40 shadow-lg">

                </div>
                <div className="p-4 rounded-2xl h-full md:h-1/2 bg-neutral-800/40 shadow-lg">

                </div>
            </div>
            <div>
                <div className="p-4 rounded-2xl h-[88vh] bg-neutral-800/40 shadow-lg">
                    <CustomDataTable  data={data} fields={fields}/>

                </div>
            </div>
        </div>
    )
}
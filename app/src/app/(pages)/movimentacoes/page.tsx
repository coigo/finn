import { CustomDataTable, FieldProps } from "@/app/components/DataTable";

export default function Movimentacoes() {

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
    
    return (
        <CustomDataTable  data={data} fields={fields}/>
        
    )
}
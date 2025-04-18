import { CurrencyField } from "@/app/components/Inputs/CurrencyField"
import { DateField } from "@/app/components/Inputs/DateField"
import dayjs from "dayjs"
import { Control, Controller, FieldValues, UseFormHandleSubmit } from "react-hook-form"
import { z } from "zod"
import { SelectField } from "@/app/components/Inputs/SelectField"

const schema = z.object({
    identificador: z.string(),
    categoriaId: z.number(),
    preco: z.number(),
    dataCompra: z.custom<dayjs.Dayjs>(),
})

export type RecorrenteForm = z.infer<typeof schema>

type FormProps = {
    config: {
        control: Control<FieldValues, any, FieldValues>
        handleSubmit: UseFormHandleSubmit<FieldValues, FieldValues>
        onSubmit: (data: FieldValues) => void
    }
    categorias: MovimentacaoCategoria[]
}

export const FormRecorrente = ({ config: { control, handleSubmit, onSubmit }, categorias }: FormProps)  => {
    return (

        <form onSubmit={() => handleSubmit(onSubmit)} className="flex flex-col md:flex-row gap-3 justify-evenly">

        <Controller
            name="categoriaId"
            control={control}
            render={({ field }) => (
                <SelectField
                    data={categorias}
                    label="Quantidade"
                    {...field}
                />
            )}
        />
        <Controller
            name="preco"
            control={control}
            render={({ field }) =>
                <CurrencyField
                    {...field}
                    label="Preço"
                />
            }
        />
        <Controller
            name="dataCompra"
            control={control}
            render={({ field }) =>
                <DateField
                    {...field}
                    label="Data da Compra"

                />
            }
        />

    </form>

    )
}
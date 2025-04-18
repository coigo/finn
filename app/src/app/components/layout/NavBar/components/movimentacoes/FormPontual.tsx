import { CurrencyField } from "@/app/components/Inputs/CurrencyField"
import { NumberField } from "@/app/components/Inputs/NumberField"
import { DateField } from "@/app/components/Inputs/DateField"
import { TextField } from "@/app/components/Inputs/TextField"
import dayjs from "dayjs"
import { Control, Controller, FieldValues, UseFormHandleSubmit } from "react-hook-form"
import { z } from "zod"
import { SelectField } from "@/app/components/Inputs/SelectField"

const schema = z.object({
    categoriaId: z.number(),
    valor: z.number(),
    data: z.custom<dayjs.Dayjs>(),
})

export type PontualForm = z.infer<typeof schema>

type FormProps = {
    config: {
        control: Control<FieldValues, any, FieldValues>
        handleSubmit: UseFormHandleSubmit<FieldValues, FieldValues>
        onSubmit: (data: FieldValues) => void
    }
    categorias: MovimentacaoCategoria[]
}

export const FormPontual = ({ config: { control, handleSubmit, onSubmit }, categorias }: FormProps) => {
    return (

        <form onSubmit={() => handleSubmit(onSubmit)} className="flex flex-col md:flex-row gap-3 justify-evenly">
        <Controller
            name="categoriaId"
            control={control}
            render={({ field }) => (
                <SelectField
                    data={categorias}
                    label="Categoria"
                    {...field}
                />
            )}
        />
        <Controller
            name="valor"
            control={control}
            render={({ field }) =>
                <CurrencyField
                    {...field}
                    label="Valor"
                />
            }
        />
        <Controller
            name="data"
            control={control}
            render={({ field }) =>
                <DateField
                    {...field}
                    label="Data"

                />
            }
        />

    </form>

    )
}
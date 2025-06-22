import { CurrencyField } from "@/app/components/Inputs/CurrencyField"
import { DateField } from "@/app/components/Inputs/DateField"
import dayjs from "dayjs"
import { Control, Controller, FieldValues, UseFormHandleSubmit } from "react-hook-form"
import { z } from "zod"
import { SelectField } from "@/app/components/Inputs/SelectField"
import { TextField } from "@/app/components/Inputs/TextField"

export const persistenteSchema = z.object({
    descricao: z.string(),
    categoriaId: z.number(),
    valor: z.string(),
    dataCompra: z.custom<dayjs.Dayjs>(),
})

export type PersistenteForm = z.infer<typeof persistenteSchema>

type FormProps = {
    config: {
        control: Control<FieldValues, any, FieldValues>
        handleSubmit: UseFormHandleSubmit<FieldValues, FieldValues>
        onSubmit: (data: FieldValues) => void
    }
    categorias: MovimentacaoCategoria[]
}

export const FormPersistente = ({ config: { control, handleSubmit, onSubmit }, categorias }: FormProps) => {
    return (

        <form onSubmit={() => handleSubmit(onSubmit)} className="flex flex-col md:flex-row flex-wrap gap-3 justify-evenly">

            <Controller
                name="categoriaId"
                control={control}
                render={({ field }) => (
                    <SelectField
                        placeholder="Categoria"
                        data={categorias}
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
                        placeholder="valor"
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
            <Controller
                name="descricao"
                control={control}
                render={({ field }) =>
                    <TextField
                        {...field}
                        className="w-full"
                        placeholder="Descrição"
                    />
                }
            />

        </form>

    )
}
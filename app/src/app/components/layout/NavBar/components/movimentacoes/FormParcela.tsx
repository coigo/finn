import { CurrencyField } from "@/app/components/Inputs/CurrencyField"
import { NumberField } from "@/app/components/Inputs/NumberField"
import { DateField } from "@/app/components/Inputs/DateField"
import dayjs from "dayjs"
import { Control, Controller, FieldValues, UseFormHandleSubmit } from "react-hook-form"
import { z } from "zod"
import { SelectField } from "@/app/components/Inputs/SelectField"
import { TextField } from "@/app/components/Inputs/TextField"

export const parcelaSchema = z.object({
    categoriaId: z.number(),
    descricao: z.string(),
    valor: z.number(),
    quantidadeParcelas: z.number(),
    primeiroVencimento: z.date(),
}) 

export type ParcelaForm = z.infer<typeof parcelaSchema>

type FormProps = {
    config: {
        control: Control<FieldValues, any, FieldValues>
        handleSubmit: UseFormHandleSubmit<FieldValues, FieldValues>
        onSubmit: (data: FieldValues) => void
    }
    categorias: MovimentacaoCategoria[]
}

export const FormParcela = ({ config: { control, handleSubmit, onSubmit }, categorias }: FormProps) => {
    return (

        <form  className="flex flex-col md:flex-row flex-wrap gap-3 justify-evenly">
            <Controller
                name="categoriaId"
                control={control}
                render={({ field }) => (
                    <SelectField
                        data={categorias}
                        placeholder="Categoria"
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
                        placeholder="Valor Total"
                    />
                }
            />
            <Controller
                name="quantidadeParcelas"
                control={control}
                render={({ field }) =>
                    <NumberField
                        {...field}
                        onChange={(e) => field.onChange(Number(e.target.value))}
                        placeholder="Quantidade de Parcelas"
                    />
                }
            />
            <Controller
                name="primeiroVencimento"
                control={control}
                render={({ field }) =>
                    <DateField
                        {...field}
                        label="Primeiro Vencimento"

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
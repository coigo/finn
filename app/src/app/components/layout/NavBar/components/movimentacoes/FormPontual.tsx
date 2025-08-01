import { CurrencyField } from "@/app/components/Inputs/CurrencyField"
import { DateField } from "@/app/components/Inputs/DateField"
import dayjs from "dayjs"
import { Control, Controller, FieldValues, UseFormHandleSubmit } from "react-hook-form"
import { z } from "zod"
import { SelectField } from "@/app/components/Inputs/SelectField"
import { useEffect, useState } from "react"
import { TextField } from "@/app/components/Inputs/TextField"

export const pontualSchema = z.object({
    categoriaId: z.number(),
    valor: z.number(),
    data: z.date(),
    descricao: z.string()
})

export type PontualForm = z.infer<typeof pontualSchema>

type FormProps = {
    config: {
        control: Control<FieldValues, any, FieldValues>
        handleSubmit: UseFormHandleSubmit<FieldValues, FieldValues>
        onSubmit: (data: FieldValues) => void
    }
    categorias: MovimentacaoCategoria[]
    dividendos?: boolean
}

export const FormPontual = ({ config: { control, handleSubmit, onSubmit }, categorias, dividendos }: FormProps) => {

    const [aporteSelect, setAporteSelect] = useState<boolean>(Boolean(dividendos))

    useEffect(() => {
        setAporteSelect(Boolean(dividendos))
    }, [dividendos])

    return (

        <form onSubmit={() => handleSubmit(onSubmit)} className="flex flex-col md:flex-row flex-wrap gap-3 justify-evenly">
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
            />{
                aporteSelect &&
                <Controller
                    name="identificador"
                    control={control}
                    render={({ field }) => (
                        <SelectField
                            data={categorias}
                            placeholder="Aporte"
                            {...field}
                        />
                    )}
                />
            }
            <Controller
                name="valor"
                control={control}
                render={({ field }) =>
                    <CurrencyField
                        {...field}
                        placeholder="Valor"
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
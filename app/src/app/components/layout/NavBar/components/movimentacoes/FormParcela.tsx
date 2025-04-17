import { CurrencyField } from "@/app/components/Inputs/CurrencyField"
import { NumberField } from "@/app/components/Inputs/NumberField"
import { DateField } from "@/app/components/Inputs/DateField"
import { TextField } from "@/app/components/Inputs/TextField"
import dayjs from "dayjs"
import { Control, Controller, UseFormHandleSubmit } from "react-hook-form"
import { z } from "zod"

const schema = z.object({
    identificador: z.string(),
    quantidade: z.number(),
    preco: z.number(),
    dataCompra: z.custom<dayjs.Dayjs>(),
})

export type ParcelaForm = z.infer<typeof schema>

type FormProps = {
    control: Control<ParcelaForm, any, ParcelaForm>
    handleSubmit: UseFormHandleSubmit<ParcelaForm, ParcelaForm>
    onSubmit: (data: any) => void
}

export const FormParcela = ({ handleSubmit, control, onSubmit } : FormProps) => {
    return (

        <form onSubmit={() => handleSubmit(onSubmit)} className="md:flex ">
        <Controller
            name="identificador"
            control={control}
            render={({ field }) =>
                <TextField
                    {...field}
                    label="Identificador"
                />
            }
        />
        <Controller
            name="quantidade"
            control={control}
            render={({ field }) => (
                <NumberField
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
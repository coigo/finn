import { CurrencyField } from "@/app/components/Inputs/CurrencyField"
import { DateField } from "@/app/components/Inputs/DateField"
import { NumberField } from "@/app/components/Inputs/NumberField"
import { SelectField } from "@/app/components/Inputs/SelectField"
import { TextField } from "@/app/components/Inputs/TextField"
import { ModalContent } from "@/app/components/Modal/ModalContent"
import { ModalFooter } from "@/app/components/Modal/ModalFooter"
import { ModalHeader } from "@/app/components/Modal/ModalHeader"
import { Button } from "@mui/material"
import { Controller, useForm } from "react-hook-form"
import CheckIcon from '@mui/icons-material/Check';
import { CriarAporteRequest } from "@/services/aportes"
import { useToast } from "@/app/components/Toast/useToast"
import { useBuscarAporteCategorias } from "@/hooks/useBuscarAporteCategorias"
import { useEffect } from "react"
import { useAportesHook } from "@/hooks/UseBuscarAportes"
import { z } from 'zod'
import dayjs from "dayjs"

const schema = z.object({
    identificador: z.string(),
    quantidade: z.number(),
    preco: z.number(),
    categoria: z.number(),
    dataCompra: z.custom<dayjs.Dayjs>(),
})

type AporteForm = z.infer<typeof schema>

export const AporteModal = () => {

    const { control, handleSubmit } = useForm<AporteForm>()
    const { showToast } = useToast()

    const { buscar, categorias } = useBuscarAporteCategorias()
    const { buscar: onClose } = useAportesHook()
    useEffect(() => {
        buscar()
    },[])

    const submit = async (data: AporteForm) => {
        try {
            console.log(data)
            await CriarAporteRequest(data)
            onClose()
        }
        catch (err: any) {
            showToast(err.message, "error")
            console.log(err)
        }
    }

    return (
        <>
            <ModalHeader title="Aportes" />
            <ModalContent>
                <form className=" flex flex-col md:flex-row gap-3 justify-evenly">
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
                        name="categoria"
                        control={control}
                        render={({ field }) =>
                            <SelectField
                                data={categorias}
                                label="Categoria"
                                {...field}
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
            </ModalContent>
            <ModalFooter>
                <Button startIcon={<CheckIcon />} variant="contained" type="submit" onClick={handleSubmit(submit)} color="warning"  > Enviar </Button>
            </ModalFooter>
        </>
    )

}
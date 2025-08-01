import { CurrencyField } from "@/app/components/Inputs/CurrencyField"
import { DateField } from "@/app/components/Inputs/DateField"
import { NumberField } from "@/app/components/Inputs/NumberField"
import { SelectField } from "@/app/components/Inputs/SelectField"
import { TextField } from "@/app/components/Inputs/TextField"
import { ModalContent } from "@/app/components/Modal/ModalContent"
import { ModalFooter } from "@/app/components/Modal/ModalFooter"
import { ModalHeader } from "@/app/components/Modal/ModalHeader"
import { CriarAporteRequest } from "@/services/aportes"
import { useBuscarAporteCategorias } from "@/hooks/useBuscarAporteCategorias"
import { useEffect } from "react"
import { useAportesHook } from "@/hooks/UseBuscarAportes"
import { z } from 'zod'
import { useModal } from "@/app/components/Modal/useModal"
import { toast } from "sonner"
import { Controller, useForm } from "react-hook-form"
import { Button } from "@/app/components/Button"
import { corrigirTipagem } from "@/lib/utils"

const schema = z.object({
    identificador: z.string(),
    quantidade: z.number(),
    preco: z.string(),
    categoria: z.string(),
    dataCompra: z.date(),
})

type AporteForm = z.infer<typeof schema>

export const AporteModal = () => {

    const { control, handleSubmit } = useForm<AporteForm>()
    const { closeModal } = useModal()

    const { buscar, categorias } = useBuscarAporteCategorias()
    const { buscar: onClose } = useAportesHook()
    useEffect(() => {
        buscar()
    },[])

    const submit = async (data: AporteForm) => {
        try {
            (data as any) = corrigirTipagem(data as any)
            await CriarAporteRequest(data)
            onClose()
        }
        catch (err: any) {
            toast(err.message)
        }finally {
            closeModal()
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
                                placeholder="Identificador"
                                {...field}
                            />
                        }
                    />
                    <Controller
                        name="categoria"
                        control={control}
                        render={({ field }) =>
                            <SelectField
                                placeholder="Categoria"
                                data={categorias}
                                {...field}
                            />
                        }
                    />
                    <Controller
                        name="quantidade"
                        control={control}
                        render={({ field }) => (
                            <NumberField
                                placeholder="Quantidade"
                                {...field}
                            />
                        )}
                    />
                    <Controller
                        name="preco"
                        control={control}
                        render={({ field }) =>
                            <CurrencyField
                                placeholder="Preço"
                                {...field}
                                
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
                <Button type="submit" onClick={handleSubmit(submit)} > Enviar </Button>
            </ModalFooter>
        </>
    )

}
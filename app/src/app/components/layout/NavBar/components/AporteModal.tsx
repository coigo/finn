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



export const AporteModal = () => {

    const { control, handleSubmit } = useForm()
    const { showToast } = useToast()

    const { buscar, categorias, loading } = useBuscarAporteCategorias()

    useEffect(() => {
        buscar()
    }, [])

    const submit = async (data: any) => {
        try {
            console.log(data)
            await CriarAporteRequest(data)
        }
        catch (err: any) {
            showToast(err.message, "error")
            console.log(err)
        }
    }

    return (
        <>
        {JSON.stringify(categorias)}
            <ModalHeader title="Aportes" />
            <ModalContent>
                <form className="md:flex ">
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
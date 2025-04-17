import { ModalContent } from "@/app/components/Modal/ModalContent"
import { ModalFooter } from "@/app/components/Modal/ModalFooter"
import { ModalHeader } from "@/app/components/Modal/ModalHeader"
import { Button } from "@mui/material"
import { Controller, useForm } from "react-hook-form"
import CheckIcon from '@mui/icons-material/Check';
import { CriarAporteRequest } from "@/services/aportes"
import { useToast } from "@/app/components/Toast/useToast"
import { useBuscarAporteCategorias } from "@/hooks/useBuscarAporteCategorias"
import { useEffect, useState } from "react"
import { useAportesHook } from "@/hooks/UseBuscarAportes"
import { Chips } from "@/app/components/Chips"
import { FormPontual, PontualForm } from "./movimentacoes/FormPontual"
import { FormRecorrente, RecorrenteForm } from "./movimentacoes/FormRecorrente"
import { FormParcela, ParcelaForm } from "./movimentacoes/FormParcela"



const MovimentacooesTipos: ["PONTUAL",  "PARCELADA" , "RECORRENTE"] = ["PONTUAL", "PARCELADA", "RECORRENTE"]

export const MovimentacaoModal = () => {

    const [movimentacaoTipo, setTipo] = useState<"PONTUAL" | "PARCELADA" | "RECORRENTE">("PONTUAL")

    const { control, handleSubmit, reset } = useForm<PontualForm | RecorrenteForm | ParcelaForm>()
    const { showToast } = useToast()
    const { buscar, categorias } = useBuscarAporteCategorias()
    const { buscar: onClose } = useAportesHook()

    const submit = async (data: PontualForm | RecorrenteForm | ParcelaForm) => {
        try {
            console.log(data)
            // await CriarAporteRequest(data)
            onClose()
        }
        catch (err: any) {
            showToast(err.message, "error")
            console.log(err)
        }
    }

    const onSelectTipo = (mov: "PONTUAL" | "PARCELADA" | "RECORRENTE") => {
        if (movimentacaoTipo != mov ) {
            setTipo(mov)
        }
        reset()
    }

    const forms = {
        "PONTUAL" : <FormPontual control={control} handleSubmit={handleSubmit} onSubmit={submit}/>,
        "PARCELADA" : <FormParcela control={control} handleSubmit={handleSubmit} onSubmit={submit}/>,
        "RECORRENTE" : <FormRecorrente control={control} handleSubmit={handleSubmit} onSubmit={submit}/>
    }

    return (
        <>
            <ModalHeader title="Movimentações" >
                {MovimentacooesTipos.map(mov => {
                    return (

                        <Chips
                            key={"chip_" + mov}
                            onClick={() => onSelectTipo(mov)}
                            active={mov == movimentacaoTipo}
                            label={mov.charAt(0).toUpperCase() + mov.slice(1).toLowerCase()} />
                    )
                })}
            </ModalHeader>
            <ModalContent  >
                {
                    <FormPontual control={control} handleSubmit={handleSubmit} onSubmit={submit}/>
                }
            </ModalContent>
            <ModalFooter>
                <Button startIcon={<CheckIcon />} variant="contained" type="submit" onClick={handleSubmit(submit)} color="warning"  > Enviar </Button>
            </ModalFooter>
        </>
    )

}
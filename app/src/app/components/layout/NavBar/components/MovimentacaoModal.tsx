import { ModalContent } from "@/app/components/Modal/ModalContent"
import { ModalFooter } from "@/app/components/Modal/ModalFooter"
import { ModalHeader } from "@/app/components/Modal/ModalHeader"
import { Button } from "@mui/material"
import {  FieldValues, useForm } from "react-hook-form"
import CheckIcon from '@mui/icons-material/Check';
import { useToast } from "@/app/components/Toast/useToast"
import { useEffect, useState } from "react"
import { useAportesHook } from "@/hooks/UseBuscarAportes"
import { Chips } from "@/app/components/Chips"
import { FormPontual, PontualForm } from "./movimentacoes/FormPontual"
import { FormRecorrente, RecorrenteForm } from "./movimentacoes/FormRecorrente"
import { FormParcela, ParcelaForm } from "./movimentacoes/FormParcela"
import { useBuscarMovimentacoesCategoria } from "@/hooks/useBuscarMovimentacoesCategorias"



const MovimentacooesTipos: ["PONTUAL",  "PARCELADA" , "RECORRENTE"] = ["PONTUAL", "PARCELADA", "RECORRENTE"]

export const MovimentacaoModal = () => {

    const [movimentacaoTipo, setTipo] = useState<"PONTUAL" | "PARCELADA" | "RECORRENTE">("PONTUAL")

    const { control, handleSubmit, reset } = useForm<FieldValues>()
    const { showToast } = useToast()
    const { buscar, categorias, loading } = useBuscarMovimentacoesCategoria()
    const { buscar: onClose } = useAportesHook()

    const categoriasPorFiltro = (tipo: MovimentacaoTipo) => {
        return categorias.filter(categoria => categoria.tipo == tipo) || [] as MovimentacaoCategoria[]
    }

    useEffect(() => {
        buscar()
    }, [])

    const submit = async (data: FieldValues) => {
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
            reset()
        }
    }
    
    const formConfig = {
        control, handleSubmit, onSubmit: submit
    }  

    const forms = {
        "PONTUAL" : <FormPontual config={formConfig} categorias={categoriasPorFiltro("SAIDA")}/>,
        "PARCELADA" : <FormParcela config={formConfig} categorias={categoriasPorFiltro("SAIDA")} />,
        "RECORRENTE" : <FormRecorrente config={formConfig} categorias={categoriasPorFiltro("SAIDA")} />
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
                    forms[movimentacaoTipo]
                }
            </ModalContent>
            <ModalFooter>
                <Button startIcon={<CheckIcon />} variant="contained" type="submit" onClick={handleSubmit(submit)} color="warning"  > Enviar </Button>
            </ModalFooter>
        </>
    )

}
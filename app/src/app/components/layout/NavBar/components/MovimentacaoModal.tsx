import { ModalContent } from "@/app/components/Modal/ModalContent"
import { ModalFooter } from "@/app/components/Modal/ModalFooter"
import { ModalHeader } from "@/app/components/Modal/ModalHeader"
import { Button } from "@mui/material"
import {  FieldValues, useForm, useWatch } from "react-hook-form"
import CheckIcon from '@mui/icons-material/Check';
import { useToast } from "@/app/components/Toast/useToast"
import { useEffect, useState } from "react"
import { useAportesHook } from "@/hooks/UseBuscarAportes"
import { Chips } from "@/app/components/Chips"
import { FormPontual, pontualSchema } from "./movimentacoes/FormPontual"
import { FormRecorrente, recorrenteSchema } from "./movimentacoes/FormRecorrente"
import { FormParcela, parcelaSchema } from "./movimentacoes/FormParcela"
import { useBuscarMovimentacoesCategoria } from "@/hooks/useBuscarMovimentacoesCategorias"
import { CriarMovimentacaoRequest } from "@/services/movimentacoes"

const MovimentacooesTipos: ["PONTUAL",  "PARCELADA" , "RECORRENTE", "ENTRADA"] = ["PONTUAL", "PARCELADA", "RECORRENTE", "ENTRADA"]

export const MovimentacaoModal = () => {

    const [movimentacaoTipo, setTipo] = useState<TipoMovimentacaoFormulario>("PONTUAL")

    const { control, handleSubmit, reset } = useForm<FieldValues>()
    const categoriaId =  useWatch({control, name: "categoriaId"})
    const { showToast } = useToast()
    const { buscar, categorias } = useBuscarMovimentacoesCategoria()
    const { buscar: onClose } = useAportesHook()

    const categoriasPorFiltro = (tipo: MovimentacaoTipo) => {
        return categorias.filter(categoria => categoria.tipo == tipo) || [] as MovimentacaoCategoria[]
    }

    useEffect(() => {
        buscar()
    }, [])

    const validate = (data: any) => {
        const schema = {
            "PONTUAL": pontualSchema,  
            "PARCELADA":parcelaSchema , 
            "RECORRENTE": recorrenteSchema, 
            "ENTRADA":pontualSchema
        }

        const validate = schema[movimentacaoTipo].safeParse(data)
        if (validate.error) {
            throw {
                message: `O campo ${validate.error.errors[0].path[0]} precisa ser preenchido!`
            }
        }
    }

    const submit = async (data: FieldValues) => {
        try {
            const tipo = movimentacaoTipo == "ENTRADA" ? 0 : 1 
            validate(data)
            await CriarMovimentacaoRequest({...data, tipo})
            onClose()
        }
        catch (err: any) {
            showToast(err.message, "error")
        }
    }

    const onSelectTipo = (mov: TipoMovimentacaoFormulario) => {
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
        "ENTRADA" : <FormPontual config={formConfig} categorias={categoriasPorFiltro("ENTRADA")} dividendos={categoriaId == 10}/>,
        "PARCELADA" : <FormParcela config={formConfig} categorias={categoriasPorFiltro("SAIDA")} />,
        "RECORRENTE" : <FormRecorrente config={formConfig} categorias={categoriasPorFiltro("SAIDA")} />
    }


    return (
        <>
            <ModalHeader title="Movimentações" >
                <div className="flex overflow-x-scroll md:overflow-x-hidden">

                {MovimentacooesTipos.map(mov => {
                    return (
                        
                        <Chips
                        key={"chip_" + mov}
                        onClick={() => onSelectTipo(mov)}
                        active={mov == movimentacaoTipo}
                        label={mov.charAt(0).toUpperCase() + mov.slice(1).toLowerCase()} />
                    )
                })}
                </div>
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
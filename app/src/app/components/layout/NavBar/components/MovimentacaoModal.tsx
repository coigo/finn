import { ModalContent } from "@/app/components/Modal/ModalContent"
import { ModalFooter } from "@/app/components/Modal/ModalFooter"
import { ModalHeader } from "@/app/components/Modal/ModalHeader"
import { FieldValues, useForm, useWatch } from "react-hook-form"
import { useEffect, useState } from "react"
import { useAportesHook } from "@/hooks/UseBuscarAportes"
import { Chips } from "@/app/components/Chips"
import { FormPontual, pontualSchema } from "./movimentacoes/FormPontual"
import { FormPersistente, persistenteSchema } from "./movimentacoes/FormPersistente"
import { FormParcela, parcelaSchema } from "./movimentacoes/FormParcela"
import { useBuscarMovimentacoesCategoria } from "@/hooks/useBuscarMovimentacoesCategorias"
import { CriarMovimentacaoRequest } from "@/services/movimentacoes"
import { useMovimentacoesHook } from "@/hooks/useBuscarMovimentacoes"
import { useModal } from "@/app/components/Modal/useModal"
import { toast } from "sonner"
import { Button } from "@/app/components/Button"

const MovimentacooesTipos: ["PONTUAL", "PARCELADA", "PERSISTENTE", "ENTRADA"] = ["PONTUAL", "PARCELADA", "PERSISTENTE", "ENTRADA"]

export const MovimentacaoModal = () => {

    const [movimentacaoTipo, setTipo] = useState<TipoMovimentacaoFormulario>("PONTUAL")

    const { control, handleSubmit, reset } = useForm<FieldValues>()
    const categoriaId = useWatch({ control, name: "categoriaId" })
    const { closeModal } = useModal()
    const { buscar } = useMovimentacoesHook()
    const { buscar: buscarCategorias, categorias } = useBuscarMovimentacoesCategoria()
    const { buscar: onClose } = useAportesHook()
    const categoriasPorFiltro = (tipo: MovimentacaoTipo) => {
        return categorias.filter(categoria => categoria.tipo == tipo) || [] as MovimentacaoCategoria[]
    }

    useEffect(() => {
        buscarCategorias()
    }, [])

    const validate = (data: any) => {
        const schema = {
            "PONTUAL": pontualSchema,
            "PARCELADA": parcelaSchema,
            "PERSISTENTE": persistenteSchema,
            "ENTRADA": pontualSchema
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
            const tipo = movimentacaoTipo == "ENTRADA"
                ? 0
                : data.categoriaId == 3
                    ? 2
                    : 1;

            const persistente = movimentacaoTipo == "PERSISTENTE" 

            validate(data)
            await CriarMovimentacaoRequest({ ...data, tipo, persistente })
            await buscar()
            onClose()
        }
        catch (err: any) {
            onClose()
            toast(err.message)
        }finally {
            closeModal()
        }
    }

    const onSelectTipo = (mov: TipoMovimentacaoFormulario) => {
        if (movimentacaoTipo != mov) {
            setTipo(mov)
            reset()
        }
    }

    const formConfig = {
        control, handleSubmit, onSubmit: submit
    }

    const forms = {
        "PONTUAL": <FormPontual config={formConfig} categorias={categoriasPorFiltro("SAIDA")} />,
        "ENTRADA": <FormPontual config={formConfig} categorias={categoriasPorFiltro("ENTRADA")} dividendos={categoriaId == 10} />,
        "PARCELADA": <FormParcela config={formConfig} categorias={categoriasPorFiltro("SAIDA")} />,
        "PERSISTENTE": <FormPersistente config={formConfig} categorias={categoriasPorFiltro("SAIDA")} />
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
                <></>
                <Button  type="submit" onClick={handleSubmit(submit)}  > Enviar </Button>
            </ModalFooter>
        </>
    )

}
import { CurrencyField } from "@/app/components/Inputs/CurrencyField"
import { DateField } from "@/app/components/Inputs/DateField"
import { SelectField } from "@/app/components/Inputs/SelectField"
import { TextField } from "@/app/components/Inputs/TextField"
import { useForm, Controller } from "react-hook-form"
import { z } from "zod"
import { toast } from "sonner"
import {  CriarMovimentacaoRequest, EditarMovimentacaoPersistente } from "@/services/movimentacoes"
import { corrigirTipagem } from "@/lib/utils"
import { useEffect, useState } from "react"
import { usePersistente } from "@/hooks/usePersistente"

export const persistenteSchema = z.object({
    id: z.number(),
    descricao: z.string(),
    categoriaId: z.string(),
    valor: z.string(),
    dataCompra: z.date(),
})

export type PersistenteForm = z.infer<typeof persistenteSchema>

type FormProps = {
    categorias: MovimentacaoCategoria[]
    onSuccess: () => void
    id?: number
}

export const FormPersistente = ({ categorias, onSuccess, id }: FormProps) => {

    const {buscar, loading, movimentacao} = usePersistente()
    const { control, handleSubmit, reset } = useForm<PersistenteForm>()

    useEffect(() => {
        if (id) {
            buscar(id)
        }
    }, [])

    useEffect(() => {
        if (movimentacao) {
            reset({
                id,
                categoriaId: String(movimentacao.categoriaId),
                dataCompra: new Date(),
                descricao: movimentacao.descricao,
                valor: String(movimentacao.valor)
            })
        }
    },[movimentacao, reset])

    const onSubmit = async (data: PersistenteForm) => {
        try {
            const corrigido = corrigirTipagem(data as any)
            if (id) {
                await EditarMovimentacaoPersistente({...corrigido, persistenteId: data.id})
            }
            else {
                const payload = {
                    ...corrigido,
                    tipo: 1,
                    persistente: true
                }
                console.log(payload)
                await CriarMovimentacaoRequest(payload)

            }
            toast("Movimentação persistente criada com sucesso!")
            onSuccess()
        } catch (err: any) {
            toast(err.message || "Erro ao salvar movimentação persistente")
        }
    }

    return (
        <form
            id="movimentacao-form"
            onSubmit={handleSubmit(onSubmit)}
            className="flex flex-col md:flex-row flex-wrap gap-3 justify-evenly"
        >
            <Controller
                name="categoriaId"
                control={control}
                render={({ field }) => (
                    <SelectField
                        placeholder="Categoria"
                        data={categorias}
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
                        placeholder="Valor"
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

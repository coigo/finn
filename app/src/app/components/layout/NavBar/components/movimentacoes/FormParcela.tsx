import { CurrencyField } from "@/app/components/Inputs/CurrencyField"
import { NumberField } from "@/app/components/Inputs/NumberField"
import { DateField } from "@/app/components/Inputs/DateField"
import { SelectField } from "@/app/components/Inputs/SelectField"
import { TextField } from "@/app/components/Inputs/TextField"
import { useForm, Controller } from "react-hook-form"
import { z } from "zod"
import { toast } from "sonner"
import { CriarMovimentacaoRequest } from "@/services/movimentacoes"
import { corrigirTipagem } from "@/lib/utils"

export const parcelaSchema = z.object({
  categoriaId: z.string(),
  descricao: z.string(),
  valor: z.string(),
  quantidadeParcelas: z.string(),
  primeiroVencimento: z.date(),
})

export type ParcelaForm = z.infer<typeof parcelaSchema>

type FormProps = {
  categorias: MovimentacaoCategoria[]
  onSuccess: () => void
  id?: number
}

export const FormParcela = ({ categorias, onSuccess }: FormProps) => {
  const { control, handleSubmit } = useForm<ParcelaForm>({
    defaultValues: {
      primeiroVencimento: new Date()
    } as any
  })

  const onSubmit = async (data: ParcelaForm) => {
    try {
      const corrigido = corrigirTipagem(data as any)

      const payload = {
        ...corrigido,
        tipo: 1,
        persistente: false,
      }

      await CriarMovimentacaoRequest(payload)
      toast("Compra parcelada criada com sucesso!")
      onSuccess()
    } catch (err: any) {
      toast(err.message ?? "Erro ao salvar compra parcelada")
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
            data={categorias}
            placeholder="Categoria"
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
            placeholder="Valor Total"
          />
        }
      />
      <Controller
        name="quantidadeParcelas"
        control={control}
        render={({ field }) =>
          <NumberField
            {...field}
            onChange={e => field.onChange(Number(e.target.value))}
            placeholder="Quantidade de Parcelas"
          />
        }
      />
      <Controller
        name="primeiroVencimento"
        control={control}
        render={({ field }) =>
          <DateField
            {...field}
            label="Primeiro Vencimento"
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

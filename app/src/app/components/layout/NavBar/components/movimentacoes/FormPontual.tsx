import { CurrencyField } from "@/app/components/Inputs/CurrencyField"
import { DateField } from "@/app/components/Inputs/DateField"
import { SelectField } from "@/app/components/Inputs/SelectField"
import { TextField } from "@/app/components/Inputs/TextField"
import { useForm, Controller } from "react-hook-form"
import { z } from "zod"
import { toast } from "sonner"
import { corrigirTipagem } from "@/lib/utils"
import { CriarMovimentacaoRequest } from "@/services/movimentacoes"
import { useEffect } from "react"

export const pontualSchema = z.object({
    categoriaId: z.string(),
    valor: z.string(),
    data: z.date(),
    descricao: z.string()
})

export type PontualForm = z.infer<typeof pontualSchema>

type FormProps = {
  formTipo: "ENTRADA" | "SAIDA"
  categorias: MovimentacaoCategoria[]
  onSuccess: () => void
  id?: number
}

export const FormPontual = ({ categorias, onSuccess, formTipo }: FormProps) => {

  const { control, handleSubmit, reset } = useForm<PontualForm>({
    defaultValues: {
      data: new Date()
    } as any
  })

    const onSubmit = async (data: PontualForm) => {
        try {
            const payload = corrigirTipagem(data as any)
            const tipo = formTipo == "ENTRADA"
                ? 0
                : payload.categoriaId == 3
                    ? 2
                    : 1;
            console.log(payload)
            await CriarMovimentacaoRequest({ ...payload, tipo })
            onSuccess()
        }
        catch (err: any) {
            toast(err.message)
        }finally {
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
            placeholder="Valor"
          />
        }
      />
      <Controller
        name="data"
        control={control}
        render={({ field }) =>
          <DateField
            {...field}
            label="Data"
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

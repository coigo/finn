// MovimentacaoModal.tsx

import { ModalContent } from "@/app/components/Modal/ModalContent"
import { ModalFooter } from "@/app/components/Modal/ModalFooter"
import { ModalHeader } from "@/app/components/Modal/ModalHeader"
import { useEffect, useState } from "react"
import { useAportesHook } from "@/hooks/UseBuscarAportes"
import { Chips } from "@/app/components/Chips"
import { FormPontual } from "./movimentacoes/FormPontual"
import { FormPersistente } from "./movimentacoes/FormPersistente"
import { FormParcela } from "./movimentacoes/FormParcela"
import { useBuscarMovimentacoesCategoria } from "@/hooks/useBuscarMovimentacoesCategorias"
import { useMovimentacoesHook } from "@/hooks/useBuscarMovimentacoes"
import { useModal } from "@/app/components/Modal/useModal"
import { Button } from "@/app/components/Button"

const MovimentacooesTipos = ["PONTUAL", "PARCELADA", "PERSISTENTE", "ENTRADA"] as const
export type TipoMovimentacaoFormulario = typeof MovimentacooesTipos[number]

type Props = {
  lockIn?: TipoMovimentacaoFormulario
  id?: number
}

export const MovimentacaoModal = ({ lockIn, id }: Props) => {
  const [movimentacaoTipo, setTipo] = useState<TipoMovimentacaoFormulario>(lockIn || "PONTUAL")

  const { closeModal } = useModal()
  const { buscar } = useMovimentacoesHook()
  const { buscar: buscarCategorias, categorias } = useBuscarMovimentacoesCategoria()
  const { buscar: atualizarAportes } = useAportesHook()

  const categoriasPorFiltro = (tipo: MovimentacaoTipo) => {
    return categorias.filter(categoria => categoria.tipo == tipo) || [] as MovimentacaoCategoria[]
  }

  useEffect(() => {
    buscarCategorias()
  }, [])

  const onSuccess = async () => {
    await buscar()
    closeModal()
  }

  const onSelectTipo = (mov: TipoMovimentacaoFormulario) => {
    if (movimentacaoTipo !== mov && !lockIn) {
      setTipo(mov)
    }
  }

  return (
    <>
      <ModalHeader title="Movimentações" >
        <div className="flex overflow-x-scroll md:overflow-x-hidden">
          {MovimentacooesTipos.map(mov => (
            <Chips
              key={"chip_" + mov}
              onClick={() => onSelectTipo(mov)}
              active={mov === movimentacaoTipo}
              label={mov.charAt(0).toUpperCase() + mov.slice(1).toLowerCase()} />
          ))}
        </div>
      </ModalHeader>

      <ModalContent>
        {movimentacaoTipo === "PONTUAL" && (
          <FormPontual
            categorias={categoriasPorFiltro("SAIDA")}
            formTipo="SAIDA"
            onSuccess={onSuccess}
            />
        )}

        {movimentacaoTipo === "ENTRADA" && (
            <FormPontual
            formTipo="ENTRADA"
            categorias={categoriasPorFiltro("ENTRADA")}
            onSuccess={onSuccess}
          />
        )}

        {movimentacaoTipo === "PARCELADA" && (
          <FormParcela
            categorias={categoriasPorFiltro("SAIDA")}
            onSuccess={onSuccess}
          />
        )}

        {movimentacaoTipo === "PERSISTENTE" && (
          <FormPersistente
            id={id}
            categorias={categoriasPorFiltro("SAIDA")}
            onSuccess={onSuccess}
          />
        )}
      </ModalContent>

      <ModalFooter>
        <></>
        {/* botão só submete o form visível, via <form> padrão */}
        <Button form="movimentacao-form" type="submit">Enviar</Button>
      </ModalFooter>
    </>
  )
}

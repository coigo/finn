import { PencilIcon, Trash } from 'lucide-react'
import { Button } from '@/components/ui/button'
import { Popover, PopoverContent, PopoverTrigger } from '@/components/ui/popover'
import { DeletarMovimentacaoPersistente, DesfazerMovimentacaoRequest } from '@/services/movimentacoes';
import { useState } from 'react';
import { toast } from 'sonner';
import { useModal } from '@/app/components/Modal/useModal';
import { MovimentacaoModal } from '@/app/components/layout/NavBar/components/MovimentacaoModal';

type Props = {
  row: MovimentacoesPendentes
  refresh: () =>  void 
}

export const DetalhePendentePopup = ({row, refresh}: Props) => {
  
  const [loading, setLoading] = useState(false)
  const [open, setOpen] = useState(false)
  const {openModal} = useModal()

  const onEdit = async () => {
    try {
      openModal(<MovimentacaoModal id={row.id} lockIn='PERSISTENTE'/>)
      setOpen(false)
    }
    catch (err: any) {
      toast(err.message || "Algo deu errado ao desfazer a movimentação!")
    }
    finally {
      setLoading(false)

    }
  }

  const onDelete = async () => {
    try {
      setLoading(true)
      await DeletarMovimentacaoPersistente(row.id)
      toast("Movimentção desfeita!")
      await refresh()
      setOpen(false)
    }
    catch (err: any) {
      toast(err.message || "Algo deu errado ao desfazer a movimentação!")
    }
    finally {
      setLoading(false)

    }
  }

  return (
    <Popover open={open} onOpenChange={setOpen}>
      <PopoverTrigger asChild>
        <button className='absolute inset-0 ' />
      </PopoverTrigger>
      <PopoverContent className='w-80 bg-[#1b191d] border-neutral-700 '>
        <div className='grid gap-4'>
          <div className='space-y-2'>
            <h4 className='leading-none font-medium text-white'>Detalhes da Movimentação</h4>
            <p className='text-neutral-400 text-sm'>{row.tipoDerivado}</p>
          </div>
          <div className='flex gap-2 text-neutral-400'>
            <span className='font-semibold'>Descrição:</span> {row.descricao}

          </div>
          <div className='flex justify-end gap-2'>
            { 
              row.tipoDerivado == "PERSISTENTE" && <>
              <Button 
              onClick={onDelete}
              className='small-button bg-transparent font-semibold hover:!bg-[#b84149]'>
              <Trash />
            </Button>
            <Button 
              onClick={onEdit}
              className='small-button bg-[#de983b] font-semibold'>
              <PencilIcon color="black"/>
            </Button>
            </>
            }
          </div>
        </div>
      </PopoverContent>
    </Popover>
  )
}

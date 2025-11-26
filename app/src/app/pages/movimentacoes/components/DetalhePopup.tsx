import { ArrowRight, ArrowRightIcon, PlusIcon, Trash } from 'lucide-react'
import { MdOutlineKeyboardArrowRight } from "react-icons/md";


import { Button } from '@/components/ui/button'
import { Popover, PopoverContent, PopoverTrigger } from '@/components/ui/popover'
import { DesfazerMovimentacaoRequest } from '@/services/movimentacoes';
import { useState } from 'react';
import { toast } from 'sonner';

type Props = {
  row: Movimentacao
  refresh: () =>  void 
}

export const DetalhePopup = ({row, refresh}: Props) => {
  
  const [loading, setLoading] = useState(false)
  const [open, setOpen] = useState(false)

  const onDesfazer = async () => {
    try {
      setLoading(true)
      await DesfazerMovimentacaoRequest(row.id)
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
        <button className='absolute w-full h-10' >
        </button>
      </PopoverTrigger>
      <PopoverContent className='w-80 bg-[#1b191d] border-neutral-700 '>
        <div className='grid gap-4'>
          <div className='space-y-2'>
            <h4 className='leading-none font-medium text-white'>Detalhes da Movimentação</h4>
            <p className='text-neutral-400 text-sm'>{row?.tipo}</p>
          </div>
          <div className='flex gap-2 text-neutral-400'>
            <span className='font-semibold'>Descrição:</span> {row.descricao}

          </div>
          <div className='flex justify-end'>
            <Button 
              onClick={onDesfazer}
              className='small-button bg-[#de983b] font-semibold'>
              <Trash color="black"/>
            </Button>
          </div>
        </div>
      </PopoverContent>
    </Popover>
  )
}

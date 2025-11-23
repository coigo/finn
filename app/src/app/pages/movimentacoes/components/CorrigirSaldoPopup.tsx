import { PencilIcon, PencilRulerIcon, Send } from 'lucide-react'

import { Button } from '@/components/ui/button'
import { Popover, PopoverContent, PopoverTrigger } from '@/components/ui/popover'
import { CurrencyField } from '@/app/components/Inputs/CurrencyField'
import { Controller, useForm } from 'react-hook-form'
import { useState } from 'react'
import { toast } from 'sonner'
import { CorrigirSaldoRequest } from '@/services/saldo'

type Props = {
  onSubmit: () => Promise<void>
}

export const CorrigirSaldoPopup = ({onSubmit}: Props) => {

  const { control, handleSubmit } = useForm<{valor: string}>()
  const [loading, setLoading] = useState(false)

  const submit = async (data: any) => {
    try {
      setLoading(true);
      // (data as any) = corrigirTipagem(data as any)
      CorrigirSaldoRequest({
        ...data,
        nome: "Corrente"
      })
      await onSubmit()
    }
    catch (err: any) {
      toast(err.message || "Algo deu errado ao corrigir o saldo!")
    }
    finally {
      setLoading(false)
    }
  }

  return (
    <Popover>
      <PopoverTrigger asChild>
        <Button className='small-button bg-[#de983b]'>
          <PencilIcon color='black' />
        </Button>
      </PopoverTrigger>
      <PopoverContent className='w-80 bg-[#1b191d] border-neutral-700 '>
        <div className='grid gap-4'>
          <div className='space-y-2'>
            <h4 className='leading-none font-medium text-white'>Corrigir Saldo</h4>
            <p className='text-muted-foreground text-sm'>Edite o saldo atual</p>
          </div>
          <div className='grid gap-2'>
            <form className="flex gap-4"onSubmit={handleSubmit(submit)}>
              <Controller
                name="valor"
                control={control}
                render={({ field }) =>
                  <CurrencyField
                    className='text-white'
                    {...field}
                    placeholder="valor"
                  />
                }
              />
                <Button className='bg-[#de983b] text-black'>
                  <Send  /> Enviar 
                </Button>
            </form>

          </div>
        </div>
      </PopoverContent>
    </Popover>
  )
}

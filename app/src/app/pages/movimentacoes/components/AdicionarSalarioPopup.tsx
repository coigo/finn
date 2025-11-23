import { PlusIcon, Send } from 'lucide-react'

import { Button } from '@/components/ui/button'
import { Popover, PopoverContent, PopoverTrigger } from '@/components/ui/popover'
import { CurrencyField } from '@/app/components/Inputs/CurrencyField'
import { Controller, useForm } from 'react-hook-form'
import { useEffect, useState } from 'react'
import { toast } from 'sonner'
import { useBuscarSalarioAtual } from '@/hooks/useBuscarSalarioAtual'
import { AdicionarSalarioRequest } from '@/services/salario'

export const AdicionarSalarioPopup = () => {

  const { control, handleSubmit } = useForm()
  const [loading, setLoading] = useState(false)

  const {buscar, salario, loading: loadingSalario} =  useBuscarSalarioAtual()

  useEffect(() => {
    console.log("wewewe")
    if (!salario) {
      buscar()
    }
  }, [])

    const onAdicionarSalario = async () => {
        try {
            setLoading(true)
            await AdicionarSalarioRequest()
            // await buscarSaldo("Corrente")
        }
        catch (err: any) {
            toast(err.message)
        }
        finally {
            setLoading(false)
        }
    }


  
  return (
    <Popover>
      <PopoverTrigger asChild>
        <Button className='small-button bg-[#de983b]'>
          <PlusIcon color='black' />
        </Button>
      </PopoverTrigger>
      <PopoverContent className='w-80 bg-[#1b191d] border-neutral-700 '>
        <div className='grid gap-4'>
          <div className='space-y-2'>
            <h4 className='leading-none font-medium text-white'>Adicionar Salário</h4>
            <p className='text-muted-foreground text-sm'>Some seu salário ao saldo</p>
          </div>
          <div className='grid gap-2'>
            {!loadingSalario 
              ? <div className="flex items-end gap-4">
                
                <p className='text-muted-foreground '>Adicionar R$ {salario?.valor.toFixed(2)}</p>
                <Button 
                  onClick={onAdicionarSalario}
                  disabled={loading}
                  className='bg-[#de983b] text-black'>
                  <PlusIcon  /> Adicionar 
                </Button>
            </div>

            : <>wee</>
            }
            

          </div>
        </div>
      </PopoverContent>
    </Popover>
  )
}

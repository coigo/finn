"use client"

import { FaDollarSign } from "react-icons/fa";
import { GoArrowSwitch } from "react-icons/go";
import { AporteModal } from './components/AporteModal';
import { MovimentacaoModal } from './components/MovimentacaoModal';
import CustomPopover from '../../Popover';
import { useEffect, useState } from 'react';
import { CurrencyField } from '../../Inputs/CurrencyField';
import { useModal } from "../../Modal/useModal";
import { useBuscarSalarioAtual } from "@/hooks/useBuscarSalarioAtual";
import { toast } from "sonner";


export default function NavBar() {
  const { openModal } = useModal()
  const [value, setValue] = useState<string>("")
  const { buscar, salario, criar } = useBuscarSalarioAtual()

  useEffect(() => {
    buscar()
  }, [])


  const handleSalario = async (e: string) => {
    try {
      criar(e)
      setValue("")
    }
    catch (err: any) {
      toast("asdasdasdasdasdhasawyaj ajahsfa k a asjha ")
    }
  }

  return (
    <>
      <div className='w-full md:flex md:justify-center mb-8 '>
        <div className='flex justify-between w-full md:w-3/4 p-2 bg-[#de983b] rounded-b-2xl'>
          <span className='flex gap-2 ml-4'>
            <div onClick={() => openModal(<MovimentacaoModal />)} className='custom-button-round'>
              <GoArrowSwitch className='text-yellow-950' />
            </div>
            <div onClick={() => openModal(<AporteModal />)} className='custom-button-round'>
              <FaDollarSign className='text-yellow-950' />
            </div>
          </span>
          <div className=' mx-1 h-full w-fit content-evenly'>
            <CustomPopover >
              <div className='text-neutral-400'>
                <div className='w-full p-2 text-center'>
                  Configurações
                </div>
                <hr />
                <div className='flex content-end text-end mt-2'>
                  Salario:
                  <CurrencyField
                    className='w-fit'
                    id="standard-basic"
                    placeholder={salario ? salario.valor?.toFixed(2) : '0,00'}
                    variant="standard"
                    value={value}
                    onChange={(e: any) => setValue(e.target.value)}
                    onBlur={(e: any) => handleSalario(e.target.value)}
                  />

                </div>
              </div>
            </CustomPopover>
          </div>
        </div>
      </div>
    </>
  );
}

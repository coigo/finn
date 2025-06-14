"use client"
import { IconButton, TextField, useColorScheme } from '@mui/material';
import { useModal } from '@/app/components/Modal/useModal';

import WbSunnyIcon from '@mui/icons-material/WbSunny';
import BedtimeIcon from '@mui/icons-material/Bedtime';

import AttachMoneyIcon from '@mui/icons-material/AttachMoney';
import CompareArrowsIcon from '@mui/icons-material/CompareArrows';
import { AporteModal } from './components/AporteModal';
import { MovimentacaoModal } from './components/MovimentacaoModal';
import CustomPopover from '../../Popover';
import { useBuscarSalarioAtual } from '@/hooks/useBuscarSalarioAtual';
import { useEffect, useState } from 'react';
import { CurrencyField } from '../../Inputs/CurrencyField';
import { CriarSalarioRequest } from '@/services/salario';
import { useToast } from '../../Toast/useToast';


export default function NavBar() {
  const { mode, setMode } = useColorScheme();
  const { openModal } = useModal()
  const { showToast } = useToast()
  const [value, setValue] = useState<string>("")
  const { buscar, salario, criar } = useBuscarSalarioAtual()

  useEffect(() => {
    buscar()
  }, [])

  const handleChangeTheme = () => {
    setMode(mode == "dark" ? "light" : "dark")
  };

  const handleSalario = async (e: string) => {
    try {
      criar(e)
      setValue("")
    }
    catch (err: any) {
      showToast(err.message, 'error')
    }
  }

  return (
    <>
      <div className='w-full md:flex md:justify-center mb-8 '>
        <div className='flex justify-between w-full md:w-3/4 p-2 bg-[#de983b] rounded-b-2xl'>
          <span className='flex gap-2 ml-4'>
            <div onClick={() => openModal(<MovimentacaoModal />)} className='custom-buttom-round'>
              <CompareArrowsIcon className='text-yellow-950' />
            </div>
            <div onClick={() => openModal(<AporteModal />)} className='custom-buttom-round'>
              <AttachMoneyIcon className='text-yellow-950' />
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
                    placeholder={salario? salario.valor?.toFixed(2) : '0,00' }
                    variant="standard"
                    value={value}
                    onChange={(e) => setValue(e.target.value)}
                    onBlur={(e) => handleSalario(e.target.value)}
                  />

                </div>
              </div>
            </CustomPopover>
            <IconButton onClick={handleChangeTheme}>
              {
                mode == "dark"
                  ? <WbSunnyIcon className='text-yellow-950' />
                  : <BedtimeIcon className='text-yellow-950' />
              }
            </IconButton>
          </div>
        </div>
      </div>
    </>
  );
}

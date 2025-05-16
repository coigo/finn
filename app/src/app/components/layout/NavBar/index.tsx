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
import { useEffect } from 'react';


export default function NavBar() {
  const { mode, setMode } = useColorScheme();
  const { openModal } = useModal()

  const {buscar, salario} = useBuscarSalarioAtual()

  useEffect(() => {
    buscar()
  }, [])

  const handleChangeTheme = () => {
    setMode(mode == "dark" ? "light" : "dark")
  };

  return (
    <>
      <div className='w-full md:flex md:justify-center mb-8 '>
        <div className='flex justify-between w-full md:w-3/4 p-2 bg-[#de983b] rounded-b-2xl'>
          <span className='flex gap-2 ml-4'>
            <div onClick={() => openModal(<MovimentacaoModal />)} className='nav-bar-item-round'>
              <CompareArrowsIcon className='text-yellow-950' />
            </div>
            <div onClick={() => openModal(<AporteModal />)} className='nav-bar-item-round'>
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
                  Salario: <TextField className='w-fit' id="standard-basic" placeholder={`${salario?.valor.toFixed(2)}` || '0'} variant="standard" />

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

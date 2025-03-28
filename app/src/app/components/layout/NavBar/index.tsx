"use client"
import * as React from 'react';
import { IconButton, useColorScheme, useTheme } from '@mui/material';
import { useModal } from '@/app/components/Modal/useModal';

import WbSunnyIcon from '@mui/icons-material/WbSunny';
import BedtimeIcon from '@mui/icons-material/Bedtime';

import AttachMoneyIcon from '@mui/icons-material/AttachMoney';
import CompareArrowsIcon from '@mui/icons-material/CompareArrows';
import { AporteModal } from './components/AporteModal';
import { MovimentacaoModal } from './components/MovimentacaoModal';


export default function NavBar() {
  const { mode, setMode } = useColorScheme();
  const { openModal, closeModal } = useModal()

  const { palette } = useTheme()

  const handleChangeTheme = () => {
    setMode(mode == "dark" ? "light" : "dark")
  };

  return (
    <>
      <div className='w-full md:flex md:justify-center mb-8 '>
        <div className='flex justify-between w-full md:w-3/4 p-2 bg-yellow-600 rounded-b-2xl'>
          <span className='flex gap-2 ml-4'>
            <div onClick={() => openModal(<MovimentacaoModal/>)} className='nav-bar-item-round'>
              <CompareArrowsIcon className='text-yellow-950'/>
            </div>
            <div onClick={() => openModal(<AporteModal/>)} className='nav-bar-item-round'>
              <AttachMoneyIcon  className='text-yellow-950'/>
            </div>
          </span>
          <span>
            <IconButton onClick={handleChangeTheme}>
              {
                mode == "dark"
                  ? <WbSunnyIcon className='text-yellow-950' />
                  : <BedtimeIcon className='text-yellow-950' />
              }
            </IconButton>
          </span>
        </div>
      </div>
    </>
  );
}

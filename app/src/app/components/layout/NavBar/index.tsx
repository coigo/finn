"use client"
import * as React from 'react';
import { IconButton, useColorScheme, useTheme } from '@mui/material';

import WbSunnyIcon from '@mui/icons-material/WbSunny';
import BedtimeIcon from '@mui/icons-material/Bedtime';
import { useModal } from '@/app/components/Modal/useModal';

export default function NavBar() {
  const { mode, setMode } = useColorScheme();
  const { openModal, closeModal } = useModal()

  const { palette } = useTheme()

  const handleChangeTheme = () => {
    setMode(mode == "dark" ? "light" : "dark")
  };


  const componente = <></>


  const itemsTempl = () => {
    return (
      <div onClick={() => openModal(<>weeeeeeeeeeeeee</>)} className='nav-bar-item-round'>
        oi
      </div>
    )
  }


  return (
    <>
      <div className='w-full md:flex md:justify-center mb-8 '>
        <div className='flex justify-between w-full md:w-3/4 p-2 bg-yellow-600 rounded-b-2xl'>
          <span>
            {itemsTempl()}
          </span>
          <span>
            <IconButton onClick={handleChangeTheme}>
              {
                mode == "dark"
                  ? <WbSunnyIcon className='text-yellow-950' />
                  : <BedtimeIcon className='text-yellow-950'/>
              }
            </IconButton>
          </span>
        </div>
      </div>
    </>
  );
}

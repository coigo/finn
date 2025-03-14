"use client"
import * as React from 'react';
import { useTheme } from 'next-themes';
import { IconButton, useColorScheme } from '@mui/material';

import WbSunnyIcon from '@mui/icons-material/WbSunny';
import BedtimeIcon from '@mui/icons-material/Bedtime';

export default function NavBar() {
  const { mode, setMode } = useColorScheme();


  const handleChangeTheme = () => {
    setMode(mode == "dark" ? "light" : "dark")
  };

  const itemsTempl = () => {
    return (
      <div className='nav-bar-item-round'>
        oi
      </div>
    )
  }

  return (
    <>
      <div className='w-full md:flex md:justify-center mb-8'>
        <div className='flex justify-between w-full md:w-3/4 p-2 bg-yellow-600 rounded-b-2xl'>
          <span>
            {itemsTempl()}
          </span>
          <span>
            <IconButton color="primary" aria-label="Alterar tema" onClick={handleChangeTheme}>
              {
                mode == "dark"
                  ? <WbSunnyIcon />
                  : <BedtimeIcon />
              }
            </IconButton>
          </span>
        </div>
      </div>
    </>
  );
}

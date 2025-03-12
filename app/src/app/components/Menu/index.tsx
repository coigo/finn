"use client"
import * as React from 'react';
import { useTheme } from 'next-themes';
import { useEffect, useState } from 'react';
import { createTheme, ThemeProvider } from '@mui/material/styles';
import AccessAlarmIcon from '@mui/icons-material/AccessAlarm';

export default function Menu() {
  const { setTheme, resolvedTheme } = useTheme();
  const [muiTheme, setMuiTheme] = useState(createTheme({ palette: { mode: "light" } }));

  useEffect(() => {
    setMuiTheme(createTheme({ palette: { mode: resolvedTheme === "dark" ? "dark" : "light" } }));
  }, [resolvedTheme]);

  const handleChangeTheme = () => {
    setTheme(resolvedTheme === "dark" ? "light" : "dark");
  };

  const itemsTempl = () => {
    return (
      <div className='menu-item relative md:my-10 md:mx-0 md:hover:bg-amber-500 md:hover:p-2 hover:rounded-2xl '>
        <AccessAlarmIcon fontSize='large'/>
      </div>
    )
  }

  return (
    <>
      <div className="menu absolute flex bottom-0 md:left-0 w-full h-fit md:h-full border-gray-600 md:border-0  text-center">
        <div className="flex p-4 md:p-6 md:left-0 w-full h-fit md:h-full md:w-fit">
          <div className='flex md:block justify-around h-fit w-full self-center gap-4'>
            
            {itemsTempl()}
            {itemsTempl()}
          </div>
        </div>
        <span className="hidden md:block justify-self-end menu-outline"></span>
      </div>
    </>
  );
}

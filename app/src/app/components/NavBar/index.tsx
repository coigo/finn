"use client"
import * as React from 'react';
import { useTheme } from 'next-themes';
import { useEffect, useState } from 'react';
import { createTheme, ThemeProvider } from '@mui/material/styles';

export default function NavBar() {
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
      <div className='nav-bar-item-round'>
        oi
      </div>
    )
  }

  
  
  return (
    <>
      <div className='w-full md:flex md:justify-center'>
        <div className='w-full md:w-3/4 p-2 bg-yellow-600 rounded-b-2xl'>
          {itemsTempl()}
        </div>
      </div>
    </>
  );
}

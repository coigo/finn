"use client"

import { ThemeProvider as NextThemesProvider } from 'next-themes';
import { ThemeProvider, createTheme } from '@mui/material/styles';
import { useEffect, useState } from 'react';

export function Providers({ children }: { children: React.ReactNode }) {
  const [muiTheme, setMuiTheme] = useState(() => createTheme({ palette: { mode: "light" } }));

  return (
    <NextThemesProvider attribute="class" defaultTheme="system" enableSystem>
      <ThemeProvider theme={muiTheme}>
        {children}
      </ThemeProvider>
    </NextThemesProvider>
  );
}

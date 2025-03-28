"use client";

import { CssBaseline, useColorScheme } from "@mui/material";
import { ThemeProvider, createTheme } from "@mui/material/styles";
import { useTheme } from "next-themes";
import { useEffect, useState } from "react";
import { ModalProvider } from "./components/Modal/ModalProvider";
import Modal from "./components/Modal/Modal";

export default function Providers({ children }: { children: React.ReactNode }) {

  const { mode } = useColorScheme()

  const darkTheme = createTheme({
    colorSchemes: {
      dark: true,
      light: true
    },
    palette: {
      mode: "dark",
      background: {
        default: "#1b191d",
        paper: "#1b191d",
      },
      primary: {
        main: "#eee",
      },
      text: {
        primary: "#eee",
        secondary: "#432004"
      },
      
    },
    components: {
      MuiPaper: {
        styleOverrides: {
          root: {
            backgroundImage: "none",
          },
        },
      },
    },
  });

  const lightTheme = createTheme({
    colorSchemes: {
      dark: true,
      light: true
    },
    palette: {
      mode: "light",
      background: {
        default: "#f0f0f0",
        paper: "#f0f0f0",
      },
      primary: {
        main: "#777",
      },
      text: {
        primary: "#222",
        secondary: "#432004"
      },
    },
    components: {
      MuiPaper: {
        styleOverrides: {
          root: {
            backgroundImage: "none",
          },
        },
      },
    },
  });

  return (
    <ThemeProvider theme={mode === "dark" ? darkTheme : lightTheme}>
      <CssBaseline />
      <ModalProvider>

        {children}
        <Modal />
      </ModalProvider>
    </ThemeProvider>
  );
}

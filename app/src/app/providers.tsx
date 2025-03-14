"use client";

import { CssBaseline } from "@mui/material";
import { ThemeProvider, createTheme } from "@mui/material/styles";


export default function Providers({ children }: { children: React.ReactNode }) {

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
        main: "#4b3d05",
      },
      text: {
        primary: "#ffffff",
      },
    },
    components: {
      MuiPaper: {
        styleOverrides: {
          root: {
            backgroundImage: "none"
          },
        },
      },
    }
  });

  return (
    <ThemeProvider theme={darkTheme}>
      <CssBaseline />

      {children}
    </ThemeProvider>
  );
}
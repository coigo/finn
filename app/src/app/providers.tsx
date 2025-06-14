"use client"
import { CssBaseline, useColorScheme } from "@mui/material";
import { ThemeProvider, createTheme } from "@mui/material/styles";
import { ModalProvider } from "./components/Modal/ModalProvider";
import { Modal } from "./components/Modal/Modal";
import { LocalizationProvider } from "@mui/x-date-pickers";
import dayjs from 'dayjs';
import utc from 'dayjs/plugin/utc';
import { AdapterDayjs } from '@mui/x-date-pickers/AdapterDayjs';
import { ToastProvider } from "./components/Toast/ToastProvider";
import { AportesProvider } from "@/hooks/UseBuscarAportes";
import { MovimentacoesProvider } from "@/hooks/useBuscarMovimentacoes";


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

  dayjs.extend(utc);

  return (
    <LocalizationProvider dateAdapter={AdapterDayjs}>
        <ToastProvider >
          <AportesProvider>
          <MovimentacoesProvider>
            <ModalProvider>
              {children}
              <Modal />
            </ModalProvider>
          </MovimentacoesProvider>
          </AportesProvider>
        </ToastProvider>
    </LocalizationProvider>
  );
}

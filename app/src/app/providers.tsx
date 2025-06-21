"use client"
import { ModalProvider } from "./components/Modal/ModalProvider";
import { Modal } from "./components/Modal/Modal";
import { AportesProvider } from "@/hooks/UseBuscarAportes";
import { MovimentacoesProvider } from "@/hooks/useBuscarMovimentacoes";
import { Toast } from "radix-ui";
import { Toaster } from "sonner";
import { BrowserRouter, RouterProvider } from "react-router-dom";
import { Router } from "./Routes";

export default function Providers({ children }: { children: React.ReactNode }) {

  return (
    <>
    <BrowserRouter >
      <AportesProvider>
        <MovimentacoesProvider>
          <ModalProvider>
            <Toaster />
            {children}
            <Modal />
          </ModalProvider>
        </MovimentacoesProvider>
      </AportesProvider>
    </BrowserRouter>
    </>


  );
}

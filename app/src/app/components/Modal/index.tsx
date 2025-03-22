"use client"

import { ReactNode, useEffect, useRef } from "react"

export interface IDialog {
    title: string
    children: ReactNode
    onClose: () => void    
} 

export default function Modal ({ children, title, onClose }:IDialog) {

    const modalRef = useRef<null | HTMLDialogElement>(null)
    

    return (
        <div className="absolute inset-0 bg-white p-5 md:top-auto md:right-auto md:left-1/2 md:bottom-0 md:w-3/5 md:-translate-x-1/2">
            conteudo
            <button onClick={onClose} >fechar</button>
        </div>
    )
}
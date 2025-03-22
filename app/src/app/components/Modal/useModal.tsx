"use client"

import Modal from "@/app/components/Modal"
import { ReactNode, useEffect, useRef, useState } from "react"


type OpenModalProps = {
    component: ReactNode
    initialData: any
}

export function useModal() {
    const [isOpen, setIsOpen] = useState<boolean>(false)

    const openModal = () => {
        setIsOpen(true)
    }

    const closeModal = () => {
        setIsOpen(true)
    }

    const ModalWrapper = ({ title, children }: { title: string; children: ReactNode }) => (
        isOpen && <Modal title={title} onClose={closeModal}>{children}</Modal>
    );

    return { openModal, closeModal, ModalWrapper }

}
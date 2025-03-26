"use client"
import React, { useEffect } from "react";
import ReactDOM from "react-dom";
import { useModal } from "./useModal";
import { useTheme } from "@mui/material";

const Modal: React.FC = () => {
    const { isOpen, content, closeModal } = useModal();
    const {palette} = useTheme()

    
    useEffect(() => {
        const handleKeyDown = (event: KeyboardEvent) => {
            if (event.key === "Escape") closeModal();
        };
        
        window.addEventListener("keydown", handleKeyDown);
        return () => window.removeEventListener("keydown", handleKeyDown);
    }, [closeModal]);

    if (!isOpen) return null;
    
    return ReactDOM.createPortal(
        <div className="modal-border absolute mb-4 overflow-hidden md:rounded-lg md:p-[1px] md:pb-0 bg-white inset-0 md:top-auto md:right-auto md:left-1/2 md:bottom-0 md:w-4/5 lg:w-3/5 md:-translate-x-1/2"
            style={{background:`radial-gradient(circle, rgba(255,255,255,0) 0%, color-mix(in srgb, ${palette.text.primary} 70%, transparent) 31%, ${palette.background.default} 100%)`}}
        >
                <div className="modal-content shadow-sm shadow-gray-700 h-full md:h-fit md:rounded-lg overflow-hidden"
                    style={{
                        backgroundColor: `${[palette.background.default]}`,
                    }}
                >
                    <div className="w-full p-4 pb-2 bg-neutral-900">
                        Titulo
                    </div>
                    <div className="p-4 pt-2">
                        {content}
                    </div>
                    <div className="px-4 p-2">
                        enviar
                    </div>
                </div>

            </div>,
        document.body
    );
};

export default Modal;
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
        <div className="modal absolute mb-4 overflow-hidden md:p-[1px] md:rounded-lg inset-0 md:top-auto md:right-auto md:left-1/2 md:bottom-0 md:w-4/5 lg:w-3/5 md:-translate-x-1/2"
        >   
                    
                    <div className="modal-border fixed z-[-1] border border-white"
                style={{background:`radial-gradient(circle, color-mix(in srgb, ${palette.text.primary} 70%, transparent) 0%, ${palette.background.default} 100%)`}}
                     />
                <div className=" shadow-sm shadow-gray-700 h-full md:h-fit md:rounded-lg overflow-hidden"
                    style={{
                        backgroundColor: `${[palette.background.default]}`,
                    }}
                >
                    { content }
                </div>

            </div>,
        document.body
    );
};

export default Modal;

{/* <div  */}
{/* Titulo */}
{/* </div> */}
{/* <div */}
{/* {content} */}
{/* </div> */}

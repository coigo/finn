import { ReactNode } from "react"
import { useModal } from "./useModal"
import CloseIcon from '@mui/icons-material/Close';

type ModalHeaderProps = {
    title: string
}

export const ModalHeader = ({ title }: ModalHeaderProps) => {
    
    const { closeModal } = useModal()
    
    return <div className="w-full p-4 pb-2 bg-neutral-900">

        {title}
        
        <span className="fixed top-3 right-5" 
            onClick={closeModal}
        >
            <CloseIcon />
        </span>
    </div>
}
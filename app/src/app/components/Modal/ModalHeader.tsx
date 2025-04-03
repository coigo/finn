import { ReactNode } from "react"
import { useModal } from "./useModal"
import CloseIcon from '@mui/icons-material/Close';

type ModalHeaderProps = {
    title: string
}

export const ModalHeader = ({ title }: ModalHeaderProps) => {
    
    const { closeModal } = useModal()
    
    return  <div className="w-full overflow-hidden backdrop-blur-sm p-4 pb-2 bg-neutral-800/30">

                {title}
                
                <span className="fixed top-3 right-5" 
                    onClick={closeModal}
                >
                    <CloseIcon />
                </span>
            </div>

}
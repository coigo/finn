import { ReactNode } from "react";
import { useModal } from "./useModal"
import CloseIcon from '@mui/icons-material/Close';

type ModalHeaderProps = {
    title: string,
    children?: ReactNode
}

export const ModalHeader = ({ title, children }: ModalHeaderProps) => {

    const { closeModal } = useModal()

    return <div className="w-full overflow-hidden backdrop-blur-sm p-4 pb-2 bg-neutral-800/30">
        <div className="flex">
            {title}
            {children && children}

        </div>

        <span className="fixed top-3 right-5"
            onClick={closeModal}
        >
            <CloseIcon />
        </span>
    </div>

}
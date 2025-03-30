import { ReactNode } from "react"

export const ModalFooter = ({ children }: { children: ReactNode }) => {
    return  <div className="flex flex-row-reverse px-6 p-2 gap-2"> 
     {children}
     </div> 
}
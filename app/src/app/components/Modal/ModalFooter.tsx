import { ReactNode } from "react"

export const ModalFooter = ({ children }: { children: ReactNode }) => {
    return  <div className="px-4 p-2"> 
     {children}
     </div> 
}
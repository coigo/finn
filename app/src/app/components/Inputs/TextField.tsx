import { Input } from "@/components/ui/input"
import { ComponentProps } from "react"




export const TextField = (props: ComponentProps<"input">) => {
    return (
            <Input 
                className="flex flex-1"
                {...props} 
                value={props.value || ''}
            />
    )
}
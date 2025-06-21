import { Input } from "@/components/ui/input"
import { ComponentProps } from "react"

export const NumberField = ({ ...props }: ComponentProps<"input">) => {

    return (
        <Input
            className="flex flex-1"
            {...props}
            value={props.value || ""}
            type="number"
        />
    )
}
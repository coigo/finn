import { TextField, TextFieldProps } from "@mui/material"

export const NumberField = ({ ...props }: TextFieldProps) => {

    return (
        <TextField
            className="flex flex-1"
            {...props}
            type="number"
        />
    )
}
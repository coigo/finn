import { TextField, TextFieldProps } from "@mui/material"

export const NumberField = ({ ...props }: TextFieldProps) => {

    return (
        <TextField
            className="flex flex-1"
            {...props}
            value={props.value || ""}
            type="number"
        />
    )
}
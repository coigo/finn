import { TextField, TextFieldProps } from "@mui/material"

export const NumberField = ({ ...props }: TextFieldProps) => {

    return (
        <div className="mx-2">
            <TextField
                {...props}
                type="number"
            />
        </div>
    )
}
import { TextField, TextFieldProps } from "@mui/material"
import { useState } from "react"




export const NumberField = ({ onChange, value, ...props }: TextFieldProps) => {

    return (
        <div className="mx-2">
            <TextField
                {...props}
                type="number"
            />
        </div>
    )
}
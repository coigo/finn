import { formatCurrency } from "@/utils/string"
import { InputAdornment, TextField, TextFieldProps } from "@mui/material"

import AttachMoneyIcon from '@mui/icons-material/AttachMoney';


export function CurrencyField (props: TextFieldProps) {
    return <TextField
            className="flex flex-1"
            {...props}
            value={props.value || ""}
            onChange={(e) => {
                e.target.value = formatCurrency(e.target.value)
                if (props.onChange) {
                    props.onChange(e)
                  }
            }}

            slotProps={{
                input: {
                    startAdornment: <InputAdornment position="start"><AttachMoneyIcon /></InputAdornment>,
                },
            }}
        />
    
}
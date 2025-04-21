import { TextField as Input, TextFieldProps} from "@mui/material"




export const TextField = (props: TextFieldProps) => {
    return (
            <Input 
                className="flex flex-1"
                {...props} 
                value={props.value || ''}
            />
    )
}
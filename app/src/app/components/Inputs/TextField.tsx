import { TextField as Input, TextFieldProps} from "@mui/material"




export const TextField = (props: TextFieldProps) => {
    return (
        <div className="mx-2">
            <Input 
                {...props} 
                value={props.value || ''}
            />
        </div>
    )
}
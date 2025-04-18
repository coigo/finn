import { TextField as Input, TextFieldProps} from "@mui/material"




export const TextField = (props: TextFieldProps) => {
    return (
        <div className="flex flex-1">
            <Input 
                {...props} 
                value={props.value || ''}
            />
        </div>
    )
}
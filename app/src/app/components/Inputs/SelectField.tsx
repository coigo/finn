import { FormControl, InputLabel, MenuItem, Select, SelectProps } from "@mui/material"

type SelectFieldProps = SelectProps & {
    controlClass?: string
    data: SelectValues[]
}

export const SelectField = ({ data, label, value, controlClass,...props }: SelectFieldProps) => {
    return (
        <FormControl className={controlClass || "w-full md:w-60 px-2" } >
            {label && <InputLabel id="demo-simple-select-label">{label}</InputLabel>}
            <Select
                style={{outline:'none', border:'none'}}
                labelId="demo-simple-select-label"
                id="demo-simple-select"
                value={value ?? ""}
                {...props}
            >
                {data.map((i) => (
                    <MenuItem key={i.id} value={i.id}>
                        {i.name}
                    </MenuItem>
                ))}
            </Select>
        </FormControl>
    );
};
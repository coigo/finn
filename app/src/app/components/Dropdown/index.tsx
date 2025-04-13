import { InputBase, MenuItem, Select, SelectChangeEvent, SelectProps } from "@mui/material"
import { ReactNode } from "react"

type DropdownProps = SelectProps & {
    data: SelectValues[]
    onChange: (e: SelectChangeEvent<unknown>) => void
    input?: ReactNode
    value: any

}

export default function Dropdown({data, input, onChange, value}: DropdownProps) {
    return (
        <Select
            onChange={onChange}
            className="w-full pl-4 p-1 outline-0 border-0 bg-neutral-500/10 rounded-lg"
            input={input || <InputBase />}
            MenuProps={{
                PaperProps: {
                    sx: {
                        mt: 1,
                        borderRadius: 1,
                    },
                },
            }}
            value={value}
        >
            {data.map((i) => (
                <MenuItem key={i.id} value={i.id}>
                    {i.name}
                </MenuItem>
            ))}
        </Select>
    )
}
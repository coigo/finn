import { DatePicker, DatePickerProps } from "@mui/x-date-pickers"
import { Dayjs } from "dayjs"



export const DateField = (props: DatePickerProps<Dayjs>) => {
    return (
        <DatePicker 
            {...props}
            value={props.value ?? null}
            format="DD/MM/YYYY"
        />
    )
}
import { DatePicker, DatePickerProps } from "@mui/x-date-pickers"
import { Dayjs } from "dayjs"



export const DateField = (props: DatePickerProps<Dayjs>) => {
    return (
            <DatePicker 
                className="flex flex-1"
                {...props}
                value={props.value ?? null}
                format="DD/MM/YYYY"
            />
    )
}
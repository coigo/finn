import * as React from "react"
import { format } from "date-fns"
import { Calendar as CalendarIcon } from "lucide-react"

import { Button } from "@/components/ui/button"
import { Calendar } from "@/components/ui/calendar"
import { Popover, PopoverContent, PopoverTrigger } from "@/components/ui/popover"

type DatePickerProps =  {
  label: string
  value: Date
  onChange: (e?: Date) => void
  name: string
}

export const DateField = ({ label, value, onChange }: DatePickerProps) => {
  const [date, setDate] = React.useState<Date | undefined>(value)

  React.useEffect(() => {
    setDate(value)
  }, [value])

  return (
    <Popover>
      <PopoverTrigger asChild>
        <Button
          variant="outline"
          data-empty={!date}
          className="border-neutral-500 data-[empty=true]:text-muted-foreground w-[280px] justify-start text-left font-normal hover:bg-transparent"
        >
          <CalendarIcon />
          {date ? format(date, "PPP") : <span>{label}</span>}
        </Button>
      </PopoverTrigger>

      <PopoverContent className="w-auto p-0">
        <Calendar
          mode="single"
          selected={date}
          onSelect={(e) => {
            setDate(e ?? undefined)
            onChange(e ?? undefined)
          }}
        />
      </PopoverContent>
    </Popover>
  )
}


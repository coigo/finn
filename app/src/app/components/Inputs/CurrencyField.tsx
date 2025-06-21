import { Input } from "@/components/ui/input";
import { formatCurrency } from "@/utils/string"

import { Form,  } from "radix-ui";

type InputProps = {
  value?: string
  onChange?: (e: React.ChangeEvent<HTMLInputElement>) => void
  className?: string
  formatValue?: (value: string) => string
  [key: string]: any
}

export function CurrencyField (props: InputProps) {



    return <Input
            className="flex flex-1"
            {...props}
            value={props.value || ""}
            onChange={(e) => {
                e.target.value = formatCurrency(e.target.value)
                if (props.onChange) {
                    props.onChange(e)
                  }
            }}

        />
    
}
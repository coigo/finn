import { TableCellProps } from "@mui/material"
import { ReactNode } from "react"

export interface  FieldProps extends TableCellProps { field?: string, description: string, body?: (data?: any) => string | ReactNode  }


export const Column = (props: FieldProps) => {
    return null
}
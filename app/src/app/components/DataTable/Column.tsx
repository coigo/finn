import { ReactNode } from "react"

export interface  FieldProps { field?: string, description: string, body?: (data?: any) => string | ReactNode  }


export const Column = (props: FieldProps) => {
    return null
}
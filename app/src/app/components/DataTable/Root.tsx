import { ReactElement, ReactNode } from "react";
import { FieldProps } from "./Column";
import { Table, TableBody, TableCell, TableHead, TableRow } from "@/components/ui/table";


export interface ICustomDataTable {
    data: any[]
    children: ReactElement<FieldProps | FieldProps>[]
}

export function Root({ data, children }: ICustomDataTable) {


    return (
            <Table className="w-full">
                <TableHead>
                    <TableRow>
                        {
                            children.map(({ props }, i) => {
                                return (
                                    <TableCell  key={"header" + i} >{props.description}</TableCell>
                                )
                            })
                        }
                    </TableRow>
                </TableHead>
                <TableBody>
                    {data.map((row, i) => (
                        <TableRow
                            key={"row" + i}
                        >
                            {children.map(( {props}, j) => {
                                const content = props.field ? row[props.field ] : props.body && props.body(row)
                                return (
                                    <TableCell key={"coluna" + j + i} scope="row">
                                        { content }
                                    </TableCell>
                                )
                            })}
                        </TableRow>
                    ))}
                </TableBody>
            </Table>
    )
}
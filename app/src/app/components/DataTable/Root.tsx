import { ReactElement, ReactNode } from "react";
import { FieldProps } from "./Column";
import { Table, TableBody, TableCell, TableHead, TableRow } from "@/components/ui/table";


export interface ICustomDataTable {
    data: any[]
    children: ReactElement<FieldProps | FieldProps>[]
    onClickRow?: (data: any) => void
}

export function Root({ data, children, onClickRow }: ICustomDataTable) {


    return (
            <Table className="w-full">
                <TableHead>
                    <TableRow>
                        {
                            children.map(({ props }, i) => {
                                return (
                                    <TableCell {...props} key={"header" + i} >{props.description}</TableCell>
                                )
                            })
                        }
                    </TableRow>
                </TableHead>
                <TableBody>
                    {!data.length ? <div className="py-4">Não há nada por aqui!</div> :
                    data.map((row, i) => (
                        <TableRow
                            onClick={onClickRow ? () => onClickRow(row) : undefined}

                            key={"row" + i}
                        >
                            {children.map(( {props}, j) => {
                                const content = props.field ? row[props.field ] : props.body && props.body(row)
                                return (
                                    <TableCell {...props} key={"coluna" + j + i} scope="row">
                                        { content }
                                    </TableCell>
                                )
                            })}
                        </TableRow>
                    ))
                    }
                    
                </TableBody>
            </Table>
    )
}
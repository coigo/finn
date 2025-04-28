import { Paper, Table, TableBody, TableCell, TableContainer, TableHead, TableRow, TableCellProps } from "@mui/material";
import { ReactElement, ReactNode } from "react";
import { FieldProps } from "./Column";


export interface ICustomDataTable {
    data: any[]
    children: ReactElement<FieldProps | FieldProps>[]
}

export function Root({ data, children }: ICustomDataTable) {


    return (
        <TableContainer sx={{backgroundColor:"transparent", overflow:'hidden'}} component={Paper}>
            <Table sx={{ minWidth: 650 }} aria-label="simple table">
                <TableHead>
                    <TableRow>
                        {
                            children.map(({ props }, i) => {
                                return (
                                    <TableCell key={"header" + i} >{props.description}</TableCell>
                                )
                            })
                        }
                    </TableRow>
                </TableHead>
                <TableBody>
                    {data.map((row, i) => (
                        <TableRow
                            key={"row" + i}
                            sx={{ '&:last-child td, &:last-child th': { border: 0 } }}
                        >
                            {children.map(( {props}, j) => {
                                const content = props.field ? row[props.field ] : props.body && props.body(row)
                                return (
                                    <TableCell key={"coluna" + j + i} component="th" scope="row">
                                        { content }
                                    </TableCell>
                                )
                            })}
                        </TableRow>
                    ))}
                </TableBody>
            </Table>
        </TableContainer>
    )
}
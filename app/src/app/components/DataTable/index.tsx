"use client"

import { Paper, Table, TableBody, TableCell, TableContainer, TableHead, TableRow, TableCellProps } from "@mui/material";
import { ReactNode } from "react";

export type FieldProps = TableCellProps & { field?: string, description: string, body?: (data?: any) => string | ReactNode  }

export interface ICustomDataTable {
    data: any[]
    fields: FieldProps[]
}

export function CustomDataTable({ data, fields }: ICustomDataTable) {


    return (
        <TableContainer sx={{backgroundColor:"transparent", overflow:'hidden'}} component={Paper}>
            <Table sx={{ minWidth: 650 }} aria-label="simple table">
                <TableHead>
                    <TableRow>
                        {
                            fields.map(({ description,...fieldProps }, i) => {
                                return (
                                    <TableCell key={"header" + i} {...fieldProps}>{description}</TableCell>
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
                            {fields.map(({field, body}, j) => {
                                return (
                                    <TableCell key={"coluna" + j + i} component="th" scope="row">
                                        {field ? row[field ] : body && body(row) }
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
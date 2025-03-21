"use client"

import { Paper, Table, TableBody, TableCell, TableContainer, TableHead, TableRow, TableCellProps } from "@mui/material";

export type FieldProps = TableCellProps & { field: string, description: string }

export interface ICustomDataTable {
    data: any[]
    fields: FieldProps[]
}

export function CustomDataTable({ data, fields }: ICustomDataTable) {


    return (
        <TableContainer component={Paper}>
            <Table sx={{ minWidth: 650 }} aria-label="simple table">
                <TableHead>
                    <TableRow>
                        {
                            fields.map(({ field, description,...fieldProps }, i) => {
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
                            {fields.map(({field}, j) => {
                                return (
                                    <TableCell key={"coluna" + j + i} component="th" scope="row">
                                        {row[field]}
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
/***
 * mandar a data
 * de algum jetio mandar os campos tbm
 * ele que no header mapear os campos
 * no body mapear a data e vincular com os campos
 * 
 *  campos
 * --------
 * data > campo
 */
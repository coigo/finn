"use client"

import DataTable from "@/app/components/DataTable"
import { useMovimentacoesHook } from "@/hooks/useBuscarMovimentacoes"
import { totalizarMovimentacoesPorCategoria, totalPorTipo } from "@/utils/array"
import { useEffect, useState } from "react"

import { IoMdArrowDropdown, IoMdArrowDropup } from "react-icons/io";

import dayjs from "dayjs"
import { useBuscarSaldo } from "@/hooks/useBuscarSaldo"
import { Button } from "@/app/components/Button"
import { ProcessarMovimentacoesPendentesRequest } from "@/services/movimentacoes"
import { toast } from "sonner"
import { SelectField } from "@/app/components/Inputs/SelectField"
import { PieChart } from "@/app/components/Charts/pie"
import { CorrigirSaldoPopup } from "./CorrigirSaldoPopup"
import { AdicionarSalarioPopup } from "./AdicionarSalarioPopup"
import { DetalhePopup } from "./DetalhePopup"
import { DetalhePendentePopup } from "./DetalhePendentesPopup"
import { InfoIcon } from "lucide-react"
import { Tabs, TabsContent, TabsList, TabsTrigger } from "@radix-ui/react-tabs"


export const MovimentacoesDetalhes = () => {

    const { movimentacoes, pendentes, buscar, loading, definirPeriodo, periodoPendentes, periodo } = useMovimentacoesHook()
    const { saldo, buscar: buscarSaldo, loading: loadingSaldo } = useBuscarSaldo()
    const movimentacoesAgrupadas = totalizarMovimentacoesPorCategoria(movimentacoes, "SAIDA")
    const total = totalPorTipo(movimentacoes)

    const [pendentesLoading, setPendentesLoadgind] = useState(false)

    useEffect(() => {
        buscar()
        buscarSaldo("Corrente")
    }, [periodo])

    const processarMovimentacoesPendentes = async () => {
        try {
            setPendentesLoadgind(true)
            await ProcessarMovimentacoesPendentesRequest()
            await buscar()
        }
        catch (err: any) {
            toast(err.message)
        }
        finally {
            setPendentesLoadgind(false)
        }
    }

    const refreshSaldo = async () => {
        try {
            await buscarSaldo("Corrente")
        }
        catch (err) {
            toast("Algo deu errado buscando pelo saldo atual!")
        }
    }

    const onDesfazer = async () => {
        try {
            await buscarSaldo("Corrente")
            await buscar()
        }
        catch (err) {
            toast("Algo deu errado buscando pelo saldo atual!")
        }
    }

    const tipoTempl = (row: Movimentacao) => {
        return row.tipo == 'SAIDA'
            ? <IoMdArrowDropdown color="warning" className="text-bold text-[#ec9b30]" />
            : <IoMdArrowDropup color="success" className="text-[#39c429]" />
    }

    const dataTmpl = (row: Movimentacao) => {
        return dayjs(row.data).format('DD/MM/YYYY')
    }

    const valorTempl = (row: Movimentacao) => {
        return `R$ ${row.valor.toFixed(2)}`
    }

    const SelectDate: SelectValues[] = [
        { id: 'SEMANA', name: 'Semana' },
        { id: 'MES', name: 'Mês' },
        { id: 'SEIS_MESES', name: 'Ultímos Seis Mêses' },
        { id: 'DURANTE_ANO', name: 'Durante o Ano' },
        { id: 'DOZE_MESES', name: 'Últimos 12 Mêses' }
    ]

    const tabTriggerClass = "data-[state=active]:bg-background dark:data-[state=active]:text-foreground focus-visible:border-ring focus-visible:ring-ring/50 focus-visible:outline-ring dark:data-[state=active]:border-input dark:data-[state=active]:bg-input/30 text-foreground dark:text-muted-foreground inline-flex h-[calc(100%-1px)] flex-1 items-center justify-center gap-1.5 rounded-md border border-transparent px-2 py-1 text-sm font-medium whitespace-nowrap transition-[color,box-shadow] focus-visible:ring-[3px] focus-visible:outline-1 disabled:pointer-events-none disabled:opacity-50 data-[state=active]:shadow-sm [&_svg]:pointer-events-none [&_svg]:shrink-0 [&_svg:not([class*='size-'])]:size-4"

    return (

        <div className="grid grid-cols-4 gap-4 h-full grid-rows-[min-content_1fr]">
            <div className=" align-center items-center card flex py-4 rounded-2xl h-[8vh] bg-neutral-800/40 shadow-lg !pr-3">
                <div
                    className="flex w-full justify-between gap-5 text-lg mr-2">
                    Saldo <span>R$ {saldo?.valor.toFixed(2) || 0}</span>
                </div>
                <div className="flex gap-1">
                    <CorrigirSaldoPopup onSubmit={refreshSaldo} />
                    <AdicionarSalarioPopup onSubmit={refreshSaldo} />
                </div>
            </div>
            <div className="align-center items-center card flex py-4 rounded-2xl h-[8vh] bg-neutral-800/40 shadow-lg">
                <div className="flex w-full justify-between text-lg">
                    Gastos <span className="text-[#b84149]">R$ {total.saidas.toFixed(2) || 0}</span>
                </div>
            </div>

            <div className="align-center items-center card flex py-4 rounded-2xl h-[8vh] bg-neutral-800/40 shadow-lg">
                <div className="flex w-full justify-between text-lg">
                    Entradas <span className=" text-[#468856]">R$ {total.entradas.toFixed(2) || 0}</span>
                </div>
            </div>

            <div className="align-center items-center card flex py-4 rounded-2xl h-[8vh] bg-neutral-800/40 shadow-lg">
                <div className="flex w-full justify-between text-lg">
                    Investimentos
                    <span className="text-[#de983b]">R$ {total.investimentos.toFixed(2) || 0}</span>
                </div>
            </div>


            <div className=" flex w-full flex-col gap-4 col-span-2">

                <div className="card border-1 border-neutral-600/70 p-4 pt-2 rounded-2xl h-[78.3vh] bg-neutral-800/40 shadow-lg">
                    <div className="block md:flex">
                        <div className=" mb-20 md:mb-0 flex flex-col justify-center w-full ">
                            <div className="m-4 flex font-semibold w-full md:mb-12">
                                <h3 className="mt-2">Saldo</h3>
                                <div className="mx-4 pr-4 font-semibold w-full ">
                                    <SelectField
                                        classname="bg-neutral-700/50 border-none focus:outline-none focus:border-none"
                                        onChange={(e) => definirPeriodo(e as MovimentacoesPeriodo)}
                                        value={periodo}
                                        data={SelectDate}
                                        placeholder="Tipo de Movimentação "

                                    />
                                </div>
                            </div>
                            <div className="flex justify-center w-full h-80">
                                <PieChart loading={loading} data={movimentacoesAgrupadas} />
                            </div>
                            <div className="mt-6 flex flex-col ">
                                <div className="flex justify-between p-4 ">

                                </div>
                                <div className="flex flex-1 scroll-smooth transparent-scrollbar">

                                </div>
                            </div>
                        </div>
                    </div>
                </div>

            </div>

            <div className="card col-span-2 p-4 rounded-2xl flex flex-1 bg-neutral-800/40 shadow-lg overflow-y-scroll scroll-smooth transparent-scrollbar ">
                <Tabs defaultValue="movimentacoes" className="w-full" >
                    <TabsList className="flex gap-2 mb-4">
                        <TabsTrigger className={tabTriggerClass} value="movimentacoes">Movimentações</TabsTrigger>
                        <TabsTrigger className={tabTriggerClass} value="pendentes">Pendentes</TabsTrigger>
                    </TabsList>
                    <TabsContent value="movimentacoes">
                        <DataTable.Root data={movimentacoes} >
                            <DataTable.Column description="" body={tipoTempl} />
                            <DataTable.Column description="Valor" body={valorTempl} />
                            <DataTable.Column description="Categoria" field="categoria" />
                            <DataTable.Column description="Data" body={dataTmpl} />
                            <DataTable.Column description=""
                                body={(row) => <DetalhePopup refresh={onDesfazer} row={row} />}

                                class="flex justify-end pr-2" />
                        </DataTable.Root>
                    </TabsContent>
                    <TabsContent value="pendentes">
                        {pendentes && (
                            <>
                                <div className="flex mb-4">
                                <div className="flex text-start flex-col w-4/5">
                                    <div>Pendentes</div>

                                    {periodoPendentes == 1 && pendentes.length ? (
                                        <div className="flex text-[14px] text-center items-center gap-2 text-neutral-500">
                                            <InfoIcon size="16" />
                                            <span>As pendências a seguir são do próximo mês.</span>
                                        </div>
                                    ) : null}
                                    </div>
                                    <Button
                                        disabled={pendentesLoading || pendentes?.length == 0}
                                        onClick={processarMovimentacoesPendentes}
                                    >
                                        Pagar
                                    </Button>

                                </div>


                                <DataTable.Root data={pendentes}>
                                    <DataTable.Column description="Valor" body={valorTempl} />
                                    <DataTable.Column description="Categoria" field="categoria" />
                                    <DataTable.Column description="Data" body={dataTmpl} />
                                    <DataTable.Column
                                        description="Data"
                                        body={(row) => (
                                            <DetalhePendentePopup refresh={onDesfazer} row={row} />
                                        )}
                                        class="flex justify-end pr-2"
                                    />
                                </DataTable.Root>
                            </>
                        )}
                    </TabsContent>
                </Tabs>
            </div>
        </div>



    )
}
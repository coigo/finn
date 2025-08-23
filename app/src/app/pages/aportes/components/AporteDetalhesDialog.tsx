import { ModalContent } from "@/app/components/Modal/ModalContent"
import { ModalHeader } from "@/app/components/Modal/ModalHeader"
import { useBuscarAporteHistorico } from "@/hooks/useBuscarAporteHistorico"
import { useContext, useEffect, useState } from "react"
import Datatable from "@/app/components/DataTable"
import dayjs from "dayjs"
import { FaDivide, FaPlus, FaMinus } from "react-icons/fa";
import { CurrencyField } from "@/app/components/Inputs/CurrencyField"
import { NumberField } from "@/app/components/Inputs/NumberField"
import { DateField } from "@/app/components/Inputs/DateField"
import { z } from "zod"
import { Controller, useForm } from "react-hook-form"
import { Button } from "@/components/ui/button"
import { IoMdSend } from "react-icons/io";
import { MovimentarAporteRequest } from "@/services/aportes"
import { corrigirTipagem } from "@/lib/utils"
import { toast } from "sonner"
import { useAportesHook } from "@/hooks/UseBuscarAportes"


function schema(aporteTipo: AporteTipo) {
    return z.object({
        quantidade: z.string(),
        preco: aporteTipo == "DESDOBRAMENTO" ? z.string().optional() : z.string(),
        tipoStr: z.custom<AporteTipo>(),
        data: z.date()
    })
}


type props = {
    identificador: string
}
export const AporteDetalhesDialog = ({ identificador }: props) => {
    const [loading, setLoading] = useState(false)
    const { buscar, historico } = useBuscarAporteHistorico()
    const [currentForm, setCurrentForm] = useState<AporteTipo>("COMPRA")

    const {buscar: buscarAportes} = useAportesHook()

    const formSchema = schema(currentForm)
    type MovimentarAporte = z.infer<typeof formSchema>

    const { control, handleSubmit, reset } = useForm<MovimentarAporte>()

    const onSubmit = async (data: MovimentarAporte) => {
        data.tipoStr = currentForm
        setLoading(true)
        try {
            (data as any) = corrigirTipagem(data as any)
            await MovimentarAporteRequest(identificador, data) 
            await buscarAportes()
        }
        catch (err: any) {
            toast(err.message || "Não deu pra movimentar esse aqui")
        }
        finally {
            reset()
            setLoading(false)
        }
    }

    useEffect(() => {
        buscar(identificador)
    }, [])

    const changeForm = (form: AporteTipo) => {
        reset()
        setCurrentForm(form)
    }

    const totalTempl = (row: AporteHistorico) => {
        return row.preco ? `R$ ${row.preco.toFixed(2)} `: 'N/A'
    }

    const dataCompraTempl = (row: AporteHistorico) => {
        return dayjs(row.data).format('DD/MM/YYYY')
    }

    const tipoTempl = (row: AporteHistorico) => {
        let cor
        switch (row.tipo) {
            case "COMPRA":
                cor = "text-[#76c96d]"
                break
            case "VENDA":
                cor = "text-bold text-[#ec9b30]"
                break

        }
        return <div className={cor}>{row.tipo}</div>
    }



    return (
        <>
            <ModalHeader title={`Detalhes de ${identificador}`} />
            <ModalContent  >
                <div className="block md:flex h-full w-full md:h-48">
                    <div className="w-full md:w-5/7 overflow-y-scroll transparent-scrollbar">
                        <Datatable.Root data={historico} >
                            <Datatable.Column description="Preço" body={totalTempl} />
                            <Datatable.Column description="Quantidade" field="quantidade" />
                            <Datatable.Column description="Data" body={dataCompraTempl} />
                            <Datatable.Column description="Tipo" body={tipoTempl} />
                        </Datatable.Root>
                    </div>
                    <div className="flex-none md:flex-1 block md:flex px-4">
                        <div className="w-full md:w-fit md:flex-1">
                            <form className="grid gap-2" onSubmit={() => handleSubmit(onSubmit)}>
                                <div className="px-4 mb-3">{currentForm}</div>
                                { currentForm == "COMPRA" && <Controller
                                    control={control}
                                    name="preco"
                                    render={({ field }) =>
                                        <CurrencyField
                                            placeholder="Preço"
                                            {...field}
                                        />
                                    }
                                />}
                                <Controller
                                    control={control}
                                    name="quantidade"
                                    render={({ field }) =>
                                        <NumberField
                                            placeholder="Quantidade"
                                            {...field}
                                        />
                                    }
                                />
                                <Controller
                                    name="data"
                                    control={control}
                                    render={({ field }) =>
                                        <DateField
                                            {...field}
                                            label="Data"

                                        />
                                    }
                                />
                            </form>
                        </div>
                        <div className="h-full block p-4">
                            <button className="custom-buttom-round hover:!bg-[#404040]"
                                onClick={() => changeForm("COMPRA")}
                            >
                                <FaPlus />
                            </button>
                            <button className="custom-buttom-round hover:!bg-[#404040]"
                                onClick={() => changeForm("VENDA")}
                            >
                                <FaMinus />
                            </button>
                            <button className="custom-buttom-round hover:!bg-[#404040]"
                                onClick={() => changeForm("DESDOBRAMENTO")}
                            >
                                <FaDivide style={{ transform: 'rotate(45deg)' }} />
                            </button>
                            <Button
                                disabled={loading}
                                className="custom-buttom-round hover:!bg-[#404040]"
                                type="submit"
                                onClick={handleSubmit(onSubmit)} >
                                <IoMdSend style={{ color: '#de983b' }} />
                            </Button>
                        </div>
                    </div>
                </div>

            </ModalContent>
        </>
    )

}
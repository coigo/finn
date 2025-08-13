import { ModalContent } from "@/app/components/Modal/ModalContent"
import { ModalHeader } from "@/app/components/Modal/ModalHeader"
import { useBuscarAporteHistorico } from "@/hooks/useBuscarAporteHistorico"
import { useEffect } from "react"
import Datatable from "@/app/components/DataTable"
import dayjs from "dayjs"



type props = {
    identificador: string
}
export const AporteDetalhesDialog = ({ identificador }: props) => {

    const { buscar, historico, loading } = useBuscarAporteHistorico()

    useEffect(() => {
        buscar(identificador)
    }, [])

    const totalTempl = (row: AporteHistorico) => {
        return `R$ ${row.preco.toFixed(2)}`
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
                    <div className="w-full md:w-6/8 overflow-y-scroll transparent-scrollbar">
                        <Datatable.Root data={historico} >
                            <Datatable.Column description="Preço" body={totalTempl} />
                            <Datatable.Column description="Quantidade" field="quantidade" />
                            <Datatable.Column description="Data" body={dataCompraTempl} />
                            <Datatable.Column description="Tipo" body={tipoTempl} />

                        </Datatable.Root>
                    </div>
                    <div className="bg-red-400">a</div>
                </div>

            </ModalContent>
        </>
    )

}
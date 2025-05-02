"use client"

import { useToast } from "@/app/components/Toast/useToast"
import { BuscarMovimentacoesRequest } from "@/services/movimentacoes"
import { periodos } from "@/utils/date"
import dayjs from "dayjs"
import { useState } from "react"

export const useBuscarMovimentacoes = () => {

    const {showToast} = useToast()

    const [ movimentacoes, setMovimentacoes ] = useState<Movimentacao[]>([])
    const [ loading, setLoading ] = useState(false)
    
    const buildDate = (periodo: MovimentacoesPeriodo) => {
        const [inicio, fim] = periodos[periodo]()
        return {
            inicio: dayjs(inicio).format('DD-MM-YYYY'),
            fim: dayjs(fim).format('DD-MM-YYYY'),
        }
    }

    const buscar = async (periodo: MovimentacoesPeriodo) => {

        const data = buildDate(periodo)
        console.log(data)
        
        try {
            setLoading(true)
            const movimentacoes = await BuscarMovimentacoesRequest(data)
            setMovimentacoes(movimentacoes)
            setLoading(false)
        }
        catch ( err: any ) {
            showToast(err.message, "error")
            console.log(err)
            setLoading(false)
        }
    }

    return {
        buscar, 
        movimentacoes, 
        loading
    }

}
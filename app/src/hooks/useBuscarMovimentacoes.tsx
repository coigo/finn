"use client"

import { BuscarMovimentacoesRequest } from "@/services/movimentacoes"
import { useState } from "react"

export const useBuscarMovimentacoes = () => {

    const [ movimentacoes, setMovimentacoes ] = useState([])
    const [ loading, setLoading ] = useState(false)
    
    const buscar = async () => {
        try {
            setLoading(true)
            const movimentacoes = await BuscarMovimentacoesRequest()
            setMovimentacoes(movimentacoes)
            setLoading(false)
        }
        catch ( err: any ) {
            setLoading(false)
        }
    }

    return {
        buscar, 
        movimentacoes, 
        loading
    }

}
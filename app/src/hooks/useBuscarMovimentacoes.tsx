"use client"

import { useToast } from "@/app/components/Toast/useToast"
import { BuscarMovimentacoesRequest } from "@/services/movimentacoes"
import { useState } from "react"

export const useBuscarMovimentacoes = () => {

    const {showToast} = useToast()

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
            showToast(err.message, "error")
            setLoading(false)
        }
    }

    return {
        buscar, 
        movimentacoes, 
        loading
    }

}
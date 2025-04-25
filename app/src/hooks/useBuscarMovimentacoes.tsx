"use client"

import { useToast } from "@/app/components/Toast/useToast"
import { BuscarMovimentacoesRequest } from "@/services/movimentacoes"
import { useState } from "react"

export const useBuscarMovimentacoes = () => {

    const {showToast} = useToast()

    const [ movimentacoes, setMovimentacoes ] = useState<Movimentacao[]>([])
    const [ loading, setLoading ] = useState(false)
    
    const buscar = async () => {
        try {
            setLoading(true)


            const movimentacoes = await BuscarMovimentacoesRequest({inicio: '01-01-2025', fim:'01-06-2025'})
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
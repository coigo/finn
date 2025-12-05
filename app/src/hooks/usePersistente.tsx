"use client"

import { BuscarMovimentacaoPersistente } from "@/services/movimentacoes"
import { useState } from "react"

export const usePersistente = () => {
    const [movimentacao, setMovimentacao] = useState<MovimentacaoPersistente>()
    const [loading, setLoading] = useState<boolean>(true)

    const buscar = async (id: number) => {
        try {
            const data = await BuscarMovimentacaoPersistente(id)
            setMovimentacao(data)
            setLoading(false)
        } 
        catch (err) {
            setLoading(false)
            
        }
    }

    return {
        loading, 
        buscar,
        movimentacao
    }

}
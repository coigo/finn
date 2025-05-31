"use client"

import { useToast } from "@/app/components/Toast/useToast"
import { BuscarMovimentacoesPendentesRequest } from "@/services/movimentacoes"
import { useState } from "react"

export const useBuscarMovimentacoesPendetes = () => {

    const {showToast} = useToast()

    const [ pendentes, setPendentes ] = useState<Movimentacao[]>([])
    const [ loading, setLoading ] = useState(false)
    
    const buscar = async () => {

        try {
            setLoading(true)
            const pendentes = await BuscarMovimentacoesPendentesRequest()
            setPendentes(pendentes)
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
        pendentes, 
        loading
    }

}
"use client"

import { BuscarMovimentacoesPendentesRequest } from "@/services/movimentacoes"
import { useState } from "react"
import { toast } from "sonner"

export const useBuscarMovimentacoesPendetes = () => {


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
            toast(err.message)
            setLoading(false)
        }
    }

    return {
        buscar, 
        pendentes, 
        loading
    }

}
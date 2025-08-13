"use client"

import { BuscarCategoriasRequest, BuscarHistoricoRequest } from "@/services/aportes"
import { useState } from "react"
import { toast } from "sonner"

export const useBuscarAporteHistorico = () => {


    const [ historico, setHistorico ] = useState<AporteHistorico[]>([])
    const [ loading, setLoading ] = useState(false)
    
    const buscar = async (identificador: string) => {
        try {
            setLoading(true)
            const historico = await BuscarHistoricoRequest(identificador)
            setHistorico(historico)
            setLoading(false)
        }
        catch ( err: any ) {
            toast(err.message)
            setLoading(false)
        }
    }

    return {
        buscar, 
        historico, 
        loading
    }

}
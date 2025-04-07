"use client"

import { useToast } from "@/app/components/Toast/useToast"
import { BuscarAportesRequest } from "@/services/aportes"
import { BuscarMovimentacoesRequest } from "@/services/movimentacoes"
import { useState } from "react"

export const useBuscarAportes = () => {

    const { showToast } = useToast()
    const [ aportes, setAportes ] = useState([])
    const [ loading, setLoading ] = useState(false)
    
    const buscar = async () => {
        try {
            setLoading(true)
            const aportes = await BuscarAportesRequest()
            setAportes(aportes)
            setLoading(false)
        }
        catch ( err: any ) {
            showToast(err.message, "error")
            setLoading(false)
        }
    }

    return {
        buscar, 
        aportes, 
        loading
    }

}
"use client"

import { useToast } from "@/app/components/Toast/useToast"
import { BuscarCategoriasRequest } from "@/services/aportes"
import { useState } from "react"

export const useBuscarAporteCategorias = () => {

    const {showToast} = useToast()

    const [ categorias, setCategorias ] = useState([])
    const [ loading, setLoading ] = useState(false)
    
    const buscar = async () => {
        try {
            setLoading(true)
            const categorias = await BuscarCategoriasRequest()
            setCategorias(categorias)
            setLoading(false)
        }
        catch ( err: any ) {
            showToast(err.message, "error")
            setLoading(false)
        }
    }

    return {
        buscar, 
        categorias, 
        loading
    }

}
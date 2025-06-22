"use client"

import { BuscarCategoriasRequest } from "@/services/aportes"
import { useState } from "react"
import { toast } from "sonner"

export const useBuscarAporteCategorias = () => {


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
            toast(err.message)
            setLoading(false)
        }
    }

    return {
        buscar, 
        categorias, 
        loading
    }

}
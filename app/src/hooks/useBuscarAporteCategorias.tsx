"use client"

import { BuscarCategoriasRequest } from "@/services/aportes"
import { useState } from "react"

export const useBuscarAporteCategorias = () => {

    const [ categorias, setCategorias ] = useState([])
    const [ loading, setLoading ] = useState(false)
    
    const buscar = async () => {
        try {
            setLoading(true)
            const categorias = await BuscarCategoriasRequest()
            setCategorias(categorias)
            console.log(categorias)
            setLoading(false)
        }
        catch ( err: any ) {
            setLoading(false)
        }
    }

    return {
        buscar, 
        categorias, 
        loading
    }

}
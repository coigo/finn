"use client"

import { BuscarCategoriasRequest } from "@/services/movimentacoes"
import { useState } from "react"

export const useBuscarMovimentacoesCategoria = () => {

    const [ loading, setLoading ] = useState<boolean>(false)
    const [ categorias, setCategoria ] = useState<MovimentacaoCategoria[]>([])
    
    const buscar = async () => {
        try {
            setLoading(true)
            const categorias = await BuscarCategoriasRequest()
            setCategoria(categorias)
            setLoading(false)
        }
        catch (err: any) {
            setLoading(false)
            
        }
    }

    return {
        buscar,
        loading, 
        categorias
    }

}
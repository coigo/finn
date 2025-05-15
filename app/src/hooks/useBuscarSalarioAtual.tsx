"use client"

import { BuscarSalarioAtualRequest } from "@/services/salario"
import { useState } from "react"

export const useBuscarSalarioAtual = () => {

    const [ loading, setLoading ] = useState<boolean>(false)
    const [ salario, setSalarios ] = useState<MovimentacaoCategoria[]>([])
    
    const buscar = async () => {
        try {
            setLoading(true)
            const salario = await BuscarSalarioAtualRequest()
            setSalarios(salario)
            setLoading(false)
        }
        catch (err: any) {
            setLoading(false)
            
        }
    }

    return {
        buscar,
        loading, 
        salario
    }

}
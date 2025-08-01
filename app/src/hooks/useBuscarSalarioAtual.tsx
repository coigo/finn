"use client"

import { BuscarSalarioAtualRequest, CriarSalarioRequest } from "@/services/salario"
import { useState } from "react"
import { toast } from "sonner"

export const useBuscarSalarioAtual = () => {

    const [ loading, setLoading ] = useState<boolean>(false)
    const [ salario, setSalarios ] = useState<Salario>()
    
    const buscar = async () => {
        try {
            setLoading(true)
            const salario = await BuscarSalarioAtualRequest()
            setSalarios(salario)
            setLoading(false)
        }
        catch (err: any) {
            setLoading(false)
            toast(err.data.message)
        }
    }

    const criar = async (valor: string) => {
        try {
            setLoading(true)
            const salario = await CriarSalarioRequest({valor: Number(valor)})
            setSalarios(salario)
            setLoading(false)
        }
        catch (err: any) {
            setLoading(false)
            
        }
    }

    return {
        criar,
        buscar,
        loading, 
        salario
    }

}
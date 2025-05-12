"use client"

import { BuscarSaldoPorNomeRequest } from "@/services/saldo"
import { useState } from "react"

export const useBuscarSaldo = () => {
    const [loading, setLoading] = useState<boolean>(false)
    const [saldo, setSaldo] = useState<Saldo>()

    const buscar = async (saldoNome: string) => {
        try {
            setLoading(true)
            const data = await BuscarSaldoPorNomeRequest(saldoNome)
            setSaldo(data)
            setLoading(true)
        } 
        catch (err) {
            setLoading(false)
            
        }
    }

    return {
        loading, 
        buscar,
        saldo
    }

}
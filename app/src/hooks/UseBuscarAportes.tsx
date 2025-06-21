"use client"

import { BuscarAportesRequest } from "@/services/aportes"
import { sortBy } from "@/utils/array"
import { createContext, ReactNode, useContext, useState } from "react"
import { toast } from "sonner"

type AporteContextProps = {
    buscar: () => void
    loading: boolean
    aportes: Aporte[]
}

const AporteContext = createContext<AporteContextProps | undefined>(undefined)

export const AportesProvider = ({ children }: { children: ReactNode }) => {

    const [aportes, setAportes] = useState<AporteTotalizado[]>([])
    const [loading, setLoading] = useState(false)

    const buscar = async () => {
        try {
            setLoading(true)
            const aportes = await BuscarAportesRequest()

            const aportescoisados = sortBy(aportes.map((aporte) => {
                return {
                    ...aporte,
                    total: aporte.precoAtual * aporte.quantidade
                }
            })
            , "total")

            setAportes(aportescoisados)
            setLoading(false)
        }
        catch (err: any) {
            toast(err.message)
            setLoading(false)
        }
    }


    return <AporteContext.Provider value={{ buscar, aportes, loading }} >
        {children}
    </AporteContext.Provider>
}

export const useAportesHook = () => {
    const context = useContext(AporteContext)
    if (!context) {
        throw new Error('useAportesHook deve user usado dentro de AportesProvider')
    }
    return context

}



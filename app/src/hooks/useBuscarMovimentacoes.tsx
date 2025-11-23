"use client"

import { BuscarMovimentacoesPendentesRequest, BuscarMovimentacoesRequest } from "@/services/movimentacoes"
import { periodos } from "@/utils/date"
import dayjs from "dayjs"
import { createContext, ReactNode, useContext, useState } from "react"
import { toast } from "sonner"

type MovimentacoesContextProps = {
    buscar: () => void
    definirPeriodo: (periodo: MovimentacoesPeriodo) => void
    loading: boolean
    movimentacoes: Movimentacao[]
    periodo: MovimentacoesPeriodo
    pendentes: MovimentacoesPendentes[]
}

const MovimentacoesContext = createContext<MovimentacoesContextProps | undefined>(undefined)

export const MovimentacoesProvider = ({ children }: { children: ReactNode }) => {


    const [ movimentacoes, setMovimentacoes ] = useState<Movimentacao[]>([])
    const [ pendentes, setPendentes ] = useState<MovimentacoesPendentes[]>([])
    const [ loading, setLoading ] = useState(false)
    const [ periodo, setPeriodo ] = useState<MovimentacoesPeriodo>("MES")
    
    const definirPeriodo = (novoPeriodo: MovimentacoesPeriodo) => {
        setPeriodo(novoPeriodo)
    }

    const buildDate = (periodo: MovimentacoesPeriodo) => {
        const [inicio, fim] = periodos[periodo]()
        return {
            inicio: dayjs(inicio).format('YYYY-MM-DD'),
            fim: dayjs(fim).format('YYYY-MM-DD'),
        }
    }

    const buscar = async () => {

        const data = buildDate(periodo)
        try {
            setLoading(true)
            const [movimentacoes, pendentes] = await Promise.all([
                BuscarMovimentacoesRequest(data),
                BuscarMovimentacoesPendentesRequest()
            ]) 
            setMovimentacoes(movimentacoes)
            setPendentes(pendentes)
            setLoading(false)
        }
        catch ( err: any ) {
            toast(err.message)
            setLoading(false)
        }
    }


    return <MovimentacoesContext.Provider value={{ buscar, definirPeriodo ,movimentacoes, pendentes, loading, periodo }} >
        {children}
    </MovimentacoesContext.Provider>
}

export const useMovimentacoesHook = () => {
    const context = useContext(MovimentacoesContext)
    if (!context) {
        throw new Error('useMovimentacoesHook deve user usado dentro de MovimentacoesProvider')
    }
    return context

}



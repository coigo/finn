import { api } from "./api"

export const  CriarAporteRequest = async (data: any) => {
    await api.post("/aportes", data)
}

export const BuscarAportesRequest = async (): Promise<Aporte[]> => {
    const { data } = await api.get('/aportes') 
    return data
}

export const BuscarCategoriasRequest = async () => {
    const {data} = await api.get('/aportes/categorias')
    return data
}

export const BuscarHistoricoRequest = async (identificador: string) => {
    const {data} = await api.get(`/aportes/${identificador}/historico`)
    return data
}
import { api } from "./api"

export const  CriarAporteRequest = async (data: any) => {
    console.log("asd")
    await api.post("/aportes", data)
}

export const BuscarAportesRequest = async () => {
    const { data } = await api.get('/aportes') 
    return data
}

export const BuscarCategoriasRequest = async () => {
    const {data} = await api.get('/aportes/categorias')
    return data
}
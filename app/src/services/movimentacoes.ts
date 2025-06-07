import { api } from "./api"

export const  CriarMovimentacaoRequest = async (data: any) => {
    await api.post("/movimentacoes", data)
}

export const  BuscarMovimentacoesRequest = async (params: { inicio: string, fim: string }) => {
    const { data } =  await api.get("/movimentacoes", { params })
    return data
}
export const  BuscarMovimentacoesPendentesRequest = async () => {
    const { data } =  await api.get("/movimentacoes/pendentes")
    return data
}

export const  BuscarCategoriasRequest = async () => {
    const { data } =  await api.get("/movimentacoes/categorias")
    return data
}

export const ProcessarMovimentacoesPendentesRequest = async () => {
    return api.post("/movimentacoes/pendentes")
}
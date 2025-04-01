import { api } from "./api"

export const  CriarMovimentacaoRequest = async (data: any) => {
    await api.post("/movimentacoes", data)
}

export const  BuscarMovimentacoesRequest = async () => {
    const { data } =  await api.get("/movimentacoes")
    return data
}
import { api } from "./api"

type EditarPeristente = {
    valor: number,
    descricao: string,
    categoriaId: number
}

export const  CriarMovimentacaoRequest = async (data: any) => {
    await api.post("/movimentacoes", data)
}

export const  BuscarMovimentacoesRequest = async (params: { inicio: string, fim: string }) => {
    const { data } =  await api.get("/movimentacoes", { params })
    return data
}
export const  BuscarMovimentacoesPendentesRequest = async (): Promise<Pendentes> => {
    const { data } =  await api.get("/movimentacoes/pendentes")
    return data
}

export const  BuscarCategoriasRequest = async () => {
    const { data } =  await api.get("/movimentacoes/categorias")
    return data
}

export const ProcessarMovimentacoesPendentesRequest = async () => {
    await api.post("/movimentacoes/pendentes")
}

export const DesfazerMovimentacaoRequest = async (id: number) => {
    await api.delete(`/movimentacoes/${id}`)
}

export const DeletarMovimentacaoPersistente = async (id: number) => {
    await api.delete(`/movimentacoes/persistentes/${id}`)
}

export const BuscarMovimentacaoPersistente = async (id: number) => {
    const { data } = await api.get(`/movimentacoes/persistentes/${id}`)
    return data
}

export const EditarMovimentacaoPersistente = async (payload: any) => {
    const { data } = await api.put(`/movimentacoes/persistentes`, payload)
    return data
}
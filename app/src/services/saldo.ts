import { api } from "./api"

export const BuscarSaldoPorNomeRequest = async (saldoNome: string) => {
    const { data } = await api.get('/saldos', { params: { saldoNome } })
    return data
}
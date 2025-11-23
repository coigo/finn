import { api } from "./api"

type CorrigirSaldo = {
    valor: number,
    nome: string
}

export const BuscarSaldoPorNomeRequest = async (saldoNome: string) => {
    const { data } = await api.get('/saldos', { params: { saldoNome } })
    return data
}

export const CorrigirSaldoRequest = async (data: CorrigirSaldo) => {
    await api.post('/saldos', data)
}
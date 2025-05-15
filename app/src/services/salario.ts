import { api } from "./api"

export const BuscarSalarioAtualRequest = async () => {
    const { data } = await api.get('/salario')
    return data
}

export const CriarSalarioRequest = async (data: { valor: number }) => {
    await api.post('/salario', data)
}
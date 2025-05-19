import { api } from "./api"

export const BuscarSalarioAtualRequest = async () => {
    const { data } = await api.get('/salario')
    return data
}

export const CriarSalarioRequest = async (salario: { valor: number }) => {
    const { data } = await api.post('/salario', salario)
    return data
}
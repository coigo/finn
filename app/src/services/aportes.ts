import { api } from "./api"

export const  CriarAporteRequest = async (data: any) => {
    await api.post("/aportes", data)
}
import { api } from "./api"

export const  CriarAporteRequest = async (data: any) => {
    console.log("asd")
    await api.post("/aportes", data)
}
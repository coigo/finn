import { config } from "@/app/config";
import axios, { AxiosError } from "axios";


export const api = axios.create({
    baseURL: config.baseUrl + "/api",
    headers: {
        "Content-Type": 'application/json'
    },

})

api.interceptors.response.use(
    (response) => { return response },
    (error: AxiosError) => {
        return Promise.reject(errorHandler(error))
    }
)

function errorHandler (error: AxiosError) {
    const response = error.response
    return {
        success: false,
        data: response?.data || { message: "Hmm... Parece que algo deu errado."},
        status: (response?.data as { status?: number })?.status || response?.status
    }
}
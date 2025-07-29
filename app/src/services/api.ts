import { convertStringsToNumbers } from "@/utils/object";
import axios, { AxiosError, InternalAxiosRequestConfig } from "axios";
import { BaseUrl } from "../../vite.env";


export const api = axios.create({
    baseURL: BaseUrl || "http://localhost:5000/api",
    headers: {
        "Content-Type": 'application/json'
    },

})

api.interceptors.request.use((config: InternalAxiosRequestConfig) => {

    if (config.method == "post") {
        config.data = convertStringsToNumbers(config.data)
    }

    return config
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
import { env } from "next-runtime-env"

const NEXT_PUBLIC_API_URL = env("NEXT_PUBLIC_API_URL")

type configExport = {
    baseUrl: string
}

export const config: configExport = {
    baseUrl: NEXT_PUBLIC_API_URL || "http://localhost:5021aaaa"
}
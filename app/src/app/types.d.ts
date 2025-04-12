type Error = {
    succes:boolean
    message: string
    status: number
}

type SelectValues = {
    id: string | number
    name: string
}

type Aporte = {
    identificador: string 
    nomeCurto: string 
    precoMedio: number 
    precoAtual: number 
    quantidade: number 
    categoria: string 
}

type PieData = {
    label: string
    value: number
}
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

type MovimentacaoCategoria = {
    id: number
    name: string
    tipo: MovimentacaoTipo
}

type MovimentacaoTipo = "ENTRADA" |"SAIDA"

type MovimentacaoForm<T> = {
    control: Control<T, any, T>
    handleSubmit: UseFormHandleSubmit<T, T>
    onSubmit: (data: T) => void
}

type TipoMovimentacaoFormulario = "PONTUAL" | "PARCELADA" | "RECORRENTE" | "ENTRADA"

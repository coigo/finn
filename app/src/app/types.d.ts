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
    precoMedio: number 
    precoAtual: number 
    quantidade: number 
    categoria: string 
}

type AporteTotalizado = {
    identificador: string 
    total: number
    precoMedio: number 
    precoAtual: number 
    quantidade: number 
    categoria: string 
}

type Movimentacao = {
    valor: number
    categoria: string
    tipo: string
    data: string
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

type TipoMovimentacaoFormulario = "PONTUAL" | "PARCELADA" | "PERSISTENTE" | "ENTRADA"

type PieData = {
    label: string
    value: number
}

type Saldo = {
    valor: number
    nome: string
}

type Salario = {
    id: number
    valor: number
    criadoEm: Date
}

type MovimentacoesPeriodo = 'SEMANA' | 'MES' | 'SEIS_MESES' | 'DURANTE_ANO' | 'DOZE_MESES' 
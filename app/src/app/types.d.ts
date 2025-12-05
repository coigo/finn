
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

type MovimentarAporte = {
    preco?: string 
    data: Date 
    quantidade: string 
    tipoStr: string 
}

type AporteHistorico = {
    identificador: string 
    preco: number 
    data: Date 
    quantidade: number 
    tipo: string
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
    id: number
    valor: number
    categoria: string
    descricao: string
    tipo: "ENTRADA" | "SAIDA" | "INVESTIMENTOS"
    data: string
}

type MovimentacoesPendentes = {
    id: number
    valor: number
    categoria: string
    tipo: string
    tipoDerivado: "PARCELA" | "PERSISTENTE"
    vencimento: Date
    descricao: string
}
type Pendentes = {
    periodo: 0 | 1
    pendentes: MovimentacoesPendentes[]
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

type AporteTipo = 'COMPRA' | 'VENDA' | 'DESDOBRAMENTO' 

type MovimentacaoPersistente = {
    id: number,
    valor: number,
    descricao: string,
    tipo: number,
    categoriaId: number,
    criadoEm: Date,
    deletadoEm: Date|null
}
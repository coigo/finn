export const groupBy = function (arr: any[], key: any) {
    return arr.reduce(function (acc: any, curr: any) {
        (acc[curr[key]] = acc[curr[key]] || []).push(curr);
        return acc;
    }, {});
};

export const countBy = (arr: any[], key: string) => {
    return arr.reduce((acc: any[], curr: any) => {
        const existente = acc.find(obj => obj.label == curr[key])
        if (existente) {
            existente.value += 1
        }
        else acc.push({ label: curr[key], value: 1 })
        return acc
    }, [])

}

export const filterBy = (arr: any[], key: string, filter: string) => {
    return arr.reduce((acc: any[], curr: any) => {
        curr[key] == filter && acc.push(curr)
        return acc
    }, [])
}

export const totalizarAportes = (aportes: Aporte[]): PieData[] => {
    return aportes.map(aporte => {
        return {
            label: aporte.identificador,
            value: aporte.quantidade * aporte.precoAtual
        }
    })
}

export const totalizarAportesPorCategoria = (aportes: Aporte[]): PieData[] => {
    const aportesAgrupados = groupBy(aportes, "categoria") 

    return Object.keys(aportesAgrupados).map(categoria => {
        const total = aportesAgrupados[categoria].reduce((acc: number, curr: Aporte) => {
            return acc += curr.precoAtual * curr.quantidade
        }, 0)
        return {
            label: categoria,
            value: total
        }
    })
        
            
}

export const totalizarMovimentacoesPorCategoria = (movimentacoes: Movimentacao[]): PieData[] => {
    const movimentacoesAgrupadas = groupBy(movimentacoes, "categoria") 

    return Object.keys(movimentacoesAgrupadas).map(categoria => {
        const total = movimentacoesAgrupadas[categoria].reduce((acc: number, curr: Movimentacao) => {
            return acc += curr.valor 
        }, 0)
        return {
            label: categoria,
            value: total
        }
    })
        
            
}



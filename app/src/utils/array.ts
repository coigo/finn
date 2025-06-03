export function groupBy<T extends Record<string, any>, K extends keyof T>(
    arr: T[],
    key: K
  ): Record<string, T[]> {
    return arr.reduce((acc: Record<string, T[]>, curr: T) => {
      const groupKey = String(curr[key]);
      (acc[groupKey] = acc[groupKey] || []).push(curr);
      return acc;
    }, {});
  }

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

export const totalizarMovimentacoesPorCategoria = (movimentacoes: Movimentacao[], tipo: "ENTRADA" | "SAIDA"): PieData[] => {
    const movimentacoesAgrupadas = groupBy(
        movimentacoes.filter(m => m.tipo == tipo)
        , "categoria"
    ) 

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

export function sortBy (arr: any[], key: string): any[] {
    if ( arr.length <= 1) {
        return arr
    }

    let pivot = arr[0]
    // let pivot = Math.floor(Math.random() * arr.length)
    let leftArry = []
    let rightArry = []

    console.log(pivot)

    for (let i = 1; i < arr.length; i++) { 
        arr[i][key] < pivot[key] 
            ? leftArry.push(arr[i]) 
            : rightArry.push(arr[i])
    }
    return [...sortBy(leftArry, key), pivot, ...sortBy(rightArry, key) ]
}
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
        console.log(aporte)
        return {
            label: aporte.identificador,
            value: aporte.quantidade * aporte.precoAtual
        }
    })
}
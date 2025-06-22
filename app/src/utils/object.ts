export function convertStringsToNumbers(obj: {[x: string]: string}) {
    let result: any = {}
    for(const key in obj) {
        if (typeof obj[key] == 'string' && !isNaN(Number(obj[key]))) {
            result[key] = Number(obj[key])
        }
        else {
            result[key] = obj[key]
        }
    }
    return result
}
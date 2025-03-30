export const formatCurrency = (value: string) => {
    let num = value.replace(/\D/g, "")
    return (Number(num) / 100).toFixed(2);
}
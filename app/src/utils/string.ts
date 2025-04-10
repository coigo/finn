export const formatCurrency = (value: string) => {
    const num = value.replace(/\D/g, "")
    return (Number(num) / 100).toFixed(2);
}
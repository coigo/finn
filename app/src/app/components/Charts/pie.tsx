import { PieChart as Pie } from "@mui/x-charts"

type PieChartProps = {
    data: any[]
    loading: boolean
}

export function PieChart({data, loading}: PieChartProps) {
    return (
        <Pie
        loading={loading}
        width={220}
        height={220}
        colors= {
            ['#e0aa3e', '#de983b', '#f0903a', '#ee7630', '#d86120']

        }
            series={[
                {
                    data: data,
                    innerRadius: 30,
                    outerRadius: 100,
                    paddingAngle: 5,
                    cornerRadius: 5,
                    startAngle: -45,
                    endAngle: 250,
                    cx: 110,
                    cy: 110,
                    
                },

            ]}
            slotProps= {{ legend: { hidden: true } }}


        />
    )
}
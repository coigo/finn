import { CardContent } from "@/components/ui/card"
import { ChartContainer, ChartTooltip, ChartTooltipContent } from "@/components/ui/chart"
import { Pie, ResponsiveContainer, PieChart as Chart, Sector } from "recharts"


type PieChartProps = {
    data: {
        label: string,
        value: number
    }[]
    loading: boolean
}


export function PieChart({ data, loading }: PieChartProps) {

    const colors = ['#e0aa3e', '#de983b', '#f0903a', '#ee7630', '#d86120']
    return (
      loading ? <>Carregando gráfico</> :
      <CardContent className="flex-1 pb-0">
        <ChartContainer
          config={{}}
          className="mx-auto aspect-square max-h-[250px]"
        >
          <Chart>
            <ChartTooltip
              cursor={false}
              content={<ChartTooltipContent hideLabel />}
            />
            <Pie
              data={data.map((obj, i ) => {
                return {
                  ...obj,
                  fill: colors[i % 5]
                }
              }) }
              paddingAngle={3}
              startAngle={-150}
              endAngle={150}
              cornerRadius={6}
              dataKey="value"
              nameKey="label"
              innerRadius={40}

            />
          </Chart>
        </ChartContainer>
      </CardContent>
    );
}
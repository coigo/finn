import { CurrencyField } from "@/app/components/Inputs/CurrencyField"
import { DateField } from "@/app/components/Inputs/DateField"
import { NumberField } from "@/app/components/Inputs/NumberField"
import { SelectField } from "@/app/components/Inputs/SelectField"
import { TextField } from "@/app/components/Inputs/TextField"
import { ModalContent } from "@/app/components/Modal/ModalContent"
import { ModalFooter } from "@/app/components/Modal/ModalFooter"
import { ModalHeader } from "@/app/components/Modal/ModalHeader"
import { formatCurrency } from "@/utils/string"
import { Controller, useForm } from "react-hook-form"

const categorias = [
    {id:1, name:"asdasd"},
    {id:2, name:"dois"},
    {id:3, name:"3"},
    {id:4, name:"jhin"},
]

export const AporteModal = () => {

    const { control, handleSubmit } = useForm()

    const submit = (data: any) => {
        console.log(data)
    }

    return (
        <>
            <ModalHeader title="Aportes"/>
            <ModalContent>
                <form className="md:flex ">
                    <Controller 
                        name="identificador"
                        control={control}
                        render={({field}) => 
                            <TextField 
                                {...field}
                                label="Identificador" 
                            />
                        }
                    />
                    <Controller 
                        name="categoriaId"
                        control={control}
                        render={({field}) =>  
                            <SelectField 
                        data={categorias} 
                        label="Categoria" 
                        {...field} 
                        />
                    }
                    />
                    <Controller 
                    name="quantidade"
                    control={control}
                    render={({ field }) => (
                        <NumberField 
                            {...field}
                            label="Quantidade"
                        />
                    )}
                    />
                    <Controller 
                        name="preco"
                        control={control}
                        render={({field}) => 
                            <CurrencyField 
                                {...field}
                                label="Preço"
                            />
                        }
                    />
                    <Controller 
                        name="dataCompra"
                        control={control}
                        render={({field}) => 
                            <DateField 
                                {...field}
                                label="Data da Compra" 
                                
                            />
                        }
                    />

                </form>
            </ModalContent>
            <ModalFooter>
                <button type="submit" onClick={handleSubmit(submit)}> asd</button>
            </ModalFooter>
        </>        
    )

}
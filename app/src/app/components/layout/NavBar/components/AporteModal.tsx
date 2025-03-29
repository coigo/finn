import { SelectField } from "@/app/components/Inputs/SelectField"
import { TextField } from "@/app/components/Inputs/TextField"
import { ModalContent } from "@/app/components/Modal/ModalContent"
import { ModalFooter } from "@/app/components/Modal/ModalFooter"
import { ModalHeader } from "@/app/components/Modal/ModalHeader"
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

                </form>
            </ModalContent>
            <ModalFooter>
                <button type="submit" onClick={handleSubmit(submit)}> asd</button>
            </ModalFooter>
        </>        
    )

}
"use cliente"
import Menu from "./components/layout/Menu";
import NavBar from "./components/layout/NavBar";
import { Box } from "@mui/material";
import { useModal } from "./components/Modal/useModal";

export default function App({ children }: { children: React.ReactNode }) {

  const {openModal} = useModal()


    return (
        <>
            <Menu />
            <NavBar />
            <Box sx={{ display: "flex", justifyContent: "center" }}>
                            <button onClick={() => openModal(<>weeeeeeeeeeeeeeeeeeeeeeeee</>)}>oiasdasd</button>
                
                <div className="w-full px-8 md:w-3/4 md:px-0 ">
                    {children}
                </div>
            </Box>
        </>
    )
}
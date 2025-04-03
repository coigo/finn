"use cliente"
import Menu from "./components/layout/Menu";
import NavBar from "./components/layout/NavBar";
import { Box } from "@mui/material";
import { useModal } from "./components/Modal/useModal";

export default function App({ children }: { children: React.ReactNode }) {

    return (
        <>
            <Menu />
            <NavBar />
            <Box sx={{ display: "flex", justifyContent: "center", height:'88vh' }}>                
                <div className="w-full h-full px-8 md:w-3/4 md:px-0 overflow-y-scroll md:overflow-y-hidden">
                    {children}
                </div>
            </Box>
        </>
    )
}
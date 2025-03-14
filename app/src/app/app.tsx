"use cliente"
import Menu from "./components/layout/Menu";
import NavBar from "./components/layout/NavBar";
import { Box } from "@mui/material";

export default function App({ children }: { children: React.ReactNode }) {

    return (
        <>
            <Menu />
            <NavBar />
            <Box sx={{ display: "flex", justifyContent: "center" }}>
                <div className="w-full px-8 md:w-3/4 md:px-0 ">
                    {children}
                </div>
            </Box>
        </>
    )
}
import Movimentacoes from "./pages/movimentacoes"
import {Navigate, Route, Routes} from 'react-router-dom'
import Aportes from "./pages/aportes"

export const Router = () => {
    return (
    <Routes >
        <Route path="/movimentacoes" element={<Movimentacoes />} />
        <Route path="/aportes" element={<Aportes />} />
        <Route path="/" element={<Navigate to={'/movimentacoes'} replace/>} />
        
    </Routes>)
}
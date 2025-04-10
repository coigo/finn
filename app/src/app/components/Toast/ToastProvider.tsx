import { Snackbar, useColorScheme, useTheme } from "@mui/material"
import { ReactNode, useState } from "react"
import { createContext } from "react"

import { CheckCircleOutline } from "@mui/icons-material";
import CloseIcon from '@mui/icons-material/Close';
import ErrorOutlineIcon from '@mui/icons-material/ErrorOutline';

type ToastMessage = {
    message: string;
    severity: "success" | "error" | "warning"  ;
};

type ToastContextProps = {
    showToast: (message: string, severity: "success" | "error" | "warning" ) => void;
};

export const ToastContext = createContext<ToastContextProps | undefined>(undefined);

export const ToastProvider = ({ children }: { children: ReactNode }) => {
    const [toast, setToast] = useState<ToastMessage | null>(null);
    const { palette } = useTheme()
    const showToast = (message: string, severity: "success" | "error" | "warning" ) => {
        setToast({ message, severity });
    };

    const { mode } = useColorScheme()

    const icons = {
        "success":  <CheckCircleOutline />,
        "error": <CloseIcon/>,
        "warning": <ErrorOutlineIcon />
    
    }

    return (
        <ToastContext.Provider value={{ showToast }}>
            {children}
            <Snackbar
                className="shadow-2xl mt-2 w-full md:w-1/4"
                open={!!toast}
                autoHideDuration={3000}
                onClose={() => setToast(null)}
                anchorOrigin={{ vertical: "top", horizontal: "right" }}
            >
                <div className={`${mode == "dark" ? "bg-neutral-800/80" : "bg-neutral-300/90"} w-full flex relative  p-4 rounded-lg select-none`}>
                    <span className="mr-3">
                        {toast && icons[toast.severity]}
                    </span>
                    <p style={{color: palette.text.primary}}>
                        {toast && toast.message}
                    </p>
                </div>

            </Snackbar>
        </ToastContext.Provider>
    );
};

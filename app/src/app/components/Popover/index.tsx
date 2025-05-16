import { Button, IconButton, Popover, PopoverRoot } from "@mui/material";
import { ReactNode, useState } from "react";
import SettingsIcon from '@mui/icons-material/Settings';

interface PopoverProps {
    children: ReactNode
}

export default function CustomPopover({children}: PopoverProps) {
    
        const [anchorEl, setAnchorEl] = useState<HTMLButtonElement | null>(null);
      
        const handleClick = (event: React.MouseEvent<HTMLButtonElement>) => {
          setAnchorEl(event.currentTarget);
        };
      
        const handleClose = () => {
          setAnchorEl(null);
        };
      
        const open = Boolean(anchorEl);
        const id = open ? 'popover' : undefined;
      
    
    return (
        <>
            <IconButton  aria-describedby={id} onClick={handleClick}>
                <SettingsIcon className='text-yellow-950' />
            </IconButton>
            <Popover
                id={id}
                open={open}
                anchorEl={anchorEl}
                onClose={handleClose}
                anchorOrigin={{
                    vertical: 'bottom',
                    horizontal: 'left',
                }}
            >
                <div className="p-4 rounded-4xl">
                    {children}
                </div>
            </Popover></>
    )
}
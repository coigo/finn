import { Popover } from "radix-ui";
import { ReactNode, useState } from "react";
import { IoClose } from "react-icons/io5";
import { FaBars } from "react-icons/fa";



interface PopoverProps {
    children: ReactNode
}

export default function CustomPopover({ children }: PopoverProps) {

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

            <Popover.Root>
                <Popover.Trigger asChild>
                    <button className="custom-button-round" aria-label="Update dimensions">
                        <FaBars className="text-bold text-yellow-950" />
                    </button>
                </Popover.Trigger>
                <Popover.Portal>
                    <Popover.Content className="bg-neutral-800 border border-neutral-700 rounded-lg p-4 " sideOffset={5}>
                        <div style={{ display: "flex", flexDirection: "column", gap: 10 }}>
                            {children}
                        </div>
                        <Popover.Arrow className="PopoverArrow" />
                    </Popover.Content>
                </Popover.Portal>
            </Popover.Root>
            {/* <IconButton  aria-describedby={id} onClick={handleClick}>
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
                <div className="p-4 " style={{borderRadius:'16px'}}>
                    {children}
                </div>
            </Popover> */}
        </>
    )
}
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
			<button className="IconButton" aria-label="Update dimensions">
				<FaBars />
			</button>
		</Popover.Trigger>
		<Popover.Portal>
			<Popover.Content className="PopoverContent" sideOffset={5}>
				<div style={{ display: "flex", flexDirection: "column", gap: 10 }}>
					<p className="Text" style={{ marginBottom: 10 }}>
						Dimensions
					</p>
					<fieldset className="Fieldset">
						<label className="Label" htmlFor="width">
							Width
						</label>
						<input className="Input" id="width" defaultValue="100%" />
					</fieldset>
					<fieldset className="Fieldset">
						<label className="Label" htmlFor="maxWidth">
							Max. width
						</label>
						<input className="Input" id="maxWidth" defaultValue="300px" />
					</fieldset>
					<fieldset className="Fieldset">
						<label className="Label" htmlFor="height">
							Height
						</label>
						<input className="Input" id="height" defaultValue="25px" />
					</fieldset>
					<fieldset className="Fieldset">
						<label className="Label" htmlFor="maxHeight">
							Max. height
						</label>
						<input className="Input" id="maxHeight" defaultValue="none" />
					</fieldset>
				</div>
				<Popover.Close className="PopoverClose" aria-label="Close">
					<IoClose />
				</Popover.Close>
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
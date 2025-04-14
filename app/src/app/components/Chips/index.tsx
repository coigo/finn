
type ChipProps = {
    label: string
    active?: boolean
    activeClass?: string
    onClick: () => void
}

export function Chips({label, onClick, active, activeClass}: ChipProps) {
    return (
        <>
            <div  className={`chip text-sm p-1 px-3 mx-2 rounded-4xl ${ active ? activeClass || " bg-[#de983b] text-amber-950" : "bg-neutral-500/50 hover:bg-neutral-600/50 cursor-pointer select-none" } `}  
                onClick={onClick}
            >
                {label}
            </div>
        </>
    )
}
type ButtonProps = React.ButtonHTMLAttributes<HTMLButtonElement>


export const Button = ({ children, ...props }: ButtonProps) => {
    return (
        <button
            className="custom-button bg-[#de983b]  text-amber-950 "
            {...props}
        >{children}
        </button>
    )
}
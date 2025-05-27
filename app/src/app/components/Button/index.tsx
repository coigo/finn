type ButtonProps = React.ButtonHTMLAttributes<HTMLButtonElement>


export const Button = ({children, ...props}: ButtonProps) => {
    return (
        <button
        {...props}
        className="custom-buttom flex-1 bg-[#de983b] text-amber-950 "
        >{children}
        </button>
    )
}
import { clsx, type ClassValue } from "clsx"
import { twMerge } from "tailwind-merge"

export function cn(...inputs: ClassValue[]) {
  return twMerge(clsx(inputs))
}

export function corrigirTipagem (value: {[key: string]: string | number}) {
  Object.keys(value).map(key => {

    if (typeof value[key] == 'string') {
      const parsed = Number(value[key])
      if (!isNaN(parsed)) {
        value[key] = parsed
      }
    }
    
  })
  return value
}
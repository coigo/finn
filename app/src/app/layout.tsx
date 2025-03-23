"use client"

import "./globals.css";
import Providers from "./providers";
import App from "./app";
import { useModal } from "./components/Modal/useModal";

export default function RootLayout({ children }: { children: React.ReactNode }) {



  return (
    <html lang="en" suppressHydrationWarning>

      <body className="root h-screen relative">
        <Providers>
          <App>
            {children}
          </App>
        </Providers>
      </body>
    </html>

  );
}

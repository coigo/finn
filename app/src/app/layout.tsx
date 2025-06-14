import "./globals.css";
import Providers from "./providers";
import App from "./app";
import { PublicEnvScript } from "next-runtime-env";

export default function RootLayout({ children }: { children: React.ReactNode }) {

  return (
    <html lang="en">
      <head >
        <PublicEnvScript />
      </head>
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

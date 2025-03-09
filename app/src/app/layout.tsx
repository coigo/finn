import "./globals.css";
import NavBar from "./components/NavBar";
import { Providers } from "./providers";

export default function RootLayout({ children }: { children: React.ReactNode }) {
  return (
    <html lang="en" suppressHydrationWarning className="dark">
      <body>
        <Providers>
          <NavBar />
          {children}
        </Providers>
      </body>
    </html>
  );
}

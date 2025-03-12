import "./globals.css";
import NavBar from "./components/NavBar";
import Menu from "./components/Menu";

export default function RootLayout({ children }: { children: React.ReactNode }) {
  return (
    <html lang="en" >
      <body className="h-screen relative bg-indigo-950">
          <Menu />
          <NavBar />
          oijkahsd
          {children}
      </body>
    </html>
  );
}

"use client"
import * as React from 'react';
import AppBar from '@mui/material/AppBar';
import Box from '@mui/material/Box';
import Toolbar from '@mui/material/Toolbar';
import Typography from '@mui/material/Typography';
import Button from '@mui/material/Button';
import IconButton from '@mui/material/IconButton';
import MenuIcon from '@mui/icons-material/Menu';
import { useTheme } from 'next-themes';
import { useEffect, useState } from 'react';
import { createTheme, ThemeProvider } from '@mui/material/styles';

export default function NavBar() {
  const { setTheme, resolvedTheme } = useTheme();
  const [muiTheme, setMuiTheme] = useState(createTheme({ palette: { mode: "light" } }));

  useEffect(() => {
    setMuiTheme(createTheme({ palette: { mode: resolvedTheme === "dark" ? "dark" : "light" } }));
  }, [resolvedTheme]);

  const handleChangeTheme = () => {
    setTheme(resolvedTheme === "dark" ? "light" : "dark");
  };

  return (
    <ThemeProvider theme={muiTheme}>
      <Box sx={{ flexGrow: 1 }}>
        <AppBar position="static" color="primary">
          <Toolbar>
            <IconButton size="large" edge="start" color="inherit" aria-label="menu" sx={{ mr: 2 }}>
              <MenuIcon />
            </IconButton>
            <Typography variant="h6" component="div" sx={{ flexGrow: 1 }}>
              News
            </Typography>
            <Button onClick={handleChangeTheme} color="inherit">
              {resolvedTheme === "dark" ? "Light Mode" : "Dark Mode"}
            </Button>
          </Toolbar>
        </AppBar>
      </Box>
    </ThemeProvider>
  );
}

const hoje = new Date();
export const periodos = {
    "SEMANA": () => {
      const inicio = new Date();
      inicio.setDate(hoje.getDate() - 6);
      return [ inicio, hoje ];
    },
  
    "MES": () => {
      const inicio = new Date(hoje.getFullYear(), hoje.getMonth(), 1);
      return [ inicio, hoje ];
    },
  
    "SEIS_MESES": () => {
      const inicio = new Date(hoje);
      inicio.setMonth(hoje.getMonth() - 5);
      inicio.setDate(1);
      return [ inicio, hoje ];
    },
  
    "DURANTE_ANO": () => {
      const inicio = new Date(hoje.getFullYear(), 0, 1);
      return [ inicio, hoje ];
    },
  
    "DOZE_MESES": () => {
      const inicio = new Date(hoje);
      inicio.setFullYear(hoje.getFullYear() - 1);
      inicio.setDate(inicio.getDate() + 1);
      return [ inicio, hoje ];
    }
  };

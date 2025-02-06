namespace Models.Aporte;

public class AporteContext {

    private int id { get ; set; }
    private int valor { get ; set; }
    private AporteTipo tipo { get ; set; }
    private DateTime criadoEm { get ; set; }

} 


public enum AporteTipo {
    Acao,
    FII,
    RendaFixa,
    Criptomoedas,
    Lazer,
    Saude,
    Casa,
    Educacaoo,
    Presentes,
    Pets,
    Tecnologia,
    Servicos,
    Imprevistos,
}
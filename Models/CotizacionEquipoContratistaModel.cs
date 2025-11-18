namespace CotizadorEquipoContratista.Models
{
    public class CotizacionEquipoContratistaModel
    {
        public Cotizacion Cotizacion { get; set; }
        public EContratista EContratista { get; set; }
    }

    public class Cotizacion
    {
        public string Asegurado { get; set; }
        public string Cuit { get; set; }
        public string Domicilio { get; set; }
        public string Organizador { get; set; }
        public string Productor { get; set; }
        public string Provincia { get; set; }
        public bool VisibleOrganizador { get; set; }
    }

    public class EContratista
    {
        public int ActidadId { get; set; }
        public string Actividad { get; set; }
        public string Amortizacion { get; set; }
        public string Caracteristicas { get; set; }
        public string Cobertura { get; set; }
        public DateTime Desde { get; set; }
        public DateTime Hasta { get; set; }
        public string HastaTxt { get; set; }
        public string Id_Tasas { get; set; }
        public string Modelo { get; set; }
        public string Moneda { get; set; }
        public string Operacion { get; set; }
        public string Pais { get; set; }
        public string PorcAsegurado { get; set; }
        public string Raltura { get; set; }
        public string Rcalle { get; set; }
        public string Rlocalidad { get; set; }
        public string Rprovincia { get; set; }
        public decimal Suma { get; set; }
        public decimal SumaPlCollder { get; set; }
        public string Tipocotizacion { get; set; }
        public int Confirmacion { get; set; }
    }
}

namespace CotizadorEquipoContratista.Models
{
    public class ActionResponseList
    {
        public int draw { get; set; }
        public int recordsFiltered { get; set; }
        public int recordsTotal { get; set; }
        public object data { get; set; }
        public string message { get; set; }
        public bool isError { get; set; }
        public bool showMessage { get; set; }
    }

    public class SysCotizacionEquiposContratistasResult
    {
        public int Id_Cotizacion { get; set; }
        public string Nrocotizacion { get; set; }
        public string Fecha { get; set; }
        public string Cuit { get; set; }
        public string Asegurado { get; set; }
        public string Domicilio { get; set; }
        public string Provincia { get; set; }
        public string Productor { get; set; }
        public string Organizador { get; set; }
        public string Pais { get; set; }
        public string Tipocotizacion { get; set; }
        public string Operacion { get; set; }
        public int? Amortizacion { get; set; }
        public string Cobertura { get; set; }
        public int? Rc { get; set; }
        public string Desde { get; set; }
        public string Hasta { get; set; }
        public string Unidad { get; set; }
        public string Caracteristicas { get; set; }
        public string Modelo { get; set; }
        public string Suma { get; set; }
        public string Moneda { get; set; }
        public string Rcalle { get; set; }
        public string Raltura { get; set; }
        public string Rlocalidad { get; set; }
        public string Rprovincia { get; set; }
        public string Rprovinciadesc { get; set; }
        public string Id_Sucursal { get; set; }
        public string Sucursal { get; set; }
        public string Porcasegurada { get; set; }
        public string Actividad { get; set; }
        public string Prima { get; set; }
        public string Total { get; set; }
        public decimal? Couta { get; set; }
        public string UbicacionRiesgoFull { get; set; }
        public string Clase { get; set; }
        public string AmortizacionDesc { get; set; }
        public string BasesMoneda { get; set; }
        public string fileupload { get; set; }
    }
}

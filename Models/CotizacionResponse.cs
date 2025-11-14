namespace CotizadorEquipoContratista.Models
{
    public class CotizacionResponse
    {
        public int Draw { get; set; }
        public int RecordsFiltered { get; set; }
        public int RecordsTotal { get; set; }
        public SysCotizacionEquiposContratistasData Data { get; set; }
        public string Message { get; set; }
        public bool IsError { get; set; }
        public bool ShowMessage { get; set; }
    }

    public class SysCotizacionEquiposContratistasData
    {
        public string Prima { get; set; }
        public string Total { get; set; }
        public decimal? Couta { get; set; }
        public string UrlFile { get; set; }
    }

}



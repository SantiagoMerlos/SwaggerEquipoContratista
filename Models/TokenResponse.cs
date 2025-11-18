namespace CotizadorEquipoContratista.Models
{
    public class TokenResponse
    {
        public TokenResponse() { }

        public TokenResponse(object data)
        {
            Data = data;
            IsError = false;
            Message = string.Empty;
        }

        public TokenResponse(string message, bool isError = true)
        {
            Message = message;
            IsError = isError;
        }

        public object Data { get; set; }
        public string Message { get; set; }
        public bool IsError { get; set; }
    }
}

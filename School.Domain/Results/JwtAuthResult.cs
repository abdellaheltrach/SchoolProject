namespace School.Domain.Results
{
    public class JwtAuthResult
    {
        public bool Succeeded { get; set; }
        public string Message { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }

}

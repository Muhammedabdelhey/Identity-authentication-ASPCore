namespace Identity_Authentication.Dtos
{
    public class TokenResponse
    {
        public required string AccessToken { get; set; }
        public DateTime ExpireOn { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace School.Domain.Responses
{
    public class JwtAuthResponse
    {
        public string AccessToken { get; set; }
        public RefreshToken refreshToken { get; set; }
    }
    public class RefreshToken
    {
        public string UserName { get; set; }
        public string RefToken { get; set; }
        public DateTime ExpireAt { get; set; }
    }
}

﻿namespace Lima.Incomes.Api.Model
{
    public class JwtAuthenticationConfiguration
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int AccessTokenExpirationMinutes  { get; set; }
        public string AccessTokenSecret { get; set; }
        public string RefreshTokenSecret { get; set; }
        public double RefreshTokenExpirationMinutes { get; set; }
    }
}

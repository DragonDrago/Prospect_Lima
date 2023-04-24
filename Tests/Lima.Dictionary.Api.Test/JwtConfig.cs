using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lima.Dictionary.Api.Test
{
    internal class JwtConfig
    {
        public string Issuer { get; set; } = "https://localhost:5001";
        public string Audience { get; set; } = "https://localhost:5001";
        public int AccessTokenExpirationMinutes { get; set; } = 10;
        public string AccessTokenSecret { get; set; } = "9An5DXeJs4Eh78igOjTHeIhzseM3tKOZxIoDTJUVH-lQ10BABJ-WJWfjmt18mHuva83gBgX4R2hfBLcqkPrd16qn0XQVXhb9MjiqTQ0gNFZS6mQWL5tzs8hqthggOM93pG96a4Zsmt7wvfc4Yag_aYOK1DU22lnWb_rFA-KoUyU";
        public string RefreshTokenSecret { get; set; } = "5kapL2xkgrCwF1oMnG1cIMEAxrvyHWWcXirVz0bNnXwsbBlnkTnAbRgqtdLlodbdDohYKtaXKOqgK5UI0DbsD8wXelEaxnMZhL2snS-V0lRje42mt4JSWXsKhmrNhiJRlOIaBhmjcPh2w-F9egtP-tvD94fFfJq8fLZ9UP_Th5E";
        public double RefreshTokenExpirationMinutes { get; set; } = 7200;
    }
}

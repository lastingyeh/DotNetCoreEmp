using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server
{
    public class Constants
    {
        public static string ISSUER = "JwtSettings:issuer";
        public static string AUDIANCE = "JwtSettings:audiance";
        public static string SECRET = "JwtSettings:secret";

    }
}

﻿using System.Collections.Generic;
using System.Security.Claims;
using IdentityModel;
using LeadChina.PM.IdentityServer.Models;

namespace LeadChina.PM.IdentityServer.Utilities
{
    public static class ClaimsUtility
    {
        public static IEnumerable<Claim> GetClaims(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtClaimTypes.Subject, user.Id.ToString()),
                new Claim(JwtClaimTypes.PreferredUserName, user.Username),
            };

            return claims;
        }
    }
}
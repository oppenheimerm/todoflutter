using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoFlutter.data.Infrastructure.Helpers
{
    public class JwtTokenHelper
    {

        public KeyVaultSecret AuthSettingSecretKey { get; }
        public KeyVaultSecret JwtIssuer { get; }
        public KeyVaultSecret JwtAudience { get; }
        public KeyVaultSecret JwtAuthority { get; }
       

        public JwtTokenHelper()
        {
            var _secretClient = new SecretClient(new Uri("https://todoflutterkeyvault.vault.azure.net/"), new DefaultAzureCredential());
            this.AuthSettingSecretKey = _secretClient.GetSecret("AuthSettings--SecretKey").Value;
            this.JwtIssuer = _secretClient.GetSecret("JwtIssuerOptions--Issuer").Value;
            this.JwtAudience = _secretClient.GetSecret("JwtIssuerOptions--Audience").Value;
            this.JwtAuthority = _secretClient.GetSecret("JwtIssuerOptions--Authority").Value;
        }
    }
}


/*
 * 
             var clientSecrects = new SecretClient(new Uri("https://todoflutterkeyvalut.vault.azure.net/"), new DefaultAzureCredential());
            KeyVaultSecret authsettingsSeceretKey = clientSecrects.GetSecret("AuthSettings--SecretKey").Value;
            KeyVaultSecret jwtIssuer = clientSecrects.GetSecret("JwtIssuerOptions--Issuer").Value;
            KeyVaultSecret jwtAudience = clientSecrects.GetSecret("JwtIssuerOptions--Audience").Value;
            KeyVaultSecret jwtAuthority = clientSecrects.GetSecret("JwtIssuerOptions--Authority").Value;
 * 
 */
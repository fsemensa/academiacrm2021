using System;
using System.Net;
using System.ServiceModel.Description;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Tooling.Connector;

namespace TreinamentoExtending {
    class Conexao {

        private static CrmServiceClient crmServiceClientDestino;        

        public CrmServiceClient Obter() {
            //--------------Ambiente para Praticar -----------------//

           /* var connectionStringCRM = @"AuthType=OAuth;
            Username = flavio@academia2021crm.onmicrosoft.com;
            Password = R3s1d3nt2; SkipDiscovery = True;
            AppId = 51f81489-12ee-4a9e-aaae-a2591f45987d;
            RedirectUri = app://58145B91-0C36-4500-8554-080854F2AC97;
            Url = https://org1eab5d24.crm2.dynamics.com/main.aspx;";*/

            //-------Ambiente acompanhado do Professor ------------//

            var connectionStringCRM = @"AuthType=OAuth;
            Username = flarstec@flarstec.onmicrosoft.com;
            Password = R3s1d3nt2; SkipDiscovery = True;
            AppId = 51f81489-12ee-4a9e-aaae-a2591f45987d;
            RedirectUri = app://58145B91-0C36-4500-8554-080854F2AC97;
            Url = https://org5382e030.crm2.dynamics.com/main.aspx;";

            if (crmServiceClientDestino == null) {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                crmServiceClientDestino = new CrmServiceClient(connectionStringCRM);
            }
            return crmServiceClientDestino;
        }
    }
}

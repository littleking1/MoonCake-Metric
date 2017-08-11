/*
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication6
{
    class Program
    {
        static void Main(string[] args)
        {
        }
    }
}
*/

using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication6
{
    class Program
    {
        static void Main(string[] args)
        {
            var token = GetAuthorizationHeader1().Result;

            Console.WriteLine(token);

            Console.ReadLine();

            ExportDB(token);

           // Console.Read();

        }

        static async void ExportDB(string token)
        {
            var struti = "https://management.chinacloudapi.cn/subscriptions/8736ce48-afaf-406a-b5b6-df18d76d2433/resourceGroups/testdw/providers/Microsoft.Sql/servers/testdw/databases/testdw/metrics?api-version=2014-04-01-Preview&$filter=(name/value eq 'dtu_used' or name/value eq 'dtu_limit' or name/value eq 'dtu_consumption_percent' or name/value eq 'cpu_percent') and timeGrain eq '00:05:00' and startTime eq '2017-08-05 01:00:00' and endTime eq '2017-08-11 05:00:00'";
            //var struti = "https://management.chinacloudapi.cn/subscriptions/8736ce48-afaf-406a-b5b6-df18d76d2433/resourceGroups/testdw/providers/Microsoft.Sql/servers/testdw/databases/testdw?api-version=2014-04-01-Preview";
            // For SQL DB DW
            //var struti = "https://management.chinacloudapi.cn/subscriptions/8736ce48-afaf-406a-b5b6-df18d76d2433/resourceGroups/testdw/providers/Microsoft.Sql/servers/testdw/databases/testdw/metrics?api-version=2014-04-01-Preview&$filter=(name/value eq 'cpu_percent') and timeGrain eq '00:05:00' and startTime eq '2017-08-08 02:00:00' and endTime eq '2017-08-11 08:00:00'";
            HttpResponseMessage response = null;
            var client = new HttpClient();

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            response = await client.GetAsync(struti);

            string result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                FileInfo myresult = new FileInfo(@"C:\temp\result.json");
                StreamWriter sw5 = myresult.CreateText();

                sw5.Write(result);
            }
        }

        private async static Task<string> GetAuthorizationHeader1()
        {
            AuthenticationResult result = null;

            var context = new AuthenticationContext(string.Format(
                "https://login.chinacloudapi.cn/{0}",
                "common"));

            var param = new PlatformParameters(PromptBehavior.Always, null);

            result = await context.AcquireTokenAsync("https://management.chinacloudapi.cn/",
                "1950a258-227b-4e31-a9cf-717495945fc2",
                new Uri("urn:ietf:wg:oauth:2.0:oob"),
                param);

            if (result == null)
            {
                throw new InvalidOperationException("Failed to obtain the JWT token");
            }

            string token = result.AccessToken;
            return token;
        }
    }
}


﻿using System;
using System.Threading.Tasks;
using Amica.Sentinel;
using SimpleObjectCache;
using System.IO;

namespace Playground
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Test().Wait();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);
            }
        }

        private static async Task Test()
        {
            var expectedDatabasePath = Path.Combine(
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "test"));

            Directory.CreateDirectory(expectedDatabasePath);

            var db = Path.Combine(expectedDatabasePath, "SimpleObjectCache.db");
            var r = new Sentinel()
            {
                // Unless a valid SSL certificate is installed, we 
                // need Fiddler running with Decrypt HTTPS option active, 
                // see http://docs.telerik.com/fiddler/Configure-Fiddler/Tasks/DecryptHTTPS

                // these are test values only valid on my local machine.
                // have fun using them on a live system.
                ClientId = Environment.GetEnvironmentVariable("SentinelClientId"),
                Username = Environment.GetEnvironmentVariable("SentinelUsername"),
                Password = Environment.GetEnvironmentVariable("SentinelPassword"),
				LocalCache = new SqliteObjectCache() { DatabasePath=db }
            };
            var t = await r.GetBearerAuthenticator();
	    
            Console.WriteLine(r.Token);
	    if (r.HttpResponse != null)
                Console.WriteLine(r.HttpResponse.StatusCode);

            //using (var client = new HttpClient { BaseAddress = new Uri("http://10.0.2.2:9000/") })
            //{
            //    client.DefaultRequestHeaders.Accept.Clear();
            //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //    client.DefaultRequestHeaders.Authorization = t.AuthenticationHeader();


            //    var res = await client.GetAsync("/countries");

            //}
        }
    }
}

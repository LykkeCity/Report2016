using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using AzureStorage.Blob;
using System.Security.Cryptography.X509Certificates;
using Common;

namespace Report2016
{
    public class Program
    {
        public static void Main(string[] args)
        {
			var sertConnString = Environment.GetEnvironmentVariable("CertConnectionString");

			if (string.IsNullOrWhiteSpace(sertConnString) || sertConnString.Length < 10)
			{

				var host = new WebHostBuilder()
					.UseKestrel(opts => opts.ThreadCount = 1)
					.UseContentRoot(Directory.GetCurrentDirectory())
					.UseUrls("http://*:80/")
					.UseIISIntegration()
					.UseStartup<Startup>()
					.Build();

				host.Run();

			}
			else
			{
				var sertContainer = Environment.GetEnvironmentVariable("CertContainer");
				var sertFilename = Environment.GetEnvironmentVariable("CertFileName");
				var sertPassword = Environment.GetEnvironmentVariable("CertPassword");

				var certBlob = new AzureBlobStorage(sertConnString);
				var cert = certBlob.GetAsync(sertContainer, sertFilename).Result.ToBytes();

				X509Certificate2 xcert = new X509Certificate2(cert, sertPassword);

				var host = new WebHostBuilder()
					.UseKestrel(x =>
					{
						x.ThreadCount = 1;
						x.UseHttps(xcert);
						x.AddServerHeader = false;

					})
					.UseContentRoot(Directory.GetCurrentDirectory())
					.UseUrls("https://*:443/")
					.UseIISIntegration()
					.UseStartup<Startup>()
					.Build();

				host.Run();
			}
        }
    }
}

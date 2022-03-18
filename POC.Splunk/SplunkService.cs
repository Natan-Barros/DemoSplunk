using System;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using Splunk.Client;

namespace POC.Splunk
{
    public class SplunkService
    {
        private readonly IService _service;

        public SplunkService()
        {
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) =>
            {
                return true;
            };
            var context = new Context(Scheme.Https, "localhost", 8089, default(TimeSpan), handler);
            ServicePointManager.ServerCertificateValidationCallback +=
                (sender, certificate, chain, errors) =>
                {
                   return true;
                };
            _service = new Service(new Uri(@"https://localhost:8089/service"));
        }
        
        public async void testService()
        {
     
            try
            {
                await _service.LogOnAsync("natanbarros12", "N@tanb3649");

                var transmitter = _service.Transmitter;
                
                var teste = JsonSerializer.Serialize(new 
                    {
                         Successo = "Hello World Success"
                    }
                );
                
                //Aqui envia o conteudo
                await transmitter.SendAsync(teste, "star_wars");
               
                var job = await _service.Jobs.CreateAsync("search index=star_wars | head 10");
                SearchResultStream stream;

                using (stream = await job.GetSearchResultsAsync())
                {
                    try
                    {
                        foreach (var result in stream)
                        {
                            Console.WriteLine($"{stream.ReadCount:D8}: {result}");
                        }
                  
                        Console.WriteLine("End of search results");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"SearchResults error: {e.Message}");
                    }
                }
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
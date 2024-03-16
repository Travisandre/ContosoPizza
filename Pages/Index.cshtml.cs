using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;

namespace ContosoPizza.Pages
{
    public class IndexModel : PageModel
    {
        public TimeSpan TimeInBusiness {get; set; }
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public async void OnGet()
        {
            //Creation of ServiceBus Client & Processor
            ServiceBusClient client;

            ServiceBusProcessor processor;

            async Task MessageHandler(ProcessMessageEventArgs args)
            {
                string body = args.Message.Body.ToString();
                ViewData["Message"] = body;

                await args.CompleteMessageAsync(args.Message);
            }

            Task ErrorHandler(ProcessErrorEventArgs args)
            {
                return Task.CompletedTask;
            }

            // Create the clients that we'll use for sending and processing messages.
            // TODO: Replace the <NAMESPACE-CONNECTION-STRING> placeholder
            client = new ServiceBusClient("Endpoint=sb://shulerii.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=oC4IwblvkLxn82lOeA7ONR2L2QKfBND9t+ASbLVGBwM=");

            // Create a processor that we can use to process the messages
            processor = client.CreateProcessor("shulerii_proj2", "S1", new ServiceBusProcessorOptions());

            try
            {
                processor.ProcessMessageAsync += MessageHandler;

                processor.ProcessErrorAsync += ErrorHandler;

                await processor.StartProcessingAsync();

                Thread.Sleep(2000);

                await processor.StopProcessingAsync();
            } finally 
            {
                await processor.DisposeAsync();
                await client.DisposeAsync();
            }
        }       
    }
}
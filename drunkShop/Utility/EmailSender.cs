//using System;
//using System.Net;
//using Mailjet.Client;
//using Mailjet.Client.Resources;
//using Newtonsoft.Json.Linq;
//using Microsoft.AspNetCore.Identity.UI.Services;

//namespace drunkShop.Utility
//{
//    public class EmailSender : IEmailSender
//    {
//        public Task SendEmailAsync(string email, string subject, string htmlMessage)
//        {
//            return Execute(email, subject, htmlMessage);
//        }

//        //public async Task Execute(string email, string subject, string body)
//        //{
//        //    MailjetClient client = new MailjetClient(Environment.GetEnvironmentVariable("MJ_APIKEY_PUBLIC"), Environment.GetEnvironmentVariable("MJ_APIKEY_PRIVATE"))
//        //    {
//        //        Version = ApiVersion.V3,
//        //    };
//        //    MailjetRequest request = new MailjetRequest
//        //    {
//        //        Resource = Dns.Resource,
//        //        ResourceId = ResourceId.Numeric(ID)
//        //    }
//        // MailjetResponse response = await client.GetAsync(request);
//        //    if (response.IsSuccessStatusCode)
//        //    {
//        //        Console.WriteLine(string.Format("Total: {0}, Count: {1}\n", response.GetTotal(), response.GetCount()));
//        //        Console.WriteLine(response.GetData());
//        //    }
//        //    else
//        //    {
//        //        Console.WriteLine(string.Format("StatusCode: {0}\n", response.StatusCode));
//        //        Console.WriteLine(string.Format("ErrorInfo: {0}\n", response.GetErrorInfo()));
//        //        Console.WriteLine(response.GetData());
//        //        Console.WriteLine(string.Format("ErrorMessage: {0}\n", response.GetErrorMessage()));
//        //    }
//        }
//    }
//}


using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using MyResumeSiteModels;

using SendGrid;
using SendGrid.Helpers.Mail;

namespace MyResumeSiteBackEnd.Services
{
    public class EmailService : IEmailService
    {
        string apiKey = "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";
        public async Task SendAsyncToAdminException(Exception exception)
        {
            try
            {
                List<string> vts = new List<string>() { "No other detail" };
                await SendAsyncToAdminException(exception, vts);
            }
            catch (Exception)
            {
            }
        }

        public async Task SendAsyncToAdminException(Exception exception, object objectDetails)
        {
            try
            {

                string htmlContent = $"<h5>Exception</h5><code>{Newtonsoft.Json.JsonConvert.SerializeObject(exception)}</code><h5>Other</h5><code>{Newtonsoft.Json.JsonConvert.SerializeObject(objectDetails)}</code>";
                await SendEmail("tisquip6@gmail.com", "An Exception Your Resume Site", $"An exception occured on {VariablesCore.ServerUrl}", htmlContent);
            }
            catch (Exception)
            {
            }
        }

        public async Task SendEmail(string emailAddressArg, string subjectArg, string plainTextContentArg, string htmlContentArg = "")
        {
            try
            {
                var client = new SendGridClient(apiKey);
                var from = new EmailAddress("no-reply@mrd.co.zw", "Mr. D");
                var subject = subjectArg;
                var to = new EmailAddress(emailAddressArg);
                var plainTextContent = plainTextContentArg;
                var htmlContent = htmlContentArg;
                var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
                var response = await client.SendEmailAsync(msg);
            }
            catch (Exception)
            {
            }
        }

        public async Task SendEmailToAdmin(string subjectArg, string plainTextContentArg)
        {
            try
            {
                string htmlContent = $"<h5>Exception</h5><code>{plainTextContentArg}</code>";
                await SendEmail("tisquip6@gmail.com", subjectArg, "", htmlContent);
            }
            catch (Exception)
            {
            }
        }
    }
}

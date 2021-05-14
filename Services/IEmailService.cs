using System;
using System.Threading.Tasks;

namespace MyResumeSiteBackEnd.Services
{
    public interface IEmailService
    {
        Task SendAsyncToAdminException(Exception exception);
        Task SendAsyncToAdminException(Exception exception, object objectDetails);
        Task SendEmail(string emailAddressArg, string subjectArg, string plainTextContentArg, string htmlContentArg = "");
        Task SendEmailToAdmin(string subjectArg, string plainTextContentArg);
    }
}
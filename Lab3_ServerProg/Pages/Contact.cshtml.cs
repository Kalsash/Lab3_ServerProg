using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Serilog;
using System.Net.Mail;
using System.Net;

namespace Lab3_ServerProg.Pages
{

    public class ContactModel : PageModel
    {
        [BindProperty]
        public SubscribeModel Subscribe { get; set; }

        [BindProperty]
        public ContactRecord ContactR{ get; set; } 

        public bool ShowSuccessPopup { get; private set; }

        public bool ShowContactSuccess { get; private set; }


        public async Task<IActionResult> OnPostSendAsync()
        {
            ModelState.Remove("Email");
            if (!ModelState.IsValid)
                return Page();

            if (!ContactR.ContactEmail.EndsWith(".edu"))
            {
                ModelState.AddModelError("ContactEmail", "Email must be from a .edu domain.");
                return Page();
            }
            ShowContactSuccess = true;
            // Save data to CSV
            await CsvHelperService.SaveRecordAsync(ContactR);

            return Page();
        }

        public async Task<IActionResult> OnPostSubscribeAsync()
        {
            ModelState.Remove("ContactEmail");
            ModelState.Remove("Name");
            ModelState.Remove("Topic");
            ModelState.Remove("Message");
            if (ModelState.IsValid)
            {
                Log.Information("New subscription from {Email}", Subscribe.Email);
                await SendEmailAsync(Subscribe.Email);
                ShowSuccessPopup = true;
                return Page();
            }
            return Page();
        }

        private async Task SendEmailAsync(string email)
        {
            string myEmail = "kalsashs@gmail.com";
            string myPassword = System.IO.File.ReadAllText("PostPassword.txt").Trim();
            using (var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(myEmail, myPassword),
                EnableSsl = true,
            })
            {
                var mailMessage = new MailMessage(myEmail, email, "Subscription Confirmation", "Thank you for subscribing!");
                await smtpClient.SendMailAsync(mailMessage);
            }
        }
        public IActionResult OnPostClosePopup()
        {
            ShowSuccessPopup = false;
            return Page();
        }
        public IActionResult OnPostCloseContact()
        {
            ShowContactSuccess = false;
            return Page();
        }
        public void OnGet()
        {

        }
    }
}

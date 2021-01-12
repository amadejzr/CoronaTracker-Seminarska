using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using web.Models;
using web.Data;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading;
using System.ComponentModel;

namespace web.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<Uporabnik> _signInManager;
        private readonly UserManager<Uporabnik> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly CoronaContext _context;

        public RegisterModel(
            UserManager<Uporabnik> userManager,
            SignInManager<Uporabnik> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            CoronaContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _context = context;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {

            [Required]
            [Display(Name = "Ime")]
            public string Ime { get; set; }

            [Required]
            [Display(Name = "Priimek")]
            public string Priimek { get; set; }


            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [Display(Name = "Mesto")]
            public String Mesto { get; set; }

            [Required]
            [Display(Name = "Telefon")]
            public String Telefon { get; set; }

            [Required]
            [Display(Name = "Naslov")]
            public String Naslov { get; set; }


            [Required]
            [Display(Name = "Datum zacetka")]
            public DateTime DatumZacetka { get; set; }

            [Required]
            [Display(Name = "Datum konca")]
            public DateTime DatumKonca { get; set; }
        }
        

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var odlok = new Odlok {DatumZacetka = Input.DatumZacetka, DatumKonca = Input.DatumKonca};
                var prebivalisce= new Prebivalisce{Naslov = Input.Naslov, Mesto= Input.Mesto};
                var user = new Uporabnik { UserName = Input.Email, Email = Input.Email,Ime = Input.Ime,Priimek = Input.Priimek,Telefon = Input.Telefon,Odloki = odlok,Prebivalisca = prebivalisce};
                
               

                string validChars = "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789-&#";  
                Random random = new Random();
                int length = 6;  
  
    
                char[] chars = new char[length+3]; 
                chars[0] = 'a';
                chars[1] = 'A';
                chars[2] = '-';
                chars[3] = '1';    
                for (int i = 4; i < length+4; i++)  
                {  
                     chars[i] = validChars[random.Next(0, validChars.Length)];  
                } 
                string pw = new string(chars); 


                var result = await _userManager.CreateAsync(user, pw);
                
                SmtpClient client = new SmtpClient("coronatracker333@gmail.com");
                client.Credentials = new NetworkCredential("coronatracker333@gmail.com", "CoronaisBad");
                var smtpClient = new SmtpClient("smtp.gmail.com");
                smtpClient.Port = 587;
                smtpClient.Credentials = new NetworkCredential("coronatracker333@gmail.com", "CoronaisBad");
                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = false;

                var mailMessage = new MailMessage
                {
            
                    From = new MailAddress("coronatracker333@gmail.com"),
                    Subject = "Karantena",
                    Body = $"Pozdravljeni,{Environment.NewLine}{Environment.NewLine}Vaš test je bil pozitiven. Prosimo vas, da se prijavite na spletno stran https://coronatrackerr.azurewebsites.net z vašim emailom in z geslom: {pw}{Environment.NewLine}{Environment.NewLine}Naš sistem vam bo omogočal, da vpišete stike in spremljate kdaj vam poteče karantena.",
                    
                };
                mailMessage.To.Add(Input.Email);
                
            
                
            
                
                

                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");
                    smtpClient.Send(mailMessage);
                    

                    

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");
                    return RedirectToPage("Register");
                    
                    
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}

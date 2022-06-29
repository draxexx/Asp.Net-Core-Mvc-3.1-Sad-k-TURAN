using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using shopapp.business.Abstract;
using shopapp.webui.EmailServices;
using shopapp.webui.Extensions;
using shopapp.webui.Identity;
using shopapp.webui.Models;

namespace shopapp.webui.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class AccountController:Controller
    {
        private UserManager<User> _userManager;
        private SignInManager<User> _signInManager;
        private IEmailSender _emailSender;
        private ICartService _cartService;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, IEmailSender emailSender, ICartService cartService)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._emailSender = emailSender;
            this._cartService = cartService;
        }

        [HttpGet]
        public IActionResult Login(string ReturnUrl=null){
            return View(new LoginModel(){
                ReturnUrl = ReturnUrl,
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model){

            if(!ModelState.IsValid){
                return View(model);
            }

            //var user = await _userManager.FindByNameAsync(model.UserName);

            var user = await _userManager.FindByEmailAsync(model.Email);

            if(user==null){
                ModelState.AddModelError("","Bu email ile daha önce hesap oluşturulmamış");
                return View(model);
            }

            if(!await _userManager.IsEmailConfirmedAsync(user)){
                ModelState.AddModelError("","Lütfen email adresinize gelen link ile hesabınızı onaylayınız.");
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(user,model.Password,false,false);

            if(result.Succeeded){
                return Redirect(model.ReturnUrl??"~/"); //ReturnUrl null ise ~/ ile direk anasayfaya gider yok ise ReturnUrl gider
            }

            ModelState.AddModelError("","Girilen email veya parola yanlış");
            return View(model);
        }

        public async Task<IActionResult> Logout(){
            await _signInManager.SignOutAsync();
            TempData.Put("message",new AlertMessage(){
                    Title = "Oturum Kapatıldı",
                    Message = "Hesabınız güvenli bir şekilde kapatıldı.",
                    AlertType = "warning",
                });
            return Redirect("~/");
        }

        [HttpGet]
        public IActionResult Register(){
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken] //get ile gönderilen token post'a gelmiyorsa bize hata verecektir, bu şekilde cross site ataklarının önüne geçilir
        public async Task<IActionResult> Register(RegisterModel model){

            if(!ModelState.IsValid){
                return View(model);
            }

            var user = new User(){
                FirstName = model.FirstName,
                LastName = model.LastName,
                UserName = model.UserName,
                Email = model.Email,
            };
            
            var result = await _userManager.CreateAsync(user,model.Password);

            if(result.Succeeded){

                //üye olduğu an cart tablosu içerisine userID kayıtını yapıyoruz
                _cartService.InitializeCart(user.Id);

                //await _userManager.AddToRoleAsync(user,"customer"); //customer role atıyoruz yeni üye olan kullanıcıyı
                
                //generate token
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user); //token oluşturur
                var url = Url.Action("ConfirmEmail","Account", new {
                    userId = user.Id,
                    token = code,
                }); //url oluşturuyoruz

                //await _emailSender.SendEmailAsync(model.Email,"Hesabınızı onaylayınız.",$"Lütfen email hesabınızı onaylamak için linke <a href='http://localhost:5000{url}'>tıklayınız.</a>");
                return RedirectToAction("Login","Account");
            }

            ModelState.AddModelError("","Bilinmeyen bir hata oldu lütfen tekrar deneyiniz.");

            return View(model);
        }
    
        public async Task<IActionResult> ConfirmEmail(string userId, string token){

            if(userId==null || token == null){
                TempData.Put("message",new AlertMessage(){
                    Title = "Geçersiz Token",
                    Message = "Geçersiz token nedeniyle onaylama işlemi başarısız.",
                    AlertType = "danger",
                });
                return View();
            }

            var user = await _userManager.FindByIdAsync(userId);

            if(user==null){
                TempData.Put("message",new AlertMessage(){
                    Title = "Kullanıcı Bulunamadı",
                    Message = "Kullanıcı bulunamadığından dolayı hesabınız onaylanmadı.",
                    AlertType = "warning",
                });
                return View();
            }

            var result = await _userManager.ConfirmEmailAsync(user,token);

            if(result.Succeeded){
                TempData.Put("message",new AlertMessage(){
                    Title = "Hesabınız Onaylandı",
                    Message = "Hesabınızın onaylanma işlemi başarıyla gerçekleştirildi.",
                    AlertType = "success",
                });
                return View();
            }

            return View();
        }
    
        public IActionResult ForgotPassword(){
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string Email){

            if(string.IsNullOrEmpty(Email)){
                return View();
            }

            var user = await _userManager.FindByEmailAsync(Email);

            if(user==null){
                return View();
            }

            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            
            var url = Url.Action("ResetPassword","Account", new {
                userId = user.Id,
                token = code,
            }); //url oluşturuyoruz

            //await _emailSender.SendEmailAsync(model.Email,"Reset Password",$"Parolanızı yenilemek için linke <a href='http://localhost:5000{url}'>tıklayınız.</a>");

            return View();
        }

        public IActionResult ResetPassword(string userId, string token){

            if(userId==null || token == null){
                return RedirectToAction("Index","Home");
            }

            var model = new ResetPasswordModel{Token=token};

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel model){

            if(!ModelState.IsValid){
                return View(model);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);

            if(user==null){
                return RedirectToAction("Index","Home");
            }

            var result = await _userManager.ResetPasswordAsync(user,model.Token,model.Password);

            if(result.Succeeded){
                return RedirectToAction("Login","Account");
            }
            return View(model);
        }
        
        public IActionResult AccessDenied(){
            return View();
        }
    }
}
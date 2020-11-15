using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using TODOLists.Models;
using TODOLists.DAL;
using System.Data.Entity.SqlServer;
using System.Security.Cryptography;
using System.IO;
using System.Text;
using System.Data.Entity.Infrastructure;

namespace TODOLists.Controllers
{
    public class AccountController : Controller
    {
        // Return Home Page
        public ActionResult Index()
        {
            return View();
        }

        //GET: Register
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        //POST:Register
        [HttpPost]
        
        public ActionResult AddUser(Tbl_User registerUser)
        {
            if (ModelState.IsValid)
            {
                using (var databaseContext = new TODOListDBEntities())
                {
                    try
                    {
                        registerUser.Password = Encrypt(registerUser.Password);
                        databaseContext.Tbl_User.Add(registerUser);
                        databaseContext.SaveChanges();
                    }catch(DbUpdateException e)
                    {
                        ViewBag.Message = "Username ALready Exists";
                        return View("Register");
                    }
                    

                }

                ViewBag.Message = "User Details Saved";
                return View("Register");
            }
            else
            {
                //If the validation fails, we are returning the model object with errors to the view, which will display the error messages.
                return View("Register", registerUser);
                
            }
            
        }

        // GET: Login
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        // POST: Login
        [HttpPost]
        public ActionResult VerifyLogin(Tbl_User loginUser)
        {
            if (ModelState.IsValid)
            {
                var isValidUser = IsValidUser(loginUser);

                if (isValidUser != null)
                {
                    FormsAuthentication.SetAuthCookie(loginUser.Username, false);
                    return RedirectToAction("Index");
                }
                else
                {

                    ModelState.AddModelError("Failure", "Wrong Username and password combination !");
                    return View("Login");
                }
            }
            else
            {
                //If model state is not valid, the model with error message is returned to the View.
                return View("Login",loginUser);
            }



        }

        private static byte[] Generate256BitsOfRandomEntropy()
        {
            var randomBytes = new byte[32]; // 32 Bytes will give us 256 bits.
            using (var rngCsp = new RNGCryptoServiceProvider())
            {
                // Fill the array with cryptographically secure random bytes.
                rngCsp.GetBytes(randomBytes);
            }
            return randomBytes;
        }
        // This constant is used to determine the keysize of the encryption algorithm in bits.
        // We divide this by 8 within the code below to get the equivalent number of bytes.
        private const int Keysize = 256;

        // This constant determines the number of iterations for the password bytes generation function.
        private const int DerivationIterations = 1000;

        public static string Encrypt(string plainText)
        {
            String output = "";
            foreach(char c in plainText.ToCharArray())
            {
                output = output + (char)(c + 10);
            }
            return output;
        }

        public static string Decrypt(string cipherText)
        {
            String output = "";
            foreach (char c in cipherText.ToCharArray())
            {
                output = output + (char)(c - 10);
            }
            return output;
        }
        //function to check if User is valid or not
        [HttpPost]
        public ActionResult IsValidUser(Tbl_User loginUser)
        {
            using (var dataContext = new TODOListDBEntities())
            {
                //String password = Decrypt(loginUser.Password);
                //Retireving the user details from DB based on username and password enetered by user.
                Tbl_User addedUser = dataContext.Tbl_User.Where(query => query.Username.Equals(loginUser.Username)).SingleOrDefault();
                //If user is not present, then false is returned.
                if (addedUser == null)
                {
                    ViewBag.Message = "Incorrect username";
                    return View("Login");
                }
                //If user is present true is returned.
                else
                {
                    String testpassword = loginUser.Password;
                    String password = Decrypt(addedUser.Password);
                    if (password.Equals(testpassword))
                    {
                        Session["UserInfo"] = addedUser;
                        return RedirectToAction("ToDo", "Task");
                    }
                    ViewBag.Message = "Incorrect password";
                    return View("Login");
                }
            }
        }



        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Abandon(); // it will clear the session at the end of request
            return RedirectToAction("Index","Home");
        }


    }
}
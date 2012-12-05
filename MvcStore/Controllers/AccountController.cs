﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using MvcStore.Models;

namespace MvcStore.Controllers
{
    public class AccountController : Controller
    {
		readonly IRepository<Cart> cartRepo;
		readonly IRepository<CartItem> cartItemRepo;
		
		public AccountController (IRepository<Cart> cartRepo, IRepository<CartItem> cartItemRepo)
		{
			if (cartItemRepo == null)
				throw new ArgumentNullException ("cartItemRepo");
			this.cartItemRepo = cartItemRepo;
			if (cartRepo == null)
				throw new ArgumentNullException ("cartRepo");
			this.cartRepo = cartRepo;
		}

        //
        // GET: /Account/LogOn

        public ActionResult LogOn()
        {
            return View();
        }

        //
        // POST: /Account/LogOn

        [HttpPost]
        public ActionResult LogOn(LogOnModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (Membership.ValidateUser(model.UserName, model.Password))
                {
					MigrateShoppingCart(model.UserName);
					
                    FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
                    if (IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                        && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Store");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "The user name or password provided is incorrect.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/LogOff

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
			Session.Clear ();

            return RedirectToAction("Index", "Store");
        }

        //
        // GET: /Account/Register

        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register

        [HttpPost]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                // Attempt to register the user
                MembershipCreateStatus createStatus;
                Membership.CreateUser(model.UserName, model.Password, model.Email, "question", "answer", true, null, out createStatus);

                if (createStatus == MembershipCreateStatus.Success)
                {
					MigrateShoppingCart(model.UserName);
					
                    FormsAuthentication.SetAuthCookie(model.UserName, false /* createPersistentCookie */);
                    return RedirectToAction("Index", "Store");
                }
                else
                {
                    ModelState.AddModelError("", ErrorCodeToString(createStatus));
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ChangePassword

        [Authorize]
        public ActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Account/ChangePassword

        [Authorize]
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {

                // ChangePassword will throw an exception rather
                // than return false in certain failure scenarios.
                bool changePasswordSucceeded;
                try
                {
                    MembershipUser currentUser = Membership.GetUser(User.Identity.Name, true /* userIsOnline */);
                    changePasswordSucceeded = currentUser.ChangePassword(model.OldPassword, model.NewPassword);
                }
                catch (Exception)
                {
                    changePasswordSucceeded = false;
                }

                if (changePasswordSucceeded)
                {
                    return RedirectToAction("ChangePasswordSuccess");
                }
                else
                {
                    ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ChangePasswordSuccess

        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }

        #region Status Codes
        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        #endregion

		private bool IsLocalUrl(string url)
		{
			if (string.IsNullOrEmpty(url))
			{
				return false;
			}
			
			Uri absoluteUri;
			if (Uri.TryCreate(url, UriKind.Absolute, out absoluteUri))
			{
				return String.Equals(this.Request.Url.Host, absoluteUri.Host, 
				                     StringComparison.OrdinalIgnoreCase);
			}
			else
			{
				bool isLocal = !url.StartsWith("http:", StringComparison.OrdinalIgnoreCase)
					&& !url.StartsWith("https:", StringComparison.OrdinalIgnoreCase)
						&& Uri.IsWellFormedUriString(url, UriKind.Relative);
				return isLocal;
			}
		}
		
		void MigrateShoppingCart (string userName)
		{
			// Associate shopping cart items with logged-in user
			var cart = cartRepo.GetCart (HttpContext);
			
			// check if user already has a cart
			var userCart = cartRepo.GetItems ().SingleOrDefault (c => c.Owner == userName);
			// if user already has a cart
			if (userCart != null) {
				// migrate items
				var oldCart = cart;
				cart = userCart;
				foreach (var item in oldCart.Items) {
					for (int i = 0; i < item.Count; i++)
						cartRepo.AddToCart (cart, item.Product, cartItemRepo);
				}
				
				// delete anonymous cart
				cartRepo.DeleteItem (oldCart);
			} else {
				// just rename the cart
				cart.Owner = userName;
				cartRepo.UpdateItem (cart);
			}
			
			Session [RepoExtensions.CartSessionKey] = userName;
		}
    }
}

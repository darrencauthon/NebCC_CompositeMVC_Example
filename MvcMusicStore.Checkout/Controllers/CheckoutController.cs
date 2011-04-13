﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MvcMusicStore.Checkout.Helpers;
using MvcMusicStore.Checkout.Models;

namespace MvcMusicStore.Checkout.Controllers
{
    [Authorize]
    public class CheckoutController : Controller
    {
        private readonly ICartRetriever cartRetriever;
        CheckoutEntities storeDB = new CheckoutEntities();
        const string PromoCode = "FREE";

        public CheckoutController(ICartRetriever cartRetriever)
        {
            this.cartRetriever = cartRetriever;
        }

        //
        // GET: /Checkout/AddressAndPayment

        public ActionResult AddressAndPayment()
        {
            return View();
        }

        //
        // POST: /Checkout/AddressAndPayment
        [HttpPost]
        public ActionResult AddressAndPayment(FormCollection values)
        {
            var order = new Order();
            order.OrderDetails = new List<OrderDetail>();

            try
            {
                // Updat the model
                UpdateModel(order);

                if (string.Equals(values["PromoCode"], 
                    PromoCode, 
                    StringComparison.OrdinalIgnoreCase) == false)
                {
                    return View(order);
                }
                else
                {
                    if (ModelState.IsValid)
                    {
                        // Promo Code supplied
                        order.Username = User.Identity.Name;
                        order.OrderDate = DateTime.Now;

                        // Save Order
                        storeDB.Orders.Add(order);
                        storeDB.SaveChanges();

                        // Process the order
                        //var cart = ShoppingCart.GetCart(this);
                        var cart = cartRetriever.GetTheCurrentCart();
                        cart.CreateOrder(order);

                        return RedirectToAction("Complete", new { id = order.OrderId });
                    }
                    else
                    {
                        throw new Exception("Model State is not valid!");
                    }
                }
            }
            catch
            {
                // Invalid -- redisplay with errors
                return View(order);
            }
        }

        //
        // GET: /Checkout/Complete

        public ActionResult Complete(int id)
        {
            // Validate that the customer owns this order
            bool isValid = storeDB.Orders.Any(
                o => o.OrderId == id && o.Username == User.Identity.Name);

            if (isValid)
            {
                return View(id);
            }
            else
            {
                return View("Error");
            }
        }
    }
}
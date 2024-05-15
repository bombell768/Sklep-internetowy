using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Lista10_v2.Data;
using Lista10_v2.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;

namespace Lista10_v2.Controllers
{
    public class ShopController : Controller
    {
        private readonly ShopDbContext _context;

        public ShopController(ShopDbContext context)
        {
            _context = context;
        }

        // GET: Shop
        public IActionResult Index()
        {
            var categories = _context.Category.ToList();
            return View(categories);
        }

        // GET: Shop/Category/5
        public IActionResult Category(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = _context.Category
                .Include(c => c.Articles)
                .FirstOrDefault(c => c.Id == id);

            if (category == null)
            {
                return NotFound();
            }
            if (Request.Cookies.Count > 0)
            {
                // Istnieją ciasteczka, wykonaj odpowiednie działania
                // Możesz np. przeiterować przez ciasteczka i wyświetlić ich zawartość
                foreach (var cookie in Request.Cookies)
                {
                    var cookieName = cookie.Key;
                    var cookieValue = cookie.Value;
                    // Przykładowe działanie, np. wyświetlenie w konsoli
                    Console.WriteLine($"Nazwa ciasteczka: {cookieName}, Wartość: {cookieValue}");
                }
            }
            else
            {
                Console.WriteLine("Brak ciasteczek!");
            }

            return View(category);
        }

		[Authorize(Policy = "AccessToCart")]
		public IActionResult AddToCart(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var article = _context.Article.FirstOrDefault(a => a.Id == id);

            if (article == null)
            {
                return NotFound();
            }

            // Sprawdź, czy istnieje ciasteczko dla danego artykułu
            var cartItem = Request.Cookies[$"cart_{article.Id}"];
            if (cartItem == null)
            {
                // Jeśli artykuł nie istnieje w koszyku, utwórz nowe ciasteczko dla niego
                SetCookie($"{article.Id}", "1");
            }
            else
            {
                // Jeśli artykuł już istnieje w koszyku, zwiększ jego licznik
                var quantity = int.Parse(cartItem) + 1;
                SetCookie($"{article.Id}", quantity.ToString());
            }

            return RedirectToAction("Category", new { id = article.CategoryId });
        }

		[Authorize(Policy = "AccessToCart")]
		public IActionResult Cart()
        {
            var cartItems = ItemsFromCookies();

            // Pobierz informacje o artykułach
            var articles = _context.Article.Where(a => cartItems.Keys.Contains(a.Id)).ToList();

            if (cartItems.Count > 0 && articles.Count > 0) {
                // Przekazanie danych do widoku
                ViewData["CartItems"] = cartItems;
                ViewData["Articles"] = articles;
            }
            else
            {
                cartItems = null;
                articles = null;
            }

            return View();
        }

        [Authorize(Policy = "AccessToCart")]
        [HttpPost]
        public IActionResult UpdateCart(int? articleId, int? quantity)
        {
            if (articleId == null || quantity == null)
            {
                return NotFound();
            }

            var article = _context.Article.FirstOrDefault(a => a.Id == articleId);

            if (article == null)
            {
                return NotFound();
            }

            // Aktualizuj liczbę sztuk artykułu w koszyku
            SetCookie($"{article.Id}", quantity.ToString());

            return RedirectToAction("Cart");
        }

        [Authorize(Policy = "AccessToCart")]
        public IActionResult RemoveFromCart(int? articleId)
		{
			if (articleId == null)
			{
				return NotFound();
			}

			var article = _context.Article.FirstOrDefault(a => a.Id == articleId);

			if (article == null)
			{
				return NotFound();
			}

			// Usuń ciasteczko artykułu z koszyka
			Response.Cookies.Delete($"cart_{article.Id}");

			return RedirectToAction("Cart");
		}

        [Authorize(Policy = "AccessToCart")]
        [Authorize]
        public IActionResult Checkout()
        {
            var cartItems = ItemsFromCookies();

            // Pobierz informacje o artykułach
            var articles = _context.Article.Where(a => cartItems.Keys.Contains(a.Id)).ToList();

            if (cartItems.Count > 0 && articles.Count > 0)
            {
                // Przekazanie danych do widoku
                ViewData["CartItems"] = cartItems;
                ViewData["Articles"] = articles;
            }
            else
            {
                cartItems = null;
                articles = null;
            }

            return View();
        }


        [Authorize(Policy = "AccessToCart")]
        [Authorize]
        [HttpPost]
        public IActionResult ConfirmOrder(string firstName, string lastName, string street, string buildingNumber, string apartmentNumber, string postalCode, string city, string paymentMethod)
        {
            // Tutaj można wykonać logikę zapisu zamówienia do bazy danych, wysłania powiadomienia itp.

            // Wyczyszczenie koszyka
            RemoveCookies();

            // Przekazanie danych do widoku potwierdzenia zamówienia za pomocą ViewBag
            ViewBag.FirstName = firstName;
            ViewBag.LastName = lastName;
            ViewBag.Street = street;
            ViewBag.BuildingNumber = buildingNumber;
            ViewBag.ApartmentNumber = apartmentNumber;
            ViewBag.PostalCode = postalCode;
            ViewBag.City = city;
            ViewBag.PaymentMethod = paymentMethod;

            return View("ConfirmOrder");
        }


        private void SetCookie(string key, string value, int? numberOfdays = 7)
        {
            CookieOptions option = new CookieOptions();
            if (numberOfdays.HasValue)
                option.Expires = DateTime.Now.AddDays(numberOfdays.Value);

            // Dodanie przedrostka "cart_" do klucza
            string prefixedKey = $"cart_{key}";

            // Dodanie ciasteczka z użyciem zmodyfikowanego klucza
            Response.Cookies.Append(prefixedKey, value, option);
        }

        private Dictionary<int, int> ItemsFromCookies()
        {
            var cartItems = new Dictionary<int, int>();

            // Iteracja przez ciasteczka
            foreach (var cookie in Request.Cookies)
            {
                //Console.WriteLine($"{int.TryParse(cookie.Value, out int quantity1)}");
                if (cookie.Key.StartsWith("cart_") && int.TryParse(cookie.Key.Substring(5), out int articleId))
                {
                    if (int.TryParse(cookie.Value, out int quantity))
                    {
                        cartItems.Add(articleId, quantity);
                      //  Console.WriteLine($"{articleId}, {quantity}");
                    }
                }  
            }
            return cartItems;
        }

        private void RemoveCookies()
        {
            foreach (var cookie in Request.Cookies)
            {
                if (cookie.Key.StartsWith("cart_"))
                {
                    Response.Cookies.Delete(cookie.Key);
                }
            }
        }

    }
}

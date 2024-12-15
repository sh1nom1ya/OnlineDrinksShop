using System.Security.Claims;
using drunkShop.Data;
using drunkShop.Email;
using drunkShop.Models;
using drunkShop.Models.ViewModels;
using drunkShop.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;

namespace drunkShop.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IEmailService _emailService; 

        [BindProperty]
        public ProductUserVM ProductUserVM { get; set; }

        public CartController(AppDbContext db, IEmailService emailService) 
        {
            _db = db;
            _emailService = emailService;
        }

        public IActionResult Index()
        {
            List<ShoppingCart> shoppingCartList = GetShoppingCart();

            List<int> productInCart = shoppingCartList.Select(i => i.ProductId).ToList();
            IEnumerable<Product> productList = _db.Product.Where(u => productInCart.Contains(u.Id)).ToList();
            return View(productList);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Index")]
        public IActionResult IndexPost(List<ShoppingCart> shoppingCartItems)
        {
            HttpContext.Session.Set(WC.SessionCart, shoppingCartItems);
            return RedirectToAction(nameof(Summary));
        }

        public IActionResult Summary()
        {
            var claimIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);

            List<ShoppingCart> shoppingCartList = GetShoppingCart();

            if (!shoppingCartList.Any())
            {
                return RedirectToAction(nameof(Index));
            }

            List<int> productInCart = shoppingCartList.Select(i => i.ProductId).ToList();
            IEnumerable<Product> productList = _db.Product.Where(u => productInCart.Contains(u.Id)).ToList();

            ProductUserVM = new ProductUserVM()
            {
                ApplicationUser = _db.ApplicationUser.FirstOrDefault(u => u.Id == claim.Value),
                ProductList = productList,
                ShoppingCart = shoppingCartList
            };

            return View(ProductUserVM);
        }

        public IActionResult Remove(int id)
        {
            List<ShoppingCart> shoppingCartList = GetShoppingCart();

            shoppingCartList.Remove(shoppingCartList.FirstOrDefault(u => u.ProductId == id));
            HttpContext.Session.Set(WC.SessionCart, shoppingCartList);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProcessOrder()
        {
            // Получение информации о текущем пользователе
            var claimIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);

            // Получение списка товаров в корзине
            List<ShoppingCart> shoppingCartList = GetShoppingCart();

            if (shoppingCartList == null || shoppingCartList.Count == 0)
            {
                return RedirectToAction(nameof(Index));
            }

            // Создание нового заказа
            var newOrder = new AppOrder
            {
                OrderDate = DateTime.UtcNow,
                Code = GenerateOrderCode(10)
            };
            _db.AppOrder.Add(newOrder);
            await _db.SaveChangesAsync();

            // Привязка продуктов к заказу
            foreach (var cartItem in shoppingCartList)
            {
                var orderProduct = new AppOrderProduct
                {
                    AppOrderId = newOrder.Id,
                    ProductId = cartItem.ProductId,
                    Quantity = cartItem.Quantity
                };
                _db.AppOrderProduct.Add(orderProduct);
            }

            // Привязка пользователя к заказу
            var orderUser = new AppOrderUser
            {
                AppOrderId = newOrder.Id,
                UserId = claim.Value
            };
            _db.AppOrderUser.Add(orderUser);

            await _db.SaveChangesAsync();

            // Очищение корзины
            HttpContext.Session.Set(WC.SessionCart, new List<ShoppingCart>());

            // Отправка email пользователю
            //var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == claim.Value);
            //if (user != null)
            //{
            //    string subject = "Спасибо за ваш заказ!";
            //    string message = $@"
            //<p>Здравствуйте, {user.UserName}!</p>
            //<p>Спасибо за ваш заказ #{newOrder.Code}. Мы начнем обработку вашего заказа в ближайшее время.</p>
            //<p>Дата заказа: {newOrder.OrderDate:dd MMMM yyyy HH:mm}</p>";

            //    try
            //    {
            //        Console.WriteLine("Отправка письма на email: " + user.Email);

            //        await _emailService.SendEmailAsync(new EmailMessage
            //        {
            //            RecipientEmail = user.Email,
            //            Subject = subject,
            //            Body = message
            //        });

            //        Console.WriteLine("Письмо успешно отправлено.");
            //    }
            //    catch (Exception ex)
            //    {
            //        Console.WriteLine($"Ошибка отправки email: {ex.Message}");
            //        throw;  
            //    }
            //}

            return RedirectToAction(nameof(Index), new { id = newOrder.Id });
        }


        private List<ShoppingCart> GetShoppingCart()
        {
            return HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart)?.ToList() ?? new List<ShoppingCart>();
        }

        private string GenerateOrderCode(int length)
        {
            if (length <= 0 || length > 32)
            {
                throw new ArgumentOutOfRangeException(nameof(length), "Length must be between 1 and 32.");
            }

            string guid = Guid.NewGuid().ToString("N").ToUpper();

            return guid.Substring(0, length);
        }
    }
}

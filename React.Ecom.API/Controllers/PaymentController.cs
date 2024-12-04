using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using React.Ecom.API.Models;
using React.Ecom.Data;
using Stripe;

namespace React.Ecom.API.Controllers
{
    [Route("api/Payment")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly ApplicationDBContext _db;
        private readonly IConfiguration _configuration;
        private ApiResponse _response;

        public PaymentController(ApplicationDBContext db, IConfiguration configuration)
        {
            _db = db;
            _configuration = configuration;
            _response = new ApiResponse();
        }
        [HttpPost]
        public async Task<ActionResult<ApiResponse>> MakePayment(string userId)
        {
            ShoppingCart shoppingCart = await _db.ShoppingCarts.Include(x => x.CartItems)
                .ThenInclude(x => x.MenuItem)
                .FirstOrDefaultAsync(x => x.UserId == userId);
            if (shoppingCart == null || shoppingCart.CartItems == null || shoppingCart.CartItems.Count() == 0)
            {
                _response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                return BadRequest(_response);
            }

            #region Create Payment Intent
            StripeConfiguration.ApiKey = _configuration["StripeSettings:SecretKey"];
            shoppingCart.CartTotal = shoppingCart.CartItems.Sum(u => u.Quantity * u.MenuItem.Price);

            PaymentIntentCreateOptions options = new()
            {
                Amount = (int)(shoppingCart.CartTotal * 100),
                Currency = "inr",
                PaymentMethodTypes = new List<string>
                  {
                    "card",
                  },
            };
            PaymentIntentService service = new();
            PaymentIntent response = service.Create(options);
            shoppingCart.StripePaymentIntentId = response.Id;
            shoppingCart.ClientSecret = response.ClientSecret;
            #endregion
            _response.StatusCode = System.Net.HttpStatusCode.OK;
            _response.IsSuccess = true;
            _response.Result = shoppingCart;
            return Ok(_response);
        }
    }
}

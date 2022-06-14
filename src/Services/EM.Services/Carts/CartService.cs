namespace EM.Services.Carts
{
    using EM.Services.Carts.Helpers;

    using Microsoft.AspNetCore.Http;

    [TransientService]
    public class CartService : ICartService
    {
        private const string NullCartExceptionMessage = "Cart is null";

        private const string CartSessionKey = "Cart";

        private readonly ISession session;

        private IDictionary<Guid, CartItem>? cart;

        public CartService(IHttpContextAccessor contextAccessor)
        {
            this.session = contextAccessor.HttpContext.Session;
            this.cart = this.session.Get<IDictionary<Guid, CartItem>>(CartSessionKey);
        }

        public IDictionary<Guid, CartItem>? Cart => this.cart;

        public void AddToCart(CartItem item)
        {
            if (this.cart is null)
            {
                this.cart = new Dictionary<Guid, CartItem>() { { item.Id, item } };
            }
            else
            {
                this.cart.Add(item.Id, item);
            }

            this.SaveCart();
        }

        public void RemoveFromCart(Guid id)
        {
            if (this.cart is not null)
            {
                this.cart.Remove(id);

                this.SaveCart();
            }
        }

        public void ClearCart()
        {
            if (this.cart is not null)
            {
                this.session.Clear();
            }
        }

        public int GetCount()
        {
            if (this.cart is null)
            {
                return 0;
            }

            return this.cart.Count;
        }

        private void SaveCart()
        {
            if (this.cart is null)
            {
                throw new InvalidOperationException(NullCartExceptionMessage);
            }

            this.session.Set(CartSessionKey, this.cart);
        }
    }
}

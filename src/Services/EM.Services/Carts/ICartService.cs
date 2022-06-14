namespace EM.Services.Carts
{
    public interface ICartService
    {
        IDictionary<Guid, CartItem>? Cart { get; }

        void AddToCart(CartItem item);

        void RemoveFromCart(Guid id);

        void ClearCart();

        int GetCount();
    }
}

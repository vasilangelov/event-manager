namespace EM.Web.Models.ViewModels.Payment
{
    public class PaymentStatusViewModel
    {
        public bool Success { get; set; }

        public string? RetryUrl { get; set; } = default!;
    }
}

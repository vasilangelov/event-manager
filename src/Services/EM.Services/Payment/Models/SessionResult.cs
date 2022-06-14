namespace EM.Services.Payment.Models
{
    using Stripe.Checkout;

    public class SessionResult
    {
        public SessionResult(SessionStatus sessionStatus)
        {
            this.SessionStatus = sessionStatus;
        }

        public SessionResult(SessionStatus sessionStatus, Session? session)
            : this(sessionStatus)
        {
            this.Session = session;
        }

        public Session? Session { get; }

        public SessionStatus SessionStatus { get; }
    }
}

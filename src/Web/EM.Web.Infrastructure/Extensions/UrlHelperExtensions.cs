namespace EM.Web.Infrastructure.Extensions
{
    using System.Web;

    using Microsoft.AspNetCore.Mvc;

    public static class UrlHelperExtensions
    {
        public static string ActionRaw(this IUrlHelper urlHelper, string protocol, string controller, string action, object values)
            => HttpUtility.UrlDecode(urlHelper.Action(action, controller, values, protocol));
    }
}

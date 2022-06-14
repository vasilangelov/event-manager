namespace EM.Web.Infrastructure.Helpers
{
    using Microsoft.AspNetCore.Mvc;

    public static class NameHelpers
    {
        private const string Controller = "Controller";

        public static string GetControllerName<T>()
            where T : Controller
        {
            return typeof(T).Name.Replace(Controller, string.Empty);
        }
    }
}

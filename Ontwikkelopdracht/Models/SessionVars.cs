using System.Web;

namespace Ontwikkelopdracht.Models
{
    /// <summary>
    ///     Variables that can be used in <see cref="HttpSessionStateBase">HttpContext.Session</see>.
    /// </summary>
    public struct SessionVars
    {
        /// <summary>
        ///     The logged in user.
        /// </summary>
        public const string User = "user";

        /// <summary>
        ///     The pending order.
        /// </summary>
        public const string Order = "order";
    }
}
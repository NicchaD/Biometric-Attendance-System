using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
namespace YMCAObjects
{
    public sealed class YMCAEvents
    {
        public static event YMCAEventDelegates.PINVerificationEventHandler PINVerification = null;

        public static void OnPINVerification(HttpContext context, PINVerificationEventArgs e)
        {
            if (PINVerification != null)
            {
                PINVerification(context, e);
            }
        }
    }
}

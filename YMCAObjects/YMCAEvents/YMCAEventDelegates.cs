using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace YMCAObjects
{
    public class YMCAEventDelegates
    {
        public delegate void PINVerificationEventHandler(HttpContext context, PINVerificationEventArgs e);
    }
}

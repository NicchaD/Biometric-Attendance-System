//****************************************************
//Modification History
//****************************************************
//Modified by          Date          Description
//****************************************************
//Manthan Rajguru      2015.09.16    YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
//*****************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YMCAObjects;
using System.Web;
namespace YMCARET.YmcaBusinessObject
{
   public class YMCAHandler
    {
        public static void OnPINVerification(HttpContext context, PINVerificationEventArgs e)
        {
            YMCAActionEntry Action = null;
            try
            {
                Action = new YMCAActionEntry();
                Action.Action = "PIN Verification";
                Action.ActionBy = Convert.ToString(context.Session["LoginId"]);
                Action.Data = e.ToString();
                Action.EntityId = e.guiPersId;
                Action.EntityType = EntityTypes.PERSON;
                Action.Module = e.PINModule.ToString();
                Action.SuccessStatus = e.IsSucceeded;
                LoggerBO.WriteLog(Action);
            }
            catch (Exception)
            {
                
                throw;
            }
        }
    }
}

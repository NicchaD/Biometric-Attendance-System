using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YMCAObjects
{
  public  class EmailTemplate
    {
        private int intTemplateID;
        private string strName;
        private string strFromEmail;
        private string strToEmail;
        private string strCcEmail;
        private string strBccEmail;
        private string strSubject;
        private string strBody;
        private string strFooter;
        private bool blnActive;
        private string strStaticFileAttachment;
        private bool blnDynamicFileAttachment;
        private List<EmailAttachment> emailAttachment;

        public int TemplateID
        {
            get 
            {     
                return intTemplateID;
            }
            set
            {
                intTemplateID = value;
            }
    
        }

        public string Name
        {
            get
            {
                return strName;
            }
            set
            {
                strName = value;
            }
        }

        public string FromMail
        {
            get
            {
                return strFromEmail;
            }
            set
            {
                strFromEmail = value;
            }
        }

        public string ToEmail
        {
            get
            {
                return strToEmail;
            }
            set
            {
                strToEmail = value;
            }
        }

        public string CcEmail
        {
            get
            {
                return strCcEmail;
            }
            set
            {
                strCcEmail = value;
            }
        }

        public string BccEmail
        {
            get
            {
                return strBccEmail;
            }
            set
            {
                strBccEmail = value;
            }
        }

        public string Subject
        {
            get
            {
                return strSubject;
            }
            set
            {
                strSubject = value;
            }
        }

        public string Body
        {
            get
            {
                return strBody;
            }
            set
            {
                strBody = value;
            }
        }

        public string Footer
        {
            get
            {
                return strFooter;
            }
            set
            {
                strFooter = value;
            }
        }

        public bool IsActive
        {
            get
            {
                return blnActive;
            }
            set
            {
                blnActive = value;
            }
        }

        public string StaticFileAttachment
        {
            get
            {
                return strStaticFileAttachment;
            }
            set
            {
                strStaticFileAttachment = value;
            }
        }

        public bool DynamicFileAttachment
        {
            get
            {
                return blnDynamicFileAttachment;
            }
            set
            {
                blnDynamicFileAttachment = value;
            }
        }

        public List<EmailAttachment> ListEmailAttachment
        {
            get
            {
                return emailAttachment;
            }
            set
            { 
                emailAttachment = value; 
            }
        }
    }
}

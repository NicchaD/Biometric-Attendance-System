using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YMCAObjects
{
  public class EmailAttachment
    {
     
        public EmailAttachment(string strAttachmentFileNameWithExtension, string strAttachmentFileNameWithFullPath)
        {
            AttachmentFileNameWithExtension = strAttachmentFileNameWithExtension;
            AttachmentFileNameWithFullPath = strAttachmentFileNameWithFullPath;
        }

        public EmailAttachment(Byte[] bytAttachment, string strAttachmentFileNameWithExtension)
        {
            Attachment = bytAttachment;
            AttachmentFileNameWithExtension = strAttachmentFileNameWithExtension;
        }
        
      #region "Public Properties"

        public long EmailAttachementID
        {
            get;
            set;
        }

        public long EmailDetailID
        {
            get;
            set;
        }

        public Byte[] Attachment
        {
            get;
            set;
        }


        public string AttachmentFileNameWithExtension
        {
            get;
            set;
        }

        public string AttachmentFileNameWithFullPath
        {
            get;
            set;
        }
        #endregion
    }
}

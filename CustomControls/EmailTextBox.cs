// ***************************************
// Project ID: Yerdi
// Author: Bhavna S
// Created on: 08-10-2012
// Summary of Functionality: 
// ***************************************
//
// REVISION HISTORY:
// ------------------------------------------------------------------------------------------------------
// Developer Name | Date       | Version No | Project/IssueNo | Change
// ------------------------------------------------------------------------------------------------------

//                |            |            |                 |
// ------------------------------------------------------------------------------------------------------
// ***************************************
using System;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CustomControls
{
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:EmailTextBox runat=server></{0}:EmailTextBox>")]
    public class EmailTextBox : TextBox  
    {

        public EmailTextBox()
        {
            Message = "Please Enter Proper Email Address.";
        }


        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]
        private string message;
        public string Message
        {
            get
            {
                return message;
            }
            set
            {
                message = value;
            }
        }
		public int maxLength;

		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
		public override int MaxLength
		{
			get
			{
				return maxLength;
			}
			set
			{
				maxLength = value;
			}
		}
      
     
        protected override void OnPreRender(EventArgs e)
        {
            Page.ClientScript.RegisterClientScriptResource(this.GetType(), "CustomControls.Resources.Common2All.js");
            Page.ClientScript.RegisterClientScriptResource(this.GetType(), "CustomControls.Resources.ControlCommon.js");

            this.Attributes.Add("onkeypress", "return checkemailonEnterkey(this,event,'" + this.Message + "')");
            this.Attributes.Add("onblur", "return CheckEmailId(this,'" + this.Message + "')");
            this.MaxLength = 50;
            if (Message.Length > 0)
            {
                this.Attributes.Add("Msg", Message);
            }
            base.OnPreRender(e);
        }
       
    }
}

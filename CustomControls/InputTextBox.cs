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
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CustomControls
{
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:InputTextBox runat=server></{0}:InputTextBox>")]
    public class InputTextBox : TextBox
    {

        public InputTextBox()
        {
			Message = "Please ensure entered value is string.";
			callingJSFunction = "";
			AllowChars = "";
        }

		private string allowChars;

		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
		public string AllowChars
		{
			get
			{
				return allowChars;
			}
			set
			{
				allowChars = value;

			}
		}
		private string message;

		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
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

		private TextBoxMode textMode;
		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
		public override TextBoxMode TextMode
		{
			get
			{
				return textMode;
			}
			set
			{
				textMode = value;
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

		private string callingJSFunction;

		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
		public string CallingJSFunction
		{
			get
			{
				return callingJSFunction;
			}
			set
			{
				callingJSFunction = value;
			}
		}
		
		protected override void OnPreRender(EventArgs e)
		{
			Page.ClientScript.RegisterClientScriptResource(this.GetType(), "CustomControls.Resources.Common2All.js");
			Page.ClientScript.RegisterClientScriptResource(this.GetType(), "CustomControls.Resources.ControlCommon.js");
			base.MaxLength = maxLength;
			base.TextMode = textMode;

			if (this.AllowChars.Trim().Length > 0)
			{
				base.Attributes.Add("onkeypress", "return goodchars(event,'" + this.AllowChars + "',this," + this.MaxLength + "," + (base.TextMode == TextBoxMode.MultiLine).ToString().ToLower() + ")");
				if (this.callingJSFunction.Length > 0)
				{
					base.Attributes.Add("onblur", "javascript: if(validatepassword(this,'" + this.AllowChars + "'," + this.MaxLength + "))" + this.CallingJSFunction);
				}
				else
				{
					base.Attributes.Add("onblur", "return validatepassword(this, '" + this.AllowChars + "'," + this.MaxLength + ")");

				}
			}
			else
			{
				this.Attributes.Add("onkeypress", "return checkInput(event, this, " + this.MaxLength + "," + (base.TextMode == TextBoxMode.MultiLine).ToString().ToLower() + ");");
				if (this.callingJSFunction.Length > 0)
				{
					base.Attributes.Add("onblur", "javascript:if(validateInput(this,'" + this.MaxLength + "'))" + this.CallingJSFunction);
				}
				else
				{
					base.Attributes.Add("onblur", "return validateInput(this, " + this.MaxLength + ");");

				}
			}
			base.OnPreRender(e);
		}
		
	

       
    }
}

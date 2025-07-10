// ***************************************
// Copyright YMCA Retirement Fund All Rights Reserved. 
//
// Project ID: Yerdi
// Author: bc rout
// Created on: 04-09-2012
// Summary of Functionality: 
// ***************************************
//
// REVISION HISTORY:
// ------------------------------------------------------------------------------------------------------
// Developer Name | Date       | Version No | Project/IssueNo | Change
// ------------------------------------------------------------------------------------------------------

// Deven          | 04/14/2014 |   8.1.0    | YERDI3I-2357    | AtsTelehone issue with hyphens and brackets.
// Harshalal      | 05/19/2014 |   8.1.0    | BT-2539         | Number control return "X" if value is ".X"
// Megha Lad      | 09/16/2019 |   7.0      | YRS-AT-4598     | YRS enh: State Withholding Project - Annuity Payroll Processing
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
    [ToolboxData("<{0}:NumberTextBox runat=server></{0}:NumberTextBox>")]
    public class NumberTextBox : TextBox
    {
        public NumberTextBox()
        {
            //Message = "Please ensure entered value is numeric &  positive only.";
			Message = "";
            callingJSFunction = "";
            defaultText = "";
        }

        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]
        public override string Text
        {
            get
            {
                return base.Text.Replace(",", "");
            }
            set
            {
                if (isCommaSeperated)
                    base.Text = GetCommaSeparatedValue(value);
                else if (isPhoneNumber) // DS:04/14/2014 - Added else if(isPhoneNumber) condition for YERDI3I-2357
                {
                    string stPhoneNumber = value;
                    if (stPhoneNumber != null)
                    {
                        stPhoneNumber = SanitizePhonenumber(stPhoneNumber);
                        if (stPhoneNumber is string && !string.IsNullOrEmpty(stPhoneNumber))
                        {
                            if (stPhoneNumber.Length == 10)
                            {
                                double dTelephoneNo;
                                if(double.TryParse(stPhoneNumber, out dTelephoneNo))
                                {
                                    stPhoneNumber = string.Format("{0:###-###-####}", dTelephoneNo);
                                }
                            }
                        }
                    }

                    base.Text = stPhoneNumber;
                }
                ///START : ML |YRS-AT-4598 | 2019.10.09 | Convert Decimal Number to Whole Number upto last 2 digits.
                else if (iswholeNumber)
                {
                    double dwholeNumber;
                    string swholeNumber = value;
                    if (swholeNumber != null)
                    {
                        if (double.TryParse(swholeNumber,out dwholeNumber))
                        {
                         swholeNumber = Math.Round(dwholeNumber, MidpointRounding.AwayFromZero).ToString("N2");
                        }                    
                    }                    
                    base.Text = swholeNumber;
                }///END : ML |YRS-AT-4598 | 2019.10.09 | Convert Decimal Number to Whole Number upto last 2 digits.
                else
                    base.Text = value;
            }
        }

        public string defaultText;

        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]
        public string DefaultText
        {
            get
            {
                return defaultText;
            }
            set
            {
                defaultText = value;
            }
        }


        public bool isCommaSeperated;

        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]
        public bool IsCommaSeperated
        {
            get
            {
                return isCommaSeperated;
            }
            set
            {
                isCommaSeperated = value;
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

        private bool allowDecimal = true;

        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]
        public bool AllowDecimal
        {
            get
            {
                return allowDecimal;
            }
            set
            {
                allowDecimal = value;
            }
        }

        private bool allowNegative;

        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]
        public bool AllowNegative
        {
            get
            {
                return allowNegative;
            }
            set
            {
                allowNegative = value;
            }
        }

        private bool isPhoneNumber = false;
        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]
        public bool IsPhoneNumber
        {
            get
            {
                return isPhoneNumber;
            }
            set
            {
                isPhoneNumber = value;
            }
        }

        /// START : ML |YRS-AT-4598 | 2019.10.09 | Property Declaration -IswholeNumer. 
        /// <summary>
        ///  If user pass True then entered value in textbox will be convert to whole number.
        /// </summary>
        private bool iswholeNumber = false;
        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]
        public bool IsWholeNumber
        {
            get
            {
                return iswholeNumber;
            }
            set
            {
                iswholeNumber = value;
            }
        }
   
        /// END : ML |YRS-AT-4598 | 2019.10.09 | Property Declaration -IswholeNumer. If user pass True then entered value in textbox will be convert to whole number .
        
        protected override void OnPreRender(EventArgs e)
        {
            Page.ClientScript.RegisterClientScriptResource(this.GetType(), "CustomControls.Resources.Common2All.js");
            Page.ClientScript.RegisterClientScriptResource(this.GetType(), "CustomControls.Resources.ControlCommon.js");
            base.Attributes.Add("onkeypress", "return CheckNumber(this,event,'" + this.Message + "'," + allowNegative.ToString().ToLower() + ", " + allowDecimal.ToString().ToLower() + ",'" + this.IsCommaSeperated.ToString().ToLower() + "','" + defaultText + "', " + isPhoneNumber.ToString().ToLower() + ");");
            if (allowDecimal)
            {
                base.Attributes.Add("onkeyup", "CheckDecimal(this);");
            }
            if (this.callingJSFunction.Length > 0)
            {
                base.Attributes.Add("onblur", "javascript: if(CheckNumberblur(this,'" + this.Message + "'," + allowNegative.ToString().ToLower() + ", " + allowDecimal.ToString().ToLower() + ",'" + this.IsCommaSeperated.ToString().ToLower() + "','" + defaultText + "', " + isPhoneNumber.ToString().ToLower() + "))" + this.CallingJSFunction);
            }
            else
            {
                base.Attributes.Add("onblur", "CheckNumberblur(this,'" + this.Message + "'," + allowNegative.ToString().ToLower() + ", " + allowDecimal.ToString().ToLower() + ",'" + this.IsCommaSeperated.ToString().ToLower() + "','" + defaultText + "', " + isPhoneNumber.ToString().ToLower() + ");");

            }

            if (Message.Length > 0)
            {
                this.Attributes.Add("Msg", Message);
            }
            base.OnPreRender(e);
        }

        public string GetCommaSeparatedValue(string Input)
        {
            if (Input.Trim().Length > 0)
            {
                String val = Input;                
                bool isNegative;
                isNegative = Convert.ToBoolean(Input.Substring(0, 1) == "-"); //(Convert.ToDouble(val) < 0);        //Harshala : 05/19/2014 : BT-2539 : Number control return "X" if value is ".X"

                if (isNegative)
                    val = val.Substring(1);

                String data = val.Replace(",", "");  //String(val).replace(/[\$,]/g,'');
                String[] temp = data.Split(".".ToCharArray());
                //START : Harshala : 05/19/2014 : BT-2539 : Number control return "X" if value is ".X"
                if (temp[0].Trim() == string.Empty)
                {
                    return Input;
                }
                //END : Harshala : 05/19/2014 : BT-2539 : Number control return "X" if value is ".X"
                else
                {
                    string output = "";
                    string re = @"(\d{1,3})$";


                    // Instantiate the regular expression object.
                    Regex r = new Regex(re, RegexOptions.IgnoreCase);

                    // Match the regular expression pattern against a text string.
                    Match m = r.Match(temp[0].ToString());
                    int matchCount = 0;
                    while (m.Success)
                    {
                        ++matchCount;
                        re = @"(\d{1,3})$";

                        output = "," + m.Value + output;
                        temp[0] = r.Replace(temp[0], "");
                        r = new Regex(re, RegexOptions.IgnoreCase);
                        m = r.Match(temp[0].ToString());

                    }

                    if (temp.Length > 1)
                        output += "." + temp[1].ToString();

                    string dataobj = "";
                    if (isNegative)
                        dataobj = "-" + output.Substring(1);
                    else
                        dataobj = output.Substring(1);


                    return dataobj;
                }
            }
            else
            {
                return string.Empty;
            }
        }

        //DS:04/14/2014 - Added function SanitizePhonenumber for YERDI3I-2357
        private string SanitizePhonenumber(string stPhoneNo)
        {
            if (stPhoneNo != null)
            {
                return stPhoneNo.Replace("-", "").Replace("(", "").Replace(")", "").Replace(" ", "");
            }
            else
            {
                return stPhoneNo;
            }
        }
    }
}

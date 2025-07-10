/******************************************************************************************************************************
 * Author           : Deven Sawant
 * Creation Date    : 04/15/2014
 * Purpose          : A custom label to display numbers, introduced for YERDI3I-2357 : AtsTelehone issue with hyphens and brackets.
 * 
 * Modification Hsitory
 * ----------------------------------------------------------------------------------------------------------------------------
 * Modified By          |   Modification Date   |   Purpose
 * ----------------------------------------------------------------------------------------------------------------------------
 * 
 ******************************************************************************************************************************/
using System;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CustomControls
{
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:NumberLabel runat=server></{0}:NumberLabel>")]
    public class NumberLabel : Label
    {
        public NumberLabel()
        {
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
                else if (isPhoneNumber)
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

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
        }

        public string GetCommaSeparatedValue(string Input)
        {
            if (Input.Trim().Length > 0)
            {
                String val = Input;
                bool isNegative;
                isNegative = (Convert.ToDouble(val) < 0);

                if (isNegative)
                    val = val.Substring(1);

                String data = val.Replace(",", "");
                String[] temp = data.Split(".".ToCharArray());
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
            else
            {
                return string.Empty;
            }
        }

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

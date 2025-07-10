/******************************************************************************************************************************
 * Modification Hsitory
 * ----------------------------------------------------------------------------------------------------------------------------
 * Modified By           |   Modification Date   |   Purpose
 * ----------------------------------------------------------------------------------------------------------------------------
 * Deven                 |   05/07/2013          |   BT:2034 Added flag to show hide calender image
 * Rakesh V              |   05/27/2015          |   BT-2886: Remove compiler/build warnings
 * Pramod Prakash Pokale |   09/22/2015          |   Added CustomValidator, RequiredFieldValidator, Message and also provided inbuilt JS validation function. These validators are optional, 
 *                                                   user has to enable it by setting their respective property true (EnableCustomValidator and EnableRequiredFieldValidator)
 * Pramod Prakash Pokale |   10/05/2015          |   YRS-AT-2596: Added trackDateChange which helps to enable OnChange event of the control if date text is changed
 * Pramod Prakash Pokale |   11/25/2015          |   Removing constructor, assigning default messages as well as making it configurable for RequiredFieldValidator
 ******************************************************************************************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CustomControls
{
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:CalenderTextBox runat=server></{0}:CalenderTextBox>")]
    public class CalenderTextBox : TextBox
    {
        CustomValidator customValidator;
        RequiredFieldValidator requiredFieldValidator;

        public CalenderTextBox()
        {
            message = "Invalid Date";
        }

        private string message;
        /// <summary>
        /// Displays message in a alert box if javascript is enabled
        /// </summary>
        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]
        public string Message
        {
            get
            {
                EnsureChildControls();
                return message;
            }
            set
            {
                EnsureChildControls();
                message = value;
            }
        }

        private bool showDateFormat = false;
        /// <summary>
        /// Appears on UI besides control for e.g. control (mm/dd/yyyy)
        /// </summary>
        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("false")]
        [Localizable(true)]
        public bool ShowDateFormat
        {
            get
            {
                EnsureChildControls();
                return showDateFormat;
            }
            set
            {
                EnsureChildControls();
                showDateFormat = value;
            }
        }

        private bool isFocusRequired = true;
        /// <summary>
        /// Keeps focus on date text box after selection of date from datepicker
        /// </summary>
        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("true")]
        [Localizable(true)]
        public bool IsFocusRequired
        {
            get
            {
                EnsureChildControls();
                return isFocusRequired;
            }
            set
            {
                EnsureChildControls();
                isFocusRequired = value;
            }
        }

        private string yearRange = string.Empty;
        /// <summary>
        /// Defines Year range which can be displayed on year dropdown, format is c-50:c+50 where "c" is current year
        /// </summary>
        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("0")]
        [Localizable(true)]
        public string YearRange
        {
            get
            {
                EnsureChildControls();
                return yearRange;
            }
            set
            {
                EnsureChildControls();
                yearRange = value;
            }
        }

        private DateTime minDate = new DateTime(1900, 01, 01);
        /// <summary>
        /// Calendar will start from defined date, default is 01/01/1900
        /// </summary>
        [Bindable(true)]
        [Category("Appearance")]
        [Localizable(true)]
        public DateTime MinDate
        {
            get
            {
                EnsureChildControls();
                return minDate;
            }
            set
            {
                EnsureChildControls();
                minDate = value;
            }
        }

        private DateTime maxDate = new DateTime((DateTime.Today.Year + 10), 12, 31);
        /// <summary>
        /// Calendar will show upto defined max date, default is Current date + 50 years
        /// </summary>
        [Bindable(true)]
        [Category("Appearance")]
        [Localizable(true)]
        public DateTime MaxDate
        {
            get
            {
                EnsureChildControls();
                return maxDate;
            }
            set
            {
                EnsureChildControls();
                maxDate = value;
            }
        }

        private bool bShowCalenderImage = true;
        /// <summary>
        /// Sets a flag indicating whether to show calender image or not
        /// </summary>
        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("true")]
        [Localizable(true)]
        public bool ShowCalenderImage
        {
            get
            {
                EnsureChildControls();
                return bShowCalenderImage;
            }
            set
            {
                EnsureChildControls();
                bShowCalenderImage = value;
            }
        }

        private bool enableInBuiltJSValidation = false;
        /// <summary>
        /// Provides javascipt validation for selected/provided date. On every invalid format page will have an javascript alert box with defined Message 
        /// </summary>
        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("false")]
        [Localizable(true)]
        public bool EnableInBuiltJSValidation
        {
            get
            {
                EnsureChildControls();
                return enableInBuiltJSValidation;
            }
            set
            {
                EnsureChildControls();
                enableInBuiltJSValidation = value;
            }
        }

        private bool enableCustomValidator = false;
        /// <summary>
        /// Provides a CustomValidator with defined Message
        /// </summary>
        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("false")]
        [Localizable(true)]
        public bool EnableCustomValidator
        {
            get
            {
                EnsureChildControls();
                return enableCustomValidator;
            }
            set
            {
                EnsureChildControls();
                enableCustomValidator = value;
            }
        }

        private bool enableRequiredFieldValidator = false;
        /// <summary>
        /// Provides RequiredFieldValidator
        /// </summary>
        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("false")]
        [Localizable(true)]
        public bool EnableRequiredFieldValidator
        {
            get
            {
                EnsureChildControls();
                return enableRequiredFieldValidator;
            }
            set
            {
                EnsureChildControls();
                enableRequiredFieldValidator = value;
            }
        }

        private string customValidatorMessage;
        /// <summary>
        /// Defines CustomValidator Message
        /// </summary>
        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]
        public string CustomValidatorMessage
        {
            get
            {
                EnsureChildControls();
                return customValidatorMessage;
            }
            set
            {
                EnsureChildControls();
                customValidatorMessage = value;
            }
        }

        private string requiredFieldValidatorMessage;
        /// <summary>
        /// Defines RequiredFieldValidator Message
        /// </summary>
        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]
        public string RequiredFieldValidatorMessage
        {
            get
            {
                EnsureChildControls();
                return requiredFieldValidatorMessage;
            }
            set
            {
                EnsureChildControls();
                requiredFieldValidatorMessage = value;
            }
        }

        private string callingJSFunction = string.Empty;
        /// <summary>
        /// Binds defined javascript function to onBlur event of the control.
        /// </summary>
        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]
        public string CallingJSFunction
        {
            get
            {
                EnsureChildControls();
                return callingJSFunction;
            }
            set
            {
                EnsureChildControls();
                callingJSFunction = value;
            }
        }

        //START: PPP | 10/05/2015 | YRS-AT-2596
        private bool trackDateChange = false;
        /// <summary>
        /// Identify date text change, if date is not changed using calender then default "OnChange" event will not fire.
        /// </summary>
        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("false")]
        [Localizable(true)]
        public bool TrackDateChange
        {
            get
            {
                EnsureChildControls();
                return trackDateChange;
            }
            set
            {
                EnsureChildControls();
                trackDateChange = value;
            }
        }
        //END: PPP | 10/05/2015 | YRS-AT-2596

        protected override void OnPreRender(EventArgs e)
        {
            StringBuilder javaScriptText;
            try
            {
                javaScriptText = new StringBuilder();
                javaScriptText.Append("$(function () {$('#");
                javaScriptText.Append(this.ClientID);
                javaScriptText.Append("').datepicker({changeMonth: true, changeYear: true");
                if (string.IsNullOrEmpty(yearRange))
                {
                    javaScriptText.Append(", yearRange: 'c-50:c+50'");
                }
                else
                {
                    javaScriptText.Append(", yearRange: '");
                    javaScriptText.Append(yearRange);
                    javaScriptText.Append("'");
                }

                javaScriptText.Append(", minDate: new Date(");
                javaScriptText.Append(minDate.Year.ToString());
                javaScriptText.Append(", ");
                javaScriptText.Append((minDate.Month - 1).ToString());
                javaScriptText.Append(", ");
                javaScriptText.Append(minDate.Day.ToString());
                javaScriptText.Append(")");

                javaScriptText.Append(", maxDate: new Date(");
                javaScriptText.Append(maxDate.Year.ToString());
                javaScriptText.Append(", ");
                javaScriptText.Append((maxDate.Month - 1).ToString());
                javaScriptText.Append(", ");
                javaScriptText.Append(maxDate.Day.ToString());
                javaScriptText.Append(")");

                if (this.ShowCalenderImage) // Added by DS on 05/07/2013 for YERDI3I-2034
                    javaScriptText.Append(", showOn: 'button', buttonText: 'Select Date', buttonImageOnly: true, buttonImage: 'images/Calendar.gif'");

                if (!this.Enabled)
                    javaScriptText.Append(", disabled : true");

                if (this.showDateFormat)
                    javaScriptText.Append(", appendText : ' (mm/dd/yyyy)'");

                //START: PPP | 10/05/2015 | YRS-AT-2596 | trackDateChange property will help to trigger client side "OnChange" event if the selected date is different than the earlier date
                // isFocusRequired flag will disable the client side "OnChange" event in any case.
                //if (this.isFocusRequired)
                if (this.isFocusRequired && !this.trackDateChange)
                {
                    javaScriptText.Append(", onSelect: function (date, inst) {this.focus();return false;}})});");
                }
                else if (!this.isFocusRequired && this.trackDateChange)
                {
                    javaScriptText.Append(", onSelect: function (date, inst) {if (date !== inst.lastVal) { this.focus(); $(this).change(); } else { this.focus(); return false;} }})});");
                }
                //END: PPP | 10/05/2015 | YRS-AT-2596
                else
                {
                    javaScriptText.Append("})});");
                }

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), this.ID, javaScriptText.ToString(), true);
                ScriptManager.RegisterClientScriptResource(this, this.GetType(), "CustomControls.Resources.CalenderTextBox.js");

                if (this.enableInBuiltJSValidation)
                {
                    if (string.IsNullOrEmpty(this.callingJSFunction))
                    {
                        this.Attributes.Add("onblur", string.Format("return CheckDateValue(this,'{0}');", this.message));
                    }
                    else
                    {
                        this.Attributes.Add("onblur", string.Format("javascript:if(CheckDateValue(this,'{0}')) {1}", this.message, this.callingJSFunction));
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(this.callingJSFunction))
                    {
                        this.Attributes.Add("onblur", string.Format("return {0};", this.callingJSFunction));
                    }
                }
                base.OnPreRender(e);
            }
            catch  // Rakesh V | 05/27/2015 | BT-2886: Remove compiler/build warnings
            {
                throw;
            }
            finally
            {
                javaScriptText = null;
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.enableCustomValidator)
            {
                customValidator = new CustomValidator
                {
                    Enabled = true,
                    EnableClientScript = true,
                    Display = ValidatorDisplay.Dynamic,
                    ControlToValidate = ID,
                    ClientValidationFunction = "IsValidControlDate",
                    // START: PPP | 11/25/2015 | Created customValidatorMessage which is configurable
                    //ErrorMessage = this.message,
                    ErrorMessage = string.IsNullOrEmpty(this.customValidatorMessage) ? "Invalid Date" : this.customValidatorMessage,
                    // END: PPP | 11/25/2015 | Created customValidatorMessage which is configurable
                    CssClass = "Error_Message",
                    ID = string.Format("{0}_CTBCustomValidator", ID)

                };

                Controls.Add(customValidator);
            }

            if (this.enableRequiredFieldValidator)
            {
                requiredFieldValidator = new RequiredFieldValidator
                {
                    Enabled = true,
                    EnableClientScript = true,
                    Display = ValidatorDisplay.Static,
                    ControlToValidate = ID,
                    // START: PPP | 11/25/2015 | Created requiredFieldValidatorMessage which is configurable
                    //ErrorMessage = "Date cannot be blank",
                    ErrorMessage = string.IsNullOrEmpty(this.requiredFieldValidatorMessage) ? "Date cannot be blank" : this.requiredFieldValidatorMessage,
                    // END: PPP | 11/25/2015 | Created requiredFieldValidatorMessage which is configurable
                    CssClass = "Error_Message",
                    Text = "*",
                    ID = string.Format("{0}_CTBRequiredFieldValidator", ID)
                };
                Controls.Add(requiredFieldValidator);
            }
            this.Page.RegisterRequiresControlState(this);
            base.OnInit(e);
        }

        protected override void Render(HtmlTextWriter writer)
        {
            base.Render(writer);
            RenderChildren(writer);
        }

        protected override void RenderChildren(HtmlTextWriter writer)
        {
            if (HasControls())
            {
                if (this.enableCustomValidator && customValidator != null)
                    customValidator.RenderControl(writer);

                if (this.enableRequiredFieldValidator && requiredFieldValidator != null)
                    requiredFieldValidator.RenderControl(writer);
            }
        }
    }
}



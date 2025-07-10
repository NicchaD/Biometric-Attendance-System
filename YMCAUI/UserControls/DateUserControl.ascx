<%@ Register TagPrefix="rjs" Namespace="RJS.Web.WebControl" Assembly="RJS.Web.WebControl.PopCalendar" %>
<%@ Control Language="vb" AutoEventWireup="false" Codebehind="DateUserControl.ascx.vb" Inherits="YMCAUI.DateUserControl" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<script language="javascript">
function IsValidDate(sender, args)
		{
			fmt = "MM/DD/YYYY";
			if (fnvalidateGendate_tmp(args,fmt))
			{
				args.IsValid = true;
			}
			else
			{
				args.IsValid = false;
			}
		}
	function fnvalidateGendate_tmp(value1,fmt)
		{
			switch (fmt)
			{
				case("MM/DD/YYYY"):
					//alert("Inside MMDDYYY");
					for(q=0;q<fnvalidateGendate_tmp.arguments.length-1;q++)
					{
						indatefieldtext= fnvalidateGendate_tmp.arguments[q];
						indatefield=value1.Value;
						if (indatefield.indexOf("-")!=-1)
						{
							var sdate = indatefield.split("-");
						}
						else
						{
							var sdate = indatefield.split("/");
						}
						var cmpDate;
						var chkDate=new Date(Date.parse(indatefield))

						var cmpDate1=(chkDate.getMonth()+1)+"/"+(chkDate.getDate())+"/"+(chkDate.getFullYear());
						var cmpDate2=(chkDate.getMonth()+1)+"/"+(chkDate.getDate())+"/"+(chkDate.getYear());

						var indate2=(Math.abs(sdate[0]))+"/"+(Math.abs(sdate[1]))+"/"+(Math.abs(sdate[2]));

						var num=sdate[2];
						var num1=num+"8";

						var num2=num1.length ;
						if(num2==3)
						{
							cmpDate=cmpDate2;
						}
						if(num2==5)
						{
							cmpDate=cmpDate1;
						}
						if(indate2!=cmpDate)
						{
							//alert("before invalid");
							//alert("Invalid date or date format on field "+value1.id);
							//indatefieldtext.focus();
							return false;
						}
						else
						{
							if (cmpDate=="NaN/NaN/NaN")
							{
								//alert("before invalid1");
								//alert("Invalid date or date format on field "+value1.id);
								//indatefieldtext.focus();
								return false;
							}
						}
					}
					return true;
					break;


				case("DD/MM/YYYY")  :
					//alert("Inside DDMMYYYY");
					for(q=0;q<fnvalidateGendate_tmp.arguments.length-1;q++)
					{
						indatefieldtext= fnvalidateGendate_tmp.arguments[q];
						indatefield=value1.Value;
						if (indatefield.indexOf("-")!=-1)
						{
							var sdate = indatefield.split("-");
						}
						else
						{
							var sdate = indatefield.split("/");
						}

						var cmpDate;
						indatefield = (Math.abs(sdate[1]))+"/"+(Math.abs(sdate[0]))+"/"+(Math.abs(sdate[2]));
						var chkDate=new Date(Date.parse(indatefield))

						var cmpDate1=(chkDate.getDate())+"/"+(chkDate.getMonth()+1)+"/"+(chkDate.getFullYear());
						var cmpDate2=(chkDate.getDate())+"/"+(chkDate.getMonth()+1)+"/"+(chkDate.getYear());
						var indate2=(Math.abs(sdate[0]))+"/"+(Math.abs(sdate[1]))+"/"+(Math.abs(sdate[2]));


						//alert(indate2)
						//alert(cmpDate2)
						var num=sdate[2];
						var num1=num+"8";

						var num2=num1.length;
						if(num2==3)
						{
							cmpDate=cmpDate2;
						}
						if(num2==5)
						{
							cmpDate=cmpDate1;
						}

						if(indate2!=cmpDate)
						{

							//alert("Invalid date or date format on field " + value1.id);
							//indatefieldtext.focus();
							return false;
						}
						else
						{
							if (cmpDate=="NaN/NaN/NaN")
							{

								//alert("Invalid date or date format on field "+value1.id);
								//indatefieldtext.focus();
								return false;
							}
						}
					}
					return true;
					break;

			}
		}
</script>
<%-- DateControl Class added by prasad :2011-08-26:For BT-895,YRS 5.0-1364 : prompt user to if changes not saved--%>
<asp:panel id="PanelDate" Height="20px" Width="120px" runat="server">
	<asp:TextBox onpaste="return false" id="TextBoxUCDate" runat="server" Width="70px" CssClass="TextBox_Normal DateControl"
		MaxLength="10"></asp:TextBox>
	<rjs:popcalendar id="PopcalendarDate" runat="server" TextMessage="*" MessageAlignment="RightCalendarControl"
		Separator="/" Control="TextBoxUCDate" Format="mm dd yyyy" ScriptsValidators="IsValidDate"></rjs:popcalendar>
	<asp:customvalidator id="valCustomDOB" runat="server" Enabled="False" Display="Dynamic" ControlToValidate="TextBoxUCDate"
		ClientValidationFunction="IsValidDate" ErrorMessage="Invalid Date" CssClass="Error_Message">*</asp:customvalidator>
	<asp:RequiredFieldValidator id="RequiredFieldValidator1" runat="server" Enabled="False" Display="Dynamic" ControlToValidate="TextBoxUCDate"
		ErrorMessage="Date cannot be blank" DESIGNTIMEDRAGDROP="5" CssClass="Error_Message">*</asp:RequiredFieldValidator>
		<asp:RangeValidator id="RangeValidatorUCDate" runat="server" Enabled="False" Display="Dynamic" ControlToValidate="TextBoxUCDate" Type="Date" MinimumValue="01/01/1900" MaximumValue ="01/01/2050"  CssClass="Error_Message"></asp:RangeValidator>
</asp:panel>

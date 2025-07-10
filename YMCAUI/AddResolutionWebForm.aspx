<%@ Register TagPrefix="rjs" Namespace="RJS.Web.WebControl" Assembly="RJS.Web.WebControl.PopCalendar" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="AddResolutionWebForm.aspx.vb" Inherits="YMCAUI.AddResolutionWebForm" %>
<!--#include virtual="TopNew.htm"-->
<script language="javascript">
//rahul 07 mar,06
function Validate(obj)
{
	str = String(obj.value);
	var mytool_array=str.split("/");
	if(mytool_array[1]>1)
	{
		mytool_array[1]=01;
		str=mytool_array[0] + "/" + mytool_array[1] + "/" +mytool_array[2];
		obj.value=str;
	}	
}
/*
function Validate1()
{
	str = String(document.Form1.all.TextboxEffDate.value);
	var mytool_array=str.split("/");
	if(mytool_array[1]>1)
	{
		mytool_array[1]=01;
		str=mytool_array[0] + "/" + mytool_array[1] + "/" +mytool_array[2];
		document.getElementById("TextboxEffDate").value=str;
	}	
}

function Validate2()
{
	str = String(document.Form1.all.TextboxTermDate.value);
	var mytool_array=str.split("/");
	if(mytool_array[1]>1)
	{
		mytool_array[1]=01;
		str=mytool_array[0] + "/" + mytool_array[1] + "/" +mytool_array[2];
		document.getElementById("TextboxTermDate").value=str;
	}
}
*/

//rahul 07 mar,06
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
function ValidateNumeric()
{
	if ((event.keyCode < 48)||(event.keyCode > 57))
	{
		event.returnValue = false;
	}
}
</script>
<form id="Form1" method="post" runat="server">
	<div class="Div_Center">
		<table width="700">
			<tr>
				<td class="Td_HeadingFormContainer" vAlign="top" align="left"><IMG title="image" height="10" alt="image" src="images/spacer.gif" width="10">
					YMCA Information<asp:label id="LabelHdr" runat="server" CssClass="Td_HeadingFormContainer"></asp:label>
				</td>
			</tr>
			<tr>
				<td>&nbsp;</td>
			</tr>
		</table>
	</div>
	<div class="Div_Center" align="center" >
		<table class="Table_WithBorder" width="700" align="center">
			<tr>
				<td align="center">
					<table width="100%">
						<tr>
							<td width="279"></td>
							<td>&nbsp;</td>
						</tr>
						<tr>
							<td align="left" width="279" height="28">
								<asp:label id="lblEffDate" runat="server" CssClass="Label_Small">Effective Date</asp:label>
							</td>
							<td align="left" height="29">
								<asp:textbox id="TextboxEffDate" runat="server" CssClass="TextBox_Normal" 
									Width="160px"></asp:textbox> <%-- MMR | 2018.04.30 | YRS-AT-3849 | Removed function 'Validate1' called on blur event as not required --%>
								<rjs:popcalendar id="PopCalendar1" runat="server" Separator="/" Format="mm dd yyyy" ScriptsValidators="IsValidDate"></rjs:popcalendar>
								<asp:customvalidator id="valCustomEffDate" runat="server" Display="Dynamic" ControlToValidate="TextboxEffDate"
									ClientValidationFunction="IsValidDate" ErrorMessage="Enter valid effective date">*</asp:customvalidator>
								<asp:requiredfieldvalidator id="RequiredFieldValidator1" runat="server" ControlToValidate="TextboxEffDate" ErrorMessage="Effective date cannot be blank">*</asp:requiredfieldvalidator>
							</td>
						</tr>
						<tr>
							<td align="left" width="279" height="29"><asp:label id="LabelTermDate" runat="server" CssClass="Label_Small">Term Date</asp:label></td>
							<td align="left" height="29"><asp:textbox id="TextboxTermDate" runat="server" CssClass="TextBox_Normal" 
									Width="160px"></asp:textbox> <%-- MMR | 2018.04.30 | YRS-AT-3849 | Removed function 'Validate2' called on blur event as not required --%>
								<rjs:popcalendar id="PopCalendar2" runat="server" Separator="/" Format="mm dd yyyy" ScriptsValidators="IsValidDate"></rjs:popcalendar>
								<asp:customvalidator id="valCustomTermDate" runat="server" Display="Dynamic" ControlToValidate="TextboxTermDate"
									ClientValidationFunction="IsValidDate" ErrorMessage="Enter valid term date">*</asp:customvalidator>
								<!--<asp:ImageButton id="ImageButton1" runat="server" ImageUrl="file:///C:\Inetpub\wwwroot\YMCAUI\images\nullify.gif"></asp:ImageButton></td>-->
							</td>
						</tr>
						<tr>
							<td align="left" width="279" height="24"><asp:label id="lblContributionRate" runat="server" CssClass="Label_Small">Contribution Rate</asp:label></td>
							<td align="left" height="24"><asp:dropdownlist id="drdwContributionRate" runat="server" CssClass="DropDown_Normal" Width="168px"
									AutoPostBack="True"></asp:dropdownlist></td>
						</tr>
						<tr>
							<td vAlign="top" align="left" width="279"><asp:label id="lblContributionOptions" runat="server" CssClass="Label_Small">Contribution Options</asp:label></td>
							<td align="left" height="180" valign="top"><asp:datagrid id="DataGridOptions" runat="server" CssClass="DataGrid_Grid"  autogeneratecolumns="False"
									PageSize="5" Width="168px">
									<AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
									<ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
									<HeaderStyle CssClass="DataGrid_HeaderStyle"></HeaderStyle>
									<Columns>
										<asp:TemplateColumn HeaderText="Selection">
											<ItemTemplate>
												<asp:CheckBox id="CheckBoxSelect" runat="server" autopostback="true" OnCheckedChanged="CheckBox_CheckedChanged"></asp:CheckBox>
											</ItemTemplate>
										</asp:TemplateColumn>
										<asp:BoundColumn DataField="Participant %" HeaderText="EE %" DataFormatString="{0:0.00}"></asp:BoundColumn>
										<asp:BoundColumn DataField="YMCA %" HeaderText="ER %" DataFormatString="{0:0.00}"></asp:BoundColumn>
									</Columns>
								</asp:datagrid>
							</td>
						</tr>
						<tr>
							<td width="279" align="left">
								<P><asp:label id="LabelTermDateErrorMsg" runat="server" Width="299px" DESIGNTIMEDRAGDROP="687"
										Visible="False">Term Date should be greater than Effective date</asp:label></P>
								<asp:validationsummary id="ValidationSummary1" runat="server"></asp:validationsummary></td>
						</tr>
						<tr>
							<td class="Td_ButtonContainer" align="right" width="279"><asp:button id="ButtonCancel" runat="server" CssClass="Button_Normal" Width="87" CausesValidation="False"
									Text="Cancel"></asp:button></td>
							<td class="Td_ButtonContainer" align="left"><asp:button id="ButtonOK" runat="server" CssClass="Button_Normal" Width="87" Text="OK"></asp:button></td>
						</tr>
					</table>
				</td>
			</tr>
		</table>
	</div>
	<asp:placeholder id="PlaceHolder1" runat="server"></asp:placeholder></form>
<!--#include virtual="bottom.html"-->

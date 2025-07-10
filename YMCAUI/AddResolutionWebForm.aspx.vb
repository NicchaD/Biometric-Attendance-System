'*******************************************************************************
' Copyright YMCA Retirement Fund All Rights Reserved. 
' Project Name		:	YMCA-YRS
' FileName			:	AddResolutionWebForm.aspx.vb
'*******************************************************************************
' Cache-Session     :   Vipul 03Feb06
'*******************************************************************************
''Changed By: Rahul Nasa Changed On:09 Mar,06 IssueId:2009: Resoultion date for YMCA's
'Shubhrata YREN 2691 Sep 21st 2006 Reason:To default vesting type to 2-2 for resolutions greater
'than 07/01/2006
'Modification History
'************************************************************************************
'Modified By        Date            Description
'*********************************************************************************************************************
'Aparna Samala      21/03/2007      YREN-3107
'Aparna Samala      21/03/07        Check if the sliding scale values is blank
'Aparna             04/10/2007      Removed top n left values to avoid mesaagebox breaking in IE7 -
'Anil               02/01/2008      Add Range validator for Add'l YMCA Contribution
'Swopna             13-May-08       BT-430 Re-opened
'Priya              04-May-2009     Bug Id 715 changes made in if condition added =.
'NP/PP/SR           2009.05.18      Optimizing the YMCA Screen
'Priya              08-June-2009    YRS 5.0-779 : Warning msg if new resolution effective date too far in future
'Neeraj Singh       12/Nov/2009     Added form name for security issue YRS 5.0-940 
'Shashi Shekhar     28 feb 2011     changes for YRS 5.0-1256 : Termination date when adding a new resolution 
'Priya				24-Mar-2011		BT-795 : Application should not allow the YMCA resolution Effective date is less than the most recent effective resolution.
'Priya				22-May-2012		BT-1029, YRS 5.0-1582 - Validate new Resolution effective date
'Shashank Patel		11-Jul-2012		BT-716,YRS 5.0-1256 : Termination date when adding a new resolution (re-opened)
'Anudeep            05-mar-2013     Bt-1859:YRS 5.0-1965 - cannnot chagne YMCA Resolution
'Shashank Patel     2014.10.13      BT-1995\YRS 5.0-2052: Erroneous updates occuring 
'Manthan Rajguru    2015.09.16      YRS-AT-2550: YRS data cleanup: copyright comments and namespace/reference (Changed the Namespace/reference from 'Infotech' to 'YMCARET')
'Manthan Rajguru    2018.04.30      YRS-AT-3849 -  YRS enh: correct error message for new resolution
'*********************************************************************************************************************

Imports System
Imports System.Collections
Imports System.ComponentModel
Imports System.Data
Imports System.Data.SqlClient
Imports System.Drawing
Imports System.Threading
Imports System.Reflection
Imports System.Web
Imports System.Web.SessionState
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.HtmlControls
Imports System.Data.DataRow
Imports System.Security.Permissions
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling


Public Class AddResolutionWebForm
    Inherits System.Web.UI.Page

    'below line is Added by Neeraj for issue id YRS 5.0-940 
    Dim strFormName As String = New String("AddResolutionWebForm.aspx")
    'End issue id YRS 5.0-940

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents LabelTermDate As System.Web.UI.WebControls.Label
    Protected WithEvents TextboxTermDate As System.Web.UI.WebControls.TextBox
    Protected WithEvents ButtonCancel As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonOK As System.Web.UI.WebControls.Button
    Protected WithEvents DataGridOptions As System.Web.UI.WebControls.DataGrid
    Protected WithEvents RequiredFieldValidator1 As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents ValidationSummary1 As System.Web.UI.WebControls.ValidationSummary
    Protected WithEvents valCustomEffDate As System.Web.UI.WebControls.CustomValidator
    Protected WithEvents valCustomTermDate As System.Web.UI.WebControls.CustomValidator
    Protected WithEvents LabelHdr As System.Web.UI.WebControls.Label
    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object
    Dim g_dataset_dsVestingType As DataSet
    Dim g_dataset_dsResolutionType As DataSet
    Dim InsertRowVesting As DataRow
    Protected WithEvents PlaceHolder1 As System.Web.UI.WebControls.PlaceHolder
    Dim InsertRowResolution As DataRow
    Protected WithEvents LabelTermDateErrorMsg As System.Web.UI.WebControls.Label
    Protected WithEvents LabelDateErrorMsg As System.Web.UI.WebControls.Label
    Protected WithEvents PopCalendar1 As RJS.Web.WebControl.PopCalendar
    Protected WithEvents PopCalendar2 As RJS.Web.WebControl.PopCalendar
    Protected WithEvents ImageButton1 As System.Web.UI.WebControls.ImageButton
    Protected WithEvents lblContributionRate As System.Web.UI.WebControls.Label
    Protected WithEvents lblContributionOptions As System.Web.UI.WebControls.Label
    Protected WithEvents lblEffDate As System.Web.UI.WebControls.Label
    Protected WithEvents TextboxEffDate As System.Web.UI.WebControls.TextBox
    Protected WithEvents drdwContributionRate As System.Web.UI.WebControls.DropDownList
    Protected WithEvents LabelEffDate As System.Web.UI.WebControls.Label
    Protected WithEvents LabelVestingType As System.Web.UI.WebControls.Label
    Protected WithEvents DropDownListVestingType As System.Web.UI.WebControls.DropDownList
    Protected WithEvents LabelResolutionType As System.Web.UI.WebControls.Label
    Protected WithEvents DropdownlistResolutionType As System.Web.UI.WebControls.DropDownList
    Protected WithEvents LabelOptions As System.Web.UI.WebControls.Label
    Protected WithEvents LabelSlidingScale As System.Web.UI.WebControls.Label
    Protected WithEvents TextboxSlidingScale As System.Web.UI.WebControls.TextBox
    Protected WithEvents RegularExpressionValidator1 As System.Web.UI.WebControls.RegularExpressionValidator
    Protected WithEvents RegularExpressionValidator3 As System.Web.UI.WebControls.RegularExpressionValidator
    Protected WithEvents LabelAddlYMCA As System.Web.UI.WebControls.Label
    Protected WithEvents TextboxAddlYMCA As System.Web.UI.WebControls.TextBox
    Protected WithEvents RegularExpressionValidator2 As System.Web.UI.WebControls.RegularExpressionValidator
    Protected WithEvents RegularExpressionValidator4 As System.Web.UI.WebControls.RegularExpressionValidator
    Protected WithEvents RangeValidator1 As System.Web.UI.WebControls.RangeValidator

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region
    Dim g_ds_datasetResolution As DataSet
    Dim g_DatasetYMCAList As DataSet
#Region "Property"
    'SP 2014.10.13 BT-1995\YRS 5.0-2052: Erroneous updates occuring -Start
    '1. Define Property 
    Public Property Session(sName As String) As Object
        Get
            Return MyBase.Session(Me.UniqueSessionId + sName)
        End Get
        Set(value As Object)
            MyBase.Session(Me.UniqueSessionId + sName) = value
        End Set
    End Property

    ' 2. UniqueSession-forMultiTabs
    Public ReadOnly Property UniqueSessionId As String
        Get
            If Request.QueryString("UniqueSessionID") = Nothing Then
                Return String.Empty
            Else
                Return Request.QueryString("UniqueSessionID").ToString()
            End If

        End Get
    End Property
    'SP 2014.10.13 BT-1995\YRS 5.0-2052: Erroneous updates occuring -End

    Public Property dsContributionOption() As DataSet
        Get
            Return Session("dsContributionOption")
        End Get
        Set(ByVal Value As DataSet)
            Session("dsContributionOption") = Value
        End Set
    End Property
#End Region

    Private Sub LoadData()
        Session("dsContributionOption") = Nothing
        dsContributionOption = YMCARET.YmcaBusinessObject.AddResolutionBOClass.LookUpOptionType()
    End Sub

    Private Sub PopulateData()
        g_DatasetYMCAList = Session("YMCA List")
        g_ds_datasetResolution = Session("YMCA Resolution")
		Dim EffectiveDate As DateTime
		Dim TerminateDate As DateTime
		Dim MaxDate As DateTime

        Dim k As Integer
		If HelperFunctions.isNonEmpty(g_ds_datasetResolution) Then
			'Priya : 24-Mar-2011 :BT-795 : Application should not allow the YMCA resolution Effective date is less than the most recent effective resolution.
			'Commented previous code and added new code to calculate max value of Effective date and max value of Termination date.
            Dim Value As DateTime
            'Start:Anudeep:05-03-2013 - Bt-1859:YRS 5.0-1965 - cannnot chagne YMCA Resolution
            'EffectiveDate = Convert.ToDateTime(g_ds_datasetResolution.Tables(0).Compute("Max([Eff. Date])", String.Empty)).ToString("MM/dd/yyyy")
            '''If EffectiveDate = String.Empty AndAlso IsDBNull(EffectiveDate) Then
            'If IsDBNull(EffectiveDate) Then
            '	EffectiveDate = DateTime.MinValue
            'End If

            If Not IsDBNull(g_ds_datasetResolution.Tables(0).Compute("Max([Eff. Date])", String.Empty)) Then
                EffectiveDate = Convert.ToDateTime(g_ds_datasetResolution.Tables(0).Compute("Max([Eff. Date])", String.Empty)).ToString("MM/dd/yyyy")
            Else
                EffectiveDate = DateTime.MinValue
            End If
            Value = EffectiveDate
            'TerminateDate = Convert.ToDateTime(g_ds_datasetResolution.Tables(0).Compute("Max([Term. Date])", String.Empty)).ToString("MM/dd/yyyy")
            ''If TerminateDate = String.Empty AndAlso IsDBNull(TerminateDate) Then
            'If IsDBNull(TerminateDate) Then
            '	TerminateDate = DateTime.MinValue
            'End If

            If Not IsDBNull((g_ds_datasetResolution.Tables(0).Compute("Max([Term. Date])", String.Empty))) Then
                TerminateDate = Convert.ToDateTime(g_ds_datasetResolution.Tables(0).Compute("Max([Term. Date])", String.Empty)).ToString("MM/dd/yyyy")
            Else
                TerminateDate = DateTime.MinValue
            End If
            'End:Anudeep:05-03-2013 - Bt-1859:YRS 5.0-1965 - cannnot chagne YMCA Resolution
			'If TerminateDate <> String.Empty AndAlso EffectiveDate <> String.Empty Then
			If (TerminateDate > Value) Then
				Value = Convert.ToDateTime(TerminateDate).ToString("MM/dd/yyyy")
			ElseIf EffectiveDate > Value Then
				Value = Convert.ToDateTime(EffectiveDate).ToString("MM/dd/yyyy")
			Else
				Value = Convert.ToDateTime(EffectiveDate).ToString("MM/dd/yyyy")
			End If
			'ElseIf EffectiveDate >= Value Then
			'Value = Convert.ToDateTime(EffectiveDate).ToString("MM/dd/yyyy")
			'End If
			MaxDate = Value
			'For k = 0 To g_ds_datasetResolution.Tables(0).Rows.Count - 1
			'	Dim Value As String
			'	If Not IsDBNull(g_ds_datasetResolution.Tables(0).Rows(k)("Eff. Date")) Then
			'		EffectiveDate = Convert.ToDateTime(g_ds_datasetResolution.Tables(0).Rows(k)("Eff. Date")).ToString("MM/dd/yyyy")
			'		Value = EffectiveDate
			'	Else
			'		EffectiveDate = ""
			'	End If
			'	If Not IsDBNull(g_ds_datasetResolution.Tables(0).Rows(k)("Term. Date")) Then
			'		TerminateDate = Convert.ToDateTime(g_ds_datasetResolution.Tables(0).Rows(k)("Term. Date")).ToString("MM/dd/yyyy")
			'	Else
			'		TerminateDate = ""
			'	End If
			'	If TerminateDate <> "" AndAlso EffectiveDate <> "" Then
			'		If (TerminateDate > Value) Then
			'			Value = Convert.ToDateTime(TerminateDate).ToString("MM/dd/yyyy")
			'		ElseIf EffectiveDate >Value Then
			'			Value = Convert.ToDateTime(EffectiveDate).ToString("MM/dd/yyyy")
			'		End If
			'	ElseIf EffectiveDate >= Value Then
			'		Value = Convert.ToDateTime(EffectiveDate).ToString("MM/dd/yyyy")
			'	End If
			'	MaxDate = Value
			'Next
			'End 24-Mar-2011 :BT-795 : Application should not allow the YMCA resolution Effective date is less than the most recent effective resolution.
			ViewState("MAxDate") = MaxDate
		Else
			ViewState("MAxDate") = Nothing
		End If

		Dim EffDate As String
		If (Convert.ToString(ViewState("MAxDate")) <> "") Then
			If CType(ViewState("MAxDate"), Date) < System.DateTime.Today() Then
				EffDate = CType(System.DateTime.Today().Subtract(TimeSpan.FromDays(DateTime.Now.Day - 1)), String)
				Me.TextboxEffDate.Text = EffDate
				'Priya : 24-Mar-2011 :BT-795 : Application should not allow the YMCA resolution Effective date is less than the most recent effective resolution.
				'Added else part is max date is greater than today's date
			Else
				EffDate = CType(ViewState("MAxDate"), Date).AddDays(1)
				Me.TextboxEffDate.Text = EffDate
			End If
		Else
			EffDate = CType(System.DateTime.Today().Subtract(TimeSpan.FromDays(DateTime.Now.Day - 1)), String)
			Me.TextboxEffDate.Text = EffDate
		End If

		If HelperFunctions.isNonEmpty(dsContributionOption) Then
			Dim i As Integer
			For i = 0 To dsContributionOption.Tables(0).Rows.Count - 1
				If i <> 0 Then
					If dsContributionOption.Tables(0).Rows(i)("Total") <> dsContributionOption.Tables(0).Rows(i - 1)("Total") Then
						drdwContributionRate.Items.Add(CType(dsContributionOption.Tables(0).Rows(i)("Total"), Decimal).ToString("###,###,##0.00").Trim)
					End If
				Else
					drdwContributionRate.Items.Add(Convert.ToString(dsContributionOption.Tables(0).Rows(i)("Total")).Trim)
				End If
			Next

			dsContributionOption.Tables(0).DefaultView.RowFilter = "total =" & drdwContributionRate.SelectedItem.Text
			dsContributionOption.Tables(0).DefaultView.Sort = "[Participant %] DESC"
			DataGridOptions.DataSource = dsContributionOption.Tables(0).DefaultView
			DataGridOptions.DataBind()
		End If

	End Sub

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If MyBase.Session("LoggedUserKey") Is Nothing Then
                Response.Redirect("Login.aspx", False)
            End If

            If Not IsPostBack Then
				' Me.TextboxEffDate.Attributes.Add("onblur", "javascript:Validate(this);") '2012.07.11 SP : BT-716,YRS 5.0-1256 : Termination date when adding a new resolution
                Me.TextboxTermDate.Attributes.Add("onblur", "javascript:Validate(this);")
                LoadData()
                PopulateData()
            End If

			If Request.Form("Continue") = "Continue" Then
				'Priya	22-May-2012 BT-1029, YRS 5.0-1582 - Validate new Resolution effective date
				If Session("performTransmitalValidation") <> "True" Then
					performTransmitalValidation()
					Session("performTransmitalValidation") = ""
					Exit Sub
				End If
				'END	22-May-2012 BT-1029, YRS 5.0-1582 - Validate new Resolution effective date
				SaveResolution()
			End If

        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + ex.Message)
        End Try
    End Sub

    Public Sub CheckBox_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim l_CheckBox As CheckBox

        Try
            For Each l_DataGridItem As DataGridItem In Me.DataGridOptions.Items
                l_CheckBox = l_DataGridItem.FindControl("CheckBoxSelect")
                If (Not l_CheckBox Is Nothing) Then
                    If l_CheckBox.Checked = True Then
                        l_CheckBox.Checked = Not (l_CheckBox.Checked)
                    End If
                End If
            Next

            l_CheckBox = CType(sender, CheckBox)

            If Not l_CheckBox Is Nothing Then
                l_CheckBox.Checked = True
            End If

        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = ex.Message.Trim.ToString()
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message)
        End Try

    End Sub
	'Priya	22-May-2012 BT-1029, YRS 5.0-1582 - Validate new Resolution effective date
	Private Function TransmittalExsits() As Boolean
		Session("performTransmitalValidation") = "True"
		Dim l_transmital_date As String
		l_transmital_date = YMCARET.YmcaBusinessObject.AddResolutionBOClass.GetYMCATransmitalDate(Session("GuiUniqueId").ToString(), Me.TextboxEffDate.Text.Trim)
		If l_transmital_date = String.Empty Then
			Return False
		Else
			Return True

		End If

	End Function
	Private Function performTransmitalValidation() As Boolean
		If TransmittalExsits() = True Then
			MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Transmittals exist that are dated later than the effective date of the new resolution. <br/>  Do you wish to continue?", MessageBoxButtons.ContinueCancel) '2012.07.11 SP :BT-716, YRS 5.0-1256 : Termination date when adding a new resolution
			Return True
		Else
			Return False
		End If

	End Function
	'End 22-May-2012 BT-1029, YRS 5.0-1582 - Validate new Resolution effective date
    Private Sub ButtonOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonOK.Click


		Try
			
			If Me.TextboxTermDate.Text.Trim.Length > 0 Then
				If CType(Me.TextboxTermDate.Text, Date) < CType(Me.TextboxEffDate.Text, Date) Then
					MessageBox.Show(PlaceHolder1, "Error", "Termination Date must be equal to or greater than Effective Date", MessageBoxButtons.Stop)
					Exit Sub
				ElseIf CType(Me.TextboxTermDate.Text, Date) > DateTime.Now.AddMonths(1) Then
					MessageBox.Show(PlaceHolder1, "Error", "Termination Date cannot be later than next month", MessageBoxButtons.Stop)
					Exit Sub
				End If
			End If


			Dim l_CheckBox As CheckBox
			Dim i As Integer
			Dim l_rowindex As Integer
			l_rowindex = -1
			For Each l_DataGridItem As DataGridItem In Me.DataGridOptions.Items
				l_CheckBox = l_DataGridItem.FindControl("CheckBoxSelect")
				If (Not l_CheckBox Is Nothing) Then
					If l_CheckBox.Checked = True Then
						l_rowindex = i
					End If
				End If
				i = i + 1
			Next
			If l_rowindex < 0 Then
				MessageBox.Show(PlaceHolder1, "Error", "Please select an option", MessageBoxButtons.Stop)
				Exit Sub
			End If

			If Me.TextboxEffDate.Text.Trim.Length > 0 Then
				If Not ViewState("MAxDate") Is Nothing Then
					'If CType(Me.TextboxEffDate.Text, Date) < CType(Session("ResoPopupEffDate"), Date) Then
					If CType(Me.TextboxEffDate.Text, Date) < CType(ViewState("MAxDate"), Date) Then
						If CType(Session("SingleResolution"), Boolean) = True Then
							MessageBox.Show(PlaceHolder1, "Error", "Effective Date must be Greater Than Effective Date of previous resolution", MessageBoxButtons.Stop)
							Exit Sub
                        Else
                            'START: MMR | 2018.04.30 | YRS-AT-3849 | Commented existing code and changed message text. Removed "or Equal to" from text 
                            'MessageBox.Show(PlaceHolder1, "Error", "Effective Date must be Greater Than or Equal to Termination Date of last resolution", MessageBoxButtons.Stop)
                            MessageBox.Show(PlaceHolder1, "Error", "Effective Date must be Greater than Termination Date of last resolution", MessageBoxButtons.Stop)
                            'END: MMR | 2018.04.30 | YRS-AT-3849 | Commented existing code and changed message text. Removed "or Equal to" from text
                            Exit Sub
						End If
					End If
				End If

				Dim todayDate As DateTime
				todayDate = System.DateTime.Today.ToShortDateString()

				'08-June-2009 YRS 5.0-779 : Warning msg if new resolution effective date too far in future
				Dim ds_6Months As DataSet
				ds_6Months = YMCARET.YmcaBusinessObject.AddResolutionBOClass.getConfigurationValue("NEW_RESOLUTION_VALIDATE_MONTHS")
				Dim HardshipRenewalMonths As Integer

				If HelperFunctions.isNonEmpty(ds_6Months) AndAlso Not IsDBNull(ds_6Months.Tables(0).Rows(0)("Value")) AndAlso Convert.ToString(ds_6Months.Tables(0).Rows(0)("Value")).Trim <> "" Then
					HardshipRenewalMonths = Convert.ToInt32(Convert.ToString(ds_6Months.Tables(0).Rows(0)("Value")).Trim)
				Else
					HardshipRenewalMonths = 6
				End If

				Dim arr As Array = todayDate.ToString.Split("/")
				arr(1) = "01"
				todayDate = arr(0) + "/" + arr(1) + "/" + arr(2)
				
				Dim str6monthDate As String
				str6monthDate = todayDate.AddMonths(HardshipRenewalMonths + 1).ToShortDateString()

				If (Convert.ToDateTime(TextboxEffDate.Text) >= Convert.ToDateTime(str6monthDate)) Then
					MessageBox.Show(PlaceHolder1, "YMCA-YRS", "The selected effective date is 6 or more months into the future.  Please confirm by selecting Continue or select Cancel to re-enter.", MessageBoxButtons.ContinueCancel)
					Exit Sub
				End If
				'End YRS 5.0-779
				'Priya	22-May-2012 BT-1029, YRS 5.0-1582 - Validate new Resolution effective date
				If performTransmitalValidation() = True Then
					Exit Sub
				End If
				'Priya	22-May-2012 BT-1029, YRS 5.0-1582 - Validate new Resolution effective date

			End If
			SaveResolution()

		Catch ex As Exception
			Dim l_String_Exception_Message As String
			l_String_Exception_Message = ex.Message.Trim.ToString()
			Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message)
		End Try
    End Sub
    '08-June-2009 YRS 5.0-779 : Warning msg if new resolution effective date too far in future
    'Make separat save function to save informaion if user click on continue
    Private Sub SaveResolution()
        Dim l_CheckBox As CheckBox
        Dim i As Integer
        ' Dim l_rowindex As Integer
        Dim l_dataset_OptionType As DataSet
        Dim l_datarow As DataRow
        Dim msg As String

        ' Dim MessageBoxFlag As Boolean

        Dim objRes As New Resolution

        msg = ""
        'l_rowindex = -1

        For Each l_DataGridItem As DataGridItem In Me.DataGridOptions.Items
            l_CheckBox = l_DataGridItem.FindControl("CheckBoxSelect")
            If (Not l_CheckBox Is Nothing) Then
                If l_CheckBox.Checked = True Then
                    l_CheckBox.Checked = False
                    'l_rowindex = i
                    objRes.ParticipantPerc = l_DataGridItem.Cells(1).Text.Trim()
                    objRes.YMCAPerc = l_DataGridItem.Cells(2).Text.Trim()
                End If
            End If
            'i = i + 1
        Next
        'If l_rowindex < 0 Then
        ' MessageBox.Show(PlaceHolder1, "Error", "Please select an option", MessageBoxButtons.Stop)
        ' MessageBoxFlag = True
        'Else
        'If l_rowindex >= 0 Then

        If (Me.TextboxEffDate.Text = "") Then
            objRes.EffectiveDate = Nothing
        Else
            objRes.EffectiveDate = TextboxEffDate.Text
        End If

        If (Me.TextboxTermDate.Text = "") Then
            objRes.TermDate = String.Empty
        Else
            objRes.TermDate = TextboxTermDate.Text
        End If

        'Priya 13-April-2009 Change Vesting typecode to 22 for PhaseV
        If Not IsDBNull(dsContributionOption.Tables(1)) Then
            If (dsContributionOption.Tables(1).Rows.Count > 0) Then
                If Not IsDBNull(dsContributionOption.Tables(1).Rows(0)("chrAccttype")) Then
                    objRes.ResolutionType = dsContributionOption.Tables(1).Rows(0)("chrAccttype")
                End If
                If Not IsDBNull(dsContributionOption.Tables(1).Rows(0)("chvShortDescription")) Then
                    objRes.ResolutionTypeDesc = dsContributionOption.Tables(1).Rows(0)("chvShortDescription")
                End If
            End If
        End If
        If Not IsDBNull(dsContributionOption.Tables(2)) Then
            If (dsContributionOption.Tables(2).Rows.Count > 0) Then
                If Not IsDBNull(dsContributionOption.Tables(2).Rows(0)("chrVestingTypeCode")) Then
                    objRes.VestingType = dsContributionOption.Tables(2).Rows(0)("chrVestingTypeCode")
                End If
                If Not IsDBNull(dsContributionOption.Tables(2).Rows(0)("chvShortDescription")) Then
                    objRes.VestingTypeDesc = dsContributionOption.Tables(2).Rows(0)("chvShortDescription")
                End If
            End If
        End If

        objRes.SlidingScalePerc = "0"


        objRes.AddlYMCAPerc = "0"

        Dim objPopupAction As PopupResult = New PopupResult
        objPopupAction.Page = "RESOLUTION"
        objPopupAction.Action = PopupResult.ActionTypes.ADD
        objPopupAction.State = objRes
        Session("PopUpAction") = objPopupAction
        ''End 13-April-2009

        'If MessageBoxFlag = False Then
        msg = msg + "<Script Language='JavaScript'>"
        msg = msg + "window.opener.document.forms(0).submit();" '"window.opener.location.href=window.opener.location.href;"
        msg = msg + "self.close();"
        msg = msg + "</Script>"
        Response.Write(msg)
        'End If

        'End If
    End Sub
    Private Sub ButtonCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonCancel.Click
        Dim msg As String
        msg = ""
        Try
            msg = msg + "<Script Language='JavaScript'>"
            msg = msg + "window.opener.document.forms(0).submit();" '"window.opener.location.href=window.opener.location.href;"
            msg = msg + "self.close();"
            msg = msg + "</Script>"
            Response.Write(msg)
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = ex.Message.Trim.ToString()
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message)
        End Try
    End Sub

    Private Sub ButtonClearDate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.TextboxTermDate.Text = String.Empty
    End Sub

    Private Sub ImageButton1_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButton1.Click
        Me.TextboxTermDate.Text = String.Empty
    End Sub

    Private Sub PopCalendar1_SelectionChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PopCalendar1.SelectionChanged
        Try
            Dim l_date As Date
            l_date = PopCalendar1.SelectedDate
            If l_date.Day <> 1 Then
                l_date.AddMonths(1)
                'SS:28 feb 2011: changes below line for YRS 5.0-1256 : Termination date when adding a new resolution (Second part)
                l_date = Convert.ToString(l_date.Month()) + "/" + Convert.ToString(l_date.Day()) + "/" + Convert.ToString(l_date.Year())
                TextboxEffDate.Text = l_date
            Else
                TextboxEffDate.Text = l_date
            End If
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message)
        End Try
    End Sub

    Private Sub PopCalendar2_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles PopCalendar2.SelectionChanged
        Try
            Dim l_messagebox As MessageBoxClass
            Dim l_date As Date

            Dim rightNow As DateTime = DateTime.Now
            TextboxTermDate.Text = PopCalendar2.SelectedDate
            l_date = TextboxTermDate.Text
            If l_date.Day <> 1 Then
                l_date.AddMonths(1)
                l_date = Convert.ToString(l_date.Month()) + "/" + "1/" + Convert.ToString(l_date.Year())
                TextboxTermDate.Text = l_date
            Else
                TextboxTermDate.Text = l_date
            End If
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message)
        End Try
    End Sub    'Rahul 09 Mar,06

    Private Sub TextboxEffDate_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            Dim l_date As Date
            l_date = TextboxEffDate.Text
            If l_date.Day <> 1 Then
                l_date.AddMonths(1)
                l_date = Convert.ToString(l_date.Month()) + "/" + "1/" + Convert.ToString(l_date.Year())
                TextboxTermDate.Text = l_date
            Else
                TextboxTermDate.Text = l_date
            End If
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message)
        End Try
    End Sub

    Private Sub drdwContributionRate_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles drdwContributionRate.SelectedIndexChanged
        If HelperFunctions.isNonEmpty(dsContributionOption) Then
            dsContributionOption.Tables(0).DefaultView.RowFilter = "total =" & drdwContributionRate.SelectedItem.Text
            dsContributionOption.Tables(0).DefaultView.Sort = "[Participant %] ASC"
            DataGridOptions.DataSource = dsContributionOption.Tables(0).DefaultView
            DataGridOptions.DataBind()
        End If
    End Sub
End Class
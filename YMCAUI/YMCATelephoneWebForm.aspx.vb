'*******************************************************************************
' Copyright YMCA Retirement Fund All Rights Reserved. 
'
' Project Name		:	YMCA-YRS
' FileName			:	YMCATelephoneWebForm.aspx.vb
' Author Name		:	Shefali
' Employee ID		:	33488
' Email				:	shefali.bharti@3i-infotech.com
' Contact No		:	8740
' Creation Time		:	5/17/2005 4:55:05 PM
' Program Specification Name	: Doc 3.1.3	
' Unit Test Plan Name			:	
' Description					:	This is a telephone information pop up window in a General Tab
' Changed by			:	Shefali Bharti  
' Changed on			:	01/09/2005
' Change Description	:	Coding
'Cache to Session       :   Ragesh 34231 02/02/06 Cache to Session

' Changed by			:	Swopna Valappil  
' Changed on			:	2-Jan-2008
' Change Description	:	YREN-3792
'****************************************************
'Modification History
'****************************************************
'Modified by     Date           Description
'****************************************************
'NP/PP/SR       2009.05.18      Optimizing the YMCA Screen
'Sanjay Rawat   2009.06.02      Optimizing the Catch Block
'Priya Jawale   2009.06.03      BT 814 Telephone Popup updates the wrong phone number Added ViewState value
'Priya Jawale   2009.06.03      BT 813 Validation is being fired at the wrong click event - Telephone
'Neeraj Singh   12/Nov/2009     Added form name for security issue YRS 5.0-940 
'Priya          2010-06-03      Changes made for enhancement in vs-2010 
'Anudeep        2013-08.02      Bt 1555 : YRS 5.0-1769:Length of phone numbers.
'Shashank Patel 2014.10.13      BT-1995\YRS 5.0-2052: Erroneous updates occuring 
'Manthan Rajguru 2015.09.16     YRS-AT-2550: YRS data cleanup: copyright comments and namespace/reference (Changed the Namespace/reference from 'Infotech' to 'YMCARET')
'Pramod Pokale  2015.10.13      YRS-AT-2588: implement some basic telephone number validation Rules
'*******************************************************************************
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

Public Class YMCATelephoneWebForm
    Inherits System.Web.UI.Page
    'below line is Added by Neeraj for issue id YRS 5.0-940 
    Dim strFormName As String = New String("YMCATelephoneWebForm.aspx")
    'End issue id YRS 5.0-940
    Protected WithEvents LabelNoRecord As System.Web.UI.WebControls.Label
    Protected WithEvents PlaceHolder1 As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents RequiredFieldValidator1 As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents ValidationSummary1 As System.Web.UI.WebControls.ValidationSummary
    Protected WithEvents RegularExpressionValidator1 As System.Web.UI.WebControls.RegularExpressionValidator
    Dim Page_Mode As String
    Dim g_dataset_dsTelephoneType As DataSet
    Dim g_dataset_dsTelephoneInformation As New DataSet

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    Protected WithEvents TextBoxEffectiveDate As YMCAUI.DateUserControl
    Protected WithEvents TextBoxTelephone As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxExt As System.Web.UI.WebControls.TextBox
    Protected WithEvents ButtonCancel As System.Web.UI.WebControls.Button
    Protected WithEvents DropdownType As System.Web.UI.WebControls.DropDownList
    Protected WithEvents CheckBoxPrimary As System.Web.UI.WebControls.CheckBox
    Protected WithEvents CheckBoxActive As System.Web.UI.WebControls.CheckBox
    Protected WithEvents DataGridYMCATelephone As System.Web.UI.WebControls.DataGrid
    Protected WithEvents LabelType As System.Web.UI.WebControls.Label
    Protected WithEvents LabelEffectiveDate As System.Web.UI.WebControls.Label
    Protected WithEvents LabelTelephone As System.Web.UI.WebControls.Label
    Protected WithEvents LabelExt As System.Web.UI.WebControls.Label
    Protected WithEvents ButtonOK As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonTelephoneSave As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonTelephoneAdd As System.Web.UI.WebControls.Button
    Protected WithEvents HiddenText As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HiddenSecControlName As System.Web.UI.HtmlControls.HtmlInputHidden

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Public Property Session(sname As String) As Object
        Get
            Return MyBase.Session(Me.uniqueSessionId + sname)
        End Get
        Set(value As Object)
            MyBase.Session(Me.uniqueSessionId + sname) = value
        End Set
    End Property

    ' 2. Vipul 13Aug14 UniqueSession-forMultiTabs

    Public Property uniqueSessionId As String
        Get
            Return IIf(ViewState("Sessionname") = Nothing, String.Empty, ViewState("Sessionname"))
        End Get
        Set(ByVal Value As String)
            ViewState("Sessionname") = Value
        End Set
    End Property     'Vipul 13Aug14 UniqueSession-forMultiTabs  /e

    Public Sub getSecuredControls()
        Dim l_Int_UserId As Integer
        Dim l_String_FormName As String
        Dim l_control_Id As Integer
        Dim ds_AllsecItems As DataSet

        Dim l_int_access As Integer
        Dim l_string_controlNames As String
        Dim l_ds_ctrlNames As DataSet
        l_string_controlNames = ""
        l_Int_UserId = Convert.ToInt32(MyBase.Session("LoggedUserKey"))
        l_String_FormName = Convert.ToString("YMCATelephoneWebForm.aspx")
        Me.TextBoxTelephone.Attributes.Add("onkeypress", "javascript:OnSpace();")
        ds_AllsecItems = YMCARET.YmcaBusinessObject.ControlSecurityBOClass.GetSecuredControlsOnForm(l_String_FormName)

        If Not ds_AllsecItems Is Nothing Then
            If Not ds_AllsecItems.Tables(0).Rows.Count = 0 Then
                For Each dr As DataRow In ds_AllsecItems.Tables(0).Rows
                    l_control_Id = Convert.ToInt32(dr("sfc_ControlId"))
                    l_ds_ctrlNames = YMCARET.YmcaBusinessObject.ControlSecurityBOClass.GetControlNames(l_control_Id)
                    l_int_access = YMCARET.YmcaBusinessObject.SecurityCheckBOClass.LookUpLoginAccessPermission(l_Int_UserId, l_ds_ctrlNames.Tables(0).Rows(0)(0).ToString().Trim())

                    If l_int_access = 0 Or l_int_access = 1 Then
                        l_string_controlNames = l_string_controlNames + l_ds_ctrlNames.Tables(0).Rows(0)(0).ToString().Trim() + ","
                    End If
                Next
                Me.HiddenSecControlName.Value = l_string_controlNames
            End If
        End If
    End Sub
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.LabelType.AssociatedControlID = Me.DropdownType.ID
        Me.LabelTelephone.AssociatedControlID = Me.TextBoxTelephone.ID
        Me.LabelExt.AssociatedControlID = Me.TextBoxExt.ID
        Me.ButtonTelephoneAdd.Attributes.Add("onclick", "javascript:CheckAccess('ButtonTelephoneAdd');")
        Me.ButtonTelephoneSave.Attributes.Add("onclick", "javascript:CheckAccess('ButtonTelephoneSave');")
        Me.TextBoxEffectiveDate.RequiredDate = True
        If MyBase.Session("LoggedUserKey") Is Nothing Then
            Response.Redirect("Login.aspx", False)
        End If
        Try
            If Not Me.IsPostBack Then
                If Request.QueryString("UniqueSessionID") IsNot Nothing Then
                    uniqueSessionId = Request.QueryString("UniqueSessionID")
                End If
                getSecuredControls()
                g_dataset_dsTelephoneType = YMCARET.YmcaBusinessObject.YMCATelephoneBOClass.LookUpTelephoneType()
                Me.DropdownType.DataSource = g_dataset_dsTelephoneType
                Me.DropdownType.DataMember = "Telephone Type"
                Me.DropdownType.DataTextField = "chvShortDescription"
                Me.DropdownType.DataValueField = "chvCodeValue"
                Me.DropdownType.SelectedValue = "OFFICE"
                Me.DropdownType.DataBind()
                PopulateData()
                Me.LabelNoRecord.Visible = False
                DisableSaveCancelButtons()
            Else
                Me.LabelNoRecord.Visible = False
            End If

        Catch ex As SqlException
            'Throw
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = ex.Message.Trim.ToString()
            Response.Redirect("ErrorPageForm.aspx?DBMessage=" + l_String_Exception_Message + "&FormType=Popup", False)
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = ex.Message.Trim.ToString()
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message + "&FormType=Popup", False)
            'Throw
        End Try
    End Sub
    Public Sub PopulateData()
        g_dataset_dsTelephoneInformation = DirectCast(Session("Telephone Information"), DataSet)
        If g_dataset_dsTelephoneInformation.Tables.Count <> 0 Then
            Me.DataGridYMCATelephone.DataSource = g_dataset_dsTelephoneInformation.Tables(0)
            Me.DataGridYMCATelephone.DataBind()
        End If
    End Sub
    Private Sub UpdateTelephone(ByVal dr As DataRow)
        If Not dr Is Nothing Then
            If dr.IsNull("guiUniqueId") Then
                dr("guiUniqueId") = Guid.NewGuid().ToString()
            End If
            dr("Type") = Me.DropdownType.SelectedValue
            dr("Primary") = IIf(Me.CheckBoxPrimary.Checked = False, 0, 1)
            dr("Active") = IIf(Me.CheckBoxActive.Checked = False, 0, 1)

            If Me.TextBoxEffectiveDate.Text.Trim.Length = 0 Then
                dr("Effective Date") = DBNull.Value
            Else
                'dr("Effective Date") = String.Format("{0:MM/dd/yyyy}", Date.Parse(TextBoxEffectiveDate.Text.Trim))
                dr("Effective Date") = TextBoxEffectiveDate.Text.Trim
            End If
            'Telephone cannot be blank
            dr("Telephone") = TextBoxTelephone.Text.Trim
            dr("Ext.") = Me.TextBoxExt.Text.Trim
            'If this address is being saved as primary then other addresses are to be marked as non-primary
            If dr("Primary") = True Then
                Dim r As DataRow
                For Each r In dr.Table.Rows
                    If Not r.IsNull("Primary") AndAlso r("Primary") = True _
                        AndAlso r("guiUniqueId") <> dr("guiUniqueId") Then
                        r.Item("Primary") = 0
                    End If
                Next
            End If
        End If
        Session("Telephone Information") = g_dataset_dsTelephoneInformation
        PopulateData()
    End Sub
    Private Function PopulateDataIntoControls(ByVal TelephoneUniqueid As String) As Boolean
        Dim l_DataSet As DataSet
        Dim l_DataTable As DataTable
        Dim l_DataRow As DataRow
        Dim l_InsertRowCountry As DataRow
        Dim l_InsertRowState As DataRow

        Me.DropdownType.SelectedIndex = 0
        Me.TextBoxTelephone.Text = String.Empty
        Me.TextBoxExt.Text = String.Empty
        Me.TextBoxEffectiveDate.Text = String.Empty
        Me.CheckBoxActive.Checked = False
        Me.CheckBoxPrimary.Checked = False
        ViewState("TelephoneUniqueId") = String.Empty

        l_DataSet = g_dataset_dsTelephoneInformation
        If HelperFunctions.isEmpty(l_DataSet) Then Return False

        l_DataRow = HelperFunctions.GetRowForUpdation(g_dataset_dsTelephoneInformation.Tables(0), "guiUniqueId", TelephoneUniqueid)
        If IsNothing(l_DataRow) Then Return False

        Me.DropdownType.SelectedValue = CType(l_DataRow("Type"), String).Trim
        Session("Selected_TelephoneType") = CType(l_DataRow("Type"), String).Trim

        If l_DataRow("Primary").GetType.ToString = "System.DBNull" Then
            Me.CheckBoxPrimary.Checked = False
        Else
            If l_DataRow("Primary") = 0 Then
                Me.CheckBoxPrimary.Checked = False
            Else
                Me.CheckBoxPrimary.Checked = True
            End If
        End If

        If l_DataRow("Active").GetType.ToString = "System.DBNull" Then
            Me.CheckBoxActive.Checked = False
        Else
            If l_DataRow("Active") = 0 Then
                Me.CheckBoxActive.Checked = False
            Else
                Me.CheckBoxActive.Checked = True
            End If
        End If

        If l_DataRow("Effective Date").GetType.ToString = "System.DBNull" Then
            Me.TextBoxEffectiveDate.Text = "01/01/1900"
        Else
            'Me.TextBoxEffectiveDate.Text = CType(l_DataRow("Effective Date"), String).Trim
            Me.TextBoxEffectiveDate.Text = String.Format("{0:MM/dd/yyyy}", Date.Parse(l_DataRow("Effective Date")))
        End If

        'field is a compulsory field
        Me.TextBoxTelephone.Text = CType(l_DataRow("Telephone"), String).Trim

        If l_DataRow("Ext.").GetType.ToString = "System.DBNull" Then
            Me.TextBoxExt.Text = String.Empty
        Else
            Me.TextBoxExt.Text = CType(l_DataRow("Ext."), String).Trim
        End If
        'BT 814 : Telephone Popup updates the wrong phone number
        ViewState("TelephoneUniqueId") = TelephoneUniqueid.Trim
        'End BT 814
        Return True
    End Function
    Private Sub DataGridYMCATelephone_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DataGridYMCATelephone.SelectedIndexChanged
        Try
            Page_Mode = "EDIT"
            PopulateData()
            EnableSaveCancelButtons()
            PopulateDataIntoControls(DataGridYMCATelephone.SelectedItem.Cells(0).Text.Trim)
            SetSelectedImageOfDataGrid(sender, e, "ImageButtonSelect")
        Catch ex As Exception
            'Throw
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = ex.Message.Trim.ToString()
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message + "&FormType=Popup", False)
        End Try
    End Sub

    Private Sub SetSelectedImageOfDataGrid(ByVal sender As System.Object, ByVal e As System.EventArgs, ByVal RadioButtonName As String)
        Dim i As Integer
        Dim dg As DataGrid = CType(sender, DataGrid)
        For i = 0 To dg.Items.Count - 1
            If dg.Items(i).ItemType = ListItemType.AlternatingItem OrElse dg.Items(i).ItemType = ListItemType.Item OrElse dg.Items(i).ItemType = ListItemType.SelectedItem Then
                Dim l_button_Select As ImageButton

                'l_button_Select = CType(DataGridYMCAContact.Items(i).FindControl(RadioButtonName), ImageButton)
                l_button_Select = CType(dg.Items(i).FindControl(RadioButtonName), ImageButton)
                If Not l_button_Select Is Nothing Then
                    If i = dg.SelectedIndex Then
                        l_button_Select.ImageUrl = "images\selected.gif"
                    Else
                        l_button_Select.ImageUrl = "images\select.gif"
                    End If
                End If
            End If
        Next
    End Sub
    Private Sub ButtonCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonCancel.Click
        Try
            Page_Mode = String.Empty
            DisableSaveCancelButtons()
            'Making Readonly True for all TextBoxes
            Dim TelephoneUniqueid As String = String.Empty
            ' Making TextBoxes Blank
            If Not DataGridYMCATelephone.SelectedItem Is Nothing Then
                TelephoneUniqueid = DataGridYMCATelephone.SelectedItem.Cells(0).Text
            End If
            PopulateDataIntoControls(TelephoneUniqueid)
        Catch ex As SqlException
            'Throw
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = ex.Message.Trim.ToString()
            Response.Redirect("ErrorPageForm.aspx?DBMessage=" + l_String_Exception_Message + "&FormType=Popup", False)
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = ex.Message.Trim.ToString()
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message + "&FormType=Popup", False)
            'Throw
        End Try
    End Sub
    Private Sub ButtonOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonOK.Click
        Dim msg As String
        Try
            Dim objPopupAction As PopupResult = New PopupResult
            objPopupAction.Page = "TELEPHONE"
            objPopupAction.Action = PopupResult.ActionTypes.ADD
            objPopupAction.State = Nothing
            Session("PopUpAction") = objPopupAction

            msg = msg + "<Script Language='JavaScript'>"
            msg = msg + "window.opener.document.forms(0).submit();" '"window.opener.location.href=window.opener.location.href;"
            msg = msg + "self.close();"
            msg = msg + "</Script>"
            Response.Write(msg)
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message + "&FormType=Popup", False)
            ' Throw
        Finally
            msg = String.Empty
        End Try
    End Sub
    Private Sub ButtonTelephoneAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonTelephoneAdd.Click
        Try
            'Added by neeraj on 19-Nov-2009 : issue is YRS 5.0-940
            Dim checkSecurity As String = SecurityCheck.Check_Authorization("ButtonTelephoneAdd", Convert.ToInt32(MyBase.Session("LoggedUserKey")))

            If Not checkSecurity.Equals("True") Then
                MessageBox.Show(PlaceHolder1, "YMCA-YRS", checkSecurity, MessageBoxButtons.Stop)
                Exit Sub
            End If
            'End : YRS 5.0-940

            Page_Mode = "ADD"

            EnableSaveCancelButtons()

            Me.CheckBoxActive.Checked = True
            Me.CheckBoxPrimary.Checked = False
            Me.DropdownType.SelectedIndex = IIf(Me.DropdownType.Items.Count >= 4, 3, 0)
            Me.TextBoxTelephone.Text = String.Empty
            Me.TextBoxExt.Text = String.Empty
            Me.TextBoxEffectiveDate.Text = String.Empty
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message + "&FormType=Popup", False)
            'Throw
        End Try
    End Sub
    Private Sub ButtonTelephoneSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonTelephoneSave.Click
        Dim dsAddressInformation As DataSet
        Dim drAddressInformation As DataRow()
        Dim stTelephoneError As String 'PPP | 2015.10.13 | YRS-AT-2588
        Try

            'Added by neeraj on 19-Nov-2009 : issue is YRS 5.0-940
            Dim checkSecurity As String = SecurityCheck.Check_Authorization("ButtonTelephoneSave", Convert.ToInt32(MyBase.Session("LoggedUserKey")))

            If Not checkSecurity.Equals("True") Then
                MessageBox.Show(PlaceHolder1, "YMCA-YRS", checkSecurity, MessageBoxButtons.Stop)
                Exit Sub
            End If
            'End : YRS 5.0-940

            If TextBoxTelephone.Text.Trim = String.Empty Then
                MessageBox.Show(PlaceHolder1, "YMCA", "Please enter a telephone number.", MessageBoxButtons.Stop)
                Exit Sub
            End If

            'BT 813 Validation is being fired at the wrong click event - Telephone
            Dim drPrimary As DataRow()
            drPrimary = g_dataset_dsTelephoneInformation.Tables(0).Select("Primary = 1 And guiUniqueid <> '" & ViewState("TelephoneUniqueId") & "'")

            If drPrimary.Length = 0 AndAlso CheckBoxPrimary.Checked = False Then
                MessageBox.Show(PlaceHolder1, "Please Confirm", "There are no primary status's set. Please update.", MessageBoxButtons.OK)
                EnableSaveCancelButtons()
                Exit Sub
            End If
            'Start:Anudeep: 02.08.2013 - Bt 1555 : YRS 5.0-1769:Length of phone numbers.
            dsAddressInformation = Session("Address Information")
            If HelperFunctions.isNonEmpty(dsAddressInformation) Then
                drAddressInformation = dsAddressInformation.Tables(0).Select("isPrimary = 1 And isActive = 1")
                If drAddressInformation.Length > 0 Then
                    'START: PPP | 2015.10.13 | YRS-AT-2588 | New validation rules applied and also error message is retrieved from AtsMetaMessages
                    'If (drAddressInformation(0)("country") = "US" Or drAddressInformation(0)("country") = "CA") And TextBoxTelephone.Text.Trim.Length <> 10 Then
                    '   MessageBox.Show(PlaceHolder1, "YMCA", "Telephone number must be 10 digits.", MessageBoxButtons.OK)
                    If (drAddressInformation(0)("country") = "US" Or drAddressInformation(0)("country") = "CA") And TextBoxTelephone.Text.Trim.Length > 0 Then
                        stTelephoneError = Validation.Telephone(TextBoxTelephone.Text.Trim, YMCAObjects.TelephoneType.Telephone)
                        If (Not String.IsNullOrEmpty(stTelephoneError)) Then
                            MessageBox.Show(PlaceHolder1, "YMCA", stTelephoneError, MessageBoxButtons.Stop)
                            Exit Sub
                        End If
                    End If
                    'END: PPP | 2015.10.13 | YRS-AT-2588 | New validation rules applied and also error message is retrieved from AtsMetaMessages
                End If
            End If
            'End:Anudeep: 02.08.2013 - Bt 1555 : YRS 5.0-1769:Length of phone numbers.

            'End BT 813

            Dim dr As DataRow
            If Page_Mode = "EDIT" Then
                'BT 814 : Telephone Popup updates the wrong phone number Added ViewState value
                dr = HelperFunctions.GetRowForUpdation(g_dataset_dsTelephoneInformation.Tables(0), "guiUniqueId", Convert.ToString(ViewState("TelephoneUniqueId")).Trim)
                UpdateTelephone(dr)
            ElseIf Page_Mode = "ADD" Then
                dr = g_dataset_dsTelephoneInformation.Tables(0).NewRow()
                UpdateTelephone(dr)
                g_dataset_dsTelephoneInformation.Tables(0).Rows.Add(dr)
            Else
                MessageBox.Show(PlaceHolder1, "YMCA", "Page was found to be in an invalid mode. Please retry the operation after logging out of the application.", MessageBoxButtons.Stop, True)
                Exit Sub
            End If

            DataGridYMCATelephone.SelectedIndex = -1
            PopulateData()
            DisableSaveCancelButtons()

            Page_Mode = String.Empty
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message + "&FormType=Popup", False)
            'Throw
        End Try
    End Sub
    Private Sub SetSaveCancelButtonsEnableTo(ByVal status As Boolean)
        ButtonTelephoneSave.Enabled = status
        ButtonCancel.Enabled = status
        ButtonOK.Enabled = Not status
        ButtonTelephoneAdd.Enabled = Not status
        'Other controls dependent on whether we are in edit mode or not
        DropdownType.Enabled = status
        TextBoxEffectiveDate.Enabled = status
        CheckBoxActive.Enabled = status
        CheckBoxPrimary.Enabled = status
        'added by sanjay to update extension
        TextBoxExt.Enabled = status
        TextBoxTelephone.Enabled = status
    End Sub
    Private Sub EnableSaveCancelButtons()
        SetSaveCancelButtonsEnableTo(True)
    End Sub
    Private Sub DisableSaveCancelButtons()
        SetSaveCancelButtonsEnableTo(False)
    End Sub
#Region "Persistence Mechanism"
    Protected Overrides Sub LoadViewState(ByVal savedState As Object)
        MyBase.LoadViewState(savedState)
        g_dataset_dsTelephoneInformation = DirectCast(Session("Telephone Information"), DataSet)
        Page_Mode = ViewState("Page_Mode")
    End Sub
    Protected Overrides Function SaveViewState() As Object
        ViewState("Page_Mode") = Page_Mode
        Session("Telephone Information") = g_dataset_dsTelephoneInformation
        Return MyBase.SaveViewState()
    End Function
#End Region

#Region "Sorting in Grids"
    Sub SortCommand_OnClick(ByVal Source As Object, ByVal e As DataGridSortCommandEventArgs)
        Try
            Dim dv As New DataView
            Dim SortExpression As String
            SortExpression = e.SortExpression
            Dim dg As DataGrid = DirectCast(Source, DataGrid)

            Dim ds As DataSet
            ds = DirectCast(Session("Telephone Information"), DataSet)

            dv = ds.Tables(0).DefaultView
            If Not ViewState(dg.ID) Is Nothing Then
                If ViewState(dg.ID).ToString.Trim.EndsWith("ASC") Then
                    dv.Sort = "[" + SortExpression + "] DESC"
                Else
                    dv.Sort = "[" + SortExpression + "] ASC"
                End If
            Else
                dv.Sort = "[" + SortExpression + "] ASC"
            End If

            dg.DataSource = Nothing
            dg.DataSource = dv
            dg.DataBind()
            ViewState(dg.ID) = dv.Sort
        Catch ex As Exception
            Throw
            'Dim l_String_Exception_Message As String
            'l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            'Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
    End Sub
#End Region

End Class

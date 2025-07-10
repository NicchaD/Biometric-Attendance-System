'*******************************************************************************
' Copyright YMCA Retirement Fund All Rights Reserved. 
'
' Project Name		:	YMCA-YRS
' FileName			:	YMCAAddressWebForm.aspx.vb
' Author Name		:	Shefali
' Employee ID		:	33488
' Email				:	shefali.bharti@3i-infotech.com
' Contact No		:	8740
' Creation Time		:	5/17/2005 4:49:43 PM
' Program Specification Name	: Doc 3.1.3
' Unit Test Plan Name			:	
' Description					:	This is an addtress pop up window of General Tab
' Changed by			:	Shefali Bharti  
' Changed on			:	30/08/2005
' Change Description	:	Coding
'Cache to Session       :   Ragesh 34231 02/02/06 Cache to Session
'*******************************************************************************
'Name : Preeti Date:6'thFeb06 IssueId:YRST-2046 Reason:When adding a YMCA address the default is "HOME." 2.3.7 has no default.  
'Name : Rahul Date:1'st Mar06 IssueId:YRST-2099 Reason:Cannot add an address to the YMCA Record.
'************************************************************************************
'Modification History
'************************************************************************************
'Modified By        Date            Description
'************************************************************************************
'Ashutosh Patil     20-Mar-2007     YREN-3028, YREN- 3029
'Ashutosh Patil     15-May-2007     For Updating State correctly
'NP/PP/SR			2009.05.18      Optimizing the YMCA Screen
'Priya Jawale       2009.06.03      BT 814 
'Priya Jawale       2009.06.03      BT 813 
'Nikunj Patel       2009.06.09      BT-830
'Neeraj Singh       12/Nov/2009     Added form name for security issue YRS 5.0-940 
'Priya              2010-06-03      Changes made for enhancement in vs-2010 
'Priya				10-Dec-2010:	YRS 5.0-1177/BT 588 Changes made as sate n country fill up with javascript in user control
'Bhavna Shrivastav  18-May-2012     YRS 5.0-1470: Link to Address Edit program from Person Maintenance 
'Anudeep            2013.07.02      Bt-1501:YRS 5.0-1745:Capture Beneficiary addresses 
'Anudeep            2013.08.02      Bt-1683 : YRS 5.0-1862:Add notes record when user enters address in any module.
'Anudeep            2014.09.30      BT:2676 - YRS 5.0-2423:YMCA Address updating not working correctly 
'Shashank Patel     2014.10.13      BT-1995\YRS 5.0-2052: Erroneous updates occuring 
'Manthan Rajguru    2015.09.16      YRS-AT-2550: YRS data cleanup: copyright comments and namespace/reference (Changed the Namespace/reference from 'Infotech' to 'YMCARET')
'*********************************************************************************************************************
Imports System
Imports System.Collections
Imports System.ComponentModel
Imports System.Data
Imports System.Data.SqlClient
Imports System.Web
Imports System.Web.SessionState
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.HtmlControls
Imports System.Data.DataRow
Imports System.Security.Permissions

Public Class YMCAAddressWebForm
    Inherits System.Web.UI.Page
    'below line is Added by Neeraj for issue id YRS 5.0-940 
    Dim strFormName As String = New String("YMCAAddressWebForm.aspx")
    'End issue id YRS 5.0-940
    Protected WithEvents LabelNoRecords As System.Web.UI.WebControls.Label
    Protected WithEvents PlaceHolder1 As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents ReqFldValType As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents LabelEffectiveDate As System.Web.UI.WebControls.Label
    Dim Page_Mode As String 'Stores the mode the page is in currently. If it is in EDIT mode or ADD mode so that corresponding actions can be taken on the click of Button Save
    Dim g_dataset_dsAddressInformation As DataSet

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents ChkPrimary As System.Web.UI.WebControls.CheckBox
    Protected WithEvents ChkActive As System.Web.UI.WebControls.CheckBox
    Protected WithEvents ButtonAddressSave As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonAddressAdd As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonCancel As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonOK As System.Web.UI.WebControls.Button
    Protected WithEvents DataGridYMCAAddress As System.Web.UI.WebControls.DataGrid
    Protected WithEvents DropDownType As System.Web.UI.WebControls.DropDownList
    Protected WithEvents CheckBoxPrimary As System.Web.UI.WebControls.CheckBox
    Protected WithEvents CheckBoxActive As System.Web.UI.WebControls.CheckBox
    Protected WithEvents LabelType As System.Web.UI.WebControls.Label
    Protected WithEvents Button As System.Web.UI.WebControls.Button
    ''Protected WithEvents ButtonEdit As System.Web.UI.WebControls.Button
    Protected WithEvents HiddenText As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HiddenSecControlName As System.Web.UI.HtmlControls.HtmlInputHidden
    'Protected WithEvents TextBoxEffectiveDate As YMCAUI.DateUserControl
    Protected WithEvents AddressWebUserControl1 As YMCAUI.AddressUserControl
    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region
    '1. Define Property 
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
        l_String_FormName = Convert.ToString("YMCAAddressWebForm.aspx")

        ds_AllsecItems = YMCARET.YmcaBusinessObject.ControlSecurityBOClass.GetSecuredControlsOnForm(l_String_FormName)

        If HelperFunctions.isNonEmpty(ds_AllsecItems) Then
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
    End Sub

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.LabelType.AssociatedControlID = Me.DropDownType.ID
        'Me.TextBoxEffectiveDate.RequiredDate = True
        Me.ButtonAddressAdd.Attributes.Add("onclick", "javascript:CheckAccess('ButtonAddressAdd');")
        Me.ButtonAddressSave.Attributes.Add("onclick", "javascript:CheckAccess('ButtonAddressSave');")
        If MyBase.Session("LoggedUserKey") Is Nothing Then
            Response.Redirect("Login.aspx", False)
        End If
        Try
            If Not Me.IsPostBack Then
                If Request.QueryString("UniqueSessionID") IsNot Nothing Then
                    uniqueSessionId = Request.QueryString("UniqueSessionID")
                End If
                g_dataset_dsAddressInformation = Session("Address Information")
                getSecuredControls()
                InitializeControls()
                Me.AddressWebUserControl1.IsPrimary = 1
                Me.AddressWebUserControl1.FromWebAddr = True
                Me.AddressWebUserControl1.EnableControls = False
                DisableSaveCancelButtons()


            Else
                Me.LabelNoRecords.Visible = False
            End If
            'BS:2012.05.16:YRS 5.0-1470: here check if page request is yes then save data in database
            If Session("VerifiedAddress") = "VerifiedAddress" Then
                If Request.Form("Yes") = "Yes" Then
                    Session("VerifiedAddress") = ""
                    AddressSave()
                    Exit Sub
                ElseIf Request.Form("No") = "No" Then
                    Session("VerifiedAddress") = ""
                    Exit Sub
                End If
            End If
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = ex.Message.Trim.ToString()
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message, False)
            'Throw
        End Try
    End Sub

    Public Sub InitializeControls()
        'DropDownStateBind()
        '*********************************
        Dim g_dataset_dsAddressStateType As DataSet
        g_dataset_dsAddressStateType = YMCARET.YmcaBusinessObject.YMCAAddressBOClass.LookUpAddressStateType()
        Dim InsertRowState As DataRow
        InsertRowState = g_dataset_dsAddressStateType.Tables(0).NewRow()
        InsertRowState.Item("chvCodeValue") = String.Empty
        InsertRowState.Item("chvShortDescription") = String.Empty
        If Not g_dataset_dsAddressStateType Is Nothing Then
            g_dataset_dsAddressStateType.Tables(0).Rows.InsertAt(InsertRowState, 0)
        End If

        Dim l_dataset_dsAddressCountry As DataSet
        'l_dataset_dsAddressCountry = YMCARET.YmcaBusinessObject.YMCAAddressBOClass.LookUpAddressCountry()
        l_dataset_dsAddressCountry = Address.GetCountryList()
        If HelperFunctions.isNonEmpty(l_dataset_dsAddressCountry) Then
            Dim InsertRowCountry As DataRow
            InsertRowCountry = l_dataset_dsAddressCountry.Tables(0).NewRow()
            InsertRowCountry.Item("chvAbbrev") = String.Empty
            InsertRowCountry.Item("chvDescription") = String.Empty
            l_dataset_dsAddressCountry.Tables(0).Rows.InsertAt(InsertRowCountry, 0)
        End If

        Dim l_dataset_dsAddressType As DataSet
        l_dataset_dsAddressType = YMCARET.YmcaBusinessObject.YMCAAddressBOClass.LookUpAddressType()
        If HelperFunctions.isNonEmpty(l_dataset_dsAddressType) Then
            Me.DropDownType.DataSource = l_dataset_dsAddressType
            Me.DropDownType.DataMember = "Type"
            Me.DropDownType.DataTextField = "chvShortDescription"
            Me.DropDownType.DataValueField = "chvCodeValue"
            Me.DropDownType.SelectedValue = "OFFICE"
            Me.DropDownType.DataBind()
            'Me.DropDownType.Items.Insert(0, "")
        End If
        PopulateAddressList()
    End Sub

    Private Function PopulateDataIntoControls(ByVal addressId As String) As Boolean
        Dim selectedRows As DataRow()
        Dim selectedRow As DataRow
        Dim l_InsertRowCountry As DataRow
        Dim l_InsertRowState As DataRow

        AddressWebUserControl1.ClearControls = ""    'Clears all the address controls
        Me.DropDownType.SelectedIndex = 0
        'Me.TextBoxEffectiveDate.Text = String.Empty
        Me.CheckBoxActive.Checked = False
        Me.CheckBoxPrimary.Checked = False
        ViewState("AddressUniqueId") = String.Empty
        If HelperFunctions.isEmpty(g_dataset_dsAddressInformation) Then
            Return False
        End If

        'selectedRows = g_dataset_dsAddressInformation.Tables(0).Select("guiUniqueId = '" & addressId & "'")
        'If selectedRows Is Nothing OrElse selectedRows.Length = 0 Then
        '    Return False
        'End If

        'selectedRow = selectedRows(0)

        'DropDownType.SelectedValue = CType(selectedRow("Type"), String).Trim
        'Session("Selected_Type") = DirectCast(selectedRow("Type"), String).Trim
        'If selectedRow.IsNull("Make this Primary") OrElse selectedRow("Make this Primary") = 0 Then
        '    Me.CheckBoxPrimary.Checked = False
        'Else
        '    Me.CheckBoxPrimary.Checked = True
        'End If

        'If selectedRow.IsNull("Active") OrElse selectedRow("Active") = 0 Then
        '    Me.CheckBoxActive.Checked = False
        'Else
        '    Me.CheckBoxActive.Checked = True
        'End If

        'If selectedRow.IsNull("Effective Date") Then
        '    Me.TextBoxEffectiveDate.Text = ""
        'Else
        '    'Me.TextBoxEffectiveDate.Text = CType(selectedRow("Effective Date"), String).Trim
        '    Me.TextBoxEffectiveDate.Text = String.Format("{0:MM/dd/yyyy}", Date.Parse(selectedRow("Effective Date")))
        'End If

        'AddressWebUserControl1.Address1 = selectedRow.Item("Address")
        'AddressWebUserControl1.Address2 = selectedRow.Item("Address 2")
        'AddressWebUserControl1.Address3 = selectedRow.Item("Address 3")
        'AddressWebUserControl1.City = selectedRow("City")
        'If selectedRow("State").ToString().Trim <> String.Empty Then AddressWebUserControl1.DropDownListStateValue = selectedRow("State")
        'If selectedRow("Country").ToString().Trim <> String.Empty Then AddressWebUserControl1.DropDownListCountryValue = selectedRow("Country")
        'If selectedRow.IsNull("Zip") = False Then
        '    AddressWebUserControl1.ZipCode = selectedRow("Zip").ToString().Trim
        '    If selectedRow("Country").ToString().Trim <> String.Empty AndAlso selectedRow("Country") = "CA" Then
        '        AddressWebUserControl1.ZipCode = AddressWebUserControl1.ZipCode.Insert(3, " ")
        '    End If
        'Else
        '    AddressWebUserControl1.ZipCode = String.Empty
        'End If
        'Priya : 14/12/2010:Made changes in adddress user control
        selectedRows = g_dataset_dsAddressInformation.Tables(0).Select("UniqueId = '" & addressId & "'")
        If selectedRows Is Nothing OrElse selectedRows.Length = 0 Then
            Return False
        End If

        selectedRow = selectedRows(0)

        DropDownType.SelectedValue = CType(selectedRow("addrCode"), String).Trim
        Session("Selected_Type") = DirectCast(selectedRow("addrCode"), String).Trim
        If selectedRow.IsNull("isPrimary") OrElse selectedRow("isPrimary") = 0 Then
            Me.CheckBoxPrimary.Checked = False
        Else
            Me.CheckBoxPrimary.Checked = True
        End If

        If selectedRow.IsNull("isActive") OrElse selectedRow("isActive") = 0 Then
            Me.CheckBoxActive.Checked = False
        Else
            Me.CheckBoxActive.Checked = True
        End If

        'If selectedRow.IsNull("effectiveDate") Then
        '    Me.TextBoxEffectiveDate.Text = ""
        'Else
        '    'Me.TextBoxEffectiveDate.Text = CType(selectedRow("Effective Date"), String).Trim
        '    Me.TextBoxEffectiveDate.Text = String.Format("{0:MM/dd/yyyy}", Date.Parse(selectedRow("effectiveDate")))
        'End If

        'AddressWebUserControl1.Address1 = selectedRow.Item("addr1")
        'AddressWebUserControl1.Address2 = selectedRow.Item("addr2")
        'AddressWebUserControl1.Address3 = selectedRow.Item("addr3")
        'AddressWebUserControl1.City = selectedRow("city")
        'If selectedRow("state").ToString().Trim <> String.Empty Then AddressWebUserControl1.DropDownListStateValue = selectedRow("State")
        'If selectedRow("country").ToString().Trim <> String.Empty Then AddressWebUserControl1.DropDownListCountryValue = selectedRow("Country")
        'If selectedRow.IsNull("zipCode") = False Then
        '    AddressWebUserControl1.ZipCode = selectedRow("zipCode").ToString().Trim
        '    If selectedRow("country").ToString().Trim <> String.Empty AndAlso selectedRow("country") = "CA" Then
        '        AddressWebUserControl1.ZipCode = AddressWebUserControl1.ZipCode.Insert(3, " ")
        '    End If
        'Else
        '    AddressWebUserControl1.ZipCode = String.Empty
        'End If
        AddressWebUserControl1.LoadAddressDetail(selectedRows)
        'Me.AddressWebUserControl1.SetValidationsForAddress()

        ViewState("AddressUniqueId") = addressId
        Return True
    End Function
    Public Sub UpdateAddress(ByRef dr As DataRow)
        If Not dr Is Nothing Then
            'If dr.IsNull("UniqueId") Then
            '    dr("UniqueId") = Guid.NewGuid().ToString()
            'End If
            'dr("Type") = Me.DropDownType.SelectedValue
            'dr("Make this Primary") = IIf(Me.CheckBoxPrimary.Checked = False, 0, 1)
            'dr("Active") = IIf(Me.CheckBoxActive.Checked = False, 0, 1)
            'If Me.TextBoxEffectiveDate.Text.Trim.Length = 0 Then
            '    dr("Effective Date") = DBNull.Value
            'Else
            '    dr("Effective Date") = TextBoxEffectiveDate.Text.Trim
            '    'dr("Effective Date") = String.Format("{0:MM/dd/yyyy}", Date.Parse(TextBoxEffectiveDate.Text.Trim))
            'End If


            'dr.Item("Address") = AddressWebUserControl1.Address1.Trim()
            'dr.Item("Address 2") = AddressWebUserControl1.Address2.Trim()
            'dr.Item("Address 3") = AddressWebUserControl1.Address3.Trim()
            'dr("City") = AddressWebUserControl1.City.Trim()
            'If AddressWebUserControl1.DropDownListStateValue = "-Select State-" Then
            '    dr("State") = ""
            'Else
            '    dr("State") = AddressWebUserControl1.DropDownListStateValue
            'End If
            ''dr("State") = AddressWebUserControl1.DropDownListStateValue
            'If AddressWebUserControl1.DropDownListStateValue = "-Select Country-" Then
            '    dr("Country") = ""
            'Else
            '    dr("Country") = AddressWebUserControl1.DropDownListCountryValue
            'End If
            ''dr("Country") = AddressWebUserControl1.DropDownListCountryValue

            'If AddressWebUserControl1.DropDownListCountryValue.ToString = "CA" Then
            '    dr("Zip") = Replace(AddressWebUserControl1.ZipCode, " ", "").Trim()
            'Else
            '    dr("Zip") = AddressWebUserControl1.ZipCode.Trim()
            'End If

            ''If this address is being saved as primary then other addresses are to be marked as non-primary
            'If dr("Make this Primary") = True Then
            '    Dim r As DataRow
            '    For Each r In dr.Table.Rows
            '        If Not r.IsNull("Make this Primary") AndAlso r("Make this Primary") = True _
            '            AndAlso r("guiUniqueId") <> dr("guiUniqueId") Then
            '            r.Item("Make this Primary") = 0
            '        End If
            '    Next
            'End If

            If dr.IsNull("UniqueId") Then
                dr("UniqueId") = Guid.NewGuid().ToString()
            End If
            dr("addrCode") = Me.DropDownType.SelectedValue
            dr("isPrimary") = IIf(Me.CheckBoxPrimary.Checked = False, 0, 1)
            dr("isActive") = IIf(Me.CheckBoxActive.Checked = False, 0, 1)
            If Me.AddressWebUserControl1.EffectiveDate.Trim.Length = 0 Then
                dr("effectiveDate") = DBNull.Value
            Else
                dr("effectiveDate") = AddressWebUserControl1.EffectiveDate.Trim
                'dr("Effective Date") = String.Format("{0:MM/dd/yyyy}", Date.Parse(TextBoxEffectiveDate.Text.Trim))
            End If


            dr.Item("addr1") = AddressWebUserControl1.Address1.Trim()
            dr.Item("addr2") = AddressWebUserControl1.Address2.Trim()
            dr.Item("addr3") = AddressWebUserControl1.Address3.Trim()
            dr("city") = AddressWebUserControl1.City.Trim()
            If AddressWebUserControl1.DropDownListStateValue = "-Select State-" Then
                dr("state") = ""
            Else
                dr("state") = AddressWebUserControl1.DropDownListStateValue
            End If
            'dr("State") = AddressWebUserControl1.DropDownListStateValue
            If AddressWebUserControl1.DropDownListStateValue = "-Select Country-" Then
                dr("country") = ""
            Else
                dr("country") = AddressWebUserControl1.DropDownListCountryValue
            End If
            'dr("Country") = AddressWebUserControl1.DropDownListCountryValue

            If AddressWebUserControl1.DropDownListCountryValue.ToString = "CA" Then
                dr("zipCode") = Replace(AddressWebUserControl1.ZipCode, " ", "").Trim()
            Else
                dr("zipCode") = AddressWebUserControl1.ZipCode.Trim()
            End If

            'If this address is being saved as primary then other addresses are to be marked as non-primary
            If dr("isPrimary") = True Then
                Dim r As DataRow
                For Each r In dr.Table.Rows
                    If Not r.IsNull("isPrimary") AndAlso r("isPrimary") = True _
                        AndAlso r("UniqueId") <> dr("UniqueId") Then
                        r.Item("isPrimary") = 0
                    End If
                Next
            End If
        End If

    End Sub

    Private Sub ButtonOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonOK.Click

        Dim msg As String
        'Dim objAddss As New Address
        Dim objPopupAction As PopupResult = New PopupResult
        Try


            objPopupAction.Page = "ADDRESS"
            objPopupAction.Action = PopupResult.ActionTypes.ADD
            objPopupAction.State = "Address Information"   'objAddss
            Session("PopUpAction") = objPopupAction

            msg = msg + "<Script Language='JavaScript'>"
            msg = msg + "window.opener.document.forms(0).submit(); self.close();"
            msg = msg + "</Script>"
            Response.Write(msg)
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = ex.Message.Trim.ToString()
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message, False)
            'Throw
        End Try
    End Sub
    Private Sub ButtonCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonCancel.Click
        Try
            'Enable / Disable the controls
            DisableSaveCancelButtons()

            Dim addressId As String = String.Empty
            If DataGridYMCAAddress.SelectedIndex > -1 Then
                addressId = DataGridYMCAAddress.SelectedItem.Cells(0).Text()
            End If
            'Repopulate the existing selected item's controls
            PopulateDataIntoControls(addressId)

            Page_Mode = String.Empty
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = ex.Message.Trim.ToString()
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message, False)
            'Throw
        End Try
    End Sub

    Private Sub DataGridYMCAAddress_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DataGridYMCAAddress.SelectedIndexChanged
        Try
            Page_Mode = "EDIT"
            Me.DropDownType.SelectedValue = CType(Session("Selected_Type"), String)
            Me.AddressWebUserControl1.MakeReadonly = False
            Dim addressid As String
            addressid = (CType(sender, DataGrid)).SelectedItem.Cells(0).Text
            If PopulateDataIntoControls(addressid) Then
                EnableSaveCancelButtons()
            Else
                DisableSaveCancelButtons()
            End If
            AddressWebUserControl1.FromWebAddr = True
            'AddressWebUserControl1.GridClicked = True 'AA:30.09.2014 BT:2676-YRS 5.0-2423: Commoneted because this property is not used
            SetSelectedImageOfDataGrid(sender, e, "ImageButtonSelect")
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = ex.Message.Trim.ToString()
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message, False)

            'Throw
        End Try

    End Sub
    Private Sub ButtonAddressAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonAddressAdd.Click
        Try

            'Added by neeraj on 19-Nov-2009 : issue is YRS 5.0-940
            Dim checkSecurity As String = SecurityCheck.Check_Authorization("ButtonAddressAdd", Convert.ToInt32(MyBase.Session("LoggedUserKey")))

            If Not checkSecurity.Equals("True") Then
                MessageBox.Show(PlaceHolder1, "YMCA-YRS", checkSecurity, MessageBoxButtons.Stop)
                Exit Sub
            End If
            'End : YRS 5.0-940

            Page_Mode = "ADD"
            EnableSaveCancelButtons()

            'Initialize controls
            Me.DropDownType.SelectedIndex = IIf(Me.DropDownType.Items.Count >= 3, 2, 0)
            'Me.TextBoxEffectiveDate.Text = String.Empty
            Me.CheckBoxActive.Checked = True
            Me.CheckBoxPrimary.Checked = False

            Me.AddressWebUserControl1.MakeReadonly = False
            Me.AddressWebUserControl1.ClearControls = ""
            'Priya : 14/12/2010:Made changes in adddress user control
            'Me.AddressWebUserControl1.SetValidationsForAddress()

        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = ex.Message.Trim.ToString()
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message, False)

            'Throw
        End Try
    End Sub
    'BS:2012.05.18:YRS 5.0-1470: Create Method
    Private Sub AddressSave()
        If HelperFunctions.isNonEmpty(g_dataset_dsAddressInformation) Then
            Dim drCheck As DataRow()
            'If no primary addresses set then give the following message
            drCheck = g_dataset_dsAddressInformation.Tables(0).Select("isPrimary = 1 AND UniqueId <> '" & Convert.ToString(ViewState("AddressUniqueId")).Trim & "'")
            If drCheck Is Nothing OrElse drCheck.Length = 0 AndAlso CheckBoxPrimary.Checked = False Then
                MessageBox.Show(PlaceHolder1, "Please Confirm", "There are no primary status's set. Please update", MessageBoxButtons.OK)
                Exit Sub
            End If
        Else    'NP:2009.06.09 - Adding check to ensure that if this is the first address it is marked as primary
            If CheckBoxPrimary.Checked = False Then
                MessageBox.Show(PlaceHolder1, "Please Confirm", "There are no primary status's set. Please update", MessageBoxButtons.OK)
                Exit Sub
            End If
        End If

        Dim dr As DataRow
        If Page_Mode = "EDIT" Then
            'Get a datarow and then pass it into the function
            dr = HelperFunctions.GetRowForUpdation(g_dataset_dsAddressInformation.Tables(0), "UniqueId", Convert.ToString(ViewState("AddressUniqueId")).Trim)
            UpdateAddress(dr)
            'Anudeep: 02.08.2013 - Bt1683 : YRS 5.0-1862:Add notes record when user enters address in any module.
            AddNote()
        ElseIf Page_Mode = "ADD" Then
            'Create a new datarow and pass it into the UpdateAddress function
            dr = g_dataset_dsAddressInformation.Tables(0).NewRow()
            UpdateAddress(dr)
            g_dataset_dsAddressInformation.Tables(0).Rows.Add(dr)
            'Anudeep: 02.08.2013 - Bt1683 : YRS 5.0-1862:Add notes record when user enters address in any module.
            AddNote()
        Else
            MessageBox.Show(PlaceHolder1, "YMCA", "Page was found to be in an invalid mode. Please retry the operation after logging out of the application.", MessageBoxButtons.Stop, True)
            Exit Sub
        End If

        DataGridYMCAAddress.SelectedIndex = -1
        PopulateAddressList()

        DisableSaveCancelButtons()

        Page_Mode = String.Empty
    End Sub
    Private Sub ButtonAddressSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonAddressSave.Click
        Try

            'Added by neeraj on 19-Nov-2009 : issue is YRS 5.0-940
            Dim checkSecurity As String = SecurityCheck.Check_Authorization("ButtonAddressSave", Convert.ToInt32(MyBase.Session("LoggedUserKey")))

            If Not checkSecurity.Equals("True") Then
                MessageBox.Show(PlaceHolder1, "YMCA-YRS", checkSecurity, MessageBoxButtons.Stop)
                Exit Sub
            End If
            'End : YRS 5.0-940
            Dim i_str_Message As String = String.Empty
            i_str_Message = AddressWebUserControl1.ValidateAddress()
            If i_str_Message <> "" Then
                MessageBox.Show(PlaceHolder1, "YMCA", i_str_Message, MessageBoxButtons.Stop)
                Exit Sub
            End If
            'BS:2012.05.16:YRS 5.0-1470 :-validate verify address
            Dim i_str_Message1 As String = String.Empty
            i_str_Message1 = AddressWebUserControl1.ValidateAddress()
            If i_str_Message1 <> "" Then
                Session("VerifiedAddress") = "VerifiedAddress"
                MessageBox.Show(PlaceHolder1, "YMCA", i_str_Message1, MessageBoxButtons.YesNo)
                Exit Sub
            End If
            'Priya 10-Dec-2010: YRS 5.0-1177:Changes made as sate n country fill up with javascript in user control
            'If Trim(AddressWebUserControl1.Address1) = "" Then
            '    MessageBox.Show(PlaceHolder1, "YMCA", "Please enter Address1.", MessageBoxButtons.Stop)
            '    Exit Sub
            'End If
            'If Trim(AddressWebUserControl1.City) = "" Then
            '    MessageBox.Show(PlaceHolder1, "YMCA", "Please enter City.", MessageBoxButtons.Stop)
            '    Exit Sub
            'End If
            'If AddressWebUserControl1.DropDownListCountryText = "-Select Country-" And AddressWebUserControl1.DropDownListStateText = "-Select State-" Then
            '    MessageBox.Show(PlaceHolder1, "YMCA", "Please select either State or Country.", MessageBoxButtons.Stop)
            '    Exit Sub
            'End If
            'Dim l_string_Message As String
            'l_string_Message = ValidateCountrySelStateZip(AddressWebUserControl1.DropDownListCountryValue, AddressWebUserControl1.DropDownListStateValue, AddressWebUserControl1.ZipCode)
            'If l_string_Message <> "" Then
            '    MessageBox.Show(PlaceHolder1, "YMCA", l_string_Message, MessageBoxButtons.Stop)
            '    Exit Sub
            'End If
            'If AddressWebUserControl1.DropDownListCountryValue = "CA" And (System.Text.RegularExpressions.Regex.IsMatch(AddressWebUserControl1.ZipCode, "^([a-zA-Z])([0-9])([a-zA-Z]) ([0-9])([a-zA-Z])([0-9])")) = False Then
            '    MessageBox.Show(PlaceHolder1, "YMCA", "Invalid Zip Code format For Country Canada", MessageBoxButtons.Stop)
            '    Exit Sub
            'End If
            'End 10-Dec-2010: YRS 5.0-1177:Changes made as sate n country fill up with javascript in user control
            AddressSave()
        Catch secEx As System.Security.SecurityException
            Response.Redirect("ErrorPageForm.aspx")
        Catch sqlEx As SqlException
            Response.Redirect("ErrorPageForm.aspx")

        End Try
    End Sub

    Private Sub PopulateAddressList()
        If Not g_dataset_dsAddressInformation Is Nothing AndAlso g_dataset_dsAddressInformation.Tables.Count > 0 Then
            DataGridYMCAAddress.DataSource = g_dataset_dsAddressInformation.Tables(0)
        Else
            LabelNoRecords.Visible = True
            DataGridYMCAAddress.DataSource = Nothing
        End If
        DataGridYMCAAddress.DataBind()
    End Sub

    Private Sub DropDownState_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            Dim strValue As String
            strValue = GetDropDownCountrySelValue()

        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = ex.Message.Trim.ToString()
            'Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message, False)
            'Throw
        End Try
    End Sub
    Sub DropDownStateBind()
        'Code added by ashutosh 27-June
        Dim dataset_States As DataSet
        If Session("States") Is Nothing Or Page.IsPostBack = False Then
            'dataset_States = YMCARET.YmcaBusinessObject.YMCAAddressBOClass.GetStates()
            dataset_States = Address.GetState()
            If dataset_States.Tables(0).Rows.Count > 0 Then
                Session("States") = dataset_States
            End If
        End If
    End Sub
    Function GetDropDownCountrySelValue() As String
        Dim dataset_States As DataSet
        Dim datarow_state As DataRow()
        Dim strCountryCode As String
        dataset_States = DirectCast(Session("States"), DataSet)
        If datarow_state.Length > 0 Then
            strCountryCode = datarow_state(0)("chvCountryCode")
        End If
        Return strCountryCode
    End Function

    Private Function ValidateCountrySelStateZip(ByVal CountryValue As String, ByVal StateValue As String, ByVal str_Pri_Zip As String) As String
        '**********************************************************************************************************************
        ' Author            : Ashutosh Patil 
        ' Created On        : 25-Jan-2007
        ' Desc              : This function will validate for the state and Zip Code selected against country USA and Canada
        '                     FOR USA and CANADA - State and Zip Code is mandatory
        ' Related To        : YREN-3029,YREN-3028
        ' Modifed By        : 
        ' Modifed On        :
        ' Reason For Change : 
        '***********************************************************************************************************************
        Dim l_Str_Msg As String = ""

        If (CountryValue = "US" Or CountryValue = "CA") And StateValue = "" Then
            l_Str_Msg = "Please select state"
        ElseIf (CountryValue = "US" Or CountryValue = "CA") And str_Pri_Zip = "" Then
            l_Str_Msg = "Please enter Zip Code"
        ElseIf CountryValue = "US" And (Len(str_Pri_Zip) <> 5 And Len(str_Pri_Zip) <> 9) Then
            l_Str_Msg = "Invalid Zip Code format"
        ElseIf CountryValue = "CA" And (Len(str_Pri_Zip) <> 7) Then
            l_Str_Msg = "Invalid Zip Code format"
        End If
        If CountryValue = "US" And l_Str_Msg <> "" Then
            l_Str_Msg = l_Str_Msg & " for United States"
        ElseIf CountryValue = "CA" And l_Str_Msg <> "" Then
            l_Str_Msg = l_Str_Msg & " for Canada"
        End If
        ValidateCountrySelStateZip = l_Str_Msg
        
    End Function

    Private Sub SetSaveCancelButtonsEnableTo(ByVal status As Boolean)
        ButtonAddressSave.Enabled = status
        ButtonCancel.Enabled = status
        ButtonOK.Enabled = Not status
        ButtonAddressAdd.Enabled = Not status
        'Other controls dependent on whether we are in edit mode or not
        DropDownType.Enabled = status
        'TextBoxEffectiveDate.Enabled = status
        CheckBoxActive.Enabled = status
        CheckBoxPrimary.Enabled = status
        'CheckBoxPrimary.Checked = status
        AddressWebUserControl1.EnableControls = status
        'DataGridYMCAAddress.Enabled = Not status
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
        g_dataset_dsAddressInformation = Session("Address Information")
        Page_Mode = ViewState("Page_Mode")
    End Sub
    Protected Overrides Function SaveViewState() As Object
        ViewState("Page_Mode") = Page_Mode
        Session("Address Information") = g_dataset_dsAddressInformation
        Return MyBase.SaveViewState()
    End Function
#End Region

#Region "Sorting in Grids"

    Sub SortCommand_OnClick(ByVal Source As Object, ByVal e As DataGridSortCommandEventArgs)
        Dim dv As New DataView
        Dim SortExpression As String
        Dim ds As DataSet
        Try
            SortExpression = e.SortExpression
            Dim dg As DataGrid = DirectCast(Source, DataGrid)
            ds = Session("Address Information")
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
        End Try
    End Sub

#End Region

    Private Sub SetSelectedImageOfDataGrid(ByVal sender As System.Object, ByVal e As System.EventArgs, ByVal RadioButtonName As String)
        Try
            Dim i As Integer
            Dim dg As DataGrid = DirectCast(sender, DataGrid)

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
        Catch ex As Exception
            Throw
        End Try
    End Sub
    'Start:Anudeep: 02.08.2013 - Bt1683 : YRS 5.0-1862:Add notes record when user enters address in any module.
    Private Sub AddNote()
        Try

        
        Dim InsertRowNotes As DataRow
        Dim g_DataSetYMCANotes As DataSet
        g_DataSetYMCANotes = Session("YMCA Notes")
        If Not IsNothing(g_DataSetYMCANotes) Then
            InsertRowNotes = g_DataSetYMCANotes.Tables(0).NewRow
            InsertRowNotes.Item("guiUniqueID") = Guid.NewGuid()
            InsertRowNotes.Item("guiEntityID") = Session("GuiUniqueId")
            InsertRowNotes.Item("First Line of Notes") = "Address has been updated "
            InsertRowNotes.Item("Date") = Date.Now.ToShortDateString()
            InsertRowNotes.Item("Creator") = Session("LoginId")
            InsertRowNotes.Item("bitImportant") = 0
            g_DataSetYMCANotes.Tables(0).Rows.Add(InsertRowNotes)
        End If
            Session("YMCA Notes") = g_DataSetYMCANotes
        Catch ex As Exception
            Throw
        End Try
    End Sub
    'ENd:Anudeep: 02.08.2013 - Bt1683 : YRS 5.0-1862:Add notes record when user enters address in any module.
End Class

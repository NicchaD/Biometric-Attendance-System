'*******************************************************************************
' Copyright YMCA Retirement Fund All Rights Reserved. 
'
' Project Name		:	YMCA-YRS
' FileName			:	YMCAEmailWebForm.aspx.vb
' Author Name		:	Shefali
' Employee ID		:	33488
' Email				:	shefali.bharti@3i-infotech.com
' Contact No		:	8740
' Creation Time		:	5/17/2005 4:52:12 PM
' Program Specification Name	:	Doc 3.1.3
' Unit Test Plan Name			:	
' Description					:	This is an email popup window of General Tab
' Changed by			:	Shefali Bharti  
' Changed on			:	01/09/2005
' Change Description	:	Coding
'****************************************************
'Modification History
'****************************************************
'Modified by          Date           Description
'****************************************************
'Ragesh 34231         02/02/06        Cache to Session
'NP/PP/SR             2009.05.18      Optimizing the YMCA Screen
'Sanjay Rawat         2009.06.02      Optimizing the Catch Block
'Neeraj Singh         12/Nov/2009     Added form name for security issue YRS 5.0-940 
'Priya                2010-06-03      Changes made for enhancement in vs-2010 
'Shashank Patel       2014.10.13      BT-1995\YRS 5.0-2052: Erroneous updates occuring 
'Manthan Rajguru      2015.09.16      YRS-AT-2550: YRS data cleanup: copyright comments and namespace/reference (Changed the Namespace/reference from 'Infotech' to 'YMCARET')
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
Public Class YMCAEmailWebForm
    Inherits System.Web.UI.Page
    'below line is Added by Neeraj for issue id YRS 5.0-940 
    Dim strFormName As String = New String("YMCAEmailWebForm.aspx")
    'End issue id YRS 5.0-940
    Protected WithEvents LabelNoRecord As System.Web.UI.WebControls.Label
    Protected WithEvents ButtonAdd As System.Web.UI.WebControls.Button
    Protected WithEvents PlaceHolder1 As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents RequiredFieldValidator1 As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents ValidationSummary1 As System.Web.UI.WebControls.ValidationSummary
    Protected WithEvents RegularExpressionValidator1 As System.Web.UI.WebControls.RegularExpressionValidator
    Protected WithEvents ButtonEdit As System.Web.UI.WebControls.Button
    Dim Page_Mode As String
    Dim g_dataset_dsEmailType As DataSet
    Dim g_dataset_dsEmailInformation As New DataSet

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents DtgYMCATelinfo As System.Web.UI.WebControls.DataGrid
    Protected WithEvents CheckBoxPrimary As System.Web.UI.WebControls.CheckBox
    Protected WithEvents CheckBoxActive As System.Web.UI.WebControls.CheckBox
    Protected WithEvents TextBoxEffectiveDate As YMCAUI.DateUserControl
    Protected WithEvents ButtonSave As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonCancel As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonOk As System.Web.UI.WebControls.Button
    Protected WithEvents DataGridYMCAEmail As System.Web.UI.WebControls.DataGrid
    Protected WithEvents DropDownType As System.Web.UI.WebControls.DropDownList
    Protected WithEvents LabelType As System.Web.UI.WebControls.Label
    Protected WithEvents LabelEffectiveDate As System.Web.UI.WebControls.Label
    Protected WithEvents LabelAddress As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxAddress As System.Web.UI.WebControls.TextBox
    ''Protected WithEvents ButtonEdit As System.Web.UI.WebControls.Button

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
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.LabelType.AssociatedControlID = Me.DropDownType.ID
        Me.TextBoxEffectiveDate.RequiredDate = True
        Me.LabelAddress.AssociatedControlID = Me.TextBoxAddress.ID
        If MyBase.Session("LoggedUserKey") Is Nothing Then
            Response.Redirect("Login.aspx", False)
        End If
        Try
            If Not Me.IsPostBack Then
                If Request.QueryString("UniqueSessionID") IsNot Nothing Then
                    uniqueSessionId = Request.QueryString("UniqueSessionID")
                End If
                g_dataset_dsEmailInformation = Session("Email Information")
                g_dataset_dsEmailType = YMCARET.YmcaBusinessObject.YMCANetBOClass.LookUpEmailType()
                Me.DropDownType.DataSource = g_dataset_dsEmailType
                Me.DropDownType.DataMember = "Email Type"
                Me.DropDownType.DataTextField = "chvShortDescription"
                Me.DropDownType.DataValueField = "chvCodeValue"
                Me.DropDownType.SelectedValue = "OFFICE"
                Me.DropDownType.DataBind()
                DisableSaveCancelButtons()
                PopulateEmailList()
                Me.LabelNoRecord.Visible = False
            Else
                LabelNoRecord.Visible = False
            End If
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = ex.Message.Trim.ToString()
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message, False)

            'Throw
            'Dim l_String_Exception_Message As String
            'l_String_Exception_Message = ex.Message.Trim.ToString()
            'Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
        End Try
    End Sub
    Public Sub PopulateEmailList()
        If Not g_dataset_dsEmailInformation Is Nothing AndAlso g_dataset_dsEmailInformation.Tables.Count > 0 Then
            DataGridYMCAEmail.DataSource = g_dataset_dsEmailInformation.Tables(0)
        Else
            LabelNoRecord.Visible = True
            DataGridYMCAEmail.DataSource = Nothing
        End If
        DataGridYMCAEmail.DataBind()
    End Sub
    Private Sub UpdateEmail(ByVal dr As DataRow)
        If dr Is Nothing Then Exit Sub
        If dr.IsNull("guiUniqueId") Then
            dr("guiUniqueId") = Guid.NewGuid().ToString()
        End If
        dr("Type") = Me.DropDownType.SelectedValue
        dr("Primary") = IIf(CheckBoxPrimary.Checked = False, 0, 1)
        dr("Active") = IIf(CheckBoxActive.Checked = False, 0, 1)
        If Me.TextBoxEffectiveDate.Text.Trim.Length = 0 Then
            dr("Effective Date") = DBNull.Value
        Else
            dr("Effective Date") = TextBoxEffectiveDate.Text.Trim
        End If
        dr("Email Address") = TextBoxAddress.Text.Trim
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
    End Sub

    Private Function PopulateDataIntoControls(ByVal EmailId As String) As Boolean

        'DropDownType.SelectedValue = ""
        Me.DropDownType.SelectedIndex = 0
        CheckBoxPrimary.Checked = False
        CheckBoxActive.Checked = False
        TextBoxEffectiveDate.Text = ""
        TextBoxAddress.Text = ""
        ViewState("MailId") = ""

        If HelperFunctions.isEmpty(g_dataset_dsEmailInformation) Then Return False
        Dim dr As DataRow
        dr = HelperFunctions.GetRowForUpdation(g_dataset_dsEmailInformation.Tables(0), "guiUniqueId", EmailId)
        If dr Is Nothing Then Return False

        Me.DropDownType.SelectedValue = CType(dr("Type"), String).Trim
        Me.CheckBoxPrimary.Checked = IIf(dr.IsNull("Primary") OrElse dr("Primary") = 0, False, True)
        Me.CheckBoxActive.Checked = IIf(dr.IsNull("Active") OrElse dr("Active") = 0, False, True)
        If dr.IsNull("Effective Date") Then
            Me.TextBoxEffectiveDate.Text = "01/01/1900"
        Else
            Me.TextBoxEffectiveDate.Text = String.Format("{0:MM/dd/yyyy}", Date.Parse(dr("Effective Date")))
            'Me.TextBoxEffectiveDate.Text = CType(dr("Effective Date"), String).Trim
        End If
        'address field is a compulsory field
        Me.TextBoxAddress.Text = CType(dr("Email Address"), String).Trim
        ViewState("MailId") = EmailId
        Return True
    End Function
    Private Sub DataGridYMCAEmail_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DataGridYMCAEmail.SelectedIndexChanged
        Try
            If Not DataGridYMCAEmail.SelectedItem Is Nothing Then
                Page_Mode = "EDIT"
                PopulateDataIntoControls(DataGridYMCAEmail.SelectedItem.Cells(0).Text)
                EnableSaveCancelButtons()
            End If
            SetSelectedImageOfDataGrid(sender, e, "ImageButtonSelect")
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = ex.Message.Trim.ToString()
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message, False)

            'Throw
        End Try
    End Sub

    Private Sub ButtonOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonOk.Click
        Dim msg As String
        Try

            Dim objPopupAction As PopupResult = New PopupResult
            objPopupAction.Page = "EMAIL"
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
            l_String_Exception_Message = ex.Message.Trim.ToString()
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message, False)

            'Throw
        Finally
            msg = String.Empty
        End Try
    End Sub
    Private Sub ButtonAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAdd.Click
        Try
            Page_Mode = "ADD"
            EnableSaveCancelButtons()
            Me.DataGridYMCAEmail.SelectedIndex = -1
            Me.DropDownType.SelectedIndex = IIf(Me.DropDownType.Items.Count >= 2, 1, 0)
            Me.TextBoxAddress.Text = String.Empty
            Me.TextBoxEffectiveDate.Text = String.Empty
            Me.CheckBoxActive.Checked = True
            Me.CheckBoxPrimary.Checked = False
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = ex.Message.Trim.ToString()
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message, False)

            'Throw
        End Try
    End Sub
    Private Sub ButtonSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonSave.Click

        Dim drPrimary As DataRow()
        Dim drActive As DataRow()
        Dim dr As DataRow
        Try
            If TextBoxAddress.Text.Trim = String.Empty Then
                MessageBox.Show(PlaceHolder1, "YMCA", "Please enter an email address.", MessageBoxButtons.Stop)
                Exit Sub
            End If

            drPrimary = g_dataset_dsEmailInformation.Tables(0).Select("Primary = 1 and guiUniqueId <> '" & Convert.ToString(ViewState("MailId")).Trim & "'")
            If drPrimary.Length = 0 AndAlso CheckBoxPrimary.Checked = False Then
                MessageBox.Show(PlaceHolder1, "Please Confirm", "There are no primary status's set. Please update.", MessageBoxButtons.OK)
                Exit Sub
            End If

            If Page_Mode = "EDIT" Then
                dr = HelperFunctions.GetRowForUpdation(g_dataset_dsEmailInformation.Tables(0), "guiUniqueId", ViewState("MailId"))
                UpdateEmail(dr)
            ElseIf Page_Mode = "ADD" Then
                'Create a new datarow and pass it into the UpdateEmail function
                dr = g_dataset_dsEmailInformation.Tables(0).NewRow()
                UpdateEmail(dr)
                g_dataset_dsEmailInformation.Tables(0).Rows.Add(dr)
            Else
                MessageBox.Show(PlaceHolder1, "YMCA", "Page was found to be in an invalid mode. Please retry the operation after logging out of the application.", MessageBoxButtons.Stop, True)
                Exit Sub
            End If
            DataGridYMCAEmail.SelectedIndex = -1
            PopulateEmailList()
            DisableSaveCancelButtons()
            Page_Mode = String.Empty

        Catch secEx As System.Security.SecurityException
            'Throw
            'Response.Redirect("ErrorPageForm.aspx")
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = secEx.Message.Trim.ToString()
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message, False)

        Catch ex As Exception
            'Throw
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = ex.Message.Trim.ToString()
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message, False)

            'Response.Redirect("ErrorPageForm.aspx")
        End Try
    End Sub
    Private Sub ButtonCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonCancel.Click
        Try
            'Enable / Disable the controls
            DisableSaveCancelButtons()
            Dim emailId As String = String.Empty
            If DataGridYMCAEmail.SelectedIndex > -1 Then
                emailId = DataGridYMCAEmail.SelectedItem.Cells(0).Text()
            End If
            'Repopulate the existing selected item's controls
            PopulateDataIntoControls(emailId)
            Page_Mode = String.Empty
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = ex.Message.Trim.ToString()
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message, False)

            '    Throw
        End Try
    End Sub
    Private Sub SetSaveCancelButtonsEnableTo(ByVal status As Boolean)
        ButtonSave.Enabled = status
        ButtonCancel.Enabled = status
        ButtonOk.Enabled = Not status
        ButtonAdd.Enabled = Not status
        'Other controls dependent on whether we are in edit mode or not
        DropDownType.Enabled = status
        TextBoxEffectiveDate.Enabled = status
        CheckBoxActive.Enabled = status
        CheckBoxPrimary.Enabled = status
        TextBoxAddress.Enabled = status
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
        g_dataset_dsEmailInformation = Session("Email Information")
        Page_Mode = ViewState("Page_Mode")
    End Sub
    Protected Overrides Function SaveViewState() As Object
        ViewState("Page_Mode") = Page_Mode
        Session("Email Information") = g_dataset_dsEmailInformation
        Return MyBase.SaveViewState()
    End Function
#End Region

#Region "Sorting in Grids"

    Sub SortCommand_OnClick(ByVal Source As Object, ByVal e As DataGridSortCommandEventArgs)

        Dim dv As New DataView
        Dim SortExpression As String
        Try

            SortExpression = e.SortExpression
            Dim dg As DataGrid = DirectCast(Source, DataGrid)

            Dim ds As DataSet
            ds = Session("Email Information")


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
        Dim i As Integer
        Dim dg As DataGrid = DirectCast(sender, DataGrid)
        Try

            For i = 0 To dg.Items.Count - 1
                If dg.Items(i).ItemType = ListItemType.AlternatingItem OrElse dg.Items(i).ItemType = ListItemType.Item OrElse dg.Items(i).ItemType = ListItemType.SelectedItem Then
                    Dim l_button_Select As ImageButton
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
End Class

'*******************************************************************************
' Copyright YMCA Retirement Fund All Rights Reserved. 
' Project Name		:	YMCA YRS
' FileName			:	AddContactWebForm.aspx.vb
' Author Name		:	
' Employee ID		:	
' Email			    :	
' Contact No		:	
' Creation Time	    :	
' Program Specification Name	:	
' Unit Test Plan Name			:	
' Description					:	For adding contacts under a YMCA, Called in the YMCAWebForm.aspx.vb
'******************************************************************************* 
' Changed by        Changed on      Change Description
'******************************************************************************* 
' Vipul             02Feb06         Cache-Session
' Mohammed Hafiz    17Nov06         YREN-2884
'Shubhrata          28Nov07         YREN-4016 (To add Extn No Field)
'NP/PP/SR           2009.05.18      Optimizing the YMCA Screen
'Sanjay Rawat       2009.06.02      Optimizing the Catch Block
'Nikunj Patel       2009.06.09      BT-831 Removing the label message and replaced with Required field validator in save routine
'Neeraj Singh       12/Nov/2009     Added form name for security issue YRS 5.0-940 
'Shashi Shekhar Singh2010.03.04     Changes for  issue  YRS 5.0-942
'Shashi Shekhar     2010-04-13      Changes made for Gemini-1051
'Shashi Shekhar     2010-06-07      Merge the SearchContact screen with Add contact screen and optimize the code for YRS 5.0-1080.
'Shashi Shekhar Singh:11-june-2010  :BT-538
'Priya              2010-06-03      Changes made for enhancement in vs-2010 
'Shashi Shekhar     2010-07-19      Integrate Issue fixes and new features which was released in Maintenance VP5 code ( YRS 5.0-1080.)
'prasad Jadhav      2011.11.01      For BT-909, YRS 5.0-1379 : New job position field in atsYmcaContacts
'prasad jadhav	    2012.01.06      For BT-971, YRS 5.0-1503 : Replace job position code with job postion description on ymca contacts
'Prasad jadhav	    2012.01.20      For BT-979, YRS 5.0-1521 : Allow all job titles in the YMCA Contacts dropdown selection
'Shashank Patel     2014.10.13      BT-1995\YRS 5.0-2052: Erroneous updates occuring 
'Anudeep A          2015.02.10      BT:2738:YRS 5.0-2456:YERDI3I-2319: YERDI YMCA Officer Information Update function - YRS Changes
'Anudeep A          2015.02.12      BT:2738:YRS 5.0-2456:YERDI3I-2319: YERDI YMCA Officer Information Update function - YRS Changes
'Anudeep A          2015.06.19      BT:2738:YRS 5.0-2456:YERDI3I-2319: YERDI YMCA Officer Information Update function - YRS Changes
'Manthan Rajguru    2015.09.16      YRS-AT-2550: YRS data cleanup: copyright comments and namespace/reference (Changed the Namespace/reference from 'Infotech' to 'YMCARET')
'Pramod P. Pokale   2015.10.13      YRS-AT-2588: implement some basic telephone number validation Rules
'Manthan Rajguru    2016.06.23      YRS-AT-2959 -  YRS Bug: Maintenance - YMCA: cannot update "Contact" tab email if name has apostrophe
'Santosh Bura       2018.05.23      YRS-AT-2818 - YRS enh: -in YMCA Maintenance, do not allow more than one LPA contact (TrackIT 25125)
'Manthan Rajguru    2018.06.04      YRS-AT-3961 -  YRS enh: add new Contact Type for YMCAs (TrackIT 33514) 
'******************************************************************************* 

#Region " Namespaces "
Imports System.Data
Imports System.Data.SqlClient
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
#End Region

Public Class AddContactWebForm
    Inherits System.Web.UI.Page

#Region " Global variable declaration "
    Dim g_bool_AddFlagContact As Boolean
    Dim Page_Mode As String = String.Empty
    Dim g_dataset_dsContactType As DataSet
    Dim g_dataset_dsTitle As DataSet
    'Added by prasad for YRS 5.0-1379 : New job position field in atsYmcaContacts
    Dim InsertRowTitle As DataRow
    Dim InsertRow As DataRow
    'below line is Added by Neeraj for issue id YRS 5.0-940 
    Dim strFormName As String = New String("AddContactWebForm.aspx")
    Dim g_dataset_dsContact As DataSet
    Dim g_integer_count As Integer
    Dim l_int_SelectedDataGridItem As Integer   'Tracks the selected index of the Search Datagrid
#End Region

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents LabelFundNo As System.Web.UI.WebControls.Label
    Protected WithEvents TextboxFundNo As System.Web.UI.WebControls.TextBox
    Protected WithEvents ButtonUnlink As System.Web.UI.WebControls.Button
    Protected WithEvents LabelType As System.Web.UI.WebControls.Label
    Protected WithEvents DropDownListType As System.Web.UI.WebControls.DropDownList
    'prasad Jadhav  For BT-909, YRS 5.0-1379 : New job position field in atsYmcaContacts
    Protected WithEvents DropDownListTitle As System.Web.UI.WebControls.DropDownList
    Protected WithEvents RequiredFieldValidator4 As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents LabelFname As System.Web.UI.WebControls.Label
    Protected WithEvents TextboxFname As System.Web.UI.WebControls.TextBox
    Protected WithEvents Requiredfieldvalidator3 As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents LabelMname As System.Web.UI.WebControls.Label
    Protected WithEvents TextboxMname As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelLname As System.Web.UI.WebControls.Label
    Protected WithEvents TextboxLname As System.Web.UI.WebControls.TextBox
    Protected WithEvents Requiredfieldvalidator6 As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents LabelTelephone As System.Web.UI.WebControls.Label
    Protected WithEvents TextboxTelephone As System.Web.UI.WebControls.TextBox
    Protected WithEvents RequiredFieldValidator2 As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents RegularExpressionValidator1 As System.Web.UI.WebControls.RegularExpressionValidator
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    Protected WithEvents TextboxExtnNo As System.Web.UI.WebControls.TextBox
    Protected WithEvents Regularexpressionvalidator2 As System.Web.UI.WebControls.RegularExpressionValidator
    Protected WithEvents LabelContactsEmail As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxContactsEmail As System.Web.UI.WebControls.TextBox
    Protected WithEvents ValidateContactsEmail As System.Web.UI.WebControls.RegularExpressionValidator
    Protected WithEvents LabelEffectiveDate As System.Web.UI.WebControls.Label
    Protected WithEvents LabelContactNotes As System.Web.UI.WebControls.Label
    Protected WithEvents TextboxContactNotes As System.Web.UI.WebControls.TextBox
    Protected WithEvents ValidationSummary1 As System.Web.UI.WebControls.ValidationSummary
    Protected WithEvents ButtonOK As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonCancel As System.Web.UI.WebControls.Button
    Protected WithEvents PanelAddContact As System.Web.UI.WebControls.Panel
    Protected WithEvents Textbox1 As System.Web.UI.WebControls.TextBox
    Protected WithEvents Button1 As System.Web.UI.WebControls.Button
    Protected WithEvents PanelSearchContact As System.Web.UI.WebControls.Panel
    Protected WithEvents TextboxEffectiveDate As YMCAUI.DateUserControl
    Protected WithEvents ButtonContact As System.Web.UI.WebControls.Button 'Shashi Shekhar:2010-03-04
    Protected WithEvents LabelExtnNo As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxName As System.Web.UI.WebControls.TextBox
    Protected WithEvents PlaceHolder1 As System.Web.UI.WebControls.PlaceHolder 'Shashi Shekhar:For YRS-5.0-942

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    'Search Contact Screen Controls
    Protected WithEvents dg As System.Web.UI.WebControls.DataGrid
    Protected WithEvents ButtonFind As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonClear As System.Web.UI.WebControls.Button
    Protected WithEvents LabelListSSNo As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxListSSNo As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelListFundNo As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxListFundNo As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelLastName As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxLastName As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelFirstName As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxFirstName As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelSSNo As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxSSNo As System.Web.UI.WebControls.TextBox
    Protected WithEvents MessageBoxPlaceHolder As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents NotesFlag As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents ButtonCancelSearch As System.Web.UI.WebControls.Button
    Protected WithEvents RefundMultiPage As Microsoft.Web.UI.WebControls.MultiPage
    Protected WithEvents LabelNoRecord As System.Web.UI.WebControls.Label
    Protected WithEvents LabelNoneRecord As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_Search_MoreItems As System.Web.UI.WebControls.Label




    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    'SP 2014.10.13 BT-1995\YRS 5.0-2052: Erroneous updates occuring 
#Region "Properties"

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
#End Region
    'SP 2014.10.13 BT-1995\YRS 5.0-2052: Erroneous updates occuring 

#Region " Page_Load "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here

        Dim l_DatasetYMCAList As DataSet
        Dim l_ds_datasetContact As DataSet
        Dim l_DataTableContact As DataTable
        Dim l_DataRow As DataRow
        TextboxEffectiveDate.RequiredValidatorErrorMessage = "Effective Date cannot be blank" 'MMR | 2018.06.04 | YRS-AT-3961 | Replaced existing error message of date user control 
        If MyBase.Session("LoggedUserKey") Is Nothing Then
            Response.Redirect("Login.aspx", False)
        End If
        Me.TextboxEffectiveDate.RequiredDate = True
        Me.TextboxTelephone.Attributes.Add("onkeypress", "javascript:OnSpace();")
        Me.TextboxExtnNo.Attributes.Add("onkeypress", "javascript:OnSpace();")
        Try

            If IsPostBack AndAlso HelperFunctions.isNonEmpty(g_dataset_dsContact) Then
                'Shashi Shekhar Singh:11-june-2010:BT-538
                If Not g_dataset_dsContact.Tables(0).Columns.Contains("City") Then
                    RemoveDataGridCol()
                End If
                dg.DataSource = g_dataset_dsContact.Tables(0).DefaultView
                dg.SelectedIndex = l_int_SelectedDataGridItem
                dg.DataBind()
            End If

            If Not Me.IsPostBack Then
                Session("AC_g_dataset_dsContact") = Nothing

                Me.TextboxFundNo.Enabled = False 'Shashi Shekhar:Yrs-5.0-942 
                FillDropDown()

                'Added by sanjay 
                Dim ContactId As String
                If Not Request("ContactId") Is Nothing Then
                    PanelSearchContact.Visible = False
                    ContactId = Request("ContactId").Trim()
                Else
                    PanelAddContact.Visible = False
                End If

                Dim C As Contacts = GetContactsById(ContactId)
                If C Is Nothing Then
                    Throw New Exception("Invalid Contact Id passed. Please close this window and try again.")
                End If
                'commented by prasad wrong value is selected while updating
                'Me.DropDownListType.SelectedValue = C.Type.ToUpper().Trim
                Me.DropDownListType.SelectedValue = C.Type.Trim
                'Added by prasad YRS 5.0-1379 : New job position field in atsYmcaContacts
                If DropDownListTitle.Items.FindByValue(C.Title.Trim) IsNot Nothing Then
                    Me.DropDownListTitle.SelectedValue = C.Title
                Else
                    Me.DropDownListTitle.SelectedValue = String.Empty
                End If
                Me.TextBoxName.Text = C.Name.Trim
                Me.TextboxTelephone.Text = C.Telephone.Trim
                Me.TextboxEffectiveDate.Text = C.EffectiveDate.Trim
                Me.TextBoxContactsEmail.Text = C.Email.Trim
                Me.TextboxContactNotes.Text = C.Note.Trim
                Me.TextboxExtnNo.Text = C.ExtNo.Trim

                'Shashi Shekhar:08-Mar-2010:For YRS-5.0-942
                If (C.FundNo = Nothing Or C.FundNo = String.Empty) Then
                    Me.TextboxFundNo.Text = String.Empty
                Else
                    Me.TextboxFundNo.Text = C.FundNo.Trim
                End If

                Me.TextboxFname.Text = C.FirstName.Trim
                Me.TextboxMname.Text = C.MiddleName.Trim
                Me.TextboxLname.Text = C.LastName.Trim

                '-----------------------------------------------

                If ContactId Is Nothing OrElse ContactId = String.Empty Then
                    Page_Mode = "ADD"
                    Me.ButtonUnlink.Visible = False
                Else
                    Page_Mode = "UPDATE"
                    'Shashi Shekhar:2010-04-13:Commented-For Gemini-1051
                    If (C.FundNo = Nothing Or C.FundNo = String.Empty) Then
                        Me.TextboxFname.Enabled = True
                        Me.TextboxMname.Enabled = True
                        Me.TextboxLname.Enabled = True
                        Me.ButtonUnlink.Visible = False
                        Me.ButtonContact.Visible = True
                    Else
                        Me.TextboxFname.Enabled = False
                        Me.TextboxMname.Enabled = False
                        Me.TextboxLname.Enabled = False
                        Me.TextboxFundNo.Enabled = False
                        Me.ButtonUnlink.Visible = True
                        Me.ButtonContact.Visible = False

                    End If
                End If

            End If

        Catch ex As SqlException
            ' Throw
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message, False)
            'Throw
            'Dim l_String_Exception_Message As String
            'l_String_Exception_Message = ex.Message.Trim.ToString()
            'Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)

        End Try
    End Sub

   
#End Region

#Region " Private Functions "

    Private Function GetContactsById(ByVal contactId As String) As Contacts
        Dim n As Contacts
        If contactId = Nothing OrElse contactId = String.Empty Then
            n = New Contacts
            n.EffectiveDate = String.Empty
            n.Email = String.Empty
            n.Name = String.Empty
            n.ContactId = String.Empty
            n.Telephone = String.Empty
            n.ExtNo = String.Empty
            n.Note = String.Empty
            n.Type = String.Empty

            '------Shashi Shekhar:2010-03-08:YRS-5.0-942----------
            n.FundNo = String.Empty
            n.FirstName = String.Empty
            n.MiddleName = String.Empty
            n.LastName = String.Empty
            '----------------------------------------------------
            'prasad Jadhav  For BT-909, YRS 5.0-1379 : New job position field in atsYmcaContacts
            n.Title = String.Empty
            Return n
        End If
        If Session("YMCA Contact") Is Nothing Then Return Nothing
        Dim ds As DataSet = DirectCast(Session("YMCA Contact"), DataSet)
        If HelperFunctions.isEmpty(ds) Then Return Nothing
        Dim dr As DataRow()
        dr = ds.Tables(0).Select("guiUniqueId = '" & contactId & "'")
        If dr Is Nothing OrElse dr.Length = 0 Then Return Nothing
        n = New Contacts
        n.EffectiveDate = IIf(dr(0).IsNull("Effective Date"), String.Empty, String.Format("{0:MM/dd/yyyy}", Date.Parse(dr(0).Item("Effective Date"))))
        n.Email = IIf(dr(0).IsNull("Email"), String.Empty, dr(0).Item("Email"))
        n.Name = IIf(dr(0).IsNull("Contact Name"), String.Empty, dr(0).Item("Contact Name"))
        n.Telephone = IIf(dr(0).IsNull("Phone No"), String.Empty, dr(0).Item("Phone No"))
        n.ExtNo = IIf(dr(0).IsNull("Extn No"), String.Empty, dr(0).Item("Extn No"))
        n.Note = IIf(dr(0).IsNull("ContactNotes"), String.Empty, dr(0).Item("ContactNotes"))
        'prasad Jadhav  For BT-909, YRS 5.0-1379 : New job position field in atsYmcaContacts
        n.Type = IIf(dr(0).IsNull("TypeCode"), String.Empty, dr(0).Item("TypeCode"))

        '------------'Shashi Shekhar:2010-03-08:YRS-5.0-942---------------------------------------------
        n.FundNo = IIf((dr(0).IsNull("Fund No")), String.Empty, dr(0).Item("fund No"))
        n.FirstName = IIf((dr(0).IsNull("First Name")), String.Empty, dr(0).Item("First Name"))
        n.MiddleName = IIf((dr(0).IsNull("Middle Name")), String.Empty, dr(0).Item("Middle Name"))
        n.LastName = IIf((dr(0).IsNull("Last Name")), String.Empty, dr(0).Item("Last Name"))
        'prasad Jadhav  For BT-909, YRS 5.0-1379 : New job position field in atsYmcaContacts
		n.Title = IIf((dr(0).IsNull("Title")), String.Empty, dr(0).Item("Title"))
        '--------------------------------------------------------
        n.EffectiveDate = n.EffectiveDate.Trim
        n.Email = n.Email.Trim
        n.Name = n.Name.Trim
        n.Telephone = n.Telephone.Trim
        n.ExtNo = n.ExtNo.Trim
        n.Note = n.Note.Trim
        n.Type = n.Type.Trim
        '---------------'Shashi Shekhar:2010-03-08:YRS-5.0-942--------------
        n.FundNo = n.FundNo
        n.FirstName = n.FirstName.Trim
        n.MiddleName = n.MiddleName.Trim
        n.LastName = n.LastName.Trim
        'prasad Jadhav  For BT-909, YRS 5.0-1379 : New job position field in atsYmcaContacts
        n.Title = n.Title.Trim
        '------------------------------------------------------
        Return n
    End Function

    Private Function ClearControls()
        Try
            '----------------------------------------------------
            'Shashi Shekhar:2010-03-04
            Me.TextboxFname.Text = String.Empty
            Me.TextboxLname.Text = String.Empty
            Me.TextboxMname.Text = String.Empty
            Me.TextboxFundNo.Text = String.Empty
            Me.TextboxTelephone.Text = String.Empty
            Me.DropDownListType.SelectedValue = String.Empty
            Me.TextBoxContactsEmail.Text = String.Empty
            Me.TextboxContactNotes.Text = String.Empty
            Me.TextboxExtnNo.Text = String.Empty
            '--------------------------------------------------------
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
        End Try

    End Function

    Private Function ClearSearchControls()
        Try
            dg.Visible = False
            Me.TextBoxListFundNo.Text = String.Empty
            Me.TextBoxLastName.Text = String.Empty
            Me.TextBoxFirstName.Text = String.Empty
            Me.LabelNoneRecord.Visible = False
            Me.LabelNoRecord.Visible = False
            Me.lbl_Search_MoreItems.Visible = False
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
        End Try

    End Function

    Private Function FillDropDown()
        Try
            '----------------------------------------------------
            'Shashi Shekhar:2010-03-04
            g_dataset_dsContactType = YMCARET.YmcaBusinessObject.AddContactBOClass.LookUpContactType()
            'adding an empty row dynamically
            InsertRow = g_dataset_dsContactType.Tables(0).NewRow()
            InsertRow.Item("chvCodeValue") = String.Empty
            InsertRow.Item("chvShortDescription") = String.Empty
            g_dataset_dsContactType.Tables(0).Rows.Add(InsertRow)
            Me.DropDownListType.DataSource = g_dataset_dsContactType
            Me.DropDownListType.DataMember = "Contact Type"
            Me.DropDownListType.DataTextField = "chvShortDescription"
            Me.DropDownListType.DataValueField = "chvCodeValue"
            Me.DropDownListType.DataBind()
            '--------------------------------------------------------
			'Added by prasad for YRS 5.0-1379 : New job position field in atsYmcaContacts
			'Commented by prasad For BT-979, YRS 5.0-1521 : Allow all job titles in the YMCA Contacts dropdown selection
			g_dataset_dsTitle = YMCARET.YmcaBusinessObject.SelectTitleBOClass.LookUpTitle()
			InsertRowTitle = g_dataset_dsTitle.Tables(0).NewRow()
            InsertRowTitle.Item("chrPositionType") = String.Empty
			InsertRowTitle.Item("Name") = String.Empty
			g_dataset_dsTitle.Tables(0).Rows.Add(InsertRowTitle)
			Me.DropDownListTitle.DataSource = g_dataset_dsTitle.Tables(0)
            Me.DropDownListTitle.DataMember = "Title"
            Me.DropDownListTitle.DataTextField = "Name"
            Me.DropDownListTitle.DataValueField = "chrPositionType"
            Me.DropDownListTitle.DataBind()
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message, False)

            'Throw
            'Dim l_String_Exception_Message As String
            'l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            'Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
        End Try

    End Function

    Private Function AssignValueToObject()
        '----------------------------------------------------
        'Shashi Shekhar:2010-03-04
        Dim msg As String
        Dim objcontacts As New Contacts
        msg = ""
        Dim strName As String = ""
        Try
            'Shashi Shekhar:2010-03-03:For YRS-5.0-942
            'Start:AA:02.10.2015 BT:2738:YRS 5.0-2456:YERDI3I-2319: Added for not to add unnecassary spaces in database for name column
            'objcontacts.Name = Me.TextboxFname.Text.Trim + " " + Me.TextboxMname.Text + " " + Me.TextboxLname.Text
            strName = HelperFunctions.GetFullName(Me.TextboxFname.Text.Trim, Me.TextboxMname.Text, Me.TextboxLname.Text)
            objcontacts.Name = strName
            'End:AA:02.10.2015 BT:2738:YRS 5.0-2456:YERDI3I-2319: Added for not to add unnecassary spaces in database for name column
            objcontacts.FundNo = IIf(IsDBNull(Me.TextboxFundNo.Text.Trim), String.Empty, Me.TextboxFundNo.Text.Trim)
            objcontacts.FirstName = IIf(IsDBNull(Me.TextboxFname.Text.Trim), String.Empty, Me.TextboxFname.Text.Trim)
            objcontacts.MiddleName = IIf(IsDBNull(Me.TextboxMname.Text.Trim), String.Empty, Me.TextboxMname.Text.Trim)
            objcontacts.LastName = IIf(IsDBNull(Me.TextboxLname.Text.Trim), String.Empty, Me.TextboxLname.Text.Trim)
            objcontacts.Type = Me.DropDownListType.SelectedValue
            objcontacts.Telephone = Me.TextboxTelephone.Text.Trim
            objcontacts.ExtNo = Me.TextboxExtnNo.Text.Trim
            objcontacts.EffectiveDate = Me.TextboxEffectiveDate.Text.Trim
            objcontacts.Email = Me.TextBoxContactsEmail.Text.Trim
            objcontacts.Note = Me.TextboxContactNotes.Text.Trim
            'prasad:2011-10-31 for YRS 5.0-1379 : New job position field in atsYmcaContacts
            objcontacts.Title = Me.DropDownListTitle.SelectedValue
            'prasad:2012-01-06 for YRS 5.0-1503 : Replace job position code with job postion description on ymca contacts
            objcontacts.TitleDescription = Me.DropDownListTitle.SelectedItem.ToString
            objcontacts.TypeValue = Me.DropDownListType.SelectedItem.ToString
            If Page_Mode = "ADD" Then
                objcontacts.ContactId = Guid.NewGuid().ToString()
            Else
                objcontacts.ContactId = Request("ContactId").Trim()
            End If

            Dim objPopupAction As PopupResult = New PopupResult
            objPopupAction.Page = "CONTACTS"
            objPopupAction.Action = PopupResult.ActionTypes.ADD
            objPopupAction.State = objcontacts
            Session("PopUpAction") = objPopupAction

            msg = msg + "<Script Language='JavaScript'>"
            msg = msg + "window.opener.document.forms(0).submit();" '"window.opener.location.href=window.opener.location.href;"
            msg = msg + "self.close();"
            msg = msg + "</Script>"
            Response.Write(msg)

        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
        End Try
        '--------------------------------------------------------
    End Function

    Private Function SearchContact(ByVal parameterFundNo As String, ByVal paramterLastName As String, ByVal parameterFirstName As String)
        Dim l_DataSet As DataSet
        Try
            g_dataset_dsContact = YMCARET.YmcaBusinessObject.AddContactBOClass.LookUpContact(parameterFundNo, paramterLastName, parameterFirstName, Session("GeneralYMCANo"))
            If (Not IsNothing(g_dataset_dsContact)) Then
                If g_dataset_dsContact.Tables(0).Rows.Count = 0 Then
                    'No records found for either First or Second phase search so display message "No records found"
                    LabelNoneRecord.Visible = False
                    lbl_Search_MoreItems.Visible = False
                    Me.LabelNoRecord.Visible = True
                    'Shashi Shekhar:2010-04-12:For BT-499
                    dg.DataSource = Nothing
                    dg.DataBind()

                Else 'Records found -- below is check for records belonging to which phase search result.

                    If g_dataset_dsContact.Tables(0).Columns.Contains("City") Then
                        'If Datatable contains "City" column name and record count of datatable is more than 0 then 
                        'Grid is bind with second phase search which include three more column to display i.e(City,State,Fund status).
                        LabelNoneRecord.Visible = True
                        Me.LabelNoRecord.Visible = False    'Shashi Shekhar:2010-04-12:For BT-499

                        If dg.Columns.Count <= 5 Then
                            AddDataGridCol()
                        End If

                        dg.DataSource = g_dataset_dsContact
                        dg.DataBind()

                        If dg.Items.Count > dg.PageSize Then
                            lbl_Search_MoreItems.Visible = True
                        Else
                            lbl_Search_MoreItems.Visible = False
                        End If
                    Else 'First Phase search is bind with grid

                        If dg.Columns.Count > 5 Then
                            RemoveDataGridCol()
                        End If

                        LabelNoneRecord.Visible = False
                        lbl_Search_MoreItems.Visible = False
                        Me.LabelNoRecord.Visible = False 'Shashi Shekhar:2010-04-12:For BT-499
                        dg.DataSource = g_dataset_dsContact
                        dg.DataBind()
                    End If

                End If
            End If
        Catch ex As Exception
            MessageBox.Show(Me.MessageBoxPlaceHolder, " YMCA - YRS", ex.Message, MessageBoxButtons.OK, False)
        End Try

    End Function

    'If Data found in First phase search, then remove extra column from grid which are used for second phase search.
    Private Function RemoveDataGridCol()
        Try
            For i As Integer = dg.Columns.Count - 1 To 0 Step -1
                If dg.Columns(i).HeaderText = "City" Or dg.Columns(i).HeaderText = "State" Or dg.Columns(i).HeaderText = "Fund Status" Then
                    dg.Columns.RemoveAt(i)
                End If
            Next
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
        End Try
    End Function

    Private Function AddDataGridCol()
        Try
            Dim bc As New BoundColumn
            bc.HeaderText = "City"
            bc.DataField = "City"
            bc.SortExpression = "City"
            dg.Columns.Add(bc)
            Dim bc1 As New BoundColumn
            bc1.HeaderText = "State"
            bc1.DataField = "States"
            bc1.SortExpression = "States"
            dg.Columns.Add(bc1)
            Dim bc2 As New BoundColumn
            bc2.HeaderText = "Fund Status"
            bc2.DataField = "FundStatus"
            bc2.SortExpression = "FundStatus"
            dg.Columns.Add(bc2)
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
        End Try

    End Function
    'Start:AA:2015.02.10 BT:2738:YRS 5.0-2456:YERDI3I-2319: 
    Private Function ValidateContact() As Boolean
        Dim dsContact As DataSet = CType(Session("YMCA Contact"), DataSet)
        Dim drContact As DataRow()
        Dim transmContactRow As DataRow()     'SB | 2018.05.23 | YRS-AT-2818 | Added new data row for validation of LPA purpose.
        Dim strName As String = ""
        Dim stTelephoneError As String 'PPP | 2015.10.13 | YRS-AT-2588
        Try
            'Added Validation for not to create multiple participants with same fund no with OTHER type if already contact exists for the TRANSM or ACH DB
            'Added Validation for not to create multiple participants with same name with OTHER type if already contact exists for the TRANSM or ACH DB
            'Added Validation for not to create multiple participants with same fund no with TRANSM or ACH DB type if already contact exists for the  OTHER 
            'Added Validation for not to create multiple participants with same name with TRANSM or ACH DB type if already contact exists for the OTHER
            If DropDownListType.SelectedValue = "OTHER" OrElse DropDownListType.SelectedValue = "OTHPOC" Then 'MMR | 2018.06.04 | YRS-AT-3961 | Added validation to check 'Other POC' contact type 
                If HelperFunctions.isNonEmpty(dsContact) Then
                    If TextboxFundNo.Text.Trim <> "" Then
                        'Start AA:06.16.2015 BT:2738:YRS 5.0-2456:YERDI3I-2319: Added below code to validate contacts already a contact exists with 'TRANSM' or 'ACH DR' but not current one
                        If Page_Mode = "UPDATE" Then
                            drContact = dsContact.Tables(0).Select("[Fund No] = '" + TextboxFundNo.Text + "' AND [TypeCode] IN ('TRANSM','ACH DR') AND guiUniqueId <> '" + Request("ContactId") + "'")
                        Else
                            drContact = dsContact.Tables(0).Select("[Fund No] = '" + TextboxFundNo.Text + "' AND [TypeCode] IN ('TRANSM','ACH DR')")
                        End If
                        'END AA:06.16.2015 BT:2738:YRS 5.0-2456:YERDI3I-2319: Added below code to validate contacts already a contact exists with 'TRANSM' or 'ACH DR' but not current one
                        If drContact IsNot Nothing AndAlso (drContact.Length > 0) Then
                            'START: MMR | 2018.06.04 | YRS-AT-3961 | Replaced hardcoded position type 'OTHER' with dynamic contact type selcted from dropdown 
                            'MessageBox.Show(PlaceHolder1, "YMCA-YRS", "This person (Fund No. " + TextboxFundNo.Text + ") is already listed as Local Plan Admin (TRANSM) / ACH Debit. The same person cannot be added with the position of OTHER.", MessageBoxButtons.Stop)
                            MessageBox.Show(PlaceHolder1, "YMCA-YRS", String.Format("This person (Fund No. {0}) is already listed as Local Plan Admin (TRANSM) / ACH Debit. The same person cannot be added with the position of {1}.", TextboxFundNo.Text, DropDownListType.SelectedItem.Text), MessageBoxButtons.Stop)
                            'END: MMR | 2018.06.04 | YRS-AT-3961 | Replaced hardcoded position type 'OTHER' with dynamic contact type selcted from dropdown 
                            Return False
                        End If
                    End If

                    strName = HelperFunctions.GetFullName(Me.TextboxFname.Text.Trim, Me.TextboxMname.Text, Me.TextboxLname.Text)
                    If strName <> "" Then
                        'Start AA:06.16.2015 BT:2738:YRS 5.0-2456:YERDI3I-2319: Added below code to validate contacts already a contact exists with 'TRANSM' or 'ACH DR' but not current one
                        If Page_Mode = "UPDATE" Then
                            'Start --Manthan | 2016.06.23 | YRS-AT-2959 | Commented existing code and replacing single apostrophe with double apostrophe in select query to avoid error
                            'drContact = dsContact.Tables(0).Select("TRIM([Contact Name]) = '" + strName.Trim + "' AND [TypeCode] IN ('TRANSM','ACH DR') AND guiUniqueId <> '" + Request("ContactId") + "'")
                            drContact = dsContact.Tables(0).Select(String.Format("TRIM([Contact Name]) = '{0}' AND [TypeCode] IN ('TRANSM','ACH DR') AND guiUniqueId <> '{1}'", strName.Trim.Replace("'", "''"), Request("ContactId")))
                            'End --Manthan | 2016.06.23 | YRS-AT-2959 | Commented existing code and replacing single apostrophe with double apostrophe in select query to avoid error
                        Else
                            'Start --Manthan | 2016.06.23 | YRS-AT-2959 | Commented existing code and replacing single apostrophe with double apostrophe in select query to avoid error
                            'drContact = dsContact.Tables(0).Select("TRIM([Contact Name]) = '" + strName.Trim + "' AND [TypeCode] IN ('TRANSM','ACH DR')")
                            drContact = dsContact.Tables(0).Select(String.Format("TRIM([Contact Name]) = '{0}' AND [TypeCode] IN ('TRANSM','ACH DR')", strName.Trim.Replace("'", "''")))
                            'End --Manthan | 2016.06.23 | YRS-AT-2959 | Commented existing code and replacing single apostrophe with double apostrophe in select query to avoid error
                        End If
                        'END AA:06.16.2015 BT:2738:YRS 5.0-2456:YERDI3I-2319: Added below code to validate contacts already a contact exists with 'TRANSM' or 'ACH DR' but not current one
                        If drContact IsNot Nothing AndAlso (drContact.Length > 0) Then
                            'START: MMR | 2018.06.04 | YRS-AT-3961 | Replaced hardcoded position type 'OTHER' with dynamic contact type selcted from dropdown
                            'MessageBox.Show(PlaceHolder1, "YMCA-YRS", "This person (" + strName.Trim + ") is already listed as Local Plan Admin (TRANSM) / ACH Debit. The same person cannot be added with the position of OTHER.", MessageBoxButtons.Stop) 
                            MessageBox.Show(PlaceHolder1, "YMCA-YRS", String.Format("This person ({0}) is already listed as Local Plan Admin (TRANSM) / ACH Debit. The same person cannot be added with the position of {1}.", strName.Trim, DropDownListType.SelectedItem.Text), MessageBoxButtons.Stop)
                            'END: MMR | 2018.06.04 | YRS-AT-3961 | Replaced hardcoded position type 'OTHER' with dynamic contact type selcted from dropdown
                            Return False
                        End If
                    End If

                End If
            ElseIf DropDownListType.SelectedValue = "TRANSM" OrElse DropDownListType.SelectedValue = "ACH DR" Then
                If HelperFunctions.isNonEmpty(dsContact) Then

                    'START: SB | 2018.05.23 | YRS-AT-2818 | Added new code for not allowing more than one LPA contact.
                    If DropDownListType.SelectedValue = "TRANSM" Then
                        If Page_Mode = "UPDATE" Then
                            transmContactRow = dsContact.Tables(0).Select(String.Format("[TypeCode] = 'TRANSM' AND guiUniqueId <> '{0}'", Request("ContactId")))
                        Else
                            transmContactRow = dsContact.Tables(0).Select("[TypeCode] = 'TRANSM'")
                        End If
                        If transmContactRow IsNot Nothing AndAlso (transmContactRow.Length > 0) Then
                            MessageBox.Show(PlaceHolder1, "YMCA-YRS", "A Local Plan Admin already exists for this YMCA. Delete or change the Type of the existing contact before adding a new one.", MessageBoxButtons.Stop)
                            Return False
                        End If
                    End If
                    'END: SB | 2018.05.23 | YRS-AT-2818 | Added new code for not allowing more than one LPA contact.

                    If TextboxFundNo.Text.Trim <> "" Then
                        'Start AA:06.16.2015 BT:2738:YRS 5.0-2456:YERDI3I-2319: Added below code to validate contacts already a contact exists with OTHER but not current one
                        If Page_Mode = "UPDATE" Then
                            'START: MMR | 2018.06.04 | YRS-AT-3961 | Added 'OTHPOC' contact type in condition
                            'drContact = dsContact.Tables(0).Select("[Fund No] = '" + TextboxFundNo.Text + "' AND [TypeCode] IN ('OTHER') AND guiUniqueId <> '" + Request("ContactId") + "'")
                            drContact = dsContact.Tables(0).Select(String.Format("[Fund No] = '{0}' AND [TypeCode] IN ('OTHER','OTHPOC') AND guiUniqueId <> '{1}'", TextboxFundNo.Text, Request("ContactId")))
                            'END: MMR | 2018.06.04 | YRS-AT-3961 | Added 'OTHPOC' contact type in condition
                        Else
                            'START:MMR | 2018.06.04 | YRS-AT-3961 | Added 'OTHPOC' contact type in condition
                            'drContact = dsContact.Tables(0).Select("[Fund No] = '" + TextboxFundNo.Text + "' AND [TypeCode] IN ('OTHER')")
                            drContact = dsContact.Tables(0).Select(String.Format("[Fund No] = '{0}' AND [TypeCode] IN ('OTHER','OTHPOC')", TextboxFundNo.Text))
                            'END:MMR | 2018.06.04 | YRS-AT-3961 | Added 'OTHPOC' contact type in condition
                        End If
                        'End AA:06.16.2015 BT:2738:YRS 5.0-2456:YERDI3I-2319: Added below code to validate contacts already a contact exists with OTHER but not current one
                        If drContact IsNot Nothing AndAlso (drContact.Length > 0) Then
                            MessageBox.Show(PlaceHolder1, "YMCA-YRS", "This person (Fund No. " + TextboxFundNo.Text + ") is already listed as OTHER. The same person cannot be added with the position of Local Plan Admin (TRANSM) / ACH Debit.", MessageBoxButtons.Stop)
                            Return False

                        End If
                    End If
                    strName = HelperFunctions.GetFullName(Me.TextboxFname.Text.Trim, Me.TextboxMname.Text, Me.TextboxLname.Text)
                    If strName <> "" Then
                        'Start AA:06.16.2015 BT:2738:YRS 5.0-2456:YERDI3I-2319: Added below code to validate contacts already a contact exists with OTHER but not current one
                        If Page_Mode = "UPDATE" Then
                            'Start --Manthan | 2016.04.28 | YRS-AT-2959 | Commented existing code and replacing single apostrophe with double apostrophe in select query to avoid error
                            'drContact = dsContact.Tables(0).Select("TRIM([Contact Name]) = '" + strName.Trim + "' AND [TypeCode] = 'OTHER' AND guiUniqueId <> '" + Request("ContactId") + "'")
                            drContact = dsContact.Tables(0).Select(String.Format("TRIM([Contact Name]) = '{0}' AND [TypeCode] IN ('OTHER', 'OTHPOC') AND guiUniqueId <> '{1}'", strName.Trim.Replace("'", "''"), Request("ContactId"))) 'MMR | 2018.06.04 | YRS-AT-3961 | Added 'OTHPOC' contact type in condition
                            'End --Manthan | 2016.04.28 | YRS-AT-2959 | Commented existing code and replacing single apostrophe with double apostrophe in select query to avoid error
                        Else
                            'Start --Manthan | 2016.04.28 | YRS-AT-2959 | Commented existing code and replacing single apostrophe with double apostrophe in select query to avoid error
                            'drContact = dsContact.Tables(0).Select("TRIM([Contact Name]) = '" + strName.Trim + "' AND [TypeCode] = 'OTHER'")
                            drContact = dsContact.Tables(0).Select(String.Format("TRIM([Contact Name]) = '{0}' AND [TypeCode] IN ('OTHER', 'OTHPOC')", strName.Trim.Replace("'", "''"))) 'MMR | 2018.06.04 | YRS-AT-3961 | Added 'OTHPOC' contact type in condition
                            'End --Manthan | 2016.04.28 | YRS-AT-2959 | Commented existing code and replacing single apostrophe with double apostrophe in select query to avoid error
                        End If
                        'End AA:06.16.2015 BT:2738:YRS 5.0-2456:YERDI3I-2319: Added below code to validate contacts already a contact exists with OTHER but not current one
                        If drContact IsNot Nothing AndAlso (drContact.Length > 0) Then
                            MessageBox.Show(PlaceHolder1, "YMCA-YRS", "This person (" + strName.Trim + ") is already listed as OTHER. The same person cannot be added with the position of Local Plan Admin (TRANSM) / ACH Debit.", MessageBoxButtons.Stop)
                            Return False
                        End If
                    End If

                End If
            End If

            'START: PPP | 2015.10.13 | YRS-AT-2588 | New validation rules applied and also error message is retrieved from AtsMetaMessages
            If TextboxTelephone.Text.Trim().Length > 0 Then
                stTelephoneError = Validation.Telephone(TextboxTelephone.Text.Trim(), YMCAObjects.TelephoneType.PhoneNumber)
                If (Not String.IsNullOrEmpty(stTelephoneError)) Then
                    MessageBox.Show(PlaceHolder1, "YMCA-YRS", stTelephoneError, MessageBoxButtons.Stop)
                    Return False
                End If
            End If
            'END: PPP | 2015.10.13 | YRS-AT-2588 | New validation rules applied and also error message is retrieved from AtsMetaMessages

            Return True
        Catch
            Throw
        End Try
    End Function
    'End:AA:2015.02.10 BT:2738:YRS 5.0-2456:YERDI3I-2319

#End Region

#Region " Add Contact Events "

    Private Sub ButtonUnlink_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonUnlink.Click
        Dim ContactId As String
        Try
            If Page_Mode = "UPDATE" Then
                If Not Request("ContactId") Is Nothing Then
                    Me.TextboxFundNo.Text = String.Empty
                    Me.TextboxFname.Enabled = True
                    Me.TextboxLname.Enabled = True
                    Me.TextboxMname.Enabled = True
                    ButtonUnlink.Enabled = False
                    Me.TextboxFundNo.Text = String.Empty
                End If
            End If

        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message, False)
            'Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
        Finally
            ContactId = Nothing
        End Try
    End Sub

    Private Sub ButtonCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonCancel.Click
        Dim msg As String = ""
        Try
            msg = msg + "<Script Language='JavaScript'>"
            msg = msg + "window.opener.document.forms(0).submit();" '"window.opener.location.href=window.opener.location.href;"
            msg = msg + "self.close();"
            msg = msg + "</Script>"
            Response.Write(msg)

        Catch ex As Exception
            'Throw
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
    End Sub

    Private Sub ButtonOK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonOK.Click

        Try
            'AA:2015.02.10 BT:2738:YRS 5.0-2456:YERDI3I-2319: Added to validate position type
            If ValidateContact() Then
                AssignValueToObject()
            End If

        Catch ex As Exception
            'Throw
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = ex.Message.Trim.ToString()
            HelperFunctions.LogException("AddContactWebForm_ButtonOK_Click", ex)
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)

        End Try
    End Sub

    Private Sub ButtonContact_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonContact.Click
        Try
            ClearSearchControls()
            PanelAddContact.Visible = False
            PanelSearchContact.Visible = True
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = ex.Message.Trim.ToString()
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try

    End Sub

#End Region

#Region " Search Contact Events "

    Private Sub ButtonFind_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonFind.Click
        Try
            g_dataset_dsContact = Nothing

            If Me.TextBoxListFundNo.Text.Trim.Length < 1 And _
                Me.TextBoxLastName.Text.Trim.Length < 1 And _
                Me.TextBoxFirstName.Text.Trim.Length < 1 Then
                MessageBox.Show(Me.MessageBoxPlaceHolder, " YMCA - YRS", "Please enter search criteria.", MessageBoxButtons.Stop, False)
                Return
            End If
            Me.dg.Visible = True
            Me.SearchContact(Me.TextBoxListFundNo.Text.Trim, Me.TextBoxLastName.Text.Trim, Me.TextBoxFirstName.Text.Trim)
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
    End Sub

    Private Sub ButtonClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonClear.Click
        Try
            Me.TextBoxFirstName.Text = String.Empty
            Me.TextBoxLastName.Text = String.Empty
            Me.TextBoxListFundNo.Text = String.Empty

        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
    End Sub

    Private Sub dg_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dg.SelectedIndexChanged
        Dim l_Dataset As DataSet
        Dim l_DataTable As DataTable
        Dim l_DataRow As DataRow
        Dim l_StringPositionType As String
        Dim l_StringShortDesc As String
        Dim l_FundNo As String
        Dim expr As String
        Dim msg As String
        Dim dr() As DataRow
        msg = ""

        Try

            g_integer_count = dg.SelectedIndex
            l_FundNo = dg.SelectedItem.Cells(1).Text.Trim
            expr = "FundNo ='" + l_FundNo.ToString().Trim + "' "
            l_Dataset = g_dataset_dsContact
            dr = l_Dataset.Tables(0).Select(expr.ToString())
            If dr.Length > 0 Then
                l_DataRow = dr(0)
            Else
                l_DataRow = l_Dataset.Tables(0).Rows.Item(g_integer_count)
            End If

            Me.TextboxFundNo.Text = IIf(IsDBNull(l_DataRow.Item("FundNo")), String.Empty, l_DataRow.Item("FundNo"))
            Me.TextboxFname.Text = IIf(IsDBNull(l_DataRow.Item("FirstName")), String.Empty, l_DataRow.Item("FirstName"))
            Me.TextboxLname.Text = IIf(IsDBNull(l_DataRow.Item("LastName")), String.Empty, l_DataRow.Item("LastName"))
            Me.TextboxMname.Text = IIf(IsDBNull(l_DataRow.Item("MiddleName")), String.Empty, l_DataRow.Item("MiddleName"))
            'Added by prasad for YRS 5.0-1379 : New job position field in atsYmcaContacts
            Dim ddlValue As String = IIf(IsDBNull(l_DataRow.Item("Title")), String.Empty, l_DataRow.Item("Title")).ToString
            
            If DropDownListTitle.Items.FindByValue(ddlValue) IsNot Nothing Then
                Me.DropDownListTitle.SelectedValue = ddlValue
            End If


            If (Me.DropDownListType.SelectedValue = String.Empty) Then
                Me.DropDownListType.SelectedValue = String.Empty
            End If


            If (Me.TextboxContactNotes.Text = String.Empty) Then
            Else
                Me.TextboxContactNotes.Text = Me.TextboxContactNotes.Text
            End If

            Me.TextboxFname.Enabled = False
            Me.TextboxMname.Enabled = False
            Me.TextboxLname.Enabled = False

            Session("AC_g_dataset_dsContact") = Nothing

            PanelSearchContact.Visible = False
            PanelAddContact.Visible = True


        Catch ex As SqlException
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = ex.Message.Trim.ToString()
            Server.Transfer("ErrorPageForm.aspx?DBMessage=" + l_String_Exception_Message)

        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = ex.Message.Trim.ToString()
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)

        End Try


    End Sub

    Private Sub dg_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dg.ItemCommand
        Try
            Dim l_button_Select As ImageButton
            l_button_Select = e.Item.FindControl("ImgBtSel")

            If (e.Item.ItemIndex = Me.dg.SelectedIndex And Me.dg.SelectedIndex >= 0) Then
                l_button_Select.ImageUrl = "images\selected.gif"
            End If
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
    End Sub

    Private Sub dg_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles dg.SortCommand
        Try

            dg.SelectedIndex = -1
            If HelperFunctions.isNonEmpty(g_dataset_dsContact) Then
                If Not ViewState("previousSearchSortExpression") Is Nothing AndAlso ViewState("previousSearchSortExpression") <> "" Then
                    'If SortExpression is not the same as the previous one then initialize new one
                    If (e.SortExpression <> ViewState("previousSearchSortExpression")) Then
                        ViewState("previousSearchSortExpression") = e.SortExpression
                        g_dataset_dsContact.Tables(0).DefaultView.Sort = e.SortExpression + " ASC"
                    Else
                        'else toggle existing sort expression
                        g_dataset_dsContact.Tables(0).DefaultView.Sort = IIf(g_dataset_dsContact.Tables(0).DefaultView.Sort.EndsWith("ASC") = True, e.SortExpression + " DESC", e.SortExpression + " ASC")
                    End If
                Else
                    'First time in the sort function
                    g_dataset_dsContact.Tables(0).DefaultView.Sort = e.SortExpression + " ASC"
                    ViewState("previousSearchSortExpression") = e.SortExpression
                End If
                BindGrid(dg, g_dataset_dsContact.Tables(0).DefaultView)

            End If

        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
    End Sub

    Private Sub ButtonCancelSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonCancelSearch.Click

        Try
            PanelSearchContact.Visible = False
            PanelAddContact.Visible = True
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = ex.Message.Trim.ToString()
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try

    End Sub

    Private Sub dg_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dg.ItemDataBound
        Try
            If e.Item.ItemIndex > dg.PageSize Then
                e.Item.Visible = False
                lbl_Search_MoreItems.Visible = True
                lbl_Search_MoreItems.Text = "Results truncated. Showing only " + dg.PageSize.ToString() + " rows out of " + g_dataset_dsContact.Tables(0).DefaultView.Count.ToString()

            End If
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = ex.Message.Trim.ToString()
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try

    End Sub
#End Region

#Region " Local Variable Persistence mechanism "
    Protected Overrides Function SaveViewState() As Object
        l_int_SelectedDataGridItem = dg.SelectedIndex
        Dim al As New ArrayList
        ViewState("Page_Mode") = Page_Mode
        al.Add(StoreLocalVariablesToCache())
        al.Add(MyBase.SaveViewState())
        al.Add(Page_Mode)
        Return al
    End Function
    Protected Overrides Sub LoadViewState(ByVal savedState As Object)
        Dim al As ArrayList = CType(savedState, ArrayList)
        InitializeLocalVariablesFromCache(al.Item(0))
        MyBase.LoadViewState(al.Item(1))
        Page_Mode = ViewState("Page_Mode")
    End Sub

    Private Sub InitializeLocalVariablesFromCache(ByRef obj As Object)
        'Session: Load data from Session so that it is accessible on Postback - This can be replaced by a load from viewstate or database

        g_dataset_dsContact = Session("AC_g_dataset_dsContact")
        l_int_SelectedDataGridItem = Session("AC_l_int_SelectedDataGridItem")
    End Sub
    Private Function StoreLocalVariablesToCache() As Object
        'Session: Pass data into Session so that it is accessible on Postback - This can be replaced by a save to viewstate
        Session("AC_g_dataset_dsContact") = g_dataset_dsContact
        Session("AC_l_int_SelectedDataGridItem") = l_int_SelectedDataGridItem
    End Function

#End Region

#Region " General Utility Functions "
    'dg = The datagrid to bind data to
    'ds = The dataset which contains the data
    'forceVisible = Whether the datagrid should be displayed if it does not contain any data
    Private Sub BindGrid(ByRef dg As DataGrid, ByRef ds As DataSet, Optional ByVal forceVisible As Boolean = False)
        If ds Is Nothing OrElse ds.Tables.Count = 0 OrElse ds.Tables(0).Rows.Count = 0 Then
            dg.DataSource = Nothing
            dg.DataBind()
            dg.Visible = forceVisible
            Exit Sub
        Else
            dg.DataSource = ds.Tables(0)
            dg.DataBind()
            dg.Visible = True

        End If

    End Sub
    Private Sub BindGrid(ByRef dg As DataGrid, ByRef dv As DataView, Optional ByVal forceVisible As Boolean = False)
        If dv Is Nothing OrElse dv.Count = 0 Then
            dg.DataSource = Nothing
            dg.DataBind()
            dg.Visible = forceVisible
            Exit Sub
        Else
            dg.DataSource = dv
            dg.DataBind()
            dg.Visible = True
        End If
    End Sub
#End Region


End Class

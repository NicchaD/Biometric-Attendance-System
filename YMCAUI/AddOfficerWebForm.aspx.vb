'*******************************************************************************
' Copyright YMCA Retirement Fund All Rights Reserved. 
' Project Name		:	YMCA-YRS
' FileName			:	AddOfficerWebForm.aspx
'*******************************************************************************
' Cache-Session     :   Vipul 03Feb06
'*******************************************************************************

'Imports Microsoft.Practices.EnterpriseLibrary.Cachings

'Name:Preeti Date:8Feb06 IssueId:YRST-2051 Reason:Validation control was not functioning properly. Changes in HTML only
'***********************************************************************************************************************************************************
'Modification History
'***********************************************************************************************************************************************************
'Modified By                Date            Description
'***********************************************************************************************************************************************************
'Ashutosh Patil             21-Jun-2007     For avoiding runtime error object reference not set
'NP/PP/SR                   2009.05.18      Optimizing the YMCA Screen
'Sanjay Rawat               2009.06.02      Optimizing the Catch Block
'Neeraj Singh               12/Nov/2009     Added form name for security issue YRS 5.0-940 YRS 5.0-942
'Shashi Shekhar             2010.03.03      Changes for  issue  YRS 5.0-942
'Shashi Shekhar             2010-04-13      Changes made for Gemini-1051
'Shashi Shekhar             2010-04-20      Changes for YRS-5.0-1057
'Shashi Shekhar             2010-06-04      Merge the Search officer popup in Addofficer popup for YRS 5.0-1080
'Shashi Shekhar Singh:      11-june-2010    :BT-538
'Priya                      2010-06-03      Changes made for enhancement in vs-2010 
'Shashi Shekhar             2010-07-19      Integrate Issue fixes and new features which was released in Maintenance VP5 code ( YRS 5.0-1080.)
'Prasad Jadhav              2011-09-19      For YRS 5.0-1389 : Do not allow CEO and CEOI officer records to co-exist
'Prasad Jadhav              2011-10-03      BTID:937 For Search Officer always selecting the first record.
'Prasad Jadhav              2011-10-19      YRS 5.0-1389 : Do not allow CEO and CEOI officer records to co-exist - updated by NP:2011.10.20 for message - 2011.10.20 - Updated to compare against the right column
'prasad Jadhav              2011-10-31      YRS 5.0-1383 : Changes to allow only officers in officers tab
'prasad Jadhav              2011-11-10      YRS 5.0-1383 : Changes to allow only officers in officers tab(reopen)
'prasad Jadhav              2011-11-14      YRS 5.0-1383 : Changes to allow only officers in officers tab(reopen)
'Shashank Patel             2014.10.13      BT-1995\YRS 5.0-2052: Erroneous updates occuring 
'Anudeep A                  2015.02.10      BT:2738:YRS 5.0-2456:YERDI3I-2319: YERDI YMCA Officer Information Update function - YRS Changes
'Manthan Rajguru            2015.09.16      YRS-AT-2550: YRS data cleanup: copyright comments and namespace/reference (Changed the Namespace/reference from 'Infotech' to 'YMCARET')
'Pramod P. Pokale           2015.10.13      YRS-AT-2588: implement some basic telephone number validation Rules'Manthan Rajguru    2018.06.04      YRS-AT-3961 -  YRS enh: add new Contact Type for YMCAs (TrackIT 33514) 
'Chandra sekar              2018.06.07      YRS-AT-3961 -  YRS enh: add new Contact Type for YMCAs (TrackIT 33514) 
'***********************************************************************************************************************************************************
#Region " Namespaces "
Imports System.Data
Imports System.Data.SqlClient
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
#End Region

Public Class AddOfficerWebForm
    Inherits System.Web.UI.Page

#Region " Global variable declaration "
    Dim Page_Mode As String = String.Empty
    Dim strFormName As String = New String("AddOfficerWebForm.aspx")

    Dim g_bool_flagTitleCancel As Boolean
    Dim g_dataset_dsOfficer As DataSet
    Dim g_integer_count As Integer
    'YRS 5.0-1383 : Changes to allow only officers in officers tab(reopen)
    Dim counts As Integer

    Dim l_int_SelectedDataGridItem As Integer   'Tracks the selected index of the Search Datagrid
#End Region

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents ButtonUnlink As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonSearch As System.Web.UI.WebControls.Button
    Protected WithEvents Requiredfieldvalidator3 As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents Requiredfieldvalidator6 As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents LabelTitle As System.Web.UI.WebControls.Label
    Protected WithEvents TextboxTitle As System.Web.UI.WebControls.TextBox
    Protected WithEvents ButtonTitle As System.Web.UI.WebControls.Button
    Protected WithEvents RequiredFieldValidator4 As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents LabelTelephone As System.Web.UI.WebControls.Label
    Protected WithEvents TextboxTelephone As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextboxEffectiveDate As YMCAUI.DateUserControl
    Protected WithEvents RequiredFieldValidator2 As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents RegularExpressionValidator1 As System.Web.UI.WebControls.RegularExpressionValidator
    Protected WithEvents LabelExtnNo As System.Web.UI.WebControls.Label
    Protected WithEvents TextboxExtnNo As System.Web.UI.WebControls.TextBox
    Protected WithEvents Regularexpressionvalidator2 As System.Web.UI.WebControls.RegularExpressionValidator
    Protected WithEvents LabelOfficersEmail As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxOficersEmail As System.Web.UI.WebControls.TextBox
    Protected WithEvents ValidateOfficersEmail As System.Web.UI.WebControls.RegularExpressionValidator
    Protected WithEvents LabelEffectiveDate As System.Web.UI.WebControls.Label
    Protected WithEvents ValidationSummary1 As System.Web.UI.WebControls.ValidationSummary
    Protected WithEvents ButtonOK As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonCancel As System.Web.UI.WebControls.Button
    Protected WithEvents LabelFname As System.Web.UI.WebControls.Label
    Protected WithEvents TextboxFname As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelMname As System.Web.UI.WebControls.Label
    Protected WithEvents TextboxMname As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxName As System.Web.UI.WebControls.TextBox
    Protected WithEvents Requiredfieldvalidator5 As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents LabelLname As System.Web.UI.WebControls.Label
    Protected WithEvents TextboxLname As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelFundNo As System.Web.UI.WebControls.Label
    Protected WithEvents TextboxFundNo As System.Web.UI.WebControls.TextBox
    Protected WithEvents Requiredfieldvalidator7 As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents Button2 As System.Web.UI.WebControls.Button
    Protected WithEvents Button3 As System.Web.UI.WebControls.Button
    'below line is Added by Neeraj for issue id YRS 5.0-940 
    Protected WithEvents PanelAddOfficer As System.Web.UI.WebControls.Panel

    'Search Pages controls
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
    Private designerPlaceholderDeclaration As System.Object
    Protected WithEvents NotesFlag As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents ButtonCancelSearch As System.Web.UI.WebControls.Button
    Protected WithEvents RefundMultiPage As Microsoft.Web.UI.WebControls.MultiPage
    Protected WithEvents LabelNoRecord As System.Web.UI.WebControls.Label
    Protected WithEvents PanelSearchOfficer As System.Web.UI.WebControls.Panel
    Protected WithEvents Button1 As System.Web.UI.WebControls.Button
    Protected WithEvents LabelNoneRecord As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_Search_MoreItems As System.Web.UI.WebControls.Label

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.


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

#Region " Page Load "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        Dim obj As YMCAUI.SessionManager.SessionHandler  'Shashi Shekhar:2010-03-04       
        Try

            If IsPostBack AndAlso HelperFunctions.isNonEmpty(g_dataset_dsOfficer) Then
                'Shashi Shekhar Singh:11-june-2010:BT-538
                If Not g_dataset_dsOfficer.Tables(0).Columns.Contains("City") Then
                    RemoveDataGridCol()
                End If
                dg.DataSource = g_dataset_dsOfficer.Tables(0).DefaultView
                dg.SelectedIndex = l_int_SelectedDataGridItem
                dg.DataBind()
            End If

            'Priya  BT729 


            Me.TextboxTelephone.Attributes.Add("onkeypress", "javascript:return ValidateSpaceBar();")
            Me.TextboxExtnNo.Attributes.Add("onkeypress", "javascript:return ValidateSpaceBar();")
            If MyBase.Session("LoggedUserKey") Is Nothing Then
                Response.Redirect("Login.aspx", False)
            End If
            Me.TextboxEffectiveDate.RequiredDate = True



            If Not Me.IsPostBack Then
                Session("AO_g_dataset_dsOfficer") = Nothing
                Session("TitleType") = Nothing
                Session("TitleName") = Nothing

                Me.TextboxFundNo.Enabled = False
                TextboxEffectiveDate.RequiredValidatorErrorMessage = "Effective Date cannot be blank" 'Chandra sekar | 2018.06.07 | YRS-AT-3961 | Replaced existing error message of date user control 

                If Not Session("SessionManager") Is Nothing Then
                    Dim l_sm As YMCAUI.SessionManager.SessionHandler = CType(Session("SessionManager"), YMCAUI.SessionManager.SessionHandler)

                    If l_sm.YMCAMaintenance Is Nothing Then
                        l_sm.YMCAMaintenance = New YMCAUI.SessionManager.YMCAMaintenance
                    End If
                    l_sm.YMCAMaintenance.OfficerSearchCancel = True
                    Session("SessionManager") = l_sm
                End If


                Dim OfficerId As String
                If Not Request("OfficerId") Is Nothing Then
                    PanelSearchOfficer.Visible = False
                    OfficerId = Request("OfficerId").Trim()
                Else
                    PanelAddOfficer.Visible = False
                End If

                Dim o As Officer = GetOfficerById(OfficerId)
                If o Is Nothing Then
                    Throw New Exception("Invalid Officer Id passed. Please close this window and try again.")
                End If
                Me.TextBoxName.Text = o.Name.Trim
                ' cancel button of Title form is not clicked
                '-------------------------------------------------------------------------
                'Shashi Shekhar:03-Mar-2010:For YRS-5.0-942
                If (o.FundNo = Nothing Or o.FundNo = String.Empty) Then
                    Me.TextboxFundNo.Text = String.Empty
                Else
                    Me.TextboxFundNo.Text = o.FundNo.Trim
                End If

                Me.TextboxFname.Text = o.FirstName.Trim
                Me.TextboxMname.Text = o.MiddleName.Trim
                Me.TextboxLname.Text = o.LastName.Trim

                '-----------------------------------------------

                Me.TextboxTelephone.Text = o.Telephone.Trim
                Me.TextboxExtnNo.Text = o.ExtnNo.Trim
                Me.TextBoxOficersEmail.Text = o.Email.Trim
                Me.TextboxEffectiveDate.Text = o.EffectiveDate.Trim
                Session("TitleType") = o.TitleType.Trim
                Session("TitleName") = o.Title.Trim
                'YRS 5.0-1383 : Changes to allow only officers in officers tab(reopen)
                counts = OfficerOrNonofficer(o.TitleType.Trim)
                If counts = 0 Then
                    Me.TextboxTitle.Text = " "
                    Session("TitleName") = Nothing
                Else
                    Me.TextboxTitle.Text = o.Title.Trim
                End If

                If OfficerId Is Nothing OrElse OfficerId = String.Empty Then
                    Page_Mode = "ADD"
                    'Shashi Shekhar:2010-04-20:Changes for YRS-5.0-1057
                    Session("Page_Mode") = "ADD" 'using this session on serch officer screen to nullify the title session variables
                    Me.ButtonUnlink.Visible = False
                Else
                    Page_Mode = "UPDATE"
                    Session("Page_Mode") = "UPDATE"
                    If (o.FundNo = Nothing Or o.FundNo = String.Empty) Then
                        Me.TextboxFname.Enabled = True
                        Me.TextboxMname.Enabled = True
                        Me.TextboxLname.Enabled = True
                        Me.ButtonSearch.Enabled = True
                        Me.ButtonUnlink.Visible = False
                        Me.ButtonSearch.Visible = True
                    Else
                        Me.TextboxFname.Enabled = False
                        Me.TextboxMname.Enabled = False
                        Me.TextboxLname.Enabled = False
                        Me.ButtonSearch.Enabled = False
                        Me.TextboxFundNo.Enabled = False
                        Me.ButtonUnlink.Visible = True
                        Me.ButtonSearch.Visible = False
                    End If
                End If

            Else
                If CType(Session("TitleCancel"), Boolean) = False Then
                    Me.TextboxTitle.Text = CType(Session("TitleName"), String)
                End If
                '-------------------------------------------------------------------------
                'Shashi Shekhar:03-Mar-2010:For YRS-5.0-942
                obj = DirectCast(Session("SessionManager"), YMCAUI.SessionManager.SessionHandler)

                If Not IsNothing(obj) Then
                    If Not obj.YMCAMaintenance Is Nothing Then
                        If obj.YMCAMaintenance.OfficerSearchCancel = False Then
                            Me.TextboxFundNo.Enabled = True
                            Me.TextboxFname.Text = obj.YMCAMaintenance.FirstName
                            Me.TextboxLname.Text = obj.YMCAMaintenance.LastName
                            Me.TextboxMname.Text = obj.YMCAMaintenance.MiddleName
                            Me.TextboxFundNo.Text = obj.YMCAMaintenance.FundNo

                            'Shashi Shekhar:2010-04-20:Changes for YRS-5.0-1057
                            If Page_Mode = "UPDATE" Then
                                Me.TextboxTitle.Text = CType(Session("TitleName"), String)
                            Else
                                If Session("TitleType") = Nothing And Session("TitleName") = Nothing Then
                                    Me.TextboxTitle.Text = obj.YMCAMaintenance.TitleName
                                Else
                                    Me.TextboxTitle.Text = CType(Session("TitleName"), String)
                                End If

                            End If

                            Me.TextboxFname.Enabled = False
                            Me.TextboxMname.Enabled = False
                            Me.TextboxLname.Enabled = False
                            Me.TextboxFundNo.Enabled = False
                        Else
                            Me.TextboxFundNo.Enabled = False
                        End If
                    End If

                Else
                    Me.TextboxFundNo.Enabled = False
                End If

            End If
        Catch ex As Exception
            'Throw
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = ex.Message.Trim.ToString()
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message, False)
            'Throw

            'Dim l_String_Exception_Message As String
            'l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            'Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
        Finally
            obj = Nothing 'Shashi Shekhar:2010-03-04
        End Try

    End Sub
#End Region

#Region " Events of Add Officer "

    Private Sub ButtonTitle_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonTitle.Click
        Try
            Dim msg1 As String = "<script language='javascript'>" & _
         "window.open('SelectTitle.aspx?Page=AddOffiecer&UniqueSessionID=" + UniqueSessionId + "','CustomPopUp1', " & _
         "'width=750, height=450, menubar=no, Resizable=Yes,top=120,left=120, scrollbars = yes ')" & _
       "</script>"

            Response.Write(msg1)

        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = ex.Message.Trim.ToString()
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message, False)

            'Throw
            'Dim l_String_Exception_Message As String
            'l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            'Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
        End Try
    End Sub

    Private Sub ButtonSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonSearch.Click
        Try
            ClearSearchControls()
            PanelAddOfficer.Visible = False
            PanelSearchOfficer.Visible = True
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = ex.Message.Trim.ToString()
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message, False)
            'Throw
            'Dim l_String_Exception_Message As String
            'l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            'Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
        End Try

    End Sub

    Private Sub ButtonOK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonOK.Click
        Dim msg As String = String.Empty
        Dim objOfficer As New Officer
        Dim obj As YMCAUI.SessionManager.SessionHandler 'Shashi Shekhar:2010-03-04
        'Added by prasad 2011-09-21 :YRS 5.0-1389 : Do not allow CEO and CEOI officer records to co-exist
        Dim isvalid As Boolean = True
        Dim strName As String = ""
        Dim stTelephoneError As String 'PPP | 2015.10.13 | YRS-AT-2588

        Try
          
            'Shashi Shekhar:2010-03-03:For YRS-5.0-942
            'objOfficer.Name = Me.TextBoxName.Text.Trim
            'Start:AA:02.10.2015 BT:2738:YRS 5.0-2456:YERDI3I-2319: Added for not to add unnecassary spaces in database for name column
            'objcontacts.Name = Me.TextboxFname.Text.Trim + " " + Me.TextboxMname.Text + " " + Me.TextboxLname.Text
            strName = HelperFunctions.GetFullName(Me.TextboxFname.Text.Trim, Me.TextboxMname.Text, Me.TextboxLname.Text)
            objOfficer.Name = strName
            'End:AA:02.10.2015 BT:2738:YRS 5.0-2456:YERDI3I-2319: Added for not to add unnecassary spaces in database for name column
            objOfficer.FundNo = IIf(IsDBNull(Me.TextboxFundNo.Text.Trim), String.Empty, Me.TextboxFundNo.Text.Trim)
            objOfficer.FirstName = IIf(IsDBNull(Me.TextboxFname.Text.Trim), String.Empty, Me.TextboxFname.Text.Trim)
            objOfficer.MiddleName = IIf(IsDBNull(Me.TextboxMname.Text.Trim), String.Empty, Me.TextboxMname.Text.Trim)
            objOfficer.LastName = IIf(IsDBNull(Me.TextboxLname.Text.Trim), String.Empty, Me.TextboxLname.Text.Trim)

            obj = DirectCast(Session("SessionManager"), YMCAUI.SessionManager.SessionHandler)

            If Not IsNothing(obj) Then

                'Shashi Shekhar:2010-04-20:Changes for YRS-5.0-1057
                If Page_Mode = "UPDATE" Then
                    objOfficer.TitleType = Session("TitleType")
                    objOfficer.Title = Session("TitleName")
                Else

                    If (obj.YMCAMaintenance.OfficerSearchCancel = True) Then 'Data coming from search officer screen
                        ' If (obj.YMCAMaintenance.Title = String.Empty) Then
                        objOfficer.TitleType = Session("TitleType")
                        objOfficer.Title = Session("TitleName")
                    Else
                        'Shashi Shekhar:2010-04-20:Changes for YRS-5.0-1057
                        'If Sessions are set to nothing it means we have to take titile data from object where data has been assigned from Search officer screen.  
                        If Session("TitleType") = Nothing And Session("TitleName") = Nothing Then
                            objOfficer.TitleType = obj.YMCAMaintenance.Title.Trim
                            objOfficer.Title = obj.YMCAMaintenance.TitleName.Trim
                        Else 'get data from session
                            objOfficer.TitleType = Session("TitleType")
                            objOfficer.Title = Session("TitleName")

                        End If

                    End If
                End If

                'objOfficer.TitleType = IIf((obj.Title = String.Empty), Session("TitleType"), obj.Title.Trim)
                'objOfficer.Title = IIf((obj.TitleName = String.Empty), Session("TitleName"), obj.TitleName.Trim)
            Else
                objOfficer.TitleType = Session("TitleType")
                objOfficer.Title = Session("TitleName")
            End If

            '---------------------------------------------------------

            'Commented by shashi:2010-03-05:For YRS-5.0-942
            'objOfficer.TitleType = Session("TitleType")
            ' objOfficer.Title = Me.TextboxTitle.Text.Trim

            objOfficer.Telephone = Me.TextboxTelephone.Text.Trim
            objOfficer.ExtnNo = Me.TextboxExtnNo.Text.Trim
            objOfficer.Email = Me.TextBoxOficersEmail.Text.Trim
            objOfficer.EffectiveDate = CType(Me.TextboxEffectiveDate.Text, Date).ToShortDateString.Trim

            If Page_Mode = "ADD" Then
                objOfficer.OfficerId = Guid.NewGuid().ToString()
            Else
                objOfficer.OfficerId = Request("OfficerId").Trim()
            End If
            'Added by prasad 2011-09-21 :YRS 5.0-1389 : Do not allow CEO and CEOI officer records to co-exist
            'Added by prasad 2011-10-19 :YRS 5.0-1389 : Do not allow CEO and CEOI officer records to co-exist
            Dim bitAllowMultiple As Boolean = True
            Dim row As DataRow()
            Dim ds As DataSet = CType(Session("YMCA Officer"), DataSet)
            Dim ds_titles As DataSet

            'Added by prasad 2011-10-19 :YRS 5.0-1389 : Do not allow CEO and CEOI officer records to co-exist
            ds_titles = YMCARET.YmcaBusinessObject.SelectTitleBOClass.LookUpTitle()

            row = ds_titles.Tables(0).Select("chrPositionType='" & objOfficer.TitleType.Trim & "'")

            If row.Count <> 0 Then
                If (Not row(0)("bitAllowMultiple")) Then
                    bitAllowMultiple = False
                End If
            End If

            If Not bitAllowMultiple Then
                Dim drOriginal As DataRow()
                Dim drDuplicate As DataRow()
                drDuplicate = ds.Tables(0).Select("chvpositionTitlecode='" & objOfficer.TitleType.Trim & "' And guiUniqueId <> '" & objOfficer.OfficerId & "'")
                
                'NP:2011.10.20 - Changed the message format as well as the order in which the validation is being checked
                If objOfficer.TitleType.Trim = "CEOI" Or objOfficer.TitleType.Trim = "CEO" Then
                    Dim drceo As DataRow() = ds.Tables(0).Select("guiUniqueId <> '" & objOfficer.OfficerId & "' AND (chvpositionTitlecode = 'CEO' OR chvpositionTitlecode ='CEOI')")
                    If drceo.Length <> 0 Then
                        MessageBox.Show(MessageBoxPlaceHolder, " YMCA - YRS", "Only one CEO or CEOI record can exist at a time. Please delete the existing CEO or CEOI record before applying your change.", MessageBoxButtons.Stop, False)
                        isvalid = False
                    End If
                    'Start:AA:2015.02.10 BT:2738:YRS 5.0-2456:YERDI3I-2319: 
                    'Added Validation for not to create CVO or VOLDIR if any one of them exists
                    'Added Validation for not to create CHRO or HRVP if any one of them exists
                ElseIf objOfficer.TitleType.Trim = "CVO" Or objOfficer.TitleType.Trim = "VOLDIR" Then
                    Dim drCVO As DataRow() = ds.Tables(0).Select("guiUniqueId <> '" & objOfficer.OfficerId & "' AND (chvpositionTitlecode = 'CVO' OR chvpositionTitlecode ='VOLDIR')")
                    If drCVO.Length <> 0 Then
                        MessageBox.Show(MessageBoxPlaceHolder, " YMCA - YRS", "Only one CVO or VOLDIR record can exist at a time. Please delete the existing CVO or VOLDIR record before applying your change.", MessageBoxButtons.Stop, False)
                        isvalid = False
                    End If
                ElseIf objOfficer.TitleType.Trim = "CHRO" Or objOfficer.TitleType.Trim = "HRVP" Then
                    Dim drCVO As DataRow() = ds.Tables(0).Select("guiUniqueId <> '" & objOfficer.OfficerId & "' AND (chvpositionTitlecode = 'CHRO' OR chvpositionTitlecode ='HRVP')")
                    If drCVO.Length <> 0 Then
                        MessageBox.Show(MessageBoxPlaceHolder, " YMCA - YRS", "Only one CHRO or HRVP record can exist at a time. Please delete the existing CHRO or HRVP record before applying your change.", MessageBoxButtons.Stop, False)
                        isvalid = False
                    End If
                    'End:AA:2015.02.10 BT:2738:YRS 5.0-2456:YERDI3I-2319
                ElseIf drDuplicate.Length <> 0 Then
                    MessageBox.Show(MessageBoxPlaceHolder, " YMCA - YRS", String.Format("Only one officer of type {0} can exist at a time.", TextboxTitle.Text), MessageBoxButtons.Stop, False)
                    isvalid = False
                End If
            End If

            'START: PPP | 2015.10.13 | YRS-AT-2588 | New validation rules applied and also error message is retrieved from AtsMetaMessages
            If (TextboxTelephone.Text.Trim().Length > 0) And isvalid Then ' PPP | 2015.10.20 | YRS-AT-2588 | Added 'isvalid' flag checking, if there are no validation errors then only validate telephone number
                stTelephoneError = Validation.Telephone(TextboxTelephone.Text.Trim(), YMCAObjects.TelephoneType.PhoneNumber)
                If (Not String.IsNullOrEmpty(stTelephoneError)) Then
                    MessageBox.Show(MessageBoxPlaceHolder, " YMCA - YRS", stTelephoneError, MessageBoxButtons.Stop, False)
                    isvalid = False
                End If
            End If
            'END: PPP | 2015.10.13 | YRS-AT-2588 | New validation rules applied and also error message is retrieved from AtsMetaMessages

            'Added by prasad 2011-09-21 :YRS 5.0-1389 : Do not allow CEO and CEOI officer records to co-exist
            If isvalid Then
                Session("OfficerValues") = objOfficer
                Dim objPopupAction As PopupResult = New PopupResult
                objPopupAction.Page = "OFFICER"
                objPopupAction.Action = PopupResult.ActionTypes.ADD
                objPopupAction.State = objOfficer
                Session("PopUpAction") = objPopupAction

                msg = msg + "<Script Language='JavaScript'>"
                msg = msg + "window.opener.document.forms(0).submit();" '"window.opener.location.href=window.opener.location.href;"
                msg = msg + "self.close();"
                msg = msg + "</Script>"
                Response.Write(msg)
            End If
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = ex.Message.Trim.ToString()
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message, False)
            'Throw
            'Dim l_String_Exception_Message As String
            'l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            'Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
        Finally
            obj = Nothing
            Session("TitleType") = Nothing
            Session("TitleName") = Nothing

        End Try
    End Sub
    Private Sub ButtonCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonCancel.Click
        Dim msg As String = String.Empty
        Try
            Dim objPopupAction As PopupResult = New PopupResult
            objPopupAction.Page = "OFFICER"
            objPopupAction.Action = PopupResult.ActionTypes.CANCEL
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
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
        End Try
    End Sub

    Private Sub ButtonUnlink_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonUnlink.Click
        Dim OfficerId As String
        Try
            If Page_Mode = "UPDATE" Then
                If Not Request("OfficerId") Is Nothing Then
                    Me.TextboxFundNo.Text = String.Empty
                    Me.TextboxFname.Enabled = True
                    Me.TextboxLname.Enabled = True
                    Me.TextboxMname.Enabled = True
                    Me.TextboxTitle.Enabled = True
                    Me.ButtonTitle.Enabled = True
                    ButtonUnlink.Enabled = False
                End If
            End If

        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            'Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message, False)
        Finally
            OfficerId = Nothing
        End Try
    End Sub

#End Region

#Region " Events of Search Officer "

    Private Sub ButtonFind_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonFind.Click
        Try
            g_dataset_dsOfficer = Nothing
            If Me.TextBoxListFundNo.Text.Trim.Length < 1 And _
                Me.TextBoxLastName.Text.Trim.Length < 1 And _
                Me.TextBoxFirstName.Text.Trim.Length < 1 Then
                MessageBox.Show(Me.MessageBoxPlaceHolder, " YMCA - YRS", "Please enter search criteria.", MessageBoxButtons.Stop, False)
                Return
            End If
            Me.dg.Visible = True
            Me.SearchOfficer(Me.TextBoxListFundNo.Text.Trim, Me.TextBoxLastName.Text.Trim, Me.TextBoxFirstName.Text.Trim)
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

    Private Sub dg_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles dg.SortCommand
        Try
            dg.SelectedIndex = -1
            If HelperFunctions.isNonEmpty(g_dataset_dsOfficer) Then
                If Not ViewState("previousSearchSortExpression") Is Nothing AndAlso ViewState("previousSearchSortExpression") <> "" Then
                    'If SortExpression is not the same as the previous one then initialize new one
                    If (e.SortExpression <> ViewState("previousSearchSortExpression")) Then
                        ViewState("previousSearchSortExpression") = e.SortExpression
                        g_dataset_dsOfficer.Tables(0).DefaultView.Sort = e.SortExpression + " ASC"
                    Else
                        'else toggle existing sort expression
                        g_dataset_dsOfficer.Tables(0).DefaultView.Sort = IIf(g_dataset_dsOfficer.Tables(0).DefaultView.Sort.EndsWith("ASC") = True, e.SortExpression + " DESC", e.SortExpression + " ASC")
                    End If
                Else
                    'First time in the sort function
                    g_dataset_dsOfficer.Tables(0).DefaultView.Sort = e.SortExpression + " ASC"
                    ViewState("previousSearchSortExpression") = e.SortExpression
                End If
                BindGrid(dg, g_dataset_dsOfficer.Tables(0).DefaultView)

            End If

        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
    End Sub

    Private Sub dg_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dg.ItemCommand
        Try
            Dim l_button_Select As ImageButton
            l_button_Select = e.Item.FindControl("ImageButtonSelect")

            If (e.Item.ItemIndex = Me.dg.SelectedIndex And Me.dg.SelectedIndex >= 0) Then
                l_button_Select.ImageUrl = "images\selected.gif"
            End If
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
    End Sub

    Private Sub dg_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dg.ItemDataBound
        Try
            If e.Item.ItemIndex > dg.PageSize Then
                e.Item.Visible = False
                lbl_Search_MoreItems.Visible = True
                lbl_Search_MoreItems.Text = "Results truncated. Showing only " + dg.PageSize.ToString() + " rows out of " + g_dataset_dsOfficer.Tables(0).DefaultView.Count.ToString()
            End If
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = ex.Message.Trim.ToString()
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
        End Try

    End Sub

    Private Sub dg_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dg.SelectedIndexChanged
        Dim l_Dataset As DataSet
        Dim l_DataTable As DataTable
        Dim l_DataRow As DataRow
        Dim l_StringPositionType As String
        Dim l_StringShortDesc As String
        Dim SM As YMCAUI.SessionManager.SessionHandler
        'YRS 5.0-1383 : Changes to allow only officers in officers tab(reopen)
        Dim count As Integer
        'Commented by prasad:2011-10-03: BTID:937 For Search Officer always selecting the first record.
        'Dim l_FundNo As String
        'Commented by prasad:2011-10-03: BTID:937 For Search Officer always selecting the first record.
        'Dim expr As String
        Dim msg As String
        Dim dr() As DataRow
        msg = ""

        Try
            If Not Session("SessionManager") Is Nothing Then
                SM = CType(Session("SessionManager"), YMCAUI.SessionManager.SessionHandler)
            Else
                SM = New YMCAUI.SessionManager.SessionHandler
            End If

            If SM.YMCAMaintenance Is Nothing Then
                SM.YMCAMaintenance = New YMCAUI.SessionManager.YMCAMaintenance
            End If

            SM.YMCAMaintenance.OfficerSearchCancel = False
            g_integer_count = dg.SelectedIndex
            'Commented by prasad:2011-10-03: BTID:937 For Search Officer always selecting the first record.
            'l_FundNo = dg.SelectedItem.Cells(2).Text.Trim
            'Commented by prasad
            'expr = "FundNo ='" + l_FundNo.ToString().Trim + "' "
            l_Dataset = g_dataset_dsOfficer
            'Commented by prasad:2011-10-03: BTID:937 For Search Officer always selecting the first record.
            'dr = l_Dataset.Tables(0).Select(expr.ToString())
            'If dr.Length > 0 Then
            '    l_DataRow = dr(0)
            'Else
            l_DataRow = l_Dataset.Tables(0).Rows.Item(g_integer_count)
            'End If
            SM.YMCAMaintenance.FundNo = IIf(IsDBNull(l_DataRow.Item("FundNo")), String.Empty, l_DataRow.Item("FundNo"))
            SM.YMCAMaintenance.FirstName = IIf(IsDBNull(l_DataRow.Item("FirstName")), String.Empty, l_DataRow.Item("FirstName"))
            SM.YMCAMaintenance.LastName = IIf(IsDBNull(l_DataRow.Item("LastName")), String.Empty, l_DataRow.Item("LastName"))
            SM.YMCAMaintenance.MiddleName = IIf(IsDBNull(l_DataRow.Item("MiddleName")), String.Empty, l_DataRow.Item("MiddleName"))
            SM.YMCAMaintenance.Title = IIf(IsDBNull(l_DataRow.Item("Title")), String.Empty, l_DataRow.Item("Title"))
            SM.YMCAMaintenance.TitleName = IIf(IsDBNull(l_DataRow.Item("TitleName")), String.Empty, l_DataRow.Item("TitleName"))

            'Shashi Shekhar:2010-04-20:changes for YRS-5.0-1057
            If Session("Page_Mode") = "ADD" Then
                Session("TitleType") = Nothing
                Session("TitleName") = Nothing
            End If
            '-----------------------------------------------------

            Session("SessionManager") = SM
           

            If CType(Session("TitleCancel"), Boolean) = False Then
                Me.TextboxTitle.Text = CType(Session("TitleName"), String)
            End If
            '-------------------------------------------------------------------------
            'Shashi Shekhar:03-Mar-2010:For YRS-5.0-942
            Dim obj As YMCAUI.SessionManager.SessionHandler
            obj = DirectCast(Session("SessionManager"), YMCAUI.SessionManager.SessionHandler)

            If Not IsNothing(obj) Then
                If Not obj.YMCAMaintenance Is Nothing Then
                    If obj.YMCAMaintenance.OfficerSearchCancel = False Then
                        Me.TextboxFundNo.Enabled = True
                        Me.TextboxFname.Text = obj.YMCAMaintenance.FirstName
                        Me.TextboxLname.Text = obj.YMCAMaintenance.LastName
                        Me.TextboxMname.Text = obj.YMCAMaintenance.MiddleName
                        Me.TextboxFundNo.Text = obj.YMCAMaintenance.FundNo
                        'prasad:31-october-2011 YRS 5.0-1383 : Changes to allow only officers in officers tab
                        'YRS 5.0-1383 : Changes to allow only officers in officers tab(reopen)
                        count = OfficerOrNonofficer(SM.YMCAMaintenance.Title)
                        'Shashi Shekhar:2010-04-20:Changes for YRS-5.0-1057
                        If Page_Mode = "UPDATE" Then
                            If count = 0 Then
                                Me.TextboxTitle.Text = " "
                            Else
                                Me.TextboxTitle.Text = SM.YMCAMaintenance.TitleName
                                'Added by prasad wrong value picked from session.
                                Session("TitleName") = SM.YMCAMaintenance.TitleName
                                Session("TitleType") = SM.YMCAMaintenance.Title
                            End If
                            'Commented by prasad wrong value from session is picked
                            'Me.TextboxTitle.Text = CType(Session("TitleName"), String)
                        Else

                            If count = 0 Then
                                Me.TextboxTitle.Text = " "
                                'prasad:10-november-2011 YRS 5.0-1383 : Changes to allow only officers in officers tab
                                obj.YMCAMaintenance.TitleName = " "
                            Else
                                If Session("TitleType") = Nothing And Session("TitleName") = Nothing Then
                                    Me.TextboxTitle.Text = obj.YMCAMaintenance.TitleName
                                Else
                                    Me.TextboxTitle.Text = CType(Session("TitleName"), String)
                                End If
                            End If

                            'If Session("TitleType") = Nothing And Session("TitleName") = Nothing Then
                            '    Me.TextboxTitle.Text = obj.YMCAMaintenance.TitleName
                            'Else
                            '    Me.TextboxTitle.Text = CType(Session("TitleName"), String)
                            'End If

                        End If
                            Me.TextboxFname.Enabled = False
                            Me.TextboxMname.Enabled = False
                            Me.TextboxLname.Enabled = False
                            Me.TextboxFundNo.Enabled = False
                        Else
                            Me.TextboxFundNo.Enabled = False
                        End If
                End If

            Else
                Me.TextboxFundNo.Enabled = False
            End If
            '---------------------------------------------------------------------------------------

            Session("AO_g_dataset_dsOfficer") = Nothing
            PanelSearchOfficer.Visible = False
            PanelAddOfficer.Visible = True


        Catch ex As SqlException
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = ex.Message.Trim.ToString()
            Server.Transfer("ErrorPageForm.aspx?DBMessage=" + l_String_Exception_Message)

        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = ex.Message.Trim.ToString()
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
        Finally
            SM = Nothing
            'Shashi Shekhar:2010-04-20:Changes for YRS-5.0-1057
            Session("Page_Mode") = Nothing
        End Try

    End Sub
    'YRS 5.0-1383 : Changes to allow only officers in officers tab(reopen)
    Private Function OfficerOrNonofficer(ByVal jobtitle As String) As Integer
        Dim datarows() As DataRow
        Dim datasettitles As DataSet
        datasettitles = YMCARET.YmcaBusinessObject.SelectTitleBOClass.LookUpTitle()
        'datarows = datasettitles.Tables(0).Select("chrPositionType='" & SM.YMCAMaintenance.Title & "' AND bitOfficer=1")
        datarows = datasettitles.Tables(0).Select("chrPositionType='" & jobtitle & "' AND bitOfficer=1")
        Return datarows.Count
    End Function

    Private Sub ButtonCancelSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonCancelSearch.Click
        Try
            PanelSearchOfficer.Visible = False
            PanelAddOfficer.Visible = True
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = ex.Message.Trim.ToString()
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
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
        g_dataset_dsOfficer = Session("AO_g_dataset_dsOfficer")
        l_int_SelectedDataGridItem = Session("AO_l_int_SelectedDataGridItem")
    End Sub

    Private Function StoreLocalVariablesToCache() As Object
        'Session: Pass data into Session so that it is accessible on Postback - This can be replaced by a save to viewstate
        Session("AO_g_dataset_dsOfficer") = g_dataset_dsOfficer
        Session("AO_l_int_SelectedDataGridItem") = l_int_SelectedDataGridItem
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

#Region " Private Functions "

    Private Function GetOfficerById(ByVal officerId As String) As Officer
        Dim n As Officer
        If officerId = Nothing OrElse officerId = String.Empty Then
            n = New Officer
            n.EffectiveDate = String.Empty
            n.Email = String.Empty
            n.ExtnNo = String.Empty
            n.Name = String.Empty
            n.OfficerId = String.Empty
            n.Telephone = String.Empty
            n.Title = String.Empty
            n.TitleType = String.Empty

            '------Shashi Shekhar:2010-03-05:YRS-5.0-942----------
            n.FundNo = String.Empty
            n.FirstName = String.Empty
            n.MiddleName = String.Empty
            n.LastName = String.Empty
            '----------------------------------------------------

            Return n
        End If
        If Session("YMCA Officer") Is Nothing Then Return Nothing
        Dim ds As DataSet = CType(Session("YMCA Officer"), DataSet)
        If HelperFunctions.isEmpty(ds) Then Return Nothing
        Dim dr As DataRow()
        dr = ds.Tables(0).Select("guiUniqueId = '" & officerId & "'")
        If dr Is Nothing OrElse dr.Length = 0 Then Return Nothing
        n = New Officer
        'NPTODO: Handle Null values here
        n.OfficerId = IIf(dr(0).IsNull("guiUniqueID"), String.Empty, dr(0).Item("guiUniqueID"))
        n.EffectiveDate = IIf(dr(0).IsNull("Effective Date"), String.Empty, String.Format("{0:MM/dd/yyyy}", Date.Parse(dr(0).Item("Effective Date"))))
        n.Email = IIf((dr(0).IsNull("Email")), String.Empty, dr(0).Item("Email"))
        n.ExtnNo = IIf(dr(0).IsNull("Extn No"), String.Empty, dr(0).Item("Extn No"))
        n.Name = IIf(dr(0).IsNull("Name"), String.Empty, dr(0).Item("Name"))
        n.Telephone = IIf(dr(0).IsNull("Phone No"), String.Empty, dr(0).Item("Phone No"))
        n.Title = IIf(dr(0).IsNull("Title"), String.Empty, dr(0).Item("Title"))
        n.TitleType = IIf(dr(0).IsNull("chvPositionTitleCode"), String.Empty, dr(0).Item("chvPositionTitleCode"))

        '------------'Shashi Shekhar:2010-03-05:YRS-5.0-942---------------------------------------------
        n.FundNo = IIf((dr(0).IsNull("Fund No")), String.Empty, dr(0).Item("fund No"))
        n.FirstName = IIf((dr(0).IsNull("First Name")), String.Empty, dr(0).Item("First Name"))
        n.MiddleName = IIf((dr(0).IsNull("Middle Name")), String.Empty, dr(0).Item("Middle Name"))
        n.LastName = IIf((dr(0).IsNull("Last Name")), String.Empty, dr(0).Item("Last Name"))

        '--------------------------------------------------------

        n.OfficerId = n.OfficerId.Trim
        n.EffectiveDate = n.EffectiveDate.Trim
        n.Email = n.Email.Trim
        n.ExtnNo = n.ExtnNo.Trim
        n.Name = n.Name.Trim
        n.Telephone = n.Telephone.Trim
        n.Title = n.Title.Trim
        n.TitleType = n.TitleType.Trim

        '---------------'Shashi Shekhar:2010-03-05:YRS-5.0-942--------------
        n.FundNo = n.FundNo
        n.FirstName = n.FirstName.Trim
        n.MiddleName = n.MiddleName.Trim
        n.LastName = n.LastName.Trim

        '------------------------------------------------------

        Return n

    End Function

    Private Function ClearControls()
        '----------------------------------------------------
        'Shashi Shekhar:2010-03-04
        Me.TextboxFname.Text = String.Empty
        Me.TextboxLname.Text = String.Empty
        Me.TextboxMname.Text = String.Empty
        Me.TextboxFundNo.Text = String.Empty
        Me.TextboxTelephone.Text = String.Empty
        Me.TextboxTitle.Text = String.Empty
        Me.TextBoxOficersEmail.Text = String.Empty
        Me.TextboxExtnNo.Text = String.Empty
        '--------------------------------------------------------
    End Function

    Private Function ClearSearchControls()
        dg.Visible = False
        Me.TextBoxListFundNo.Text = String.Empty
        Me.TextBoxLastName.Text = String.Empty
        Me.TextBoxFirstName.Text = String.Empty
        Me.LabelNoneRecord.Visible = False
        Me.LabelNoRecord.Visible = False
        Me.lbl_Search_MoreItems.Visible = False
    End Function

    Private Function SearchOfficer(ByVal parameterFundNo As String, ByVal paramterLastName As String, ByVal parameterFirstName As String)
        Dim l_DataSet As DataSet
        Dim SM As YMCAUI.SessionManager.SessionHandler
        Try
            If Not Session("SessionManager") Is Nothing Then
                SM = CType(Session("SessionManager"), YMCAUI.SessionManager.SessionHandler)
            Else
                SM = New YMCAUI.SessionManager.SessionHandler
            End If

            If SM.YMCAMaintenance Is Nothing Then
                SM.YMCAMaintenance = New YMCAUI.SessionManager.YMCAMaintenance
            End If

            ' SM = DirectCast(Session("SessionManager"), YMCAUI.SessionManager.YMCAMaintenance)
            g_dataset_dsOfficer = YMCARET.YmcaBusinessObject.AddOfficerBOClass.LookUpOfficer(parameterFundNo, paramterLastName, parameterFirstName, Session("GeneralYMCANo"))
            'viewstate("RefundRequest_Sort") = g_dataset_dsOfficer
            If (Not IsNothing(g_dataset_dsOfficer)) Then
                If g_dataset_dsOfficer.Tables(0).Rows.Count = 0 Then
                    'No records found for either First or Second phase search so display message "No records found"
                    LabelNoneRecord.Visible = False
                    lbl_Search_MoreItems.Visible = False
                    Me.LabelNoRecord.Visible = True
                    SM.YMCAMaintenance.OfficerSearchCancel = True
                    Session("SessionManager") = SM
                    'Shashi Shekhar:2010-04-12:For BT-499
                    dg.DataSource = Nothing
                    dg.DataBind()

                Else 'Records found -- below is check for records belonging to which phase search result.

                    If g_dataset_dsOfficer.Tables(0).Columns.Contains("City") Then
                        'If Datatable contains "City" column name and record count of datatable is more than 0 then 
                        'Grid is bind with second phase search which include three more column to display i.e(City,State,Fund status).
                        LabelNoneRecord.Visible = True
                        Me.LabelNoRecord.Visible = False    'Shashi Shekhar:2010-04-12:For BT-499

                        If dg.Columns.Count <= 5 Then
                            AddDataGridCol()
                        End If


                        dg.DataSource = g_dataset_dsOfficer
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
                        Me.LabelNoRecord.Visible = False    'Shashi Shekhar:2010-04-12:For BT-499
                        dg.DataSource = g_dataset_dsOfficer
                        dg.DataBind()
                    End If

                End If
            End If
        Catch ex As Exception
            MessageBox.Show(Me.MessageBoxPlaceHolder, " YMCA - YRS", ex.Message, MessageBoxButtons.OK, False)
        Finally
            SM = Nothing

        End Try

    End Function

    'If Data found in First phase search, then remove extra column from grid which are used for second phase search.
    Private Function RemoveDataGridCol()

        For i As Integer = dg.Columns.Count - 1 To 0 Step -1
            If dg.Columns(i).HeaderText = "City" Or dg.Columns(i).HeaderText = "State" Or dg.Columns(i).HeaderText = "Fund Status" Then
                dg.Columns.RemoveAt(i)
            End If
        Next

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


#End Region


End Class



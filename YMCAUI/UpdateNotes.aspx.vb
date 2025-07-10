'*******************************************************************************
' Copyright YMCA Retirement Fund All Rights Reserved. 
'
' Project Name		            :	YMCA_YRS
' FileName			            :	UpdateNotes.aspx.vb
' Author Name		            :	
' Employee ID		            :	
' Email				            :	  
' Contact No		            :	
' Creation Time		            :	
' Program Specification Name	:	
' Unit Test Plan Name			:	
' Description					:	To Add Notes to Notes Tab 
' Cache-Session   
'*******************************************************************************
'Changed By:            On:             IssueId: 
'Aparna Samala          06/03/2007      YREN-3115

' Cache-Session         : Hafiz 04Feb06

'Hafiz 04Feb06 Cache-Session
'Imports Microsoft.Practices.EnterpriseLibrary.Caching
'Hafiz 04Feb06 Cache-Session
'*********************************************************************************************************************
'Modification History
'*********************************************************************************************************************
'Modified By                Date                        Description
'*********************************************************************************************************************
'Ashutosh Patil             25-May-2007                 Changes related to IE7 Messagebox issue.
'Swopna                     19-May-2008                 BT-393
'Neeraj Singh               12/Nov/2009                 Added form name for security issue YRS 5.0-940 
'Shashi Shekhar             27-Dec-2010                 For YRS 5.0-450, BT-643 : Replace SSN with Fund Id in title on all screens.
'Shashi                     04 Mar. 2011                Replacing Header formating with user control (YRS 5.0-450 )
'Anudeep A                  22 sep-2012                 BT-1126/Additional change YRS 5.0-1246 : : Handling DLIN records
'Bala                       12 feb-2016                 YRS-AT-1718 -  Adding Notes - YMCA Maintenance
'*********************************************************************************************************************

Public Class UpdateNotes
    Inherits System.Web.UI.Page
    'below line is Added by Neeraj for issue id YRS 5.0-940 
    Dim strFormName As String = New String("UpdateNotes.aspx")
    'End issue id YRS 5.0-940

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    'Start: 27/12/2015: YRS-AT-1718: Ok button changed to save
    'Protected WithEvents ButtonOK As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonSave As System.Web.UI.WebControls.Button
    'End: 27/12/2015: YRS-AT-1718: Ok button changed to save
    Protected WithEvents ButtonCancel As System.Web.UI.WebControls.Button
    Protected WithEvents txtNotes As System.Web.UI.WebControls.TextBox
    Protected WithEvents CheckBoxImportant As System.Web.UI.WebControls.CheckBox
    Protected WithEvents LabelImportant As System.Web.UI.WebControls.Label

    Protected WithEvents PlaceHolderMessage As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents Headercontrol As YMCA_Header_WebUserControl
    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region
    'Start: Bala: 01/12/2016: YRS-AT-1718: Property added
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
    'End: Bala: 01/12/2016: YRS-AT-1718: Property added
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Start: Bala: 01/12/2016: YRS-AT-1718: Check if the loggin user is valid or not.
        Dim logginId As String
        If Not Session("LoggedUserKey") Is Nothing Then
            logginId = Session("LoggedUserKey")
        ElseIf Not MyBase.Session("LoggedUserKey") Is Nothing Then
            logginId = MyBase.Session("LoggedUserKey").ToString
        End If
        If logginId Is Nothing Then
            'End: Bala: 01/12/2016: YRS-AT-1718: Check if the loggin user is valid or not.
            Response.Redirect("Login.aspx", False)
        End If
        Try
            'Put user code to initialize the page here
            If Not IsPostBack Then

                'Shashi: 04 Mar. 2011: Replacing Header formating with user control (YRS 5.0-450 )
                'Start: Bala: YRS-AT-1718: Title changed as Add/View.
                If Not Session("FundNo") Is Nothing Then
                    'Headercontrol.PageTitle = "Add/Update Notes"
                    Headercontrol.PageTitle = "Add/View Notes"
                    Headercontrol.FundNo = Session("FundNo").ToString().Trim()
                ElseIf Not Session("YMCA General") Is Nothing Then
                    Dim g_DataSetYMCAGeneral As DataSet = Session("YMCA General")
                    Dim dr As DataRow = g_DataSetYMCAGeneral.Tables(0).Rows(0)
                    Headercontrol.CustomTitle = String.Format("Add/View Notes -- {0}, YMCA No: {1}", dr("chvymcaname").ToString().Trim, dr("chrymcano").ToString().Trim)
                End If
                'End: Bala: YRS-AT-1718: Title changed as Add/View.
                '-----------------------------------------------------------------------------

                If Not Request.QueryString("UniqueID") Is Nothing Or Not Request.QueryString("Index") Is Nothing Then
                    txtNotes.Text = Session("Note")
                    'Start: 27/12/2015: YRS-AT-1718: Ok Button changed as Save.
                    'ButtonOK.Enabled = False
                    ButtonSave.Enabled = False
                    'End: 27/12/2015: YRS-AT-1718: Ok Button changed as Save.
                    txtNotes.ReadOnly = True
                    Me.CheckBoxImportant.Enabled = False
                    If CType(Session("BitImportant"), Boolean) = True Then
                        Me.CheckBoxImportant.Checked = True
                    Else
                        Me.CheckBoxImportant.Checked = False
                    End If


                End If
                'End If

            End If
        Catch ex As Exception
            Throw
        End Try

    End Sub

    'Start: Bala : 12/01/2016: YRS-AT-1718: Ok Button changed as Save.
    'Private Sub ButtonOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonOK.Click
    Private Sub ButtonSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonSave.Click
        'End: : Bala : 12/01/2016: YRS-AT-1718: Ok Button changed as Save.
        Try


            If Me.txtNotes.Text = "" Then
                'Commented By Ashutosh Patil as on 25-May-2007
                'MessageBox.Show(Me.PlaceHolderMessage, " YMCA - YRS", "Notes cannot be Empty.  Please enter the Notes.", MessageBoxButtons.OK, True)
                'Commented/Added by Swopna -BT-393-19May08-----start
                'MessageBox.Show(Me.PlaceHolderMessage, " YMCA - YRS", "Notes cannot be Empty.  Please enter the Notes.", MessageBoxButtons.OK)
                'Commented by Anudeep for bt-1126
                'MessageBox.Show(Me.PlaceHolderMessage, " YMCA - YRS", "Notes cannot be Empty.", MessageBoxButtons.OK)
                MessageBox.Show(Me.PlaceHolderMessage, " YMCA - YRS", Resources.Updatenotes.MESSAGE_UPDATE_NOTES_CANNOT_BE_EMPTY, MessageBoxButtons.OK)
                'Commented/Added by Swopna -BT-393-19May08-----end
                Exit Sub
            End If
            'Start: Bala : 12/01/2016: YRS-AT-1718
            'Added by Atul Deo on 10th Oct for Retirees
            'If Request.QueryString("UniqueID") Is Nothing And Request.QueryString("Index") Is Nothing Then
            '    Session("blnAddNotes") = True
            'End If
            'End: Bala : 12/01/2016: YRS-AT-1718

            'End Add
            Dim msg As String
            Dim l_datatable_Notes As New DataTable

            Dim drRows() As DataRow

            Dim drUpdated As DataRow



            'Hafiz 04Feb06 Cache-Session
            'Dim l_CacheManager As CacheManager
            'l_CacheManager = CacheFactory.GetCacheManager
            'Hafiz 04Feb06 Cache-Session
            'Start: Bala : 12/01/2016: YRS-AT-1718
            'Session("Note") = txtNotes.Text 'Bala : 12/01/2016: YRS-AT-1718
            'by Aparna YREN-3115 
            'If the mode is add then need the session
            'If Me.CheckBoxImportant.Checked = True Then
            '    Session("BitImportant") = 1
            'Else
            '    Session("BitImportant") = 0
            'End If
            'End: Bala : 12/01/2016: YRS-AT-1718

            If Not Request.QueryString("UniqueID") Is Nothing Then

                'Hafiz 04Feb06 Cache-Session
                'l_datatable_Notes = CType(l_CacheManager.GetData("dtNotes"), DataTable)
                l_datatable_Notes = DirectCast(Session("dtNotes"), DataTable)
                'Hafiz 04Feb06 Cache-Session

                If Not IsNothing(l_datatable_Notes) Then
                    drRows = l_datatable_Notes.Select("UniqueID='" & Request.QueryString("UniqueID") & "'")
                    drUpdated = drRows(0)
                    drUpdated("Note") = Me.txtNotes.Text
                    drUpdated("Date") = Date.Now()
                    'Aparna YREN-3115 New Column in AtsNotes
                    Me.CheckBoxImportant.Enabled = False
                    If Me.CheckBoxImportant.Checked = True Then
                        drUpdated("bitImportant") = 1
                    Else
                        drUpdated("bitImportant") = 0
                    End If

                    'Aparna YREN-3115 
                    Session("blnUpdateNotes") = True
                    'Hafiz 04Feb06 Cache-Session
                    'l_CacheManager.Add("dtNotes", l_datatable_Notes)
                    Session("dtNotes") = l_datatable_Notes
                    'Hafiz 04Feb06 Cache-Session
                End If
            End If


            If Not Request.QueryString("Index") Is Nothing Then

                'Hafiz 04Feb06 Cache-Session
                'l_datatable_Notes = CType(l_CacheManager.GetData("dtNotes"), DataTable)
                l_datatable_Notes = DirectCast(Session("dtNotes"), DataTable)
                'Hafiz 04Feb06 Cache-Session

                If Not IsNothing(l_datatable_Notes) Then

                    drUpdated = l_datatable_Notes.Rows(Request.QueryString("Index"))
                    drUpdated("Note") = Me.txtNotes.Text
                    drUpdated("Date") = Date.Now()
                    'Aparna YREN-3115 New Column in AtsNotes
                    Me.CheckBoxImportant.Enabled = False
                    If drUpdated("bitImportant") = 1 Then
                        Me.CheckBoxImportant.Checked = True
                    Else
                        Me.CheckBoxImportant.Checked = False
                    End If

                    'Aparna YREN-3115 
                    Session("blnUpdateNotes") = True
                    'Hafiz 04Feb06 Cache-Session
                    'l_CacheManager.Add("dtNotes", l_datatable_Notes)
                    Session("dtNotes") = l_datatable_Notes
                    'Hafiz 04Feb06 Cache-Session
                End If
            Else
                Session("Flag") = "AddNotes"
                'Start: Bala: 12/01/2016: Adding to the database here.
                NotesManagement.InsertNotes(Session("NotesEntityID"), txtNotes.Text, IIf(Me.CheckBoxImportant.Checked, True, False))
                Session("NotesEntityID") = ""
                'End: Bala: 12/01/2016: Adding to the database here.
            End If

            msg = msg + "<Script Language='JavaScript'>"

            'Vipul 06Feb 06 - To Fix the issue of parent form losing the data
            'msg = msg + "window.opener.location.href=window.opener.location.href;"
            msg = msg + "window.opener.document.forms(0).submit();"
            'Vipul 06Feb 06 - To Fix the issue of parent form losing the data 


            msg = msg + "self.close();"

            msg = msg + "</Script>"
            Response.Write(msg)

        Catch ex As Exception
            Throw
        End Try

    End Sub

    Private Sub ButtonCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonCancel.Click
        Try
            Session("blnUpdateNotes") = False
            Session("blnAddNotes") = False
            Session("Note") = False
            'By Aparna YREN-3115
            Session("BitImportant") = False
            Session("Flag") = ""

            Dim closeWindow As String = "<script language='javascript'>" & _
                                                          "self.close();" & _
                                                          "</script>"

            If (Not Me.IsStartupScriptRegistered("CloseWindow")) Then
                Page.RegisterStartupScript("CloseWindow", closeWindow)
            End If
        Catch ex As Exception
            Throw
        End Try
    End Sub
End Class

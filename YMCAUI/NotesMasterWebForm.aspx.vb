'*******************************************************************************
' Copyright YMCA Retirement Fund All Rights Reserved. 
'
' Project Name		:	YMCA - YRS
' FileName			:	NotesMasterWebForm.aspx.vb
' Author Name		:	Srimurugan G
' Employee ID		:	32365
' Email				:	srimurugan.ag@icici-infotech.com
' Contact No		:	8744
' Creation Time		:	9/29/2005 2:54:35 PM
' Program Specification Name	:	
' Unit Test Plan Name			:	
' Description					:	<<Please put the brief description here...>>
' Cache-Session                 : Hafiz 03Feb06
'*******************************************************************************
'Hafiz 03Feb06 Cache-Session
'Imports Microsoft.Practices.EnterpriseLibrary.Caching
'Hafiz 03Feb06 Cache-Session
'*******************************************************************************
'Modified by        Date                Description
'*******************************************************************************
'Aparna Samala      22/03/2007          YREN-3115
'Neeraj Singh       12/Nov/2009         Added form name for security issue YRS 5.0-940 
'Shashi             25 Feb 2011         Replacing Header formating with user control (YRS 5.0-450 )
'Manthan Rajguru    2015.09.16          YRS-AT-2550: YRS data cleanup: copyright comments and namespace/reference (Changed the Namespace/reference from 'Infotech' to 'YMCARET')
'*******************************************************************************

Public Class NotesMasterWebForm

    Inherits System.Web.UI.Page
    'below line is Added by Neeraj for issue id YRS 5.0-940 
    Dim strFormName As String = New String("NotesMasterWebForm.aspx")
    'End issue id YRS 5.0-940

    Dim m_String_Mode As String

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents ButtonCancel As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonOK As System.Web.UI.WebControls.Button
    Protected WithEvents TextBoxNotes As System.Web.UI.WebControls.TextBox
    'Protected WithEvents PlaceHolderMessage As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents LabelImportant As System.Web.UI.WebControls.Label
    Protected WithEvents CheckBoxImportant As System.Web.UI.WebControls.CheckBox
    'Protected WithEvents LabelHdr As System.Web.UI.WebControls.Label

    'Protected WithEvents Headercontrol As YMCA_Header_WebUserControl
    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    'To get / set the PersonID property.    
    Private Property SessonPersonID() As String
        Get
            If Not Session("PersonID") Is Nothing Then
                Return (CType(Session("PersonID"), String))
            Else
                Return String.Empty
            End If

        End Get
        Set(ByVal Value As String)
            Session("PersonID") = Value
        End Set
    End Property


    'To Keep the Refund Request Datagrid to Refresh 

    Private Property SessionIsRefundRequest() As Boolean
        Get
            If Not (Session("IsRefundRequest")) Is Nothing Then
                Return (CType(Session("IsRefundRequest"), Boolean))
            Else
                Return False
            End If
        End Get
        Set(ByVal Value As Boolean)
            Session("IsRefundRequest") = Value
        End Set
    End Property


    'To get / set the Calling from, which form.    
    Private Property SessionCallingFrom() As String
        Get
            If Not Session("CallFrom") Is Nothing Then
                Return (CType(Session("CallFrom"), String))
            Else
                Return String.Empty
            End If
        End Get
        Set(ByVal Value As String)
            Session("CallFrom") = Value
        End Set
    End Property

    'To get / set the Calling from, which form.    
    Private Property SessionMode() As String
        Get
            If Not Session("Mode") Is Nothing Then
                Return (CType(Session("Mode"), String))
            Else
                Return String.Empty
            End If
        End Get
        Set(ByVal Value As String)
            Session("Mode") = Value
        End Set
    End Property

    'To get the selected index of Notes DataGrid.
    Private ReadOnly Property SessionNotesIndex() As Integer
        Get
            If Not Session("NotesIndex") Is Nothing Then
                Return (CType(Session("NotesIndex"), Integer))
            Else
                Return -1
            End If
        End Get
    End Property


    'To Keep the Notes DataGrid in Refund Request Main Form.
    Private Property SessionIsNotes() As Boolean
        Get
            If Not (Session("IsNotes")) Is Nothing Then
                Return (CType(Session("IsNotes"), Boolean))
            Else
                Return False
            End If
        End Get
        Set(ByVal Value As Boolean)
            Session("IsNotes") = Value
        End Set
    End Property

    'To Keep the flag to Raise the Notes Popup window
    Private Property SessionIsNotesPopupAllowed() As Boolean
        Get
            If Not (Session("IsNotesPopupAllowed")) Is Nothing Then
                Return (CType(Session("IsNotesPopupAllowed"), Boolean))
            Else
                Return False
            End If
        End Get
        Set(ByVal Value As Boolean)
            Session("IsNotesPopupAllowed") = Value
        End Set
    End Property

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        '  Put user code to initialize the page here
        Dim LabelTitle As Label
        If Not IsPostBack Then
            'Shashi:25 Feb 2011: Replacing Header formating with user control (YRS 5.0-450 )
            'Headercontrol.pageTitle = "Notes"
            'Headercontrol.guiPerssId = Me.SessonPersonID.Trim
            LabelTitle = Master.FindControl("lblPopupmodulename")
            If LabelTitle IsNot Nothing Then
                LabelTitle.Text = "Notes"
            End If

        End If
        If Session("LoggedUserKey") Is Nothing Then
            Response.Redirect("Login.aspx", False)
        End If
        If Not (Page.Request.QueryString("CF")) Is Nothing Then
            SessionCallingFrom = CType(Page.Request.QueryString("CF"), String)
        End If

        If Not (Page.Request.QueryString("MD")) Is Nothing Then '--- MD  is Mode, in which mode the form should Open. 
            Me.ButtonOK.Enabled = False
            Me.TextBoxNotes.ReadOnly = True
            Me.CheckBoxImportant.Enabled = False
            Me.ShowNotes()
        End If

        Me.SessionIsNotesPopupAllowed = False

    End Sub

    Private Function ShowNotes()
        '' This segment is used to Show the Notes for selected Person.
        '' Here the concept is, the Member Note Table is already in Cache,
        '' So, Here i am picking the Table (Not from Database) and showing the Message.

        'Hafiz 03Feb06 Cache-Session
        'Dim l_CacheManager As CacheManager
        'Hafiz 03Feb06 Cache-Session
        Dim l_DataTable As DataTable
        Dim l_DataRow As DataRow

        Try

            'Hafiz 03Feb06 Cache-Session
            'l_CacheManager = CacheFactory.GetCacheManager()
            'l_DataTable = CType(l_CacheManager.GetData("MemberNotes"), DataTable)
            l_DataTable = CType(Session("MemberNotes"), DataTable)
            'Hafiz 03Feb06 Cache-Session

            If Me.SessionNotesIndex = -1 Then Return 0

            If Not (l_DataTable) Is Nothing Then

                If l_DataTable.Rows.Count >= Me.SessionNotesIndex Then

                    l_DataRow = l_DataTable.Rows.Item(Me.SessionNotesIndex)

                    If Not l_DataRow Is Nothing Then
                        Me.TextBoxNotes.Text = CType(l_DataRow("Note"), String)

                        'Aparna -YREN-3115 11/03/2007
                        If CType(l_DataRow.Item("bitImportant"), Boolean) = True Then
                            Me.CheckBoxImportant.Checked = True
                        Else
                            Me.CheckBoxImportant.Checked = False
                        End If
                    End If

                End If

            End If

        Catch ex As Exception
            Throw ex
        End Try


    End Function

    Private Sub ButtonOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonOK.Click
        Try
            Dim l_bitImportant As Boolean
        '' Add the Notes, when the Notes is calling from the Refund Request Form.
        If Me.SessionCallingFrom = "RR" Then        '--- RR - Refun Request
            If Me.SessonPersonID <> String.Empty Then

                If Me.TextBoxNotes.Text.Trim.Length > 0 Then
                    'By Aparna YREN-3115 15/03/2007
                    If Me.CheckBoxImportant.Checked = True Then
                            l_bitImportant = True
                    Else
                            l_bitImportant = False
                    End If
                        YMCARET.YmcaBusinessObject.NotesBOClass.InsertNotes(Me.SessonPersonID, Me.TextBoxNotes.Text, l_bitImportant)

                    Me.ParentFormReload()

                Else
                        'last parameter changed to False by Anita 31-05-2007
                        HelperFunctions.ShowMessageToUser("Notes cannot be Empty.  Please enter the Notes.", EnumMessageTypes.Error)
                End If
            Else
                    HelperFunctions.ShowMessageToUser("No Member is Selected. Please select a Member to add Notes.", EnumMessageTypes.Error)
            End If


        End If

        Catch ex As Exception

        End Try

    End Sub


    Private Sub ParentFormReload()


        Me.SessionIsNotes = True

        'Dim closeWindowRR As String = "<script language='javascript'>" & _
        '                       "self.close(); window.opener.location.reload();" & _
        '                       "</script>"

        'If (Not Me.IsStartupScriptRegistered("CloseWindowRR")) Then
        '    Page.RegisterStartupScript("CloseWindowRR", closeWindowRR)
        'End If

        'Me.Response.Write("<script language='javascript'> { window.opener.document.forms(0).submit(); self.close(); }</script>")
        'Me.Response.Write("<script language='javascript'> { self.close() }</script>")



        Me.SessionIsRefundRequest = True
        Session("Reload") = True

        ' Response.Write("<script> window.opener.location.reload(); self.close();" & Chr(60) & "/script>")

        Response.Write("<script> window.opener.document.forms(0).submit(); self.close(); </script>")


    End Sub

    Private Sub ButtonCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonCancel.Click
        Me.SessionIsNotesPopupAllowed = True
        Me.Response.Write("<script language='javascript'> { self.close() }</script>")
    End Sub
    
End Class

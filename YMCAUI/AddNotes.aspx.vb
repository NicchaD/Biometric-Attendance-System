'*******************************************************************************
' Copyright YMCA Retirement Fund All Rights Reserved. 
'
' Project Name		:	YMCA_YRS
' FileName			:	AddNotes.aspx.vb
' Author Name		:	Ruchi Saxena
' Employee ID		:	33494
' Email				:	ruchi.saxena@3i-infotech.com
' Contact No		:	8751
' Creation Time		:	6/13/2005 4:48:19 PM
' Program Specification Name	:	
' Unit Test Plan Name			:	
' Description					:	pop up for Participants Information
'*******************************************************************************
'****************************************************
'Modification History
'****************************************************
'Modified by          Date      Description
'****************************************************
'Vipul              03Feb06     Cache-Session
'NP/PP/SR           2009.05.18  Optimizing the YMCA Screen
'Neeraj Singh       12/Nov/2009 Added form name for security issue YRS 5.0-940 
'Priya              2010-06-03  Changes made for enhancement in vs-2010 
'Anudeep            2012.10.09  To show the label as View notes and Add Notes
'Shashank Patel     2014.10.13  BT-1995\YRS 5.0-2052: Erroneous updates occuring
'*******************************************************************************
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

Public Class AddNotes
    Inherits System.Web.UI.Page
    Protected WithEvents LabelImportant As System.Web.UI.WebControls.Label
    Protected WithEvents CheckBoxImportant As System.Web.UI.WebControls.CheckBox
    'below line is Added by Neeraj for issue id YRS 5.0-940 
    Dim strFormName As String = New String("AddNotes.aspx")
    'End issue id YRS 5.0-940

    Dim Page_Mode As String = String.Empty
#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents TextBoxNotes As System.Web.UI.WebControls.TextBox
    Protected WithEvents ButtonOK As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonCancel As System.Web.UI.WebControls.Button
    Protected WithEvents LabelNotes As System.Web.UI.WebControls.Label
    Protected WithEvents PlaceHolderMessage As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents lblHeadtext As System.Web.UI.WebControls.Label 'Added by Anudeep to show the label as View notes and Add Notes
    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

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

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        Me.LabelNotes.AssociatedControlID = Me.TextBoxNotes.ID
        If MyBase.Session("LoggedUserKey") Is Nothing Then
            Response.Redirect("Login.aspx", False)
        End If
        Dim noteId As String = Request("NoteId")


        Try
            If Page.IsPostBack = False Then

                Dim n As Notes = GetNoteById(noteId)
                If n Is Nothing Then
                    Throw New Exception("Invalid Note Id passed. Please close this window and try again.")
                End If
                CheckBoxImportant.Checked = n.BitImportant
                TextBoxNotes.Text = n.NotesText
                If noteId Is Nothing OrElse noteId = String.Empty Then
                    Page_Mode = "ADD"
                    lblHeadtext.Text = "Add Notes" 'Added by Anudeep to show the label as View notes and Add Notes
                    CheckBoxImportant.Enabled = True
                    TextBoxNotes.Enabled = True
                Else
                    Page_Mode = "VIEW"
                    lblHeadtext.Text = "View Notes" 'Added by anudeep to show the label as View notes and Add Notes
                    CheckBoxImportant.Enabled = False
                    TextBoxNotes.Enabled = False
                End If
            End If
       

        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = ex.Message.Trim.ToString()
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message, False)
            'Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
        End Try
    End Sub

    Private Sub ButtonOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonOK.Click
        Dim objNotes As Notes = New Notes
        Dim objPopupAction As PopupResult = New PopupResult
        Dim msg As String = String.Empty
        Try
            'By Aparna Toavoid empty note being added 21/03/2007
            If Me.TextBoxNotes.Text = "" Then
                MessageBox.Show(Me.PlaceHolderMessage, " YMCA - YRS", "Note cannot be Empty.  Please enter the Note.", MessageBoxButtons.OK, True)
                Exit Sub
            End If


            objNotes.NotesText = Me.TextBoxNotes.Text
            objNotes.BitImportant = IIf(CheckBoxImportant.Checked, 1, 0)


            objPopupAction.Page = "NOTES"
            If Page_Mode = "ADD" Then
                objPopupAction.Action = PopupResult.ActionTypes.ADD
            Else
                objPopupAction.Action = PopupResult.ActionTypes.CANCEL
            End If
            objPopupAction.State = objNotes
            Session("PopUpAction") = objPopupAction


            msg = msg + "<Script Language='JavaScript'>"
            msg = msg + "window.opener.document.forms(0).submit();" '"window.opener.location.href=window.opener.location.href;"
            msg = msg + "self.close();"
            msg = msg + "</Script>"
            Response.Write(msg)
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = ex.Message.Trim.ToString()
            'Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message, False)
        End Try
    End Sub

    Private Sub ButtonCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonCancel.Click

        Dim objPopupAction As PopupResult = New PopupResult
        Dim msg As String = String.Empty

        Try

            objPopupAction.Page = "NOTES"
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
            l_String_Exception_Message = ex.Message.Trim.ToString()
            'Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message, False)
        End Try
    End Sub

    Private Function GetNoteById(ByVal noteId As String) As Notes
        Dim n As Notes
        If noteId = Nothing OrElse noteId = String.Empty Then
            n = New Notes
            n.BitImportant = False
            n.NotesText = String.Empty
            Return n
        End If
        'If Session("YMCA Notes") = Nothing Then Return Nothing
        If IsNothing(Session("YMCA Notes")) Then Return Nothing
        Dim ds As DataSet = DirectCast(Session("YMCA Notes"), DataSet)
        If HelperFunctions.isEmpty(ds) Then Return Nothing
        Dim dr As DataRow()
        dr = ds.Tables(0).Select("guiUniqueId = '" & noteId & "'")
        If dr Is Nothing OrElse dr.Length = 0 Then Return Nothing
        n = New Notes
        'NPTODO: Handle Null values here
        n.NoteId = Convert.ToString(dr(0).Item("guiUniqueID")).Trim
        n.BitImportant = dr(0).Item("bitImportant")
        n.NotesText = dr(0).Item("First Line of Notes")
        Return n
    End Function

#Region "Persistence Mechanism"
    Protected Overrides Sub LoadViewState(ByVal savedState As Object)
        MyBase.LoadViewState(savedState)
        Page_Mode = ViewState("Page_Mode")
    End Sub
    Protected Overrides Function SaveViewState() As Object
        ViewState("Page_Mode") = Page_Mode
        Return MyBase.SaveViewState()
    End Function
#End Region

End Class

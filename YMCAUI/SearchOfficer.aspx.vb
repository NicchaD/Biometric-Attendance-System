'*******************************************************************************
' Copyright YMCA Retirement Fund All Rights Reserved. 
'
' Project Name		:	YMCA_YRS
' FileName			:	SearchOfficer.aspx.vb
' Author Name		:	Shashi Shekhar Singh
' Employee ID		:	51426
' Email				:	shashi.singh@3i-infotech.com    
' Contact No		:	8684
' Creation Time		:	03-Mar-2010
' Program Specification Name	:	
' Unit Test Plan Name			:	
' Description					:	<<Please put the brief description here...>>
'*******************************************************************************
'Modification History
'********************************************************************************************************************************
'Modified By        Date            Description
'********************************************************************************************************************************
' Shashi Shekhar    2010-04-12      For BT-499
'Shashi Shekhar     2010-04-13      Changes made for Gemini-1051
'Shashi Shekhar     2010-04-20      Changes made for YRS-5.0-1057
'Priya              2010-06-03      Changes made for enhancement in vs-2010 
'Manthan Rajguru    2015.09.16      YRS-AT-2550: YRS data cleanup: copyright comments and namespace/reference (Changed the Namespace/reference from 'Infotech' to 'YMCARET')
'********************************************************************************************************************************
Imports System.Data
Imports System.Data.SqlClient
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling


Public Class SearchOfficer
    Inherits System.Web.UI.Page
    Dim g_bool_flagTitleCancel As Boolean
    Dim g_dataset_dsOfficer As DataSet
    Dim g_integer_count As Integer



#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub


    Protected WithEvents DataGridList As System.Web.UI.WebControls.DataGrid
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
    Protected WithEvents ButtonCancel As System.Web.UI.WebControls.Button

    Protected WithEvents RefundMultiPage As Microsoft.Web.UI.WebControls.MultiPage
    Protected WithEvents LabelNoRecord As System.Web.UI.WebControls.Label



    'Added by Shubhrata May 21st,2007
    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        If Session("LoggedUserKey") Is Nothing Then
            Response.Redirect("Login.aspx", False)
        End If

        Try
            If Page.IsPostBack() Then

            End If

        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            'Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message, False)
        End Try
    End Sub

    Private Sub ButtonFind_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonFind.Click
        Try

            If Me.TextBoxListFundNo.Text.Trim.Length < 1 And _
                Me.TextBoxLastName.Text.Trim.Length < 1 And _
                Me.TextBoxFirstName.Text.Trim.Length < 1 Then
                MessageBox.Show(Me.MessageBoxPlaceHolder, " YMCA - YRS", "Please enter search criteria.", MessageBoxButtons.Stop, False)
                Return
            End If
            Me.SearchOfficer(Me.TextBoxListFundNo.Text.Trim, Me.TextBoxLastName.Text.Trim, Me.TextBoxFirstName.Text.Trim)
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            'Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message, False)
        End Try

    End Sub

    Private Function SearchOfficer(ByVal parameterFundNo As String, ByVal paramterLastName As String, ByVal parameterFirstName As String)

        Dim l_DataSet As DataSet
        Dim SM As YMCAUI.SessionManager.SessionHandler

        Try

            If Not Session("SessionManager") Is Nothing Then
                SM = DirectCast(Session("SessionManager"), YMCAUI.SessionManager.SessionHandler)
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
                    Me.LabelNoRecord.Visible = True
                    SM.YMCAMaintenance.OfficerSearchCancel = True
                    Session("SessionManager") = SM
                    'Shashi Shekhar:2010-04-12:For BT-499
                    DataGridList.DataSource = Nothing
                    DataGridList.DataBind()

                Else
                    Me.LabelNoRecord.Visible = False    'Shashi Shekhar:2010-04-12:For BT-499
                    DataGridList.DataSource = g_dataset_dsOfficer
                    DataGridList.DataBind()
                End If
            End If
        Catch ex As Exception
            MessageBox.Show(Me.MessageBoxPlaceHolder, " YMCA - YRS", ex.Message, MessageBoxButtons.OK, False)
        Finally
            SM = Nothing

        End Try

    End Function

    Private Sub ButtonClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonClear.Click
        Try
            Me.TextBoxFirstName.Text = String.Empty
            Me.TextBoxLastName.Text = String.Empty
            Me.TextBoxListFundNo.Text = String.Empty

        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            'Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message, False)
        End Try
    End Sub

    Private Sub DataGridList_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DataGridList.SelectedIndexChanged
        Dim l_Dataset As DataSet
        Dim l_DataTable As DataTable
        Dim l_DataRow As DataRow
        Dim l_StringPositionType As String
        Dim l_StringShortDesc As String
        Dim SM As YMCAUI.SessionManager.SessionHandler
        'Dim SM As YMCAUI.SessionManager.YMCAMaintenance

        Dim l_FundNo As String
        Dim expr As String
        Dim msg As String
        Dim dr() As DataRow
        msg = ""

        Try
            If Not Session("SessionManager") Is Nothing Then
                SM = DirectCast(Session("SessionManager"), YMCAUI.SessionManager.SessionHandler)
            Else
                SM = New YMCAUI.SessionManager.SessionHandler
            End If

            If SM.YMCAMaintenance Is Nothing Then
                SM.YMCAMaintenance = New YMCAUI.SessionManager.YMCAMaintenance
            End If

            ''     SM.YMCAMaintenance = New YMCAUI.SessionManager.YMCAMaintenance

            SM.YMCAMaintenance.OfficerSearchCancel = False

            ''Me.SearchOfficer(Me.TextBoxListFundNo.Text.Trim, Me.TextBoxLastName.Text.Trim, Me.TextBoxFirstName.Text.Trim)
            g_integer_count = DataGridList.SelectedIndex '(((DataGridAccountTypes.CurrentPageIndex) * DataGridAccountTypes.PageSize) + DataGridAccountTypes.SelectedIndex)

            ' Me.SearchOfficer(Me.TextBoxListFundNo.Text.Trim, Me.TextBoxLastName.Text.Trim, Me.TextBoxFirstName.Text.Trim)
            l_FundNo = DataGridList.SelectedItem.Cells(2).Text.Trim
            expr = "FundNo ='" + l_FundNo.ToString().Trim + "' "

            l_Dataset = g_dataset_dsOfficer
            dr = l_Dataset.Tables(0).Select(expr.ToString())

            If dr.Length > 0 Then
                l_DataRow = dr(0)
            Else
                l_DataRow = l_Dataset.Tables(0).Rows.Item(g_integer_count)
            End If


            SM.YMCAMaintenance.FundNo = IIf(IsDBNull(l_DataRow.Item("FundNo")), String.Empty, l_DataRow.Item("FundNo"))
            SM.YMCAMaintenance.FirstName = IIf(IsDBNull(l_DataRow.Item("FirstName")), String.Empty, l_DataRow.Item("FirstName"))
            SM.YMCAMaintenance.LastName = IIf(IsDBNull(l_DataRow.Item("LastName")), String.Empty, l_DataRow.Item("LastName"))
            SM.YMCAMaintenance.MiddleName = IIf(IsDBNull(l_DataRow.Item("MiddleName")), String.Empty, l_DataRow.Item("MiddleName"))
            SM.YMCAMaintenance.Title = IIf(IsDBNull(l_DataRow.Item("Title")), String.Empty, l_DataRow.Item("Title"))
            SM.YMCAMaintenance.TitleName = IIf(IsDBNull(l_DataRow.Item("TitleName")), String.Empty, l_DataRow.Item("TitleName"))

            'Shashi Shekhar:2010-04-13:Commented for Gemini-1051
            'SM.YMCAMaintenance.Email = IIf(IsDBNull(l_DataRow.Item("Email")), String.Empty, l_DataRow.Item("Email"))
            'SM.YMCAMaintenance.Extn = IIf(IsDBNull(l_DataRow.Item("ExtnNo")), String.Empty, l_DataRow.Item("ExtnNo"))
            'SM.YMCAMaintenance.Phone = IIf(IsDBNull(l_DataRow.Item("PhoneNo")), String.Empty, l_DataRow.Item("PhoneNo"))

            'If (SM.YMCAMaintenance.Extn = "NULL") Then
            '    SM.YMCAMaintenance.Extn = String.Empty
            'End If

            'Shashi Shekhar:2010-04-20:changes for YRS-5.0-1057
            If Session("Page_Mode") = "ADD" Then
                Session("TitleType") = Nothing
                Session("TitleName") = Nothing
            End If
            '-----------------------------------------------------

            Session("SessionManager") = SM


            msg = msg + "<Script Language='JavaScript'>"
            msg = msg + "window.opener.document.forms(0).submit();"
            msg = msg + "self.close();"
            msg = msg + "</Script>"
            Response.Write(msg)


        Catch ex As SqlException
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = ex.Message.Trim.ToString()
            Server.Transfer("ErrorPageForm.aspx?DBMessage=" + l_String_Exception_Message)

        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = ex.Message.Trim.ToString()
            'Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message, False)
        Finally
            SM = Nothing
            'Shashi Shekhar:2010-04-20:Changes for YRS-5.0-1057
            Session("Page_Mode") = Nothing
        End Try


    End Sub
    Private Sub DataGridList_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DataGridList.ItemCommand
        Try
            Dim l_button_Select As ImageButton
            l_button_Select = e.Item.FindControl("ImageButtonSelect")

            If (e.Item.ItemIndex = Me.DataGridList.SelectedIndex And Me.DataGridList.SelectedIndex >= 0) Then
                l_button_Select.ImageUrl = "images\selected.gif"
            End If
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            'Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message, False)
        End Try
    End Sub

    Private Sub DataGridList_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles DataGridList.SortCommand
        Try

            DataGridList.SelectedIndex = -1
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
                BindGrid(DataGridList, g_dataset_dsOfficer.Tables(0).DefaultView)

            End If

        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            'Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message, False)
        End Try
    End Sub
    Private Sub ButtonCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonCancel.Click
        Dim msg As String
        msg = ""
        Dim SM As YMCAUI.SessionManager.SessionHandler
        Try
            If Not Session("SessionManager") Is Nothing Then
                SM = DirectCast(Session("SessionManager"), YMCAUI.SessionManager.SessionHandler)
            Else
                SM = New YMCAUI.SessionManager.SessionHandler
            End If

            If SM.YMCAMaintenance Is Nothing Then
                SM.YMCAMaintenance = New YMCAUI.SessionManager.YMCAMaintenance
            End If

            SM.YMCAMaintenance.OfficerSearchCancel = True
            Session("SessionManager") = SM

        Catch ex As SqlException

            Dim l_String_Exception_Message As String
            l_String_Exception_Message = ex.Message.Trim.ToString()
            Server.Transfer("ErrorPageForm.aspx?DBMessage=" + l_String_Exception_Message)

        Catch ex As Exception

            Dim l_String_Exception_Message As String
            l_String_Exception_Message = ex.Message.Trim.ToString()
            'Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message, False)
        Finally
            SM = Nothing

        End Try

        msg = msg + "<Script Language='JavaScript'>"
        msg = msg + "window.opener.location.href=window.opener.location.href;"
        msg = msg + "self.close();"
        msg = msg + "</Script>"
        Response.Write(msg)

    End Sub
#Region "Local Variable Persistence mechanism"
    Protected Overrides Function SaveViewState() As Object
        'l_int_SelectedDataGridItem = DataGrid_Search.SelectedIndex
        Dim al As New ArrayList
        al.Add(StoreLocalVariablesToCache())
        al.Add(MyBase.SaveViewState())
        Return al
    End Function
    Protected Overrides Sub LoadViewState(ByVal savedState As Object)
        Dim al As ArrayList = CType(savedState, ArrayList)
        InitializeLocalVariablesFromCache(al.Item(0))
        MyBase.LoadViewState(al.Item(1))
    End Sub

    Private Sub InitializeLocalVariablesFromCache(ByRef obj As Object)
        'Session: Load data from Session so that it is accessible on Postback - This can be replaced by a load from viewstate or database
        g_dataset_dsOfficer = Session("DC_g_dataset_dsOfficer")
    End Sub
    Private Function StoreLocalVariablesToCache() As Object
        'Session: Pass data into Session so that it is accessible on Postback - This can be replaced by a save to viewstate
        Session("DC_g_dataset_dsOfficer") = g_dataset_dsOfficer
    End Function
#End Region
#Region "General Utility Functions"
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

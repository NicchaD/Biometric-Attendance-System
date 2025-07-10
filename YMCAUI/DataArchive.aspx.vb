'*******************************************************************************
' Copyright YMCA Retirement Fund All Rights Reserved. 
'
' Project Name		:	YMCA-YRS
' FileName			:	DataArchive.aspx.vb
' Author Name		:	Shashi Shekhar
' Employee ID		:	51426
' Email				:	shashi.singh23i-infotech.com
' Contact No		:	
' Creation Time		:	
' Program Specification Name	:	YMCA PS Data Archive.Doc
' Unit Test Plan Name			:	Data Archive - Test Cases .xls
' Description					:	This form is used to Retrieve the archived data
'*******************************************************************************
' Change history
'****************************************************
'Modification History
'****************************************************
' Modified by           Date        Description
'****************************************************
' Shashi Shekhar		2010.01.26	Added RetrieveData() to update the bitIsArchived field  in table "AtsPerss"
' Shashi Shekhar		2010.03.26	makes changes for BT-489,484.485,486
' Shashi Shekhar	    2010.04.07	Handling DataArchive error message (concate sql error message with user defined error message returned from procedure)
' Shashi Shekhar        2010-04-12  Ref:mail sent by Nikunj -Issues identified with 7.4.2 code release - Internally identified #1 - Remove ref output variable, which was used in RetrieveData method
' Shashi Shekhar        2010-06-03  Migration related changes.
' Shashi Shekhar        2010-07-07  Code review changes.
' Manthan Rajguru       2015.09.16  YRS-AT-2550: YRS data cleanup: copyright comments and namespace/reference (Changed the Namespace/reference from 'Infotech' to 'YMCARET')
' Pooja K               2019.02.28  YRS-AT-4248 -  YRS enh: YRS read-only access during MonthEnd Processing (TrackIT 35547) 
'****************************************************

Imports System.Data
Imports System.Data.SqlClient
Imports System.Web
Imports System.Web.SessionState
Imports Microsoft.Practices.EnterpriseLibrary.Data

Public Class DataArchive
    Inherits System.Web.UI.Page


#Region "Local Variables"
    Dim l_dataset_SearchResults As DataSet      'Store search results information of the search that was performed
    Protected WithEvents MultiPageDeathCalc As Microsoft.Web.UI.WebControls.MultiPage
    Dim l_dataset_Moved As DataSet 'Store information which was moved from search results.


#End Region

#Region "EnumMaxlength"
    Public Enum EnumMaxlength
        SSNo = 11   'NP:PS:2007.08.31 - Changing value from 9 to 11
        FirstName = 20
        LastName = 30
        City = 29
        State = 29



    End Enum
#End Region

#Region " Web Form Designer Generated Code "

    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    Protected WithEvents RegularExpressionValidator1 As System.Web.UI.WebControls.RegularExpressionValidator
    Protected WithEvents TextBoxFundNo As System.Web.UI.WebControls.TextBox
    Protected WithEvents lbl_Search_MoreItems As System.Web.UI.WebControls.Label
    '    Protected WithEvents DataGrid_Search As System.Web.UI.WebControls.DataGrid
    Protected WithEvents DataGrid_Search As System.Web.UI.WebControls.DataGrid
    Protected WithEvents DataGrid_Moved As System.Web.UI.WebControls.DataGrid
    Protected WithEvents LabelFormHead As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxSSNo As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxLastName As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxFirstName As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxCity As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxState As System.Web.UI.WebControls.TextBox

    '    Protected WithEvents TextBoxCalcDeathDate As System.Web.UI.WebControls.TextBox
    Protected WithEvents Menu1 As skmMenu.Menu
    Protected WithEvents ButtonFind As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonForms As System.Web.UI.WebControls.Button
    Protected WithEvents LabelTitle As System.Web.UI.WebControls.Label
    Protected WithEvents PlaceHolder1 As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents ButtonClear As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonCancel As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonOk As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonRetrieve As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonRemoveAll As System.Web.UI.WebControls.Button

    Protected WithEvents LabelNoDataFound As System.Web.UI.WebControls.Label
    Protected WithEvents ButtonSelectAll As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonMove As System.Web.UI.WebControls.Button
    Protected WithEvents trSearchGridHeader As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trMovedGridHeader As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents divGridSearch As System.Web.UI.HtmlControls.HtmlGenericControl
    Protected WithEvents divGridMove As System.Web.UI.HtmlControls.HtmlGenericControl
    Protected WithEvents hdNoOfMovedRec As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents hdCount As System.Web.UI.HtmlControls.HtmlInputHidden

    Protected WithEvents hrSerch As System.Web.UI.HtmlControls.HtmlGenericControl




    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region


    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        Try
            If Session("LoggedUserKey") Is Nothing Then
                Response.Redirect("Login.aspx", False)
            End If

            'Load menu items
            Menu1.DataSource = Server.MapPath("SimpleXML.xml")
            Menu1.DataBind()

            If IsPostBack AndAlso HelperFunctions.isNonEmpty(l_dataset_SearchResults) Then
                DataGrid_Search.DataSource = l_dataset_SearchResults.Tables(0).DefaultView
                DataGrid_Search.DataBind()
            Else
                DummyDataTable(DataGrid_Search)
            End If

            If IsPostBack AndAlso HelperFunctions.isNonEmpty(l_dataset_Moved) Then
                DataGrid_Moved.DataSource = l_dataset_Moved.Tables(0).DefaultView
                DataGrid_Moved.DataBind()
            Else
                DummyDataTable(DataGrid_Moved)
            End If
            CheckReadOnlyMode() 'PK| 02/28/2019 | YRS-AT-4248 | Check security method called here
            'Initializing Search control's max length
            Me.TextBoxSSNo.MaxLength = EnumMaxlength.SSNo
            Me.TextBoxFirstName.MaxLength = EnumMaxlength.FirstName
            Me.TextBoxLastName.MaxLength = EnumMaxlength.LastName
            Me.TextBoxCity.MaxLength = EnumMaxlength.City
            Me.TextBoxState.MaxLength = EnumMaxlength.State

            If Session("ConfirmationBoxResponse") = "ButtonRetrieveClick" Then
                If Request.Form("Yes") = "Yes" Then
                    'If the user confirms 
                    'call function to retrieve the data
                    RetrieveData()
                ElseIf Request.Form("No") = "No" Then
                    'Nothing
                End If
                Session("ConfirmationBoxResponse") = String.Empty
            End If

            If Session("ConfirmationBoxResponse") = "ButtonRemoveAllClick" Then
                If Request.Form("Yes") = "Yes" Then
                    'If the user confirms 
                    'call function to retrieve the data
                    RemoveAllSelectedRecord()
                ElseIf Request.Form("No") = "No" Then
                    'Nothing
                End If
                Session("ConfirmationBoxResponse") = String.Empty
            End If



            If Not Me.IsPostBack Then
                ButtonSelectAll.Enabled = False
                ButtonMove.Enabled = False
                ButtonRetrieve.Enabled = False
                ButtonRemoveAll.Enabled = False

                Session("DC_l_dataset_SearchResults") = Nothing
                Session("DC_l_dataset_Moved") = Nothing

            End If

        Catch ex As SqlException
            Response.Redirect("ErrorPageForm.aspx?DBMessage=" + Server.UrlEncode(ex.Message.Trim.ToString()))
        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()))
        End Try

    End Sub

    Function ConvertToDecimal(ByVal strNumber As String) As Decimal
        'This function is written to handle empty string conversion to decimal
        Dim strNumberToReturn As Decimal
        Try
            strNumberToReturn = Convert.ToDecimal(strNumber.Trim)
        Catch ex As Exception
            strNumberToReturn = 0
        End Try

        Return strNumberToReturn
    End Function

#Region "Search Section related code"

    Private Sub DataGrid_Search_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles DataGrid_Search.SortCommand

        Dim l_CheckedList As ArrayList = New ArrayList
        Dim l_blnChecked As Boolean
        Try

            '--------------------------------------------------------------------------------------------
            'Keeping Checked record in arraylist to maintain the checkbox selection after sorting
            For Each itm As DataGridItem In DataGrid_Search.Items
                l_blnChecked = DirectCast(itm.FindControl("CheckBoxSelect"), CheckBox).Checked
                If l_blnChecked Then
                    l_CheckedList.Add(itm.Cells(1).Text.ToString())
                End If
            Next

            '-------------------------------------------------------------
            ' Logic for this routine
            '-------------------------------------------------------------
            ' Reinitialize the selected Index
            ' If there are search results only then we perform the sort
            ' If this is the second time around then get the previous sort expression from viewstate
            ' If last time we had sorted on the same column then toggle the sorting order else sort with the normal order
            ' Save the new sort expression to the ViewState
            DataGrid_Search.SelectedIndex = -1
            If HelperFunctions.isNonEmpty(l_dataset_SearchResults) Then
                If Not ViewState("previousSearchSortExpression") Is Nothing AndAlso ViewState("previousSearchSortExpression") <> "" Then
                    'If SortExpression is not the same as the previous one then initialize new one
                    If (e.SortExpression <> ViewState("previousSearchSortExpression")) Then
                        ViewState("previousSearchSortExpression") = e.SortExpression
                        l_dataset_SearchResults.Tables(0).DefaultView.Sort = e.SortExpression + " ASC"
                    Else
                        'else toggle existing sort expression
                        l_dataset_SearchResults.Tables(0).DefaultView.Sort = IIf(l_dataset_SearchResults.Tables(0).DefaultView.Sort.EndsWith("ASC") = True, e.SortExpression + " DESC", e.SortExpression + " ASC")
                    End If
                Else
                    'First time in the sort function
                    l_dataset_SearchResults.Tables(0).DefaultView.Sort = e.SortExpression + " ASC"
                    ViewState("previousSearchSortExpression") = e.SortExpression
                End If
                BindGrid(DataGrid_Search, l_dataset_SearchResults.Tables(0).DefaultView)

                '-------------------------------------------------------------------------
                'Checking all check box by getting id from arraylist in which checked id is stored before sorting
                Dim CheckBoxSelect As System.Web.UI.WebControls.CheckBox
                Dim dgi As DataGridItem
                If DataGrid_Search.Items.Count > 0 Then
                    For Each dgi In DataGrid_Search.Items
                        CheckBoxSelect = dgi.FindControl("CheckBoxSelect")
                        If l_CheckedList.Contains(dgi.Cells(1).Text.ToString()) Then
                            CheckBoxSelect.Checked = True
                        Else
                            CheckBoxSelect.Checked = False
                        End If
                    Next
                End If
                '------------------------------------------------------------------------

            End If
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        Finally
            l_CheckedList = Nothing
            l_blnChecked = Nothing
        End Try

    End Sub

    Private Sub DataGrid_Moved_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles DataGrid_Moved.SortCommand
        Try
            '-------------------------------------------------------------
            ' Logic for this routine
            '-------------------------------------------------------------
            ' Reinitialize the selected Index
            ' If there are search results only then we perform the sort
            ' If this is the second time around then get the previous sort expression from viewstate
            ' If last time we had sorted on the same column then toggle the sorting order else sort with the normal order
            ' Save the new sort expression to the ViewState
            DataGrid_Moved.SelectedIndex = -1
            If HelperFunctions.isNonEmpty(l_dataset_Moved) Then
                If Not ViewState("previousMovedSortExpression") Is Nothing AndAlso ViewState("previousMovedSortExpression") <> "" Then
                    'If SortExpression is not the same as the previous one then initialize new one
                    If (e.SortExpression <> ViewState("previousMovedSortExpression")) Then
                        ViewState("previousMovedSortExpression") = e.SortExpression
                        l_dataset_Moved.Tables(0).DefaultView.Sort = e.SortExpression + " ASC"
                    Else
                        'else toggle existing sort expression
                        l_dataset_Moved.Tables(0).DefaultView.Sort = IIf(l_dataset_Moved.Tables(0).DefaultView.Sort.EndsWith("ASC") = True, e.SortExpression + " DESC", e.SortExpression + " ASC")
                    End If
                Else
                    'First time in the sort function
                    l_dataset_Moved.Tables(0).DefaultView.Sort = e.SortExpression + " ASC"
                    ViewState("previousMovedSortExpression") = e.SortExpression
                End If
                BindGrid(DataGrid_Moved, l_dataset_Moved.Tables(0).DefaultView)
            End If
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
    End Sub

    Private Sub DataGrid_Search_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DataGrid_Search.ItemDataBound

        'Hiding cells in DataGrid_Search 
        e.Item.Cells(1).Visible = False
        If e.Item.Cells.Count > 7 Then e.Item.Cells(7).Visible = False
        If e.Item.Cells.Count > 8 Then e.Item.Cells(8).Visible = False
        If e.Item.Cells.Count > 9 Then e.Item.Cells(9).Visible = False
    End Sub

    Private Sub DataGrid_Moved_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DataGrid_Moved.ItemDataBound

        'Hiding cells in DataGrid_Moved
        e.Item.Cells(1).Visible = False
        If e.Item.Cells.Count > 7 Then e.Item.Cells(7).Visible = False
        If e.Item.Cells.Count > 8 Then e.Item.Cells(8).Visible = False
        If e.Item.Cells.Count > 9 Then e.Item.Cells(9).Visible = False
    End Sub

    Private Sub ButtonFind_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonFind.Click
        Try
            Find()
        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()))
        End Try
    End Sub

#End Region



    Private Sub ButtonClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonClear.Click
        'Clear Search criteria
        Try
            Me.TextBoxFirstName.Text = ""
            Me.TextBoxLastName.Text = ""
            Me.TextBoxSSNo.Text = ""
            Me.TextBoxFundNo.Text = ""
            Me.TextBoxCity.Text = ""
            Me.TextBoxState.Text = ""
            Me.LabelTitle.Text = ""
            Me.LabelNoDataFound.Visible = False

        Catch ex As SqlException
            Response.Redirect("ErrorPageForm.aspx?DBMessage=" + Server.UrlEncode(ex.Message.Trim.ToString()))
        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()))
        End Try
    End Sub

    Private Sub ButtonCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonCancel.Click

        'To come on main form
        Try
            Session("DeathCalc_Sort") = Nothing
            Response.Redirect("MainWebForm.aspx", False)
        Catch ex As SqlException
            Response.Redirect("ErrorPageForm.aspx?DBMessage=" + Server.UrlEncode(ex.Message.Trim.ToString()))
        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()))
        End Try
    End Sub


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

#Region "Local Variable Persistence mechanism"
    Protected Overrides Function SaveViewState() As Object
        'l_int_SelectedDataGridItem = DataGrid_Search.SelectedIndex
        Dim al As New ArrayList
        al.Add(StoreLocalVariablesToCache())
        al.Add(MyBase.SaveViewState())
        Return al
    End Function
    Protected Overrides Sub LoadViewState(ByVal savedState As Object)
        Dim al As ArrayList = DirectCast(savedState, ArrayList)
        InitializeLocalVariablesFromCache(al.Item(0))
        MyBase.LoadViewState(al.Item(1))
    End Sub

    Private Sub InitializeLocalVariablesFromCache(ByRef obj As Object)
        'Session: Load data from Session so that it is accessible on Postback - This can be replaced by a load from viewstate or database
        l_dataset_SearchResults = Session("DC_l_dataset_SearchResults")
        l_dataset_Moved = Session("DC_l_dataset_Moved")
    End Sub
    Private Function StoreLocalVariablesToCache() As Object
        'Session: Pass data into Session so that it is accessible on Postback - This can be replaced by a save to viewstate
        Session("DC_l_dataset_SearchResults") = l_dataset_SearchResults
        Session("DC_l_dataset_Moved") = l_dataset_Moved
    End Function
#End Region


    Private Sub ButtonSelectAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonSelectAll.Click
        'To select all record in grid 
        Try
            If ButtonSelectAll.Text = "Select All" Then
                SelectAll()
                ButtonSelectAll.Text = "Select None"
            ElseIf ButtonSelectAll.Text = "Select None" Then
                UncheckAll()
                ButtonSelectAll.Text = "Select All"
                ' ButtonSelectAll.Enabled = False
            End If


        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
    End Sub

    Private Sub ButtonMove_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonMove.Click
        BindGridMove()
        UncheckAll()    'To uncheck all record in search grid
        ButtonSelectAll.Enabled = True
        ButtonSelectAll.Text = "Select All"

    End Sub

    'To bind the grid which will contain moved record for retrieval
    Private Sub BindGridMove()
        Dim dt As DataTable
        Try
            DataGrid_Moved.AllowSorting = True
            ViewState("previousMovedSortExpression") = ""  ' Reinitialize the Sort Expression.

            Dim l_String_PersID As String

            If (hdCount.Value = "") Then
                l_dataset_Moved = l_dataset_SearchResults.Clone() 'Cloning dataset
            End If

            Dim l_blnFlag As Boolean = True
            'Getting Unique id in string which is checked in serch result grid
            For Each itm As DataGridItem In DataGrid_Search.Items
                Dim l_blnChecked As Boolean
                l_blnChecked = DirectCast(itm.FindControl("CheckBoxSelect"), CheckBox).Checked
                If l_blnChecked Then
                    l_blnFlag = False
                    'l_String_PersID = l_String_PersID + "'" + (itm.Cells(1).Text.ToString()) + "'" + ","
                    l_String_PersID = itm.Cells(1).Text.ToString().Trim
                    Dim expression As String
                    expression = "[PersID] = '" + l_String_PersID + "' "
                    'Importing selected row from l_dataset_SearchResults to l_dataset_Moved(new dataset for moved result)
                    For Each dr As DataRow In l_dataset_SearchResults.Tables(0).Select(expression)
                        l_dataset_Moved.Tables(0).ImportRow(dr)
                        l_dataset_SearchResults.Tables(0).Rows.Remove(dr) 'Shashi Shekhar:2010-03-26: for BT-489
                    Next


                End If
            Next

            'If user clicks move button, without selecting any record from grid, then show alert message
            If (l_blnFlag = True) Then
                MessageBox.Show(PlaceHolder1, "Alert", "Please select record. ", MessageBoxButtons.OK)
                Exit Sub

            End If

            trMovedGridHeader.Visible = True
            divGridMove.Visible = True
            ButtonRetrieve.Visible = True
            ButtonRemoveAll.Visible = True
            ButtonRemoveAll.Enabled = True
            ButtonRetrieve.Enabled = True
            dt = l_dataset_Moved.Tables(0)
            dt = RemoveDuplicateRows(dt, "PersID")
            BindGrid(DataGrid_Moved, l_dataset_Moved)
            BindGrid(DataGrid_Search, l_dataset_SearchResults) 'Shashi Shekhar:2010-03-26: for BT-489
            hdCount.Value = "filled"
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        Finally
            ''dt.Dispose()
        End Try
    End Sub

    'Function to select all check box in Search grid
    Private Sub SelectAll()
        Dim CheckBoxSelect As System.Web.UI.WebControls.CheckBox
        Dim dgi As DataGridItem

        Try
            If DataGrid_Search.Items.Count > 0 Then
                For Each dgi In DataGrid_Search.Items
                    CheckBoxSelect = dgi.FindControl("CheckBoxSelect")
                    CheckBoxSelect.Checked = True
                Next
            End If

        Catch
            Throw
        End Try

    End Sub
    'To uncheck all record in search grid
    Private Sub UncheckAll()
        Dim CheckBoxSelect As System.Web.UI.WebControls.CheckBox
        Dim dgi As DataGridItem
        Try

            If DataGrid_Search.Items.Count > 0 Then
                For Each dgi In DataGrid_Search.Items
                    CheckBoxSelect = dgi.FindControl("CheckBoxSelect")
                    CheckBoxSelect.Checked = False
                Next
            End If

        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)

        End Try

    End Sub

    Private Sub DataGrid_Moved_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DataGrid_Moved.ItemCommand
        'Removing corresponding item form DataGrid_Moved when user click remove link button in grid
        Try
            If e.CommandName = "lbtnRemove" Then
                l_dataset_Moved.Tables(0).Rows.RemoveAt(e.Item.ItemIndex)

                If (l_dataset_Moved.Tables(0).Rows.Count = 0) Then
                    DummyDataTable(DataGrid_Moved)
                    ButtonRetrieve.Enabled = False
                    ButtonRemoveAll.Enabled = False
                Else
                    BindGrid(DataGrid_Moved, l_dataset_Moved)
                    ButtonRetrieve.Enabled = True
                    ButtonRemoveAll.Enabled = True
                End If
            End If

        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try

    End Sub

    Private Sub ButtonRetrieve_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonRetrieve.Click
        Try
            Dim l_NoOfRecord As String = l_dataset_Moved.Tables(0).Rows.Count.ToString()
            MessageBox.Show(PlaceHolder1, "Data Archive", l_NoOfRecord & "  Records selected for retrieval , do you want to proceed?", MessageBoxButtons.YesNo)
            Session("ConfirmationBoxResponse") = "ButtonRetrieveClick"
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try

    End Sub

    'To remove duplicate row from datatable
    Public Function RemoveDuplicateRows(ByVal dTable As DataTable, ByVal colName As String) As DataTable

        Dim l_hTable As Hashtable = New Hashtable
        Dim l_duplicateList As ArrayList = New ArrayList

        Try

            Dim drow As DataRow
            For Each drow In dTable.Rows
                If l_hTable.Contains(drow(colName)) Then
                    l_duplicateList.Add(drow)
                Else
                    l_hTable.Add(drow(colName), String.Empty)
                End If
            Next

            Dim dRow1 As DataRow
            For Each dRow1 In l_duplicateList
                dTable.Rows.Remove(dRow1)
            Next

            Return dTable

        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        Finally

            l_hTable = Nothing
            l_duplicateList = Nothing
        End Try

    End Function


    'Function to create dummy datatable and bind it with grid for showing grid's header even there is no data in grids
    Public Function DummyDataTable(ByRef dg As DataGrid)

        'Create a new DataTable object
        Dim objDataTable As New System.Data.DataTable

        Try
            'Create columns 
            objDataTable.Columns.Add("PersID")
            objDataTable.Columns.Add("SSN")
            objDataTable.Columns.Add("LastName")
            objDataTable.Columns.Add("FirstName")
            objDataTable.Columns.Add("MiddleName")
            objDataTable.Columns.Add("FundStatus")
            objDataTable.Columns.Add("FundIDNo")
            objDataTable.Columns.Add("FundUniqueId")
            objDataTable.Columns.Add("FundStatusType")

            'Adding some data in the rows of this DataTable
            Dim dr As DataRow = objDataTable.NewRow()
            dr.Item(0) = ""
            dr.Item(1) = ""
            dr.Item(2) = ""
            dr.Item(3) = ""
            dr.Item(4) = ""
            dr.Item(5) = ""
            dr.Item(6) = ""
            dr.Item(7) = ""
            dr.Item(8) = ""

            objDataTable.Rows.Add(dr)

            'if there is no data in grid then making shorting false
            If dg.ClientID Is "DataGrid_Search" And l_dataset_SearchResults Is Nothing Then
                DataGrid_Search.AllowSorting = False
            End If

            If dg.ClientID Is "DataGrid_Moved" Then
                DataGrid_Moved.AllowSorting = False
            End If

            If objDataTable.Rows.Count > 0 Then
                dg.DataSource = objDataTable
                dg.DataBind()
                dg.Items(0).Visible = False  'making default row visibility false
            End If

        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        Finally


        End Try

    End Function


    'To remove all record from l_dataset_Moved dataset on Remove All Button click
    Private Sub ButtonRemoveAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonRemoveAll.Click
        Try
            'Shashi Shekhar:2010-03-26:For BT-486
            Dim l_NoOfRecord As String = l_dataset_Moved.Tables(0).Rows.Count.ToString()
            MessageBox.Show(PlaceHolder1, "Data Archive", "Do you want to remove all records from Selected data for retrieval?", MessageBoxButtons.YesNo)
            Session("ConfirmationBoxResponse") = "ButtonRemoveAllClick"
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
    End Sub


    'Shashi Shekhar:2010-01-26
    'To Update the  record in database
    Private Function RetrieveData()
        Dim list As New ArrayList
        Dim errorMsg As String 'Shashi Shekhar:2010-04-07
        Try
            If DataGrid_Moved.Items.Count > 0 Then
                For Each itm As DataGridItem In DataGrid_Moved.Items
                    list.Add(itm.Cells(1).Text.ToString().Trim())
                Next
                'Shashi Shekhar:2010-04-07:Adding one parameter for error message returned from procedure as output parameter
                errorMsg = YMCARET.YmcaBusinessObject.DataArchiveBOClass.RetrieveData(list)
                'If (errorMsg <> String.Empty) Then
                '    MessageBox.Show(PlaceHolder1, "YMCA-YRS", errorMsg, MessageBoxButtons.Stop)
                '    Find()
                '    Exit Function
                'End If

                'MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Records have been updated successfully.", MessageBoxButtons.OK)
                l_dataset_Moved.Tables(0).Rows.Clear()
                DataGrid_Moved.DataSource = l_dataset_Moved.Tables(0).DefaultView
                DataGrid_Moved.DataBind()
                ButtonRetrieve.Enabled = False
                ButtonRemoveAll.Enabled = False

                Find()

            End If

        Catch ex As Exception
            '' Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
            'Shashi Shekhar:2010-04-07:Handling DataArchive error message (concate sql error message with user defined error message returned from procedure)
            'Shashi Shekhar:2010-04-12:Handling DataArchive error message (Now userdefined error message is already concatenated in DA class with sql exception to show both message. )
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
        Finally
            list = Nothing
            Session("DC_l_dataset_Moved") = Nothing
            l_dataset_Moved = Nothing
        End Try

    End Function

    'Shashi Shekhar:2010-01-27
    'To find the record according to filter criteria
    Private Function Find()
        Try

            'making controls invisible and clearing grid for fresh result
            trSearchGridHeader.Visible = False
            ButtonSelectAll.Text = "Select All" 'Shashi shekhar:2010-03-26: for BT-485
            ButtonSelectAll.Enabled = False
            ButtonMove.Enabled = False
            DataGrid_Search.AllowSorting = True
            divGridSearch.Visible = False
            hrSerch.Visible = False
            '' BindGrid(DataGrid_Search, CType(Nothing, DataSet))

            TextBoxSSNo.Text = TextBoxSSNo.Text.Replace("-", "")    'Feature: User can put SSN in 222-22-2222 format also
            If Me.TextBoxSSNo.Text.Trim = "" And Me.TextBoxLastName.Text.Trim = "" And Me.TextBoxFirstName.Text.Trim = "" And Me.TextBoxFundNo.Text.Trim = "" And Me.TextBoxCity.Text.Trim = "" And Me.TextBoxState.Text.Trim = "" Then
                ''BindGrid(DataGrid_Search, CType(Nothing, DataSet))
                MessageBox.Show(PlaceHolder1, "Retrieve Archived Data", "Please enter a search criteria. ", MessageBoxButtons.OK)

                trSearchGridHeader.Visible = True
                divGridSearch.Visible = True
                divGridMove.Visible = True
                trMovedGridHeader.Visible = True
                hrSerch.Visible = True
                If l_dataset_SearchResults Is Nothing Then
                Else
                    ButtonSelectAll.Enabled = True
                    ButtonMove.Enabled = True
                    ButtonSelectAll.Enabled = True
                End If

                DataGrid_Search.SelectedIndex = 0
                Me.LabelNoDataFound.Visible = False

                Exit Function
            End If

            ViewState("previousSearchSortExpression") = ""  'Reinitialize the Sort Expression.

            'Then process the lookup
            ' l_dataset_SearchResults = YMCARET.YmcaBusinessObject.DeathBenefitsCalculatorBOClass.LookUp_DeathCalc_MemberListForDeath(Me.TextBoxSSNo.Text.Trim, Me.TextBoxLastName.Text.Trim, Me.TextBoxFirstName.Text.Trim, TextBoxFundNo.Text.Trim)
            l_dataset_SearchResults = YMCARET.YmcaBusinessObject.DataArchiveBOClass.LookUpPersons(Me.TextBoxSSNo.Text.Trim(), Me.TextBoxFundNo.Text.Trim, Me.TextBoxLastName.Text.Trim, Me.TextBoxFirstName.Text.Trim(), Me.TextBoxCity.Text.Trim(), Me.TextBoxState.Text.Trim())

            'Check if any results were returned??
            If HelperFunctions.isEmpty(l_dataset_SearchResults) Then
                Me.LabelNoDataFound.Visible = True
                Me.LabelTitle.Text = ""
                BindGrid(DataGrid_Search, l_dataset_SearchResults)
                ButtonSelectAll.Enabled = False
                ButtonMove.Enabled = False
                Exit Function
            End If

            trSearchGridHeader.Visible = True
            divGridSearch.Visible = True
            divGridMove.Visible = True
            trMovedGridHeader.Visible = True
            hrSerch.Visible = True
            ButtonSelectAll.Enabled = True

            DataGrid_Search.SelectedIndex = 0
            Me.LabelNoDataFound.Visible = False
            BindGrid(DataGrid_Search, l_dataset_SearchResults)
            ButtonSelectAll.Enabled = True
            ButtonMove.Enabled = True

        Catch ex As SqlException
            LabelNoDataFound.Visible = True
            ButtonSelectAll.Enabled = False
            ButtonMove.Enabled = False
        Catch
            Throw
        End Try

    End Function


    'Shashi Shekhar:2010-03-26:Function which will remove all record from DataGrid_Moved
    Private Function RemoveAllSelectedRecord()
        Try

            l_dataset_Moved.Tables(0).Clear() 'Clearing all rows from dataset
            If (l_dataset_Moved.Tables(0).Rows.Count = 0) Then
                DummyDataTable(DataGrid_Moved)
                ButtonRetrieve.Enabled = False
                ButtonRemoveAll.Enabled = False
            Else
                BindGrid(DataGrid_Moved, l_dataset_Moved)
                ButtonRetrieve.Enabled = True
                ButtonRemoveAll.Enabled = True
            End If

        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
    End Function
    'START : PK | 02-28-2019 | YRS-AT-4248 | Check ReadOnly Flag, if true then disable the buttons on which security is not implemented and add Tool tip.
    Public Sub CheckReadOnlyMode()
        If (SecurityCheck.IsApplicationInReadOnlyMode()) Then
            ButtonFind.Enabled = False
            ButtonFind.ToolTip = YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.TOOLTIP_READONLY).DisplayText
        End If
    End Sub
    'END : PK | 02-28-2019 | YRS-AT-4248 | Check ReadOnly Flag, if true then disable the buttons on which security is not implemented and add Tool tip.
End Class


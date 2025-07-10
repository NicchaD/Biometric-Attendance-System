'*******************************************************************************
' Copyright YMCA Retirement Fund All Rights Reserved. 
'
' Project Name		:	YMCA-YRS
' FileName			:	EmployeeEventsWebForm.aspx.vb
' Author Name		:	Shefali Bharti
' Employee ID		:	33488
' Email				:	shefali.bharti@3i-infotech.com
' Contact No		:	8740
' Creation Time		:	5/17/2005 5:00:35 PM
' Program Specification Name	:	Doc 5.4.1.7
' Unit Test Plan Name			:	
' Description					:	<<Please put the brief description here...>>

'*******************************************************************************
'Modification History
'*******************************************************************************
'Modified by                    Date                Description
'*******************************************************************************
'Vipul                          04Feb06             Cache-Session
'Neeraj Singh                   12/Nov/2009         Added form name for security issue YRS 5.0-940 
'Neeraj Singh                   06/jun/2010         Enhancement for .net 4.0
'Manthan Rajguru                2015.09.16          YRS-AT-2550: YRS data cleanup: copyright comments and namespace/reference (Changed the Namespace/reference from 'Infotech' to 'YMCARET')
'Pooja K                        2019.28.02          YRS-AT-4248 - YRS enh: YRS read-only access during MonthEnd Processing (TrackIT 35547)
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
'Imports Microsoft.Practices.EnterpriseLibrary.Caching
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling


Public Class EmployeeEventsWebForm
    Inherits System.Web.UI.Page
    'below line is Added by Neeraj for issue id YRS 5.0-940 
    Dim strFormName As String = New String("EmployeeEventsWebForm.aspx")
    'End issue id YRS 5.0-940

    Dim g_dataset_dsEmployeeEventStatus As New DataSet
    Dim g_bool_SearchFlag As New Boolean
    Dim g_bool_EditFlag As New Boolean
    Dim g_bool_AddFlag As New Boolean
    Dim g_integer_count As New Integer
    Protected WithEvents PlaceHolder1 As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents RequiredFieldValidator1 As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents ValidationSummary1 As System.Web.UI.WebControls.ValidationSummary
    Dim g_bool_DeleteFlag As New Boolean

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents LabelLook As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxFind As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxEmpEventType As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxShortDesc As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxLongDesc As System.Web.UI.WebControls.TextBox
    Protected WithEvents ButtonSave As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonCancel As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonAdd As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonOK As System.Web.UI.WebControls.Button
    Protected WithEvents DataGridEmployeeEvents As System.Web.UI.WebControls.DataGrid
    Protected WithEvents LabelEmpEventType As System.Web.UI.WebControls.Label
    Protected WithEvents LabelShortDesc As System.Web.UI.WebControls.Label
    Protected WithEvents LabelLongDesc As System.Web.UI.WebControls.Label
    Protected WithEvents Menu1 As skmMenu.Menu
    Protected WithEvents Buttonsearch As System.Web.UI.WebControls.Button
    Protected WithEvents LabelNoRecordFound As System.Web.UI.WebControls.Label

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
        'Me.DataGridEmployeeEvents.DataSource = CommonModule.CreateDataSource
        'Me.DataGridEmployeeEvents.DataBind()
        Me.LabelLook.AssociatedControlID = Me.TextBoxFind.ID
        Me.LabelEmpEventType.AssociatedControlID = Me.TextBoxEmpEventType.ID
        Me.LabelShortDesc.AssociatedControlID = Me.TextBoxShortDesc.ID
        Me.LabelLongDesc.AssociatedControlID = Me.TextBoxLongDesc.ID
        If Session("LoggedUserKey") Is Nothing Then
            Response.Redirect("Login.aspx", False)
        End If
        Try

       
        Menu1.DataSource = Server.MapPath("SimpleXML.xml")
        Menu1.DataBind()
            CheckReadOnlyMode() 'PK | 02-28-2019 |YRS-AT-4248 | Check security method called here
        ' At First load initializing Add Flag ,Edit Flag and Search Flag to False
        If Not Me.IsPostBack Then

            g_bool_AddFlag = False
            Session("BoolAddFlag") = g_bool_AddFlag

            g_bool_DeleteFlag = False
            Session("BoolDeleteFlag") = g_bool_DeleteFlag

            g_bool_EditFlag = False
            Session("BoolEditFlag") = g_bool_EditFlag

            g_bool_SearchFlag = False
            Session("BoolSearchFlag") = g_bool_SearchFlag
            Me.LabelNoRecordFound.Visible = False
            PopulateData()
            ' populating the textboxes with the current row
            g_integer_count = Session("dataset_index")
            If (g_integer_count <> -1) Then
                Me.PopulateDataIntoControls(g_integer_count)
            End If

        Else
            g_bool_SearchFlag = Session("BoolSearchFlag")
            g_bool_EditFlag = Session("BoolEditFlag")
            g_bool_DeleteFlag = Session("BoolDeleteFlag")

            'If g_bool_SearchFlag = True Then
            '    SearchAndPopulateData()
            'Else
            '    PopulateData()
            'End If
            Me.LabelNoRecordFound.Visible = False
        End If

        If Request.Form("Ok") = "OK" Then
            Me.ButtonCancel.Enabled = False
            Me.ButtonSave.Enabled = False
            ''Me.ButtonEdit.Enabled = False
            Me.ButtonAdd.Enabled = True
            Me.ButtonOK.Enabled = True
            Me.Buttonsearch.Enabled = True
            Me.TextBoxFind.ReadOnly = False
            Me.TextBoxEmpEventType.ReadOnly = True
            Me.TextBoxLongDesc.ReadOnly = True
            Me.TextBoxShortDesc.ReadOnly = True

            If g_bool_SearchFlag = True Then
                SearchAndPopulateData()
            Else
                PopulateData()
            End If
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Public Sub PopulateData()
        'Vipul 04Feb06 Cache-Session
        'Dim Cache As CacheManager
        'Cache = CacheFactory.GetCacheManager()

        Try

            g_dataset_dsEmployeeEventStatus = YMCARET.YmcaBusinessObject.EmployeeEventsBOClass.LookupEmployeeEventStatus()
            viewstate("Dataset_EmpEvents") = g_dataset_dsEmployeeEventStatus

            'Vipul 04Feb06 Cache-Session
            'Cache.Add("Employee Event Status", g_dataset_dsEmployeeEventStatus)
            Session("Employee Event Status") = g_dataset_dsEmployeeEventStatus
            'Vipul 04Feb06 Cache-Session

            If g_dataset_dsEmployeeEventStatus.Tables.Count = 0 Then
                ''Me.ButtonEdit.Enabled = False
                Me.TextBoxFind.ReadOnly = True
                Me.Buttonsearch.Enabled = False

                Me.LabelNoRecordFound.Visible = True
            Else

                Me.DataGridEmployeeEvents.DataSource = g_dataset_dsEmployeeEventStatus.Tables(0)
                Me.DataGridEmployeeEvents.DataBind()

            End If

            ''CommonModule.HideColumnsinDataGrid(g_dataset_dsEmployeeEventStatus, Me.DataGridEmployeeEvents, "Long Desc.")

            'Me.DataGridEmployeeEvents.AllowSorting = True

        Catch sqlEx As SqlException
            If sqlEx.Number = 60006 Then
                Me.LabelNoRecordFound.Visible = True

                'Vipul 04Feb06 Cache-Session
                'g_dataset_dsEmployeeEventStatus = Cache.GetData("Employee Event Status")
                g_dataset_dsEmployeeEventStatus = Session("Employee Event Status")
                'Vipul 04Feb06 Cache-Session

                g_dataset_dsEmployeeEventStatus.Tables(0).Clear()
                Me.DataGridEmployeeEvents.DataSource = g_dataset_dsEmployeeEventStatus
                Me.DataGridEmployeeEvents.DataBind()
            Else
                Response.Redirect("ErrorPageForm.aspx")
            End If

        Catch secEx As System.Security.SecurityException
            Response.Redirect("ErrorPageForm.aspx")
        End Try
    End Sub

    Private Function PopulateDataIntoControls(ByVal index As Integer) As Boolean

        Dim l_DataSet As DataSet
        Dim l_DataTable As DataTable
        Dim l_DataRow As DataRow

        Try

            l_DataSet = g_dataset_dsEmployeeEventStatus

            If Not l_DataSet Is Nothing Then

                l_DataTable = l_DataSet.Tables("Employee Event Status")

                If Not l_DataTable Is Nothing Then

                    If index < l_DataTable.Rows.Count Then

                        l_DataRow = l_DataTable.Rows.Item(index)

                        If (Not l_DataRow Is Nothing) Then

                            Me.TextBoxEmpEventType.Text = CType(l_DataRow("Emp. Event Type"), String).Trim
                            If l_DataRow("Short Desc.").GetType.ToString = "System.DBNull" Then
                                Me.TextBoxShortDesc.Text = String.Empty
                            Else
                                Me.TextBoxShortDesc.Text = CType(l_DataRow("Short Desc."), String).Trim
                            End If

                            If l_DataRow("Long Desc.").GetType.ToString = "System.DBNull" Then
                                Me.TextBoxLongDesc.Text = String.Empty
                            Else
                                Me.TextBoxLongDesc.Text = CType(l_DataRow("Long Desc."), String).Trim
                            End If


                            Return True
                        Else
                            Return False
                        End If
                    Else
                        Return False
                    End If
                Else
                    Return False
                End If

            Else
                Return False
            End If


        Catch
            Throw
        End Try



    End Function

    Private Sub Buttonsearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Buttonsearch.Click
        Try
            If g_bool_SearchFlag = True Then
                SearchAndPopulateData()
            Else
                PopulateData()
            End If
            g_bool_SearchFlag = True
            Session("BoolSearchFlag") = g_bool_SearchFlag

            ' Clear the controls.
            ' Me.TextBoxFind.Text = ""
            Me.TextBoxEmpEventType.Text = String.Empty
            Me.TextBoxShortDesc.Text = String.Empty
            Me.TextBoxLongDesc.Text = String.Empty

            'g_integer_count = Session("dataset_index")
            'If (g_integer_count <> -1) Then
            '    Me.PopulateDataIntoControls(g_integer_count)
            'End If

            ' Do search & populate the data.
            SearchAndPopulateData()
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub SearchAndPopulateData()
        'Vipul 04Feb06 Cache-Session
        'Dim Cache As CacheManager
        'Cache = CacheFactory.GetCacheManager()
        Try

            g_dataset_dsEmployeeEventStatus = YMCARET.YmcaBusinessObject.EmployeeEventsBOClass.SearchEmployeeEventStatus(Me.TextBoxFind.Text)
            viewstate("Dataset_EmpEvents") = g_dataset_dsEmployeeEventStatus
            If g_dataset_dsEmployeeEventStatus.Tables.Count = 0 Then
                Me.LabelNoRecordFound.Visible = True
            Else
                Me.DataGridEmployeeEvents.CurrentPageIndex = 0
                Me.DataGridEmployeeEvents.DataSource = g_dataset_dsEmployeeEventStatus
                Me.DataGridEmployeeEvents.DataBind()
            End If

            ''CommonModule.HideColumnsinDataGrid(g_dataset_dsEmployeeEventStatus, Me.DataGridEmployeeEvents, "Long Desc.")
        Catch secEx As System.Security.SecurityException
            Response.Redirect("ErrorPageForm.aspx")
        Catch sqlEx As SqlException
            If sqlEx.Number = 60006 Then
                ' PopulateData()
                Me.LabelNoRecordFound.Visible = True

                'Vipul 04Feb06 Cache-Session
                'g_dataset_dsEmployeeEventStatus = Cache.GetData("Employee Event Status")
                g_dataset_dsEmployeeEventStatus = Session("Employee Event Status")
                'Vipul 04Feb06 Cache-Session

                g_dataset_dsEmployeeEventStatus.Tables(0).Clear()
                Me.DataGridEmployeeEvents.DataSource = g_dataset_dsEmployeeEventStatus
                Me.DataGridEmployeeEvents.DataBind()
            Else
                Response.Redirect("ErrorPageForm.aspx")
            End If
        End Try



    End Sub

    Private Sub DataGridEmployeeEvents_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DataGridEmployeeEvents.SelectedIndexChanged
        Try
            If g_bool_SearchFlag = True Then
                SearchAndPopulateData()
            Else
                PopulateData()
            End If

            '' Me.ButtonEdit.Enabled = True
            g_integer_count = DataGridEmployeeEvents.SelectedIndex '(((DataGridEmployeeEvents.CurrentPageIndex) * DataGridEmployeeEvents.PageSize) + DataGridEmployeeEvents.SelectedIndex)

            Session("dataset_index") = g_integer_count
            If Me.PopulateDataIntoControls(g_integer_count) = True Then
                '    Me.ButtonEdit.Enabled = True

                'Else
                '    Me.ButtonEdit.Enabled = True
            End If

            Me.TextBoxEmpEventType.ReadOnly = True
            Me.TextBoxShortDesc.ReadOnly = False
            Me.TextBoxLongDesc.ReadOnly = False


            Me.ButtonSave.Enabled = True
            Me.ButtonCancel.Enabled = True
            Me.TextBoxFind.ReadOnly = True
            Me.Buttonsearch.Enabled = False
            Me.ButtonAdd.Enabled = False
            Me.ButtonOK.Enabled = False
            ''Me.ButtonEdit.Enabled = False

            g_bool_AddFlag = False
            Session("BoolAddFlag") = g_bool_AddFlag

            g_bool_EditFlag = True
            Session("BoolEditFlag") = g_bool_EditFlag

            g_bool_DeleteFlag = False
            Session("BoolDeleteFlag") = g_bool_DeleteFlag
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub DataGridEmployeeEvents_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles DataGridEmployeeEvents.PageIndexChanged
        DataGridEmployeeEvents.CurrentPageIndex = e.NewPageIndex

        'Bind the DataGrid again with the Data Source
        'depending on wheather Search Flag is true or False

        If g_bool_SearchFlag = True Then
            SearchAndPopulateData()
        Else
            PopulateData()
        End If
    End Sub

    ''Private Sub ButtonEdit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonEdit.Click
    ''    ' Enable / Disable the controls.
    ''    If g_bool_SearchFlag = True Then
    ''        SearchAndPopulateData()
    ''    Else
    ''        PopulateData()
    ''    End If

    ''    Me.TextBoxEmpEventType.ReadOnly = True
    ''    Me.TextBoxShortDesc.ReadOnly = False
    ''    Me.TextBoxLongDesc.ReadOnly = False


    ''    Me.ButtonSave.Enabled = True
    ''    Me.ButtonCancel.Enabled = True
    ''    Me.TextBoxFind.ReadOnly = True
    ''    Me.Buttonsearch.Enabled = False
    ''    Me.ButtonAdd.Enabled = False
    ''    Me.ButtonOK.Enabled = False
    ''    Me.ButtonEdit.Enabled = False

    ''    g_bool_AddFlag = False
    ''    Session("BoolAddFlag") = g_bool_AddFlag

    ''    g_bool_EditFlag = True
    ''    Session("BoolEditFlag") = g_bool_EditFlag

    ''    g_bool_DeleteFlag = False
    ''    Session("BoolDeleteFlag") = g_bool_DeleteFlag

    ''End Sub

    Private Sub ButtonAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAdd.Click
        Try
            If g_bool_SearchFlag = True Then
                SearchAndPopulateData()
            Else
                PopulateData()
            End If

            g_bool_AddFlag = True
            Session("BoolAddFlag") = g_bool_AddFlag

            g_bool_SearchFlag = False
            Session("BoolSearchFlag") = g_bool_SearchFlag

            'Making Readonly False for all TextBoxes
            Me.TextBoxEmpEventType.ReadOnly = False
            Me.TextBoxShortDesc.ReadOnly = False
            Me.TextBoxLongDesc.ReadOnly = False


            ' Making TextBoxes Blank
            Me.TextBoxEmpEventType.Text = String.Empty
            Me.TextBoxShortDesc.Text = String.Empty
            Me.TextBoxLongDesc.Text = String.Empty

            Me.ButtonSave.Enabled = True
            Me.ButtonCancel.Enabled = True
            Me.TextBoxFind.ReadOnly = True
            Me.Buttonsearch.Enabled = False
            ''Me.ButtonEdit.Enabled = False
            Me.ButtonOK.Enabled = False
            Me.ButtonAdd.Enabled = False
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub ButtonCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonCancel.Click
        Try
            PopulateData()

            'Enable / Disable the controls
            Me.ButtonSave.Enabled = False
            Me.ButtonCancel.Enabled = False
            Me.ButtonAdd.Enabled = True
            ''Me.ButtonEdit.Enabled = False
            Me.TextBoxFind.ReadOnly = False
            Me.Buttonsearch.Enabled = True
            Me.ButtonOK.Enabled = True

            'Making Readonly True for all TextBoxes
            Me.TextBoxEmpEventType.ReadOnly = True
            Me.TextBoxShortDesc.ReadOnly = True
            Me.TextBoxLongDesc.ReadOnly = True
            Me.DataGridEmployeeEvents.SelectedIndex = -1

            g_integer_count = Session("dataset_index")
            If (g_integer_count <> -1) Then
                Me.PopulateDataIntoControls(g_integer_count)
            End If

            g_bool_AddFlag = False
            Session("BoolAddFlag") = g_bool_AddFlag

            g_bool_SearchFlag = False
            Session("BoolSearchFlag") = g_bool_SearchFlag

            g_bool_DeleteFlag = False
            Session("BoolDeleteFlag") = g_bool_DeleteFlag

            g_bool_EditFlag = False
            Session("BoolEditFlag") = g_bool_EditFlag

        Catch ex As Exception
            Throw ex
        End Try


    End Sub

    Private Sub ButtonOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonOK.Click
        Session("EmpEventsSort") = Nothing
        Response.Redirect("MainWebForm.aspx", False)
    End Sub

    Private Sub ButtonSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonSave.Click
        Dim InsertRow As DataRow
        Dim l_DataRow As DataRow

        Try
            'Added by neeraj on 18-Nov-2009 : issue is YRS 5.0-940
            Dim checkSecurity As String = SecurityCheck.Check_Authorization(strFormName, Convert.ToInt32(Session("LoggedUserKey")))

            If Not checkSecurity.Equals("True") Then
                MessageBox.Show(PlaceHolder1, "YMCA-YRS", checkSecurity, MessageBoxButtons.Stop)
                Exit Sub
            End If
            'End : YRS 5.0-940

            If g_bool_SearchFlag = True Then
                SearchAndPopulateData()
            Else
                PopulateData()
            End If
            g_bool_AddFlag = Session("BoolAddFlag")

            If g_bool_AddFlag = True Then
                If Not g_dataset_dsEmployeeEventStatus Is Nothing Then

                    InsertRow = g_dataset_dsEmployeeEventStatus.Tables(0).NewRow

                    ' Assign the values.
                    InsertRow.Item("Emp. Event Type") = TextBoxEmpEventType.Text.Trim
                    If Me.TextBoxShortDesc.Text.Trim.Length = 0 Then
                        InsertRow.Item("Short Desc.") = String.Empty
                    Else
                        InsertRow.Item("Short Desc.") = TextBoxShortDesc.Text.Trim
                    End If

                    If Me.TextBoxLongDesc.Text.Trim.Length = 0 Then
                        InsertRow.Item("Long Desc.") = String.Empty
                    Else
                        InsertRow.Item("Long Desc.") = TextBoxLongDesc.Text.Trim
                    End If
                    ' Insert the row into Table.
                    g_dataset_dsEmployeeEventStatus.Tables(0).Rows.Add(InsertRow)

                    g_integer_count = 0
                    Me.PopulateDataIntoControls(g_integer_count)
                End If

            Else

                If Not g_dataset_dsEmployeeEventStatus Is Nothing Then

                    g_integer_count = Session("dataset_index")
                    l_DataRow = g_dataset_dsEmployeeEventStatus.Tables(0).Rows(g_integer_count)

                    If Not l_DataRow Is Nothing Then

                        'Update the values in current(selected) DataRow
                        l_DataRow("Emp. Event Type") = TextBoxEmpEventType.Text.Trim
                        If Me.TextBoxShortDesc.Text.Trim.Length = 0 Then
                            l_DataRow("Short Desc.") = String.Empty
                        Else
                            l_DataRow("Short Desc.") = TextBoxShortDesc.Text.Trim
                        End If

                        If Me.TextBoxLongDesc.Text.Trim.Length = 0 Then
                            l_DataRow("Long Desc.") = String.Empty
                        Else
                            l_DataRow("Long Desc.") = TextBoxLongDesc.Text.Trim
                        End If


                        g_integer_count = Session("dataset_index")
                        If (g_integer_count <> -1) Then
                            Me.PopulateDataIntoControls(g_integer_count)
                        End If
                    End If

                End If
            End If
            ' Call business layer to Save the DataSet/
            YMCARET.YmcaBusinessObject.EmployeeEventsBOClass.InsertEmployeeEventStatus(g_dataset_dsEmployeeEventStatus)
            PopulateData()

            Me.ButtonSave.Enabled = False
            Me.ButtonCancel.Enabled = False
            Me.ButtonAdd.Enabled = True
            ''Me.ButtonEdit.Enabled = False
            Me.TextBoxFind.ReadOnly = False
            Me.Buttonsearch.Enabled = True
            Me.ButtonOK.Enabled = True

            Me.TextBoxEmpEventType.ReadOnly = True
            Me.TextBoxShortDesc.ReadOnly = True
            Me.TextBoxLongDesc.ReadOnly = True
            Me.DataGridEmployeeEvents.SelectedIndex = -1

            g_bool_AddFlag = False
            Session("BoolAddFlag") = g_bool_AddFlag

            g_bool_SearchFlag = False
            Session("BoolSearchFlag") = g_bool_SearchFlag

        Catch secEx As System.Security.SecurityException
            Response.Redirect("ErrorPageForm.aspx")
        Catch sqlEx As SqlException
            If sqlEx.Number = 60010 Then
                MessageBox.Show(PlaceHolder1, "Please Confirm", "Duplicate Record", MessageBoxButtons.OK)
            Else
                Response.Redirect("ErrorPageForm.aspx")
            End If
        End Try
    End Sub


    Private Sub DataGridEmployeeEvents_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DataGridEmployeeEvents.ItemDataBound
        Dim l_button_Select As ImageButton
        l_button_Select = e.Item.FindControl("ImageButtonSelect")
        If (e.Item.ItemIndex = Me.DataGridEmployeeEvents.SelectedIndex And Me.DataGridEmployeeEvents.SelectedIndex >= 0) Then
            l_button_Select.ImageUrl = "images\selected.gif"
        End If
        e.Item.Cells(3).Visible = False
    End Sub

    Private Sub DataGridEmployeeEvents_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles DataGridEmployeeEvents.SortCommand
        Try
            Me.DataGridEmployeeEvents.SelectedIndex = -1
            If Not viewstate("Dataset_EmpEvents") Is Nothing Then
                Dim dv As New DataView
                Dim SortExpression As String
                SortExpression = e.SortExpression
                g_dataset_dsEmployeeEventStatus = viewstate("Dataset_EmpEvents")
                dv = g_dataset_dsEmployeeEventStatus.Tables(0).DefaultView
                dv.Sort = SortExpression
                If Not Session("EmpEventsSort") Is Nothing Then
                    If Session("EmpEventsSort").ToString.Trim.EndsWith("ASC") Then
                        dv.Sort = SortExpression + " DESC"
                    Else
                        dv.Sort = SortExpression + " ASC"
                    End If
                Else
                    dv.Sort = SortExpression + " ASC"
                End If
                Me.DataGridEmployeeEvents.DataSource = Nothing
                Me.DataGridEmployeeEvents.DataSource = dv
                Me.DataGridEmployeeEvents.DataBind()
                Session("EmpEventsSort") = dv.Sort
            End If
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
    End Sub
    'START : PK | 02-28-2019 | YRS-AT-4248 | Check ReadOnly Flag, if true then disable the buttons on which security is not implemented and add Tool tip.
    Public Sub CheckReadOnlyMode()
        If (SecurityCheck.IsApplicationInReadOnlyMode()) Then
            DataGridEmployeeEvents.Enabled = False
            DataGridEmployeeEvents.ToolTip = YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.TOOLTIP_READONLY).DisplayText
        End If
    End Sub
    'END : PK | 02-28-2019 | YRS-AT-4248 | Check ReadOnly Flag, if true then disable the buttons on which security is not implemented and add Tool tip.
End Class

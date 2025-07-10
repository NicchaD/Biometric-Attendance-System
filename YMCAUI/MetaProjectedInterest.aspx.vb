'*******************************************************************************
' Copyright YMCA Retirement Fund All Rights Reserved. 
'
' Project Name		:	YMCA-YRS
' FileName			:	MetaProjectedInterest.aspx.vb
' Author Name		:	Vartika Jain
' Employee ID		:	33495
' Email				:	vartika.jain@3i-infotech.com
' Contact No		:	8733
' Creation Time		:	5/17/2005 4:59:02 PM
' Program Specification Name	:	YMCA PS 5.4.1.9
' Unit Test Plan Name			:	
' Description					:	<<Please put the brief description here...>>

'
' Changed by			:	Shefali Bharti
' Changed on			:	25-07-2005
' Change Description	:	Coding
' Cache to Session       :   Ragesh 34231 02/02/06 Cache to Session
'****************************************************
'Modification History
'*******************************************************************************
'Modified by                    Date                Description
'*******************************************************************************
'Neeraj Singh                   12/Nov/2009         Added form name for security issue YRS 5.0-940 
'Shashi Singh                   15/june/2010        Migration related changes.
'Shashi Singh                   08/July/2010        Code review changes.
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



Public Class MetaProjectedInterest
    Inherits System.Web.UI.Page
    'below line is Added by Neeraj for issue id YRS 5.0-940 
    Dim strFormName As String = New String("MetaProjectedInterest.aspx")
    'End issue id YRS 5.0-940

    Dim g_dataset_dsProjectedInterestRate As New DataSet
    Dim g_bool_SearchFlag As New Boolean
    Dim g_bool_EditFlag As New Boolean
    Dim g_bool_AddFlag As New Boolean
    Dim g_integer_count As New Integer
    Protected WithEvents LabelNoRecordFound As System.Web.UI.WebControls.Label

    Protected WithEvents ButtonOk As System.Web.UI.WebControls.Button
    Protected WithEvents PlaceHolder1 As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents RequiredFieldValidator1 As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents ValidationSummary1 As System.Web.UI.WebControls.ValidationSummary
    Protected WithEvents RegularExpressionValidator1 As System.Web.UI.WebControls.RegularExpressionValidator
    Dim g_bool_DeleteFlag As New Boolean


#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Shared Sub InitializeComponent()

    End Sub
    ''Protected WithEvents ButtonEdit As System.Web.UI.WebControls.Button
    Protected WithEvents LabelLook As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxFind As System.Web.UI.WebControls.TextBox
    Protected WithEvents DataGridProjectedInterest As System.Web.UI.WebControls.DataGrid
    Protected WithEvents TextBoxInterestYear As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxInterestRate As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxInterestEndDate As YMCAUI.DateUserControl
    Protected WithEvents TextBoxEffectiveDate As YMCAUI.DateUserControl
    Protected WithEvents ButtonSave As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonCancel As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonAdd As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonDelete As System.Web.UI.WebControls.Button
    Protected WithEvents LabelInterestYear As System.Web.UI.WebControls.Label
    Protected WithEvents LabelAdmin As System.Web.UI.WebControls.Label
    Protected WithEvents LabelHeading As System.Web.UI.WebControls.Label
    Protected WithEvents LabelInterest As System.Web.UI.WebControls.Label
    Protected WithEvents LabelInterestEndDate As System.Web.UI.WebControls.Label
    Protected WithEvents LabelEffectiveDate As System.Web.UI.WebControls.Label
    Protected WithEvents LabelInterestRate As System.Web.UI.WebControls.Label
    Protected WithEvents Menu1 As skmMenu.Menu
    Protected WithEvents ButtonSearch As System.Web.UI.WebControls.Button

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    'Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        'Me.DataGridProjectedInterest.DataSource = CommonModule.CreateDataSource
        'Me.DataGridProjectedInterest.DataBind()
        Try

            Me.LabelInterestYear.AssociatedControlID = Me.TextBoxInterestYear.ID
            Me.LabelLook.AssociatedControlID = Me.TextBoxFind.ID
            Me.LabelInterestRate.AssociatedControlID = Me.TextBoxInterestRate.ID
            Menu1.DataSource = Server.MapPath("SimpleXML.xml")
            Menu1.DataBind()
            If Session("LoggedUserKey") Is Nothing Then
                Response.Redirect("Login.aspx", False)
            End If
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
                g_integer_count = Session("dataset_index")
                If (g_integer_count <> -1) Then
                    Me.PopulateDataIntoControls(g_integer_count)
                End If

                'if Search flag is true then call 'SearchPopulate' method else call 'populate' method

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

            If Request.Form("Yes") = "Yes" Then
                DeleteSub()
                g_integer_count = 0
                Me.PopulateDataIntoControls(g_integer_count)
            End If

            If Request.Form("No") = "No" Then
                If g_bool_SearchFlag = True Then
                    SearchAndPopulateData()
                Else
                    PopulateData()
                End If
            End If

            If Request.Form("Ok") = "OK" Then
                Me.ButtonCancel.Enabled = False
                Me.ButtonDelete.Enabled = False
                Me.ButtonSave.Enabled = False
                ''Me.ButtonEdit.Enabled = False
                Me.ButtonAdd.Enabled = True
                Me.ButtonOk.Enabled = True

                Me.ButtonSearch.Enabled = True
                Me.TextBoxFind.ReadOnly = False


                Me.TextBoxInterestYear.ReadOnly = True
                Me.TextBoxInterestRate.ReadOnly = True

                If g_bool_SearchFlag = True Then
                    SearchAndPopulateData()
                Else
                    PopulateData()
                End If
            End If
        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
        End Try
    End Sub

    Public Sub PopulateData()
        'Dim Cache As CacheManager
        Try
            g_dataset_dsProjectedInterestRate = YMCARET.YmcaBusinessObject.MetaProjectedInterest.LookupProjectedInterestRate()
            viewstate("Dataset_Interest") = g_dataset_dsProjectedInterestRate
            '/b Cache to Session       :   Ragesh 34231 02/02/06 Cache to Session
            'Cache = CacheFactory.GetCacheManager()
            'Cache.Add("Projected Interest Rates", g_dataset_dsProjectedInterestRate)
            Session("g_dataset_dsProjectedInterestRate") = g_dataset_dsProjectedInterestRate
            '/e Cache to Session       :   Ragesh 34231 02/02/06 Cache to Session

            If g_dataset_dsProjectedInterestRate.Tables.Count = 0 Then
                ''Me.ButtonEdit.Enabled = False
                Me.TextBoxFind.ReadOnly = True
                Me.ButtonSearch.Enabled = False
                Me.ButtonDelete.Enabled = False
                Me.LabelNoRecordFound.Visible = True
            Else

                Me.DataGridProjectedInterest.DataSource = g_dataset_dsProjectedInterestRate.Tables(0)
                Me.DataGridProjectedInterest.DataBind()
                
            End If

            'CommonModule.HideColumnsinDataGrid(g_dataset_dsProjectedInterestRate, Me.DataGridProjectedInterest, "Interest end date, Effective Date")
        Catch sqlEx As SqlException
            If sqlEx.Number = 60006 Then
                Me.LabelNoRecordFound.Visible = True
                '/b Cache to Session       :   Ragesh 34231 02/02/06 Cache to Session
                'g_dataset_dsProjectedInterestRate = Cache.GetData("Projected Interest Rates")
                g_dataset_dsProjectedInterestRate = DirectCast(Session("g_dataset_dsProjectedInterestRate"), DataSet)
                '/e Cache to Session       :   Ragesh 34231 02/02/06 Cache to Session

                g_dataset_dsProjectedInterestRate.Tables(0).Clear()
                Me.DataGridProjectedInterest.DataSource = g_dataset_dsProjectedInterestRate
                Me.DataGridProjectedInterest.DataBind()

            Else
                Response.Redirect("ErrorPageForm.aspx")
            End If


        Catch secEx As System.Security.SecurityException
            Response.Redirect("ErrorPageForm.aspx")
        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
        End Try
        
    End Sub

    Public Sub SearchAndPopulateData()
        'Dim Cache As CacheManager
        Try
            ' Cache = CacheFactory.GetCacheManager()
            'g_dataset_dsProjectedInterestRate = Cache.GetData("Projected Interest Rates")
            g_dataset_dsProjectedInterestRate = YMCARET.YmcaBusinessObject.MetaProjectedInterest.SearchProjectedInterestRates(Me.TextBoxFind.Text)
            viewstate("Dataset_Interest") = g_dataset_dsProjectedInterestRate
            If g_dataset_dsProjectedInterestRate.Tables.Count = 0 Then
                Me.LabelNoRecordFound.Visible = True

            Else
                Me.DataGridProjectedInterest.CurrentPageIndex = 0
                Me.DataGridProjectedInterest.DataSource = g_dataset_dsProjectedInterestRate
                Me.DataGridProjectedInterest.DataBind()

            End If

            'CommonModule.HideColumnsinDataGrid(g_dataset_dsProjectedInterestRate, Me.DataGridProjectedInterest, "Interest end date, Effective Date")
        Catch secEx As System.Security.SecurityException
            Response.Redirect("ErrorPageForm.aspx")
        Catch sqlEx As SqlException
            If sqlEx.Number = 60006 Then
                ' PopulateData()
                Me.LabelNoRecordFound.Visible = True
                '/b Cache to Session       :   Ragesh 34231 02/02/06 Cache to Session
                'g_dataset_dsProjectedInterestRate = Cache.GetData("Projected Interest Rates")
                g_dataset_dsProjectedInterestRate = DirectCast(Session("g_dataset_dsProjectedInterestRate"), DataSet)
                '/e Cache to Session       :   Ragesh 34231 02/02/06 Cache to Session
                g_dataset_dsProjectedInterestRate.Tables(0).Clear()
                Me.DataGridProjectedInterest.DataSource = g_dataset_dsProjectedInterestRate
                Me.DataGridProjectedInterest.DataBind()
            Else
                Response.Redirect("ErrorPageForm.aspx")
            End If
        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
        End Try

    End Sub

    Private Sub ButtonSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonSearch.Click
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
            Me.TextBoxInterestRate.Text = String.Empty
            Me.TextBoxInterestYear.Text = String.Empty
            Me.TextBoxInterestEndDate.Text = String.Empty
            Me.TextBoxEffectiveDate.Text = String.Empty

            'g_integer_count = Session("dataset_index")
            'If (g_integer_count <> -1) Then
            '    Me.PopulateDataIntoControls(g_integer_count)
            'End If

            ' Do search & populate the data.
            SearchAndPopulateData()
        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
        End Try
    End Sub

    Private Sub DataGridProjectedInterest_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DataGridProjectedInterest.SelectedIndexChanged
        Try
            If g_bool_SearchFlag = True Then
                SearchAndPopulateData()
            Else
                PopulateData()
            End If

            Dim fp As System.IFormatProvider
            ''Me.ButtonEdit.Enabled = True
            Me.ButtonDelete.Enabled = True
            g_integer_count = DataGridProjectedInterest.SelectedIndex '(((DataGridProjectedInterest.CurrentPageIndex) * DataGridProjectedInterest.PageSize) + DataGridProjectedInterest.SelectedIndex)

            Session("dataset_index") = g_integer_count

            If Me.PopulateDataIntoControls(g_integer_count) = True Then
                ''Me.ButtonEdit.Enabled = True
                Me.ButtonDelete.Enabled = True
            Else
                ''Me.ButtonEdit.Enabled = True
                Me.ButtonDelete.Enabled = True
            End If
            Me.TextBoxInterestYear.ReadOnly = True
            Me.TextBoxInterestRate.ReadOnly = False



            Me.ButtonSave.Enabled = True
            Me.ButtonCancel.Enabled = True
            Me.TextBoxFind.ReadOnly = True
            Me.ButtonSearch.Enabled = False
            Me.ButtonAdd.Enabled = False
            Me.ButtonDelete.Enabled = False
            Me.ButtonOk.Enabled = False
            ''Me.ButtonEdit.Enabled = False

            g_bool_AddFlag = False
            Session("BoolAddFlag") = g_bool_AddFlag

            g_bool_EditFlag = True
            Session("BoolEditFlag") = g_bool_EditFlag

            g_bool_DeleteFlag = False
            Session("BoolDeleteFlag") = g_bool_DeleteFlag
            'Me.TextBoxInterestYear.Text = Convert.ToString(DataGridProjectedInterest.SelectedItem.Cells(1).Text, fp)
            'Me.TextBoxInterestRate.Text = Convert.ToString(DataGridProjectedInterest.SelectedItem.Cells(2).Text, fp)
            'Me.TextBoxInterestEndDate.Text = Convert.ToString(DataGridProjectedInterest.SelectedItem.Cells(3).Text, fp)
            'Me.TextBoxEffectiveDate.Text = Convert.ToString(DataGridProjectedInterest.SelectedItem.Cells(4).Text, fp)


            'g_integer_count = (((DataGridProjectedInterest.CurrentPageIndex) * DataGridProjectedInterest.PageSize) + DataGridProjectedInterest.SelectedIndex)

            'Session("dataset_index") = g_integer_count
        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
        End Try
    End Sub
    Private Function PopulateDataIntoControls(ByVal index As Integer) As Boolean

        Dim l_DataSet As DataSet
        Dim l_DataTable As DataTable
        Dim l_DataRow As DataRow

        Try

            l_DataSet = g_dataset_dsProjectedInterestRate

            If Not l_DataSet Is Nothing Then

                l_DataTable = l_DataSet.Tables("Projected Interest Rates")

                If Not l_DataTable Is Nothing Then

                    If index < l_DataTable.Rows.Count Then

                        l_DataRow = l_DataTable.Rows.Item(index)

                        If (Not l_DataRow Is Nothing) Then

                            Me.TextBoxInterestYear.Text = CType(l_DataRow("Interest Year"), String).Trim
                            If l_DataRow("Interest Rate").GetType.ToString = "System.DBNull" Then
                                Me.TextBoxInterestRate.Text = String.Empty
                            Else
                                Me.TextBoxInterestRate.Text = CType(l_DataRow("Interest Rate"), String).Trim
                            End If

                            If l_DataRow("Interest end date").GetType.ToString = "System.DBNull" Then
                                Me.TextBoxInterestEndDate.Text = String.Empty
                            Else
                                Me.TextBoxInterestEndDate.Text = CType(l_DataRow("Interest end date"), String).Trim
                            End If

                            If l_DataRow("Effective Date").GetType.ToString = "System.DBNull" Then
                                Me.TextBoxEffectiveDate.Text = "01/01/1900"
                            Else
                                Me.TextBoxEffectiveDate.Text = CType(l_DataRow("Effective Date"), String).Trim
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
    Private Sub ButtonAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAdd.Click
        Try
            Dim checkSecurity As String = SecurityCheck.Check_Authorization(strFormName, Convert.ToInt32(Session("LoggedUserKey")))
            If Not checkSecurity.Equals("True") Then
                MessageBox.Show(PlaceHolder1, "YMCA-YRS", checkSecurity, MessageBoxButtons.Stop)
                Exit Sub
            End If
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
            Me.TextBoxInterestYear.ReadOnly = False
            Me.TextBoxInterestRate.ReadOnly = False


            Me.ButtonSave.Enabled = True
            Me.ButtonCancel.Enabled = True
            Me.TextBoxFind.ReadOnly = True
            Me.ButtonSearch.Enabled = False
            ''Me.ButtonEdit.Enabled = False
            Me.ButtonDelete.Enabled = False
            Me.ButtonOk.Enabled = False
            Me.ButtonAdd.Enabled = False

            ' Making TextBoxes Blank
            Me.TextBoxInterestYear.Text = String.Empty
            Me.TextBoxInterestRate.Text = String.Empty
            Me.TextBoxInterestEndDate.Text = String.Empty
            Me.TextBoxEffectiveDate.Text = String.Empty
        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
        End Try
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
            'If add Flag Is true then do Insertion else update
            g_bool_AddFlag = Session("BoolAddFlag")
            If g_bool_AddFlag = True Then
                If Not g_dataset_dsProjectedInterestRate Is Nothing Then

                    InsertRow = g_dataset_dsProjectedInterestRate.Tables(0).NewRow

                    ' Assign the values.
                    InsertRow.Item("Interest Year") = TextBoxInterestYear.Text.Trim
                    If Me.TextBoxInterestRate.Text.Trim.Length = 0 Then
                        InsertRow.Item("Interest Rate") = 0
                    Else
                        InsertRow.Item("Interest Rate") = TextBoxInterestRate.Text.Trim
                    End If

                    If Me.TextBoxInterestEndDate.Text.Trim.Length = 0 Then
                        InsertRow.Item("Interest end date") = "01/01/1900"
                    Else
                        InsertRow.Item("Interest end date") = TextBoxInterestEndDate.Text.Trim
                    End If

                    If Me.TextBoxEffectiveDate.Text.Trim.Length = 0 Then
                        InsertRow.Item("Effective Date") = "01/01/1900"
                    Else
                        InsertRow.Item("Effective Date") = TextBoxEffectiveDate.Text.Trim
                    End If





                    ' Insert the row into Table.
                    g_dataset_dsProjectedInterestRate.Tables(0).Rows.Add(InsertRow)
                    g_integer_count = 0
                    Me.PopulateDataIntoControls(g_integer_count)
                End If

            Else

                If Not g_dataset_dsProjectedInterestRate Is Nothing Then

                    g_integer_count = Session("dataset_index")
                    l_DataRow = g_dataset_dsProjectedInterestRate.Tables(0).Rows(g_integer_count)

                    If Not l_DataRow Is Nothing Then

                        'Update the values in current(selected) DataRow
                        'if interest rate is empty then take it zero
                        If Me.TextBoxInterestRate.Text.Trim.Length = 0 Then
                            l_DataRow("Interest Rate") = 0
                        Else
                            l_DataRow("Interest Rate") = TextBoxInterestRate.Text.Trim
                        End If

                        If Me.TextBoxInterestEndDate.Text.Trim.Length = 0 Then
                            l_DataRow("Interest end date") = "01/01/1900"
                        Else
                            l_DataRow("Interest end date") = TextBoxInterestEndDate.Text.Trim
                        End If

                        If Me.TextBoxEffectiveDate.Text.Trim.Length = 0 Then
                            l_DataRow("Effective Date") = "01/01/1900"
                        Else
                            l_DataRow("Effective Date") = TextBoxEffectiveDate.Text.Trim

                            g_integer_count = Session("dataset_index")
                            If (g_integer_count <> -1) Then
                                Me.PopulateDataIntoControls(g_integer_count)
                            End If



                        End If

                    End If

                End If
            End If

            ' Call business layer to Save the DataSet/
            YMCARET.YmcaBusinessObject.MetaProjectedInterest.InsertProjectedInterestRate(g_dataset_dsProjectedInterestRate)
            PopulateData()

            Me.ButtonSave.Enabled = False
            Me.ButtonCancel.Enabled = False
            Me.ButtonAdd.Enabled = True
            ''Me.ButtonEdit.Enabled = False
            Me.TextBoxFind.ReadOnly = False
            Me.ButtonSearch.Enabled = True
            Me.ButtonDelete.Enabled = False
            Me.ButtonOk.Enabled = True
            Me.TextBoxInterestYear.ReadOnly = True
            Me.TextBoxInterestRate.ReadOnly = True
            Me.DataGridProjectedInterest.SelectedIndex = -1

            ' Making TextBoxes Blank

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
                Response.Redirect("ErrorPageForm.aspx", False)
            End If
        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
        End Try
    End Sub

    ''Private Sub ButtonEdit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonEdit.Click
    ''    If g_bool_SearchFlag = True Then
    ''        SearchAndPopulateData()
    ''    Else
    ''        PopulateData()
    ''    End If

    ''    ' Enable / Disable the controls.

    ''    Me.TextBoxInterestYear.ReadOnly = True
    ''    Me.TextBoxInterestRate.ReadOnly = False



    ''    Me.ButtonSave.Enabled = True
    ''    Me.ButtonCancel.Enabled = True
    ''    Me.TextBoxFind.ReadOnly = True
    ''    Me.ButtonSearch.Enabled = False
    ''    Me.ButtonAdd.Enabled = False
    ''    Me.ButtonDelete.Enabled = False
    ''    Me.ButtonOk.Enabled = False
    ''    Me.ButtonEdit.Enabled = False

    ''    g_bool_AddFlag = False
    ''    Session("BoolAddFlag") = g_bool_AddFlag

    ''    g_bool_EditFlag = True
    ''    Session("BoolEditFlag") = g_bool_EditFlag

    ''    g_bool_DeleteFlag = False
    ''    Session("BoolDeleteFlag") = g_bool_DeleteFlag

    ''End Sub

    Private Sub DataGridProjectedInterest_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles DataGridProjectedInterest.PageIndexChanged
        Try
            DataGridProjectedInterest.CurrentPageIndex = e.NewPageIndex

            'Bind the DataGrid again with the Data Source
            'depending on wheather Search Flag is true or False

            If g_bool_SearchFlag = True Then
                SearchAndPopulateData()
            Else
                PopulateData()
            End If
        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
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
            Me.ButtonDelete.Enabled = False
            Me.TextBoxFind.ReadOnly = False
            Me.ButtonSearch.Enabled = True
            Me.ButtonOk.Enabled = True

            Me.TextBoxInterestYear.ReadOnly = True
            Me.TextBoxInterestRate.ReadOnly = True
            Me.DataGridProjectedInterest.SelectedIndex = -1

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
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
        End Try
    End Sub

    Private Sub ButtonDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonDelete.Click
        Try
            Dim checkSecurity As String = SecurityCheck.Check_Authorization(strFormName, Convert.ToInt32(Session("LoggedUserKey")))
            If Not checkSecurity.Equals("True") Then
                MessageBox.Show(PlaceHolder1, "YMCA-YRS", checkSecurity, MessageBoxButtons.Stop)
                Exit Sub
            End If
            If g_bool_SearchFlag = True Then
                SearchAndPopulateData()
            Else
                PopulateData()
            End If

            MessageBox.Show(PlaceHolder1, "Please Confirm", "Are you SURE you want to delete this Projected Interest Rates?", MessageBoxButtons.YesNo)
        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
        End Try

    End Sub
    Public Sub DeleteSub()
        Try
            If g_bool_SearchFlag = True Then
                SearchAndPopulateData()
            Else
                PopulateData()
            End If
            g_bool_DeleteFlag = True
            Session("BoolDeleteFlag") = g_bool_DeleteFlag

            Dim l_DataRow As DataRow
            If Not g_dataset_dsProjectedInterestRate Is Nothing Then

                g_integer_count = Session("dataset_index")
                l_DataRow = g_dataset_dsProjectedInterestRate.Tables(0).Rows(g_integer_count)
                l_DataRow.Delete()
            End If
            YMCARET.YmcaBusinessObject.MetaProjectedInterest.InsertProjectedInterestRate(g_dataset_dsProjectedInterestRate)
            PopulateData()


            Me.ButtonDelete.Enabled = False
            ''Me.ButtonEdit.Enabled = False
        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
        End Try
    End Sub


    Private Sub ButtonOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonOk.Click
        Try
            Session("InterestSort") = Nothing
            Response.Redirect("MainWebForm.aspx", False)
        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
        End Try
    End Sub

    Private Sub DataGridProjectedInterest_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DataGridProjectedInterest.ItemDataBound
        Dim l_button_Select As ImageButton
        Try
            l_button_Select = e.Item.FindControl("ImageButtonSelect")
            If (e.Item.ItemIndex = Me.DataGridProjectedInterest.SelectedIndex And Me.DataGridProjectedInterest.SelectedIndex >= 0) Then
                l_button_Select.ImageUrl = "images\selected.gif"
            End If
            e.Item.Cells(3).Visible = False
            e.Item.Cells(4).Visible = False
        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
       
        End Try
    End Sub

    Private Sub DataGridProjectedInterest_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles DataGridProjectedInterest.SortCommand
        Try
            Me.DataGridProjectedInterest.SelectedIndex = -1
            If Not viewstate("Dataset_Interest") Is Nothing Then
                Dim dv As New DataView
                Dim SortExpression As String
                SortExpression = e.SortExpression
                g_dataset_dsProjectedInterestRate = viewstate("Dataset_Interest")
                dv = g_dataset_dsProjectedInterestRate.Tables(0).DefaultView
                dv.Sort = SortExpression
                If Not Session("InterestSort") Is Nothing Then
                    If Session("InterestSort").ToString.Trim.EndsWith("ASC") Then
                        dv.Sort = SortExpression + " DESC"
                    Else
                        dv.Sort = SortExpression + " ASC"
                    End If
                Else
                    dv.Sort = SortExpression + " ASC"
                End If
                Me.DataGridProjectedInterest.DataSource = Nothing
                Me.DataGridProjectedInterest.DataSource = dv
                Me.DataGridProjectedInterest.DataBind()
                Session("InterestSort") = dv.Sort
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
            DataGridProjectedInterest.Enabled = False
            DataGridProjectedInterest.ToolTip = YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.TOOLTIP_READONLY).DisplayText
        End If
    End Sub
    'END : PK | 02-28-2019 | YRS-AT-4248 | Check ReadOnly Flag, if true then disable the buttons on which security is not implemented and add Tool tip.
End Class

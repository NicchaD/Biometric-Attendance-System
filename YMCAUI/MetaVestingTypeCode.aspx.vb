'*******************************************************************************
' Copyright YMCA Retirement Fund All Rights Reserved. 
'
' Project Name		:	YMCA-YRS
' FileName			:	MetaVestingTypeCode.aspx.vb
' Author Name		:	Vartika Jain
' Employee ID		:	33495
' Email				:	vartika.jain@3i-infotech.com
' Contact No		:	8733
' Creation Time		:	5/17/2005 5:13:53 PM
' Program Specification Name	:	YMCA PS 5.4.1.14
' Unit Test Plan Name			:	
' Description					:	<<Please put the brief description here...>>
'
'Changed by             :  Shefali Bharti
'Changed on             :  08-18-2005
'Change Description     :  Changing whole of the code
'Cache to Session       :   Ragesh 34231 02/02/06 Cache to Session
'****************************************************
'Modification History
'*******************************************************************************
'Modified by                    Date                Description
'*******************************************************************************
'Neeraj Singh                   12/Nov/2009         Added form name for security issue YRS 5.0-940 
'Shashi Singh                   15/June/2010        Migration related changes.
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
Public Class MetaVestingTypeCode
    Inherits System.Web.UI.Page
    'below line is Added by Neeraj for issue id YRS 5.0-940 
    Dim strFormName As String = New String("MetaVestingTypeCode.aspx")
    'End issue id YRS 5.0-940


#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents LabelLook As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxFind As System.Web.UI.WebControls.TextBox
    Protected WithEvents DataGridMetaVestingType As System.Web.UI.WebControls.DataGrid
    Protected WithEvents LabelVestingTypeCode As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxVestingTypeCode As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelShortDesc As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxShortDesc As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelDesc As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxDesc As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelEligMonth As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxEligMonth As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelVestedMonth As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxVestedMonth As System.Web.UI.WebControls.TextBox
    Protected WithEvents ButtonSave As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonCancel As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonAdd As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonDelete As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonOK As System.Web.UI.WebControls.Button
    Protected WithEvents LabelAdmin As System.Web.UI.WebControls.Label
    Protected WithEvents LabelHeading As System.Web.UI.WebControls.Label
    Protected WithEvents Menu1 As skmMenu.Menu
    '' Protected WithEvents ButtonEditForm As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonSearch As System.Web.UI.WebControls.Button
    Protected WithEvents LabelNoRecordFound As System.Web.UI.WebControls.Label
    Protected WithEvents PlaceHolder1 As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents RequiredFieldValidator1 As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents ValidationSummary1 As System.Web.UI.WebControls.ValidationSummary
    Protected WithEvents RegularExpressionValidator1 As System.Web.UI.WebControls.RegularExpressionValidator
    Protected WithEvents RegularExpressionValidator2 As System.Web.UI.WebControls.RegularExpressionValidator
    Protected WithEvents ButtonEditForm As System.Web.UI.WebControls.Button

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Dim g_dataset_dsVestingTypeCode As New DataSet
    Dim g_bool_SearchFlag As New Boolean
    Dim g_bool_EditFlag As New Boolean
    Dim g_bool_AddFlag As New Boolean
    Dim g_integer_count As New Integer
    Dim g_bool_DeleteFlag As New Boolean

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            'Put user code to initialize the page here
            'Me.DataGridMetaVestingType.DataSource = CommonModule.CreateDataSource
            'Me.DataGridMetaVestingType.DataBind()
            Me.LabelDesc.AssociatedControlID = Me.TextBoxDesc.ID
            Me.LabelEligMonth.AssociatedControlID = Me.TextBoxEligMonth.ID
            Me.LabelLook.AssociatedControlID = Me.TextBoxFind.ID
            Me.LabelShortDesc.AssociatedControlID = Me.TextBoxShortDesc.ID
            Me.LabelVestedMonth.AssociatedControlID = Me.TextBoxVestedMonth.ID
            Me.LabelVestingTypeCode.AssociatedControlID = Me.TextBoxVestingTypeCode.ID
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
            ' deletes if yes
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
                ''Me.ButtonEditForm.Enabled = False
                Me.ButtonAdd.Enabled = True
                Me.ButtonOK.Enabled = True

                Me.ButtonSearch.Enabled = True
                Me.TextBoxFind.ReadOnly = False

                Me.TextBoxVestingTypeCode.ReadOnly = True
                Me.TextBoxShortDesc.ReadOnly = True
                Me.TextBoxDesc.ReadOnly = True
                Me.TextBoxVestedMonth.ReadOnly = True
                Me.TextBoxEligMonth.ReadOnly = True


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
            If Not g_dataset_dsVestingTypeCode Is Nothing Then

                g_integer_count = Session("dataset_index")
                l_DataRow = g_dataset_dsVestingTypeCode.Tables(0).Rows(g_integer_count)
                l_DataRow.Delete()
            End If
            YMCARET.YmcaBusinessObject.MetaVestingTypeCodeBOClass.InsertVestingTypeCodes(g_dataset_dsVestingTypeCode)
            PopulateData()


            Me.ButtonDelete.Enabled = False
        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
        End Try
        ''Me.ButtonEditForm.Enabled = False
    End Sub
    Public Sub PopulateData()
        'Dim Cache As CacheManager
        Try

            g_dataset_dsVestingTypeCode = YMCARET.YmcaBusinessObject.MetaVestingTypeCodeBOClass.LookUpVestingTypeCodes()
            viewstate("Dataset_VestingType") = g_dataset_dsVestingTypeCode
            '/b Cache to Session       :   Ragesh 34231 02/02/06 Cache to Session
            'Cache = CacheFactory.GetCacheManager()
            'Cache.Add("VEsting Type Code", g_dataset_dsVestingTypeCode)

            Session("g_dataset_dsVestingTypeCode") = g_dataset_dsVestingTypeCode
            '/e Cache to Session       :   Ragesh 34231 02/02/06 Cache to Session

            If g_dataset_dsVestingTypeCode.Tables.Count = 0 Then
                ''Me.ButtonEditForm.Enabled = False
                Me.TextBoxFind.ReadOnly = True
                Me.ButtonSearch.Enabled = False
                Me.ButtonDelete.Enabled = False
                Me.LabelNoRecordFound.Visible = True
            Else

                Me.DataGridMetaVestingType.DataSource = g_dataset_dsVestingTypeCode.Tables(0)
                Me.DataGridMetaVestingType.DataBind()

            End If

            ''CommonModule.HideColumnsinDataGrid(g_dataset_dsVestingTypeCode, Me.DataGridMetaVestingType, "Short Desc,Desc,Elig Month,Vested Month")

            'Me.DataGridMetaVestingType.AllowSorting = True

        Catch sqlEx As SqlException
            If sqlEx.Number = 60006 Then
                Me.LabelNoRecordFound.Visible = True
                '/b Cache to Session       :   Ragesh 34231 02/02/06 Cache to Session
                'g_dataset_dsVestingTypeCode = Cache.GetData("Vesting Type Code")
                g_dataset_dsVestingTypeCode = CType(Session("g_dataset_dsVestingTypeCode"), DataSet)
                '/e Cache to Session       :   Ragesh 34231 02/02/06 Cache to Session
                g_dataset_dsVestingTypeCode.Tables(0).Clear()

                Me.DataGridMetaVestingType.DataSource = g_dataset_dsVestingTypeCode
                Me.DataGridMetaVestingType.DataBind()

            Else
                Response.Redirect("ErrorPageForm.aspx")
            End If

        Catch secEx As System.Security.SecurityException
            Response.Redirect("ErrorPageForm.aspx")
        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
        End Try
    End Sub

    Private Function PopulateDataIntoControls(ByVal index As Integer) As Boolean

        Dim l_DataSet As DataSet
        Dim l_DataTable As DataTable
        Dim l_DataRow As DataRow

        Try

            l_DataSet = g_dataset_dsVestingTypeCode

            If Not l_DataSet Is Nothing Then

                l_DataTable = l_DataSet.Tables("Vesting Type Code")

                If Not l_DataTable Is Nothing Then

                    If index < l_DataTable.Rows.Count Then

                        l_DataRow = l_DataTable.Rows.Item(index)

                        If (Not l_DataRow Is Nothing) Then

                            Me.TextBoxVestingTypeCode.Text = CType(l_DataRow("Vesting Type Code"), String).Trim
                            If l_DataRow("Description").GetType.ToString = "System.DBNull" Then
                                Me.TextBoxShortDesc.Text = String.Empty
                            Else
                                Me.TextBoxShortDesc.Text = CType(l_DataRow("Description"), String).Trim
                            End If

                            If l_DataRow("Desc").GetType.ToString = "System.DBNull" Then
                                Me.TextBoxDesc.Text = String.Empty
                            Else
                                Me.TextBoxDesc.Text = CType(l_DataRow("Desc"), String).Trim
                            End If

                            If l_DataRow("Elig Month").GetType.ToString = "System.DBNull" Then
                                Me.TextBoxEligMonth.Text = 0
                            Else
                                Me.TextBoxEligMonth.Text = CType(l_DataRow("Elig Month"), String).Trim
                            End If

                            If l_DataRow("Vested Month").GetType.ToString = "System.DBNull" Then
                                Me.TextBoxVestedMonth.Text = 0
                            Else
                                Me.TextBoxVestedMonth.Text = CType(l_DataRow("Vested Month"), String).Trim
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


    Private Sub DataGridMetaVestingType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DataGridMetaVestingType.SelectedIndexChanged
        Try
            If g_bool_SearchFlag = True Then
                SearchAndPopulateData()
            Else
                PopulateData()
            End If

            ''Me.ButtonEditForm.Enabled = True
            g_integer_count = DataGridMetaVestingType.SelectedIndex

            Session("dataset_index") = g_integer_count
            If Me.PopulateDataIntoControls(g_integer_count) = True Then
                ''Me.ButtonEditForm.Enabled = True
                Me.ButtonDelete.Enabled = True
            Else
                ''Me.ButtonEditForm.Enabled = True
                Me.ButtonDelete.Enabled = True
            End If

            Me.TextBoxVestingTypeCode.ReadOnly = True
            Me.TextBoxShortDesc.ReadOnly = False
            Me.TextBoxDesc.ReadOnly = False
            Me.TextBoxVestedMonth.ReadOnly = False
            Me.TextBoxEligMonth.ReadOnly = False


            Me.ButtonSave.Enabled = True
            Me.ButtonCancel.Enabled = True
            Me.TextBoxFind.ReadOnly = True
            Me.ButtonSearch.Enabled = False
            Me.ButtonAdd.Enabled = False
            Me.ButtonDelete.Enabled = False
            Me.ButtonOK.Enabled = False

            g_bool_AddFlag = False
            Session("BoolAddFlag") = g_bool_AddFlag

            g_bool_EditFlag = True
            Session("BoolEditFlag") = g_bool_EditFlag

            g_bool_DeleteFlag = False
            Session("BoolDeleteFlag") = g_bool_DeleteFlag
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
            Me.TextBoxDesc.Text = String.Empty
            Me.TextBoxEligMonth.Text = String.Empty
            Me.TextBoxShortDesc.Text = String.Empty
            Me.TextBoxVestedMonth.Text = String.Empty
            Me.TextBoxVestingTypeCode.Text = String.Empty

            ' Do search & populate the data.
            SearchAndPopulateData()
        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
        End Try
    End Sub

    Public Sub SearchAndPopulateData()
        'Dim Cache As CacheManager
        Try
            'Cache = CacheFactory.GetCacheManager()
            'g_dataset_dsProjectedInterestRate = Cache.GetData("Projected Interest Rates")
            g_dataset_dsVestingTypeCode = YMCARET.YmcaBusinessObject.MetaVestingTypeCodeBOClass.SearchVestingTypeCode(Me.TextBoxFind.Text)
            viewstate("Dataset_VestingType") = g_dataset_dsVestingTypeCode
            If g_dataset_dsVestingTypeCode.Tables.Count = 0 Then
                Me.LabelNoRecordFound.Visible = True

            Else
                Me.DataGridMetaVestingType.CurrentPageIndex = 0
                Me.DataGridMetaVestingType.DataSource = g_dataset_dsVestingTypeCode
                Me.DataGridMetaVestingType.DataBind()

            End If

            ''CommonModule.HideColumnsinDataGrid(g_dataset_dsVestingTypeCode, Me.DataGridMetaVestingType, "Short Desc,Elig Month,Vested Month")
        Catch secEx As System.Security.SecurityException
            Response.Redirect("ErrorPageForm.aspx")
        Catch sqlEx As SqlException
            If sqlEx.Number = 60006 Then
                Me.LabelNoRecordFound.Visible = True
                '/b Cache to Session       :   Ragesh 34231 02/02/06 Cache to Session
                'g_dataset_dsVestingTypeCode = Cache.GetData("Vesting Type Code")
                g_dataset_dsVestingTypeCode = DirectCast(Session("g_dataset_dsVestingTypeCode"), DataSet)
                '/b Cache to Session       :   Ragesh 34231 02/02/06 Cache to Session
                g_dataset_dsVestingTypeCode.Tables(0).Clear()

                Me.DataGridMetaVestingType.DataSource = g_dataset_dsVestingTypeCode
                Me.DataGridMetaVestingType.DataBind()
            Else
                Response.Redirect("ErrorPageForm.aspx")
            End If
        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
        End Try



    End Sub

    Private Sub DataGridMetaVestingType_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles DataGridMetaVestingType.PageIndexChanged
        Try
            DataGridMetaVestingType.CurrentPageIndex = e.NewPageIndex

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

    ''Private Sub ButtonEditForm_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonEditForm.Click
    ''    If g_bool_SearchFlag = True Then
    ''        SearchAndPopulateData()
    ''    Else
    ''        PopulateData()
    ''    End If

    ''    ' Enable / Disable the controls.


    ''    Me.TextBoxVestingTypeCode.ReadOnly = True
    ''    Me.TextBoxShortDesc.ReadOnly = False
    ''    Me.TextBoxDesc.ReadOnly = False
    ''    Me.TextBoxVestedMonth.ReadOnly = False
    ''    Me.TextBoxEligMonth.ReadOnly = False


    ''    Me.ButtonSave.Enabled = True
    ''    Me.ButtonCancel.Enabled = True
    ''    Me.TextBoxFind.ReadOnly = True
    ''    Me.ButtonSearch.Enabled = False
    ''    Me.ButtonAdd.Enabled = False
    ''    Me.ButtonDelete.Enabled = False
    ''    Me.ButtonOK.Enabled = False
    ''    Me.ButtonEditForm.Enabled = False

    ''    g_bool_AddFlag = False
    ''    Session("BoolAddFlag") = g_bool_AddFlag

    ''    g_bool_EditFlag = True
    ''    Session("BoolEditFlag") = g_bool_EditFlag

    ''    g_bool_DeleteFlag = False
    ''    Session("BoolDeleteFlag") = g_bool_DeleteFlag
    ''End Sub

    Private Sub ButtonCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonCancel.Click
        Try
            PopulateData()

            'Enable / Disable the controls
            Me.ButtonSave.Enabled = False
            Me.ButtonCancel.Enabled = False
            Me.ButtonAdd.Enabled = True
            ''Me.ButtonEditForm.Enabled = False
            Me.ButtonDelete.Enabled = False
            Me.TextBoxFind.ReadOnly = False
            Me.ButtonSearch.Enabled = True
            Me.ButtonOK.Enabled = True

            'Making Readonly True for all TextBoxes
            Me.TextBoxVestingTypeCode.ReadOnly = True
            Me.TextBoxShortDesc.ReadOnly = True
            Me.TextBoxDesc.ReadOnly = True
            Me.TextBoxVestedMonth.ReadOnly = True
            Me.TextBoxEligMonth.ReadOnly = True
            Me.DataGridMetaVestingType.SelectedIndex = -1
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
            Me.TextBoxVestingTypeCode.ReadOnly = False
            Me.TextBoxShortDesc.ReadOnly = False
            Me.TextBoxDesc.ReadOnly = False
            Me.TextBoxVestedMonth.ReadOnly = False
            Me.TextBoxEligMonth.ReadOnly = False

            ' Making TextBoxes Blank

            Me.TextBoxVestingTypeCode.Text = String.Empty
            Me.TextBoxShortDesc.Text = String.Empty
            Me.TextBoxDesc.Text = String.Empty
            Me.TextBoxVestedMonth.Text = String.Empty
            Me.TextBoxEligMonth.Text = String.Empty

            Me.ButtonSave.Enabled = True
            Me.ButtonCancel.Enabled = True
            Me.TextBoxFind.ReadOnly = True
            Me.ButtonSearch.Enabled = False
            ''Me.ButtonEditForm.Enabled = False
            Me.ButtonDelete.Enabled = False
            Me.ButtonOK.Enabled = False
            Me.ButtonAdd.Enabled = False
        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
        End Try
    End Sub

    Private Sub ButtonOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonOK.Click
        Try
            Session("VestingSort") = Nothing
            Response.Redirect("MainWebForm.aspx", False)
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
                If Not g_dataset_dsVestingTypeCode Is Nothing Then

                    InsertRow = g_dataset_dsVestingTypeCode.Tables(0).NewRow

                    ' Assign the values.
                    InsertRow.Item("Vesting Type Code") = TextBoxVestingTypeCode.Text.Trim
                    If Me.TextBoxShortDesc.Text.Trim.Length = 0 Then
                        InsertRow.Item("Description") = String.Empty
                    Else
                        InsertRow.Item("Description") = TextBoxShortDesc.Text.Trim
                    End If

                    If Me.TextBoxDesc.Text.Trim.Length = 0 Then
                        InsertRow.Item("Desc") = String.Empty
                    Else
                        InsertRow.Item("Desc") = TextBoxDesc.Text.Trim
                    End If

                    If Me.TextBoxEligMonth.Text.Trim.Length = 0 Then
                        InsertRow.Item("Elig Month") = 0
                    Else
                        InsertRow.Item("Elig Month") = TextBoxEligMonth.Text.Trim
                    End If

                    If Me.TextBoxVestedMonth.Text.Trim.Length = 0 Then
                        InsertRow.Item("Vested Month") = 0
                    Else
                        InsertRow.Item("Vested Month") = TextBoxVestedMonth.Text.Trim
                    End If


                    ' Insert the row into Table.
                    g_dataset_dsVestingTypeCode.Tables(0).Rows.Add(InsertRow)

                    g_integer_count = 0
                    Me.PopulateDataIntoControls(g_integer_count)
                End If

            Else



                If Not g_dataset_dsVestingTypeCode Is Nothing Then

                    g_integer_count = Session("dataset_index")
                    l_DataRow = g_dataset_dsVestingTypeCode.Tables(0).Rows(g_integer_count)

                    If Not l_DataRow Is Nothing Then

                        'Update the values in current(selected) DataRow
                        'if interest rate is empty then take it zero
                        l_DataRow("Vesting Type Code") = TextBoxVestingTypeCode.Text.Trim
                        If Me.TextBoxShortDesc.Text.Trim.Length = 0 Then
                            l_DataRow("Description") = String.Empty
                        Else
                            l_DataRow("Description") = TextBoxShortDesc.Text.Trim
                        End If

                        If Me.TextBoxDesc.Text.Trim.Length = 0 Then
                            l_DataRow("Desc") = String.Empty
                        Else
                            l_DataRow("Desc") = TextBoxDesc.Text.Trim
                        End If

                        If Me.TextBoxEligMonth.Text.Trim.Length = 0 Then
                            l_DataRow("Elig Month") = 0
                        Else
                            l_DataRow("Elig Month") = TextBoxEligMonth.Text.Trim
                        End If

                        If Me.TextBoxVestedMonth.Text.Trim.Length = 0 Then
                            l_DataRow("Vested Month") = 0
                        Else
                            l_DataRow("Vested Month") = TextBoxVestedMonth.Text.Trim
                        End If

                        g_integer_count = Session("dataset_index")
                        If (g_integer_count <> -1) Then
                            Me.PopulateDataIntoControls(g_integer_count)
                        End If
                    End If

                End If

            End If

            ' Call business layer to Save the DataSet/
            YMCARET.YmcaBusinessObject.MetaVestingTypeCodeBOClass.InsertVestingTypeCodes(g_dataset_dsVestingTypeCode)
            PopulateData()

            Me.ButtonSave.Enabled = False
            Me.ButtonCancel.Enabled = False
            Me.ButtonAdd.Enabled = True
            ''Me.ButtonEditForm.Enabled = False
            Me.TextBoxFind.ReadOnly = False
            Me.ButtonSearch.Enabled = True
            Me.ButtonDelete.Enabled = False
            Me.ButtonOK.Enabled = True

            Me.TextBoxVestingTypeCode.ReadOnly = True
            Me.TextBoxShortDesc.ReadOnly = True
            Me.TextBoxDesc.ReadOnly = True
            Me.TextBoxVestedMonth.ReadOnly = True
            Me.TextBoxEligMonth.ReadOnly = True
            Me.DataGridMetaVestingType.SelectedIndex = -1

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

            MessageBox.Show(PlaceHolder1, "Please Confirm", "Are you SURE you want to delete this Vesting Type Code?", MessageBoxButtons.YesNo)
        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
        End Try
    End Sub

    Private Sub DataGridMetaVestingType_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DataGridMetaVestingType.ItemDataBound
        Dim l_button_Select As ImageButton
        Try
            l_button_Select = e.Item.FindControl("ImageButtonSelect")
            If (e.Item.ItemIndex = Me.DataGridMetaVestingType.SelectedIndex And Me.DataGridMetaVestingType.SelectedIndex >= 0) Then
                l_button_Select.ImageUrl = "images\selected.gif"
            End If
            e.Item.Cells(3).Visible = False
            e.Item.Cells(4).Visible = False
            e.Item.Cells(5).Visible = False
        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
        
        End Try
    End Sub

    Private Sub DataGridMetaVestingType_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles DataGridMetaVestingType.SortCommand
        Try
            Me.DataGridMetaVestingType.SelectedIndex = -1
            If Not viewstate("Dataset_VestingType") Is Nothing Then
                Dim dv As New DataView
                Dim SortExpression As String
                SortExpression = e.SortExpression
                g_dataset_dsVestingTypeCode = viewstate("Dataset_VestingType")
                dv = g_dataset_dsVestingTypeCode.Tables(0).DefaultView
                dv.Sort = SortExpression
                If Not Session("VestingSort") Is Nothing Then
                    If Session("VestingSort").ToString.Trim.EndsWith("ASC") Then
                        dv.Sort = SortExpression + " DESC"
                    Else
                        dv.Sort = SortExpression + " ASC"
                    End If
                Else
                    dv.Sort = SortExpression + " ASC"
                End If
                Me.DataGridMetaVestingType.DataSource = Nothing
                Me.DataGridMetaVestingType.DataSource = dv
                Me.DataGridMetaVestingType.DataBind()
                Session("VestingSort") = dv.Sort
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
            DataGridMetaVestingType.Enabled = False
            DataGridMetaVestingType.ToolTip = YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.TOOLTIP_READONLY).DisplayText
        End If
    End Sub
    'END : PK | 02-28-2019 | YRS-AT-4248 | Check ReadOnly Flag, if true then disable the buttons on which security is not implemented and add Tool tip.
End Class

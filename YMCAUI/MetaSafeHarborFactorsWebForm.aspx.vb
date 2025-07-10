'*******************************************************************************
' Copyright YMCA Retirement Fund All Rights Reserved. 
'
' Project Name		:	YMCA YRS
' FileName			:	MetaSafeHarborFactorsWebForm.aspx.vb
' Author Name		:	Shefali Bharti
' Employee ID		:	33488
' Email				:	shefali.bharti@3i-infotech.com
' Contact No		:	8743
' Creation Time		:	7/26/2005 5:10:54 PM
' Program Specification Name	:	
' Unit Test Plan Name			:	
' Description					:	coding
' Changed by			:	Shefali Bharti  
' Changed on			:	08/19/2005
' Change Description	:	Coding
' Cache to Session       :   Ragesh 34231 02/02/06 Cache to Session
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
Imports System.Diagnostics



Public Class MetaSafeHarborFactorsWebForm
    Inherits System.Web.UI.Page
    'below line is Added by Neeraj for issue id YRS 5.0-940 
    Dim strFormName As String = New String("MetaSafeHarborFactorsWebForm.aspx")
    'End issue id YRS 5.0-940
    Dim g_dataset_dsSafeHarborFactors As New DataSet
    Dim g_bool_EditFlag As New Boolean
    Protected WithEvents ButtonOk As System.Web.UI.WebControls.Button
    Dim g_integer_count As New Integer
    Protected WithEvents RegularExpressionValidator1 As System.Web.UI.WebControls.RegularExpressionValidator
    Protected WithEvents RegularExpressionValidator2 As System.Web.UI.WebControls.RegularExpressionValidator
    Protected WithEvents RegularExpressionValidator3 As System.Web.UI.WebControls.RegularExpressionValidator
    Protected WithEvents ValidationSummary1 As System.Web.UI.WebControls.ValidationSummary
 
    Protected WithEvents Menu1 As skmMenu.Menu
    Protected WithEvents ButtonEdit As System.Web.UI.WebControls.Button
    Protected WithEvents PlaceHolder1 As System.Web.UI.WebControls.PlaceHolder
    Shared var As Int16

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Shared Sub InitializeComponent()

    End Sub

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    'Private designerPlaceholderDeclaration As System.Object
    Protected WithEvents LabelFactorGroup As System.Web.UI.WebControls.Label
    Protected WithEvents LabelRetireDateLow As System.Web.UI.WebControls.Label
    Protected WithEvents LabelRetireDateHigh As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxFactor As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxFactorGroup As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxRetireDateLow As YMCAUI.DateUserControl
    Protected WithEvents TextBoxRetireDateHigh As YMCAUI.DateUserControl
    Protected WithEvents LabelAgeLow As System.Web.UI.WebControls.Label
    Protected WithEvents LabelAgeHigh As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxAgeHigh As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelFactor As System.Web.UI.WebControls.Label
    Protected WithEvents ButtonSave As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonCancel As System.Web.UI.WebControls.Button
    Protected WithEvents DataGridMetaSafeHarborFactors As System.Web.UI.WebControls.DataGrid
    Protected WithEvents MenuMetaSafeHarborFactors As skmMenu.Menu
    '' Protected WithEvents ButtonEdit As System.Web.UI.WebControls.Button
    Protected WithEvents TextBoxAgeLow As System.Web.UI.WebControls.TextBox

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            'Put user code to initialize the page here

            'Me.DataGridMetaSafeHarborFactors.DataSource = CommonModule.CreateDataSource
            'Me.DataGridMetaSafeHarborFactors.DataBind()

            Menu1.DataSource = Server.MapPath("SimpleXML.xml")
            Menu1.DataBind()
            If Session("LoggedUserKey") Is Nothing Then
                Response.Redirect("Login.aspx", False)
            End If
            Me.LabelFactorGroup.AssociatedControlID = Me.TextBoxFactorGroup.ID

            Me.LabelFactor.AssociatedControlID = Me.TextBoxFactor.ID

            Me.LabelAgeLow.AssociatedControlID = Me.TextBoxAgeLow.ID
            Me.LabelAgeHigh.AssociatedControlID = Me.TextBoxAgeHigh.ID
            CheckReadOnlyMode() 'PK | 02-28-2019 |YRS-AT-4248 | Check security method called here
            ' At First load initializing Edit Flag to False
            If Not Me.IsPostBack Then
                g_bool_EditFlag = False
                Session("BoolEditFlag") = g_bool_EditFlag
                PopulateData()
                ' populating the textboxes with the current row
                g_integer_count = Session("dataset_index")
                If (g_integer_count <> -1) Then
                    Me.PopulateDataIntoControls(g_integer_count)
                End If
            Else

                g_integer_count = Session("dataset_index")
                If (g_integer_count <> -1) Then
                    Me.PopulateDataIntoControls(g_integer_count)
                End If
                'PopulateData()
                ' populating the textboxes with the current row

            End If
        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
        End Try


    End Sub


    Private Function PopulateDataIntoControls(ByVal index As Integer) As Boolean

        Dim l_DataSet As DataSet
        Dim l_DataTable As DataTable
        Dim l_DataRow As DataRow

        Try

            l_DataSet = g_dataset_dsSafeHarborFactors

            If Not l_DataSet Is Nothing Then

                l_DataTable = l_DataSet.Tables("Safe Harbor Factors")

                If Not l_DataTable Is Nothing Then

                    If index < l_DataTable.Rows.Count Then

                        l_DataRow = l_DataTable.Rows.Item(index)

                        If (Not l_DataRow Is Nothing) Then

                            Me.TextBoxFactorGroup.Text = CType(l_DataRow("Factor Group"), String).Trim
                            If l_DataRow(" Retire Date Low").GetType.ToString = "System.DBNull" Then
                                Me.TextBoxRetireDateLow.Text = "01/01/1900"
                            Else
                                Me.TextBoxRetireDateLow.Text = CType(l_DataRow(" Retire Date Low"), String).Trim
                            End If

                            If l_DataRow("Retire Date High").GetType.ToString = "System.DBNull" Then
                                Me.TextBoxRetireDateHigh.Text = "01/01/1900"
                            Else
                                Me.TextBoxRetireDateHigh.Text = CType(l_DataRow("Retire Date High"), String).Trim
                            End If

                            If l_DataRow("Age Low").GetType.ToString = "System.DBNull" Then
                                Me.TextBoxAgeLow.Text = 0
                            Else
                                Me.TextBoxAgeLow.Text = CType(l_DataRow("Age Low"), String).Trim
                            End If

                            If l_DataRow("Age High").GetType.ToString = "System.DBNull" Then
                                Me.TextBoxAgeHigh.Text = 0
                            Else
                                Me.TextBoxAgeHigh.Text = CType(l_DataRow("Age High"), String).Trim
                            End If

                            If l_DataRow("Factor").GetType.ToString = "System.DBNull" Then
                                Me.TextBoxFactor.Text = 0
                            Else
                                Me.TextBoxFactor.Text = CType(l_DataRow("Factor"), String).Trim
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
    Public Sub PopulateData()
        Try
            g_dataset_dsSafeHarborFactors = YMCARET.YmcaBusinessObject.MetaSafeHarborFactorsBOClass.LookupSafeHarborFactors()
            viewstate("Dataset_SafeHarbor") = g_dataset_dsSafeHarborFactors
            If g_dataset_dsSafeHarborFactors.Tables.Count = 0 Then
                ''Me.ButtonEdit.Enabled = False
            Else

                Me.DataGridMetaSafeHarborFactors.DataSource = g_dataset_dsSafeHarborFactors.Tables(0)
                Me.DataGridMetaSafeHarborFactors.DataBind()

            End If

            ''CommonModule.HideColumnsinDataGrid(g_dataset_dsSafeHarborFactors, Me.DataGridMetaSafeHarborFactors, " Retire Date Low, Retire Date High, Age High, Factor")
        Catch sqlEx As SqlException
            Response.Redirect("ErrorPageForm.aspx")
        Catch secEx As System.Security.SecurityException
            Response.Redirect("ErrorPageForm.aspx")
        End Try
    End Sub

    Private Sub DataGridMetaSafeHarborFactors_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles DataGridMetaSafeHarborFactors.PageIndexChanged
        Try
            DataGridMetaSafeHarborFactors.CurrentPageIndex = e.NewPageIndex

            'Bind the DataGrid again with the Data Source
            PopulateData()
        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
        End Try
    End Sub

    Private Sub DataGridMetaSafeHarborFactors_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DataGridMetaSafeHarborFactors.SelectedIndexChanged
        Try
            PopulateData()
            'Me.ButtonEdit.Enabled = True
            g_integer_count = DataGridMetaSafeHarborFactors.SelectedIndex '(((DataGridMetaSafeHarborFactors.CurrentPageIndex) * DataGridMetaSafeHarborFactors.PageSize) + DataGridMetaSafeHarborFactors.SelectedIndex)

            Session("dataset_index") = g_integer_count
            If Me.PopulateDataIntoControls(g_integer_count) = True Then
                'Me.ButtonEdit.Enabled = True
                'Else
                'Me.ButtonEdit.Enabled = True
            End If

            Me.TextBoxFactorGroup.ReadOnly = True
            Me.TextBoxAgeHigh.ReadOnly = False
            Me.TextBoxAgeLow.ReadOnly = False
            Me.TextBoxFactor.ReadOnly = False

            Me.TextBoxRetireDateHigh.Enabled = True
            Me.TextBoxRetireDateLow.Enabled = True

            ''Me.PopCalendarRetDateHigh.Enabled = True
            ''Me.PopCalendarRetDateLow.Enabled = True

            Me.ButtonSave.Enabled = True
            Me.ButtonCancel.Enabled = True
            Me.ButtonOk.Enabled = False
            'Me.ButtonEdit.Enabled = False

            g_bool_EditFlag = True
            Session("BoolEditFlag") = g_bool_EditFlag

        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
       
    End Sub

    ''Private Sub ButtonEdit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonEdit.Click
    ''    PopulateData()
    ''    Me.TextBoxFactorGroup.ReadOnly = True
    ''    Me.TextBoxAgeHigh.ReadOnly = False
    ''    Me.TextBoxAgeLow.ReadOnly = False
    ''    Me.TextBoxFactor.ReadOnly = False

    ''    Me.TextBoxRetireDateHigh.Enabled = True
    ''    Me.TextBoxRetireDateLow.Enabled = True

    ''    ''Me.PopCalendarRetDateHigh.Enabled = True
    ''    ''Me.PopCalendarRetDateLow.Enabled = True

    ''    Me.ButtonSave.Enabled = True
    ''    Me.ButtonCancel.Enabled = True
    ''    Me.ButtonOk.Enabled = False
    ''    Me.ButtonEdit.Enabled = False

    ''    g_bool_EditFlag = True
    ''    Session("BoolEditFlag") = g_bool_EditFlag
    ''End Sub

    Private Sub ButtonSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonSave.Click
        Dim l_DataRow As DataRow
        Try

            'Added by neeraj on 18-Nov-2009 : issue is YRS 5.0-940
            Dim checkSecurity As String = SecurityCheck.Check_Authorization(strFormName, Convert.ToInt32(Session("LoggedUserKey")))

            If Not checkSecurity.Equals("True") Then
                MessageBox.Show(PlaceHolder1, "YMCA-YRS", checkSecurity, MessageBoxButtons.Stop)
                Exit Sub
            End If
            'End : YRS 5.0-940

            PopulateData()
            If Not g_dataset_dsSafeHarborFactors Is Nothing Then

                g_integer_count = Session("dataset_index")
                l_DataRow = g_dataset_dsSafeHarborFactors.Tables(0).Rows(g_integer_count)

                If Not l_DataRow Is Nothing Then

                    'Update the values in current(selected) DataRow
                    If Me.TextBoxRetireDateLow.Text.Trim.Length = 0 Then
                        l_DataRow(" Retire Date Low") = "01/01/1900"
                    Else
                        l_DataRow(" Retire Date Low") = Me.TextBoxRetireDateLow.Text.Trim
                    End If

                    If Me.TextBoxRetireDateHigh.Text.Trim.Length = 0 Then
                        l_DataRow("Retire Date High") = "01/01/1900"
                    Else
                        l_DataRow("Retire Date High") = Me.TextBoxRetireDateHigh.Text.Trim
                    End If

                    If Me.TextBoxAgeLow.Text.Trim.Length = 0 Then
                        l_DataRow("Age Low") = 0
                    Else
                        l_DataRow("Age Low") = Me.TextBoxAgeLow.Text.Trim
                    End If

                    If Me.TextBoxAgeHigh.Text.Trim.Length = 0 Then
                        l_DataRow("Age High") = 0
                    Else
                        l_DataRow("Age High") = Me.TextBoxAgeHigh.Text.Trim
                    End If

                    If Me.TextBoxFactor.Text.Trim.Length = 0 Then
                        l_DataRow("Factor") = 0
                    Else
                        l_DataRow("Factor") = Me.TextBoxFactor.Text.Trim
                    End If
                    g_integer_count = Session("dataset_index")
                    If (g_integer_count <> -1) Then
                        Me.PopulateDataIntoControls(g_integer_count)
                    End If
                End If
            End If

            ' Call business layer to Save the DataSet/
            YMCARET.YmcaBusinessObject.MetaSafeHarborFactorsBOClass.InsertSafeHarborFactors(g_dataset_dsSafeHarborFactors)
            PopulateData()

            Me.ButtonSave.Enabled = False
            Me.ButtonCancel.Enabled = False
            ''Me.ButtonEdit.Enabled = False
            Me.ButtonOk.Enabled = True

            Me.TextBoxFactorGroup.ReadOnly = True
            Me.TextBoxAgeHigh.ReadOnly = True
            Me.TextBoxAgeLow.ReadOnly = True
            Me.TextBoxFactor.ReadOnly = True

            ''Me.PopCalendarRetDateHigh.Enabled = False
            ''Me.PopCalendarRetDateLow.Enabled = False
            Me.TextBoxRetireDateHigh.Enabled = False
            Me.TextBoxRetireDateLow.Enabled = False
            Me.DataGridMetaSafeHarborFactors.SelectedIndex = -1
            g_bool_EditFlag = False
            Session("BoolEditFlag") = g_bool_EditFlag

        Catch secEx As System.Security.SecurityException
            Response.Redirect("ErrorPageForm.aspx")
        Catch sqlEx As SqlException
            If sqlEx.Number = 60006 Then

                Me.ButtonSave.Enabled = False
            Else
                Response.Redirect("ErrorPageForm.aspx")
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
            ''Me.ButtonEdit.Enabled = False
            Me.ButtonOk.Enabled = True

            Me.TextBoxFactorGroup.ReadOnly = True
            Me.TextBoxAgeHigh.ReadOnly = True
            Me.TextBoxAgeLow.ReadOnly = True
            Me.TextBoxFactor.ReadOnly = True

            ''Me.PopCalendarRetDateHigh.Enabled = False
            ''Me.PopCalendarRetDateLow.Enabled = False

            Me.TextBoxRetireDateHigh.Enabled = False
            Me.TextBoxRetireDateLow.Enabled = False
            Me.DataGridMetaSafeHarborFactors.SelectedIndex = -1
            g_integer_count = Session("dataset_index")
            If (g_integer_count <> -1) Then
                Me.PopulateDataIntoControls(g_integer_count)
            End If


            g_bool_EditFlag = False
            Session("BoolEditFlag") = g_bool_EditFlag
        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
        End Try

    End Sub


    Private Sub ButtonOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonOk.Click
        Try
            Session("SafeHarborSort") = Nothing
            Response.Redirect("MainWebForm.aspx", False)
        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
        End Try
    End Sub

    Private Sub DataGridMetaSafeHarborFactors_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DataGridMetaSafeHarborFactors.ItemDataBound
        Dim l_button_Select As ImageButton
        Try
            l_button_Select = e.Item.FindControl("ImageButtonSelect")
            If (e.Item.ItemIndex = Me.DataGridMetaSafeHarborFactors.SelectedIndex And Me.DataGridMetaSafeHarborFactors.SelectedIndex >= 0) Then
                l_button_Select.ImageUrl = "images\selected.gif"
            End If
            e.Item.Cells(2).Visible = False
            e.Item.Cells(3).Visible = False
            e.Item.Cells(5).Visible = False
            e.Item.Cells(6).Visible = False
        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
       
        End Try
    End Sub

    Private Sub DataGridMetaSafeHarborFactors_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles DataGridMetaSafeHarborFactors.SortCommand
        Try
            Me.DataGridMetaSafeHarborFactors.SelectedIndex = -1
            If Not viewstate("Dataset_SafeHarbor") Is Nothing Then
                Dim dv As New DataView
                Dim SortExpression As String
                SortExpression = e.SortExpression
                g_dataset_dsSafeHarborFactors = viewstate("Dataset_SafeHarbor")
                dv = g_dataset_dsSafeHarborFactors.Tables(0).DefaultView
                dv.Sort = SortExpression
                If Not Session("SafeHarborSort") Is Nothing Then
                    If Session("SafeHarborSort").ToString.Trim.EndsWith("ASC") Then
                        dv.Sort = SortExpression + " DESC"
                    Else
                        dv.Sort = SortExpression + " ASC"
                    End If
                Else
                    dv.Sort = SortExpression + " ASC"
                End If
                Me.DataGridMetaSafeHarborFactors.DataSource = Nothing
                Me.DataGridMetaSafeHarborFactors.DataSource = dv
                Me.DataGridMetaSafeHarborFactors.DataBind()
                Session("SafeHarborSort") = dv.Sort
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
            DataGridMetaSafeHarborFactors.Enabled = False
            DataGridMetaSafeHarborFactors.ToolTip = YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.TOOLTIP_READONLY).DisplayText
        End If
    End Sub
    'END : PK | 02-28-2019 | YRS-AT-4248 | Check ReadOnly Flag, if true then disable the buttons on which security is not implemented and add Tool tip.
End Class

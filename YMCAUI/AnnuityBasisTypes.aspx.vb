'*******************************************************************************
' Copyright YMCA Retirement Fund All Rights Reserved. 
'
' Project Name		:	YMCA-YRS
' FileName			:	AnnuityBasisTypes.aspx.vb
' Author Name		:	Vartika
' Employee ID		:	33495
' Email				:	vartika.jain@3i-infotech.com
' Contact No		:	8733
' Creation Time		:	5/17/2005 4:14:23 PM
' Program Specification Name	:	YMCA PS 5.4.1.2
' Unit Test Plan Name			:	
' Description					:	<<Please put the brief description here...>>
' Changed by			:	Shefali Bharti
' Changed on			:	30.june.2005
' Change Description	:	Coding 
'
' Changed by			:	SrimuruganG
' Changed on			:	06.July.2005
' Change Description	:   To make FxCop Complient & code review.	
'*******************************************************************************
'Modification History 
'*******************************************************************************
'Modified By                    Date                Description
'*******************************************************************************
'Vipul                          03Feb06             Cache-Session
'Neeraj Singh                   12/Nov/2009         Added form name for security issue YRS 5.0-940 
'Neeraj Singh                   06-Jun-2010         Enahancement Changes
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
Imports System.Globalization

'Imports Microsoft.Practices.EnterpriseLibrary.Caching
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling


Public Class AnnuityBasisTypes
    Inherits System.Web.UI.Page
    'below line is Added by Neeraj for issue id YRS 5.0-940 
    Dim strFormName As String = New String("AnnuityBasisTypes.aspx")
    'End issue id YRS 5.0-940

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Shared Sub InitializeComponent()

    End Sub
    Protected WithEvents LabelLook As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxFind As System.Web.UI.WebControls.TextBox
    Protected WithEvents DataGridAnnuityBasis As System.Web.UI.WebControls.DataGrid
    Protected WithEvents LabelAnnuityBasis As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxAnnuityBasis As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelTermDate As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxTermDate As YMCAUI.DateUserControl
    Protected WithEvents ButtonSave As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonCancel As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonAdd As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonDelete As System.Web.UI.WebControls.Button

    Protected WithEvents LabelAdmin As System.Web.UI.WebControls.Label
    Protected WithEvents Menu1 As skmMenu.Menu
    ''Protected WithEvents ButtonEdit As System.Web.UI.WebControls.Button
    Protected WithEvents TextBoxLongDescription As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelShortDescription As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxShortDescription As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelEffectiveDate As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxEffectiveDate As YMCAUI.DateUserControl
    Protected WithEvents LabelLongDescription As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxAnnuityBasisPercentage As System.Web.UI.WebControls.TextBox
    Protected WithEvents ButtonSearch As System.Web.UI.WebControls.Button
    Protected WithEvents LabelNoRecordFound As System.Web.UI.WebControls.Label
    Protected WithEvents ButtonOk As System.Web.UI.WebControls.Button
    Protected WithEvents RequiredFieldValidator1 As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents RegularExpressionValidator1 As System.Web.UI.WebControls.RegularExpressionValidator
    Protected WithEvents ValidationSummary1 As System.Web.UI.WebControls.ValidationSummary
    ''Protected WithEvents PopCalendarEffectiveDate As RJS.Web.WebControl.PopCalendar
    ''Protected WithEvents PopCalendarTermDate As RJS.Web.WebControl.PopCalendar
    Protected WithEvents PlaceHolder1 As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents Menu2 As skmMenu.Menu
    Protected WithEvents PlaceHolder2 As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents ButtonEdit As System.Web.UI.WebControls.Button
    'Protected WithEvents SaveValidateButton As ValidateButtonClassLibrary.ValidateButtonControl.ValidateButton
    'Protected WithEvents CancelValidateButton As ValidateButtonClassLibrary.ValidateButtonControl.ValidateButton
    'Protected WithEvents AddValidateButton As ValidateButtonClassLibrary.ValidateButtonControl.ValidateButton
    'Protected WithEvents EditValidateButton As ValidateButtonClassLibrary.ValidateButtonControl.ValidateButton
    'Protected WithEvents OKValidateButton As ValidateButtonClassLibrary.ValidateButtonControl.ValidateButton
    Protected WithEvents LabelAnnuityBasisPercentage As System.Web.UI.WebControls.Label
    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    'Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        Me.InitializeComponent()
    End Sub

#End Region



    'Global Variable declaration goes here.

    Dim g_dataset_dsAnnuityBasisTypes As New DataSet
    Dim g_integer_count As New Integer
    Dim g_bool_flag As New Boolean
    Dim g_bool_SearchFlag As New Boolean
    Dim g_bool_EditFlag As New Boolean


    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            ' Associate the controls.
            Me.LabelLook.AssociatedControlID = Me.TextBoxFind.ID
            Me.LabelAnnuityBasis.AssociatedControlID = Me.TextBoxAnnuityBasis.ID
            Me.LabelShortDescription.AssociatedControlID = Me.TextBoxShortDescription.ID
            Me.LabelLongDescription.AssociatedControlID = Me.TextBoxLongDescription.ID

            Me.LabelAnnuityBasisPercentage.AssociatedControlID = Me.TextBoxAnnuityBasisPercentage.ID

            Menu1.DataSource = Server.MapPath("SimpleXML.xml")
            Menu1.DataBind()
            If Session("LoggedUserKey") Is Nothing Then
                Response.Redirect("Login.aspx", False)
            End If
            'Menu2.DataSource = Server.MapPath("SimpleXML.xml")
            'Menu2.DataBind()

            ' At First load initializing Add Flag ,Edit Flag and Search Flag to False
            CheckReadOnlyMode() 'PK | 02-28-2019 |YRS-AT-4248 | Check security method called here
            If Not Me.IsPostBack Then

                g_bool_flag = False
                Session("BoolAddEditFlag") = g_bool_flag

                g_bool_SearchFlag = False
                Session("BoolSearchFlag") = g_bool_SearchFlag

                g_bool_EditFlag = False
                Session("BoolEditFlag") = g_bool_EditFlag

                'Call Common Module once for updating the appdomain.
                'CommonModule.LookupTables()
                'Call method to populate the data.
                PopulateData()
                Me.DataGridAnnuityBasis.Visible = True
                Me.LabelNoRecordFound.Visible = False
                g_integer_count = Session("dataset_index")
                If (g_integer_count <> -1) Then
                    Me.PopulateDataIntoControls(g_integer_count)
                End If
                'if Search flag is true then call 'SearchPopulate' method else call 'populate' method
            Else
                g_integer_count = Session("dataset_index")
                If (g_integer_count <> -1) Then
                    Me.PopulateDataIntoControls(g_integer_count)
                End If

                g_bool_SearchFlag = Session("BoolSearchFlag")
                g_bool_EditFlag = Session("BoolEditFlag")

                'If g_bool_SearchFlag = True Then
                '    SearchAndPopulateData()
                'Else
                '    PopulateData()
                'End If
                Me.DataGridAnnuityBasis.Visible = True
                Me.LabelNoRecordFound.Visible = False
            End If

            If Request.Form("Ok") = "OK" Then
                Me.ButtonCancel.Enabled = False
                Me.ButtonSave.Enabled = False
                ''Me.ButtonEdit.Enabled = False
                Me.ButtonAdd.Enabled = True
                Me.ButtonOk.Enabled = True

                Me.ButtonSearch.Enabled = True
                Me.TextBoxFind.ReadOnly = False

                Me.TextBoxAnnuityBasis.ReadOnly = True
                Me.TextBoxShortDescription.ReadOnly = True
                Me.TextBoxLongDescription.ReadOnly = True
                Me.TextBoxEffectiveDate.Enabled = False
                Me.TextBoxTermDate.Enabled = False
                Me.TextBoxAnnuityBasisPercentage.ReadOnly = True
                'Me.PopCalendarEffectiveDate.Enabled = False
                'Me.PopCalendarTermDate.Enabled = False

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


    'populating the TextBoxes corresponding to the row selected in the DataGrid
    'Row Index is calculated as ((CurrentPage index * Pagesize ) + Selected Index)

    Private Sub DataGridAnnuityBasis_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DataGridAnnuityBasis.SelectedIndexChanged
        Try
            If g_bool_SearchFlag = True Then
                SearchAndPopulateData()
            Else
                PopulateData()
            End If

            ''Me.ButtonEdit.Enabled = True
            g_integer_count = DataGridAnnuityBasis.SelectedIndex '(((DataGridAnnuityBasis.CurrentPageIndex) * DataGridAnnuityBasis.PageSize) + DataGridAnnuityBasis.SelectedIndex)

            Session("dataset_index") = g_integer_count
            If Me.PopulateDataIntoControls(g_integer_count) = True Then
                '    Me.ButtonEdit.Enabled = True
                'Else
                '    Me.ButtonEdit.Enabled = True
            End If
            Me.TextBoxAnnuityBasis.ReadOnly = True
            Me.TextBoxShortDescription.ReadOnly = False
            Me.TextBoxLongDescription.ReadOnly = False
            Me.TextBoxEffectiveDate.Enabled = True
            ''Me.PopCalendarTermDate.Enabled = True
            ''Me.PopCalendarEffectiveDate.Enabled = True
            Me.TextBoxTermDate.Enabled = True
            Me.TextBoxAnnuityBasisPercentage.ReadOnly = False
            Me.ButtonSave.Enabled = True
            Me.ButtonCancel.Enabled = True
            Me.TextBoxFind.ReadOnly = True
            Me.ButtonSearch.Enabled = False
            Me.ButtonAdd.Enabled = False
            ''Me.ButtonEdit.Enabled = False
            Me.ButtonOk.Enabled = False

            g_bool_flag = False
            Session("BoolAddEditFlag") = g_bool_flag

            g_bool_EditFlag = True
            Session("BoolEditFlag") = g_bool_EditFlag


        Catch ex As Exception
            Throw ex
        End Try


    End Sub

    Private Sub ButtonAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAdd.Click
        Try

            If g_bool_SearchFlag = True Then
                SearchAndPopulateData()
            Else
                PopulateData()
            End If

            g_bool_flag = True
            Session("BoolAddEditFlag") = g_bool_flag

            g_bool_SearchFlag = False
            Session("BoolSearchFlag") = g_bool_SearchFlag

            'Making Readonly False for all TextBoxes
            Me.TextBoxAnnuityBasis.ReadOnly = False
            Me.TextBoxShortDescription.ReadOnly = False
            Me.TextBoxLongDescription.ReadOnly = False
            Me.TextBoxEffectiveDate.Enabled = True
            ''Me.PopCalendarEffectiveDate.Enabled = True
            ''Me.PopCalendarTermDate.Enabled = True
            Me.TextBoxTermDate.Enabled = True
            Me.TextBoxAnnuityBasisPercentage.ReadOnly = False
            Me.ButtonSave.Enabled = True
            Me.ButtonCancel.Enabled = True
            Me.TextBoxFind.ReadOnly = True
            Me.ButtonSearch.Enabled = False
            ''Me.ButtonEdit.Enabled = False
            Me.ButtonOk.Enabled = False
            Me.ButtonAdd.Enabled = False


            ' Making TextBoxes Blank
            Me.TextBoxFind.Text = String.Empty
            Me.TextBoxAnnuityBasis.Text = String.Empty
            Me.TextBoxShortDescription.Text = String.Empty
            Me.TextBoxLongDescription.Text = String.Empty
            Me.TextBoxEffectiveDate.Text = String.Empty
            Me.TextBoxTermDate.Text = String.Empty
            Me.TextBoxAnnuityBasisPercentage.Text = String.Empty

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub ButtonSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonSave.Click
        Dim InsertRow As DataRow
        Dim l_DataRow As DataRow

        Try
            'Added by neeraj on 16-Nov-2009 : issue is YRS 5.0-940
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
            'g_dataset_dsAnnuityBasisTypes = New DataSet
            g_dataset_dsAnnuityBasisTypes.Locale = CultureInfo.InvariantCulture
            'If add Flag Is true then do Insertion else update
            g_bool_flag = Session("BoolAddEditFlag")

            If g_bool_flag = True Then

                If Not g_dataset_dsAnnuityBasisTypes Is Nothing Then

                    InsertRow = g_dataset_dsAnnuityBasisTypes.Tables(0).NewRow

                    ' Assign the values.
                    InsertRow.Item("Annuity Basis Type") = TextBoxAnnuityBasis.Text.Trim
                    If TextBoxShortDescription.Text.Trim.Length = 0 Then
                        InsertRow.Item("Short[Desc]") = ""
                    Else
                        InsertRow.Item("Short[Desc]") = TextBoxShortDescription.Text.Trim
                    End If

                    If TextBoxLongDescription.Text.Trim.Length = 0 Then
                        InsertRow.Item("Long [Desc]") = ""
                    Else
                        InsertRow.Item("Long [Desc]") = TextBoxLongDescription.Text.Trim
                    End If

                    'If TextBoxEffectiveDate.Text.Trim = "" Then
                    '    TextBoxEffectiveDate.Text = "NULL"
                    'End If
                    If TextBoxEffectiveDate.Text.Trim.Length = 0 Then
                        InsertRow.Item("Eff Date") = "01/01/1900"
                    Else
                        InsertRow.Item("Eff Date") = TextBoxEffectiveDate.Text.Trim
                    End If
                    If TextBoxTermDate.Text.Trim.Length = 0 Then
                        InsertRow.Item("Term Date") = "01/01/1900"
                    Else
                        InsertRow.Item("Term Date") = TextBoxTermDate.Text.Trim
                    End If
                    If TextBoxAnnuityBasisPercentage.Text.Trim.Length = 0 Then
                        InsertRow.Item("Annuity BasisPct") = 0
                    Else
                        InsertRow.Item("Annuity BasisPct") = TextBoxAnnuityBasisPercentage.Text.Trim
                    End If


                    ' Insert the row into Table.
                    g_dataset_dsAnnuityBasisTypes.Tables(0).Rows.Add(InsertRow)
                    g_integer_count = 0
                    Me.PopulateDataIntoControls(g_integer_count)
                End If
            Else

                If Not g_dataset_dsAnnuityBasisTypes Is Nothing Then

                    g_integer_count = Session("dataset_index")
                    l_DataRow = g_dataset_dsAnnuityBasisTypes.Tables(0).Rows(g_integer_count)

                    If Not l_DataRow Is Nothing Then

                        'Update the values in current(selected) DataRow
                        l_DataRow("Annuity Basis Type") = TextBoxAnnuityBasis.Text.Trim
                        l_DataRow("Short[Desc]") = TextBoxShortDescription.Text.Trim
                        l_DataRow("Long [Desc]") = TextBoxLongDescription.Text.Trim

                        If Not Me.TextBoxTermDate.Text.Trim.Length = 0 Then
                            l_DataRow("Eff Date") = CType(TextBoxEffectiveDate.Text.Trim, Date)
                        End If


                        If Not Me.TextBoxTermDate.Text.Trim.Length = 0 Then
                            l_DataRow("Term Date") = CType(TextBoxTermDate.Text.Trim, Date)
                        End If


                        If Me.TextBoxAnnuityBasisPercentage.Text.Trim.Length = 0 Then
                            l_DataRow("Annuity BasisPct") = 0.0
                        Else
                            l_DataRow("Annuity BasisPct") = CType(TextBoxAnnuityBasisPercentage.Text.Trim, Decimal)
                        End If

                        g_integer_count = Session("dataset_index")
                        If (g_integer_count <> -1) Then
                            Me.PopulateDataIntoControls(g_integer_count)
                        End If

                    End If

                End If
            End If
            ' Call business layer to Save the DataSet/
            YMCARET.YmcaBusinessObject.AnnuityBasisTypesBOClass.InsertAnnuityBasisTypes(g_dataset_dsAnnuityBasisTypes)

            PopulateData()

            Me.ButtonSave.Enabled = False
            Me.ButtonCancel.Enabled = False
            Me.ButtonAdd.Enabled = True
            ''Me.ButtonEdit.Enabled = False
            Me.TextBoxFind.ReadOnly = False
            Me.ButtonSearch.Enabled = True
            Me.ButtonOk.Enabled = True
            Me.TextBoxAnnuityBasis.ReadOnly = True
            Me.TextBoxShortDescription.ReadOnly = True
            Me.TextBoxLongDescription.ReadOnly = True
            Me.TextBoxEffectiveDate.Enabled = False
            ''Me.PopCalendarEffectiveDate.Enabled = False
            ''Me.PopCalendarTermDate.Enabled = False
            Me.TextBoxTermDate.Enabled = False
            Me.TextBoxAnnuityBasisPercentage.ReadOnly = True
            Me.DataGridAnnuityBasis.SelectedIndex = -1

            g_bool_flag = False
            Session("BoolAddEditFlag") = g_bool_flag

            g_bool_SearchFlag = False
            Session("BoolSearchFlag") = g_bool_SearchFlag
            ' Me.LabelMandatory1.Visible = False
            ' Me.LabelMandatory2.Visible = False
            'Me.LabelMandatory3.Visible = False


            ' if any exception is there then form 'ErrorPageForm.aspx' is redirected
        Catch secEx As System.Security.SecurityException
            Response.Redirect("ErrorPageForm.aspx")
        Catch sqlEx As SqlException
            If sqlEx.Number = 60010 Then
                MessageBox.Show(PlaceHolder2, "Please Confirm", "Duplicate Record", MessageBoxButtons.OK)
            Else
                Response.Redirect("ErrorPageForm.aspx")
            End If

        End Try

    End Sub

    ''Private Sub ButtonEdit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonEdit.Click
    ''    If g_bool_SearchFlag = True Then
    ''        SearchAndPopulateData()
    ''    Else
    ''        PopulateData()
    ''    End If
    ''    ' Enable / Disable the controls.

    ''    Me.TextBoxAnnuityBasis.ReadOnly = True
    ''    Me.TextBoxShortDescription.ReadOnly = False
    ''    Me.TextBoxLongDescription.ReadOnly = False
    ''    Me.TextBoxEffectiveDate.Enabled = True
    ''    Me.PopCalendarTermDate.Enabled = True
    ''    Me.PopCalendarEffectiveDate.Enabled = True
    ''    Me.TextBoxTermDate.Enabled = True
    ''    Me.TextBoxAnnuityBasisPercentage.ReadOnly = False
    ''    Me.ButtonSave.Enabled = True
    ''    Me.ButtonCancel.Enabled = True
    ''    Me.TextBoxFind.ReadOnly = True
    ''    Me.ButtonSearch.Enabled = False
    ''    Me.ButtonAdd.Enabled = False
    ''    Me.ButtonEdit.Enabled = False
    ''    Me.ButtonOk.Enabled = False

    ''    g_bool_flag = False
    ''    Session("BoolAddEditFlag") = g_bool_flag

    ''    g_bool_EditFlag = True
    ''    Session("BoolEditFlag") = g_bool_EditFlag

    ''End Sub

    Private Sub DataGridAnnuityBasis_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DataGridAnnuityBasis.ItemCreated
        'If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.SelectedItem Then
        '    e.Item.Attributes.Add("onmouseover", "this.style.backgroundColor='beige';this.style.cursor='hand'")
        '    e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor='white';")
        '    e.Item.Attributes.Add("onclick", "javascript:__doPostBack('" & "DataGrid1:" & "ctrl" & e.Item.ItemIndex & ":ctrl0','')")
        'End If
    End Sub

    Private Sub DataGridAnnuityBasis_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DataGridAnnuityBasis.ItemDataBound
        Dim l_button_Select As ImageButton
        l_button_Select = e.Item.FindControl("ImageButtonSelect")
        If (e.Item.ItemIndex = Me.DataGridAnnuityBasis.SelectedIndex And Me.DataGridAnnuityBasis.SelectedIndex >= 0) Then
            l_button_Select.ImageUrl = "images\selected.gif"
        End If
        e.Item.Cells(3).Visible = False
        e.Item.Cells(4).Visible = False
        e.Item.Cells(5).Visible = False
        e.Item.Cells(6).Visible = False
    End Sub
    Private Sub DataGridAnnuityBasis_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles DataGridAnnuityBasis.PageIndexChanged
        Try
            DataGridAnnuityBasis.CurrentPageIndex = e.NewPageIndex

            'Bind the DataGrid again with the Data Source
            'depending on wheather Search Flag is true or False

            If g_bool_SearchFlag = True Then
                SearchAndPopulateData()
            Else
                PopulateData()
            End If
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Private Sub ButtonCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonCancel.Click
        Try
            ' Load the data to DataGrid.
            PopulateData()

            'Enable / Disable the controls
            Me.ButtonSave.Enabled = False
            Me.ButtonCancel.Enabled = False
            Me.ButtonAdd.Enabled = True
            ''Me.ButtonEdit.Enabled = False
            Me.TextBoxFind.ReadOnly = False
            Me.ButtonSearch.Enabled = True
            Me.ButtonOk.Enabled = True

            Me.TextBoxAnnuityBasis.ReadOnly = True
            Me.TextBoxShortDescription.ReadOnly = True
            Me.TextBoxLongDescription.ReadOnly = True
            Me.TextBoxEffectiveDate.Enabled = False
            Me.TextBoxTermDate.Enabled = False
            Me.TextBoxAnnuityBasisPercentage.ReadOnly = True
            ''Me.PopCalendarEffectiveDate.Enabled = False
            ''Me.PopCalendarTermDate.Enabled = False
            Me.DataGridAnnuityBasis.SelectedIndex = -1
            ' Clear the controls.
            g_integer_count = Session("dataset_index")
            If (g_integer_count <> -1) Then
                Me.PopulateDataIntoControls(g_integer_count)
            End If



            g_bool_flag = False
            Session("BoolAddEditFlag") = g_bool_flag

            g_bool_SearchFlag = False
            Session("BoolSearchFlag") = g_bool_SearchFlag
        Catch ex As Exception
            Throw ex
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
            Me.TextBoxAnnuityBasis.Text = String.Empty
            Me.TextBoxShortDescription.Text = String.Empty
            Me.TextBoxLongDescription.Text = String.Empty
            Me.TextBoxEffectiveDate.Text = String.Empty
            Me.TextBoxTermDate.Text = String.Empty
            Me.TextBoxAnnuityBasisPercentage.Text = String.Empty

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
    '** -----------------------------------------------------------------------------
    '** <summary>
    '**     Populating dataset for the search against 'Annuisty Basis'.
    '**     by calling function SearchAnnuityBasisTypes of business layer
    '**     Storing the dataset in the Cache of Cachefactory
    '** </summary>
    '** <remarks>
    '** </remarks>
    '** <history>
    '** 	[32365]	7/6/2005	Created
    '** </history>
    '** -----------------------------------------------------------------------------
    Public Sub SearchAndPopulateData()

        'Vipul 03Feb06 Cache-Session
        'Dim Cache As CacheManager

        Try
            'g_dataset_dsAnnuityBasisTypes = New DataSet

            g_dataset_dsAnnuityBasisTypes.Locale = CultureInfo.InvariantCulture
            'Vipul 03Feb06 Cache-Session
            'Cache = CacheFactory.GetCacheManager()
            'g_dataset_dsAnnuityBasisTypes = Cache.GetData("AnnuityBasisTypes")
            g_dataset_dsAnnuityBasisTypes = Session("AnnuityBasisTypes")
            'Vipul 03Feb06 Cache-Session

            ' Call business layer to load the Datagrid.
            g_dataset_dsAnnuityBasisTypes = YMCARET.YmcaBusinessObject.AnnuityBasisTypesBOClass.SearchAnnuityBasisTypes(Me.TextBoxFind.Text)
            viewstate("Dataset_AnnBasis") = g_dataset_dsAnnuityBasisTypes

            If g_dataset_dsAnnuityBasisTypes.Tables.Count = 0 Then
                Me.LabelNoRecordFound.Visible = True
            Else
                Me.DataGridAnnuityBasis.CurrentPageIndex = 0
                Me.DataGridAnnuityBasis.DataSource = g_dataset_dsAnnuityBasisTypes
                Me.DataGridAnnuityBasis.DataBind()

            End If
            ''CommonModule.HideColumnsinDataGrid(g_dataset_dsAnnuityBasisTypes, Me.DataGridAnnuityBasis, "Long [Desc], Eff Date, Term Date, Annuity BasisPct")

        Catch secEx As System.Security.SecurityException
            Response.Redirect("ErrorPageForm.aspx")
        Catch sqlEx As SqlException
            If sqlEx.Number = 60006 Then
                Me.LabelNoRecordFound.Visible = True

                'Vipul 03Feb06 Cache-Session
                'g_dataset_dsAnnuityBasisTypes = Cache.GetData("AnnuityBasisTypes")
                g_dataset_dsAnnuityBasisTypes = Session("AnnuityBasisTypes")
                'Vipul 03Feb06 Cache-Session

                g_dataset_dsAnnuityBasisTypes.Tables(0).Clear()
                Me.DataGridAnnuityBasis.DataSource = g_dataset_dsAnnuityBasisTypes
                Me.DataGridAnnuityBasis.DataBind()
                Me.DataGridAnnuityBasis.Visible = False


            Else
                Response.Redirect("ErrorPageForm.aspx")
            End If
        End Try
    End Sub
    '** -----------------------------------------------------------------------------
    '** <summary>
    '**     Populating dataset containing all rows of table 'AtsMetaAnnuityBasisTypes' 
    '**     by calling function LookUpAnnuityBasisTypes of business layer
    '**     Storing the dataset in the Cache of Cachefactory
    '** </summary>
    '** <remarks>
    '** </remarks>
    '** <history>
    '** 	[32365]	7/6/2005	Created
    '** </history>
    '** -----------------------------------------------------------------------------
    Public Sub PopulateData()

        'Vipul 03Feb06 Cache-Session
        'Dim Cache As CacheManager

        Try

            g_dataset_dsAnnuityBasisTypes.Locale = CultureInfo.InvariantCulture

            ' Call business layer to Load the DataSet.
            'g_dataset_dsAnnuityBasisTypes = YMCARET.YmcaBusinessObject.AnnuityBasisTypesBOClass.LookupAnnuityBasisTypes()

            YMCARET.YmcaBusinessObject.CommonLookUpTablesBOClass.LookupTables()
            g_dataset_dsAnnuityBasisTypes = AppDomain.CurrentDomain.GetData("DataSetAnnuityBasisTypes")

            'Vipul 03Feb06 Cache-Session
            'Cache = CacheFactory.GetCacheManager()
            'Cache.Add("AnnuityBasisTypes", g_dataset_dsAnnuityBasisTypes)
            Session("AnnuityBasisTypes") = g_dataset_dsAnnuityBasisTypes
            'Vipul 03Feb06 Cache-Session

            viewstate("Dataset_AnnBasis") = g_dataset_dsAnnuityBasisTypes
            If g_dataset_dsAnnuityBasisTypes.Tables.Count = 0 Then
                ''Me.ButtonEdit.Enabled = False
                Me.TextBoxFind.ReadOnly = True
                Me.ButtonSearch.Enabled = False
                Me.LabelNoRecordFound.Visible = True
            Else
                Me.DataGridAnnuityBasis.DataSource = g_dataset_dsAnnuityBasisTypes.Tables(0)
                Me.DataGridAnnuityBasis.DataBind()
            End If
            ''CommonModule.HideColumnsinDataGrid(g_dataset_dsAnnuityBasisTypes, Me.DataGridAnnuityBasis, "Long [Desc], Eff Date, Term Date, Annuity BasisPct")
        Catch sqlEx As SqlException
            If sqlEx.Number = 60006 Then
                Me.LabelNoRecordFound.Visible = True
                'Vipul 03Feb06 Cache-Session
                'g_dataset_dsAnnuityBasisTypes = Cache.GetData("AnnuityBasisTypes")
                g_dataset_dsAnnuityBasisTypes = Session("AnnuityBasisTypes")
                'Vipul 03Feb06 Cache-Session

                g_dataset_dsAnnuityBasisTypes.Tables(0).Clear()
                Me.DataGridAnnuityBasis.DataSource = g_dataset_dsAnnuityBasisTypes
                Me.DataGridAnnuityBasis.DataBind()
            Else
                Response.Redirect("ErrorPageForm.aspx")
            End If

        Catch secEx As System.Security.SecurityException
            Response.Redirect("ErrorPageForm.aspx")
        End Try
    End Sub

    Private Sub ButtonOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonOk.Click
        Session("AnnBasisSort") = Nothing
        Response.Redirect("MainWebForm.aspx", False)
    End Sub



    Private Function PopulateDataIntoControls(ByVal index As Integer) As Boolean

        Dim l_DataSet As DataSet
        Dim l_DataTable As DataTable
        Dim l_DataRow As DataRow
        'Dim l_Cache As CacheManager

        Try

            ' l_Cache = CacheFactory.GetCacheManager()
            l_DataSet = g_dataset_dsAnnuityBasisTypes '(l_Cache.GetData("Annuity Basis Types"), DataSet)

            If Not l_DataSet Is Nothing Then

                l_DataTable = l_DataSet.Tables("Annuity Basis Types")

                If Not l_DataTable Is Nothing Then

                    If index < l_DataTable.Rows.Count Then

                        l_DataRow = l_DataTable.Rows.Item(index)

                        If (Not l_DataRow Is Nothing) Then

                            Me.TextBoxAnnuityBasis.Text = CType(l_DataRow("Annuity Basis Type"), String).Trim
                            If l_DataRow("Short[Desc]").GetType.ToString = "System.DBNull" Then
                                Me.TextBoxShortDescription.Text = String.Empty
                            Else
                                Me.TextBoxShortDescription.Text = CType(l_DataRow("Short[Desc]"), String).Trim
                            End If

                            If l_DataRow("Long [Desc]").GetType.ToString = "System.DBNull" Then
                                Me.TextBoxLongDescription.Text = String.Empty
                            Else
                                Me.TextBoxLongDescription.Text = CType(l_DataRow("Long [Desc]"), String).Trim
                            End If

                            If l_DataRow("Eff Date").GetType.ToString = "System.DBNull" Then
                                Me.TextBoxEffectiveDate.Text = String.Empty
                            Else
                                Me.TextBoxEffectiveDate.Text = CType(l_DataRow("Eff Date"), String).Trim
                            End If

                            If l_DataRow("Term Date").GetType.ToString = "System.DBNull" Then
                                Me.TextBoxTermDate.Text = String.Empty
                            Else
                                Me.TextBoxTermDate.Text = CType(l_DataRow("Term Date"), String).Trim

                            End If

                            If l_DataRow("Annuity BasisPct").GetType.ToString = "System.DBNull" Then
                                Me.TextBoxAnnuityBasisPercentage.Text = 0
                            Else
                                Me.TextBoxAnnuityBasisPercentage.Text = CType(l_DataRow("Annuity BasisPct"), String).Trim
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

    Private Sub Menu2_MenuItemClick(ByVal sender As System.Object, ByVal e As skmMenu.MenuItemClickEventArgs) Handles Menu2.MenuItemClick

    End Sub

    Private Sub DataGridAnnuityBasis_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles DataGridAnnuityBasis.SortCommand
        Try
            Me.DataGridAnnuityBasis.SelectedIndex = -1
            If Not viewstate("Dataset_AnnBasis") Is Nothing Then
                Dim dv As New DataView
                Dim SortExpression As String
                SortExpression = e.SortExpression
                g_dataset_dsAnnuityBasisTypes = viewstate("Dataset_AnnBasis")
                dv = g_dataset_dsAnnuityBasisTypes.Tables(0).DefaultView
                dv.Sort = SortExpression
                If Not Session("AnnBasisSort") Is Nothing Then
                    If Session("AnnBasisSort").ToString.Trim.EndsWith("ASC") Then
                        dv.Sort = SortExpression + " DESC"
                    Else
                        dv.Sort = SortExpression + " ASC"
                    End If
                Else
                    dv.Sort = SortExpression + " ASC"
                End If
                Me.DataGridAnnuityBasis.DataSource = Nothing
                Me.DataGridAnnuityBasis.DataSource = dv
                Me.DataGridAnnuityBasis.DataBind()
                Session("AnnBasisSort") = dv.Sort
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
            Dim tooltip = YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.TOOLTIP_READONLY).DisplayText
            DataGridAnnuityBasis.Enabled = False
            DataGridAnnuityBasis.ToolTip = tooltip
            ButtonAdd.Enabled = False
            ButtonAdd.ToolTip = tooltip
        End If
    End Sub
    'END : PK | 02-28-2019 | YRS-AT-4248 | Check ReadOnly Flag, if true then disable the buttons on which security is not implemented and add Tool tip.
End Class

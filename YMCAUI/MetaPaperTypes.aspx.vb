
'*******************************************************************************
' Copyright YMCA Retirement Fund All Rights Reserved. 
'
' Project Name		:	YMCA-YRS
' FileName			:	MetaPaperType.aspx.vb
' Author Name		:	Imran
' Employee ID		:	51494
' Email				:	imran.bedrekar@3i-infotech.com
' Contact No		:	
' Creation Time		:	17/08/2010 
' Program Specification Name	:	YMCA Enhancement 8
' Unit Test Plan Name			:	
' Description					:	<<Please put the brief description here...>>
'
' Changed by			:	
' Changed on			:	
' Change Description	:	
' Cache to Session       :  
'*******************************************************************************
'Modification History 
'*******************************************************************************
'Modified By                    Date                Description
'*******************************************************************************
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
Public Class MetaPaperTypes
    Inherits System.Web.UI.Page
    Dim strFormName As String = "MetaPaperTypes.aspx" 'New String("MetaPrinters.aspx")
    'End issue id YRS 5.0-940
    Dim g_dataset_dsprinters As New DataSet
    Dim g_bool_SearchFlag As New Boolean
    Dim g_bool_EditFlag As New Boolean
    Dim g_bool_AddFlag As New Boolean
    Dim g_integer_count As New Integer
    Dim g_bool_DeleteFlag As New Boolean
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        'Me.DataGridMetaPaperType.DataSource = CommonModule.CreateDataSource
        'Me.DataGridMetaPaperType.DataBind()
        Me.LabelPaperTypeName.AssociatedControlID = Me.TextBoxPaperType.ID
        Me.LabelDescription.AssociatedControlID = Me.TextBoxDescription.ID
        Me.LabelLook.AssociatedControlID = Me.TextBoxFind.ID
        Try

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
                If g_bool_SearchFlag = True Then
                    SearchAndPopulateData()
                Else
                    PopulateData()
                End If

                Me.TextBoxFind.ReadOnly = False
                Me.ButtonSearch.Enabled = True

                Me.TextBoxPaperType.ReadOnly = True
                Me.TextBoxDescription.ReadOnly = True



            End If

        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
        End Try
    End Sub

    Public Sub DeleteSub()
        Dim paraPaperTypeid(1) As String
        Try
            'If g_bool_SearchFlag = True Then
            '    SearchAndPopulateData()
            'Else
            '    PopulateData()
            'End If
            g_dataset_dsprinters = ViewState("Dataset_PaperType")
            g_bool_DeleteFlag = True
            Session("BoolDeleteFlag") = g_bool_DeleteFlag

            Dim l_DataRow As DataRow
            If Not g_dataset_dsprinters Is Nothing Then

                g_integer_count = Session("dataset_index")
                l_DataRow = g_dataset_dsprinters.Tables(0).Rows(g_integer_count)
                paraPaperTypeid(0) = l_DataRow("PaperTypeId")
                l_DataRow.Delete()
            End If
            YMCARET.YmcaBusinessObject.MetaShellPaperTypesBOClass.InsertPaperType(g_dataset_dsprinters)
            PopulateData()

            ' Making TextBoxes Blank
            Me.TextBoxPaperType.Text = String.Empty
            Me.TextBoxDescription.Text = String.Empty
            If DataGridMetaPaperType.Items.Count > 0 Then
                Me.DataGridMetaPaperType.SelectedIndex = 0
            End If



            Me.ButtonDelete.Enabled = False
            Me.ButtonSave.Enabled = False
            Me.ButtonCancel.Enabled = False
            Me.ButtonAdd.Enabled = True
            Me.ButtonOK.Enabled = True
            ''Me.ButtonEditForm.Enabled = False
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Public Sub SearchAndPopulateData()
        'Dim Cache As CacheManager
        Try
            'Cache = CacheFactory.GetCacheManager()
            'g_dataset_dsProjectedInterestRate = Cache.GetData("Projected Interest Rates")
            'g_dataset_dsprinters = YMCARET.YmcaBusinessObject.MetaShellPaperTypesBOClass.Search(Me.TextBoxFind.Text)
            ViewState("Dataset_PaperType") = g_dataset_dsprinters
            If g_dataset_dsprinters.Tables.Count = 0 Then
                Me.LabelNoRecordFound.Visible = True

            Else
                Me.DataGridMetaPaperType.CurrentPageIndex = 0
                Me.DataGridMetaPaperType.DataSource = g_dataset_dsprinters
                Me.DataGridMetaPaperType.DataBind()

            End If

            ''CommonModule.HideColumnsinDataGrid(g_dataset_dsprinters, Me.DataGridMetaPaperType, "Code Value, Active,Editable,Eff Date")
        Catch secEx As System.Security.SecurityException
            Response.Redirect("ErrorPageForm.aspx")
        Catch sqlEx As SqlException
            If sqlEx.Number = 60006 Then
                ' PopulateData()
                Me.LabelNoRecordFound.Visible = True
                '/b Cache to Session       :   Ragesh 34231 02/02/06 Cache to Session
                'g_dataset_dsprinters = Cache.GetData("Country Types")
                g_dataset_dsprinters = CType(Session("g_dataset_dsprinters"), DataSet)
                '/e Cache to Session       :   Ragesh 34231 02/02/06 Cache to Session
                g_dataset_dsprinters.Tables(0).Clear()
                Me.DataGridMetaPaperType.DataSource = g_dataset_dsprinters
                Me.DataGridMetaPaperType.DataBind()
            Else
                Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(sqlEx.Message.Trim.ToString()), False)
            End If
        End Try



    End Sub


    Public Sub PopulateData()
        'Dim Cache As CacheManager
        Try

            g_dataset_dsprinters = YMCARET.YmcaBusinessObject.MetaShellPaperTypesBOClass.LookupPaperTypes()
            ViewState("Dataset_PaperType") = g_dataset_dsprinters
            '/b Cache to Session       :   Ragesh 34231 02/02/06 Cache to Session
            'Cache = CacheFactory.GetCacheManager()
            'Cache.Add("Country Types", g_dataset_dsprinters)
            Session("g_dataset_dsprinters") = g_dataset_dsprinters
            '/e Cache to Session       :   Ragesh 34231 02/02/06 Cache to Session

            RequiredFieldValidator1.Enabled = True
            If g_dataset_dsprinters.Tables.Count = 0 Then
                ''Me.ButtonEditForm.Enabled = False
                Me.TextBoxFind.ReadOnly = True
                Me.ButtonSearch.Enabled = False
                Me.ButtonDelete.Enabled = False
                Me.LabelNoRecordFound.Visible = True
                Me.DataGridMetaPaperType.DataBind()
            Else
                If g_dataset_dsprinters.Tables(0).Rows.Count > 0 Then
                    Me.DataGridMetaPaperType.DataSource = g_dataset_dsprinters.Tables(0)
                    Me.DataGridMetaPaperType.DataBind()
                    LabelNoRecordFound.Visible = False
                Else
                    LabelNoRecordFound.Visible = True
                    Me.DataGridMetaPaperType.DataBind()
                    RequiredFieldValidator1.Enabled = False
                End If
                

            End If

            ''CommonModule.HideColumnsinDataGrid(g_dataset_dsprinters, Me.DataGridMetaPaperType, "Code Value, Active,Editable,Eff Date")

            'Me.DataGridMetaPaperType.AllowSorting = True

        Catch sqlEx As SqlException
            If sqlEx.Number = 60006 Then
                Me.LabelNoRecordFound.Visible = True

                '/b Cache to Session       :   Ragesh 34231 02/02/06 Cache to Session
                'g_dataset_dsprinters = Cache.GetData("Country Types")
                'g_dataset_dsprinters = CType(Session("g_dataset_dsprinters"), DataSet)
                '/e Cache to Session       :   Ragesh 34231 02/02/06 Cache to Session

                'g_dataset_dsprinters.Tables(0).Clear()
                'Me.DataGridMetaPaperType.DataSource = g_dataset_dsprinters
                'Me.DataGridMetaPaperType.DataBind()
            Else
                Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(sqlEx.Message.Trim.ToString()), False)
            End If

        Catch secEx As System.Security.SecurityException
            Response.Redirect("ErrorPageForm.aspx")
        End Try
    End Sub

    Private Function PopulateDataIntoControls(ByVal index As Integer) As Boolean

        Dim l_DataSet As DataSet
        Dim l_DataTable As DataTable
        Dim l_DataRow As DataRow
        Dim dr As DataRow()
        Dim l_CheckBox As CheckBox
        Try

            l_DataSet = g_dataset_dsprinters
            trprinter.Visible = False
            trReport.Visible = False
            If Not l_DataSet Is Nothing Then

                l_DataTable = l_DataSet.Tables("PaperTypes")

                If Not l_DataTable Is Nothing Then

                    If index < l_DataTable.Rows.Count Then

                        l_DataRow = l_DataTable.Rows.Item(index)

                        If (Not l_DataRow Is Nothing) Then

                            Me.TextBoxPaperType.Text = CType(l_DataRow("PaperType"), String).Trim
                            If l_DataRow("PaperType").GetType.ToString = "System.DBNull" Then
                                Me.TextBoxPaperType.Text = String.Empty
                            Else
                                Me.TextBoxPaperType.Text = CType(l_DataRow("PaperType"), String).Trim
                            End If

                            If l_DataRow("Description").GetType.ToString = "System.DBNull" Then
                                Me.TextBoxDescription.Text = String.Empty
                            Else
                                Me.TextBoxDescription.Text = CType(l_DataRow("Description"), String).Trim
                            End If
                            If Not l_DataSet.Tables("Printers") Is Nothing Then

                                Dim dtvPrinter As New DataView(l_DataSet.Tables("Printers"))

                                dtvPrinter.RowFilter = "[PaperTypeid]= " & l_DataRow("PaperTypeId") & ""
                                If dtvPrinter.Count > 0 Then
                                    trprinter.Visible = True
                                    dgPrinter.DataSource = dtvPrinter
                                    dgPrinter.DataBind()
                                    trprinter.Visible = True
                                Else
                                    trprinter.Visible = False
                                End If
                                
                                'dr = l_DataSet.Tables("Printers").Select("[PaperTypeid]= " & l_DataRow("PaperTypeId") & "")
                                'dgPrinter.DataSource = dr
                                'dgPrinter.DataBind()
                            End If

                            
                            If Not l_DataSet.Tables("Reports") Is Nothing Then
                                'dr = l_DataSet.Tables("Reports").Select("[PaperTypeid]= " & l_DataRow("PaperTypeId") & "")
                                'dgReport.DataSource = dr
                                'dgReport.DataBind()

                                Dim dtvReport As New DataView(l_DataSet.Tables("Reports"))

                                dtvReport.RowFilter = "[PaperTypeid]= " & l_DataRow("PaperTypeId") & ""

                                If dtvReport.Count > 0 Then
                                    trReport.Visible = True
                                    dgReport.DataSource = dtvReport
                                    dgReport.DataBind()
                                    trReport.Visible = True
                                Else
                                    trReport.Visible = False
                                End If
                               
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
    Private Sub DataGridMetaPaperType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DataGridMetaPaperType.SelectedIndexChanged
        Try
            If g_bool_SearchFlag = True Then
                SearchAndPopulateData()
            Else
                PopulateData()
            End If

            ''Me.ButtonEditForm.Enabled = True
            g_integer_count = DataGridMetaPaperType.SelectedIndex ' (((DataGridMetaPaperType.CurrentPageIndex) * DataGridMetaPaperType.PageSize) + DataGridMetaPaperType.SelectedIndex)


            Session("dataset_index") = g_integer_count

            Me.TextBoxPaperType.ReadOnly = False
            Me.TextBoxDescription.ReadOnly = False

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


            If Me.PopulateDataIntoControls(g_integer_count) = True Then
                ''Me.ButtonEditForm.Enabled = True
                Me.ButtonDelete.Enabled = True
            Else
                ''Me.ButtonEditForm.Enabled = True
                Me.ButtonDelete.Enabled = True
            End If
        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
        End Try
    End Sub

    Private Sub DataGridMetaPaperType_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles DataGridMetaPaperType.PageIndexChanged
        Try
            DataGridMetaPaperType.CurrentPageIndex = e.NewPageIndex

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

            g_integer_count = Session("dataset_index")
            If (g_integer_count <> -1) Then
                Me.PopulateDataIntoControls(g_integer_count)
            End If

            ' Do search & populate the data.
            SearchAndPopulateData()
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
            trprinter.Visible = False
            trReport.Visible = False
            g_bool_AddFlag = True
            Session("BoolAddFlag") = g_bool_AddFlag

            g_bool_SearchFlag = False
            Session("BoolSearchFlag") = g_bool_SearchFlag

            'Making Readonly False for all TextBoxes
            Me.TextBoxPaperType.ReadOnly = False
            Me.TextBoxDescription.ReadOnly = False
            RequiredFieldValidator1.Enabled = True



            ' Making TextBoxes Blank
            Me.TextBoxPaperType.Text = String.Empty
            Me.TextBoxDescription.Text = String.Empty

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
            Me.TextBoxPaperType.ReadOnly = True
            Me.TextBoxDescription.ReadOnly = True
            Me.DataGridMetaPaperType.SelectedIndex = -1
            ''Me.PopCalendarEffDate.Enabled = False

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

    Private Sub ButtonSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonSave.Click
        Dim InsertRow As DataRow
        Dim l_DataRow As DataRow
        Dim l_CheckBox As CheckBox
        Dim parameterListPaperTypeId As Array

        Dim cnt As Int32
        Dim l_string_Relids As String = ""


        Try

            'Added by neeraj on 18-Nov-2009 : issue is YRS 5.0-940
            Dim checkSecurity As String = SecurityCheck.Check_Authorization(strFormName, Convert.ToInt32(Session("LoggedUserKey")))

            If Not checkSecurity.Equals("True") Then
                MessageBox.Show(PlaceHolder1, "YMCA-YRS", checkSecurity, MessageBoxButtons.Stop)
                Exit Sub
            End If
            'End : YRS 5.0-940

            'If g_bool_SearchFlag = True Then
            '    SearchAndPopulateData()
            'Else
            '    PopulateData()
            'End If
            g_dataset_dsprinters = ViewState("Dataset_PaperType")
            'If add Flag Is true then do Insertion else update
            g_bool_AddFlag = Session("BoolAddFlag")

            If g_bool_AddFlag = True Then
                If Not g_dataset_dsprinters Is Nothing Then

                    InsertRow = g_dataset_dsprinters.Tables("PaperTypes").NewRow

                    ' Assign the values.
                    InsertRow.Item("PaperType") = TextBoxPaperType.Text.Trim
                    If Me.TextBoxPaperType.Text.Trim.Length = 0 Then
                        InsertRow.Item("PaperType") = String.Empty
                    Else
                        InsertRow.Item("PaperType") = TextBoxPaperType.Text.Trim
                    End If
                    ' Assign the values.
                    InsertRow.Item("Description") = TextBoxDescription.Text.Trim
                    If Me.TextBoxDescription.Text.Trim.Length = 0 Then
                        InsertRow.Item("Description") = String.Empty
                    Else
                        InsertRow.Item("Description") = TextBoxDescription.Text.Trim
                    End If
                    'InsertRow.Item("PaperTypeID") = DropdownPaperTypes.SelectedValue
                    InsertRow.Item("PaperTypeID") = 0
                    ' Insert the row into Table.
                    g_dataset_dsprinters.Tables(0).Rows.Add(InsertRow)

                    
                    Me.TextBoxPaperType.Text = ""
                    Me.TextBoxDescription.Text = ""
                    ' g_integer_count = 0
                    'Me.PopulateDataIntoControls(g_integer_count)
                End If

            Else

                If Not g_dataset_dsprinters Is Nothing Then

                    g_integer_count = Session("dataset_index")
                    l_DataRow = g_dataset_dsprinters.Tables(0).Rows(g_integer_count)

                    If Not l_DataRow Is Nothing Then

                        'Update the values in current(selected) DataRow
                        'if interest rate is empty then take it zero
                        l_DataRow("PaperType") = TextBoxPaperType.Text.Trim
                        If Me.TextBoxPaperType.Text.Trim.Length = 0 Then
                            l_DataRow("PaperType") = String.Empty
                        Else
                            l_DataRow("PaperType") = TextBoxPaperType.Text.Trim
                        End If

                        If Me.TextBoxDescription.Text.Trim.Length = 0 Then
                            l_DataRow("Description") = String.Empty
                        Else
                            l_DataRow("Description") = TextBoxDescription.Text.Trim
                        End If
                        ' l_DataRow("PaperTypeId") = l_DataRow("PaperTypeId")
                      
                    End If

                End If
            End If
           

            YMCARET.YmcaBusinessObject.MetaShellPaperTypesBOClass.InsertPaperType(g_dataset_dsprinters)
            PopulateData()

            Me.ButtonSave.Enabled = False
            Me.ButtonCancel.Enabled = False
            Me.ButtonAdd.Enabled = True
            ''Me.ButtonEditForm.Enabled = False
            Me.TextBoxFind.ReadOnly = False
            Me.ButtonSearch.Enabled = True
            Me.ButtonDelete.Enabled = False
            Me.ButtonOK.Enabled = True

          
            Me.TextBoxPaperType.ReadOnly = True
            Me.TextBoxDescription.ReadOnly = True
            




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
                Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(sqlEx.Message.Trim.ToString()), False)
            End If
        End Try

    End Sub


    Private Sub ButtonOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonOK.Click
        Session("CountrySort") = Nothing
        Response.Redirect("MainWebForm.aspx", False)
    End Sub

    Private Sub ButtonDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonDelete.Click
        'If g_bool_SearchFlag = True Then
        '    SearchAndPopulateData()
        'Else
        '    PopulateData()
        'End If
        Dim checkSecurity As String = SecurityCheck.Check_Authorization(strFormName, Convert.ToInt32(Session("LoggedUserKey")))
        If Not checkSecurity.Equals("True") Then
            MessageBox.Show(PlaceHolder1, "YMCA-YRS", checkSecurity, MessageBoxButtons.Stop)
            Exit Sub
        End If
        MessageBox.Show(PlaceHolder1, "Please Confirm", "Are you sure you want to delete this Paper Type?", MessageBoxButtons.YesNo)
    End Sub
    Private Sub DataGridMetaPaperType_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DataGridMetaPaperType.ItemDataBound
        Dim l_button_Select As ImageButton
        l_button_Select = e.Item.FindControl("ImageButtonSelect")
        If (e.Item.ItemIndex = Me.DataGridMetaPaperType.SelectedIndex And Me.DataGridMetaPaperType.SelectedIndex >= 0) Then
            l_button_Select.ImageUrl = "images\selected.gif"
        End If
        e.Item.Cells(1).Visible = False
        'e.Item.Cells(4).Visible = False
        'e.Item.Cells(5).Visible = False
        'e.Item.Cells(6).Visible = False
    End Sub

    Private Sub DataGridMetaPaperType_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles DataGridMetaPaperType.SortCommand
        Try
            Me.DataGridMetaPaperType.SelectedIndex = -1
            If Not ViewState("Dataset_PaperType") Is Nothing Then
                Dim dv As New DataView
                Dim SortExpression As String
                SortExpression = e.SortExpression
                g_dataset_dsprinters = ViewState("Dataset_PaperType")
                dv = g_dataset_dsprinters.Tables(0).DefaultView
                dv.Sort = SortExpression
                If Not Session("CountrySort") Is Nothing Then
                    If Session("CountrySort").ToString.Trim.EndsWith("ASC") Then
                        dv.Sort = SortExpression + " DESC"
                    Else
                        dv.Sort = SortExpression + " ASC"
                    End If
                Else
                    dv.Sort = SortExpression + " ASC"
                End If
                Me.DataGridMetaPaperType.DataSource = Nothing
                Me.DataGridMetaPaperType.DataSource = dv
                Me.DataGridMetaPaperType.DataBind()
                Session("CountrySort") = dv.Sort
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
            DataGridMetaPaperType.Enabled = False
            DataGridMetaPaperType.ToolTip = YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.TOOLTIP_READONLY).DisplayText
        End If
    End Sub
    'END : PK | 02-28-2019 | YRS-AT-4248 | Check ReadOnly Flag, if true then disable the buttons on which security is not implemented and add Tool tip.
End Class
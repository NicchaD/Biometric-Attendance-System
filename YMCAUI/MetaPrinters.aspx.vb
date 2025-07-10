'*******************************************************************************
' Copyright YMCA Retirement Fund All Rights Reserved. 
'
' Project Name		:	YMCA-YRS
' FileName			:	MetaPrinters.aspx.vb
' Author Name		:	Imran
' Employee ID		:	51494
' Email				:	imran.bedrekar@3i-infotech.com
' Contact No		:	8733
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
Public Class MetaPrinters
    Inherits System.Web.UI.Page
    Dim strFormName As String = "MetaPrinters.aspx"
    'End issue id YRS 5.0-940
    Dim g_dataset_dsprinters As New DataSet
    Dim g_bool_SearchFlag As New Boolean
    Dim g_bool_EditFlag As New Boolean
    Dim g_bool_AddFlag As New Boolean
    Dim g_integer_count As New Integer
    Dim g_bool_DeleteFlag As New Boolean
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        'Me.DataGridMetaCountry.DataSource = CommonModule.CreateDataSource
        'Me.DataGridMetaCountry.DataBind()
        Me.LabelPrinterName.AssociatedControlID = Me.TextBoxPrinterName.ID
        Me.LabelPrinterDecription.AssociatedControlID = Me.TextBoxPrinterDecription.ID
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

                Me.TextBoxPrinterName.ReadOnly = True
                Me.TextBoxPrinterDecription.ReadOnly = True



            End If

        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
        End Try
    End Sub

    Public Sub DeleteSub()
        Dim paraPrinterid(1) As String
        Try
            'If g_bool_SearchFlag = True Then
            '    SearchAndPopulateData()
            'Else
            '    PopulateData()
            'End If
            g_dataset_dsprinters = ViewState("Dataset_Printers")
            g_bool_DeleteFlag = True
            Session("BoolDeleteFlag") = g_bool_DeleteFlag

            Dim l_DataRow As DataRow
            If Not g_dataset_dsprinters Is Nothing Then

                g_integer_count = Session("dataset_index")
                l_DataRow = g_dataset_dsprinters.Tables(0).Rows(g_integer_count)
                paraPrinterid(0) = l_DataRow("IntUniqueid")
                l_DataRow.Delete()
            End If
            YMCARET.YmcaBusinessObject.MetaShellPrintersBOClass.InsertPrinter(g_dataset_dsprinters, paraPrinterid)
            PopulateData()

            ' Making TextBoxes Blank
            Me.TextBoxPrinterName.Text = String.Empty
            Me.TextBoxPrinterDecription.Text = String.Empty
            'If DataGridMetaCountry.Items.Count > 0 Then
            '    Me.DataGridMetaCountry.SelectedIndex = 0
            'End If



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
            g_dataset_dsprinters = YMCARET.YmcaBusinessObject.MetaShellPrintersBOClass.SearchPrinter(Me.TextBoxFind.Text)
            viewstate("Dataset_Printers") = g_dataset_dsprinters
            If g_dataset_dsprinters.Tables.Count = 0 Then
                Me.LabelNoRecordFound.Visible = True

            Else
                Me.DataGridMetaCountry.CurrentPageIndex = 0
                Me.DataGridMetaCountry.DataSource = g_dataset_dsprinters
                Me.DataGridMetaCountry.DataBind()

            End If

            ''CommonModule.HideColumnsinDataGrid(g_dataset_dsprinters, Me.DataGridMetaCountry, "Code Value, Active,Editable,Eff Date")
        Catch secEx As System.Security.SecurityException
            Response.Redirect("ErrorPageForm.aspx")
        Catch sqlEx As SqlException
            If sqlEx.Number = 60006 Then
                ' PopulateData()
                Me.LabelNoRecordFound.Visible = True
                '/b Cache to Session       :   Ragesh 34231 02/02/06 Cache to Session
                'g_dataset_dsprinters = Cache.GetData("Country Types")
                ' g_dataset_dsprinters = CType(Session("g_dataset_dsprinters"), DataSet)
                '/e Cache to Session       :   Ragesh 34231 02/02/06 Cache to Session
                'g_dataset_dsprinters.Tables(0).Clear()
                'Me.DataGridMetaCountry.DataSource = g_dataset_dsprinters
                'Me.DataGridMetaCountry.DataBind()
            Else
                Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(sqlEx.Message.Trim.ToString()), False)
            End If
        End Try



    End Sub


    Public Sub PopulateData()
        'Dim Cache As CacheManager
        Try

            g_dataset_dsprinters = YMCARET.YmcaBusinessObject.MetaShellPrintersBOClass.LookupPrinters()
            ViewState("Dataset_Printers") = g_dataset_dsprinters
            '/b Cache to Session       :   Ragesh 34231 02/02/06 Cache to Session
            'Cache = CacheFactory.GetCacheManager()
            'Cache.Add("Country Types", g_dataset_dsprinters)
            ' Session("g_dataset_dsprinters") = g_dataset_dsprinters
            '/e Cache to Session       :   Ragesh 34231 02/02/06 Cache to Session

            RequiredFieldValidator1.Enabled = True
            RequiredFieldValidator2.Enabled = True
            If g_dataset_dsprinters.Tables.Count = 0 Then
                ''Me.ButtonEditForm.Enabled = False
                Me.TextBoxFind.ReadOnly = True
                Me.ButtonSearch.Enabled = False
                Me.ButtonDelete.Enabled = False
                Me.LabelNoRecordFound.Visible = True
                Me.DataGridMetaCountry.DataBind()
            Else

                If g_dataset_dsprinters.Tables(0).Rows.Count > 0 Then
                    Me.DataGridMetaCountry.DataSource = g_dataset_dsprinters.Tables(0)
                    Me.DataGridMetaCountry.DataBind()
                    LabelNoRecordFound.Visible = False
                Else

                    LabelNoRecordFound.Visible = True
                    Me.DataGridMetaCountry.DataBind()
                    RequiredFieldValidator1.Enabled = False
                    RequiredFieldValidator2.Enabled = False
                End If

                

            End If

            ''CommonModule.HideColumnsinDataGrid(g_dataset_dsprinters, Me.DataGridMetaCountry, "Code Value, Active,Editable,Eff Date")

            'Me.DataGridMetaCountry.AllowSorting = True

        Catch sqlEx As SqlException
            If sqlEx.Number = 60006 Then
                Me.LabelNoRecordFound.Visible = True

                '/b Cache to Session       :   Ragesh 34231 02/02/06 Cache to Session
                'g_dataset_dsprinters = Cache.GetData("Country Types")
                'g_dataset_dsprinters = CType(Session("g_dataset_dsprinters"), DataSet)

                '/e Cache to Session       :   Ragesh 34231 02/02/06 Cache to Session

                'g_dataset_dsprinters.Tables(0).Clear()
                'Me.DataGridMetaCountry.DataSource = g_dataset_dsprinters
                'Me.DataGridMetaCountry.DataBind()
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

            If Not l_DataSet Is Nothing Then


                If l_DataSet.Tables("PaperTypes").Rows.Count > 0 Then
                    dgPaperType.DataSource = l_DataSet.Tables("PaperTypes")
                    dgPaperType.DataBind()
                    LabelPaperType.Visible = True
                Else
                    LabelPaperType.Visible = False
                End If
                

                l_DataTable = l_DataSet.Tables("Printers")

                If Not l_DataTable Is Nothing Then

                    If index < l_DataTable.Rows.Count Then

                        l_DataRow = l_DataTable.Rows.Item(index)

                        If (Not l_DataRow Is Nothing) Then

                            Me.TextBoxPrinterName.Text = CType(l_DataRow("PrinterName"), String).Trim
                            If l_DataRow("PrinterName").GetType.ToString = "System.DBNull" Then
                                Me.TextBoxPrinterName.Text = String.Empty
                            Else
                                Me.TextBoxPrinterName.Text = CType(l_DataRow("PrinterName"), String).Trim
                            End If

                            If l_DataRow("PrinterDescription").GetType.ToString = "System.DBNull" Then
                                Me.TextBoxPrinterDecription.Text = String.Empty
                            Else
                                Me.TextBoxPrinterDecription.Text = CType(l_DataRow("PrinterDescription"), String).Trim
                            End If

                            Me.hdnPrinterID.Value = l_DataRow("IntUniqueid")



                            ' If Not l_DataTable Is Nothing Then
                            dr = l_DataSet.Tables("PrinterPaperTypes").Select("[PrinterID]= " & l_DataRow("IntUniqueid") & "")
                            For Each l_dr As DataRow In dr
                                For Each l_DataGridItem As DataGridItem In dgPaperType.Items

                                    l_CheckBox = l_DataGridItem.FindControl("CheckBoxPaperType")

                                    If Convert.ToInt64(l_dr("PaperID")) = Convert.ToInt64(l_DataGridItem.Cells.Item(1).Text.Trim) Then

                                        If Not l_CheckBox Is Nothing Then
                                            l_CheckBox.Checked = True
                                        End If
                                    End If
                                Next
                            Next


                            'End If


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

    Private Sub DataGridMetaCountry_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DataGridMetaCountry.SelectedIndexChanged
        Try
            If g_bool_SearchFlag = True Then
                SearchAndPopulateData()
            Else
                PopulateData()
            End If

            ''Me.ButtonEditForm.Enabled = True
            g_integer_count = DataGridMetaCountry.SelectedIndex ' (((DataGridMetaCountry.CurrentPageIndex) * DataGridMetaCountry.PageSize) + DataGridMetaCountry.SelectedIndex)


            Session("dataset_index") = g_integer_count

            Me.TextBoxPrinterName.ReadOnly = False
            Me.TextBoxPrinterDecription.ReadOnly = False

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
            Throw ex
        End Try
    End Sub

    Private Sub DataGridMetaCountry_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles DataGridMetaCountry.PageIndexChanged
        Try
            DataGridMetaCountry.CurrentPageIndex = e.NewPageIndex

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

    ''Private Sub ButtonEditForm_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonEditForm.Click
    ''    If g_bool_SearchFlag = True Then
    ''        SearchAndPopulateData()
    ''    Else
    ''        PopulateData()
    ''    End If


    ''    ' Enable / Disable the controls.


    ''    Me.TextBoxPrinterName.ReadOnly = True
    ''    Me.TextBoxActive.ReadOnly = False
    ''    Me.TextBoxCodeValue.ReadOnly = False
    ''    Me.TextBoxDesc.ReadOnly = False
    ''    Me.TextBoxEditable.ReadOnly = False

    ''    Me.PopCalendarEffDate.Enabled = True

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

    Private Sub ButtonAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAdd.Click
        Dim l_CheckBox As CheckBox
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
            Me.TextBoxPrinterName.ReadOnly = False
            Me.TextBoxPrinterDecription.ReadOnly = False
            RequiredFieldValidator1.Enabled = True
            RequiredFieldValidator2.Enabled = True



            ' Making TextBoxes Blank
            Me.TextBoxPrinterName.Text = String.Empty
            Me.TextBoxPrinterDecription.Text = String.Empty
            For Each l_DataGridItem As DataGridItem In dgPaperType.Items

                l_CheckBox = l_DataGridItem.FindControl("CheckBoxPaperType")
                If Not l_CheckBox Is Nothing Then
                    l_CheckBox.Checked = False
                End If

            Next
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

    Private Sub _ClickButtonCancel(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonCancel.Click
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
            Me.TextBoxPrinterName.ReadOnly = True
            Me.TextBoxPrinterDecription.ReadOnly = True
            Me.DataGridMetaCountry.SelectedIndex = -1
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
            g_dataset_dsprinters = ViewState("Dataset_Printers")
            'If add Flag Is true then do Insertion else update
            g_bool_AddFlag = Session("BoolAddFlag")

            If g_bool_AddFlag = True Then
                If Not g_dataset_dsprinters Is Nothing Then

                    InsertRow = g_dataset_dsprinters.Tables(0).NewRow

                    ' Assign the values.
                    InsertRow.Item("PrinterName") = TextBoxPrinterName.Text.Trim
                    If Me.TextBoxPrinterName.Text.Trim.Length = 0 Then
                        InsertRow.Item("PrinterName") = String.Empty
                    Else
                        InsertRow.Item("PrinterName") = TextBoxPrinterName.Text.Trim
                    End If
                    ' Assign the values.
                    InsertRow.Item("PrinterDescription") = TextBoxPrinterDecription.Text.Trim
                    If Me.TextBoxPrinterDecription.Text.Trim.Length = 0 Then
                        InsertRow.Item("PrinterDescription") = String.Empty
                    Else
                        InsertRow.Item("PrinterDescription") = TextBoxPrinterDecription.Text.Trim
                    End If

                    ' Insert the row into Table.
                    g_dataset_dsprinters.Tables(0).Rows.Add(InsertRow)

                    Me.TextBoxPrinterName.Text = ""
                    Me.TextBoxPrinterDecription.Text = ""
                   


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
                        l_DataRow("PrinterName") = TextBoxPrinterName.Text.Trim
                        If Me.TextBoxPrinterName.Text.Trim.Length = 0 Then
                            l_DataRow("PrinterName") = String.Empty
                        Else
                            l_DataRow("PrinterName") = TextBoxPrinterName.Text.Trim
                        End If

                        If Me.TextBoxPrinterDecription.Text.Trim.Length = 0 Then
                            l_DataRow("PrinterDescription") = String.Empty
                        Else
                            l_DataRow("PrinterDescription") = TextBoxPrinterDecription.Text.Trim
                        End If

                        l_DataRow("IntUniqueid") = l_DataRow("IntUniqueid")
                        'g_integer_count = Session("dataset_index")
                        'If (g_integer_count <> -1) Then
                        '    Me.PopulateDataIntoControls(g_integer_count)
                        'End If
                    End If

                End If
            End If
            ' Call business layer to Save the DataSet/
            cnt = 0
            l_string_Relids = ""
            For Each l_DataGridItem As DataGridItem In dgPaperType.Items

                l_CheckBox = l_DataGridItem.FindControl("CheckBoxPaperType")
                If Not l_CheckBox Is Nothing Then
                    If l_CheckBox.Checked Then
                        'parameterListPaperTypeId(cnt) = Convert.ToInt64(l_DataGridItem.Cells.Item(1).Text.Trim)
                        l_string_Relids = l_string_Relids + "," + l_DataGridItem.Cells.Item(1).Text.Trim
                    End If
                    cnt += 1
                End If

            Next


            YMCARET.YmcaBusinessObject.MetaShellPrintersBOClass.InsertPrinter(g_dataset_dsprinters, l_string_Relids.Split(","))
            PopulateData()

            Me.ButtonSave.Enabled = False
            Me.ButtonCancel.Enabled = False
            Me.ButtonAdd.Enabled = True
            ''Me.ButtonEditForm.Enabled = False
            Me.TextBoxFind.ReadOnly = False
            Me.ButtonSearch.Enabled = True
            Me.ButtonDelete.Enabled = False
            Me.ButtonOK.Enabled = True

          
            If g_bool_AddFlag Then
                For Each l_DataGridItem As DataGridItem In dgPaperType.Items

                    l_CheckBox = l_DataGridItem.FindControl("CheckBoxPaperType")
                    If Not l_CheckBox Is Nothing Then
                        l_CheckBox.Checked = False
                    End If

                Next
            End If



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
        MessageBox.Show(PlaceHolder1, "Please Confirm", "Are you sure you want to delete this Printer?", MessageBoxButtons.YesNo)
    End Sub

    Private Sub DataGridMetaCountry_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DataGridMetaCountry.ItemDataBound
        Dim l_button_Select As ImageButton
        l_button_Select = e.Item.FindControl("ImageButtonSelect")
        If (e.Item.ItemIndex = Me.DataGridMetaCountry.SelectedIndex And Me.DataGridMetaCountry.SelectedIndex >= 0) Then
            l_button_Select.ImageUrl = "images\selected.gif"
        End If
        e.Item.Cells(1).Visible = False
        'e.Item.Cells(4).Visible = False
        'e.Item.Cells(5).Visible = False
        'e.Item.Cells(6).Visible = False
    End Sub

    Private Sub DataGridMetaCountry_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles DataGridMetaCountry.SortCommand
        Try
            Me.DataGridMetaCountry.SelectedIndex = -1
            If Not viewstate("Dataset_Printers") Is Nothing Then
                Dim dv As New DataView
                Dim SortExpression As String
                SortExpression = e.SortExpression
                g_dataset_dsprinters = viewstate("Dataset_Printers")
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
                Me.DataGridMetaCountry.DataSource = Nothing
                Me.DataGridMetaCountry.DataSource = dv
                Me.DataGridMetaCountry.DataBind()
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
            DataGridMetaCountry.Enabled = False
            DataGridMetaCountry.ToolTip = YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.TOOLTIP_READONLY).DisplayText
        End If
    End Sub
    'END : PK | 02-28-2019 | YRS-AT-4248 | Check ReadOnly Flag, if true then disable the buttons on which security is not implemented and add Tool tip.
End Class
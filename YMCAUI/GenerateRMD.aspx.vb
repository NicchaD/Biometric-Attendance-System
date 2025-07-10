'*******************************************************************************
' Copyright YMCA Retirement Fund All Rights Reserved. 
' Project Name		:	YMCA-YRS
' FileName			:	GenerateRMD.aspx.vb
' Author Name		:	Anudeep  
' Contact No		:	
' Creation Date		:	04/15/2014
' Description		:	This form is used to View generated RMD records & generate current year RMD records.
'*******************************************************************************
'Modification History
'********************************************************************************************************************************
'Modified By        Date            Description
'********************************************************************************************************************************
'Dinesh kanojia     04/21/2014      YRS 5.0-2487:In RMD Utility, freeze column headings and fix RMD Print Letters button
'Manthan Rajguru    2015.09.16      YRS-AT-2550: YRS data cleanup: copyright comments and namespace/reference (Changed the Namespace/reference from 'Infotech' to 'YMCARET')
'Manthan Rajguru    2016.10.19      YRS-AT-2922 -  YRS enh- RMD Utility- separate March and Dec, text changes, filter Fund ID, save details (TrackIT 25626)
'Pooja K            2019.02.28      YRS-AT-4248 -  YRS enh: YRS read-only access during MonthEnd Processing (TrackIT 35547) 
'*******************************************************************************
Public Class GenerateRMD
    Inherits System.Web.UI.Page
    'START: MMR | 2016.10.19 | YRS-AT-2922 | Declared constant variable
#Region "Global Declaration"
    Const isExhaustedRMDSettle As Integer = 8
#End Region
    'END: MMR | 2016.10.19 | YRS-AT-2922 | Declared constant variable

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                GetListofGeneratedRMDYears()
                GetLastProcessedDate()
            Else
                'START: MMR | 2016.10.19 | YRS-AT-2922 | Added to change Year in RMD month dynamically on post back
                AppendYearToRMDMonth(dropdownlistRMDYear.SelectedItem.Text)
                'END: MMR | 2016.10.19 | YRS-AT-2922 | Added to change Year in RMD month dynamically on post back
            End If
            CheckReadOnlyMode() 'PK| 02/28/2019 | YRS-AT-4248 | Check security method called here 
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("GenerateRMD_Page_Load", ex)
        End Try
    End Sub

    Public Sub GetListofGeneratedRMDYears()
        Dim dsRMDYears As DataSet
        Dim strNextRMDYear As String
        Dim dsGeneratedRMDRecords As DataSet
        Try
            dsRMDYears = YMCARET.YmcaBusinessObject.MRDBO.GetGeneratedRMDYears()
            If HelperFunctions.isNonEmpty(dsRMDYears) Then
                'START: MMR | 2016.10.19 | YRS-AT-2922 | Commented existing code and called method to populate MRD Years
                'rptMenuHistory.Visible = True
                'rptMenuHistory.DataSource = dsRMDYears
                'rptMenuHistory.DataBind()
                BindDropDownRMDYear(dsRMDYears)
                'END: MMR | 2016.10.19 | YRS-AT-2922 | Commented existing code and called method to populate MRD Years
                strNextRMDYear = dsRMDYears.Tables(0).Rows(0)("Year").ToString()
                If Not String.IsNullOrEmpty(strNextRMDYear) Then
                    'START: MMR | 2016.10.19 | YRS-AT-2922 | Commented existing code and added parameters to BindGrid method
                    'BindGrid(Convert.ToInt32(strNextRMDYear))
                    BindGrid(Convert.ToInt32(strNextRMDYear), 0, String.Empty)
                    'END: MMR | 2016.10.19 | YRS-AT-2922 | Commented existing code and added parameters to BindGrid method
                    lnkGenerateNextYear.Text = (Convert.ToInt32(strNextRMDYear) + 1).ToString() + " - Generate RMD"
                    ViewState("strNextRMDYear") = (Convert.ToInt32(strNextRMDYear) + 1).ToString()
                End If
                BindDropDownRMDMonth()  'MMR | 2016.10.19 | YRS-AT-2922 | Called Method to change year in RMD month on page load
            End If
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("GenerateRMD_GetListofGeneratedRMDYears", ex)
        End Try
    End Sub

    Public Sub GetLastProcessedDate()
        Dim strLastProcessedDate As String = String.Empty
        Try
            strLastProcessedDate = YMCARET.YmcaBusinessObject.MRDBO.GetLastRMDProcessedDate()
            If Not String.IsNullOrEmpty(strLastProcessedDate) Then
                lblMRDMsg.Text = "Last RMD Closed Batch: " + strLastProcessedDate
            End If
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("GenerateRMD_GetLastProccesedDate", ex)
        End Try
    End Sub

    Public Sub BindGrid(ByVal intCurrentRMDYear As Integer, ByVal month As Integer, ByVal fundNo As String) 'MMR | 2016.10.19 | YRS-AT-2922 | Added parameters for month and fund no
        Dim dsGeneratedRMDRecords As DataSet
        Try
            dsGeneratedRMDRecords = YMCARET.YmcaBusinessObject.MRDBO.GetMRDRecords(intCurrentRMDYear, month, fundNo) 'MMR | 2016.10.19 | YRS-AT-2922 | Added parameters for month and fund no
            If HelperFunctions.isNonEmpty(dsGeneratedRMDRecords) Then
                ViewState("GenerateRMD_sort") = Nothing
                gvGenerateMRDRecords.DataSource = dsGeneratedRMDRecords
                gvGenerateMRDRecords.DataBind()
                'START: MMR | 2016.10.19 | YRS-AT-2922 | Commented existing code and added to show MRD year selected
                'HighlightSubMenu(intCurrentRMDYear.ToString())
                lblRMDSelectedYear.Text = "RMD Records for the year " + intCurrentRMDYear.ToString()
                'END: MMR | 2016.10.19 | YRS-AT-2922 | Commented existing code and added to show MRD year selected
                'Start:Dinesh kanojia:04/21/2014:YRS 5.0-2487:In RMD Utility, freeze column headings and fix RMD Print Letters button
                'ViewState("GenerateRMD_dsMRDInfo") = dsGeneratedRMDRecords
                Session("GenerateRMD_dsMRDInfo") = dsGeneratedRMDRecords
                'End:Dinesh kanojia:04/21/2014:YRS 5.0-2487:In RMD Utility, freeze column headings and fix RMD Print Letters button
            Else
                'START: MMR | 2016.10.19 | YRS-AT-2922 | setting gridview datasource to nothing if dataset has empty values
                gvGenerateMRDRecords.DataSource = Nothing
                gvGenerateMRDRecords.DataBind()
                'END: MMR | 2016.10.19 | YRS-AT-2922 | setting gridview datasource to nothing if dataset has empty values
            End If
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("GenerateRMD_BindGrid", ex)
        End Try
    End Sub
    'START: MMR | 2016.10.19 | YRS-AT-2922 | Commented existing code as not required
    'Public Sub HighlightSubMenu(ByRef strRMDYear As String)
    '    Try
    '        Dim htmlGenericControl As HtmlTableCell
    '        Dim lnkRMD As LinkButton
    '        For Each rptItem As RepeaterItem In rptMenuHistory.Items
    '            lnkRMD = rptItem.FindControl("lnkRMDYear")
    '            htmlGenericControl = rptItem.FindControl("liRMDYear")
    '            If lnkRMD.Text = strRMDYear Then
    '                htmlGenericControl.Attributes("class") = "tabSelectedLink"
    '                lblRMDSelectedYear.Text = "RMD Records for the year " + strRMDYear
    '            Else
    '                htmlGenericControl.Attributes("class") = ""
    '                htmlGenericControl.Attributes("padding-bottom") = "10px"
    '            End If
    '        Next
    '    Catch ex As Exception
    '        HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
    '        HelperFunctions.LogException("GenerateRMD_HighlightSubMenu", ex)
    '    End Try
    'End Sub

    'Protected Sub lnkRMDYear_Click(sender As Object, e As EventArgs)
    '    Dim strSelectedYear As String
    '    Dim dsGeneratedRMDRecords As DataSet
    '    Try
    '        If sender IsNot Nothing AndAlso sender.Text() IsNot Nothing Then
    '            strSelectedYear = sender.Text()
    '            If Not String.IsNullOrEmpty(strSelectedYear) Then
    '                BindGrid(Convert.ToInt32(strSelectedYear))
    '            End If
    '        End If
    '    Catch ex As Exception
    '        HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
    '        HelperFunctions.LogException("GenerateRMD_lnkRMDYear_Click", ex)
    '    End Try
    'End Sub
    'END: MMR | 2016.10.19 | YRS-AT-2922 | Commented existing code as not required

    Private Sub lnkGenerateNextYear_Click(sender As Object, e As EventArgs) Handles lnkGenerateNextYear.Click
        Dim blnAllowToGenerate As Boolean
        Try
            blnAllowToGenerate = YMCARET.YmcaBusinessObject.MRDBO.IsAllowedToGenerateRMDForCurrentYear()
            If blnAllowToGenerate And Not ViewState("strNextRMDYear") Is Nothing Then
                lblMessage.Text = "Are you sure you want to generate RMD records for the year: " + ViewState("strNextRMDYear").ToString()
                ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "Open", "showConfirmdialog();", True)
            Else
                HelperFunctions.ShowMessageToUser("Please close previous year RMD", EnumMessageTypes.Error)
            End If
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("lnkGenerateNextYear_Click", ex)
        End Try
    End Sub

    Private Sub gvGenerateMRDRecords_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvGenerateMRDRecords.RowDataBound
        Try
            If e.Row.RowType = DataControlRowType.Header Then
                HelperFunctions.SetSortingArrows(ViewState("GenerateRMD_sort"), e)
            Else : e.Row.RowType = DataControlRowType.DataRow
                'START: MMR | 2016.10.19 | YRS-AT-2922 | Commented existing value and added constant instead of passing index value and also added condition to check empty row cell value
                'If e.Row.Cells(7).Text.Trim().ToLower = "yes" Then
                'If e.Row.Cells(IsExhaustedRMDSettle).Text.Trim().ToLower = "yes" Then
                If e.Row.Cells.Count > 1 AndAlso e.Row.Cells(isExhaustedRMDSettle).Text.Trim().ToLower = "yes" Then
                    'END: MMR | 2016.10.19 | YRS-AT-2922 | Commented existing value and added constant instead of passing index value and also added condition to check empty row cell value
                    e.Row.CssClass = "RMDRowHighlight"
                End If
            End If
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("GenerateRMD_gvGenerateMRDRecords_RowDataBound", ex)
        End Try
    End Sub

    Private Sub gvGenerateMRDRecords_Sorting(sender As Object, e As GridViewSortEventArgs) Handles gvGenerateMRDRecords.Sorting
        Dim dv As New DataView
        Dim dsMRDInfo As DataSet
        Try

            Me.gvGenerateMRDRecords.SelectedIndex = -1
            'Start:Dinesh kanojia:04/21/2014:YRS 5.0-2487:In RMD Utility, freeze column headings and fix RMD Print Letters button
            'If Not ViewState("GenerateRMD_dsMRDInfo") Is Nothing Then
            If Not Session("GenerateRMD_dsMRDInfo") Is Nothing Then
                'End:Dinesh kanojia:04/21/2014:YRS 5.0-2487:In RMD Utility, freeze column headings and fix RMD Print Letters button
                Dim SortExpression As String
                SortExpression = e.SortExpression
                'Start:Dinesh kanojia:04/21/2014:YRS 5.0-2487:In RMD Utility, freeze column headings and fix RMD Print Letters button
                'dsMRDInfo = DirectCast(ViewState("GenerateRMD_dsMRDInfo"), DataSet)
                dsMRDInfo = DirectCast(Session("GenerateRMD_dsMRDInfo"), DataSet)
                'End:Dinesh kanojia:04/21/2014:YRS 5.0-2487:In RMD Utility, freeze column headings and fix RMD Print Letters button
                dv = dsMRDInfo.Tables(0).DefaultView
                dv.Sort = SortExpression
                HelperFunctions.gvSorting(ViewState("GenerateRMD_sort"), e.SortExpression, dv)
                Me.gvGenerateMRDRecords.DataSource = dv
                Me.gvGenerateMRDRecords.DataBind()
            End If
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("GenerateRMD_gvGenerateMRDRecords_Sorting", ex)
        End Try
    End Sub

    Private Sub btnPrintLetters_Click(sender As Object, e As EventArgs) Handles btnPrintLetters.Click
        Try
            'Dinesh kanojia:04/21/2014:YRS 5.0-2487:In RMD Utility, freeze column headings and fix RMD Print Letters button
            Response.Redirect("SecurityCheck.aspx?Form=RMDPrintLetters.aspx")
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("GenerateRMD_btnPrintLetters_Click", ex)
        End Try
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Try
            Response.Redirect("MainWebForm.aspx")
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("GenerateRMD_btnClose_Click", ex)
        End Try
    End Sub

    Private Sub btnYes_Click(sender As Object, e As EventArgs) Handles btnYes.Click
        Dim blnAllowToGenerate As Boolean
        Dim dtmGenerateMRD As DateTime
        Dim strDtProcessDate As String = String.Empty
        Dim strStatus As String = String.Empty
        Try
            blnAllowToGenerate = YMCARET.YmcaBusinessObject.MRDBO.IsAllowedToGenerateRMDForCurrentYear()
            If blnAllowToGenerate Then
                strDtProcessDate = YMCARET.YmcaBusinessObject.MRDBO.GetLastRMDProcessedDate()
                If strDtProcessDate <> String.Empty Then
                    dtmGenerateMRD = Convert.ToDateTime(strDtProcessDate).AddDays(1)
                    strStatus = YMCARET.YmcaBusinessObject.MRDBO.GenerateMRDRecords(dtmGenerateMRD)
                    If String.IsNullOrEmpty(strStatus) Then
                        GetListofGeneratedRMDYears()
                        HelperFunctions.ShowMessageToUser("RMD generated successfully.", EnumMessageTypes.Success)
                    Else
                        HelperFunctions.ShowMessageToUser(strStatus, EnumMessageTypes.Error)
                    End If
                End If
            Else
                HelperFunctions.ShowMessageToUser("Please close previous year RMD", EnumMessageTypes.Error)
            End If
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("GenerateRMD_lnkGenerateNextYear_Click", ex)
        End Try
    End Sub
    'START: MMR | 2016.10.19 | YRS-AT-2922 | Added to populate MRD Years
    Public Sub BindDropDownRMDYear(ByRef yearsData As DataSet)
        If HelperFunctions.isNonEmpty(yearsData) Then
            dropdownlistRMDYear.DataSource = yearsData.Tables(0)
            dropdownlistRMDYear.DataTextField = "Year"
            dropdownlistRMDYear.DataValueField = "Year"
            dropdownlistRMDYear.DataBind()
            dropdownlistRMDYear.SelectedValue = yearsData.Tables(0).Rows(0)("Year")
        End If
    End Sub
    'END: MMR | 2016.10.19 | YRS-AT-2922 | Added to populate MRD Years

    'START: MMR | 2016.10.19 | YRS-AT-2922 | To Search data in List based on MRD Year, MRD Month and FundNo
    Private Sub buttonSearch_Click(sender As Object, e As EventArgs) Handles buttonSearch.Click
        Try
            BindGrid(Convert.ToInt32(dropdownlistRMDYear.SelectedValue), Convert.ToInt32(dropdownlistRMDMonth.SelectedValue), textboxFundNo.Text.Trim())
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("buttonSearch_Click", ex)
        End Try
    End Sub
    'END: MMR | 2016.10.19 | YRS-AT-2922 | To Search data in List based on MRD Year, MRD Month and FundNo

    'START: MMR | 2016.10.19 | YRS-AT-2922 | Added to populate RMD month
    Public Sub BindDropDownRMDMonth()
        dropdownlistRMDMonth.Items.Clear()
        dropdownlistRMDMonth.Items.Insert(0, New ListItem("Any", "0"))
        dropdownlistRMDMonth.Items.Insert(1, New ListItem(String.Format("{0}{1}", "Dec - ", dropdownlistRMDYear.SelectedItem.Text), "12"))
        dropdownlistRMDMonth.Items.Insert(2, New ListItem(String.Format("{0}{1}", "Mar - ", Convert.ToInt32(dropdownlistRMDYear.SelectedItem.Text) + 1), "3"))
    End Sub
    'END: MMR | 2016.10.19 | YRS-AT-2922 | Added to populate RMD month

    'START: MMR | 2016.10.19 | YRS-AT-2922 | Added to add year to RMD Month
    Public Sub AppendYearToRMDMonth(ByVal year As String)
        dropdownlistRMDMonth.Items(1).Text = String.Format("{0}{1}", "Dec - ", year)
        dropdownlistRMDMonth.Items(2).Text = String.Format("{0}{1}", "Mar - ", Convert.ToInt32(year) + 1)
    End Sub
    'END: MMR | 2016.10.19 | YRS-AT-2922 | Added to add year to RMD Month
    'START : PK | 02-28-2019 | YRS-AT-4248 | Check ReadOnly Flag, if true then disable the buttons on which security is not implemented and add Tool tip.
    Public Sub CheckReadOnlyMode()
        If (SecurityCheck.IsApplicationInReadOnlyMode()) Then
            lnkGenerateNextYear.Enabled = False
            lnkGenerateNextYear.ToolTip = YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.TOOLTIP_READONLY).DisplayText
        End If
    End Sub
    'END : PK | 02-28-2019 | YRS-AT-4248 | Check ReadOnly Flag, if true then disable the buttons on which security is not implemented and add Tool tip. 
End Class

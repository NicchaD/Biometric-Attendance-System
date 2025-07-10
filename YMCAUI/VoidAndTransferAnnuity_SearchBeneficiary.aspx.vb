'*******************************************************************************
' Copyright YMCA Retirement Fund All Rights Reserved. 
'
' Project Name		:	YMCA-YRS
' FileName			:	VoidAndTransferAnnuity_SearchBeneficiary.aspx.vb
' Author Name		:	Sanjay Rawat
' Employee ID		:	51193
' Email			    :	sanjay.singh@3i-infotech.com
' Contact No		:	8637
' Creation Time	    :	16/09/2013
' Program Specification Name	:	
' Unit Test Plan Name			:	
' Description					:	<<Please put the brief description here...>>
'*******************************************************************************
'Modification History
'********************************************************************************************************************************
'Modified By        Date            Description
'********************************************************************************************************************************
'Shashank           2014.08.25      YRS 5.0-2279 : Add in Administration Screen ability to change messages for YRS or Web site
'Manthan Rajguru    2015.09.16      YRS-AT-2550: YRS data cleanup: copyright comments and namespace/reference (Changed the Namespace/reference from 'Infotech' to 'YMCARET')
'********************************************************************************************************************************
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
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports YMCAObjects
Imports YMCARET.YmcaBusinessObject

Public Class VoidAndTransferAnnuity_SearchBeneficiary
    Inherits System.Web.UI.Page

#Region "Local Variables"
    Dim dsSearchResult As DataSet
    Dim intSelectedDataGridItem As Integer
    Dim strSSNo As String
    Dim dsDisbursement As DataSet
#End Region
    '**********************************Page_Load()**********************************'
    ' 1. If Session("VTA_SearchPersonResult") is not empty then populate in dataset.   
    '*******************************************************************************'
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Session("LoggedUserKey") Is Nothing Then
                Response.Redirect("Login.aspx", False)
            End If

            If Not IsPostBack Then
                ViewState("dtAdded") = Nothing
            End If
            '1. If Session("VTA_SearchPersonResult") is not empty then populate in dataset. 
            If Not IsNothing(Session("VTA_SearchPersonResult")) Then
                dsSearchResult = Session("VTA_SearchPersonResult")
            End If

        Catch ex As Exception
            HelperFunctions.LogException("Void & transfer Annuity - Search Payee--> Page_Load", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
        End Try
    End Sub
    '**********************************Populatedata()****************************************************************************************'
    ' 1. Validation for empty text boxes
    ' 2. Get the data(undeceased participant) in dataset and Session("VTA_SearchPersonResult") according to search criteria from BAL layer 
    ' 3. Bind datagrid with dataset 
    ' 4. This procedure is used in btnFind_Click() method.
    '**********************************************************************************************************************************************'

    Private Sub Populatedata()
        Try
            ' 1. Validation for empty text boxes
            If txtFundno.Text = String.Empty And txtFirstName.Text = String.Empty And txtLastName.Text = String.Empty And txtSSNo.Text = String.Empty Then
                'SP 2014.08.25 -YRS 5.0- 2279 - Start
                'MessageBox.Show(PlaceHolder1, "Void and Transfer Annuity", "This search will take a longer time. It is suggested that you enter a search criteria.", MessageBoxButtons.OK)
                HelperFunctions.ShowMessageToUser(MetaMessageList.MESSAGE_VTA_SEARCH_LONGER_TIME_VALIDATION)
                Exit Sub
                'SP 2014.08.25 -YRS 5.0- 2279 - End
            Else
                ' 2. Get the data in dataset and Session("VTA_SearchPersonResult") according to search criteria from BAL layer 
                dsSearchResult = YMCARET.YmcaBusinessObject.VoidAndTransferAnnuityBOClass.LookUpPerson(Me.txtSSNo.Text.Trim(), Me.txtFundno.Text.Trim(), Me.txtLastName.Text.Trim(), Me.txtFirstName.Text.Trim(), "VTA_Payee")
                Session("VTA_SearchPersonResult") = dsSearchResult
                ' 3. Bind datagrid with dataset 
                If HelperFunctions.isNonEmpty(dsSearchResult) Then
                    gvSearchPayee.DataSource = dsSearchResult
                    gvSearchPayee.DataBind()
                    'Added by Anudeep as per Observations 23-oct-2012 
                    'shows how many record exists in grid
                    'SP 2014.08.25 -YRS 5.0- 2279 - Start
                    'lblTotalCount.Text = GetMessage("MESSAGE_VTA_SHOW_TOTAL_RECORDS") + " " + dsSearchResult.Tables(0).Rows.Count.ToString()
                    Dim dict As New Dictionary(Of String, String)
                    dict.Add("TOTALRECORDS", dsSearchResult.Tables(0).Rows.Count.ToString())
                    lblTotalCount.Text = MetaMessageBO.GetMessageByTextMessageNo(MetaMessageList.MESSAGE_VTA_SHOW_TOTAL_RECORDS, dict)
                    'SP 2014.08.25 -YRS 5.0- 2279 - End
                    LabelNoDataFound.Visible = False
                Else
                    gvSearchPayee.DataSource = Nothing
                    gvSearchPayee.DataBind()
                    lblTotalCount.Text = ""
                    LabelNoDataFound.Visible = True
                End If
            End If
        Catch
            Throw
        End Try
    End Sub
    '****************************gvSearchPayee_PageIndexChanging()***************************'
    ' 1. Save data in dataset from session
    ' 2. change page index
    ' 3. Apply sorting 
    ' 3. Bind grid with page index change
    ' 4. Save new page index value in viewstate
    '*****************************************************************************************'

    Private Sub gvSearchPayee_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvSearchPayee.PageIndexChanging
        Dim dv As DataView
        Dim l_dataset As DataSet
        Dim gvSort As GridViewCustomSort 'SP 2014.08.27   YRS 5.0-2279  
        Try

            gvSearchPayee.SelectedIndex = -1
            If Not Session("VTA_SearchPersonResult") Is Nothing Then
                l_dataset = Session("VTA_SearchPersonResult")
                dv = l_dataset.Tables(0).DefaultView
                Me.gvSearchPayee.PageIndex = e.NewPageIndex
                If Not ViewState("Add_Sort") Is Nothing Then
                    'SP 2014.08.27   YRS 5.0-2279  - Start
                    ' dv.Sort = ViewState("Add_Sort")
                    gvSort = ViewState("Add_Sort")
                    dv.Sort = gvSort.SortExpression + " " + gvSort.SortDirection
                    'SP 2014.08.27   YRS 5.0-2279  - End
                End If
                gvSearchPayee.DataSource = dv
                gvSearchPayee.DataBind()
                ViewState("Add_PageIndex") = e.NewPageIndex
            End If
        Catch ex As Exception
            HelperFunctions.LogException("Void & transfer Annuity - Search Payee--> gvSearchPayee_PageIndexChanging", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
        End Try
    End Sub
    'SP 2014.08.27   YRS 5.0-2279  - Start
    Private Sub gvSearchPayee_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvSearchPayee.RowDataBound
        Try
            HelperFunctions.SetSortingArrows(ViewState("Add_Sort"), e)
        Catch ex As Exception
            HelperFunctions.LogException("Void & transfer Annuity - Search Payee--> gvSearchPayee_RowDataBound", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
        End Try
    End Sub
    'SP 2014.08.27   YRS 5.0-2279  - End
    '****************************gvSearchPayee_SelectedIndexChanged()***************************'
    ' 1. Get Payee record from selected row.
    ' 2. Add Payee record in datatable & save it in session
    ' 3. Close the pop-up window .  
    '************************************************************************************'

    Private Sub gvSearchPayee_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvSearchPayee.SelectedIndexChanged
        'Dim drSelected As DataRow
        Dim dtSelected As New DataTable
        Dim strfilterCriteria As String
        Try
            ' 1. Get Payee record from selected row.
            strfilterCriteria = "SSNo='" & IIf(String.IsNullOrEmpty(gvSearchPayee.SelectedRow.Cells(2).Text.Trim), "", gvSearchPayee.SelectedRow.Cells(2).Text.Trim) & "'"
            dtSelected = dsSearchResult.Tables(0).Clone()
            ' 2. Add Payee record in datatable & save it in session
            For Each dr In dsSearchResult.Tables(0).Select(strfilterCriteria)
                dtSelected.ImportRow(dr)
            Next
            dtSelected.AcceptChanges()
            Session("VTA_PayeeDetails") = dtSelected

            ' 3. Close the pop-up window .  
            Dim msg As String
            msg = msg + "<Script Language='JavaScript'>"
            msg = msg + "window.opener.document.forms(0).submit();"
            msg = msg + "self.close();"
            msg = msg + "</Script>"
            Response.Write(msg)

        Catch ex As Exception
            HelperFunctions.LogException("Void & transfer Annuity - Search Payee--> gvSearchPayee_SelectedIndexChanged", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
        End Try
    End Sub
    '****************************gvSearchPayee_Sorting()***************************'
    ' 1. Call Sortgrid() procedure for sorting.
    '***********************************************************************'

    Private Sub gvSearchPayee_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvSearchPayee.Sorting
        Me.gvSearchPayee.SelectedIndex = -1
        Try
            Sortgrid(e)
        Catch ex As Exception
            HelperFunctions.LogException("Void & transfer Annuity - Search Payee--> gvSearchPayee_Sorting", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
        End Try
    End Sub
    '****************************Sortgrid()*******************************************************************************'
    '1. This procedure used to sort grid view fields
    '2. This procedure used in gvSearchPayee_Sorting method    
    '*********************************************************************************************************************'

    Public Sub Sortgrid(ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs)
        Dim l_dataset As DataSet
        Dim dv As DataView
        Dim gvSort As GridViewCustomSort ' 'SP 2014.08.27   YRS 5.0-2279 
        Try
            If Not Session("VTA_SearchPersonResult") Is Nothing Then
                l_dataset = Session("VTA_SearchPersonResult")

                'SP 2014.08.27   YRS 5.0-2279 - Start
                'Dim SortExpression As String
                'SortExpression = e.SortExpression
                dv = l_dataset.Tables(0).DefaultView
                HelperFunctions.gvSorting(ViewState("Add_Sort"), e.SortExpression, dv)
                'dv.Sort = SortExpression
                'If Not ViewState("Add_Sort") Is Nothing Then
                '    If ViewState("Add_Sort").ToString.Trim.EndsWith("ASC") Then
                '        dv.Sort = SortExpression + " DESC"
                '    Else
                '        dv.Sort = SortExpression + " ASC"
                '    End If
                'Else
                '    dv.Sort = SortExpression + " ASC"
                'End If
                If ViewState("Add_Sort") IsNot Nothing Then
                    gvSort = ViewState("Add_Sort")
                    dv.Sort = gvSort.SortExpression + " " + gvSort.SortDirection
                End If
                'SP 2014.08.27   YRS 5.0-2279 - End
                gvSearchPayee.DataSource = dv
                gvSearchPayee.DataBind()
                ' ViewState("Add_Sort") = dv.Sort() 'SP 2014.08.27   YRS 5.0-2279
            End If
        Catch
            Throw
        End Try
    End Sub
    '****************************GetMessage()*******************************************************************************'
    '1. Gets the message from resource file
    '2. This procedure used in Populatedata() procedure.
    '*********************************************************************************************************************'
    'SP 2014.08.25 -YRS 5.0- 2279 - Start
    'Public Function GetMessage(ByVal ResorceKey As String)
    '    Try
    '        Dim strMessage As String
    '        Try
    '            strMessage = GetGlobalResourceObject("Disbursement", ResorceKey).ToString()
    '        Catch ex As Exception
    '            strMessage = ResorceKey
    '        End Try
    '        Return strMessage
    '    Catch ex As Exception
    '        Throw ex
    '    End Try
    'End Function
    'SP 2014.08.25 -YRS 5.0- 2279 - End
    '****************************btnClear_Click()************************************'
    ' 1. clear page controls & bind grid view with no rows.
    '*********************************************************************************'

    Private Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear.Click
        Try
            txtFirstName.Text = ""
            txtLastName.Text = ""
            txtFundno.Text = ""
            txtSSNo.Text = ""
            lblTotalCount.Text = ""
            gvSearchPayee.PageIndex = 0
            gvSearchPayee.SelectedIndex = -1
            gvSearchPayee.DataSource = Nothing
            gvSearchPayee.DataBind()
        Catch ex As Exception
            HelperFunctions.LogException("Void & transfer Annuity - Search Payee--> btnClear_Click", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
        End Try
    End Sub
    '**********************************btnFind_Click()**********************************'
    ' 1. Validation for empty text boxes.
    ' 2. Call Populatedata() procedure.
    '****************************************************************************************'

    Protected Sub btnFind_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnFind.Click
        Try
            gvSearchPayee.SelectedIndex = -1
            gvSearchPayee.PageIndex = 0
            Populatedata()
        Catch ex As Exception
            HelperFunctions.LogException("Void & transfer Annuity - Search Payee--> btnFind_Click", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
        End Try
    End Sub
End Class
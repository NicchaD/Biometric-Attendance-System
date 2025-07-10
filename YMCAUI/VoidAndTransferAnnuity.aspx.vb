'*******************************************************************************
' Copyright YMCA Retirement Fund All Rights Reserved. 
'
' Project Name		:	YMCA-YRS
' FileName			:	VoidAndTransferAnnuity.aspx.vb
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
'Sanjay R.         2014.02.06       YRS 5.0-1328: Add guiEntityID in address table for update & Insert.
'Shashank          2014.08.25       YRS 5.0-2279 : Add in Administration Screen ability to change messages for YRS or Web site 
'Shashank          2014.09.09       YRS 5.0-2279 : Add in Administration Screen ability to change messages for YRS or Web site 
'Manthan Rajguru   2015.09.16       YRS-AT-2550: YRS data cleanup: copyright comments and namespace/reference (Changed the Namespace/reference from 'Infotech' to 'YMCARET')
'Pramod P. Pokale  2015.10.13       YRS-AT-2588: implement some basic telephone number validation Rules
'Chandra C         2016.02.18       YRS-AT-1483: YRS enh: Withdrawals Phase2: sprint 2 - Move web methods for address updates into YRSwebService, (GetAddressPhone, UpdateAddressPhone add new Lock Address validation)
'********************************************************************************************************************************
Imports System.Data
Imports System.Data.SqlClient
Imports System.Web
Imports System.Web.SessionState
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports YMCAObjects
Imports YMCARET.YmcaBusinessObject


Public Class VoidAndTransferAnnuity
    Inherits System.Web.UI.Page

#Region "Local Variables"
    'Dim dsSearchResults As DataSet
    Dim intSelectedDataGridItem As Integer
    Dim strSSNo As String
    'Dim dsDisbursement As DataSet
    Dim checkBoxArray As ArrayList
#End Region

#Region "Property"
    Protected Property dsSearchResults As DataSet
        Get
            Return Session("VTA_dsSearchResults")
        End Get
        Set(ByVal value As DataSet)
            Session("VTA_dsSearchResults") = value
        End Set
    End Property

    Protected Property dsDisbursement As DataSet
        Get
            Return Session("VTA_dsDisbursement")
        End Get
        Set(ByVal value As DataSet)
            Session("VTA_dsDisbursement") = value
        End Set
    End Property

    Protected Property strRetireeDeathDate As String
        Get
            Return ViewState("VTA_Deathdate")
        End Get
        Set(ByVal value As String)
            ViewState("VTA_Deathdate") = value
        End Set
    End Property

    Protected Property strRetireePersId As String
        Get
            Return ViewState("VTA_PersId")
        End Get
        Set(ByVal value As String)
            ViewState("VTA_PersId") = value
        End Set
    End Property

#End Region

    Private Property hdnDirty As Control


    '#Region "Local Variable Persistence mechanism"
    '    Protected Overrides Function SaveViewState() As Object
    '        intSelectedDataGridItem = gvSearchRetiree.SelectedIndex
    '        Dim al As New ArrayList
    '        al.Add(StoreLocalVariablesToCache())
    '        al.Add(MyBase.SaveViewState())
    '        Return al
    '    End Function
    '    Protected Overrides Sub LoadViewState(ByVal savedState As Object)
    '        Dim al As ArrayList = DirectCast(savedState, ArrayList)
    '        InitializeLocalVariablesFromCache(al.Item(0))
    '        MyBase.LoadViewState(al.Item(1))
    '    End Sub

    '    Private Sub InitializeLocalVariablesFromCache(ByRef obj As Object)
    '        Try
    '            'Session: Load data from Session so that it is accessible on Postback - This can be replaced by a load from viewstate or database
    '            dsSearchResults = Session("VTA_dsSearchResults")
    '            intSelectedDataGridItem = Session("VTA_SelectedDataGridItem")
    '            dsDisbursement = Session("VTA_dsDisbursement")
    '        Catch ex As Exception
    '            Throw ex
    '        End Try
    '    End Sub
    '    Private Function StoreLocalVariablesToCache() As Object
    '        Try
    '            'Session: Pass data into Session so that it is accessible on Postback - This can be replaced by a save to viewstate
    '            Session("VTA_dsSearchResults") = dsSearchResults
    '            Session("VTA_SelectedDataGridItem") = intSelectedDataGridItem
    '            Session("VTA_dsDisbursement") = dsDisbursement
    '        Catch ex As Exception
    '            Throw ex
    '        End Try
    '    End Function
    '#End Region




    '**********************************Page_Load()****************************************'
    ' 1. Initialise page controls, address control
    ' 2. Populate & initialise page controls if user landing from existing payee page
    '****************************************************************************************'

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Session("LoggedUserKey") Is Nothing Then
                Response.Redirect("Login.aspx", False)
            End If

            Me.txtSSNo.Attributes.Add("OnKeyPress", "javascript:ValidateNumericSSNO(document.getElementById('" + txtSSNo.ClientID + "').value);")
            Me.txtTelephone.Attributes.Add("OnKeyPress", "javascript:ValidateNumeric();")
            ' Me.txtEmail.Attributes.Add("onblur", "javascript:isValidEmail(document.getElementById('" + txtEmail.ClientID + "').value);") 'SP 2014.08.26 -YRS 5.0- 2279

            'Initialise page controls, address control
            If Not IsPostBack Then
                ucPayeeAddress.IsPrimary = 1
                btnSearchPayee.Attributes.Add("onclick", "javascript: OpenPopUp('All'); return false;")
                EnableControls(False)
                ucPayeeAddress.EnableControls = False
                btnTransfer.Enabled = False
                btnCancel.Enabled = False
                Me.Form.DefaultButton = btnFind.UniqueID
            End If

            'Populate & initialise page controls if user landing from existing payee page
            If Not IsNothing(Session("VTA_PayeeDetails")) Then
                LoadPayee()
                EnableControls(False)
                ucPayeeAddress.EnableControls = True
                btnAddPayee.Enabled = False
                btnSearchPayee.Enabled = False
                btnTransfer.Enabled = True
                btnCancel.Enabled = True
                SetDirtyFlag("true")
            End If

        Catch ex As Exception
            HelperFunctions.LogException("Void & transfer Annuity --> Page_Load", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
        End Try
    End Sub
    '**********************************ButtonFind_Click()**********************************'
    ' 1. Validation for empty text boxes
    ' 2. Populate datagrid with deceased retiree based on search condition.
    '****************************************************************************************'
    Private Sub btnFind_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFind.Click
        Try
            'Validation for empty text boxes
            txtRetireeSSNo.Text = txtRetireeSSNo.Text.Replace("-", "")
            If Me.txtRetireeSSNo.Text.Trim = "" And Me.txtRetireeLastName.Text.Trim = "" And Me.txtRetireeFirstName.Text.Trim = "" And Me.txtRetireeFundNo.Text.Trim = "" Then
                HelperFunctions.BindGrid(gvSearchRetiree, CType(Nothing, DataSet))
                'SP 2014.08.25   YRS 5.0-2279  - Start
                'HelperFunctions.ShowMessageToUser(GetMessage("MESSAGE_VTA_SHOW_ENTER_VALUE"), EnumMessageTypes.Error, Nothing)
                HelperFunctions.ShowMessageToUser(MetaMessageList.MESSAGE_VTA_SHOW_ENTER_VALUE)
                'SP 2014.08.25   YRS 5.0-2279  - End
                Exit Sub
            End If

            'Populate datagrid with deceased retiree based on search condition.
            dsSearchResults = YMCARET.YmcaBusinessObject.VoidAndTransferAnnuityBOClass.LookUpPerson(Me.txtRetireeSSNo.Text.Trim(), Me.txtRetireeFundNo.Text.Trim(), Me.txtRetireeLastName.Text.Trim(), Me.txtRetireeFirstName.Text.Trim(), "VTA_Person")
            If HelperFunctions.isEmpty(dsSearchResults) Then
                Me.lblNoDataFound.Visible = True
                Me.tabStripVoidandTransferAnnuity.Items(1).Enabled = False
                HelperFunctions.BindGrid(gvSearchRetiree, dsSearchResults)
            Else
                Me.tabStripVoidandTransferAnnuity.Items(1).Enabled = True
                'gvSearchRetiree.SelectedIndex = 0
                Me.lblNoDataFound.Visible = False
                HelperFunctions.BindGrid(gvSearchRetiree, dsSearchResults)
                gvSearchRetiree.SelectedIndex = -1
            End If
        Catch ex As Exception
            HelperFunctions.LogException("Void & transfer Annuity --> ButtonFind_Click ", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
        End Try
    End Sub
    '****************************gvDisbursement_PageIndexChanging()***************************'
    ' 1. change page index
    ' 2. Bind grid with page index and sorting changes
    '*****************************************************************************************'
    Private Sub gvSearchRetiree_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvSearchRetiree.PageIndexChanging
        Try
            Me.gvSearchRetiree.PageIndex = e.NewPageIndex
            ViewState("gvRetiree_PageIndex") = e.NewPageIndex
            Dim dtRetiree As DataTable
            Dim dv As DataView
            Dim gvSort As GridViewCustomSort 'SP 2014.08.27   YRS 5.0-2279  
            Try
                If HelperFunctions.isNonEmpty(dsSearchResults) Then
                    dtRetiree = dsSearchResults.Tables(0)
                End If

                If HelperFunctions.isNonEmpty(dtRetiree) Then
                    dv = dtRetiree.DefaultView
                    If Not ViewState("previousSearchSortExpression") Is Nothing Then
                        'SP 2014.08.27   YRS 5.0-2279  - Start
                        ' dv.Sort() = ViewState("previousSearchSortExpression")  
                        gvSort = ViewState("previousSearchSortExpression")
                        dv.Sort = gvSort.SortExpression + " " + gvSort.SortDirection
                        'SP 2014.08.27   YRS 5.0-2279  - End
                    End If
                    'SP 2014.08.27   YRS 5.0-2279  - Start -Commented becuase already assigned in first line
                    'If Not ViewState("gvRetiree_PageIndex") Is Nothing Then
                    '    Me.gvSearchRetiree.PageIndex = ViewState("gvRetiree_PageIndex")
                    'End If
                    'SP 2014.08.27   YRS 5.0-2279  - End
                End If

                gvSearchRetiree.DataSource = dv
                gvSearchRetiree.DataBind()
                SetSelectedRow(dv)  'Set Selected row
                dv = Nothing
            Catch
                Throw
            Finally
                dv = Nothing
            End Try

        Catch ex As Exception
            HelperFunctions.LogException("Void & transfer Annuity --> gvDisbursement_PageIndexChanging ", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
        End Try
    End Sub

    '**********************************gvSearchRetiree_ItemDataBound()**********************************'
    ' 1. If number of records exceeds page size then display message
    '********************************************************************************************'
    'Private Sub gvSearchRetiree_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles gvSearchRetiree.RowDataBound
    '    Try
    '        If e.Item.ItemIndex > gvSearchRetiree.PageSize Then
    '            e.Item.Visible = False
    '            lbl_Search_MoreItems.Visible = True
    '            lbl_Search_MoreItems.Text = String.Format(GetMessage("MESSAGE_VTA_TRUNCATE_RESULT"), gvSearchRetiree.PageSize.ToString(), dsSearchResults.Tables(0).DefaultView.Count.ToString())
    '        End If
    '    Catch ex As Exception
    '        HelperFunctions.LogException("Void & transfer Annuity --> gvSearchRetiree_ItemDataBound ", ex)
    '        HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
    '    End Try
    'End Sub

    '**********************************gvSearchRetiree_RowDataBound()***************************'
    ' 1. If number of records exceeds page size then display message
    '********************************************************************************************'
    '--not needed
    Private Sub gvSearchRetiree_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvSearchRetiree.RowDataBound

        Try
            HelperFunctions.SetSortingArrows(ViewState("previousSearchSortExpression"), e)
            If e.Row.RowIndex > gvSearchRetiree.PageSize Then
                e.Row.Visible = False
                lbl_Search_MoreItems.Visible = True
                'SP 2014.08.25   YRS 5.0-2279  - Start (not fired)
                'lbl_Search_MoreItems.Text = String.Format(GetMessage("MESSAGE_VTA_TRUNCATE_RESULT"), gvSearchRetiree.PageSize.ToString(), dsSearchResults.Tables(0).DefaultView.Count.ToString())
                'SP 2014.08.25   YRS 5.0-2279  - End
            End If
        Catch ex As Exception
            HelperFunctions.LogException("Void & transfer Annuity --> gvSearchRetiree_ItemDataBound ", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
        End Try
    End Sub

    '****************************TabStripVoidandTransferAnnuity_SelectedIndexChange()***********************'
    ' 1. If user select List tab then set Find as default button.
    ' 2. If user select Disbursement tab without selecting deacesed retiree then display appropriate message.
    '*********************************************************************************'
    Private Sub TabStripVoidandTransferAnnuity_SelectedIndexChange(ByVal sender As Object, ByVal e As System.EventArgs) Handles tabStripVoidandTransferAnnuity.SelectedIndexChange
        Try
            If Me.tabStripVoidandTransferAnnuity.SelectedIndex > -1 Then
                Me.multiPageVoidandTransferAnnuity.SelectedIndex = Me.tabStripVoidandTransferAnnuity.SelectedIndex
                ' 1. If user select List tab then set Find as default button.
                If Me.tabStripVoidandTransferAnnuity.SelectedIndex = 0 Then
                    Me.Form.DefaultButton = btnFind.UniqueID
                Else
                    ' 2. If user select Disbursement tab without selecting deacesed retiree then display appropriate message.
                    If gvSearchRetiree.SelectedIndex < 0 Then
                        'SP 2014.08.25   YRS 5.0-2279  - Start
                        'HelperFunctions.ShowMessageToUser(GetMessage("MESSAGE_VTA_SELECT_RETIREE"), EnumMessageTypes.Error, Nothing)
                        HelperFunctions.ShowMessageToUser(MetaMessageList.MESSAGE_VTA_SELECT_RETIREE)
                        'SP 2014.08.25   YRS 5.0-2279  - End
                        Me.tabStripVoidandTransferAnnuity.SelectedIndex = 0
                        Me.multiPageVoidandTransferAnnuity.SelectedIndex = 0
                        Me.Form.DefaultButton = btnFind.UniqueID
                        Exit Sub
                    End If
                End If
            End If
        Catch ex As Exception
            HelperFunctions.LogException("Void & transfer Annuity --> TabStripVoidandTransferAnnuity_SelectedIndexChange ", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
        End Try
    End Sub
    '****************************PopulateDisbursement()*******************************************************************************'
    '1. Call BAL layer to get disbursemet records
    '2. Save disbursement records in Session("VTA_dsDisbursement")
    '3. Bind griedview with disbursemet records
    '4. This procedure used in SaveandtransferAnnuity procdure and TabStripVoidandTransferAnnuity_SelectedIndexChange() method
    '*********************************************************************************************************************************'
    Public Sub PopulateDisbursement()
        Try
            lblcolorcode.Visible = False
            lblDisbAfterDeath.Visible = False
            dsDisbursement = YMCARET.YmcaBusinessObject.VoidAndTransferAnnuityBOClass.GetDisbursementsByPersId(strRetireePersId)
            Session("VTA_dsDisbursement") = dsDisbursement
            HelperFunctions.BindGrid(gvDisbursement, dsDisbursement)
            If gvDisbursement.Rows.Count = 0 Then
                lblNoDisbursement.Visible = True
                btnSearchPayee.Enabled = False
                btnAddPayee.Enabled = False
            Else
                lblNoDisbursement.Visible = False
                btnSearchPayee.Enabled = True
                btnAddPayee.Enabled = True
            End If

        Catch ex As Exception
            Throw
        End Try
    End Sub
    '****************************gvDisbursement_PageIndexChanging()***************************'
    ' 1. If user select record(s) by clicking checkboxes then save these records in viewstate
    ' 2. change page index
    ' 3. Bind grid with page index change
    ' 4. select checked records from viewstate
    '*****************************************************************************************'
    Private Sub gvDisbursement_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvDisbursement.PageIndexChanging
        Try
            SaveCheckedValues()
            Me.gvDisbursement.PageIndex = e.NewPageIndex
            ViewState("PageIndex") = e.NewPageIndex
            Bindgrids()
            PopulateCheckedValues()
        Catch ex As Exception
            HelperFunctions.LogException("Void & transfer Annuity --> gvDisbursement_PageIndexChanging ", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
        End Try
    End Sub
    '****************************Bindgrids()******************************'
    '1. This procedure used to bind data grid after sorting / page changes
    '2. This procedure used in gvDisbursement_Sorting method 
    '*********************************************************************'
    Public Sub Bindgrids()
        Dim dtDisbursement As DataTable
        Dim dv As DataView
        Dim gvSort As GridViewCustomSort ' 'SP 2014.08.25   YRS 5.0-2279  
        Try
            If HelperFunctions.isNonEmpty(dsDisbursement) Then
                dtDisbursement = dsDisbursement.Tables(0)
            End If

            If HelperFunctions.isNonEmpty(dtDisbursement) Then
                dv = dtDisbursement.DefaultView
                If Not ViewState("Sort") Is Nothing Then
                    ' 'SP 2014.08.25   YRS 5.0-2279  - Start
                    gvSort = ViewState("Sort")
                    'dv.Sort() = ViewState("Sort")   
                    dv.Sort = gvSort.SortExpression + " " + gvSort.SortDirection
                    'SP 2014.08.25   YRS 5.0-2279 - End
                End If
                If Not ViewState("PageIndex") Is Nothing Then
                    Me.gvDisbursement.PageIndex = ViewState("PageIndex")
                End If
            End If
            'SP 2014.08.25   YRS 5.0-2279 -Start
            HelperFunctions.BindGrid(gvDisbursement, dv)
            'gvDisbursement.DataSource = dv
            'gvDisbursement.DataBind()
            'SP 2014.08.25   YRS 5.0-2279 -End
            dv = Nothing
        Catch
            Throw
        Finally
            dv = Nothing
        End Try
    End Sub

    '****************************Sortgrid()*******************************************************************************'
    '1. This procedure used to sort grid view fields
    '2. This procedure used in gvDisbursement_Sorting method    
    '*********************************************************************************************************************'
    Public Sub Sortgrid(ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs)
        Dim SortExpression As String
        Try
            SortExpression = e.SortExpression
            If Not ViewState("Sort") Is Nothing Then
                If ViewState("Sort").ToString.Trim.EndsWith(" DESC") Then
                    ViewState("Sort") = SortExpression + " ASC"
                Else
                    ViewState("Sort") = SortExpression + " DESC"
                End If
            Else
                ViewState("Sort") = SortExpression + " DESC"
            End If
        Catch
            Throw
        End Try
    End Sub
    '****************************gvDisbursement_RowDataBound()************************************'
    ' 1. Highlight row with light blue color/custom color if annuity record exist after retiree death date   
    '*********************************************************************************************'
    Private Sub gvDisbursement_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvDisbursement.RowDataBound
        Dim drv As DataRowView = DirectCast(e.Row.DataItem, DataRowView)
        Try
            HelperFunctions.SetSortingArrows(ViewState("Sort"), e)
            If e.Row.RowType = DataControlRowType.DataRow Then
                If Not String.IsNullOrEmpty(e.Row.Cells(3).Text.ToString()) Then
                    If Convert.ToDateTime(e.Row.Cells(3).Text.ToString) > Convert.ToDateTime(strRetireeDeathDate) Then
                        'If (System.Configuration.ConfigurationSettings.AppSettings("Gridview_Color_Option") Is Nothing Or System.Configuration.ConfigurationSettings.AppSettings("Gridview_Color_Option").ToString.Trim() = "") Then
                        '    'e.Row.BackColor = Drawing.Color.LightBlue
                        '    'lblcolorcode.BackColor = Drawing.Color.LightBlue                           
                        'Else
                        '    '    e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml(System.Configuration.ConfigurationSettings.AppSettings("Gridview_Color_Option"))
                        '    '    lblcolorcode.BackColor = System.Drawing.ColorTranslator.FromHtml(System.Configuration.ConfigurationSettings.AppSettings("Gridview_Color_Option"))
                        '    e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml(System.Configuration.ConfigurationSettings.AppSettings("Gridview_Color_Option"))
                        '    lblcolorcode.BackColor = System.Drawing.ColorTranslator.FromHtml(System.Configuration.ConfigurationSettings.AppSettings("Gridview_Color_Option"))
                        'End If
                        'End If
                        e.Row.CssClass = "BG_Color_DisbursementExist"
                        lblcolorcode.CssClass = "BG_Color_DisbursementExist"
                        lblcolorcode.Visible = True
                        lblDisbAfterDeath.Visible = True
                    End If
                End If
            End If
        Catch ex As Exception
            HelperFunctions.LogException("Void & Transfer Annuity --> gvDisbursement_RowDataBound ", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
        End Try
    End Sub
    '****************************gvDisbursement_Sorting()************************************'
    ' 1. Call Sortgrid procedure
    ' 2. Call Sortgrid procedure
    '*********************************************************************************************'
    Private Sub gvDisbursement_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvDisbursement.Sorting
        Dim dtDisbursement As DataTable
        Try
            If HelperFunctions.isNonEmpty(dsDisbursement) Then
                'Sortgrid(e)
                HelperFunctions.gvSorting(ViewState("Sort"), e.SortExpression, dsDisbursement.Tables(0).AsDataView)
                Bindgrids()
            End If
        Catch ex As Exception
            HelperFunctions.LogException("Void & transfer Annuity --> gvDisbursement_Sorting ", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
        End Try
    End Sub
    '****************************btnClear_Click()*********************************************'
    ' 1. Clear text box controls
    '*********************************************************************************************'
    Private Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear.Click
        Try
            Me.txtFirstName.Text = ""
            Me.txtRetireeFundNo.Text = ""
            Me.txtRetireeLastName.Text = ""
            Me.txtRetireeSSNo.Text = ""
        Catch ex As Exception
            HelperFunctions.LogException("Void & transfer Annuity --> ButtonClear_Click ", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
        End Try
    End Sub
    '****************************EnableControls()*************************************************************************'
    '1. This procedure used to enable/diable page controls based on parameter value
    '2. This procedure used in AddPayee, Cancel, page Laod & SaveandtransferAnnuity method    
    '*********************************************************************************************************************'

    Public Sub EnableControls(ByVal value As Boolean)
        txtSSNo.Enabled = value
        txtFirstName.Enabled = value
        txtLastName.Enabled = value
        txtMiddleName.Enabled = value
        txtSuffix.Enabled = value
        txtBirthDate.Enabled = value
        txtEmail.Enabled = value
        txtTelephone.Enabled = value
        ddlSalutaion.Enabled = value
    End Sub
    '****************************LoadPayee()*************************************************************************'
    '1. This procedure populate existing payee information from pop-up in to payee details using session variable. 
    '2. After populating payee detail session variable is reset
    '2. This procedure used in page load method   
    '****************************************************************************************************************'

    Public Sub LoadPayee()
        Dim dtPayee As DataTable
        Dim dsAddress As New DataSet
        Dim drAddress As DataRow()
        Dim dsContactInfo As New DataSet
        Try
            dtPayee = CType(Session("VTA_PayeeDetails"), DataTable)
            If HelperFunctions.isNonEmpty(dtPayee) Then
                txtSSNo.Text = dtPayee.Rows(0).Item(0).ToString().Trim
                txtFirstName.Text = dtPayee.Rows(0).Item(4).ToString().Trim
                txtMiddleName.Text = dtPayee.Rows(0).Item(5).ToString().Trim
                txtLastName.Text = dtPayee.Rows(0).Item(3).ToString().Trim
                txtSuffix.Text = dtPayee.Rows(0).Item(2).ToString().Trim
                txtBirthDate.Text = dtPayee.Rows(0).Item(10).ToString().Trim
                ddlSalutaion.SelectedValue = dtPayee.Rows(0).Item(1).ToString().Trim
                ViewState("Payee_PersId") = dtPayee.Rows(0).Item(9).ToString().Trim
                dsAddress = Address.GetAddressByEntity(dtPayee.Rows(0).Item(9).ToString().Trim, EnumEntityCode.PERSON)

                If HelperFunctions.isNonEmpty(dsAddress) Then
                    drAddress = dsAddress.Tables(0).Select("isPrimary = True")
                    ucPayeeAddress.LoadAddressDetail(drAddress)
                End If

                dsContactInfo = YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.LookUpAddressInfo(ViewState("Payee_PersId").ToString)

                If HelperFunctions.isNonEmpty(dsContactInfo) Then
                    If dsContactInfo.Tables.Count > 1 Then
                        If dsContactInfo.Tables(1).Rows.Count > 0 Then
                            If (dsContactInfo.Tables(1).Rows(0).Item("EmailAddress").ToString() <> "System.DBNull" And dsContactInfo.Tables(1).Rows(0).Item("EmailAddress").ToString() <> "") Then
                                Me.txtEmail.Text = dsContactInfo.Tables(1).Rows(0).Item("EmailAddress").ToString()
                            End If
                        End If
                    End If
                    If dsContactInfo.Tables.Count > 2 Then
                        If dsContactInfo.Tables(2).Rows.Count > 0 Then
                            If (dsContactInfo.Tables(2).Rows(0).Item("PhoneNumber").ToString() <> "System.DBNull" And dsContactInfo.Tables(2).Rows(0).Item("PhoneNumber").ToString() <> "") Then
                                Me.txtTelephone.Text = dsContactInfo.Tables(2).Rows(0).Item("PhoneNumber").ToString()
                            End If
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            Throw
        Finally
            Session("VTA_PayeeDetails") = Nothing
        End Try
    End Sub
    '****************************clearControls()*********************************************************************'
    '1. This procedure used to clear page controls. 
    '2. This procedure used in button Cancel & Add payee method.   
    '****************************************************************************************************************'

    Public Sub ClearControls()
        txtSSNo.Text = ""
        txtFirstName.Text = ""
        txtMiddleName.Text = ""
        txtLastName.Text = ""
        txtSuffix.Text = ""
        ddlSalutaion.SelectedValue = ""
        txtBirthDate.Text = ""
        txtEmail.Text = ""
        txtTelephone.Text = ""
        ucPayeeAddress.LoadAddressDetail(Nothing)
    End Sub
    '****************************btnAddPayee_Click()************************************'
    ' 1. Set session & page controls to add new Payee
    '***********************************************************************************'

    Private Sub btnAddPayee_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddPayee.Click
        Try
            Session("VTA_PayeeDetails") = Nothing
            btnAddPayee.Enabled = False
            btnSearchPayee.Enabled = False
            btnTransfer.Enabled = True
            btnCancel.Enabled = True
            EnableControls(True)
            ClearControls()
            ucPayeeAddress.EnableControls = True
        Catch ex As Exception
            HelperFunctions.LogException("Void & transfer Annuity --> btnAddPayee_Click ", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
        End Try
    End Sub
    '****************************btnCancel_Click()************************************'
    ' 1. clear session & page controls to cancel any action
    '*********************************************************************************'
    Private Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Try
            btnCancel.Enabled = False
            btnAddPayee.Enabled = True
            btnSearchPayee.Enabled = True
            btnTransfer.Enabled = False
            ClearControls()
            EnableControls(False)
            ucPayeeAddress.EnableControls = False
            Session("VTA_CHECKED_ITEMS") = Nothing
            ViewState("checkBoxArray") = Nothing
            ViewState("Payee_PersId") = Nothing
            ClearSelectedRecords(gvDisbursement, "chkSelect")
            SetDirtyFlag(String.Empty)
        Catch ex As Exception
            HelperFunctions.LogException("Void & transfer Annuity --> btnCancel_Click ", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
        End Try
    End Sub
    '****************************btnTransfer_Click()****************************************'
    ' 1. Check whether logged in user has right to perform void & transfer annuity operation 
    ' 2. Validation for required fields
    ' 3. Message for confirmation
    '***************************************************************************************'

    Private Sub btnTransfer_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnTransfer.Click
        Dim drPayee As DataRow()
        Try

            ' 1. Check whether logged in user has right to perform void & transfer annuity operation 
            Dim checkSecurity As String = SecurityCheck.Check_Authorization("btnTransfer", Convert.ToInt32(Session("LoggedUserKey")))
            If Not checkSecurity.Equals("True") Then
                HelperFunctions.ShowMessageToUser(checkSecurity, EnumMessageTypes.Error, Nothing)
                Exit Sub
            End If
            ' 2. Validation for required fields
            If Validation() Then
                Exit Sub
            End If
            ' 3. Message for confirmation
            'SP 2014.08.25   YRS 5.0-2279  - Start
            'lblMessage1.Text = String.Format(GetMessage("MESSAGE_VTA_CONFIRM_VOID&TRANSFER"), txtLastName.Text.Trim, txtFirstName.Text.Trim)
            Dim dict As New Dictionary(Of String, String)
            dict.Add("LASTNAME", txtLastName.Text.Trim)
            dict.Add("FIRSTNAME", txtFirstName.Text.Trim)
            lblMessage1.Text = MetaMessageBO.GetMessageByTextMessageNo(MetaMessageList.MESSAGE_VTA_CONFIRM_VOID_TRANSFER, dict)
            'SP 2014.08.25   YRS 5.0-2279  - End
            ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "Open", "showDialog('ConfirmDialog','" & lblMessage1.Text & "','NO')", True)


        Catch ex As Exception
            HelperFunctions.LogException("Void & transfer Annuity --> btnTransfer_Click ", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
        End Try
    End Sub
    '****************************Validation()****************************************'
    ' 1. Validation to select atleast one disbursement for void and transfer.
    ' 2. Validation to enter 9-digit SSN.
    ' 3. Validation to enter First Name of Payee.
    ' 4. Validation to enter Last Name of Payee.
    ' 5. Validation to ener valid Birth Date of Payee.
    ' 6. Validation to ener Payee address.
    ' 7. This procedure is used in btnTransfer_Click() method.
    '***************************************************************************************'

    Public Function Validation() As Boolean
        Dim blnValidate As Boolean
        Dim strSSN As String
        Dim blnSeleted As Boolean
        Dim dtmBirthdate As DateTime
        Try
            blnValidate = False


            blnSeleted = GetSelectedRecords(gvDisbursement)
            ' 1. Validation to select atleast one disbursement for void and transfer.
            If (blnSeleted = False) Then
                'SP 2014.08.25   YRS 5.0-2279  - Start
                'HelperFunctions.ShowMessageToUser(GetMessage("MESSAGE_VTA_SELECT_DISBURSEMENT"), EnumMessageTypes.Error, Nothing)
                HelperFunctions.ShowMessageToUser(MetaMessageList.MESSAGE_VTA_SELECT_DISBURSEMENT)
                'SP 2014.08.25   YRS 5.0-2279  - End
                blnValidate = True
            End If

            ' 2. Validation to enter 9-digit SSN.
            txtSSNo.Text = txtSSNo.Text.Trim.Replace("-", "")
            If txtSSNo.Text.Length <> 9 Then
                'SP 2014.08.25   YRS 5.0-2279  - Start
                'HelperFunctions.ShowMessageToUser(GetMessage("MESSAGE_VTA_VALID_SSN"), EnumMessageTypes.Error, Nothing)
                HelperFunctions.ShowMessageToUser(MetaMessageList.MESSAGE_VTA_VALID_SSN)
                'SP 2014.08.25   YRS 5.0-2279  - End
                blnValidate = True
            End If

            ' 3. Validation to enter First Name of Payee.
            If txtFirstName.Text.Trim = "" Then
                'SP 2014.08.25   YRS 5.0-2279  - Start
                'HelperFunctions.ShowMessageToUser(GetMessage("MESSAGE_VTA_ENTER_FIRST_NAME"), EnumMessageTypes.Error, Nothing)
                HelperFunctions.ShowMessageToUser(MetaMessageList.MESSAGE_VTA_ENTER_FIRST_NAME)
                'SP 2014.08.25   YRS 5.0-2279  - End
                blnValidate = True
            End If

            ' 4. Validation to enter Last Name of Payee.
            If txtLastName.Text.Trim = "" Then
                'SP 2014.08.25   YRS 5.0-2279  - Start
                'HelperFunctions.ShowMessageToUser(GetMessage("MESSAGE_VTA_ENTER_LAST_NAME"), EnumMessageTypes.Error, Nothing)
                HelperFunctions.ShowMessageToUser(MetaMessageList.MESSAGE_VTA_ENTER_LAST_NAME)
                'SP 2014.08.25   YRS 5.0-2279  - End
                blnValidate = True
            End If

            ' 5. Validation to ener valid Birth Date of Payee.
            If txtBirthDate.Text.trim <> "" Then
                dtmBirthdate = Convert.ToDateTime(txtBirthDate.Text)
            End If

            If txtBirthDate.Text = "" Then
                'SP 2014.08.25   YRS 5.0-2279  - Start
                'HelperFunctions.ShowMessageToUser(GetMessage("MESSAGE_VTA_ENTER_BIRTH_DATE"), EnumMessageTypes.Error, Nothing)
                HelperFunctions.ShowMessageToUser(MetaMessageList.MESSAGE_VTA_ENTER_BIRTH_DATE)
                'SP 2014.08.25   YRS 5.0-2279  - End
                blnValidate = True
            ElseIf (dtmBirthdate > System.DateTime.Today) Then
                'SP 2014.08.25   YRS 5.0-2279  - Start
                'HelperFunctions.ShowMessageToUser(GetMessage("MESSAGE_VTA_FUTURE_BIRTH_DATE"), EnumMessageTypes.Error, Nothing)
                HelperFunctions.ShowMessageToUser(MetaMessageList.MESSAGE_VTA_FUTURE_BIRTH_DATE)
                'SP 2014.08.25   YRS 5.0-2279  - End
                blnValidate = True
            End If


            ' 6. Validation to ener Payee address.
            If ucPayeeAddress.ValidateAddress <> String.Empty Then
                HelperFunctions.ShowMessageToUser(ucPayeeAddress.ValidateAddress, EnumMessageTypes.Error, Nothing)
                blnValidate = True
            End If


            If ucPayeeAddress.DropDownListCountryValue = "US" Or ucPayeeAddress.DropDownListCountryValue = "CA" Then
                'START: PPP | 2015.10.13 | YRS-AT-2588 | New validation rules applied and also error message is retrieved from AtsMetaMessages
                'If txtTelephone.Text.Trim().Length <> 10 And txtTelephone.Text.Trim().Length > 0 Then
                '    'SP 2014.08.25   YRS 5.0-2279  - Start
                '    'HelperFunctions.ShowMessageToUser(GetMessage("MESSAGE_VTA_TELEPHONE_LENGTH"), EnumMessageTypes.Error, Nothing)
                '    HelperFunctions.ShowMessageToUser(MetaMessageList.MESSAGE_VTA_TELEPHONE_LENGTH)
                '    'SP 2014.08.25   YRS 5.0-2279  - End
                '    blnValidate = True
                'End If

                If txtTelephone.Text.Trim().Length > 0 Then
                    'START: CC | 2016.02.18 | YRS-AT-1483 | Changed the namespace
                    'If (Not CommonUtilities.Validation.Telephone(txtTelephone.Text.Trim())) Then
                    If (Not YMCARET.CommonUtilities.Validation.Telephone(txtTelephone.Text.Trim())) Then
                        'END: CC | 2016.02.18 | YRS-AT-1483 | Changed the namespace
                        HelperFunctions.ShowMessageToUser(YMCAObjects.MetaMessageList.MESSAGE_GEN_TELEPHONE_ERROR)
                        blnValidate = True
                    End If
                End If
                'END: PPP | 2015.10.13 | YRS-AT-2588 | New validation rules applied and also error message is retrieved from AtsMetaMessages
            End If

            'SP 2014.08.25   YRS 5.0-2279  - Start
            '7. Validation for email
            If Not (String.IsNullOrEmpty(txtEmail.Text)) Then
                Dim isEmail As Boolean = Regex.IsMatch(txtEmail.Text.Trim(), "\A(?:[A-Za-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[A-Za-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[A-Za-z0-9](?:[A-Za-z0-9-]*[A-Za-z0-9])?\.)+[A-Za-z0-9](?:[A-Za-z0-9-]*[A-Za-z0-9])?)\Z")
                If Not isEmail Then
                    HelperFunctions.ShowMessageToUser(MetaMessageList.MESSAGE_VTA_INVALID_EMAILID)
                    blnValidate = True
                End If
            End If
            'SP 2014.08.25   YRS 5.0-2279  - End

            Return blnValidate
        Catch ex As Exception
            Throw
        End Try
    End Function
    '****************************getSelectedRecords()****************************************'
    ' 1. This procedure is used to check at least one disbursement is selected from grid view.  
    ' 2. This procedure is used in btnTransfer_Click() method.
    '***************************************************************************************'

    Private Function GetSelectedRecords(ByVal gv As GridView) As Boolean
        Dim lstDisbursement As New List(Of String)
        Dim blnSelected As Boolean = False
        Dim chk As New CheckBox
        Dim i As Integer = 0
        Try
            For i = 0 To gv.Rows.Count - 1
                chk = gv.Rows(i).FindControl("chkSelect")
                If Not IsNothing(chk) Then
                    If chk.Checked = True Then
                        blnSelected = True
                        Exit For
                    End If
                End If
            Next
            Return blnSelected
        Catch
            Throw
        End Try
    End Function
    '****************************SaveandTransferAnnuity()****************************************'
    ' 1. Get selected disbursemrnt from grid view. 
    ' 2. Get Address details
    ' 3. If payee already exist get Persid else create new persid
    ' 4. If address already exist get addressid else save and get new address id.
    ' 5. Call BAL to Save and Transfer Annuity.
    ' 6. Show message returned from BAL layer.
    ' 7. Refresh disbursements
    ' 8. This procedure is used in btnYes_Click() method. 
    '****************************************************************************************'

    Public Sub SaveandTransferAnnuity()
        Dim dsPayeeAddress As New DataSet
        Dim intCheckSSN As Integer
        Dim lstDisbursementIds As List(Of Dictionary(Of String, String))
        Dim dtAddress As New DataTable
        Dim strAddressId As String
        Dim blnPayeeExist As Boolean
        Dim strPersId As String
        Dim strResult As String
        Try
            ' 1. Get selected disbursemrnt from grid view. 
            lstDisbursementIds = GetSelectedRecords(gvDisbursement, "chkSelect")

            If lstDisbursementIds.Count > 0 Then
                ' 2. Get Address details
                ucPayeeAddress.EntityCode = "PERSON"
                ucPayeeAddress.AddrCode = "HOME"

                dtAddress = ucPayeeAddress.GetAddressTable()
                dsPayeeAddress.Tables.Add(dtAddress)

                ' 3. If payee already exist get Persid else create new persid
                intCheckSSN = YMCARET.YmcaBusinessObject.VoidAndTransferAnnuityBOClass.CheckForExistingSSN(txtSSNo.Text.Trim)

                blnPayeeExist = IIf(intCheckSSN <= 0, False, True)
                If blnPayeeExist Then
                    strPersId = ViewState("Payee_PersId")
                Else
                    strPersId = Guid.NewGuid().ToString
                End If

                ' 4. If address already exist get addressid else save and get new address id.
                'Start:SR:2014.02.06 : Add guiEntityID in address table for update & Insert.
                If HelperFunctions.isNonEmpty(dtAddress) Then
                    dtAddress.Rows(0).Item(1) = strPersId
                    strAddressId = Address.SaveAddress(dtAddress)
                Else
                    strAddressId = ucPayeeAddress.UniqueId
                End If
                'End:SR:2014.02.06 : Add guiEntityID in address table for update & Insert.

                ' 5. Call BAL to Save and Transfer Annuity.
                strResult = YMCARET.YmcaBusinessObject.VoidAndTransferAnnuityBOClass.SaveandTransferAnnuity(strPersId, txtSSNo.Text.Trim, txtLastName.Text.Trim, txtFirstName.Text.Trim, txtMiddleName.Text.Trim, ddlSalutaion.SelectedItem.Text, txtSuffix.Text, txtBirthDate.Text.ToString.Trim, "S", lstDisbursementIds, strAddressId, blnPayeeExist, txtEmail.Text.Trim, txtTelephone.Text.Trim)

                ' 6. Show message returned from BAL layer.
                If Not String.IsNullOrEmpty(strResult) Then
                    HelperFunctions.ShowMessageToUser(strResult, EnumMessageTypes.Information, Nothing)
                Else
                    'SP 2014.08.25   YRS 5.0-2279  - Start
                    Dim dict As New Dictionary(Of String, String)
                    dict.Add("LASTNAME", txtLastName.Text.Trim)
                    dict.Add("FIRSTNAME", txtFirstName.Text.Trim)
                    'HelperFunctions.ShowMessageToUser(String.Format(GetMessage("MESSAGE_VTA_TRANSFERED_SUCCESSFULLY"), txtLastName.Text.Trim, txtFirstName.Text.Trim), EnumMessageTypes.Success, Nothing)
                    HelperFunctions.ShowMessageToUser(MetaMessageList.MESSAGE_VTA_TRANSFERED_SUCCESSFULLY, Nothing, dict)
                    'SP 2014.08.25   YRS 5.0-2279  - End
                End If

                ' 7. Refresh disbursements
                PopulateDisbursement()
                ucPayeeAddress.EnableControls = False
                btnTransfer.Enabled = False
                btnCancel.Enabled = False
                ClearControls()
                EnableControls(False)
                SetDirtyFlag(String.Empty)
            End If
        Catch ex As Exception
            Throw
        End Try
    End Sub
    '****************************txtSSNo_TextChanged()****************************************'
    ' 1. validation for 9-digit SSN.
    ' 2. Validation for same Retiree and Payee SSN.
    ' 3. Validation for SSN already exist.
    ' 4. Enable/Disable tansfer button based on validation.
    '*****************************************************************************************'
    Private Sub txtSSNo_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtSSNo.TextChanged
        Dim intCheckSSN As Integer
        Dim blnValidate As Boolean
        Try
            blnValidate = False
            ' 1. validation for 9-digit SSN.
            txtSSNo.Text = txtSSNo.Text.Trim.Replace("-", "")
            If txtSSNo.Text.Length <> 9 Then
                'SP 2014.08.25   YRS 5.0-2279  - Start
                HelperFunctions.ShowMessageToUser(MetaMessageList.MESSAGE_VTA_VALID_SSN)
                'SP 2014.08.25   YRS 5.0-2279  - End
                EnableDisabledTransferButton(False)
                Exit Sub
            End If
            ' 2. Validation for same Retiree and Payee SSN.
            If txtSSNo.Text.Trim.Equals(ViewState("Participaint_SSN").ToString.Trim) Then
                'SP 2014.08.25   YRS 5.0-2279  - Start
                HelperFunctions.ShowMessageToUser(MetaMessageList.MESSAGE_VTA_SAME_SSN)
                'SP 2014.08.25   YRS 5.0-2279  - End
                EnableDisabledTransferButton(False)
                Exit Sub
            End If
            ' 3. Validation for SSN already exist.
            intCheckSSN = YMCARET.YmcaBusinessObject.VoidAndTransferAnnuityBOClass.CheckForExistingSSN(txtSSNo.Text.Trim)
            If intCheckSSN > 0 Then
                'SP 2014.08.25   YRS 5.0-2279  - Start
                HelperFunctions.ShowMessageToUser(MetaMessageList.MESSAGE_VTA_SSN_EXIST)
                'SP 2014.08.25   YRS 5.0-2279  - End
                EnableDisabledTransferButton(False)
                Exit Sub
            End If
            EnableDisabledTransferButton(True)

        Catch ex As Exception
            HelperFunctions.LogException("Void & transfer Annuity --> txtSSNo_TextChanged ", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
        End Try
    End Sub
    'SP 2014.09.09 : YRS 5.0-2279 -Start
    Private Sub EnableDisabledTransferButton(ByVal bIsEnabled As Boolean)
        ' 4. Enable/Disable tansfer button based on validation.
        btnTransfer.Enabled = bIsEnabled
    End Sub
    'SP 2014.09.09 : YRS 5.0-2279 -End

    '****************************btnSearchPayee_Click()****************************************'
    ' 1. Enable/Disable buttons.   
    '******************************************************************************************'
    Private Sub btnSearchPayee_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearchPayee.Click
        Try
            btnAddPayee.Enabled = False
            btnSearchPayee.Enabled = False
            btnTransfer.Enabled = True
            btnCancel.Enabled = True
        Catch ex As Exception
            HelperFunctions.LogException("Void & transfer Annuity --> btnSearchPayee_Click ", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
        End Try
    End Sub
    '****************************btnClose_Click()**********************************************'
    ' 1. Clear all session & viewstate before leaving page
    ' 2. Go to main page
    '******************************************************************************************'
    Private Sub btnClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Try
            Session("VTA_dsSearchResults") = Nothing
            Session("VTA_dsDisbursement") = Nothing
            Session("VTA_CHECKED_ITEMS") = Nothing
            Session("VTA_PayeeDetails") = Nothing
            ViewState("VTA_PersId") = Nothing
            ViewState("VTA_Deathdate") = Nothing
            ViewState("checkBoxArray") = Nothing
            ViewState("Payee_PersId") = Nothing
            ViewState("Participaint_SSN") = Nothing
            ViewState("Sort") = Nothing
            ViewState("PageIndex") = Nothing
            ViewState("previousSearchSortExpression") = Nothing
            ClearSelectedRecords(gvDisbursement, "chkSelect")
            Response.Redirect("MainWebForm.aspx", False)
        Catch ex As Exception
            HelperFunctions.LogException("Void & transfer Annuity --> btnClose_Click ", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
        End Try
    End Sub
    '****************************getSelectedRecords()******************************************************'
    ' 1. This procedure is used to save disbursement id of selected records from grid view in list control.  
    ' 2. This procedure is used in SaveandTransferAnnuity() procedure.
    '*******************************************************************************************************'

    Private Function GetSelectedRecords(ByVal gv As GridView, ByVal chkName As String) As List(Of Dictionary(Of String, String))
        Dim lDisbursementIds As New List(Of Dictionary(Of String, String))
        Dim chk As New CheckBox
        Dim dt As DataTable
        Dim dr As DataRow()
        Try
            Dim i As Integer = 0
            For i = 0 To gv.Rows.Count - 1
                chk = gv.Rows(i).FindControl(chkName)
                If Not IsNothing(chk) Then
                    If chk.Checked Then
                        lDisbursementIds.Add(New Dictionary(Of String, String)() From { _
                        {"DisbId", gv.DataKeys(i).Value.ToString()}, {"boolvalue", chk.Checked.ToString()}})
                    End If
                End If

            Next
            Return lDisbursementIds
        Catch
            Throw
        End Try
    End Function
    '****************************SaveCheckedValues()***********************************************************************************************************'
    ' 1. Save chkSelectAll checkbox value from each page index in viewstate & selected records(Checkbos selected) from each page index of grid view in session.  
    ' 2. This procedure is used in gvDisbursement_PageIndexChanging() method.
    '**********************************************************************************************************************************************************'

    Private Sub SaveCheckedValues()
        Dim userdetails As New ArrayList()
        Dim index As String = "-1"
        ' For select all
        Dim CheckBoxIndex As Integer
        Dim CheckAllWasChecked As Boolean = False
        If ViewState("checkBoxArray") IsNot Nothing Then
            checkBoxArray = DirectCast(ViewState("checkBoxArray"), ArrayList)
        Else
            checkBoxArray = New ArrayList()
        End If

        Dim chkAll As New CheckBox
        chkAll = DirectCast(gvDisbursement.HeaderRow.Cells(0).FindControl("chkSelectAll"), CheckBox)
        Dim checkAllIndex As String = "chkAll-" & gvDisbursement.PageIndex

        If chkAll.Checked Then
            If checkBoxArray.IndexOf(checkAllIndex) = -1 Then
                checkBoxArray.Add(checkAllIndex)
            End If
        Else
            If checkBoxArray.IndexOf(checkAllIndex) <> -1 Then
                checkBoxArray.Remove(checkAllIndex)
                CheckAllWasChecked = True
            End If
        End If
        ViewState("checkBoxArray") = checkBoxArray
        'end,  For select all


        For Each gvrow As GridViewRow In gvDisbursement.Rows
            index = gvDisbursement.DataKeys(gvrow.RowIndex).Value
            Dim result As Boolean = DirectCast(gvrow.FindControl("chkSelect"), CheckBox).Checked  ' Check in the Session 
            If Session("VTA_CHECKED_ITEMS") IsNot Nothing Then
                userdetails = DirectCast(Session("VTA_CHECKED_ITEMS"), ArrayList)
            End If
            If result Then
                If Not userdetails.Contains(index) Then
                    userdetails.Add(index)
                End If
            Else
                userdetails.Remove(index)
            End If
        Next
        If userdetails IsNot Nothing AndAlso userdetails.Count > 0 Then
            Session("VTA_CHECKED_ITEMS") = userdetails
        End If
    End Sub
    '****************************PopulateCheckedValues()************************************************************************************************************'
    ' 1. This procedure Select chkSelectAll checkbox from viewstate and chkSelect checkbox from session after data grid is binded after sorting / page index changes
    ' 2. This procedure used in gvDisbursement_PageIndexChanging method 
    '****************************************************************************************************************************************************************'

    Private Sub PopulateCheckedValues()
        'For Select All check box
        Dim checkBoxArray As ArrayList = DirectCast(ViewState("checkBoxArray"), ArrayList)
        Dim checkAllIndex As String = "chkAll-" & gvDisbursement.PageIndex

        If checkBoxArray.IndexOf(checkAllIndex) <> -1 Then
            Dim chkAll As CheckBox = DirectCast(gvDisbursement.HeaderRow.Cells(0).FindControl("chkSelectAll"), CheckBox)
            chkAll.Checked = True
        End If
        'end, For Select All check box

        Dim userdetails As ArrayList = DirectCast(Session("VTA_CHECKED_ITEMS"), ArrayList)
        If userdetails IsNot Nothing AndAlso userdetails.Count > 0 Then
            For Each gvrow As GridViewRow In gvDisbursement.Rows
                Dim index As String = gvDisbursement.DataKeys(gvrow.RowIndex).Value
                If userdetails.Contains(index) Then
                    Dim myCheckBox As CheckBox = DirectCast(gvrow.FindControl("chkSelect"), CheckBox)
                    myCheckBox.Checked = True
                End If
            Next
        End If
    End Sub
    '****************************ClearSelectedRecords()************************************************************************************************************'
    ' 1. Clear all checkboxes checked in disbursement grid
    ' 2. This procedure used in btnClose_Click, btnCancel_Click method 
    '****************************************************************************************************************************************************************'
    Private Sub ClearSelectedRecords(ByVal gv As GridView, ByVal chkName As String)
        Dim chk As New CheckBox
        Dim dt As DataTable
        Dim dr As DataRow()
        Dim i As Integer = 0
        Dim chkAll As CheckBox
        Try
            If gvDisbursement.Rows.Count > 0 Then
                chkAll = DirectCast(gvDisbursement.HeaderRow.Cells(0).FindControl("chkSelectAll"), CheckBox)
                chkAll.Checked = True
                For i = 0 To gv.Rows.Count - 1
                    chk = gv.Rows(i).FindControl(chkName)
                    If Not IsNothing(chk) Then
                        If chk.Checked Then
                            chk.Checked = False
                        End If
                    End If
                Next
            End If

        Catch
            Throw
        End Try
    End Sub
    '****************************btnYes_Click()****************************************'
    ' 1. Call SaveandTransferAnnuity() procdure to void and transfer annuity.
    ' 2. close jquery pop-up.
    '******************************************************************************************'
    Private Sub btnYes_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnYes.Click
        Try
            SaveandTransferAnnuity()
            ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "close", "closeDialog('ConfirmDialog')", True)
        Catch ex As Exception
            HelperFunctions.LogException("Void & transfer Annuity --> btnYes_Click ", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
        End Try
    End Sub
    '****************************btnNo_Click()****************************************'
    ' 1. close jquery pop-up.
    '******************************************************************************************'
    Private Sub btnNo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNo.Click
        Try
            ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "close", "closeDialog('ConfirmDialog')", True)
        Catch ex As Exception
            HelperFunctions.LogException("Void & transfer Annuity --> btnNo_Click ", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
        End Try
    End Sub
    '****************************btnOK_Click()****************************************'
    ' 1. close jquery pop-up.
    '******************************************************************************************'
    Private Sub btnOK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOK.Click
        Try
            ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "close", "closeDialog('divMessage')", True)
        Catch ex As Exception
            HelperFunctions.LogException("Void & transfer Annuity --> btnOK_Click ", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
        End Try
    End Sub
    'SP 2014.08.25   YRS 5.0-2279  - Start
    '****************************GetMessage()****************************************'
    ' 1. Get message from rsource file based on resource Key.
    '********************************************************************************'
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
    'SP 2014.08.25   YRS 5.0-2279  - End

    '**********************************gvSearchRetiree_SelectedIndexChanged()***********************'
    ' 1. Load session,viewsatae & datarow variables based on selected retiree from datagrid
    ' 2. Validation for archive data
    ' 3. call tabstrip selected index change event
    ' 4. Bind datagrid with deceased retiree 
    '***********************************************************************************************'
    Private Sub gvSearchRetiree_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvSearchRetiree.SelectedIndexChanged
        Dim dr As DataRow()
        Try

            '1 Clear disbursement Tab
            ViewState("Sort") = Nothing
            ViewState("PageIndex") = Nothing
            Session("VTA_CHECKED_ITEMS") = Nothing
            Session("VTA_PayeeDetails") = Nothing
            Session("VTA_dsDisbursement") = Nothing
            ViewState("VTA_PersId") = Nothing
            ViewState("VTA_Deathdate") = Nothing
            ViewState("checkBoxArray") = Nothing
            ViewState("Payee_PersId") = Nothing
            ViewState("Participaint_SSN") = Nothing
            ClearControls()

            ' 1. Load session,viewsatae & datarow variables based on selected retiree from datagrid
            btnAddPayee.Enabled = True
            btnSearchPayee.Enabled = True
            btnTransfer.Enabled = False
            btnCancel.Enabled = False
            ViewState("Participaint_SSN") = Me.gvSearchRetiree.SelectedRow.Cells(1).Text.Trim
            strRetireePersId = Me.gvSearchRetiree.SelectedRow.Cells(10).Text.Trim

            strRetireeDeathDate = IIf(String.IsNullOrEmpty(Me.gvSearchRetiree.SelectedRow.Cells(11).Text.Trim), "", Me.gvSearchRetiree.SelectedRow.Cells(11).Text.Trim)
            lblDisbAfterDeath.Text = "Annuity exist after retiree death(" & strRetireeDeathDate & ")"
            dr = dsSearchResults.Tables(0).Select("[SSN]= '" & ViewState("Participaint_SSN").ToString.Trim & "'")

            ' 2. Validation for archive data
            If dr(0).Item("IsArchived") = True Then
                ''SP 2014.08.25   YRS 5.0-2279  - Start
                'HelperFunctions.ShowMessageToUser(GetMessage("MESSAGE_VTA_ARCHIVED_DATA"), EnumMessageTypes.Error, Nothing)
                HelperFunctions.ShowMessageToUser(MetaMessageList.MESSAGE_VTA_ARCHIVED_DATA)
                'SP 2014.08.25   YRS 5.0-2279  - End
                Me.tabStripVoidandTransferAnnuity.Items(1).Enabled = False
                Me.Form.DefaultButton = btnFind.UniqueID
                Exit Sub
            End If

            ' 3. call tabstrip selected index change event
            Me.tabStripVoidandTransferAnnuity.Items(1).Enabled = True
            tabStripVoidandTransferAnnuity.SelectedIndex = 1
            TabStripVoidandTransferAnnuity_SelectedIndexChange(Me, Nothing)

            ' 4. Bind disbursement grid
            PopulateDisbursement()

        Catch ex As Exception
            HelperFunctions.LogException("Void & transfer Annuity --> gvSearchRetiree_SelectedIndexChanged ", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
        End Try
    End Sub


    '**********************************gvSearchRetiree_SortCommand()***********************'
    ' 1. Sort deacease retiree dataset based on sort expression
    ' 2. copy sort expression in viewstate
    ' 3. Bind deaceased retiree datagrid with sorted dataset
    '****************************************************************************************'
    Private Sub gvSearchRetiree_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvSearchRetiree.Sorting
        Try
            Dim gvSort As GridViewCustomSort ' 'SP 2014.08.27   YRS 5.0-2279  
            gvSearchRetiree.SelectedIndex = -1
            If HelperFunctions.isNonEmpty(dsSearchResults) Then
                HelperFunctions.gvSorting(ViewState("previousSearchSortExpression"), e.SortExpression, dsSearchResults.Tables(0).AsDataView)
                'SP 2014.08.27   YRS 5.0-2279  - Start
                'If Not ViewState("previousSearchSortExpression") Is Nothing AndAlso ViewState("previousSearchSortExpression") <> "" Then
                'If SortExpression is not the same as the previous one then initialize new one
                'If (e.SortExpression <> ViewState("previousSearchSortExpression")) Then
                '    ViewState("previousSearchSortExpression") = e.SortExpression
                '    dsSearchResults.Tables(0).DefaultView.Sort = e.SortExpression + " ASC"
                'Else
                '    'else toggle existing sort expression
                '    dsSearchResults.Tables(0).DefaultView.Sort = IIf(dsSearchResults.Tables(0).DefaultView.Sort.EndsWith("ASC") = True, e.SortExpression + " DESC", e.SortExpression + " ASC")
                'End If
                'Else
                '    'First time in the sort function
                '    dsSearchResults.Tables(0).DefaultView.Sort = e.SortExpression + " ASC"
                '    ViewState("previousSearchSortExpression") = e.SortExpression
                'End If
                If Not ViewState("previousSearchSortExpression") Is Nothing Then
                    gvSort = ViewState("previousSearchSortExpression")
                    dsSearchResults.Tables(0).DefaultView.Sort = gvSort.SortExpression + " " + gvSort.SortDirection

                End If
                'SP 2014.08.27   YRS 5.0-2279  - End
                ' Bind deaceased retiree datagrid with sorted dataset
                HelperFunctions.BindGrid(gvSearchRetiree, dsSearchResults.Tables(0).DefaultView)

                'Set selected Row
                SetSelectedRow(dsSearchResults.Tables(0).DefaultView)

            End If
        Catch ex As Exception
            HelperFunctions.LogException("Void & transfer Annuity --> gvSearchRetiree_SortCommand ", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
        End Try
    End Sub
    '**********************************btnCloseList_Click()**********************************'
    ' 1. Clear all viewstate & session before leaving page.
    ' 1. Go to Main page on Close button click.
    '****************************************************************************************'
    Private Sub btnCloseList_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCloseList.Click
        Try
            Session("VTA_dsSearchResults") = Nothing
            Session("VTA_dsDisbursement") = Nothing
            Session("VTA_CHECKED_ITEMS") = Nothing
            Session("VTA_PayeeDetails") = Nothing
            ViewState("VTA_PersId") = Nothing
            ViewState("VTA_Deathdate") = Nothing
            ViewState("checkBoxArray") = Nothing
            ViewState("Payee_PersId") = Nothing
            ViewState("Participaint_SSN") = Nothing
            ViewState("Sort") = Nothing
            ViewState("PageIndex") = Nothing
            ViewState("previousSearchSortExpression") = Nothing
            Response.Redirect("MainWebForm.aspx", False)
        Catch ex As Exception
            HelperFunctions.LogException("Void & transfer Annuity --> btnCloseList_Click ", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
        End Try
    End Sub

    Private Sub VoidAndTransferAnnuity_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        Dim LabelModuleName As Label
        Try
            If tabStripVoidandTransferAnnuity.SelectedIndex = 1 Then
                LabelModuleName = Master.FindControl("LabelModuleName")
                If Not LabelModuleName Is Nothing Then
                    LabelModuleName.Text += " - Fund Id " + Me.gvSearchRetiree.SelectedRow.Cells(9).Text.Trim + " - " + Me.gvSearchRetiree.SelectedRow.Cells(2).Text.Trim + ", " + Me.gvSearchRetiree.SelectedRow.Cells(3).Text.Trim
                End If
            End If
        Catch ex As Exception
            HelperFunctions.LogException("Void & transfer Annuity --> btnCloseList_Click ", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
        End Try
    End Sub

    Public Sub SetDirtyFlag(ByVal flag As String)
        Dim hdnDirty As HiddenField
        Try
            hdnDirty = Master.FindControl("HiddenFieldDirty")
            If Not hdnDirty Is Nothing Then
                hdnDirty.Value = flag
            End If
        Catch ex As Exception
            Throw
        End Try
    End Sub
    Public Sub SetSelectedRow(ByVal dv As DataView)
        Dim i As Integer
        Try
            If HelperFunctions.isNonEmpty(dv) Then
                If gvSearchRetiree.Rows.Count > 0 Then
                    If Not ViewState("Participaint_SSN") Is Nothing AndAlso ViewState("Participaint_SSN") <> "" Then
                        gvSearchRetiree.SelectedIndex = -1
                        For i = 0 To gvSearchRetiree.Rows.Count - 1
                            If gvSearchRetiree.Rows(i).Cells(1).Text.Trim = ViewState("Participaint_SSN") Then
                                gvSearchRetiree.SelectedIndex = i
                                Exit For
                            End If
                        Next
                    End If
                End If
            End If
        Catch ex As Exception
            Throw
        End Try

    End Sub
End Class
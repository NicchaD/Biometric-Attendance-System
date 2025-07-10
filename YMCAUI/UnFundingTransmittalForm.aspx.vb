'*******************************************************************************
' Copyright YMCA Retirement Fund All Rights Reserved. 
'
' Project Name		:	YMCA Yrs
' FileName			:	UnFundingTransmittalForm.aspx.vb

' Author Name		:	Anil
' Employee ID		:	
' Email				:	
' Contact No		:	

'History
'********************************************************************************************************************************
'Changed by     Changed on       Change Description
'********************************************************************************************************************************
'Swopna         24June08
'Swopna         11Jul08          Dynamically change display on deleting transmittals. 
'Swopna         21Jul08          'Object reference not set to an instance of an object' error
'Swopna         22Jul08          Avoid repeated insertion of same record in Datagrid_UnFundDelete_TranList
'Swopna         06Aug08          On Deleting either AE or AY transmittal,remove associated AE/AY transmittal from datagrid if present.
'Swopna         11Aug08          Delete associated AE/AY transmittal only if it does not have any applied receipts/credits.
'Paramesh K.    19Sept2008       Added validation for checking UEIN Transmittal exists before unfunding
'Neeraj Singh   12/Nov/2009      Added form name for security issue YRS 5.0-940 
'Priya          2010-07-02       Changes made for enhancement in vs-2010 
'Manthan Rajguru 2015.09.16      YRS-AT-2550: YRS data cleanup: copyright comments and namespace/reference (Changed the Namespace/reference from 'Infotech' to 'YMCARET')
'Shilpa N       02/22/2019       YRS-AT-4248- YRS enh: YRS read-only access during MonthEnd Processing (TrackIT 35547) 

'*******************************************************************************
Public Class UnFundingTransmittalForm
    Inherits System.Web.UI.Page
    'below line is Added by Neeraj for issue id YRS 5.0-940 
    'START : Shilpa N | 02/22/2019 | YRS-AT-4248 | Commented Existing code. It was passing  wrong form name for check security code, Declare variable to use across the page.
    'Dim strFormName As String = New String("UnFundingTransmittalForm.aspx")
    Dim strFormName
    Dim paramQuerystring
    'END : Shilpa N | 02/22/2019 | YRS-AT-4248 |Commented Existing code. It was passing  wrong form name for check security code, Declare variable to use across the page.

    'End issue id YRS 5.0-940

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents Menu1 As skmMenu.Menu
    Protected WithEvents ButtonOk As System.Web.UI.WebControls.Button
    Protected WithEvents LabelRecordNotFound As System.Web.UI.WebControls.Label
    Protected WithEvents LabelYmcaNo As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxYmcaNo As System.Web.UI.WebControls.TextBox
    Protected WithEvents ButtonFind As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonClear As System.Web.UI.WebControls.Button
    Protected WithEvents LabelTransmittalNo As System.Web.UI.WebControls.Label
    Protected WithEvents LabelTransmittalSDate As System.Web.UI.WebControls.Label
    Protected WithEvents LabelTransmittalEDate As System.Web.UI.WebControls.Label
    Protected WithEvents LabelReceiptNo As System.Web.UI.WebControls.Label
    Protected WithEvents TextboxRecptNo As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxSTransmittalDate As YMCAUI.DateUserControl
    Protected WithEvents TextboxETransmittalDate As YMCAUI.DateUserControl
    Protected WithEvents ButtonAddToList As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonUnFund As System.Web.UI.WebControls.Button
    Protected WithEvents TextBoxTransmittalNo As System.Web.UI.WebControls.TextBox
    Protected WithEvents DataGridUnFundDelSearchResult As System.Web.UI.WebControls.DataGrid
    Protected WithEvents Datagrid_UnFundDelete_TranList As System.Web.UI.WebControls.DataGrid
    Protected WithEvents PlaceHolder1 As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents LabelLastRunDateTime As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxLastRunDateTime As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelStatus As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxStatus As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelCurrentMode As System.Web.UI.WebControls.Label
    Protected WithEvents DropdownlistCurrentMode As System.Web.UI.WebControls.DropDownList
    Protected WithEvents LabelDescription As System.Web.UI.WebControls.Label
    Protected WithEvents TextboxDescription As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelStartTime As System.Web.UI.WebControls.Label
    Protected WithEvents TextboxStartTime As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextboxScheduler As System.Web.UI.WebControls.TextBox
    Protected WithEvents ButtonSave As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonReports As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonDelete As System.Web.UI.WebControls.Button
    'Swopna
    Protected WithEvents LabelTitle As System.Web.UI.WebControls.Label
    Protected WithEvents LabelSelectedList As System.Web.UI.WebControls.Label
    Protected WithEvents DataGridUnFundSearchResult As System.Web.UI.WebControls.DataGrid
    'Swopna

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region
    'Swopna -18June08 -Start
#Region "properties"
    Private Property FunctionType() As Integer
        Get
            If Not Session("FunctionType") Is Nothing Then
                Return (DirectCast(Session("FunctionType"), Integer))
            Else
                Return String.Empty
            End If

        End Get
        Set(ByVal Value As Integer)
            Session("FunctionType") = Value
        End Set
    End Property
    'Swopna -18June08 -End
#End Region

    Public Enum enumMessageBoxType
        Javascript = 0
        DotNet = 1
    End Enum
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        Try
            Set_Attributes()
            Me.Menu1.DataSource = Server.MapPath("SimpleXML.xml")
            Menu1.DataBind()

            If Session("LoggedUserKey") Is Nothing Then
                Response.Redirect("Login.aspx", False)
            End If

            'START : Shilpa N | 02/22/2019 | YRS-AT-4248 | Passing strFormName depends on the querystring.  
            If Not Page.Request.QueryString("Process") Is Nothing Then
                paramQuerystring = CType(Page.Request.QueryString("Process"), Integer)
                If paramQuerystring = 0 Then
                    strFormName = ("UnFundingTransmittalForm.aspx?Process=0")
                ElseIf paramQuerystring = 1 Then
                    strFormName = ("UnFundingTransmittalForm.aspx?Process=1")
                End If
            End If
            CheckReadOnlyMode()
            'END : Shilpa N | 02/22/2019 | YRS-AT-4248 |  Passing strFormName depends on the querystring. 

            If Not Me.IsPostBack Then
                Clear_Session()
                If Not Page.Request.QueryString("Process") Is Nothing Then
                    Me.FunctionType = CType(Page.Request.QueryString("Process"), Integer)
                    Session("FunctionType") = Me.FunctionType
                    Me.ButtonAddToList.Enabled = False
                    If Me.FunctionType = 0 Then '--------Un-Fund
                        Me.LabelTitle.Text = "Un-Funding Transmittals"
                        Me.LabelSelectedList.Text = "List of transmittals to be unfunded"
                        Me.ButtonUnFund.Visible = True
                        Me.ButtonDelete.Visible = False
                        Me.ButtonUnFund.Enabled = False
                    ElseIf Me.FunctionType = 1 Then '----------Delete
                        Me.LabelTitle.Text = "Delete Transmittals"
                        Me.LabelSelectedList.Text = "List of transmittals to be deleted"
                        Me.ButtonUnFund.Visible = False
                        Me.ButtonDelete.Visible = True
                        Me.ButtonDelete.Enabled = False
                    Else
                        Me.LabelTitle.Text = ""
                    End If
                Else
                    Response.Redirect("Login.aspx", False)
                End If
            Else
                If Request.Form("YES") = "Yes" Then
                    If Not Session("Un-Fund") Is Nothing Then
                        If Session("Un-Fund") = True Then
                            Session("Un-Fund") = Nothing
                            ProcessUnFund()
                        End If
                    Else
                        If Not Session("Delete") Is Nothing Then
                            If Session("Delete") = True Then
                                Session("Delete") = Nothing
                                ProcessDelete()
                            End If
                        End If
                    End If
                End If
            End If

        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
    End Sub
    Private Sub Set_Attributes()
        Try
            Me.TextBoxYmcaNo.Attributes.Add("onblur", "javascript:FormatYMCANo();")
            Me.ButtonFind.Attributes.Add("onclick", "javascript:FormatYMCANo();")
        Catch
            Throw
        End Try
    End Sub

    Private Sub ButtonFind_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonFind.Click

        Try

            If TextBoxYmcaNo.Text = String.Empty AndAlso TextBoxTransmittalNo.Text = String.Empty AndAlso TextboxRecptNo.Text = String.Empty AndAlso TextBoxSTransmittalDate.Text = String.Empty AndAlso TextboxETransmittalDate.Text = String.Empty Then
                MessageBox.Show(PlaceHolder1, "Please Confirm", "Please Enter a search value", MessageBoxButtons.OK)
                Exit Sub
            Else

                'Dim l_string_TransmittalDate As String
                'l_string_TransmittalDate = Convert.ToString(DateTime.Now.Month) + "/" + "1" + "/" + Convert.ToString(DateTime.Now.Year)
                'l_string_TransmittalDate = "01/01/2008"

                Me.DataGridUnFundDelSearchResult.SelectedIndex = -1
                Dim l_dataset_YmcasTransmittal As DataSet
                'Swopna -s
                Dim dateTime_TransmittalStart As DateTime
                Dim dateTime_TransmittalEnd As DateTime
                'Dim l_firstdayofmonth As DateTime
                'Dim l_lastdayofmonth As DateTime
                'l_firstdayofmonth = System.DateTime.Today().Subtract(TimeSpan.FromDays(DateTime.Now.Day - 1))
                'l_lastdayofmonth = l_firstdayofmonth.AddMonths(1).Subtract(TimeSpan.FromDays(1))

                If Me.TextBoxSTransmittalDate.Text.Trim = String.Empty Then
                    dateTime_TransmittalStart = Convert.ToDateTime("01/01/1900")

                Else
                    dateTime_TransmittalStart = Convert.ToDateTime(Me.TextBoxSTransmittalDate.Text.Trim)
                End If
                If Me.TextboxETransmittalDate.Text.Trim = String.Empty Then
                    dateTime_TransmittalEnd = Convert.ToDateTime("01/01/2050")

                Else
                    dateTime_TransmittalEnd = Convert.ToDateTime(Me.TextboxETransmittalDate.Text.Trim)
                End If
                If dateTime_TransmittalStart > dateTime_TransmittalEnd Then
                    MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Transmittal Start Date cannot be later than Transmittal End Date.", MessageBoxButtons.Stop)
                    Exit Sub
                End If
                'Swopna -e
                'Fetch data from database
                l_dataset_YmcasTransmittal = YMCARET.YmcaBusinessObject.UnFundingTransmittalBO.LookUpYmcaTransmittals(CType(Session("FunctionType"), Integer), TextBoxYmcaNo.Text.Trim(), TextBoxTransmittalNo.Text.Trim(), TextboxRecptNo.Text.Trim(), dateTime_TransmittalStart, dateTime_TransmittalEnd)

                If Not l_dataset_YmcasTransmittal Is Nothing Then
                    If Not l_dataset_YmcasTransmittal.Tables(0) Is Nothing Then
                        If l_dataset_YmcasTransmittal.Tables(0).Rows.Count > 0 Then
                            Dim l_dt_TotalTransmittalsList As DataTable
                            l_dt_TotalTransmittalsList = l_dataset_YmcasTransmittal.Tables(0)
                            Session("l_dt_TotalTransmittalsList") = l_dt_TotalTransmittalsList
                            Me.DataGridUnFundDelSearchResult.DataSource = l_dt_TotalTransmittalsList
                            Me.DataGridUnFundDelSearchResult.DataBind()

                            Me.DataGridUnFundDelSearchResult.Visible = True
                            Me.LabelRecordNotFound.Visible = False
                            Me.ButtonAddToList.Enabled = True
                        Else
                            Me.DataGridUnFundDelSearchResult.Visible = False
                            Me.LabelRecordNotFound.Visible = True
                            Me.ButtonAddToList.Enabled = False


                        End If
                    End If
                End If

                'review
                'If Me.Datagrid_UnFundDelete_TranList.Items.Count = 0 Then
                '    Me.Datagrid_UnFundDelete_TranList.DataSource = Nothing
                '    Me.Datagrid_UnFundDelete_TranList.DataBind()
                'Else

                'End If

                'For Each di As DataGridItem In DataGridUnFundDelSearchResult.Items
                '    If di.Cells(5).Text = "&nbsp;" Then
                '        di.Cells(5).Text = "0.00"
                '    End If

                '    If di.Cells(6).Text = "&nbsp;" Then
                '        di.Cells(6).Text = "0.00"
                '    End If
                '    If di.Cells(7).Text = "&nbsp;" Then
                '        di.Cells(7).Text = "0.00"
                '    End If

                'Next
                'review
            End If
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
        End Try
    End Sub

    Private Sub ButtonAddToList_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAddToList.Click
        Try
            PopulateValues()
        Catch
            Throw
        End Try
    End Sub
    Public Sub PopulateValues()
        Dim l_counter As Integer
        Dim l_selected As Boolean
        Dim l_dt_SelectedTransmittals As DataTable
        Dim l_dt_NotSelectedTransmittals As DataTable
        Dim l_dt_Check As DataTable

        'Dim l_dt_TransmittalsList As New DataTable
        ' Dim l_dt_UnFundDel_TransmittalsList As New DataTable
        Dim drDel As DataRow()
        Try
            ''l_dt_UnFundDel_TransmittalsList contains the UnFunding/Delete Transmittals List that are selected for Unfunding
            'l_dt_UnFundDel_TransmittalsList.Columns.Add("YmcaId", GetType(String))
            'l_dt_UnFundDel_TransmittalsList.Columns.Add("YmcaNo", GetType(String))
            'l_dt_UnFundDel_TransmittalsList.Columns.Add("TransmittalID", GetType(String))
            'l_dt_UnFundDel_TransmittalsList.Columns.Add("TransmittalNo", GetType(String))
            'l_dt_UnFundDel_TransmittalsList.Columns.Add("TransmittalDate", GetType(String))
            'l_dt_UnFundDel_TransmittalsList.Columns.Add("AmtDue", GetType(String))
            'l_dt_UnFundDel_TransmittalsList.Columns.Add("AppliedReceipts", GetType(String))
            'l_dt_UnFundDel_TransmittalsList.Columns.Add("AppliedCredit", GetType(String))
            'l_dt_UnFundDel_TransmittalsList.Columns.Add("UniqueId", GetType(String))

            ''l_dt_TransmittalsList contains the UnFunding/Delete Transmittals List that are not selected for Unfunding       
            'l_dt_TransmittalsList.Columns.Add("YmcaId", GetType(String))
            'l_dt_TransmittalsList.Columns.Add("YmcaNo", GetType(String))
            'l_dt_TransmittalsList.Columns.Add("TransmittalID", GetType(String))
            'l_dt_TransmittalsList.Columns.Add("TransmittalNo", GetType(String))
            'l_dt_TransmittalsList.Columns.Add("TransmittalDate", GetType(String))
            'l_dt_TransmittalsList.Columns.Add("AmtDue", GetType(String))
            'l_dt_TransmittalsList.Columns.Add("AppliedReceipts", GetType(String))
            'l_dt_TransmittalsList.Columns.Add("AppliedCredit", GetType(String))
            'l_dt_TransmittalsList.Columns.Add("UniqueId", GetType(String))

            'Swopna 22July08 Start
            'Added in order to avoid adding same record in DataGridUnFundDelSearchResult repeatedly.
            If Not Session("UnFund_Del_TransmittalsList") Is Nothing Then
                l_dt_Check = DirectCast(Session("UnFund_Del_TransmittalsList"), DataTable)
                If l_dt_Check.Rows.Count = 0 Then
                    Session("Check") = False  'Case when there are no records in DataGridUnFundDelSearchResult
                Else
                    Session("Check") = True   'Case when there are records in DataGridUnFundDelSearchResult
                End If
            Else
                Session("Check") = False
            End If
            'Swopna 22July08 End

            'When Button AddToList is clicked for the first time,datatable schema is created.
            If Session("SelectedTransmittalsSchema") Is Nothing Then
                CreateSchema()
                l_dt_SelectedTransmittals = DirectCast(Session("SelectedTransmittalsSchema"), DataTable)
            Else
                'Added/Commented by Swopna 21July08 Start
                'l_dt_SelectedTransmittals = CType(Session("UnFund_Del_TransmittalsList"), DataTable) 'This session contains list of transmittals to be unfunded/deleted.  
                If Not Session("UnFund_Del_TransmittalsList") Is Nothing Then
                    l_dt_SelectedTransmittals = DirectCast(Session("UnFund_Del_TransmittalsList"), DataTable) 'This session contains list of transmittals to be unfunded/deleted.
                Else
                    l_dt_SelectedTransmittals = DirectCast(Session("SelectedTransmittalsSchema"), DataTable)
                End If
                'Added/Commented bySwopna 21July08 End
            End If

            'l_dt_NotSelectedTransmittals datatable is assigned list of transmittals which is the output of search result 
            l_dt_NotSelectedTransmittals = DirectCast(Session("l_dt_TotalTransmittalsList"), DataTable)
            l_counter = 0

            'List of selected transmittals is assigned to l_dt_SelectedTransmittals which is later bound to Datagrid_UnFundDelete_TranList
            'Unselected transmittals remain in DataGridUnFundDelSearchResult

            'Commented/Added by Swopna 22July08 Start----Added in order to avoid adding same record in DataGridUnFundDelSearchResult repeatedly.

            'For Each di As DataGridItem In DataGridUnFundDelSearchResult.Items
            '    l_selected = CType(di.FindControl("Checkbox1"), CheckBox).Checked
            '    If l_selected = True Then
            '        l_counter += 1
            '        Dim dr As DataRow
            '        dr = l_dt_SelectedTransmittals.NewRow()

            '        dr.Item("YmcaId") = di.Cells(0).Text
            '        dr.Item("YmcaNo") = di.Cells(2).Text
            '        dr.Item("TransmittalNo") = di.Cells(3).Text
            '        dr.Item("TransmittalDate") = di.Cells(4).Text
            '        dr.Item("AmtDue") = di.Cells(5).Text
            '        dr.Item("AppliedReceipts") = di.Cells(6).Text
            '        dr.Item("AppliedCredit") = di.Cells(7).Text
            '        dr.Item("UniqueId") = di.Cells(8).Text
            '        l_dt_SelectedTransmittals.Rows.Add(dr)
            '        Dim drow As DataRow()
            '        drow = l_dt_NotSelectedTransmittals.Select("UniqueId='" & di.Cells(8).Text & "'")
            '        drow(0).Delete()
            '        l_dt_NotSelectedTransmittals.AcceptChanges()

            '        'Else
            '        '    Dim dr As DataRow
            '        '    dr = l_dt_NotSelectedTransmittals.NewRow()

            '        '    dr.Item("YmcaId") = di.Cells(0).Text
            '        '    dr.Item("YmcaNo") = di.Cells(2).Text
            '        '    dr.Item("TransmittalNo") = di.Cells(3).Text
            '        '    dr.Item("TransmittalDate") = di.Cells(4).Text
            '        '    dr.Item("AmtDue") = di.Cells(5).Text
            '        '    dr.Item("AppliedReceipts") = di.Cells(6).Text
            '        '    dr.Item("AppliedCredit") = di.Cells(7).Text
            '        '    dr.Item("UniqueId") = di.Cells(8).Text
            '        '    l_dt_NotSelectedTransmittals.Rows.Add(dr) '--------not selected

            '    End If
            'Next

            'Case when there are no records in DataGridUnFundDelSearchResult
            'In this case there is no need to check if selected record exists in Datagrid_UnFundDelete_TranList. 
            If CType(Session("Check"), Boolean) = False Then
                For Each di As DataGridItem In DataGridUnFundDelSearchResult.Items
                    l_selected = DirectCast(di.FindControl("Checkbox1"), CheckBox).Checked
                    If l_selected = True Then
                        l_counter += 1
                        Dim dr As DataRow
                        dr = l_dt_SelectedTransmittals.NewRow()

                        dr.Item("YmcaId") = di.Cells(0).Text
                        dr.Item("YmcaNo") = di.Cells(2).Text
                        dr.Item("TransmittalNo") = di.Cells(3).Text
                        dr.Item("TransmittalDate") = di.Cells(4).Text
                        dr.Item("AmtDue") = di.Cells(5).Text
                        dr.Item("AppliedReceipts") = di.Cells(6).Text
                        dr.Item("AppliedCredit") = di.Cells(7).Text
                        dr.Item("UniqueId") = di.Cells(8).Text
                        l_dt_SelectedTransmittals.Rows.Add(dr)
                        Dim drow As DataRow()
                        drow = l_dt_NotSelectedTransmittals.Select("UniqueId='" & di.Cells(8).Text & "'")
                        drow(0).Delete()
                        l_dt_NotSelectedTransmittals.AcceptChanges()
                    End If
                Next
            Else
                'Case when there are  records in DataGridUnFundDelSearchResult
                'In this case there is need to check if selected record exists in Datagrid_UnFundDelete_TranList. 
                'Here first FOR loop checks whether record repeats and second FOR loop inserts & delete selected record from respective datatables.
                '2 loops are used to avoid 'index out of bound' error

                For Each di As DataGridItem In DataGridUnFundDelSearchResult.Items
                    l_selected = DirectCast(di.FindControl("Checkbox1"), CheckBox).Checked
                    If l_selected = True Then
                        For Each row As DataGridItem In Datagrid_UnFundDelete_TranList.Items
                            If di.Cells(8).Text = row.Cells(9).Text Then
                                MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Transmittal no." & di.Cells(3).Text & " is already selected.", MessageBoxButtons.Stop)
                                Exit Sub
                            End If
                        Next
                    End If
                Next

                For Each di As DataGridItem In DataGridUnFundDelSearchResult.Items
                    l_selected = DirectCast(di.FindControl("Checkbox1"), CheckBox).Checked
                    If l_selected = True Then
                        l_counter += 1
                        Dim dr As DataRow
                        dr = l_dt_SelectedTransmittals.NewRow()

                        dr.Item("YmcaId") = di.Cells(0).Text
                        dr.Item("YmcaNo") = di.Cells(2).Text
                        dr.Item("TransmittalNo") = di.Cells(3).Text
                        dr.Item("TransmittalDate") = di.Cells(4).Text
                        dr.Item("AmtDue") = di.Cells(5).Text
                        dr.Item("AppliedReceipts") = di.Cells(6).Text
                        dr.Item("AppliedCredit") = di.Cells(7).Text
                        dr.Item("UniqueId") = di.Cells(8).Text
                        l_dt_SelectedTransmittals.Rows.Add(dr)
                        Dim drow As DataRow()
                        drow = l_dt_NotSelectedTransmittals.Select("UniqueId='" & di.Cells(8).Text & "'")
                        drow(0).Delete()
                        l_dt_NotSelectedTransmittals.AcceptChanges()
                    End If

                Next
            End If
            'Commented/Added by Swopna 22July08 End

            If l_counter = 0 Then
                MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Select at least one transmittal.", MessageBoxButtons.Stop)
                Exit Sub
            End If

            'For Each di As DataGridItem In Datagrid_UnFundDelete_TranList.Items
            '    Dim dr As DataRow
            '    dr = l_dt_UnFundDel_TransmittalsList.NewRow()
            '    dr.Item("YmcaId") = di.Cells(0).Text
            '    dr.Item("YmcaNo") = di.Cells(3).Text
            '    dr.Item("TransmittalNo") = di.Cells(4).Text
            '    dr.Item("TransmittalDate") = di.Cells(5).Text
            '    dr.Item("AmtDue") = di.Cells(6).Text
            '    dr.Item("AppliedReceipts") = di.Cells(7).Text
            '    dr.Item("AppliedCredit") = di.Cells(8).Text
            '    dr.Item("UniqueId") = di.Cells(9).Text
            '    l_dt_UnFundDel_TransmittalsList.Rows.Add(dr) '--------selected
            'Next


            Datagrid_UnFundDelete_TranList.DataSource = l_dt_SelectedTransmittals
            Datagrid_UnFundDelete_TranList.DataBind()
            'List of final rows  in DataGridUnFundDelSearchResult
            Session("UnFund_Del_TransmittalsList") = l_dt_SelectedTransmittals

            DataGridUnFundDelSearchResult.DataSource = l_dt_NotSelectedTransmittals
            DataGridUnFundDelSearchResult.DataBind()
            'List of final rows after clicking add to list button in DataGridUnFundDelSearchResult
            Session("l_dt_TotalTransmittalsList") = l_dt_NotSelectedTransmittals

            'DataGridUnFundDelSearchResult.DataSource = l_dt_TransmittalsList
            'DataGridUnFundDelSearchResult.DataBind()

            If CType(Session("FunctionType"), Integer) = 0 Then
                Me.ButtonUnFund.Enabled = True
            ElseIf CType(Session("FunctionType"), Integer) = 1 Then
                Me.ButtonDelete.Enabled = True
            End If

            'Selected and UnSelected UnFunding/Delete Records are stored in Session variables
            'Session("UnFund_Del_TransmittalsList") = l_dt_SelectedTransmittals '--------selected
            'Session("l_dt_TransmittalsList") = l_dt_TransmittalsList '--------not selected
            'l_dt_UnFundDel_TransmittalsList = Nothing
            'l_dt_TransmittalsList = Nothing
        Catch
            Throw
        End Try
    End Sub

    Private Sub ButtonClear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonClear.Click
        Try
            'Clear the values
            TextBoxYmcaNo.Text = ""
            TextBoxTransmittalNo.Text = ""
            TextBoxSTransmittalDate.Text = ""
            TextboxETransmittalDate.Text = ""
            TextboxRecptNo.Text = ""
        Catch
            Throw
        End Try

    End Sub


    Public Sub Clear_Session()
        Try
            'Set to nothing-called during not ispostback and Button OK click
            Session("FunctionType") = Nothing
            Session("l_dt_TotalTransmittalsList") = Nothing
            Session("UnFund_Del_TransmittalsList") = Nothing
            Session("l_dt_TransmittalsList") = Nothing
            Session("l_dt_TTList") = Nothing
            Session("SearchResult_Sort") = Nothing
            Session("UnFund_DelResult_Sort") = Nothing
            Session("Un-Fund") = Nothing
            Session("Delete") = Nothing
            Session("FunctionType") = Nothing
            Session("SelectedTransmittalsSchema") = Nothing
            Session("Check") = Nothing

        Catch
            Throw
        End Try
    End Sub


    Public Sub ButtonUnFund_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonUnFund.Click
        Try
            'Added by neeraj on 18-Nov-2009 : issue is YRS 5.0-940
            Dim checkSecurity As String = SecurityCheck.Check_Authorization(strFormName, Convert.ToInt32(Session("LoggedUserKey")))

            If Not checkSecurity.Equals("True") Then
                MessageBox.Show(PlaceHolder1, "YMCA-YRS", checkSecurity, MessageBoxButtons.Stop)
                Exit Sub
            End If
            'End : YRS 5.0-940

            Dim l_bool_select As Boolean
            Dim l_checkbox As CheckBox
            Dim l_dgitem As DataGridItem
            Dim l_count As Integer
            Dim l_string_Message As String
            l_count = 0
            For Each l_dgitem In Me.Datagrid_UnFundDelete_TranList.Items
                l_checkbox = DirectCast(l_dgitem.FindControl("CheckBoxSelect"), CheckBox)
                If l_checkbox.Checked = True Then
                    l_count += 1
                End If
            Next
            If l_count = 0 Then
                l_string_Message = "Select at least one transmittal."
                MessageBox.Show(PlaceHolder1, "Un-Fund Process", l_string_Message.Trim(), MessageBoxButtons.Stop)
                Exit Sub
            Else
                If l_count > 0 Then
                    l_string_Message = "Are you sure you want to un-fund " & l_count.ToString() & " transmittal(s)?"
                    MessageBox.Show(PlaceHolder1, "Un-Fund Process", l_string_Message.Trim(), MessageBoxButtons.YesNo)
                    Session("Un-Fund") = True
                    Exit Sub
                End If
            End If

        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try

    End Sub
    Public Function CheckAcctBalance(ByVal UniqueID As String) As Integer
        'This functions checks if enough balance is available to unfund a transmittal

        'Dim dsAcctTypes As New DataSet
        Dim l_int As Integer
        Dim flag As Boolean
        'Dim drs() As DataRow
        Try
            'dsAcctTypes contains account balance list on person basis
            'dsAcctTypes = YMCARET.YmcaBusinessObject.UnFundingTransmittalBO.CheckAcctBalance(UniqueID)
            l_int = YMCARET.YmcaBusinessObject.UnFundingTransmittalBO.CheckAcctBalance(UniqueID)
            'drs = dsAcctTypes.Tables(0).Select("Amount <= 0 ")
            'If drs.Length > 0 Then
            '    flag = False
            'Else
            '    flag = True
            'End If
            'Return flag
            Return l_int
        Catch
            Throw
        End Try

    End Function
    'Swopna 11Aug08-Start
    Public Function CheckAppliedReceiptsCredits(ByVal TransmittalNo As String) As Integer
        'This functions checks if receipts or credits were applied against transmittal
        Dim l_int As Integer

        Try
            l_int = YMCARET.YmcaBusinessObject.UnFundingTransmittalBO.CheckAppliedReceiptsCredits(TransmittalNo)
            Return l_int
        Catch
            Throw
        End Try

    End Function
    'Swopna 11Aug08-End

    Public Sub Datagrid_UnFundDelete_TranList_ItemCommand(ByVal sender As System.Object, ByVal e As DataGridCommandEventArgs) Handles Datagrid_UnFundDelete_TranList.ItemCommand
        Try

            'Dim dsValidateTransmittals As New DataSet
            'Dim l_dt_CTList As New DataTable
            'Dim selected As Boolean
            'Dim i_Counter As Integer
            'Dim flag As Boolean
            'Dim delRow As DataRow()
            'Dim dr As DataRow()
            'Dim l_dt_TList As New DataTable
            'Dim l_dt_TTList As New DataTable
            'l_dt_TTList = Session("UnFund_Del_TransmittalsList")
            'Try
            '    'To Remove the transmittal from the datagrid 
            '    If e.CommandName = "Delete" Then
            '        'MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Are you sure you want to delete the Transmittal", MessageBoxButtons.YesNo)
            '        l_dt_CTList.Columns.Add("YmcaId", GetType(String))
            '        l_dt_CTList.Columns.Add("YmcaNo", GetType(String))
            '        l_dt_CTList.Columns.Add("TransmittalNo", GetType(String))
            '        l_dt_CTList.Columns.Add("TransmittalDate", GetType(String))
            '        l_dt_CTList.Columns.Add("AmtDue", GetType(String))
            '        l_dt_CTList.Columns.Add("AppliedReceipts", GetType(String))
            '        l_dt_CTList.Columns.Add("AppliedCredit", GetType(String))
            '        l_dt_CTList.Columns.Add("UniqueId", GetType(String))

            '        l_dt_TList.Columns.Add("YmcaId", GetType(String))
            '        l_dt_TList.Columns.Add("YmcaNo", GetType(String))
            '        l_dt_TList.Columns.Add("TransmittalNo", GetType(String))
            '        l_dt_TList.Columns.Add("TransmittalDate", GetType(String))
            '        l_dt_TList.Columns.Add("AmtDue", GetType(String))
            '        l_dt_TList.Columns.Add("AppliedReceipts", GetType(String))
            '        l_dt_TList.Columns.Add("AppliedCredit", GetType(String))
            '        l_dt_TList.Columns.Add("UniqueId", GetType(String))

            '        'l_dt_TTList.Columns.Add("YmcaId", GetType(String))
            '        'l_dt_TTList.Columns.Add("YmcaNo", GetType(String))
            '        'l_dt_TTList.Columns.Add("TransmittalNo", GetType(String))
            '        'l_dt_TTList.Columns.Add("TransmittalDate", GetType(String))
            '        'l_dt_TTList.Columns.Add("AmtDue", GetType(String))
            '        'l_dt_TTList.Columns.Add("AppliedReceipts", GetType(String))
            '        'l_dt_TTList.Columns.Add("AppliedCredit", GetType(String))
            '        'l_dt_TTList.Columns.Add("UniqueId", GetType(String))
            '        If Datagrid_UnFundDelete_TranList.Items.Count > 0 Then
            '            For Each di As DataGridItem In Datagrid_UnFundDelete_TranList.Items
            '                selected = CType(di.FindControl("CheckBoxSelect"), CheckBox).Checked
            '                If selected = True Then
            '                    Dim drTransmittalList As DataRow
            '                    drTransmittalList = l_dt_CTList.NewRow()
            '                    drTransmittalList.Item("YmcaId") = di.Cells(0).Text
            '                    drTransmittalList.Item("YmcaNo") = di.Cells(3).Text
            '                    drTransmittalList.Item("TransmittalNo") = di.Cells(4).Text
            '                    drTransmittalList.Item("TransmittalDate") = di.Cells(5).Text
            '                    drTransmittalList.Item("AmtDue") = di.Cells(6).Text
            '                    drTransmittalList.Item("AppliedReceipts") = di.Cells(7).Text
            '                    drTransmittalList.Item("AppliedCredit") = di.Cells(8).Text
            '                    drTransmittalList.Item("UniqueId") = di.Cells(9).Text
            '                    l_dt_CTList.Rows.Add(drTransmittalList)
            '                Else
            '                    Dim drUNTransmittalList As DataRow
            '                    drUNTransmittalList = l_dt_TList.NewRow()
            '                    drUNTransmittalList.Item("YmcaId") = di.Cells(0).Text
            '                    drUNTransmittalList.Item("YmcaNo") = di.Cells(3).Text
            '                    drUNTransmittalList.Item("TransmittalNo") = di.Cells(4).Text
            '                    drUNTransmittalList.Item("TransmittalDate") = di.Cells(5).Text
            '                    drUNTransmittalList.Item("AmtDue") = di.Cells(6).Text
            '                    drUNTransmittalList.Item("AppliedReceipts") = di.Cells(7).Text
            '                    drUNTransmittalList.Item("AppliedCredit") = di.Cells(8).Text
            '                    drUNTransmittalList.Item("UniqueId") = di.Cells(9).Text
            '                    l_dt_TList.Rows.Add(drUNTransmittalList)

            '                End If
            '            Next
            '        End If
            '        'If Datagrid_UnFundDelete_TranList.Items.Count > 0 Then
            '        '    For Each di As DataGridItem In Datagrid_UnFundDelete_TranList.Items
            '        '        Dim drTTransmittalList As DataRow
            '        '        drTTransmittalList = l_dt_TTList.NewRow()
            '        '        drTTransmittalList.Item("YmcaId") = di.Cells(0).Text
            '        '        drTTransmittalList.Item("YmcaNo") = di.Cells(3).Text
            '        '        drTTransmittalList.Item("TransmittalNo") = di.Cells(4).Text
            '        '        drTTransmittalList.Item("TransmittalDate") = di.Cells(5).Text
            '        '        drTTransmittalList.Item("AmtDue") = di.Cells(6).Text
            '        '        drTTransmittalList.Item("AppliedReceipts") = di.Cells(7).Text
            '        '        drTTransmittalList.Item("AppliedCredit") = di.Cells(8).Text
            '        '        drTTransmittalList.Item("UniqueId") = di.Cells(9).Text
            '        '        l_dt_TTList.Rows.Add(drTTransmittalList)
            '        '    Next
            '        'End If

            '        For i_Counter = 0 To l_dt_CTList.Rows.Count - 1
            '            'dsValidateTransmittals Checks IF Any Receipts or Credits have been applied for Transmittal
            '            dsValidateTransmittals = YMCARET.YmcaBusinessObject.UnFundingTransmittalBO.ValidateTransmittal(l_dt_CTList.Rows(i_Counter).Item("UniqueId"))
            '            If (dsValidateTransmittals.Tables(0).Rows(0)(0).GetType().ToString() = "System.DBNull") And (dsValidateTransmittals.Tables(0).Rows(0)(1).GetType().ToString() = "System.DBNull") Then
            '                YMCARET.YmcaBusinessObject.UnFundingTransmittalBO.DeleteYmcaTransmittals(l_dt_CTList.Rows(i_Counter).Item("UniqueId"), l_dt_CTList.Rows(i_Counter).Item("TransmittalNo"), Convert.ToDouble(l_dt_CTList.Rows(i_Counter).Item("AmtDue")))
            '                flag = True
            '            Else
            '                MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Please Un-Fund the Transmittal First", MessageBoxButtons.OK)
            '                flag = False
            '            End If
            '        Next
            '        If flag = True Then
            '            Datagrid_UnFundDelete_TranList.DataSource = l_dt_TList
            '            Datagrid_UnFundDelete_TranList.DataBind()

            '        Else
            '            Datagrid_UnFundDelete_TranList.DataSource = l_dt_TTList
            '            Datagrid_UnFundDelete_TranList.DataBind()
            '        End If
            '    End If
            '    Session("l_dt_TTList") = l_dt_TTList


            'On click of Remove transmittal button,row is removed from the datagrid and not from database.
            Dim l_dr_RemoveTrans As DataRow()
            Dim l_dt_UnFundDelRemove As DataTable
            If e.CommandName = "Remove" Then
                l_dt_UnFundDelRemove = Session("UnFund_Del_TransmittalsList")
                l_dr_RemoveTrans = l_dt_UnFundDelRemove.Select("UniqueId='" & e.Item.Cells(9).Text & "'")
                l_dr_RemoveTrans(0).Delete()
                Datagrid_UnFundDelete_TranList.DataSource = l_dt_UnFundDelRemove
                Datagrid_UnFundDelete_TranList.DataBind()
                l_dt_UnFundDelRemove.AcceptChanges() 'Added by Swopna11Aug08
            End If

        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
    End Sub
    Public Sub Datagrid_UnFundDelete_TranList_SortCommand(ByVal sender As System.Object, ByVal e As DataGridSortCommandEventArgs) Handles Datagrid_UnFundDelete_TranList.SortCommand
        'Dim l_string_SortExpression As String
        Dim l_dt_UnFund_Del As DataTable

        Try


            'l_dt_TTList = Session("l_dt_TTList")
            'l_dt_TTList = Session("UnFund_Del_TransmittalsList")
            'l_string_SortExpression = e.SortExpression.ToString()
            'If Not Session("l_string_SortExpression") Is Nothing Then
            '    If Session("l_string_SortExpression").ToString.Trim.EndsWith("ASC") Then
            '        l_string_SortExpression = l_string_SortExpression + " DESC"
            '    Else
            '        l_string_SortExpression = l_string_SortExpression + " ASC"
            '    End If
            'Else
            '    l_string_SortExpression = l_string_SortExpression + " ASC"
            'End If
            'Session("l_string_SortExpression") = l_string_SortExpression
            'dv = l_dt_TTList.DefaultView
            'dv.Sort = l_string_SortExpression
            'Datagrid_UnFundDelete_TranList.DataSource = dv
            'Datagrid_UnFundDelete_TranList.DataBind()

            'If l_string_SortExpression.Trim.EndsWith("
            If Not Session("UnFund_Del_TransmittalsList") Is Nothing Then
                Dim dv As New DataView
                Dim SortExpression As String
                SortExpression = e.SortExpression
                l_dt_UnFund_Del = DirectCast(Session("UnFund_Del_TransmittalsList"), DataTable)
                dv = l_dt_UnFund_Del.DefaultView
                dv.Sort = SortExpression
                If Not Session("UnFund_DelResult_Sort") Is Nothing Then
                    If Session("UnFund_DelResult_Sort").ToString.Trim.EndsWith("ASC") Then
                        dv.Sort = SortExpression + " DESC"
                    Else
                        dv.Sort = SortExpression + " ASC"
                    End If
                Else
                    dv.Sort = SortExpression + " ASC"
                End If
                Me.Datagrid_UnFundDelete_TranList.DataSource = Nothing
                Me.Datagrid_UnFundDelete_TranList.DataSource = dv
                Me.Datagrid_UnFundDelete_TranList.DataBind()
                Session("UnFund_DelResult_Sort") = dv.Sort
            End If

        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
    End Sub
    Public Sub DataGridUnFundSearchResult_SortCommand(ByVal sender As System.Object, ByVal e As DataGridSortCommandEventArgs) Handles DataGridUnFundDelSearchResult.SortCommand
        'Dim l_string_SortExpression As String

        Dim l_dt_TTList As DataTable
        'Dim l_dt_UTList As DataTable

        Try
            If Not Session("l_dt_TotalTransmittalsList") Is Nothing Then
                Dim dv As New DataView
                Dim SortExpression As String
                SortExpression = e.SortExpression
                l_dt_TTList = DirectCast(Session("l_dt_TotalTransmittalsList"), DataTable)
                dv = l_dt_TTList.DefaultView
                dv.Sort = SortExpression
                If Not Session("SearchResult_Sort") Is Nothing Then
                    If Session("SearchResult_Sort").ToString.Trim.EndsWith("ASC") Then
                        dv.Sort = SortExpression + " DESC"
                    Else
                        dv.Sort = SortExpression + " ASC"
                    End If
                Else
                    dv.Sort = SortExpression + " ASC"
                End If
                Me.DataGridUnFundDelSearchResult.DataSource = Nothing
                Me.DataGridUnFundDelSearchResult.DataSource = dv
                Me.DataGridUnFundDelSearchResult.DataBind()
                Session("SearchResult_Sort") = dv.Sort
            End If

            'l_dt_TTList = CType(Session("l_dt_TotalTransmittalsList"), DataTable)
            ''l_dt_UTList = CType(Session("l_dt_TransmittalsList"), DataTable)
            'dv = l_dt_TTList.DefaultView
            'l_string_SortExpression = e.SortExpression.ToString()
            'DataGridUnFundDelSearchResult.SelectedIndex = -1
            'dv.Sort = l_string_SortExpression
            'If Not Session("l_string_SortExpression") Is Nothing Then
            '    If Session("l_string_SortExpression").ToString.Trim.EndsWith("ASC") Then
            '        dv.Sort = l_string_SortExpression + " DESC"
            '    Else
            '        dv.Sort = l_string_SortExpression + " ASC"
            '    End If
            'Else
            '    dv.Sort = l_string_SortExpression + " ASC"
            'End If

            'Session("l_string_SortExpression") = l_string_SortExpression
            ''If Datagrid_UnFundDelete_TranList.Items.Count > 0 Then
            ''    dv = l_dt_UTList.DefaultView
            ''Else
            ''    dv = l_dt_TTList.DefaultView
            ''End If
            'DataGridUnFundDelSearchResult.DataSource = Nothing
            'DataGridUnFundDelSearchResult.DataSource = dv
            'DataGridUnFundDelSearchResult.DataBind()

        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
    End Sub
    Private Sub ProcessUnFund() 'called when user wish to proceed with un-funding process
        Try
            Dim l_selected As Boolean
            Dim l_YmcaId As String
            Dim l_UniqueId As String
            Dim l_TransmittalNo As String
            Dim l_AmtDue As Double
            'Dim flag As Boolean
            Dim l_int As Integer
            Dim l_Counter As Integer
            Dim check As Boolean
            Dim l_dt_CheckedTransmittalsList As New DataTable
            Dim delRow As DataRow()
            l_dt_CheckedTransmittalsList = Session("UnFund_Del_TransmittalsList")

            For Each di As DataGridItem In Datagrid_UnFundDelete_TranList.Items
                l_selected = DirectCast(di.FindControl("CheckBoxSelect"), CheckBox).Checked
                If l_selected = True Then
                    l_YmcaId = di.Cells(0).Text
                    l_TransmittalNo = di.Cells(4).Text
                    l_AmtDue = Convert.ToDouble(di.Cells(6).Text)
                    l_UniqueId = di.Cells(9).Text
                    l_int = CheckAcctBalance(l_UniqueId)
                    If l_int = 1 Then
                        MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Enough money not available in accounts.", MessageBoxButtons.Stop)
                        Exit Sub
                    ElseIf l_int = 2 Then

                        'Added by Paramesh K. on Sept 19th 2008
                        'For checking any UEIN Transmittals exists or not 
                        l_int = CheckUEINTransmittalExists(l_UniqueId)
                        If l_int = 1 Then
                            MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Cannot UnFund the Transmittal, as it has a associated UEIN transmittal which was generated for multiple Transmittals.", MessageBoxButtons.Stop)
                            Exit Sub
                        ElseIf l_int = 2 Then
                            MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Cannot UnFund the Transmittal, as it has a associated UEIN which is funded.", MessageBoxButtons.Stop)
                            Exit Sub
                        ElseIf l_int = 3 Then
                            'Selected Records are Unfunded
                            YMCARET.YmcaBusinessObject.UnFundingTransmittalBO.SaveUnFundingTransmittals(l_YmcaId, l_UniqueId, l_AmtDue, l_TransmittalNo)
                            'MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Do you want to proceed with the adjustment", MessageBoxButtons.YesNo)
                            delRow = l_dt_CheckedTransmittalsList.Select("UniqueID = '" & l_UniqueId & "'")
                            delRow(0).Delete()
                            'If delRow.Length > 0 Then
                            '    l_dt_CheckedTransmittalsList.Rows.Remove(delRow(0))
                            'End If
                        End If
                    End If
                End If
            Next
            'For Each di As DataGridItem In Datagrid_UnFundDelete_TranList.Items
            '    l_selected = CType(di.FindControl("CheckBoxSelect"), CheckBox).Checked
            '    If l_selected = True Then
            '        check = True
            '        Exit For

            '    Else
            '        check = False
            '    End If
            'Next
            'If check = False Then
            '    MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Please select a record to un-fund", MessageBoxButtons.OK)
            'End If


            'Binds the Grid with unfunded records 
            Datagrid_UnFundDelete_TranList.DataSource = l_dt_CheckedTransmittalsList
            Datagrid_UnFundDelete_TranList.DataBind()
            Session("UnFund_Del_TransmittalsList") = l_dt_CheckedTransmittalsList


        Catch
            Throw
        End Try
    End Sub

    'Created By Paramesh K. on Sept 19th 2008
    'This functions checks if any UEIN Transmital exists to unfund a transmittal
    Public Function CheckUEINTransmittalExists(ByVal transmittalUniqueID As String) As Integer

        Dim l_int As Integer
        Try
            'Calling the CheckUEINTransmittalExists() method UnfundingTransmittal BO class
            'which will check any UEIN Transmittal exists are not
            l_int = YMCARET.YmcaBusinessObject.UnFundingTransmittalBO.CheckUEINTransmittalExists(transmittalUniqueID)
            Return l_int
        Catch
            Throw
        End Try

    End Function

    Private Sub ButtonOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonOk.Click
        'Redirects the page to main form
        Try
            Clear_Session()
            Response.Redirect("MainWebForm.aspx", False)
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
    End Sub

    Private Sub ButtonDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonDelete.Click
        Try

            'Added by neeraj on 18-Nov-2009 : issue is YRS 5.0-940
            Dim checkSecurity As String = SecurityCheck.Check_Authorization(strFormName, Convert.ToInt32(Session("LoggedUserKey")))

            If Not checkSecurity.Equals("True") Then
                MessageBox.Show(PlaceHolder1, "YMCA-YRS", checkSecurity, MessageBoxButtons.Stop)
                Exit Sub
            End If
            'End : YRS 5.0-940

            Dim l_bool_select As Boolean
            Dim l_checkbox As CheckBox
            Dim l_dgitem As DataGridItem
            Dim l_count As Integer
            Dim l_string_Message As String
            'Swopna 11July08--Start
            Dim l_transmittalno As String
            Dim l_int_transmittalno As Integer
            'Swopna 11July08--End

            l_count = 0
            l_int_transmittalno = 0
            For Each l_dgitem In Me.Datagrid_UnFundDelete_TranList.Items
                l_checkbox = DirectCast(l_dgitem.FindControl("CheckBoxSelect"), CheckBox)

                If l_checkbox.Checked = True Then
                    l_count += 1
                    'Swopna 11July08--Start
                    l_transmittalno = l_dgitem.Cells(4).Text
                    If l_transmittalno.EndsWith("AY") Or l_transmittalno.EndsWith("AE") Then
                        l_int_transmittalno += 1
                    End If
                    'Swopna 11July08--End
                End If

            Next
            If l_count = 0 Then
                l_string_Message = "Select at least one transmittal."
                MessageBox.Show(PlaceHolder1, "Delete Process", l_string_Message.Trim(), MessageBoxButtons.Stop)
                Exit Sub
            Else
                'commented -Swopna-11July08 
                'If l_count > 0 Then
                '    l_string_Message = "Are you sure you want to delete " & l_count.ToString() & " transmittal(s)?"
                '    MessageBox.Show(PlaceHolder1, "Delete Process", l_string_Message.Trim(), MessageBoxButtons.YesNo)
                '    Session("Delete") = True
                '    Exit Sub
                'End If

                'Swopna 11July08--Start
                'To check if there is any AY or AE transmitttal in selected transmittals and display message accordingly
                If l_int_transmittalno = 0 Then
                    l_string_Message = "Do you want to delete the selected transmittal(s)?"
                ElseIf l_int_transmittalno > 0 Then
                    l_string_Message = "Do you want to delete the selected transmittal(s) and associated AY or AE trasmttals?"
                End If
                MessageBox.Show(PlaceHolder1, "Delete Process", l_string_Message.Trim(), MessageBoxButtons.YesNo)
                Session("Delete") = True
                Exit Sub
                'Swopna 11July08--End
            End If

        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
    End Sub
    Private Sub ProcessDelete() 'called when user wish to proceed with delete process

        Dim l_dt_Schema As DataTable

        Try

            ''Dim dsValidateTransmittals As New DataSet

            'Dim l_dt_CTList As New DataTable 'list of selected transmittals to delete
            'Dim selected As Boolean
            'Dim i_Counter As Integer
            'Dim flag As Boolean
            'Dim delRow As DataRow()
            'Dim dr As DataRow()
            'Dim l_dt_TList As New DataTable 'list of unselected transmittals to delete
            'Dim l_dt_TTList As New DataTable
            'l_dt_TTList = Session("UnFund_Del_TransmittalsList") 'actual list in 2 grid



            ''MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Are you sure you want to delete the Transmittal", MessageBoxButtons.YesNo)
            'l_dt_CTList.Columns.Add("YmcaId", GetType(String))
            'l_dt_CTList.Columns.Add("YmcaNo", GetType(String))
            'l_dt_CTList.Columns.Add("TransmittalNo", GetType(String))
            'l_dt_CTList.Columns.Add("TransmittalDate", GetType(String))
            'l_dt_CTList.Columns.Add("AmtDue", GetType(String))
            'l_dt_CTList.Columns.Add("AppliedReceipts", GetType(String))
            'l_dt_CTList.Columns.Add("AppliedCredit", GetType(String))
            'l_dt_CTList.Columns.Add("UniqueId", GetType(String))

            'l_dt_TList.Columns.Add("YmcaId", GetType(String))
            'l_dt_TList.Columns.Add("YmcaNo", GetType(String))
            'l_dt_TList.Columns.Add("TransmittalNo", GetType(String))
            'l_dt_TList.Columns.Add("TransmittalDate", GetType(String))
            'l_dt_TList.Columns.Add("AmtDue", GetType(String))
            'l_dt_TList.Columns.Add("AppliedReceipts", GetType(String))
            'l_dt_TList.Columns.Add("AppliedCredit", GetType(String))
            'l_dt_TList.Columns.Add("UniqueId", GetType(String))

            ''l_dt_TTList.Columns.Add("YmcaId", GetType(String))
            ''l_dt_TTList.Columns.Add("YmcaNo", GetType(String))
            ''l_dt_TTList.Columns.Add("TransmittalNo", GetType(String))
            ''l_dt_TTList.Columns.Add("TransmittalDate", GetType(String))
            ''l_dt_TTList.Columns.Add("AmtDue", GetType(String))
            ''l_dt_TTList.Columns.Add("AppliedReceipts", GetType(String))
            ''l_dt_TTList.Columns.Add("AppliedCredit", GetType(String))
            ''l_dt_TTList.Columns.Add("UniqueId", GetType(String))
            'If Datagrid_UnFundDelete_TranList.Items.Count > 0 Then
            '    For Each di As DataGridItem In Datagrid_UnFundDelete_TranList.Items
            '        selected = CType(di.FindControl("CheckBoxSelect"), CheckBox).Checked
            '        If selected = True Then
            '            Dim drTransmittalList As DataRow
            '            drTransmittalList = l_dt_CTList.NewRow()
            '            drTransmittalList.Item("YmcaId") = di.Cells(0).Text
            '            drTransmittalList.Item("YmcaNo") = di.Cells(3).Text
            '            drTransmittalList.Item("TransmittalNo") = di.Cells(4).Text
            '            drTransmittalList.Item("TransmittalDate") = di.Cells(5).Text
            '            drTransmittalList.Item("AmtDue") = di.Cells(6).Text
            '            drTransmittalList.Item("AppliedReceipts") = di.Cells(7).Text
            '            drTransmittalList.Item("AppliedCredit") = di.Cells(8).Text
            '            drTransmittalList.Item("UniqueId") = di.Cells(9).Text
            '            l_dt_CTList.Rows.Add(drTransmittalList)
            '        Else
            '            Dim drUNTransmittalList As DataRow
            '            drUNTransmittalList = l_dt_TList.NewRow()
            '            drUNTransmittalList.Item("YmcaId") = di.Cells(0).Text
            '            drUNTransmittalList.Item("YmcaNo") = di.Cells(3).Text
            '            drUNTransmittalList.Item("TransmittalNo") = di.Cells(4).Text
            '            drUNTransmittalList.Item("TransmittalDate") = di.Cells(5).Text
            '            drUNTransmittalList.Item("AmtDue") = di.Cells(6).Text
            '            drUNTransmittalList.Item("AppliedReceipts") = di.Cells(7).Text
            '            drUNTransmittalList.Item("AppliedCredit") = di.Cells(8).Text
            '            drUNTransmittalList.Item("UniqueId") = di.Cells(9).Text
            '            l_dt_TList.Rows.Add(drUNTransmittalList)

            '        End If
            '    Next
            'End If
            ''If Datagrid_UnFundDelete_TranList.Items.Count > 0 Then
            ''    For Each di As DataGridItem In Datagrid_UnFundDelete_TranList.Items
            ''        Dim drTTransmittalList As DataRow
            ''        drTTransmittalList = l_dt_TTList.NewRow()
            ''        drTTransmittalList.Item("YmcaId") = di.Cells(0).Text
            ''        drTTransmittalList.Item("YmcaNo") = di.Cells(3).Text
            ''        drTTransmittalList.Item("TransmittalNo") = di.Cells(4).Text
            ''        drTTransmittalList.Item("TransmittalDate") = di.Cells(5).Text
            ''        drTTransmittalList.Item("AmtDue") = di.Cells(6).Text
            ''        drTTransmittalList.Item("AppliedReceipts") = di.Cells(7).Text
            ''        drTTransmittalList.Item("AppliedCredit") = di.Cells(8).Text
            ''        drTTransmittalList.Item("UniqueId") = di.Cells(9).Text
            ''        l_dt_TTList.Rows.Add(drTTransmittalList)
            ''    Next
            ''End If

            ''For i_Counter = 0 To l_dt_CTList.Rows.Count - 1
            ''dsValidateTransmittals Checks IF Any Receipts or Credits have been applied for Transmittal
            ''dsValidateTransmittals = YMCARET.YmcaBusinessObject.UnFundingTransmittalBO.ValidateTransmittal(l_dt_CTList.Rows(i_Counter).Item("UniqueId"))
            ''If (dsValidateTransmittals.Tables(0).Rows(0)(0).GetType().ToString() = "System.DBNull") And (dsValidateTransmittals.Tables(0).Rows(0)(1).GetType().ToString() = "System.DBNull") Then
            ''    YMCARET.YmcaBusinessObject.UnFundingTransmittalBO.DeleteYmcaTransmittals(l_dt_CTList.Rows(i_Counter).Item("UniqueId"), l_dt_CTList.Rows(i_Counter).Item("TransmittalNo"), Convert.ToDouble(l_dt_CTList.Rows(i_Counter).Item("AmtDue")))
            ''    flag = True
            ''Else
            ''    MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Please Un-Fund the Transmittal First", MessageBoxButtons.OK)
            ''    flag = False
            ''End If

            ''Next
            'For Each drow As DataRow In l_dt_CTList.Rows
            '    If drow("AppliedReceipts") <> 0 Or drow("AppliedCredit") <> 0 Then
            '        MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Un-Fund transmittal no." & drow("TransmittalNo") & " for delete process to proceed.", MessageBoxButtons.Stop)
            '        Exit Sub
            '    End If
            'Next

            'For Each drow As DataRow In l_dt_CTList.Rows
            '    YMCARET.YmcaBusinessObject.UnFundingTransmittalBO.DeleteYmcaTransmittals(CType(drow("UniqueId"), String), CType(drow("TransmittalNo"), String), CType(drow("AmtDue"), Double))
            '    flag = True
            'Next

            'If flag = True Then
            '    Datagrid_UnFundDelete_TranList.DataSource = l_dt_TList
            '    Datagrid_UnFundDelete_TranList.DataBind()

            'Else
            '    Datagrid_UnFundDelete_TranList.DataSource = l_dt_TTList
            '    Datagrid_UnFundDelete_TranList.DataBind()
            'End If

            ''Session("l_dt_TTList") = l_dt_TTList
            'Session("UnFund_Del_TransmittalsList") = l_dt_TList

            '''''''''''''
            Dim l_counter As Integer
            Dim l_selected As Boolean
            Dim l_dt_SelectedTransmittals As DataTable

            Dim dr As DataRow

            '''''''''''''

            l_dt_Schema = DirectCast(Session("UnFund_Del_TransmittalsList"), DataTable)
            l_dt_SelectedTransmittals = l_dt_Schema.Clone()

            'drow = l_dt_Schema.Select("AppliedReceipts <> 0 OR AppliedCredit <> 0")
            '
            For Each dw As DataRow In l_dt_Schema.Rows
                If dw("AppliedReceipts") <> 0 Or dw("AppliedCredit") <> 0 Then
                    MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Un-Fund transmittal no." & dw("TransmittalNo") & " for delete process to proceed.", MessageBoxButtons.Stop)
                    Exit Sub
                End If
            Next
            '

            'If drow.Length > 0 Then
            '    MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Un-Fund transmittal no." & drow(0)("TransmittalNo") & " for delete process to proceed.", MessageBoxButtons.Stop)
            '    Exit Sub
            'End If

            'Swopna 06Aug08 Start
            Dim l_str As String
            Dim l_count As Integer

            Dim l_ArrList As ArrayList
            l_ArrList = New ArrayList

            Dim l_associateStr As String
            l_associateStr = String.Empty

            l_count = 0
            'Swopna 06Aug08 End

            l_dt_Schema.AcceptChanges() 'Added by Swopna11Aug08

            For Each di As DataGridItem In Datagrid_UnFundDelete_TranList.Items
                l_selected = DirectCast(di.FindControl("CheckBoxSelect"), CheckBox).Checked
                If l_selected = True Then
                    'Swopna 06Aug08 Start
                    'l_ArrList contains selected AE/AY transmittal's associated AE/AY transmittal number.
                    'In order to avoid error if both AE and AY transmittals are selected,we check in array list.If present,we collect next row in datagrid. 
                    If l_ArrList.Count > 0 Then
                        For l_count = 0 To l_ArrList.Count - 1
                            If di.Cells(4).Text = l_ArrList.Item(l_count) Then
                                GoTo outerNext
                            End If
                        Next
                    End If
                    'Swopna 06Aug08 End
                    l_counter += 1
                    dr = l_dt_SelectedTransmittals.NewRow()

                    dr.Item("YmcaId") = di.Cells(0).Text
                    dr.Item("YmcaNo") = di.Cells(3).Text
                    dr.Item("TransmittalNo") = di.Cells(4).Text
                    dr.Item("TransmittalDate") = di.Cells(5).Text
                    dr.Item("AmtDue") = di.Cells(6).Text
                    dr.Item("AppliedReceipts") = di.Cells(7).Text
                    dr.Item("AppliedCredit") = di.Cells(8).Text
                    dr.Item("UniqueId") = di.Cells(9).Text
                    l_dt_SelectedTransmittals.Rows.Add(dr)

                    'Swopna 06Aug08 Start
                    'This code deletes associated AE/AY transmittal from datagrid
                    If di.Cells(4).Text.EndsWith("AE") Or di.Cells(4).Text.EndsWith("AY") Then

                        l_str = di.Cells(4).Text

                        If l_str.EndsWith("AE") Then
                            l_associateStr = l_str.Substring(0, l_str.Length - 1) + "Y"
                        ElseIf l_str.EndsWith("AY") Then
                            l_associateStr = l_str.Substring(0, l_str.Length - 1) + "E"
                        End If

                        'Swopna 11Aug08 Start
                        'To check if associated transmittals have any applied receipts/credits. 
                        Dim l_int As Integer
                        l_int = CheckAppliedReceiptsCredits(l_associateStr)
                        If l_int = 2 Then
                            MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Un-Fund transmittal no." & l_associateStr & " for delete process to proceed.", MessageBoxButtons.Stop)
                            l_dt_Schema.RejectChanges()
                            Exit Sub
                        End If
                        'Swopna 11Aug08 End

                        'Inserting into array list                         
                        l_ArrList.Add(l_associateStr)


                        For Each dItem As DataGridItem In Datagrid_UnFundDelete_TranList.Items
                            If dItem.Cells(4).Text = l_associateStr Then
                                Dim datarow As DataRow()
                                datarow = l_dt_Schema.Select("UniqueId='" & dItem.Cells(9).Text & "' and TransmittalNo='" & l_associateStr & "'")
                                datarow(0).Delete()
                                Exit For
                            End If
                        Next
                    End If
                    'Swopna 06Aug08 End
                    'This code deletes selected transmittal from datagrid
                    Dim drow As DataRow()
                    drow = l_dt_Schema.Select("UniqueId='" & di.Cells(9).Text & "'")
                    drow(0).Delete()
                End If
outerNext:
            Next
            'Dim ds As New DataSet
            'Session("temp") = l_dt_SelectedTransmittals
            'ds = CType(Session("temp"), DataSet)

            YMCARET.YmcaBusinessObject.UnFundingTransmittalBO.DeleteYmcaTransmittals(l_dt_SelectedTransmittals)
            'YMCARET.YmcaBusinessObject.UnFundingTransmittalBO.DeleteYmcaTransmittals(ds)
            Datagrid_UnFundDelete_TranList.DataSource = l_dt_Schema
            Datagrid_UnFundDelete_TranList.DataBind()

            l_dt_Schema.AcceptChanges()
        Catch
            If Not l_dt_Schema Is Nothing Then
                If l_dt_Schema.GetChanges(DataRowState.Deleted).Rows.Count > 0 Then
                    l_dt_Schema.RejectChanges()
                End If
            End If

            Throw

        End Try
    End Sub
    Private Sub CreateSchema()
        Try
            Dim l_dt_SelectedTransmittals As New DataTable


            'l_dt_SelectedTransmittals contains the UnFunding/Delete Transmittals List that are selected for Unfunding
            l_dt_SelectedTransmittals.Columns.Add("YmcaId", GetType(String))
            l_dt_SelectedTransmittals.Columns.Add("YmcaNo", GetType(String))
            l_dt_SelectedTransmittals.Columns.Add("TransmittalID", GetType(String))
            l_dt_SelectedTransmittals.Columns.Add("TransmittalNo", GetType(String))
            l_dt_SelectedTransmittals.Columns.Add("TransmittalDate", GetType(String))
            l_dt_SelectedTransmittals.Columns.Add("AmtDue", GetType(String))
            l_dt_SelectedTransmittals.Columns.Add("AppliedReceipts", GetType(String))
            l_dt_SelectedTransmittals.Columns.Add("AppliedCredit", GetType(String))
            l_dt_SelectedTransmittals.Columns.Add("UniqueId", GetType(String))


            Session("SelectedTransmittalsSchema") = l_dt_SelectedTransmittals


        Catch
            Throw
        End Try
    End Sub
    Private Sub ShowCustomMessage(ByVal pstrMessage As String, ByVal pMessageBoxType As enumMessageBoxType, ByVal msgBoxButtons As MessageBoxButtons, Optional ByVal pstrMessageTitle As String = "")
        Dim alertWindow As String
        Try
            If pMessageBoxType = enumMessageBoxType.Javascript Then
                alertWindow = "<script language='javascript'>" & _
                            "alert('" & pstrMessage & "');" & _
                            "</script>"
                If (Not Me.IsStartupScriptRegistered("alertWindow")) Then
                    Page.RegisterStartupScript("alertWindow", alertWindow)
                End If
            ElseIf pMessageBoxType = enumMessageBoxType.DotNet Then
                If pstrMessageTitle.Trim.ToString() = String.Empty Then
                    pstrMessageTitle = "YMCA-YRS"
                End If
                MessageBox.Show(PlaceHolder1, pstrMessageTitle, pstrMessage, msgBoxButtons)
            End If
        Catch ex As Exception
            Throw
        End Try
    End Sub

    'START: Shilpa N | 03/19/2019 | YRS-AT-4248 | Check ReadOnly Flag, if true then disable the buttons on which security is not implemented and add Tool tip.
    Public Sub CheckReadOnlyMode()
        If (SecurityCheck.IsApplicationInReadOnlyMode()) Then
            Dim toolTip = YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.TOOLTIP_READONLY).DisplayText
            ButtonFind.Enabled = False
            ButtonFind.ToolTip = toolTip
        End If
    End Sub
    'END: Shilpa N | 03/19/2019 | YRS-AT-4248 | Check ReadOnly Flag, if true then disable the buttons on which security is not implemented and add Tool tip.

End Class

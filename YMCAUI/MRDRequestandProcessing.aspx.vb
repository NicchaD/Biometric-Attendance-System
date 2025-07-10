'*******************************************************************************
' Copyright YMCA Retirement Fund All Rights Reserved. 
'
' Project Name		:	YMCA - YRS
' FileName			:	MRDRequestandProcessing..aspx.vb
' Author Name		:	Sanjay 
' Employee ID		:	
' Email				:	
' Contact No		:	
' Creation Time		:	
' Program Specification Name	:	
' Unit Test Plan Name			:	
'
' Designed by			:	
' Designed on			:	
'
'**********************************************************************************************************************  
'** Modification History    
'**********************************************************************************************************************    
'** Modified By         Date(MM/DD/YYYY)    Description  
'**********************************************************************************************************************  
'   Imran               19/Apr/2011         BT-785 -If current balance less than $5000 and not single mrd processed then not allow to take MRD option.
'   Sanjeev(SG)         2012.03.15          BT-1011: YRS 5.0-1553 - enhancements to RMD processing pages.
'   Sanjeev(SG)         2012.03.16          Commented code Deleted
'   Dineshk             2013.10.03          YRS 5.0-2222 - Incorrct user name as chvCreator (Question regarding record creation via RMD batch processing)
'   Dinesh Kanojia      2013.10.03          BT-2139: YRS 5.0-2165:RMD enhancements 
'   Dinesh Kanojia      2013.10.28          BT-2012: YRS 5.0-2063: Handling RMD candidates 
'   Sanjay Rawat        2014.01.29          BT 2139: YRS 5.0-2165:RMD enhancements.
'   Sanjay Rawat        2014.02.25          BT 2139/YRS 5.0-2165 :RMD enhancements(Observation given by Murali).
'   Sanjay Rawat        2014.03.03          BT 2139/YRS 5.0-2165:Reverted changes done on 2014.02.15 for new reports
'   Manthan Rajguru     2015.09.16          YRS-AT-2550: YRS data cleanup: copyright comments and namespace/reference (Changed the Namespace/reference from 'Infotech' to 'YMCARET')
'*********************************************************************************************************************/ 

Imports System.IO
Imports System.Collections
Imports System.Xml
Imports System.Xml.Serialization
Imports System.Web.Services
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports Microsoft.Practices.EnterpriseLibrary.Logging

Public Class MRDRequestandProcessing
    Inherits System.Web.UI.Page
    Protected FinalDataSet As DataSet = Nothing
    Protected OriginalDataSet As DataSet = Nothing
    Dim ArraySlectedRetirementAccounts() As String
    Dim ArraySlectedSavingsAccounts() As String
    Dim myStringRetirementElements As Integer = 0
    Dim myStringSavingsElements As Integer = 0
    Dim ds_MRDRecords As DataSet
    Dim dsYears As DataSet
    Dim intCount As Integer
    Dim dtSelectedBatchRecords As DataTable
    Dim numberOfThreads As Integer = 2
    ' Code Added by Dineshk:2013.10.03: BT:-2228: Question regarding record creation via RMD batch processing
    'YRS 5.0-2222 - Incorrct user name as chvCreator
    Private threadIdentity As System.Security.Principal.WindowsIdentity
    ' Start:2014.01.29:SR - BT 2139: YRS 5.0-2165:RMD enhancements.
    Dim checkBoxArray As ArrayList
    Dim dtmRMDProcesingDate As DateTime
    ' Start:2014.01.29:SR - BT 2139: YRS 5.0-2165:RMD enhancements.

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("LoggedUserKey") Is Nothing Then
            Response.Redirect("Login.aspx", False)
        End If
        Try
            Menu1.DataSource = Server.MapPath("SimpleXML.xml")
            Menu1.DataBind()
            'LblMessage.Text = ""
            'populateMRDDetails()
            If Not Me.IsPostBack Then
                'populateYear()
                'populateMRDDetails()
                ds_MRDRecords = YMCARET.YmcaBusinessObject.MRDBO.GetBatchMRDRecords()
                Session("dsMRD") = ds_MRDRecords
                lblDate.Text = ds_MRDRecords.Tables("ProcessDate").Rows(0).Item(0).ToString()
                Session("ProcessingDate") = ds_MRDRecords.Tables("ProcessDate").Rows(0).Item(0).ToString()
                'Start-2014.01.29:SR-BT 2139: YRS 5.0-2165:RMD enhancements 
                EnableDisablePrintLetterOption()
                'End-2014.01.29:SR-BT 2139: YRS 5.0-2165:RMD enhancements 
            Else
                If Not Session("dsMRD") Is Nothing Then
                    ds_MRDRecords = CType(Session("dsMRD"), DataSet)
                End If
            End If

            If Request.Form("Yes") = "Yes" Then
                ProcessCuurentMRD()
            ElseIf Request.Form("No") = "No" Then

            ElseIf Request.Form("OK") = "OK" Then

            End If

        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
        End Try
    End Sub
#Region "Private Functions"

    Private Sub populateMRDDetails()
        Dim chkSelect As CheckBox
        Try
            'Start: 2014.02.12 : BT 2139/YRS 5.0-2165 - Clear objects
            ViewState("checkBoxArray") = Nothing
            Session("RMD_CHECKED_ITEMS") = Nothing
            'End: 2014.02.12 : BT 2139/YRS 5.0-2165 - Clear objects
            Session("dsMRD") = Nothing
            ds_MRDRecords = YMCARET.YmcaBusinessObject.MRDBO.GetBatchMRDRecords()
            Session("dsMRD") = ds_MRDRecords
            If ds_MRDRecords.Tables.Count > 0 And ds_MRDRecords.Tables("BatchRecords").Rows.Count > 0 Then
                lblDate.Text = ds_MRDRecords.Tables("ProcessDate").Rows(0).Item(0).ToString()
                dgMRD.DataSource = ds_MRDRecords.Tables("BatchRecords")
                If ds_MRDRecords.Tables("BatchRecords").Rows.Count > 0 Then dgMRD.SelectedIndex = -1
                dgMRD.DataBind()
                dgMRD.Visible = True
                dgMRD.Columns(0).Visible = True
                dgMrdProcessStatus.Visible = False
                btnPrintReport.Enabled = True
                btnProcess.Enabled = True
                'Comment code Deleted By SG: 2012.03.16
            Else
                MessageBox.Show(PlaceHolder1, "MRD Batch Prorcess", GetRMDMessage("MESSAGE_CLOSE_RMD_PROCESS"), MessageBoxButtons.OK)
                btnPrintReport.Enabled = False
                btnProcess.Enabled = False
            End If
        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
        End Try
    End Sub

    Sub SortCommand_OnClick(ByVal Source As Object, ByVal e As DataGridSortCommandEventArgs)
        Dim dg As DataGrid = DirectCast(Source, DataGrid)
        Dim SortExpression As String
        Try
            SortExpression = e.SortExpression
            SaveCheckedValues()
            Sort(dg, SortExpression)
            PopulateCheckedValues()
        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
        End Try
    End Sub

    Public Sub ProcessCuurentMRD()
        Dim dtProcess As Date
        Dim strMsg As String
        Try
            If ds_MRDRecords.Tables("ProcessDate").Rows(0).Item(0).ToString() = "" Then
                MessageBox.Show(PlaceHolder1, "MRD Batch Process", GetRMDMessage("MESSAGE_RMD_DATE_BLANK"), MessageBoxButtons.OK)
                Exit Sub
            End If
            dtProcess = CType(ds_MRDRecords.Tables("ProcessDate").Rows(0).Item(0).ToString(), Date)
            intCount = YMCARET.YmcaBusinessObject.MRDBO.SaveCurrentMRD(dtProcess)
            If intCount > 0 Then
                MessageBox.Show(PlaceHolder1, "MRD Batch Process", GetRMDMessage("MESSAGE_CLOSED_SUCCESS"), MessageBoxButtons.OK)
            Else
                MessageBox.Show(PlaceHolder1, "MRD Batch Process", GetRMDMessage("MESSAGE_CLOSEED_OR_NODATE"), MessageBoxButtons.OK)
            End If
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Function SaveArraylistPropertys(ByVal FundID As String, ByVal PlanType As String, ByVal MRDYear As Integer) As String
        Try
            Dim objProperty As New SavePropertys
            Dim ArraySlectedRetirementAccounts As String()
            Dim ArraySlectedSavingsAccounts As String()
            Dim RetirementRefundType As String
            Dim SavingsRefundType As String
            Dim RetirementMRDYear As Integer
            Dim SavingsMRDYear As Integer

            If PlanType = "RETIREMENT" Then
                RetirementRefundType = "MRD"
                RetirementMRDYear = MRDYear
                SavingsRefundType = String.Empty
                SavingsMRDYear = 0
            End If

            If PlanType = "SAVINGS" Then
                SavingsRefundType = "MRD"
                SavingsMRDYear = MRDYear
                RetirementRefundType = String.Empty
                RetirementMRDYear = 0
            End If


            'objProperty.StrSSNO = Me.SSNNo.Trim()
            objProperty.FundEventID = FundID
            'SetFundType("IsRetirement")
            'SetFundType("IsSavings")
            objProperty.RetirementPlanWithdrawalType = RetirementRefundType
            objProperty.SavingsPlanWithdrawalType = SavingsRefundType

            objProperty.SelectedRetirementPlanAccounts = ArraySlectedRetirementAccounts
            objProperty.SelectedSavingsPlanAccounts = ArraySlectedSavingsAccounts

            objProperty.RetirementPlanPartialAmount = 0 'IIf(CType(TextboxPartialRetirement.Trim, String) <> String.Empty, CType(TextboxPartialRetirement.Trim, String), "0.0")
            objProperty.SavingsPlanPartialAmount = 0 ' IIf(CType(Me.TextboxPartialSavings.Text, String) <> String.Empty, CType(Me.TextboxPartialSavings.Text, String), "0.0")

            'MRD PHASE -II
            If (RetirementRefundType = "MRD") Then
                objProperty.RetirementMRDYear = RetirementMRDYear
            End If
            If (SavingsRefundType = "MRD") Then
                objProperty.SavingsMRDYear = SavingsMRDYear
            End If
            'Start- SR:2014.03.03-BT 2139/YRS 5.0-2165:Reverted changes done on 2014.02.15
            '' Start:2014.01.29:SR - BT 2139: YRS 5.0-2165: Pass MRD processing date as paramter to webservice for displaying report based on  RMD processing date
            ''objProperty.RMDProcessingDate = MRDProcessingDate
            '' End:2014.01.29:SR - BT 2139: YRS 5.0-2165: Pass MRD processing date as paramter to webservice for displaying report based on  RMD processing date
            ' End- SR:2014.03.03-BT 2139/YRS 5.0-2165:Reverted changes done on 2014.02.15

            Dim strPropertysXML As String
            strPropertysXML = ConvertToXML(objProperty)
            strPropertysXML = strPropertysXML.Replace("<string>", "<AcctId>")
            strPropertysXML = strPropertysXML.Replace("</string>", "</AcctId>")

            Return strPropertysXML
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function ConvertToXML(ByVal objCl As Object) As String
        Dim objXml As New XmlSerializer(objCl.GetType())

        Dim objSW As New StringWriter
        objXml.Serialize(objSW, objCl)

        Dim xmlDoc As New XmlDocument
        xmlDoc.LoadXml(objSW.ToString())

        Dim XmlStr As String = ""
        XmlStr += "<" & xmlDoc.DocumentElement.Name & ">"
        XmlStr += xmlDoc.DocumentElement.InnerXml
        XmlStr += "</" & xmlDoc.DocumentElement.Name & ">"

        Return XmlStr
    End Function

    Public Sub ProcessMRD()
        Dim cloneDS As New DataSet
        Dim l_CheckBox As CheckBox

        Dim l_MRDRow As DataRow
        Dim blnSuccess As Boolean

        Dim alThreads As ArrayList = New ArrayList()

        Try
            dtSelectedBatchRecords = New DataTable
            dtSelectedBatchRecords.Columns.Add("FundIdNo", System.Type.GetType("System.Int32"))
            dtSelectedBatchRecords.Columns.Add("PlanType", System.Type.GetType("System.String"))
            dtSelectedBatchRecords.Columns.Add("CurrentBalance", System.Type.GetType("System.Decimal")) ''SR:2011.03.28 BT-797 : changed datatype from int32 to Decimal 
            dtSelectedBatchRecords.Columns.Add("MRDAmount", System.Type.GetType("System.Decimal")) ''SR:2011.03.28 BT-797 : changed datatype from int32 to Decimal 
            dtSelectedBatchRecords.Columns.Add("PaidAmount", System.Type.GetType("System.Decimal")) ''SR:2011.03.28 BT-797 : changed datatype from int32 to Decimal 
            dtSelectedBatchRecords.Columns.Add("MRDYear", System.Type.GetType("System.Int32"))
            dtSelectedBatchRecords.Columns.Add("StatusTypeDescription", System.Type.GetType("System.String"))
            dtSelectedBatchRecords.Columns.Add("FundEventID", System.Type.GetType("System.String"))
            dtSelectedBatchRecords.Columns.Add("Process_Status", System.Type.GetType("System.String"))
            dtSelectedBatchRecords.Columns.Add("IsLocked", System.Type.GetType("System.Boolean"))
            dtSelectedBatchRecords.Columns.Add("IsBlocked", System.Type.GetType("System.Int16"))
            dtSelectedBatchRecords.Columns.Add("IsMultipleYearMRD", System.Type.GetType("System.Int16"))
            'Added on 20/Apr/2011 BT:785 -If current balance less than $5000 and not single mrd processed then not allow to take MRD option     
            dtSelectedBatchRecords.Columns.Add("IsLess5kFirstTimeMrd", System.Type.GetType("System.Int16"))

            'Start :Code Added by Dinesh Kanojia BT 2012 : YRS 5.0-2165:RMD enhancements 
            dtSelectedBatchRecords.Columns.Add("Name", System.Type.GetType("System.String"))
            dtSelectedBatchRecords.Columns.Add("ProcessDate", System.Type.GetType("System.String"))
            dtSelectedBatchRecords.Columns.Add("WithholdingPercent", System.Type.GetType("System.String"))
            dtSelectedBatchRecords.Columns.Add("IsPrintReport", System.Type.GetType("System.Boolean"))
            'End :Code Added by Dinesh Kanojia BT 2012 : YRS 5.0-2165:RMD enhancements 

            ' Code Added by Dineshk:2013.10.03: BT:-2228: Question regarding record creation via RMD batch processing
            'YRS 5.0-2222 - Incorrct user name as chvCreator
            threadIdentity = System.Security.Principal.WindowsIdentity.GetCurrent()

            For Each l_DataGridItem As DataGridItem In dgMRD.Items

                l_CheckBox = l_DataGridItem.FindControl("ChkSelect")

                If Not l_CheckBox Is Nothing Then
                    If l_CheckBox.Checked = True Then
                        If l_DataGridItem.Cells.Item(3).Text.Trim = String.Empty Then
                        Else
                            l_MRDRow = dtSelectedBatchRecords.NewRow

                            'Comment code Deleted By SG: 2012.03.16

                            'Added on 20/Apr/2011 BT:785 -If current balance less than $5000 and not single mrd processed then not allow to take MRD option     
                            'Remove defendency on cell in datagrid.
                            Dim dr As DataRow()
                            dr = GetDataRow(l_DataGridItem)
                            l_MRDRow("FundIdNo") = CType(dr(0)("FundIdNo").ToString().Trim, Integer)
                            l_MRDRow("PlanType") = dr(0)("PlanType").ToString().Trim
                            l_MRDRow("CurrentBalance") = CType(dr(0)("CurrentBalance").ToString().Trim, Decimal)
                            l_MRDRow("MRDAmount") = CType(dr(0)("MRDAmount").ToString().Trim, Decimal)
                            l_MRDRow("PaidAmount") = CType(dr(0)("PaidAmount").ToString().Trim, Decimal)
                            l_MRDRow("StatusTypeDescription") = dr(0)("StatusTypeDescription").ToString().Trim
                            l_MRDRow("FundEventID") = dr(0)("FundEventID").ToString().Trim
                            l_MRDRow("MRDYear") = CType(dr(0)("MRDYear").ToString().Trim, Integer)
                            l_MRDRow("Process_Status") = dr(0)("Process_Status").ToString().Trim
                            l_MRDRow("IsLocked") = CType(dr(0)("IsLocked").ToString().Trim, Boolean)
                            l_MRDRow("IsBlocked") = CType(dr(0)("IsBlocked").ToString().Trim, Integer)
                            l_MRDRow("IsMultipleYearMRD") = CType(dr(0)("IsMultipleYearMRD").ToString().Trim, Integer)
                            l_MRDRow("IsLess5kFirstTimeMrd") = CType(dr(0)("IsLess5kFirstTimeMrd").ToString().Trim, Integer)
                            'Start   Dinesh Kanojia      2013.10.28          BT:2012 : YRS 5.0-2063: Handling RMD candidates
                            l_MRDRow("Name") = dr(0)("Name").ToString().Trim
                            l_MRDRow("ProcessDate") = lblDate.Text
                            l_MRDRow("WithholdingPercent") = dr(0)("WithHoldingTax").ToString().Trim
                            l_MRDRow("IsPrintReport") = 0
                            'End  Dinesh Kanojia      2013.10.28           BT:2012 : YRS 5.0-2063: Handling RMD candidates
                            dtSelectedBatchRecords.Rows.Add(l_MRDRow)
                        End If
                    End If
                End If
            Next
            dtSelectedBatchRecords.AcceptChanges()


            blnSuccess = True 'Intialise the boolean variable
            For i = 0 To numberOfThreads - 1
                Dim t As Threading.Thread

                t = New Threading.Thread(AddressOf Process)
                t.Name = i
                t.Start()
                alThreads.Add(t)
                't = New Threading.Thread(objService.SaveRefundRequestAndProcess(SaveArraylistPropertys(dtSelectedBatchRecords.Rows(i).Item("FundEventID").ToString(), dtSelectedBatchRecords.Rows(i).Item("PlanType").ToString(), dtSelectedBatchRecords.Rows(i).Item("MRDYear").ToString()), True))
            Next

            For i = 0 To numberOfThreads - 1
                DirectCast(alThreads(i), Threading.Thread).Join()
            Next

            If dtSelectedBatchRecords.Rows.Count > 0 Then
                cloneDS.Tables.Add(dtSelectedBatchRecords)
                cloneDS.Tables(0).TableName = "BatchRecords"
                Session("dsMRD") = cloneDS
                dtSelectedBatchRecords.AcceptChanges()

                'Comment code Deleted By SG: 2012.03.16

                dgMRD.Visible = False
                dgMRD.DataBind()
                dgMrdProcessStatus.Visible = True
                dgMrdProcessStatus.DataSource = dtSelectedBatchRecords
                dgMrdProcessStatus.DataBind()

                'Start :Code Added by Dinesh Kanojia BT 2012 : YRS 5.0-2165:RMD enhancements 
                Session("DisburseMRDRecords") = dtSelectedBatchRecords
                'End :Code Added by Dinesh Kanojia BT 2012 : YRS 5.0-2165:RMD enhancements 

                MessageBox.Show(PlaceHolder1, "MRD Batch Process", GetRMDMessage("MESSAGE_BATCH_PROCESS"), MessageBoxButtons.OK)
            Else
                MessageBox.Show(PlaceHolder1, "MRD Batch Process", GetRMDMessage("MESSAGE_SELECT_RECORDS_BATCH_PROCESS"), MessageBoxButtons.OK)
            End If

            'Comment code Deleted By SG: 2012.03.16

        Catch ex As Exception
            ExceptionPolicy.HandleException(ex, "Exception Policy")
            Throw ex
        End Try

    End Sub

#End Region

    <Serializable()> _
    Public Class SavePropertys

        Private m_FundEventID As String
        Public Property FundEventID() As String
            Get
                Return m_FundEventID
            End Get
            Set(ByVal value As String)
                m_FundEventID = value
            End Set
        End Property

        Private m_RetirementRefundType As String
        Public Property RetirementPlanWithdrawalType() As String
            Get
                Return m_RetirementRefundType
            End Get
            Set(ByVal value As String)
                m_RetirementRefundType = value
            End Set
        End Property

        Private m_SavingsRefundType As String
        Public Property SavingsPlanWithdrawalType() As String
            Get
                Return m_SavingsRefundType
            End Get
            Set(ByVal value As String)
                m_SavingsRefundType = value
            End Set
        End Property

        Private m_RetirementPlanPartialAmount As String
        Public Property RetirementPlanPartialAmount() As String
            Get
                Return m_RetirementPlanPartialAmount
            End Get
            Set(ByVal value As String)
                m_RetirementPlanPartialAmount = value
            End Set
        End Property

        Private m_SavingsPlanPartialAmount As String
        Public Property SavingsPlanPartialAmount() As String
            Get
                Return m_SavingsPlanPartialAmount
            End Get
            Set(ByVal value As String)
                m_SavingsPlanPartialAmount = value
            End Set
        End Property

        Private m_RetirementAcounts As String()
        Public Property SelectedRetirementPlanAccounts() As String()
            Get
                Return m_RetirementAcounts
            End Get
            Set(ByVal value As String())
                m_RetirementAcounts = value
            End Set
        End Property

        Private m_SlectedSavingsAcounts As String()
        Public Property SelectedSavingsPlanAccounts() As String()
            Get
                Return m_SlectedSavingsAcounts
            End Get
            Set(ByVal value As String())
                m_SlectedSavingsAcounts = value
            End Set
        End Property
        'MRD PHASE-II
        Private m_RetirementMRDYear As Integer
        Public Property RetirementMRDYear() As Integer
            Get
                Return m_RetirementMRDYear
            End Get
            Set(ByVal value As Integer)
                m_RetirementMRDYear = value
            End Set
        End Property
        Private m_SavingsMRDYear As Integer
        Public Property SavingsMRDYear() As Integer
            Get
                Return m_SavingsMRDYear
            End Get
            Set(ByVal value As Integer)
                m_SavingsMRDYear = value
            End Set
        End Property
        'Start - SR:2014.03.03-BT 2139/YRS 5.0-2165:Reverted changes done on 2014.02.15
        '' Start:2014.01.29:SR - BT 2139: YRS 5.0-2165: define property for MRD processing date as paramter to webservice for displaying report based on  RMD processing date
        ''Private m_RMDProcessingDate As String
        ''Public Property RMDProcessingDate() As String
        ''    Get
        ''        Return m_RMDProcessingDate
        ''    End Get
        ''    Set(ByVal value As String)
        ''        m_RMDProcessingDate = value
        ''    End Set
        ''End Property
        '' End:2014.01.29:SR - BT 2139: YRS 5.0-2165: define property for MRD processing date as paramter to webservice for displaying report based on  RMD processing date
        'End - SR:2014.03.03-BT 2139/YRS 5.0-2165:Reverted changes done on 2014.02.15

        'Private m_RetirementMRDAmount As Decimal
        'Public Property RetirementMRDAmount() As Decimal
        '    Get
        '        Return m_RetirementMRDAmount
        '    End Get
        '    Set(ByVal value As Decimal)
        '        m_RetirementMRDAmount = value
        '    End Set
        'End Property
        'Private m_SavingsMRDAmount As Decimal
        'Public Property SavingsMRDAmount() As Decimal
        '    Get
        '        Return m_SavingsMRDAmount
        '    End Get
        '    Set(ByVal value As Decimal)
        '        m_SavingsMRDAmount = value
        '    End Set
        'End Property
    End Class
    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancel.Click
        Try
            Session("dsMRD") = Nothing
            'Start: 2014.02.12 : BT 2139/YRS 5.0-2165 - Clear objects
            ViewState("checkBoxArray") = Nothing
            Session("RMD_CHECKED_ITEMS") = Nothing
            'End: 2014.02.12 : BT 2139/YRS 5.0-2165 - Clear objects
            'LblMessage.Text = ""
            Response.Redirect("MainWebForm.aspx", False)
        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
        End Try
    End Sub
    Protected Sub btnLoad_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnLoad.Click
        Try
            'Dim dtmProcessDate As DateTime
            'Dim iDay, iMonth, iYear As Integer
            'Dim dtmNextDate As DateTime
            'dtmProcessDate = Convert.ToDateTime(lblDate.Text)
            'dtmProcessDate = dtmProcessDate.AddYears(1)
            'iYear = dtmProcessDate.Year
            'dtmNextDate = Convert.ToDateTime(iYear.ToString + "/03/31")
            'If (dtmNextDate) Then
            '    btnPrintLetter.Enabled = True
            'End If

            ViewState("PrintLletter") = ""
            ViewState("previousSearchSortExpression") = ""   'SR:2014.01.29 -BT 2139: YRS 5.0-2165: clear viewstate for sorting
            populateMRDDetails()
            'Start :Code Added by Dinesh Kanojia BT 2012 : YRS 5.0-2165:RMD enhancements 
            'btnProcess.Enabled = True

            'btnPrintReport.Enabled = True
            btnPrintLetter.Text = "Print Letters"
            btnProcess.Text = "Process Distributions"
            'End :Code Added by Dinesh Kanojia BT 2012 : YRS 5.0-2165:RMD enhancements 
            EnableDisablePrintLetterOption()
        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
        End Try
    End Sub
    Private Sub btnCloseCurrentMRD_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCloseCurrentMRD.Click
        'START: SG: 2012.03.15: BT-1011
        Dim intCntUnsatisfiedRMD As Integer = 0

        If dgMRD.Items.Count > 0 Then
            intCntUnsatisfiedRMD = dgMRD.Items.Count
        Else
            ds_MRDRecords = YMCARET.YmcaBusinessObject.MRDBO.GetBatchMRDRecords()
            Session("dsMRD") = ds_MRDRecords 'SR:2014.02.25 - BT 2139/YRS 5.0-2165 - Maintain state to avoid Object refernce error on click of CloseCurrentMRD button
            If ds_MRDRecords.Tables.Count > 0 And ds_MRDRecords.Tables("BatchRecords").Rows.Count > 0 Then
                intCntUnsatisfiedRMD = ds_MRDRecords.Tables("BatchRecords").Rows.Count
            End If
        End If

        If intCntUnsatisfiedRMD > 0 Then
            Dim ErrMessage As String
            ErrMessage = GetRMDMessage("MESSAGE_UNSATISFIED_RMD_CONTINUE").Replace("@CntUnsatisfiedRMD", intCntUnsatisfiedRMD.ToString())
            MessageBox.Show(PlaceHolder1, "Current MRD Process", ErrMessage, MessageBoxButtons.YesNo)
        Else
            MessageBox.Show(PlaceHolder1, "Current MRD Process", GetRMDMessage("MESSAGE_CONFIRM_CLOSE_RMD_PROCESS"), MessageBoxButtons.YesNo)
        End If
        'END: SG: 2012.03.15: BT-1011 
    End Sub
    Private Sub btnProcess_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnProcess.Click
        Try
            'Start :Code Added by Dinesh Kanojia BT 2012 : YRS 5.0-2165:RMD enhancements 
            If btnProcess.Text.Trim.ToUpper() = "PROCESS DISTRIBUTIONS" Then
                Page.RegisterStartupScript("CHEKCALL", "<script language='javascript'>CheckAllCheckBox();</script>")

                Dim l_CheckBox As New CheckBox
                Dim bFlag As Boolean = False
                ViewState("PrintLletter") = ""
                populateMRDDetails()

                For Each l_DataGridItem As DataGridItem In dgMRD.Items
                    l_CheckBox = l_DataGridItem.FindControl("ChkSelect")
                    If Not l_CheckBox Is Nothing Then
                        If l_CheckBox.Enabled Then
                            l_CheckBox.Checked = True
                        End If
                    Else
                        l_CheckBox.Checked = False
                    End If

                    If l_CheckBox.Checked = True Then
                        bFlag = True
                        Exit For
                    End If
                Next

                If Not bFlag Then
                    MessageBox.Show(PlaceHolder1, "MRD Batch Process", GetRMDMessage("MESSAGE_SELECT_RECORDS_BATCH_PROCESS"), MessageBoxButtons.OK)
                    Exit Sub
                End If
                btnProcess.Text = "Disburse"
                btnPrintLetter.Enabled = False
                btnPrintReport.Enabled = False
            ElseIf btnProcess.Text.Trim.ToUpper() = "DISBURSE" Then
                If dgMRD.Items.Count > 0 Then
                    Session("GridItems") = dgMRD
                    ProcessMRD()
                    btnProcess.Text = "Process Distributions"
                    EnableDisablePrintLetterOption() 'SR:2014.01.29 -BT 2139: YRS 5.0-2165:Enable/disable button after disbursement
                    Session("ReportName") = "Processed Disbursement Report"
                    Dim dtDisburseMRD As DataTable
                    dtDisburseMRD = GetMRDDisbursement()
                    If Not dtDisburseMRD Is Nothing Then
                        If dtDisburseMRD.Rows.Count > 0 Then
                            Dim popupScript1 As String = "<script language='javascript'>" & _
                           "window.open('CallReport.aspx', 'ReportPopUp1', " & _
                           "'width=1024,height=768, menubar=no, resizable=yes,top=0,left=0, scrollbars=yes, status=yes')" & _
                           "</script>"
                            If (Not Me.IsStartupScriptRegistered("PopupScript1")) Then
                                Page.RegisterStartupScript("PopupScript1", popupScript1)
                            End If
                        End If
                    End If
                Else
                    MessageBox.Show(PlaceHolder1, "MRD Batch Process", GetRMDMessage("MESSAGE_LOAD_RECORDS_PROCESS"), MessageBoxButtons.OK)
                End If
                'End Code Added by Dinesh Kanojia BT 2012 : YRS 5.0-2165:RMD enhancements 
            End If

        Catch ex As Exception
            HelperFunctions.LogException("Button Process --> Button_Click ", ex)
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
        End Try
    End Sub

    'Private Sub btnSelect_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSelect.Click

    '    Dim l_CheckBox As CheckBox
    '    Dim l_bool_flag As Boolean
    '    Dim intSelected As Integer
    '    Try
    '        If dgMRD.Items.Count > 0 Then
    '            If btnSelect.Text = "Select None" Then
    '                l_bool_flag = False
    '                intSelected = 0
    '                btnSelect.Text = "Select All"
    '            Else
    '                l_bool_flag = True
    '                intSelected = 1
    '                btnSelect.Text = "Select None"
    '            End If
    '            'PopulateData()
    '            For Each l_DataGridItem As DataGridItem In dgMRD.Items
    '                l_CheckBox = l_DataGridItem.FindControl("ChkSelect")
    '                l_CheckBox.Checked = intSelected
    '            Next
    '        End If
    '    Catch ex As Exception
    '        Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
    '    End Try
    'End Sub

    Private Sub Process()
        Dim blnSuccess As Boolean = True
        Dim objService As New YRSWebService.YRSWithdrawalService
        Dim objWebServiceReturn As New YRSWebService.WebServiceReturn
        Dim stepCount As Integer = Integer.Parse(Threading.Thread.CurrentThread.Name)
        objService.PreAuthenticate = True
        objService.Credentials = System.Net.CredentialCache.DefaultCredentials
        Dim exMsg As Exception

        Try
            ' Code Added by Dineshk:2013.10.03: BT:-2228: Question regarding record creation via RMD batch processing
            'YRS 5.0-2222 - Incorrct user name as chvCreator
            Dim strIsImpersonation As String = String.Empty
            strIsImpersonation = ConfigurationManager.AppSettings("ReadMRDImpersonation").ToString()
            If (strIsImpersonation.ToUpper = "TRUE") Then
                threadIdentity.Impersonate()
            End If

            For i = stepCount To dtSelectedBatchRecords.Rows.Count - 1 Step numberOfThreads

                'Start- SR:2014.03.03-BT 2139/YRS 5.0-2165:Reverted changes done on 2014.02.15
                objWebServiceReturn = objService.SaveRefundRequestAndProcess(SaveArraylistPropertys(dtSelectedBatchRecords.Rows(i).Item("FundEventID").ToString(), dtSelectedBatchRecords.Rows(i).Item("PlanType").ToString(), dtSelectedBatchRecords.Rows(i).Item("MRDYear").ToString()), True)
                'objWebServiceReturn = objService.SaveRefundRequestAndProcess(SaveArraylistPropertys(dtSelectedBatchRecords.Rows(i).Item("FundEventID").ToString(), dtSelectedBatchRecords.Rows(i).Item("PlanType").ToString(), dtSelectedBatchRecords.Rows(i).Item("MRDYear").ToString(), IIf(String.IsNullOrEmpty(lblDate.Text.Trim), dtSelectedBatchRecords.Rows(i).Item("ProcessDate").ToString(), lblDate.Text.Trim)), True)
                'End- SR:2014.03.03-BT 2139/YRS 5.0-2165:Reverted changes done on 2014.02.15

                'objWebServiceReturn = objService.SaveRefundRequest(SaveArraylistPropertys(ArraySlectedRetirementAccounts, ArraySlectedSavingsAccounts), True)
                If objWebServiceReturn.ReturnStatus.Equals(YRSWebService.Status.Success) Then
                    Session("ServiceRequestReport") = objWebServiceReturn.WebServiceDataSet
                    dtSelectedBatchRecords.Rows(i).Item("Process_Status") = "Success : MRD Process completed Successfully for FundId No. - " + dtSelectedBatchRecords.Rows(i).Item("FundIdNo").ToString()
                    'Code Added by Dinesh Kanojia BT 2012 : YRS 5.0-2165:RMD enhancements 
                    'To print Disburse report
                    dtSelectedBatchRecords.Rows(i).Item("IsPrintReport") = 1
                    Logger.Write("Success", "Application", 0, 0, System.Diagnostics.TraceEventType.Verbose)
                ElseIf objWebServiceReturn.ReturnStatus.Equals(YRSWebService.Status.Warning) Then
                    If (Not objWebServiceReturn.MessageCode Is Nothing) AndAlso (objWebServiceReturn.MessageCode.ToUpper() = "MESSAGE_WITHDRAWAL_CopyToServerError".ToUpper()) Then
                        Session("ServiceRequestReport") = objWebServiceReturn.WebServiceDataSet
                    End If
                    'Session("GenerateErrors") = objWebServiceReturn.Message
                    dtSelectedBatchRecords.Rows(i).Item("Process_Status") = "Warning :" + objWebServiceReturn.Message
                    'Code Added by Dinesh Kanojia BT 2012 : YRS 5.0-2165:RMD enhancements 
                    'To print Disburse report
                    dtSelectedBatchRecords.Rows(i).Item("IsPrintReport") = 1
                    blnSuccess = False
                    exMsg = New Exception(objWebServiceReturn.Message)
                    ExceptionPolicy.HandleException(exMsg, "Exception Policy")
                Else
                    dtSelectedBatchRecords.Rows(i).Item("Process_Status") = "Failed : " + objWebServiceReturn.Message
                    'Code Added by Dinesh Kanojia BT 2012 : YRS 5.0-2165:RMD enhancements 
                    'To print Disburse report
                    dtSelectedBatchRecords.Rows(i).Item("IsPrintReport") = 0
                    blnSuccess = False
                    exMsg = New Exception(objWebServiceReturn.Message)
                    ExceptionPolicy.HandleException(exMsg, "Exception Policy")
                End If
            Next
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub dgMRD_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgMRD.ItemCommand


    End Sub

    Private Sub dgMRD_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgMRD.ItemDataBound
        Try
            Dim chkSelect As CheckBox
            If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
                If String.IsNullOrEmpty(ViewState("PrintLletter")) Then

                    If Convert.ToBoolean(e.Item.DataItem("IsLocked")) = True Then
                        'SG: 2012.03.16: BT-1011
                        'e.Item.BackColor = Drawing.Color.LightGreen
                        e.Item.CssClass = "BG_ColourIsLocked"
                        chkSelect = e.Item.Cells(0).FindControl("ChkSelect")
                        chkSelect.Enabled = False
                    ElseIf Convert.ToInt32(e.Item.DataItem("IsBlocked")) = 1 Then
                        'SG: 2012.03.16: BT-1011
                        'e.Item.BackColor = Drawing.Color.LightPink
                        e.Item.CssClass = "BG_ColourIsBlocked"
                        chkSelect = e.Item.Cells(0).FindControl("ChkSelect")
                        chkSelect.Enabled = False
                    ElseIf Convert.ToInt32(e.Item.DataItem("IsMultipleYearMRD")) = 1 Then
                        'SG: 2012.03.16: BT-1011
                        'e.Item.BackColor = Drawing.Color.LightSkyBlue
                        e.Item.CssClass = "BG_ColourIsMultipleYearMRD"
                        chkSelect = e.Item.Cells(0).FindControl("ChkSelect")
                        chkSelect.Enabled = False
                        'START: SG: 2012.03.15: BT-1011
                        '  BT:785 -If current balance less than $5000 and not single mrd processed then not allow to take MRD option     
                        'ElseIf Convert.ToInt32(e.Item.DataItem("IsLess5kFirstTimeMrd")) = 1 Then
                        '    e.Item.BackColor = Drawing.Color.Chocolate
                        '    chkSelect = e.Item.Cells(0).FindControl("ChkSelect")
                        '    chkSelect.Enabled = False
                        'END: SG: 2012.03.15: BT-1011
                        'Starts Code Added by Dinesh Kanojia 2013.10.28  BT 2012 : YRS 5.0-2063: Handling RMD candidates 
                    ElseIf Not String.IsNullOrEmpty(Convert.ToString(e.Item.DataItem("bitAnnualMRDRequested"))) Then
                        If Convert.ToInt32(e.Item.DataItem("bitAnnualMRDRequested")) = 0 Then
                            e.Item.CssClass = "BG_ColourIsNotEnrollAnnualMRDPayments"
                            chkSelect = e.Item.Cells(0).FindControl("ChkSelect")
                            chkSelect.Enabled = False
                        End If
                    ElseIf String.IsNullOrEmpty(Convert.ToString(e.Item.DataItem("bitAnnualMRDRequested"))) Then
                        e.Item.CssClass = "BG_ColourIsNotEnrollAnnualMRDPayments"
                        chkSelect = e.Item.Cells(0).FindControl("ChkSelect")
                        chkSelect.Enabled = False
                        'End Code Added by Dinesh Kanojia 2013.10.28  BT 2012 : YRS 5.0-2063: Handling RMD candidates 
                    End If

                    If (e.Item.DataItem("PaidAmount") >= e.Item.DataItem("MRDAmount")) Then
                        chkSelect = e.Item.Cells(0).FindControl("ChkSelect")
                        chkSelect.Enabled = False
                        chkSelect.Checked = False
                    End If
                Else
                    'Start:SR:2014.01.29 -BT 2139: YRS 5.0-2165: Add visual effect for participant whom initial letter already sent.
                    If Not String.IsNullOrEmpty(Convert.ToString(e.Item.DataItem("RMDInitialLetterDate"))) Then
                        e.Item.CssClass = "BG_ColourInitialLetter"
                        chkSelect = e.Item.Cells(0).FindControl("ChkSelect")
                        chkSelect.Enabled = True
                    End If
                    'End:SR:2014.01.29 -BT 2139: YRS 5.0-2165: Add visual effect for participant whom initial letter already sent.
                End If

            End If
            'e.Item.Cells(1).Visible = False
            'e.Item.Cells(2).Visible = False
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            HelperFunctions.LogException("Grid  --> Item bound ", ex)
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
    End Sub
    Private Function GetDataRow(ByVal gridItem As DataGridItem) As DataRow()
        Try
            Dim ds_MRDRecords As DataSet
            Dim dr As DataRow()
            If Not Session("dsMRD") Is Nothing Then
                ds_MRDRecords = CType(Session("dsMRD"), DataSet)
                'dr = ds_MRDRecords.Tables("BatchRecords").Select("FundIdNo='" & gridItem.DataItem("FundIdNo") & "' And PlanType='" & gridItem.DataItem("PlanType") & "' ")
                dr = ds_MRDRecords.Tables("BatchRecords").Select("FundIdNo='" & CType(gridItem.Cells.Item(1).Text.Trim, Integer) & "' And PlanType='" & CType(gridItem.Cells.Item(2).Text.Trim, String) & "' ")
            End If
            Return dr
        Catch ex As Exception
            Throw ex
        End Try
    End Function

#Region "SG: 2012.03.16: Function to get Display Message from message code."
    Private Function GetRMDMessage(ByVal stMessageCode As String) As String

        Dim l_Message As String
        l_Message = String.Empty

        If stMessageCode = "MESSAGE_CLOSE_RMD_PROCESS" Then
            l_Message = Resources.RegenerateRMDMessage.MESSAGE_CLOSE_RMD_PROCESS
        End If

        If stMessageCode = "MESSAGE_RMD_DATE_BLANK" Then
            l_Message = Resources.RegenerateRMDMessage.MESSAGE_RMD_DATE_BLANK
        End If

        If stMessageCode = "MESSAGE_CLOSED_SUCCESS" Then
            l_Message = Resources.RegenerateRMDMessage.MESSAGE_CLOSED_SUCCESS
        End If

        If stMessageCode = "MESSAGE_CLOSEED_OR_NODATE" Then
            l_Message = Resources.RegenerateRMDMessage.MESSAGE_CLOSEED_OR_NODATE
        End If

        If stMessageCode = "MESSAGE_BATCH_PROCESS" Then
            l_Message = Resources.RegenerateRMDMessage.MESSAGE_BATCH_PROCESS
        End If

        If stMessageCode = "MESSAGE_SELECT_RECORDS_BATCH_PROCESS" Then
            l_Message = Resources.RegenerateRMDMessage.MESSAGE_SELECT_RECORDS_BATCH_PROCESS
        End If

        If stMessageCode = "MESSAGE_CONFIRM_CLOSE_RMD_PROCESS" Then
            l_Message = Resources.RegenerateRMDMessage.MESSAGE_CONFIRM_CLOSE_RMD_PROCESS
        End If

        If stMessageCode = "MESSAGE_LOAD_RECORDS_PROCESS" Then
            l_Message = Resources.RegenerateRMDMessage.MESSAGE_LOAD_RECORDS_PROCESS
        End If

        If stMessageCode = "MESSAGE_UNSATISFIED_RMD_CONTINUE" Then
            l_Message = Resources.RegenerateRMDMessage.MESSAGE_UNSATISFIED_RMD_CONTINUE
        End If

        'Start :Code Added by Dinesh Kanojia BT 2012 : YRS 5.0-2165:RMD enhancements 
        If stMessageCode = "MESSAGE_PRINT_LETTER" Then
            l_Message = Resources.RegenerateRMDMessage.MESSAGE_PRINT_LETTER
        End If

        If stMessageCode = "MESSAGE_SELECT_INITIAL_LETTERS" Then
            l_Message = Resources.RegenerateRMDMessage.MESSAGE_SELECT_INITIAL_LETTERS
        End If

        If stMessageCode = "MESSAGE_ELIGIBLE_INITIAL_LETTER" Then
            l_Message = Resources.RegenerateRMDMessage.MESSAGE_ELIGIBLE_INITIAL_LETTER
        End If

        'End :Code Added by Dinesh Kanojia BT 2012 : YRS 5.0-2165:RMD enhancements 
        Return l_Message

    End Function
#End Region

#Region "Dinesh Kanojia      2013.10.03          YRS 5.0-2165:RMD enhancements "

#Region "Events"
    Private Sub btnPrintLetter_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrintLetter.Click

        Try
            If btnPrintLetter.Text.Trim.ToUpper = "PRINT LETTERS" Then
                'Page.RegisterStartupScript("CHEKCALL", "<script language='javascript'>CheckAllCheckBox();</script>")
                ViewState("previousSearchSortExpression") = "" 'SR:2014.01.29 -BT 2139: YRS 5.0-2165: clear viewstate for sorting
                GetBatchRMDForInitialLetter()
                PopulateRMDforInitialLetter()
                btnPrintLetter.Text = "Print"
                btnProcess.Enabled = False
                btnPrintReport.Enabled = False
            ElseIf btnPrintLetter.Text.Trim.ToUpper = "PRINT" Then
                Dim chkCheckbox As CheckBox
                Dim bFlag As Boolean = False
                Dim strMessage As String
                For iCount As Integer = 0 To dgMRD.Items.Count - 1
                    chkCheckbox = CType(dgMRD.Items(iCount).Cells(0).FindControl("chkSelect"), CheckBox)
                    If (chkCheckbox.Checked) Then
                        bFlag = True
                        Exit For
                    End If
                Next
                If (Not bFlag) Then
                    MessageBox.Show(PlaceHolder1, "PRINT REPORT", GetRMDMessage("MESSAGE_ELIGIBLE_INITIAL_LETTER"), MessageBoxButtons.Stop, False)
                    Exit Sub
                End If

                'Start:SR:2014.01.29 -BT 2139: YRS 5.0-2165:Save initial letter record for participant.
                For iCount As Integer = 0 To dgMRD.Items.Count - 1
                    chkCheckbox = CType(dgMRD.Items(iCount).Cells(0).FindControl("chkSelect"), CheckBox)
                    If (chkCheckbox.Checked) Then
                        strMessage = YMCARET.YmcaBusinessObject.MRDBO.SaveRMDInitialLetterData(dgMRD.Items(iCount).Cells(12).Text.Trim)
                    End If
                Next
                'End:SR:2014.01.29 -BT 2139: YRS 5.0-2165:Save initial letter record for participant.
                CreateAndCopyMRDForm(True)
                btnPrintLetter.Text = "Print Letters"
                btnProcess.Enabled = True
                'btnPrintReport.Enabled = True
                GetBatchRMDForInitialLetter()
                'PopulateRMDforInitialLetter()
            End If
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            HelperFunctions.LogException("Print Letter --> Button_Click ", ex)
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
    End Sub

    Private Sub btnPrintReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrintReport.Click
        Session("GridItems") = dgMRD
        Session("ReportName") = "RMD Report"
        Dim l_CheckBox As New CheckBox
        Dim bFlag As Boolean = False
        Dim popupScript1 As String = "<script language='javascript'>" & _
           "window.open('CallReport.aspx', 'ReportPopUp1', " & _
           "'width=1024,height=768, menubar=no, resizable=yes,top=0,left=0, scrollbars=yes, status=yes')" & _
           "</script>"
        If (Not Me.IsStartupScriptRegistered("PopupScript1")) Then
            Page.RegisterStartupScript("PopupScript1", popupScript1)
        End If
    End Sub
#End Region

#Region "Function and Procedure"

    'Copy Initial letter to IDM
    Public Sub CreateAndCopyMRDForm(ByVal blnCopyIDM As Boolean)
        Dim l_stringDocType As String = String.Empty
        Dim l_StringReportName As String = String.Empty
        Dim l_ArrListParamValues As New ArrayList
        Dim l_string_OutputFileType As String = String.Empty
        Dim l_StringErrorMessage As String = String.Empty
        Dim alFundNO As New ArrayList
        Try

            If dgMRD.Items.Count > 0 Then
                Dim chkCheckbox As CheckBox
                Dim bFlag As Boolean = False

                Session("FundId") = ""
                For iCount As Integer = 0 To dgMRD.Items.Count - 1
                    chkCheckbox = CType(dgMRD.Items(iCount).Cells(0).FindControl("chkSelect"), CheckBox)
                    If (chkCheckbox.Checked) Then
                        If Not alFundNO.Contains(dgMRD.Items(iCount).Cells(1).Text.Trim) Then 'SR:2014.02.25:BT 2139/YRS 5.0-2165: Check Initial letter already print.
                            Session("FundId") += dgMRD.Items(iCount).Cells(1).Text.Trim + "|"

                            Session("strReportName") = "Initial_RMD_Letter_April_Distribution_Deadline"
                            l_StringReportName = "Initial_RMD_Letter_April_Distribution_Deadline"
                            'Call ReportViewer.aspx 

                            If blnCopyIDM Then
                                l_stringDocType = "MRDLETTR"
                                l_StringReportName = "Initial_RMD_Letter_April_Distribution_Deadline"
                                l_ArrListParamValues.Add(dgMRD.Items(iCount).Cells(1).Text)
                                l_string_OutputFileType = "Initial_RMD_Letter_April_Distribution_Deadline_" + l_stringDocType
                                'Copies report into idm convert into pdf and stores information idx 
                                l_StringErrorMessage = CopyToIDM(l_stringDocType, l_StringReportName, l_ArrListParamValues, l_string_OutputFileType, dgMRD.Items(iCount).Cells(12).Text)
                                If Not l_StringErrorMessage Is String.Empty Then
                                    MessageBox.Show(PlaceHolder1, "IDM Error", l_StringErrorMessage, MessageBoxButtons.Stop, False)
                                End If
                            End If
                            alFundNO.Add(dgMRD.Items(iCount).Cells(1).Text.Trim) 'SR:2014.02.25:BT 2139/YRS 5.0-2165:Add Fundidno for Initial letter printed.
                        End If
                    End If
                Next
                Session("FundId") = Session("FundId").ToString().Substring(0, Session("FundId").ToString().Length - 1)

                Dim popupScript2 As String = "<script language='javascript'>" & _
                        "window.open('FT\\ReportViewer.aspx', 'ReportPopUp2', " & _
                        "'width=1024,height=768, menubar=no, resizable=yes,top=0,left=0, scrollbars=yes, status=yes')" & _
                        "</script>"

                If (Not Me.IsStartupScriptRegistered("PopupScript2")) Then
                    Page.RegisterStartupScript("PopupScript2", popupScript2)
                End If

            End If

            'If blnCopyIDM Then
            '    l_stringDocType = "DTHBENAP"
            '    l_StringReportName = "Death Benefit Application"
            '    l_ArrListParamValues.Add(intDBAppFormID)
            '    l_string_OutputFileType = "DeathBenefit_" + l_stringDocType
            '    'Copies report into idm convert into pdf and stores information idx 
            '    l_StringErrorMessage = CopyToIDM(l_stringDocType, l_StringReportName, l_ArrListParamValues, l_string_OutputFileType)
            '    If Not l_StringErrorMessage Is String.Empty Then
            '        MessageBox.Show(PlaceHolder1, "IDM Error", l_StringErrorMessage, MessageBoxButtons.Stop, False)
            '    End If
            '    'Call ReportViewer.aspx 
            'End If
        Catch
            Throw
        End Try
    End Sub

    Private Function CopyToIDM(ByVal l_StringDocType As String, ByVal l_StringReportName As String, ByVal l_ArrListParamValues As ArrayList, ByVal l_string_OutputFileType As String, ByVal strPerssID As String) As String
        Dim l_StringErrorMessage As String
        Dim IDM As New IDMforAll
        Try
            'Anudeep 04.12.2012 Code changes to copy report into IDM folder
            'gets the columns for idm and stored in session varilable 

            If Session("FTFileList") Is Nothing Then
                If IDM.DatatableFileList(False) Then
                    Session("FTFileList") = IDM.SetdtFileList
                End If
            End If

            If Not Session("FTFileList") Is Nothing Then
                IDM.SetdtFileList = DirectCast(Session("FTFileList"), DataTable)
            End If
            'Assign values to the properties
            IDM.PreviewReport = True
            IDM.LogonToDb = True
            IDM.CreatePDF = True
            IDM.CreateIDX = True
            IDM.CopyFilesToIDM = True
            IDM.AppType = "P"

            'IDM.PersId = Session("String_PersId")
            IDM.PersId = strPerssID
            IDM.DocTypeCode = l_StringDocType
            IDM.OutputFileType = l_string_OutputFileType
            IDM.ReportName = l_StringReportName.ToString().Trim & ".rpt"
            IDM.ReportParameters = l_ArrListParamValues

            l_StringErrorMessage = IDM.ExportToPDF()
            l_ArrListParamValues.Clear()

            Session("FTFileList") = IDM.SetdtFileList


            If Not Session("FTFileList") Is Nothing Then
                Try
                    Dim popupScriptCopytoServer As String = "var a = window.open('FT\\CopyFilestoFileServer.aspx?OR=1&CO=1', 'FileCopyPopUp', " & _
                    "'width=1,height=1, menubar=no, resizable=yes,top=1300,left=1300, scrollbars=no, status=no');"
                    ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "popupScriptCopytoServer", popupScriptCopytoServer, True)
                Catch
                    Throw
                End Try
            End If

            Return l_StringErrorMessage

        Catch ex As Exception
            HelperFunctions.LogException("MRD Process --> Copy to IDM -->" + ex.Message.Trim.ToString(), ex)
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()))
        Finally
            IDM = Nothing
        End Try
    End Function
    ' Print SSRS RMD report
    Public Function GetSelectedRMDRecords() As DataSet
        Dim cloneDS As New DataSet
        Dim l_CheckBox As CheckBox
        Dim dsPrinReportData As DataSet
        Dim l_MRDRow As DataRow
        Try
            dsPrinReportData = YMCARET.YmcaBusinessObject.MRDBO.GetBatchRMDForPrintReport(Session("ProcessingDate").ToString)
            Return dsPrinReportData
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
    End Function

    'Print SSRS report for Disburse RMD report
    Public Function GetMRDDisbursement() As DataTable
        Dim dtDisburseMRD As DataTable
        Dim dvDisburseMRD As DataView = Nothing
        Try
            If Not Session("DisburseMRDRecords") Is Nothing Then
                dtDisburseMRD = CType(Session("DisburseMRDRecords"), DataTable)
                If (dtDisburseMRD.Rows.Count > 0) Then
                    dvDisburseMRD = New DataView(dtDisburseMRD)
                    dvDisburseMRD.RowFilter = "IsPrintReport = 1"
                End If
            Else
                Return (CType(Session("DisburseMRDRecords"), DataTable))
            End If
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
        Return dvDisburseMRD.ToTable
    End Function

    'Get BatchRMD records to print initial letters.
    Private Sub GetBatchRMDForInitialLetter()
        Dim dtBatchRMD As New DataSet
        Try
            'Start: 2014.02.12 : BT 2139/YRS 5.0-2165 - Clear objects
            ViewState("checkBoxArray") = Nothing
            Session("RMD_CHECKED_ITEMS") = Nothing
            'End: 2014.02.12 : BT 2139/YRS 5.0-2165 - Clear objects
            'dtBatchRMD = YMCARET.YmcaBusinessObject.MRDBO.GetBatchRMDForInitialLetter()
            dtBatchRMD = YMCARET.YmcaBusinessObject.MRDBO.GetBatchMRDRecords()
            If Not dtBatchRMD Is Nothing Then
                If dtBatchRMD.Tables(1).Rows.Count > 0 Then
                    ViewState("PrintLletter") = "No"
                    dgMRD.DataSource = dtBatchRMD.Tables(1) 'SR:2014.01.29 -BT 2139: Get RMD recorde from table 1 for current Processing date.
                    dgMRD.DataBind()
                    Session("dsMRD") = dtBatchRMD
                End If
            End If

            'Start:SR:2014.01.29 -BT 2139: Get current RMD Processing date.
            If HelperFunctions.isNonEmpty(dtBatchRMD) Then
                dtmRMDProcesingDate = IIf(Not String.IsNullOrEmpty(dtBatchRMD.Tables("ProcessDate").Rows(0).Item(0).ToString()), DateTime.Parse(dtBatchRMD.Tables("ProcessDate").Rows(0).Item(0).ToString().Trim), DateTime.Parse(lblDate.Text.Trim))
            End If
            'End:SR:2014.01.29 -BT 2139: Get current RMD Processing date.

        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
    End Sub
    'Start-SR:BT 2139: YRS 5.0-2165:RMD enhancements 
    'Enable Disable PrintLetter button based on current processing date
    Private Sub EnableDisablePrintLetterOption()
        Dim dtmInitialLetterdate As DateTime
        Try
            If lblDate.Text <> "" Then
                dtmInitialLetterdate = Convert.ToDateTime(lblDate.Text.Trim)
                If dtmInitialLetterdate.Month = 3 And dtmInitialLetterdate.Day = 31 Then
                    btnPrintLetter.Enabled = True
                Else
                    btnPrintLetter.Enabled = False
                End If
            End If
        Catch ex As Exception
            Throw
        End Try

    End Sub
    'End - SR:BT 2139: YRS 5.0-2165:RMD enhancements

#End Region

#End Region
    'Start-SR:2014.01.29:BT 2139: YRS 5.0-2165:RMD enhancements 
    ' Save selected records in cache before sorting
    Private Sub SaveCheckedValues()
        Dim userdetails As New ArrayList()
        Dim index As String = "-1"
        ' For select all
        Dim CheckBoxIndex As Integer
        Dim CheckAllWasChecked As Boolean = False
        Dim ctrl As Table
        Try
            ctrl = dgMRD.Controls(0)
            If ViewState("checkBoxArray") IsNot Nothing Then
                checkBoxArray = DirectCast(ViewState("checkBoxArray"), ArrayList)
            Else
                checkBoxArray = New ArrayList()
            End If
            Dim chkAll As New CheckBox
            chkAll = DirectCast(ctrl.Rows(0).FindControl("CheckBox2"), CheckBox)
            Dim checkAllIndex As String = "chkAll-" & dgMRD.CurrentPageIndex

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

            'For Each l_DataGridItem As DataGridItem In dgMRD.Items
            For Each dgi As DataGridItem In dgMRD.Items

                index = dgMRD.DataKeys(dgi.ItemIndex).ToString()
                Dim result As Boolean = DirectCast(dgi.FindControl("chkSelect"), CheckBox).Checked  ' Check in the Session 
                If Session("RMD_CHECKED_ITEMS") IsNot Nothing Then
                    userdetails = DirectCast(Session("RMD_CHECKED_ITEMS"), ArrayList)
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
                Session("RMD_CHECKED_ITEMS") = userdetails
            End If
        Catch ex As Exception
            HelperFunctions.LogException("RMD Batch Process --> SaveCheckedValues() -->" + ex.Message.Trim.ToString(), ex)
            Throw
        End Try
    End Sub
    'End-SR:2014.01.29:BT 2139: YRS 5.0-2165:RMD enhancements 
    'Start-SR:2014.01.29:BT 2139: YRS 5.0-2165:RMD enhancements 
    ' Populate selected records from cache after sorting
    Private Sub PopulateCheckedValues()
        Try
            'For Select All check box
            Dim checkBoxArray As ArrayList = DirectCast(ViewState("checkBoxArray"), ArrayList)
            Dim checkAllIndex As String = "chkAll-" & dgMRD.CurrentPageIndex
            Dim ctrl As Table

            ctrl = dgMRD.Controls(0)
            If checkBoxArray.IndexOf(checkAllIndex) <> -1 Then
                Dim chkAll As CheckBox = DirectCast(ctrl.Rows(0).FindControl("CheckBox2"), CheckBox)
                chkAll.Checked = True
            End If
            'end, For Select All check box
            Dim userdetails As ArrayList = DirectCast(Session("RMD_CHECKED_ITEMS"), ArrayList)
            If userdetails IsNot Nothing AndAlso userdetails.Count > 0 Then
                For Each l_DataGridItem As DataGridItem In dgMRD.Items
                    Dim index As String = dgMRD.DataKeys(l_DataGridItem.ItemIndex).ToString()
                    If userdetails.Contains(index) Then
                        Dim myCheckBox As CheckBox = DirectCast(l_DataGridItem.FindControl("chkSelect"), CheckBox)
                        myCheckBox.Checked = True
                    End If
                Next
            End If
        Catch ex As Exception
            HelperFunctions.LogException("RMD Batch Process --> PopulateCheckedValues() -->" + ex.Message.Trim.ToString(), ex)
            Throw
        End Try
    End Sub
    'End:SR:2014.01.29:BT 2139/YRS 5.0-2165:RMD enhancements 

    Private Sub Sort(ByVal dg As DataGrid, ByVal SortExpression As String)
        Dim dv As New DataView
        Dim ds As DataSet
        Try
            Select Case dg.ID
                Case "dgMRD"
                    ds = Session("dsMRD")
                Case Else
                    Throw New Exception("Sorting is not handling in " & dg.ID)
            End Select

            If Not ds.Tables("BatchRecords") Is Nothing Then
                dv = ds.Tables("BatchRecords").DefaultView
            End If


            If Not ds.Tables("BatchRMDForInitialLetter") Is Nothing Then
                dv = ds.Tables("BatchRMDForInitialLetter").DefaultView
            End If

            'BatchRMDForInitialLetter()
            If Not ViewState(dg.ID) Is Nothing Then
                If ViewState(dg.ID).ToString.Trim.EndsWith("ASC") Then
                    dv.Sort = "[" + SortExpression + "] DESC"
                Else
                    dv.Sort = "[" + SortExpression + "] ASC"
                End If
            Else
                dv.Sort = "[" + SortExpression + "] ASC"
            End If

            dg.DataSource = Nothing
            dg.DataSource = dv
            dg.DataBind()
            ViewState(dg.ID) = dv.Sort
        Catch ex As Exception
            HelperFunctions.LogException("RMD Batch Process --> Sort() --> " + ex.Message.Trim.ToString(), ex)
            Throw
        End Try
    End Sub
    Public Sub PopulateRMDforInitialLetter()
        Dim chkCheckbox As CheckBox
        Dim cheHeaderCheckBox As CheckBox
        Dim bFlag As Boolean = False
        Dim dt As DataGridItem
        Dim ctrl As Table
        Dim dtmRMDExpiryDate As DateTime
        Try
            ctrl = dgMRD.Controls(0)
            cheHeaderCheckBox = CType(ctrl.Rows(0).FindControl("CheckBox2"), CheckBox)
            cheHeaderCheckBox.Checked = True
            For iCount As Integer = 0 To dgMRD.Items.Count - 1
                chkCheckbox = CType(dgMRD.Items(iCount).Cells(0).FindControl("chkSelect"), CheckBox)
                'Start:SR:2014.01.29 -BT 2139: YRS 5.0-2165: Get RMD expiry date
                If Not String.IsNullOrEmpty(dgMRD.Items(iCount).Cells(11).Text.ToString) Then
                    dtmRMDExpiryDate = DateTime.Parse(dgMRD.Items(iCount).Cells(11).Text.Trim)
                Else
                    dtmRMDExpiryDate = DateTime.Parse(lblDate.Text.Trim)
                End If
                'End: SR:2014.01.29 -BT 2139: YRS 5.0-2165: Get RMD expiry date

                If Not chkCheckbox Is Nothing Then
                    'If Convert.ToDecimal(dgMRD.Items(iCount).Cells(5).Text) >= Convert.ToDecimal(dgMRD.Items(iCount).Cells(4).Text) Then
                    'SR:2014.01.29 -BT 2139: YRS 5.0-2165: Enable only current RMD processing records
                    If dtmRMDProcesingDate = dtmRMDExpiryDate Then
                        chkCheckbox.Checked = True
                    Else
                        chkCheckbox.Enabled = False
                        chkCheckbox.Checked = False
                        cheHeaderCheckBox.Checked = False
                    End If
                End If
                'Start:SR:2014.01.29 -BT 2139: YRS 5.0-2165:Uncheck for participant whom initial letter already sent.
                If Not String.IsNullOrEmpty(dgMRD.Items(iCount).Cells(16).Text.Replace("&nbsp;", "")) Then
                    chkCheckbox.Enabled = True
                    chkCheckbox.Checked = False
                    cheHeaderCheckBox.Checked = False
                End If
                'End:SR:2014.01.29 -BT 2139: YRS 5.0-2165: Uncheck for participant whom initial letter already sent.
            Next
        Catch ex As Exception
            HelperFunctions.LogException("RMD Batch Process --> PopulateRMDforInitialLetter() -->" + ex.Message.Trim.ToString(), ex)
            Throw
        End Try
        
        End sub
End Class
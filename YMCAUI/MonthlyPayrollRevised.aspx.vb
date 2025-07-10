

'*******************************************************************************
' Copyright YMCA Retirement Fund All Rights Reserved. 
'
' Project Name		:	YMCA Yrs
' FileName			:	MonthlyPayrollRevised.aspx.vb

' Author Name		:	Ragesh .V.P
' Employee ID		:	34231
' Email				:	ragesh.vp@3i-infotech.com
' Contact No		:	55928736

'History
'***********
' Author Name		:	Vartika
' Employee ID		:	33495
' Email				:	vartika.jain@3i-infotech.com
' Contact No		:	55928733
' Creation Time		:	5/18/05 2:03:48 PM
' Program Specification Name	:	
' Unit Test Plan Name			:	
' Description					:	<<Please put the brief description here...>>
'
' Changed by			:	Vartika Jain    
' Changed on			:	05/23/2005
' Change Description	:	Applying Style Sheet
'****************************************************
'Modification History
'****************************************************
'Modified by             Date          Description
'****************************************************
'Neeraj Singh            12/Nov/2009   Added form name for security issue YRS 5.0-940 
'Shashi Shekhar Singh   :02-Dec-2010 : For BT- 685 - on payroll process screen, system showing error message - "There is no row at position 0." 
'Prasad Jadhav          :12-July-2011: BT-855 YRS 5.0-1344 : Create new IAT output file
'Manthan Rajguru         2015.09.16    YRS-AT-2550: YRS data cleanup: copyright comments and namespace/reference (Changed the Namespace/reference from 'Infotech' to 'YMCARET')
'Manthan Rajguru         2017.01.26    YRS-AT-3288 -  YRS bug: bitActive and bitPrimary validation for Annuity Processing 
'Megha Lad               2020.01.08    YRS-AT-4602 - YRS enh:State Withholding Project - Export file Annuity Payroll.
'Sanjay Singh            2020.01.19    YRS-AT-4641 - State Tax Withholding- New screen for "Monthly Annuity Payroll Processing" (Reverse Feed) 
'Pooja Kumkar            2020.01.23    YRS-AT-4641 - YRS REPORTS: State Tax Withholding- New screen for "Monthly Annuity Payroll Processing" (Reverse Feed) 
'*******************************************************************************


Public Class MonthlyPayrollRevised
    Inherits System.Web.UI.Page
    'below line is Added by Neeraj for issue id YRS 5.0-940 
    Dim strFormName As String = New String("MonthlyPayrollRevised.aspx")
    'End issue id YRS 5.0-940

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Private Enum EnableMode
        Poof_Report
        Process
    End Enum
    Private Enum LoadDatasetMode
        Table
        Cache
    End Enum
    Private Property SessionPayrollMonth() As String
        Get
            If Not (Session("Integer_PayrollMonth")) Is Nothing Then
                Return (CType(Session("Integer_PayrollMonth"), String).Trim)
            Else
                Return 0
            End If
        End Get
        Set(ByVal Value As String)
            Session("Integer_PayrollMonth") = Value
        End Set
    End Property
    Private Property SessionLongUSCheckNo() As Long
        Get
            If Not (Session("Long_USCheckNo")) Is Nothing Then
                Return (CType(Session("Long_USCheckNo"), System.Int32).ToString)
            Else
                Return 0
            End If
        End Get
        Set(ByVal Value As Long)
            Session("Long_USCheckNo") = Value
        End Set
    End Property
    Private Property SessionLongUSCheckSeriesPrefix() As Char
        Get
            If Not (Session("Long_USCheckSeriesPrefix")) Is Nothing Then
                Return (CType(Session("Long_USCheckSeriesPrefix"), String))
            Else
                Return ""
            End If
        End Get
        Set(ByVal Value As Char)
            Session("Long_USCheckSeriesPrefix") = Value
        End Set
    End Property
    Private Property SessionLongCANADACheckSeriesPrefix() As Char
        Get
            If Not (Session("Long_CANADACheckSeriesPrefix")) Is Nothing Then
                Return (CType(Session("Long_CANADACheckSeriesPrefix"), String))
            Else
                Return ""
            End If
        End Get
        Set(ByVal Value As Char)
            Session("Long_CANADACheckSeriesPrefix") = Value
        End Set
    End Property

    Private Property SessionLongCANADACheckNo() As Long
        Get
            If Not (Session("Long_CANADACheckNo")) Is Nothing Then
                Return (CType(Session("Long_CANADACheckNo"), System.Int32).ToString)
            Else
                Return 0
            End If
        End Get
        Set(ByVal Value As Long)
            Session("Long_CANADACheckNo") = Value
        End Set
    End Property
    Private Property SessionPayrollDate() As Date
        Get
            If Not (Session("Date_PayrollDate")) Is Nothing Then
                Return (CType(Session("Date_PayrollDate"), System.DateTime).Date())
            Else
                Return Convert.ToDateTime("01/01/1900")
            End If
        End Get
        Set(ByVal Value As Date)
            Session("Date_PayrollDate") = Value
        End Set
    End Property
    Private Property SessionPayrollMonthDate() As Date
        Get
            If Not (Session("Date_PayrollMonthDate")) Is Nothing Then
                Return (CType(Session("Date_PayrollMonthDate"), System.DateTime).Date())
            Else
                Return Convert.ToDateTime("01/01/1900")
            End If
        End Get
        Set(ByVal Value As Date)
            Session("Date_PayrollMonthDate") = Value
        End Set
    End Property

    Private Property SessionNextBusinessDay() As Date
        Get
            If Not (Session("Date_NextBusinessDay")) Is Nothing Then
                Return (CType(Session("Date_NextBusinessDay"), System.DateTime).Date())
            Else
                Return Convert.ToDateTime("01/01/1900")
            End If
        End Get
        Set(ByVal Value As Date)
            Session("Date_NextBusinessDay") = Value
        End Set
    End Property

    'START : SR | 2020.01.08 | YRS-AT-4602 | declare property for outsourcing payment key.
    Private Property IsPaymentOutSourcingKeyON() As Boolean
        Get
            If Not (ViewState("IsPaymentOutSourcingKeyON")) Is Nothing Then
                Return (CType(ViewState("IsPaymentOutSourcingKeyON"), System.Boolean))
            Else
                Return False
            End If
        End Get
        Set(ByVal Value As Boolean)
            ViewState("IsPaymentOutSourcingKeyON") = Value
        End Set
    End Property
    'END : SR | 2020.01.08 | YRS-AT-4602 | declare property for outsourcing payment key.


    Protected WithEvents LabelCheckDate As System.Web.UI.WebControls.Label
    'Protected WithEvents TextboxCheckDate As YMCAUI.DateUserControl 'MMR | 2017.01.24 | YRS-AT-3288 | Commented existing menu reference as master page introduced
    Protected WithEvents LabelUSCheckNo As System.Web.UI.WebControls.Label
    Protected WithEvents TextboxUSCheckNo As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelCanadianCheckNo As System.Web.UI.WebControls.Label
    Protected WithEvents TextboxCanadianCheckNo As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelProofReport As System.Web.UI.WebControls.Label
    Protected WithEvents CheckBoxProofReport As System.Web.UI.WebControls.CheckBox
    Protected WithEvents ButtonCancel As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonRun As System.Web.UI.WebControls.Button
    Protected WithEvents PopCalendar1 As RJS.Web.WebControl.PopCalendar
    Protected WithEvents PlaceHolder1 As System.Web.UI.WebControls.PlaceHolder
    'Protected WithEvents Menu1 As skmMenu.Menu 'MMR | 2017.01.24 | YRS-AT-3288 | Commented existing menu reference as master page introduced
    'START: MMR | 2017.01.24 | YRS-AT-3288 | Declared controls
    Protected WithEvents TextboxCheckDate As System.Web.UI.WebControls.TextBox
    Protected WithEvents RangeValidatorUCDate As System.Web.UI.WebControls.RangeValidator
    Protected WithEvents btnConfirmDialogYes As System.Web.UI.WebControls.Button
    Protected WithEvents btnConfirmDialogOk As System.Web.UI.WebControls.Button
    Protected WithEvents DivSuccessMessage As System.Web.UI.HtmlControls.HtmlGenericControl
    Protected WithEvents DivMainMessage As System.Web.UI.HtmlControls.HtmlGenericControl
    Protected WithEvents lblHeaderText As System.Web.UI.WebControls.Label
    Protected WithEvents gvDuplicateMissingAddressDetail As System.Web.UI.WebControls.GridView
    'END: MMR | 2017.01.24 | YRS-AT-3288 | Declared controls
    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object
    Protected WithEvents gvWarnErrorMessage As System.Web.UI.WebControls.GridView ' ML|12-12-2019 |YRS-AT-4677|Control declaration
    Protected WithEvents lblNote As System.Web.UI.WebControls.Label 'ML | 2019.12.19 | YRS-AT 4677 | Label to display Note.
    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim duplicateMissingAddressInfo As DataSet
        Dim boolAddressProblemExists As Boolean

        'Dim l_String_CloseWindowPayrollProcess As String = "<script language='javascript'>" & _
        '                                                "window.close()" & _
        ''                                                "</script>"
        If Session("LoggedUserKey") Is Nothing Then
            Response.Redirect("Login.aspx", False)
        End If
        Try
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "YRS", "BindEvents();", True)
            Me.LabelCanadianCheckNo.AssociatedControlID = Me.TextboxCanadianCheckNo.ID

            Me.LabelProofReport.AssociatedControlID = Me.CheckBoxProofReport.ID
            Me.LabelUSCheckNo.AssociatedControlID = Me.TextboxUSCheckNo.ID
            'START: MMR | 2017.01.24 | YRS-AT-3288 | Commented existing menu reference as master page introduced
            'Me.Menu1.DataSource = Server.MapPath("SimpleXML.xml")
            'Me.Menu1.DataBind()
            'END: MMR | 2017.01.24 | YRS-AT-3288 | Commented existing menu reference as master page introduced
            'Me.TextboxCheckDate.RequiredDate = True 'MMR | 2017.01.24 | YRS-AT-3288 | added textbox for date input instead of date user control
            If Not Me.IsPostBack Then
                NextCheckNo()
                Me.TextboxUSCheckNo.Text = getCheckNos("PAYROL").ToString
                Me.TextboxCanadianCheckNo.Text = getCheckNos("CANADA").ToString
                getPayrollLast()
                RangeValidatorUCDate.MaximumValue = String.Format("12/31/{0}", (Date.Now.AddYears(10).Year)) 'MMR | 2017.01.24 | YRS-AT-3288 | setting maximum date range for Range validator

                'START: MMR | 2017.01.24 | YRS-AT-3288 | commented existing code as not required 
                'Else
                '    If Request.Form("Yes") = "Yes" Then
                '        ProcessPayroll(True)

                '    ElseIf Request.Form("No") = "No" Then
                '        ProcessPayroll(False)
                'ElseIf Request.Form("OK") = "OK" Then
                'If (Not Me.IsStartupScriptRegistered("CloseWindowPayrollProcess")) Then
                '    Page.RegisterStartupScript("CloseWindowPayrollProcess", l_String_CloseWindowPayrollProcess)
                'End If

                '    Response.Redirect("MainWebForm.aspx", False)
                'End If
                'END: MMR | 2017.01.24 | YRS-AT-3288 | commented existing code as not required

                'START: MMR | 2017.01.24 | YRS-AT-3288 | Loading address error details and displaying in grid
                If Not String.IsNullOrEmpty(TextboxCheckDate.Text.Trim()) Then
                    duplicateMissingAddressInfo = YMCARET.YmcaBusinessObject.MonthlyPayroll.GetDuplicateMissingAddressInfo(Convert.ToDateTime(TextboxCheckDate.Text.Trim()))
                    boolAddressProblemExists = DisplayDuplicateAddressDetails(duplicateMissingAddressInfo)
                    SetLabelTextAndProperties(boolAddressProblemExists) 'Method will set label text based on address error detail available
                End If
                'END: MMR | 2017.01.24 | YRS-AT-3288 | Loading address error details and displaying in grid
            End If
        Catch ex As Exception
            'Added by prasad : BT-855:YRS 5.0-1344 : Create new IAT output file
            HelperFunctions.LogException("Error", ex)
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
        End Try

    End Sub
    Private Function getPayrollLast()
        Dim l_dataset As DataSet
        Dim l_datarow As DataRow

        Dim l_datasetNextBusinessday As DataSet

        Dim lNextMonth As String
        Dim ldateNextPayROll As Date
        Dim ldateNextBusinessdate As Date
        Dim ldateAccountingDate As Date
        Dim ldatePayrollMonthDate As Date
        Dim lstring As String
        Dim ldatePayrollCheckdate As Date

        Try

            l_dataset = YMCARET.YmcaBusinessObject.MonthlyPayroll.getPayrollLast()

            If Not l_dataset Is Nothing Then

                l_datarow = l_dataset.Tables(0).Rows(0)

                If Not l_datarow Is Nothing Then
                    lNextMonth = CType(l_datarow("Month"), String).PadLeft(2, "0")
                    lNextMonth = lNextMonth + "/01/"
                    lNextMonth = lNextMonth + CType(l_datarow("Year"), String)

                    ldatePayrollMonthDate = DateAdd(DateInterval.Month, 1, System.Convert.ToDateTime(lNextMonth))

                    If System.DateTime.Now > CType(l_datarow("PayrollDate"), Date) Then
                        ldateNextBusinessdate = System.DateTime.Now()
                    Else
                        ldateNextBusinessdate = CType(l_datarow("PayrollDate"), Date)
                    End If


                    l_datasetNextBusinessday = YMCARET.YmcaBusinessObject.MonthlyPayroll.getnextBusinessDay(ldateNextBusinessdate)

                    'Shashi Shekhar: Dec 01 2010: For BT- 685: on payroll process screen, system showing error message - "There is no row at position 0." --
                    If HelperFunctions.isEmpty(l_datasetNextBusinessday) Then
                        Dim string_Message As String
                        string_Message = "Unable to obtain the business date to use for Payroll generation."
                        'START: MMR | 2017.01.24 | YRS-AT-3288 | Displaying message in jquey dialog box instead of server side message box
                        'MessageBox.Show(PlaceHolder1, "Payroll Process", string_Message, MessageBoxButtons.OK)
                        ShowModalPopupMessage("Payroll Process", string_Message, "error")
                        'END: MMR | 2017.01.24 | YRS-AT-3288 | Displaying message in jquey dialog box instead of server side message box
                        Exit Function
                        '-----------------------------------------------------------------------------------------------
                    End If
                    ' ElseIf Not l_datasetNextBusinessday Is Nothing Then
                    'If Not l_datasetNextBusinessday.Tables(0).Rows(0) Is Nothing Then
                    ldateNextBusinessdate = CType(l_datasetNextBusinessday.Tables(0).Rows(0)!dtsActivityDate, Date)

                    If ldateNextBusinessdate = System.DateTime.Now And ldateNextBusinessdate.Date.Now.Hour > 17 Then
                        ldateNextBusinessdate = DateAdd(DateInterval.Day, 1, ldateNextBusinessdate)


                    End If
                    Me.SessionNextBusinessDay = ldateNextBusinessdate

                    'End If

                    'End If

                    Me.SessionPayrollDate = ldateNextBusinessdate
                    lstring = CType(ldatePayrollMonthDate.Month, String).ToString.PadLeft(2, "0")
                    lstring = lstring + "\" + CType(ldatePayrollMonthDate.Month, String).ToString

                    Me.SessionPayrollMonth = lstring
                    l_datasetNextBusinessday = Nothing
                    l_datasetNextBusinessday = YMCARET.YmcaBusinessObject.MonthlyPayroll.getnextBusinessDay(ldatePayrollMonthDate)

                    If Not l_datasetNextBusinessday Is Nothing Then
                        If Not l_datasetNextBusinessday.Tables(0).Rows(0) Is Nothing Then
                            ldatePayrollCheckdate = CType(l_datasetNextBusinessday.Tables(0).Rows(0)!dtsActivityDate, Date)

                            If ldatePayrollCheckdate = System.DateTime.Now And ldateNextBusinessdate.Date.Now.Hour > 17 Then
                                ldatePayrollCheckdate = DateAdd(DateInterval.Day, 1, ldateNextBusinessdate)

                            End If

                        End If

                    End If

                    Me.TextboxCheckDate.Text = CType(ldatePayrollCheckdate, String)
                    Me.SessionPayrollMonthDate = CType(ldatePayrollCheckdate, String)

                    ldateAccountingDate = DateAdd(DateInterval.Month, 1, CType(l_datarow("PayrollDate"), Date))

                End If
            End If

        Catch
            Throw
        End Try

    End Function
    Private Function getCheckNos(ByVal ParameterSeriesCode As String) As String

        Try

            If ParameterSeriesCode.ToString = "PAYROL" Then
                getCheckNos = Me.SessionLongUSCheckSeriesPrefix.ToString + Me.SessionLongUSCheckNo.ToString.Trim()
            Else
                getCheckNos = Me.SessionLongCANADACheckSeriesPrefix.ToString + Me.SessionLongCANADACheckNo.ToString.Trim()
            End If

        Catch
            Throw
        End Try

    End Function
    Private Function NextCheckNo()
        Dim l_DatasetCheckSeries As DataSet
        Dim l_datatrowCheckSeries As DataRow
        Dim i As Integer
        Dim longCheckNo As Long

        Try
            l_DatasetCheckSeries = YMCARET.YmcaBusinessObject.MonthlyPayroll.getCurrentCheckSeries()

            If Not l_DatasetCheckSeries Is Nothing Then

                If l_DatasetCheckSeries.Tables(0).Rows.Count > 0 Then

                    For i = 0 To 1

                        l_datatrowCheckSeries = l_DatasetCheckSeries.Tables(0).Rows(i)

                        If Not l_datatrowCheckSeries Is Nothing Then

                            If Not (l_datatrowCheckSeries("CheckSeriesCode").GetType.ToString = "System.DBNull") Then

                                If CType(l_datatrowCheckSeries("CurrentCheckNumber"), Long) < CType(l_datatrowCheckSeries("StartingCheckNumber"), Long) Then
                                    longCheckNo = CType(l_datatrowCheckSeries("StartingCheckNumber"), Long)
                                Else
                                    longCheckNo = CType(l_datatrowCheckSeries("CurrentCheckNumber"), Long) + 1

                                End If

                                If CType(l_datatrowCheckSeries("CheckSeriesCode"), String) = "PAYROL" Then

                                    Me.SessionLongUSCheckNo = longCheckNo
                                    If l_datatrowCheckSeries("CheckSeriesPrefix").GetType.ToString = "System.DBNull" Then
                                        Me.SessionLongUSCheckSeriesPrefix = String.Empty
                                    Else
                                        Me.SessionLongUSCheckSeriesPrefix = CType(l_datatrowCheckSeries("CheckSeriesPrefix"), String)
                                    End If

                                ElseIf CType(l_datatrowCheckSeries("CheckSeriesCode"), String) = "CANADA" Then

                                    If l_datatrowCheckSeries("CheckSeriesPrefix").GetType.ToString = "System.DBNull" Then
                                        Me.SessionLongCANADACheckSeriesPrefix = String.Empty
                                    Else
                                        Me.SessionLongCANADACheckSeriesPrefix = CType(l_datatrowCheckSeries("CheckSeriesPrefix"), String)
                                    End If

                                    Me.SessionLongCANADACheckNo = longCheckNo

                                End If

                            End If

                        End If
                    Next i
                End If
            End If

        Catch
            Throw
        End Try

    End Function
    Private Function VerifyCheckNo(ByVal ParameterStringUsCheckNO As String, ByVal ParameterStringCanadaCheckNO As String) As Boolean
        Dim l_DatasetCheckSeries As DataSet
        Dim l_datatrowCheckSeries As DataRow
        Dim i As Integer
        Dim longCheckNo As Single
        Dim longUSCheckNo As Single
        Dim longCanadaCheckNo As Single
        Dim intLen As Integer
        Dim bool_Contiue As Boolean
        Dim string_Message As String

        bool_Contiue = False
        string_Message = ""

        Try
            l_DatasetCheckSeries = YMCARET.YmcaBusinessObject.MonthlyPayroll.getCurrentCheckSeries()

            If Not l_DatasetCheckSeries Is Nothing Then

                bool_Contiue = True

                If l_DatasetCheckSeries.Tables(0).Rows.Count > 0 Then

                    For i = 0 To 1

                        l_datatrowCheckSeries = l_DatasetCheckSeries.Tables(0).Rows(i)

                        If Not l_datatrowCheckSeries Is Nothing Then

                            If Not (l_datatrowCheckSeries("CheckSeriesCode").GetType.ToString = "System.DBNull") Then

                                If CType(l_datatrowCheckSeries("CurrentCheckNumber"), Long) < CType(l_datatrowCheckSeries("StartingCheckNumber"), Long) Then
                                    longCheckNo = CType(l_datatrowCheckSeries("StartingCheckNumber"), Long)
                                Else
                                    longCheckNo = CType(l_datatrowCheckSeries("CurrentCheckNumber"), Long) + 1

                                End If


                                If CType(l_datatrowCheckSeries("CheckSeriesCode"), String) = "PAYROL" Then

                                    If Me.SessionLongUSCheckSeriesPrefix = Nothing Then
                                        intLen = 0
                                    Else
                                        intLen = 1

                                        If Me.SessionLongUSCheckSeriesPrefix <> (ParameterStringUsCheckNO.Trim().Substring(0, intLen)) Then
                                            bool_Contiue = False
                                            string_Message = "Invalid US Check Number prefix used"
                                            Exit For
                                        End If

                                    End If

                                    Try

                                        longUSCheckNo = System.Convert.ToSingle(ParameterStringUsCheckNO.Trim().Substring(intLen, ParameterStringUsCheckNO.Trim.Length - intLen))
                                        If longUSCheckNo >= longCheckNo Then

                                            Me.SessionLongUSCheckNo = longUSCheckNo
                                            'l_datatrowCheckSeries("CurrentCheckNumber") = longUSCheckNo

                                        Else
                                            bool_Contiue = False
                                            string_Message = "Invalid US Check Number"

                                            Exit For
                                        End If

                                    Catch ex As Exception
                                        bool_Contiue = False
                                        string_Message = "Invalid US Check Number"
                                        Exit For
                                    End Try



                                ElseIf CType(l_datatrowCheckSeries("CheckSeriesCode"), String) = "CANADA" Then

                                    If Me.SessionLongCANADACheckSeriesPrefix = Nothing Then
                                        intLen = 0
                                    Else
                                        intLen = 1

                                        If Me.SessionLongCANADACheckSeriesPrefix <> (ParameterStringCanadaCheckNO.Trim().Substring(0, intLen)) Then
                                            bool_Contiue = False
                                            string_Message = "Invalid Canadian Check Number prefix used"
                                            Exit For
                                        End If

                                    End If

                                    Try

                                        longCanadaCheckNo = System.Convert.ToSingle(ParameterStringCanadaCheckNO.Trim().Substring(intLen, ParameterStringCanadaCheckNO.Trim().Length - intLen))

                                        If longCanadaCheckNo >= longCheckNo Then
                                            Me.SessionLongCANADACheckNo = longCanadaCheckNo
                                            'l_datatrowCheckSeries("CurrentCheckNumber") = longCanadaCheckNo

                                        Else
                                            bool_Contiue = False
                                            string_Message = "Invalid Canadian Check Number"
                                            Exit For
                                        End If

                                    Catch ex As Exception
                                        bool_Contiue = False
                                        string_Message = "Invalid Canadian Check Number"
                                        Exit For

                                    End Try



                                End If

                            End If

                        End If

                    Next i
                End If
                If bool_Contiue = True Then

                    VerifyCheckNo = True

                Else

                    'START: MMR | 2017.01.24 | YRS-AT-3288 | Showing error message on top instead of message box
                    'MessageBox.Show(PlaceHolder1, "Payroll Process", string_Message, MessageBoxButtons.OK)
                    HelperFunctions.ShowMessageToUser(string_Message, EnumMessageTypes.Error, Nothing)                    
                    'END: MMR | 2017.01.24 | YRS-AT-3288 | Showing error message on top instead of message box
                    VerifyCheckNo = False

                End If

            End If

        Catch
            Throw
        End Try

    End Function
    Private Function ProcessPayroll(ByVal parameter_boolProcess As Boolean)

        Dim bool_Proof As Boolean
        Dim datetime_CheckDate As DateTime
        Dim datatime_PayrollMonthDate As DateTime
        Dim string_Message As String

        Try
            YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("PayrollProcess", "ProcessPayroll() START") ' 2020.01.08 | SR | YRS-AT-4602 | Implement Log trace for payroll process.
            If parameter_boolProcess Then
                If Me.CheckBoxProofReport.Checked = True Then
                    bool_Proof = True
                Else
                    bool_Proof = False
                End If

                datetime_CheckDate = Convert.ToDateTime(Me.TextboxCheckDate.Text.Trim())
                'Set the Date time to First of every Month.
                'datatime_PayrollMonthDate = Convert.ToDateTime(datetime_CheckDate.Month().ToString.PadLeft(2, "0") + "\01\" + _
                '                                   datetime_CheckDate.Year().ToString.PadLeft(4, "0"))

                If Me.VerifyCheckNo(Me.TextboxUSCheckNo.Text().Trim(), Me.TextboxCanadianCheckNo.Text().Trim()) Then

                    If Me.CheckBoxProofReport.Checked = True Then
                        bool_Proof = True
                        string_Message = "Payroll proof completed successfully."
                    Else
                        bool_Proof = False
                        'START : PK | 01/23/2020 |YRS-AT-4641 | Set message from database
                        'string_Message = "Payroll completed successfully."
                        string_Message = YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.MESSAGE_PAYROLL_FINAL_SUCCESS).DisplayText()
                        'END : PK | 01/23/2020 |YRS-AT-4641 | Set message from database
                    End If

                    'Start Processing of Payroll data.
                    'If YMCARET.YmcaBusinessObject.MonthlyPayroll.ProcessPayrolldata(Me.SessionPayrollDate, bool_Proof, Me.SessionLongCANADACheckNo, Me.SessionLongUSCheckNo, datetime_CheckDate) = True Then
                    Dim bool_Result As Boolean
                    Dim objMonthlyPayroll As YMCARET.YmcaBusinessObject.MonthlyPayroll
                    objMonthlyPayroll = New YMCARET.YmcaBusinessObject.MonthlyPayroll
                    lblNote.Text = "Note:" + YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.MESSAGE_PM_DISBURSEMENT_FAILURENOTE).DisplayText() ' ML : YRS-AT-4677 | 2019.12.16 | Set error Note.

                    Dim datatable_FileList As DataTable
                    bool_Result = objMonthlyPayroll.ProcessPayrolldata(Me.SessionPayrollDate, bool_Proof, Me.SessionLongCANADACheckNo, Me.SessionLongUSCheckNo, datetime_CheckDate)
                    If bool_Result = True Then
                        datatable_FileList = objMonthlyPayroll.datatable_FileList
                        Session("FTFileList") = Nothing
                        Session("FTFileList") = datatable_FileList

                        ' Call the calling of the ASPX to copy the file.
                        Try
                            Dim popupScript As String = "<script language='javascript'>" & _
                            "var a = window.open('FT\\CopyFilestoFileServer.aspx?OR=1', 'FileCopyPopUp', " & _
                            "'width=1,height=1, menubar=no, resizable=yes,top=1300,left=1300, scrollbars=no, status=no')" & _
                            "</script>"
                            'START : MMR | 2017.01.24 | YRS-AT-3288 | Comented existing code and added script manager to call client side script as update panel introduced
                            'If (Not Me.IsStartupScriptRegistered("PopupScript1")) Then
                            'Page.RegisterStartupScript("PopupScript1", popupScript)
                            ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "PopupScript1", popupScript, False)
                            'End If
                            'END : MMR | 2017.01.24 | YRS-AT-3288 | Comented existing code and added script manager to call client side script as update panel introduced
                        Catch ex As Exception
                            HelperFunctions.LogException("ProcessPayroll", ex)
                            MessageBox.Show(PlaceHolder1, "Error While Writing File", ex.Message, MessageBoxButtons.OK)
                        End Try


                        ''START: MMR | 2017.01.24 | YRS-AT-3288 | Displaying confirmation message client side instead of server side message box 
                        ''MessageBox.Show(PlaceHolder1, "Payroll Process", string_Message.Trim(), MessageBoxButtons.OK)
                        'ShowModalPopupMessage("Payroll Process", string_Message.Trim(), "ok")

                        ''END: MMR | 2017.01.24 | YRS-AT-3288 | Displaying confirmation message client side instead of server side message box 
                        IsPaymentOutSourcingKeyON = objMonthlyPayroll.SetPaymentOutsourcingKey(datetime_CheckDate)
                        YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("PayrollProcess", String.Format("IsPaymentOutSourcingKeyON : {0}", IsPaymentOutSourcingKeyON))

                        If IsPaymentOutSourcingKeyON Then
                            If bool_Proof Then
                                If (HelperFunctions.isEmpty(objMonthlyPayroll.dtExceptionLogForNTPYRL)) Then
                                    string_Message = YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.MESSAGE_PAYROLL_PROOF_SUCCESS).DisplayText()
                                    HelperFunctions.ShowMessageToUser(string_Message, EnumMessageTypes.Success)
                                Else
                                    gvWarnErrorMessage.DataSource = objMonthlyPayroll.dtExceptionLogForNTPYRL
                                    gvWarnErrorMessage.DataBind()
                                    string_Message = YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.MESSAGE_PAYROLL_PROOF_SUCCESS).DisplayText() + YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.MESSAGE_PM_DISBURSEMENT_DISPLAY_ERRORWARNINGS).DisplayText()
                                    HelperFunctions.ShowMessageToUser(string_Message, EnumMessageTypes.Warning)
                                End If
                            Else
                                ' START: 2020.01.17 | SR | YRS-AT-4641 | Display message in DIV instead of popup.
                                'ShowModalPopupMessage("Payroll Process", string_Message.Trim(), "ok")
                                HelperFunctions.ShowMessageToUser(string_Message, EnumMessageTypes.Success)
                                ' END: 2020.01.17 | SR | Display message in DIV instead of popup.
                            End If
                        Else
                            ShowModalPopupMessage("Payroll Process", string_Message.Trim(), "ok")
                        End If
                    End If
                Else
                    'Display Message
                End If
            Else
                'Revert back to Normal
                'Me.TextboxCheckDate.Text = CType(Me.SessionPayrollDate, String)

            End If

        Catch ex As Exception
            HelperFunctions.LogException("ProcessPayroll_1", ex)
            ' START: 2020.01.17 | SR | YRS-AT-4641 | Display error message in payroll process page instead of error aspx page.
            'Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
            HelperFunctions.ShowMessageToUser(ex.Message.ToString(), EnumMessageTypes.Error, Nothing)
            ' END: 2020.01.17 | SR | YRS-AT-4641 | Display error message in payroll process page instead of error aspx page.
        Finally
            YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("PayrollProcess", "ProcessPayroll() END")
            YMCARET.CommonUtilities.WebPerformanceTracer.EndTrace()
        End Try
    End Function
    Private Sub ButtonRun_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonRun.Click
        Dim datetime_CheckDate As DateTime
        Dim string_ExMessage As String
        Dim bool_Proof As Boolean
        Dim string_Message As String
        Dim duplicateMissingAddressDetail As DataSet
        Dim boolAddressProblemExists As Boolean

        Try
            'Added by neeraj on 24-Nov-2009 : issue is YRS 5.0-940
            Dim checkSecurity As String = SecurityCheck.Check_Authorization(strFormName, Convert.ToInt32(Session("LoggedUserKey")))

            If Not checkSecurity.Equals("True") Then
                'START: MMR | 2017.01.24 | YRS-AT-3288 | Displaying message in jquey dialog box instead of server side message box
                'MessageBox.Show(PlaceHolder1, "YMCA-YRS", checkSecurity, MessageBoxButtons.Stop)
                ShowModalPopupMessage("Payroll Process", checkSecurity, "error")
                'END: MMR | 2017.01.24 | YRS-AT-3288 | Displaying message in jquey dialog box instead of server side message box
                Exit Sub
            End If
            'End : YRS 5.0-940   

            'START: MMR | 2017.01.24 | YRS-AT-3288 | Validating input before running monthly process and displaying error message on top
            If Me.TextboxCheckDate.Text.Trim() = "" Then
                string_Message = "Check Date cannot be blank"
            ElseIf Me.TextboxUSCheckNo.Text.Trim() = "" Then
                string_Message = "Next US Check No cannot be blank"
            ElseIf Me.TextboxCanadianCheckNo.Text.Trim() = "" Then
                string_Message = "Next Canadian Check No cannot be blank"
            End If

            If Not String.IsNullOrEmpty(string_Message) Then
                HelperFunctions.ShowMessageToUser(string_Message, EnumMessageTypes.Error, Nothing)
                Exit Sub
            End If           
            'END: MMR | 2017.01.24 | YRS-AT-3288 | Validating input before running monthly process and displaying error message on top

            datetime_CheckDate = Convert.ToDateTime(Me.TextboxCheckDate.Text.Trim())
            bool_Proof = Me.CheckBoxProofReport.Checked

            'START: MMR | 2017.01.24 | YRS-AT-3288 | Loading address error detail and displating in grid, setting lablel text based on address error detail available and showing error message on top
            duplicateMissingAddressDetail = YMCARET.YmcaBusinessObject.MonthlyPayroll.GetDuplicateMissingAddressInfo(datetime_CheckDate)

            boolAddressProblemExists = DisplayDuplicateAddressDetails(duplicateMissingAddressDetail)
            SetLabelTextAndProperties(boolAddressProblemExists)

            If boolAddressProblemExists Then
                HelperFunctions.ShowMessageToUser("Unable to run the Monthly Payroll process due to address problem with listed persons. Please add the missing / correct the duplicate address for the process to continue.", EnumMessageTypes.Error, Nothing)
                Exit Sub
            End If
            'END: MMR | 2017.01.24 | YRS-AT-3288 | Loading address error detail and displating in grid, setting lablel text based on address error detail available and showing error message on top

            If Me.VerifyCheckNo(Me.TextboxUSCheckNo.Text().Trim(), Me.TextboxCanadianCheckNo.Text().Trim()) Then 'MMR | 2017.01.24 | YRS-AT-3288 | Validating Check No input before proceeding further for monthly payroll process

                If datetime_CheckDate <> Me.SessionPayrollMonthDate And bool_Proof = False Then
                    string_Message = "Are you sure you want to change the Check Date and run Payroll?"
                ElseIf datetime_CheckDate <> Me.SessionPayrollMonthDate And bool_Proof = True Then
                    string_Message = "Are you sure you want to change the Check Date and run Payroll proof?"
                ElseIf datetime_CheckDate = Me.SessionPayrollMonthDate And bool_Proof = True Then
                    string_Message = "Are you sure you want to run Payroll proof?"
                Else
                    string_Message = "Are you sure you want to run Payroll?"
                End If
                'START: MMR | 2017.01.24 | YRS-AT-3288 | Displaying confirmation message client side instead of server side message box 
                'MessageBox.Show(PlaceHolder1, "Payroll Process", string_Message.Trim(), MessageBoxButtons.YesNo)
                ShowModalPopupMessage("Payroll Process", string_Message.Trim(), "infoYesNo")
                'END: MMR | 2017.01.24 | YRS-AT-3288 | Displaying confirmation message client side instead of server side message box 
                Exit Sub
            End If
        Catch ex As Exception
            HelperFunctions.LogException("ButtonRun_Click", ex)
            Response.Redirect(String.Format("ErrorPageForm.aspx?Message={0}", Server.UrlEncode(ex.Message.Trim.ToString())))
        End Try

    End Sub

    Private Sub ButtonCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonCancel.Click

        Try
            Response.Redirect("MainWebForm.aspx", False)
            'Dim l_String_CloseWindowPayrollProcess As String = "<script language='javascript'>" & _
            '                                            "window.close()" & _
            '                                            "</script>"


            'If (Not Me.IsStartupScriptRegistered("CloseWindowPayrollProcess")) Then
            '    Page.RegisterStartupScript("CloseWindowPayrollProcess", l_String_CloseWindowPayrollProcess)
            'End If


        Catch ex As Exception
            HelperFunctions.LogException("ButtonCancel_Click", ex)
            Response.Redirect(String.Format("ErrorPageForm.aspx?Message={0}", Server.UrlEncode(ex.Message.Trim.ToString())))
        End Try
    End Sub

    'START: MMR | 2017.01.24 | YRS-AT-3288 | Added to display message in jquery dialog box
    Private Sub ShowModalPopupMessage(ByVal title As String, ByVal message As String, ByVal type As String)
        Select Case type
            Case "infoYesNo"
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "DialogMessage", String.Format("ShowDialog('{0}', '{1}','{2}');", title, message, type), True)
            Case "ok"
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "DialogMessage", String.Format("ShowDialog('{0}', '{1}','{2}');", title, message, type), True)
            Case "error"
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "DialogMessage", String.Format("ShowDialog('{0}', '{1}','{2}');", title, message, type), True)
        End Select
    End Sub
    'END: MMR | 2017.01.24 | YRS-AT-3288 | Added to display message in jquery dialog box

    'START: MMR | 2017.01.24 | YRS-AT-3288 | Process payroll on click of yes button in confirmation dialog box
    Private Sub btnConfirmDialogYes_Click(sender As Object, e As EventArgs) Handles btnConfirmDialogYes.Click
        Try
            ProcessPayroll(True)
        Catch ex As Exception
            HelperFunctions.LogException("btnConfirmDialogYes_Click", ex)
            Response.Redirect(String.Format("ErrorPageForm.aspx?Message={0}", Server.UrlEncode(ex.Message.Trim.ToString())))
        End Try
    End Sub
    'END: MMR | 2017.01.24 | YRS-AT-3288 | Process payroll on click of yes button in confirmation dialog box

    'START: MMR | 2017.01.24 | YRS-AT-3288 | close confirmation dialog box on click of no button 
    Private Sub btnConfirmDialogOk_Click(sender As Object, e As EventArgs) Handles btnConfirmDialogOk.Click
        Try
            Response.Redirect("MainWebForm.aspx", False)
        Catch ex As Exception
            HelperFunctions.LogException("btnConfirmDialogOk_Click", ex)
            Response.Redirect(String.Format("ErrorPageForm.aspx?Message={0}", Server.UrlEncode(ex.Message.Trim.ToString())))
        End Try
    End Sub
    'END: MMR | 2017.01.24 | YRS-AT-3288 | close confirmation dialog box on click of no button 

    'START: MMR | 2017.01.25 | YRS-AT-3288 | Setting label text and property
    Private Sub SetLabelTextAndProperties(ByVal bln As Boolean)
        If bln Then
            lblHeaderText.Text = "List of persons having problem with address information."
            lblHeaderText.ForeColor = Drawing.Color.Red
            lblHeaderText.Attributes.Add("class", "Label_Small")
        Else
            lblHeaderText.Text = "No problems identified with the address information of persons included in the monthly payroll."
            lblHeaderText.ForeColor = Drawing.Color.Black
            lblHeaderText.Attributes.Add("class", "Label_Small")
        End If
    End Sub
    'END: MMR | 2017.01.25 | YRS-AT-3288 | Setting label text and property

    'START: MMR | 2017.01.25 | YRS-AT-3288 | Display duplicate/missing address detail in grid
    Private Function DisplayDuplicateAddressDetails(ByVal dupicateMissingAddressInfo As DataSet)
        Dim boolResult As Boolean = False
        If HelperFunctions.isNonEmpty(dupicateMissingAddressInfo) Then
            boolResult = True

            gvDuplicateMissingAddressDetail.DataSource = dupicateMissingAddressInfo.Tables(0)
            gvDuplicateMissingAddressDetail.DataBind()
        Else
            gvDuplicateMissingAddressDetail.DataSource = Nothing
            gvDuplicateMissingAddressDetail.DataBind()
        End If

        Return boolResult
    End Function
    'END: MMR | 2017.01.25 | YRS-AT-3288 | Display duplicate/missing address detail in grid
End Class

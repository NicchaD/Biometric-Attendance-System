
'*******************************************************************************
' Copyright YMCA Retirement Fund All Rights Reserved. 
'
' Project Name		:	YMCA Yrs
' FileName			:	MonthlyPayrollRevised.aspx.vb

' Author Name		:	Ragesh .V.P
' Employee ID		:	34231
' Email				:	ragesh.vp@3i-infotech.com
' Contact No		:	55928736
'****************************************************
'Modification History
'*******************************************************************************
'Modified by                    Date                Description
'*******************************************************************************
'Neeraj Singh                   12/Nov/2009         Added form name for security issue YRS 5.0-940 
'Manthan Rajguru                2015.09.16          YRS-AT-2550: YRS data cleanup: copyright comments and namespace/reference (Changed the Namespace/reference from 'Infotech' to 'YMCARET')
'*******************************************************************************


Public Class MonthlyExpDividend
    Inherits System.Web.UI.Page
    'below line is Added by Neeraj for issue id YRS 5.0-940 
    Dim strFormName As String = New String("MonthlyExpDividend.aspx")
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
    Private Property SessionCurrentExpUniqueiId() As String
        Get
            If Not (Session("CurrentExpUniqueiId")) Is Nothing Then
                Return (Session("CurrentExpUniqueiId").ToString())
            Else
                Return String.Empty
            End If
        End Get
        Set(ByVal Value As String)
            Session("CurrentExpUniqueiId") = Value
        End Set
    End Property

    Protected WithEvents LabelCheckDate As System.Web.UI.WebControls.Label
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
    Protected WithEvents Menu1 As skmMenu.Menu
    Protected WithEvents TextboxCheckDate As System.Web.UI.WebControls.TextBox

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Dim l_String_CloseWindowPayrollProcess As String = "<script language='javascript'>" & _
        '                                                "window.close()" & _
        ''                                                "</script>"
        If Session("LoggedUserKey") Is Nothing Then
            Response.Redirect("Login.aspx", False)
        End If

        Try
            Me.LabelCanadianCheckNo.AssociatedControlID = Me.TextboxCanadianCheckNo.ID
            Me.LabelProofReport.AssociatedControlID = Me.CheckBoxProofReport.ID
            Me.LabelUSCheckNo.AssociatedControlID = Me.TextboxUSCheckNo.ID
            Me.Menu1.DataSource = Server.MapPath("SimpleXML.xml")
            Me.Menu1.DataBind()

            'commented by hafiz on 10-Dec-2006 for YREN-2977
            'Me.TextboxCheckDate.RequiredDate = True
            'Me.TextboxCheckDate.Enabled = False

            If Not Me.IsPostBack Then

                NextCheckNo()
                Me.TextboxUSCheckNo.Text = getCheckNos("PAYROL").ToString
                Me.TextboxCanadianCheckNo.Text = getCheckNos("CANADA").ToString
                getPayrollLast()

            Else
                If Request.Form("Yes") = "Yes" Then
                    ProcessPayroll(True)

                ElseIf Request.Form("No") = "No" Then
                    ProcessPayroll(False)
                ElseIf Request.Form("OK") = "OK" Then
                    'If (Not Me.IsStartupScriptRegistered("CloseWindowPayrollProcess")) Then
                    '    Page.RegisterStartupScript("CloseWindowPayrollProcess", l_String_CloseWindowPayrollProcess)
                    'End If

                    Response.Redirect("MainWebForm.aspx", False)
                End If
            End If
        Catch ex As Exception

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

            l_dataset = YMCARET.YmcaBusinessObject.MonthlyPayroll.getnextPayrollDayteForExp()

            If Not l_dataset Is Nothing Then
                If l_dataset.Tables(0).Rows.Count > 0 Then
                    If l_dataset.Tables(0).Rows(0)("dtmPaymentDate").GetType.ToString() = "System.DBNull" Then
                        MessageBox.Show(PlaceHolder1, "Special Dividend Payroll Process", "No Valid Record found to Process Special Dividend", MessageBoxButtons.Stop)
                        Exit Function
                    End If

                    ldateNextBusinessdate = CType(l_dataset.Tables(0).Rows(0)!dtmPaymentDate, Date)

                    Me.SessionCurrentExpUniqueiId = l_dataset.Tables(0).Rows(0)("guiUniqueid").ToString()

                    If ldateNextBusinessdate = System.DateTime.Now And ldateNextBusinessdate.Date.Now.Hour > 17 Then
                        ldateNextBusinessdate = DateAdd(DateInterval.Day, 1, ldateNextBusinessdate)
                    End If

                    Me.SessionNextBusinessDay = ldateNextBusinessdate

                Else
                    MessageBox.Show(PlaceHolder1, "Special Dividend Payroll Process", "No Valid Record found to Process Special Dividend", MessageBoxButtons.Stop)
                    Exit Function
                End If

            Else

                MessageBox.Show(PlaceHolder1, "Special Dividend Payroll Process", "No Valid Record found to Process Special Dividend", MessageBoxButtons.Stop)
                Exit Function
            End If

            ldatePayrollMonthDate = ldateNextBusinessdate

            'l_datasetNextBusinessday = YMCARET.YmcaBusinessObject.MonthlyPayroll.getnextBusinessDay(ldateNextBusinessdate)

            'If Not l_datasetNextBusinessday Is Nothing Then
            '    If Not l_datasetNextBusinessday.Tables(0).Rows(0) Is Nothing Then
            '        ldateNextBusinessdate = CType(l_datasetNextBusinessday.Tables(0).Rows(0)!dtsActivityDate, Date)

            '        If ldateNextBusinessdate = System.DateTime.Now And ldateNextBusinessdate.Date.Now.Hour > 17 Then
            '            ldateNextBusinessdate = DateAdd(DateInterval.Day, 1, ldateNextBusinessdate)


            '        End If
            '        Me.SessionNextBusinessDay = ldateNextBusinessdate

            '    End If

            'End If

            Me.SessionPayrollDate = ldateNextBusinessdate
            Me.TextboxCheckDate.Text = CType(ldateNextBusinessdate, String)
            Me.SessionPayrollMonthDate = CType(ldateNextBusinessdate, String)

        Catch ex As Exception
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

        Catch ex As Exception
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

        Catch ex As Exception
            Throw
        End Try

    End Function
    Private Function VeryfyCheckNo(ByVal ParameterStringUsCheckNO As String, ByVal ParameterStringCanadaCheckNO As String) As Boolean
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

                    VeryfyCheckNo = True

                Else

                    MessageBox.Show(PlaceHolder1, "Special Dividend Payroll Process", string_Message, MessageBoxButtons.OK)
                    VeryfyCheckNo = False

                End If

            End If

        Catch ex As Exception
            Throw
        End Try

    End Function
    Private Function ProcessPayroll(ByVal parameter_boolProcess As Boolean)

        Dim bool_Proof As Boolean
        Dim datetime_CheckDate As DateTime
        Dim datatime_PayrollMonthDate As DateTime
        Dim string_Message As String

        Try
            If parameter_boolProcess Then

                If Me.SessionCurrentExpUniqueiId.ToString() = String.Empty Then
                    MessageBox.Show(PlaceHolder1, "Special Dividend Payroll Process", "Unable to read Master Value from the Memory, Cannot Proceed", MessageBoxButtons.Stop)
                    Exit Function
                End If
                If Me.CheckBoxProofReport.Checked = True Then
                    bool_Proof = True
                Else
                    bool_Proof = False
                End If

                datetime_CheckDate = Convert.ToDateTime(Me.TextboxCheckDate.Text.Trim())
                'Set the Date time to First of every Month.
                'datatime_PayrollMonthDate = Convert.ToDateTime(datetime_CheckDate.Month().ToString.PadLeft(2, "0") + "\01\" + _
                '                                   datetime_CheckDate.Year().ToString.PadLeft(4, "0"))

                If Me.VeryfyCheckNo(Me.TextboxUSCheckNo.Text().Trim(), Me.TextboxCanadianCheckNo.Text().Trim()) Then

                    If Me.CheckBoxProofReport.Checked = True Then
                        bool_Proof = True
                        string_Message = "Special Dividend Payroll proof completed successfully."
                    Else
                        bool_Proof = False
                        string_Message = "Special Dividend Payroll completed successfully."
                    End If

                    'Start Processing of Payroll data.
                    'If YMCARET.YmcaBusinessObject.MonthlyPayroll.ProcessPayrolldata(Me.SessionPayrollDate, bool_Proof, Me.SessionLongCANADACheckNo, Me.SessionLongUSCheckNo, datetime_CheckDate) = True Then
                    Dim bool_Result As Boolean
                    Dim objMonthlyPayroll As YMCARET.YmcaBusinessObject.MonthlyPayroll
                    objMonthlyPayroll = New YMCARET.YmcaBusinessObject.MonthlyPayroll

                    Dim datatable_FileList As DataTable
                    bool_Result = objMonthlyPayroll.ProcessXPDividentData(Me.SessionPayrollDate, bool_Proof, Me.SessionLongCANADACheckNo, Me.SessionLongUSCheckNo, datetime_CheckDate, Me.SessionCurrentExpUniqueiId)
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
                            If (Not Me.IsStartupScriptRegistered("PopupScript1")) Then
                                Page.RegisterStartupScript("PopupScript1", popupScript)
                            End If
                        Catch ex As Exception
                            MessageBox.Show(PlaceHolder1, "Error While Writing File", ex.Message, MessageBoxButtons.OK)
                        End Try
                        MessageBox.Show(PlaceHolder1, "Special Dividend Payroll Process", string_Message.Trim(), MessageBoxButtons.OK)
                    End If
                Else
                    'Display Message
                End If
            Else
                'Revert back to Normal
                'Me.TextboxCheckDate.Text = CType(Me.SessionPayrollDate, String)

            End If

        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
        End Try
    End Function
    Private Function LastPayRollRan() As Boolean
        Dim datetime_CheckDate As DateTime
        Try
            datetime_CheckDate = Convert.ToDateTime(Me.TextboxCheckDate.Text.Trim())
            If YMCARET.YmcaBusinessObject.MonthlyPayroll.ValidateLastPayroll(datetime_CheckDate) > 0 Then
                LastPayRollRan = True

            Else
                LastPayRollRan = False
            End If
        Catch ex As Exception
            Throw
        End Try
    End Function
    Private Sub ButtonRun_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonRun.Click
        Dim datetime_CheckDate As DateTime
        Dim string_ExMessage As String
        Dim bool_Proof As Boolean
        Dim string_Message As String

        Try
            'Added by neeraj on 24-Nov-2009 : issue is YRS 5.0-940
            Dim checkSecurity As String = SecurityCheck.Check_Authorization(strFormName, Convert.ToInt32(Session("LoggedUserKey")))

            If Not checkSecurity.Equals("True") Then
                MessageBox.Show(PlaceHolder1, "YMCA-YRS", checkSecurity, MessageBoxButtons.Stop)
                Exit Sub
            End If
            'End : YRS 5.0-940 

            datetime_CheckDate = Convert.ToDateTime(Me.TextboxCheckDate.Text.Trim())

            If LastPayRollRan() = True Then

                bool_Proof = Me.CheckBoxProofReport.Checked
                If datetime_CheckDate <> Me.SessionPayrollMonthDate And bool_Proof = False Then
                    string_Message = "Are you sure you want to change the Check Date and run Special Dividend Payroll?"
                ElseIf datetime_CheckDate <> Me.SessionPayrollMonthDate And bool_Proof = True Then
                    string_Message = "Are you sure you want to change the Check Date and run Special Dividend Payroll proof?"
                ElseIf datetime_CheckDate = Me.SessionPayrollMonthDate And bool_Proof = True Then
                    string_Message = "Are you sure you want to run Special Dividend Payroll proof?"
                Else
                    string_Message = "Are you sure you want to run Special Dividend Payroll?"
                End If

                MessageBox.Show(PlaceHolder1, "Special Dividend Payroll Process", string_Message.Trim(), MessageBoxButtons.YesNo)
                Exit Sub
            Else
                'commented by hafiz on 11-Dec-2006 for YREN-2977
                'string_Message = "Please run Payroll first to proceed the Special Dividend Processing"
                'added by hafiz on 11-Dec-2006 for YREN-2977
                string_Message = "Please run Payroll Process before running the Special Dividend Process"
                MessageBox.Show(PlaceHolder1, "Special Dividend Payroll Process", string_Message.Trim(), MessageBoxButtons.Stop)
                Exit Sub
            End If


        Catch ex As Exception
            Throw
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

        End Try
    End Sub
End Class

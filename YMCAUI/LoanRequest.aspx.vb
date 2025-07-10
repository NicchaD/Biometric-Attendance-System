'*******************************************************************************
' Copyright YMCA Retirement Fund All Rights Reserved. 
'
' Project Name		:	YMCA_YRS
' FileName			:	LoanRequest.aspx.vb
' Author Name		:	Ruchi Saxena
' Employee ID		:	33494
' Email				:	ruchi.saxena@icici-infotech.com    
' Contact No		:	8642
' Creation Time		:	March 9,2006
' Program Specification Name	:	
' Unit Test Plan Name			:	
' Description					:	<<Please put the brief description here...>>
'Change History
'Shubhrata Sep 13th 2006 Reason YREN 2677
' 'Shubhrata Oct 10th 2006 To add LoanNumber for unsuspended and closed loan
'Shubhrata Jan 13th 2006 YREN - 3014 to create new loan requests on termination in loan maintenance screen
'Shubhrata Mar 27th 2008 YMCA PHASE IV
'******************************************************************************
'Modification History
'******************************************************************************
'Modified by                    Date                Description
'******************************************************************************
'Neeraj Singh                   12/Nov/2009         Added form name for security issue YRS 5.0-940 
'Manthan Rajguru                2015.09.16          YRS-AT-2550: YRS data cleanup: copyright comments and namespace/reference (Changed the Namespace/reference from 'Infotech' to 'YMCARET')
'******************************************************************************
Public Class LoanRequest
    Inherits System.Web.UI.Page
    'below line is Added by Neeraj for issue id YRS 5.0-940 
    Dim strFormName As String = New String("LoanRequest.aspx")
    'End issue id YRS 5.0-940

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents TextBoxSSNo As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxFirstName As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxMiddleName As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxLastName As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextboxAge As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextboxFeeWithheld As System.Web.UI.WebControls.TextBox
    Protected WithEvents ButtonSave As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonOK As System.Web.UI.WebControls.Button
    Protected WithEvents MessageBoxPlaceHolder As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents LabelAccountBalance As System.Web.UI.WebControls.Label
    Protected WithEvents TextboxAccountBalance As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelAmountAvailable As System.Web.UI.WebControls.Label
    Protected WithEvents TextboxAmountAvailable As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelAmountRequested As System.Web.UI.WebControls.Label
    Protected WithEvents TextboxAmountRequested As System.Web.UI.WebControls.TextBox
    Protected WithEvents ButtonProcess As System.Web.UI.WebControls.Button
    Protected WithEvents LabelTitle1 As System.Web.UI.WebControls.Label
    Protected WithEvents LabelTitle As System.Web.UI.WebControls.Label
    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region
#Region "Form Properties"
    'to get/set the form empevent id
    Private Property FormEmpEventId() As String
        Get
            If Not Session("FormEmpEventId") Is Nothing Then
                Return (CType(Session("FormEmpEventId"), String))
            Else
                Return String.Empty
            End If
        End Get
        Set(ByVal Value As String)
            Session("FormEmpEventId") = Value
        End Set
    End Property
    'to get/set the form Ymca id
    Private Property FormYMCAId() As String
        Get
            If Not Session("FormYMCAId") Is Nothing Then
                Return (CType(Session("FormYMCAId"), String))
            Else
                Return String.Empty
            End If
        End Get
        Set(ByVal Value As String)
            Session("FormYMCAId") = Value
        End Set
    End Property
    'to get/set the form person id
    Private Property FormPersonId() As String
        Get
            If Not Session("FormPersonId") Is Nothing Then
                Return (CType(Session("FormPersonId"), String))
            Else
                Return String.Empty
            End If
        End Get
        Set(ByVal Value As String)
            Session("FormPersonId") = Value
        End Set
    End Property
    'To get/set the Loan Request Dataset
    Private Property DataSetLoanRequest() As DataSet
        Get
            If Not Session("LoanRequestsDataSet") Is Nothing Then
                Return (CType(Session("LoanRequestsDataSet"), DataSet))
            End If
        End Get
        Set(ByVal Value As DataSet)
            Session("LoanRequestsDataSet") = Value
        End Set
    End Property
    'To get / set YMCA Amount
    Private Property YMCAAvailableAmount() As Decimal
        Get
            If Not Session("YMCAAvailableAmount") Is Nothing Then
                Return (CType(Session("YMCAAvailableAmount"), Decimal))
            Else
                Return 0.0
            End If
        End Get
        Set(ByVal Value As Decimal)
            Session("YMCAAvailableAmount") = Value
        End Set
    End Property


    ' To get / set TD Account Amount
    Private Property TDBalance() As Decimal
        Get
            If Not Session("TDBalance") Is Nothing Then
                Return (CType(Session("TDBalance"), Decimal))
            Else
                Return 0.0
            End If
        End Get
        Set(ByVal Value As Decimal)
            Session("TDBalance") = Value
        End Set
    End Property


    'To get / set the PersonID property.    
    Private Property PersonID() As String
        Get
            If Not Session("PersonID") Is Nothing Then
                Return (CType(Session("PersonID"), String))
            Else
                Return String.Empty
            End If

        End Get
        Set(ByVal Value As String)
            Session("PersonID") = Value
        End Set
    End Property

    ' To get / set FundID
    Private Property FundID() As String
        Get
            If Not Session("FundID") Is Nothing Then
                Return (CType(Session("FundID"), String))
            Else
                Return String.Empty
            End If

        End Get
        Set(ByVal Value As String)
            Session("FundID") = Value
        End Set
    End Property
    'To Get / Set Total Amount
    Private Property TotalAmount() As Decimal
        Get
            If Not Session("TotalAmount") Is Nothing Then
                Return (CType(Session("TotalAmount"), String))
            Else
                Return 0.0
            End If

        End Get
        Set(ByVal Value As Decimal)
            Session("TotalAmount") = Value
        End Set
    End Property

    'To Get / Set Employee Total Amount
    Private Property PersonTotalAmount() As Decimal
        Get
            If Not Session("PersonTotalAmount") Is Nothing Then
                Return (CType(Session("PersonTotalAmount"), String))
            Else
                Return 0.0
            End If

        End Get
        Set(ByVal Value As Decimal)
            Session("PersonTotalAmount") = Value
        End Set
    End Property
    Private Property SessionAmountRequested() As String
        Get
            If Not (Session("AmountRequested")) Is Nothing Then
                Return (CType(Session("AmountRequested"), String))
            Else
                Return String.Empty
            End If
        End Get
        Set(ByVal Value As String)
            Session("AmountRequested") = Value
        End Set
    End Property
#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        '  Me.TextboxAmountRequested.Attributes.Add("onkeypress", "javascript:ValidateNumeric();")
        Me.TextboxAmountRequested.Attributes.Add("onkeypress", "Javascript:return HandleAmountFiltering(this);")
        Me.TextboxAmountRequested.Attributes.Add("onchange", "javascript:FormatAmtControl(this);")

        If Session("LoggedUserKey") Is Nothing Then
            Response.Redirect("Login.aspx", False)
        End If
        'Commented by Shubhrata Sep 13th 2006 Reason YREN 2677
        'HttpContext.Current.Session.Timeout = 60
        'Commented by Shubhrata Sep 13th 2006 Reason YREN 2677

        If Not Page.IsPostBack Then
            If Not Session("Title") Is Nothing Then
                Me.LabelTitle.Text = Session("Title")
            End If
            Me.LoadInformation()
            If Not Session("AccountBalance") Is Nothing Then
                Me.TextboxAccountBalance.Text = FormatCurrency(Convert.ToDouble(Session("AccountBalance")))
            End If
            If Not Session("TDBalance") Is Nothing Then
                Me.TextboxAmountAvailable.Text = FormatCurrency(Convert.ToDouble(Session("TDBalance")))
            End If
            'Shubhrata Jan 12th 2007 YREN - 3014
            'If Not Session("LoanNumber") Is Nothing Then
            '    Me.SetAlert(CType(Session("LoanNumber"), String))
            'End If

        End If


    End Sub

    Private Sub ButtonOK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonOK.Click
        'Shubhrata Oct 10th 2006
        'Session("LoanNumber") = Nothing
        'Shubhrata Oct 10th 2006
        Me.Response.Write("<script language='javascript'> { self.close() }</script>")
    End Sub
    Private Function LoadInformation()

        Dim l_DataSet As DataSet
        Dim l_DataTable As DataTable

        Try
            'PersonInformation

            l_DataSet = Session("PersonInformation")

            If Not l_DataSet Is Nothing Then

                l_DataTable = l_DataSet.Tables("Member Details")

                If Not l_DataTable Is Nothing Then
                    If l_DataTable.Rows.Count > 0 Then
                        Me.LoadGeneralInfoToControls(l_DataTable.Rows.Item(0))
                    End If
                End If

                ' This segment for 
                l_DataTable = l_DataSet.Tables("Member Employment")

                If Not l_DataTable Is Nothing Then
                    If l_DataTable.Rows.Count > 0 Then
                        Me.LoadEmploymentDetails(l_DataTable.Rows.Item(0))
                    End If
                End If


            End If

        Catch ex As Exception
            Throw
        End Try
    End Function
    Private Function LoadGeneralInfoToControls(ByVal parameterDataRow As DataRow)
        Try

            If Not parameterDataRow Is Nothing Then

                Me.TextBoxSSNo.Text = CType(parameterDataRow("SS No"), String)
                Me.TextBoxLastName.Text = CType(parameterDataRow("Last Name"), String)
                Me.TextBoxMiddleName.Text = CType(parameterDataRow("Middle Name"), String)
                Me.TextBoxFirstName.Text = CType(parameterDataRow("First Name"), String)



                If (parameterDataRow("BirthDate").GetType.ToString = "System.DBNull") Then
                    Me.TextboxAge.Text = "0.00"

                ElseIf (parameterDataRow("DeathDate").GetType.ToString = "System.DBNull") Then
                    'Me.TextboxAge.Text = Me.CalculateAge(CType(parameterDataRow("BirthDate"), String), String.Empty)
                    'Shubhrata
                    Me.TextboxAge.Text = Math.Round(Me.CalculateAge(CType(parameterDataRow("BirthDate"), String), String.Empty))
                    'Shubhrata

                Else
                    'Me.TextboxAge.Text = Me.CalculateAge(CType(parameterDataRow("BirthDate"), String), CType(parameterDataRow("DeathDate"), String))
                    'Shubhrata
                    Me.TextboxAge.Text = Math.Round(Me.CalculateAge(CType(parameterDataRow("BirthDate"), String), CType(parameterDataRow("DeathDate"), String)))
                    'Shubhrata
                End If

            End If
            'Shubhrata YMCA Phase 4-- to remove hardcoded $50
            Me.TextboxFeeWithheld.Text = YMCARET.YmcaBusinessObject.LoanInformationBOClass.LoanInformationWrapperClass.GetLoanFees()

        Catch ex As Exception
            Throw
        End Try
    End Function
    Private Function CalculateAge(ByVal parameterDOB As String, ByVal parameterDOD As String) As Decimal
        Try

            If parameterDOB = String.Empty Then Return 0

            If parameterDOD = String.Empty Then
                Return Math.Round(((DateDiff(DateInterval.Month, CType(parameterDOB, DateTime), Now.Date)) / 12.0), 2)
            Else
                Return Math.Round(((DateDiff(DateInterval.Year, CType(parameterDOB, DateTime), CType(parameterDOD, DateTime))) / 12.0))
            End If

        Catch ex As Exception
            Throw
        End Try
    End Function


    Private Function LoadEmploymentDetails(ByVal parameterDataRow As DataRow)
        Try

            If Not parameterDataRow Is Nothing Then

                If parameterDataRow("PersonID").GetType.ToString = "System.DBNull" Then
                    Me.PersonID = String.Empty
                Else
                    Me.PersonID = CType(parameterDataRow("PersonID"), String)
                End If

                If parameterDataRow("FundEventID").GetType.ToString = "System.DBNull" Then
                    Me.FundID = String.Empty
                Else
                    Me.FundID = CType(parameterDataRow("FundEventID"), String)
                End If

            End If

        Catch ex As Exception
            Throw
        End Try

    End Function
    Public Function FormatCurrency(ByVal paramNumber As Decimal) As String
        Try
            Dim n As String
            Dim m As String()
            Dim myNum As String
            'Changed by Ruchi on 7th March,2006
            Dim myDec As String
            'end of change
            Dim len As Integer
            Dim i As Integer
            Dim val As String
            If paramNumber = 0 Then
                val = 0
            Else
                n = paramNumber.ToString()
                m = (Math.Round(n * 100) / 100).ToString().Split(".")
                myNum = m(0).ToString()

                len = myNum.Length
                Dim fmat(len) As String
                For i = 0 To len - 1
                    fmat(i) = myNum.Chars(i)
                Next
                Array.Reverse(fmat)
                For i = 1 To len - 1
                    If i Mod 3 = 0 Then
                        fmat(i + 1) = fmat(i + 1) & ","
                    End If
                Next
                Array.Reverse(fmat)
                'start of change


                'end of change
                If m.Length = 1 Then
                    val = String.Join("", fmat) + ".00"
                Else
                    myDec = m(1).ToString
                    If myDec.Length = 1 Then
                        myDec = myDec + "0"
                    End If
                    val = String.Join("", fmat) + "." + myDec
                End If

            End If

            Return val

        Catch ex As Exception
            Return paramNumber
        End Try

    End Function

    Private Sub ButtonSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonSave.Click
        Dim l_dataset_LoanRequests As DataSet
        Dim l_datatable_LoanRequests As DataTable
        Dim l_datarow As DataRow
        Try
            If Me.TextboxAmountRequested.Text.Trim <> "" Then
                If Convert.ToDouble(Me.TextboxAmountRequested.Text) <> 0 Then
                    If Convert.ToDouble(Me.TextboxAmountRequested.Text) <= Convert.ToDouble(Me.TextboxAmountAvailable.Text) Then
                        If CheckNullTransactionRefId() = True Then
                            MessageBox.Show(Me.MessageBoxPlaceHolder, "YMCA-YRS", "TransactionRefId is Null for some RT account transactions,cannot proceed.", MessageBoxButtons.Stop, False)
                            Exit Sub
                        End If
                        l_dataset_LoanRequests = Me.DataSetLoanRequest
                        l_datatable_LoanRequests = l_dataset_LoanRequests.Tables("LoanRequests")
                        l_datarow = l_datatable_LoanRequests.NewRow
                        l_datarow("PersId") = Me.FormPersonId
                        l_datarow("EmpEventId") = Me.FormEmpEventId
                        l_datarow("YmcaId") = Me.FormYMCAId

                        'commented by hafiz on 17Jul2006
                        'l_datarow("TDBalance") = Me.TDBalance
                        'code add by hafiz on 17Jul2006
                        l_datarow("TDBalance") = Session("AccountBalance")

                        l_datarow("RequestedAmount") = Me.TextboxAmountRequested.Text
                        'Shubhrata Oct 10th 2006 To add LoanNumber for unsuspended and closed loan
                        'Commented by shubhrata Jan 12th 2007 YREN 3014
                        'If Not Session("LoanNumber") Is Nothing Then
                        '    l_datarow("OriginalLoanNumber") = CType(Session("LoanNumber"), String)
                        'Else
                        '    l_datarow("OriginalLoanNumber") = String.Empty
                        'End If
                        l_datarow("OriginalLoanNumber") = String.Empty
                        'Shubhrata Oct 10th 2006 To add LoanNumber for unsuspended and closed loan
                        l_datatable_LoanRequests.Rows.Add(l_datarow)
                        'l_dataset_LoanRequests.AcceptChanges()
                        YMCARET.YmcaBusinessObject.LoanInformationBOClass.AddRequest(l_dataset_LoanRequests)
                        Session("ReloadLoan") = True
                        Session("LoanNumber") = Nothing
                        Dim msg As String
                        msg = msg + "<Script Language='JavaScript'>"

                        msg = msg + "window.opener.document.forms(0).submit();"

                        msg = msg + "self.close();"

                        msg = msg + "</Script>"
                        Response.Write(msg)
                    Else
                        'last parameter changed to False by Anita 30-05-2007
                        MessageBox.Show(Me.MessageBoxPlaceHolder, "YMCA-YRS", "You cannot request more than the total available amount.", MessageBoxButtons.Stop, False)
                    End If
                Else
                    'last parameter changed to False by Anita 30-05-2007
                    MessageBox.Show(Me.MessageBoxPlaceHolder, "YMCA-YRS", "The requested amount cannot be zero.", MessageBoxButtons.Stop, False)
                End If
            Else
                'last parameter changed to False by Anita 30-05-2007
                MessageBox.Show(Me.MessageBoxPlaceHolder, "YMCA-YRS", "Please enter the requested amount.", MessageBoxButtons.Stop, False)
            End If

        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message)

        End Try

    End Sub
    'Shubhrata TD LoansPhase 2
    Private Function SetAlert(ByVal parameterLoanNumber As String)
        Try
            Dim setmsg As String = ""
            Dim x As String
            setmsg = "<script language='javascript'>"
            x = parameterLoanNumber
            setmsg = setmsg + "alert('This Loan request is the continuation of the Loan Number " & x & " ');"
            setmsg = setmsg + "</Script>"
            Page.RegisterStartupScript("MsgScript", setmsg)
        Catch
            Throw
        End Try
    End Function
    'Shubhrata TD LoansPhase 2

    'Private Sub Page_DataBinding(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.DataBinding
    '  
    'End Sub
#Region "YMCA PHASEIV"
    Private Function CheckNullTransactionRefId() As Boolean
        Dim l_int_TransRefIdIsNull As Integer = 0
        Try
            l_int_TransRefIdIsNull = YMCARET.YmcaBusinessObject.LoanInformationBOClass.CheckNullTransactionRefId(Me.FundID)
            If l_int_TransRefIdIsNull = 1 Then
                Return True
            Else
                Return False
            End If

        Catch ex As Exception
            Throw ex
        End Try
    End Function
#End Region
End Class


'Modification History
'********************************************************************************************************************************
'Modified By			Date				Description
'********************************************************************************************************************************
'Ashutosh Patil         27-Apr-2007         For avoidng runtime error while specifying the selected value
'                                           for Vesting Reason Dropdownlist
'Neeraj Singh           12/Nov/2009         Added form name for security issue YRS 5.0-940 
'Shashi Shekhar:        26-Oct-2010:        For (BT-663 YRS 5.0-1197 - Replace SSN with Fund Id in title bar)
'Shashi                 04 Mar. 2011        Replacing Header formating with user control (YRS 5.0-450 )
'Manthan Rajguru        2015.09.16          YRS-AT-2550: YRS data cleanup: copyright comments and namespace/reference (Changed the Namespace/reference from 'Infotech' to 'YMCARET')
'********************************************************************************************************************************
Imports System.Math
Public Class UpdateParticipantEmploymentService
    Inherits System.Web.UI.Page
    'below line is Added by Neeraj for issue id YRS 5.0-940 
    Dim strFormName As String = New String("UpdateParticipantEmploymentService.aspx")
    'End issue id YRS 5.0-940

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents Menu1 As skmMenu.Menu
    Protected WithEvents LabelService As System.Web.UI.WebControls.Label
    Protected WithEvents LabelYears As System.Web.UI.WebControls.Label
    Protected WithEvents LabelMonths As System.Web.UI.WebControls.Label
    Protected WithEvents LabelPaid As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxYear As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelYear As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxMonth As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelMonth As System.Web.UI.WebControls.Label
    Protected WithEvents LabelTotal As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxYearTotal As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelYearTotal As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxMonthTotal As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelMonthTotal As System.Web.UI.WebControls.Label
    Protected WithEvents CheckBoxNotVested As System.Web.UI.WebControls.CheckBox
    Protected WithEvents ButtonCancel As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonSave As System.Web.UI.WebControls.Button
    Protected WithEvents TextBoxVestingDate As YMCAUI.DateUserControl
    Protected WithEvents PlaceHolder1 As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents RequiredFieldValidator1 As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents RequiredFieldValidator2 As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents RequiredFieldValidator3 As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents RequiredFieldValidator4 As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents RangeValidator1 As System.Web.UI.WebControls.RangeValidator
    Protected WithEvents RangeValidator2 As System.Web.UI.WebControls.RangeValidator
    Protected WithEvents DropDownListVestingReason As System.Web.UI.WebControls.DropDownList
    Protected WithEvents Headercontrol As YMCA_Header_WebUserControl

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
        'Put user code to initialize the page here
        Dim l_string_Script As String
        Try
            If IsPostBack = False Then
                LoadControls()
                SetDetails()
            End If

            If Request.Form("OK") = "OK" Then
                If Not Session("NoFundEventDetails") Is Nothing Then
                    If CType(Session("NoFundEventDetails"), Boolean) = True Then
                        Session("NoFundEventDetails") = Nothing

                        l_string_Script = l_string_Script + "<Script Language='JavaScript'>"
                        l_string_Script = l_string_Script + "self.close();"
                        l_string_Script = l_string_Script + "</Script>"
                        Response.Write(l_string_Script)
                    End If
                End If
            End If
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message, False)
        End Try
    End Sub

    Private Sub ButtonCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonCancel.Click
        Dim l_string_Script As String
        Try
            l_string_Script = l_string_Script + "<Script Language='JavaScript'>"
            l_string_Script = l_string_Script + "self.close();"
            l_string_Script = l_string_Script + "</Script>"
            Response.Write(l_string_Script)
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message, False)
        End Try
    End Sub

    Private Sub ButtonSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonSave.Click
        Dim l_string_Msg As String
        Dim l_bool_Result As Boolean
        Try

            l_bool_Result = ValidateForSave()
            If l_bool_Result = False Then
                Exit Sub
            End If

            If ServiceUpdate() = False Then
                Exit Sub
            End If

            'for refreshing the main page and getting the data from database instead of taking from session 
            Session("Flag") = "Edited"

            l_string_Msg = l_string_Msg + "<Script Language='JavaScript'>"
            l_string_Msg = l_string_Msg + "window.opener.document.forms(0).submit();"
            l_string_Msg = l_string_Msg + "self.close();"
            l_string_Msg = l_string_Msg + "</Script>"
            Response.Write(l_string_Msg)
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message, False)
        End Try
    End Sub
#Region "Custom Methods"
    Private Function ServiceUpdate() As Boolean
        Dim l_integer_Paid As Integer
        Dim l_integer_NonPaid As Integer
        Dim l_string_VestingDate As String
        Dim l_string_VestingReason As String
        Dim l_bool_Vested As Boolean
        Dim l_bool_Result As Boolean
        Dim l_string_FundEventId As String
        Try
            ServiceUpdate = False

            l_integer_Paid = CalculateMonth("PAID")
            l_integer_NonPaid = CalculateMonth("NONPAID")
            l_string_VestingReason = DropDownListVestingReason.SelectedValue.Trim()
            l_string_VestingDate = Trim(TextBoxVestingDate.Text)
            l_bool_Vested = Not CheckBoxNotVested.Checked
            l_string_FundEventId = CType(Session("FundId"), String)

            l_bool_Result = YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.UpdateService(l_string_FundEventId, l_string_VestingDate, l_string_VestingReason, l_integer_Paid, l_integer_NonPaid)
            If l_bool_Result = True Then
                ServiceUpdate = True
            End If
        Catch
            Throw
        End Try
    End Function
    Private Function ValidateForSave() As Boolean
        Try
            ValidateForSave = True

            If CheckBoxNotVested.Checked = True Then
                If Trim(TextBoxVestingDate.Text) <> "" Or DropDownListVestingReason.SelectedValue.Trim() <> "" Then
                    MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Participant not vested then Vesting Date & Vesting Reason should be kept blank.", MessageBoxButtons.Stop)
                    ValidateForSave = False
                    Exit Function
                End If
            ElseIf CheckBoxNotVested.Checked = False Then
                If Trim(TextBoxVestingDate.Text) <> "" Or DropDownListVestingReason.SelectedValue.Trim() <> "" Then
                    If DropDownListVestingReason.SelectedValue.Trim() = "" Then
                        MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Vesting Reason cannot be blank.", MessageBoxButtons.Stop)
                        ValidateForSave = False
                        Exit Function
                    ElseIf Trim(TextBoxVestingDate.Text) = "" Then
                        MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Vesting Date cannot be blank.", MessageBoxButtons.Stop)
                        ValidateForSave = False
                        Exit Function
                    End If
                End If
            End If
        Catch
            Throw
        End Try
    End Function
    '*************************************************************************
    'To Calculate The years and moths for Service Paid and Non Paid
    '*************************************************************************
    Function CalculateMonth(ByVal l_string_Type As String) As Integer
        Dim l_integer_ReturnValue As Integer = 0
        Try
            l_string_Type = Trim(l_string_Type.ToUpper())
            If l_string_Type = "PAID" Then
                If TextBoxYear.Text.Trim() <> "" Then
                    l_integer_ReturnValue = Convert.ToInt32(TextBoxYear.Text.Trim()) * 12
                End If
                If TextBoxMonth.Text.Trim() <> "" Then
                    l_integer_ReturnValue = l_integer_ReturnValue + Convert.ToInt32(TextBoxMonth.Text.Trim())
                End If
            ElseIf l_string_Type = "NONPAID" Then
                If TextBoxYearTotal.Text.Trim() <> "" Then
                    l_integer_ReturnValue = Convert.ToInt32(TextBoxYearTotal.Text.Trim()) * 12
                End If
                If TextBoxMonthTotal.Text.Trim() <> "" Then
                    l_integer_ReturnValue = l_integer_ReturnValue + Convert.ToInt32(TextBoxMonthTotal.Text.Trim())
                End If
            End If

            Return l_integer_ReturnValue
        Catch
            Throw
        End Try
    End Function
    Private Sub LoadControls()
        Dim l_string_Header As String = ""
        Try

            '----------------------------------------------------------------------------------------------------------------
            'Shashi: 04 Mar. 2011: Replacing Header formating with user control (YRS 5.0-450 )

            If Not Session("FundNo") Is Nothing Then
                Headercontrol.PageTitle = "Participant Information - Edit Employment Service"
                Headercontrol.FundNo = Session("FundNo").ToString().Trim()
            End If





            Me.TextBoxMonth.Attributes.Add("OnKeyPress", "javascript:ValidateNumeric();")
            Me.TextBoxMonthTotal.Attributes.Add("OnKeyPress", "javascript:ValidateNumeric();")
            Me.TextBoxYear.Attributes.Add("OnKeyPress", "javascript:ValidateNumeric();")
            Me.TextBoxYearTotal.Attributes.Add("OnKeyPress", "javascript:ValidateNumeric();")

            Me.TextBoxMonth.Text = "0"
            Me.TextBoxYear.Text = "0"
            Me.TextBoxYearTotal.Text = "0"
            Me.TextBoxMonthTotal.Text = "0"
            Me.TextBoxVestingDate.Text = ""
            Me.DropDownListVestingReason.SelectedValue = ""
            Me.CheckBoxNotVested.Checked = True
        Catch
            Throw
        End Try
    End Sub
    Private Sub SetDetails()
        Dim l_dataset_FundEvent As DataSet
        Dim l_datarow_FundEvent As DataRow
        Try
            If Not Session("FundEventInfo") Is Nothing Then
                l_dataset_FundEvent = Session("FundEventInfo")
            Else
                MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Unable to load Fund event details", MessageBoxButtons.Stop, False)
                Session("NoFundEventDetails") = True
            End If

            If l_dataset_FundEvent.Tables("FundEventInfo").Rows.Count > 0 Then
                l_datarow_FundEvent = l_dataset_FundEvent.Tables("FundEventInfo").Rows(0)
                If l_datarow_FundEvent.Item("Paid").ToString() <> "System.DBNull" And l_datarow_FundEvent.Item("Paid").ToString() <> "" Then
                    Me.TextBoxMonth.Text = Me.CalculateYearMonth("MONTH", Convert.ToInt32(l_datarow_FundEvent.Item("Paid")))
                    Me.TextBoxYear.Text = Me.CalculateYearMonth("YEAR", Convert.ToInt32(l_datarow_FundEvent.Item("Paid")))
                End If

                If l_datarow_FundEvent.Item("NonPaid").ToString() <> "System.DBNull" And l_datarow_FundEvent.Item("NonPaid").ToString() <> "" Then
                    Me.TextBoxMonthTotal.Text = Me.CalculateYearMonth("MONTH", Convert.ToInt32(l_datarow_FundEvent.Item("NonPaid")))
                    Me.TextBoxYearTotal.Text = Me.CalculateYearMonth("YEAR", Convert.ToInt32(l_datarow_FundEvent.Item("NonPaid")))
                End If

                If l_datarow_FundEvent.Item("VestingDate").ToString() = "System.DBNull" Or l_datarow_FundEvent.Item("VestingDate").ToString() = "" Then
                    'Me.TextBoxVestingDate.Text = ""
                    'Me.CheckBoxNotVested.Checked = True
                Else
                    Me.TextBoxVestingDate.Text = Trim(l_datarow_FundEvent.Item("VestingDate"))
                    'Commented and Modified By Ashutosh Patil as on 27-Apr-2007
                    'For avoidng runtime error while specifying the selected value
                    'for Vesting Reason Dropdownlist
                    'Me.DropDownListVestingReason.SelectedValue = Trim(l_datarow_FundEvent.Item("chrVestWhy"))
                    'Start Ashutosh Patil
                    If Trim(l_datarow_FundEvent.Item("chrVestWhy").ToString.ToUpper()) = "AGE" Then   ''updated by sanjay on 24 Aug 09 for YRS 5.0-874.
                        Me.DropDownListVestingReason.SelectedIndex = 1
                    ElseIf Trim(l_datarow_FundEvent.Item("chrVestWhy").ToString.ToUpper()) = "SERV" Then   ''updated by sanjay on 24 Aug 09 for YRS 5.0-874.
                        Me.DropDownListVestingReason.SelectedIndex = 2
                    ElseIf Trim(l_datarow_FundEvent.Item("chrVestWhy").ToString.ToUpper()) = "QDRO" Then    ''updated by sanjay on 24 Aug 09 for YRS 5.0-874.
                        Me.DropDownListVestingReason.SelectedIndex = 3
                    End If
                    'End Ashutosh Patil
                    Me.CheckBoxNotVested.Checked = False
                End If
            End If

            l_datarow_FundEvent = Nothing
            l_dataset_FundEvent = Nothing
        Catch
            Throw
        End Try
    End Sub
    '*************************************************************************
    'To Calculate The years and moths for Service Paid and Non Paid
    '*************************************************************************
    Function CalculateYearMonth(ByVal l_string_Type As String, ByVal l_integer_Months As Integer) As Integer
        Dim l_integer_ReturnValue As Integer
        Try
            l_string_Type = Trim(l_string_Type.ToUpper())
            If l_integer_Months >= 0 Then
                If l_string_Type = "YEAR" Then
                    l_integer_ReturnValue = Convert.ToInt32(Floor(l_integer_Months / 12))
                Else
                    l_integer_ReturnValue = Convert.ToInt32(l_integer_Months Mod 12)
                End If
            Else
                If l_string_Type = "YEAR" Then
                    l_integer_ReturnValue = Convert.ToInt32(Floor(l_integer_Months / 12))
                Else
                    l_integer_ReturnValue = Convert.ToInt32(l_integer_Months Mod -12)

                End If
            End If

            Return l_integer_ReturnValue
        Catch
            Throw
        End Try
    End Function
#End Region     'Custom Methods

    Private Sub CheckBoxNotVested_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CheckBoxNotVested.CheckedChanged
        Try
            If CheckBoxNotVested.Checked = True Then
                TextBoxVestingDate.Text = ""
                DropDownListVestingReason.SelectedValue = ""
            End If
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message, False)
        End Try
    End Sub

    Private Sub DropDownListVestingReason_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DropDownListVestingReason.SelectedIndexChanged
        Try
            If DropDownListVestingReason.SelectedValue.Trim() = "" And Trim(TextBoxVestingDate.Text) = "" Then
                CheckBoxNotVested.Checked = True
            Else
                CheckBoxNotVested.Checked = False
            End If
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message, False)
        End Try
    End Sub
End Class

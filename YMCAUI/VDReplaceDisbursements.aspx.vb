'*******************************************************************************
' Copyright YMCA Retirement Fund All Rights Reserved. 
'
' Project Name		:	YMCA-YRS
' FileName			:	VDReplaceDisbursements.aspx.vb
' Author Name		:	Ruchi Saxena
' Employee ID		:	33494
' Email				:	ruchi.saxena@3i-infotech.com
' Contact No		:	8751
' Creation Time		:	8/20/2005 1:04:34 PM
' Program Specification Name	:	
' Unit Test Plan Name			:	
' Description					:	Form opened as a popup in VR Manager Replace Disbursements
'*******************************************************************************
'Modified By                    Date                Description
'*******************************************************************************
'Hafiz                          04Feb06             Cache-Session
'Shubhrata                      Sep 22nd 2006       TD Loans Phase 2
'Neeraj Singh                   12/Nov/2009         Added form name for security issue YRS 5.0-940 
'Manthan Rajguru                2015.09.16          YRS-AT-2550: YRS data cleanup: copyright comments and namespace/reference (Changed the Namespace/reference from 'Infotech' to 'YMCARET')
'*******************************************************************************

Public Class VDReplaceDisbursements
    Inherits System.Web.UI.Page
    'below line is Added by Neeraj for issue id YRS 5.0-940 
    Dim strFormName As String = New String("VDReplaceDisbursements.aspx")
    'End issue id YRS 5.0-940
#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents DataGridYMCAAddress As System.Web.UI.WebControls.DataGrid
    Protected WithEvents LabelAddress1 As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxAddress1 As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelAddress2 As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxAddress2 As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelAddress3 As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxAddress3 As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelCity As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxCity As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelState As System.Web.UI.WebControls.Label
    Protected WithEvents DropDownState As System.Web.UI.WebControls.DropDownList
    Protected WithEvents LabelZip As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxZip As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelCountry As System.Web.UI.WebControls.Label
    Protected WithEvents DropDownCountry As System.Web.UI.WebControls.DropDownList
    Protected WithEvents ButtonSave As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonCancel As System.Web.UI.WebControls.Button
    Protected WithEvents placeholderMessage As System.Web.UI.WebControls.PlaceHolder

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Dim g_dataset_AddressList As DataSet
    Dim g_dataset_LatestAddress As DataSet

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim l_string_DisbNbr As String
        Dim l_string_Id As String
        Dim l_string_AddressId As String
        Dim l_string_DisbId As String
        If Session("LoggedUserKey") Is Nothing Then
            Response.Redirect("Login.aspx", False)
        End If
        Try
           
            If Not IsPostBack Then
                If Not Request.QueryString.Get("DisbNo") = "" Then
                    l_string_DisbNbr = Request.QueryString.Get("DisbNo").ToString()
                End If
                If Not Request.QueryString.Get("PayId") = "" Then
                    l_string_Id = Request.QueryString.Get("PayId").ToString()
                Else
                    l_string_Id = Request.QueryString.Get("PersId").ToString()
                End If
                If Not Request.QueryString.Get("AddId") = "" Then
                    l_string_AddressId = Request.QueryString.Get("AddId").ToString()
                Else
                    l_string_AddressId = ""
                End If
                If Not Request.QueryString.Get("DisbId") = "" Then
                    l_string_DisbId = Request.QueryString.Get("DisbId").ToString()
                End If

                ''************************************HARD CODED VALUES ***************************************
                'l_string_DisbNbr = "646693"
                'l_string_Id = "52b55482-d932-4075-a353-503c455df87c"
                'l_string_AddressId = "5ac16dac-b4bf-461f-b158-ed310cc9259b"
                ''l_string_AddressId = ""

                '******************************************************************************************
                If (l_string_AddressId <> "") Then
                    
                    g_dataset_AddressList = YMCARET.YmcaBusinessObject.VDReplaceDisbursementsBOClass.LookUpAddress(l_string_Id)
                    g_dataset_LatestAddress = YMCARET.YmcaBusinessObject.VDReplaceDisbursementsBOClass.LookUpLatestAddress(l_string_Id)
                    If (g_dataset_AddressList.Tables("AddressList").Rows.Count > 0) Then
                        Session("dsAddressList") = g_dataset_AddressList
                        Session("dsLatestAddress") = g_dataset_LatestAddress
                        PopulateGrid()
                    End If
                Else
                    MessageBox.Show(placeholderMessage, "YMCA-YRS", "Unable to retrieve any address reference for payee.", MessageBoxButtons.OK, True)
                End If

            End If

        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message, False)
        End Try

    End Sub
    Private Sub PopulateGrid()
        Try
            'g_dataset_AddressList = System.AppDomain.CurrentDomain.GetData("dsAddressList")
            g_dataset_AddressList = Session("dsAddressList")
            If (g_dataset_AddressList.Tables("AddressList").Rows.Count > 0) Then
                'Commented by Shubhrata Sep 26th 2006
                'CommonModule.HideColumnsinDataGrid(g_dataset_AddressList, Me.DataGridYMCAAddress, "UniqueID,EntityID,EntityCode,EffDate,AddrCode,Addr2,Addr3,City,State,Zip,Country,PrimaryInd,ActiveInd")
                'Commented by Shubhrata Sep 26th 2006
                Me.DataGridYMCAAddress.DataSource = g_dataset_AddressList
                Me.DataGridYMCAAddress.DataBind()
                'Commented By Shubhrata YREN 2695-to pick latest address for the person
                'TextBoxAddress1.Text = g_dataset_AddressList.Tables("AddressList").Rows(0).Item("Addr1").ToString
                'TextBoxAddress2.Text = g_dataset_AddressList.Tables("AddressList").Rows(0).Item("Addr2").ToString
                'TextBoxAddress3.Text = g_dataset_AddressList.Tables("AddressList").Rows(0).Item("Addr3").ToString
                'TextBoxCity.Text = g_dataset_AddressList.Tables("AddressList").Rows(0).Item("City").ToString
                'TextBoxZip.Text = g_dataset_AddressList.Tables("AddressList").Rows(0).Item("Zip").ToString
                'DropDownCountry.Items.Add(g_dataset_AddressList.Tables("AddressList").Rows(0).Item("Country").ToString)
                'DropDownState.Items.Add(g_dataset_AddressList.Tables("AddressList").Rows(0).Item("State").ToString)
                'Added by Shubhrata YREN 2695-to pick latest address for the person
                If Not Session("dsLatestAddress") Is Nothing Then
                    g_dataset_LatestAddress = CType(Session("dsLatestAddress"), DataSet)
                    If g_dataset_LatestAddress.Tables.Count > 0 Then
                        If g_dataset_LatestAddress.Tables("LatestAddress").Rows.Count > 0 Then
                            Dim drow As DataRow
                            drow = g_dataset_LatestAddress.Tables("LatestAddress").Rows(0)
                            If Not IsDBNull(drow("UniqueId")) Then
                                Session("LatestAddId") = drow("UniqueId").ToString()
                            End If
                            If Not IsDBNull(drow("Address1")) Then
                                Me.TextBoxAddress1.Text = drow("Address1").ToString()
                            End If
                            If Not IsDBNull(drow("Address2")) Then
                                Me.TextBoxAddress2.Text = drow("Address2").ToString()
                            End If
                            If Not IsDBNull(drow("Address3")) Then
                                Me.TextBoxAddress3.Text = drow("Address3").ToString()
                            End If
                            If Not IsDBNull(drow("City")) Then
                                Me.TextBoxCity.Text = drow("City").ToString()
                            End If
                            If Not IsDBNull(drow("Zip")) Then
                                Me.TextBoxZip.Text = drow("Zip").ToString()
                            End If
                            If Not IsDBNull(drow("Country")) Then
                                Me.DropDownCountry.Items.Add(drow("Country").ToString())
                            End If
                            If Not IsDBNull(drow("State")) Then
                                Me.DropDownState.Items.Add(drow("State").ToString())
                            End If
                        End If
                    End If

                End If
                ButtonSave.Enabled = True

            End If
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message, False)
        End Try
    End Sub

    Private Sub ButtonSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonSave.Click
        Dim l_string_ReplacementStatus As String
        Dim l_string_message As String
        Dim l_int_ZeroFundingStatus As Integer = 0
        l_string_message = ""
        Try
            If Session("LoanReplace") = True Then
                l_string_message = YMCARET.YmcaBusinessObject.VDReplaceDisbursementsBOClass.ReplaceLoan(Request.QueryString.Get("DisbId"), Session("LatestAddId"))
                If l_string_message <> "" Then
                    MessageBox.Show(placeholderMessage, "YMCA-YRS", l_string_message, MessageBoxButtons.OK)
                    Exit Sub
                End If
                'Shubhrata Dec 1st 2006 CashOuts
            ElseIf Session("CashOutsReplace") = True Then
                l_string_message = YMCARET.YmcaBusinessObject.VDReplaceDisbursementsBOClass.ReplaceCashOuts(Request.QueryString.Get("DisbId"), Session("LatestAddId"), Session("ReplaceFees"), l_int_ZeroFundingStatus)
                If l_string_message <> "" Then
                    MessageBox.Show(placeholderMessage, "YMCA-YRS", l_string_message, MessageBoxButtons.OK)
                    Exit Sub
                End If
                'we are showing this message when a zero record is inserted in funding table i.e when disb amt is less
                'than the dedcution fee(25$)
                If l_int_ZeroFundingStatus = 1 Then
                    Session("IsZeroFunding") = True
                    ' MessageBox.Show(placeholderMessage, "YMCA-YRS", "No amount due. Net amount after withholding = $0.00", MessageBoxButtons.OK)
                End If
            Else
                'Commented by Shubhrata Sep 26th 2006 YREN 2695 To pick latest address
                'l_string_ReplacementStatus = YMCARET.YmcaBusinessObject.VDReplaceDisbursementsBOClass.ReplaceDisbursement(Request.QueryString.Get("DisbId"), Request.QueryString.Get("AddId"))
                'Added by Shubhrata Sep 26th 2006 YREN 2695 To pick latest address
                l_string_ReplacementStatus = YMCARET.YmcaBusinessObject.VDReplaceDisbursementsBOClass.ReplaceDisbursement(Request.QueryString.Get("DisbId"), Session("LatestAddId"))
            End If

            Session("LatestAddId") = Nothing
            Session("LoanReplace") = Nothing
            Session("CashOutsReplace") = Nothing
            Session("ReplaceFees") = Nothing
            Session("Called") = True

            Dim closeWindow As String = "<script language='javascript'>" & _
                                                         "self.close();window.opener.document.forms(0).submit();" & _
                                                         "</script>"

            If (Not Me.IsStartupScriptRegistered("CloseWindow")) Then
                Page.RegisterStartupScript("CloseWindow", closeWindow)
            End If

        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message, False)
        End Try
    End Sub

    Private Sub ButtonCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonCancel.Click
        Session("LoanReplace") = Nothing
        Session("LatestAddId") = Nothing
        Session("CashOutsReplace") = Nothing
        Session("ReplaceFees") = Nothing
        Dim closeWindow1 As String = "<script language='javascript'>" & _
                                                     "window.close()" & _
                                                     "</script>"

        If (Not Me.IsStartupScriptRegistered("CloseWindow1")) Then
            Page.RegisterStartupScript("CloseWindow1", closeWindow1)
        End If
    End Sub
End Class

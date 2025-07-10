'*******************************************************************************
' Copyright YMCA Retirement Fund All Rights Reserved. 
'
' Project Name		            :	YMCA_YRS
' FileName			            :	UpdateFedWithHoldingInfo.aspx.vb
' Author Name		            :	Prasanna Penumurthy
' Employee ID		            :	
' Email				            :	
' Contact No		            :	
' Creation Time		            :	
' Program Specification Name	:	YRS_PS_RetirementProcessing.doc
' Unit Test Plan Name			:	
' Description					:	To Add/Update Federal Withholding details
' Cache-Session   
'******************************************************************************************************************************************************
'Modification History
'******************************************************************************************************************************************************
'Modified By    Modification Date   Desription
'******************************************************************************************************************************************************
'Mohammed Hafiz 27-Mar-2008         YRPS-4720    
'Neeraj Singh   12/Nov/2009         Added form name for security issue YRS 5.0-940 
'Priya          1-April-2010        YRS 5.0-1041-Not filling in AtsFedTaxWithholdings.chrTaxEntityType Added andalso condition into if condition
'Sanjay R.      2010.06.21          Enhancement changes(CType to DirectCast)
'Sanjay R.      2010.07.12          Code Review changes(Region,variable declarations etc.) 
'Shashi Shekhar 27-Dec-2010         For YRS 5.0-450, BT-643 : Replace SSN with Fund Id in title on all screens.
'Shashi         04 Mar. 2011        Replacing Header formating with user control (YRS 5.0-450 )
'Sanjay         2014.07.09          BT 2593 - UI changes in Beneficiary information page
'Manthan Rajguru 2015.09.16         YRS-AT-2550: YRS data cleanup: copyright comments and namespace/reference (Changed the Namespace/reference from 'Infotech' to 'YMCARET')
'Megha Lad      2019.11.15          YRS-AT-4719 -  State Withholding - Additional text & warning messages for AL, CA and MA. 
'******************************************************************************************************************************************************
Public Class UpdateFedWithHoldingInfo
    Inherits System.Web.UI.Page
    'below line is Added by Neeraj for issue id YRS 5.0-940 
    Dim strFormName As String = New String("UpdateFedWithHoldingInfo.aspx")
    'End issue id YRS 5.0-940

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents cmbMaritalStatus As System.Web.UI.WebControls.DropDownList
    Protected WithEvents cmbTaxEntity As System.Web.UI.WebControls.DropDownList
    Protected WithEvents cmbWithHolding As System.Web.UI.WebControls.DropDownList
    Protected WithEvents txtExemptions As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtAddlAmount As System.Web.UI.WebControls.TextBox
    Protected WithEvents ButtonOK As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonCancel As System.Web.UI.WebControls.Button
    Protected WithEvents LabelAnnuitiesLastName As System.Web.UI.WebControls.Label
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    Protected WithEvents Label2 As System.Web.UI.WebControls.Label
    Protected WithEvents Label3 As System.Web.UI.WebControls.Label
    Protected WithEvents Label4 As System.Web.UI.WebControls.Label
    Protected WithEvents LabelDuplicateFedWith As System.Web.UI.WebControls.Label
    Protected WithEvents Headercontrol As YMCA_Header_WebUserControl
    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object
    'START : ML| 11/18/2019| YRS-AT-4719| Control declared here.
    Protected WithEvents lblFederalWitholdingMessage As System.Web.UI.WebControls.Label
    Protected WithEvents btnOK As System.Web.UI.WebControls.Button
    'END : ML| 11/18/2019| YRS-AT-4719| Control declared here.

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region


#Region " Event Handlers"
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        If Session("LoggedUserKey") Is Nothing Then
            Response.Redirect("Login.aspx", False)
        End If
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "YRS", "BindEvents();", True) 'ML| 11/18/2019| YRS-AT-4719| Code added to register the script.
        Try
            '-------------------------------------------------------------------
            'Shashi: 04 Mar. 2011: Replacing Header formating with user control (YRS 5.0-450 )

            If Not Session("FundNo") Is Nothing Then
                Headercontrol.PageTitle = "Retiree Information - Add/Update Federal Withholding"
                Headercontrol.FundNo = Session("FundNo").ToString().Trim()
            End If

            '-------------------------------------------------------------------
            If Not IsPostBack Then
                Dim l_string_PersId As String
                l_string_PersId = Session("PersId")
                Dim l_dataset_TaxEntityTypes As New DataSet
                Dim l_dataset_MaritalTypes As New DataSet
                Dim l_dataset_WithHoldingTypes As New DataSet
                Dim l_bit_IsFederalTaxForMaritalStatus As Integer
                l_dataset_TaxEntityTypes = YMCARET.YmcaBusinessObject.RetireesInformationBOClass.TaxEntityTypes()
                If Session("IsFedTaxForMaritalStatus") = True Then
                    l_bit_IsFederalTaxForMaritalStatus = 1
                ElseIf Session("IsNotFedTaxForMaritalStatus") = True Then
                    l_bit_IsFederalTaxForMaritalStatus = 0
                End If

                l_dataset_MaritalTypes = YMCARET.YmcaBusinessObject.RetireesInformationBOClass.MaritalTypes(l_bit_IsFederalTaxForMaritalStatus)
                l_dataset_WithHoldingTypes = YMCARET.YmcaBusinessObject.RetireesInformationBOClass.WithHoldingTypes()

                cmbTaxEntity.DataSource = l_dataset_TaxEntityTypes
                cmbTaxEntity.DataTextField = "Description"
                cmbTaxEntity.DataValueField = "TaxEntityType"
                cmbTaxEntity.DataBind()

                cmbMaritalStatus.DataSource = l_dataset_MaritalTypes
                cmbMaritalStatus.DataTextField = "Description"
                cmbMaritalStatus.DataValueField = "Code"
                cmbMaritalStatus.DataBind()

                cmbWithHolding.DataSource = l_dataset_WithHoldingTypes
                cmbWithHolding.DataTextField = "Description"
                cmbWithHolding.DataValueField = "Code"
                cmbWithHolding.DataBind()

                Me.txtExemptions.Attributes.Add("onKeyPress", "javascript:ValidateNumeric();")
                Me.txtAddlAmount.Attributes.Add("onkeypress", "Javascript:return HandleAmountFiltering(this);")
                Me.txtAddlAmount.Attributes.Add("onchange", "javascript:FormatAmtControl(this);")
                Me.txtAddlAmount.Attributes.Add("OnPaste", "javascript:ValidateNumeric();")

                If Not Request.QueryString("UniqueID") Is Nothing Or Not Request.QueryString("Index") Is Nothing Then
                    If Not Session("cmbWithHolding") Is Nothing Then
                        cmbWithHolding.SelectedValue = Session("cmbWithHolding")
                    End If

                    'setting the default selection in the fields.
                    If cmbWithHolding.SelectedItem.Text.Trim = "Default" Then
                        txtExemptions.Enabled = False
                        txtExemptions.Text = 3

                        'YRPS-4720 - Start
                        txtAddlAmount.Enabled = False
                        txtAddlAmount.Text = "0.00"
                        'YRPS-4720 - End

                        cmbMaritalStatus.Enabled = False
                        cmbMaritalStatus.SelectedIndex = 0
                    ElseIf cmbWithHolding.SelectedItem.Text.Trim = "Flat" Then
                        txtExemptions.Enabled = False
                        txtAddlAmount.Enabled = True
                        cmbMaritalStatus.Enabled = False
                        cmbMaritalStatus.SelectedIndex = 0
                    ElseIf cmbWithHolding.SelectedItem.Text.Trim = "Formula" Then
                        txtExemptions.Enabled = True
                        txtAddlAmount.Enabled = True
                        cmbMaritalStatus.Enabled = True
                        cmbMaritalStatus.SelectedIndex = 0
                    End If

                    'setting the values from the existing federal withholding record.
                    'Priya:1-April-2010:YRS 5.0-1041-Not filling in AtsFedTaxWithholdings.chrTaxEntityType
                    'Added andalso condition into if condition
                    If Not Session("cmbTaxEntity") Is Nothing AndAlso CType(Session("cmbTaxEntity"), String).Trim() <> String.Empty AndAlso CType(Session("cmbTaxEntity"), String).Trim() <> "&nbsp;" Then
                        cmbTaxEntity.SelectedValue = DirectCast(Session("cmbTaxEntity"), String).Trim() 'Changed from CType to Directcast by SR:2010.06.21 for migration
                    Else
                        cmbTaxEntity.SelectedIndex = 0
                    End If

                    If Not CType(Session("cmbMaritalStatus"), String).Trim = "" And Not CType(Session("cmbMaritalStatus"), String).Trim = "&nbsp;" Then
                        cmbMaritalStatus.SelectedValue = Session("cmbMaritalStatus")
                    Else
                        cmbMaritalStatus.SelectedIndex = 0
                    End If

                    txtExemptions.Text = Session("txtExemptions")
                    txtAddlAmount.Text = Session("txtAddlAmount")
                Else
                    'i.e a new record is added 
                    txtExemptions.Enabled = False
                    txtExemptions.Text = 3
                    txtAddlAmount.Enabled = False
                    txtAddlAmount.Text = "0.00"
                    cmbMaritalStatus.Enabled = False
                    cmbMaritalStatus.SelectedIndex = 0
                End If

            End If

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub ButtonOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonOK.Click
        'START : ML| 11/18/2019| YRS-AT-4719| Data will be save after federal input vs State Input data validation 
        'Dim msg As String
        'Dim dsFedWithDrawals As New DataSet
        'Dim drRows() As DataRow
        'Dim drUpdated As DataRow

        Try
            If (Not Me.ValidateFederalInput()) Then
                Exit Sub
            Else
                Me.SaveFederalInput() ' Save Federal Inputs
            End If
            ' Validate Inputs

            'If Request.QueryString("UniqueID") Is Nothing And Request.QueryString("Index") Is Nothing Then
            '    Session("blnAddFedWithHoldings") = True
            'End If

            '' Check if duplicate Withholding had been added
            'If Session("blnAddFedWithHoldings") = True Then
            '    Dim dsFedWith As DataSet = Session("FedWithDrawals")
            '    If dsFedWith.Tables.Count > 0 Then
            '        If dsFedWith.Tables(0).Select("[Tax Entity] ='" & cmbTaxEntity.SelectedValue & "'").Length > 0 Then
            '            'Dont allow to add the user to add duplicate FedWithholding
            '            LabelDuplicateFedWith.Visible = True
            '            Session("blnAddFedWithHoldings") = False
            '            Exit Sub
            '        End If
            '    End If
            'End If


            'Session("cmbTaxEntity") = cmbTaxEntity.SelectedValue.Trim
            'Session("cmbWithHolding") = cmbWithHolding.SelectedValue
            'If Me.txtExemptions.Text.Trim() = String.Empty Then
            '    Session("txtExemptions") = "0"
            '    Me.txtExemptions.Text = "0"
            'Else
            '    Session("txtExemptions") = Me.txtExemptions.Text
            'End If

            'If Me.txtAddlAmount.Text.Trim = String.Empty Then
            '    Session("txtAddlAmount") = "0.00"
            '    Me.txtAddlAmount.Text = "0.00"
            'Else
            '    Session("txtAddlAmount") = Me.txtAddlAmount.Text
            'End If

            'Session("cmbMaritalStatus") = Me.cmbMaritalStatus.SelectedValue

            ''for alerting & restricting the user, if the user has made any changes and tried to purchase the annuity of a participant
            ''used in RetirementProcessingForm.aspx.vb page.
            'Session("RP_DataChanged") = True

            'If Not Request.QueryString("UniqueID") Is Nothing Then
            '    dsFedWithDrawals = DirectCast(Session("FedWithDrawals"), DataSet)
            '    If Not IsNothing(dsFedWithDrawals) Then
            '        drRows = dsFedWithDrawals.Tables(0).Select("FedWithdrawalID='" & Request.QueryString("UniqueID") & "'")
            '        drUpdated = drRows(0)
            '        drUpdated("Exemptions") = Me.txtExemptions.Text
            '        drUpdated("Add'l Amount") = Me.txtAddlAmount.Text
            '        drUpdated("Tax Entity") = cmbTaxEntity.SelectedValue
            '        drUpdated("Type") = cmbWithHolding.SelectedValue
            '        drUpdated("Marital Status") = Me.cmbMaritalStatus.SelectedValue

            '        Session("EnableSaveCancel") = True
            '        Session("blnUpdateFedWithHoldings") = True
            '        Session("FedWithDrawals") = dsFedWithDrawals
            '    End If
            'End If

            'If Not Request.QueryString("Index") Is Nothing Then
            '    dsFedWithDrawals = DirectCast(Session("FedWithDrawals"), DataSet)

            '    If Not IsNothing(dsFedWithDrawals) Then
            '        drUpdated = dsFedWithDrawals.Tables(0).Rows(Request.QueryString("Index"))
            '        drUpdated("Exemptions") = Me.txtExemptions.Text
            '        drUpdated("Add'l Amount") = Me.txtAddlAmount.Text
            '        drUpdated("Tax Entity") = cmbTaxEntity.SelectedValue
            '        drUpdated("Type") = cmbWithHolding.SelectedValue
            '        drUpdated("Marital Status") = Me.cmbMaritalStatus.SelectedValue

            '        Session("EnableSaveCancel") = True
            '        Session("blnUpdateFedWithHoldings") = True
            '        Session("FedWithDrawals") = dsFedWithDrawals
            '    End If
            'End If
            'Session("IsFedTaxForMaritalStatus") = Nothing
            'Session("IsNotFedTaxForMaritalStatus") = Nothing
            'Session("Flag") = ""

            'msg = msg + "<Script Language='JavaScript'>"
            'msg = msg + "window.opener.document.forms(0).submit();"
            'msg = msg + "self.close();"
            'msg = msg + "</Script>"

            'Response.Write(msg)

        Catch ex As Exception
            Throw
        End Try
        'END : ML| 11/18/2019| YRS-AT-4719| Data will be save after federal input vs State Input data validation '
    End Sub
    'START : ML | 15.11.2019 | YRS-AT-4719 | Validate Federal Tax Input details vs State Tax Input details
    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        Me.SaveFederalInput()
    End Sub
    Private Function SaveFederalInput()
        Dim msg As String
        Dim dsFedWithDrawals As New DataSet
        Dim drRows() As DataRow
        Dim drUpdated As DataRow

        Try
            If Request.QueryString("UniqueID") Is Nothing And Request.QueryString("Index") Is Nothing Then
                Session("blnAddFedWithHoldings") = True
            End If

            ' Check if duplicate Withholding had been added
            If Session("blnAddFedWithHoldings") = True Then
                Dim dsFedWith As DataSet = Session("FedWithDrawals")
                If dsFedWith.Tables.Count > 0 Then
                    If dsFedWith.Tables(0).Select("[Tax Entity] ='" & cmbTaxEntity.SelectedValue & "'").Length > 0 Then
                        'Dont allow to add the user to add duplicate FedWithholding
                        LabelDuplicateFedWith.Visible = True
                        Session("blnAddFedWithHoldings") = False
                        Exit Function
                    End If
                End If
            End If


            Session("cmbTaxEntity") = cmbTaxEntity.SelectedValue.Trim
            Session("cmbWithHolding") = cmbWithHolding.SelectedValue
            If Me.txtExemptions.Text.Trim() = String.Empty Then
                Session("txtExemptions") = "0"
                Me.txtExemptions.Text = "0"
            Else
                Session("txtExemptions") = Me.txtExemptions.Text
            End If

            If Me.txtAddlAmount.Text.Trim = String.Empty Then
                Session("txtAddlAmount") = "0.00"
                Me.txtAddlAmount.Text = "0.00"
            Else
                Session("txtAddlAmount") = Me.txtAddlAmount.Text
            End If

            Session("cmbMaritalStatus") = Me.cmbMaritalStatus.SelectedValue

            'for alerting & restricting the user, if the user has made any changes and tried to purchase the annuity of a participant
            'used in RetirementProcessingForm.aspx.vb page.
            Session("RP_DataChanged") = True

            If Not Request.QueryString("UniqueID") Is Nothing Then
                dsFedWithDrawals = DirectCast(Session("FedWithDrawals"), DataSet)
                If Not IsNothing(dsFedWithDrawals) Then
                    drRows = dsFedWithDrawals.Tables(0).Select("FedWithdrawalID='" & Request.QueryString("UniqueID") & "'")
                    drUpdated = drRows(0)
                    drUpdated("Exemptions") = Me.txtExemptions.Text
                    drUpdated("Add'l Amount") = Me.txtAddlAmount.Text
                    drUpdated("Tax Entity") = cmbTaxEntity.SelectedValue
                    drUpdated("Type") = cmbWithHolding.SelectedValue
                    drUpdated("Marital Status") = Me.cmbMaritalStatus.SelectedValue

                    Session("EnableSaveCancel") = True
                    Session("blnUpdateFedWithHoldings") = True
                    Session("FedWithDrawals") = dsFedWithDrawals
                End If
            End If

            If Not Request.QueryString("Index") Is Nothing Then
                dsFedWithDrawals = DirectCast(Session("FedWithDrawals"), DataSet)

                If Not IsNothing(dsFedWithDrawals) Then
                    drUpdated = dsFedWithDrawals.Tables(0).Rows(Request.QueryString("Index"))
                    drUpdated("Exemptions") = Me.txtExemptions.Text
                    drUpdated("Add'l Amount") = Me.txtAddlAmount.Text
                    drUpdated("Tax Entity") = cmbTaxEntity.SelectedValue
                    drUpdated("Type") = cmbWithHolding.SelectedValue
                    drUpdated("Marital Status") = Me.cmbMaritalStatus.SelectedValue

                    Session("EnableSaveCancel") = True
                    Session("blnUpdateFedWithHoldings") = True
                    Session("FedWithDrawals") = dsFedWithDrawals
                End If
            End If
            Session("IsFedTaxForMaritalStatus") = Nothing
            Session("IsNotFedTaxForMaritalStatus") = Nothing
            Session("Flag") = ""

            msg = msg + "<Script Language='JavaScript'>"
            msg = msg + "window.opener.document.forms(0).submit();"
            msg = msg + "self.close();"
            msg = msg + "</Script>"

            Response.Write(msg)

        Catch ex As Exception
            Throw
        End Try

    End Function
    Private Function ValidateFederalInput() As Boolean
        Dim dt As DataTable
        Dim l_dataset_FedWith As DataSet
        Dim dr As DataRow
        Dim federalAmount As Double?
        Dim annuityAmount As Double
        Dim federalType As String
        Dim warningMessage As String
        Dim LstSWHPerssDetail As List(Of YMCAObjects.StateWithholdingDetails)
        Try
            l_dataset_FedWith = New DataSet
            dt = New DataTable
            dt.Columns.Add("Tax Entity")
            dt.Columns.Add("Type")
            dt.Columns.Add("Add'l Amount")
            dt.Columns.Add("Marital Status")
            dt.Columns.Add("Exemptions")
            l_dataset_FedWith.Tables.Add(dt)

            dr = l_dataset_FedWith.Tables(0).NewRow
            dr("Tax Entity") = Me.cmbTaxEntity.SelectedValue
            dr("Type") = Me.cmbWithHolding.SelectedValue
            dr("Add'l Amount") = Me.txtAddlAmount.Text
            dr("Marital Status") = Me.cmbMaritalStatus.SelectedValue
            dr("Exemptions") = Me.txtExemptions.Text
            l_dataset_FedWith.Tables(0).Rows.Add(dr)
            If (Not Session("AnnuityAmount") Is Nothing) Then
                annuityAmount = Convert.ToDouble(Session("AnnuityAmount"))

                federalAmount = YMCARET.YmcaBusinessObject.RetirementBOClass.GetFedWithDrawalAmount(l_dataset_FedWith, 0, annuityAmount)
                federalType = l_dataset_FedWith.Tables(0).Rows(0)("Type").ToString().Trim.ToUpper()

                If (Not SessionManager.SessionStateWithholding.LstSWHPerssDetail Is Nothing) Then
                    If (SessionManager.SessionStateWithholding.LstSWHPerssDetail.Count > 0) Then
                        LstSWHPerssDetail = SessionManager.SessionStateWithholding.LstSWHPerssDetail
                        If (Not YMCARET.YmcaBusinessObject.StateWithholdingBO.ValidateFedTaxVSStateTaxInputDetailForCA(LstSWHPerssDetail.FirstOrDefault, federalAmount, federalType, warningMessage)) Then
                            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "WarningMessage", "ShowDialog();", True)
                            lblFederalWitholdingMessage.Text = warningMessage
                            Return False
                        End If
                    End If
                End If
            End If
            Return True
        Catch
            Throw
        End Try
    End Function
    'END : ML | 15.11.2019 | YRS-AT-4719 | Validate Federal Tax Input details vs State Tax Input details
    Private Sub ButtonCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonCancel.Click
        Session("blnUpdateFedWithHoldings") = False 'SR:2014.07.09-BT 2593 - UI changes in Beneficiary information page
        Session("blnAddFedWithHoldings") = False
        Session("IsFedTaxForMaritalStatus") = Nothing
        Session("IsNotFedTaxForMaritalStatus") = Nothing

        Dim msg As String
        msg = msg + "<Script Language='JavaScript'>"
        msg = msg + "self.close();"
        msg = msg + "</Script>"
        Response.Write(msg)
    End Sub

    Private Sub cmbWithHolding_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbWithHolding.SelectedIndexChanged
        If cmbWithHolding.SelectedItem.Text.Trim = "Default" Then
            txtExemptions.Enabled = False
            txtExemptions.Text = 3
            txtAddlAmount.Enabled = False
            txtAddlAmount.Text = "0.00"
            cmbMaritalStatus.Enabled = False
            cmbMaritalStatus.SelectedIndex = 0
        ElseIf cmbWithHolding.SelectedItem.Text.Trim = "Flat" Then
            txtExemptions.Enabled = False
            txtAddlAmount.Enabled = True
            txtExemptions.Text = 0
            txtAddlAmount.Text = "0.00"
            cmbMaritalStatus.Enabled = False
            cmbMaritalStatus.SelectedIndex = 0
        ElseIf cmbWithHolding.SelectedItem.Text.Trim = "Formula" Then
            txtExemptions.Enabled = True
            txtAddlAmount.Enabled = True
            txtExemptions.Text = 0
            txtAddlAmount.Text = "0.00"
            cmbMaritalStatus.Enabled = True
            cmbMaritalStatus.SelectedIndex = 0
        End If
    End Sub
#End Region

End Class

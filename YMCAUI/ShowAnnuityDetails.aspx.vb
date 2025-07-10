'*******************************************************************************
'Modified By                    Date                Description
'*******************************************************************************
'Hafiz                          04Feb06             Cache-Session    
'Neeraj Singh                   12/Nov/2009         Added form name for security issue YRS 5.0-940 
'Shashi Shekhar:                26-Oct-2010:        For (BT-663 YRS 5.0-1197 - Replace SSN with Fund Id in title bar)
'Shashi                         03 Mar. 2011        Replacing Header formating with user control (YRS 5.0-450 )
'Manthan Rajguru                2015.09.16          YRS-AT-2550: YRS data cleanup: copyright comments and namespace/reference (Changed the Namespace/reference from 'Infotech' to 'YMCARET')
'*******************************************************************************
Public Class ShowAnnuityDetails
    Inherits System.Web.UI.Page
    'below line is Added by Neeraj for issue id YRS 5.0-940 
    Dim strFormName As String = New String("ShowAnnuityDetails.aspx")
    'End issue id YRS 5.0-940

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents TabStripRetireesInformation As Microsoft.Web.UI.WebControls.TabStrip
    Protected WithEvents DataGridAdjustments As System.Web.UI.WebControls.DataGrid
    Protected WithEvents MenuRetireesInformation As skmMenu.Menu
    Protected WithEvents MultiPageRetireesInformation As Microsoft.Web.UI.WebControls.MultiPage
    Protected WithEvents ButtonRetireesInfoSave As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonRetireesInfoCancel As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonRetireesInfoPHR As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonRetireesInfoOK As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonClose As System.Web.UI.WebControls.Button


    Protected WithEvents lblAnnuityType As System.Web.UI.WebControls.Label '
    Protected WithEvents lblAnnuitySource As System.Web.UI.WebControls.Label
    Protected WithEvents lblPensionerType As System.Web.UI.WebControls.Label
    Protected WithEvents lblPurchaseDate As System.Web.UI.WebControls.Label
    Protected WithEvents lblTaxClass As System.Web.UI.WebControls.Label
    Protected WithEvents lblTerminationDate As System.Web.UI.WebControls.Label
    Protected WithEvents lblDefaultTaxStatus As System.Web.UI.WebControls.Label
    Protected WithEvents lblTerminationReason As System.Web.UI.WebControls.Label
    Protected WithEvents lblPersonalPreTaxReserves As System.Web.UI.WebControls.Label
    Protected WithEvents lblPopUpValue As System.Web.UI.WebControls.Label
    Protected WithEvents lblPersonalPostTaxReserves As System.Web.UI.WebControls.Label
    Protected WithEvents lblYMCAReserves As System.Web.UI.WebControls.Label
    Protected WithEvents lblDeathBenefit As System.Web.UI.WebControls.Label
    Protected WithEvents lblSocialSecurityLeveling As System.Web.UI.WebControls.Label
    Protected WithEvents lblSocialSecurityReduction As System.Web.UI.WebControls.Label
    Protected WithEvents lblReductionEffectiveDate As System.Web.UI.WebControls.Label '
    Protected WithEvents chkSuppressPayroll As System.Web.UI.WebControls.CheckBox

    Protected WithEvents lblPersonalPreTax As System.Web.UI.WebControls.Label
    Protected WithEvents lblPersonalPreTaxRem As System.Web.UI.WebControls.Label
    Protected WithEvents lblPersonalPostTax As System.Web.UI.WebControls.Label
    Protected WithEvents lblPersonalPostTaxRes As System.Web.UI.WebControls.Label
    Protected WithEvents lblYMCA As System.Web.UI.WebControls.Label

    Protected WithEvents lblYMCARes As System.Web.UI.WebControls.Label
    Protected WithEvents lblTotalPayment As System.Web.UI.WebControls.Label
    Protected WithEvents lblSocialSecurity As System.Web.UI.WebControls.Label
    Protected WithEvents lblDeathBenefitRemaining As System.Web.UI.WebControls.Label
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    Protected WithEvents Label2 As System.Web.UI.WebControls.Label
    Protected WithEvents Label3 As System.Web.UI.WebControls.Label
    Protected WithEvents Label4 As System.Web.UI.WebControls.Label
    Protected WithEvents PersonalPreTax As System.Web.UI.WebControls.Label
    Protected WithEvents PersonalPreTaxRem As System.Web.UI.WebControls.Label
    Protected WithEvents PersonalPostTax As System.Web.UI.WebControls.Label
    Protected WithEvents PersonalPostTaxRes As System.Web.UI.WebControls.Label
    Protected WithEvents YMCA As System.Web.UI.WebControls.Label
    Protected WithEvents YMCARes As System.Web.UI.WebControls.Label
    Protected WithEvents TotalPayment As System.Web.UI.WebControls.Label
    Protected WithEvents SocialSecurity As System.Web.UI.WebControls.Label
    Protected WithEvents DeathBenefitRemaining As System.Web.UI.WebControls.Label
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
        If Session("LoggedUserKey") Is Nothing Then
            Response.Redirect("Login.aspx", False)
        End If
        Try

            'If Not IsPostBack Then
            '----------------------------------------------------------------------------------------------------------------

            'Shashi :03 Mar. 2011: Replacing Header formating with user control (YRS 5.0-450 )
            If Not Session("FundNo") Is Nothing Then
                Headercontrol.PageTitle = "Retirees Information"
                Headercontrol.FundNo = Session("FundNo").ToString().Trim()
            End If
            '----------------------------------------------------------------------------------------------------------------
            LoadAnnuityDetails()
            'End If

        Catch ex As Exception
            Throw

        End Try
    End Sub

    Private Sub TabStripRetireesInformation_SelectedIndexChange(ByVal sender As Object, ByVal e As System.EventArgs) Handles TabStripRetireesInformation.SelectedIndexChange

        MultiPageRetireesInformation.SelectedIndex = TabStripRetireesInformation.SelectedIndex

        If TabStripRetireesInformation.SelectedIndex = 0 Then
            LoadAnnuityDetails()
        End If
        If TabStripRetireesInformation.SelectedIndex = 1 Then
            LoadCurrentValues()
        End If
        If TabStripRetireesInformation.SelectedIndex = 2 Then
            LoadAdjustments()
        End If

    End Sub
    Public Sub LoadAnnuityDetails()
        Dim l_string_PersId As String
        l_string_PersId = Session("PersId")
        Dim l_string_AnnuityId As String
        l_string_AnnuityId = Request.QueryString("AnnuityID").ToString().Trim()
        Dim l_dataset_AnnuityDet As DataSet
        Dim dr As DataRow

        l_dataset_AnnuityDet = YMCARET.YmcaBusinessObject.RetireesInformationBOClass.LookUpAnnuityDetails(l_string_PersId, l_string_AnnuityId)
        If (l_dataset_AnnuityDet.Tables(0).Rows.Count > 0) Then
            dr = l_dataset_AnnuityDet.Tables(0).Rows(0)
            lblAnnuityType.Text = IIf(IsDBNull(dr.Item("Annuity Type")), "", dr.Item("Annuity Type"))
            lblPensionerType.Text = IIf(IsDBNull(dr.Item("Pensioner Type")), "", dr.Item("Pensioner Type")) 'dr.Item("Pensioner Type")
            lblTaxClass.Text = IIf(IsDBNull(dr.Item("Tax Class")), "", dr.Item("Tax Class")) 'dr.Item("Tax Class")
            lblDefaultTaxStatus.Text = IIf(IsDBNull(dr.Item("Default Tax Status")), "", dr.Item("Default Tax Status")) 'dr.Item("Default Tax Status")
            lblPersonalPreTaxReserves.Text = IIf(IsDBNull(dr.Item("Personal Pre-Tax Reserves")), "", dr.Item("Personal Pre-Tax Reserves")) 'dr.Item("Personal Pre-Tax Reserves")
            lblPersonalPostTaxReserves.Text = IIf(IsDBNull(dr.Item("Personal Post-Tax Reserve")), "", dr.Item("Personal Post-Tax Reserve")) 'dr.Item("Personal Post-Tax Reserve")
            lblYMCAReserves.Text = IIf(IsDBNull(dr.Item("YMCA Reserves")), "", dr.Item("YMCA Reserves")) 'dr.Item("YMCA Reserves")
            lblSocialSecurityLeveling.Text = IIf(IsDBNull(dr.Item("Social Security Leveling")), "", dr.Item("Social Security Leveling")) 'dr.Item("Social Security Leveling")
            lblSocialSecurityReduction.Text = IIf(IsDBNull(dr.Item("Social Security Reduction")), "", dr.Item("Social Security Reduction")) 'dr.Item("Social Security Reduction")

            'code add by hafiz on 25Jul2006 - Bug Id :- YREN-2533 
            lblReductionEffectiveDate.Text = IIf(IsDBNull(dr.Item("Reduction Effective Date")), "", dr.Item("Reduction Effective Date")) 'dr.Item("Reduction Effective Date")

            lblAnnuitySource.Text = IIf(IsDBNull(dr.Item("Annuity Source")), "", dr.Item("Annuity Source")) 'dr.Item("Annuity Source")
            lblPurchaseDate.Text = IIf(IsDBNull(dr.Item("Purchase Dare")), "", dr.Item("Purchase Dare")) 'dr.Item("Purchase Dare")
            lblTerminationDate.Text = IIf(IsDBNull(dr.Item("dtmTerminationDate")), "", dr.Item("dtmTerminationDate")) 'dr.Item("dtmTerminationDate")
            lblTerminationReason.Text = IIf(IsDBNull(dr.Item("Termination Reason")), "", dr.Item("Termination Reason")) 'dr.Item("Termination Reason")
            lblPopUpValue.Text = IIf(IsDBNull(dr.Item("Pop-Up Value")), "", dr.Item("Pop-Up Value")) 'dr.Item("Pop-Up Value")
            lblDeathBenefit.Text = IIf(IsDBNull(dr.Item("Death Benefit")), "", dr.Item("Death Benefit")) 'dr.Item("Death Benefit")
            chkSuppressPayroll.Checked = IIf(IsDBNull(dr.Item("Suppressed")), "", dr.Item("Suppressed")) 'dr.Item("Suppressed")
        End If
    End Sub
    Public Sub LoadCurrentValues()
        Dim l_string_PersId As String
        l_string_PersId = Session("PersId")
        Dim l_string_AnnuityId As String
        l_string_AnnuityId = Request.QueryString("AnnuityID").ToString().Trim()
        Dim l_dataset_AnnuityDet As DataSet
        Dim dr As DataRow

        l_dataset_AnnuityDet = YMCARET.YmcaBusinessObject.RetireesInformationBOClass.LookUpAnnuityDetails(l_string_PersId, l_string_AnnuityId)
        If (l_dataset_AnnuityDet.Tables(0).Rows.Count > 0) Then
            dr = l_dataset_AnnuityDet.Tables(0).Rows(0)
            lblPersonalPreTax.Text = IIf(IsDBNull(dr.Item("Personal Pre-Tax")), "", dr.Item("Personal Pre-Tax")) 'dr.Item("Personal Pre-Tax")
            lblPersonalPostTax.Text = IIf(IsDBNull(dr.Item("Personal Post-Tax")), "", dr.Item("Personal Post-Tax")) 'dr.Item("Personal Post-Tax")
            lblYMCA.Text = IIf(IsDBNull(dr.Item("YMCA")), "", dr.Item("YMCA")) 'dr.Item("YMCA")
            lblTotalPayment.Text = IIf(IsDBNull(dr.Item("Total Payment")), "", dr.Item("Total Payment")) 'dr.Item("Total Payment")
            lblSocialSecurity.Text = IIf(IsDBNull(dr.Item("Social Security Adj")), "", dr.Item("Social Security Adj")) 'dr.Item("Social Security Adj")
            lblPersonalPreTaxRem.Text = IIf(IsDBNull(dr.Item("Personal Pre-Tax Res")), "", dr.Item("Personal Pre-Tax Res")) 'dr.Item("Personal Pre-Tax Res")
            lblPersonalPostTaxRes.Text = IIf(IsDBNull(dr.Item("Personal Post-Tax Res")), "", dr.Item("Personal Post-Tax Res")) 'dr.Item("Personal Post-Tax Res")
            lblYMCARes.Text = IIf(IsDBNull(dr.Item("YMCA Res")), "", dr.Item("YMCA Res")) ' dr.Item("YMCA Res")
            lblDeathBenefitRemaining.Text = IIf(IsDBNull(dr.Item("Death Benefit Remaining")), "", dr.Item("Death Benefit Remaining")) 'dr.Item("Death Benefit Remaining")

        End If
    End Sub
    Public Sub LoadAdjustments()

        Dim l_string_AnnuityId As String
        l_string_AnnuityId = Request.QueryString("AnnuityID")
        Dim l_dataset_AnnuityPaid As DataSet
        l_dataset_AnnuityPaid = YMCARET.YmcaBusinessObject.RetireesInformationBOClass.LookUpAdjustments(l_string_AnnuityId)
        If (l_dataset_AnnuityPaid.Tables(0).Rows.Count > 0) Then
            DataGridAdjustments.DataSource = l_dataset_AnnuityPaid
            DataGridAdjustments.DataBind()
        End If
    End Sub

    Private Sub ButtonClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonClose.Click
        Dim msg As String

        msg = msg + "<Script Language='JavaScript'>"

        msg = msg + "self.close();"

        msg = msg + "</Script>"

        Response.Write(msg)
    End Sub
End Class

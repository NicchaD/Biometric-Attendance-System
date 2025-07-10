'**************************************************************************************************************/
' Copyright YMCA Retirement Fund All Rights Reserved. 
' Project Name		:	YMCA-YRS
' FileName			:   AddStatewithholdingtax.vb
' Author Name		:	Megha Lad
' Employee ID		:	
' Email	    	    :	
' Contact No		:	
' Creation Time 	:	01/10/2019
' Description	    :	Add/Edit State Withholding Tax Details.
' Declared in Version : 20.7.0 | YRS-AT-4598 - YRS enh: State Withholding Project - Annuity Payroll Processing
'**************************************************************************************************************
' MODIFICATION HISTORY:
' ------------------------------------------------------------------------------------------------------
' Developer Name              | Date         | Version No      | Ticket
' ------------------------------------------------------------------------------------------------------
' Pooja Kumkar                | 11/19/2019   | 20.7.0          | YRS-AT-4719 - State Withholding - Additional text & warning messages for AL, CA and MA.
' ------------------------------------------------------------------------------------------------------
'**************************************************************************************************************/
Imports YMCAObjects


Public Class AddStatewithholdingtax
    Inherits System.Web.UI.Page
    Protected WithEvents NTNoOfExemption As CustomControls.NumberTextBox
    Protected WithEvents NTFlatAmount As CustomControls.NumberTextBox
    Protected WithEvents NTAdditionalAmount As CustomControls.NumberTextBox
    Protected WithEvents NTNewyorkCityOrYonkers As CustomControls.NumberTextBox

    Public Property LstSTWInputList() As List(Of YMCAObjects.StateWithholdingInput)
        Get
            Return CType(ViewState("LstSTWInput"), List(Of YMCAObjects.StateWithholdingInput))
        End Get
        Set(ByVal Value As List(Of YMCAObjects.StateWithholdingInput))
            ViewState("LstSTWInput") = Value
        End Set
    End Property
    Public Property UserStateCode As String
        Get
            If Not (ViewState("UserStateCode")) Is Nothing Then
                Return (CType(ViewState("UserStateCode"), String))
            Else
                Return False
            End If
        End Get
        Set(ByVal Value As String)
            ViewState("UserStateCode") = Value
        End Set
    End Property
    Public Property UserGrossAmount As Double?
        Get
            If Not (ViewState("UserGrossAmount")) Is Nothing Then
                Return (CType(ViewState("UserGrossAmount"), Double?))
            Else
                Return Nothing
            End If
        End Get
        Set(ByVal Value As Double?)
            ViewState("UserGrossAmount") = Value
        End Set
    End Property
    Public Property UserFederalAmount As Double?
        Get
            If Not (ViewState("UserFederalAmount")) Is Nothing Then
                Return (CType(ViewState("UserFederalAmount"), Double?))
            Else
                Return Nothing
            End If
        End Get
        Set(ByVal Value As Double?)
            ViewState("UserFederalAmount") = Value
        End Set
    End Property
    Public Property UserFederalType As String
        Get
            If Not (ViewState("UserFederalType")) Is Nothing Then
                Return (CType(ViewState("UserFederalType"), String))
            Else
                Return Nothing
            End If
        End Get
        Set(ByVal Value As String)
            ViewState("UserFederalType") = Value
        End Set
    End Property
    Public Property PersonID As String
        Get
            If Not (ViewState("perssID")) Is Nothing Then
                Return (CType(ViewState("perssID"), String))
            Else
                Return False
            End If
        End Get
        Set(ByVal Value As String)
            ViewState("perssID") = Value
        End Set
    End Property
    Public Property PreFixID As String
        Get
            If Not (ViewState("preFix")) Is Nothing Then
                Return (CType(ViewState("preFix"), String))
            Else
                Return False
            End If
        End Get
        Set(ByVal Value As String)
            ViewState("preFix") = Value
        End Set
    End Property
    Public Property ObjSWHPerssDetail As YMCAObjects.StateWithholdingDetails
        Get
            If Not (ViewState("ObjSWHPerssDetail")) Is Nothing Then
                Return (CType(ViewState("ObjSWHPerssDetail"), YMCAObjects.StateWithholdingDetails))
            Else
                Return Nothing
            End If
        End Get
        Set(ByVal Value As YMCAObjects.StateWithholdingDetails)
            ViewState("ObjSWHPerssDetail") = Value
        End Set
    End Property
    Public Property STWSaveAtMainPage As Boolean
        Get
            If Not (ViewState("STWSaveAtMainPage")) Is Nothing Then
                Return (CType(ViewState("STWSaveAtMainPage"), Boolean))
            Else
                Return False
            End If
        End Get
        Set(ByVal Value As Boolean)
            ViewState("STWSaveAtMainPage") = Value
        End Set
    End Property
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("LoggedUserKey") Is Nothing Then
            Response.Redirect("Login.aspx", False)
        End If

        Dim strMode As String
        Try

            If Not Me.IsPostBack Then
                Dim lblPopupmodulename As Label
                lblPopupmodulename = Master.FindControl("lblPopupmodulename")
                lblPopupmodulename.Text = "State Withholding"

                Me.PersonID = Session("STWPersID")
                Session("STWPersID") = Nothing

                Me.PreFixID = Session("PreFix")
                Session("PreFix") = Nothing

                If (Not Session("STWSaveAtMainPage") = Nothing) Then
                    Me.STWSaveAtMainPage = Convert.ToBoolean(Session("STWSaveAtMainPage"))
                    Session("STWSaveAtMainPage") = Nothing
                End If

                'Get Participant Details
                SetUserPersonalDetail()

                'State withholding Input detail
                Me.LstSTWInputList = YMCARET.YmcaBusinessObject.StateWithholdingBO.GetStateWiseInputList()
                BindDropdown()
                strMode = Request.QueryString("type").ToString()
                If (strMode = "Edit") Then
                    Me.FillUserStateTaxDetail()
                End If
            End If
            ValidateUserInputs()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "YRS", "BindEvents();", True)

        Catch ex As Exception
            HelperFunctions.LogException("Page_Load", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
        End Try
    End Sub
    'Set User personal Details
    Protected Sub SetUserPersonalDetail()
        Dim objSTWPersonDetail As StateWithholdingPersonDetails
        Try
            objSTWPersonDetail = YMCARET.YmcaBusinessObject.StateWithholdingBO.GetPersonDetails(Me.PersonID)
            If (Not objSTWPersonDetail Is Nothing) Then

                If (objSTWPersonDetail.intFundNumber <> 0) Then
                    lblFundNo.Text = Convert.ToString(objSTWPersonDetail.intFundNumber)
                Else
                    If (Not Session("STWPersonFundNo") = Nothing) Then
                        lblFundNo.Text = Convert.ToString(Session("STWPersonFundNo"))
                        Session("STWPersonFundNo") = Nothing
                    End If
                End If

                If (Not objSTWPersonDetail.firstName Is Nothing) Then
                    lblName.Text = objSTWPersonDetail.firstName + " " + objSTWPersonDetail.lastName
                Else
                    If (Not Session("STWPersonName") = Nothing) Then
                        lblName.Text = Session("STWPersonName")
                        Session("STWPersonName") = Nothing
                    End If
                End If

                If (Me.PreFixID = "beneficiaryInformation") Or (Me.PreFixID = "retireeInfo") Then
                    If (Not Session("STWPersonStateCode") = Nothing) Then
                        Me.UserStateCode = Session("STWPersonStateCode")
                        Session("STWPersonStateCode") = Nothing
                    Else
                        Me.UserStateCode = objSTWPersonDetail.chrStateCode
                    End If
                    If (Not Session("STWPersonStateName") = Nothing) Then
                        lblCurrentState.Text = Session("STWPersonStateName")
                        Session("STWPersonStateName") = Nothing
                    Else
                        lblCurrentState.Text = objSTWPersonDetail.chvStateName
                    End If
                Else
                    Me.UserStateCode = objSTWPersonDetail.chrStateCode
                    lblCurrentState.Text = objSTWPersonDetail.chvStateName
                End If
               
                If (Not Session("STWGrossAmount") Is Nothing) Then
                    Me.UserGrossAmount = Session("STWGrossAmount")
                    Session("STWGrossAmount") = Nothing
                Else
                    Me.UserGrossAmount = objSTWPersonDetail.numCurrentAnnuity
                End If

            End If

            If (Not Session("STWFederalAmount") Is Nothing) Then
                Me.UserFederalAmount = Session("STWFederalAmount")
                Session("STWFederalAmount") = Nothing
            Else
                Me.UserFederalAmount = Nothing
            End If
            If (Not Session("STWFederalType") Is Nothing) Then
                Me.UserFederalType = Session("STWFederalType")
                Session("STWFederalType") = Nothing
            Else
                Me.UserFederalType = Nothing
            End If
        Catch ex As Exception
            HelperFunctions.LogException("SetUserPersonalDetail", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
        End Try
    End Sub

    'CheckValidation on Every page load
    Protected Sub ValidateUserInputs()
        If ((ddlStateCode.SelectedValue.ToString() <> "N/A") And (UserStateCode <> ddlStateCode.SelectedValue.ToString())) Then
            HelperFunctions.ShowMessageToUser(YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.MESSAGE_STW_INVALID_STATE).DisplayText, EnumMessageTypes.Warning)
        End If
    End Sub

    Protected Sub ValidateWithholdingType()
        If (ddlWithHoldingType.SelectedItem.Value.ToString() = "REF") Then
            HelperFunctions.ShowMessageToUser(YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.MESSAGE_STW_INVALID_DISBURSEMENT).DisplayText, EnumMessageTypes.Error)
            btnSave.Enabled = False
        Else
            btnSave.Enabled = True
        End If

    End Sub

    'This method bind dropdown for state code.
    Protected Sub BindDropdown()
        Try
            ddlStateCode.DataSource = Me.LstSTWInputList
            ddlStateCode.DataTextField = "chvStateCode"
            ddlStateCode.DataValueField = "chvStateCode"
            ddlStateCode.DataBind()
            ddlStateCode.Items.Insert(0, "N/A")

            Dim oRecords As List(Of YMCAObjects.StateWithholdingInput) = Me.LstSTWInputList.Where(Function(x) _
                                                    (x.chvStateCode = Me.UserStateCode)).ToList()
            If (oRecords.Count > 0) Then
                ddlStateCode.SelectedValue = oRecords.FirstOrDefault.chvStateCode
            Else
                ddlStateCode.SelectedValue = "N/A"
            End If
            ShowFieldsApplicableStateWise(ddlStateCode.SelectedValue)
        Catch ex As Exception
            HelperFunctions.LogException("BindDropdown", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
        End Try
    End Sub

    'This method show fields in UI according to the state code provided as parameter
    Protected Sub ShowFieldsApplicableStateWise(ByVal strStateCode As String)
        Dim percentOfFederal As String
        Dim lstSelectedState As List(Of YMCAObjects.StateWithholdingInput) = Me.LstSTWInputList.Where(Function(x) _
                                                (x.chvStateCode = ddlStateCode.SelectedValue.ToString())).ToList()
        Try
            If (lstSelectedState.Count > 0) Then
                ShowAllControl()
                lblStateTaxInfo.Text = lstSelectedState.FirstOrDefault.chvDisplayText
                lblStateTitle.Text = lstSelectedState.FirstOrDefault.chvStateName + " State Withholding"


                If (lstSelectedState.FirstOrDefault.bitDisbursementType.ToString() = "True") Then
                    ddlWithHoldingType.SelectedValue = "ANN"
                End If

                If (lstSelectedState.FirstOrDefault.bitStateTaxNotElected.ToString() = "True") Then
                    trStateTaxNotElected.Visible = True
                    ddlElected.SelectedValue = "No"
                Else
                    trStateTaxNotElected.Visible = False
                End If


                If (lstSelectedState.FirstOrDefault.bitMaritalStatusCode.ToString() = "True") Then
                    trMaritalStatusCode.Visible = True
                    lblMaritalStatus.Enabled = True
                    ddlMaritalStatus.Enabled = True
                    ddlMaritalStatus.SelectedValue = String.Empty
                Else
                    trMaritalStatusCode.Visible = False
                End If


                If (lstSelectedState.FirstOrDefault.bitNoOfExemption.ToString() = "True") Then
                    trNoOfExemption.Visible = True
                    lblNoOfExemption.Enabled = True
                    NTNoOfExemption.Enabled = True
                    NTNoOfExemption.Text = 0
                Else
                    trNoOfExemption.Visible = False
                    NTNoOfExemption.Text = Nothing
                End If

                If (lstSelectedState.FirstOrDefault.bitNewYorkCityAmount.ToString() = "True") Or (lstSelectedState.FirstOrDefault.bitYonkersAmount.ToString() = "True") Then
                    trNewyork.Visible = True
                    rblNYCT.Enabled = True
                    rblYonkers.Enabled = True
                    NTNewyorkCityOrYonkers.Enabled = True
                    rblNYCT.Checked = False
                    rblYonkers.Checked = False
                    NTNewyorkCityOrYonkers.Text = Nothing
                Else
                    trNewyork.Visible = False
                End If


                If (lstSelectedState.FirstOrDefault.bitPercentageofFederalWithholding.ToString() = "True") Then

                    trpercentageoffedwithhold.Visible = True
                    lblPercentageoffederalwithholding.Enabled = True
                    ddlPercentageofFedralwithholding.Enabled = True
                    ddlPercentageofFedralwithholding.SelectedValue = "No"
                    'Federal withholding label text dynamic
                    percentOfFederal = YMCARET.YmcaBusinessObject.MetaConfigMaintenance.GetConfigurationKeyValue("STW_CA_PERCENTOFFEDERAL_AMOUNT").ToString().ToLower().Trim()
                    If (String.IsNullOrEmpty(percentOfFederal)) Then
                        lblPercentageoffederalwithholding.Text = "10% of Federal Withholding"
                        hdnMetaFederalWithholdingValue.Value = "10"
                    Else
                        lblPercentageoffederalwithholding.Text = percentOfFederal + "% of Federal Withholding"
                        hdnMetaFederalWithholdingValue.Value = percentOfFederal
                    End If
                Else
                    trpercentageoffedwithhold.Visible = False
                End If

                If (lstSelectedState.FirstOrDefault.bitFlatAmount.ToString() = "True") Then
                    trFlatAmount.Visible = True
                    lblFlatAmount.Enabled = True
                    NTFlatAmount.Enabled = True
                    rblFlatAmtYes.Enabled = True
                    rblFlatAmtNo.Enabled = True
                    NTFlatAmount.Text = "0.00"
                    If (strStateCode = "CA") Then
                        rblFlatAmtYes.Visible = True
                        rblFlatAmtNo.Visible = True
                        rblFlatAmtNo.Checked = True
                        rblFlatAmtYes.Checked = False
                        rblFlatAmtNo_CheckedChanged(Me, EventArgs.Empty)
                    ElseIf (strStateCode = "NY") Or (strStateCode = "NJ") Then
                        rblFlatAmtYes.Visible = False
                        rblFlatAmtNo.Visible = False
                        NTFlatAmount.Enabled = True
                    End If
                Else
                    trFlatAmount.Visible = False
                    NTFlatAmount.Text = Nothing
                End If

                If (lstSelectedState.FirstOrDefault.bitAdditionalAmount.ToString() = "True") Then
                    trAdditionalAmount.Visible = True
                    lblAdditionalAmount.Enabled = True
                    NTAdditionalAmount.Enabled = True
                    NTAdditionalAmount.Text = "0.00"
                    If (ddlStateCode.SelectedValue.ToString() = "CA") Then
                        aestrikAdditionalAmt.Visible = True
                    Else
                        aestrikAdditionalAmt.Visible = False
                    End If
                Else
                    trAdditionalAmount.Visible = False
                    NTAdditionalAmount.Text = Nothing
                End If
                btnSave.Enabled = True
            Else
                HideAllControl()
                btnSave.Enabled = False
            End If
        Catch ex As Exception
            HelperFunctions.LogException("ShowFieldsApplicableStateWise", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
        End Try
    End Sub

    Protected Sub ddlStateCode_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlStateCode.SelectedIndexChanged
        Try
            ShowFieldsApplicableStateWise(ddlStateCode.SelectedValue.ToString)
        Catch ex As Exception
            HelperFunctions.LogException("ddlStateCode_SelectedIndexChanged", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
        End Try
    End Sub

    Protected Sub ddlWithHoldingType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlWithHoldingType.SelectedIndexChanged
        Try
            ValidateWithholdingType()
        Catch ex As Exception
            HelperFunctions.LogException("ddlWithHoldingType_SelectedIndexChanged", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
        End Try
    End Sub

    'Method to Disable Control fields in UI
    Protected Sub HideAllControl()
        Try
            lblStateTaxInfo.Text = String.Empty
            lblStateTitle.Text = String.Empty
            trpercentageoffedwithhold.Visible = False
            trNoOfExemption.Visible = False
            trNewyork.Visible = False
            trMaritalStatusCode.Visible = False
            trFlatAmount.Visible = False
            trAdditionalAmount.Visible = False
            trStateTaxNotElected.Visible = False
            'btnSave.Enabled = False
        Catch
            Throw
        End Try
    End Sub
    Protected Sub ShowAllControl()
        Try
            lblStateTaxInfo.Text = String.Empty
            lblStateTitle.Text = String.Empty
            trpercentageoffedwithhold.Visible = True
            trNoOfExemption.Visible = True
            trNewyork.Visible = True
            trMaritalStatusCode.Visible = True
            trFlatAmount.Visible = True
            trAdditionalAmount.Visible = True
            trStateTaxNotElected.Visible = True
            'btnSave.Enabled = True
        Catch
            Throw
        End Try
    End Sub
    Protected Sub btnConfirmDialog_Click(sender As Object, e As EventArgs) Handles btnConfirmDialog.Click
        Dim saveSuccess As Boolean
        Dim msg As String
        Dim LstSWHPerssDetail As List(Of YMCAObjects.StateWithholdingDetails)
        Try
            saveSuccess = False
            If (Me.STWSaveAtMainPage = False) Then
                YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("AddStatewithholdingtax", "Save START")
                saveSuccess = YMCARET.YmcaBusinessObject.StateWithholdingBO.SavePersStateTaxdetails(Me.ObjSWHPerssDetail)

                If (saveSuccess = True) Then
                    YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("AddStatewithholdingtax", String.Format("StateWithHolding Data Saved for : {0}", Me.PersonID))
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "SuccessMessage", "ShowDialog('divSuccess');", True)
                    SessionManager.SessionStateWithholding.LstSWHPerssDetail = Nothing
                    Me.ClearField()
                End If
            Else
                SessionManager.SessionStateWithholding.LstSWHPerssDetail.Clear()
                SessionManager.SessionStateWithholding.LstSWHPerssDetail.Add(Me.ObjSWHPerssDetail)
                msg = msg + "<Script Language='JavaScript'>"
                msg = msg + "window.opener.document.forms(0).submit();"
                msg = msg + "self.close();"
                msg = msg + "</Script>"
                Response.Write(msg)
            End If
        Catch ex As Exception
            HelperFunctions.LogException("btnConfirmDialog_Click", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
        Finally
            Me.ObjSWHPerssDetail = Nothing
            YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("AddStatewithholdingtax", "Save END")
            YMCARET.CommonUtilities.WebPerformanceTracer.EndTrace()
        End Try
    End Sub


    Private Function GetPerssStateWithholdingDetail() As YMCAObjects.StateWithholdingDetails
        Dim objSTWdetail As YMCAObjects.StateWithholdingDetails
        Dim lstSelectedState As List(Of YMCAObjects.StateWithholdingInput)
        Dim objOldSTWPerssInput As YMCAObjects.StateWithholdingDetails
        Try
            objSTWdetail = New YMCAObjects.StateWithholdingDetails

            objSTWdetail.bitActive = True
            objSTWdetail.numComputedTaxAmount = Nothing
            objSTWdetail.guiPersID = Me.PersonID
            objSTWdetail.chvStateCode = ddlStateCode.SelectedValue.ToString()

            lstSelectedState = Me.LstSTWInputList.Where(Function(x) _
                                                    (x.chvStateCode = ddlStateCode.SelectedValue.ToString())).ToList()
            If (lstSelectedState.Count > 0) Then

                If (lstSelectedState.FirstOrDefault.bitDisbursementType.ToString() = "True") Then
                    objSTWdetail.chvDisbursementType = ddlWithHoldingType.SelectedValue.ToString()
                End If
                If (lstSelectedState.FirstOrDefault.bitStateTaxNotElected.ToString() = "True") Then
                    If (ddlElected.SelectedValue.ToString() = "No") Then
                        objSTWdetail.bitStateTaxNotElected = 0

                        If (lstSelectedState.FirstOrDefault.bitFlatAmount.ToString() = "True") Then
                            If (Not String.IsNullOrEmpty(NTFlatAmount.Text.ToString())) Then
                                objSTWdetail.numFlatAmount = Convert.ToDouble(NTFlatAmount.Text)
                                If (ddlStateCode.SelectedValue.ToString() = "NY" Or ddlStateCode.SelectedValue.ToString() = "NJ") Then
                                    objSTWdetail.numFlatAmount = Math.Round(Convert.ToDouble(NTFlatAmount.Text))
                                End If
                            Else
                                objSTWdetail.numFlatAmount = "0.00"
                            End If
                        Else
                            objSTWdetail.numFlatAmount = Nothing
                        End If

                        If (lstSelectedState.FirstOrDefault.bitNewYorkCityAmount.ToString() = "True") Then
                            If ((rblNYCT.Checked = True)) Then
                                If (Not String.IsNullOrEmpty(NTNewyorkCityOrYonkers.Text.ToString())) Then
                                    objSTWdetail.numNewYorkCityAmount = Math.Round(Convert.ToDouble(NTNewyorkCityOrYonkers.Text))
                            End If
                        End If
                    Else
                        objSTWdetail.numNewYorkCityAmount = Nothing
                    End If

                    If (lstSelectedState.FirstOrDefault.bitYonkersAmount.ToString() = "True") Then
                        If ((rblYonkers.Checked = True)) Then
                            If (Not String.IsNullOrEmpty(NTNewyorkCityOrYonkers.Text.ToString())) Then
                                objSTWdetail.numYonkersAmount = Math.Round(Convert.ToDouble(NTNewyorkCityOrYonkers.Text))
                            End If
                        End If
                    Else
                        objSTWdetail.numYonkersAmount = Nothing
                    End If

                    If (lstSelectedState.FirstOrDefault.bitPercentageofFederalWithholding.ToString() = "True") Then
                        If (ddlPercentageofFedralwithholding.SelectedValue.ToString() = "Yes") Then
                            objSTWdetail.numPercentageofFederalWithholding = hdnMetaFederalWithholdingValue.Value
                        Else
                            objSTWdetail.numPercentageofFederalWithholding = Nothing
                        End If
                    Else
                        objSTWdetail.numPercentageofFederalWithholding = Nothing
                    End If

                    If (lstSelectedState.FirstOrDefault.bitAdditionalAmount.ToString() = "True") Then
                        If (Not String.IsNullOrEmpty(NTAdditionalAmount.Text.ToString())) Then
                            objSTWdetail.numAdditionalAmount = Convert.ToDouble(NTAdditionalAmount.Text)
                        Else
                            objSTWdetail.numAdditionalAmount = "0.00"
                        End If
                    Else
                        objSTWdetail.numAdditionalAmount = Nothing
                    End If

                    If (lstSelectedState.FirstOrDefault.bitMaritalStatusCode.ToString() = "True") Then
                        objSTWdetail.chvMaritalStatusCode = ddlMaritalStatus.SelectedValue.ToString()
                    Else
                        objSTWdetail.chvMaritalStatusCode = Nothing

                    End If

                    If (lstSelectedState.FirstOrDefault.bitNoOfExemption.ToString() = "True") Then
                        If (Not String.IsNullOrEmpty(NTNoOfExemption.Text.ToString())) Then
                            objSTWdetail.intNoOfExemption = Convert.ToInt32(NTNoOfExemption.Text)
                        Else
                            objSTWdetail.intNoOfExemption = 0
                        End If

                    Else
                        objSTWdetail.intNoOfExemption = Nothing
                    End If

                    If (ddlStateCode.SelectedValue = "CA") Then
                        If (rblFlatAmtYes.Checked = True) Then
                            objSTWdetail.numAdditionalAmount = Nothing
                            objSTWdetail.chvMaritalStatusCode = Nothing
                            objSTWdetail.intNoOfExemption = Nothing
                            objSTWdetail.numPercentageofFederalWithholding = Nothing
                        ElseIf (ddlPercentageofFedralwithholding.SelectedValue = "Yes") Then
                            objSTWdetail.numAdditionalAmount = Nothing
                            objSTWdetail.chvMaritalStatusCode = Nothing
                            objSTWdetail.intNoOfExemption = Nothing
                            objSTWdetail.numFlatAmount = Nothing
                        Else
                            objSTWdetail.numFlatAmount = Nothing
                            objSTWdetail.numPercentageofFederalWithholding = Nothing
                        End If
                    End If
                Else
                    objSTWdetail.bitStateTaxNotElected = 1
                    objSTWdetail.chvMaritalStatusCode = Nothing
                    objSTWdetail.intNoOfExemption = Nothing
                    objSTWdetail.numNewYorkCityAmount = Nothing
                    objSTWdetail.numYonkersAmount = Nothing
                    objSTWdetail.numPercentageofFederalWithholding = Nothing
                    objSTWdetail.numFlatAmount = Nothing
                    objSTWdetail.numAdditionalAmount = Nothing
                End If
            End If
            End If
            Return objSTWdetail
        Catch ex As Exception
            HelperFunctions.LogException("GetPerssStateWithholdingDetail", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
        Finally
            objSTWdetail = Nothing
        End Try
    End Function

    Protected Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click

        Dim errorMessage As String
        Dim btnAddText As String
        Dim btnEditText As String
        Dim checkSecurityAdd As String
        Dim checkSecurityEdit As String
        Dim warningMessage As String 'PK |11/19/2019|Declared variable.

        Try
            errorMessage = String.Empty
            warningMessage = String.Empty 'PK |11/19/2019|Assigned empty value to variable.
            'Check Security Control- Funcationality wise for Add Edit
            btnAddText = Me.PreFixID + "btnStateWithholdAdd"
            checkSecurityAdd = SecurityCheck.Check_Authorization(btnAddText, Convert.ToInt32(Session("LoggedUserKey")))

            btnEditText = Me.PreFixID + "btnStateWithholdEdit"
            checkSecurityEdit = SecurityCheck.Check_Authorization(btnAddText, Convert.ToInt32(Session("LoggedUserKey")))


            If (Not checkSecurityAdd.Equals("True")) Then
                MessageBox.Show(PlaceHolder1, "YMCA-YRS", checkSecurityAdd, MessageBoxButtons.Stop)
                Exit Sub
            End If

            If (Not checkSecurityEdit.Equals("True")) Then
                MessageBox.Show(PlaceHolder1, "YMCA-YRS", checkSecurityEdit, MessageBoxButtons.Stop)
                Exit Sub
            End If



            Me.ObjSWHPerssDetail = GetPerssStateWithholdingDetail()
            If (IsSTWInputDataSamewithExisingData() = False) Then
                If (Not YMCARET.YmcaBusinessObject.StateWithholdingBO.ValidateStateTaxInputDetail(Me.ObjSWHPerssDetail, Me.UserGrossAmount, Me.UserFederalAmount, Me.UserFederalType, errorMessage, warningMessage)) Then
                    'START: PK |11/19/2019 |YRS-AT-4719 | Condition to check error/warning message and display message accordingly.

                    If (Not String.IsNullOrEmpty(errorMessage)) Then
                        HelperFunctions.ShowMessageToUser(errorMessage, EnumMessageTypes.Error)
                    ElseIf (Not String.IsNullOrEmpty(warningMessage)) Then
                        lblFederalWitholdingMessage.Text = warningMessage
                        Me.ObjSWHPerssDetail.bitActive = False
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "DialogMessage", "ShowDialog('ConfirmDialog');", True)
                    End If
                Else
                    lblFederalWitholdingMessage.Text = String.Empty
                    Me.ObjSWHPerssDetail.bitActive = False
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "DialogMessage", "ShowDialog('ConfirmDialog');", True)
                End If
            Else
                lblNochanges.Text = YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.MESSAGE_META_MESSAGES_SAVE_WITHOUT_MODIFIED_MESSAGE).DisplayText
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "DialogMessage", "ShowDialog('divNoChanges');", True)
            End If

        Catch ex As Exception
            HelperFunctions.LogException("btnSave_Click", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
        Finally
            errorMessage = Nothing
        End Try
    End Sub

    Protected Sub ddlElected_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlElected.SelectedIndexChanged
        Try
            If (ddlElected.SelectedValue.ToString() = "Yes") Then
                ddlMaritalStatus.Enabled = False
                lblMaritalStatus.Enabled = False

                NTNoOfExemption.Enabled = False
                lblNoOfExemption.Enabled = False

                rblNYCT.Enabled = False
                rblYonkers.Enabled = False
                NTNewyorkCityOrYonkers.Enabled = False

                lblPercentageoffederalwithholding.Enabled = False
                ddlPercentageofFedralwithholding.Enabled = False

                lblFlatAmount.Enabled = False
                rblFlatAmtYes.Enabled = False
                rblFlatAmtNo.Enabled = False
                NTFlatAmount.Enabled = False
                'rblFlatAmtNo.Checked = True

                lblAdditionalAmount.Enabled = False
                NTAdditionalAmount.Enabled = False
                ValidateWithholdingType()
            Else
                ShowFieldsApplicableStateWise(ddlStateCode.SelectedValue.ToString())
            End If
        Catch ex As Exception
            HelperFunctions.LogException("ddlElected_SelectedIndexChanged", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
        End Try
    End Sub
    'Method to clear all textfields
    Protected Sub ClearField()
        Try
            NTFlatAmount.Text = Nothing
            NTAdditionalAmount.Text = Nothing
            NTNewyorkCityOrYonkers.Text = Nothing
            NTNoOfExemption.Text = Nothing
        Catch
            Throw
        End Try
    End Sub

    Protected Sub ddlPercentageofFedralwithholding_SelectedIndexChanged(sender As Object, e As EventArgs)
        Try
            If (ddlStateCode.SelectedItem.Text = "CA") Then
                If (ddlPercentageofFedralwithholding.SelectedValue.ToString() = "Yes") Then
                    ddlMaritalStatus.Enabled = False
                    lblMaritalStatus.Enabled = False
                    ddlMaritalStatus.SelectedValue = String.Empty

                    lblAdditionalAmount.Enabled = False
                    NTAdditionalAmount.Enabled = False
                    NTAdditionalAmount.Text = "0.00"

                    NTNoOfExemption.Enabled = False
                    lblNoOfExemption.Enabled = False
                    NTNoOfExemption.Text = 0

                    lblFlatAmount.Enabled = False
                    rblFlatAmtYes.Enabled = False
                    rblFlatAmtNo.Enabled = False
                    NTFlatAmount.Enabled = False
                    NTFlatAmount.Text = "0.00"

                ElseIf (ddlPercentageofFedralwithholding.SelectedValue.ToString() = "No") Then
                    ddlMaritalStatus.Enabled = True
                    lblMaritalStatus.Enabled = True

                    lblAdditionalAmount.Enabled = True
                    NTAdditionalAmount.Enabled = True

                    NTNoOfExemption.Enabled = True
                    lblNoOfExemption.Enabled = True

                    lblFlatAmount.Enabled = True
                    rblFlatAmtYes.Enabled = True
                    rblFlatAmtNo.Enabled = True
                    NTFlatAmount.Enabled = False
                    NTFlatAmount.Text = "0.00"
                End If
            End If
            ValidateWithholdingType()
        Catch ex As Exception
            HelperFunctions.LogException("ddlPercentageofFedralwithholding_SelectedIndexChanged", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
        End Try
    End Sub

    Protected Sub rblFlatAmtYes_CheckedChanged(sender As Object, e As EventArgs) Handles rblFlatAmtYes.CheckedChanged
        Try
            If (ddlStateCode.SelectedItem.Text = "CA") Then
                NTFlatAmount.Enabled = True
                ddlMaritalStatus.Enabled = False
                lblMaritalStatus.Enabled = False
                ddlMaritalStatus.SelectedValue = String.Empty

                NTNoOfExemption.Enabled = False
                lblNoOfExemption.Enabled = False
                NTNoOfExemption.Text = 0

                lblPercentageoffederalwithholding.Enabled = False
                ddlPercentageofFedralwithholding.Enabled = False
                ddlPercentageofFedralwithholding.SelectedValue = "No"

                lblAdditionalAmount.Enabled = False
                NTAdditionalAmount.Enabled = False
                NTAdditionalAmount.Text = "0.00"
            End If

            If (ddlStateCode.SelectedItem.Text = "NY") Or (ddlStateCode.SelectedItem.Text = "NJ") Then
                NTFlatAmount.Enabled = True
            End If
            ValidateWithholdingType()
        Catch ex As Exception
            HelperFunctions.LogException("rblFlatAmtYes_CheckedChanged", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
        End Try
    End Sub

    Protected Sub rblFlatAmtNo_CheckedChanged(sender As Object, e As EventArgs) Handles rblFlatAmtNo.CheckedChanged
        Try
            If (ddlStateCode.SelectedItem.Text = "CA") Then
                ddlMaritalStatus.Enabled = True
                lblMaritalStatus.Enabled = True
                NTNoOfExemption.Enabled = True
                lblNoOfExemption.Enabled = True

                lblPercentageoffederalwithholding.Enabled = True
                ddlPercentageofFedralwithholding.Enabled = True
                lblAdditionalAmount.Enabled = True
                NTAdditionalAmount.Enabled = True

                NTFlatAmount.Enabled = False
                NTFlatAmount.Text = "0.00"
            End If

            If (ddlStateCode.SelectedItem.Text = "NY") Or (ddlStateCode.SelectedItem.Text = "NJ") Then
                NTFlatAmount.Enabled = True
            End If
            ValidateWithholdingType()
        Catch ex As Exception
            HelperFunctions.LogException("rblFlatAmtNo_CheckedChanged", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
        End Try
    End Sub

    Protected Sub FillUserStateTaxDetail()
        Dim perssid As String
        Dim LstSWHPerssDetail As List(Of YMCAObjects.StateWithholdingDetails)
        Try
            If (Not SessionManager.SessionStateWithholding.LstSWHPerssDetail Is Nothing) Then
                LstSWHPerssDetail = SessionManager.SessionStateWithholding.LstSWHPerssDetail
            Else
                LstSWHPerssDetail = YMCARET.YmcaBusinessObject.StateWithholdingBO.GetStateTaxDetails(Me.PersonID)
            End If

            If (LstSWHPerssDetail.Count > 0) Then
                With LstSWHPerssDetail.FirstOrDefault
                    If (Not IsDBNull(.chvStateCode)) Then
                        ddlStateCode.SelectedValue = .chvStateCode
                    End If
                    ddlStateCode_SelectedIndexChanged(Me, EventArgs.Empty)


                    If (Not IsDBNull(.chvDisbursementType)) Then
                        ddlWithHoldingType.SelectedValue = .chvDisbursementType
                    End If

                    If (Not IsDBNull(.bitStateTaxNotElected)) Then
                        Dim isStateTaxNotElected As Boolean = .bitStateTaxNotElected
                        If (isStateTaxNotElected = True) Then
                            ddlElected.SelectedValue = "Yes"
                        Else
                            ddlElected.SelectedValue = "No"
                        End If
                        ddlElected_SelectedIndexChanged(Me, EventArgs.Empty)
                    End If
                    If (ddlElected.SelectedValue.ToString() = "No") Then
                        If (Not .numAdditionalAmount Is Nothing) Then
                            NTAdditionalAmount.Text = .numAdditionalAmount
                        End If

                        If (Not .numFlatAmount Is Nothing) Then
                            NTFlatAmount.Text = .numFlatAmount
                            rblFlatAmtYes.Checked = True
                            rblFlatAmtNo.Checked = False

                            rblFlatAmtYes_CheckedChanged(Me, EventArgs.Empty)
                        Else
                            rblFlatAmtNo.Checked = True
                            rblFlatAmtYes.Checked = False
                            rblFlatAmtNo_CheckedChanged(Me, EventArgs.Empty)
                        End If

                        If (Not .numNewYorkCityAmount Is Nothing) Then
                            NTNewyorkCityOrYonkers.Text = .numNewYorkCityAmount
                            rblNYCT.Checked = True
                            rblYonkers.Checked = False
                        End If

                        If (Not .numYonkersAmount Is Nothing) Then
                            NTNewyorkCityOrYonkers.Text = .numYonkersAmount
                            rblYonkers.Checked = True
                            rblNYCT.Checked = False
                        End If

                        If (Not .intNoOfExemption Is Nothing) Then
                            NTNoOfExemption.Text = .intNoOfExemption
                        End If

                        If (Not String.IsNullOrEmpty(.chvMaritalStatusCode)) Then
                            ddlMaritalStatus.SelectedValue = .chvMaritalStatusCode
                        End If


                        If (Not .numPercentageofFederalWithholding Is Nothing) Then
                            Dim numPerofFederalWithholding As String = .numPercentageofFederalWithholding.ToString
                            If (Not String.IsNullOrEmpty(numPerofFederalWithholding)) Then
                                ddlPercentageofFedralwithholding.SelectedValue = "Yes"
                            Else
                                ddlPercentageofFedralwithholding.SelectedValue = "No"
                            End If
                            ddlPercentageofFedralwithholding_SelectedIndexChanged(Me, EventArgs.Empty)
                        End If
                    End If

                End With
            End If
        Catch ex As Exception
            HelperFunctions.LogException("FillUserStateTaxDetail", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
        Finally
            perssid = Nothing
            LstSWHPerssDetail = Nothing
        End Try
    End Sub

    Protected Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Try
            'Make all session Clear


            Dim msg As String
            msg = msg + "<Script Language='JavaScript'>"
            msg = msg + "self.close();"
            msg = msg + "</Script>"
            Response.Write(msg)
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
    End Sub



    Protected Function IsSTWInputDataSamewithExisingData() As Boolean
        Dim objNewSTWPerssInput As YMCAObjects.StateWithholdingDetails
        Dim objOldSTWPerssInput As YMCAObjects.StateWithholdingDetails
        Dim flag As Boolean = False
        Try
            objNewSTWPerssInput = Me.ObjSWHPerssDetail
            If (Not SessionManager.SessionStateWithholding.LstSWHPerssDetail Is Nothing) Then
                If (SessionManager.SessionStateWithholding.LstSWHPerssDetail.Count > 0) Then
                    objOldSTWPerssInput = SessionManager.SessionStateWithholding.LstSWHPerssDetail.FirstOrDefault
                    objOldSTWPerssInput.bitActive = True
                End If
            Else
                objOldSTWPerssInput = YMCARET.YmcaBusinessObject.StateWithholdingBO.GetStateTaxDetails(Me.PersonID).FirstOrDefault
            End If

            If (Not objOldSTWPerssInput Is Nothing) Then
                flag = HelperFunctions.CompareObjects(objOldSTWPerssInput, objNewSTWPerssInput)
            End If

            Return flag
        Catch ex As Exception
            Throw
        End Try
    End Function
    

End Class

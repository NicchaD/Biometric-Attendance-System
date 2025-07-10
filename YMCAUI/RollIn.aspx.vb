'*******************************************************************************
' Copyright YMCA Retirement Fund All Rights Reserved. 
' Project Name		:	YMCA-YRS
' FileName			:	RollIn.aspx.vb
' Author Name		:	Anudeep  
' Creation Date		:	05/15/2014
' Description		:	This form is used to add rollin , process rollin, cancel rollin.
'*******************************************************************************
'Modification History
'********************************************************************************************************************************
'Modified By        Date            Description
'********************************************************************************************************************************
'Anudeep            09/25/2014      BT:2344:YRS 5.0-2279:Add in Administration Screen ability to change messages for YRS or Web site
'Anudeep            05/08/2015      BT:2825:YRS 5.0-2500:Letter of Acceptance In YRS 
'Manthan Rajguru    2015.09.16      YRS-AT-2550: YRS data cleanup: copyright comments and namespace/reference (Changed the Namespace/reference from 'Infotech' to 'YMCARET')
'Manthan Rajguru    2015.11.09      YRS-AT-2475: Changes to Process/Cancel control - enable "Cancel" button for Customer Service reps
'Manthan Rajguru    2016.08.12      YRS-AT-2980 -  YRS enh: FiratName, LastName, Addr1, Addr2 fields should not allow Accent Characters or Tilde Characters
'Santosh Bura       2017.05.29      YRS-AT-3465 -  YRS enh: Rollovers allow Rollback for processing routing under transaction block 
'*******************************************************************************
Imports System.Data.DataRow
Imports System.Security.Permissions
Imports System.Math
Imports System.Web.Services
Imports System.Xml.Serialization
Imports System.IO
Imports System.Xml
Imports YMCAUI.SessionManager
Imports YMCAObjects.MetaMessageList

Public Class RollIn
    Inherits System.Web.UI.Page
    Dim strFormName As String = "FindInfo.aspx?Name=Rollover"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("LoggedUserKey") Is Nothing Then
            Response.Redirect("Login.aspx", False)
            Exit Sub
        End If
        Dim LabelModuleName As Label
        Try
            If Not IsPostBack Then
                FillDropDown()
                LoadAllData()
                ClearControls()
                LabelModuleName = Master.FindControl("LabelModuleName")
                If LabelModuleName IsNot Nothing Then
                    LabelModuleName.Text = "Activities > Roll In > Roll In Receipt >  Fund Id " + lblFundNo.Text.Trim + " - " + lblName.Text
                End If
                SessionRollIns.RollIn_SaveRollover = Nothing
                SessionRollIns.RollIn_SaveReciepts = Nothing
            ElseIf SessionRollIns.RollIn_SaveRollover = True Then
                SaveRollover()
                ClearControls()
                HelperFunctions.ShowMessageToUser(MESSAGE_ROLLIN_CREATION_CONFIRMATION)
            ElseIf SessionRollIns.RollIn_SaveReciepts = True Then
                HelperFunctions.ShowMessageToUser(MESSAGE_ROLLIN_PROCESS_CONFIRMATION)
                LoadRolloverData()
            ElseIf SessionRollIns.RollIn_CancelRollover = True Then
                HelperFunctions.ShowMessageToUser(MESSAGE_ROLLIN_CANCEL_CONFIRMATION)
                LoadRolloverData()
            ElseIf SessionRollIns.Rollin_AddressFill <> Nothing Then
                Dim dtAdddress As DataTable
                Dim dvAddrs As DataView
                dtAdddress = SessionRollIns.Rollin_GetAddress_dtAddress
                ViewState("gvAddrs_Sorting") = Nothing
                ViewState("gvAddrs_PageIndexChanging") = Nothing
                lnkAdd.Visible = True
                lnkChooseAddress.Visible = False
                gvAddrs.SelectedIndex = -1
                gvAddrs.PageIndex = 0
                If dtAdddress IsNot Nothing AndAlso dtAdddress.Rows.Count > 0 Then
                    dvAddrs = dtAdddress.DefaultView
                    HelperFunctions.BindGrid(gvAddrs, dvAddrs, True)
                    ucAddressAdd.Visible = False
                    lblInstAddrs.Text = "Choose any address from below"
                End If

                ScriptManager.RegisterStartupScript(Me, GetType(Page), "BindOpen", "BindEvents(); ShowRolloverDialog('AddrsFill');", True)
            ElseIf SessionRollIns.Rollin_GetAddress_ReloadDialog IsNot Nothing Then
                ucAddressAdd.LoadAddressDetail(Nothing)
                ScriptManager.RegisterStartupScript(Me, GetType(Page), "BindOpen", "BindEvents(); ShowRolloverDialog('AddrsFill');", True)
            ElseIf SessionRollIns.RollIn_gvAddrs_SelectedChanged <> Nothing Then
                Dim strAddressUniqueID As String
                Dim dtAdddress As DataTable
                Dim drAddrs As DataRow()
                dtAdddress = SessionRollIns.Rollin_GetAddress_dtAddress
                strAddressUniqueID = gvAddrs.SelectedRow.Cells(1).Text
                drAddrs = dtAdddress.Select("UniqueId = '" + strAddressUniqueID + "'")
                ucAddressAdd.Visible = True
                ucAddressAdd.EnableControls = False
                ucAddressAdd.LoadAddressDetail(drAddrs)
                lblInstAddrs.Text = "Address"
                ScriptManager.RegisterStartupScript(Me, GetType(Page), "BindOpen", "BindEvents(); ShowRolloverDialog('AddrsFill');", True)
            ElseIf hdnChooseButton.Value = "true" Then
                lnkChooseAddress_Click()
                hdnChooseButton.Value = ""
            ElseIf hdnCloseRollinDialog.Value = "true" Then
                ClearControls()
            End If
        Catch ex As Exception
            HelperFunctions.LogException("RollIn --> Page_Load", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
        Finally
            SessionRollIns.RollIn_SaveRollover = Nothing
            SessionRollIns.RollIn_SaveReciepts = Nothing
            SessionRollIns.RollIn_CancelRollover = Nothing
            SessionRollIns.Rollin_AddressFill = Nothing
            SessionRollIns.Rollin_GetAddress_ReloadDialog = Nothing
            SessionRollIns.RollIn_gvAddrs_SelectedChanged = Nothing
        End Try
    End Sub

    ''' <summary>
    ''' Loads all the related data for the person
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub LoadAllData()
        Try
            LoadGeneralData()
            LoadEmploymentData()
            LoadRolloverData()
        Catch ex As Exception
            HelperFunctions.LogException("RollIn --> LoadAllData", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
        End Try
    End Sub

    ''' <summary>
    ''' Loads all the general data for the person like ssn,fundno, name etc..
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub LoadGeneralData()
        Dim strPersId As String
        Dim dsGeneral As DataSet
        Dim dt As DataTable = Nothing
        Dim ds As DataSet
        Try
            strPersId = Session("PersId")
            dsGeneral = YMCARET.YmcaBusinessObject.RolloversBOClass.LookUpGeneralInfo(strPersId)
            If HelperFunctions.isNonEmpty(dsGeneral) Then
                If dsGeneral.Tables("GeneralInfo").Rows(0).Item("SSNo").ToString() <> "System.DBNull" And dsGeneral.Tables("GeneralInfo").Rows(0).Item("SSNo").ToString() <> "" Then
                    lblSSNo.Text = dsGeneral.Tables("GeneralInfo").Rows(0).Item("SSNo").ToString()
                End If
                If dsGeneral.Tables("GeneralInfo").Rows(0).Item("FundNo").ToString() <> "System.DBNull" And dsGeneral.Tables("GeneralInfo").Rows(0).Item("FundNo").ToString() <> "" Then
                    lblFundNo.Text = dsGeneral.Tables("GeneralInfo").Rows(0).Item("FundNo").ToString()
                End If
                If (dsGeneral.Tables("GeneralInfo").Rows(0).Item("FirstName").ToString() <> "System.DBNull" And dsGeneral.Tables("GeneralInfo").Rows(0).Item("FirstName").ToString() <> "") Then
                    Me.lblName.Text = dsGeneral.Tables("GeneralInfo").Rows(0).Item("FirstName")
                End If

                If (dsGeneral.Tables("GeneralInfo").Rows(0).Item("FirstName").ToString() <> "System.DBNull" And dsGeneral.Tables("GeneralInfo").Rows(0).Item("FirstName").ToString() <> "") Then
                    Me.lblName.Text += " " + dsGeneral.Tables("GeneralInfo").Rows(0).Item("MiddleName")
                End If

                If (dsGeneral.Tables("GeneralInfo").Rows(0).Item("LastName").ToString() <> "System.DBNull" And dsGeneral.Tables("GeneralInfo").Rows(0).Item("LastName").ToString() <> "") Then
                    Me.lblName.Text += " " + dsGeneral.Tables("GeneralInfo").Rows(0).Item("LastName")
                End If
                If (dsGeneral.Tables("GeneralInfo").Rows(0).Item("Marritalstatus").ToString() <> "System.DBNull" And dsGeneral.Tables("GeneralInfo").Rows(0).Item("Marritalstatus").ToString() <> "") Then
                    Me.lblMaritalStatus.Text = " " + dsGeneral.Tables("GeneralInfo").Rows(0).Item("Marritalstatus").ToString()
                End If
                If (dsGeneral.Tables("GeneralInfo").Rows(0).Item("Age").ToString() <> "System.DBNull" And dsGeneral.Tables("GeneralInfo").Rows(0).Item("Age").ToString() <> "") Then
                    Me.lblAge.Text = " " + dsGeneral.Tables("GeneralInfo").Rows(0).Item("Age").ToString() + "Y"
                End If

                ds = Address.GetAddressByEntity(strPersId, EnumEntityCode.PERSON)
                dt = ds.Tables("Address")
                If HelperFunctions.isNonEmpty(dt) Then
                    AddressWebUserControl1.LoadAddressDetail(dt.Select("isPrimary = True"))
                Else
                    AddressWebUserControl1.LoadAddressDetail(Nothing)
                End If

                If dsGeneral.Tables("GeneralInfo").Rows(0).Item("EmailId").ToString() <> "System.DBNull" And dsGeneral.Tables("GeneralInfo").Rows(0).Item("EmailId").ToString() <> "" Then
                    lblEmail.Text = dsGeneral.Tables("GeneralInfo").Rows(0).Item("EmailId").ToString()
                Else
                    lblEmail.Text = ""
                End If
                If dsGeneral.Tables("GeneralInfo").Rows(0).Item("PhoneNo").ToString() <> "System.DBNull" And dsGeneral.Tables("GeneralInfo").Rows(0).Item("PhoneNo").ToString() <> "" Then
                    lblTelephone.Text = dsGeneral.Tables("GeneralInfo").Rows(0).Item("PhoneNo").ToString()
                Else
                    lblTelephone.Text = ""
                End If
            End If
        Catch ex As Exception
            HelperFunctions.LogException("RollIn --> LoadGeneralData", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
        End Try
    End Sub

    ''' <summary>
    ''' Loads and fills the drop-down of the Info source 
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub FillDropDown()

        Dim dsInfoSource As DataSet
        Try
            dsInfoSource = YMCARET.YmcaBusinessObject.RolloversBOClass.GetInfoSource()
            If HelperFunctions.isNonEmpty(dsInfoSource) Then
                drdInfoSource.DataSource = dsInfoSource
                drdInfoSource.DataTextField = "Description"
                drdInfoSource.DataValueField = "Code"
                drdInfoSource.DataBind()
                drdInfoSource.Items.Insert(0, "Select Source")
            Else
                drdInfoSource.DataSource = Nothing
                drdInfoSource.DataBind()
            End If

        Catch ex As Exception
            HelperFunctions.LogException("RollIn --> FillDropDown", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
        End Try
    End Sub


    ''' <summary>
    ''' Loads employement details of person
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub LoadEmploymentData()
        Try
            Dim strPersId As String
            Dim strFundId As String
            strPersId = Session("PersId")
            strFundId = Session("FundId")
            Dim dsEmployment As DataSet
            Dim dsFundEvent As DataSet
            dsEmployment = YMCARET.YmcaBusinessObject.RolloversBOClass.LookUpEmploymentInfo(strPersId, strFundId)
            If HelperFunctions.isNonEmpty(dsEmployment) Then
                Me.gvRollOverEmp.DataSource = dsEmployment
                Me.gvRollOverEmp.DataBind()
            End If
            dsFundEvent = YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.LookUpFundEventInfo(strPersId, strFundId)
        Catch ex As Exception
            HelperFunctions.LogException("RollIn --> LoadEmployementData", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
        End Try
    End Sub

    ''' <summary>
    ''' Loads rollover details of person
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub LoadRolloverData()
        Dim strPersId As String
        Dim dsRollover As DataSet
        Try
            strPersId = Session("PersId")
            dsRollover = YMCARET.YmcaBusinessObject.RolloversBOClass.LookUpRolloverInfo(strPersId)
            HelperFunctions.BindGrid(gvRolloverRoll, dsRollover, True)
        Catch ex As Exception
            HelperFunctions.LogException("RollIn --> LoadRolloverData", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
        End Try
    End Sub

    ''' <summary>
    ''' On click of cancel in the grid this method will be called and opens a confirmation dialog to cancel rollover request
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub gvRolloverRoll_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs) Handles gvRolloverRoll.RowCancelingEdit
        Dim strOutPut As String
        Try
            Dim checkSecurityForm As String = SecurityCheck.Check_Authorization(strFormName, Convert.ToInt32(Session("LoggedUserKey")))
            If Not checkSecurityForm.Equals("True") Then
                HelperFunctions.ShowMessageToUser(checkSecurityForm, EnumMessageTypes.Error)
                Exit Sub
            End If
            'START--Manthan Rajguru | 2015.11.09 | YRS-AT-2475 | Changed the parameter from 'RollInReceipt' to 'RollInReceiptCancel' as separate control rights created for cancelling Roll in reciept
            'Dim checkSecurity As String = SecurityCheck.Check_Authorization("RollInReceipt", Convert.ToInt32(HttpContext.Current.Session("LoggedUserKey")))
            Dim checkSecurity As String = SecurityCheck.Check_Authorization("RollInReceiptCancel", Convert.ToInt32(HttpContext.Current.Session("LoggedUserKey")))
            'END--Manthan Rajguru | 2015.11.09 | YRS-AT-2475 | Changed the parameter from 'RollInReceipt' to 'RollInReceiptCancel' as separate control rights created for cancelling Roll in reciept
            If Not checkSecurity.Equals("True") Then
                HelperFunctions.ShowMessageToUser(checkSecurity, EnumMessageTypes.Error)
                Exit Sub
            End If
            If gvRolloverRoll.Rows(e.RowIndex).Cells(6).Text.ToUpper.Trim = "CLOSED" Then
                HelperFunctions.ShowMessageToUser(MESSAGE_ROLLIN_CANCEL_CLOSED_ROLLIN)
            ElseIf gvRolloverRoll.Rows(e.RowIndex).Cells(6).Text.ToUpper.Trim = "CANCEL" Then
                HelperFunctions.ShowMessageToUser(MESSAGE_ROLLIN_CANCEL_CANCELLED_ROLLIN)
            ElseIf gvRolloverRoll.Rows(e.RowIndex).Cells(6).Text.ToUpper.Trim = "OPEN" Then
                SessionRollIns.RollIn_CancelRollover_RolloverId = gvRolloverRoll.Rows(e.RowIndex).Cells(2).Text
                ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "PopupScriptCancel", "ShowConfirmDialog('" + gvRolloverRoll.Rows(e.RowIndex).Cells(4).Text.Replace("&amp;", "").Replace("&nbsp;", "") + "','" + gvRolloverRoll.Rows(e.RowIndex).Cells(5).Text.Replace("&amp;", "").Replace("&nbsp;", "") + "')", True)
            End If
        Catch ex As Exception
            HelperFunctions.LogException("RollIn --> gvRolloverRoll_RowCancelingEdit", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
        End Try
    End Sub

    ''' <summary>
    ''' On databind of gvRolloverRoll this method will be called for hiding the columns
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub gvRolloverRoll_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvRolloverRoll.RowDataBound
        Dim imgSelect As ImageButton
        Try
            If e.Row.RowType <> DataControlRowType.EmptyDataRow Then
                e.Row.Cells(2).Visible = False
                e.Row.Cells(3).Visible = False
                e.Row.Cells(8).Visible = False
                If e.Row.RowType = DataControlRowType.DataRow AndAlso e.Row.Cells(6).Text = "CLOSED" Then
                    imgSelect = e.Row.Cells(0).FindControl("ImageButtonSel")
                    If imgSelect IsNot Nothing Then
                        imgSelect.ImageUrl = "images\view.gif"
                        imgSelect.ToolTip = "View Receipt"
                    End If
                End If
            End If
        Catch ex As Exception
            HelperFunctions.LogException("RollIn --> gvRolloverRoll_RowDataBound", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
        End Try
    End Sub

    ''' <summary>
    ''' On click of process in the grid this method will be called and opens a reciept dialog to process rollover request
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub gvRolloverRoll_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvRolloverRoll.SelectedIndexChanged
        Dim i As Integer
        Dim strYmcapk As String
        Dim strEmpeventpk As String
        Dim strFundId As String
        Dim strPersId As String
        Dim strRolloverid As String
        Dim dsActiveEvent As DataSet
        Try
            Dim checkSecurityForm As String = SecurityCheck.Check_Authorization(strFormName, Convert.ToInt32(Session("LoggedUserKey")))
            If Not checkSecurityForm.Equals("True") Then
                HelperFunctions.ShowMessageToUser(checkSecurityForm, EnumMessageTypes.Error)
                Exit Sub
            End If
            'START--Manthan Rajguru | 2015.11.09 | YRS-AT-2475 | Changed the parameter from 'RollInReceipt' to 'RollInReceiptProcess' as separate control rights created for Processing Roll in reciept
            'Dim checkSecurity As String = SecurityCheck.Check_Authorization("RollInReceipt", Convert.ToInt32(HttpContext.Current.Session("LoggedUserKey")))
            Dim checkSecurity As String = SecurityCheck.Check_Authorization("RollInReceiptProcess", Convert.ToInt32(HttpContext.Current.Session("LoggedUserKey")))
            'END--Manthan Rajguru | 2015.11.09 | YRS-AT-2475 | Changed the parameter from 'RollInReceipt' to 'RollInReceiptProcess' as separate control rights created for Processing Roll in reciept
            If Not checkSecurity.Equals("True") Then
                HelperFunctions.ShowMessageToUser(checkSecurity, EnumMessageTypes.Error)
                Exit Sub
            End If
            dsActiveEvent = YMCARET.YmcaBusinessObject.RolloversBOClass.GetActiveEmpEvent(Convert.ToString(Session("PersId")))
            If HelperFunctions.isNonEmpty(dsActiveEvent) Then
                strYmcapk = dsActiveEvent.Tables("ActiveEmpEventInfo").Rows(0).Item("Ymcapk").ToString()
                strEmpeventpk = dsActiveEvent.Tables("ActiveEmpEventInfo").Rows(0).Item("EmpEventPk").ToString()
                strRolloverid = gvRolloverRoll.SelectedRow.Cells(2).Text
                Session("Rolloverid") = strRolloverid
                strFundId = Session("FundId")
                strPersId = Session("PersId")
                SessionRollIns.Rollover_ReceivedDate = gvRolloverRoll.SelectedRow.Cells(9).Text

                If gvRolloverRoll.SelectedRow.Cells(6).Text <> "&nbsp;" And gvRolloverRoll.SelectedRow.Cells(6).Text.ToUpper.Trim = "CLOSED" Then
                    Dim dsRolloverRcpts As DataSet
                    dsRolloverRcpts = YMCARET.YmcaBusinessObject.RolloverRecieptBOClass.GetRolloverRcptsData(strRolloverid)
                    If HelperFunctions.isNonEmpty(dsRolloverRcpts) Then
                        TextboxCheckDate.Text = dsRolloverRcpts.Tables("RolloverRcptsInfo").Rows(0).Item("CheckDate").ToString()
                        TextboxCheckNo.Text = dsRolloverRcpts.Tables("RolloverRcptsInfo").Rows(0).Item("CheckNum").ToString()
                        TextboxheckReceivedDate.Text = dsRolloverRcpts.Tables("RolloverRcptsInfo").Rows(0).Item("ReceivedDate").ToString()
                        TextboxTaxableAmount.Text = IIf(dsRolloverRcpts.Tables("RolloverRcptsInfo").Rows(0).Item("Taxable").ToString().Trim = "", "0.0", dsRolloverRcpts.Tables("RolloverRcptsInfo").Rows(0).Item("Taxable").ToString())
                        TextboxNonTaxableAmount.Text = IIf(dsRolloverRcpts.Tables("RolloverRcptsInfo").Rows(0).Item("NonTaxable").ToString().Trim = "", "0.0", dsRolloverRcpts.Tables("RolloverRcptsInfo").Rows(0).Item("NonTaxable").ToString())
                        TextboxCheckTotal.Text = Convert.ToDecimal(TextboxTaxableAmount.Text) + Convert.ToDecimal(TextboxNonTaxableAmount.Text)
                        TextboxCheckDate.Enabled = False
                        TextboxCheckNo.Enabled = False
                        TextboxheckReceivedDate.Enabled = False
                        TextboxTaxableAmount.Enabled = False
                        TextboxNonTaxableAmount.Enabled = False
                        TextboxCheckTotal.Enabled = False
                        TextboxCheckDate.Enabled = False
                        lblInstName.Text = gvRolloverRoll.SelectedRow.Cells(4).Text
                        lblAccNo.Text = gvRolloverRoll.SelectedRow.Cells(5).Text
                        lblInfos.Text = gvRolloverRoll.SelectedRow.Cells(7).Text
                        lblRcvdDate.Text = gvRolloverRoll.SelectedRow.Cells(9).Text
                        ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "PopupScript3", "ShowRecieptsDialog('Read')", True)
                    End If
                ElseIf gvRolloverRoll.SelectedRow.Cells(6).Text.ToUpper.Trim = "OPEN" Then
                    TextboxCheckDate.Text = ""
                    TextboxCheckNo.Text = ""
                    TextboxheckReceivedDate.Text = ""
                    TextboxTaxableAmount.Text = ""
                    TextboxNonTaxableAmount.Text = ""
                    TextboxCheckTotal.Text = ""
                    TextboxCheckDate.Enabled = True
                    TextboxCheckNo.Enabled = True
                    TextboxheckReceivedDate.Enabled = True
                    TextboxTaxableAmount.Enabled = True
                    TextboxNonTaxableAmount.Enabled = True
                    TextboxCheckTotal.Enabled = True
                    TextboxCheckTotal.ReadOnly = True
                    TextboxCheckDate.Enabled = True
                    lblInstName.Text = gvRolloverRoll.SelectedRow.Cells(4).Text
                    lblAccNo.Text = gvRolloverRoll.SelectedRow.Cells(5).Text
                    lblInfos.Text = gvRolloverRoll.SelectedRow.Cells(7).Text
                    lblRcvdDate.Text = gvRolloverRoll.SelectedRow.Cells(9).Text
                    TextboxTaxableAmount.Text = 0.0
                    TextboxNonTaxableAmount.Text = 0.0
                    TextboxCheckTotal.Text = 0.0
                    ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "PopupScript3", "ShowRecieptsDialog('Edit')", True)
                ElseIf gvRolloverRoll.SelectedRow.Cells(6).Text.ToUpper.Trim = "CANCEL" Then
                    HelperFunctions.ShowMessageToUser(MESSAGE_ROLLIN_PROCESS_CLOSED_ROLLIN)
                End If
            End If

        Catch ex As Exception
            HelperFunctions.LogException("RollIn --> gvRolloverRoll_SelectedIndexChanged", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
        End Try
    End Sub

    ''' <summary>
    ''' On click of add button this method will be called and opens a rollover dialog to add a rollover request to the person
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ButtonAddForm_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonAddForm.Click
        Try
            Dim checkSecurityForm As String = SecurityCheck.Check_Authorization(strFormName, Convert.ToInt32(Session("LoggedUserKey")))
            If Not checkSecurityForm.Equals("True") Then
                HelperFunctions.ShowMessageToUser(checkSecurityForm, EnumMessageTypes.Error)
                Exit Sub
            End If
            Dim checkSecurity As String = SecurityCheck.Check_Authorization("RollInAdd", Convert.ToInt32(Session("LoggedUserKey")))
            If Not checkSecurity.Equals("True") Then
                HelperFunctions.ShowMessageToUser(checkSecurity, EnumMessageTypes.Error)
                Exit Sub
            End If
            Dim strFundEvent As String
            strFundEvent = Session("FundEvent")
            If strFundEvent.Trim.ToUpper = "AE" Or strFundEvent.Trim.ToUpper = "PE" Or strFundEvent.Trim.ToUpper = "RA" Or strFundEvent.Trim.ToUpper = "RE" Then
                ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "Open", "ShowRolloverDialog('Open');", True)
            Else
                HelperFunctions.ShowMessageToUser(MESSAGE_ROLLIN_ACTIVE_FUNDEVENT_NOT_FOUND)
            End If
        Catch ex As Exception
            HelperFunctions.LogException("RollIn --> ButtonAddForm_Click", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
        End Try
    End Sub

    ''' <summary>
    ''' On click of save button in the rollover dialog this method will be called for validating the fields given
    ''' </summary>
    ''' <param name="InstitutionName"></param>
    ''' <param name="AccountNo"></param>
    ''' <param name="ReceivedDate"></param>
    ''' <param name="SourceInfo"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <WebMethod()> _
    Public Shared Function CheckRolloverValues(ByVal InstitutionName As String, ByVal AccountNo As String, ByVal ReceivedDate As String, ByVal SourceInfo As String) As String()
        Dim rl As New RollIn
        Dim strMessage(2) As String
        Dim strMsg As String = String.Empty
        Dim dtmCompareDate1 As DateTime
        Dim checkSecurity As String
        Dim objMessage As YMCARET.YmcaBusinessObject.MetaMessageBO
        Try
            checkSecurity = SecurityCheck.Check_Authorization(rl.strFormName, Convert.ToInt32(HttpContext.Current.Session("LoggedUserKey")))
            If Not checkSecurity.Equals("True") Then
                strMsg = checkSecurity
                Exit Function
            End If
            dtmCompareDate1 = System.DateTime.Today.AddDays(-182)
            If Trim(InstitutionName) = "" Then
                strMsg = objMessage.GetMessageByTextMessageNo(MESSAGE_ROLLIN_INST_CANT_BE_BLANK)
            ElseIf Trim(ReceivedDate) = "" Then
                strMsg = objMessage.GetMessageByTextMessageNo(MESSAGE_ROLLIN_RCVD_CANT_BE_BLANK)
            ElseIf (SourceInfo = "0") Then
                strMsg = objMessage.GetMessageByTextMessageNo(MESSAGE_ROLLIN_SELECT_INFO_SOURCE)
            ElseIf (System.DateTime.Compare(Convert.ToDateTime(ReceivedDate), System.DateTime.Today()) = 1) Then
                strMsg = objMessage.GetMessageByTextMessageNo(MESSAGE_ROLLIN_RCVD_GRTR_THAN_TODAY)
            ElseIf (System.DateTime.Compare(Convert.ToDateTime(ReceivedDate), dtmCompareDate1) <> 1) Then
                strMsg = objMessage.GetMessageByTextMessageNo(MESSAGE_ROLLIN_RCVD_ATLST_SIX_OLD)
            End If
            If strMsg <> String.Empty Then
                strMessage(0) = "error"
                strMessage(1) = strMsg
            Else
                strMessage(0) = "success"
                strMessage(1) = ""
                SessionRollIns.RollIn_SaveRollover = True
            End If
            Return strMessage
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    ''' <summary>
    ''' On click of save button in the rollover dialog After validating the fields in rollover this method will be called for saving the rollover request
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SaveRollover()
        Try
            Dim strRolloverId As String = String.Empty
            Dim strGuid As String
            Dim strPersId As String
            Dim strFundId As String
            Dim dsAddress As New DataSet
            Dim strAddressId As String = String.Empty
            Dim dtRollInDetails As DataTable 'Manthan Rajguru | 2016.08.12 | YRS-AT-2980 | Declared datatable Object
            strGuid = Session("Guid")

            strPersId = Session("PersId")
            strFundId = Session("FundId")

            ucAddressAdd.IsPrimary = "1"
            ucAddressAdd.EntityCode = EnumEntityCode.INST.ToString
            ucAddressAdd.AddrCode = "OFFICE"
            dsAddress.Tables.Add(ucAddressAdd.GetAddressTable.Copy)
            dsAddress.Tables(0).TableName = "Address"
            If ucAddressAdd.UniqueId.ToString <> String.Empty Then
                strAddressId = ucAddressAdd.UniqueId
            End If

            strRolloverId = YMCARET.YmcaBusinessObject.RolloversBOClass.SaveRolloverInfo(strPersId, strFundId, TextBoxInstitution.Text.Trim(), "OPEN", TextBoxDateRecieved.Text.Trim(), Convert.ToString(drdInfoSource.SelectedValue), TextBoxAccountNo.Text, Not chkdntGenerateLtr.Checked, dsAddress, strAddressId)

            'Start - Manthan Rajguru | 2016.08.12 | YRS-AT-2980 | Commented existing code and added parameters to get RollIn details
            If Not String.IsNullOrEmpty(strRolloverId) Then
                dtRollInDetails = YMCARET.YmcaBusinessObject.RolloversBOClass.GetInstitutionNameAndAddress(strRolloverId)
                If Not chkdntGenerateLtr.Checked AndAlso HelperFunctions.isNonEmpty(dtRollInDetails) Then
                    'OpenReportViewer(strRolloverId)
                    OpenReportViewer(strRolloverId, dtRollInDetails)
                End If
            End If
            'End - Manthan Rajguru | 2016.08.12 | YRS-AT-2980 | Commented existing code and added parameters to get RollIn details
            LoadRolloverData()
        Catch ex As Exception
            HelperFunctions.LogException("RollIn --> SaveRollover", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
        End Try
    End Sub

    ''' <summary>
    ''' On click of close button this method will be called to exit from the page
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ButtonCloseForm_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonCloseForm.Click
        Try
            Session("Page") = "Rollover"
            Response.Redirect("FindInfo.aspx?Name=Rollover", False)
        Catch ex As Exception
            HelperFunctions.LogException("RollIn --> ButtonCloseForm_Click", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
        End Try
    End Sub

    ''' <summary>
    ''' Clears all the controls of rollover dialog 
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ClearControls()
        Try
            Dim dvAddrs As DataView
            TextBoxDateRecieved.Text = System.DateTime.Today()
            TextBoxInstitution.Text = ""
            If drdInfoSource.Items.Count > 0 Then
                drdInfoSource.SelectedIndex = 0
            End If
            TextBoxAccountNo.Text = ""
            ucAddressAdd.LoadAddressDetail(Nothing)
            ucAddressAdd.EnableControls = True
            ucAddressAdd.Visible = True
            chkdntGenerateLtr.Checked = False
            HelperFunctions.BindGrid(gvAddrs, dvAddrs)
            lnkAdd.Visible = False
            lnkChooseAddress.Visible = False
            ViewState("gvAddrs_Sorting") = Nothing
            ViewState("gvAddrs_PageIndexChanging") = Nothing
            SessionRollIns.Rollin_GetAddress_dtAddress = Nothing
            hdnCloseRollinDialog.Value = ""
            lblInstAddrs.Text = "Address"
        Catch ex As Exception
            HelperFunctions.LogException("RollIn --> ClearControls", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
        End Try
    End Function

    ''' <summary>
    ''' On click of save button in the reciepts dialog this method will be called for validating and saving the reciept entry
    ''' </summary>
    ''' <param name="NonTaxableAmount"></param>
    ''' <param name="TaxableAmount"></param>
    ''' <param name="ReceivedDate"></param>
    ''' <param name="CheckNo"></param>
    ''' <param name="CheckDate"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <WebMethod()> _
    Public Shared Function SaveReceipt(ByVal NonTaxableAmount As String, ByVal TaxableAmount As String, ByVal ReceivedDate As String, ByVal CheckNo As String, ByVal CheckDate As String) As String()
        Dim strYmcapk As String
        Dim strEmpeventpk As String
        Dim strFundeventpk As String
        Dim strRolloverId As String
        Dim strPersId As String
        Dim strMessage(2) As String
        Dim dsActiveEvent As DataSet
        Dim objMessage As YMCARET.YmcaBusinessObject.MetaMessageBO
        Try
            Dim checkSecurity As String = SecurityCheck.Check_Authorization("Rollovers.aspx?Name=Receipts", Convert.ToInt32(HttpContext.Current.Session("LoggedUserKey")))
            If Not checkSecurity.Equals("True") Then
                strMessage(0) = "error"
                strMessage(1) = checkSecurity
                Return strMessage
            End If
            strPersId = HttpContext.Current.Session("PersId")
            dsActiveEvent = YMCARET.YmcaBusinessObject.RolloversBOClass.GetActiveEmpEvent(Convert.ToString(strPersId))
            If HelperFunctions.isNonEmpty(dsActiveEvent) Then
                strYmcapk = dsActiveEvent.Tables("ActiveEmpEventInfo").Rows(0).Item("Ymcapk").ToString()
                strEmpeventpk = dsActiveEvent.Tables("ActiveEmpEventInfo").Rows(0).Item("EmpEventPk").ToString()
                strRolloverId = HttpContext.Current.Session("Rolloverid")
                strFundeventpk = HttpContext.Current.Session("FundId")
            End If

            Dim dtmCompareDate1 As DateTime
            dtmCompareDate1 = System.DateTime.Today.AddDays(-182)
            Dim dateAccountDate As Date
            dateAccountDate = YMCARET.YmcaBusinessObject.RolloverRecieptBOClass.GetAccountDate().ToString().Trim()
            Dim dtmFirstdate As Date = DateAdd(DateInterval.Month, DateDiff(DateInterval.Month, Date.MinValue, dateAccountDate), Date.MinValue)
            Dim blnDateErr As Boolean
            Dim strMsg As String
            blnDateErr = False
            If (System.DateTime.Compare(Convert.ToDateTime(CheckDate), System.DateTime.Today()) = 1) Then
                blnDateErr = True
                strMsg = objMessage.GetMessageByTextMessageNo(MESSAGE_ROLLIN_CHECK_DATE_CANT_BE_GRTR_THAN_TODAY)
            ElseIf (System.DateTime.Compare(Convert.ToDateTime(ReceivedDate), System.DateTime.Today()) = 1) Then
                blnDateErr = True
                strMsg = objMessage.GetMessageByTextMessageNo(MESSAGE_ROLLIN_CHECKRCVD_DATE_CANT_BE_GRTR_THAN_TODAY)
            ElseIf (System.DateTime.Compare(Convert.ToDateTime(ReceivedDate), dtmCompareDate1) <> 1) Then
                blnDateErr = True
                strMsg = objMessage.GetMessageByTextMessageNo(MESSAGE_ROLLIN_CHECKRCVDDATE_ATLST_SIX_OLD)
            ElseIf (System.DateTime.Compare(Convert.ToDateTime(CheckDate), dtmCompareDate1) <> 1) Then
                blnDateErr = True
                strMsg = objMessage.GetMessageByTextMessageNo(MESSAGE_ROLLIN_CHECKDATE_ATLST_SIX_OLD)
            ElseIf (Convert.ToDateTime(ReceivedDate) < Convert.ToDateTime(SessionRollIns.Rollover_ReceivedDate)) Then
                blnDateErr = True
                strMsg = objMessage.GetMessageByTextMessageNo(MESSAGE_ROLLIN_CHECKRCVDDATE_IS_EARLIER_RCVDDATE)
            ElseIf (Convert.ToDateTime(ReceivedDate) < Convert.ToDateTime(dtmFirstdate)) Then
                blnDateErr = True
                strMsg = objMessage.GetMessageByTextMessageNo(MESSAGE_ROLLIN_CHECKRCVD_DATE_NOT_FIRSTDAY_MONTH)
            ElseIf (Convert.ToDateTime(CheckDate) > Convert.ToDateTime(ReceivedDate)) Then
                blnDateErr = True
                strMsg = objMessage.GetMessageByTextMessageNo(MESSAGE_ROLLIN_CHECK_DATE_LESS_RCVD_DATE)
            End If
            If (blnDateErr) Then
                strMessage(0) = "error"
                strMessage(1) = strMsg
            Else
                Dim stroutput As String
                stroutput = YMCARET.YmcaBusinessObject.RolloverRecieptBOClass.AddRolloverData(strPersId, strFundeventpk, strYmcapk, strRolloverId, CheckNo.Trim(), CheckDate.Trim(), ReceivedDate.Trim(), TaxableAmount.Trim(), NonTaxableAmount.Trim())
                If stroutput = "0" Then
                    strMessage(0) = "success"
                    strMessage(1) = ""
                    SessionRollIns.RollIn_SaveReciepts = True
                End If

            End If
            Return strMessage
        Catch ex As Exception
            'START:  SB | 2017.05.29 | YRS-AT-3465 | Throw is commented and the values are assigned to the string variables which display error message, also exception is logged 
            'Throw ex
            strMessage(0) = "error"
            strMessage(1) = ex.Message
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("RollIn_SaveReceipt", ex)
            Return strMessage ' Function will return human readable error message to the ajax function
            'END:  SB | 2017.05.29 | YRS-AT-3465 | Throw is commented and the values are assigned to the string variables which display error message, also exception is logged 
        End Try
    End Function

    ''' <summary>
    ''' On click of cancel button in the cancel confirm dialog this method will be called for cancelling the rollover request
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <WebMethod()>
    Public Shared Function CancelRollover() As String
        Dim strOutPut As String
        Dim strRolloverID As String
        Try
            If SessionRollIns.RollIn_CancelRollover_RolloverId IsNot Nothing Then
                strRolloverID = SessionRollIns.RollIn_CancelRollover_RolloverId
                strOutPut = YMCARET.YmcaBusinessObject.RolloversBOClass.CancelRolloverRequest(strRolloverID)
                SessionRollIns.RollIn_CancelRollover_RolloverId = Nothing
                SessionRollIns.RollIn_CancelRollover = True
            End If
            Return strOutPut
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    ''' <summary>
    ''' On click of close button in the cancel confirm dialog this method will be called for clearing the session variable
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <WebMethod()>
    Public Shared Function ClearRolloverId() As String
        Try
            SessionRollIns.RollIn_CancelRollover_RolloverId = Nothing
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    ''' <summary>
    ''' On selection of the institution in the intellisense dialog this method will be called for getting the address of institution
    ''' </summary>
    ''' <param name="strinstID"></param>
    ''' <param name="strAddress"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <WebMethod()>
    Public Shared Function GetAddress(ByVal strinstID As String, ByVal strAddress As String) As String
        Dim dsInstAddress As DataSet
        Dim strResult As String = String.Empty
        Try
            If strinstID.Length > 0 Then
                dsInstAddress = Address.GetAddressByEntity(strinstID, EnumEntityCode.INST)
                If HelperFunctions.isNonEmpty(dsInstAddress) Then
                    SessionRollIns.Rollin_GetAddress_dtAddress = dsInstAddress.Tables(0)
                    SessionRollIns.Rollin_AddressFill = True
                    strResult = "1"
                ElseIf strAddress <> "" Then
                    SessionRollIns.Rollin_GetAddress_ReloadDialog = "ReloadDialog"
                End If
            End If
            Return strResult
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    ''' <summary>
    ''' After saving the rollover request to open the report and copy to IDM this method will be called
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function OpenReportViewer(ByVal guiRolloverId As String, ByVal dtRollInDetails As DataTable) 'Manthan Rajguru |2016.08.12 |YRS-AT-2980 | Adding parameters to get address details
        Dim lstAttributes(7) As String
        Try       
            lstAttributes(0) = lblFundNo.Text
            'Start - Manthan Rajguru |2016.08.12 |YRS-AT-2980 | Commented existing code and assigning address details and institution name from database instead of Controls
            'lstAttributes(1) = TextBoxInstitution.Text
            lstAttributes(1) = dtRollInDetails.Rows(0)("RollOverInstitutionName")
            lstAttributes(2) = String.Empty
            lstAttributes(3) = String.Empty
            lstAttributes(4) = String.Empty
            lstAttributes(5) = String.Empty

            If HelperFunctions.isNonEmpty(dtRollInDetails) Then
                'START:  SB | 2017.05.29 | YRS-AT-3465 | For handling DBnull error .toString() method is used, Previous code is commented 
                'lstAttributes(2) = dtRollInDetails.Rows(0)("Address1") 'ucAddressAdd.Address1
                'lstAttributes(3) = dtRollInDetails.Rows(0)("Address2") 'ucAddressAdd.Address2
                'lstAttributes(4) = dtRollInDetails.Rows(0)("Address3") 'ucAddressAdd.Address3
                'lstAttributes(5) = dtRollInDetails.Rows(0)("City") + "," + dtRollInDetails.Rows(0)("State") + " " + dtRollInDetails.Rows(0)("ZipCode") 'ucAddressAdd.City + ", " + ucAddressAdd.DropDownListStateValue + " " + ucAddressAdd.ZipCode 'AA 05.08.2015 BT:2825:YRS 5.0-2500:Added to change the format of address
                lstAttributes(2) = dtRollInDetails.Rows(0)("Address1").ToString()
                lstAttributes(3) = dtRollInDetails.Rows(0)("Address2").ToString()
                lstAttributes(4) = dtRollInDetails.Rows(0)("Address3").ToString()
                lstAttributes(5) = String.Format("{0},{1} {2}", dtRollInDetails.Rows(0)("City").ToString(), dtRollInDetails.Rows(0)("State").ToString(), dtRollInDetails.Rows(0)("ZipCode").ToString())
                'END:  SB | 2017.05.29 | YRS-AT-3465 | For handling DBnull error .toString() method is used, Previous code is commented 
            End If
            'End - Manthan Rajguru |2016.08.12 |YRS-AT-2980 | Commented existing code and assigning address details and institution name from database instead of Controls
            lstAttributes(6) = TextBoxAccountNo.Text
            lstAttributes(7) = "N"
            Session("RollInLetter") = lstAttributes
            Session("strReportName") = "Letter of Acceptance"
            CopyToIDM("ROLINLOA", "Letter of Acceptance", lstAttributes, "RollIn_ROLINLOA", Session("PersId"), guiRolloverId.ToUpper)
            Dim popupScript As String = " newwindow = window.open('FT\\ReportViewer.aspx', 'ReportPopUp', " +
                                        "'width=1024,height=768, menubar=no, resizable=yes,top=0,left=0, scrollbars=yes, status=yes');" +
                                        " if (window.focus) {newwindow.focus()}" +
                                        " if (!newwindow.closed) {newwindow.focus()}"
            ScriptManager.RegisterStartupScript(Me, GetType(Page), "Report", popupScript, True)
        Catch ex As Exception
            HelperFunctions.LogException("RollIn --> OpenReportViewer", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
        End Try
    End Function

    ''' <summary>
    ''' To copy the report in IDM
    ''' </summary>
    ''' <param name="strDocType"></param>
    ''' <param name="strReportName"></param>
    ''' <param name="ArrParamValues"></param>
    ''' <param name="strOutputFileType"></param>
    ''' <param name="strPersId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CopyToIDM(ByVal strDocType As String, ByVal strReportName As String, ByVal ArrParamValues() As String, ByVal strOutputFileType As String, ByVal strPersId As String, ByVal guiRolloverId As String) As String
        Dim strErrorMessage As String
        Dim IDM As New IDMforAll
        Dim arrAttributes As New ArrayList
        Try
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
            IDM.RefRequestsID = guiRolloverId

            If Not strPersId Is Nothing Then
                IDM.PersId = strPersId
            End If

            IDM.DocTypeCode = strDocType
            IDM.OutputFileType = strOutputFileType
            IDM.ReportName = strReportName.ToString().Trim & ".rpt"
            For Each strAttribute As String In ArrParamValues
                arrAttributes.Add(strAttribute)
            Next
            IDM.ReportParameters = arrAttributes
            strErrorMessage = IDM.ExportToPDF()
            ArrParamValues = Nothing

            Session("FTFileList") = IDM.SetdtFileList


            If Not Session("FTFileList") Is Nothing Then
                Try
                    Dim popupScriptCopytoServer As String = "var a = window.open('FT\\CopyFilestoFileServer.aspx?OR=1&CO=1', 'FileCopyPopUp', " & _
                    "'width=1,height=1, menubar=no, resizable=yes,top=1300,left=1300, scrollbars=no, status=no');"
                    ScriptManager.RegisterStartupScript(Me, GetType(Page), "popupScriptCopytoServer", popupScriptCopytoServer, True)
                Catch
                    Throw
                End Try
            End If
            Return strErrorMessage

        Catch ex As Exception
            HelperFunctions.LogException("RollIn --> CopyToIDM", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
        Finally
            IDM = Nothing
        End Try
    End Function

    Private Sub gvAddrs_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles gvAddrs.PageIndexChanging

        Try
            Dim dtAdddress As DataTable
            Dim dvAddrs As DataView
            Dim gvSort As GridViewCustomSort
            dtAdddress = SessionRollIns.Rollin_GetAddress_dtAddress
            dvAddrs = dtAdddress.DefaultView
            ViewState("gvAddrs_PageIndexChanging") = e.NewPageIndex
            If ViewState("gvAddrs_Sorting") IsNot Nothing Then
                gvSort = ViewState("gvAddrs_Sorting")
                dvAddrs.Sort = gvSort.SortExpression + " " + gvSort.SortDirection
            End If
            gvAddrs.SelectedIndex = -1
            gvAddrs.PageIndex = e.NewPageIndex
            HelperFunctions.BindGrid(gvAddrs, dvAddrs)
        Catch ex As Exception
            HelperFunctions.LogException("RollIn --> gvAddrs_PageIndexChanging", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
        End Try
    End Sub


    Private Sub gvAddrs_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvAddrs.RowDataBound
        Dim strAddrs2 As String
        Dim strAddrs3 As String
        Try
            If e.Row.RowType = DataControlRowType.DataRow Or e.Row.RowType = DataControlRowType.Header Then
                e.Row.Cells(1).Visible = False
                If e.Row.RowType = DataControlRowType.DataRow Then
                    strAddrs2 = IIf(e.Row.Cells(3).Text.Trim = "" Or e.Row.Cells(3).Text = "&nbsp;", "", " " + e.Row.Cells(3).Text)
                    strAddrs3 = IIf(e.Row.Cells(4).Text.Trim = "" Or e.Row.Cells(4).Text = "&nbsp;", "", " " + e.Row.Cells(4).Text)
                    e.Row.Cells(2).Text += " " + strAddrs2 + " " + strAddrs3
                ElseIf e.Row.RowType = DataControlRowType.Header Then
                    HelperFunctions.SetSortingArrows(ViewState("gvAddrs_Sorting"), e)
                End If
                e.Row.Cells(3).Visible = False
                e.Row.Cells(4).Visible = False
            End If
        Catch ex As Exception
            HelperFunctions.LogException("RollIn --> gvAddrs_RowDataBound", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
        End Try
    End Sub

    Private Sub gvAddrs_SelectedIndexChanged(sender As Object, e As EventArgs) Handles gvAddrs.SelectedIndexChanged
        Dim i As Integer
        Dim l_button_select As ImageButton
        Try
            lnkAdd.Visible = True
            lnkChooseAddress.Visible = True
            gvAddrs.Visible = False
            SessionRollIns.RollIn_gvAddrs_SelectedChanged = True

            While i < Me.gvAddrs.Rows.Count
                l_button_select = TryCast(Me.gvAddrs.Rows(i).Cells(0).FindControl("ImageButtonSelAddrs"), ImageButton)
                If i = Me.gvAddrs.SelectedIndex Then
                    If Not l_button_select Is Nothing Then
                        l_button_select.ImageUrl = "images\selected.gif"
                    End If
                Else
                    If Not l_button_select Is Nothing Then
                        l_button_select.ImageUrl = "images\select.gif"
                    End If

                End If
                i = i + 1
            End While
            ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "Reload", "document.forms(0).submit();", True)
        Catch ex As Exception
            HelperFunctions.LogException("RollIn --> gvAddrs_SelectedIndexChanged", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
        End Try
    End Sub

    Private Sub gvAddrs_Sorting(sender As Object, e As GridViewSortEventArgs) Handles gvAddrs.Sorting
        Try
            Dim dtAdddress As DataTable
            Dim dvAddrs As DataView
            dtAdddress = SessionRollIns.Rollin_GetAddress_dtAddress
            dvAddrs = dtAdddress.DefaultView
            HelperFunctions.gvSorting(ViewState("gvAddrs_Sorting"), e.SortExpression, dvAddrs)
            If ViewState("gvAddrs_PageIndexChanging") IsNot Nothing Then
                gvAddrs.PageIndex = ViewState("gvAddrs_PageIndexChanging")
            End If
            gvAddrs.SelectedIndex = -1
            HelperFunctions.BindGrid(gvAddrs, dvAddrs)
        Catch ex As Exception
            HelperFunctions.LogException("RollIn --> gvAddrs_Sorting", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
        End Try
    End Sub

    Private Sub lnkChooseAddress_Click()
        Try
            ScriptManager.RegisterStartupScript(Me, GetType(Page), "ReOpen", "BindEvents(); ShowRolloverDialog('AddrsFill');", True)
            gvAddrs.Visible = True
            ucAddressAdd.Visible = False
            lnkChooseAddress.Visible = False
            lnkAdd.Visible = True
            lblInstAddrs.Text = "Choose any address from below"
        Catch ex As Exception
            HelperFunctions.LogException("RollIn --> lnkChooseAddress_Click", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
        End Try
    End Sub

    Private Sub lnkAdd_Click(sender As Object, e As EventArgs) Handles lnkAdd.Click
        Dim i As Integer
        Dim l_button_select As ImageButton
        Try
            ucAddressAdd.LoadAddressDetail(Nothing)
            ucAddressAdd.ClearControls() = ""
            ucAddressAdd.EnableControls = True
            ucAddressAdd.UniqueId = ""
            ucAddressAdd.guiEntityId = ""
            ucAddressAdd.Visible = True
            lnkAdd.Visible = False
            lnkChooseAddress.Visible = True
            gvAddrs.Visible = False
            gvAddrs.SelectedIndex = -1
            lblInstAddrs.Text = "Address"
            While i < Me.gvAddrs.Rows.Count
                l_button_select = TryCast(Me.gvAddrs.Rows(i).Cells(0).FindControl("ImageButtonSelAddrs"), ImageButton)
                If i = Me.gvAddrs.SelectedIndex Then
                    If Not l_button_select Is Nothing Then
                        l_button_select.ImageUrl = "images\selected.gif"
                    End If
                Else
                    If Not l_button_select Is Nothing Then
                        l_button_select.ImageUrl = "images\select.gif"
                    End If

                End If
                i = i + 1
            End While
            ScriptManager.RegisterStartupScript(Me, GetType(Page), "ReOpenAdd", "BindEvents(); ShowRolloverDialog('AddrsFill');", True)
            ScriptManager.RegisterStartupScript(Me, GetType(Page), "OpenAdd", "OpenAddressPopUp();", True)
        Catch ex As Exception
            HelperFunctions.LogException("RollIn --> lnkAdd_Click", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
        End Try
    End Sub
End Class
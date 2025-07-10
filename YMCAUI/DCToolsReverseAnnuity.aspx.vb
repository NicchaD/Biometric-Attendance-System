Imports YMCAObjects
Imports System.IO
Public Class DCToolsReverseAnnuity
    Inherits System.Web.UI.Page

    Protected WithEvents divNotes As System.Web.UI.HtmlControls.HtmlGenericControl
    Protected WithEvents Headercontrol As YMCA_Header_WebUserControl
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
    End Sub

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        InitializeComponent()
    End Sub

#Region "Properties"
    Public Property ReverseAnnuity() As YMCAObjects.DCTools.ReverseAnnuity
        Get
            If Not ViewState("ReverseAnnuity") Is Nothing Then
                Return TryCast(ViewState("ReverseAnnuity"), YMCAObjects.DCTools.ReverseAnnuity)
            Else
                Return Nothing
            End If
        End Get
        Set(ByVal value As YMCAObjects.DCTools.ReverseAnnuity)
            ViewState("ReverseAnnuity") = value
        End Set
    End Property

    ''TO get/set the FundEventId 
    Public Property FundEventId() As String
        Get
            If Not ViewState("FundEventId") Is Nothing Then
                Return (CType(ViewState("FundEventId"), String))
            Else
                Return String.Empty
            End If
        End Get
        Set(ByVal Value As String)
            ViewState("FundEventId") = Value
        End Set
    End Property

    ''TO get/set the PersId 
    Public Property PersId() As String
        Get
            If Not ViewState("PersId") Is Nothing Then
                Return (CType(ViewState("PersId"), String))
            Else
                Return String.Empty
            End If
        End Get
        Set(ByVal Value As String)
            ViewState("PersId") = Value
        End Set
    End Property

    ''TO get/set the Fund No 
    Public Property FundNo() As String
        Get
            If Not ViewState("FundNo") Is Nothing Then
                Return (CType(ViewState("FundNo"), String))
            Else
                Return String.Empty
            End If
        End Get
        Set(ByVal Value As String)
            ViewState("FundNo") = Value
        End Set
    End Property

    ''TO get/set the First Name
    Public Property FirstName() As String
        Get
            If Not ViewState("FirstName") Is Nothing Then
                Return (CType(ViewState("FirstName"), String))
            Else
                Return String.Empty
            End If
        End Get
        Set(ByVal Value As String)
            ViewState("FirstName") = Value
        End Set
    End Property

    ''TO get/set the Last Name
    Public Property LastName() As String
        Get
            If Not ViewState("LastName") Is Nothing Then
                Return (CType(ViewState("LastName"), String))
            Else
                Return String.Empty
            End If
        End Get
        Set(ByVal Value As String)
            ViewState("LastName") = Value
        End Set
    End Property

    ''TO get/set the FundStatus
    Public Property FundStatus() As String
        Get
            If Not ViewState("FundStatus") Is Nothing Then
                Return (CType(ViewState("FundStatus"), String))
            Else
                Return String.Empty
            End If
        End Get
        Set(ByVal Value As String)
            ViewState("FundStatus") = Value
        End Set
    End Property

    ''TO get/set the AnnuityDetails
    Public Property AnnuityDetails() As DataSet
        Get
            If Not Session("AnnuityDetails") Is Nothing Then
                Return (CType(Session("AnnuityDetails"), DataSet))
            Else
                Return Nothing
            End If
        End Get
        Set(ByVal Value As DataSet)
            Session("AnnuityDetails") = Value
        End Set
    End Property

    Public Property IsReadOnlyAccess() As Boolean
        Get
            If Not Session("IsReadOnlyAccess") Is Nothing Then
                Return (CType(Session("IsReadOnlyAccess"), Boolean))
            Else
                Return False
            End If
        End Get
        Set(ByVal value As Boolean)
            Session("IsReadOnlyAccess") = value
        End Set
    End Property

    Public Property ReadOnlyWarningMessage() As String
        Get
            If Not Session("ReadOnlyWarningMessage") Is Nothing Then
                Return (CType(Session("ReadOnlyWarningMessage"), String))
            Else
                Return String.Empty
            End If
        End Get
        Set(ByVal value As String)
            Session("ReadOnlyWarningMessage") = value
        End Set
    End Property
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("Reverse Annuity page load", "START: Page Load.")
            If Session("LoggedUserKey") Is Nothing Then
                Response.Redirect("Login.aspx", False)
            End If

            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "YRS", "BindEvents();", True)
            If Not IsPostBack Then
                CheckAccessRights()
                SetModuleName()
                LoadSessionValues()
                LoadAnnuities(gvAnnuities)
                LoadFundStatus()
                InitialiseControlProperties()
            End If
            YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("Reverse Annuity page load", "END: Page Load.")
            YMCARET.CommonUtilities.WebPerformanceTracer.EndTrace()
        Catch ex As Exception
            HelperFunctions.LogException("DCToolsReverseAnnuity_Page_Load", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
        End Try
    End Sub
    ''' <summary>
    ''' This method will load annuitants all annuities (group by disbursement) which are not in paid status.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub LoadAnnuities(ByVal gv As GridView)
        Dim dsAnnuity As DataSet
        Try
            'LookUpAnnuities will return two tables. 1. Annuities (group by disbursement) 2. All Annuities to be displayed in review tab.
            dsAnnuity = YMCARET.YmcaBusinessObject.DCToolsBO.LookUpAnnuities(FundEventId)
            AnnuityDetails = dsAnnuity
            If (HelperFunctions.isNonEmpty(dsAnnuity)) Then
                gv.DataSource = dsAnnuity 'Bind Annuities (group by disbursement)
                gv.DataBind()
            Else
                gv.DataSource = Nothing 'Bind empty datasource
                gv.DataBind()
            End If
        Catch
            Throw
        End Try
    End Sub

    Private Sub gvAnnuities_SelectedIndexChanged(sender As Object, e As EventArgs) Handles gvAnnuities.SelectedIndexChanged
        Dim buttonSelect As ImageButton
        Dim i As Integer
        Dim RecommendedFundEventStatus As String
        Dim Annuity As New YMCAObjects.DCTools.ReverseAnnuity
        Try
            'Change selected row image
            While i < Me.gvAnnuities.Rows.Count
                'buttonSelect = TryCast(Me.gvAnnuities.Rows(i).Cells(0).Controls(0), ImageButton)
                buttonSelect = TryCast(Me.gvAnnuities.Rows(i).Cells(0).FindControl("imgbtn"), ImageButton)

                If i = Me.gvAnnuities.SelectedIndex Then
                    If Not buttonSelect Is Nothing Then
                        buttonSelect.ImageUrl = "images\selected.gif"
                    End If
                Else
                    If Not buttonSelect Is Nothing Then
                        buttonSelect.ImageUrl = "images\select.gif"
                    End If
                End If
                i = i + 1
            End While

            'Get selected annuity details           
            Annuity.CurrentPayment = gvAnnuities.SelectedRow.Cells(1).Text.Trim()
            Annuity.PurchaseDate = gvAnnuities.SelectedRow.Cells(2).Text.Trim()
            Annuity.PlanType = gvAnnuities.SelectedRow.Cells(3).Text.Trim()
            Annuity.DisbursementId = gvAnnuities.SelectedRow.Cells(4).Text.Trim()
            ReverseAnnuity = Annuity ' Copy selected annuity details in class/property

            'Provide user to change fundstatus with recommendation based on annuitant retiree & transaction status.
            RecommendedFundEventStatus = YMCARET.YmcaBusinessObject.DCToolsBO.RecommendedFundEventStatus(ReverseAnnuity.DisbursementId)

            If Not String.IsNullOrEmpty(RecommendedFundEventStatus) Then
                ddlFundStatus.SelectedValue = RecommendedFundEventStatus.Trim
            End If

            'Validate annuity details
            '1. Annuity is void & reversed
            '1. QDRO split has not been performed using Annuity.
            If Not isValidAnnuity(ReverseAnnuity.DisbursementId) Then
                btnNext.Visible = False
                btnPrevious.Visible = False
            Else
                btnNext.Visible = True
                btnPrevious.Visible = False
                divFundStatus.Visible = True
                divNotes.Visible = True
            End If
        Catch ex As Exception
            HelperFunctions.LogException("ReverseAnnuity_gvAnnuities_SelectedIndexChanged", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
        End Try
    End Sub

    Private Sub btnNext_Click(sender As Object, e As EventArgs) Handles btnNext.Click
        Try
            If String.IsNullOrEmpty(txtNotes.Text.Trim) Then
                HelperFunctions.ShowMessageToUser(YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.MESSAGE_DCTOOLS_REVERSEANNUITY_NOTES).DisplayText, EnumMessageTypes.Error)
                Exit Sub
            End If
            SetControlPropertiesForNextButton()
            lblNewFundStatus.Text = ddlFundStatus.SelectedValue.ToString().Trim() + " - " + ddlFundStatus.SelectedItem.ToString()
            LoadReviewAnnuities(gvReviewAnnuity)
            SetReviewNotes()
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("ReverseAnnuity_btnNext_Click", ex)
        Finally

        End Try
    End Sub

    Private Sub btnPrevious_Click(sender As Object, e As EventArgs) Handles btnPrevious.Click
        Try
            SetControlPropertiesForPreviousButton()
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("ReverseAnnuity_btnPrevious_Click", ex)
        Finally

        End Try
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Try
            ClearSessionVariables()
            Response.Redirect("MainWebForm.aspx", False)
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("ReverseAnnuity_btnClose_Click", ex)
        End Try
    End Sub
    ''' <summary>
    ''' This method will clear all sessions used in Reverse Annuity process.    
    ''' </summary>
    ''' <remarks></remarks> 
    Public Sub ClearSessionVariables()
        Session("AnnuityDetails") = Nothing
        Session("FundId") = Nothing
    End Sub

    ''' <summary>
    ''' This method will load all the fund status from master table    
    ''' </summary>
    ''' <remarks></remarks> 
    Public Sub LoadFundStatus()
        Dim dsFundStatus As DataSet
        Try
            dsFundStatus = YMCARET.YmcaBusinessObject.DisbursementReversalBOClass.GetStatusList()
            If (HelperFunctions.isNonEmpty(dsFundStatus)) Then
                Me.ddlFundStatus.ClearSelection()
                Me.ddlFundStatus.Items.Clear()
                Me.ddlFundStatus.DataSource = dsFundStatus
                Me.ddlFundStatus.DataTextField = dsFundStatus.Tables("StatusList").Columns("Description").ColumnName
                Me.ddlFundStatus.DataValueField = dsFundStatus.Tables("StatusList").Columns("FundStatus").ColumnName
                Me.ddlFundStatus.DataBind()
            End If
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("ReverseAnnuity_LoadFundStatus", ex)
        End Try
    End Sub

    ''' <summary>
    ''' This method will validate selected annuity.    
    ''' </summary>
    ''' <remarks></remarks>    
    Public Function isValidAnnuity(ByVal AnnuityId As String) As Boolean
        Dim AnnuityStatus As String
        Try
            AnnuityStatus = YMCARET.YmcaBusinessObject.DCToolsBO.ValidateAnnuity(AnnuityId)
            isValidAnnuity = True
            If AnnuityStatus = "1" Then '1 means annuity is not void & reversed
                isValidAnnuity = False
                HelperFunctions.ShowMessageToUser(YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.MESSAGE_DCTOOLS_REVERSEANNUITY_CHECK_VOID).DisplayText, EnumMessageTypes.Error)
            ElseIf AnnuityStatus = "2" Then '2 means QDRO split has been performed using Annuity
                isValidAnnuity = False
                HelperFunctions.ShowMessageToUser(YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.MESSAGE_DCTOOLS_REVERSEANNUITY_QDRO).DisplayText, EnumMessageTypes.Error)
            ElseIf AnnuityStatus = "3" Then ' 3 means experience devidend exist for retiree.need to reverse it manually as the reversal process is not capable of doing it 
                isValidAnnuity = True
                HelperFunctions.ShowMessageToUser(YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.MESSAGE_DCTOOLS_REVERSEANNUITY_EXPERIENCE_DIVIDEND).DisplayText, EnumMessageTypes.Warning)
            End If
            Return isValidAnnuity
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("ReverseAnnuity_isValidAnnuity", ex)
        End Try
    End Function

    ''' <summary>
    ''' This method will list annuities selected for reverse.    
    ''' </summary>
    ''' <remarks></remarks>   
    Public Sub LoadReviewAnnuities(ByVal gv As GridView)
        Dim dsAnnuity As DataSet
        Try
            dsAnnuity = AnnuityDetails
            If (HelperFunctions.isNonEmpty(dsAnnuity.Tables(1))) Then
                dsAnnuity.Tables(1).DefaultView.RowFilter = "DisbursementId IN ('" & ReverseAnnuity.DisbursementId & "')"
                gv.DataSource = dsAnnuity.Tables(1).DefaultView
                gv.DataBind()
            End If
        Catch
            Throw
        End Try
    End Sub

    ''' <summary>
    ''' This method will set notes for review tab.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub SetReviewNotes()
        Dim dtNotes As DataTable
        Dim defaultNotes As String
        Try
            ' Get default notes
            dtNotes = YMCARET.YmcaBusinessObject.RefundRequest.SearchConfigurationMaintenance("DCTOOLS_REVERSE_ANNUITY_SYSTEM_NOTE")
            If (HelperFunctions.isNonEmpty(dtNotes)) Then
                defaultNotes = String.Format(Convert.ToString(dtNotes.Rows(0)("Value")), ReverseAnnuity.PurchaseDate, ReverseAnnuity.CurrentPayment)
            End If
            ' Append default notes with user notes.
            lblReviewNotes.Text = defaultNotes.Trim() + " " + txtNotes.Text.Trim()
        Catch
            Throw
        End Try
    End Sub

    Private Sub btnConfirmDialogYes_Click(sender As Object, e As EventArgs) Handles btnConfirmDialogYes.Click
        Dim activityLogEntry As YMCAObjects.YMCAActionEntry
        Dim output As String
        Dim participantDetails As Dictionary(Of String, String)
        Try
            If Me.IsReadOnlyAccess Then
                HelperFunctions.ShowMessageToUser(Me.ReadOnlyWarningMessage, EnumMessageTypes.Error)
                Exit Sub
            Else
                activityLogEntry = New YMCAObjects.YMCAActionEntry
                activityLogEntry = CreateDataForActivityLog(ReverseAnnuity)
                output = YMCARET.YmcaBusinessObject.DCToolsBO.ReverseAnnuity(ReverseAnnuity.DisbursementId, ddlFundStatus.SelectedValue.ToString(), lblReviewNotes.Text, PersId, activityLogEntry)
                If output = "" Then
                    participantDetails = New Dictionary(Of String, String)
                    participantDetails.Add("FundNo", FundNo)
                    participantDetails.Add("LastName", LastName)
                    participantDetails.Add("FirstName", FirstName)
                    HelperFunctions.ShowMessageOnNextPage(YMCAObjects.MetaMessageList.MESSAGE_DCTOOLS_REVERSEANNUITY_SUCCESS, Nothing, participantDetails)
                    Response.Redirect("FindInfo.aspx?Name=DCToolsReverseAnnuity", True)
                    btnSave.Visible = False
                    btnPrevious.Visible = False
                Else
                    HelperFunctions.ShowMessageToUser(output.Trim(), EnumMessageTypes.Error)
                End If
            End If
        Catch ex As Exception
            HelperFunctions.LogException("btnConfirmDialogYes_Click", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
        Finally
            activityLogEntry = Nothing
        End Try
    End Sub

    ''' <summary>
    ''' This method will create activity data for logging purpose.
    ''' </summary>
    ''' <remarks></remarks>
    Private Function CreateDataForActivityLog(ByVal ReverseAnnuity As YMCAObjects.DCTools.ReverseAnnuity) As YMCAObjects.YMCAActionEntry
        Dim reverseAnnuityDetails As String = ""
        Dim action As New YMCAObjects.YMCAActionEntry
        If Not ReverseAnnuity Is Nothing Then
            reverseAnnuityDetails = String.Format("<DC><DisbursementId>{0}</DisbursementId><AnnuityAmount>${1}</AnnuityAmount><PlanType>{2}</PlanType><PurchaseDate>{3}</PurchaseDate></DC>", ReverseAnnuity.DisbursementId, ReverseAnnuity.CurrentPayment, ReverseAnnuity.PlanType, ReverseAnnuity.PurchaseDate)
            action.Action = YMCAObjects.ActionYRSActivityLog.REVERSE_ANNUITY
            action.ActionBy = Session("LoginId")
            action.Data = reverseAnnuityDetails
            action.EntityId = PersId
            action.EntityType = YMCAObjects.EntityTypes.PERSON
            action.Module = YMCAObjects.Module.DCTools
            action.SuccessStatus = True
        End If
        Return action
    End Function
    ''' <summary>
    ''' This method will load session values set in Findinfo.aspx page.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub LoadSessionValues()
        Try
            If Not Session("FundNo") Is Nothing Then
                FundNo = Session("FundNo")
            End If
            If Not Session("LastName") Is Nothing Then
                LastName = Session("LastName")
            End If
            If Not Session("FirstName") Is Nothing Then
                FirstName = Session("FirstName")
            End If
            If Not Session("PersId") Is Nothing Then
                PersId = Session("PersId")
            End If
            If Not Session("FundEventID") Is Nothing Then
                FundEventId = Session("FundEventID")
            End If
            If Not Session("FundStatus") Is Nothing Then
                FundStatus = Session("FundStatus")
            End If
        Catch
            Throw
        End Try
    End Sub
    ''' <summary>
    ''' This method will initialise control properties on first time page load.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub InitialiseControlProperties()
        btnSave.Attributes.Add("onclick", "javascript:return ShowDialog();")
        TAB1.Visible = True 'Reverse Annuity Tab
        TAB2.Visible = False 'Review & Submit Tab
        'On First time nevigation do not allow user to go for next tab(Review & Submit Tab) unless user select an annuity.
        ' Hence set controls properties to false
        btnNext.Visible = False
        btnSave.Visible = False
        divFundStatus.Visible = False
        divNotes.Visible = False
        btnPrevious.Visible = False
    End Sub
    ''' <summary>
    ''' This method will Set Control Properties For Next Button.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub SetControlPropertiesForNextButton()
        btnPrevious.Visible = True
        btnSave.Visible = True
        btnNext.Visible = False
        TAB1.Visible = False
        TAB2.Visible = True
        divRequiredFields.Visible = False
        tdReverseAnnuity.Attributes.Add("class", "InActiveDisabledTab")
        tdReviewAnnuity.Attributes.Add("class", "ActiveTab")
    End Sub

    ''' <summary>
    ''' This method will Set Control Properties For Previous Button.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub SetControlPropertiesForPreviousButton()
        TAB1.Visible = True
        TAB2.Visible = False
        divRequiredFields.Visible = True
        tdReverseAnnuity.Attributes.Add("class", "ActiveTab")
        tdReviewAnnuity.Attributes.Add("class", "InActiveDisabledTab")
        LoadAnnuities(gvAnnuities)
        btnPrevious.Visible = False
        btnSave.Visible = False
        btnNext.Visible = True
    End Sub


    Private Sub SetModuleName()
        Dim lblModuleName As Label
        Try
            lblModuleName = Master.FindControl("LabelModuleName")
            If lblModuleName IsNot Nothing Then
                lblModuleName.Text = "Administration > DC Tools > Reverse Annuity" + " - " + Session("FundNo").ToString().Trim() + " - " + Session("LastName").ToString().Trim() + ", " + Session("FirstName")
            End If
        Catch
            Throw
        Finally
            lblModuleName = Nothing
        End Try
    End Sub


    ''' <summary>
    ''' THis method will check read-only access rights
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub CheckAccessRights()
        Dim readOnlyWarningMessage As String
        Try
            readOnlyWarningMessage = SecurityCheck.Check_Authorization("FindInfo.aspx?Name=DCToolsReverseAnnuity", Convert.ToInt32(Session("LoggedUserKey")))
            If Not String.IsNullOrEmpty(readOnlyWarningMessage) AndAlso Not readOnlyWarningMessage.ToUpper().Contains("TRUE") Then
                Me.IsReadOnlyAccess = True
                Me.ReadOnlyWarningMessage = readOnlyWarningMessage
            End If
        Catch
            Throw
        Finally
            readOnlyWarningMessage = Nothing
        End Try
    End Sub
End Class
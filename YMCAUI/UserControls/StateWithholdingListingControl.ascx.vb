'**************************************************************************************************************/
' Copyright YMCA Retirement Fund All Rights Reserved. 
' Project Name		:	YMCA-YRS
' FileName			:   StateWithholdingListingControl.ascx.vb
' Author Name		:	Megha Lad
' Employee ID		:	
' Email	    	    :	
' Contact No		:	
' Creation Time 	:	01/10/2019
' Description	    :	UserControl for State Withholding Data. User can View,Edit and Insert State withholding details.
' Declared in Version : 20.7.0 | YRS-AT-4598 - YRS enh: State Withholding Project - Annuity Payroll Processing
'**************************************************************************************************************
' MODIFICATION HISTORY:
' ------------------------------------------------------------------------------------------------------
' Developer Name              | Date         | Version No      | Ticket
' ------------------------------------------------------------------------------------------------------

' ------------------------------------------------------------------------------------------------------
'**************************************************************************************************************/
Imports YMCAObjects
Public Class StateWithholdingListingControl
    Inherits System.Web.UI.UserControl

    'PreFixID is used to  check User permission for Add,Edit and Save. To apply user permisson save buttonName as PreFixID +button name
    'Page : RetireesInformationWebForm.aspx  PreFixID = "retireeInfo"
    'Page : RetirementProcessingForm.aspx  PreFixID = "annuityProcess"
    Public Property PreFixID As String
        Get
            Return ViewState("preFix")
        End Get
        Set(ByVal value As String)
            ViewState("preFix") = value
        End Set
    End Property
    Public Property PersonID As String
        Get
            Return ViewState("STWPerssID")
        End Get
        Set(ByVal value As String)
            ViewState("STWPerssID") = value
        End Set
    End Property
    ' This property hold persons Gross Amount (Monthly AnnuityAmt - Total Withholding)
    ' Total Withholding means Federal Amount + General Withholding Amount
    Public Property GrossAmount As Double?
        Get
            Return ViewState("STWGrossAmount")
        End Get
        Set(ByVal value As Double?)
            ViewState("STWGrossAmount") = value
        End Set
    End Property
    ' This propety hold persons Federal Amount 
    Public Property FederalAmount As Double?
        Get
            Return ViewState("STWFederalAmount")
        End Get
        Set(ByVal value As Double?)
            ViewState("STWFederalAmount") = value
        End Set
    End Property    
    ' This propety hold persons Federal Type 
    Public Property FederalType As String
        Get
            Return ViewState("STWFederalType")
        End Get
        Set(ByVal value As String)
            ViewState("STWFederalType") = value
        End Set
    End Property
    ' This propety hold Bool Value. STW data should save direct in Database or not.
    Public Property STWDataSaveAtMainPage As Boolean
        Get
            Return ViewState("STWSaveAtMainPage")
        End Get
        Set(ByVal value As Boolean)
            ViewState("STWSaveAtMainPage") = value
        End Set
    End Property
    ' This propety hold Person State Value. 
    Public Property PersonStateCode As String
        Get
            Return ViewState("STWPersonStateCode")
        End Get
        Set(ByVal value As String)
            ViewState("STWPersonStateCode") = value
        End Set
    End Property

    Public Property PersonStateName As String
        Get
            Return ViewState("STWPersonStateName")
        End Get
        Set(ByVal value As String)
            ViewState("STWPersonStateName") = value
        End Set
    End Property
    ' This propety hold Person Name. 
    Public Property PersonName As String
        Get
            Return ViewState("STWPersonName")
        End Get
        Set(ByVal value As String)
            ViewState("STWPersonName") = value
        End Set
    End Property
    ' This propety hold Person Name. 
    Public Property PersonFundNo As String
        Get
            Return ViewState("STWPersonFundNo")
        End Get
        Set(ByVal value As String)
            ViewState("STWPersonFundNo") = value
        End Set
    End Property
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            Me.Refresh()
            Dim imgbtn As ImageButton = DataGridStateWithholding.FindControl("btnStateWithholdEdit")
        End If

    End Sub

    Public Sub Refresh()
        Dim LstSWHPerssDetail As List(Of YMCAObjects.StateWithholdingDetails)
        Try
            LstSWHPerssDetail = SessionManager.SessionStateWithholding.LstSWHPerssDetail
            If LstSWHPerssDetail Is Nothing Then
                LstSWHPerssDetail = YMCARET.YmcaBusinessObject.StateWithholdingBO.GetStateTaxDetails(Me.PersonID)
                SessionManager.SessionStateWithholding.LstSWHPerssDetail = LstSWHPerssDetail
            End If
            If (HelperFunctions.isNonEmpty(LstSWHPerssDetail)) Then
                If (LstSWHPerssDetail.Count > 0) Then
                    DataGridStateWithholding.DataSource = LstSWHPerssDetail
                    DataGridStateWithholding.DataBind()
                    DataGridStateWithholding.ShowHeader = True
                    With LstSWHPerssDetail.FirstOrDefault
                        If (Not IsDBNull(.chvStateCode)) Then
                            lblstateName.Text = .chvStateCode
                            trStateName.Visible = True
                        Else
                            trStateName.Visible = False
                        End If

                        If (Not .numAdditionalAmount Is Nothing) Then
                            lblAdditionalAmt.Text = .numAdditionalAmount
                            trAdditionalAmt.Visible = True
                        Else
                            trAdditionalAmt.Visible = False
                        End If
                        If (Not .numFlatAmount Is Nothing) Then
                            lblFlatamt.Text = .numFlatAmount
                            trFlatamt.Visible = True
                        Else
                            trFlatamt.Visible = False
                        End If

                        If (Not .numNewYorkCityAmount Is Nothing) Then
                            lblNewYourkCity.Text = .numNewYorkCityAmount
                            trNewYourkCity.Visible = True
                        Else
                            trNewYourkCity.Visible = False
                        End If

                        If (Not .numYonkersAmount Is Nothing) Then
                            lblYonkers.Text = .numYonkersAmount
                            trYonkers.Visible = True
                        Else
                            trYonkers.Visible = False
                        End If

                        If (Not .intNoOfExemption Is Nothing) Then
                            lblExemptions.Text = .intNoOfExemption
                            trExemptions.Visible = True
                        Else
                            trExemptions.Visible = False
                        End If

                        If (Not String.IsNullOrEmpty(.chvMaritalStatusCode)) Then
                            lblMaritalStatus.Text = .chvMaritalStatusCode
                            trMaritalStatus.Visible = True
                        Else
                            trMaritalStatus.Visible = False
                        End If

                        If (Not IsDBNull(.bitStateTaxNotElected)) Then
                            Dim isStateTaxNotElected As Boolean = .bitStateTaxNotElected
                            If (isStateTaxNotElected = True) Then
                                lblElected.Text = "Yes"
                            Else
                                lblElected.Text = "No"
                            End If

                        End If

                        If (Not IsDBNull(.chvDisbursementType)) Then
                            If (.chvDisbursementType = "ANN") Then
                                lblDisbursement.Text = "Annuity"
                            Else
                                lblDisbursement.Text = "Lumpsum"
                            End If
                            trDisbursement.Visible = True
                        End If

                        If (Not .numPercentageofFederalWithholding Is Nothing) Then
                            Dim numPerofFederalWithholding As String = .numPercentageofFederalWithholding.ToString
                            If (Not String.IsNullOrEmpty(numPerofFederalWithholding)) Then
                                lblFederalWithhold.Text = "Yes"
                                lblpercentage.Text = numPerofFederalWithholding + "% of Federal Withholding"
                            Else
                                lblFederalWithhold.Text = "No"
                            End If

                            trFederalWithhold.Visible = True
                        Else
                            trFederalWithhold.Visible = False
                        End If
                    End With
                End If
            End If

        Catch ex As Exception
            HelperFunctions.LogException("Refresh", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
        End Try
    End Sub

    Protected Function bitStateTaxNotElected(ByVal strbit As Boolean) As String
        If strbit = True Then
            bitStateTaxNotElected = "Yes"
        Else
            bitStateTaxNotElected = "No"
        End If
    End Function

    Private Sub btnStateWithholdAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnStateWithholdAdd.Click

        Dim AddbuttonID As String = String.Empty
        Dim PlaceHolder As PlaceHolder = Nothing
        Try
            If (String.IsNullOrEmpty(Me.PersonID)) Then
                ' ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ErrorMessage", "ErrMessageOpen();", True)
                PlaceHolder = Me.Parent.FindControl("PlaceHolder1")
                If (Not PlaceHolder Is Nothing) Then
                    MessageBox.Show(PlaceHolder, "State Withholding Error", "Person is not associate with State Withholding Control. ", MessageBoxButtons.Stop)
                End If
                Exit Sub
            Else
                Session("STWPersID") = Me.PersonID
            End If

            AddbuttonID = Me.PreFixID + "btnStateWithholdAdd"
            Session("PreFix") = Me.PreFixID
            Session("STWGrossAmount") = Me.GrossAmount
            Session("STWFederalAmount") = Me.FederalAmount
            Session("STWFederalType") = Me.FederalType

           
            If (Not Me.STWDataSaveAtMainPage = Nothing) Then
                Session("STWSaveAtMainPage") = Me.STWDataSaveAtMainPage
            Else
                Session("STWSaveAtMainPage") = False
            End If

            If (Not String.IsNullOrEmpty(Me.PersonStateCode)) Then
                Session("STWPersonStateCode") = Me.PersonStateCode
            End If

            If (Not String.IsNullOrEmpty(Me.PersonStateName)) Then
                Session("STWPersonStateName") = Me.PersonStateName
            End If

            If (Not String.IsNullOrEmpty(Me.PersonName)) Then
                Session("STWPersonName") = Me.PersonName
            End If

            If (Not String.IsNullOrEmpty(Me.PersonFundNo)) Then
                Session("STWPersonFundNo") = Me.PersonFundNo
            End If
            Dim checkSecurity As String = SecurityCheck.Check_Authorization(AddbuttonID, Convert.ToInt32(Session("LoggedUserKey")))

            If Not checkSecurity.Equals("True") Then
                PlaceHolder = Me.Parent.FindControl("PlaceHolder1")
                If (Not PlaceHolder Is Nothing) Then
                    MessageBox.Show(PlaceHolder, "YMCA-YRS", checkSecurity, MessageBoxButtons.Stop)
                End If
                Exit Sub
            End If

            Dim popupScript As String = "<script language='javascript'>" & _
                         "window.open('AddStatewithholdingtax.aspx?type=Add', 'CustomPopUp', " & _
                         "'width=650, height=680, menubar=no, Resizable=NO,top=70,left=120, scrollbars=auto')" & _
                         "</script>"


            Page.RegisterStartupScript("PopupScript2", popupScript)
        Catch ex As Exception
            HelperFunctions.LogException("btnStateWithholdAdd_Click", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)

        Finally
            AddbuttonID = Nothing
            PlaceHolder = Nothing
        End Try
    End Sub

    Private Sub EditStateWithHolding()
        Dim popupScript As String
        Dim EditbuttonID As String = String.Empty
        Dim PlaceHolder As PlaceHolder = Nothing

        Try
            EditbuttonID = Me.PreFixID + "btnStateWithholdEdit"
            Session("PreFix") = Me.PreFixID
            Session("STWPersID") = Me.PersonID
            Session("STWGrossAmount") = Me.GrossAmount
            Session("STWFederalAmount") = Me.FederalAmount
            Session("STWFederalType") = Me.FederalType

            If (Not Me.STWDataSaveAtMainPage = Nothing) Then
                Session("STWSaveAtMainPage") = Me.STWDataSaveAtMainPage
            Else
                Session("STWSaveAtMainPage") = False
            End If


            If (Not String.IsNullOrEmpty(Me.PersonStateCode)) Then
                Session("STWPersonStateCode") = Me.PersonStateCode
            End If

            If (Not String.IsNullOrEmpty(Me.PersonStateName)) Then
                Session("STWPersonStateName") = Me.PersonStateName
            End If

            If (Not String.IsNullOrEmpty(Me.PersonName)) Then
                Session("STWPersonName") = Me.PersonName
            End If

            If (Not String.IsNullOrEmpty(Me.PersonName)) Then
                Session("STWPersonName") = Me.PersonName
            End If

            If (Not String.IsNullOrEmpty(Me.PersonFundNo)) Then
                Session("STWPersonFundNo") = Me.PersonFundNo
            End If
            Dim checkSecurity As String = SecurityCheck.Check_Authorization(EditbuttonID, Convert.ToInt32(Session("LoggedUserKey")))

            If Not checkSecurity.Equals("True") Then
                PlaceHolder = Me.Parent.FindControl("PlaceHolder1")
                If (Not PlaceHolder Is Nothing) Then
                    MessageBox.Show(PlaceHolder, "YMCA-YRS", checkSecurity, MessageBoxButtons.Stop)
                End If
                Exit Sub
            End If

            popupScript = "<script language='javascript'>" & _
                        "window.open('AddStatewithholdingtax.aspx?type=Edit', 'CustomPopUp', " & _
                        "'width=650, height=680, menubar=no, Resizable=NO,top=70,left=120, scrollbars=auto')" & _
                        "</script>"
            Page.RegisterStartupScript("PopupScript2", popupScript)

        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
        End Try
    End Sub

   
    Sub image_OnClick(ByVal Source As Object, ByVal e As ImageClickEventArgs)

        Dim strimg As String
        Dim img As ImageButton
        img = CType(Source, ImageButton)

        strimg = img.ClientID
        If Not IsNothing(img) Then
            EditStateWithHolding()
        End If
    End Sub
End Class
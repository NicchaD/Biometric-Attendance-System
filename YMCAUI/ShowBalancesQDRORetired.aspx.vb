'*******************************************************************************
' Copyright YMCA Retirement Fund All Rights Reserved. 
'
' Project Name		:	YMCA-YRS
' FileName			:	ShowBalancesQDRORetired.aspx.vb
' Author Name		:	Amit Nigam	
' Employee ID		:	36413
' Email			    :	amit.nigam@3i-infotech.com
' Contact No		:	080-39876761
' Creation Time	    :	08/07/2008 
' Program Specification Name	:	
' Unit Test Plan Name			:	
' Description					:	<<Please put the brief description here...>>

'
' Changed by			:	
' Changed on			:	
' Change Description	:	
'*******************************************************************************
'Modified By                    Date                Description
'*******************************************************************************
'Neeraj Singh                   12/Nov/2009         Added form name for security issue YRS 5.0-940 
'Manthan Rajguru                2015.09.16          YRS-AT-2550: YRS data cleanup: copyright comments and namespace/reference (Changed the Namespace/reference from 'Infotech' to 'YMCARET')
'Chandra sekar                  2016.08.22          YRS-AT-3081 - YRS Enh: request to QDRO function (Retired ) to allow different amt or percent (TrackIT 23098)
'*******************************************************************************

Imports System.IO 'Chandra sekar| 2016.08.22 | YRS-AT-3081 For using the HTML String Builder Class
Public Class ShowBalancesQDRORetired
    Inherits System.Web.UI.Page
    'below line is Added by Neeraj for issue id YRS 5.0-940 
    Dim strFormName As String = New String("ShowBalancesQDRORetired.aspx")
    'End issue id YRS 5.0-940

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    'Protected WithEvents DataListParticipant As System.Web.UI.WebControls.DataList
    Protected WithEvents DatalistBeneficiary As System.Web.UI.WebControls.DataList
    Protected WithEvents ButtonOK As System.Web.UI.WebControls.Button


    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

#Region "Properties"
    '***************************************************************************************************//
    'Class Name                :ShowBalancesQDRORetired               Used In     : YMCAUI           //
    'Created By                :Amit Nigam            Modified On : 08/07/2008                          //
    'Modified By               :                                                                        //
    'Modify Reason             :                                                                        //
    'Constructor Description   :                                                                        //
    'Class Description         :Used to add the properties for sessions.                                //
    '***************************************************************************************************//
    Private Property Session_datatable_dtPartAccount() As DataTable
        Get
            If Not (Session("dtPartAccount")) Is Nothing Then

                Return (CType(Session("dtPartAccount"), DataTable))
            Else
                Return Nothing
            End If
        End Get

        Set(ByVal Value As DataTable)
            Session("dtPartAccount") = Value
        End Set
    End Property

    Private Property Session_datatable_dtRecptAccount() As DataTable
        Get
            If Not (Session("dtRecptAccount")) Is Nothing Then

                Return (CType(Session("dtRecptAccount"), DataTable))
            Else
                Return Nothing
            End If
        End Get

        Set(ByVal Value As DataTable)
            Session("dtRecptAccount") = Value
        End Set
    End Property

    Private Property Session_datatable_dtBeneficiarySession() As DataTable
        Get
            If Not (Session("dtBeneficiarySession")) Is Nothing Then

                Return (CType(Session("dtBeneficiarySession"), DataTable))
            Else
                Return Nothing
            End If
        End Get

        Set(ByVal Value As DataTable)
            Session("dtBeneficiarySession") = Value
        End Set
    End Property

    Private Property string_PersSSID() As String
        Get
            If Not Session("PersSSID") Is Nothing Then
                Return Session("PersSSID")
            Else
                Return Nothing
            End If

        End Get
        Set(ByVal Value As String)
            Session("PersSSID") = Value
        End Set
    End Property

    Private Property Session_datatable_dtBenifAccount() As DataTable
        Get
            If Not (Session("dtBenifAccount")) Is Nothing Then

                Return (CType(Session("dtBenifAccount"), DataTable))
            Else
                Return Nothing
            End If
        End Get

        Set(ByVal Value As DataTable)
            Session("dtBenifAccount") = Value
        End Set
    End Property
#End Region

#Region "Variable Declarations"
    '***************************************************************************************************//
    'Class Name                :ShowBalancesQDRORetired               Used In     : YMCAUI           //
    'Created By                :Amit Nigam            Modified On : 08/07/2008                          //
    'Modified By               :                                                                        //
    'Modify Reason             :                                                                        //
    'Constructor Description   :                                                                        //
    'Class Description         :Used to declare the variables used in the class.                        //
    '***************************************************************************************************//

    Dim dtFind As New DataTable
    Protected WithEvents DatagridBenificiaryList As System.Web.UI.WebControls.DataGrid
    Dim dsBal As New DataSet
    Dim dsBalBeneficiary As New DataSet
    Protected WithEvents DatagridBeneficiarySummaryBalList As System.Web.UI.WebControls.DataGrid
    Protected WithEvents DataListParticipant As System.Web.UI.WebControls.DataList
    Dim l_dataset_ParticipantDetail As New DataSet
    Dim dtFindBeneficiary As New DataTable
    Dim dgBeneficiary As New DataGrid
    Dim dgParticipant As New DataGrid
    Dim dtBeneficiarySession As New DataTable
    Dim dtfindset As New DataSet
#End Region

#Region "Page Load"

    '***************************************************************************************************//
    'Class Name                :ShowBalancesQDRORetired               Used In     : YMCAUI           //
    'Created By                :Amit Nigam            Modified On : 08/07/2008                          //
    'Modified By               :                                                                        //
    'Modify Reason             :                                                                        //
    'Constructor Description   :                                                                        //
    'Class Description         :This class is being used to adding attribute to control.                //
    '***************************************************************************************************//
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        Try
            dgBeneficiary.DataSource = Nothing
            dgBeneficiary.DataBind()
            dgParticipant.DataSource = Nothing
            dgParticipant.DataBind()
            Session.Remove("dtBeneficiarySession")
            LoadSummaryBal()
            LoadParticipant()
            LoadBeneficiary()
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
        End Try
    End Sub
#End Region

#Region "LOAD DETAILS"
    '***************************************************************************************************//
    'Class Name                :ShowBalancesQDRORetired               Used In     : YMCAUI           //
    'Created By                :Amit Nigam            Modified On : 08/07/2008                          //
    'Modified By               :                                                                        //
    'Modify Reason             :                                                                        //
    'Constructor Description   :                                                                        //
    'Class Description         :This class is being used to Load Summary Bal                            //
    '***************************************************************************************************//

    Private Sub LoadSummaryBal()
        'participant Bal
        If Not Me.Session_datatable_dtRecptAccount Is Nothing Then
            dtBeneficiarySession = Me.Session_datatable_dtRecptAccount
        End If
        Me.Session_datatable_dtBeneficiarySession = dtBeneficiarySession
        If Not Me.Session_datatable_dtBeneficiarySession Is Nothing Then
            dtFindBeneficiary = Me.Session_datatable_dtBeneficiarySession
        End If
    End Sub

    '***************************************************************************************************//
    'Class Name                :ShowBalancesQDRONonRetired               Used In     : YMCAUI           //
    'Created By                :Amit Nigam            Modified On : 08/07/2008                          //
    'Modified By               :                                                                        //
    'Modify Reason             :                                                                        //
    'Constructor Description   :                                                                        //
    'Class Description         :This class is being used to Load Participant Details                    //
    '***************************************************************************************************//

    Private Sub LoadParticipant()
        Try
            l_dataset_ParticipantDetail = YMCARET.YmcaBusinessObject.NonRetiredQDROBOClass.GetParticipantDetail(Me.string_PersSSID)
            dtfindset = l_dataset_ParticipantDetail
            DataListParticipant.DataSource = dtfindset
            DataListParticipant.DataBind()
        Catch
            Throw
        End Try

    End Sub

    '***************************************************************************************************//
    'Class Name                :ShowBalancesQDRONonRetired               Used In     : YMCAUI           //
    'Created By                :Amit Nigam            Modified On : 08/07/2008                          //
    'Modified By               :                                                                        //
    'Modify Reason             :                                                                        //
    'Constructor Description   :                                                                        //
    'Class Description         :This class is being used to Load Beneficiary Details                    //
    '***************************************************************************************************//

    Private Sub LoadBeneficiary()

        Try
            If Not Me.Session_datatable_dtBenifAccount Is Nothing Then
                Dim dtCol As DataColumn
                Dim dttemp As New DataTable
                dtFind = Me.Session_datatable_dtBenifAccount
                dttemp = dtFind.Clone
                For Each beneficiaryDetails As DataRow In dtFind.Rows
                    For Each beneficiaryAccountRow As DataRow In dtFindBeneficiary.Rows
                        If beneficiaryAccountRow.Item(0) = beneficiaryDetails.Item(0) Then
                            dttemp.Rows.Add(New Object() {beneficiaryDetails(0), beneficiaryDetails(1), beneficiaryDetails(2), beneficiaryDetails(3), beneficiaryDetails(4), beneficiaryDetails(5)})
                            Exit For
                        End If
                    Next
                Next
                DatalistBeneficiary.DataSource = dttemp
                DatalistBeneficiary.DataBind()
            End If

        Catch
            Throw
        End Try
    End Sub
#End Region

#Region "PRIVATE  EEVENTS"

    '***************************************************************************************************//
    'Class Name                :ShowBalancesQDRORetired               Used In     : YMCAUI           //
    'Created By                :Amit Nigam            Modified On : 08/07/2008                          //
    'Modified By               :                                                                        //
    'Modify Reason             :                                                                        //
    'Constructor Description   :                                                                        //
    'Class Description         :This class is being used to create the datagrid in the datalist         //
    '                           to show heirarchy                                                        //
    '***************************************************************************************************//

    Private Sub DatalistBeneficiary_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataListItemEventArgs) Handles DatalistBeneficiary.ItemDataBound
        Try


            If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
                Dim dtTemp As New DataTable
                dtTemp = dtFindBeneficiary.Clone()
                Dim dtTempRow As DataRow()
                Dim lblRecipientAnnuitiesDetails As Label 'Chandra sekar| 2016.08.22 | YRS-AT-3081 - To displaying the Html table
                Dim dtRecipientAnnuitiesBalance As DataTable
                dtTempRow = dtFindBeneficiary.Select("RecipientPersonID='" & DatalistBeneficiary.DataKeys(e.Item.ItemIndex) & "'")
                If dtTempRow.Length > 0 Then
                    For Each dtRow1 As DataRow In dtTempRow
                        dtTemp.Rows.Add(New Object() {dtRow1(0), dtRow1(1), dtRow1(2), dtRow1(3), dtRow1(4), dtRow1(5), _
                                            dtRow1(6), dtRow1(7), dtRow1(8), dtRow1(9), dtRow1(10), dtRow1(11), dtRow1(12), dtRow1(13), dtRow1(14)})
                    Next
                    'START - Chandra sekar-2016.08.22-YRS-AT-3081 - YRS Enh: request to QDRO function (Retired ) to allow different amt or percent (TrackIT 23098)
                    'To Displaying the Recipient Annuities Grid
                    lblRecipientAnnuitiesDetails = DirectCast(e.Item.FindControl("lblBeneficiarySummaryBalList"), Label)
                    dtRecipientAnnuitiesBalance = GetRecipientAnnutiesDetailsForHtmlTable(Me.Session_datatable_dtRecptAccount, DatalistBeneficiary.DataKeys(e.Item.ItemIndex))
                    lblRecipientAnnuitiesDetails.Text = HelperFunctions.GenerateHTMLTableForRecipientAnnuities(dtRecipientAnnuitiesBalance)
                    'END - Chandra sekar-2016.08.22-YRS-AT-3081 - YRS Enh: request to QDRO function (Retired ) to allow different amt or percent (TrackIT 23098)

                    'Commented by START - Chandra sekar | 2016.08.22-YRS-AT-3081 - For Grouping the Split type based on the Beneficiary 
                    '  dgBeneficiary = DirectCast(e.Item.FindControl("LabelBeneficiarySummaryBalList"), Label)
                    ' dgBeneficiary.DataSource = dtTemp
                    ' dgBeneficiary.DataBind()
                    'Commented by END - Chandra sekar-2016.08.22-YRS-AT-3081 -  For Grouping the Split type based on the Beneficiary 
                End If

            End If
        Catch
            Throw
        End Try
    End Sub

    'Filter the Recipient Annuities based the Beneficiary
    Private Function GetRecipientAnnutiesDetailsForHtmlTable(ByVal dtParticipantAnnuitiesBalance As DataTable, ByVal RecipientPersonID As String) As DataTable

        Dim dtParticipantAnnuitiesBalanceByPerson As DataTable

        Try
            If Not dtParticipantAnnuitiesBalance Is Nothing Then
                If HelperFunctions.isNonEmpty(dtParticipantAnnuitiesBalance) Then
                    dtParticipantAnnuitiesBalanceByPerson = dtParticipantAnnuitiesBalance.Clone()
                    Dim drRecipientAccount As DataRow()
                    drRecipientAccount = dtParticipantAnnuitiesBalance.Select("RecipientPersonID='" & RecipientPersonID & "'")
                    For dtRowCount = 0 To drRecipientAccount.Length - 1
                        dtParticipantAnnuitiesBalanceByPerson.ImportRow(drRecipientAccount(dtRowCount))
                    Next
                End If
            End If

        Catch
            Throw
        End Try

        Return dtParticipantAnnuitiesBalanceByPerson

    End Function
    'END - Chandra sekar-2016.08.22-YRS-AT-3081 - YRS Enh: request to QDRO function (Retired ) to allow different amt or percent (TrackIT 23098)
    '***************************************************************************************************//
    'Class Name                :ShowBalancesQDRONonRetired               Used In     : YMCAUI           //
    'Created By                :Amit Nigam            Modified On : 08/07/2008                          //
    'Modified By               :                                                                        //
    'Modify Reason             :                                                                        //
    'Constructor Description   :                                                                        //
    'Class Description         :This class is being used to create the datagrid in the datalist         //
    '                           to show heirarchy                                                        //
    '***************************************************************************************************//

    Private Sub DataListParticipant_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataListItemEventArgs) Handles DataListParticipant.ItemDataBound
        Try


            If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
                Dim dtTemp As New DataTable
                dgParticipant = DirectCast(e.Item.FindControl("DatagridSummaryBalList"), DataGrid)
                dtTemp = Me.Session_datatable_dtPartAccount
                dgParticipant.DataSource = dtTemp
                dgParticipant.DataBind()
            End If
        Catch
            Throw
        End Try
    End Sub

    '***************************************************************************************************//
    'Class Name                :ShowBalancesQDRONonRetired               Used In     : YMCAUI           //
    'Created By                :Amit Nigam            Modified On : 08/07/2008                          //
    'Modified By               :                                                                        //
    'Modify Reason             :                                                                        //
    'Constructor Description   :                                                                        //
    'Class Description         :Used to Close the Window                                                 //
    '***************************************************************************************************//

    Private Sub ButtonOK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonOK.Click
        Try
            Dim closeWindow As String = "<script language='javascript'>" & _
                                                   "window.close()" & _
                                                   "</script>"
            Page.RegisterStartupScript("CloseWindow", closeWindow)
        Catch ex As Exception

            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message)

        End Try

    End Sub

#End Region
End Class

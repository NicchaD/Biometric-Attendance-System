'*******************************************************************************
' Copyright YMCA Retirement Fund All Rights Reserved. 
'
' Project Name		:	YMCA-YRS
' FileName			:	RVoidUserControl.ascx.vb
' Author Name		:	
' Employee ID		:	
' Email				:	
' Contact No		:	
' Creation Time		:	
' Program Specification Name	:	YMCA PS 3.7.doc
' Unit Test Plan Name			:	
' Description					:	<<Please put the brief description here...>>
' Hafiz 03Feb06 Cache-Session
'********************************************************************************************************************************
'Modified By        Date            Description
'********************************************************************************************************************************
'Priya              04-11.09        Added exir for to show address details, if there are more than one disbursement in request.
'Imran              17/11/2009      ClearDisbursementControl method move to out from loop.    
'Imran              24/11/2009      Add delegate method for getting fund status for selected request id
'Imran              16/12/2009      For Gemini Issue  968 -When disbursement is cash check then checkbox disable - code commented on 12/18/2009
'Priya				18-Feb-2011		If request is MRD then Block void reissue
'Manthan Rajguru    2015.09.16      YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
'********************************************************************************************************************************
Public Class VoidUserControl
    Inherits System.Web.UI.UserControl

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents LabelPayeeSSN As System.Web.UI.WebControls.Label
    Protected WithEvents LabelNoRecordFound As System.Web.UI.WebControls.Label
    'Protected WithEvents LabelAddressCheckSent As System.Web.UI.WebControls.Label
    'Protected WithEvents TextBoxAddress As System.Web.UI.WebControls.TextBox
    'Protected WithEvents LabelAccNo As System.Web.UI.WebControls.Label
    'Protected WithEvents TextBoxAccountNo As System.Web.UI.WebControls.TextBox
    'Protected WithEvents LabelBankInfo As System.Web.UI.WebControls.Label
    'Protected WithEvents TextBoxBankInfo As System.Web.UI.WebControls.TextBox
    'Protected WithEvents LabelEntityType As System.Web.UI.WebControls.Label
    'Protected WithEvents TextBoxEntityType As System.Web.UI.WebControls.TextBox
    'Protected WithEvents LabelEntityAddress As System.Web.UI.WebControls.Label
    'Protected WithEvents TextBoxEntityAddress As System.Web.UI.WebControls.TextBox
    'Protected WithEvents LabelLegalEntity As System.Web.UI.WebControls.Label
    'Protected WithEvents TextBoxLegalEntity As System.Web.UI.WebControls.TextBox
    Protected WithEvents Datalist2 As System.Web.UI.WebControls.DataList
    Protected WithEvents PlaceHolderMessageBox As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents dgItemDetails As System.Web.UI.WebControls.DataGrid
    Protected WithEvents RadioButton1 As System.Web.UI.WebControls.RadioButton
    Protected WithEvents LabelDeduction As System.Web.UI.WebControls.Label
    Protected WithEvents DataGridDeductions As System.Web.UI.WebControls.DataGrid
    Protected WithEvents datalistTDLoan As System.Web.UI.WebControls.DataList
    Protected WithEvents datalistRefund As System.Web.UI.WebControls.DataList
    Protected WithEvents datalistAnnuity As System.Web.UI.WebControls.DataList


    'Public Delegate Sub MyDelegate(ByVal textToSet As String)
    Public Delegate Sub DisbursementbyPersIdDelegate(ByVal textToSet As String)
    Public Event DisbursementbyPersIdEvent As DisbursementbyPersIdDelegate

    Public Delegate Sub PopulateDisbursementGrid1()
    Public Event PopulateDisbursementGridEvent As PopulateDisbursementGrid1

    Public Delegate Sub ClearDisbursementGrid()
    Public Event ClearDisbursementGridEvent As ClearDisbursementGrid
    'Added by imran on 24/11/2009 Get fund status for selected request id
    Public Delegate Sub GetFundStatusbyRequestIdDelegate(ByVal RequestID As String)
    Public Event GetFundStatusbyRequestIdEvent As GetFundStatusbyRequestIdDelegate
    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()

    End Sub

#End Region

    Public Property Activity() As String
        Get
            Return Session("Activity")
        End Get
        Set(ByVal Value As String)
            Session("Activity") = Value
        End Set
    End Property
    Public Property Action() As String
        Get
            Return Session("Action")
        End Get
        Set(ByVal Value As String)
            Session("Action") = Value
        End Set
    End Property
    Public Property PersId() As String
        Get
            Return Session("PersId")
        End Get
        Set(ByVal Value As String)
            Session("PersId") = Value
        End Set
    End Property
    Public g_dataset_dsDisbursements As DataSet
    Dim g_dataset_dsDisbursementsByPersId As DataSet
    Dim arrDisbursementId(50) As String
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'If Not IsPostBack = True Then
        If Session("ShowDisbursement") = "Yes" Then
            PopulateDisbursementGrid()
            Session("ShowDisbursement") = "No"
        End If
        'End If
    End Sub

    Public Sub PopulateDisbursementGrid()
        Dim l_string_PersId As String
        Dim l_integer_GridIndex As Integer
        Dim l_string_Activity As String
        Dim l_integer_AnnuityOnly As Integer
        Dim l_datarow_CurrentRow As DataRow
        'Aparna Samala -22/09/2006
        Dim l_searchExpr As String
        Dim l_dr_CurrentRow() As DataRow
        'Aparna Samala -22/09/2006
        Dim objLoan As New LoanClass
        Dim objRefund As New RefundClass
        Dim objAnnuity As New AnnuityClass



        Try
            If Not Session("dsDisbursements") Is Nothing Then

                LabelNoRecordFound.Visible = False

            g_dataset_dsDisbursements = Session("dsDisbursements")
            l_integer_GridIndex = Session("DisbursementIndex") 'commented by priya on 12/15/2008 for YRS 5.0-626

            l_datarow_CurrentRow = g_dataset_dsDisbursements.Tables("Disbursements").Rows(l_integer_GridIndex)
            l_string_PersId = CType(l_datarow_CurrentRow("PersId"), String)
            If Action = "REFUND" Then

                If Not IsNothing(objRefund.GetDisbursementByPersid(l_string_PersId, Activity)) Then
                    Session("dsDisbursementsByPersId") = objRefund.GetDisbursementByPersid(l_string_PersId, Activity)

                    g_dataset_dsDisbursementsByPersId = Session("dsDisbursementsByPersId")
                        datalistRefund.Visible = True
                    datalistRefund.DataSource = g_dataset_dsDisbursementsByPersId.Tables("l_disbursementHeading")
                    datalistRefund.DataBind()


                End If
            ElseIf Action = "TDLOAN" Then

                If Not IsNothing(objLoan.GetLoanDisbursementByPersid(l_string_PersId, Activity)) Then
                    Session("dsDisbursementsByPersId") = objLoan.GetLoanDisbursementByPersid(l_string_PersId, Activity)

                    g_dataset_dsDisbursementsByPersId = Session("dsDisbursementsByPersId")

                    datalistTDLoan.DataSource = g_dataset_dsDisbursementsByPersId.Tables("l_disbursementHeading")
                    datalistTDLoan.DataBind()

                End If


            ElseIf Action = "ANNUITY" Then
                If Not IsNothing(objAnnuity.GetAnnuityDisbursementByPersid(l_string_PersId, Activity)) Then
                    Session("dsDisbursementsByPersId") = objAnnuity.GetAnnuityDisbursementByPersid(l_string_PersId, Activity)

                    g_dataset_dsDisbursementsByPersId = Session("dsDisbursementsByPersId")

                    datalistAnnuity.DataSource = g_dataset_dsDisbursementsByPersId.Tables("l_disbursementHeading")
                    datalistAnnuity.DataBind()
                End If
            End If

            If Not (Session("DisbursementByPersIdIndex") > 0) Then
                Session("DisbursementByPersIdIndex") = 0
                End If

            'imran
            '***LoadDeductions()
            'PopulateDisbursementTextBoxes()
            Me.LabelNoRecordFound.Visible = False
                Me.LabelPayeeSSN.Text = l_datarow_CurrentRow("SSN").ToString + "-" + l_datarow_CurrentRow("FirstName").ToString + " " + l_datarow_CurrentRow("MiddleName").ToString + " " + l_datarow_CurrentRow("LastName").ToString
           
            End If
        Catch ex As Exception
            'LabelNoRecordFound.Visible = True
            datalistRefund.DataBind()
            'datalistRefund.Visible = False
            Throw ex
        End Try


       

    End Sub

    Private Function DisplayGridData(ByVal dg As DataGrid, ByVal bool As Boolean)

        For Each Item As DataGridItem In dg.Items
            Dim i As Integer
            Dim l_datarow_CurrentRow As DataRow
            i = 0
            Dim chk As CheckBox
            Dim imgAddress As ImageButton
            If Action = "REFUND" Then
                chk = Item.Controls(0).FindControl("chkRefund")
                imgAddress = Item.Controls(1).FindControl("ImagebtnAddress")
            ElseIf Action = "TDLOAN" Then
                chk = Item.Controls(0).FindControl("chkLoan")
                imgAddress = Item.Controls(1).FindControl("imgLoanAddress")
            ElseIf Action = "ANNUITY" Then
                chk = Item.Controls(0).FindControl("chkAnnuity")
                imgAddress = Item.Controls(1).FindControl("imgAnnuityAddress")
            End If

          


            If Not IsNothing(chk) Then

                If bool = False Then
                    chk.Checked = False
                    chk.Enabled = False
                    imgAddress.Enabled = False

                ElseIf bool = True Then
                    imgAddress.Enabled = True
                    If Action = "REFUND" Then

                        If (Activity = "REFUNDRISSUE") Then


                            If (Item.Cells(8).Text = "Yes" OrElse Item.Cells(9).Text = "Yes") Then
                                chk.Attributes.Clear()
                                chk.Enabled = False
                                chk.Checked = False
                            Else
                                chk.Checked = True
                                arrDisbursementId(i) = Item.Cells(0).Text
                                chk.Enabled = False
                            End If

                        ElseIf Activity = "REFUNDREISSUE" Then
                            If (Item.Cells(8).Text = "Yes" OrElse Item.Cells(9).Text = "Yes") Then
                                chk.Attributes.Clear()
                                chk.Checked = False
                                chk.Enabled = False
                            Else
                                arrDisbursementId(i) = Item.Cells(0).Text
                                chk.Enabled = True
                                chk.Checked = True
                            End If

                        ElseIf Activity = "REFUNDREPLACE" Then
                            If (Item.Cells(8).Text = "Yes" OrElse Item.Cells(9).Text = "Yes") Then
                                chk.Attributes.Clear()
                                chk.Checked = False
                                chk.Enabled = False
                            Else
                                chk.Attributes.Clear()
                                chk.Checked = False
                                chk.Enabled = True
                                arrDisbursementId(i) = Item.Cells(0).Text
                            End If
                        End If

                    ElseIf Action = "TDLOAN" Then
                        If Activity = "LOANREVERSE" Then

                            If (Item.Cells(8).Text = "Yes" OrElse Item.Cells(9).Text = "Yes") Then
                                chk.Attributes.Clear()
                                chk.Checked = False
                                chk.Enabled = False
                            Else
                                arrDisbursementId(i) = Item.Cells(0).Text
                                chk.Enabled = False
                                chk.Checked = True
                            End If


                        ElseIf Activity = "LOANREPLACE" Then

                            If (Item.Cells(8).Text = "Yes" OrElse Item.Cells(9).Text = "Yes") Then
                                chk.Attributes.Clear()
                                chk.Checked = True
                                chk.Enabled = False
                            Else
                                chk.Attributes.Clear()
                                chk.Checked = False
                                chk.Enabled = True
                                arrDisbursementId(i) = Item.Cells(0).Text
                            End If
                        End If

                    End If


                End If
            End If
            i = i + 1
        Next
        Session("UnvoidedDisbursementId") = arrDisbursementId
    End Function
    Private Function LoadDeductions()

        'Hafiz 03Feb06 Cache-Session
        'Dim l_CacheManager As CacheManager
        'Hafiz 03Feb06 Cache-Session

        Dim l_DataTable As DataTable

        Try
            'Hafiz 03Feb06 Cache-Session
            'l_CacheManager = CacheFactory.GetCacheManager()
            'Hafiz 03Feb06 Cache-Session

            l_DataTable = YMCARET.YmcaBusinessObject.RefundRequest.GetDeductions
            ' DataGrid1.DataSource = l_DataTable
            ' DataGrid1.DataBind()
            'Response.End()
            DataGridDeductions.DataSource = l_DataTable
            DataGridDeductions.DataBind()

            'Hafiz 03Feb06 Cache-Session
            'l_CacheManager.Add("R_Deductions", l_DataTable)
            Session("R_Deductions") = l_DataTable
            'Hafiz 03Feb06 Cache-Session

        Catch ex As Exception
            Throw
        End Try
    End Function

    Private Sub dgItemDetails_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgItemDetails.ItemCommand
        If e.CommandName = "ShowAddress" Then

            'PopulateDisbursementTextBoxes()

        End If
    End Sub

    Sub CheckBox_CheckedChanged(ByVal Source As Object, ByVal e As System.EventArgs)
        Try
            Dim strID As String
            Dim chk As CheckBox
            chk = CType(Source, CheckBox)

            strID = chk.ClientID
            If Not IsNothing(chk) Then
                Dim dgRefund As DataGrid

                ''If Activity = "REFUNDRISSUE" Then       'void Reissue withdrawal
                'Added on 12102009 for BT 953
                ClearDisbursementControl()
                If Activity = "REFUNDREPLACE" Then       'void Replace withdrawal 
                    For Each item As DataListItem In datalistRefund.Items
                        dgRefund = CType(item.FindControl("dgItemDetails"), DataGrid)
                        If Not IsNothing(dgRefund) Then
                            Dim i As Integer
                            For i = 0 To dgRefund.Items.Count - 1
                                Dim chkRefund As CheckBox
                                chkRefund = dgRefund.Items(i).FindControl("chkRefund")
                                If Not IsNothing(chkRefund) Then

                                    If chkRefund.ClientID <> strID Then
                                        chkRefund.Checked = False
                                    End If

                                    If chkRefund.Checked = True Then
                                        GenerateEvents(dgRefund.Items(i).Cells(1).Text.Trim())
                                        'Else
                                        '    ClearDisbursementControl()
                                    End If


                                End If
                            Next
                        End If
                    Next
                End If


                If Activity = "REFUNDREVERSE" Then
                    For Each item As DataListItem In datalistRefund.Items
                        dgRefund = CType(item.FindControl("dgItemDetails"), DataGrid)
                        If Not IsNothing(dgRefund) Then
                            Dim i As Integer
                            For i = 0 To dgRefund.Items.Count - 1
                                Dim chkRefund As CheckBox
                                chkRefund = dgRefund.Items(i).FindControl("chkRefund")
                                If Not IsNothing(chkRefund) Then

                                    If chkRefund.ClientID = strID Then
                                        chkRefund.Checked = True
                                        ' Else

                                        'If (dgRefund.Items(i).Cells(8).Text = "Yes" OrElse dgRefund.Items(i).Cells(9).Text = "Yes") Then
                                        '    chkRefund.Checked = True
                                        'Else
                                        '    chkRefund.Checked = False
                                        'End If
                                    End If

                                    If chkRefund.Checked = True Then
                                        GenerateEvents(dgRefund.Items(i).Cells(1).Text.Trim())
                                    Else
                                        ClearDisbursementControl()
                                    End If



                                End If
                            Next
                        End If
                    Next
                End If

                If Activity = "REFUNDRISSUE" Then
                    For Each item As DataListItem In datalistRefund.Items
                        dgRefund = CType(item.FindControl("dgItemDetails"), DataGrid)
                        If Not IsNothing(dgRefund) Then
                            Dim i As Integer
                            For i = 0 To dgRefund.Items.Count - 1
                                Dim chkRefund As CheckBox
                                chkRefund = dgRefund.Items(i).FindControl("chkRefund")
                                If Not IsNothing(chkRefund) Then

                                    'If chkRefund.ClientID <> strID Then
                                    '    chkRefund.Checked = False
                                    'End If

                                    If chkRefund.Checked = True Then
                                        GenerateEvents(dgRefund.Items(i).Cells(1).Text.Trim())
                                        '04-11.09 : Added exir for to show address details, if there are more than one disbursement in request.
                                        Exit For
                                    Else
                                        ClearDisbursementControl()
                                    End If



                                End If
                            Next
                        End If
                    Next
                End If

                If Activity = "LOANREPLACE" Then        'void Loan Replace
                    For Each item As DataListItem In datalistTDLoan.Items
                        dgRefund = CType(item.FindControl("dgLoan"), DataGrid)
                        If Not IsNothing(dgRefund) Then
                            Dim i As Integer
                            For i = 0 To dgRefund.Items.Count - 1
                                Dim chkLoan As CheckBox
                                chkLoan = dgRefund.Items(i).FindControl("chkLoan")
                                If Not IsNothing(chkLoan) Then

                                    If chkLoan.ClientID = strID Then
                                        chkLoan.Checked = True
                                    Else
                                        chkLoan.Checked = False
                                        'If (dgRefund.Items(i).Cells(8).Text = "Yes" OrElse dgRefund.Items(i).Cells(9).Text = "Yes") Then
                                        '    chkRefund.Checked = True
                                        'Else
                                        '    chkRefund.Checked = False
                                        'End If
                                    End If

                                    If chkLoan.Checked = True Then
                                        GenerateEvents(dgRefund.Items(i).Cells(1).Text.Trim())
                                    Else
                                        ClearDisbursementControl()
                                    End If

                                End If
                            Next
                        End If
                    Next
                End If

                If Activity = "VReplace" Then       'void Annunity Replace
                    For Each item As DataListItem In datalistAnnuity.Items
                        dgRefund = CType(item.FindControl("dgAnnuity"), DataGrid)
                        If Not IsNothing(dgRefund) Then
                            Dim i As Integer
                            For i = 0 To dgRefund.Items.Count - 1
                                Dim chkAnnuity As CheckBox
                                chkAnnuity = dgRefund.Items(i).FindControl("chkAnnuity")
                                If Not IsNothing(chkAnnuity) Then

                                    If chkAnnuity.ClientID <> strID Then
                                        chkAnnuity.Checked = False
                                        'Else
                                        '    chkAnnuity.Checked = False
                                    End If
                                    If chkAnnuity.Checked = True Then
                                        GenerateEvents(dgRefund.Items(i).Cells(1).Text.Trim())
                                    Else
                                        ClearDisbursementControl()
                                    End If

                                End If
                            Next
                        End If
                    Next
                End If

            End If
        Catch ex As Exception
            Throw (ex)
        End Try
    End Sub

    Sub image_OnClick(ByVal Source As Object, ByVal e As ImageClickEventArgs)
        Try
            Dim strimg As String
            Dim img As ImageButton
            img = CType(Source, ImageButton)

            strimg = img.ClientID
            If Not IsNothing(img) Then
                Dim dtgrid As DataGrid

                If Action = "REFUND" Then
                    For Each item As DataListItem In datalistRefund.Items
                        dtgrid = CType(item.FindControl("dgItemDetails"), DataGrid)
                        If Not IsNothing(dtgrid) Then
                            Dim i As Integer
                            'Added on 22/10/2009 by imran for BT 969
                            For i = 0 To dtgrid.Items.Count - 1
                                Dim btnImage As ImageButton
                                btnImage = dtgrid.Items(i).FindControl("ImagebtnAddress")
                                If Not IsNothing(btnImage) Then
                                    If btnImage.ClientID <> strimg Then
                                        'btnImage.ImageUrl = "../images/select.gif"
                                        btnImage.ImageUrl = "../images/details.gif"
                                    End If
                                End If

                            Next
                            For i = 0 To dtgrid.Items.Count - 1
                                Dim btnImage As ImageButton
                                btnImage = dtgrid.Items(i).FindControl("ImagebtnAddress")
                                If Not IsNothing(btnImage) Then
                                    If btnImage.ClientID = strimg Then
                                        '  If img.ImageUrl = "../images/select.gif" Then
                                        'img.ImageUrl = "../images/selected.gif"
                                        img.ImageUrl = "../images/details.gif"
                                        GenerateEvents(dtgrid.Items(i).Cells(1).Text.Trim())
                                        'Else
                                        '    img.ImageUrl = "../images/select.gif"
                                        '    GenerateEvents("")
                                        'End If
                                    End If
                                End If

                            Next
                        End If
                    Next
                ElseIf Action = "TDLOAN" Then
                    For Each item As DataListItem In datalistTDLoan.Items
                        dtgrid = CType(item.FindControl("dgLoan"), DataGrid)
                        If Not IsNothing(dtgrid) Then
                            Dim i As Integer

                            'Added on 22/10/2009 by imran for BT 969
                            For i = 0 To dtgrid.Items.Count - 1
                                Dim btnImage As ImageButton
                                btnImage = dtgrid.Items(i).FindControl("imgLoanAddress")
                                If Not IsNothing(btnImage) Then
                                    If btnImage.ClientID <> strimg Then
                                        'btnImage.ImageUrl = "../images/select.gif"
                                        btnImage.ImageUrl = "../images/details.gif"
                                    End If
                                End If
                            Next

                            For i = 0 To dtgrid.Items.Count - 1
                                Dim btnImage As ImageButton
                                btnImage = dtgrid.Items(i).FindControl("imgLoanAddress")
                                If Not IsNothing(btnImage) Then
                                    If btnImage.ClientID = strimg Then

                                        img.ImageUrl = "../images/selected.gif"
                                        GenerateEvents(dtgrid.Items(i).Cells(1).Text.Trim())

                                        'If btnImage.ImageUrl = "../images/select.gif" Then
                                        '    img.ImageUrl = "../images/selected.gif"
                                        '    GenerateEvents(dtgrid.Items(i).Cells(1).Text.Trim())
                                        'Else
                                        '    btnImage.ImageUrl = "../images/select.gif"
                                        '    GenerateEvents("")

                                        'End If

                                    End If
                                End If
                            Next
                        End If
                    Next

                ElseIf Action = "ANNUITY" Then
                    For Each item As DataListItem In datalistAnnuity.Items
                        dtgrid = CType(item.FindControl("dgAnnuity"), DataGrid)
                        If Not IsNothing(dtgrid) Then
                            Dim i As Integer
                            'Added on 22/10/2009 by imran for BT 969
                            For i = 0 To dtgrid.Items.Count - 1
                                Dim btnImage As ImageButton
                                btnImage = dtgrid.Items(i).FindControl("imgAnnuityAddress")
                                If Not IsNothing(btnImage) Then
                                    If btnImage.ClientID <> strimg Then
                                        'btnImage.ImageUrl = "../images/select.gif"
                                        btnImage.ImageUrl = "../images/details.gif"
                                    End If
                                End If
                            Next

                            For i = 0 To dtgrid.Items.Count - 1
                                Dim btnImage As ImageButton
                                btnImage = dtgrid.Items(i).FindControl("imgAnnuityAddress")
                                If Not IsNothing(btnImage) Then
                                    If btnImage.ClientID = strimg Then

                                        img.ImageUrl = "../images/selected.gif"
                                        GenerateEvents(dtgrid.Items(i).Cells(1).Text.Trim())

                                        'If btnImage.ImageUrl = "../images/select.gif" Then
                                        '    img.ImageUrl = "../images/selected.gif"
                                        '    GenerateEvents(dtgrid.Items(i).Cells(1).Text.Trim())
                                        'Else
                                        '    btnImage.ImageUrl = "../images/select.gif"
                                        '    GenerateEvents("")

                                        'End If

                                    End If
                                End If
                            Next
                        End If
                    Next

                End If

            End If
        Catch ex As Exception
            Throw (ex)
        End Try
    End Sub
    Public Sub GenerateEvents(ByVal textToSet As String)
        RaiseEvent DisbursementbyPersIdEvent(textToSet)

    End Sub
    Public Sub GenerateDisbursementGridEvents()
        RaiseEvent PopulateDisbursementGridEvent()

    End Sub
    Public Sub ClearDisbursementControl()
        RaiseEvent ClearDisbursementGridEvent()

    End Sub
    Public Sub CallFundEventstatus(ByVal RequestId As String)
        RaiseEvent GetFundStatusbyRequestIdEvent(RequestId)

    End Sub
    Sub Radio_CheckedChanged(ByVal Source As Object, ByVal e As System.EventArgs)
        Dim strRadio As String
        Dim radio As RadioButton
        radio = CType(Source, RadioButton)
        If Not IsNothing(radio) Then
            strRadio = radio.ClientID


            If radio.Checked = True Then
                Dim dlRadio As RadioButton
                Dim datagrid As DataGrid

                If Action = "REFUND" Then
                    For Each item As DataListItem In datalistRefund.Items
                        dlRadio = CType(item.FindControl("rdRefund"), RadioButton)
                        datagrid = CType(item.FindControl("dgItemDetails"), DataGrid)

                        If Not IsNothing(datagrid) AndAlso Not IsNothing(dlRadio) Then
                            If strRadio = dlRadio.ClientID Then
                                DisplayGridData(datagrid, True)
                            Else
                                dlRadio.Checked = False
                                DisplayGridData(datagrid, False)
                            End If
                        End If

                    Next

                ElseIf Action = "TDLOAN" Then
                    For Each item As DataListItem In datalistTDLoan.Items
                        dlRadio = CType(item.FindControl("rbLoan"), RadioButton)
                        datagrid = CType(item.FindControl("dgLoan"), DataGrid)

                        If Not IsNothing(datagrid) AndAlso Not IsNothing(dlRadio) Then
                            If strRadio = dlRadio.ClientID Then
                                DisplayGridData(datagrid, True)
                            Else
                                dlRadio.Checked = False
                                DisplayGridData(datagrid, False)
                            End If
                        End If

                    Next
                End If

            End If
        End If



    End Sub

    Private Sub datalistRefund_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataListItemEventArgs) Handles datalistRefund.ItemDataBound
        If Not IsNothing(e.Item.FindControl("dgItemDetails")) Then
            CType(Session("dsDisbursementsByPersId"), DataSet).Tables(0).DefaultView.RowFilter = "RequestId = '" & datalistRefund.DataKeys().Item(e.Item.ItemIndex).ToString.Trim & "'"
            CType(e.Item.FindControl("dgItemDetails"), DataGrid).DataSource = CType(Session("dsDisbursementsByPersId"), DataSet).Tables(0).DefaultView()
            CType(e.Item.FindControl("dgItemDetails"), DataGrid).DataBind()

            Dim img As ImageButton

            img = CType(e.Item.Controls(0).FindControl("Imagebutton1"), ImageButton)
            'CType(e.Item.Controls(0).FindControl("Imagebutton1"), ImageButton).ImageUrl = "images\select.gif"


            If Not IsNothing(img) Then


                If img.ImageUrl = "../images/selected.gif" Then
                    If e.Item.ItemType = ListItemType.AlternatingItem Then
                        CType(e.Item.Controls(0).FindControl("Imagebutton1"), ImageButton).ImageUrl = "images\select.gif"

                        Dim dgdatagrid As DataGrid
                        dgdatagrid = CType(e.Item.FindControl("dgItemDetails"), DataGrid)
                        If Not IsNothing("dgdatagrid") Then
                            DisplayGridData(dgdatagrid, True)
                            'For Each Item As DataGridItem In dgdatagrid.Items
                            '    Dim chk As CheckBox
                            '    chk = Item.Controls(0).FindControl("chkRefund")
                            '    If Not IsNothing(chk) Then
                            '        If (Activity = "REFUNDRISSUE") Then
                            '            If (Item.Cells(8).Text = "Yes" OrElse Item.Cells(9).Text = "Yes") Then
                            '                chk.Attributes.Clear()
                            '                chk.Enabled = False
                            '            Else
                            '                chk.Enabled = True
                            '            End If
                            '            chk.Checked = True
                            '        ElseIf Activity = "RefundReissue" Then
                            '            If (Item.Cells(8).Text = "Yes" OrElse Item.Cells(9).Text = "Yes") Then
                            '                chk.Attributes.Clear()
                            '                chk.Enabled = False
                            '                chk.Checked = True
                            '            Else
                            '                chk.Checked = True
                            '                chk.Enabled = True
                            '            End If
                            '        Else
                            '            If (Item.Cells(8).Text = "Yes" OrElse Item.Cells(9).Text = "Yes") Then
                            '                chk.Attributes.Clear()
                            '                chk.Checked = True
                            '                chk.Enabled = False
                            '            Else
                            '                chk.Attributes.Clear()
                            '                chk.Checked = False
                            '                chk.Enabled = True
                            '            End If
                            '        End If
                            '        End If
                            '    End If
                            'Next
                        End If

                    ElseIf e.Item.ItemType = ListItemType.Item Then
                        Dim dgdatagrid As DataGrid
                        dgdatagrid = CType(e.Item.FindControl("dgItemDetails"), DataGrid)
                        If Not IsNothing("dgdatagrid") Then
                            DisplayGridData(dgdatagrid, True)
                            'For Each Item As DataGridItem In dgdatagrid.Items
                            '    Dim chk As CheckBox
                            '    chk = Item.Controls(0).FindControl("chkRefund")
                            '    If Not IsNothing(chk) Then

                            '        If (Activity = "REFUNDRISSUE") Then
                            '            If (Item.Cells(8).Text = "Yes" OrElse Item.Cells(9).Text = "Yes") Then
                            '                chk.Attributes.Clear()
                            '                chk.Enabled = False
                            '            Else
                            '                chk.Enabled = True
                            '            End If
                            '            chk.Checked = True

                            '        Else
                            '            If (Item.Cells(8).Text = "Yes" OrElse Item.Cells(9).Text = "Yes") Then
                            '                chk.Attributes.Clear()
                            '                chk.Checked = True
                            '                chk.Enabled = False
                            '            Else
                            '                chk.Attributes.Clear()
                            '                chk.Checked = False
                            '                chk.Enabled = True
                            '            End If
                            '        End If

                            '    End If
                            'Next
                        End If

                    End If
                End If
            End If
        End If
    End Sub

    Private Sub datalistRefund_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataListCommandEventArgs) Handles datalistRefund.ItemCommand
        If e.CommandName = "Select" Then
            'Clear Address And Deduction Control
            ClearDisbursementControl()

            Dim img As ImageButton
            Dim l_datarow_CurrentRow As DataRow
            img = (e.CommandSource)
            '************************]
            For Each Item As DataListItem In datalistRefund.Items


                Dim imglst As ImageButton
                imglst = Item.Controls(0).FindControl("Imagebutton1")
                If img.ClientID <> imglst.ClientID Then
                    imglst.ImageUrl = "../images/select.gif"
                End If

                '-----Datagrid
                Dim dg As DataGrid
                dg = CType(Item.FindControl("dgItemDetails"), DataGrid)
                If Not IsNothing("dgdatagrid") Then
                    For Each dgItem As DataGridItem In dg.Items

                        Dim chk As CheckBox
                        Dim imgadd As ImageButton
						If Action = "REFUND" Then
							chk = dgItem.Controls(0).FindControl("chkRefund")
						ElseIf Action = "TDLOAN" Then
							chk = dgItem.Controls(0).FindControl("chkLoan")
						ElseIf Action = "ANNUITY" Then

						End If


						If img.ClientID <> imglst.ClientID Then
							imgadd = dgItem.Controls(0).FindControl("ImagebtnAddress")
							imgadd.Enabled = False
							chk.Checked = False
							chk.Enabled = False
							imglst.ImageUrl = "../images/select.gif"
							'imgadd.ImageUrl = "../images/select.gif"
							imgadd.ImageUrl = "../images/details.gif"
						End If

					Next
                End If
                '---'-----End Datagrid

            Next
            '**********************


            If img.ImageUrl = "../images/select.gif" Then
				img.ImageUrl = "../images/selected.gif"
                CallFundEventstatus(datalistRefund.DataKeys(e.Item.ItemIndex).ToString())

                Dim dgdatagrid As DataGrid
                dgdatagrid = CType(e.Item.FindControl("dgItemDetails"), DataGrid)
                If Not IsNothing("dgdatagrid") Then
                    For Each Item As DataGridItem In dgdatagrid.Items

                        Dim i As Integer
                        i = 0
                        Dim chk As CheckBox

                        If Action = "REFUND" Then
							chk = Item.Controls(0).FindControl("chkRefund")
							If img.ImageUrl <> "../images/select.gif" Then
								If Activity = "REFUNDRISSUE" Then
									Session("MRDRequest") = Nothing
									If CType(Session("dsDisbursementsByPersId"), DataSet).Tables(0).Rows(e.Item.ItemIndex)("RequestType").ToString().Trim() = "MRD" Then
										Session("MRDRequest") = "YES"
									End If
								End If
							End If
                        ElseIf Action = "TDLOAN" Then
                            chk = Item.Controls(0).FindControl("chkLoan")
                        ElseIf Action = "ANNUITY" Then

                        End If
                        img = Item.Controls(0).FindControl("ImagebtnAddress")
                        'img.ImageUrl = "../images/select.gif"
                        img.ImageUrl = "../images/details.gif"
                        If (Item.Cells(8).Text = "Yes" OrElse Item.Cells(9).Text = "Yes") Then
                            img.Enabled = False
                            chk.Enabled = False
                        Else
                            img.Enabled = True
                            chk.Enabled = True
                            ''Added by imran on 16/12/2009  for Gemini Issue  968 -When disbursement is cash check then checkbox disable
                            'g_dataset_dsDisbursementsByPersId = Session("dsDisbursementsByPersId")
                            'If Not g_dataset_dsDisbursementsByPersId Is Nothing Then
                            '    ''**If Not Session("DisbursementId") Is Nothing Then
                            '    Dim l_dr_CurrentRow As DataRow()
                            '    Dim l_searchExpr As String
                            '    l_searchExpr = "DisbursementID = '" + Item.Cells(1).Text + "'"
                            '    l_dr_CurrentRow = g_dataset_dsDisbursementsByPersId.Tables("DisbursementsByPersId").Select(l_searchExpr)
                            '    If l_dr_CurrentRow.Length > 0 Then
                            '        l_datarow_CurrentRow = l_dr_CurrentRow(0)

                            '        If CType(l_datarow_CurrentRow("bitpaid"), String).ToUpper = "TRUE" Then
                            '            chk.Attributes.Clear()
                            '            chk.Enabled = False
                            '            chk.Checked = False
                            '        End If
                            '    End If
                            'End If


                            If Activity = "REFUNDREVERSE" Then
                                chk.Checked = True
                            End If


                        End If

                    Next

                End If
            End If

            If img.ImageUrl = "../images/selected.gif" Then
                img.ImageUrl = "../images/select.gif"
                CallFundEventstatus("")
                Dim dgdatagrid As DataGrid
                dgdatagrid = CType(e.Item.FindControl("dgItemDetails"), DataGrid)
                If Not IsNothing("dgdatagrid") Then
                    For Each Item As DataGridItem In dgdatagrid.Items

                        Dim i As Integer
                        i = 0
                        Dim chk As CheckBox

                        If Action = "REFUND" Then
                            chk = Item.Controls(0).FindControl("chkRefund")
                        ElseIf Action = "TDLOAN" Then
                            chk = Item.Controls(0).FindControl("chkLoan")
                        ElseIf Action = "ANNUITY" Then

                        End If
                        img = Item.Controls(0).FindControl("ImagebtnAddress")
                        'img.ImageUrl = "../images/select.gif"
                        img.ImageUrl = "../images/details.gif"
                        img.Enabled = False
                        chk.Enabled = False
                        chk.Checked = False

                    Next

                End If
            End If
        End If

    End Sub

    Private Sub datalistTDLoan_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataListItemEventArgs) Handles datalistTDLoan.ItemDataBound
        If Not IsNothing(e.Item.FindControl("dgLoan")) Then
            CType(Session("dsDisbursementsByPersId"), DataSet).Tables(0).DefaultView.RowFilter = "Number = '" & datalistTDLoan.DataKeys().Item(e.Item.ItemIndex).ToString.Trim & "'"
            CType(e.Item.FindControl("dgLoan"), DataGrid).DataSource = CType(Session("dsDisbursementsByPersId"), DataSet).Tables(0).DefaultView()
            CType(e.Item.FindControl("dgLoan"), DataGrid).DataBind()


            Dim rbLoan As RadioButton
            rbLoan = e.Item.FindControl("rbLoan")
            If Not IsNothing(rbLoan) Then

                If e.Item.ItemIndex = 0 Then
                    rbLoan.Checked = True
                End If
                If e.Item.ItemType = ListItemType.AlternatingItem Then
                    CType(e.Item.Controls(0).FindControl("Imagebutton5"), ImageButton).ImageUrl = "images\select.gif"

                    Dim dgdatagrid As DataGrid
                    dgdatagrid = CType(e.Item.FindControl("dgLoan"), DataGrid)
                    If Not IsNothing("dgdatagrid") Then
                        DisplayGridData(dgdatagrid, True)
                    End If

                ElseIf e.Item.ItemType = ListItemType.Item Then
                    Dim dgdatagrid As DataGrid
                    dgdatagrid = CType(e.Item.FindControl("dgLoan"), DataGrid)
                    If Not IsNothing("dgdatagrid") Then
                        DisplayGridData(dgdatagrid, True)
                    End If

                End If
            End If
        End If

    End Sub

    Private Sub datalistTDLoan_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataListCommandEventArgs) Handles datalistTDLoan.ItemCommand
        If e.CommandName = "Select" Then
            'Clear Address And Deduction Control
            ClearDisbursementControl()

            Dim img As ImageButton
            img = (e.CommandSource)
            '************************]
            For Each Item As DataListItem In datalistRefund.Items


                Dim imglst As ImageButton
                imglst = Item.Controls(0).FindControl("imgTDLoan")
                If img.ClientID <> imglst.ClientID Then
                    imglst.ImageUrl = "../images/select.gif"
                End If

                '-----Datagrid
                Dim dg As DataGrid
                dg = CType(Item.FindControl("dgLoan"), DataGrid)
                If Not IsNothing("dg") Then
                    For Each dgItem As DataGridItem In dg.Items

                        Dim chk As CheckBox
                        Dim imgadd As ImageButton
                        If Action = "REFUND" Then
                            chk = dgItem.Controls(0).FindControl("chkRefund")
                        ElseIf Action = "TDLOAN" Then
                            chk = dgItem.Controls(0).FindControl("chkLoan")
                        ElseIf Action = "ANNUITY" Then

                        End If
                        If img.ClientID <> imglst.ClientID Then
                            imgadd = dgItem.Controls(0).FindControl("imgLoanAddress")
                            imgadd.Enabled = False
                            chk.Checked = False
                            chk.Enabled = False
                            'imglst.ImageUrl = "../images/select.gif"
                            imglst.ImageUrl = "../images/details.gif"
                        End If

                    Next
                End If
                '---'-----End Datagrid

            Next
            '**********************


            If img.ImageUrl = "../images/select.gif" Then
                img.ImageUrl = "../images/selected.gif"

                Dim dgdatagrid As DataGrid
                dgdatagrid = CType(e.Item.FindControl("dgLoan"), DataGrid)
                If Not IsNothing("dgdatagrid") Then
                    For Each Item As DataGridItem In dgdatagrid.Items

                        Dim i As Integer
                        i = 0
                        Dim chk As CheckBox

                        If Action = "REFUND" Then
                            chk = Item.Controls(0).FindControl("chkRefund")
                        ElseIf Action = "TDLOAN" Then
                            chk = Item.Controls(0).FindControl("chkLoan")
                        End If
                        img = Item.Controls(0).FindControl("imgLoanAddress")

                        If (Item.Cells(8).Text = "Yes" OrElse Item.Cells(9).Text = "Yes") Then
                            img.Enabled = False
                            chk.Enabled = False
                        Else
                            img.Enabled = True
                            chk.Enabled = True
                        End If


                        If Activity = "LOANREVERSE" Then
                            chk.Checked = True
                        End If


                    Next

                End If
            End If

            If img.ImageUrl = "../images/selected.gif" Then
                img.ImageUrl = "../images/select.gif"

                Dim dgdatagrid As DataGrid
                dgdatagrid = CType(e.Item.FindControl("dgLoan"), DataGrid)
                If Not IsNothing("dgdatagrid") Then
                    For Each Item As DataGridItem In dgdatagrid.Items

                        Dim i As Integer
                        i = 0
                        Dim chk As CheckBox

                        If Action = "REFUND" Then
                            chk = Item.Controls(0).FindControl("chkRefund")
                        ElseIf Action = "TDLOAN" Then
                            chk = Item.Controls(0).FindControl("chkLoan")
                        End If
                        img = Item.Controls(0).FindControl("imgLoanAddress")
                        'img.ImageUrl = "../images/select.gif"
                        img.ImageUrl = "../images/details.gif"

                        img.Enabled = False
                        chk.Enabled = False
                        chk.Checked = False
                        If (Item.Cells(8).Text = "Yes" OrElse Item.Cells(9).Text = "Yes") Then
                            img.Enabled = False
                            chk.Enabled = False
                        End If

                        If Activity = "LOANREVERSE" Then
                            chk.Checked = False
                        End If


                    Next

                End If
            End If
        End If
    End Sub

    Private Sub datalistAnnuity_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataListItemEventArgs) Handles datalistAnnuity.ItemDataBound
        If Not IsNothing(e.Item.FindControl("dgAnnuity")) Then

            Dim lblissuedate As Label
            If Not IsNothing(e.Item.FindControl("lblIssueDate")) Then
                lblissuedate = CType(e.Item.FindControl("lblIssueDate"), Label)
            End If

            'CType(Session("dsDisbursementsByPersId"), DataSet).Tables(0).DefaultView.RowFilter = "Number = '" & datalistAnnuity.DataKeys().Item(e.Item.ItemIndex).ToString.Trim & "'"
            CType(Session("dsDisbursementsByPersId"), DataSet).Tables(0).DefaultView.RowFilter = "RequestId = '" & datalistAnnuity.DataKeys().Item(e.Item.ItemIndex).ToString.Trim & "' "
            'CType(Session("dsDisbursementsByPersId"), DataSet).Tables(0).DefaultView.RowFilter = "DisbursementID = '" & datalistAnnuity.DataKeys().Item(e.Item.ItemIndex).ToString.Trim & "'and IssuedDate ='" & lblissuedate.Text & "'  "
            'CType(Session("dsDisbursementsByPersId"), DataSet).Tables(0).DefaultView.RowFilter = "IssuedDate = '" & CType(Session("dsDisbursementsByPersId"), DataSet).Tables(0).Rows(0)("IssuedDate") & "'"
            CType(e.Item.FindControl("dgAnnuity"), DataGrid).DataSource = CType(Session("dsDisbursementsByPersId"), DataSet).Tables(0).DefaultView()
            CType(e.Item.FindControl("dgAnnuity"), DataGrid).DataBind()

            If e.Item.ItemType = ListItemType.Item Then
                Dim dgdatagrid As DataGrid
                dgdatagrid = CType(e.Item.FindControl("dgAnnuity"), DataGrid)
                If Not IsNothing("dgdatagrid") Then
                    DisplayGridData(dgdatagrid, True)
                End If
            End If

            'Dim rbLoan As RadioButton
            'rbLoan = e.Item.FindControl("rbLoan")
            'If Not IsNothing(rbLoan) Then

            '    If e.Item.ItemIndex = 0 Then
            '        rbLoan.Checked = True
            '    End If
            '    If e.Item.ItemType = ListItemType.AlternatingItem Then
            '        CType(e.Item.Controls(0).FindControl("Imagebutton5"), ImageButton).ImageUrl = "images\select.gif"

            '        Dim dgdatagrid As DataGrid
            '        dgdatagrid = CType(e.Item.FindControl("dgLoan"), DataGrid)
            '        If Not IsNothing("dgdatagrid") Then
            '            DisplayGridData(dgdatagrid, True)
            '        End If

            '    ElseIf e.Item.ItemType = ListItemType.Item Then
            '        Dim dgdatagrid As DataGrid
            '        dgdatagrid = CType(e.Item.FindControl("dgAnnuity"), DataGrid)
            '        If Not IsNothing("dgdatagrid") Then
            '            DisplayGridData(dgdatagrid, True)
            '        End If

            '    End If
            'End If
        End If

    End Sub

    Private Sub datalistAnnuity_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataListCommandEventArgs) Handles datalistAnnuity.ItemCommand
        If e.CommandName = "Select" Then
            'Clear Address And Deduction Control
            ClearDisbursementControl()

            Dim img As ImageButton
            img = (e.CommandSource)
            '************************]
            For Each Item As DataListItem In datalistAnnuity.Items


                Dim imglst As ImageButton
                imglst = Item.Controls(0).FindControl("imgAnnuity")
                If img.ClientID <> imglst.ClientID Then
                    imglst.ImageUrl = "../images/select.gif"
                End If

                '-----Datagrid
                Dim dg As DataGrid
                dg = CType(Item.FindControl("dgAnnuity"), DataGrid)
                If Not IsNothing("dg") Then
                    For Each dgItem As DataGridItem In dg.Items

                        Dim chk As CheckBox
                        Dim imgadd As ImageButton
                        If Action = "REFUND" Then
                            chk = dgItem.Controls(0).FindControl("chkRefund")
                        ElseIf Action = "TDLOAN" Then
                            chk = dgItem.Controls(0).FindControl("chkLoan")
                        ElseIf Action = "ANNUITY" Then
                            chk = dgItem.Controls(0).FindControl("chkAnnuity")
                        End If
                        If img.ClientID <> imglst.ClientID Then
                            imgadd = dgItem.Controls(0).FindControl("imgAnnuityAddress")
                            imgadd.Enabled = False
                            chk.Checked = False
                            chk.Enabled = False
                            chk.Checked = False
                            'imglst.ImageUrl = "../images/select.gif"
                            imglst.ImageUrl = "../images/details.gif"
                        End If

                    Next
                End If
                '---'-----End Datagrid

            Next
            '**********************


            If img.ImageUrl = "../images/select.gif" Then
                img.ImageUrl = "../images/selected.gif"

                Dim dgdatagrid As DataGrid
                dgdatagrid = CType(e.Item.FindControl("dgAnnuity"), DataGrid)
                If Not IsNothing("dgdatagrid") Then
                    For Each Item As DataGridItem In dgdatagrid.Items

                        Dim i As Integer
                        i = 0
                        Dim chk As CheckBox

                        If Action = "REFUND" Then
                            chk = Item.Controls(0).FindControl("chkRefund")
                        ElseIf Action = "TDLOAN" Then
                            chk = Item.Controls(0).FindControl("chkLoan")
                        ElseIf Action = "ANNUITY" Then
                            chk = Item.Controls(0).FindControl("chkAnnuity")
                        End If
                        img = Item.Controls(0).FindControl("imgAnnuityAddress")

                        If (Item.Cells(8).Text = "Yes" OrElse Item.Cells(9).Text = "Yes") Then
                            img.Enabled = False
                            chk.Enabled = False
                            chk.Checked = False
                        Else
                            img.Enabled = True
                            chk.Enabled = True
                            '  chk.Checked = True
                        End If
                        If Activity = "VREVERSE" Then
                            chk.Checked = True
                        End If
                        'If Activity = "VREVERSE" Then
                        '    chk.Checked = True
                        'End If

                    Next


                End If
            End If


            If img.ImageUrl = "../images/selected.gif" Then
                img.ImageUrl = "../images/select.gif"

                Dim dgdatagrid As DataGrid
                dgdatagrid = CType(e.Item.FindControl("dgAnnuity"), DataGrid)
                If Not IsNothing("dgdatagrid") Then
                    For Each Item As DataGridItem In dgdatagrid.Items

                        Dim i As Integer
                        i = 0
                        Dim chk As CheckBox

                        If Action = "REFUND" Then
                            chk = Item.Controls(0).FindControl("chkRefund")
                        ElseIf Action = "TDLOAN" Then
                            chk = Item.Controls(0).FindControl("chkLoan")
                        ElseIf Action = "ANNUITY" Then
                            chk = Item.Controls(0).FindControl("chkAnnuity")
                        End If
                        img = Item.Controls(0).FindControl("imgAnnuityAddress")
                        'img.ImageUrl = "../images/select.gif"
                        img.ImageUrl = "../images/details.gif"

                        img.Enabled = False
                        chk.Enabled = False
                        chk.Checked = False

                    Next


                End If
            End If


        End If
    End Sub
End Class

' CHANGE HISTORY
'DATE       NAME            DESCRIPTION
'---------------------------------------------------------------------
'2009.10.30 Nikunj Patel    Setting the default sort order in the code to IssuedDate DESC

Public Class VoidDisbursement
    Inherits System.Web.UI.UserControl

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents LabelPayeeSSN As System.Web.UI.WebControls.Label
    Protected WithEvents Datagrid1 As System.Web.UI.WebControls.DataGrid
    Protected WithEvents LabelRecordNotFound As System.Web.UI.WebControls.Label
    'Public Delegate Sub MyDelegate(ByVal textToSet As String)

    Public Delegate Sub DisbursementbyPersIdDelegate(ByVal textToSet As String)
    Public Event DisbursementbyPersIdEvent As DisbursementbyPersIdDelegate

    Public Delegate Sub PopulateDisbursementGrid1()
    Public Event PopulateDisbursementGridEvent As PopulateDisbursementGrid1

    Public Delegate Sub ClearDisbursementGrid()
    Public Event ClearDisbursementGridEvent As ClearDisbursementGrid
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
        If Session("ShowDisbursement") = "Yes" Then
            PopulateDisbursementGrid()
            Session("ShowDisbursement") = "No"
        End If
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


                g_dataset_dsDisbursements = Session("dsDisbursements")
                l_integer_GridIndex = Session("DisbursementIndex") 'commented by priya on 12/15/2008 for YRS 5.0-626
                LabelRecordNotFound.Visible = False
                Datagrid1.Visible = True
                l_datarow_CurrentRow = g_dataset_dsDisbursements.Tables("Disbursements").Rows(l_integer_GridIndex)
                l_string_PersId = CType(l_datarow_CurrentRow("PersId"), String)
                If Action = "REFUND" Then

                    If Not IsNothing(objRefund.GetDisbursementByPersid(l_string_PersId, Activity)) Then
                        Session("dsDisbursementsByPersId") = objRefund.GetDisbursementByPersid(l_string_PersId, Activity)
                        g_dataset_dsDisbursementsByPersId = Session("dsDisbursementsByPersId")
                    End If
                ElseIf Action = "TDLOAN" Then

                    If Not IsNothing(objLoan.GetLoanDisbursementByPersid(l_string_PersId, Activity)) Then
                        Session("dsDisbursementsByPersId") = objLoan.GetLoanDisbursementByPersid(l_string_PersId, Activity)
                        g_dataset_dsDisbursementsByPersId = Session("dsDisbursementsByPersId")

                    End If


                ElseIf Action = "ANNUITY" Then
                    If Not IsNothing(objAnnuity.GetAnnuityDisbursementByPersid(l_string_PersId, Activity)) Then
                        Session("dsDisbursementsByPersId") = objAnnuity.GetAnnuityDisbursementByPersid(l_string_PersId, Activity)
                        g_dataset_dsDisbursementsByPersId = Session("dsDisbursementsByPersId")
                    End If
                End If

                If Not g_dataset_dsDisbursementsByPersId Is Nothing Then

                    Dim dv As New DataView
                    Dim SortExpression As String
                    SortExpression = "IssuedDate"
                    g_dataset_dsDisbursements = Session("dsDisbursementsByPersId")
                    dv = g_dataset_dsDisbursements.Tables(0).DefaultView
                    dv.Sort = SortExpression
                    If Not Session("VRManager_Sort") Is Nothing Then
                        If Session("VRManager_Sort").ToString.Trim.EndsWith("ASC") Then
                            dv.Sort = SortExpression + " ASC"
                        Else
                            dv.Sort = SortExpression + " DESC"
                        End If
                    Else
                        dv.Sort = SortExpression + " DESC"
                    End If

                    Datagrid1.SelectedIndex = -1
                    Me.Datagrid1.DataSource = Nothing
                    Me.Datagrid1.DataSource = dv
                    Me.Datagrid1.DataBind()
                    Session("VRManager_Sort") = dv.Sort

                    'Datagrid1.DataSource = g_dataset_dsDisbursementsByPersId.Tables("l_disbursementHeading")
                    'Datagrid1.DataBind()
                    DisplayGridData(Datagrid1)
                End If


                If Not (Session("DisbursementByPersIdIndex") > 0) Then
                    Session("DisbursementByPersIdIndex") = 0
                End If


                Me.LabelPayeeSSN.Text = l_datarow_CurrentRow("SSN").ToString + "-" + l_datarow_CurrentRow("FirstName").ToString + " " + l_datarow_CurrentRow("MiddleName").ToString + " " + l_datarow_CurrentRow("LastName").ToString
            End If
        Catch ex As Exception
            'LabelRecordNotFound.Visible = True
            Datagrid1.DataBind()
            'Datagrid1.Visible = False
            Throw ex
        End Try




    End Sub
    Private Function DisplayGridData(ByVal dg As DataGrid)

        For Each Item As DataGridItem In dg.Items
            Dim i As Integer
            i = 0
            'Dim chk As CheckBox
            Dim imgAddress As ImageButton

            'chk = Item.Controls(0).FindControl("chkRefund")
            imgAddress = Item.Controls(1).FindControl("ImagebtnAddress")

            If Not IsNothing(imgAddress) Then

                If (Item.Cells(8).Text = "Yes" OrElse Item.Cells(9).Text = "Yes") Then
                    ' chk.Attributes.Clear()
                    'chk.Enabled = False
                    imgAddress.Enabled = False
                Else
                    'chk.Enabled = True
                    imgAddress.Enabled = True
                End If



            End If

            i = i + 1
        Next
        Session("UnvoidedDisbursementId") = arrDisbursementId
    End Function

    Sub CheckBox_CheckedChanged(ByVal Source As Object, ByVal e As System.EventArgs)
        Try
            Dim strID As String
            Dim chk As CheckBox
            chk = CType(Source, CheckBox)

            strID = chk.ClientID
            If Not IsNothing(chk) Then
                Dim i As Integer
                For i = 0 To Datagrid1.Items.Count - 1
                    Dim chkRefund As CheckBox
                    chkRefund = Datagrid1.Items(i).FindControl("chkRefund")
                    If Not IsNothing(chkRefund) Then

                        If chkRefund.ClientID <> strID Then
                            chkRefund.Checked = False
                        End If

                    End If
                Next
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
                Dim i As Integer
                For i = 0 To Datagrid1.Items.Count - 1
                    Dim btnImage As ImageButton
                    btnImage = Datagrid1.Items(i).FindControl("ImagebtnAddress")
                    If Not IsNothing(btnImage) Then
                        If btnImage.ClientID <> strimg Then
                            btnImage.ImageUrl = "../images/select.gif"
                        End If
                    End If

                Next
                For i = 0 To Datagrid1.Items.Count - 1
                    Dim btnImage As ImageButton
                    btnImage = Datagrid1.Items(i).FindControl("ImagebtnAddress")
                    If Not IsNothing(btnImage) Then
                        If btnImage.ClientID = strimg Then
                            '  If img.ImageUrl = "../images/select.gif" Then
                            img.ImageUrl = "../images/selected.gif"
                            GenerateEvents(Datagrid1.Items(i).Cells(1).Text.Trim())
                            'Else
                            '    img.ImageUrl = "../images/select.gif"
                            '    GenerateEvents("")
                            'End If
                        End If
                    End If

                Next

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
    'Added by imran on 29/10/2009 For Sorting Based on IssueDate
    Private Sub Datagrid1_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles Datagrid1.SortCommand
        Try
            Me.Datagrid1.SelectedIndex = -1
            If Not Session("dsDisbursementsByPersId") Is Nothing Then
                Dim dv As New DataView
                Dim SortExpression As String
                SortExpression = e.SortExpression
                'Commented by shubhrata dec 8th 2006 TimeOutProb in 3.2.0 Patch 1
                'g_dataset_dsDisbursements = viewstate("DS_Sort_VRManager")
                g_dataset_dsDisbursements = Session("dsDisbursementsByPersId")
                dv = g_dataset_dsDisbursements.Tables(0).DefaultView
                dv.Sort = SortExpression
                If Not Session("VRManager_Sort") Is Nothing Then
                    If Session("VRManager_Sort").ToString.Trim.EndsWith("ASC") Then
                        dv.Sort = SortExpression + " DESC"
                    Else
                        dv.Sort = SortExpression + " ASC"
                    End If
                Else
                    dv.Sort = SortExpression + " ASC"
                End If

                Me.Datagrid1.DataSource = Nothing
                Me.Datagrid1.DataSource = dv
                Me.Datagrid1.DataBind()
                Session("VRManager_Sort") = dv.Sort
            End If

        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Throw
        End Try
    End Sub


End Class

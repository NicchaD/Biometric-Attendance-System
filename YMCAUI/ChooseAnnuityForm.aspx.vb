'*******************************************************************************
' Copyright YMCA Retirement Fund All Rights Reserved. 
'
' Project Name		:	YMCA-YRS
' FileName			:	ChooseAnnuityForm.aspx.vb
' Author Name		:	Ruchi Saxena
' Employee ID		:	33494
' Email				:	ruchi.saxena@3i-infotech.com
' Contact No		:	8751
' Creation Time		:	5/17/2005 4:14:47 PM
' Program Specification Name	:	YMCA PS 3.5.doc
' Unit Test Plan Name			:	
' Description					:	Pop Up window provided to select the annuity
'********************************************************************************
'Cache-Session : Vipul 03Feb06 
'********************************************************************************
'********************************************************************************
'Modification History
'********************************************************************************
'Modified by                    Date                Description
'********************************************************************************
'Neeraj Singh                   12/Nov/2009         Added form name for security issue YRS 5.0-940 
'Priya                          2010.11.17          YRS 5.0-1215 : Exact age vs nearest age annuity calculations
'Sanket Vaidya                  2011.08.10          YRS 5.0-1329:J&S options available to non-spouse beneficiaries
'Manthan Rajguru                2015.09.16          YRS-AT-2550: YRS data cleanup: copyright comments and namespace/reference (Changed the Namespace/reference from 'Infotech' to 'YMCARET')
'******************************************************************************* 

Imports YMCARET.YmcaBusinessObject

Public Class ChooseAnnuityForm
    Inherits System.Web.UI.Page
    'below line is Added by Neeraj for issue id YRS 5.0-940 
    Dim strFormName As String = New String("ChooseAnnuityForm.aspx")
    'End issue id YRS 5.0-940

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents DataGridAnnuityOptions As System.Web.UI.WebControls.DataGrid
    Protected WithEvents TextBoxAnnuityDescription As System.Web.UI.WebControls.TextBox
    Protected WithEvents ButtonCancel As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonOK As System.Web.UI.WebControls.Button
    Protected WithEvents LabelSelectAnnuity As System.Web.UI.WebControls.Label
    Protected WithEvents LabelAnnuityDescription As System.Web.UI.WebControls.Label
    Protected WithEvents LabelHeadingForm As System.Web.UI.WebControls.Label
    Protected WithEvents DataGridAnnuityOptionsExactAgeEffDate As System.Web.UI.WebControls.DataGrid
    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object
    Protected WithEvents lblExactAgeGridMessage As System.Web.UI.WebControls.Label
    Protected WithEvents lblGridMessage As System.Web.UI.WebControls.Label
    Protected WithEvents LabelJAnnuityUnAvailMessage As System.Web.UI.WebControls.Label

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region
    Dim g_ds_BeforeSelectAnnuity As New DataSet
    Dim g_ds_ExactAgeSelectAnnuity As New DataSet

#Region "Events"
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load, Me.Load
        'Put user code to initialize the page here
        Try

            Me.LabelAnnuityDescription.AssociatedControlID = Me.TextBoxAnnuityDescription.ID
            If Not Me.IsPostBack Then
                If Session("CalledAnnuity") = "OLD" Then
                    If Session("SelectedPlan") = "R" Then
                        g_ds_BeforeSelectAnnuity = Session("ds_SelectAnnuityRetirement")
                    Else
                        g_ds_BeforeSelectAnnuity = Session("ds_SelectAnnuitySavings")
                    End If
                    FillBeforeExactAgeDatagrid()
                    lblExactAgeGridMessage.Visible = False
                    lblGridMessage.Visible = False
                    'FillOldCalculatedDatagrid()
                ElseIf Session("CalledAnnuity") = "NEW" Then
                    If Session("SelectedPlan") = "R" Then
                        g_ds_ExactAgeSelectAnnuity = Session("ds_SelectAnnuityRetirement_ExactAgeEffDate")
                    Else
                        g_ds_ExactAgeSelectAnnuity = Session("ds_SelectAnnuitySavings_ExactAgeEffDate")
                    End If

                    FillExactAgeDatagrid()
                    lblExactAgeGridMessage.Visible = False
                    lblGridMessage.Visible = False

                    'FillSecondGrid()
                ElseIf Session("CalledAnnuity") = "BOTH" Then
                    If Session("TransAfterExactAgeEffDate") = "1" Then

                        If Session("SelectedPlan") = "R" Then
                            g_ds_BeforeSelectAnnuity = Session("ds_SelectAnnuityRetirement")
                            g_ds_ExactAgeSelectAnnuity = Session("ds_SelectAnnuityRetirement_ExactAgeEffDate")
                        Else
                            g_ds_BeforeSelectAnnuity = Session("ds_SelectAnnuitySavings")
                            g_ds_ExactAgeSelectAnnuity = Session("ds_SelectAnnuitySavings_ExactAgeEffDate")
                        End If
                        FillBeforeExactAgeDatagrid()
                        FillExactAgeDatagrid()
                        'FillOldCalculatedDatagrid()
                        'FillSecondGrid()
                        lblExactAgeGridMessage.Visible = True
                        lblGridMessage.Visible = True
                    Else
                        If Session("SelectedPlan") = "R" Then
                            g_ds_BeforeSelectAnnuity = Session("ds_SelectAnnuityRet_ExactAgeEffDate_Combined")
                        ElseIf Session("SelectedPlan") = "S" Then
                            g_ds_BeforeSelectAnnuity = Session("ds_SelectAnnuitySavings_ExactAgeEffDate_Combined")
                        End If
                        FillCombinedExactage()
                        lblExactAgeGridMessage.Visible = False
                        lblGridMessage.Visible = False
                        'FillBeforeExactAgeDatagrid()
                        'FillExactAgeDatagrid()
                        'FillCombinedExactage()
                    End If
                End If
            End If

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    'Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    Response.Redirect("RetirementProcessingForm.aspx")
    'End Sub

    'Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    Response.Redirect("RetirementProcessingForm.aspx")
    'End Sub

    Private Sub ButtonOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonOK.Click
        Try
            Dim msg As String
            msg = ""

            If Session("CalledAnnuity") = "OLD" Then

                Session("AnnuitySelected") = True
                Session("Annuity_Type") = Me.DataGridAnnuityOptions.SelectedItem.Cells(1).Text.Trim
                Session("Annuity_Desc") = Me.TextBoxAnnuityDescription.Text.Trim
                Session("Amount") = Convert.ToDouble(Me.DataGridAnnuityOptions.SelectedItem.Cells(3).Text.Trim)
                Session("Taxable_Amount") = Convert.ToDouble(Me.DataGridAnnuityOptions.SelectedItem.Cells(3).Text.Trim) - Convert.ToDouble(Me.DataGridAnnuityOptions.SelectedItem.Cells(7).Text.Trim)
                Session("NonTaxable") = Convert.ToDouble(Me.DataGridAnnuityOptions.SelectedItem.Cells(7).Text.Trim)
                'Session("SelectedCalledAnnuity_Ret") = "OLD"

                If Session("SelectedPlan") = "R" Then
                    Session("SelectedCalledAnnuity_Ret") = "OLD"
                ElseIf Session("SelectedPlan") = "S" Then
                    Session("SelectedCalledAnnuity_Sav") = "OLD"
                End If

            ElseIf Session("CalledAnnuity") = "NEW" Then

                Session("AnnuitySelected") = True
                Session("Annuity_Type") = Me.DataGridAnnuityOptionsExactAgeEffDate.SelectedItem.Cells(1).Text.Trim
                Session("Annuity_Desc") = Me.TextBoxAnnuityDescription.Text.Trim
                Session("Amount") = Convert.ToDouble(Me.DataGridAnnuityOptionsExactAgeEffDate.SelectedItem.Cells(3).Text.Trim)
                Session("Taxable_Amount") = Convert.ToDouble(Me.DataGridAnnuityOptionsExactAgeEffDate.SelectedItem.Cells(3).Text.Trim) - Convert.ToDouble(Me.DataGridAnnuityOptionsExactAgeEffDate.SelectedItem.Cells(7).Text.Trim)
                Session("NonTaxable") = Convert.ToDouble(Me.DataGridAnnuityOptionsExactAgeEffDate.SelectedItem.Cells(7).Text.Trim)

                'Session("SelectedCalledAnnuity") = "NEW"

                If Session("SelectedPlan") = "R" Then
                    Session("SelectedCalledAnnuity_Ret") = "NEW"
                ElseIf Session("SelectedPlan") = "S" Then
                    Session("SelectedCalledAnnuity_Sav") = "NEW"
                End If

            ElseIf Session("CalledAnnuity") = "BOTH" Then

                If Session("TransAfterExactAgeEffDate") = "0" Then

                    Session("AnnuitySelected") = True
                    Session("Annuity_Type") = Me.DataGridAnnuityOptions.SelectedItem.Cells(1).Text.Trim
                    Session("Annuity_Desc") = Me.TextBoxAnnuityDescription.Text.Trim
                    Session("Amount") = Convert.ToDouble(Me.DataGridAnnuityOptions.SelectedItem.Cells(3).Text.Trim)
                    Session("Taxable_Amount") = Convert.ToDouble(Me.DataGridAnnuityOptions.SelectedItem.Cells(3).Text.Trim) - Convert.ToDouble(Me.DataGridAnnuityOptions.SelectedItem.Cells(7).Text.Trim)
                    Session("NonTaxable") = Convert.ToDouble(Me.DataGridAnnuityOptions.SelectedItem.Cells(7).Text.Trim)

                    'Session("SelectedCalledAnnuity") = Me.DataGridAnnuityOptions.SelectedItem.Cells(12).Text.Trim
                    If Session("SelectedPlan") = "R" Then
                        Session("SelectedCalledAnnuity_Ret") = Me.DataGridAnnuityOptions.SelectedItem.Cells(12).Text.Trim
                    ElseIf Session("SelectedPlan") = "S" Then
                        Session("SelectedCalledAnnuity_Sav") = Me.DataGridAnnuityOptions.SelectedItem.Cells(12).Text.Trim
                    End If

                Else
                    If DataGridAnnuityOptions.SelectedIndex <> -1 Then
                        Session("AnnuitySelected") = True
                        Session("Annuity_Type") = Me.DataGridAnnuityOptions.SelectedItem.Cells(1).Text.Trim
                        Session("Annuity_Desc") = Me.TextBoxAnnuityDescription.Text.Trim
                        Session("Amount") = Convert.ToDouble(Me.DataGridAnnuityOptions.SelectedItem.Cells(3).Text.Trim)
                        Session("Taxable_Amount") = Convert.ToDouble(Me.DataGridAnnuityOptions.SelectedItem.Cells(3).Text.Trim) - Convert.ToDouble(Me.DataGridAnnuityOptions.SelectedItem.Cells(7).Text.Trim)
                        Session("NonTaxable") = Convert.ToDouble(Me.DataGridAnnuityOptions.SelectedItem.Cells(7).Text.Trim)

                        'Session("SelectedCalledAnnuity") = "OLD"
                        If Session("SelectedPlan") = "R" Then
                            Session("SelectedCalledAnnuity_Ret") = "OLD"
                        ElseIf Session("SelectedPlan") = "S" Then
                            Session("SelectedCalledAnnuity_Sav") = "OLD"
                        End If

                    ElseIf DataGridAnnuityOptionsExactAgeEffDate.SelectedIndex <> -1 Then
                        Session("AnnuitySelected") = True
                        Session("Annuity_Type") = Me.DataGridAnnuityOptionsExactAgeEffDate.SelectedItem.Cells(1).Text.Trim
                        Session("Annuity_Desc") = Me.TextBoxAnnuityDescription.Text.Trim
                        Session("Amount") = Convert.ToDouble(Me.DataGridAnnuityOptionsExactAgeEffDate.SelectedItem.Cells(3).Text.Trim)
                        Session("Taxable_Amount") = Convert.ToDouble(Me.DataGridAnnuityOptionsExactAgeEffDate.SelectedItem.Cells(3).Text.Trim) - Convert.ToDouble(Me.DataGridAnnuityOptionsExactAgeEffDate.SelectedItem.Cells(7).Text.Trim)
                        Session("NonTaxable") = Convert.ToDouble(Me.DataGridAnnuityOptionsExactAgeEffDate.SelectedItem.Cells(7).Text.Trim)

                        'Session("SelectedCalledAnnuity") = "NEW"
                        If Session("SelectedPlan") = "R" Then
                            Session("SelectedCalledAnnuity_Ret") = "NEW"
                        ElseIf Session("SelectedPlan") = "S" Then
                            Session("SelectedCalledAnnuity_Sav") = "NEW"
                        End If

                    End If

                End If

            End If
            'If Session("SelectedCalledAnnuity") = "OLD" Then
            '    Session("AnnuitySelected") = True
            '    Session("Annuity_Type") = Me.DataGridAnnuityOptions.SelectedItem.Cells(1).Text.Trim
            '    Session("Annuity_Desc") = Me.TextBoxAnnuityDescription.Text.Trim
            '    Session("Amount") = Convert.ToDouble(Me.DataGridAnnuityOptions.SelectedItem.Cells(3).Text.Trim)
            '    Session("Taxable_Amount") = Convert.ToDouble(Me.DataGridAnnuityOptions.SelectedItem.Cells(3).Text.Trim) - Convert.ToDouble(Me.DataGridAnnuityOptions.SelectedItem.Cells(7).Text.Trim)
            '    Session("NonTaxable") = Convert.ToDouble(Me.DataGridAnnuityOptions.SelectedItem.Cells(7).Text.Trim)
            '    Session("SelectedCalledAnnuity") = "NEW"

            'ElseIf Session("SelectedCalledAnnuity") = "NEW" Then
            '    Session("AnnuitySelected") = True
            '    Session("Annuity_Type") = Me.DataGridAnnuityOptionsExactAgeEffDate.SelectedItem.Cells(1).Text.Trim
            '    Session("Annuity_Desc") = Me.TextBoxAnnuityDescription.Text.Trim
            '    Session("Amount") = Convert.ToDouble(Me.DataGridAnnuityOptionsExactAgeEffDate.SelectedItem.Cells(3).Text.Trim)
            '    Session("Taxable_Amount") = Convert.ToDouble(Me.DataGridAnnuityOptionsExactAgeEffDate.SelectedItem.Cells(3).Text.Trim) - Convert.ToDouble(Me.DataGridAnnuityOptionsExactAgeEffDate.SelectedItem.Cells(7).Text.Trim)
            '    Session("NonTaxable") = Convert.ToDouble(Me.DataGridAnnuityOptionsExactAgeEffDate.SelectedItem.Cells(7).Text.Trim)
            '    Session("SelectedCalledAnnuity") = "NEW"

            'End If

            'Session("AnnuitySelected") = True
            'Session("Annuity_Type") = Me.DataGridAnnuityOptions.SelectedItem.Cells(1).Text.Trim
            'Session("Annuity_Desc") = Me.TextBoxAnnuityDescription.Text.Trim
            'Session("Amount") = Convert.ToDouble(Me.DataGridAnnuityOptions.SelectedItem.Cells(3).Text.Trim)
            'Session("Taxable_Amount") = Convert.ToDouble(Me.DataGridAnnuityOptions.SelectedItem.Cells(3).Text.Trim) - Convert.ToDouble(Me.DataGridAnnuityOptions.SelectedItem.Cells(7).Text.Trim)
            'Session("NonTaxable") = Convert.ToDouble(Me.DataGridAnnuityOptions.SelectedItem.Cells(7).Text.Trim)


            'Session("SSIncrease") = Convert.ToDecimal(Me.DataGridAnnuityOptions.SelectedItem.Cells(10).Text.Trim)
            'Session("SSDecrease") = Convert.ToDecimal(Me.DataGridAnnuityOptions.SelectedItem.Cells(11).Text.Trim)

            'for alerting & restricting the user, if the user has made any changes and tried to purchase the annuity of a participant
            'used in RetirementProcessingForm.aspx.vb page.
            Session("RP_DataChanged") = True

            msg = msg + "<Script Language='JavaScript'>"

            'msg = msg + "window.opener.location.href=window.opener.location.href;"
            msg = msg + "window.opener.document.forms(0).submit();"

            msg = msg + "self.close();"

            msg = msg + "</Script>"
            Response.Write(msg)

        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Private Sub ButtonCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonCancel.Click
        Try
            Dim msg As String
            msg = ""

            msg = msg + "<Script Language='JavaScript'>"

            '''msg = msg + "window.opener.location.href=window.opener.location.href;"

            msg = msg + "window.close();"

            msg = msg + "</Script>"
            Response.Write(msg)

        Catch ex As Exception
            Throw ex
        End Try


    End Sub
#End Region

#Region "Old Calcuation Datagrid region"

    Private Sub FillCombinedExactage()
        Try

            Me.DataGridAnnuityOptions.DataSource = g_ds_BeforeSelectAnnuity
            Me.DataGridAnnuityOptions.DataBind()

            If g_ds_BeforeSelectAnnuity.Tables.Count > 0 Then
                If g_ds_BeforeSelectAnnuity.Tables(0).Rows.Count > 0 Then
                    'Me.DataGridAnnuityOptions.SelectedIndex = 0
                    'Me.TextBoxAnnuityDescription.Text = Me.DataGridAnnuityOptions.Items(0).Cells(2).Text.Trim

                    Dim selectedAnnuity As String = Session("SelectedAnnuity")
                    Dim selectedIndex As Integer = -1
                    Dim i As Integer

                    If Not DataGridAnnuityOptions Is Nothing Then

                        ' Get the Index of the previously selected Item.
                        For i = 0 To DataGridAnnuityOptions.Items.Count - 1
                            If Me.DataGridAnnuityOptions.Items(i).Cells(1).Text.Trim = selectedAnnuity Then
                                selectedIndex = i
                                Exit For
                            End If
                        Next

                        ' Then highlight that item in the grid. 
                        If (selectedIndex <> -1) Then

                            Me.DataGridAnnuityOptions.SelectedIndex = selectedIndex
                            Me.TextBoxAnnuityDescription.Text = Me.DataGridAnnuityOptions.Items(selectedIndex).Cells(2).Text.Trim
                            Me.setSelectedIcon("OLD")
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub FillBeforeExactAgeDatagrid()
        Try
            'g_ds_SelectAnnuity = Session("ds_SelectAnnuity")


            Me.DataGridAnnuityOptions.DataSource = g_ds_BeforeSelectAnnuity
            Me.DataGridAnnuityOptions.DataBind()

            If g_ds_BeforeSelectAnnuity.Tables.Count > 0 Then
                If g_ds_BeforeSelectAnnuity.Tables(0).Rows.Count > 0 Then
                    'Me.DataGridAnnuityOptions.SelectedIndex = 0
                    'Me.TextBoxAnnuityDescription.Text = Me.DataGridAnnuityOptions.Items(0).Cells(2).Text.Trim

                    Dim selectedAnnuity As String = Session("SelectedAnnuity")
                    Dim selectedIndex As Integer = -1
                    Dim i As Integer

                    Dim strSelectedCalledAnnuty As String
                    If Session("CalledAnnuity") = "BOTH" Then
                        If Session("SelectedPlan") = "R" Then
                            strSelectedCalledAnnuty = Session("SelectedCalledAnnuity_Ret")
                        ElseIf Session("SelectedPlan") = "S" Then
                            strSelectedCalledAnnuty = Session("SelectedCalledAnnuity_Sav")
                        End If
                    Else
                        strSelectedCalledAnnuty = "OLD"
                    End If
                    If Not DataGridAnnuityOptions Is Nothing Then
                        If Not strSelectedCalledAnnuty Is Nothing Then
                            If strSelectedCalledAnnuty = "OLD" Then
                                ' Get the Index of the previously selected Item.
                                For i = 0 To DataGridAnnuityOptions.Items.Count - 1
                                    If Me.DataGridAnnuityOptions.Items(i).Cells(1).Text.Trim = selectedAnnuity Then
                                        selectedIndex = i
                                        Exit For
                                    End If
                                Next
                            End If
                        End If
                        ' Then highlight that item in the grid. 
                        If (selectedIndex <> -1) Then
                            Me.DataGridAnnuityOptions.SelectedIndex = selectedIndex
                            Me.TextBoxAnnuityDescription.Text = Me.DataGridAnnuityOptions.Items(selectedIndex).Cells(2).Text.Trim
                            Me.setSelectedIcon("OLD")
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub DataGridAnnuityOptions_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DataGridAnnuityOptions.ItemDataBound
        Try
            'YRS 5.0-1329:J&S options available to non-spouse beneficiaries
            Dim annuityAmt As String = e.Item.Cells(3).Text

            If (annuityAmt = RetirementBOClass.JSAnnuityUnAvailableValue) Then
                Dim l_button_select As ImageButton
                l_button_select = e.Item.Cells(0).FindControl("ImgBtnSelectAnnuityType")
                l_button_select.Enabled = False
                'Ashish:2011.10.05 YRS 5.0-1329:J&S options available to non-spouse beneficiaries display message
                LabelJAnnuityUnAvailMessage.Visible = True
            End If
            'End

            e.Item.Cells(2).Visible = False
            e.Item.Cells(4).Visible = False
            e.Item.Cells(5).Visible = False
            e.Item.Cells(6).Visible = False
            e.Item.Cells(7).Visible = False
            e.Item.Cells(8).Visible = False
            e.Item.Cells(9).Visible = False
            e.Item.Cells(10).Visible = False
            e.Item.Cells(11).Visible = False
            If Session("CalledAnnuity") = "BOTH" Then
                If Session("TransAfterExactAgeEffDate") = "0" Then
                    e.Item.Cells(12).Visible = False
                End If
            End If

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub DataGridAnnuityOptions_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DataGridAnnuityOptions.SelectedIndexChanged
        Me.setSelectedIcon("OLD")
    End Sub
#End Region

#Region "Common function "
    Private Sub setSelectedIcon(ByVal strDatagridName)
        Try
            Dim l_button_select As ImageButton
            Dim i As Integer

            If strDatagridName = "OLD" Then
                If Not DataGridAnnuityOptionsExactAgeEffDate Is Nothing Then
                    If (DataGridAnnuityOptionsExactAgeEffDate.SelectedIndex <> -1) Then
                        CType(Me.DataGridAnnuityOptionsExactAgeEffDate.Items(DataGridAnnuityOptionsExactAgeEffDate.SelectedIndex).FindControl("ImgBtnNewSelectAnnuityType"), ImageButton).ImageUrl = "images\select.gif"
                        DataGridAnnuityOptionsExactAgeEffDate.SelectedIndex = -1
                    End If
                End If

                If Not DataGridAnnuityOptions Is Nothing Then
                    For i = 0 To Me.DataGridAnnuityOptions.Items.Count - 1
                        'DataGridAnnuityOptionsExactAgeEffDate.SelectedIndex = -1
                        If i = Me.DataGridAnnuityOptions.SelectedIndex Then
                            l_button_select = Me.DataGridAnnuityOptions.Items(i).FindControl("ImgBtnSelectAnnuityType")
                            l_button_select.ImageUrl = "images\selected.gif"
                        Else
                            l_button_select = Me.DataGridAnnuityOptions.Items(i).FindControl("ImgBtnSelectAnnuityType")
                            l_button_select.ImageUrl = "images\select.gif"
                        End If
                    Next
                    Me.TextBoxAnnuityDescription.Text = Me.DataGridAnnuityOptions.SelectedItem.Cells(2).Text.Trim
                End If

            ElseIf strDatagridName = "NEW" Then
                If Not DataGridAnnuityOptions Is Nothing Then
                    If (DataGridAnnuityOptions.SelectedIndex <> -1) Then
                        CType(Me.DataGridAnnuityOptions.Items(DataGridAnnuityOptions.SelectedIndex).FindControl("ImgBtnSelectAnnuityType"), ImageButton).ImageUrl = "images\select.gif"
                        DataGridAnnuityOptions.SelectedIndex = -1
                        'l_button_select.ImageUrl = "images\select.gif"
                    End If
                End If

                If Not DataGridAnnuityOptionsExactAgeEffDate Is Nothing Then
                    For i = 0 To Me.DataGridAnnuityOptionsExactAgeEffDate.Items.Count - 1
                        If i = Me.DataGridAnnuityOptionsExactAgeEffDate.SelectedIndex Then
                            l_button_select = Me.DataGridAnnuityOptionsExactAgeEffDate.Items(i).FindControl("ImgBtnNewSelectAnnuityType")
                            l_button_select.ImageUrl = "images\selected.gif"
                        Else
                            l_button_select = Me.DataGridAnnuityOptionsExactAgeEffDate.Items(i).FindControl("ImgBtnNewSelectAnnuityType")
                            l_button_select.ImageUrl = "images\select.gif"
                        End If
                    Next
                    Me.TextBoxAnnuityDescription.Text = Me.DataGridAnnuityOptionsExactAgeEffDate.SelectedItem.Cells(2).Text.Trim
                End If
            End If


        Catch ex As Exception
            Throw ex
        End Try
    End Sub
#End Region

#Region "New Calculation datagrid "

    'Priya 2010.11.17 YRS 5.0-1215 : Exact age vs nearest age annuity calculations.
    Private Sub FillExactAgeDatagrid()
        Try

        
        Me.DataGridAnnuityOptionsExactAgeEffDate.DataSource = g_ds_ExactAgeSelectAnnuity
        Me.DataGridAnnuityOptionsExactAgeEffDate.DataBind()

        If g_ds_ExactAgeSelectAnnuity.Tables.Count > 0 Then
            If g_ds_ExactAgeSelectAnnuity.Tables(0).Rows.Count > 0 Then
                'Me.DataGridAnnuityOptions.SelectedIndex = 0
                'Me.TextBoxAnnuityDescription.Text = Me.DataGridAnnuityOptions.Items(0).Cells(2).Text.Trim

                Dim selectedAnnuity As String = Session("SelectedAnnuity")
                Dim selectedIndex As Integer = -1
                    Dim i As Integer

                    Dim strSelectedCalledAnnuty As String

                    If Session("CalledAnnuity") = "BOTH" Then
                        If Session("SelectedPlan") = "R" Then
                            strSelectedCalledAnnuty = Session("SelectedCalledAnnuity_Ret")
                        ElseIf Session("SelectedPlan") = "S" Then
                            strSelectedCalledAnnuty = Session("SelectedCalledAnnuity_Sav")
                        End If
                    Else
                        strSelectedCalledAnnuty = "NEW"
                    End If
                    If Not DataGridAnnuityOptionsExactAgeEffDate Is Nothing Then

                        If Not strSelectedCalledAnnuty Is Nothing Then
                            If strSelectedCalledAnnuty = "NEW" Then
                                ' Get the Index of the previously selected Item.
                                For i = 0 To DataGridAnnuityOptionsExactAgeEffDate.Items.Count - 1
                                    If Me.DataGridAnnuityOptionsExactAgeEffDate.Items(i).Cells(1).Text.Trim = selectedAnnuity Then
                                        selectedIndex = i
                                        Exit For
                                    End If
                                Next
                            End If
                        End If

                        '' Then highlight that item in the grid.
                        If (selectedIndex <> -1) Then

                            Me.DataGridAnnuityOptionsExactAgeEffDate.SelectedIndex = selectedIndex
                            Me.TextBoxAnnuityDescription.Text = Me.DataGridAnnuityOptionsExactAgeEffDate.Items(selectedIndex).Cells(2).Text.Trim
                            Me.setSelectedIcon("NEW")
                        End If
                    End If
                End If
            End If
            'DataGridNewAnnuityOptions()
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    'End Priya 2010.11.17 YRS 5.0-1215 : Exact age vs nearest age annuity calculations

    Private Sub DataGridAnnuityOptionsExactAgeEffDate_ItemDataBound1(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DataGridAnnuityOptionsExactAgeEffDate.ItemDataBound
        Try
            'YRS 5.0-1329:J&S options available to non-spouse beneficiaries
            Dim annuityAmt As String = e.Item.Cells(3).Text
            If (annuityAmt = RetirementBOClass.JSAnnuityUnAvailableValue) Then
                Dim l_button_select As ImageButton
                l_button_select = e.Item.Cells(0).FindControl("ImgBtnNewSelectAnnuityType")
                l_button_select.Enabled = False
                'Ashish:2011.10.05 YRS 5.0-1329:J&S options available to non-spouse beneficiaries display message
                LabelJAnnuityUnAvailMessage.Visible = True
            End If
            'End

            e.Item.Cells(2).Visible = False
            e.Item.Cells(4).Visible = False
            e.Item.Cells(5).Visible = False
            e.Item.Cells(6).Visible = False
            e.Item.Cells(7).Visible = False
            e.Item.Cells(8).Visible = False
            e.Item.Cells(9).Visible = False
            e.Item.Cells(10).Visible = False
            e.Item.Cells(11).Visible = False
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub DataGridAnnuityOptionsExactAgeEffDate_SelectedIndexChanged1(ByVal sender As Object, ByVal e As System.EventArgs) Handles DataGridAnnuityOptionsExactAgeEffDate.SelectedIndexChanged
        Me.setSelectedIcon("NEW")
    End Sub

#End Region
End Class

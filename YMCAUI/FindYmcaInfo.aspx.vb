'************************************************************************************************************************
' Author: Pramod Prakash Pokale
' Created on: 09/13/2017
' Summary of Functionality: Provides YMCA details for the given search term
' Declared in Version: 20.4.0 | YRS-AT-3665 -  YRS enh: Data Corrections Tool - Admin screen option to create a manual credit 
'
'************************************************************************************************************************
' REVISION HISTORY:
' ------------------------------------------------------------------------------------------------------
' Developer Name              | Date         | Version No      | Ticket
' ------------------------------------------------------------------------------------------------------
' 			                  | 	         |		           | 
' ------------------------------------------------------------------------------------------------------
'************************************************************************************************************************

Imports System.Data
Imports System.Data.SqlClient

Public Class FindYmcaInfo
    Inherits System.Web.UI.Page

    Public Property FindYmcaInfoSession() As SessionManager.SessionFindYmcaInfo
        Get
            If Not Session("FindYmcaInfoSession") Is Nothing Then
                Return TryCast(Session("FindYmcaInfoSession"), SessionManager.SessionFindYmcaInfo)
            Else
                Return Nothing
            End If
        End Get
        Set(ByVal value As SessionManager.SessionFindYmcaInfo)
            Session("FindYmcaInfoSession") = value
        End Set
    End Property

    Public Property FormName() As String
        Get
            If Not ViewState("FormName") Is Nothing Then
                Return Convert.ToString(ViewState("FormName"))
            Else
                Return String.Empty
            End If
        End Get
        Set(ByVal value As String)
            ViewState("FormName") = value
        End Set
    End Property

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim sessionDetails As SessionManager.SessionFindYmcaInfo
        Dim buttonSelect As ImageButton
        Try
            If Session("LoggedUserKey") Is Nothing Then
                Response.Redirect("Login.aspx", False)
            ElseIf Not IsPostBack Then
                sessionDetails = Me.FindYmcaInfoSession
                If sessionDetails Is Nothing Then
                    sessionDetails = New SessionManager.SessionFindYmcaInfo()
                End If

                If Not Session("Page") Is Nothing Then
                    txtYmcaNo.Text = sessionDetails.YmcaNo
                    txtName.Text = sessionDetails.Name
                    txtCity.Text = sessionDetails.City
                    txtState.Text = sessionDetails.State

                    Me.gvFindYmcaInfo.SelectedIndex = sessionDetails.GridIndex

                    PopulateData(sessionDetails)

                    buttonSelect = TryCast(Me.gvFindYmcaInfo.SelectedRow.Cells(0).Controls(0), ImageButton)
                    buttonSelect.ImageUrl = "images\selected.gif"
                    Session("Page") = Nothing
                    sessionDetails = Nothing
                Else
                    sessionDetails.PageIndex = -1
                    sessionDetails.GVCustomSort = Nothing
                    sessionDetails.GridIndex = -1
                End If

                Me.FormName = Convert.ToString(Request.QueryString.Get("Name"))

                If (Me.FormName = "DCToolsAddYmcaCredit") Then
                    Me.lblYmcaNo.Visible = True
                    Me.txtYmcaNo.Visible = True
                    Me.lblName.Visible = True
                    Me.txtName.Visible = True
                    Me.lblCity.Visible = True
                    Me.txtCity.Visible = True
                    Me.lblState.Visible = True
                    Me.txtState.Visible = True
                End If

                Me.FindYmcaInfoSession = sessionDetails
            End If
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("FindYmcaInfo_Page_Load", ex)
        Finally
            buttonSelect = Nothing
            sessionDetails = Nothing
        End Try
    End Sub

    Private Sub PopulateData(ByVal sessionDetails As SessionManager.SessionFindYmcaInfo)
        Dim dvFindYmcaInfo As DataView
        Dim sorting As GridViewCustomSort
        Dim searchResult As DataSet
        Try
            searchResult = sessionDetails.SearchResult

            If Not searchResult Is Nothing Then
                If Me.gvFindYmcaInfo.PageIndex >= sessionDetails.PageCount And sessionDetails.PageCount <> 0 Then
                    Exit Sub
                ElseIf (searchResult.Tables("YMCA List").Rows.Count > 0) Then
                    LabelNoDataFound.Visible = False
                    Me.gvFindYmcaInfo.Visible = True

                    Me.gvFindYmcaInfo.AllowPaging = True
                    Me.gvFindYmcaInfo.SelectedIndex = -1
                    Me.gvFindYmcaInfo.PageIndex = 0
                    Me.gvFindYmcaInfo.PageSize = 25
                    dvFindYmcaInfo = searchResult.Tables(0).DefaultView

                    If sessionDetails.GVCustomSort IsNot Nothing Then
                        sorting = sessionDetails.GVCustomSort
                        dvFindYmcaInfo.Sort = sorting.SortExpression + " " + sorting.SortDirection
                    End If
                    If sessionDetails.PageIndex >= 0 Then
                        Me.gvFindYmcaInfo.PageIndex = sessionDetails.PageIndex
                    End If
                    Me.gvFindYmcaInfo.DataSource = dvFindYmcaInfo
                    Me.gvFindYmcaInfo.DataBind()
                Else
                    sessionDetails.PageIndex = -1
                    sessionDetails.GVCustomSort = Nothing
                    sessionDetails.GridIndex = -1

                    Me.gvFindYmcaInfo.DataSource = Nothing
                    Me.gvFindYmcaInfo.DataBind()

                    LabelNoDataFound.Visible = True
                    Me.gvFindYmcaInfo.Visible = False
                End If
            Else
                LabelNoDataFound.Visible = True
                Me.gvFindYmcaInfo.Visible = False
            End If
        Catch ex As SqlException
            LabelNoDataFound.Visible = True
            Me.gvFindYmcaInfo.Visible = False
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("FindYmcaInfo_PopulateData", ex)
        Finally
            searchResult = Nothing
            dvFindYmcaInfo = Nothing
            sorting = Nothing
        End Try
    End Sub

    Private Sub btnFind_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFind.Click
        Dim sessionDetails As SessionManager.SessionFindYmcaInfo
        Try
            Me.FindYmcaInfoSession = Nothing

            If Request.QueryString.Get("Name") = "DCToolsAddYmcaCredit" Then
                If Me.txtYmcaNo.Text = String.Empty And Me.txtName.Text = String.Empty And Me.txtCity.Text = String.Empty And Me.txtState.Text = String.Empty Then
                    HelperFunctions.ShowMessageToUser("This search will take a longer time. It is suggested that you enter a search criteria.", EnumMessageTypes.Error)
                Else
                    sessionDetails = New SessionManager.SessionFindYmcaInfo

                    sessionDetails.YmcaNo = txtYmcaNo.Text.Trim()
                    sessionDetails.Name = txtName.Text.Trim()
                    sessionDetails.City = txtCity.Text.Trim()
                    sessionDetails.State = txtState.Text.Trim()

                    sessionDetails.SearchResult = YMCARET.YmcaBusinessObject.YMCABOClass.SearchYMCAList(sessionDetails.YmcaNo, sessionDetails.Name, sessionDetails.City, sessionDetails.State)
                    PopulateData(sessionDetails)
                    Me.FindYmcaInfoSession = sessionDetails
                End If
            End If
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("FindYmcaInfo_btnFind_Click", ex)
        Finally
            sessionDetails = Nothing
        End Try
    End Sub

    Private Sub gvFindYmcaInfo_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvFindYmcaInfo.PageIndexChanging
        Dim sessionDetails As SessionManager.SessionFindYmcaInfo
        Dim dv As DataView
        Dim sorting As GridViewCustomSort
        Dim searchResult As DataSet
        Try
            sessionDetails = Me.FindYmcaInfoSession
            If sessionDetails Is Nothing Then
                sessionDetails = New SessionManager.SessionFindYmcaInfo()
            End If

            Me.gvFindYmcaInfo.SelectedIndex = -1
            Me.gvFindYmcaInfo.PageIndex = e.NewPageIndex
            sessionDetails.PageIndex = e.NewPageIndex
            Try
                searchResult = sessionDetails.SearchResult
                If HelperFunctions.isNonEmpty(searchResult) Then
                    dv = searchResult.Tables(0).DefaultView
                    If Not sessionDetails.GVCustomSort Is Nothing Then
                        sorting = sessionDetails.GVCustomSort
                        dv.Sort = String.Format("{0} {1}", sorting.SortExpression, sorting.SortDirection)
                    End If
                    If sessionDetails.PageIndex >= 0 Then
                        Me.gvFindYmcaInfo.PageIndex = sessionDetails.PageIndex
                    End If
                    gvFindYmcaInfo.DataSource = dv
                    gvFindYmcaInfo.DataBind()
                End If
            Catch ex As Exception
                HelperFunctions.LogException("FindInfo_gvFindYmcaInfo_pageIndexing", ex)
                HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            Finally
                dv = Nothing
            End Try
            Me.FindYmcaInfoSession = sessionDetails
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("FindYmcaInfo_gvFindYmcaInfo_PageIndexChanging", ex)
        Finally
            searchResult = Nothing
            sorting = Nothing
            dv = Nothing
            sessionDetails = Nothing
        End Try
    End Sub

    Private Sub gvFindYmcaInfo_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvFindYmcaInfo.SelectedIndexChanged
        Dim sessionDetails As SessionManager.SessionFindYmcaInfo
        Dim selectedImageButton As ImageButton
        Dim counter As Integer
        Try
            sessionDetails = Me.FindYmcaInfoSession
            If sessionDetails Is Nothing Then
                sessionDetails = New SessionManager.SessionFindYmcaInfo()
            End If

            sessionDetails.YmcaNo = txtYmcaNo.Text.Trim()
            sessionDetails.Name = txtName.Text.Trim()
            sessionDetails.City = txtCity.Text.Trim()
            sessionDetails.State = txtState.Text.Trim()
            sessionDetails.GridIndex = Me.gvFindYmcaInfo.SelectedIndex

            While counter < Me.gvFindYmcaInfo.Rows.Count
                selectedImageButton = TryCast(Me.gvFindYmcaInfo.Rows(counter).Cells(0).Controls(0), ImageButton)
                If counter = Me.gvFindYmcaInfo.SelectedIndex Then
                    If Not selectedImageButton Is Nothing Then
                        selectedImageButton.ImageUrl = "images\selected.gif"
                    End If
                Else
                    If Not selectedImageButton Is Nothing Then
                        selectedImageButton.ImageUrl = "images\select.gif"
                    End If
                End If
                counter = counter + 1
            End While

            If Me.FormName = "DCToolsAddYmcaCredit" Then
                Session("DCYmcaIDForAddYmcaCredit") = Me.gvFindYmcaInfo.SelectedRow.Cells(1).Text
                Response.Redirect("DCToolsAddYMCACredit.aspx", False)
            End If
            Me.FindYmcaInfoSession = sessionDetails
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("FindYmcaInfo_gvFindYmcaInfo_SelectedIndexChanged", ex)
        Finally
            selectedImageButton = Nothing
            sessionDetails = Nothing
        End Try
    End Sub

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Try
            ClearSession()
            Response.Redirect("MainWebForm.aspx", False)
        Catch ex As Exception
            HelperFunctions.LogException("FindYmcaInfo_btnClose_Click", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
        End Try
    End Sub

    Private Sub gvFindYmcaInfo_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvFindYmcaInfo.RowDataBound
        Dim sessionDetails As SessionManager.SessionFindYmcaInfo
        Try
            sessionDetails = Me.FindYmcaInfoSession
            If sessionDetails Is Nothing Then
                sessionDetails = New SessionManager.SessionFindYmcaInfo()
            End If

            HelperFunctions.SetSortingArrows(sessionDetails.GVCustomSort, e)
            If e.Row.RowType = DataControlRowType.Header Or e.Row.RowType = DataControlRowType.DataRow Then
                Select Case Request.QueryString.Get("Name")
                    Case "DCToolsAddYmcaCredit"
                        e.Row.Cells(1).Visible = False
                        e.Row.Cells(2).Visible = False

                        If e.Row.RowType = DataControlRowType.Header Then
                            DirectCast(e.Row.Cells(3).Controls(0), System.Web.UI.WebControls.LinkButton).Text = "YMCA No."
                            DirectCast(e.Row.Cells(4).Controls(0), System.Web.UI.WebControls.LinkButton).Text = "YMCA Name"
                            DirectCast(e.Row.Cells(5).Controls(0), System.Web.UI.WebControls.LinkButton).Text = "City"
                            DirectCast(e.Row.Cells(6).Controls(0), System.Web.UI.WebControls.LinkButton).Text = "State"
                            DirectCast(e.Row.Cells(7).Controls(0), System.Web.UI.WebControls.LinkButton).Text = "Type"
                        End If
                    Case Else
                        e.Row.Cells(1).Visible = False
                        e.Row.Cells(2).Visible = False
                End Select
            End If
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("FindYmcaInfo_gvFindYmcaInfo_RowDataBound", ex)
        Finally
            sessionDetails = Nothing
        End Try
    End Sub

    Private Sub gvFindYmcaInfo_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvFindYmcaInfo.Sorting
        Dim sessionDetails As SessionManager.SessionFindYmcaInfo
        Dim dv As New DataView
        Dim sortExpression As String
        Dim searchResult As DataSet
        Try
            sessionDetails = Me.FindYmcaInfoSession
            If sessionDetails Is Nothing Then
                sessionDetails = New SessionManager.SessionFindYmcaInfo()
            End If

            Me.gvFindYmcaInfo.SelectedIndex = -1
            If Not sessionDetails.SearchResult Is Nothing Then
                sortExpression = e.SortExpression
                searchResult = sessionDetails.SearchResult
                dv = searchResult.Tables(0).DefaultView
                dv.Sort = sortExpression
                HelperFunctions.gvSorting(sessionDetails.GVCustomSort, e.SortExpression, dv)
                If sessionDetails.PageIndex >= 0 Then
                    Me.gvFindYmcaInfo.PageIndex = sessionDetails.PageIndex
                End If
                Me.gvFindYmcaInfo.DataSource = Nothing
                Me.gvFindYmcaInfo.DataSource = dv
                Me.gvFindYmcaInfo.DataBind()
            End If
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("FindYmcaInfo_gvFindYmcaInfo_SortCommand", ex)
        Finally
            searchResult = Nothing
            dv = Nothing
            sortExpression = Nothing
            sessionDetails = Nothing
        End Try
    End Sub

    Private Sub ClearSession()
        Me.FindYmcaInfoSession = Nothing
        Session("Page") = Nothing
    End Sub
End Class
'*******************************************************************************
' Copyright YMCA Retirement Fund All Rights Reserved. 
' Project Name		:	YMCA-YRS
' FileName			:	UserSession.aspx.vb
'*******************************************************************************

'Modification History
'****************************************************************************************************************************************************
'Modified By        Date            Description
'****************************************************************************************************************************************************
'Anudeep            29.07.2013      Bt-1983:Security rights not applied for UserSession.aspx
'Anudeep A          24.09.2014      BT:2625 :YRS 5.0-2405-Consistent screen header sections 
'Manthan Rajguru    2015.09.16      YRS-AT-2550: YRS data cleanup: copyright comments and namespace/reference (Changed the Namespace/reference from 'Infotech' to 'YMCARET')
'****************************************************************************************************************************************************
Public Class UserSession
    Inherits System.Web.UI.Page
    'Public Property ViewState_SortExpression() As String
    '    Get
    '        If ViewState("SortExpression") Is Nothing Then
    '            ViewState("SortExpression") = ""
    '        End If

    '        Return Convert.ToString(ViewState("SortExpression"))
    '    End Get
    '    Set(ByVal value As String)
    '        ViewState("SortExpression") = value
    '    End Set
    'End Property
    'Anudeep:29.07.2013 - Bt-1983:Security rights not applied for UserSession.aspx
    Public Property dtGetSessionInfo As DataTable
        Get
            Return ViewState("dtGetSessionInfo")
        End Get
        Set(ByVal value As DataTable)
            ViewState("dtGetSessionInfo") = value
        End Set
    End Property


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                'Me.Menu1.DataSource = Server.MapPath("SimpleXML.xml")
                'Me.Menu1.DataBind()
                If Session("LoggedUserKey") Is Nothing Then
                    Response.Redirect("Login.aspx", False)
                End If

                Session("UserSessionListSort") = Nothing
                Session("UserSessionListPageIndex") = Nothing

                BindDropDown()
            End If
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("UserSession_Page_Load", ex)
        End Try
    End Sub

    Private Sub BindDropDown()
        Dim dsGetSessionInfo As DataTable
        Try
            dsGetSessionInfo = YMCARET.YmcaBusinessObject.Login.GetOnlineUsers().Tables(0)
            If Not dsGetSessionInfo Is Nothing Then
                ddlUsername.DataSource = dsGetSessionInfo
                ddlUsername.DataTextField = "Username"
                ddlUsername.DataValueField = "UserId"
                ddlUsername.DataBind()
                ddlUsername.Items.Insert(0, "-- Select --")
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnBack.Click
        Try
            Response.Redirect("AdminSessionManagement.aspx")
        Catch
            Throw
        End Try
    End Sub
    Protected Sub btnSearchSession_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSearchSession.Click
        Try
            'Anudeep:29.07.2013 - Bt-1983:Security rights not applied for UserSession.aspx
            If ddlUsername.SelectedValue = "-- Select --" Then
                HelperFunctions.ShowMessageToUser("Please select a user to view history.", EnumMessageTypes.Error)
            Else
                If (ddlUsername.SelectedValue > 0) Then
                    dtGetSessionInfo = YMCARET.YmcaBusinessObject.Login.GetOnlineUserInfo(ddlUsername.SelectedItem.Text.Trim).Tables(0)
                End If

                Session("UserSessionListSort") = Nothing
                Session("UserSessionPageIndex") = Nothing

                BindGrid()

                gvUserSession.PageIndex = 0
            End If
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("UserSession_btnSearchSession_Click", ex)
        End Try
    End Sub

    Private Sub gvUserSession_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvUserSession.RowDataBound
        Try
            HelperFunctions.SetSortingArrows(Session("UserSessionListSort"), e)
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("UserSession_gvUserSession_RowDataBound", ex)
        End Try
    End Sub

    Protected Sub gvUserSession_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvUserSession.Sorting
        Dim dv As New DataView
        Dim SortExpression As String
        SortExpression = e.SortExpression

        Try
            'Anudeep:29.07.2013 - Bt-1983:Security rights not applied for UserSession.aspx
            'dv = DirectCast((YMCARET.YmcaBusinessObject.Login.GetOnlineUserInfo(ddlUsername.SelectedItem.Text.Trim).Tables(0)), DataTable).DefaultView
            If dtGetSessionInfo Is Nothing Then
                Exit Sub
            End If
            dv = dtGetSessionInfo.DefaultView
            dv.Sort = SortExpression

            'If Not dv Is Nothing Then
            '    'dv = DirectCast(SessionDataSetdsMemberInfo, DataSet).Tables(0).DefaultView
            '    dv.Sort = SortExpression
            '    If Not ViewState_SortExpression Is Nothing Then
            '        If ViewState_SortExpression.ToString.Trim.EndsWith("ASC") Then
            '            dv.Sort = SortExpression + " DESC"
            '        Else
            '            dv.Sort = SortExpression + " ASC"
            '        End If
            '    Else
            '        dv.Sort = SortExpression + " ASC"
            '    End If
            '    gvUserSession.DataSource = Nothing
            '    gvUserSession.DataSource = dv
            '    gvUserSession.DataBind()
            '    ViewState_SortExpression = dv.Sort
            'End If

            HelperFunctions.gvSorting(Session("UserSessionListSort"), e.SortExpression, dv)
            BindGrid()

        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("UserSession_gvUserSession_Sorting", ex)

        Finally

        End Try
    End Sub

    Protected Sub gvUserSession_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvUserSession.PageIndexChanging
        Try
            Session("UserSessionPageIndex") = e.NewPageIndex
            gvUserSession.PageIndex = e.NewPageIndex
            BindGrid()
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("UserSession_gvUserSession_PageIndexChanging", ex)
        End Try
        
    End Sub

    Public Sub BindGrid()
        'Anudeep:29.07.2013 - Bt-1983:Security rights not applied for UserSession.aspx
        'Dim dsGetSessionInfo As DataTable
        Dim dv As New DataView
        Dim Sorting As GridViewCustomSort
        Try


            If Not dtGetSessionInfo Is Nothing Then
                dv = dtGetSessionInfo.DefaultView
            End If

            If Session("UserSessionListSort") IsNot Nothing Then
                Sorting = Session("UserSessionListSort")
                dv.Sort = Sorting.SortExpression + " " + Sorting.SortDirection
            End If

            If Session("UserSessionListSort") IsNot Nothing Then
                gvUserSession.PageIndex = Session("UserSessionPageIndex")
            End If

            HelperFunctions.BindGrid(gvUserSession, dv, True)

        Catch
            Throw
        End Try
    End Sub
End Class
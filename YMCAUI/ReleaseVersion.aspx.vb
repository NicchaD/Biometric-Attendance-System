'*******************************************************************************
' Copyright YMCA Retirement Fund All Rights Reserved. 
' Project Name		:	YMCA-YRS
' FileName			:	ReleaseVersion.aspx.vb
'*******************************************************************************

'Modification History
'****************************************************************************************************************************************************
'Modified By        Date            Description
'****************************************************************************************************************************************************
'Anudeep            29.07.2013      Bt-1987:Release Version History - Sorting not maintained while paging.
'Anudeep A          24.09.2014      BT:2625 :YRS 5.0-2405-Consistent screen header sections
'Manthan Rajguru    2015.09.16      YRS-AT-2550: YRS data cleanup: copyright comments and namespace/reference (Changed the Namespace/reference from 'Infotech' to 'YMCARET')
'****************************************************************************************************************************************************
Public Class ReleaseVersion
    Inherits System.Web.UI.Page
    Public Property ViewState_SortExpression() As String
        Get
            If ViewState("SortExpression") Is Nothing Then
                'ViewState("SortExpression") = False
                ViewState("SortExpression") = ""
            End If

            Return Convert.ToString(ViewState("SortExpression"))
        End Get
        Set(ByVal value As String)
            ViewState("SortExpression") = value
        End Set
    End Property
    'Anudeep:29.7.2013 - Bt-1987:Release Version History - Sorting not maintained while paging.
    Public Property dsGetPatchReleaseVersion As DataSet
        Get
            Return ViewState("dsGetPatchReleaseVersion")
        End Get
        Set(ByVal value As DataSet)
            ViewState("dsGetPatchReleaseVersion") = value
        End Set
    End Property
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                If Session("LoggedUserKey") Is Nothing Then
                    Response.Redirect("Login.aspx", False)
                End If

                Session("ReleaseVersionListSort") = Nothing
                Session("ReleaseVersionListPageIndex") = Nothing

                BindGrid()
            End If
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
        End Try
    End Sub

    Private Sub BindGrid()
        'Anudeep:29.7.2013 - Bt-1987:Release Version History - Sorting not maintained while paging.
        'Dim dsGetPatchReleaseVersion As DataSet
        Dim dv As DataView
        Dim Sorting As GridViewCustomSort
        'Try
        If dsGetPatchReleaseVersion Is Nothing Then
            dsGetPatchReleaseVersion = YMCARET.YmcaBusinessObject.Login.GetPatchRelaseVersions()
        End If

        If dsGetPatchReleaseVersion IsNot Nothing Then
            dv = dsGetPatchReleaseVersion.Tables(0).DefaultView
        End If

        If Session("ReleaseVersionListSort") IsNot Nothing Then
            Sorting = Session("ReleaseVersionListSort")
            dv.Sort = Sorting.SortExpression + " " + Sorting.SortDirection
        End If

        If Session("ReleaseVersionListSort") IsNot Nothing Then
            gvReleaseVersion.PageIndex = Session("ReleaseVersionPageIndex")
        End If

        HelperFunctions.BindGrid(gvReleaseVersion, dv, True)

        'Catch ex As Exception
        '    Throw ex
        'End Try
    End Sub

    Protected Sub btnOK_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnOK.Click
        Response.Redirect("MainWebForm.aspx", False)
    End Sub

    Protected Sub gvReleaseVersion_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvReleaseVersion.PageIndexChanging
        Try
            gvReleaseVersion.PageIndex = e.NewPageIndex
            Session("ReleaseVersionPageIndex") = e.NewPageIndex
            BindGrid()
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
        End Try
    End Sub

    Private Sub gvReleaseVersion_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvReleaseVersion.RowDataBound
        HelperFunctions.SetSortingArrows(Session("ReleaseVersionListSort"), e)
    End Sub

    Protected Sub gvReleaseVersion_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvReleaseVersion.Sorting
        Dim dv As New DataView
        Dim SortExpression As String
        Try
            'Anudeep:29.7.2013 - Bt-1987:Release Version History - Sorting not maintained while paging.
            If HelperFunctions.isNonEmpty(dsGetPatchReleaseVersion) Then
                dv = dsGetPatchReleaseVersion.Tables(0).DefaultView
            End If

        
            SortExpression = e.SortExpression
            dv.Sort = SortExpression
            HelperFunctions.gvSorting(Session("ReleaseVersionListSort"), e.SortExpression, dv)

            BindGrid()

            '*********************

        Catch ex As Exception
            'Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message), False)
            'Throw ex
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
        End Try
    End Sub
End Class
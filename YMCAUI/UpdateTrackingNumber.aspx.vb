
'*******************************************************************************
' Copyright YMCA Retirement Fund All Rights Reserved. 
'
' Project Name		:	YMCA_YRS
' FileName			:	UpdateTrackingNumber.aspx.vb
' Author Name		:	Shashi Shekhar Singh
' Employee ID		:	51426
' Email				:	shashi.singh@3i-infotech.com
' Contact No		:	8684
' Creation Time		:	05/18/2011 12:40:52 PM
' Program Specification Name	:	YRS-PS-RefundTracking.doc 
' Unit Test Plan Name			:	
' Description					:Screen	to update the Tracking No record for Refund request having status 'DISB'
'
'********************************************************************************************************************************
'Modification History
'********************************************************************************************************************************
'Modified By        Date            Description
'********************************************************************************************************************************
'Shashi             26-MAy-2011     Disable sorting when user editing the record.
'Prasad Jadhav      25-Nov-2011     YRS 5.0-1453:Error message if no records returned
'Manthan Rajguru    2015.09.16      YRS-AT-2550: YRS data cleanup: copyright comments and namespace/reference (Changed the Namespace/reference from 'Infotech' to 'YMCARET')
'Pooja K                        2019.28.02          YRS-AT-4248 - YRS enh: YRS read-only access during MonthEnd Processing (TrackIT 35547)
'********************************************************************************************************************************

#Region "Namespaces"
Imports System.Data
Imports System.Data.SqlClient
Imports System.Web
Imports System.Web.SessionState
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports System.Web.UI.WebControls
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
#End Region
Public Class UpdateTrackingNumber
    Inherits System.Web.UI.Page
    Protected WithEvents PlaceHolder1 As System.Web.UI.WebControls.PlaceHolder


    'Page Load
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session("LoggedUserKey") Is Nothing Then
            Response.Redirect("Login.aspx", False)
        End If

        'Load menu items
        Menu1.DataSource = Server.MapPath("SimpleXML.xml")
        Menu1.DataBind()
        CheckReadOnlyMode() 'PK | 02-28-2019 |YRS-AT-4248 | Check security method called here
        If Not IsPostBack Then
            Session("l_dataset_Tracking") = Nothing
            LoadData()
            BindGrid(DataGridRequest)
        End If

    End Sub

    'To make record editable in then grid.
    Sub Edit_Grid(ByVal sender As Object, ByVal e As DataGridCommandEventArgs)
        DataGridRequest.AllowSorting = False
        DataGridRequest.EditItemIndex = e.Item.ItemIndex
        BindGrid(DataGridRequest)
    End Sub

    'To cancel the edit operation in Grid.
    Sub Cancel_Grid(ByVal sender As Object, ByVal e As DataGridCommandEventArgs)
        DataGridRequest.AllowSorting = True
        DataGridRequest.EditItemIndex = -1
        BindGrid(DataGridRequest)
    End Sub

    'To Update the edited record in Grid.
    Sub Update_Grid(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs)
        Dim TrackingNo As String
        Dim RefReqId As String
        Dim IsTrackingNoUsed As String
        Try
            RefReqId = DataGridRequest.DataKeys(E.Item.ItemIndex).ToString()
            TrackingNo = CType(E.Item.Cells(7).Controls(0), TextBox).Text

            'Update the record with Tracking No
            Dim errorMsg As String = String.Empty
            errorMsg = YMCARET.YmcaBusinessObject.RefundRequest.UpdateRefReqTrackingNo(RefReqId, TrackingNo.Trim)

            If (errorMsg <> String.Empty) Then
                MessageBox.Show(PlaceHolder1, "YMCA-YRS", errorMsg, MessageBoxButtons.Stop)
                errorMsg = String.Empty
                Exit Sub
            End If

            DataGridRequest.EditItemIndex = -1
            LoadData()
            BindGrid(DataGridRequest)
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            ExceptionPolicy.HandleException(ex, "Exception Policy")
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
        End Try

    End Sub

    'Cancel button click event
    Private Sub ButtonCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonCancel.Click
        'To come on main form
        Try
            Session("l_dataset_Tracking") = Nothing
            Response.Redirect("MainWebForm.aspx", False)
        Catch ex As SqlException
            Response.Redirect("ErrorPageForm.aspx?DBMessage=" + Server.UrlEncode(ex.Message.Trim.ToString()))
        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()))
        End Try
    End Sub

    'OK Button click event.
    Private Sub btnOK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOK.Click
        'To come on main form
        Try
            Session("l_dataset_Tracking") = Nothing
            Response.Redirect("MainWebForm.aspx", False)
        Catch ex As SqlException
            Response.Redirect("ErrorPageForm.aspx?DBMessage=" + Server.UrlEncode(ex.Message.Trim.ToString()))
        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()))
        End Try
    End Sub

    'For Sorting in Datagrid
    Sub SortCommand_OnClick(ByVal Source As Object, ByVal e As DataGridSortCommandEventArgs)
        Try
            Dim SortExpression As String
            SortExpression = e.SortExpression

            Dim dgTrackingNo As DataGrid = DirectCast(Source, DataGrid)

            If Not ViewState(dgTrackingNo.ID) Is Nothing Then
                If ViewState(dgTrackingNo.ID).ToString.Trim.EndsWith("ASC") Then
                    SortExpression = "[" + SortExpression + "] DESC"
                Else
                    SortExpression = "[" + SortExpression + "] ASC"
                End If
            Else
                SortExpression = "[" + SortExpression + "] ASC"
            End If

            ViewState(dgTrackingNo.ID) = SortExpression
            BindGrid(dgTrackingNo)
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
    End Sub

    'To Bind the Grid
    Private Function BindGrid(ByVal dgTrackingNo As DataGrid)
        Dim dsTrackingNo As DataSet
        Dim dvTrackingNo As New DataView
        Dim SortExpression As String

        Try
            Select Case dgTrackingNo.ID
                Case "DataGridRequest"
                    dsTrackingNo = Session("l_dataset_Tracking")
                Case Else
                    Throw New Exception("DataGrtid is not handled: " & dgTrackingNo.ID)
            End Select
            'Added by prasad 25/11/2011 YRS 5.0-1453:Error message if no records returned
            If Not dsTrackingNo Is Nothing Then
                dvTrackingNo = dsTrackingNo.Tables(0).DefaultView
            Else
                Label1.Visible = True
            End If

            If Not ViewState(dgTrackingNo.ID) Is Nothing Then
                SortExpression = ViewState(dgTrackingNo.ID)
                dvTrackingNo.Sort = SortExpression
            End If
            dgTrackingNo.DataSource = Nothing
            dgTrackingNo.DataSource = dvTrackingNo
            dgTrackingNo.DataBind()

        Catch ex As Exception
            Throw
        End Try

    End Function

    'Load data
    Private Function LoadData()
        Dim dsTrackingNo As DataSet
        Try
            DataGridRequest.AllowSorting = True
            dsTrackingNo = YMCARET.YmcaBusinessObject.RefundRequest.GetDisbRefRequest()
            If Not dsTrackingNo Is Nothing Then
                If dsTrackingNo.Tables(0).Rows.Count > 0 Then
                    Session("l_dataset_Tracking") = dsTrackingNo
                End If
            End If
        Catch ex As Exception
            Throw
        End Try
    End Function
    'START : PK | 02-28-2019 | YRS-AT-4248 | Check ReadOnly Flag, if true then disable the buttons on which security is not implemented and add Tool tip.
    Public Sub CheckReadOnlyMode()
        If (SecurityCheck.IsApplicationInReadOnlyMode()) Then
            DataGridRequest.Enabled = False
            DataGridRequest.ToolTip = YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.TOOLTIP_READONLY).DisplayText
        End If
    End Sub
    'END : PK | 02-28-2019 | YRS-AT-4248 | Check ReadOnly Flag, if true then disable the buttons on which security is not implemented and add Tool tip.
End Class
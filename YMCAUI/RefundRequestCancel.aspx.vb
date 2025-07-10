'*******************************************************************************
' Copyright YMCA Retirement Fund All Rights Reserved. 
'
' Project Name		:	YMCA_YRS
' FileName			:	RefundRequestCancel.aspx.vb
' Author Name		:	Shashi Shekhar Singh
' Employee ID		:	51426
' Email				:	shashi.singh@3i-infotech.com
' Contact No		:	8684
' Creation Time		:	05/19/2011 04:25:00 PM
' Program Specification Name	:	YRS-PS-RefundTracking.doc 
' Unit Test Plan Name			:	
' Description					:Screen	to update the Tracking No record for Refund request having status 'DISB'
'
'********************************************************************************************************************************
'Modification History
'********************************************************************************************************************************
'Modified By        Date            Description
'********************************************************************************************************************************
'Manthan Rajguru    2015.09.16      YRS-AT-2550: YRS data cleanup: copyright comments and namespace/reference (Changed the Namespace/reference from 'Infotech' to 'YMCARET')
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
Public Class RefundRequestCancel
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            PopulateCancelReasonList()
        End If
    End Sub

    Private Sub ButtonCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonCancel.Click
        Dim msg As String = ""
        Try
            Session("RefCanReasonCode") = Nothing
            msg = msg + "<Script Language='JavaScript'>"
            msg = msg + "window.opener.document.forms(0).submit();" '"window.opener.location.href=window.opener.location.href;"
            msg = msg + "self.close();"
            msg = msg + "</Script>"
            Response.Write(msg)

        Catch ex As Exception
            'Throw
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
    End Sub

    Public Sub PopulateCancelReasonList()
        Try
            Dim l_dsReadReasonCode As DataSet
            l_dsReadReasonCode = YMCARET.YmcaBusinessObject.RefundRequest.GetRefCancelReasonCodes()
            ddlReasonCode.DataSource = l_dsReadReasonCode.Tables("GetReasonCode")
            ddlReasonCode.DataTextField = "chvShortDesc"
            ddlReasonCode.DataValueField = "chvReasonCode"
            ddlReasonCode.DataBind()
            ddlReasonCode.Items.Insert(0, New ListItem("-Select-", "select"))
        Catch ex As Exception
            ExceptionPolicy.HandleException(ex, "Exception Policy")
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
        End Try

    End Sub

    Private Sub ButtonOK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonOK.Click
        Dim msg As String = String.Empty
        Try

            If (ddlReasonCode.SelectedIndex = 0) Then
                MessageBox.Show(PlaceHolder1, " YMCA - YRS", "Please select the cancel reason code.", MessageBoxButtons.OK, False)
                Exit Sub
            End If


            Session("RefCanReasonCode") = ddlReasonCode.SelectedItem.Value.ToString.Trim


            msg = msg + "<Script Language='JavaScript'>"
            msg = msg + "window.opener.document.forms(0).submit();" '"window.opener.location.href=window.opener.location.href;"
            msg = msg + "self.close();"
            msg = msg + "</Script>"
            Response.Write(msg)
        Catch ex As Exception
            ExceptionPolicy.HandleException(ex, "Exception Policy")
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
        End Try

    End Sub
End Class
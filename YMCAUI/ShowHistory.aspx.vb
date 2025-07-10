'*******************************************************************************
' Copyright YMCA Retirement Fund All Rights Reserved. 
'
' Project Name		:	YMCA_YRS
' FileName			:	ShowHistory.aspx.vb
' Author Name		:	Ruchi Saxena
' Employee ID		:	33494
' Email				:	ruchi.saxena@3i-infotech.com
' Contact No		:	8751
' Creation Time		:	6/13/2005 4:49:45 PM
' Program Specification Name	:	
' Unit Test Plan Name			:	
' Description					:	pop up for VR Manager
' Cache-Session                 : Hafiz 04Feb06
'*******************************************************************************
'********************************************************************************
'Modification History
'********************************************************************************
'Modified By            Date                  Desription
'********************************************************************************
'Aparna                 04-Oct-2007         To use bound columns in the DATAGRID            
'Neeraj Singh           12/Nov/2009         Added form name for security issue YRS 5.0-940 
'Manthan Rajguru        2015.09.16          YRS-AT-2550: YRS data cleanup: copyright comments and namespace/reference (Changed the Namespace/reference from 'Infotech' to 'YMCARET')
'********************************************************************************

Imports System.Exception
Imports System.Data
Imports System.Data.SqlClient


Public Class ShowHistory
    Inherits System.Web.UI.Page
    'below line is Added by Neeraj for issue id YRS 5.0-940 
    Dim strFormName As String = New String("ShowHistory.aspx")
    'End issue id YRS 5.0-940

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents DataGridShowHistory As System.Web.UI.WebControls.DataGrid
    Protected WithEvents ButtonClose As System.Web.UI.WebControls.Button
    Protected WithEvents LabelNoRecordFound As System.Web.UI.WebControls.Label

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Session("LoggedUserKey") Is Nothing Then
            Response.Redirect("Login.aspx", False)
        End If
        Try
            Dim l_varchar_DisbId As String
            Dim l_dataset_DisbursementHistory As New DataSet
            ButtonClose.Attributes.Add("onclick", "javascript:CloseWindow();")
            l_varchar_DisbId = Request.QueryString.Get("DisbId")
            ' l_varchar_DisbId = "E412EBE3-E0F5-4764-81C6-4401A710F4CB"
            'by Aparna Added bounded columns 03/10/2007
            l_dataset_DisbursementHistory = YMCARET.YmcaBusinessObject.ShowHistoryBOClass.GetDisbursementHistory(l_varchar_DisbId)
            Me.DataGridShowHistory.DataSource = l_dataset_DisbursementHistory
            Me.DataGridShowHistory.DataBind()

            'commented by hafiz on 3-Oct-2007 - Need to write code in aspx for bounded column 
            'CommonModule.HideColumnsinDataGrid(l_dataset_DisbursementHistory, Me.DataGridShowHistory, "UniqueID,ActionType,Created,Creator")


        Catch sqlEx As SqlException
            If sqlEx.Number = 60006 Then
                Me.LabelNoRecordFound.Visible = True
                'Me.DataGridShowHistory.Visible = False
            Else
                ' Response.Redirect("ErrorPageForm.aspx")
                Throw
            End If
        Catch secEx As System.Security.SecurityException
            ' Response.Redirect("ErrorPageForm.aspx")
            Throw

        Catch ex As Exception
            ' Response.Redirect("ErrorPageForm.aspx")
            Throw
        End Try
     
    End Sub

    Private Sub ButtonClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonClose.Click
        Try
            Dim msg As String
            Session("Flag") = ""
            msg = msg + "<Script Language='JavaScript'>"
            msg = msg + "self.close();"
            msg = msg + "</Script>"

            Response.Write(msg)
        Catch ex As Exception
            Throw
        End Try
    End Sub
End Class

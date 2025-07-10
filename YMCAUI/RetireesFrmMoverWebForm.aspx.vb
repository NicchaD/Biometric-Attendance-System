'*******************************************************************************
' Copyright YMCA Retirement Fund All Rights Reserved. 
'
' Project Name		:	YMCA-YRS
' FileName			:	RetireesFrmMoverWebForm.aspx.vb
' Author Name		:	Nirav Pandit, Prasanna Kumar Penumarthy
' Employee ID		:	33488
' Email				:	Nirav.pandit@3i-infotech.com
' Contact No		:	8641
' Creation Time		:	5/17/2005 5:08:13 PM
' Program Specification Name	: Doc 3.1.2
' Unit Test Plan Name			:	
' Description					:	This is a FrmMover Pop up window of Document Tab
' Cache-Session                 : Hafiz 04Feb06
'*******************************************************************************
'Modification History
'*******************************************************************************
'Modified by                    Date                Description
'*******************************************************************************
'Neeraj Singh                   12/Nov/2009         Added form name for security issue YRS 5.0-940 
'Manthan Rajguru                2015.09.16          YRS-AT-2550: YRS data cleanup: copyright comments and namespace/reference (Changed the Namespace/reference from 'Infotech' to 'YMCARET')
'*******************************************************************************

Imports System
Imports System.Collections
Imports System.ComponentModel
Imports System.Data
Imports System.Data.SqlClient
Imports System.Drawing
Imports System.Threading
Imports System.Reflection
Imports System.Web
Imports System.Web.SessionState
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.HtmlControls
Imports System.Data.DataRow
Imports System.Security.Permissions
'Hafiz 04Feb06 Cache-Session
'Imports Microsoft.Practices.EnterpriseLibrary.Caching
'Hafiz 04Feb06 Cache-Session
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
''Imports CrystalDecisions.CrystalReports.Engine
''Imports CrystalDecisions.Shared
''Imports CrystalDecisions.Web
Imports CrystalDecisions.CrystalReports.ViewerObjectModel
'**************************************************************************************************************************
'Modification History
'**************************************************************************************************************************
'Modified By        Date            Description
'Ashutosh Patil     09-May-2007     Added Try Catch Block in Page Load and Added False Parameter On Ok button click event
'                                   for finding out ERROR "Thread was being aborted"
'Ashutosh Patil     22-May-2007     Added False Parameter On Each Try Catch Block
'                                   for finding out ERROR "Thread was being aborted"
'Shashi Shekhar:2010-03-18:Remove "TRANSACTION ARCHIVE" and "DISBURSEMENT ARCHIVE" From the list which was'earlier added when paricipants data is archived,Now we have to remove this option 
'**************************************************************************************************************************
Public Class RetireesFrmMoverWebForm
    Inherits System.Web.UI.Page

    'below line is Added by Neeraj for issue id YRS 5.0-940 
    Dim strFormName As String = New String("RetireesFrmMoverWebForm.aspx")
    'End issue id YRS 5.0-940


#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents ListBoxAvailableItem As System.Web.UI.WebControls.ListBox
    Protected WithEvents ListBoxSelectedItems As System.Web.UI.WebControls.ListBox
    Protected WithEvents BtnAdd As System.Web.UI.WebControls.Button
    Protected WithEvents BtnAddAll As System.Web.UI.WebControls.Button
    Protected WithEvents BtnRemove As System.Web.UI.WebControls.Button
    Protected WithEvents BtnRemoveAll As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonAdd As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonAddAll As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonRemove As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonRemoveAll As System.Web.UI.WebControls.Button
    Protected WithEvents LabelAvailableItems As System.Web.UI.WebControls.Label
    Protected WithEvents LabelSelectedItems As System.Web.UI.WebControls.Label

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object
    Protected WithEvents btnOK As System.Web.UI.WebControls.Button
    Protected WithEvents btnCancel As System.Web.UI.WebControls.Button
    Dim l_dataset_FedWith As DataSet
    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
        Me.LabelSelectedItems.AssociatedControlID = Me.ListBoxSelectedItems.ID
        Me.LabelAvailableItems.AssociatedControlID = Me.ListBoxAvailableItem.ID
        If Session("LoggedUserKey") Is Nothing Then
            Response.Redirect("Login.aspx", False)
        End If
        If Not IsPostBack = True Then
            If Session("ManualTransacts") = "ManualTransacts" Then
                LoadPHRDetails()
                btnOK_Click(sender, e)
            ElseIf Session("VRManager") = "VRManager" Then

                LoadPHRDetails()
                btnOK_Click(sender, e)
            Else
                LoadPHRDetails()
                End If
            End If
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
        ''btnOK.Attributes.Add("OnClick", "javascript:btn_OkClick();")
        'Button1.Attributes.Add("OnClick", "javascript:btn_OkClick1();")

    End Sub
    Public Sub LoadPHRDetails()
        Try
            'Hafiz 03Feb06 Cache-Session
            'Dim cache As CacheManager
            'cache = CacheFactory.GetCacheManager()
            'Hafiz 03Feb06 Cache-Session

            'l_dataset_FedWith = New DataSet
            If l_dataset_FedWith Is Nothing Then
                l_dataset_FedWith = YMCARET.YmcaBusinessObject.RetireesInformationBOClass.PHRDetails()
            Else
                'Hafiz 03Feb06 Cache-Session
                'l_dataset_FedWith = cache("PHRDetails")
                l_dataset_FedWith = Session("PHRDetails")
                'Hafiz 03Feb06 Cache-Session
            End If
            Dim i As Integer
            For i = 0 To l_dataset_FedWith.Tables(0).Rows.Count - 1
                'Dim LItem As New LItems
                'LItem.name = l_dataset_FedWith.Tables(0).Rows(i).Item("UserName")
                'LItem.id = l_dataset_FedWith.Tables(0).Rows(i).Item("USerId")
                Dim list As New ListItem(l_dataset_FedWith.Tables(0).Rows(i).Item("PhrOption"), l_dataset_FedWith.Tables(0).Rows(i).Item("SortOrder"))

                'Shashi Shekhar:2010-03-18:Remove "TRANSACTION ARCHIVE" and "DISBURSEMENT ARCHIVE" From the list which was'earlier added when paricipants data is archived,Now we have to remove this option 
                ''---------------------------------------------------------------------------------------------
                ''Shashi Shekhar:2009.09.04 Ref:(PS:YMCA PS Data Archive.Doc) - Adding PHR Option "Transaction Archive" and "Disbursement Archive" if the prson's data has been archived else Escaping these two option 
                'If (Session("IsDataArchived") = "No") And ((list.Text.ToUpper.Trim = "TRANSACTION ARCHIVE".Trim) Or (list.Text.ToUpper.Trim = "DISBURSEMENT ARCHIVE".Trim)) Then
                'Else
                '    ListBoxAvailableItem.Items.Add(list)
                'End If
                ''---------------------------------------------------------------------------------------------------
                ListBoxAvailableItem.Items.Add(list)

                list = Nothing
            Next
            If Session("ManualTransacts") = "ManualTransacts" Then
                For i = 0 To ListBoxAvailableItem.Items.Count - 1
                    If Not ListBoxAvailableItem.Items(i).Text.ToUpper.Trim = "YTD SUMMARY INFORMATION".Trim _
                    And Not ListBoxAvailableItem.Items(i).Text.ToUpper.Trim = "YTD ANNUITY INFORMATION".Trim Then
                        ListBoxSelectedItems.Items.Add(ListBoxAvailableItem.Items(i).Text)
                    End If
                Next
                Session("strReportName") = "YRSParticipantHistoryReportBySSNo"
                Session("ListBoxSelectedItems") = ListBoxSelectedItems
            End If
            If Session("VRManager") = "VRManager" Then
                For i = 0 To ListBoxAvailableItem.Items.Count - 1
                    If Not ListBoxAvailableItem.Items(i).Text.ToUpper.Trim = "YTD SUMMARY INFORMATION".Trim _
                    And Not ListBoxAvailableItem.Items(i).Text.ToUpper.Trim = "YTD ANNUITY INFORMATION".Trim Then
                        ListBoxSelectedItems.Items.Add(ListBoxAvailableItem.Items(i).Text)
                    End If
                Next
                Session("strReportName") = "YRSParticipantHistoryReportBySSNo"
                Session("ListBoxSelectedItems") = ListBoxSelectedItems
            End If
            l_dataset_FedWith = Nothing
        Catch ex As SqlException
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?DBMessage=" + l_String_Exception_Message, False)
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
    End Sub
    Private Sub ButtonAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAdd.Click
        Try
            Dim i As Integer
            Dim j As Integer
            If Not ListBoxAvailableItem.SelectedIndex = -1 Then 'If item is selected then
                'Dim Litem As ListItem = CType(ListBoxAvailableItem.SelectedItem, ListItem)
                'Code Begins   
                ' Iterate through the Items collection of the ListBox and 
                ' display the selected items.
                For i = ListBoxAvailableItem.Items.Count - 1 To 0 Step -1
                    If ListBoxAvailableItem.Items(i).Selected Then
                        ListBoxSelectedItems.Items.Add(ListBoxAvailableItem.Items(i))  'Add selected item to lstSelectedObject ListBox
                        ListBoxAvailableItem.Items.RemoveAt(i) 'Remove selected item from lstObjects
                    End If
                Next
            End If
            If ListBoxAvailableItem.Items.Count > 0 Then
                ListBoxAvailableItem.SelectedIndex = 0
            End If
            If ListBoxSelectedItems.Items.Count > 0 Then
                ListBoxSelectedItems.SelectedIndex = 0
            End If
            'If ListBoxSelectedItems.Items.Count > 0 Then
            ShowReport()
            'End If
        Catch ex As SqlException
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?DBMessage=" + l_String_Exception_Message, False)
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
    End Sub
    Private Sub ButtonAddAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAddAll.Click
        Try
            Dim lItem As ListItem
            For Each lItem In ListBoxAvailableItem.Items
                ListBoxSelectedItems.Items.Add(lItem)
            Next
            ListBoxAvailableItem.Items.Clear()
            If ListBoxSelectedItems.Items.Count > 0 Then
                ListBoxSelectedItems.SelectedIndex = 0
            End If
            'If ListBoxSelectedItems.Items.Count > 0 Then
            ShowReport()
            'End If
        Catch ex As SqlException
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?DBMessage=" + l_String_Exception_Message, False)
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
    End Sub
    Private Sub ButtonRemove_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonRemove.Click
        Try
            ''This procedure will move the selected item from lstSelectedObjects to lstObject listbox
            Dim i As Integer
            For i = ListBoxSelectedItems.Items.Count - 1 To 0 Step -1
                If ListBoxSelectedItems.Items(i).Selected Then
                    ListBoxAvailableItem.Items.Add(ListBoxSelectedItems.Items(i))  'Add selected item to lstSelectedObject ListBox
                    ListBoxSelectedItems.Items.RemoveAt(i) 'Remove selected item from lstObjects
                End If
            Next
            If ListBoxSelectedItems.Items.Count > 0 Then
                ListBoxSelectedItems.SelectedIndex = 0
            End If
            If ListBoxAvailableItem.Items.Count > 0 Then
                ListBoxAvailableItem.SelectedIndex = 0
            End If
            'If ListBoxSelectedItems.Items.Count > 0 Then
            ShowReport()
            'End If
        Catch ex As SqlException
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?DBMessage=" + l_String_Exception_Message, False)
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
    End Sub
    Private Sub ButtonRemoveAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonRemoveAll.Click
        Try
            Dim lItem As ListItem
            For Each lItem In ListBoxSelectedItems.Items
                ListBoxAvailableItem.Items.Add(lItem)
            Next
            ListBoxSelectedItems.Items.Clear()
            If ListBoxAvailableItem.Items.Count > 0 Then
                ListBoxAvailableItem.SelectedIndex = 0
            End If
            'If ListBoxSelectedItems.Items.Count > 0 Then
            ShowReport()
            'End If
        Catch ex As SqlException
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?DBMessage=" + l_String_Exception_Message, False)
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
    End Sub
    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        '''code added by Prasanna on 31st October 2005
        Dim alertWindow As String
        Try
            If ListBoxSelectedItems.Items.Count > 0 Then
                '''Response.Redirect("ReportViewer.aspx", False)

                Dim popupScript As String = "<script language='javascript'>" & _
                "window.open('FT\\ReportViewer.aspx', 'ReportPopUp', " & _
                "'width=1024,height=768, menubar=no, resizable=yes,top=0,left=0, scrollbars=yes, status=yes')" & _
                "</script>"
                If (Not Me.IsStartupScriptRegistered("PopupScript1")) Then
                    Page.RegisterStartupScript("PopupScript1", popupScript)
                End If
            Else
                alertWindow = "<script language='javascript'>" & _
                                                         "alert('Please Select a Report to view PHR details');" & _
                                                         "</script>"
                If (Not Me.IsStartupScriptRegistered("alertWindow")) Then
                    Page.RegisterStartupScript("alertWindow", alertWindow)
                End If
            End If
        Catch ex As SqlException
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?DBMessage=" + l_String_Exception_Message, False)
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
    End Sub
    Private Sub ShowReport()

        Try
            Dim strReportName As String
            'Dim strMyParmValue As String
            'Dim DS As New DataSet
            'Dim ParameterFieldsCollection As New CrystalDecisions.Shared.ParameterFields
            'Dim ParameterFieldSSNo As New CrystalDecisions.Shared.ParameterField
            'Dim ParameterDiscreteValueSSNo As New CrystalDecisions.Shared.ParameterDiscreteValue
            'Dim ParameterFieldReportType As New CrystalDecisions.Shared.ParameterField
            'Dim i As Integer
            'Dim ArrList As New ArrayList
            strReportName = "YRSParticipantHistoryReportBySSNo"

            'ParameterFieldSSNo.ParameterFieldName = "SSNo" ' Parameter Name In Crystal Report
            'ParameterDiscreteValueSSNo.Value = Session("SSNo")   ' "111111111"    'Session("SSNo")          ' value For Parameter Field 
            'ParameterFieldSSNo.CurrentValues.Add(ParameterDiscreteValueSSNo)

            'ParameterFieldReportType.ParameterFieldName = "Report Type" ' Parameter Name In Crystal Report 
            'For i = 0 To ListBoxSelectedItems.Items.Count - 1
            '    Dim ParameterDiscreteValueReportType As New CrystalDecisions.Shared.ParameterDiscreteValue
            '    ArrList.Add(ListBoxSelectedItems.Items(i).Text.ToString())
            '    'param1Field1.CurrentValues.Add(param1Range1)
            '    ParameterDiscreteValueReportType.Value = ListBoxSelectedItems.Items(i).Text
            '    ParameterFieldReportType.CurrentValues.Add(ParameterDiscreteValueReportType)
            'Next
            'ParameterFieldsCollection.Add(ParameterFieldSSNo)                   ' To add parameter in parameterslist
            'If ListBoxSelectedItems.Items.Count > 0 Then
            '    ParameterFieldsCollection.Add(ParameterFieldReportType)
            'End If
            'Session("ParameterFieldsCollection") = ParameterFieldsCollection
            Session("strReportName") = strReportName
            Session("ArrList") = Nothing
            Session("ListBoxSelectedItems") = ListBoxSelectedItems
            'Server.Transfer("ReportViewer.aspx", True)
        Catch ex As SqlException
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?DBMessage=" + l_String_Exception_Message, False)
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try

    End Sub
    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Response.Write("<script language='javascript'> { self.close() }</script>")
    End Sub
    'Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
    '    Dim strReportName As String
    '    strReportName = "First Annuity Checks Request"
    '    Session("strReportName") = strReportName
    '    'Server.Transfer("ReportViewer.aspx", True)
    'End Sub
End Class

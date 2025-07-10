#Region "Modification Histroy"
'*******************************************************************************
' Copyright YMCA Retirement Fund All Rights Reserved. 
'
' Project Name		:	YMCA-YRS
' FileName			:	MetaMessages.aspx.vb
' Author Name		:	Shashank Patel
' Employee ID		:	55381
' Creation Date	    :	8/19/2014 
' Program Specification Name	:YRS-PS-MetaMessages.doc	
' Unit Test Plan Name			:	
' Description					:	<<This page show all the messages from atsmetamessage table with option to edit. >>
'*******************************************************************************
'Modification History
'********************************************************************************************************************************
'Modified By        Date            Description
'********************************************************************************************************************************
'Manthan Rajguru    2015.09.16      YRS-AT-2550: YRS data cleanup: copyright comments and namespace/reference (Changed the Namespace/reference from 'Infotech' to 'YMCARET')
'********************************************************************************************************************************
#End Region

Imports YMCARET.YmcaBusinessObject
Imports YMCAObjects
Imports YMCAUI.SessionManager
Imports System.Reflection
Imports System.ComponentModel
Imports System.Web.Services

Public Class MetaMessages
    Inherits System.Web.UI.Page
    Dim strFormName As String = "MetaMessages.aspx"

#Region "Page Events"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try

            If Not Page.IsPostBack Then
                SessionMetaMesasages.MetaMesasagesList = Nothing
                HideReloadMessagesButtonOtherThanSuperAdminRight(Session("LoggedUserKey"))
                BindGrid()
                BindDropDownMessageTypeWithDistinct(SessionMetaMesasages.MetaMesasagesList)
                BindDropDownMessageModuleWithDistinct(SessionMetaMesasages.MetaMesasagesList)
            Else
                If (SessionMetaMesasages.IsUpdatedSuccuessFull) Then
                    SessionMetaMesasages.MetaMesasagesList = Nothing
                    SessionMetaMesasages.IsUpdatedSuccuessFull = Nothing
                    MetaMessageBO.ClearMessagesCache()
                    BindGrid()
                    BindDropDownMessageModuleWithDistinct(SessionMetaMesasages.MetaMesasagesList)
                    HelperFunctions.ShowMessageToUser(MetaMessageList.MESSAGE_META_MESSAGES_SAVED_SUCCESSFULLY)
                End If
            End If

        Catch ex As Exception
            HelperFunctions.LogException("MetaMessage --> Page_Load", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
        End Try
    End Sub
#End Region

#Region "Private Methods"

    Private Sub BindGrid()
        Dim msgList As New List(Of MetaMessage)
        Try
            If (SessionMetaMesasages.MetaMesasagesList Is Nothing) Then
                msgList = MetaMessageBO.GetMessageList()
            Else
                msgList = SessionMetaMesasages.MetaMesasagesList
            End If
            gvMessages.SelectedIndex = -1
            gvMessages.DataSource = msgList
            gvMessages.DataBind()

            SessionMetaMesasages.MetaMesasagesList = msgList

        Catch
            Throw
        End Try
    End Sub

    Private Function GetPipeSeperatedDynamicParams(ByVal strInput As String) As String
        Dim match As Match
        Dim strOutput As New StringBuilder()

        Try
            match = Regex.Match(strInput.Trim(), "\${2}.*?\${2}", RegexOptions.IgnoreCase)
            While (match.Success)
                strOutput.Append("|" + match.Value.Trim())
                match = match.NextMatch()
            End While
            If (strOutput.Length > 0) Then
                strOutput = strOutput.Remove(0, 1)
            End If
        Catch
            Throw
        End Try
        Return strOutput.ToString()

    End Function

    Private Function CreateSchemaofMessageList(ByVal metaMessage As MetaMessage) As DataTable
        Dim dtReturn As New DataTable

        Dim dataColumn As DataColumn
        Try

            Dim properties As PropertyDescriptorCollection = TypeDescriptor.GetProperties(metaMessage)
            For Each prop As PropertyDescriptor In properties
                'add property as column
                dtReturn.Columns.Add(prop.Name, prop.PropertyType)
            Next
            Return dtReturn
        Catch
            Throw
        End Try

    End Function

    Private Function ConvertListToDataTable(ByVal msgMessageList As List(Of MetaMessage)) As DataTable
        Dim dtReturn As DataTable
        Try
            If msgMessageList Is Nothing Then Return Nothing
            dtReturn = CreateSchemaofMessageList(msgMessageList(0))

            Dim dr As DataRow
            Dim entType As Type = GetType(MetaMessage)
            Dim properties As PropertyDescriptorCollection = TypeDescriptor.GetProperties(entType)

            For Each msg As MetaMessage In msgMessageList
                dr = dtReturn.NewRow()
                For Each prop As PropertyDescriptor In properties
                    dr(prop.Name) = prop.GetValue(msg)
                Next
                dtReturn.Rows.Add(dr)
            Next
        Catch
            Throw
        End Try
        Return dtReturn
    End Function
    Public Function GetMessage(ByVal iMessageNo As Int32) As String
        Try
            Return MetaMessageBO.GetMessageByTextMessageNo(iMessageNo)
        Catch
            Throw
        End Try
    End Function

    Private Sub BindDropDownMessageModuleWithDistinct(ByVal msglist As List(Of MetaMessage))
        Try
            If (msglist IsNot Nothing) Then
                ddlModuleName.DataSource = msglist.Select(Function(x) x.ModuleName).Distinct().ToList()
                ddlModuleName.DataBind()
                ddlModuleName.Items.Insert(0, New ListItem("Select", ""))
            End If
        Catch
            Throw
        End Try

    End Sub

    Private Sub BindDropDownMessageTypeWithDistinct(ByVal msglist As List(Of MetaMessage))
        Try
            If (msglist IsNot Nothing) Then
                ddlMessageType.DataSource = msglist.Select(Function(x) x.DisplayMessageType).Distinct().ToList()
                ddlMessageType.DataBind()
                ddlMessageType.Items.Insert(0, New ListItem("Select", ""))
            End If
        Catch
            Throw
        End Try

    End Sub
    Private Sub ClearSession(Optional ByVal blnClearSession As Boolean = False)
        Try
            If blnClearSession Then
                SessionMetaMesasages.MetaMesasagesList = Nothing
            End If
            ViewState("gvMessages_PageIndexChanging") = Nothing
            ViewState("gvMessages_Sorting") = Nothing

        Catch
            Throw
        End Try
    End Sub
    Private Function GetMesagesWithFilterExpression(ByVal msgList As List(Of MetaMessage), ByVal strDisplayText As String, ByVal strDescription As String, ByVal strMessageType As String, ByVal strModuleName As String) As List(Of MetaMessage)

        Dim msgResult As List(Of MetaMessage)
        Try

            If msgList IsNot Nothing Then
                msgResult = (From m In msgList.AsEnumerable()
                             Where m.DisplayMessageType = IIf(String.IsNullOrEmpty(strMessageType), m.DisplayMessageType, strMessageType.Trim()) AndAlso
                                    m.MessageDescription.ToLower.Contains(IIf(String.IsNullOrEmpty(strDescription), m.MessageDescription.ToLower, strDescription.Trim().ToLower)) AndAlso
                                    m.DisplayText.ToLower.Contains(IIf(String.IsNullOrEmpty(strDisplayText), m.DisplayText.ToLower, strDisplayText.Trim().ToLower)) AndAlso
                                    m.ModuleName = IIf(String.IsNullOrEmpty(strModuleName), m.ModuleName, strModuleName.Trim())
                             Select m).ToList()

            End If
        Catch
            Throw
        End Try
        Return msgResult
    End Function
    Private Sub HideReloadMessagesButtonOtherThanSuperAdminRight(ByVal strLoginUserID As String)
        Dim dsGroupMembers As DataSet
        Try
            Dim checkSecurity As String = SecurityCheck.Check_Authorization("btnReloadAllMessage", Convert.ToInt32(Session("LoggedUserKey")))
            If Not checkSecurity.Equals("True") Then
                btnReloadAllMessage.Visible = False
            End If
        Catch
            Throw
        End Try
    End Sub

#End Region

#Region "Web Methods"

    <WebMethod()>
    Public Shared Function UpdateMessage(ByVal MessageNo As String, ByVal MessageDescription As String, ByVal MessageDisplayText As String) As String()
        Try
            Dim metaMessage As New YMCAObjects.MetaMessage
            Dim strMessage(2) As String
            metaMessage.MessageNo = Convert.ToInt32(MessageNo.Trim())
            MessageDescription = RemoveNewLine(MessageDescription.Trim())
            MessageDisplayText = RemoveNewLine(MessageDisplayText.Trim())
            metaMessage.MessageDescription = MessageDescription
            metaMessage.DisplayText = MessageDisplayText
            strMessage = ValidateMessage(MessageNo, MessageDescription, MessageDisplayText)
            If (strMessage IsNot Nothing AndAlso strMessage(0) = "error") Then
                Return strMessage
            End If

            Dim output As Int32 = MetaMessageBO.UpdateMessage(metaMessage)
            If (output > 0) Then
                SessionMetaMesasages.IsUpdatedSuccuessFull = True
                Return GetMessageReturnOutput("success", "")
            Else
                SessionMetaMesasages.IsUpdatedSuccuessFull = False
                Return GetMessageReturnOutput("error", MetaMessageBO.GetMessageByTextMessageNo(MetaMessageList.MESSAGE_META_MESSAGES_EXCEPTION))
            End If

        Catch ex As Exception
            HelperFunctions.LogException("MetaMessage->UpdateMessage", ex)
            Return GetMessageReturnOutput("error", MetaMessageBO.GetMessageByTextMessageNo(MetaMessageList.MESSAGE_META_MESSAGES_EXCEPTION))
        End Try

    End Function

    Public Shared Function RemoveNewLine(ByVal strInputParam As String) As String
        Dim strOutput As String
        If Not String.IsNullOrEmpty(strInputParam) Then
            strOutput = strInputParam
            strOutput = strOutput.Trim().Replace("\r\n", "")
            strOutput = strOutput.Trim().Replace("\n", "")
            strOutput = strOutput.Trim().Replace("\r", "")
            strOutput = strOutput.Trim.Replace(vbCrLf, "")
        Else
            strOutput = String.Empty
        End If

        Return strOutput
    End Function
    Public Shared Function ValidateMessage(ByVal MessageNo As String, ByVal MessageDescription As String, ByVal MessageDisplayText As String) As String()
        Try
            If String.IsNullOrEmpty(MessageDescription) Then
                MessageDescription = RemoveNewLine(MessageDescription.Trim())
            End If
            If String.IsNullOrEmpty(MessageDisplayText) Then
                MessageDisplayText = RemoveNewLine(MessageDisplayText.Trim())
            End If

            If (String.IsNullOrEmpty(MessageDisplayText)) Then
                Return GetMessageReturnOutput("error", MetaMessageBO.GetMessageByTextMessageNo(MetaMessageList.MESSAGE_META_MESSAGES_ENTER_MESSAGEDISPLAYTEXT))
            ElseIf (MessageDisplayText.Length > 500) Then
                Return GetMessageReturnOutput("error", MetaMessageBO.GetMessageByTextMessageNo(MetaMessageList.MESSAGE_META_MESSAGES_MESSAGEDISPLAYTEXT_LENGTH))
            ElseIf (String.IsNullOrEmpty(MessageDescription)) Then
                Return GetMessageReturnOutput("error", MetaMessageBO.GetMessageByTextMessageNo(MetaMessageList.MESSAGE_META_MESSAGES_ENTER_MESSAGEDESCRIPTION))
            ElseIf (MessageDescription.Length > 500) Then
                Return GetMessageReturnOutput("error", MetaMessageBO.GetMessageByTextMessageNo(MetaMessageList.MESSAGE_META_MESSAGES_MESSAGEDESCRIPTION_LENGTH))
            End If

        Catch
            Throw
        End Try
    End Function
    Private Shared Function GetMessageReturnOutput(ByVal strMessageType As String, ByVal strMessageDetail As String) As String()
        Dim strMessage(2) As String
        strMessage(0) = strMessageType
        strMessage(1) = strMessageDetail
        Return strMessage
    End Function
#End Region

#Region "GridView Events"
    Private Sub gvMessages_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvMessages.RowCommand
        Try
            If e.CommandName = "Select" Then
                Dim messageNo As Int32 = CType(e.CommandArgument, Int32)

                Dim checkSecurity As String = SecurityCheck.Check_Authorization("btnEditMessage", Convert.ToInt32(Session("LoggedUserKey")))
                If Not checkSecurity.Equals("True") Then
                    HelperFunctions.ShowMessageToUser(checkSecurity, EnumMessageTypes.Error)
                    Exit Sub
                End If

                If (messageNo > 0) Then
                    Dim metaMessage As MetaMessage = MetaMessageBO.GetMessageByMessageNo(messageNo)
                    hdnErrorNumber.Value = messageNo
                    hdnDynamicParam.Value = GetPipeSeperatedDynamicParams(metaMessage.DisplayText)
                    txtMessageDescription.Text = metaMessage.MessageDescription
                    lblModuleName.Text = metaMessage.ModuleName
                    txtMessageDisplayText.Text = metaMessage.DisplayText
                    lblMessageType.Text = metaMessage.MessageType.ToString()
                    ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "open", " SetMessagePreview('" + lblMessageType.Text.Trim + "'); OpenDialog('" + txtMessageDisplayText.Text.Trim.Replace("\", "\\").Replace("'", "\'") + "','" + txtMessageDescription.Text.Trim.Replace("\", "\\").Replace("'", "\'") + "');", True)
                End If
            End If
        Catch ex As Exception
            HelperFunctions.LogException("MetaMessage --> gvMessages_RowCommand", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
        End Try
    End Sub

    Private Sub gvMessages_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles gvMessages.PageIndexChanging
        Try
            Dim msgMessageList As List(Of MetaMessage)
            Dim dvMessages As DataView
            Dim gvSort As GridViewCustomSort
            Dim msgList As List(Of MetaMessage)
            If HelperFunctions.isNonEmpty(SessionMetaMesasages.MetaMesasagesList) Then
                msgList = GetMesagesWithFilterExpression(SessionMetaMesasages.MetaMesasagesList, txtDisplayText.Text.Trim(), txtDescription.Text.Trim(), ddlMessageType.SelectedValue.Trim(), ddlModuleName.SelectedValue.Trim())
                Dim dt As DataTable = ConvertListToDataTable(msgList)
                dvMessages = dt.AsDataView()
                ViewState("gvMessages_PageIndexChanging") = e.NewPageIndex
                If ViewState("gvMessages_Sorting") IsNot Nothing Then
                    gvSort = ViewState("gvMessages_Sorting")
                    dvMessages.Sort = gvSort.SortExpression + " " + gvSort.SortDirection
                End If
                gvMessages.SelectedIndex = -1
                gvMessages.PageIndex = e.NewPageIndex
                HelperFunctions.BindGrid(gvMessages, dvMessages)
            End If
        Catch ex As Exception
            HelperFunctions.LogException("MetaMessages --> gvMessages_PageIndexChanging", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
        End Try
    End Sub

    Private Sub gvMessages_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvMessages.RowDataBound
        Try
            HelperFunctions.SetSortingArrows(ViewState("gvMessages_Sorting"), e)
            If e.Row.RowType = DataControlRowType.DataRow Then
                Dim strMessageDescriptpion As String = String.Empty
                If TypeOf (e.Row.DataItem) Is DataRowView Then
                    Dim drv As DataRowView = DirectCast(e.Row.DataItem, DataRowView)
                    strMessageDescriptpion = Convert.ToString(drv("MessageDescription"))
                ElseIf TypeOf (e.Row.DataItem) Is MetaMessage Then
                    Dim message As MetaMessage = DirectCast(e.Row.DataItem, MetaMessage)
                    strMessageDescriptpion = message.MessageDescription
                End If

                Dim addToolTipDiv As HtmlContainerControl = Tooltip

                For i As Integer = 1 To e.Row.Cells.Count - 1
                    If Not String.IsNullOrEmpty(strMessageDescriptpion) Then
                        e.Row.Cells(i).Attributes.Add("onmouseover", "showToolTip('" + addToolTipDiv.ClientID + "','" + e.Row.ClientID + "','" + strMessageDescriptpion.Replace("\", "\\").Replace("'", "\'") + "');")
                        e.Row.Cells(i).Attributes.Add("onmouseout", "hideToolTip('" + addToolTipDiv.ClientID + "','" + e.Row.ClientID + "');")
                    End If
                Next
            End If
        Catch ex As Exception
            HelperFunctions.LogException("MetaMessage --> gvMessages_RowDataBound", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
        End Try
    End Sub

    Private Sub gvMessages_Sorting(sender As Object, e As GridViewSortEventArgs) Handles gvMessages.Sorting
        Try
            Dim msgMessageList As List(Of MetaMessage)
            Dim dvMessages As DataView
            If HelperFunctions.isNonEmpty(SessionMetaMesasages.MetaMesasagesList) Then
                msgMessageList = GetMesagesWithFilterExpression(SessionMetaMesasages.MetaMesasagesList, txtDisplayText.Text.Trim(), txtDescription.Text.Trim(), ddlMessageType.SelectedValue.Trim(), ddlModuleName.SelectedValue.Trim())
                Dim dt As DataTable = ConvertListToDataTable(msgMessageList)
                dvMessages = dt.AsDataView()
                HelperFunctions.gvSorting(ViewState("gvMessages_Sorting"), e.SortExpression, dvMessages)
                If ViewState("gvMessages_PageIndexChanging") IsNot Nothing Then
                    gvMessages.PageIndex = ViewState("gvMessages_PageIndexChanging")
                End If
                gvMessages.SelectedIndex = -1
                HelperFunctions.BindGrid(gvMessages, dvMessages)
            End If
        Catch ex As Exception
            HelperFunctions.LogException("MetaMessage --> gvMessages_Sorting", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
        End Try
    End Sub

#End Region

#Region "Page Controls Events"
    Private Sub btnMessageFind_Click(sender As Object, e As EventArgs) Handles btnMessageFind.Click
        Try
            ClearSession()
            Dim msgList As List(Of MetaMessage) = GetMesagesWithFilterExpression(SessionMetaMesasages.MetaMesasagesList, txtDisplayText.Text.Trim(), txtDescription.Text.Trim(), ddlMessageType.SelectedValue.Trim(), ddlModuleName.SelectedValue.Trim())
            gvMessages.DataSource = msgList
            gvMessages.DataBind()
        Catch ex As Exception
            HelperFunctions.LogException("MetaMessage --> btnMessageFind_Click", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
        End Try
    End Sub

    Private Sub btnMessageClear_Click(sender As Object, e As EventArgs) Handles btnMessageClear.Click
        Try
            txtDisplayText.Text = String.Empty
            txtDescription.Text = String.Empty
            ddlMessageType.SelectedIndex = -1
            ddlModuleName.SelectedIndex = -1
            ClearSession()
            BindGrid()
        Catch ex As Exception
            HelperFunctions.LogException("MetaMessage --> btnMessageClear_Click", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
        End Try
    End Sub

    Private Sub ButtonCloseForm_Click(sender As Object, e As EventArgs) Handles ButtonCloseForm.Click
        ClearSession(True)
        Response.Redirect("MainWebForm.aspx")
    End Sub
    Private Sub btnReloadAllMessage_Click(sender As Object, e As EventArgs) Handles btnReloadAllMessage.Click
        Try
            MetaMessageBO.ClearMessagesCache()
            HelperFunctions.ShowMessageToUser(MetaMessageList.MESSAGE_META_MESSAGES_RELOAD_MESSAGE)
        Catch ex As Exception
            HelperFunctions.LogException("MetaMessage --> btnReloadAllMessage_Click", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
        End Try
    End Sub
#End Region

   
End Class
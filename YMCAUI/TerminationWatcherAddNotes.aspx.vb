'*******************************************************************************
' Copyright YMCA Retirement Fund All Rights Reserved. 
'
' Project Name		:	YMCA-YRS
' FileName			:	TerminationWatcherAddNotes.aspx.vb
' Author Name		:	Priya Patil
' Employee ID		:	37786
' Email			    :	priya.jawale@3i-infotech.com
' Contact No		:	8416
' Creation Time	:	8/25/2012 6:36:14 PM
' Program Specification Name	:	
' Unit Test Plan Name			:	
' Description					:	<<Please put the brief description here...>>
'*******************************************************************************
'Modification History
'********************************************************************************************************************************
'Modified By        Date            Description
'********************************************************************************************************************************
'Anudeep          01-Nov-2012       Changes made as Per observations listed in bugtraker for Yrs 5.0-1484 on 01 nov 2012
'Anudeep          06-nov-2012       Changes Made as Per Observations listed in bugtraker For yrs 5.0-1484 on 06-nov 2012
'Manthan Rajguru  2015.09.16        YRS-AT-2550: YRS data cleanup: copyright comments and namespace/reference (Changed the Namespace/reference from 'Infotech' to 'YMCARET')
'********************************************************************************************************************************
Public Class TerminationWatcherAddNotes
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Session("LoggedUserKey") Is Nothing Then
                Response.Redirect("Login.aspx", False)
            End If
            If Not IsPostBack Then
                'btnCancel.Attributes.Add("onclick", "javascript:fnCloseWindow();")
                TerminationWatcherID = Request.QueryString("TerminationID").ToString().Trim
                PersID = Request.QueryString("PersID").ToString().Trim
                FetchData()
            End If
        Catch
            Throw
        End Try
    End Sub
    Private Property TerminationWatcherID() As String
        Get
            Return (CType(ViewState("TerminationWatcherID"), String))
        End Get
        Set(ByVal Value As String)
            ViewState("TerminationWatcherID") = Value
        End Set
    End Property
    Private Property PersID() As String
        Get
            Return (CType(ViewState("PersID"), String))
        End Get
        Set(ByVal Value As String)
            ViewState("PersID") = Value
        End Set
    End Property
    Private Property TWNotesID() As String
        Get
            Return (CType(ViewState("TWNotesID"), String))
        End Get
        Set(ByVal Value As String)
            ViewState("TWNotesID") = Value
        End Set
    End Property
    'Fetches the notes data from db
    Private Sub FetchData()
        Dim dsNotes As DataSet
        Dim strTerminationID As String = String.Empty
        Try
            strTerminationID = TerminationWatcherID
            dsNotes = YMCARET.YmcaBusinessObject.TerminationWatcherBO.ListNotes(strTerminationID)
            Session("dsNotes") = dsNotes
            If HelperFunctions.isNonEmpty(dsNotes) Then
                gvNotes.DataSource = dsNotes
                gvNotes.DataBind()
                LabelNoDataFound.Visible = False
            Else
                gvNotes.DataSource = Nothing
                gvNotes.DataBind()
                LabelNoDataFound.Visible = True
            End If
        Catch
            Throw
        End Try
    End Sub
    'Saves the notes into db
    Private Sub SaveNotes()
        Dim strResult As String = String.Empty
        Try
            If (TerminationWatcherID <> String.Empty AndAlso PersID <> String.Empty AndAlso txtNotes.Text.Trim() <> String.Empty) Then
                strResult = YMCARET.YmcaBusinessObject.TerminationWatcherBO.SaveTerminationWatcherNotes(TerminationWatcherID, PersID, txtNotes.Text.Trim, chkImportanat.Checked)
            Else
                MessageBox.Show(PlaceHolder1, "Termination Watcher", messageFromResourceFile("MESSAGE_TW_ADDNOTES_VALIDATION", ""), MessageBoxButtons.OK)
                Exit Sub
            End If
            If strResult = "-2" Then
                MessageBox.Show(PlaceHolder1, "Termination Watcher", messageFromResourceFile("MESSAGE_TW_ADDNOTES_ERROR_TBLTWNOTES", ""), MessageBoxButtons.OK)
            ElseIf strResult = "-1" Then
                MessageBox.Show(PlaceHolder1, "Termination Watcher", messageFromResourceFile("MESSAGE_TW_ADDNOTES_ERROR_TBLNOTES", ""), MessageBoxButtons.OK)
            ElseIf strResult = "1" Then
                'Anudeep:05-nov-2012 Changes made as per Observations listed in bugtraker for yrs 5.0-1484 --%>
                Dim msg As String
                Session("Refresh") = "YES"
                msg = msg + "<Script Language='JavaScript'>"
                msg = msg + "self.close();"
                msg = msg + "window.opener.document.forms(0).submit();"
                msg = msg + "</Script>"
                Response.Write(msg)
            End If
        Catch
            Throw
        End Try
    End Sub
    'Handles the notes gridview pageindexing
    Private Sub gvNotes_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvNotes.PageIndexChanging
        Try
            Me.gvNotes.PageIndex = e.NewPageIndex
            FetchData()
        Catch
            Throw
        End Try
    End Sub
    'Handles the notes gridview RowDataBound
    Private Sub gvNotes_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvNotes.RowDataBound
        Dim Important As String
        Dim drv As DataRowView = DirectCast(e.Row.DataItem, DataRowView)
        Try

            If e.Row.RowType = DataControlRowType.DataRow Then
                Important = Convert.ToString(drv("Important"))
                If Important = True Then
                    e.Row.ForeColor = Drawing.Color.Red
                    e.Row.ToolTip = messageFromResourceFile("MESSAGE_TW_NOTES_TOOLTIP", "")
                End If
            End If
        Catch
            Throw
        End Try
    End Sub
    'Handles the notes gridview Selectedindexchanged
    Private Sub gvNotes_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvNotes.SelectedIndexChanged
        Dim drv As DataRowView = DirectCast(gvNotes.Rows(gvNotes.SelectedIndex).DataItem, DataRowView)
        Dim dsnotes As DataSet
        Dim Notes, IsImportanat As String
        Try
            dsnotes = Session("dsNotes")
            IsImportanat = dsnotes.Tables(0).Rows(gvNotes.Rows(gvNotes.SelectedIndex).DataItemIndex)("Important").ToString()
            Notes = dsnotes.Tables(0).Rows(gvNotes.Rows(gvNotes.SelectedIndex).DataItemIndex)("Notes").ToString()
            TWNotesID = dsnotes.Tables(0).Rows(gvNotes.Rows(gvNotes.SelectedIndex).DataItemIndex)("TWNotesID").ToString()
            txtNotes.Text = Notes
            chkImportanat.Checked = IsImportanat
        Catch
            Throw
        End Try
    End Sub

    Private Sub btnOK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOK.Click
		Dim checkSecurity As String = SecurityCheck.Check_Authorization("btnProcess", Convert.ToInt32(Session("LoggedUserKey")))
        Try
            If Not checkSecurity.Equals("True") Then
                MessageBox.Show(PlaceHolder1, "Termination Watcher", checkSecurity, MessageBoxButtons.Stop)
                Exit Sub
            End If
            SaveNotes()
        Catch
            Throw
        End Try
    End Sub
    'Gets messagw from Resource file
    Private Function messageFromResourceFile(ByVal strMessage As String, ByVal strParameter As String) As String
        Dim strReturnMessage As String = String.Empty
        Dim TW As Resources.TerminationWatcher
        Try
            If strParameter = String.Empty Then
                strReturnMessage = TW.ResourceManager.GetString(strMessage).Trim()
            End If
            Return strReturnMessage
        Catch
            Throw
        End Try
    End Function
    'Added by Anudeep:22-10-2012 for Closing the Form refreshing the main page
    Private Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Dim msg As String
        Try
            msg = msg + "<Script Language='JavaScript'>"
            msg = msg + "self.close();"
            msg = msg + "</Script>"
            Response.Write(msg)
        Catch
            Throw
        End Try
    End Sub
    'Commented by Anudeep:05-nov-2012 Changes made as per Observations listed in bugtraker for yrs 5.0-1484 
    'Closes the window and refreshes the terminationwatcher.aspx page
    'Changed by ANudeep As per observations on  31-10-2012 for Refresh main page to show tooltip
    'Anudeep:05-nov-2012-Changes Made as Per Observations listed in bugtrake For yrs 5.0-1484 on 06-nov 2012
    '<System.Web.Services.WebMethod()> _
    'Public Shared Function RefreshPage()
    '    Try
    '        HttpContext.Current.Session("Refresh") = "YES"
    '    Catch
    '        Throw
    '    End Try
    'End Function
End Class
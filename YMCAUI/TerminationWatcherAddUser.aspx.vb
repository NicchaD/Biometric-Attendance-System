'*******************************************************************************
' Copyright YMCA Retirement Fund All Rights Reserved. 
'
' Project Name		:	YMCA-YRS
' FileName			:	TerminationWatcherAddUser.aspx.vb
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
'Anudeep          05-nov-2012       Changes Made as Per Observations listed in bugtraker For yrs 5.0-1484 on 06-nov 2012
'Anudeep          06-nov-2012       Changes Made as Per Observations listed in bugtraker For yrs 5.0-1484 on 06-nov 2012
'Anudeep          14-nov-2012       Changes made as per Observations listed in bugtraker for yrs 5.0-1484 on 14-nov-2012
'Anudeep          19-nov-2012       Changes made as per Observations listed in bugtraker for yrs 5.0-1484 on 16-nov-2012
'Anudeep          22-nov-2012       Commented For Showing Dropdown After Clicking Add button 
'Manthan Rajguru  2015.09.16        YRS-AT-2550: YRS data cleanup: copyright comments and namespace/reference (Changed the Namespace/reference from 'Infotech' to 'YMCARET')
'********************************************************************************************************************************

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
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling

Public Class TerminationWatcherAddUser
	Inherits System.Web.UI.Page
    Dim dsAddemployee As DataSet
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Session("LoggedUserKey") Is Nothing Then
                Response.Redirect("Login.aspx", False)
            End If
            If Not IsPostBack Then
                'Anudeep:05-11-2012 Commented below line as Per Observations listed in Bugtraker 
                'btnAddClose.Attributes.Add("onclick", "javascript:fnCloseWindow();")
                If Request.QueryString("form").ToString().Trim() <> "Status" Then
                    lblHead.Text = "Add " + Request.QueryString("form").ToString().Trim() + " Applicant"
                ElseIf Request.QueryString("form").ToString().Trim() <> String.Empty Then
                    lblHead.Text = "Add Applicant"
                End If
                'Anudeep:05-11-2012 Added below line as Per Observations listed in Bugtraker 
                ViewState("dtAdded") = Nothing
                lblAbvr.Visible = False
            End If
            If gvAdd.SelectedIndex = -1 Then
                disablecontrols()
            End If
        Catch
            Throw
        End Try
    End Sub
    'Disables all controls
    Private Sub disablecontrols()
        Try
            drdPlantype.Visible = False
            lblPlanType.Visible = False
            lblPlanTypeErr.Visible = False
            drdType.Visible = False
            lblType.Visible = False
            lblTypeErr.Visible = False
            btnAddOk.Enabled = False
            btnAdd.Enabled = False
        Catch
            Throw
        End Try
        
    End Sub
    'Gets the data according to search criteria
	Private Sub Populatedata()
        Try

            If txtFundno.Text = String.Empty And txtFirstName.Text = String.Empty And txtLastName.Text = String.Empty And txtSSNo.Text = String.Empty Then
                MessageBox.Show(PlaceHolder1, "Termination Watcher", "This search will take a longer time. It is suggested that you enter a search criteria.", MessageBoxButtons.OK)
            Else
                dsAddemployee = YMCARET.YmcaBusinessObject.TerminationWatcherBO.SearchPerson(txtFundno.Text.Trim(), txtFirstName.Text.Trim(), txtLastName.Text.Trim(), txtSSNo.Text.Trim())
                Session("dgAddEmployee") = dsAddemployee
                If HelperFunctions.isNonEmpty(dsAddemployee) Then
                    gvAdd.DataSource = dsAddemployee
                    gvAdd.DataBind()
                    'Added by Anudeep as per Observations 23-oct-2012 
                    'shows how many record exists in grid
                    lblTotalCount.Text = messageFromResourceFile("MESSAGE_TW_SHOW_TOTAL_RECORDS", "") + " " + dsAddemployee.Tables(0).Rows.Count.ToString()
                    LabelNoDataFound.Visible = False
                Else
                    gvAdd.DataSource = Nothing
                    gvAdd.DataBind()
                    lblTotalCount.Text = ""
                    LabelNoDataFound.Visible = True
                End If
            End If
        Catch
            Throw
        End Try
	End Sub
    'Saves the Data and inserts into termination watcher
    Private Function SaveData() As String
        Dim strguiPersId As String = String.Empty
        Dim strguiFundEventId As String = String.Empty
        Dim strchrType As String = String.Empty
        Dim strchvPlanType As String = String.Empty
        Dim strchvSource As String = String.Empty
        Dim strResult As String = String.Empty
        Dim dv As DataView

        Try
            dsAddemployee = Session("dgAddEmployee")
            dv = dsAddemployee.Tables(0).DefaultView
            If HelperFunctions.isNonEmpty(dsAddemployee) Then
                'strguiPersId = dsAddemployee.Tables(0).Rows(gvAdd.SelectedRow.DataItemIndex)("PersID").ToString()
                'strguiFundEventId = dsAddemployee.Tables(0).Rows(gvAdd.SelectedRow.DataItemIndex)("FundUniqueId").ToString()
                strguiPersId = gvAdd.DataKeys(gvAdd.SelectedIndex).Values("PersID").ToString()
                strguiFundEventId = gvAdd.DataKeys(gvAdd.SelectedIndex).Values("FundUniqueId").ToString()
            End If
            If drdType.Items.Count > 0 Then
                strchrType = drdType.SelectedValue
            End If
            If drdPlantype.Items.Count > 0 Then
                strchvPlanType = drdPlantype.SelectedValue
            End If
            strchvSource = "TerminationWatcher"
            If (strguiPersId <> String.Empty AndAlso strguiFundEventId <> String.Empty AndAlso strchrType <> String.Empty AndAlso strchvPlanType <> String.Empty AndAlso strchvSource <> String.Empty) Then
                strResult = YMCARET.YmcaBusinessObject.TerminationWatcherBO.SaveTerminationWatcherData(strguiPersId, strguiFundEventId, strchrType, strchvPlanType, strchvSource)
            End If
            If strResult = "1" Then
                'Anudeep:14-nov-2012 Changes made as per Observations listed in bugtraker for yrs 5.0-1484
                Session("Refresh") = "YES"
                'Start,Anudeep:05-nov-2012 Changes made as per Observations listed in bugtraker for yrs 5.0-1484
                Dim dtAdded As New DataTable
                Dim drAdded As DataRow
                If Not ViewState("dtAdded") Is Nothing Then
                    dtAdded = ViewState("dtAdded")
                Else
                    dtAdded.Columns.Add("strguiFundEventId", GetType(String))
                    dtAdded.Columns.Add("strchrType", GetType(String))
                    dtAdded.Columns.Add("strchvPlanType", GetType(String))
                End If
                drAdded = dtAdded.NewRow()
                If strchrType = "Withdrawal" Then
                    strchrType = "Wdl"
                Else
                    strchrType = "Ret"
                End If
                If strchvPlanType.ToUpper() = "RETIREMENT" Then
                    strchvPlanType = "Ret"
                ElseIf strchvPlanType.ToUpper() = "SAVINGS" Then
                    strchvPlanType = "Sav"
                Else
                    strchvPlanType = "Both"
                End If
                For i As Integer = 0 To dtAdded.Rows.Count - 1
                    If dtAdded.Rows(i)("strguiFundEventId") = strguiFundEventId Then
                        If dtAdded.Rows(i)("strchrType") = strchrType Then
                            strchrType = strchrType
                            strchvPlanType = "Both"
                        Else
                            strchrType = dtAdded.Rows(i)("strchrType") + "/" + strchrType
                            strchvPlanType = dtAdded.Rows(i)("strchvPlanType") + "/" + strchvPlanType
                        End If

                        dtAdded.Rows.RemoveAt(i)
                    End If
                Next
                drAdded("strguiFundEventId") = strguiFundEventId
                drAdded("strchrType") = strchrType
                drAdded("strchvPlanType") = strchvPlanType
                dtAdded.AcceptChanges()
                dtAdded.Rows.Add(drAdded)
                dtAdded.GetChanges()
                ViewState("dtAdded") = dtAdded
                If Not ViewState("Add_Sort") Is Nothing Then
                    dv.Sort = ViewState("Add_Sort")
                End If
                If Not ViewState("Add_PageIndex") Is Nothing Then
                    gvAdd.PageIndex = ViewState("Add_PageIndex")
                End If
                gvAdd.DataSource = dv
                gvAdd.DataBind()
                'End, Anudeep:05-nov-2012 Changes made as per Observations listed in bugtraker for yrs 5.0-1484 ,end
            End If
            Return strResult
        Catch
            Throw
        End Try
    End Function
    Protected Sub btnWithdrawalFind_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnWithdrawalFind.Click
        Try
            disablecontrols()
            gvAdd.SelectedIndex = -1
            gvAdd.PageIndex = 0
            Populatedata()
        Catch
            Throw
        End Try
    End Sub

    Protected Sub btnAddOk_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAddOk.Click
		Dim checkSecurity As String = SecurityCheck.Check_Authorization("btnProcess", Convert.ToInt32(Session("LoggedUserKey")))
        Dim strResult As String
        Dim strChvplantype As String
        Try
            If Not checkSecurity.Equals("True") Then
                MessageBox.Show(PlaceHolder1, "Termination Watcher", checkSecurity, MessageBoxButtons.Stop)
                Exit Sub
            End If
            strResult = SaveData()
            'Anudeep:05-nov-2012 Changes made as per Observations listed in bugtraker for yrs 5.0-1484 
            If strResult = "-2" Then
                strChvplantype = drdPlantype.SelectedValue
                MessageBox.Show(PlaceHolder1, "Termination Watcher", messageFromResourceFile("MESSAGE_TW_ADDUSER_RECORD_EXISTS", strChvplantype), MessageBoxButtons.OK)
            ElseIf strResult = "-1" Then
                MessageBox.Show(PlaceHolder1, "Termination Watcher", messageFromResourceFile("MESSAGE_TW_ADDUSER_INSERT_ERROR", ""), MessageBoxButtons.OK)
            ElseIf strResult = "1" Then

                Session("Refresh") = "YES"
                Dim msg As String
                msg = msg + "<Script Language='JavaScript'>"
                msg = msg + "window.opener.document.forms(0).submit();"
                msg = msg + "self.close();"
                msg = msg + "</Script>"
                Response.Write(msg)
            End If
        Catch
            Throw
        End Try

    End Sub
    'Gridview Pageindexing
	Private Sub gvAdd_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvAdd.PageIndexChanging
        Dim dv As DataView
        Dim l_dataset As DataSet
        Try
            
            gvAdd.SelectedIndex = -1
            disablecontrols()
            If Not Session("dgAddEmployee") Is Nothing Then
                l_dataset = Session("dgAddEmployee")
                dv = l_dataset.Tables(0).DefaultView
                Me.gvAdd.PageIndex = e.NewPageIndex
                If Not ViewState("Add_Sort") Is Nothing Then
                    dv.Sort = ViewState("Add_Sort")
                End If
                gvAdd.DataSource = dv
                gvAdd.DataBind()
                ViewState("Add_PageIndex") = e.NewPageIndex
            End If
        Catch
            Throw
        End Try
    End Sub
    'Start: Anudeep:05-11-2012 Added below line as Per Observations listed in Bugtraker 
    'Handles gridview Rowdatabound adds the plantype and type in the related textboxes
    Private Sub gvAdd_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvAdd.RowDataBound
        Dim l_dataset As DataTable
        Try
            If Not ViewState("dtAdded") Is Nothing Then
                If e.Row.RowIndex <> -1 Then
                    l_dataset = ViewState("dtAdded")
                    For i As Integer = 0 To l_dataset.Rows.Count - 1
                        If l_dataset.Rows(i)("strguiFundEventId").ToString() = gvAdd.DataKeys(e.Row.RowIndex).Values("FundUniqueId").ToString() Then
                            e.Row.Cells(8).Text = l_dataset.Rows(i)("strchrType")
                            e.Row.Cells(9).Text = l_dataset.Rows(i)("strchvPlanType")
                        End If
                    Next
                End If
            End If
        Catch
            Throw
        End Try
    End Sub
    'End: Anudeep:05-11-2012 Added below line as Per Observations listed in Bugtraker 
    'Handles Gridview Selected index changed
	Private Sub gvAdd_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvAdd.SelectedIndexChanged
        Dim ImagebuttonSelected, ImageSelect As ImageButton
        Dim dsPlantType As New DataSet
        Dim strfundEventId As String
        Try
            For i As Integer = 0 To gvAdd.Rows.Count - 1
                ImagebuttonSelected = gvAdd.Rows(i).FindControl("ImagebuttonSelected")
                ImageSelect = gvAdd.Rows(i).FindControl("ImageButtonSelect")
                ImagebuttonSelected.Visible = False
                ImageSelect.Visible = True
            Next
            ImagebuttonSelected = gvAdd.Rows(gvAdd.SelectedIndex).FindControl("ImagebuttonSelected")
            ImageSelect = gvAdd.Rows(gvAdd.SelectedIndex).FindControl("ImageButtonSelect")
            dsAddemployee = Session("dgAddEmployee")
            If HelperFunctions.isNonEmpty(dsAddemployee) Then
                strfundEventId = gvAdd.DataKeys(gvAdd.SelectedIndex).Values("FundUniqueId").ToString()
            End If

            dsPlantType = YMCARET.YmcaBusinessObject.TerminationWatcherBO.GetApplicantPlanType(strfundEventId)
            'Anudeep:19-11-2012 Changes made as per Observations listed in bugtraker for yrs 5.0-1484 on 16-nov-2012
            If HelperFunctions.isNonEmpty(dsPlantType) Then
                drdPlantype.Items.Clear()
                drdPlantype.DataSource = dsPlantType
                drdPlantype.DataTextField = "PlanType"
                drdPlantype.DataValueField = "PlanType".ToUpper()
                drdPlantype.DataBind()
                drdPlantype.Items.Insert(0, New ListItem("-Select-", "-1"))
                drdPlantype.SelectedIndex = 0
                If dsPlantType.Tables(0).Rows.Count = 2 Then
                    drdPlantype.Items.Insert(3, New ListItem("BOTH", "BOTH"))
                End If
            Else
                drdPlantype.Items.Clear()
                drdPlantype.Items.Insert(0, New ListItem("-Select-", "-1"))
                drdPlantype.SelectedIndex = 0
            End If

            ImagebuttonSelected.Visible = True
            ImageSelect.Visible = False
            drdPlantype.Visible = True
            lblPlanType.Visible = True
            lblPlanTypeErr.Visible = True
            btnAddOk.Enabled = True
            btnAdd.Enabled = True

            If Request.QueryString("form").ToString().Trim() <> "Status" Then
                drdType.SelectedValue = Request.QueryString("form").ToString().Trim()
                drdType.Enabled = False
                lblType.Visible = True
                lblType.Enabled = False
                drdType.Visible = True
            Else
                lblType.Visible = True
                lblTypeErr.Visible = True
                drdType.Visible = True
            End If
        Catch
            Throw
        End Try
	End Sub
    'Handles Gridview sorting
    Private Sub gvAdd_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvAdd.Sorting
        Me.gvAdd.SelectedIndex = -1
       
        Try
            Sortgrid(e)
            disablecontrols()
        Catch
            Throw
        End Try
    End Sub
    'Sorts the grid 
    Public Sub Sortgrid(ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs)
        Dim l_dataset As DataSet
        Dim dv As DataView
        Try
            If Not Session("dgAddEmployee") Is Nothing Then
                l_dataset = Session("dgAddEmployee")
                Dim SortExpression As String
                SortExpression = e.SortExpression
                dv = l_dataset.Tables(0).DefaultView
                dv.Sort = SortExpression
                If Not ViewState("Add_Sort") Is Nothing Then
                    If ViewState("Add_Sort").ToString.Trim.EndsWith("ASC") Then
                        dv.Sort = SortExpression + " DESC"
                    Else
                        dv.Sort = SortExpression + " ASC"
                    End If
                Else
                    dv.Sort = SortExpression + " ASC"
                End If
                gvAdd.DataSource = dv
                gvAdd.DataBind()
                ViewState("Add_Sort") = dv.Sort()
            End If
        Catch
            Throw
        End Try
    End Sub
    'Gets the message from resource file
	Private Function messageFromResourceFile(ByVal strMessage As String, ByVal strParameter As String) As String
        Dim strReturnMessage As String = String.Empty
		Dim TW As Resources.TerminationWatcher
		Try
            If strParameter = String.Empty Then
                strReturnMessage = TW.ResourceManager.GetString(strMessage).Trim()
            ElseIf strMessage = "MESSAGE_TW_ADDUSER_RECORD_EXISTS" Then
                strReturnMessage = String.Format(Resources.TerminationWatcher.MESSAGE_TW_ADDUSER_RECORD_EXISTS, strParameter)
            End If
            Return strReturnMessage
        Catch
            Throw
        End Try
    End Function
    'clear search
    ' Added by Anudeep:on 31-01-2012 As perobservations 
    Private Sub btnWithdrawalClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnWithdrawalClear.Click
        txtFirstName.Text = ""
        txtLastName.Text = ""
        txtFundno.Text = ""
        txtSSNo.Text = ""
        lblTotalCount.Text = ""
        disablecontrols()
        gvAdd.PageIndex = 0
        gvAdd.SelectedIndex = -1
        gvAdd.DataSource = Nothing
        gvAdd.DataBind()

    End Sub
    'Anudeep:05-nov-2012 Changes made as per Observations listed in bugtraker for yrs 5.0-1484 ,start
	Private Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click
		Dim checkSecurity As String = SecurityCheck.Check_Authorization("btnProcess", Convert.ToInt32(Session("LoggedUserKey")))
		
		Dim strResult As String
		Dim strChvplantype As String
		Try
			 If Not checkSecurity.Equals("True") Then
				MessageBox.Show(PlaceHolder1, "Termination Watcher", checkSecurity, MessageBoxButtons.Stop)
				Exit Sub
			End If
			strResult = SaveData()
			'Anudeep:22-11-2012 Commented For Showing Dropdown After Clicking Add button
			'disablecontrols()
			lblAbvr.Visible = True
			If strResult = "-2" Then
				strChvplantype = drdPlantype.SelectedValue
				MessageBox.Show(PlaceHolder1, "Termination Watcher", messageFromResourceFile("MESSAGE_TW_ADDUSER_RECORD_EXISTS", strChvplantype), MessageBoxButtons.OK)
				'Anudeep:14-nov-2012 Changes made as per Observations listed in bugtraker for yrs 5.0-1484
				lblAbvr.Visible = False
			ElseIf strResult = "-1" Then
				MessageBox.Show(PlaceHolder1, "Termination Watcher", messageFromResourceFile("MESSAGE_TW_ADDUSER_INSERT_ERROR", ""), MessageBoxButtons.OK)
				'Anudeep:14-nov-2012 Changes made as per Observations listed in bugtraker for yrs 5.0-1484
				lblAbvr.Visible = False
			End If
		Catch
			Throw
		End Try
	End Sub
    'Start :Anudeep:05-nov-2012 Changes made as per Observations listed in bugtraker for yrs 5.0-1484 
    Private Sub btnAddClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddClose.Click
        Try
            Dim msg As String
            msg = msg + "<Script Language='JavaScript'>"
            'Anudeep:06-nov-2012 Changes made as per Observations listed in bugtraker for yrs 5.0-1484
            If Session("Refresh") = "YES" Then
                msg = msg + "window.opener.document.forms(0).submit();"
            End If
            msg = msg + "self.close();"
            msg = msg + "</Script>"
            Response.Write(msg)
        Catch
            Throw
        End Try
    End Sub
    'End: Anudeep:05-nov-2012 Changes made as per Observations listed in bugtraker for yrs 5.0-1484 
End Class
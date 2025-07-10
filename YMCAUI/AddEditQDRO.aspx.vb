'*******************************************************************************
' Copyright YMCA Retirement Fund All Rights Reserved. 
' Project Name		:	YMCA-YRS
' FileName			:	AddEditQDRO.aspx.vb
'*******************************************************************************
' Cache-Session     :   Vipul 03Feb06
'*******************************************************************************
'***************************************************************************************************************************************************
'Modification History
'***************************************************************************************************************************************************
'Modified By             Date                Description
'***************************************************************************************************************************************************
'Ashutosh Patil          12-Jul-2007         YREN-3589
'Neeraj Singh            12/Nov/2009         Added form name for security issue YRS 5.0-940 
'Manthan Rajguru         2015.09.16          YRS-AT-2550: YRS data cleanup: copyright comments and namespace/reference (Changed the Namespace/reference from 'Infotech' to 'YMCARET')
'Chandra sekar.c         2016.07.05          YRS-AT-2481 - YRS and/or PHR enhancement-store QDRO beneficiary info in participant's acct after split (TrackIT 22116)
'Manthan Rajguru         2016.08.30          YRS-AT-2488 -  YRS enh: PART 1 of 4:RMD's for alternate payees (QDRO recipients) (TrackIT 22284)      
'***************************************************************************************************************************************************
Public Class AddEditQDRO
    Inherits System.Web.UI.Page

    'below line is Added by Neeraj for issue id YRS 5.0-940 
    Dim strFormName As String = New String("AddEditQDRO.aspx")
    'End issue id YRS 5.0-940

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    'Start - Manthan Rajguru | 2016.08.30 | YRS-AT-2488 | Constant variable declared for hiding column
#Region "Global Declaration"
    Const BENEFICIARY_SPOUSE As Integer = 9
#End Region
    'End - Manthan Rajguru | 2016.08.30 | YRS-AT-2488 | Constant variable declared for hiding column
    Protected WithEvents PlaceHolder1 As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents ButtonCancel As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonAdd As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonSave As System.Web.UI.WebControls.Button 'Chandrasekar - 2016.07.05 - YRS-AT-2481
    Protected WithEvents Menu1 As skmMenu.Menu
    Protected WithEvents ButtonUpdate As System.Web.UI.WebControls.Button
    Protected WithEvents LabelQdroType As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxQdroType As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelStatus As System.Web.UI.WebControls.Label
    Protected WithEvents LabelStatusDate As System.Web.UI.WebControls.Label
    Protected WithEvents LabelDraftDate As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxStatusDate As YMCAUI.DateUserControl
    Protected WithEvents TextBoxDraftDate As YMCAUI.DateUserControl
    Protected WithEvents DataGridQdroInfo As System.Web.UI.WebControls.DataGrid
    Protected WithEvents Literal1 As System.Web.UI.WebControls.Literal
    Protected WithEvents RequiredFieldValidator4 As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents DropDownListStatus As System.Web.UI.WebControls.DropDownList
    Protected WithEvents Headercontrol As YMCA_Header_WebUserControl
    'START : Chandrasekar - 2016.07.05 - YRS-AT-2481 - YRS and/or PHR enhancement-store QDRO beneficiary info in participant's acct after split (TrackIT 22116)
    Protected WithEvents DatagridQDROInformation As System.Web.UI.WebControls.DataGrid
    Protected WithEvents PlaceHolderAddEditQDRO As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents btnClose As System.Web.UI.WebControls.Button
    Protected WithEvents divQDROInformation As System.Web.UI.HtmlControls.HtmlGenericControl
    Protected WithEvents lbMgs As System.Web.UI.WebControls.Label
    Private Const QDRO_REQUESTID As Integer = 2
    Private Const QDRO_TYPE As Integer = 3
    Private Const QDRO_STATUS As Integer = 4
    Private Const QDRO_STATUSDATE As Integer = 5
    Private Const QDRO_DRAFTDATE As Integer = 6
    Private Const QDRO_STATUS_CHCK As Integer = 7
    'END : Chandrasekar - 2016.07.05 - YRS-AT-2481 - YRS and/or PHR enhancement-store QDRO beneficiary info in participant's acct after split (TrackIT 22116)
    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    'Start - Manthan Rajguru | 2016.08.30 | YRS-AT-2488 | Initialized dropdown control for spouse
    Protected WithEvents DropDownListSpouse As System.Web.UI.WebControls.DropDownList
    Protected WithEvents LabelSpouse As System.Web.UI.WebControls.Label
    'End - Manthan Rajguru | 2016.08.30 | YRS-AT-2488 | Initialized dropdown control for spouse
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region
#Region "Page Load"
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        Dim l_string_PersId As String
        Dim l_string_FundId As String
        Dim l_dataset_Qdro As DataSet
        'Start - Manthan Rajguru | 2016.08.30 | YRS-AT-2488 | Commented existing variable as not used and declared boolean variable
        'Dim l_string_QDROType As String 'Added by Dilip BT-593
        Dim blnIsRetired As Boolean
        'End - Manthan Rajguru | 2016.08.30 | YRS-AT-2488 | Commented existing variable as not used and declared boolean variable

        If Session("LoggedUserKey") Is Nothing Then
            Response.Redirect("Login.aspx", False)
        End If
        Try
            l_string_PersId = Session("PersId")
            l_string_FundId = Session("FundId")

            If Not Session("FundNo") Is Nothing Then
                Headercontrol.PageTitle = "Participant Information - Add/Update QDRO"
                Headercontrol.FundNo = Session("FundNo").ToString().Trim()
            End If

            'Session("PersId") = "BC445264-7B21-4888-98B3-C4AC7C337900"
            'Session("FundId") = "8B68588B-8BCE-4EA4-9A13-DA0C48FA1041"
            'l_string_PersId = "BC445264-7B21-4888-98B3-C4AC7C337900"
            'l_string_FundId = "8B68588B-8BCE-4EA4-9A13-DA0C48FA1041"
            If Not IsPostBack Then
                Session("Edit_QDRO_Sort") = Nothing
                'Start - Manthan Rajguru | 2016.08.30 | YRS-AT-2488 | Added code to display dropdown or not
                If Not Session("ISRetired") Is Nothing Then
                    blnIsRetired = Session("ISRetired")
                End If
                If (blnIsRetired = True) Then
                    DropDownListSpouse.Visible = False
                    LabelSpouse.Visible = False
                End If
                'End - Manthan Rajguru | 2016.08.30 | YRS-AT-2488 | Added code to display dropdown or not
                'Added By Dilip  BT-593 for making QDRO Request Date 24-09-2008
                'START Commented by : Chandrasekar - 2016.07.05 - YRS-AT-2481 - YRS and/or PHR enhancement-store QDRO beneficiary info in participant's acct after split (TrackIT 22116)
                'Dim l_Boolean_ISRetired As Boolean
                'If Not Session("ISRetired") Is Nothing Then
                '    l_Boolean_ISRetired = Session("ISRetired")
                'End If
                'If (l_Boolean_ISRetired = True) Then
                '    l_string_QDROType = "RET"
                'ElseIf (l_Boolean_ISRetired = False) Then
                '    l_string_QDROType = "NONRET"
                'End If
                'END Commented by : Chandrasekar - 2016.07.05 - YRS-AT-2481 - YRS and/or PHR enhancement-store QDRO beneficiary info in participant's acct after split (TrackIT 22116)
                'Added By Dilip  BT-593 for making QDRO Request Date 24-09-2008
                'START : Chandrasekar - 2016.07.05 - YRS-AT-2481 - YRS and/or PHR enhancement-store QDRO beneficiary info in participant's acct after split (TrackIT 22116)
                lbMgs.Text = ""
                divQDROInformation.Attributes("style") = "display:none"
                LoadQDRORequestGrid(l_string_PersId)
                'START: Manthan Rajguru | 2016.09.07 | YRS-AT-2488 | Populating dropdown values
                If (blnIsRetired = False) Then
                    If Not ViewState("QDRORequestID") Is Nothing Then
                        PopulateSpouseDetailsDropDownList(ViewState("QDRORequestID"))
                    End If
                End If
                'END: Manthan Rajguru | 2016.09.07 | YRS-AT-2488 | Populating dropdown values
                'END : Chandrasekar - 2016.07.05 - YRS-AT-2481 - YRS and/or PHR enhancement-store QDRO beneficiary info in participant's acct after split (TrackIT 22116)

                'Fetching data for the Qdro for the particular Participant
                'START Commented by : Chandrasekar - 2016.07.05 - YRS-AT-2481 - YRS and/or PHR enhancement-store QDRO beneficiary info in participant's acct after split (TrackIT 22116)

                'l_dataset_Qdro = YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.GetQDROInfo(l_string_PersId, l_string_QDROType)
                'If l_dataset_Qdro.Tables("QdroInfo").Rows.Count > 0 Then
                '    Me.DataGridQdroInfo.DataSource = l_dataset_Qdro
                '    ViewState("DS_Sort_EditQDRO") = l_dataset_Qdro
                '    Me.DataGridQdroInfo.DataBind()
                '    TextBoxQdroType.Text = Me.DataGridQdroInfo.Items(0).Cells(2).Text.Trim()
                '    'Commented and Modified By Ashutosh Patil as on 12-Jul-2007
                '    'YREN-3589. 
                '    'Start Ashutosh Patil 12-Jul-2007
                '    'DropDownListStatus.SelectedItem.Text = Me.DataGridQdroInfo.Items(0).Cells(3).Text.Trim()

                '    'Added by Paramesh K. on Sept 26th 2008
                '    '*****************
                '    'selecting the appropriate status value in the dropdownlist based on the value
                '    DropDownListStatus.ClearSelection()
                '    DropDownListStatus.Items.FindByValue(Me.DataGridQdroInfo.Items(0).Cells(6).Text).Selected = True
                '    'comment below code 
                '    'Me.DropDownListStatus.SelectedValue = Me.DataGridQdroInfo.Items(0).Cells(6).Text.Trim()
                '    '*****************

                '    'End Ashutosh Patil 12-Jul-2007
                '    TextBoxStatusDate.Text = Me.DataGridQdroInfo.Items(0).Cells(4).Text.Trim()
                '    TextBoxDraftDate.Text = Me.DataGridQdroInfo.Items(0).Cells(5).Text.Trim()
                'End If
                'END Commented by : Chandrasekar - 2016.07.05 - YRS-AT-2481 - YRS and/or PHR enhancement-store QDRO beneficiary info in participant's acct after split (TrackIT 22116)

                TextBoxQdroType.Enabled = False
                TextBoxDraftDate.Enabled = False
                TextBoxStatusDate.Enabled = False
                DropDownListStatus.Enabled = False

            End If
            'Finding whether there is any pending record or not
            Dim _icount As Integer
            _icount = 0
            While _icount < Me.DataGridQdroInfo.Items.Count
                If Me.DataGridQdroInfo.Items(_icount).Cells(QDRO_STATUS).Text = "Pending" Then 'Chandrasekar - 2016.07.05 - YRS-AT-2481 - Changed hardcoded indexing by constants
                    Me.ButtonAdd.Enabled = False
                End If
                _icount = _icount + 1

            End While

        Catch ex As Exception

            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message)

        End Try

    End Sub
#End Region
    'START : Chandrasekar - 2016.07.05 - YRS-AT-2481 - YRS and/or PHR enhancement-store QDRO beneficiary info in participant's acct after split (TrackIT 22116)
    Private Sub DataGridQdroInfo_ItemCommand(source As Object, e As DataGridCommandEventArgs) Handles DataGridQdroInfo.ItemCommand
        Try
            If e.CommandName = "ViewSelect" Then                         
                    Me.DataGridQdroInfo.SelectedIndex = e.Item.ItemIndex
                    Dim l_dataset_QdroBenecificaryInfo As DataSet
                    Dim l_string_QdroRequestId As String
                    lbMgs.Text = ""
                    divQDROInformation.Attributes("style") = "display:block"
                    l_string_QdroRequestId = e.Item.Cells(QDRO_REQUESTID).Text.Trim
                    l_dataset_QdroBenecificaryInfo = YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.GetQDROSplitDetailsByRequestId(l_string_QdroRequestId)
                    If HelperFunctions.isNonEmpty(l_dataset_QdroBenecificaryInfo) Then
                        divQDROInformation.Attributes("style") = "display:block"
                        DatagridQDROInformation.DataSource = l_dataset_QdroBenecificaryInfo
                    DatagridQDROInformation.DataBind()
                Else
                    DatagridQDROInformation.DataSource = Nothing
                    DatagridQDROInformation.DataBind()                  
                    lbMgs.Text = getmessage("MESSAGE_PARTICIPANT_SETTLEMENT_PENDING_FOR_REQUEST")
                    divQDROInformation.Attributes("style") = "display:block"
                End If
            ElseIf e.CommandName = "EditSelect" Then
                Dim strQDRORequestID As String 'Manthan Rajguru | 2016.08.30 | YRS-AT-2488 | Declared variable to store Request ID
                Dim blnIsRetired As Boolean
                'START : Chandrasekar - 2016.07.05 - YRS-AT-2481 - YRS and/or PHR enhancement-store QDRO beneficiary info in participant's acct after split (TrackIT 22116)
                ButtonSave.Enabled = True
                ButtonCancel.Enabled = True
                'END : Chandrasekar - 2016.07.05 - YRS-AT-2481 - YRS and/or PHR enhancement-store QDRO beneficiary info in participant's acct after split (TrackIT 22116)
                Me.DataGridQdroInfo.SelectedIndex = e.Item.ItemIndex
                TextBoxQdroType.Text = e.Item.Cells(QDRO_TYPE).Text.Trim()
                '*****************
                'selecting the appropriate status value in the dropdownlist based on the value
                DropDownListStatus.ClearSelection()
                DropDownListStatus.Items.FindByValue(e.Item.Cells(QDRO_STATUS_CHCK).Text).Selected = True
                'DropDownListStatus.SelectedItem.Text = Me.DataGridQdroInfo.SelectedItem.Cells(3).Text.Trim()
                '*****************
                TextBoxStatusDate.Text = e.Item.Cells(QDRO_STATUSDATE).Text.Trim()
                TextBoxDraftDate.Text = e.Item.Cells(QDRO_DRAFTDATE).Text.Trim()
                Literal1.Text = "Edit"
                TextBoxDraftDate.Enabled = True
                DropDownListStatus.Enabled = True
                'Start - Manthan Rajguru | 2016.08.30 | YRS-AT-2488 | Populating spouse beneficairy values in Dropdown
                ButtonAdd.Enabled = False 'Mantahn Rajguru | 2016.08.30 | YRS-AT-2488 | Disabling control
                strQDRORequestID = e.Item.Cells(QDRO_REQUESTID).Text.Trim
                If Not Session("ISRetired") Is Nothing Then
                    blnIsRetired = Session("ISRetired")
                End If
                If Not blnIsRetired Then
                    PopulateSpouseDetailsDropDownList(strQDRORequestID)
                    DropDownListSpouse.Enabled = True
                    'End - Manthan Rajguru | 2016.08.30 | YRS-AT-2488 | Populating spouse beneficairy values in Dropdown
                End If
            End If
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            HelperFunctions.LogException("TextBoxSSNo_TextChanged", ex) 'Manthan Rajguru | 2016.09.02 | YRS-AT-2488 | Logging exception message
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message)
        End Try
    End Sub
    'END : Chandrasekar - 2016.07.05 - YRS-AT-2481 - YRS and/or PHR enhancement-store QDRO beneficiary info in participant's acct after split (TrackIT 22116)

    Private Sub DataGridQdroInfo_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DataGridQdroInfo.SelectedIndexChanged
        Try
            'START Commented by : Chandrasekar - 2016.07.05 - YRS-AT-2481 - YRS and/or PHR enhancement-store QDRO beneficiary info in participant's acct after split (TrackIT 22116)
            'Dim i As Integer
            ' Dim l_button_select As ImageButton
            'END Commented by : Chandrasekar - 2016.07.05 - YRS-AT-2481 - YRS and/or PHR enhancement-store QDRO beneficiary info in participant's acct after split (TrackIT 22116)
            'While i < Me.DataGridQdroInfo.Items.Count
            '    If i = Me.DataGridQdroInfo.SelectedIndex Then
            '        l_button_select = New ImageButton
            '        l_button_select = Me.DataGridQdroInfo.Items(i).FindControl("ImageButtonSelect")
            '        If Not l_button_select Is Nothing Then
            '            l_button_select.ImageUrl = "images\selected.gif"
            '        End If

            '    Else
            '        l_button_select = New ImageButton
            '        l_button_select = Me.DataGridQdroInfo.Items(i).FindControl("ImageButtonSelect")
            '        If Not l_button_select Is Nothing Then
            '            l_button_select.ImageUrl = "images\select.gif"
            '        End If

            '    End If
            '    i = i + 1

            'End While
            'START Commented by : Chandrasekar - 2016.07.05 - YRS-AT-2481 - YRS and/or PHR enhancement-store QDRO beneficiary info in participant's acct after split (TrackIT 22116)

            'TextBoxQdroType.Text = Me.DataGridQdroInfo.SelectedItem.Cells(QDRO_TYPE).Text.Trim()
            ''Added by Paramesh K. on Sept 26th 2008
            ''*****************
            ''selecting the appropriate status value in the dropdownlist based on the value
            'DropDownListStatus.ClearSelection()
            'DropDownListStatus.Items.FindByValue(Me.DataGridQdroInfo.SelectedItem.Cells(7).Text).Selected = True

            ''comment below code 
            ''DropDownListStatus.SelectedItem.Text = Me.DataGridQdroInfo.SelectedItem.Cells(3).Text.Trim()
            ''*****************
            'TextBoxStatusDate.Text = Me.DataGridQdroInfo.SelectedItem.Cells(QDRO_STATUSDATE).Text.Trim()
            'TextBoxDraftDate.Text = Me.DataGridQdroInfo.SelectedItem.Cells(QDRO_DRAFTDATE).Text.Trim()
            'Literal1.Text = "Edit"

            'TextBoxDraftDate.Enabled = True

            'DropDownListStatus.Enabled = True
            'Literal1.Text = "Edit"
            'END Commented by : Chandrasekar - 2016.07.05 - YRS-AT-2481 - YRS and/or PHR enhancement-store QDRO beneficiary info in participant's acct after split (TrackIT 22116)

        Catch ex As Exception

            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message)

        End Try

    End Sub

    Private Sub DataGridQdroInfo_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DataGridQdroInfo.ItemDataBound
        Try
            e.Item.Cells(QDRO_REQUESTID).Visible = False 'Chandrasekar - 2016.07.05 - YRS-AT-2481 -  Changed hardcoded indexing by constants
            'Ashutosh Patil as on 12-Jul-2007
            'YREN-3589
            'Start Ashutosh Patil 12-Jul-2007
            e.Item.Cells(QDRO_STATUS_CHCK).Visible = False 'Chandrasekar - 2016.07.05 - YRS-AT-2481 -  Changed hardcoded indexing by constants
            'End Ashutosh Patil 12-Jul-2007
        Catch ex As Exception

            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message)

        End Try


    End Sub

    Private Sub ButtonAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonAdd.Click
        Try
            'Added By Dilip for making QDRO Request for Retired Person Date 08-09-2008
            Dim l_Boolean_ISRetired As Boolean
            If Not Session("ISRetired") Is Nothing Then
                l_Boolean_ISRetired = Session("ISRetired")
            End If
            If (l_Boolean_ISRetired = True) Then
                TextBoxQdroType.Text = "RET"
            ElseIf (l_Boolean_ISRetired = False) Then
                TextBoxQdroType.Text = "NONRET"
            End If
            'Added By Dilip for making QDRO Request for Retired Person 08-09-2008

            Literal1.Text = "Add"
            TextBoxDraftDate.Text = System.DateTime.Today()
            TextBoxStatusDate.Text = System.DateTime.Today()
            DropDownListStatus.SelectedValue = "Pend"
            TextBoxDraftDate.Enabled = True
            DropDownListStatus.Enabled = True
            'START : Chandrasekar - 2016.07.05 - YRS-AT-2481 - YRS and/or PHR enhancement-store QDRO beneficiary info in participant's acct after split (TrackIT 22116)
            ButtonSave.Enabled = True
            ButtonCancel.Enabled = True
            'END : Chandrasekar - 2016.07.05 - YRS-AT-2481 - YRS and/or PHR enhancement-store QDRO beneficiary info in participant's acct after split (TrackIT 22116)
            ButtonAdd.Enabled = False 'Mantahn Rajguru | 2016.08.30 | YRS-AT-2488 | Disabling control
            'START : Chandrasekar - 2016.07.05 - YRS-AT-2481 - YRS and/or PHR enhancement-store QDRO beneficiary info in participant's acct after split (TrackIT 22116)
            lbMgs.Text = ""
            divQDROInformation.Attributes("style") = "display:none"
            'END : Chandrasekar - 2016.07.05 - YRS-AT-2481 - YRS and/or PHR enhancement-store QDRO beneficiary info in participant's acct after split (TrackIT 22116)
            'Start - Mantahn Rajguru | 2016.08.30 | YRS-AT-2488 | Populating dropdown values for QDRO Beneficiary and enabling control
            PopulateSpouseDetailsDropDownList("")
            DropDownListSpouse.Enabled = True
            'End - Mantahn Rajguru | 2016.08.30 | YRS-AT-2488 | Populating dropdown values for QDRO Beneficiary and enabling control
        Catch ex As Exception

            Dim l_String_Exception_Message As String
            HelperFunctions.LogException("TextBoxSSNo_TextChanged", ex) 'Manthan Rajguru | 2016.09.02 | YRS-AT-2488 | Logging exception message
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message)

        End Try



    End Sub

    Private Sub ButtonUpdate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonUpdate.Click
        Try

            TextBoxDraftDate.Enabled = True

            DropDownListStatus.Enabled = True
            Literal1.Text = "Edit"
        Catch ex As Exception

            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message)

        End Try

    End Sub

    Private Sub ButtonSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonSave.Click 'Added by Chandrasekar - 2016.07.05 - YRS-AT-2481 - Changed  the Name of button "OK" to "Save" '
        Dim l_string_QDROType As String 'Added by Dilip BT-593
        Try
            Session("Edit_QDRO_Sort") = Nothing
            Dim l_string_Output As String
            Dim l_dataset_Qdro As DataSet
            Dim l_Boolean_ISRetired As String
            Dim strODROType As String ' Chandrasekar - 2016.07.05 - YRS-AT-2481
            'Added by Dilip BT-593
            If Not Session("ISRetired") Is Nothing Then
                l_Boolean_ISRetired = Session("ISRetired")
            End If
            'Added by Dilip BT-593
            'START : Chandrasekar - 2016.07.05 - YRS-AT-2481 - YRS and/or PHR enhancement-store QDRO beneficiary info in participant's acct after split (TrackIT 22116)
            lbMgs.Text = ""
            divQDROInformation.Attributes("style") = "display:none"
            'END : Chandrasekar - 2016.07.05 - YRS-AT-2481 - YRS and/or PHR enhancement-store QDRO beneficiary info in participant's acct after split (TrackIT 22116)
            If Literal1.Text = "Add" Then
                'Commented and Modified By Ashutosh Patil as on 12-Jul-2007
                'YREN-3589. DropDownListStatus.SelectedValue==> Picks up selected value luike Pending,Compelted,Canceled. 
                'Actual value should pick up Pend,Can,Comp.
                'Start Ashutosh Patil 12-Jul-2007
                'l_string_Output = YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.AddQDROInfo(Session("PersId"), Session("FundId"), TextBoxQdroType.Text, DropDownListStatus.SelectedValue, TextBoxStatusDate.Text, TextBoxDraftDate.Text)
                'If DropDownListStatus.SelectedItem.Value <> "" Then
                strODROType = IIf(l_Boolean_ISRetired = True, "RET", "NONRET") ' Chandrasekar - 2016.07.05 - YRS-AT-2481
                l_string_Output = YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.AddQDROInfo(Session("PersId"), Session("FundId"), strODROType, DropDownListStatus.SelectedItem.Value, TextBoxStatusDate.Text, TextBoxDraftDate.Text)
                ' Else
                '  MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Invalid Status", MessageBoxButtons.Stop)
                ' End If

                'End Ashutosh Patil 12-Jul-2007
            Else
                Dim l_string_UniqueId As String
                'START Commented by : Chandrasekar - 2016.07.05 - YRS-AT-2481 - YRS and/or PHR enhancement-store QDRO beneficiary info in participant's acct after split (TrackIT 22116)
                'If Me.DataGridQdroInfo.SelectedIndex = -1 Then
                '    Me.DataGridQdroInfo.SelectedIndex = 0
                'End If
                'END Commented by : Chandrasekar - 2016.07.05 - YRS-AT-2481 - YRS and/or PHR enhancement-store QDRO beneficiary info in participant's acct after split (TrackIT 22116)
                If Me.DataGridQdroInfo.Items.Count <> 0 Then
                    l_string_UniqueId = Me.DataGridQdroInfo.SelectedItem.Cells(QDRO_REQUESTID).Text ' Added by Chandrasekar - 2016.07.05 - YRS-AT-2481 - Changed hardcoded indexing by constants
                    'Commented and Modified By Ashutosh Patil as on 12-Jul-2007
                    'YREN-3589. DropDownListStatus.SelectedValue==> Picks up selected value luike Pending,Compelted,Canceled. 
                    'Actual value should pick up Pend,Can,Comp.
                    'Start Ashutosh Patil 12-Jul-2007
                    'l_string_Output = YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.UpdateQDROInfo(l_string_UniqueId, DropDownListStatus.SelectedValue, "NONRET", TextBoxStatusDate.Text, TextBoxDraftDate.Text)
                    'Modified by Dilip Patada  on 15-09-2008

                    If (l_Boolean_ISRetired = True) Then
                        l_string_Output = YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.UpdateQDROInfo(l_string_UniqueId, DropDownListStatus.SelectedItem.Value, "RET", TextBoxStatusDate.Text, TextBoxDraftDate.Text, "") 'Manthan Rajguru | 2016.08.30 | YRS-AT-2488 | Passing Empty value for Pers ID
                    Else
                        'Start - Manthan Rajguru | 2016.08.30 | YRS-AT-2488 | Commented existing code and passed value for Pers ID
                        'l_string_Output = YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.UpdateQDROInfo(l_string_UniqueId, DropDownListStatus.SelectedItem.Value, "NONRET", TextBoxStatusDate.Text, TextBoxDraftDate.Text)
                        l_string_Output = YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.UpdateQDROInfo(l_string_UniqueId, DropDownListStatus.SelectedItem.Value, "NONRET", TextBoxStatusDate.Text, TextBoxDraftDate.Text, DropDownListSpouse.SelectedItem.Value)
                        'End - Manthan Rajguru | 2016.08.30 | YRS-AT-2488 | Commented existing code and passed value for Pers ID
                    End If

                    'End Ashutosh Patil 12-Jul-2007
                End If
            End If
            'Added By Dilip  BT-593 for making QDRO Request Date 24-09-2008
            LoadQDRORequestGrid(Session("PersId")) 'Chandrasekar - 2016.07.05 - YRS-AT-2481 - YRS and/or PHR enhancement-store QDRO beneficiary info in participant's acct after split (TrackIT 22116)

            'START Commented by : Chandrasekar - 2016.07.05 - YRS-AT-2481 - YRS and/or PHR enhancement-store QDRO beneficiary info in participant's acct after split (TrackIT 22116)
            'If (l_Boolean_ISRetired = True) Then
            '    l_string_QDROType = "RET"
            'ElseIf (l_Boolean_ISRetired = False) Then
            '    l_string_QDROType = "NONRET"
            'End If
            ''Added By Dilip  BT-593 for making QDRO Request Date 24-09-2008
            'l_dataset_Qdro = YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.GetQDROInfo(Session("FundId"), l_string_QDROType)
            'If l_dataset_Qdro.Tables("QdroInfo").Rows.Count > 0 Then
            '    Me.DataGridQdroInfo.DataSource = l_dataset_Qdro
            '    viewstate("DS_Sort_EditQDRO") = l_dataset_Qdro
            '    Me.DataGridQdroInfo.DataBind()
            'End If
            'END Commented by : Chandrasekar - 2016.07.05 - YRS-AT-2481 - YRS and/or PHR enhancement-store QDRO beneficiary info in participant's acct after split (TrackIT 22116)

            'Finding whether there is any pending record or not
            ButtonAdd.Enabled = True
            Dim _icount As Integer
            _icount = 0
            While _icount < Me.DataGridQdroInfo.Items.Count
                If Me.DataGridQdroInfo.Items(_icount).Cells(QDRO_STATUS).Text = "Pending" Then 'Chandrasekar - 2016.07.05 - YRS-AT-2481 - Changed hardcoded indexing by constants
                    Me.ButtonAdd.Enabled = False
                End If
                _icount = _icount + 1

            End While
            If (l_string_Output = 0) Then
                'START- Chandrasekar - 2016.07.05 - YRS-AT-2481
                MessageBox.Show(PlaceHolderAddEditQDRO, "QDRO", "Request Saved Successfully", MessageBoxButtons.OK, False)   'START- Added by: Chandrasekar - 2016.07.05 - YRS-AT-2481 
                LoadQDRORequestGrid(Session("PersId"))
                'END- Chandrasekar - 2016.07.05 - YRS-AT-2481 
                Dim msg As String

                msg = msg + "<Script Language='JavaScript'>"

                msg = msg + "window.opener.document.forms(0).submit();"

                'msg = msg + "self.close();"   'START- Commented by: Chandrasekar - 2016.07.05 - YRS-AT-2481 

                msg = msg + "</Script>"
                Response.Write(msg)
            End If

        Catch ex As Exception

            Dim l_String_Exception_Message As String
            HelperFunctions.LogException("TextBoxSSNo_TextChanged", ex) 'Manthan Rajguru | 2016.09.02 | YRS-AT-2488 | Logging exception message
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message)

        End Try

    End Sub

    Private Sub ButtonCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonCancel.Click
        Try
            Session("Edit_QDRO_Sort") = Nothing

            'START- Commented by : Chandrasekar - 2016.07.05 - YRS-AT-2481 - YRS and/or PHR enhancement-store QDRO beneficiary info in participant's acct after split (TrackIT 22116)
            'Dim closeWindow5 As String = "<script language='javascript'>" & _
            '                                       "window.opener.document.forms(0).submit(); self.close();" & _
            '                                       "</script>"
            'If (Not Me.IsStartupScriptRegistered("CloseWindow5")) Then
            '    Page.RegisterStartupScript("CloseWindow5", closeWindow5)
            'End If 
            'END Commented by : Chandrasekar - 2016.07.05 - YRS-AT-2481 - YRS and/or PHR enhancement-store QDRO beneficiary info in participant's acct after split (TrackIT 22116)

            'START- Chandrasekar - 2016.07.05 - YRS-AT-2481
            LoadQDRORequestGrid(Session("PersId"))
            DataGridQdroInfo.SelectedIndex = -1
            DropDownListStatus.Enabled = False
            TextBoxDraftDate.Enabled = False
            'END-  Chandrasekar - 2016.07.05 - YRS-AT-2481           
            'Start - Mantahn Rajguru | 2016.08.30 | YRS-AT-2488 | Reseting controls
            DropDownListSpouse.Enabled = False
            ButtonAdd.Enabled = True
            'START: Manthan Rajguru | 2016.09.07 | YRS-AT-2488 | Commented existing code and reseting dropdown values
            'DropDownListSpouse.SelectedIndex = 0
            Literal1.Text = ""
            If Not ViewState("QDRORequestID") Is Nothing Then
                PopulateSpouseDetailsDropDownList(ViewState("QDRORequestID"))
            End If
            'END: Manthan Rajguru | 2016.09.07 | YRS-AT-2488 | Commented existing code and reseting dropdown values
            'End - Mantahn Rajguru | 2016.08.30 | YRS-AT-2488 | Reseting controls
        Catch ex As Exception

            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message)

        End Try


    End Sub

    Private Sub DataGridQdroInfo_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles DataGridQdroInfo.SortCommand
        Try
            Dim l_ds_EditQdro As DataSet
            Me.DataGridQdroInfo.SelectedIndex = -1
            If Not viewstate("DS_Sort_EditQDRO") Is Nothing Then
                Dim dv As New DataView
                Dim SortExpression As String
                SortExpression = e.SortExpression
                l_ds_EditQdro = viewstate("DS_Sort_EditQDRO")
                dv = l_ds_EditQdro.Tables(0).DefaultView
                dv.Sort = SortExpression
                If Not Session("Edit_QDRO_Sort") Is Nothing Then
                    If Session("Edit_QDRO_Sort").ToString.Trim.EndsWith("ASC") Then
                        dv.Sort = SortExpression + " DESC"
                    Else
                        dv.Sort = SortExpression + " ASC"
                    End If
                Else
                    dv.Sort = SortExpression + " ASC"
                End If
                Me.DataGridQdroInfo.DataSource = Nothing
                Me.DataGridQdroInfo.DataSource = dv
                Me.DataGridQdroInfo.DataBind()
                Session("Edit_QDRO_Sort") = dv.Sort
            End If
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
    End Sub

    'START : Chandrasekar - 2016.07.05 - YRS-AT-2481 - YRS and/or PHR enhancement-store QDRO beneficiary info in participant's acct after split (TrackIT 22116)
    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Session("Edit_QDRO_Sort") = Nothing
        Dim closeWindow5 As String = "<script language='javascript'>" & _
                                               "window.close()" & _
                                               "</script>"

        If (Not Me.IsStartupScriptRegistered("CloseWindow5")) Then
            Page.RegisterStartupScript("CloseWindow5", closeWindow5)
        End If
    End Sub

    Private Function LoadQDRORequestGrid(ByVal l_string_PersId As String)
        Dim strQDROType As String
        Dim dsQDRO As DataSet
        Dim bIsRetired As Boolean
        Try
            If Not Session("ISRetired") Is Nothing Then
                bIsRetired = Session("ISRetired")
            End If
            If (bIsRetired = True) Then
                strQDROType = "RET"
            ElseIf (bIsRetired = False) Then
                strQDROType = "NONRET"
            End If
            'START : Chandrasekar - 2016.07.05 - YRS-AT-2481 - YRS and/or PHR enhancement-store QDRO beneficiary info in participant's acct after split (TrackIT 22116)
            ButtonSave.Enabled = False
            ButtonCancel.Enabled = False
            DropDownListStatus.Enabled = False
            TextBoxDraftDate.Enabled = False
            'END : Chandrasekar - 2016.07.05 - YRS-AT-2481 - YRS and/or PHR enhancement-store QDRO beneficiary info in participant's acct after split (TrackIT 22116)
            DropDownListSpouse.Enabled = False 'Manthan Rajguru | 2016.08.30 | YRS-AT-2488 | Disabling dropdown control
            dsQDRO = YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.GetQDROInfo(l_string_PersId, strQDROType)
            If dsQDRO.Tables("QdroInfo").Rows.Count > 0 Then
                Me.DataGridQdroInfo.DataSource = dsQDRO
                ViewState("DS_Sort_EditQDRO") = dsQDRO
                Me.DataGridQdroInfo.DataBind()
                TextBoxQdroType.Text = Me.DataGridQdroInfo.Items(0).Cells(QDRO_TYPE).Text.Trim()

                'selecting the appropriate status value in the dropdownlist based on the value
                DropDownListStatus.ClearSelection()
                DropDownListStatus.Items.FindByValue(Me.DataGridQdroInfo.Items(0).Cells(QDRO_STATUS_CHCK).Text).Selected = True

                TextBoxStatusDate.Text = Me.DataGridQdroInfo.Items(0).Cells(QDRO_STATUSDATE).Text.Trim()
                TextBoxDraftDate.Text = Me.DataGridQdroInfo.Items(0).Cells(QDRO_DRAFTDATE).Text.Trim()
                ViewState("QDRORequestID") = Me.DataGridQdroInfo.Items(0).Cells(QDRO_REQUESTID).Text.Trim() 'Manthan Rajguru | 2016.09.07 | Added to store QDRO Request ID in viewstate for accessing QDRO beneficiary details
            End If
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
    End Function

    Public Function getmessage(ByVal resourcemessage As String)
        Try
            Dim strMessage As String
            Try
                strMessage = GetGlobalResourceObject("ParticipantsInformation", resourcemessage).ToString()
            Catch ex As Exception
                strMessage = resourcemessage
            End Try
            Return strMessage
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    'END : Chandrasekar - 2016.07.05 - YRS-AT-2481 - YRS and/or PHR enhancement-store QDRO beneficiary info in participant's acct after split (TrackIT 22116)
    'Start - Manthan Rajguru | 2016.08.30 | YRS-AT-2488 | Added to populate QDRO beneficiary
    Private Sub PopulateSpouseDetailsDropDownList(ByVal strQDRORequestID As String)
        Dim dsQDRORecipientDetails As DataSet
        Dim strPersID As String       
        Dim drQDRORecipientDetails As DataRow()

        If Literal1.Text = "Add" Then
            Me.DropDownListSpouse.DataSource = Nothing
            Me.DropDownListSpouse.DataBind()
            Me.DropDownListSpouse.Items.Clear()
            Me.DropDownListSpouse.Items.Insert(0, New ListItem("None", "None"))
            Me.DropDownListSpouse.SelectedIndex = 0
        Else
            dsQDRORecipientDetails = YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.GetQDROSplitDetailsByRequestId(strQDRORequestID)
            If HelperFunctions.isNonEmpty(dsQDRORecipientDetails) Then
                drQDRORecipientDetails = dsQDRORecipientDetails.Tables(0).Select("guiUniqueID = '" + strQDRORequestID + "' AND bitRecipientSpouse = 1")
                Me.DropDownListSpouse.DataSource = dsQDRORecipientDetails.Tables(0)
                Me.DropDownListSpouse.DataTextField = "QDRORecipientName"
                Me.DropDownListSpouse.DataValueField = "guiPersID"
                Me.DropDownListSpouse.DataBind()
                Me.DropDownListSpouse.Items.Insert(0, New ListItem("None", "None"))
                If drQDRORecipientDetails.Length > 0 Then
                    Me.DropDownListSpouse.SelectedValue = drQDRORecipientDetails(0)("guiPersID").ToString()
                Else
                    Me.DropDownListSpouse.SelectedIndex = 0
                End If
            End If
        End If
    End Sub
    'End - Manthan Rajguru | 2016.08.30 | YRS-AT-2488 | Added to populate QDRO beneficiary

    'Start -Manthan Rajguru | 2016.08.30 | YRS-AT-2488 | Added to Hide column for spouse beneficiary for retired person
    Private Sub DatagridQDROInformation_ItemCreated(sender As Object, e As DataGridItemEventArgs) Handles DatagridQDROInformation.ItemCreated
        Dim blnIsRetired As Boolean
        If Not Session("ISRetired") Is Nothing Then
            blnIsRetired = Session("ISRetired")
        End If
        If blnIsRetired Then
            DatagridQDROInformation.Columns(BENEFICIARY_SPOUSE).Visible = False
        End If
    End Sub
    'End -Manthan Rajguru | 2016.08.30 | YRS-AT-2488 | Added to Hide column for spouse beneficiary for retired person
End Class

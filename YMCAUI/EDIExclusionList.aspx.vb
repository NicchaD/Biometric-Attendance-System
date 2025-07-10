'*******************************************************************************
' Copyright YMCA Retirement Fund All Rights Reserved. 
'
' Project Name		:	YMCAUI
' FileName			:	EDIExclusionList.aspx.vb
' Author Name		:	Ashutosh Patil
' Employee ID		:	36307
' Email				:	ashutosh.patil@3i-infotech.com
' Extn      		:	8568
' Creation Date		:	30-Apr-2007
'********************************************************************************************************************************
'Modification History
'********************************************************************************************************************************
'Modified By                  Date                              Description
'********************************************************************************************************************************
'Ashutosh Patil               28-May-2007                       YREN-3391. Changes in buttonAdd_Click events 
'                                                               and in yrs_usp_FindPerson procedure.
'Ashutosh Patil               03-Jul-2007                       FundID No validations   
'Aparna Samala              20-Sep-2007                         Replace - in SSNO with ""
'Neeraj Singh               12/Nov/2009                         Added form name for security issue YRS 5.0-940 
'Shashi Shekhar Singh       28-06-2010                          Migration Related changes.
'Dinesh Kanojia               2013-03-19                        Bug ID: 1535:YRS 5.0-1758:Add telepone and email as lookup fields for person maintenance
'Hafiz/Anudeep         		 2014-06-13                          BT:2524 :YRS 5.0-2360 - EDI exclustion list ( This change includes master page implementation , jquery implementation)
'Anudeep         		    2014-06-25                          BT:2524 :YRS 5.0-2360 - EDI exclustion list 
'Shashank                   2014-08-03                          BT-2619\YRS 5.0-2279 : Add in Administration Screen ability to change messages for YRS or Web site
'Manthan Rajguru            2015.09.16                          YRS-AT-2550: YRS data cleanup: copyright comments and namespace/reference (Changed the Namespace/reference from 'Infotech' to 'YMCARET')
'********************************************************************************************************************************
Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Collections
Imports System.ComponentModel
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports YMCAObjects
Public Class EDIExclusionList
    Inherits System.Web.UI.Page
    Dim g_dataset_EDIExlusion As DataSet
    'below line is Added by Neeraj for issue id YRS 5.0-940 
    Dim strFormName As String = New String("EDIExclusionList.aspx")
    'End issue id YRS 5.0-940


#Region " Web Form Designer Generated Code "
    Protected WithEvents lblLookFor As System.Web.UI.WebControls.Label
    Protected WithEvents txtLookFor As System.Web.UI.WebControls.TextBox
    Protected WithEvents btnSearch As System.Web.UI.WebControls.Button
    Protected WithEvents lblNoRecordFound As System.Web.UI.WebControls.Label
    Protected WithEvents txtPerssId As System.Web.UI.WebControls.TextBox
    Protected WithEvents gvEDI As System.Web.UI.WebControls.GridView
    Protected WithEvents lblFirstName As System.Web.UI.WebControls.Label
    Protected WithEvents txtFirstName As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblLastName As System.Web.UI.WebControls.Label
    Protected WithEvents txtLastName As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblFundNo As System.Web.UI.WebControls.Label
    Protected WithEvents txtFundNo As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblSSNo As System.Web.UI.WebControls.Label
    Protected WithEvents txtSSNo As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblReason As System.Web.UI.WebControls.Label
    Protected WithEvents txtMessage As System.Web.UI.WebControls.TextBox
    Protected WithEvents btnAdd As System.Web.UI.WebControls.Button
    Protected WithEvents btnFind As System.Web.UI.WebControls.Button
    Protected WithEvents btnReport As System.Web.UI.WebControls.Button
    Protected WithEvents btnSave As System.Web.UI.WebControls.Button
    'Protected WithEvents ButtonDelete As System.Web.UI.WebControls.Button
    Protected WithEvents btnnCancel As System.Web.UI.WebControls.Button

    Protected WithEvents btnYes As System.Web.UI.WebControls.Button
    Protected WithEvents btnNo As System.Web.UI.WebControls.Button
    Protected WithEvents lblMessage As System.Web.UI.WebControls.Label

    'Protected WithEvents PlaceHolder1 As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents txtMiddleName As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblMiddleName As System.Web.UI.WebControls.Label
    'Protected WithEvents Menu1 As skmMenu.Menu
    Protected WithEvents btnOK As System.Web.UI.WebControls.Button
    Protected WithEvents btnClear As System.Web.UI.WebControls.Button
    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object
    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region
#Region " Constant Variables " 'Constant varaibles to be used in case of the datagrid columns are required to change 
    Const m_const_int_SSNo As Integer = 1
    'Const m_const_int_LastName As Integer = 1
    'Const m_const_int_FirstName As Integer = 2
    'Const m_const_int_MiddleName As Integer = 3
#End Region
#Region "Properties"
    'Private Property SessionSelectedSSNo() As String
    '    Get
    '        If Not (Session("SelectedSSNo")) Is Nothing Then
    '            Return (CType(Session("SelectedSSNo"), String))
    '        Else
    '            Return String.Empty
    '        End If
    '    End Get

    '    Set(ByVal Value As String)
    '        Session("SelectedSSNo") = Value
    '    End Set
    'End Property
    Private Property ValueFromFindInfo() As Boolean
        Get
            If Not (Session("FindInfoFromEDIListScreen")) Is Nothing Then
                Return (CType(Session("FindInfoFromEDIListScreen"), Boolean))
            Else
                Return False
            End If
        End Get

        Set(ByVal Value As Boolean)
            Session("FindInfoFromEDIListScreen") = Value
        End Set
    End Property
    Private Property SessionDeleteSSN() As String
        Get
            Return (CType(Session("DeleteSSN"), String))
        End Get
        Set(ByVal Value As String)
            Session("DeleteSSN") = Value
        End Set
    End Property
    Public Property SessionEDIListData() As DataSet
        Get
            If Not (Session("EDIExclusionData")) Is Nothing Then
                Return (DirectCast(Session("EDIExclusionData"), DataSet))
            Else
                Return Nothing
            End If
        End Get
        Set(ByVal Value As DataSet)
            Session("EDIExclusionData") = Value
        End Set
    End Property
    'Public Property SessionDTEDIListData() As DataTable
    '    Get
    '        If Not (Session("EDIDTExclusionData")) Is Nothing Then
    '            Return (CType(Session("EDIDTExclusionData"), DataTable))
    '        Else
    '            Return Nothing
    '        End If
    '    End Get
    '    Set(ByVal Value As DataTable)
    '        Session("EDIDTExclusionData") = Value
    '    End Set
    'End Property
    Private Enum LoadDatasetMode
        Table
        LookFor
        Delete
        FromFindInfo
        Sort
        Session
    End Enum
#End Region
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        'Menu1.DataSource = Server.MapPath("SimpleXML.xml")
        'Menu1.DataBind()
        Try
            If Page.IsPostBack = False Then
                Me.txtSSNo.Attributes.Add("OnKeyPress", "javascript:ValidateNumeric();")
                Me.txtFundNo.Attributes.Add("OnKeyPress", "javascript:ValidateNumeric();")
                Me.txtLookFor.Attributes.Add("OnKeyPress", "javascript:ValidateNumeric();")
                'Me.SessionSelectedSSNo = ""
                Me.SessionDeleteSSN = Nothing
                'Me.SessionDTEDIListData = Nothing
                Me.SessionEDIListData = Nothing
                Session("FindInfoFromEDIListScreen") = False
                Session("InvalidSSNo") = Nothing
                Session("EDIListSort") = Nothing
                Session("EDIListPageIndex") = Nothing
                Call PopulateEDIExclusionData(LoadDatasetMode.Table)
            Else
                Call PopulateEDIExclusionData(LoadDatasetMode.Session)

                If Session("FindInfoFromEDIListScreen") = True Then
                    Me.ValueFromFindInfo = Session("FindInfoFromEDIListScreen")
                    Call PopulateEDIExclusionData(LoadDatasetMode.FromFindInfo)
                    Session("FindInfoFromEDIListScreen") = False
                End If
            End If

            'If Request.Form("Yes") = "Yes" Then
            '    Call PopulateEDIExclusionData(LoadDatasetMode.Delete)
            '    Me.btnSave.Enabled = True
            '    Me.btnReport.Enabled = False
            '    Me.ButtonDelete.Enabled = False
            'ElseIf Request.Form("Yes") = "No" Then
            '    Call PopulateEDIExclusionData(LoadDatasetMode.Table)
            '    Me.btnSave.Enabled = False
            '    Me.ButtonDelete.Enabled = False
            'End If

            If Not Session("InvalidSSNo") Is Nothing Then
                If Session("InvalidSSNo") = "FromSSNo" Then
                    Call SetControlFocus(Me.txtSSNo)
                    Session("InvalidSSNo") = Nothing
                ElseIf Session("InvalidSSNo") = "FromLookFor" Then
                    Call SetControlFocus(Me.txtLookFor)
                    Session("InvalidSSNo") = Nothing
                ElseIf Session("InvalidSSNo") = "FromFundNo" Then
                    Call SetControlFocus(Me.txtFundNo)
                    Session("InvalidSSNo") = Nothing
                End If
            End If
        Catch ex As Exception
            HelperFunctions.LogException("EDIExclusionList --> Page_Load", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
        End Try
    End Sub
#Region " Events " 'Events 
    Private Sub ButtonSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        'Dim l_EdiList_datatable As DataTable
        Dim ds As DataSet
        Dim dr As DataRow
        'Dim l_DataRows As DataRow()
        Dim i As Long
        'Dim len As Integer

        Try

            Call PopulateEDIExclusionData(LoadDatasetMode.Session)

            If Not Me.txtLookFor.Text.Trim() = "" Then

                ds = g_dataset_EDIExlusion

                If Not ds Is Nothing Then

                    'l_DataRows = l_dataset.Tables(1).Select("bitDelete=0")


                    For i = 0 To ds.Tables(0).Rows.Count - 1

                        dr = ds.Tables(0).Rows(i)

                        If Not dr Is Nothing Then

                            'len = Me.txtLookFor.Text.Trim().Length()

                            'If Left(dr("SSNo"), len) = Left(Me.txtLookFor.Text.Trim(), len) Then
                            If dr("SSNo").ToString().Trim = Me.txtLookFor.Text.Trim() Then

                                'If dr("SSNo").ToString <> "&nbsp;" Or dr("SSNo").ToString <> "" Then
                                '    SessionSelectedSSNo = CType(dr("SSNo"), String).ToString.Trim
                                'Else
                                '    Exit Sub
                                'End If

                                Call PopulateEDIExclusionData(LoadDatasetMode.LookFor)
                                'Me.ButtonDelete.Enabled = True

                                Exit Sub

                            End If

                        End If

                    Next i



                End If
            Else
                'HelperFunctions.ShowMessageToUser(GetMessage("MESSAGE_EDI_SSN_BLANK_EXISTS"), EnumMessageTypes.Error)
                HelperFunctions.ShowMessageToUser(MetaMessageList.MESSAGE_EDI_SSN_BLANK_EXISTS)
            End If

            If Me.txtLookFor.Text.Trim() <> "" Then
                'If SessionSelectedSSNo <> Me.txtLookFor.Text.Trim() Then
                Session("InvalidSSNo") = "FromLookFor"
                'MessageBox.Show(Me.PlaceHolder1, "YMCA - YRS", "SSNo " & Me.TextBoxLookFor.Text.Trim() & " does not exist in EDI Exclusion List. Please enter a valid SSNo.", MessageBoxButtons.Stop)
                'HelperFunctions.ShowMessageToUser(String.Format(GetMessage("MESSAGE_EDI_SSN_NOT_EXISTS_IN_EDI"), Me.txtLookFor.Text.Trim()), EnumMessageTypes.Error)

                Dim dictParam As New Dictionary(Of String, String)
                dictParam.Add("SSN", Me.txtLookFor.Text.Trim())

                HelperFunctions.ShowMessageToUser(MetaMessageList.MESSAGE_EDI_SSN_NOT_EXISTS_IN_EDI, Nothing, dictParam)

                'End If
            End If
            Call ClearControls()
            Me.btnAdd.Enabled = False

        Catch ex As Exception
            HelperFunctions.LogException("EDIExclusionList --> ButtonSearch_Click", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
        End Try
    End Sub
    Private Sub TextBoxFundNo_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtFundNo.TextChanged
        Dim dsFindFundNo As DataSet
        Dim drFindFundNo As DataRow()
        Dim drFundNoDetails As DataRow
        Dim dictParam As Dictionary(Of String, String)
        Try
            dictParam = New Dictionary(Of String, String)
            BindGrid(g_dataset_EDIExlusion)
            If Trim(txtFundNo.Text) <> "" Then
                Try
                    ' STARTS 2013-03-19    Dinesh Kanojia      Bug ID: 1535:YRS 5.0-1758:Add telepone and email as lookup fields for person maintenance
                    'l_dataset_FindFundNo = YMCARET.YmcaBusinessObject.FindInfo.LookUpPersons(String.Empty, Me.txtFundNo.Text.Trim(), String.Empty, String.Empty, "EDIExclusionList", String.Empty, String.Empty)
                    dsFindFundNo = YMCARET.YmcaBusinessObject.FindInfo.LookUpPersons(String.Empty, Me.txtFundNo.Text.Trim(), String.Empty, String.Empty, "EDIExclusionList", String.Empty, String.Empty, String.Empty, String.Empty)
                    ' ENDS 2013-03-19    Dinesh Kanojia      Bug ID: 1535:YRS 5.0-1758:Add telepone and email as lookup fields for person maintenance
                    If Not dsFindFundNo Is Nothing Then
                        If dsFindFundNo.Tables("Persons").Rows.Count > 0 Then
                            drFindFundNo = dsFindFundNo.Tables("Persons").Select("FundIDNo='" & Me.txtFundNo.Text.ToString().Trim() & "'")
                            If drFindFundNo.Length > 0 Then
                                drFundNoDetails = drFindFundNo(0)
                                'Commented and Modified By Ashutosh Patil as on 03-Jul-2007
                                'For Finding exact details related to fund No
                                'Start Ashutosh Patil as on 03-Jul-2007

                                'Me.txtFundNo.Text = l_dataset_FindFundNo.Tables("Persons").Rows(0)("FundIDNo").ToString
                                'Me.txtSSNo.Text = l_dataset_FindFundNo.Tables("Persons").Rows(0)("SSNo").ToString
                                'Me.txtLastName.Text = l_dataset_FindFundNo.Tables("Persons").Rows(0)("LastName").ToString
                                'Me.txtFirstName.Text = l_dataset_FindFundNo.Tables("Persons").Rows(0)("FirstName").ToString
                                'Me.txtMiddleName.Text = l_dataset_FindFundNo.Tables("Persons").Rows(0)("MiddleName").ToString
                                'Me.TextBoxPerssId.Text = l_dataset_FindFundNo.Tables("Persons").Rows(0)("PersID").ToString
                                'Call SetControlFocus(Me.txtMessage)
                                'Me.btnAdd.Enabled = True
                                'Me.btnnCancel.Enabled = True
                                'Me.btnFind.Enabled = False
                                'Me.btnReport.Enabled = False
                                If Not drFundNoDetails Is Nothing Then
                                    Me.txtFundNo.Text = drFundNoDetails("FundIDNo").ToString().Trim()
                                    Me.txtSSNo.Text = drFundNoDetails("SSNo").ToString().Trim()
                                    Me.txtLastName.Text = drFundNoDetails("LastName").ToString().Trim()
                                    Me.txtFirstName.Text = drFundNoDetails("FirstName").ToString().Trim()
                                    Me.txtMiddleName.Text = drFundNoDetails("MiddleName").ToString().Trim()
                                    Me.txtPerssId.Text = drFundNoDetails("PersID").ToString().Trim()
                                    Call SetControlFocus(Me.txtMessage)
                                    Me.btnAdd.Enabled = True
                                    Me.btnnCancel.Enabled = True
                                    Me.btnFind.Enabled = False
                                    Me.btnReport.Enabled = False
                                Else
                                    'MessageBox.Show(Me.PlaceHolder1, "YMCA - YRS", "FundNo " & Me.txtFundNo.Text.Trim() & " does not exist in the system. Please enter a valid FundNo", MessageBoxButtons.Stop)
                                    'HelperFunctions.ShowMessageToUser(String.Format(GetMessage("MESSAGE_EDI_NOT_ELIGIBLE_FUNDNO"), Me.txtFundNo.Text.Trim()), EnumMessageTypes.Error)

                                    dictParam.Add("FUNDNO", Me.txtFundNo.Text.Trim())
                                    HelperFunctions.ShowMessageToUser(MetaMessageList.MESSAGE_EDI_NOT_ELIGIBLE_FUNDNO, Nothing, dictParam)

                                    Me.btnnCancel.Enabled = False
                                    Call ClearControls()
                                    Exit Sub
                                End If
                                'End Ashutosh Patil as on 03-Jul-2007
                            Else
                                'MessageBox.Show(Me.PlaceHolder1, "YMCA - YRS", "FundNo " & Me.txtFundNo.Text.Trim() & " does not exist in the system. Please enter a valid FundNo", MessageBoxButtons.Stop)
                                ' HelperFunctions.ShowMessageToUser(String.Format(GetMessage("MESSAGE_EDI_NOT_ELIGIBLE_FUNDNO"), Me.txtFundNo.Text.Trim()), EnumMessageTypes.Error)

                                dictParam.Add("FUNDNO", Me.txtFundNo.Text.Trim())

                                HelperFunctions.ShowMessageToUser(MetaMessageList.MESSAGE_EDI_NOT_ELIGIBLE_FUNDNO, Nothing, dictParam)
                                Me.btnnCancel.Enabled = False
                                Call ClearControls()
                                Exit Sub
                            End If
                        Else
                            'MessageBox.Show(Me.PlaceHolder1, "YMCA - YRS", "FundNo " & Me.txtFundNo.Text.Trim() & " does not exist in the system. Please enter a valid FundNo", MessageBoxButtons.Stop)
                            'HelperFunctions.ShowMessageToUser(String.Format(GetMessage("MESSAGE_EDI_NOT_ELIGIBLE_FUNDNO"), Me.txtFundNo.Text.Trim()), EnumMessageTypes.Error)

                            dictParam.Add("FUNDNO", Me.txtFundNo.Text.Trim())

                            HelperFunctions.ShowMessageToUser(MetaMessageList.MESSAGE_EDI_NOT_ELIGIBLE_FUNDNO, Nothing, dictParam)

                            Me.btnnCancel.Enabled = False
                            Call ClearControls()
                            Exit Sub
                        End If
                    End If
                Catch ex As Exception
                    If ex.Message.Trim.ToString() = "No record(s) found for the specified 'criteria'." Then
                        Session("InvalidSSNo") = "FromFundNo"
                        'MessageBox.Show(Me.PlaceHolder1, "YMCA - YRS", "FundNo " & Me.txtFundNo.Text.Trim() & " does not exist in the system. Please enter a valid FundNo", MessageBoxButtons.Stop)
                        ' HelperFunctions.ShowMessageToUser(String.Format(GetMessage("MESSAGE_EDI_NOT_ELIGIBLE_FUNDNO"), Me.txtFundNo.Text.Trim()), EnumMessageTypes.Error)
                        dictParam = New Dictionary(Of String, String)
                        dictParam.Add("FUNDNO", Me.txtFundNo.Text.Trim())

                        HelperFunctions.ShowMessageToUser(MetaMessageList.MESSAGE_EDI_NOT_ELIGIBLE_FUNDNO, Nothing, dictParam)
                        Me.btnnCancel.Enabled = False
                        Call ClearControls()
                        Exit Sub
                    End If
                End Try
            Else
                Me.txtFirstName.Text = String.Empty
                Me.txtLastName.Text = String.Empty
                Me.txtSSNo.Text = String.Empty
                Me.txtMessage.Text = String.Empty
                Me.txtPerssId.Text = String.Empty
                Me.txtMiddleName.Text = String.Empty
                Me.txtLookFor.Text = String.Empty
            End If


        Catch ex As Exception
            HelperFunctions.LogException("EDIExclusionList_gvEDI_RowDataBound", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
        End Try
    End Sub
    Private Sub TextBoxSSNo_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtSSNo.TextChanged
        Dim dsFindSSNo As DataSet
        Try
            BindGrid(g_dataset_EDIExlusion)
            txtSSNo.Text = txtSSNo.Text.Replace("-", "") 'by Aparna 19/09/2007
            'SSNo Length < 9 
            If Trim(txtSSNo.Text) <> "" Then
                If Len(Trim(txtSSNo.Text)) < 9 Then
                    'MessageBox.Show(Me.PlaceHolder1, "YMCA - YRS", "Invalid SSNo " & Me.txtSSNo.Text.Trim() & ", Please enter a valid SSNo", MessageBoxButtons.Stop)
                    'HelperFunctions.ShowMessageToUser(String.Format(GetMessage("MESSAGE_EDI_INVALID_SSN"), Me.txtSSNo.Text.Trim()), EnumMessageTypes.Error)
                    Dim dictParam As New Dictionary(Of String, String)
                    dictParam.Add("SSN", Me.txtSSNo.Text.Trim())

                    HelperFunctions.ShowMessageToUser(MetaMessageList.MESSAGE_EDI_INVALID_SSN, Nothing, dictParam)

                    Call ClearControls()
                    Exit Sub
                End If
            End If

            'SSNo does not exist in the database
            If Trim(txtSSNo.Text) <> "" Then
                If Len(Trim(txtSSNo.Text)) = 9 Then
                    Try
                        ' STARTS 2013-03-19    Dinesh Kanojia      Bug ID: 1535:YRS 5.0-1758:Add telepone and email as lookup fields for person maintenance
                        'l_dataset_FindSSNo = YMCARET.YmcaBusinessObject.FindInfo.LookUpPersons(Me.txtSSNo.Text.Trim(), String.Empty, String.Empty, String.Empty, "EDIExclusionList", String.Empty, String.Empty)
                        dsFindSSNo = YMCARET.YmcaBusinessObject.FindInfo.LookUpPersons(Me.txtSSNo.Text.Trim(), String.Empty, String.Empty, String.Empty, "EDIExclusionList", String.Empty, String.Empty, String.Empty, String.Empty)
                        ' ENDS 2013-03-19    Dinesh Kanojia      Bug ID: 1535:YRS 5.0-1758:Add telepone and email as lookup fields for person maintenance
                        If Not dsFindSSNo Is Nothing Then
                            If dsFindSSNo.Tables("Persons").Rows.Count > 0 Then
                                Me.txtFundNo.Text = dsFindSSNo.Tables("Persons").Rows(0)("FundIDNo").ToString
                                Me.txtLastName.Text = dsFindSSNo.Tables("Persons").Rows(0)("LastName").ToString
                                Me.txtFirstName.Text = dsFindSSNo.Tables("Persons").Rows(0)("FirstName").ToString
                                Me.txtMiddleName.Text = dsFindSSNo.Tables("Persons").Rows(0)("MiddleName").ToString
                                Me.txtPerssId.Text = dsFindSSNo.Tables("Persons").Rows(0)("PersID").ToString
                                Call SetControlFocus(Me.txtMessage)
                                Me.btnAdd.Enabled = True
                                Me.btnnCancel.Enabled = True
                                Me.btnFind.Enabled = False
                                Me.btnReport.Enabled = False
                            Else
                                'MessageBox.Show(Me.PlaceHolder1, "YMCA - YRS", "SSNo " & Me.txtSSNo.Text.Trim() & " does not exist in the system. Please enter a valid SSNo", MessageBoxButtons.Stop)
                                'HelperFunctions.ShowMessageToUser(String.Format(GetMessage("MESSAGE_EDI_NOT_ELIGIBLE_SSN"), Me.txtSSNo.Text), EnumMessageTypes.Error)
                                Dim dictParam As New Dictionary(Of String, String)
                                dictParam.Add("SSN", Me.txtSSNo.Text.Trim())

                                HelperFunctions.ShowMessageToUser(MetaMessageList.MESSAGE_EDI_NOT_ELIGIBLE_SSN, Nothing, dictParam)
                                Me.btnnCancel.Enabled = False
                                Call ClearControls()
                                Exit Sub
                            End If
                        End If
                    Catch ex As Exception
                        If ex.Message.Trim.ToString() = "No record(s) found for the specified 'criteria'." Then
                            Session("InvalidSSNo") = "FromSSNo"
                            'MessageBox.Show(Me.PlaceHolder1, "YMCA - YRS", "SSNo " & Me.txtSSNo.Text.Trim() & " does not exist in the system. Please enter a valid SS No.", MessageBoxButtons.Stop)
                            'HelperFunctions.ShowMessageToUser(String.Format(GetMessage("MESSAGE_EDI_NOT_ELIGIBLE_SSN"), Me.txtSSNo.Text), EnumMessageTypes.Error)
                            Dim dictParam As New Dictionary(Of String, String)
                            dictParam.Add("SSN", Me.txtSSNo.Text.Trim())

                            HelperFunctions.ShowMessageToUser(MetaMessageList.MESSAGE_EDI_NOT_ELIGIBLE_SSN, Nothing, dictParam)

                            Me.btnnCancel.Enabled = False
                            Call ClearControls()
                            Exit Sub
                        End If
                    End Try
                End If
            Else
                Me.txtFirstName.Text = String.Empty
                Me.txtLastName.Text = String.Empty

                Me.txtFundNo.Text = String.Empty
                Me.txtMessage.Text = String.Empty
                Me.txtPerssId.Text = String.Empty
                Me.txtMiddleName.Text = String.Empty
                Me.txtLookFor.Text = String.Empty
            End If

        Catch ex As Exception
            HelperFunctions.LogException("EDIExclusionList_gvEDI_RowDataBound", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
        End Try
    End Sub
    Private Sub ButtonOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        Try
            Response.Redirect("MainWebForm.aspx", False)
        Catch ex As Exception
            HelperFunctions.LogException("EDIExclusionList --> ButtonOK_Click", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
        End Try
    End Sub
    Private Sub ButtonCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnnCancel.Click
        Try
            Call PopulateEDIExclusionData(LoadDatasetMode.Table)
            Call ClearControls()
            'Me.ButtonDelete.Enabled = False
            Me.btnSave.Enabled = False
            Me.btnAdd.Enabled = False
            Me.btnnCancel.Enabled = False
            Me.btnFind.Enabled = True
            Me.gvEDI.SelectedRowStyle.CssClass = "DataGrid_NormalStyle"
            If Me.gvEDI.Rows.Count > 0 Then
                Me.btnReport.Enabled = True
            End If
        Catch ex As Exception
            HelperFunctions.LogException("EDIExclusionList --> ButtonCancel_Click", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
        End Try
    End Sub
    'Private Sub ButtonDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonDelete.Click
    '    Dim l_deleterecord_datatable As DataTable

    '    Try
    '        If Me.gvEDI.Columns.Count = 0 Then Exit Sub
    '        If Me.SessionSelectedIndex.ToString.Trim = "" Then
    '            Me.SessionSelectedIndex = Me.gvEDI.SelectedIndex
    '        End If
    '        'MessageBox.Show(160, 300, PlaceHolder1, "YMCA-YRS", "Are you sure you want to delete selected record?", MessageBoxButtons.YesNo)
    '    Catch ex As Exception
    '        Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
    '    End Try
    'End Sub
    Private Sub ButtonSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        'Dim l_dataset_records As DataSet
        'Dim l_datatable_records As DataTable
        'Dim l_str_SelectedUniqueId As String
        'Dim l_addata_datatable As DataTable
        'Dim l_Add_Record_Datarow As DataRow()
        'Dim l_Record_Add_Datarow As DataRow
        'Dim l_DataRow As DataRow
        'Dim i As Integer
        Try
            'Added by neeraj on 18-Nov-2009 : issue is YRS 5.0-940
            Dim checkSecurity As String = SecurityCheck.Check_Authorization(strFormName, Convert.ToInt32(Session("LoggedUserKey")))
            ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "clear", "clear_dirty();", True)

            If Not checkSecurity.Equals("True") Then
                'MessageBox.Show(PlaceHolder1, "YMCA-YRS", checkSecurity, MessageBoxButtons.Stop)
                HelperFunctions.ShowMessageToUser(checkSecurity, EnumMessageTypes.Error)
                Exit Sub
            End If
            'End : YRS 5.0-940

            YMCARET.YmcaBusinessObject.EDIExculsionListBOClass.InsertParticipantsintoList(g_dataset_EDIExlusion)

            'HelperFunctions.ShowMessageToUser(GetMessage("MESSAGE_EDI_SAVED_SUCCESSFULLY"), EnumMessageTypes.Success)
            HelperFunctions.ShowMessageToUser(MetaMessageList.MESSAGE_EDI_SAVED_SUCCESSFULLY)

            Call PopulateEDIExclusionData(LoadDatasetMode.Table)

            Me.btnSave.Enabled = False
            'Me.ButtonDelete.Enabled = False
            Me.btnnCancel.Enabled = False
            If Me.gvEDI.Rows.Count > 0 Then
                Me.btnReport.Enabled = True
            End If

        Catch ex As Exception
            HelperFunctions.LogException("EDIExclusionList_gvEDI_RowDataBound", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
        End Try
    End Sub
    Private Sub ButtonFind_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFind.Click
        Try

            'Dim popupScript As String = "<script language='javascript'>" & _
            '                                          "window.open('FindEDIExlcusionInfo.aspx?Name=EDIExclusionList', 'YMCAYRS', " & _
            '                                          "'width=750,height=450,menubar=no,status=Yes,Resizable=No,top=50,left=70, scrollbars=yes')" & _
            '                                           "</script>"
            'If (Not Me.IsStartupScriptRegistered("PopupScriptRR")) Then
            '   Page.RegisterStartupScript("PopupScriptRR", popupScript)
            'End If
            ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "OpenWindow", "openFindWindow();", True)
            Call ClearControls()
            Me.btnAdd.Enabled = False
        Catch ex As Exception
            HelperFunctions.LogException("EDIExclusionList --> ButtonFind_Click", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
        End Try
    End Sub
    Private Sub ButtonAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        Try
            BindGrid(g_dataset_EDIExlusion)
            ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "clear", "mark_dirty();", True)
            If Me.txtSSNo.Text.ToString <> "" And Me.txtFundNo.Text.ToString <> "" And Me.txtLastName.Text.ToString <> "" And Me.txtFirstName.Text.ToString <> "" Then
                'Added By Ashutosh Patil as on 28-May-2007
                'YREN-3391. Making reason field as compulsory.
                If Me.txtMessage.Text.ToString().Trim() = "" Then
                    'MessageBox.Show(Me.PlaceHolder1, "YMCA - YRS", "Please enter the Reason.", MessageBoxButtons.Stop)
                    'HelperFunctions.ShowMessageToUser(GetMessage("MESSAGE_EDI_ENTER_REASON"), EnumMessageTypes.Error)
                    HelperFunctions.ShowMessageToUser(MetaMessageList.MESSAGE_EDI_ENTER_REASON)

                    Exit Sub
                End If

                Call AddRecordsToGrid(Me.txtSSNo.Text.ToString, Me.txtLastName.Text.ToString().Trim(), Me.txtFirstName.Text.ToString().Trim(), Me.txtMiddleName.Text.ToString().Trim(), Me.txtPerssId.Text.ToString().Trim())
                Call ClearControls()
            Else
                'MessageBox.Show(Me.PlaceHolder1, "YMCA - YRS", "Please enter a valid SSNo Or FundNo for the data.", MessageBoxButtons.Stop)
                ' HelperFunctions.ShowMessageToUser(GetMessage("MESSAGE_EDI_ENTER_VALID_SSN_FUNDNO"), EnumMessageTypes.Error)
                HelperFunctions.ShowMessageToUser(MetaMessageList.MESSAGE_EDI_ENTER_VALID_SSN_FUNDNO)
                Me.btnnCancel.Enabled = False
                Me.btnAdd.Enabled = False
                Me.btnFind.Enabled = True
                Call ClearControls()
                Exit Sub
            End If
            Me.btnAdd.Enabled = False
            Me.btnFind.Enabled = True
        Catch ex As Exception
            HelperFunctions.LogException("EDIExclusionList --> ButtonAdd_Click", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
        End Try
    End Sub
    Private Sub ButtonReport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnReport.Click
        Dim popupScript As String
        Try
            Session("strReportName") = "EDI Exclusion List"

            popupScript = "window.open('FT\\ReportViewer.aspx', 'ReportPopUp', " & _
            "'width=1024,height=768, menubar=no, resizable=yes,top=0,left=0, scrollbars=yes, status=yes');"


            'If (Not Me.IsStartupScriptRegistered("PopupScript1")) Then
            '    Page.RegisterStartupScript("PopupScript1", popupScript)
            'End If
            ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "PopupScriptreport", popupScript, True)

        Catch ex As Exception
            HelperFunctions.LogException("EDIExclusionList --> ButtonReport_Click", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
        End Try
    End Sub

    Private Sub gvEDI_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles gvEDI.PageIndexChanging
        Try
            Session("EDIListPageIndex") = e.NewPageIndex
            BindGrid(g_dataset_EDIExlusion)
        Catch ex As Exception
            HelperFunctions.LogException("EDIExclusionList_gvEDI_PageIndexChanging", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
        End Try
    End Sub

    Private Sub gvEDI_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvEDI.RowDataBound
        'Dim l_button_Select As ImageButton
        Try
            'l_button_Select = e.Row.FindControl("ImageButtonSelect")

            'If (Not l_button_Select Is Nothing) Then
            '    l_button_Select.ImageUrl = "images\select.gif"
            'End If

            HelperFunctions.SetSortingArrows(Session("EDIListSort"), e)

        Catch ex As Exception
            HelperFunctions.LogException("EDIExclusionList_gvEDI_RowDataBound", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
        End Try
    End Sub

    Private Sub gvEDI_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles gvEDI.RowDeleting
        Dim strFirstName As String
        Dim strMiddleName As String
        Dim strLastName As String
        Try
            If Me.gvEDI.Columns.Count = 0 Then Exit Sub

            If Me.SessionDeleteSSN = Nothing OrElse Me.SessionDeleteSSN.ToString.Trim = "" Then
                Me.SessionDeleteSSN = gvEDI.Rows(e.RowIndex).Cells(m_const_int_SSNo).Text
            End If

            strFirstName = IIf(gvEDI.Rows(e.RowIndex).Cells(2).Text.Trim = "&nbsp;", "", gvEDI.Rows(e.RowIndex).Cells(2).Text.Trim)
            strMiddleName = IIf(gvEDI.Rows(e.RowIndex).Cells(3).Text.Trim = "&nbsp;", "", gvEDI.Rows(e.RowIndex).Cells(3).Text.Trim)
            strLastName = IIf(gvEDI.Rows(e.RowIndex).Cells(4).Text.Trim = "&nbsp;", "", gvEDI.Rows(e.RowIndex).Cells(4).Text.Trim)

            'lblMessage.Text = String.Format(GetMessage("MESSAGE_EDI_REMOVE_PERSON"), strFirstName + " " + strMiddleName + " " + strLastName)

            Dim dictParam As New Dictionary(Of String, String)
            dictParam.Add("FIRSTNAME", strFirstName)
            dictParam.Add("MIDDLENAME", strMiddleName)
            dictParam.Add("LASTNAME", strLastName)

            lblMessage.Text = YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByTextMessageNo(MetaMessageList.MESSAGE_EDI_REMOVE_PERSON, dictParam)

            ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "Open", "showDialog('ConfirmDialog','" & lblMessage.Text & "','YESNO');", True)
            'MessageBox.Show(160, 300, PlaceHolder1, "YMCA-YRS", "Are you sure you want to delete selected record?", MessageBoxButtons.YesNo)
        Catch ex As Exception
            HelperFunctions.LogException("EDIExclusionList_gvEDI_RowCommand", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
        End Try
    End Sub

    'Private Sub gvEDI_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles gvEDI.SelectedIndexChanged
    '    Dim gvRow As GridViewRow
    '    Dim imgbtnSelect As ImageButton
    '    'Dim l_str_SSNo As String
    '    Try
    '        For Each gvRow In Me.gvEDI.Rows
    '            imgbtnSelect = gvRow.FindControl("ImageButtonSelect")
    '            If gvRow.DataItemIndex = Me.gvEDI.SelectedIndex Then
    '                imgbtnSelect.ImageUrl = "images\selected.gif"
    '            Else
    '                imgbtnSelect.ImageUrl = "images\select.gif"
    '            End If
    '        Next

    '        Me.SessionSelectedIndex = Me.gvEDI.SelectedIndex

    '        Me.gvEDI.SelectedRowStyle.CssClass = "DataGrid_SelectedStyle"

    '        Call ClearControls()

    '        'Me.ButtonDelete.Enabled = True
    '        Me.btnnCancel.Enabled = True
    '        Me.btnReport.Enabled = False
    '    Catch ex As Exception
    '        HelperFunctions.LogException("EDIExclusionList_gvEDI_SelectIndexchange", ex)
    '        HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
    '    End Try
    'End Sub
    'Private Sub gvEDI_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles gvEDI.SortCommand
    Private Sub gvEDI_Sorting(sender As Object, e As GridViewSortEventArgs) Handles gvEDI.Sorting
        Dim dv As New DataView
        'Dim l_sort_dataset As DataSet
        Dim SortExpression As String
        'Dim l_sort_datatable As DataTable
        SortExpression = e.SortExpression
        Try

            dv = g_dataset_EDIExlusion.Tables(0).DefaultView
            dv.Sort = SortExpression

            HelperFunctions.gvSorting(Session("EDIListSort"), e.SortExpression, dv)

            Call BindGrid(g_dataset_EDIExlusion)

        Catch ex As Exception
            HelperFunctions.LogException("EDIExclusionList_gvEDI_Sorting", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
        End Try
    End Sub
    Protected Sub ButtonYes_Click(sender As Object, e As EventArgs) Handles btnYes.Click
        Try
            ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "close", "closeDialog('ConfirmDialog')", True)
            ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "clear", "mark_dirty();", True)
            Call PopulateEDIExclusionData(LoadDatasetMode.Delete)
            Me.btnSave.Enabled = True
            Me.btnReport.Enabled = False
            '    Me.ButtonDelete.Enabled = False

        Catch ex As Exception
            HelperFunctions.LogException("EDIExclusionList --> ButtonYes_Click", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
        End Try
    End Sub

    Protected Sub ButtonNo_Click(sender As Object, e As EventArgs) Handles btnNo.Click
        Try
            ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "close", "closeDialog('ConfirmDialog')", True)
            SessionDeleteSSN = Nothing
            Call PopulateEDIExclusionData(LoadDatasetMode.Table)
            Me.btnSave.Enabled = False
            '    Me.ButtonDelete.Enabled = False
        Catch ex As Exception
            HelperFunctions.LogException("EDIExclusionList --> ButtonNo_Click", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
        End Try

    End Sub
#End Region
#Region " Function " 'Functions 
    Private Sub PopulateEDIExclusionData(ByVal paramLoad As LoadDatasetMode)

        ' Dim l_datatable_EDIExlusion As DataTable
        Dim dtEDIExclusionDummy As DataTable
        'Dim l_deleterecord_datatable As DataTable
        'Dim l_str_SelectedUniqueId As String
        'Dim i As Long
        'Dim gvRow As GridViewRow
        'Dim imgbtnSelect As ImageButton
        Dim strSSNo As String
        Dim strPersId As String
        Dim strLastName As String
        Dim strFirstName As String
        Dim strMiddleName As String
        Dim strFundNo As String
        Dim dr As DataRow()
        Dim Sorting As GridViewCustomSort


        Dim str As String

        Try
            If paramLoad = LoadDatasetMode.Table Then

                g_dataset_EDIExlusion = YMCARET.YmcaBusinessObject.EDIExculsionListBOClass.GetEDIExlusionList()
                Me.SessionEDIListData = g_dataset_EDIExlusion
                'Me.SessionDTEDIListData = g_dataset_EDIExlusion.Tables(0)

                Call BindGrid(g_dataset_EDIExlusion)

                If g_dataset_EDIExlusion.Tables(0).Rows.Count = 0 Then
                    Me.btnReport.Enabled = False
                End If

            ElseIf paramLoad = LoadDatasetMode.Session Then

                g_dataset_EDIExlusion = Me.SessionEDIListData

            ElseIf paramLoad = LoadDatasetMode.LookFor Then

                Me.lblNoRecordFound.Visible = False
                'For Each gvRow In Me.gvEDI.Rows
                '    If (gvRow.Cells(m_const_int_SSNo).Text.Trim = Me.SessionSelectedSSNo.Trim) Then
                '        Me.SessionSelectedIndex = i
                '    ElseIf Me.SessionSelectedSSNo.Trim = "" Then
                '        Me.SessionSelectedIndex = Nothing
                '        Me.SessionSelectedSSNo = gvRow.Cells(m_const_int_SSNo).Text.Trim
                '    End If
                '    i = i + 1
                'Next

                'For Each gvRow In Me.gvEDI.Rows
                '    imgbtnSelect = gvRow.FindControl("ImageButtonSelect")
                '    If (gvRow.Cells(m_const_int_SSNo).Text.Trim = Me.SessionSelectedSSNo.Trim) Then
                '        imgbtnSelect.ImageUrl = "images\selected.gif"
                '        gvEDI.SelectedIndex = Convert.ToInt32(SessionSelectedIndex)
                '        'DataGrid_SelectedStyle
                '    Else
                '        imgbtnSelect.ImageUrl = "images\select.gif"
                '    End If
                'Next
                'Me.gvEDI.SelectedRowStyle.CssClass = "DataGrid_SelectedStyle"
                If txtLookFor.Text <> "" Then
                    g_dataset_EDIExlusion.Tables(0).DefaultView.RowFilter = "SSNo = '" + txtLookFor.Text.Trim + "'"
                    BindGrid(g_dataset_EDIExlusion.Tables(0).DefaultView.Table.DataSet)
                End If

                Me.btnnCancel.Enabled = True

            ElseIf paramLoad = LoadDatasetMode.Delete Then

                dtEDIExclusionDummy = g_dataset_EDIExlusion.Tables(1)

                If Session("EDIListSort") IsNot Nothing Then
                    Sorting = Session("EDIListSort")
                    g_dataset_EDIExlusion.Tables(0).DefaultView.Sort = Sorting.SortExpression + " " + Sorting.SortDirection

                End If

                g_dataset_EDIExlusion.Tables(0).DefaultView.RowFilter = "SSNo = '" + SessionDeleteSSN + "'"

                If g_dataset_EDIExlusion.Tables(0).DefaultView(0)("Uniqueid").ToString() <> "" Then

                    str = "Uniqueid = '" + g_dataset_EDIExlusion.Tables(0).DefaultView(0)("Uniqueid").ToString() + "'"
                    dr = dtEDIExclusionDummy.Select(str)

                    If dr.Length > 0 Then
                        dr(0)("bitdelete") = 1
                    End If

                End If

                g_dataset_EDIExlusion.Tables(0).DefaultView(0).Delete()
                g_dataset_EDIExlusion.Tables(0).AcceptChanges()

                'Me.gvEDI.DataSource = g_dataset_EDIExlusion
                'Me.gvEDI.DataBind()
                g_dataset_EDIExlusion.Tables(0).DefaultView.RowFilter = String.Empty
                Call BindGrid(g_dataset_EDIExlusion)

                Me.SessionDeleteSSN = Nothing
                Me.gvEDI.SelectedIndex = -1

                Me.btnnCancel.Enabled = True
                'ElseIf paramLoad = LoadDatasetMode.Sort Then




            ElseIf paramLoad = LoadDatasetMode.FromFindInfo Then

                If Not Session("EDSSNo") Is Nothing Then
                    strSSNo = Session("EDSSNo")
                Else
                    strSSNo = String.Empty
                End If

                If Not Session("EDPersId") Is Nothing Then
                    strPersId = Session("EDPersId")
                Else
                    strPersId = String.Empty
                End If

                If Not Session("EDLastName") Is Nothing Or Session("EDLastName") <> "&nbsp;" Then
                    strLastName = Session("EDLastName")
                Else
                    strLastName = String.Empty
                End If

                If Not Session("EDFirstName") Is Nothing Or Session("EDFirstName") <> "&nbsp;" Then
                    strFirstName = Session("EDFirstName")
                Else
                    strFirstName = String.Empty
                End If

                If Not Session("EDMiddleName") Is Nothing Or Session("EDMiddleName") <> "&nbsp;" Then
                    strMiddleName = Session("EDMiddleName")
                Else
                    strMiddleName = String.Empty
                End If

                If Not Session("EDFundNo") Is Nothing Or Session("EDFundNo") <> "&nbsp;" Then
                    strFundNo = Session("EDFundNo")
                Else
                    strFundNo = String.Empty
                End If

                Me.txtFirstName.Text = strFirstName.ToString
                Me.txtMiddleName.Text = strMiddleName.ToString
                Me.txtLastName.Text = strLastName.ToString
                Me.txtFundNo.Text = strFundNo.ToString
                Me.txtSSNo.Text = strSSNo.ToString
                Me.txtPerssId.Text = strPersId.ToString
                Me.btnFind.Enabled = False
                Me.btnAdd.Enabled = True
                Me.btnnCancel.Enabled = True
                Me.btnReport.Enabled = False
                'Call AddRecordsToGrid(l_str_SSNo, l_str_LastName, l_str_FirstName, l_str_MiddleName, l_str_PersId)
            End If

        Catch ex As Exception
            HelperFunctions.LogException("EDIExclusionList --> PopulateEDIExclusionData", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
        End Try
    End Sub

    Private Sub AddRecordsToGrid(ByVal paramstrSSNo As String, ByVal paramstrLastName As String, ByVal paramstrFirstName As String, ByVal paramstrMiddleName As String, ByVal paramstrPerssId As String)
        Dim dtAddRecord As DataTable
        Dim g_dataset_EDIExlusion As DataSet
        Dim drSSNoExists As DataRow()
        Dim strWhereclause As String
        Dim drAddRecord As DataRow
        Dim dv As New DataView
        Dim Sorting As GridViewCustomSort
        Try
            strWhereclause = " SSNo = '" & paramstrSSNo.ToString & "'"
            If Session("FindInfoFromEDIListScreen") = False Then
                'l_datatable_AddRecord = Me.SessionDTEDIListData
                g_dataset_EDIExlusion = Me.SessionEDIListData
                If g_dataset_EDIExlusion Is Nothing Then
                    g_dataset_EDIExlusion = Me.SessionEDIListData
                    dtAddRecord = g_dataset_EDIExlusion.Tables(0)
                    drSSNoExists = g_dataset_EDIExlusion.Tables("EDIList").Select(strWhereclause)
                Else
                    'l_SSNo_Exists = l_datatable_AddRecord.Select(l_str_whereclause)
                    drSSNoExists = g_dataset_EDIExlusion.Tables("EDIList").Select(strWhereclause)
                End If

                For Each dr As DataRow In drSSNoExists
                    dr = drSSNoExists(0)
                    If dr Is Nothing Then
                        Exit For
                    Else
                        'MessageBox.Show(Me.PlaceHolder1, "YMCA - YRS", "SSNo Already exist in EDI Exclusion list", MessageBoxButtons.Stop)
                        'HelperFunctions.ShowMessageToUser(GetMessage("MESSAGE_EDI_SSN_EXISTS"), EnumMessageTypes.Error)
                        HelperFunctions.ShowMessageToUser(MetaMessageList.MESSAGE_EDI_SSN_EXISTS)

                        Me.btnnCancel.Enabled = False
                        Me.btnReport.Enabled = True
                        Exit Sub
                    End If
                Next
            Else
                g_dataset_EDIExlusion = Me.SessionEDIListData
                '    g_dataset_EDIExlusion = Me.SessionEDIListData
                '    l_datatable_AddRecord = Me.SessionDTEDIListData
                '    If l_datatable_AddRecord Is Nothing Then
                '        l_datatable_AddRecord = g_dataset_EDIExlusion.Tables(0)
                '    End If
            End If
            drAddRecord = g_dataset_EDIExlusion.Tables(0).NewRow       'l_datatable_AddRecord.NewRow()
            drAddRecord("SSNo") = paramstrSSNo.ToString().Trim()
            drAddRecord("LastName") = paramstrLastName.ToString().Trim()
            drAddRecord("FirstName") = paramstrFirstName.ToString().Trim()
            drAddRecord("MiddleName") = paramstrMiddleName.ToString().Trim()
            drAddRecord("Reason") = Me.txtMessage.Text.ToString().Trim()
            drAddRecord("PerssId") = paramstrPerssId.ToString().Trim
            'Commented by Shashi:2010-06-28: To resolve the issue arises in Migration.
            ' l_datarow_AddRecord("UniqueId") = String.Empty 

            drAddRecord("bitAdd") = 1
            'l_datatable_AddRecord.Rows.Add(l_datarow_AddRecord)
            g_dataset_EDIExlusion.Tables(0).Rows.Add(drAddRecord)
            'Me.SessionDTEDIListData = l_datatable_AddRecord
            Me.SessionEDIListData = g_dataset_EDIExlusion

            dv = g_dataset_EDIExlusion.Tables(0).DefaultView()
            If Session("EDIListSort") IsNot Nothing Then
                Sorting = Session("EDIListSort")
                dv.Sort = Sorting.SortExpression + " " + Sorting.SortDirection
            End If
            Me.gvEDI.DataSource = dv 'l_datatable_AddRecord
            Me.gvEDI.SelectedRowStyle.CssClass = "DataGrid_NormalStyle"
            Me.gvEDI.DataBind()
            Me.btnSave.Enabled = True
            Me.btnReport.Enabled = False
        Catch ex As Exception
            HelperFunctions.LogException("EDIExclusionList --> AddRecordsToGrid", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
        End Try
    End Sub
    Private Sub BindGrid(ByVal dsEDIExlusion As DataSet, Optional ByVal strSSNFilter As String = Nothing)
        Try
            Dim dv As New DataView
            Dim Sorting As GridViewCustomSort

            dv = g_dataset_EDIExlusion.Tables(0).DefaultView()

            If Session("EDIListSort") IsNot Nothing Then
                Sorting = Session("EDIListSort")
                dv.Sort = Sorting.SortExpression + " " + Sorting.SortDirection
            End If

            If Session("EDIListPageIndex") IsNot Nothing Then
                gvEDI.PageIndex = Session("EDIListPageIndex")
            End If

            If strSSNFilter IsNot Nothing OrElse strSSNFilter <> String.Empty Then
                dv.RowFilter = "SSNo = '" + txtLookFor.Text.Trim + "'"
            End If

            HelperFunctions.BindGrid(gvEDI, dv, True)

        Catch ex As Exception
            HelperFunctions.LogException("EDIExclusionList --> BindGrid", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
        End Try
    End Sub
    Private Sub ClearControls()
        Try
            Me.txtFirstName.Text = String.Empty
            Me.txtLastName.Text = String.Empty
            Me.txtSSNo.Text = String.Empty
            Me.txtFundNo.Text = String.Empty
            Me.txtMessage.Text = String.Empty
            Me.txtPerssId.Text = String.Empty
            Me.txtMiddleName.Text = String.Empty
            Me.txtLookFor.Text = String.Empty
        Catch ex As Exception
            HelperFunctions.LogException("EDIExclusionList --> ClearControls", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
        End Try
    End Sub
    Private Sub SetControlFocus(ByVal TextBoxFocus As TextBox)
        Dim strScript As String
        Try
            strScript = "<script language='Javascript'>" & _
                            "var obj = document.getElementById('" & TextBoxFocus.ID & "');" & _
                            "if (obj!=null){if (obj.disabled==false){obj.focus();}}" & _
            "</script>"
            'If (Not Me.IsStartupScriptRegistered("scriptsetfocus")) Then
            '    Page.RegisterStartupScript("scriptsetfocus", l_string_script)
            'End If
            'AA:25.06.2014 - BT:2524 :YRS 5.0-2360 Changed for not adding scrtipt tags
            ScriptManager.RegisterStartupScript(Me, GetType(Page), "scriptsetfocus", strScript, False)
        Catch ex As Exception
            HelperFunctions.LogException("EDIExclusionList --> SetControlFocus", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
        End Try
    End Sub
    ' ''' <summary>
    ' ''' To get the message from resource file
    ' ''' </summary>
    ' ''' <param name="strMessageKey"></param>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    'Public Function GetMessage(ByVal strMessageKey) As String
    '    Dim strMessage As String
    '    Try
    '        strMessage = GetGlobalResourceObject("EDIExclusionList", strMessageKey).ToString()
    '        Return strMessage
    '    Catch ex As Exception
    '        HelperFunctions.LogException("EDIExclusionList --> getMessage", ex)
    '        HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
    '        Return Nothing
    '    End Try
    'End Function
#End Region

    
    Private Sub btnClear_Click(sender As Object, e As EventArgs) Handles btnClear.Click
        Try
            g_dataset_EDIExlusion.Tables(0).DefaultView.RowFilter = ""
            txtLookFor.Text = String.Empty
            BindGrid(g_dataset_EDIExlusion)
        Catch ex As Exception
            HelperFunctions.LogException("EDIExclusionList --> btnClear_Click", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
        End Try
    End Sub
End Class

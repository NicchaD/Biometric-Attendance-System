
'*******************************************************************************
' Copyright YMCA Retirement Fund All Rights Reserved. 
'
' Project Name		:	YMCA_YRS
' FileName			:	AddressWebUserControl.aspx.vb
' Author Name		:	Ashutosh Patil
' Employee ID		:	36307
' Email				:	ashutosh.patil@3i-infotech.com
' Contact No		:	8568
' Creation Date		:	25-Feb-2007
'********************************************************************************************************************************
'Modification History
'********************************************************************************************************************************
'Modified By        Date            Description
'Ashutosh Patil     25-Apr-2007     YREN-3298
'Aparna Samala      14-May-2007     Oject reference error
'********************************************************************************************************************************
'********************************************************************************************************************************
'Modified By        Date            Description
'********************************************************************************************************************************
'Aparna Samala      04/09/2007      Fill state dropdown when its value is not selected also.  
'Imran              03/06/2010      Changes for Enhancements -8
'Imran              10/06/2010      BT:534 showing validation messages while changing address.   
'Priya              23-aug-2010     BT-599,YRS 5.0-1126:Address update for non-US/non-Canadian address.
'Sanjay R.          2010.10.05      YRS 5.0-1177:State and Country selection when filling in the address
'Priya              11-Oct-2010     BT-599,YRS 5.0-1126:Address update for non-US/non-Canadian address.
'Priya			    10-Dec-2010     Changes made as sate n country fill up with javascript in user control
'Nikunj Patel	    17-Dec-2010	    BT-701: Not validating city field and other optimizations
'Shashi Shekhar     23-June-2011    YRS 5.0-1336 : Error if Canadian postal code is blank
'Bhavna Shrivatava  09-May-2012     YRS 5.0-1470: Link to Address Edit program from Person Maintenance & address code restructure
'Anudeep            08-10-2012      Bt-1245:Address issue replication and analysis on client machine 
'Manthan Rajguru    2015.09.16      YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
'********************************************************************************************************************************
Public Class Enhance_AddressWebUserControl
    Inherits System.Web.UI.UserControl

    Protected WithEvents TextBoxZip As System.Web.UI.WebControls.TextBox
    Protected WithEvents DropDownListCountry As System.Web.UI.HtmlControls.HtmlSelect
    Protected WithEvents DropDownListState As System.Web.UI.HtmlControls.HtmlSelect
    Protected WithEvents TextBoxAddress2 As System.Web.UI.WebControls.TextBox
    Protected WithEvents reqAddress As System.Web.UI.WebControls.RequiredFieldValidator
    'Protected WithEvents rqState As System.Web.UI.WebControls.RequiredFieldValidator
    'Protected WithEvents rqCountry As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents TextBoxAddress3 As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxAddress1 As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelAddress1 As System.Web.UI.WebControls.Label
    Protected WithEvents LabelAddress2 As System.Web.UI.WebControls.Label
    Protected WithEvents LabelAddress3 As System.Web.UI.WebControls.Label
    Protected WithEvents LabelCity As System.Web.UI.WebControls.Label
    Protected WithEvents LabelState As System.Web.UI.WebControls.Label
    Protected WithEvents LabelCountry As System.Web.UI.WebControls.Label
    Protected WithEvents LabelZip As System.Web.UI.WebControls.Label
    Protected WithEvents rqCity As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents TextBoxCity As System.Web.UI.WebControls.TextBox
    Protected WithEvents DropDownListCountry_hid As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents DropDownListState_hid As System.Web.UI.HtmlControls.HtmlInputHidden
    'BS:2012.03.09:-restructure Address Control while fixing YRS 5.0-1470:
    Protected WithEvents TextBoxEffDate As YMCAUI.DateUserControl
    'BS:2012.04.12:-restructure Address Control while fixing YRS 5.0-1470:
    Protected WithEvents chkNotes As HiddenField
    Protected WithEvents txtFinalNotes As HiddenField
    'BS:2012:04.24:YRS 5.0-1470:BT:951:BadAddress remove from participant and put on address control
    Protected WithEvents CheckboxIsBadAddress As System.Web.UI.WebControls.CheckBox
#Region " Variable Declartions "
    Dim m_strDropwonlist As String
    Dim m_Str_Msg As String
    Dim m_strPerssId As String
    'Dim m_intShowData As Integer
    Dim m_int_Primary As Integer
    Dim m_GridClicked As Boolean
    Dim m_boolFromWebAddr As Boolean
    Dim m_bool_checkValid As Boolean
    Dim m_bool_IsfromBeniFiciarySettlement As Boolean
    Dim m_dataset_AddressInfo As New DataSet
    Dim m_ds_States As DataSet
    Dim m_dt_State As New DataTable
    Dim m_dt_State_filtered As New DataTable
    Dim m_dr_state As DataRow
    Dim m_dr_SelectedState As DataRow()
    Dim m_bool_Isvalid As Boolean
    Dim m_bool_frommaintenancescren As Boolean

#End Region 'Varaible Declarations
#Region " Web Form Designer Generated Code "
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
#Region " Properties "
    Public Sub New()
        m_strDropwonlist = ""
        m_Str_Msg = ""
        m_strPerssId = ""
        'm_intShowData = 0
        m_int_Primary = 0
        m_GridClicked = False
        m_boolFromWebAddr = False
        m_bool_checkValid = False
        m_bool_IsfromBeniFiciarySettlement = False
        m_bool_Isvalid = False
        m_dataset_AddressInfo = Nothing
        m_ds_States = Nothing
        m_dt_State = Nothing
        m_dt_State_filtered = Nothing
        m_dr_state = Nothing
        m_dr_SelectedState = Nothing
    End Sub
    Dim m_AllowNotes As Boolean = False
    Public Property AllowNote() As Boolean
        Get
            Return m_AllowNotes
        End Get
        Set(ByVal Value As Boolean)
            m_AllowNotes = Value
        End Set
    End Property
    Dim m_AllowEffDate As Boolean = False
    Public Property AllowEffDate() As Boolean
        Get
            Return m_AllowEffDate
        End Get
        Set(ByVal Value As Boolean)
            m_AllowEffDate = Value
        End Set
    End Property
    Public Property Notes() As String
        Get
            If Not Me.txtFinalNotes.Value Is String.Empty Then
                Return Me.txtFinalNotes.Value.ToString
            Else
                Return String.Empty
            End If
        End Get
        Set(ByVal Value As String)
            Me.txtFinalNotes.Value = Value
        End Set
    End Property
    Public Property BitImp() As Boolean
        Get
            If chkNotes.Value = "True" Then
                Return True
            Else
                Return False
            End If
        End Get
        Set(ByVal Value As Boolean)
            chkNotes.Value = Value
        End Set
    End Property
    Public Property EffectiveDate() As String
        Get
            If Not Me.TextBoxEffDate.Text Is String.Empty Then
                Return Me.TextBoxEffDate.Text.ToString
            Else
                Return String.Empty
            End If
        End Get
        Set(ByVal Value As String)
            Me.TextBoxEffDate.Text = Value
        End Set
    End Property
    Public Property IsBadAddress() As Boolean
        Get
            If CheckboxIsBadAddress.Checked = True Then
                Return True
            Else
                Return False
            End If
        End Get
        Set(ByVal Value As Boolean)
            Me.CheckboxIsBadAddress.Checked = Value
        End Set
    End Property
    Public Property ZipCode() As String
        Get
            If Not Me.TextBoxZip.Text Is String.Empty Then
                Return Me.TextBoxZip.Text.ToString
            Else
                Return String.Empty
            End If
        End Get
        Set(ByVal Value As String)
            Me.TextBoxZip.Text = Value
        End Set
    End Property
    Public Property Address1() As String
        Get
            If Not Me.TextBoxAddress1.Text Is String.Empty Then
                Return Me.TextBoxAddress1.Text.ToString
            Else
                Return String.Empty
            End If
        End Get
        Set(ByVal Value As String)
            Me.TextBoxAddress1.Text = Value
        End Set
    End Property
    Public Property Address2() As String
        Get
            If Not Me.TextBoxAddress2.Text Is String.Empty Then
                Return Me.TextBoxAddress2.Text.ToString
            Else
                Return String.Empty
            End If
        End Get
        Set(ByVal Value As String)
            Me.TextBoxAddress2.Text = Value
        End Set
    End Property
    Public Property Address3() As String
        Get
            If Not Me.TextBoxAddress3.Text Is String.Empty Then
                Return Me.TextBoxAddress3.Text.ToString
            Else
                Return String.Empty
            End If
        End Get
        Set(ByVal Value As String)
            Me.TextBoxAddress3.Text = Value
        End Set
    End Property
    Public Property City() As String
        Get
            If Not Me.TextBoxCity.Text Is String.Empty Then
                Return Me.TextBoxCity.Text.ToString
            Else
                Return String.Empty
            End If
        End Get
        Set(ByVal Value As String)
            Me.TextBoxCity.Text = Value
        End Set
    End Property
    Public ReadOnly Property DropDownListStateText() As String
        Get
            If DropDownListState_hid.Value = String.Empty Then
                Return "-Select State-"
            End If
            'If Not Me.DropDownListState.SelectedItem.Text Is String.Empty Then
            '	Return Me.DropDownListState.SelectedItem.Text.ToString
            If Not Me.DropDownListState_hid.Value Is String.Empty Then
                Return getSelectedStateText(Me.DropDownListState_hid.Value)
                'Return Me.DropDownListState_hid.Value.ToString
            Else
                Return String.Empty
            End If
        End Get
        'Set(ByVal Value As String)
        '	Me.DropDownListState_hid.Value = Value
        'End Set
    End Property
    Public Property DropDownListStateValue() As String
        Get
            If DropDownListState_hid.Value = String.Empty Then
                Return "-Select State-"
            End If
            'If Not Me.DropDownListState.SelectedValue Is String.Empty Then
            '	Return Me.DropDownListState.SelectedValue.ToString
            If Not Me.DropDownListState_hid.Value Is String.Empty Then
                Return Me.DropDownListState_hid.Value.ToString.Trim
            Else
                Return String.Empty
            End If
        End Get
        Set(ByVal Value As String)
            'Me.DropDownListState.SelectedValue = Value
            Me.DropDownListState_hid.Value = Value
        End Set
    End Property
    Public Property DropDownListCountryValue() As String
        Get
            If DropDownListCountry_hid.Value = String.Empty Then
                Return "-Select Country-"
            End If
            'If Not Me.DropDownListCountry.SelectedValue Is String.Empty Then
            '	Return Me.DropDownListCountry.SelectedValue.ToString
            If Not Me.DropDownListCountry_hid.Value Is String.Empty Then
                Return Me.DropDownListCountry_hid.Value.ToString.Trim
            Else
                Return String.Empty
            End If
        End Get
        Set(ByVal Value As String)
            'Me.DropDownListCountry.SelectedValue = Value
            Me.DropDownListCountry_hid.Value = Value
        End Set
    End Property
    Public ReadOnly Property DropDownListCountryText() As String
        Get
            If DropDownListCountry_hid.Value = String.Empty Then
                Return "-Select Country-"
            End If
            'If Not Me.DropDownListCountry.SelectedItem.Text Is String.Empty Then
            '	Return Me.DropDownListCountry.SelectedItem.Text.ToString
            If Not Me.DropDownListCountry_hid.Value Is String.Empty Then
                Return getSelectedCountryText(Me.DropDownListCountry_hid.Value)
                'Return Me.DropDownListCountry_hid.Value.ToString
                'Else
                '	Return String.Empty
                'End If
            Else
                Return String.Empty
            End If
        End Get
        'Set(ByVal Value As String)
        '	'Me.DropDownListCountry.SelectedItem.Text = Value
        '	Me.DropDownListCountry_hid.Value = Value
        'End Set
    End Property
    Public Property guiPerssId() As String
        Get
            If Not m_strPerssId Is String.Empty Then
                Return m_strPerssId
            Else
                Return String.Empty
            End If
        End Get
        Set(ByVal Value As String)
            m_strPerssId = Value
        End Set
    End Property
    'Public Property ShowDataForParticipant() As Integer
    '	Get
    '		Return m_intShowData
    '	End Get
    '	Set(ByVal Value As Integer)
    '		m_intShowData = Value
    '		Call LoadDataToConrols()
    '	End Set
    'End Property
    Public Function ShowDataForParticipant() As Boolean
        Try
            LoadDataToConrols()
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function
    Public Property IsFromBenificarySettlement() As Boolean
        Get
            Return m_bool_IsfromBeniFiciarySettlement
        End Get
        Set(ByVal Value As Boolean)
            m_bool_IsfromBeniFiciarySettlement = Value
        End Set
    End Property
    Public Property GridClicked() As Boolean
        Get
            Return m_GridClicked
        End Get
        Set(ByVal Value As Boolean)
            m_GridClicked = Value
            If Me.FromWebAddr = False Then
                Call LoadDataToConrols()
            Else
                Call ShowDataForWebAddress()
            End If
        End Set
    End Property
    Public Property FromWebAddr() As Boolean
        Get
            Return m_boolFromWebAddr
        End Get
        Set(ByVal Value As Boolean)
            m_boolFromWebAddr = Value
        End Set
    End Property
    Public Property IsMaintenanceScreen() As Boolean
        Get
            Return m_bool_frommaintenancescren
        End Get
        Set(ByVal Value As Boolean)
            m_bool_frommaintenancescren = Value
        End Set
    End Property
    Public Property EnableControls() As Boolean
        Get
            'NP:Comment - This nmethod will always only return the state of the first control's enabled status
            Return Me.TextBoxAddress1.Enabled = True
            'Return Me.TextBoxAddress2.Enabled = True
            'Return Me.TextBoxAddress3.Enabled = True
            'Return Me.TextBoxCity.Enabled = True
            'Return Me.TextBoxZip.Enabled = True
            ''BS:2012.03.09:-restructure Address Control while fixing YRS 5.0-1470:
            'Return Me.TextBoxEffDate.Enabled = True
            ''Return Me.DropDownListState_hid.Disabled = False
            ''Return Me.DropDownListCountry_hid.Disabled = False
            'Return Me.DropDownListCountry.Disabled = False
            'Return Me.DropDownListState.Disabled = False
            ''Return Me.DropDownListState.Enabled = True
            ''Return Me.DropDownListCountry.Enabled = True
        End Get
        Set(ByVal Value As Boolean)
            Me.TextBoxAddress1.Enabled = Value
            Me.TextBoxAddress2.Enabled = Value
            Me.TextBoxAddress3.Enabled = Value
            Me.TextBoxCity.Enabled = Value
            Me.TextBoxZip.Enabled = Value
            'BS:2012.03.09:-restructure Address Control while fixing YRS 5.0-1470:
            Me.TextBoxEffDate.Enabled = Value
            'Me.DropDownListState_hid.Disabled = Not Value
            'Me.DropDownListCountry_hid.Disabled = Not Value
            Me.DropDownListCountry.Disabled = Not Value
            Me.DropDownListState.Disabled = Not Value
            'Me.DropDownListState.Enabled = Value
            'Me.DropDownListCountry.Enabled = Value
            txtFinalNotes.Value = String.Empty
            chkNotes.Value = String.Empty
            CheckboxIsBadAddress.Enabled = Value
        End Set
    End Property
    Public Property MakeReadonly() As Boolean
        Get
            Return Me.TextBoxAddress1.ReadOnly = True
            Return Me.TextBoxAddress2.ReadOnly = True
            Return Me.TextBoxAddress3.ReadOnly = True
            Return Me.TextBoxCity.ReadOnly = True
            Return Me.TextBoxZip.ReadOnly = True
        End Get
        Set(ByVal Value As Boolean)
            Me.TextBoxAddress1.ReadOnly = Value
            Me.TextBoxAddress2.ReadOnly = Value
            Me.TextBoxAddress3.ReadOnly = Value
            Me.TextBoxCity.ReadOnly = Value
            Me.TextBoxZip.ReadOnly = Value
        End Set
    End Property
    Public Property ClearControls() As String
        Get
            Return Me.TextBoxAddress1.Text = ""
            Return Me.TextBoxAddress2.Text = ""
            Return Me.TextBoxAddress3.Text = ""
            Return Me.TextBoxCity.Text = ""
            Return Me.TextBoxZip.Text = ""
            'Return Me.DropDownListState_hid.SelectedIndex = 0
            'Return Me.DropDownListCountry_hid.SelectedIndex = 0
            Return Me.DropDownListState_hid.Value = ""
            Return Me.DropDownListCountry_hid.Value = ""
        End Get
        Set(ByVal Value As String)
            Me.TextBoxAddress1.Text = Value
            Me.TextBoxAddress2.Text = Value
            Me.TextBoxAddress3.Text = Value
            Me.TextBoxCity.Text = Value
            Me.TextBoxZip.Text = Value
            'Me.DropDownListState.SelectedIndex = 0
            'Me.DropDownListCountry.SelectedIndex = 0
            Me.DropDownListState_hid.Value = ""
            Me.DropDownListCountry_hid.Value = ""
            'Call SetCountryStateDropwdowns()
        End Set
    End Property
    Public Property IsPrimary() As String
        Get
            If Not ViewState("IsPrimary") Is Nothing Then
                Return ViewState("IsPrimary")
            Else
                Return m_int_Primary
            End If
        End Get
        Set(ByVal Value As String)
            ViewState("IsPrimary") = Value
            m_int_Primary = Value
        End Set
    End Property
#End Region     '" Properties "
#Region " Events"
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        Dim l_intPri As Integer
        Dim l_bool_BenificaryGridvalue As Boolean
        Try
            Me.TextBoxCity.Attributes.Add("OnKeyPress", "javascript:ValidateAlphaNumeric();")
            Me.TextBoxCity.Attributes.Add("OnKeyUp", "javascript:fncKeyCityStop();")
            Me.TextBoxCity.Attributes.Add("OnKeyDown", "javascript:fncKeyCityStop();")
            Me.TextBoxZip.Attributes.Add("OnKeyUp", "javascript:fncKeyZipStop();")
            Me.TextBoxZip.Attributes.Add("OnKeyDown", "javascript:fncKeyZipStop();")
            TextBoxZip.Attributes.Add("onkeypress", "javascript:ValidateZipCodeKeyPress(this,'" & DropDownListCountry.ClientID & "');")
            TextBoxZip.Attributes.Add("onblur", "javascript:validateZipCode(this,'" & DropDownListCountry.ClientID & "');")
            If Me.IsPostBack = False Then
                Call LoadDataToConrols()
                l_intPri = Me.IsPrimary
                If l_intPri <> 1 Then
                    Call SetValidationsForSecondary()
                    'Call SetAttributesForSecondary(Me.DropDownListCountry.SelectedValue.ToString())
                    'Call SetAttributesForSecondary(Me.DropDownListCountry_hid.Value.ToString())
                End If
            End If
            'Priya 8-Dec-10 Address control
            LoadCountryState()
            'Priya 8-Dec-10 Address control
        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim().ToString()), False)
        End Try
    End Sub
#End Region '" Events "
#Region " Custom Methods "
    'Priya 10-Dec-2010 : Make dropdown state and country Javascript
    Private Function SanitizeValue(ByVal s As String) As String
        Return s.Replace("'", "")
    End Function
    'Private Function getJsFordrdCountries() As String
    '	Return "javascipt:setStates('" & DropDownListCountry.ClientID & "', '" & DropDownListState.ClientID & "', null);"
    'End Function
    'Private Function getJsFordrdStates() As String
    '	Return "javascipt:setCountryCodeForStateId('" & DropDownListCountry.ClientID & "', '" + DropDownListState.ClientID & "', this.value);"
    'End Function
    Private Function getSelectedStateText(ByVal strStateCode As String) As String
        Dim ds As DataSet = DirectCast(Session("States"), DataSet)
        Dim dr As DataRow()
        If HelperFunctions.isEmpty(ds) Then Return String.Empty
        If strStateCode = String.Empty Then Return String.Empty
        dr = ds.Tables(0).Select("chvcodevalue = '" & strStateCode & "'")
        If dr.Length = 0 Then Return String.Empty
        Return dr(0)("chvDescription").ToString().Trim()
    End Function
    Private Function getSelectedCountryText(ByVal strCountryCode As String) As String
        Dim ds As DataSet = DirectCast(Session("Countries"), DataSet)
        Dim dr As DataRow()
        If HelperFunctions.isEmpty(ds) Then Return String.Empty
        If strCountryCode = String.Empty Then Return String.Empty
        dr = ds.Tables(0).Select("chvAbbrev = '" & strCountryCode & "'")
        If dr.Length = 0 Then Return String.Empty
        Return dr(0)("chvDescription").ToString().Trim()
    End Function
    Private Function LoadCountryState()
        Dim dsCountries As New DataSet()
        'ds = LookUpAddressCountry();
        Dim dsSates As New DataSet()
        'dsSates = GetStates();
        Try
            dsCountries = YMCARET.YmcaBusinessObject.YMCAAddressBOClass.LookUpAddressCountry()
            dsSates = YMCARET.YmcaBusinessObject.YMCAAddressBOClass.GetStates()
            Session("States") = dsSates
            Session("Countries") = dsCountries
            If HelperFunctions.isEmpty(dsCountries) Then
                Exit Function
            End If
            If HelperFunctions.isEmpty(dsSates) Then
                Exit Function
            End If

            'Code to Write javascript array  frod dataset
            Dim javaScript As New System.Text.StringBuilder()
            javaScript.Append("var countries = [" & vbCrLf)

            Dim i, j As Integer
            Dim dataRow As DataRow
            For i = 0 To dsCountries.Tables(0).Rows.Count - 1
                dataRow = dsCountries.Tables(0).Rows(i)
                javaScript.Append(" [ ")
                For j = 0 To dataRow.Table.Columns.Count - 1
                    javaScript.Append("'" + SanitizeValue(dataRow(j).ToString()) + "'")
                    If ((j + 1) < dataRow.Table.Columns.Count) Then
                        javaScript.Append(",")
                    End If
                Next
                javaScript.Append(" ]")
                If ((i + 1) < dsCountries.Tables(0).Rows.Count) Then
                    javaScript.Append("," & vbCrLf)
                End If
            Next
            javaScript.Append(vbCrLf & "];" & vbCrLf)
            javaScript.Append("var states = new Array(); " & vbCrLf)

            Dim strCountryCode As String = String.Empty
            Dim dv As DataView
            For i = 0 To dsCountries.Tables(0).Rows.Count - 1

                strCountryCode = dsCountries.Tables(0).Rows(i)("chvAbbrev").ToString()
                dv = dsSates.Tables(0).DefaultView
                dv.RowFilter = "chvCountryCode = '" + SanitizeValue(strCountryCode) + "'"
                If dv.Count = 0 Then
                    Continue For
                End If
                javaScript.Append("states['" + strCountryCode + "'] = new Array( " & vbCrLf)
                For j = 0 To dv.Count - 1
                    javaScript.Append("['" + SanitizeValue(dv(j).Row("chvcodevalue").ToString().Trim()) + "' ,'" + SanitizeValue(dv(j).Row("chvDescription").ToString().Trim()) + "' ,'" + SanitizeValue(dv(j).Row("chvCountryCode").ToString().Trim()) + "' ] ")
                    If j < dv.Count - 1 Then
                        javaScript.Append(",")
                    End If
                Next
                javaScript.Append(" ); " & vbCrLf)
            Next

            'BS:2012.05.30:BT:951:YRS:1470:-create array for notes reason and source
            Dim dsNotesReason As New DataSet()
            dsNotesReason = YMCARET.YmcaBusinessObject.YMCAAddressBOClass.GetAddressNotesReasonSource("AddressNotesReason")
            'Session("Countries") = dsCountries
            If HelperFunctions.isEmpty(dsNotesReason) Then
                Exit Function
            End If
            'Code to Write javascript array  frod dataset

            javaScript.Append("var NotesReason = [" & vbCrLf)

            Dim k, l As Integer
            Dim dataRowReason As DataRow
            For k = 0 To dsNotesReason.Tables(0).Rows.Count - 1
                dataRowReason = dsNotesReason.Tables(0).Rows(k)
                javaScript.Append(" [ ")
                For l = 0 To dataRowReason.Table.Columns.Count - 1
                    javaScript.Append("'" + SanitizeValue(dataRowReason(l).ToString()) + "'")
                    If ((l + 1) < dataRowReason.Table.Columns.Count) Then
                        javaScript.Append(",")
                    End If
                Next
                javaScript.Append(" ]")
                If ((k + 1) < dsNotesReason.Tables(0).Rows.Count) Then
                    javaScript.Append("," & vbCrLf)
                End If
            Next
            javaScript.Append(vbCrLf & "];" & vbCrLf)

            'BS:2012.05.30:BT:951:YRS:1470:-create array for notes reason and source
            Dim dsNotesSource As New DataSet()
            dsNotesSource = YMCARET.YmcaBusinessObject.YMCAAddressBOClass.GetAddressNotesReasonSource("AddressNotesSource")
            'Session("Countries") = dsCountries
            If HelperFunctions.isEmpty(dsNotesSource) Then
                Exit Function
            End If
            'Code to Write javascript array  frod dataset

            javaScript.Append("var NotesSource = [" & vbCrLf)

            Dim g, h As Integer
            Dim dataRowSource As DataRow
            For g = 0 To dsNotesSource.Tables(0).Rows.Count - 1
                dataRowSource = dsNotesSource.Tables(0).Rows(g)
                javaScript.Append(" [ ")
                For h = 0 To dataRowSource.Table.Columns.Count - 1
                    javaScript.Append("'" + SanitizeValue(dataRowSource(h).ToString()) + "'")
                    If ((h + 1) < dataRowSource.Table.Columns.Count) Then
                        javaScript.Append(",")
                    End If
                Next
                javaScript.Append(" ]")
                If ((g + 1) < dsNotesSource.Tables(0).Rows.Count) Then
                    javaScript.Append("," & vbCrLf)
                End If
            Next
            javaScript.Append(vbCrLf & "];" & vbCrLf)




            'javaScript.Append("addLoadEvent(function () { setCountries(); setStates(); });");
            If Not Me.Page.ClientScript.IsClientScriptBlockRegistered("ArrayScript") Then
                Me.Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "ArrayScript", javaScript.ToString(), True)
            End If

            javaScript.Clear()

            'javaScript.Append("$(document).ready(function () {setCountries('" + DropDownListCountry.ClientID + "', '" + DropDownListState.ClientID + "', '" + DropDownListCountry_hid.Value + "');setStates('" + DropDownListCountry.ClientID + "', '" + DropDownListState.ClientID + "', '" + DropDownListState_hid.Value + "');});")
            'BS:2012.04.24:YRS 5.0-1470:BT:951:initialize address detail for set QAmatch hidden field value
            'javaScript.Append("$(document).ready(function() { initializeAddressControl('" + DropDownListCountry.ClientID + "', '" + DropDownListState.ClientID + "', '" + DropDownListCountry_hid.ClientID + "', '" + DropDownListState_hid.ClientID + "','" + TextBoxZip.ClientID + "');});")
            javaScript.Append("$(document).ready(function() { initializeAddressControl('" + DropDownListCountry.ClientID + "', '" + DropDownListState.ClientID + "', '" + DropDownListCountry_hid.ClientID + "', '" + DropDownListState_hid.ClientID + "','" + TextBoxZip.ClientID + "','" + TextBoxAddress1.ClientID + "','" + TextBoxAddress2.ClientID + "','" + TextBoxAddress3.ClientID + "','" + TextBoxCity.ClientID + "','" + CType(TextBoxEffDate.FindControl("TextBoxUCDate"), TextBox).ClientID + "');});")
            javaScript.Append("$(document).ready(function() { initializeAddressControl2('" + DropDownListCountry.ClientID + "', '" + DropDownListState.ClientID + "', '" + DropDownListCountry_hid.ClientID + "', '" + DropDownListState_hid.ClientID + "','" + TextBoxZip.ClientID + "','" + TextBoxAddress1.ClientID + "','" + TextBoxAddress2.ClientID + "','" + TextBoxAddress3.ClientID + "','" + TextBoxCity.ClientID + "','" + CType(TextBoxEffDate.FindControl("TextBoxUCDate"), TextBox).ClientID + "','" + CheckboxIsBadAddress.ClientID + "','" + txtFinalNotes.ClientID + "','" + chkNotes.ClientID + "');});")
            Me.Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), Me.ClientID, javaScript.ToString(), True)
            'DropDownListCountry.Attributes.Add("onChange", getJsFordrdCountries())
            'DropDownListState.Attributes.Add("onChange", getJsFordrdStates())

        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function ValidateAddress() As String
        Dim str As String = IIf(IsPrimary <> "1", "Secondary", "Primary")
        'If all the line1, line2, line3, city, country, state, zip are blank and this is the primary control
        If (IsPrimary = "1" AndAlso TextBoxAddress1.Text = String.Empty _
          AndAlso TextBoxAddress2.Text = String.Empty AndAlso TextBoxAddress3.Text = String.Empty _
          AndAlso TextBoxZip.Text = String.Empty AndAlso DropDownListCountry_hid.Value = String.Empty _
          AndAlso DropDownListState_hid.Value = String.Empty AndAlso TextBoxCity.Text = String.Empty AndAlso TextBoxEffDate.Text = String.Empty) Then ''BS:2012.03.09:-restructure Address Control
            Return "Address information not entered."
        End If
        'If any of the line1, line2, line3, city, country, state or zip are entered 
        If (TextBoxAddress1.Text <> String.Empty OrElse TextBoxAddress2.Text <> String.Empty _
         OrElse TextBoxAddress3.Text <> String.Empty OrElse TextBoxZip.Text <> String.Empty _
         OrElse DropDownListCountry_hid.Value <> String.Empty OrElse DropDownListState_hid.Value <> String.Empty _
         OrElse TextBoxCity.Text <> String.Empty OrElse TextBoxEffDate.Text <> String.Empty) Then 'BS:2012.03.09:-restructure Address Control

            'Verify if all of the mandatory information has been entered
            If TextBoxAddress1.Text.Trim = "" Then
                Return "Please enter Address1 for " & str & " Address."
            End If
            If TextBoxCity.Text.Trim = "" Then
                Return "Please enter City for " & str & " Address."
            End If
            If (DropDownListCountry_hid.Value = "" OrElse DropDownListCountry_hid.Value = "null") Then
                Return "Please select Country for " & str & " Address."
            End If
            'Perform additional validations for US and CA
            Dim l_string_Message As String = String.Empty
            If (DropDownListCountry_hid.Value = "US" Or DropDownListCountry_hid.Value = "CA") Then
                If TextBoxZip.Text = "" Then
                    l_string_Message = "Please enter Zip Code"
                End If
                If DropDownListState_hid.Value = "" Then
                    l_string_Message = "Please select state"
                End If
                If l_string_Message <> "" Then
                    If DropDownListCountry_hid.Value = "US" Then
                        Return l_string_Message & " for United States"
                    ElseIf DropDownListCountry_hid.Value = "CA" Then
                        Return l_string_Message & " for Canada"
                    End If
                End If
                l_string_Message = String.Empty
                If DropDownListCountry_hid.Value = "US" And (System.Text.RegularExpressions.Regex.IsMatch(TextBoxZip.Text, "(^[0-9]{5}$)|(^[0-9]{9}$)")) = False Then
                    Return "Invalid Zip Code format for United States"
                ElseIf DropDownListCountry_hid.Value = "CA" And (System.Text.RegularExpressions.Regex.IsMatch(TextBoxZip.Text, "^([a-zA-Z])([0-9])([a-zA-Z]) ([0-9])([a-zA-Z])([0-9])$")) = False Then
                    Return "Invalid Zip Code format for Canada"
                End If
            End If
        End If
        Return String.Empty

    End Function
    'End Priya 10-Dec-2010 : Make dropdown state and country Javascript
    Private Function LoadDataToConrols() As String
        Dim l_dataset_States As DataSet
        Dim l_datarow_state As DataRow()
        Dim l_strCountryCode As String
        Dim l_bool_IsFromBenificiarySettlement As Boolean
        Dim l_dataset_dsAddressStateType As DataSet
        Dim l_dataset_dsAddressCountry As DataSet
        Dim InsertRowState As DataRow
        Dim InsertRowCountry As DataRow
        Dim str_PerssId As String
        Dim int_IsPrimary As Integer
        Dim m_boolFromWebAddr As Boolean
        Dim l_str_zipcode As String

        Try
            l_strCountryCode = String.Empty

            l_dataset_dsAddressCountry = YMCARET.YmcaBusinessObject.YMCAAddressBOClass.LookUpAddressCountry()

            InsertRowCountry = l_dataset_dsAddressCountry.Tables(0).NewRow()
            InsertRowCountry.Item("chvAbbrev") = String.Empty
            InsertRowCountry.Item("chvDescription") = String.Empty

            If Not l_dataset_dsAddressCountry Is Nothing Then
                l_dataset_dsAddressCountry.Tables(0).Rows.InsertAt(InsertRowCountry, 0)
            End If

            m_boolFromWebAddr = Me.FromWebAddr

            If m_boolFromWebAddr = True Then
                'Call SetAttributes(Me.DropDownListCountry.SelectedValue)
                'Call SetCountStZipCodeMandatoryOnSelection()
                Exit Function
            End If

            int_IsPrimary = Me.IsPrimary
            str_PerssId = Me.guiPerssId

            If int_IsPrimary = 1 Then
                Session("Sender") = "AddressWebUserControl1:DropDownListCountry"
            Else
                Session("Sender") = "AddressWebUserControl2:DropDownListCountry"
            End If

            l_bool_IsFromBenificiarySettlement = Me.IsFromBenificarySettlement

            If str_PerssId <> "" And (int_IsPrimary = 0 Or int_IsPrimary = 1) Then
                m_dataset_AddressInfo = YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.SearchParicipantAddress(str_PerssId, int_IsPrimary)
            Else
                'Call SetAttributesForSecondary(Me.DropDownListCountry.SelectedValue)
                'Call SetAttributesForSecondary(Me.DropDownListCountry_hid.Value)
                Exit Function
            End If

            If m_dataset_AddressInfo.Tables(0).Rows.Count = 0 Then
                Session("AddressPresent") = 0
                If int_IsPrimary = 0 Then
                    'Call SetAttributesForSecondary(Me.DropDownListCountry_hid.Value)
                Else
                    If m_bool_frommaintenancescren = False Then
                        'Call SetCountStZipCodeMandatoryOnSelection()
                    Else
                        Call SetValidationsForSecondary()
                    End If
                End If
                Exit Function
            Else
                Session("AddressPresent") = 1
            End If

            Me.TextBoxAddress1.Text = IIf(IsDBNull(m_dataset_AddressInfo.Tables("AddressInfo").Rows(0).Item("Address1").ToString().Trim) = True, "", m_dataset_AddressInfo.Tables("AddressInfo").Rows(0).Item("Address1").ToString().Trim)
            Me.TextBoxAddress2.Text = IIf(IsDBNull(m_dataset_AddressInfo.Tables("AddressInfo").Rows(0).Item("Address2").ToString().Trim) = True, "", m_dataset_AddressInfo.Tables("AddressInfo").Rows(0).Item("Address2").ToString().Trim)
            Me.TextBoxAddress3.Text = IIf(IsDBNull(m_dataset_AddressInfo.Tables("AddressInfo").Rows(0).Item("Address3").ToString().Trim) = True, "", m_dataset_AddressInfo.Tables("AddressInfo").Rows(0).Item("Address3").ToString().Trim)
            Me.TextBoxCity.Text = IIf(IsDBNull(m_dataset_AddressInfo.Tables("AddressInfo").Rows(0).Item("City").ToString().Trim) = True, "", m_dataset_AddressInfo.Tables("AddressInfo").Rows(0).Item("City").ToString().Trim)
            Me.TextBoxEffDate.Text = IIf(IsDBNull(m_dataset_AddressInfo.Tables("AddressInfo").Rows(0).Item("EffDate").ToString().Trim) = True, "", m_dataset_AddressInfo.Tables("AddressInfo").Rows(0).Item("EffDate").ToString().Trim)
            If m_dataset_AddressInfo.Tables("AddressInfo").Rows(0).Item("Country").ToString.Trim() = "CA" Then

                'Shashi Shekhar:23-june-2011:For YRS 5.0-1336 : Error if Canadian postal code is blank
                'l_str_zipcode = m_dataset_AddressInfo.Tables("AddressInfo").Rows(0).Item("Zip").ToString
                l_str_zipcode = IIf(IsDBNull(m_dataset_AddressInfo.Tables("AddressInfo").Rows(0).Item("Zip").ToString().Trim) = True, "", m_dataset_AddressInfo.Tables("AddressInfo").Rows(0).Item("Zip").ToString().Trim)
                If (l_str_zipcode = "") Then
                    TextBoxZip.Text = ""
                Else
                    TextBoxZip.Text = l_str_zipcode.Replace(" ", "").Substring(0, 3) + " " + l_str_zipcode.Replace(" ", "").Substring(3, 3)
                End If
                '----------------------------------------------------------------------------------------------------
            Else
                Me.TextBoxZip.Text = IIf(IsDBNull(m_dataset_AddressInfo.Tables("AddressInfo").Rows(0).Item("Zip").ToString().Trim) = True, "", m_dataset_AddressInfo.Tables("AddressInfo").Rows(0).Item("Zip").ToString().Trim)
            End If
            If IsDBNull(m_dataset_AddressInfo.Tables("AddressInfo").Rows(0).Item("State")) = True Or m_dataset_AddressInfo.Tables("AddressInfo").Rows(0).Item("State").ToString.Trim = "" Or m_dataset_AddressInfo.Tables("AddressInfo").Rows(0).Item("State").ToString.Trim = "  " Then
                ViewState("SelectedState") = Nothing
            Else
                ViewState("SelectedState") = m_dataset_AddressInfo.Tables("AddressInfo").Rows(0).Item("State")
            End If
            'Me.DropDownListCountry.SelectedValue = m_dataset_AddressInfo.Tables("AddressInfo").Rows(0).Item("Country").ToString.Trim
            If (m_dataset_AddressInfo.Tables("AddressInfo").Rows(0).IsNull("Country") = True) Then
                Me.DropDownListCountry_hid.Value = String.Empty
            Else
                Me.DropDownListCountry_hid.Value = m_dataset_AddressInfo.Tables("AddressInfo").Rows(0).Item("Country").ToString.Trim
            End If

            If (m_dataset_AddressInfo.Tables("AddressInfo").Rows(0).IsNull("State") = True) Then
                Me.DropDownListState_hid.Value = String.Empty
            Else
                Me.DropDownListState_hid.Value = m_dataset_AddressInfo.Tables("AddressInfo").Rows(0).Item("State").ToString.Trim
            End If
            'Me.DropDownListState_hid.Value = m_dataset_AddressInfo.Tables("AddressInfo").Rows(0).Item("State").ToString.Trim
            Call Address_DropDownListState()
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    'Private Function SetAttributesForSecondary(ByVal CountryValue As String) As String
    '	'************************************************************************************************************
    '	' Author            : Ashutosh Patil 
    '	' Created On        : 25-Jan-2007
    '	' Desc              : This function will set max length of Zip Code against the respective countries
    '	'                     For USA-9,CANADA-6,OTHER COUNTRIES - Default is 10
    '	' Related To        : YREN-3029,YREN-3028
    '	' Modifed By        : 
    '	' Modifed On        :
    '	' Reason For Change : 
    '	'************************************************************************************************************
    '	Try
    '		If CountryValue = "US" Then
    '			TextBoxZip.MaxLength = 9
    '			TextBoxZip.Attributes.Add("onkeypress", "javascript:ValidateNumeric();")
    '		ElseIf CountryValue = "CA" Then
    '			TextBoxZip.MaxLength = 7
    '			TextBoxZip.Attributes.Add("onkeypress", "javascript:ValidateAlphaNumeric();")
    '			'Priya 23-aug-2010 YRS 5.0-1126:Address update for non-US/non-Canadian address. Aded "ElseIf condition CountryValue <> "US"" orelse condition
    '		ElseIf CountryValue <> "US" Then
    '			TextBoxZip.MaxLength = 10
    '			TextBoxZip.Attributes.Add("onkeypress", "javascript:ValidateAlphaNumeric();")
    '			'rgExp.Enabled = True
    '			'rgExp.ValidationExpression = "^([a-zA-Z])([0-9])([a-zA-Z])([0-9])([a-zA-Z])([0-9])|([a-zA-Z])([0-9])([a-zA-Z]) ([0-9])([a-zA-Z])([0-9])"
    '		Else
    '			TextBoxZip.MaxLength = 10
    '			TextBoxZip.Attributes.Add("onkeypress", "javascript:ValidateNumeric();")
    '		End If
    '	Catch
    '		Throw
    '	End Try
    'End Function
    Private Sub Address_DropDownListState()
        Dim dataset_States As New DataSet
        Dim datarow_state As DataRow()
        Dim strCountryCode As String = String.Empty
        Dim l_bool_FromWebAddr As Boolean
        Dim l_bool_maintainencescreen As Boolean

        Try

            m_strDropwonlist = ""
            m_strDropwonlist = Session("Sender")
            l_bool_FromWebAddr = Me.FromWebAddr

            'If DropDownListState.SelectedValue <> "" And DropDownListCountry.SelectedValue = "" Then
            If DropDownListState_hid.Value <> "" And DropDownListCountry_hid.Value = "" Then
                Me.TextBoxZip.Text = ""
            End If

            If l_bool_FromWebAddr = False Then
                If m_strDropwonlist = "AddressWebUserControl2:DropDownListCountry" Or m_strDropwonlist = "AddressWebUserControl2:DropDownListState" Then
                    Call SetValidationsForSecondary()
                    'Call SetAttributesForSecondary(Me.DropDownListCountry.SelectedValue)
                    'Call SetAttributesForSecondary(Me.DropDownListCountry_hid.Value)
                Else
                    l_bool_maintainencescreen = Me.IsMaintenanceScreen
                    If l_bool_maintainencescreen = False Then
                        'Call SetCountStZipCodeMandatoryOnSelection()
                    Else
                        Call SetValidationsForSecondary()
                    End If
                End If
            Else
                'Call SetCountStZipCodeMandatoryOnSelection()
            End If

        Catch
            Throw
        End Try
    End Sub

    'Public Sub SetCountStZipCodeMandatoryOnSelection()
    '	Try
    '		'If Me.DropDownListCountry.SelectedValue = "US" Or Me.DropDownListCountry.SelectedValue = "CA" Then
    '		If Me.DropDownListCountry_hid.Value = "US" Or Me.DropDownListCountry_hid.Value = "CA" Then
    '			Me.rqCountry.Enabled = True
    '			Me.rqCountry.IsValid = True
    '			Me.rqState.Enabled = True
    '			Me.rqState.IsValid = True
    '		Else
    '			'If Me.DropDownListCountry.SelectedValue <> "" Then
    '			If Me.DropDownListCountry_hid.Value <> "" Then
    '				Me.rqState.Enabled = False
    '				Me.rqState.IsValid = False
    '				'ElseIf Me.DropDownListState.SelectedValue <> "" Then
    '			ElseIf Me.DropDownListState_hid.Value <> "" Then
    '				Me.rqCountry.Enabled = False
    '				Me.rqCountry.IsValid = False
    '				'ElseIf Me.DropDownListCountry.SelectedValue = "" And Me.DropDownListState.SelectedValue = "" Then
    '			ElseIf Me.DropDownListCountry_hid.Value = "" And Me.DropDownListState_hid.Value = "" Then
    '				Me.rqCountry.Enabled = True
    '				Me.rqCountry.IsValid = True
    '				'30-aug-2010
    '				'If DropDownListState.Items.Count > 1 Then
    '				If DropDownListState_hid.Value = "" Then
    '					Me.rqState.Enabled = True
    '					Me.rqState.IsValid = True
    '				Else
    '					Me.rqState.Enabled = False
    '					Me.rqState.IsValid = False
    '				End If

    '			End If
    '		End If
    '	Catch
    '		Throw
    '	End Try
    'End Sub
    Public Sub SetValidationsForSecondary()
        Try
            'IB as on 10/June/2010: BT.534:showing validation messages while changing address.
            Me.reqAddress.Enabled = False
            ' Me.reqAddress.IsValid = False
            Me.rqCity.Enabled = False
            'Me.rqCity.IsValid = False
            ''Me.rqState.Enabled = False
            ' Me.rqState.IsValid = False
            ''Me.rqCountry.Enabled = False
            ' Me.rqCountry.IsValid = False
        Catch
            Throw
        End Try
    End Sub
    Public Sub SetValidationsForPrimary()
        Try

            'Ashutosh Patil as on 25-Apr-2007
            'YREN-3298
            'IB as on 10/June/2010: BT.534:showing validation messages while changing address.
            Me.reqAddress.Enabled = True
            ' Me.reqAddress.IsValid = True
            Me.rqCity.Enabled = True

            'Dim l_string_message As String = String.Empty
            'l_string_message = ValidateAddress()
            'If l_string_message <> String.Empty Then
            '	MessageBox.Show(PlaceHolder1, "YMCA-YRS", l_string_message, MessageBoxButtons.Stop)
            'End If


            'Me.rqCity.IsValid = True

            ''Me.rqState.Enabled = True
            '  Me.rqState.IsValid = True
            ''Me.rqCountry.Enabled = True
            '  Me.rqCountry.IsValid = True

            'Me.rqFieldZip.Enabled = True
            'Me.rqFieldZip.IsValid = True
        Catch ex As Exception
            Throw
        End Try
    End Sub
    Private Sub ShowDataForWebAddress()
        Dim l_DataRow As DataRow
        Dim l_str_zipcode As String = String.Empty

        'by aparna -not initialized -14/05/2007
        Dim l_str_country As String = String.Empty
        'by aparna -not initialized -14/05/2007
        Try
            l_DataRow = CType(Session("Datarow"), DataRow)

            If (Not l_DataRow Is Nothing) Then

                Me.TextBoxAddress1.Text = CType(l_DataRow("Address"), String).Trim

                If l_DataRow("Address 2").ToString = "" Then
                    Me.TextBoxAddress2.Text = String.Empty
                Else
                    Me.TextBoxAddress2.Text = CType(l_DataRow("Address 2"), String).Trim
                End If

                If l_DataRow("Address 3").ToString = "" Then
                    Me.TextBoxAddress3.Text = String.Empty
                Else
                    Me.TextBoxAddress3.Text = CType(l_DataRow("Address 3"), String).Trim
                End If

                If l_DataRow("City").ToString = "" Then
                    Me.TextBoxCity.Text = String.Empty
                Else
                    Me.TextBoxCity.Text = CType(l_DataRow("City"), String).Trim
                End If

                If l_DataRow("Country").ToString = "" Then
                    Me.DropDownListCountry_hid.Value = ""
                Else
                    'Me.DropDownListCountry.SelectedValue = CType(l_DataRow("Country"), String).Trim
                    Me.DropDownListCountry_hid.Value = CType(l_DataRow("Country"), String).Trim
                    l_str_country = CType(l_DataRow("Country"), String).Trim
                End If

                'Call SetCountryStateDropwdowns()

                If l_DataRow("State").ToString = "" Then
                    Me.DropDownListState_hid.Value = ""
                Else
                    'Me.DropDownListState.SelectedValue = CType(l_DataRow("State"), String).Trim
                    Me.DropDownListState_hid.Value = CType(l_DataRow("State"), String).Trim
                End If

                If l_str_country.ToString = "CA" Or l_str_country.ToString = "CANADA" Then
                    l_str_zipcode = CType(l_DataRow("Zip"), String).Trim
                    Me.TextBoxZip.Text = l_str_zipcode.Substring(0, 3) + " " + l_str_zipcode.Substring(3, 3)
                Else
                    If l_DataRow("Zip").ToString = "" Then
                        Me.TextBoxZip.Text = String.Empty
                    Else
                        Me.TextBoxZip.Text = CType(l_DataRow("Zip"), String).Trim
                    End If
                End If

                'Call SetCountStZipCodeMandatoryOnSelection()

            End If
        Catch ex As Exception
            Throw
        End Try
    End Sub
    Public Sub ShowDataForAddress()
        Dim l_DataRow As DataRow
        Dim l_str_zipcode As String
        Dim l_str_country As String
        Try
            l_DataRow = DirectCast(Session("Datarow"), DataRow)

            If (Not l_DataRow Is Nothing) Then

                Me.TextBoxAddress1.Text = CType(l_DataRow("chvaddr1"), String).Trim

                If l_DataRow("chvaddr2").ToString = "" Then
                    Me.TextBoxAddress2.Text = String.Empty
                Else
                    Me.TextBoxAddress2.Text = CType(l_DataRow("chvaddr2"), String).Trim
                End If

                If l_DataRow("chvaddr3").ToString = "" Then
                    Me.TextBoxAddress3.Text = String.Empty
                Else
                    Me.TextBoxAddress3.Text = CType(l_DataRow("chvaddr3"), String).Trim
                End If

                If l_DataRow("chvcity").ToString = "" Then
                    Me.TextBoxCity.Text = String.Empty
                Else
                    Me.TextBoxCity.Text = CType(l_DataRow("chvcity"), String).Trim
                End If

                If l_DataRow("chvcountry").ToString = "" Then
                    'Me.DropDownListCountry.SelectedIndex = 0
                    Me.DropDownListCountry_hid.Value = ""
                Else
                    'Me.DropDownListCountry.SelectedValue = CType(l_DataRow("chvcountry"), String).Trim
                    Me.DropDownListCountry_hid.Value = CType(l_DataRow("chvcountry"), String).Trim
                End If
                'l_str_country = Me.DropDownListCountry.SelectedValue.ToString
                l_str_country = Me.DropDownListCountry_hid.Value.ToString

                'Call SetCountryStateDropwdowns()

                If l_DataRow("chrstatetype").ToString = "" Then
                    'Me.DropDownListState.SelectedIndex = 0
                    Me.DropDownListState_hid.Value = ""
                Else
                    'Me.DropDownListState.SelectedValue = CType(l_DataRow("chrstatetype"), String).Trim
                    Me.DropDownListState_hid.Value = CType(l_DataRow("chrstatetype"), String).Trim
                End If

                If l_str_country.ToString = "CA" Or l_str_country.ToString = "CANADA" Then
                    l_str_zipcode = CType(l_DataRow("chrzip"), String).Trim
                    Me.TextBoxZip.Text = l_str_zipcode.Substring(0, 3) + " " + l_str_zipcode.Substring(3, 3)
                Else
                    If l_DataRow("chrzip").ToString = "" Then
                        Me.TextBoxZip.Text = String.Empty
                    Else
                        Me.TextBoxZip.Text = CType(l_DataRow("chrzip"), String).Trim
                    End If
                End If

                'Call SetCountStZipCodeMandatoryOnSelection()

            End If
        Catch ex As Exception
            Throw
        End Try
    End Sub
    'BS:2012.4.18::YRS 5.0-1470:BT:951:address detail is updating from user control
    Public Function UpdateAddressDetail(ByVal l_ds_Address As DataSet, ByVal bitPrimary As Boolean, ByVal bitActive As Boolean)
        Dim l_str_Country As String
        Dim l_string_message As String = String.Empty
        Dim dsNotes As New DataSet
        Dim dtNotes As New DataTable

        If Not l_ds_Address Is Nothing Then
            If l_ds_Address.Tables("AddressInfo").Rows.Count > 0 Then

                If l_ds_Address.Tables("AddressInfo").Rows(0).Item("Address1").ToString.Trim() <> Address1 Then
                    l_ds_Address.Tables("AddressInfo").Rows(0).Item("Address1") = Address1
                End If
                If l_ds_Address.Tables("AddressInfo").Rows(0).Item("Address2").ToString.Trim() <> Address2 Then
                    l_ds_Address.Tables("AddressInfo").Rows(0).Item("Address2") = Address2
                End If
                If l_ds_Address.Tables("AddressInfo").Rows(0).Item("Address3").ToString.Trim <> Address3 Then
                    l_ds_Address.Tables("AddressInfo").Rows(0).Item("Address3") = Address3
                End If
                If l_ds_Address.Tables("AddressInfo").Rows(0).Item("City").ToString.Trim <> City Then
                    l_ds_Address.Tables("AddressInfo").Rows(0).Item("City") = City
                End If
                ' Added space between the quotation by Anudeep for Bt-1245:Address issue replication and analysis on client machine on 08-10-2012
                If DropDownListCountryValue = "-Select Country-" Then
                    DropDownListCountryValue = " "
                End If
                If l_ds_Address.Tables("AddressInfo").Rows(0).Item("Country").ToString.Trim <> DropDownListCountryValue Then
                    l_ds_Address.Tables("AddressInfo").Rows(0).Item("Country") = DropDownListCountryValue
                End If
                ' Added space between the quotation by Anudeep for Bt-1245:Address issue replication and analysis on client machine on 08-10-2012
                If DropDownListStateValue = "-Select State-" Then
                    DropDownListStateValue = " "
                End If
                If l_ds_Address.Tables("AddressInfo").Rows(0).Item("State").ToString.Trim <> DropDownListStateValue Then
                    l_ds_Address.Tables("AddressInfo").Rows(0).Item("State") = DropDownListStateValue
                End If
                Dim l_zipCode As String = IIf(DropDownListCountryValue.ToString = "CA", Replace(ZipCode, " ", ""), ZipCode)
                If l_ds_Address.Tables("AddressInfo").Rows(0).Item("Zip").ToString.Trim <> l_zipCode Then
                    l_str_Country = String.Empty
                    l_str_Country = DropDownListCountryValue
                    If l_str_Country.ToString = "CA" Then
                        l_ds_Address.Tables("AddressInfo").Rows(0).Item("Zip") = l_zipCode
                    Else
                        l_ds_Address.Tables("AddressInfo").Rows(0).Item("Zip") = l_zipCode
                    End If
                End If

                'bitPrimary,bitActive
                If Convert.ToBoolean(l_ds_Address.Tables("AddressInfo").Rows(0).Item("bitPrimary")) <> bitPrimary Then
                    l_ds_Address.Tables("AddressInfo").Rows(0).Item("bitPrimary") = bitPrimary
                End If
                If Convert.ToBoolean(l_ds_Address.Tables("AddressInfo").Rows(0).Item("bitActive")) <> bitActive Then
                    l_ds_Address.Tables("AddressInfo").Rows(0).Item("bitActive") = bitActive
                End If
                'effective date
                If l_ds_Address.Tables("AddressInfo").Rows(0).Item("EffDate").ToString.Trim <> EffectiveDate Then
                    l_ds_Address.Tables("AddressInfo").Rows(0).Item("EffDate") = EffectiveDate
                End If

                'CheckboxbadAddress
                If Not IsDBNull(l_ds_Address.Tables("AddressInfo").Rows(0).Item("bitBadAddress")) Then
                    If Convert.ToBoolean(l_ds_Address.Tables("AddressInfo").Rows(0).Item("bitBadAddress")) <> IsBadAddress Then
                        If IsBadAddress = True Then
                            l_ds_Address.Tables("AddressInfo").Rows(0).Item("bitBadAddress") = True
                        Else
                            l_ds_Address.Tables("AddressInfo").Rows(0).Item("bitBadAddress") = False
                        End If
                    End If
                Else
                    l_ds_Address.Tables("AddressInfo").Rows(0).Item("bitBadAddress") = False
                End If
                AddNotes(dsNotes, dtNotes)
            Else
                'if new record then
                If Address1 <> "" Or Address2 <> "" Or Address3 <> "" Or City <> "" Or ZipCode <> "" Then
                    Dim l_AddressRow As DataRow
                    l_AddressRow = l_ds_Address.Tables("AddressInfo").NewRow
                    ' Added space between the quotation by Anudeep for Bt-1245:Address issue replication and analysis on client machine on 08-10-2012
                    If DropDownListCountryValue = "-Select Country-" Then
                        DropDownListCountryValue = " "
                    End If
                    ' Added space between the quotation by Anudeep for Bt-1245:Address issue replication and analysis on client machine on 08-10-2012
                    If DropDownListStateValue = "-Select State-" Then
                        DropDownListStateValue = " "
                    End If
                    l_AddressRow.Item("EntityId") = Session("PersId")
                    l_AddressRow.Item("Address1") = Address1
                    l_AddressRow.Item("Address2") = Address2
                    l_AddressRow.Item("Address3") = Address3
                    l_AddressRow.Item("City") = City
                    l_AddressRow.Item("Country") = DropDownListCountryValue
                    l_AddressRow.Item("State") = DropDownListStateValue
                    l_str_Country = String.Empty
                    l_str_Country = DropDownListCountryValue
                    If l_str_Country.ToString = "CA" Then
                        l_AddressRow.Item("Zip") = Replace(ZipCode, " ", "")
                    Else
                        l_AddressRow.Item("Zip") = ZipCode
                    End If
                    l_AddressRow.Item("bitPrimary") = bitPrimary
                    l_AddressRow.Item("bitActive") = bitActive
                    If EffectiveDate <> String.Empty Then
                        l_AddressRow.Item("EffDate") = EffectiveDate

                    Else
                        l_AddressRow.Item("EffDate") = System.DateTime.Now()
                    End If

                    If IsBadAddress = True Then
                        l_AddressRow.Item("bitBadAddress") = True
                        'IsBadAddress_hid.Value = "True"
                    Else
                        l_AddressRow.Item("bitBadAddress") = False
                        'IsBadAddress_hid.Value = "False"
                    End If
                    l_ds_Address.Tables("AddressInfo").Rows.Add(l_AddressRow)
                    AddNotes(dsNotes, dtNotes)
                End If

            End If
        End If
        'Add AddressInfo in DB
        If Not l_ds_Address.Tables("AddressInfo") Is Nothing Then
            If l_ds_Address.Tables("AddressInfo").Rows.Count > 0 Then
                If Not l_ds_Address.Tables("AddressInfo").GetChanges Is Nothing Then
                    YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.UpdateParticipantAddress(l_ds_Address)
                End If
            End If
        End If
        'Add NotesInfo in DB
        If Not dtNotes Is Nothing Then
            If Not dtNotes.GetChanges Is Nothing Then
                If dtNotes.GetChanges.Rows.Count > 0 Then
                    YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.InsertParticipantNotes(dsNotes)
                    dsNotes.AcceptChanges()
                    Session("Flag") = "AddNotesByAddressVerify"
                End If
            End If
        End If
    End Function
    Public Sub AddNotes(ByRef dsNotes As DataSet, ByRef dtNotes As DataTable)
        Dim dr As DataRow
        If Notes <> String.Empty Then
            dtNotes.TableName = "Member Notes"
            dtNotes.Columns.Add("UniqueID")
            dtNotes.Columns("UniqueID").DataType = System.Type.GetType("System.Guid")
            dtNotes.Columns.Add("PersonID")
            dtNotes.Columns("PersonID").DataType = System.Type.GetType("System.Guid")
            dtNotes.Columns.Add("Note")
            dtNotes.Columns.Add("Creator")
            dtNotes.Columns.Add("Date")
            dtNotes.Columns.Add("bitImportant")


            dr = dtNotes.NewRow
            dr("UniqueID") = Guid.NewGuid()
            dr("Note") = Notes
            dr("PersonID") = Session("PersId")
            dr("Date") = Date.Now
            dr("bitImportant") = chkNotes.Value
            dr("Creator") = Session("LoginId")
            dtNotes.Rows.Add(dr)
            dsNotes.Tables.Add(dtNotes)
        End If
    End Sub
    Public Sub RefreshAddressDetail(ByVal dsAddress As DataSet)
        If dsAddress IsNot Nothing Then
            'Address
            If HelperFunctions.isNonEmpty(dsAddress.Tables("AddressInfo")) Then
                With dsAddress.Tables("AddressInfo").Rows(0)
                    Address1 = .Item("Address1").ToString().Trim
                    Address2 = .Item("Address2").ToString().Trim
                    Address3 = .Item("Address3").ToString().Trim
                    City = .Item("City").ToString().Trim
                    DropDownListStateValue = .Item("State").ToString().Trim
                    DropDownListCountryValue = .Item("Country").ToString().Trim
                    ZipCode = .Item("Zip").ToString().Trim
                    EffectiveDate = .Item("EffDate").ToString().Trim
                    IsBadAddress = .Item("bitBadAddress").ToString().Trim
                End With
            End If
        End If
    End Sub

    'Public Function SetValidationsForAddress() As String
    '	Try
    '		Call SetCountStZipCodeMandatoryOnSelection()
    '	Catch ex As Exception
    '		Throw
    '	End Try
    'End Function
#End Region 'Cusotm Methods 

End Class

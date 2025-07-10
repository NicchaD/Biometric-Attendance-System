'*******************************************************************************
' Copyright YMCA Retirement Fund All Rights Reserved. 
'
' Project Name		:	YMCA-YRS
' FileName			:	AddressUserControlNew.ascx.vb

'*******************************************************************************

'Modification History
'****************************************************************************************************************************************************
'Modified By        Date            Description
'****************************************************************************************************************************************************
'Anudeep            10-Jul-2013     BT-1501:YRS 5.0-1745:Capture Beneficiary addresses 
'Anudeep            22-aug-2013     YRS 5.0-1862:Add notes record when user enters address in any module.
'Anudeep            23-sep-2013     BT-1501:YRS 5.0-1745:Capture Beneficiary addresses(re-opened)
'Anudeep            25-sep-2013     Bt-2042:YRS 5.0-2092: Zip code saving
'Anudeep            01-oct-2013     Bt-2042:YRS 5.0-2092: Zip code saving
'Anudeep            04-oct-2013     BT-2236:After modifying address save button gets disabled
'Anudeep            13-nov-2013     BT-2297:Add option to get Canada state List & US state list in "GetMasterDetails " web method in Beneficiary Web service
'Anudeep            16-dec-2013     BT:2239:YRS 5.0-2224:Changes to the address updater
'shashank			26-dec-2013		BT-2340-'2013.12.26 SP BT-2340 - Made visible true to show information like click here to add address
'Manthan Rajguru    2015.09.16      YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
'Manthan Rajguru    2015.10.21      YRS-AT-2182: limit effective date for address updates -Do not allow address updates with an effective date in the future. 
'Manthan Rajguru    2018.11.23      YRS-AT-4018 -  YRS enh: EFT Loans Project: “Update” YRS Maintenance: Person: Loan Tab
'****************************************************************************************************************************************************
Public Class AddressUserControlNew
    Inherits System.Web.UI.UserControl


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
            If imgBadAddress.Style("display") = "block" Then
                Return True
            ElseIf imgBadAddress.Style("display") = "none" Then
                Return False
            End If
        End Get
        Set(ByVal Value As Boolean)
            Me.CheckboxIsBadAddress.Checked = Value
            If Value = False Then
                Me.imgBadAddress.Style.Add("display", "none")
            ElseIf Value = True Then
                Me.imgBadAddress.Style.Add("display", "block")
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
    Public Property ZipCode() As String
        Get
            If Not Me.TextBoxZip.Text Is String.Empty Then
                'Anudeep:22.08.2013:Bt-1683-YRS 5.0-1862:Add notes record when user enters address in any module.
                'AA:15.11.2013 - BT:2297 - Commented below Code to return zip code without spaces in between
                Return Me.TextBoxZip.Text.ToString.Replace(",", "").Trim().Replace(" ", "")
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
                'Anudeep:22.08.2013:Bt-1683-YRS 5.0-1862:Add notes record when user enters address in any module.
                Return Me.TextBoxAddress1.Text.ToString.Replace(",", "").Trim()
            Else
                Return String.Empty
            End If
        End Get
        Set(ByVal Value As String)
            Me.TextBoxAddress1.Text = Value
            'AA:17.12.2013 BT:2239 Checking if uniqueid is empty then add else edit mode
            If String.IsNullOrEmpty(UniqueId) Then
                Me.Addressmode = "Add"
            Else
                Me.Addressmode = "Edit"
            End If
        End Set
    End Property
    Public Property Address2() As String
        Get
            If Not Me.TextBoxAddress2.Text Is String.Empty Then
                'Anudeep:22.08.2013:Bt-1683-YRS 5.0-1862:Add notes record when user enters address in any module.
                Return Me.TextBoxAddress2.Text.ToString.Replace(",", "").Trim()
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
                'Anudeep:22.08.2013:Bt-1683-YRS 5.0-1862:Add notes record when user enters address in any module.
                Return Me.TextBoxAddress3.Text.ToString.Replace(",", "").Trim()
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
                'Anudeep:22.08.2013:Bt-1683-YRS 5.0-1862:Add notes record when user enters address in any module.
                Return Me.TextBoxCity.Text.ToString.Replace(",", "").Trim()
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
            If DropDownListState_hid.Value = "-Select-" Then
                Return ""
            End If
            If Not Me.DropDownListState_hid.Value Is String.Empty Then
                'Anudeep:22.08.2013:Bt-1683-YRS 5.0-1862:Add notes record when user enters address in any module.
                Return getSelectedStateText(Me.DropDownListState_hid.Value).Replace(",", "").Trim()
            Else
                Return String.Empty
            End If
        End Get
    End Property
    Public Property DropDownListStateValue() As String
        Get
            If DropDownListState_hid.Value = "-Select-" Then
                Return ""
            End If
            If Not Me.DropDownListState_hid.Value Is String.Empty Then
                'Anudeep:22.08.2013:Bt-1683-YRS 5.0-1862:Add notes record when user enters address in any module.
                Return Me.DropDownListState_hid.Value.ToString.Trim.Replace(",", "").Trim()
            Else
                Return String.Empty
            End If
        End Get
        Set(ByVal Value As String)
            Me.DropDownListState_hid.Value = Value
        End Set
    End Property
    Public Property DropDownListCountryValue() As String
        Get
            If DropDownListCountry_hid.Value = "-Select-" Then
                Return ""
            End If
            If Not Me.DropDownListCountry_hid.Value Is String.Empty Then
                'Anudeep:22.08.2013:Bt-1683-YRS 5.0-1862:Add notes record when user enters address in any module.
                Return Me.DropDownListCountry_hid.Value.ToString.Trim.Replace(",", "").Trim()
            Else
                Return String.Empty
            End If
        End Get
        Set(ByVal Value As String)
            Me.DropDownListCountry_hid.Value = Value
        End Set
    End Property
    Public ReadOnly Property DropDownListCountryText() As String
        Get
            If DropDownListCountry_hid.Value = "-Select-" Then
                Return ""
            End If
            If Not Me.DropDownListCountry_hid.Value Is String.Empty Then
                'Anudeep:22.08.2013:Bt-1683-YRS 5.0-1862:Add notes record when user enters address in any module.
                Return getSelectedCountryText(Me.DropDownListCountry_hid.Value).Replace(",", "").Trim()
            Else
                Return String.Empty
            End If
        End Get
    End Property
    Public Property guiEntityId() As String
        Get
            If Not ViewState(ClientID + "_EntityId") Is String.Empty Then
                Return ViewState(ClientID + "_EntityId")
            Else
                Return String.Empty
            End If
        End Get
        Set(ByVal Value As String)
            ViewState(ClientID + "_EntityId") = Value
        End Set
    End Property
    Public Property IsFromBenificarySettlement() As Boolean
        Get
            Return m_bool_IsfromBeniFiciarySettlement
        End Get
        Set(ByVal Value As Boolean)
            m_bool_IsfromBeniFiciarySettlement = Value
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
    Public Property PopupHeight() As Integer
        Get
            Return hdnpopupheight.Value
        End Get
        Set(ByVal Value As Integer)
            hdnpopupheight.Value = Value
        End Set
    End Property
    Public Property EnableControls() As Boolean
        Get
            Return Me.TextBoxAddress1.Enabled = True
        End Get
        Set(ByVal Value As Boolean)
            Me.TextBoxAddress1.Enabled = Value
            Me.TextBoxAddress2.Enabled = Value
            Me.TextBoxAddress3.Enabled = Value
            Me.TextBoxCity.Enabled = Value
            Me.TextBoxZip.Enabled = Value
            Me.TextBoxEffDate.Enabled = Value
            Me.DropDownListCountry.Disabled = Not Value
            Me.DropDownListState.Disabled = Not Value
            txtFinalNotes.Value = String.Empty
            chkNotes.Value = String.Empty
            CheckboxIsBadAddress.Enabled = Value
            If Not Value Then
                LabelUpdateAddress.Style("display") = "none"
            ElseIf Address1 = "" Then
                LabelUpdateAddress.Style("display") = "block"
            End If
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
            Return Me.TextBoxAddress1.Text
        End Get
        Set(ByVal Value As String)
            Me.TextBoxAddress1.Text = Value
            Me.TextBoxAddress2.Text = Value
            Me.TextBoxAddress3.Text = Value
            Me.TextBoxCity.Text = Value
            Me.TextBoxZip.Text = Value
            Me.TextBoxEffDate.Text = Value
            Me.DropDownListState_hid.Value = Value
            Me.DropDownListCountry_hid.Value = Value
            Me.IsBadAddress = False
            'AA:17.12.2013 BT:2239 setting into Add mode
            If Value Is String.Empty Then
				LabelNoAddressFound.Visible = True
				'2013.12.26 SP BT-2340 - Made visible true to show information like click here to add address
				LabelNoAddressFound.Style("display") = "block"
				If EnableControls Then
					LabelUpdateAddress.Visible = True
					LabelUpdateAddress.Style("display") = "block"
				End If
				'2013.12.26 SP BT-2340 - Made visible true to show information like click here to add address
                Addressmode = "Add"
            End If
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
    Public Property Rights() As String
        Get

            Return hdnRights.Value

        End Get
        Set(ByVal Value As String)
            hdnRights.Value = Value
        End Set
    End Property
    Public Property EntityCode As String
        Get
            If Not ViewState(ClientID + "_EntityCode") Is Nothing Then
                Return ViewState(ClientID + "_EntityCode")
            Else
                Return String.Empty
            End If
        End Get
        Set(ByVal value As String)
            ViewState(ClientID + "_EntityCode") = value
        End Set
    End Property
    Public Property AddrCode As String
        Get
            If Not ViewState(ClientID + "_AddrCode") Is Nothing Then
                Return ViewState(ClientID + "_AddrCode")
            Else
                Return String.Empty
            End If
        End Get
        Set(ByVal value As String)
            ViewState(ClientID + "_AddrCode") = value
        End Set
    End Property
    Public Property UniqueId As String
        Get
            If Not ViewState(ClientID + "_UniqueId") Is Nothing Then
                Return ViewState(ClientID + "_UniqueId")
            Else
                Return String.Empty
            End If
        End Get
        Set(ByVal value As String)
            ViewState(ClientID + "_UniqueId") = value
        End Set
    End Property
    Public Property ChangesExist() As String
        Get

            Return hdnChangesExist.Value

        End Get
        Set(ByVal Value As String)
            hdnChangesExist.Value = Value
        End Set
    End Property
    Public Property AddressFor() As String
        Get
            If Not ViewState(ClientID + "_AddressFor") Is Nothing Then
                Return ViewState(ClientID + "_AddressFor")
            Else
                Return String.Empty
            End If
        End Get
        Set(ByVal value As String)
            ViewState(ClientID + "_AddressFor") = value
        End Set
    End Property
    'AA:17.12.2013 BT:2239 creating Add mode
    Public Property Addressmode() As String
        Get
            Return hdnAddressmode.Value
        End Get
        Set(ByVal value As String)
            hdnAddressmode.Value = value
        End Set
    End Property
    'START: MMR | 2018.11.23 | YRS-AT-4018 | Added property to store information indicating address can be updated/Added
    Public Property IsPIIInformationAllowedToChange() As Boolean
        Get
            If Not (ViewState("IsPIIInformationAllowedToChange")) Is Nothing Then
                Return (CType(ViewState("IsPIIInformationAllowedToChange"), Boolean))
            Else
                Return True
            End If
        End Get
        Set(ByVal Value As Boolean)
            ViewState("IsPIIInformationAllowedToChange") = Value
        End Set
    End Property
    'END: MMR | 2018.11.23 | YRS-AT-4018 | Added property to store information indicating address can be updated/Added
    'START: MMR| 2011.11.23 | YRS-AT-4018 | Added property to store participant PII Information updation allowed status.
    Public Property PIIInformationRestrictionMessageCodeAddressControl As Integer
        Get
            If Not (ViewState("PIIInformationRestrictionMessageCodeAddressControl")) Is Nothing Then
                Return (CType(ViewState("PIIInformationRestrictionMessageCodeAddressControl"), Integer))
            Else
                Return False
            End If
        End Get
        Set(ByVal Value As Integer)
            ViewState("PIIInformationRestrictionMessageCodeAddressControl") = Value
        End Set
    End Property

    Public Property PIIInformationRestrictionMessageTextAddressControl As String
        Get
            If Not (ViewState("PIIInformationRestrictionMessageTextAddressControl")) Is Nothing Then
                Return (CType(ViewState("PIIInformationRestrictionMessageTextAddressControl"), String))
            Else
                Return String.Empty
            End If
        End Get
        Set(ByVal Value As String)
            ViewState("PIIInformationRestrictionMessageTextAddressControl") = Value
        End Set
    End Property

    Public Property DivPIIErrorControlID As String
        Get
            If Not (ViewState("DivPIIErrorControlID")) Is Nothing Then
                Return (CType(ViewState("DivPIIErrorControlID"), String))
            Else
                Return String.Empty
            End If
        End Get
        Set(ByVal Value As String)
            ViewState("DivPIIErrorControlID") = Value
        End Set
    End Property
    'END: MMR| 2011.11.23 | YRS-AT-4018 | Added property to store participant PII Information updation allowed status.
#End Region     '" Properties "
#Region " Events"
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        Dim l_intPri As Integer
        Dim l_bool_BenificaryGridvalue As Boolean
        Try
            If Not Me.IsPostBack Then
                Me.TextBoxCity.Attributes.Add("OnKeyPress", "javascript:ValidateAlphaNumeric();")
                Me.TextBoxCity.Attributes.Add("OnKeyUp", "javascript:fncKeyCityStop();")
                Me.TextBoxCity.Attributes.Add("OnKeyDown", "javascript:fncKeyCityStop();")
                Me.TextBoxZip.Attributes.Add("OnKeyUp", "javascript:fncKeyZipStop();")
                Me.TextBoxZip.Attributes.Add("OnKeyDown", "javascript:fncKeyZipStop();")
                TextBoxZip.Attributes.Add("onkeypress", "javascript:ValidateZipCodeKeyPress(this,'" & DropDownListCountry.ClientID & "');")
                TextBoxZip.Attributes.Add("onblur", "javascript:validateZipCode(this,'" & DropDownListCountry.ClientID & "');")
                'AA:17.12.2013 BT:2239 Initially it addressmode will be set as add mode
                If Addressmode = "" Then
                    Addressmode = "Add"
                End If
            End If
            'Anudeep:01.07.2013 BT-1501:YRS 5.0-1745:Capture Beneficiary addresses 
            If Visible Then
                LoadCountryState()
            End If
            If TextBoxAddress1.Text <> "" Then
                LabelEffDate.Style("display") = "block"
                LabelNoAddressFound.Style("display") = "none"
                LabelUpdateAddress.Style("display") = "none"
            Else
                LabelEffDate.Style("display") = "none"
                LabelNoAddressFound.Style("display") = "block"
                If EnableControls Then
                    LabelUpdateAddress.Style("display") = "block"
                End If
            End If
            IsBadAddress = CheckboxIsBadAddress.Checked

        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim().ToString()), False)
        End Try
    End Sub
#End Region '" Events "
#Region " Custom Methods "

    Private Function SanitizeValue(ByVal s As String) As String
        Return s.Replace("'", "")
    End Function

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
        Dim dsSates As New DataSet()
        Try
            dsCountries = Address.GetCountryList()
            dsSates = Address.GetState()
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
            'AA:2013.09.23:BT-1501:Handling the getting reason from address class and getting reasons for beneficiary address change
            Dim dsNotesReason As New DataSet()
            'dsNotesReason = YMCARET.YmcaBusinessObject.YMCAAddressBOClass.GetAddressNotesReasonSource("AddressNotesReason")
            dsNotesReason = Address.GetAddressNotesReasonSource("AddressNotesReason", AddressFor)
            If HelperFunctions.isEmpty(dsNotesReason) Then
                Exit Function
            End If
            'Code to Write javascript array  frod dataset
            'javaScript.Append("var NotesReason = [" & vbCrLf)

            Dim k, l As Integer
            Dim arrNotes As String
            Dim dataRowReason As DataRow
            For k = 0 To dsNotesReason.Tables(0).Rows.Count - 1
                dataRowReason = dsNotesReason.Tables(0).Rows(k)
                arrNotes += dataRowReason(0) + "$"
                'AA:2013.09.23:BT-1501:Handling the getting reasons for beneficiary address change
                ''   javaScript.Append(" [ ")
                'For l = 0 To dataRowReason.Table.Columns.Count - 1
                '    javaScript.Append("'" + SanitizeValue(dataRowReason(l).ToString()) + "'")
                '    If ((l + 1) < dataRowReason.Table.Columns.Count) Then
                '        javaScript.Append(",")
                '    End If
                'Next
                ''  javaScript.Append(" ]")
                'If ((k + 1) < dsNotesReason.Tables(0).Rows.Count) Then
                '    javaScript.Append("," & vbCrLf)
                'End If
            Next
            arrNotes = arrNotes.Remove(arrNotes.Length - 1)
            hdnReasons_hid.Value = arrNotes
            'javaScript.Append(vbCrLf & "];" & vbCrLf)

            'BS:2012.05.30:BT:951:YRS:1470:-create array for notes reason and source
            Dim dsNotesSource As New DataSet()
            'dsNotesSource = YMCARET.YmcaBusinessObject.YMCAAddressBOClass.GetAddressNotesReasonSource("AddressNotesSource")
            dsNotesSource = Address.GetAddressNotesReasonSource("AddressNotesSource", AddressFor)
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
            If Not Me.Page.ClientScript.IsClientScriptBlockRegistered("ArrayScript") Then
                Me.Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "ArrayScript", javaScript.ToString(), True)
            End If
            javaScript.Clear()
            javaScript.Append("$(document).ready(function() { initializeAddressControl('" + DropDownListCountry.ClientID + "', '" + DropDownListState.ClientID + "', '" + DropDownListCountry_hid.ClientID + "', '" + DropDownListState_hid.ClientID + "','" + TextBoxZip.ClientID + "','" + TextBoxAddress1.ClientID + "','" + TextBoxAddress2.ClientID + "','" + TextBoxAddress3.ClientID + "','" + TextBoxCity.ClientID + "','" + CType(TextBoxEffDate.FindControl("TextBoxUCDate"), TextBox).ClientID + "');});")
            javaScript.Append("$(document).ready(function() { initializeAddressControl2('" + DropDownListCountry.ClientID + "', '" + DropDownListState.ClientID + "', '" + DropDownListCountry_hid.ClientID + "', '" + DropDownListState_hid.ClientID + "','" + TextBoxZip.ClientID + "','" + TextBoxAddress1.ClientID + "','" + TextBoxAddress2.ClientID + "','" + TextBoxAddress3.ClientID + "','" + TextBoxCity.ClientID + "','" + CType(TextBoxEffDate.FindControl("TextBoxUCDate"), TextBox).ClientID + "','" + CheckboxIsBadAddress.ClientID + "','" + txtFinalNotes.ClientID + "','" + chkNotes.ClientID + "');});")
            Me.Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), Me.ClientID, javaScript.ToString(), True)

        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function ValidateAddress() As String
        Dim str As String = IIf(IsPrimary <> "1", "Secondary", "Primary")
        Dim effDate As String = TextBoxEffDate.Text.ToString().Trim() 'Manthan Rajguru | 2015.10.21 | YRS-AT-2182 | Storing effective date in variable

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
            'Start - Manthan Rajguru | 2015.10.21 | YRS-AT-2182 | Validating two dates and providing message
            If (Not String.IsNullOrEmpty(effDate) AndAlso effDate > System.DateTime.Now) Then
                Return "Effective date of address cannot be in the future."
            End If
            'End - Manthan Rajguru | 2015.10.21 | YRS-AT-2182 | Validating two dates and providing message
            'Perform additional validations for US and CA.
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
                TextBoxZip.Text = TextBoxZip.Text.Replace(",", "").Replace(", ", "").Replace(", ", "").Replace("-", "")

                If DropDownListCountry_hid.Value = "US" And (System.Text.RegularExpressions.Regex.IsMatch(TextBoxZip.Text.Trim(), "(^[0-9]{5}$)|(^[0-9]{9}$)")) = False Then
                    Return "Invalid Zip Code format for United States"
                    'AA:15.11.2013 - BT:2297 - Added below Code to verify the zip code without spaces between
                ElseIf DropDownListCountry_hid.Value = "CA" And (System.Text.RegularExpressions.Regex.IsMatch(TextBoxZip.Text.Trim().Replace(" ", ""), "^([a-zA-Z])([0-9])([a-zA-Z])([0-9])([a-zA-Z])([0-9])$")) = False Then
                    Return "Invalid Zip Code format for Canada"
                End If

                TextBoxZip.Text = TextBoxZip.Text.Trim() + ","
            End If
        End If

        Return String.Empty

    End Function

    Public Function GetAddressTable() As DataTable

        Dim dtAddress As DataTable
        Dim drAddress As DataRow
        Dim strZip As String

        Try
            dtAddress = Address.CreateAddressDatatable()
            drAddress = dtAddress.NewRow()
            ''Added by Anudeep:10.07.2013
            If (Address1 <> "" And City <> "") And (ChangesExist = "true" Or IsFromBenificarySettlement = True) Then
                If DropDownListCountryValue = "- Select -" Then
                    DropDownListCountryValue = " "
                End If

                If DropDownListStateValue = "- Select -" Then
                    DropDownListStateValue = " "
                End If
                If Not UniqueId Is Nothing Then
                    drAddress.Item("UniqueId") = UniqueId
                Else
                    drAddress.Item("UniqueId") = ""
                End If

                drAddress.Item("guiEntityId") = guiEntityId
                drAddress.Item("addr1") = Address1.Replace(",", "").Trim()
                drAddress.Item("addr2") = Address2.Replace(",", "").Trim()
                drAddress.Item("addr3") = Address3.Replace(",", "").Trim()
                drAddress.Item("city") = City.Replace(",", "").Trim()
                drAddress.Item("country") = DropDownListCountryValue.Replace(",", "").Trim()
                drAddress.Item("StateName") = DropDownListStateText
                drAddress.Item("CountryName") = DropDownListCountryText
                drAddress.Item("state") = DropDownListStateValue.Replace(",", "").Trim()
                strZip = ZipCode.Replace("-", "")
                'AA:15.11.2013 - BT:2297 - Commented below Code to trim and replace empty spaces for canada zip code while saving in database.
                'If DropDownListCountryValue <> "CA" Then
                'strZip = strZip.Trim().Replace(" ", "")
                'End If
                drAddress.Item("zipCode") = strZip.Replace(",", "")
                drAddress.Item("isPrimary") = IIf(IsPrimary = "1", True, False)
                drAddress.Item("isActive") = True
                If EffectiveDate <> String.Empty Then
                    drAddress.Item("effectiveDate") = EffectiveDate
                Else
                    drAddress.Item("effectiveDate") = System.DateTime.Now()
                End If

                If IsBadAddress = True Then
                    drAddress.Item("isBadAddress") = True
                Else
                    drAddress.Item("isBadAddress") = False
                End If
                drAddress.Item("addrCode") = AddrCode.Trim()
                drAddress.Item("entityCode") = EntityCode.Trim()
                'Notes
                drAddress("Note") = Notes
                drAddress("bitImportant") = chkNotes.Value
                dtAddress.Rows.Add(drAddress)
            End If
            'Anudeep:04.10.2013: BT-2236:After modifying address save button gets disabled
            ChangesExist = ""
            If Not dtAddress Is Nothing Then
                Return dtAddress
            End If
        Catch Ex As Exception
            Throw
        End Try
    End Function

    Public Sub LoadAddressDetail(ByVal drAddress As DataRow())

        'Address
        'Anudeep:04.10.2013: BT-2236:After modifying address save button gets disabled
        ChangesExist = ""
        If HelperFunctions.isNonEmpty(drAddress) Then
            If drAddress.Length > 0 Then
                With drAddress(0)
                    UniqueId = .Item("UniqueId").ToString.Trim
                    guiEntityId = .Item("guiEntityId").ToString.Trim
                    Address1 = .Item("addr1").ToString().Replace(",", "").Trim
                    Address2 = .Item("addr2").ToString().Replace(",", "").Trim
                    Address3 = .Item("addr3").ToString().Replace(",", "").Trim
                    City = .Item("city").ToString().Replace(",", "").Trim
                    DropDownListStateValue = .Item("state").ToString().Replace(",", "").Trim
                    DropDownListCountryValue = .Item("country").ToString().Replace(",", "").Trim
                    'AA:15.11.2013 - BT:2297 - Added below Code to remove empty spaces exists in zip code
                    ZipCode = .Item("zipCode").ToString().Replace(",", "").Replace("-", "").Trim().Replace(" ", "")
                    EffectiveDate = .Item("effectiveDate").ToString().Replace(",", "").Trim
                    'Anudeep:22.08.2013:Bt-1683-YRS 5.0-1862:Add notes record when user enters address in any module.
                    IsBadAddress = IIf(.Item("isBadAddress").ToString().Trim = "", False, .Item("isBadAddress").ToString().Trim)
                    AddrCode = .Item("addrCode").ToString().Trim
                    EntityCode = .Item("entityCode").ToString().Trim
                    If Address1 <> "" Then
                        Address1 = Address1 + ","
                    End If
                    If Address2 <> "" Then
                        Address2 = Address2 + ","
                    End If
                    If Address3 <> "" Then
                        Address3 = Address3 + ","
                    End If
                    'AA:01.10.2013:Bt-2042:Changed to display comma after city if state value exists
                    If DropDownListStateValue <> "" Then
                        City = City + ","
                    End If
                    'AA:01.10.2013:Bt-2042:Commented un necessary code
                    'If DropDownListStateValue <> "" Then
                    '    DropDownListStateValue = DropDownListStateValue + ", "
                    'End If
                    If ZipCode <> "" Then
                        Dim strZipCode As String
                        'Anudeep:09.07.2013 -YRS 5.0-2092: Zip code saving  
                        If ZipCode.Length = 9 And DropDownListCountryValue = "US" Then
                            strZipCode = ZipCode.Substring(0, 5) + "-" + ZipCode.Substring(5, 4)
                            'AA:15.11.2013 - BT:2297 - Added below Code to add the space after the 3 characters while displaying for canada country zip code
                        ElseIf DropDownListCountryValue = "CA" Then
                            strZipCode = ZipCode.Substring(0, 3) + " " + ZipCode.Substring(3, 3)
                        Else
                            strZipCode = ZipCode
                        End If
                        ZipCode = ", " + strZipCode '+ "," Anudeep:25.09.2013 -YRS 5.0-2092: Zip code saving 
                    End If

                End With
                LabelAddress1.Style("display") = "block"
                LabelEffDate.Style("display") = "block"
                LabelNoAddressFound.Style("display") = "none"
                LabelUpdateAddress.Style("display") = "none"
            Else
                ClearControls = ""
                '    LabelAddress1.Style("display") = "none"
                LabelEffDate.Style("display") = "none"
                LabelNoAddressFound.Style("display") = "block"
                If EnableControls Then
                    LabelUpdateAddress.Style("display") = "block"
                End If
                IsBadAddress = False
            End If
        Else
            ClearControls = ""
            'LabelAddress1.Style("display") = "none"
            LabelEffDate.Style("display") = "none"
            LabelNoAddressFound.Style("display") = "block"
            If EnableControls Then
                LabelUpdateAddress.Style("display") = "block"
            End If
            IsBadAddress = False
        End If

    End Sub


#End Region 'Custom Methods 

End Class

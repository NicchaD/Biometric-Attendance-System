'Modified by Shubhrata Oct6th 2006 YREN 2711--on updating the address the bitactive and bitprimary 
'of earlier address was nt updated to 0,0
'Modified by Shubhrata Oct 30th 2006 YREN 2780--allow alpha numeric for CA zip code and numeric for US 
'zip code
'Modification History
'********************************************************************************************************************************
'Modified By        Date            Description
'********************************************************************************************************************************
'Ashutosh Patil     12-Mar-2007     YREN-3028,YREN-3029 
'Ashutosh Patil     09-Apr-2007     YREN-3028,YREN-3029 
'Ashutosh Patil     15-May-2007     Telephone no validation using both client side or and server side
'Neeraj Singh       12/Nov/2009     Added form name for security issue YRS 5.0-940 
'Bhavna Shrivastav  18-May-2012     YRS 5.0-1470: Link to Address Edit program from Person Maintenance 
'Anudeep            13.Apr.2013     BT-1555:YRS 5.0-1769:Length of phone numbers
'Anudeep            21.jun.2013     BT-1555:YRS 5.0-1769:Length of phone numbers
'Anudeep            10-Jul-2013     BT-1501:YRS 5.0-1745:Capture Beneficiary addresses 
'Anudeep            10-sep-2013     BT:2185:Address and telephone issue
'Anudeep            10-sep-2013     YRS 5.0-2168:address being re-inserted upon save of any update
'Manthan Rajguru    2015.09.16      YRS-AT-2550: YRS data cleanup: copyright comments and namespace/reference (Changed the Namespace/reference from 'Infotech' to 'YMCARET')
'Pramod P. Pokale   2015.10.13      YRS-AT-2588: implement some basic telephone number validation Rules
'********************************************************************************************************************************
Public Class UpdateAddressDetails
    Inherits System.Web.UI.Page
    'below line is Added by Neeraj for issue id YRS 5.0-940 
    Dim strFormName As String = New String("UpdateAddressDetails.aspx")
    'End issue id YRS 5.0-940
    Dim str_Address1 As String
    Dim str_Address2 As String
    Dim str_Address3 As String
    Dim str_ZipCode As String
    Dim str_City As String
    Dim str_StateSelectedValue As String
    Dim str_CountrySelectedValue As String
    Dim str_SelectedState As String
    Dim str_SelectedCountry As String
    Dim str_Country As String
    Dim str_State As String
    Dim int_IsPrimary As Integer
    Protected WithEvents PlaceHolder1 As System.Web.UI.WebControls.PlaceHolder
    Dim str_strPerssId As String
    Dim l_Str_Msg As String
    Dim m_str_UpdateAddr As Boolean
    Protected WithEvents regExpHTel As System.Web.UI.WebControls.RegularExpressionValidator
    Protected WithEvents regExpOTel As System.Web.UI.WebControls.RegularExpressionValidator
    Protected WithEvents regExpMobile As System.Web.UI.WebControls.RegularExpressionValidator
    Protected WithEvents regFax As System.Web.UI.WebControls.RegularExpressionValidator
    Dim m_str_useraccess As String
#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents Menu1 As skmMenu.Menu
    Protected WithEvents LabelHomeTelephone As System.Web.UI.WebControls.Label
    Protected WithEvents LabelMobile As System.Web.UI.WebControls.Label
    Protected WithEvents LabelFax As System.Web.UI.WebControls.Label
    Protected WithEvents LabelEffDate As System.Web.UI.WebControls.Label
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    Protected WithEvents ButtonCancel As System.Web.UI.WebControls.Button
    Protected WithEvents TextBoxPhoneWork As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxPhoneHome As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxPhoneMobile As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxFaxPhone As System.Web.UI.WebControls.TextBox
    Protected WithEvents ButtonUpdate As System.Web.UI.WebControls.Button
    Protected WithEvents DataGrid1 As System.Web.UI.WebControls.DataGrid
    Protected WithEvents LabelWorkTelephone As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxEffDate As DateUserControl
    'Protected WithEvents DropDownListCountry As System.Web.UI.WebControls.DropDownList
    'Protected WithEvents TextBoxZip As System.Web.UI.WebControls.TextBox
    'Protected WithEvents DropDownListState As System.Web.UI.WebControls.DropDownList
    'Protected WithEvents TextBoxCity As System.Web.UI.WebControls.TextBox
    'Protected WithEvents TextBoxAddress3 As System.Web.UI.WebControls.TextBox
    'Protected WithEvents TextBoxAddress2 As System.Web.UI.WebControls.TextBox
    'Protected WithEvents TextBoxAddress1 As System.Web.UI.WebControls.TextBox
    Protected WithEvents AddressWebUserControl1 As YMCAUI.AddressUserControlNew
    Protected WithEvents Headercontrol As YMCA_Header_WebUserControl
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
        'Put user code to initialize the page here
        ' Dim TextBoxEffDate1 As TextBoxEffDate
        Dim l_bool_readonly As Boolean
        If Session("LoggedUserKey") Is Nothing Then
            Response.Redirect("Login.aspx", False)
        End If
        Dim strPersonId As String
        'Anudeep:13.Jun.2013:BT-1501:YRS 5.0-1745:Capture Beneficiary addresses 
        'If Not Session("PersonID") = "" Then
        '    strPersonId = (CType(Session("PersonID"), String))
        '    AddressWebUserControl1.guiPerssId = (CType(Session("PersonID"), String))
        '    AddressWebUserControl1.IsPrimary = 1
        'End If
        

        Try
            If Not Page.IsPostBack Then
                Me.TextBoxPhoneHome.Attributes.Add("OnKeyPress", "javascript:ValidateNumeric();")
                Me.TextBoxPhoneMobile.Attributes.Add("OnKeyPress", "javascript:ValidateNumeric();")
                Me.TextBoxPhoneWork.Attributes.Add("OnKeyPress", "javascript:ValidateNumeric();")
                Me.TextBoxFaxPhone.Attributes.Add("OnKeyPress", "javascript:ValidateNumeric();")

                'Commented By Ashutosh Patil as on 15-May-2007
                'Phone no validations on Client side using Regular Expression and Server Side
                'Me.TextBoxPhoneHome.Attributes.Add("OnBlur", " javascript:ValidateTelephoneNo(this);")
                'Me.TextBoxPhoneMobile.Attributes.Add("OnBlur", "javascript:ValidateTelephoneNo(this);")
                'Me.TextBoxPhoneWork.Attributes.Add("OnBlur", "javascript:ValidateTelephoneNo(this);")
                'Me.TextBoxFaxPhone.Attributes.Add("OnBlur", "javascript:ValidateTelephoneNo(this);")
                'Anudeep:21.06.2013 -BT-1555:YRS 5.0-1769:Length of phone numbers
                Me.TextBoxPhoneHome.Attributes.Add("onclick", " javascript:ValidateTelephoneNo(this);")
                Me.TextBoxPhoneMobile.Attributes.Add("onclick", "javascript:ValidateTelephoneNo(this);")
                Me.TextBoxPhoneWork.Attributes.Add("onclick", "javascript:ValidateTelephoneNo(this);")
                Me.TextBoxFaxPhone.Attributes.Add("onclick", "javascript:ValidateTelephoneNo(this);")
                'Anudeep:14.06.2013 BT-1501:YRS 5.0-1745:Capture Beneficiary addresses 
                If Not Session("PersonID") = "" Then
                    strPersonId = (CType(Session("PersonID"), String))
                    Headercontrol.PageTitle = "Update Address And Contacts"
                    Headercontrol.guiPerssId = strPersonId

                End If
                Me.Address_LoadControls(strPersonId)
                ViewState("SelectedState") = Nothing


            End If
            'BS:2012.05.17:YRS 5.0-1470: here check if page request is yes then save data in database
            If Session("VerifiedAddress") = "VerifiedAddress" Then
                If Request.Form("Yes") = "Yes" Then
                    Session("VerifiedAddress") = ""
                    UpdateAddress()
                    Session("States") = Nothing

                    Dim msg As String

                    msg = msg + "<Script Language='JavaScript'>"

                    msg = msg + "window.opener.document.forms(0).submit();"

                    msg = msg + "self.close();"

                    msg = msg + "</Script>"
                    Response.Write(msg)
                    Exit Sub
                ElseIf Request.Form("No") = "No" Then
                    Session("VerifiedAddress") = ""
                    Exit Sub
                End If
            End If
            'Shubhrata YREN 2780
            'If Me.DropDownListCountry.SelectedValue = "US" Then
            '    Me.TextBoxZip.Attributes.Add("OnKeyPress", "javascript:ValidateNumeric();")
            'Else
            '    Me.TextBoxZip.Attributes.Add("OnKeyPress", "javascript:ValidateAlphaNumeric();")
            'End If
            'Shubhrata YREN 2780

        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message)
        End Try

    End Sub

#Region "******************************Button Event*****************************************"


    Private Sub ButtonUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonUpdate.Click
        Dim stTelephoneError As String 'PPP | 2015.10.13 | YRS-AT-2588
        Try

            'Ashutosh Patil as on 12-Mar-2007
            'YREN - 3028,YREN-3029
            Call SetAddressDetails()

            'Primary Address Validations
            'Ashutosh Patil as on 12-Mar-2007
            If Trim(str_Address1) = "" Then
                MessageBox.Show(PlaceHolder1, "YMCA", "Please enter Address1 for Primary Address.", MessageBoxButtons.Stop)
                Exit Sub
            End If

            If Trim(str_City) = "" Then
                MessageBox.Show(PlaceHolder1, "YMCA", "Please enter City for Primary Address.", MessageBoxButtons.Stop)
                Exit Sub
            End If

            If Trim(str_Address1) <> "" Then
                If str_SelectedCountry = "-Select Country-" And str_SelectedState = "-Select State-" Then
                    MessageBox.Show(PlaceHolder1, "YMCA", "Please select either State or Country for Primary Address ", MessageBoxButtons.Stop)
                    Exit Sub
                End If
            End If

            If (str_SelectedCountry = "-Select Country-") Then
                If str_SelectedState = "-Select State-" Then
                    MessageBox.Show(PlaceHolder1, "YMCA", "Please select either State or Country for Primary Address.", MessageBoxButtons.Stop)
                    Exit Sub
                End If
            End If

            If ValidateCountrySelStateZip(str_SelectedCountry, str_SelectedState, str_ZipCode) = True Then
                MessageBox.Show(PlaceHolder1, "YMCA", l_Str_Msg, MessageBoxButtons.Stop)
                Exit Sub
            End If
            'Anudeep:14.06.2013 BT-1501:YRS 5.0-1745:Capture Beneficiary addresses 
            'If str_CountrySelectedValue = "CA" And (System.Text.RegularExpressions.Regex.IsMatch(str_ZipCode, "^([a-zA-Z])([0-9])([a-zA-Z]) ([0-9])([a-zA-Z])([0-9])")) = False Then
            '    TextBoxZip.Text = ""
            '    MessageBox.Show(PlaceHolder1, "YMCA", "Invalid Zip Code format For Country Canada", MessageBoxButtons.Stop)
            '    Exit Sub
            'End If

            'Added By Ashutosh Patil as on 15-May-2007
            'Telephone no validations on server side and cleint side using Regular expression field validator
            'Anudeep:2013.04.13-BT-1555:YRS 5.0-1769:Length of phone numbers
            If str_CountrySelectedValue = "CA" Or str_CountrySelectedValue = "US" Then
                'START: PPP | 2015.10.13 | YRS-AT-2588 | New validation rules applied and also error message is retrieved from AtsMetaMessages
                stTelephoneError = ValidateTelephoneNumbers(TextBoxPhoneWork.Text.Trim(), TextBoxPhoneHome.Text.Trim(), TextBoxPhoneMobile.Text.Trim(), TextBoxFaxPhone.Text.Trim())
                If Not String.IsNullOrEmpty(stTelephoneError) Then
                    MessageBox.Show(PlaceHolder1, "YMCA", stTelephoneError, MessageBoxButtons.Stop)
                    Exit Sub
                End If
                'If TextBoxPhoneWork.Text.Trim() <> "" Then
                '    'Anudeep:21.06.2013 -BT-1555:YRS 5.0-1769:Length of phone numbers
                '    If Len(TextBoxPhoneWork.Text) <> 10 Then
                '        MessageBox.Show(PlaceHolder1, "YMCA", "Office number must be 10 digits.", MessageBoxButtons.Stop)
                '        Exit Sub
                '    End If
                'End If

                'If TextBoxPhoneHome.Text.Trim() <> "" Then
                '    'Anudeep:21.06.2013 -BT-1555:YRS 5.0-1769:Length of phone numbers
                '    If Len(TextBoxPhoneHome.Text) <> 10 Then
                '        MessageBox.Show(PlaceHolder1, "YMCA", "Home number must be 10 digits.", MessageBoxButtons.Stop)
                '        Exit Sub
                '    End If
                'End If

                'If TextBoxPhoneMobile.Text.Trim() <> "" Then
                '    'Anudeep:21.06.2013 -BT-1555:YRS 5.0-1769:Length of phone numbers
                '    If Len(TextBoxPhoneMobile.Text) <> 10 Then
                '        MessageBox.Show(PlaceHolder1, "YMCA", "Mobile number must be 10 digits.", MessageBoxButtons.Stop)
                '        Exit Sub
                '    End If
                'End If

                'If TextBoxFaxPhone.Text.Trim() <> "" Then
                '    'Anudeep:21.06.2013 -BT-1555:YRS 5.0-1769:Length of phone numbers
                '    If Len(TextBoxFaxPhone.Text) <> 10 Then
                '        MessageBox.Show(PlaceHolder1, "YMCA", "Fax number must be 10 digits.", MessageBoxButtons.Stop)
                '        Exit Sub
                '    End If
                'End If
                'END: PPP | 2015.10.13 | YRS-AT-2588 | New validation rules applied and also error message is retrieved from AtsMetaMessages
            End If
            'BS:2012.05.17:YRS 5.0-1470 :-validate verify address
            Dim i_str_Message1 As String = String.Empty
            'Anudeep:14.06.2013 BT-1501:YRS 5.0-1745:Capture Beneficiary addresses 
            i_str_Message1 = AddressWebUserControl1.ValidateAddress()
            If i_str_Message1 <> "" Then
                Session("VerifiedAddress") = "VerifiedAddress"
                MessageBox.Show(PlaceHolder1, "YMCA", i_str_Message1, MessageBoxButtons.YesNo)
                Exit Sub
            End If

            UpdateAddress()
            Session("States") = Nothing

            Dim msg As String

            msg = msg + "<Script Language='JavaScript'>"

            msg = msg + "window.opener.document.forms(0).submit();"

            msg = msg + "self.close();"

            msg = msg + "</Script>"
            Response.Write(msg)

        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message)
        End Try
    End Sub
    'Private Sub DataRow(ByVal dr As DataRow)
    '  Dim dc As DataColumn
    ' For Each dc In dr.Item
    ' End Sub
    Private Sub ButtonCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonCancel.Click
        Try
            Session("States") = Nothing
            Dim closeWindow As String = "<script language='javascript'>" & _
                                                   "window.close()" & _
                                                   "</script>"

            If (Not Me.IsStartupScriptRegistered("CloseWindow")) Then
                Page.RegisterStartupScript("CloseWindow", closeWindow)
            End If
        Catch ex As Exception

            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message)

        End Try
    End Sub

    'Anudeep:14.06.2013 BT-1501:YRS 5.0-1745:Capture Beneficiary addresses 
    'Private Sub DropDownListCountry_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    '  Address_DropDownListState()
    '    Try
    '        'Shubhrata YREN 2780
    '        If Me.DropDownListCountry.SelectedValue = "US" Then
    '            If Me.TextBoxZip.Text.GetType.FullName.ToString = "System.String" Then
    '                Me.TextBoxZip.Text = ""
    '            End If
    '        End If
    '    Catch ex As Exception
    '        Dim l_String_Exception_Message As String
    '        l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
    '        Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message)
    '    End Try
    '    'Shubhrata YREN 2780
    'End Sub

    Private Sub TextBoxTelephoneHome1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBoxPhoneHome.TextChanged

    End Sub
#End Region

#Region "**********************Private Functions  ********************************************"


    'Anudeep:14.06.2013 BT-1501:YRS 5.0-1745:Capture Beneficiary addresses 
    'Private Sub Address_BindDropDownList()
    '    Dim dataset_AddressStateType As New DataSet
    '    Dim dataset_AddressCountry As New DataSet

    '    Try


    '        dataset_AddressCountry = YMCARET.YmcaBusinessObject.YMCAAddressBOClass.LookUpAddressCountry()
    '        '  Session("dataset_AddressCountry") = dataset_AddressCountry
    '        Me.DropDownListCountry.DataSource = dataset_AddressCountry.Tables(0)
    '        Me.DropDownListCountry.DataMember = "Country"
    '        Me.DropDownListCountry.DataTextField = "chvDescription"
    '        Me.DropDownListCountry.DataValueField = "chvAbbrev"
    '        Me.DropDownListCountry.DataBind()
    '        Me.DropDownListCountry.Items.Insert(0, "-Select Country-")
    '        Me.DropDownListCountry.Items(0).Value = ""

    '    Catch
    '        Return
    '    End Try

    'End Sub

    Private Sub Address_LoadControls(ByVal parameterPersonID As String)
        Try


            Dim dataset_AddressInfo As New DataSet
            Dim datarow_Row As DataRow
            'Anudeep:14.06.2013 BT-1501:YRS 5.0-1745:Capture Beneficiary addresses 
            Dim dsAddress As DataSet
            Dim dtAddress As DataTable
            'Me.Address_BindDropDownList()
            '  dataset_AddressInfo = YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.LookUpAddressInfo(parameterPersonID)
            dataset_AddressInfo = YMCARET.YmcaBusinessObject.UpdateAddressDetailsBOClass.LookUpAddressInfo(parameterPersonID)
            DataGrid1.DataSource = dataset_AddressInfo.Tables("TelephoneInfo")
            DataGrid1.DataBind()



            'Anudeep:14.06.2013 BT-1501:YRS 5.0-1745:Capture Beneficiary addresses 
            AddressWebUserControl1.guiEntityId = parameterPersonID

            dsAddress = Address.GetAddressByEntity(parameterPersonID, EnumEntityCode.PERSON)





            If HelperFunctions.isNonEmpty(dsAddress) Then
                AddressWebUserControl1.LoadAddressDetail(dsAddress.Tables(0).Select("isPrimary = True"))
                'Anudeep:12.09.2013-Bt:2185- Handled to not to occur error if primary address does not exists
                'dtAddress = dsAddress.Tables(0)
            Else
                AddressWebUserControl1.LoadAddressDetail(Nothing)
            End If

            AddressWebUserControl1.EntityCode = EnumEntityCode.PERSON.ToString
            AddressWebUserControl1.AddrCode = "HOME"

            'Start:Anudeep:12.09.2013-Bt:2185- Handled to not to occur error if primary address does not exists
            If dsAddress IsNot Nothing AndAlso dsAddress.Tables.Count > 0 _
                    AndAlso dataset_AddressInfo IsNot Nothing AndAlso dataset_AddressInfo.Tables.Count > 0 Then
                dtAddress = dsAddress.Tables(0)
                dataset_AddressInfo.Tables.Remove("Address")
                dataset_AddressInfo.Tables.Add(dtAddress.Copy())
            End If
            'End:Anudeep:12.09.2013-Bt:2185- Handled to not to occur error if primary address does not exists

            Session("ds_PrimaryAddress") = dataset_AddressInfo

            If dataset_AddressInfo.Tables("Address").Rows.Count > 0 Then


                '    Me.TextBoxAddress1.Text = IIf(IsDBNull(datarow_Row.Item("Address1")), "", datarow_Row.Item("Address1"))
                '    Me.TextBoxAddress2.Text = IIf(IsDBNull(datarow_Row.Item("Address2")), "", datarow_Row.Item("Address2"))
                '    Me.TextBoxAddress3.Text = IIf(IsDBNull(datarow_Row.Item("Address3")), "", datarow_Row.Item("Address3"))
                '    Me.TextBoxCity.Text = IIf(IsDBNull(datarow_Row.Item("City")), "", datarow_Row.Item("City"))

                '    Me.TextBoxZip.Text = IIf(IsDBNull(datarow_Row.Item("Zip")), "", datarow_Row.Item("Zip"))
                '    Me.DropDownListCountry.SelectedValue = Trim(IIf(IsDBNull(datarow_Row.Item("Country")), "", datarow_Row.Item("Country")))
                'If datarow_Row.Item("EffDate") Is Nothing Then
                '    Me.TextBoxEffDate.Text = ""
                'Else
                '    Me.TextBoxEffDate.Text = datarow_Row.Item("EffDate")
                'End If

                '    ''If datarow_Row.Item("Country").ToString.Trim <> Me.DropDownListCountry.SelectedValue And Me.DropDownListCountry.SelectedValue <> "" Then
                '    ''    datarow_Row.Item("Country") = Me.DropDownListCountry.SelectedValue

                '    ''End If
                '    ''If datarow_Row.Item("State").ToString.Trim <> Me.DropDownListState.SelectedValue And Me.DropDownListState.SelectedValue <> "" Then
                '    ''    datarow_Row.Item("State") = Me.DropDownListState.SelectedValue

                '    ''End If
                '    ViewState("SelectedState") = Trim(IIf(IsDBNull(datarow_Row.Item("State")), "", datarow_Row.Item("State")))
                '    Address_DropDownListState()
                '    ' If Me.DropDownListState.SelectedItem.Value <> "" Then

                '    ' End If
            End If
            Dim i As Int32
            If dataset_AddressInfo.Tables("TelephoneInfo").Rows.Count > 0 Then
                ' Dim arr As String()
                'DataGrid1.DataSource = dataset_AddressInfo.Tables("TelephoneInfo")
                '  DataGrid1.DataBind()
                i = dataset_AddressInfo.Tables("TelephoneInfo").Rows.Count()
                For Each datarow_Row In dataset_AddressInfo.Tables("TelephoneInfo").Rows

                    If datarow_Row.Item("PhoneType") <> "System.DBNull" And datarow_Row("PhoneType") <> "" Then
                        If datarow_Row.Item("PhoneType").ToUpper().Trim() = "HOME" And Me.TextBoxPhoneHome.Text = "" Then
                            Me.TextBoxPhoneHome.Text = datarow_Row.Item("PhoneNumber")

                        End If
                        If datarow_Row.Item("PhoneType").ToUpper().Trim() = "OFFICE" And Me.TextBoxPhoneWork.Text = "" Then
                            Me.TextBoxPhoneWork.Text = datarow_Row.Item("PhoneNumber")
                        End If
                        If datarow_Row.Item("PhoneType").ToUpper().Trim() = "FAX" And Me.TextBoxFaxPhone.Text = "" Then
                            Me.TextBoxFaxPhone.Text = datarow_Row.Item("PhoneNumber")
                        End If

                        If CType(datarow_Row("PhoneType"), String).ToUpper().Trim() = "MOBILE" And Me.TextBoxPhoneMobile.Text = "" Then
                            Me.TextBoxPhoneMobile.Text = datarow_Row.Item("PhoneNumber")
                            'Remome This' Me.TextBoxFax.Text = parameterPersonID
                        End If
                    End If
                Next
            End If
        Catch
            Throw
        End Try
    End Sub
    Private Sub UpdateAddress()
        Dim dataset_AddressInfo As New DataSet
        dataset_AddressInfo = CType(Session("ds_PrimaryAddress"), DataSet)
        Dim datarow_Row As DataRow
        Dim datetime_EffDate As DateTime
        Dim dtAddress As DataTable
        Dim dsAddress As DataSet


        If Me.AddressWebUserControl1.EffectiveDate.Trim() <> "" Then
            datetime_EffDate = Convert.ToDateTime(Me.AddressWebUserControl1.EffectiveDate.Trim())
        End If
        Try
            If dataset_AddressInfo.Tables("Address").Rows.Count > 0 Then
                datarow_Row = dataset_AddressInfo.Tables("Address").Rows(0)
                'Ashutosh Patil as on 12-Mar-2007
                'YREN - 3028,YREN-3029
                ' If datarow_Row.Item("Address1").ToString() <> Me.TextBoxAddress1.Text Or datarow_Row.Item("Address2").ToString() <> Me.TextBoxAddress2.Text Or datarow_Row.Item("Address3").ToString() <> Me.TextBoxAddress3.Text Or _
                ''     datarow_Row.Item("City").ToString() <> Me.TextBoxCity.Text Or datarow_Row.Item("State").ToString() <> Me.DropDownListState.SelectedValue Or _
                '     datarow_Row.Item("Zip").ToString() <> Me.TextBoxZip.Text Or datarow_Row.Item("Country").ToString() <> Me.DropDownListCountry.SelectedValue Then
                '''Modified by Shubhrata Oct6th 2006 YREN 2711--on updating the address the bitactive and bitprimary 
                '''of earlier address was nt updated to 0,0
                '''datarow_Row = dataset_AddressInfo.Tables("AddressInfo").NewRow()
                '''Modified By Ashutosh Patil for YREN-3028, YREN-3029
                ''datarow_Row.Item("EntityId") = CType(Session("PersonID"), String)
                ''datarow_Row.Item("Address1") = Me.TextBoxAddress1.Text.Trim()
                ''datarow_Row.Item("Address2") = Me.TextBoxAddress2.Text.Trim()
                ''datarow_Row.Item("Address3") = Me.TextBoxAddress3.Text.Trim()
                ''datarow_Row.Item("City") = Me.TextBoxCity.Text.Trim()
                ''datarow_Row.Item("State") = Me.DropDownListState.SelectedValue
                ''datarow_Row.Item("Zip") = Me.TextBoxZip.Text.Trim()
                ''datarow_Row.Item("Country") = Me.DropDownListCountry.SelectedValue
                ''datarow_Row.Item("EffDate") = datetime_EffDate
                ''datarow_Row.Item("bitPrimary") = 1
                ''datarow_Row.Item("bitActive") = 1
                'Anudeep:14.06.2013 BT-1501:YRS 5.0-1745:Capture Beneficiary addresses 
                If datarow_Row.Item("addr1").ToString() <> AddressWebUserControl1.Address1 Or datarow_Row.Item("addr2").ToString() <> AddressWebUserControl1.Address2 Or datarow_Row.Item("addr3").ToString() <> AddressWebUserControl1.Address3 Or _
                    datarow_Row.Item("city").ToString() <> AddressWebUserControl1.City Or datarow_Row.Item("state").ToString() <> AddressWebUserControl1.DropDownListStateValue Or _
                    datarow_Row.Item("zipCode").ToString() <> AddressWebUserControl1.ZipCode Or datarow_Row.Item("country").ToString() <> AddressWebUserControl1.DropDownListCountryValue Then

                    '    'Modified by Shubhrata Oct6th 2006 YREN 2711--on updating the address the bitactive and bitprimary 
                    '    'of earlier address was nt updated to 0,0
                    '    'datarow_Row = dataset_AddressInfo.Tables("AddressInfo").NewRow()
                    '    'Ashutosh Patil as on 12-Mar-2007
                    '    'YREN - 3028,YREN-3029
                    'Anudeep:14.06.2013 BT-1501:YRS 5.0-1745:Capture Beneficiary addresses 
                    dtAddress = AddressWebUserControl1.GetAddressTable()
                    Address.SaveAddress(dtAddress)
                    datarow_Row = dtAddress(0)
                    'Call SetAddressDetails()

                    ''Ashutosh Patil as on 12-Mar-2007
                    ''YREN - 3028,YREN-3029
                    'datarow_Row.Item("guiEntityId") = CType(Session("PersonID"), String)
                    'datarow_Row.Item("addr1") = str_Address1.Replace(",", "")
                    'datarow_Row.Item("addr2") = str_Address2.Replace(",", "")
                    'datarow_Row.Item("addr3") = str_Address3.Replace(",", "")

                    'datarow_Row.Item("city") = str_City.Replace(",", "")
                    'datarow_Row.Item("state") = str_StateSelectedValue
                    'If str_CountrySelectedValue = "CA" Then
                    '    datarow_Row.Item("zipCode") = Replace(str_ZipCode.Replace(",", ""), " ", "")
                    'Else
                    '    datarow_Row.Item("zipCode") = str_ZipCode.Replace(",", "")
                    'End If
                    'datarow_Row.Item("country") = str_CountrySelectedValue
                    'datarow_Row.Item("effectiveDate") = datetime_EffDate
                    'datarow_Row.Item("isPrimary") = int_IsPrimary
                    'datarow_Row.Item("isActive") = 1

                    ''dataset_AddressInfo.Tables("AddressInfo").Rows.Add(datarow_Row)
                    'If Not dataset_AddressInfo.Tables("Address").GetChanges Is Nothing Then
                    '    YMCARET.YmcaBusinessObject.UpdateAddressDetailsBOClass.UpdateParticipantAddress(dataset_AddressInfo)
                    'End If
                End If
                'Anudeep:12.09.2013-Bt:2185- Handled to not to occur error if primary address does not exists
            ElseIf AddressWebUserControl1.Address1 <> "" And AddressWebUserControl1.City <> "" And AddressWebUserControl1.DropDownListCountryValue <> "" Then
                dtAddress = AddressWebUserControl1.GetAddressTable()
                Address.SaveAddress(dtAddress)
            End If


            'Me.IsAddressUpdated = True
            ''''''''Dim TempRow As DataRow()
            ''''''''Dim TempRow1 As DataRow
            ''''''''Dim bool_Home As Boolean
            ''''''''Dim bool_Work As Boolean
            ''''''''Dim bool_Fax As Boolean
            ''''''''Dim bool_Mobile As Boolean
            ''''''''Dim obj As Object
            ''''''''Dim i As Integer
            ''''''''Dim strTemp, strTemp1 As String
            ''''''''If dataset_AddressInfo.Tables("TelephoneInfo").Rows.Count > 0 Then

            ''''''''    i = dataset_AddressInfo.Tables("TelephoneInfo").Rows.Count
            ''''''''    Dim dv As DataView
            ''''''''    dv = New DataView(dataset_AddressInfo.Tables("TelephoneInfo"))
            ''''''''    'Dim i As Integer
            ''''''''    For Each TempRow1 In dataset_AddressInfo.Tables("TelephoneInfo").Rows
            ''''''''        If Me.TextBoxPhoneHome.Text.Trim() <> TempRow1.Item("PhoneNumber").trim() And Me.TextBoxPhoneHome.Text.Trim() <> "" Then
            ''''''''            If TempRow1.Item("PhoneType").Trim() = "HOME" Then
            ''''''''                strTemp1 = TempRow1.Item("PhoneNumber").trim()
            ''''''''                dv.RowFilter = "PhoneNumber='" & strTemp1 & "' and bitPrimary=1 "
            ''''''''                If dv.Count > 0 Then
            ''''''''                    bool_Home = False
            ''''''''                    TempRow1.Item("PhoneNumber") = Me.TextBoxPhoneHome.Text.Trim()
            ''''''''                Else
            ''''''''                    If dv.Count = 0 Then
            ''''''''                        bool_Home = True
            ''''''''                    End If
            ''''''''                End If
            ''''''''            Else
            ''''''''                bool_Home = True
            ''''''''            End If
            ''''''''        End If
            ''''''''            If Me.TextBoxPhoneWork.Text.Trim() <> TempRow1.Item("PhoneNumber").Trim() And Me.TextBoxPhoneWork.Text.Trim() <> "" Then
            ''''''''            If TempRow1.Item("PhoneType").Trim() = "WORK" Then
            ''''''''                strTemp1 = TempRow1.Item("PhoneNumber").trim()

            ''''''''                dv.RowFilter = "PhoneNumber='" & strTemp1 & "' and bitPrimary=1 "
            ''''''''                If dv.Count > 0 Then
            ''''''''                    ' TempRow1("bitPrimary") = False
            ''''''''                    ' TempRow1("bitActive") = False
            ''''''''                    bool_Work = False
            ''''''''                    TempRow1.Item("PhoneNumber") = Me.TextBoxPhoneWork.Text.Trim()
            ''''''''                Else
            ''''''''                    ' strTemp1 = Me.TextBoxPhoneWork.Text.Trim()
            ''''''''                    ' dv.RowFilter = "PhoneNumber='" & strTemp1 & "' and bitPrimary=1 and PhoneType='WORK' "
            ''''''''                    If dv.Count = 0 Then
            ''''''''                        bool_Work = True
            ''''''''                    End If
            ''''''''                End If
            ''''''''            Else

            ''''''''                bool_Work = True
            ''''''''            End If
            ''''''''        End If
            ''''''''            If Me.TextBoxFaxPhone.Text.Trim() <> TempRow1.Item("PhoneNumber").Trim() And Me.TextBoxFaxPhone.Text.Trim() <> "" Then
            ''''''''                If TempRow1.Item("PhoneType").Trim() = "FAX" Then
            ''''''''                    strTemp1 = TempRow1.Item("PhoneNumber").trim()
            ''''''''                    dv.RowFilter = "PhoneNumber='" & strTemp1 & "' and bitPrimary=1 "
            ''''''''                    If dv.Count > 0 Then
            ''''''''                        bool_Fax = False
            ''''''''                        TempRow1("bitPrimary") = False
            ''''''''                        TempRow1("bitActive") = False
            ''''''''                    End If
            ''''''''                Else
            ''''''''                    strTemp1 = Me.TextBoxFaxPhone.Text.Trim()
            ''''''''                    dv.RowFilter = "PhoneNumber='" & strTemp1 & "' and bitPrimary=1 and PhoneType='FAX' "
            ''''''''                    If dv.Count = 0 Then
            ''''''''                        bool_Fax = True
            ''''''''                    End If
            ''''''''                    ' dataset_AddressInfo.AcceptChanges()
            ''''''''                End If
            ''''''''            End If
            ''''''''            If Me.TextBoxPhoneMobile.Text.Trim() <> TempRow1.Item("PhoneNumber").Trim() And Me.TextBoxPhoneMobile.Text.Trim() <> "" Then
            ''''''''                If TempRow1.Item("PhoneType").Trim() = "MOBILE" Then
            ''''''''                    strTemp1 = TempRow1.Item("PhoneNumber").trim()
            ''''''''                    dv.RowFilter = "PhoneNumber='" & strTemp1 & "' and bitPrimary=1 "
            ''''''''                    If dv.Count > 0 Then
            ''''''''                        bool_Mobile = False
            ''''''''                        ViewState("bit") = "true"
            ''''''''                        TempRow1("bitPrimary") = False
            ''''''''                        TempRow1("bitActive") = False
            ''''''''                    End If
            ''''''''                Else
            ''''''''                    strTemp1 = Me.TextBoxPhoneMobile.Text.Trim()
            ''''''''                    dv.RowFilter = "PhoneNumber='" & strTemp1 & "' and bitPrimary=1 and PhoneType='MOBILE' "
            ''''''''                    strTemp1 = CType(ViewState("bit"), String)
            ''''''''                    If dv.Count = 0 And strTemp1 = "" Then
            ''''''''                        bool_Mobile = True
            ''''''''                    End If
            ''''''''                End If
            ''''''''            End If
            ''''''''    Next
            ''''''''    'dataset_AddressInfo.AcceptChanges()
            ''''''''Else
            ''''''''    bool_Home = True
            ''''''''    bool_Work = True
            ''''''''    bool_Mobile = True
            ''''''''    bool_Fax = True
            ''''''''End If



            ''''''''If TextBoxPhoneHome.Text.Trim() <> "" And bool_Home = True Then
            ''''''''    datarow_Row = dataset_AddressInfo.Tables("TelephoneInfo").NewRow()
            ''''''''    datarow_Row("PhoneType") = "HOME"
            ''''''''    datarow_Row("PhoneNumber") = TextBoxPhoneHome.Text.Trim()
            ''''''''    datarow_Row("EntityId") = Session("PersonID")
            ''''''''    datarow_Row("bitPrimary") = 1
            ''''''''    datarow_Row("bitActive") = 1
            ''''''''    datarow_Row("EffDate") = datetime_EffDate
            ''''''''    dataset_AddressInfo.Tables("TelephoneInfo").Rows.Add(datarow_Row)
            ''''''''    'End If
            ''''''''End If
            ''''''''If TextBoxPhoneWork.Text.Trim() <> "" And bool_Work = True Then
            ''''''''    datarow_Row = dataset_AddressInfo.Tables("TelephoneInfo").NewRow()
            ''''''''    datarow_Row("PhoneType") = "WORK"
            ''''''''    datarow_Row("PhoneNumber") = TextBoxPhoneWork.Text.Trim()
            ''''''''    datarow_Row("EntityId") = Session("PersonID")
            ''''''''    datarow_Row("bitPrimary") = 1
            ''''''''    datarow_Row("bitActive") = 1
            ''''''''    datarow_Row("EffDate") = datetime_EffDate
            ''''''''    dataset_AddressInfo.Tables("TelephoneInfo").Rows.Add(datarow_Row)
            ''''''''    '  End If
            ''''''''End If
            ''''''''If TextBoxFaxPhone.Text.Trim() <> "" And bool_Fax = True Then
            ''''''''    datarow_Row = dataset_AddressInfo.Tables("TelephoneInfo").NewRow()
            ''''''''    datarow_Row("PhoneType") = "FAX"
            ''''''''    datarow_Row("PhoneNumber") = TextBoxFaxPhone.Text.Trim()
            ''''''''    datarow_Row("EntityId") = Session("PersonID")
            ''''''''    datarow_Row("bitPrimary") = 1
            ''''''''    datarow_Row("bitActive") = 1
            ''''''''    datarow_Row("EffDate") = datetime_EffDate
            ''''''''    dataset_AddressInfo.Tables("TelephoneInfo").Rows.Add(datarow_Row)
            ''''''''    ' End If
            ''''''''End If
            ''''''''If TextBoxPhoneMobile.Text.Trim() <> "" And bool_Mobile = True Then
            ''''''''    datarow_Row = dataset_AddressInfo.Tables("TelephoneInfo").NewRow()
            ''''''''    datarow_Row("PhoneType") = "MOBILE"
            ''''''''    datarow_Row("PhoneNumber") = TextBoxPhoneMobile.Text.Trim()
            ''''''''    datarow_Row("EntityId") = Session("PersonID")
            ''''''''    datarow_Row("bitPrimary") = 1
            ''''''''    datarow_Row("bitActive") = 1
            ''''''''    datarow_Row("EffDate") = datetime_EffDate
            ''''''''    dataset_AddressInfo.Tables("TelephoneInfo").Rows.Add(datarow_Row)
            ''''''''    ' End If
            ''''''''End If


            ''''''''If Not dataset_AddressInfo.Tables("TelephoneInfo").GetChanges Is Nothing Then
            ''''''''    'YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.UpdateTelephone(dataset_AddressInfo)
            ''''''''    YMCARET.YmcaBusinessObject.UpdateAddressDetailsBOClass.UpdateTelephone(dataset_AddressInfo)
            ''''''''    Session("ds_PrimaryAddress") = dataset_AddressInfo
            ''''''''End If
            ''''''''Address_LoadControls(Session("PersonID"))
            ''''''''' DataGrid1.DataSource = dataset_AddressInfo.Tables("TelephoneInfo")
            ''''''''' DataGrid1.DataBind()
            'change by ruchi
            Dim l_datarow As DataRow
            Dim l_Bool_Home As Boolean = False
            Dim l_Bool_Work As Boolean = False
            Dim l_Bool_Fax As Boolean = False
            Dim l_Bool_Mobile As Boolean = False
            If dataset_AddressInfo.Tables("TelephoneInfo").Rows.Count > 0 Then
                If Not dataset_AddressInfo.Tables("TelephoneInfo") Is Nothing Then
                    ''For Home Phone
                    'priya YRS 5.0-424
                    Dim l_string_OFFICERow As String
                    Dim l_string_HomeRow As String
                    Dim l_string_FAXRow As String
                    Dim l_string_MOBILERow As String
                    'End YRS 5.0-424
                    For Each l_datarow In dataset_AddressInfo.Tables("TelephoneInfo").Rows
                        If (l_datarow("PhoneType").ToString() <> "System.DBNull" And l_datarow("PhoneType").ToString() <> "") Then
                            If l_datarow("PhoneType").ToString.ToUpper.Trim() = "HOME" Then
                                If l_datarow("PhoneNumber").ToString.Trim <> TextBoxPhoneHome.Text.Trim Then
                                    l_datarow("PhoneNumber") = TextBoxPhoneHome.Text.Trim
                                    'Priya YRS 5.0-424
                                    l_string_HomeRow = TextBoxPhoneHome.Text.Trim
                                    'Start:AA:10.09.2013:Removing the code which updates as bitActive and bitPrimary to '0'
                                    'If TextBoxPhoneHome.Text.Trim = "" Then
                                    '    '    l_datarow("bitActive") = 1
                                    '    '    l_datarow("bitPrimary") = 1
                                    '    'Else
                                    '    l_datarow("bitActive") = 0
                                    '    l_datarow("bitPrimary") = 0
                                    'End If
                                    'End YRS 5.0-424
                                    'End:AA:10.09.2013:Removing the code which updates as bitActive and bitPrimary to '0'
                                    If l_datarow("EffDate").ToString.Trim <> Me.TextBoxEffDate.Text.ToString.Trim() Then
                                        l_datarow("EffDate") = Me.TextBoxEffDate.Text.ToString.Trim()
                                    End If
                                End If
                                l_Bool_Home = True

                            End If
                            If l_datarow("PhoneType").ToString.ToUpper.Trim() = "OFFICE" Then
                                If l_datarow("PhoneNumber").ToString.Trim <> Me.TextBoxPhoneWork.Text.Trim Then
                                    l_datarow("PhoneNumber") = Me.TextBoxPhoneWork.Text.Trim
                                    'Priya YRS 5.0-424
                                    l_string_OFFICERow = Me.TextBoxPhoneWork.Text.Trim
                                    'Start:AA:10.09.2013:Removing the code which updates as bitActive and bitPrimary to '0'
                                    'If Me.TextBoxPhoneWork.Text.Trim = "" Then
                                    '    '    l_datarow("bitActive") = 1
                                    '    '    l_datarow("bitPrimary") = 1
                                    '    'Else
                                    '    l_datarow("bitActive") = 0
                                    '    l_datarow("bitPrimary") = 0
                                    'End If
                                    'End YRS 5.0-424
                                    'End:AA:10.09.2013:Removing the code which updates as bitActive and bitPrimary to '0'
                                    If l_datarow("EffDate").ToString.Trim <> Me.TextBoxEffDate.Text.ToString.Trim() Then
                                        l_datarow("EffDate") = Me.TextBoxEffDate.Text.ToString.Trim()
                                    End If
                                End If
                                l_Bool_Work = True

                            End If
                            If l_datarow("PhoneType").ToString.ToUpper.Trim() = "FAX" Then
                                If l_datarow("PhoneNumber").ToString.Trim <> Me.TextBoxFaxPhone.Text Then
                                    l_datarow("PhoneNumber") = Me.TextBoxFaxPhone.Text
                                    'Priya YRS 5.0-424
                                    l_string_FAXRow = Me.TextBoxFaxPhone.Text
                                    'Start:AA:10.09.2013:Removing the code which updates as bitActive and bitPrimary to '0'
                                    'If Me.TextBoxFaxPhone.Text = "" Then
                                    '    '    l_datarow("bitActive") = 1
                                    '    '    l_datarow("bitPrimary") = 1
                                    '    'Else
                                    '    l_datarow("bitActive") = 0
                                    '    l_datarow("bitPrimary") = 0
                                    'End If
                                    'End YRS 5.0-424
                                    'End:AA:10.09.2013:Removing the code which updates as bitActive and bitPrimary to '0'
                                    If l_datarow("EffDate").ToString.Trim <> Me.TextBoxEffDate.Text.ToString.Trim() Then
                                        l_datarow("EffDate") = Me.TextBoxEffDate.Text.ToString.Trim()
                                    End If
                                End If
                                l_Bool_Fax = True

                            End If
                            If l_datarow("PhoneType").ToString.ToUpper.Trim() = "MOBILE" Then
                                If l_datarow("PhoneNumber").ToString.Trim <> Me.TextBoxPhoneMobile.Text Then
                                    l_datarow("PhoneNumber") = Me.TextBoxPhoneMobile.Text
                                    'Priya YRS 5.0-424
                                    l_string_MOBILERow = Me.TextBoxPhoneMobile.Text
                                    'Start:AA:10.09.2013:Removing the code which updates as bitActive and bitPrimary to '0'
                                    'If Me.TextBoxPhoneMobile.Text = "" Then
                                    '    '    l_datarow("bitActive") = 1
                                    '    '    l_datarow("bitPrimary") = 1
                                    '    'Else
                                    '    l_datarow("bitActive") = 0
                                    '    l_datarow("bitPrimary") = 0
                                    'End If
                                    'End YRS 5.0-424
                                    'End:AA:10.09.2013:Removing the code which updates as bitActive and bitPrimary to '0'
                                    If l_datarow("EffDate").ToString.Trim <> Me.TextBoxEffDate.Text.ToString.Trim() Then
                                        l_datarow("EffDate") = Me.TextBoxEffDate.Text.ToString.Trim()
                                    End If
                                End If
                                l_Bool_Mobile = True
                            End If
                        End If
                    Next
                    If l_Bool_Home = False And TextBoxPhoneHome.Text.Trim.Length > 0 Then
                        Dim l_HomeRow As DataRow
                        l_HomeRow = dataset_AddressInfo.Tables("TelephoneInfo").NewRow
                        l_HomeRow("PhoneType") = "HOME"
                        l_HomeRow("PhoneNumber") = Me.TextBoxPhoneHome.Text.Trim
                        l_HomeRow("EntityId") = Session("PersonID")
                        l_HomeRow("bitPrimary") = 1
                        l_HomeRow("bitActive") = 1
                        l_HomeRow("EffDate") = Me.TextBoxEffDate.Text.ToString.Trim()
                        dataset_AddressInfo.Tables("TelephoneInfo").Rows.Add(l_HomeRow)
                        'Priya YRS 5.0-424
                        'Start:AA:10.09.2013:Removing the code which updates as bitActive and bitPrimary to '0'
                        'ElseIf l_Bool_Home = True AndAlso l_string_HomeRow <> "" Then
                        '    Dim l_HomeRow As DataRow
                        '    l_HomeRow = dataset_AddressInfo.Tables("TelephoneInfo").NewRow
                        '    l_HomeRow("PhoneType") = "HOME"
                        '    l_HomeRow("PhoneNumber") = Me.TextBoxPhoneHome.Text.Trim
                        '    l_HomeRow("EntityId") = Session("PersonID")
                        '    l_HomeRow("bitPrimary") = 1
                        '    l_HomeRow("bitActive") = 1
                        '    l_HomeRow("EffDate") = Me.TextBoxEffDate.Text.ToString.Trim()
                        '    dataset_AddressInfo.Tables("TelephoneInfo").Rows.Add(l_HomeRow)
                        'End YRS 5.0-424
                        'End:AA:10.09.2013:Removing the code which updates as bitActive and bitPrimary to '0'
                    End If

                    If l_Bool_Work = False And TextBoxPhoneWork.Text.Trim.Length > 0 Then
                        Dim l_WorkRow As DataRow
                        l_WorkRow = dataset_AddressInfo.Tables("TelephoneInfo").NewRow
                        l_WorkRow("PhoneType") = "OFFICE"
                        l_WorkRow("PhoneNumber") = Me.TextBoxPhoneWork.Text.Trim
                        l_WorkRow("EntityId") = Session("PersonID")
                        l_WorkRow("bitPrimary") = 1
                        l_WorkRow("bitActive") = 1
                        l_WorkRow("EffDate") = Me.TextBoxEffDate.Text.ToString.Trim()
                        dataset_AddressInfo.Tables("TelephoneInfo").Rows.Add(l_WorkRow)
                        'Priya YRS 5.0-424
                        'Start:AA:10.09.2013:Removing the code which updates as bitActive and bitPrimary to '0'
                        'ElseIf l_Bool_Work = True AndAlso l_string_OFFICERow <> "" Then
                        '    Dim l_WorkRow As DataRow
                        '    l_WorkRow = dataset_AddressInfo.Tables("TelephoneInfo").NewRow
                        '    l_WorkRow("PhoneType") = "OFFICE"
                        '    l_WorkRow("PhoneNumber") = Me.TextBoxPhoneWork.Text.Trim
                        '    l_WorkRow("EntityId") = Session("PersonID")
                        '    l_WorkRow("bitPrimary") = 1
                        '    l_WorkRow("bitActive") = 1
                        '    l_WorkRow("EffDate") = Me.TextBoxEffDate.Text.ToString.Trim()
                        '    dataset_AddressInfo.Tables("TelephoneInfo").Rows.Add(l_WorkRow)
                        'End YRS 5.0-424
                        'End:AA:10.09.2013:Removing the code which updates as bitActive and bitPrimary to '0'
                    End If

                    If l_Bool_Fax = False And TextBoxFaxPhone.Text.Trim.Length > 0 Then
                        Dim l_FaxRow As DataRow
                        l_FaxRow = dataset_AddressInfo.Tables("TelephoneInfo").NewRow
                        l_FaxRow("PhoneType") = "FAX"
                        l_FaxRow("PhoneNumber") = Me.TextBoxFaxPhone.Text.Trim
                        l_FaxRow("EntityId") = Session("PersonID")
                        l_FaxRow("bitPrimary") = 1
                        l_FaxRow("bitActive") = 1
                        l_FaxRow("EffDate") = Me.TextBoxEffDate.Text.ToString.Trim()
                        dataset_AddressInfo.Tables("TelephoneInfo").Rows.Add(l_FaxRow)
                        'Priya YRS 5.0-424
                        'Start:AA:10.09.2013:Removing the code which updates as bitActive and bitPrimary to '0'
                        'ElseIf l_Bool_Fax = True AndAlso l_string_FAXRow <> "" Then

                        '    Dim l_FaxRow As DataRow
                        '    l_FaxRow = dataset_AddressInfo.Tables("TelephoneInfo").NewRow
                        '    l_FaxRow("PhoneType") = "FAX"
                        '    l_FaxRow("PhoneNumber") = Me.TextBoxFaxPhone.Text.Trim
                        '    l_FaxRow("EntityId") = Session("PersonID")
                        '    l_FaxRow("bitPrimary") = 1
                        '    l_FaxRow("bitActive") = 1
                        '    l_FaxRow("EffDate") = Me.TextBoxEffDate.Text.ToString.Trim()
                        '    dataset_AddressInfo.Tables("TelephoneInfo").Rows.Add(l_FaxRow)
                        'End YRS 5.0-424
                        'End:AA:10.09.2013:Removing the code which updates as bitActive and bitPrimary to '0'
                    End If

                    If l_Bool_Mobile = False And TextBoxPhoneMobile.Text.Trim.Length > 0 Then
                        Dim l_MobileRow As DataRow
                        l_MobileRow = dataset_AddressInfo.Tables("TelephoneInfo").NewRow
                        l_MobileRow("PhoneType") = "MOBILE"
                        l_MobileRow("PhoneNumber") = Me.TextBoxPhoneMobile.Text.Trim
                        l_MobileRow("EntityId") = Session("PersonID")
                        l_MobileRow("bitPrimary") = 1
                        l_MobileRow("bitActive") = 1
                        l_MobileRow("EffDate") = Me.TextBoxEffDate.Text.ToString.Trim()
                        dataset_AddressInfo.Tables("TelephoneInfo").Rows.Add(l_MobileRow)
                        'Priya YRS 5.0-424
                        'Start:AA:10.09.2013:Removing the code which updates as bitActive and bitPrimary to '0'
                        'ElseIf l_Bool_Mobile = True AndAlso l_string_MOBILERow <> "" Then

                        '    Dim l_MobileRow As DataRow
                        '    l_MobileRow = dataset_AddressInfo.Tables("TelephoneInfo").NewRow
                        '    l_MobileRow("PhoneType") = "MOBILE"
                        '    l_MobileRow("PhoneNumber") = Me.TextBoxPhoneMobile.Text.Trim
                        '    l_MobileRow("EntityId") = Session("PersonID")
                        '    l_MobileRow("bitPrimary") = 1
                        '    l_MobileRow("bitActive") = 1
                        '    l_MobileRow("EffDate") = Me.TextBoxEffDate.Text.ToString.Trim()
                        '    dataset_AddressInfo.Tables("TelephoneInfo").Rows.Add(l_MobileRow)
                        'End YRS 5.0-424
                        'End:AA:10.09.2013:Removing the code which updates as bitActive and bitPrimary to '0'
                    End If

                End If
            Else
                If TextBoxPhoneHome.Text.Trim.Length > 0 Then
                    Dim l_HomeRow As DataRow
                    l_HomeRow = dataset_AddressInfo.Tables("TelephoneInfo").NewRow
                    l_HomeRow("PhoneType") = "HOME"
                    l_HomeRow("PhoneNumber") = Me.TextBoxPhoneHome.Text.Trim
                    l_HomeRow("EntityId") = Session("PersonID")
                    l_HomeRow("bitPrimary") = 1
                    l_HomeRow("bitActive") = 1
                    l_HomeRow("EffDate") = Me.TextBoxEffDate.Text.ToString.Trim()
                    dataset_AddressInfo.Tables("TelephoneInfo").Rows.Add(l_HomeRow)
                End If

                If TextBoxPhoneWork.Text.Trim.Length > 0 Then
                    Dim l_WorkRow As DataRow
                    l_WorkRow = dataset_AddressInfo.Tables("TelephoneInfo").NewRow
                    l_WorkRow("PhoneType") = "OFFICE"
                    l_WorkRow("PhoneNumber") = Me.TextBoxPhoneWork.Text.Trim
                    l_WorkRow("EntityId") = Session("PersonID")
                    l_WorkRow("bitPrimary") = 1
                    l_WorkRow("bitActive") = 1
                    l_WorkRow("EffDate") = Me.TextBoxEffDate.Text.ToString.Trim()
                    dataset_AddressInfo.Tables("TelephoneInfo").Rows.Add(l_WorkRow)
                End If

                If TextBoxFaxPhone.Text.Trim.Length > 0 Then
                    Dim l_FaxRow As DataRow
                    l_FaxRow = dataset_AddressInfo.Tables("TelephoneInfo").NewRow
                    l_FaxRow("PhoneType") = "FAX"
                    l_FaxRow("PhoneNumber") = Me.TextBoxFaxPhone.Text.Trim
                    l_FaxRow("EntityId") = Session("PersonID")
                    l_FaxRow("bitPrimary") = 1
                    l_FaxRow("bitActive") = 1
                    l_FaxRow("EffDate") = Me.TextBoxEffDate.Text.ToString.Trim()
                    dataset_AddressInfo.Tables("TelephoneInfo").Rows.Add(l_FaxRow)
                End If

                If TextBoxPhoneMobile.Text.Trim.Length > 0 Then
                    Dim l_MobileRow As DataRow
                    l_MobileRow = dataset_AddressInfo.Tables("TelephoneInfo").NewRow
                    l_MobileRow("PhoneType") = "MOBILE"
                    l_MobileRow("PhoneNumber") = Me.TextBoxPhoneMobile.Text.Trim
                    l_MobileRow("EntityId") = Session("PersonID")
                    l_MobileRow("bitPrimary") = 1
                    l_MobileRow("bitActive") = 1
                    l_MobileRow("EffDate") = Me.TextBoxEffDate.Text.ToString.Trim()
                    dataset_AddressInfo.Tables("TelephoneInfo").Rows.Add(l_MobileRow)
                End If

            End If
            If Not dataset_AddressInfo.Tables("TelephoneInfo") Is Nothing Then
                If dataset_AddressInfo.Tables("TelephoneInfo").Rows.Count > 0 Then
                    If Not dataset_AddressInfo.Tables("TelephoneInfo").GetChanges Is Nothing Then
                        YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.UpdateTelephone(dataset_AddressInfo)
                    End If
                End If
            End If
            'Anudeep:14.06.2013 BT-1501:YRS 5.0-1745:Capture Beneficiary addresses 
            dataset_AddressInfo = YMCARET.YmcaBusinessObject.UpdateAddressDetailsBOClass.LookUpAddressInfo(Session("PersonID"))

            dsAddress = Address.GetAddressByEntity(Session("PersonID"), EnumEntityCode.PERSON)
            dtAddress = dsAddress.Tables(0)
            dataset_AddressInfo.Tables.Remove("Address")
            dataset_AddressInfo.Tables.Add(dtAddress.Copy())
            Session("ds_PrimaryAddress") = dataset_AddressInfo

            'end of change
        Catch
            Throw
        End Try


    End Sub


    'Start:Anudeep:14.06.2013 BT-1501:YRS 5.0-1745:Capture Beneficiary addresses 
    'Sub Address_DropDownListState()
    '    Dim dataset_States As DataSet
    '    Dim datarow_state As DataRow()
    '    Dim strCountryCode As String
    '    strCountryCode = String.Empty
    '    If Session("States") Is Nothing Or Page.IsPostBack = False Then
    '        dataset_States = YMCARET.YmcaBusinessObject.YMCAAddressBOClass.GetStates()
    '        Me.DropDownListState.DataSource = dataset_States.Tables(0)
    '        Me.DropDownListState.DataTextField = "chvDescription"
    '        Me.DropDownListState.DataValueField = "chvcodevalue"
    '        Me.DropDownListState.DataBind()
    '        Me.DropDownListState.Items.Insert(0, "-Select State-")
    '        Me.DropDownListState.Items(0).Value = ""
    '        Session("States") = dataset_States
    '    Else
    '        dataset_States = CType(Session("States"), DataSet)
    '    End If
    '    If dataset_States.Tables(0).Rows.Count > 0 Then
    '        If Not ViewState("SelectedState") Is Nothing Then
    '            Me.DropDownListState.SelectedValue = CType(ViewState("SelectedState"), String)
    '            ' ViewState("SelectedState") = Nothing
    '            datarow_state = dataset_States.Tables(0).Select("chvcodevalue='" & DropDownListState.SelectedValue.Trim() & "' ")
    '            If datarow_state.Length > 0 Then
    '                strCountryCode = datarow_state(0)("chvCountryCode")

    '            End If
    '            Address_DropDownCountry(strCountryCode)
    '        End If

    '    End If
    'End Sub
    'Sub Address_DropDownCountry(ByVal strCountryCode As String)
    '    If strCountryCode.Length > 0 Then
    '        Me.DropDownListCountry.SelectedValue = strCountryCode
    '    Else
    '        Me.DropDownListCountry.SelectedValue = ""
    '    End If
    'End Sub

    'Private Sub DropDownListState_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    ViewState("SelectedState") = DropDownListState.SelectedValue
    '    'Address_DropDownListState()
    'End Sub
    'End:Anudeep:14.06.2013 BT-1501:YRS 5.0-1745:Capture Beneficiary addresses 
    Private Sub SetAddressDetails()
        Try
            str_Address1 = AddressWebUserControl1.Address1
            str_Address2 = AddressWebUserControl1.Address2
            str_Address3 = AddressWebUserControl1.Address3
            str_City = AddressWebUserControl1.City
            str_ZipCode = AddressWebUserControl1.ZipCode
            str_StateSelectedValue = AddressWebUserControl1.DropDownListStateValue
            str_CountrySelectedValue = AddressWebUserControl1.DropDownListCountryValue
            str_SelectedState = AddressWebUserControl1.DropDownListStateText
            str_SelectedCountry = AddressWebUserControl1.DropDownListCountryText
            int_IsPrimary = AddressWebUserControl1.IsPrimary
            str_strPerssId = AddressWebUserControl1.guiEntityId
        Catch
            Throw
        End Try
    End Sub
    Private Function ValidateCountrySelStateZip(ByVal CountryValue As String, ByVal DropDownListStateText As String, ByVal str_Pri_Zip As String) As Boolean
        '**********************************************************************************************************************
        ' Author            : Ashutosh Patil 
        ' Created On        : 25-Jan-2007
        ' Desc              : This function will validate for the state and Zip Code selected against country USA and Canada
        '                     FOR USA and CANADA - State and Zip Code is mandatory
        ' Related To        : YREN-3029,YREN-3028
        ' Modifed By        : 
        ' Modifed On        :
        ' Reason For Change : 
        '***********************************************************************************************************************
        Try
            l_Str_Msg = ""
            If (CountryValue = "US" Or CountryValue = "CA") And DropDownListStateText = "-Select State-" Then
                l_Str_Msg = "Please select state"
                ValidateCountrySelStateZip = True
            ElseIf (CountryValue = "US" Or CountryValue = "CA") And str_Pri_Zip = "" Then
                l_Str_Msg = "Please enter Zip Code"
                ValidateCountrySelStateZip = True
            ElseIf CountryValue = "US" And (Len(str_Pri_Zip) <> 5 And Len(str_Pri_Zip) <> 9) Then
                l_Str_Msg = "Invalid Zip Code format"
                ValidateCountrySelStateZip = True
            ElseIf CountryValue = "CA" And (Len(str_Pri_Zip) <> 7) Then
                l_Str_Msg = "Invalid Zip Code format"
                ValidateCountrySelStateZip = True
            End If
            If CountryValue = "US" And ValidateCountrySelStateZip = True Then
                l_Str_Msg = l_Str_Msg & " for United States"
            ElseIf CountryValue = "CA" And ValidateCountrySelStateZip = True Then
                l_Str_Msg = l_Str_Msg & " for Canada"
            End If
        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
        End Try
    End Function

    'START: PPP | 2015.10.13 | YRS-AT-2588 | Validates all four telephone box and retrieves error message from AtsMetaMessages
    Private Function ValidateTelephoneNumbers(ByVal stOffice As String, ByVal stHome As String, ByVal stMobile As String, ByVal stFax As String) As String
        Dim lsErrorList As List(Of String)
        Dim stTempErrorHolder, stErrors As String

        lsErrorList = New List(Of String)
        If Not String.IsNullOrEmpty(stOffice) And stOffice.Length > 0 Then
            stTempErrorHolder = Validation.Telephone(stOffice, YMCAObjects.TelephoneType.Office)
            If (Not String.IsNullOrEmpty(stTempErrorHolder)) Then
                lsErrorList.Add(stTempErrorHolder)
            End If
        End If

        If Not String.IsNullOrEmpty(stHome) And stHome.Length > 0 Then
            stTempErrorHolder = Validation.Telephone(stHome, YMCAObjects.TelephoneType.Home)
            If (Not String.IsNullOrEmpty(stTempErrorHolder)) Then
                lsErrorList.Add(stTempErrorHolder)
            End If
        End If

        If Not String.IsNullOrEmpty(stMobile) And stMobile.Length > 0 Then
            stTempErrorHolder = Validation.Telephone(stMobile, YMCAObjects.TelephoneType.Mobile)
            If (Not String.IsNullOrEmpty(stTempErrorHolder)) Then
                lsErrorList.Add(stTempErrorHolder)
            End If
        End If

        If Not String.IsNullOrEmpty(stFax) And stFax.Length > 0 Then
            stTempErrorHolder = Validation.Telephone(stFax, YMCAObjects.TelephoneType.Fax)
            If (Not String.IsNullOrEmpty(stTempErrorHolder)) Then
                lsErrorList.Add(stTempErrorHolder)
            End If
        End If

        stErrors = String.Empty
        If lsErrorList.Count > 0 Then
            For Each item As String In lsErrorList
                stErrors = String.Format("{0}{1}{2}", stErrors, item, "<br />")
            Next
        End If

        ValidateTelephoneNumbers = stErrors
    End Function
    'END: PPP | 2015.10.13 | YRS-AT-2588 | Validates all four telephone box and retrieves error message from AtsMetaMessages

    '******************************************End Functions ********************************************************
#End Region

#Region "Comminted Code"
    Sub ForState()

        '      Sub Address_DropDownListState1()
        '    If Me.DropDownListCountry.SelectedValue = "" Then
        '        ClearDropDownListState()
        '    Else
        '        Dim ds As DataSet
        '        ' YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.LookUp_AMP_StateNames(DropdownlistCountry.SelectedValue.ToString().Trim())
        '        ds = YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.LookUp_AMP_StateNames(DropDownListCountry.SelectedValue.ToString().Trim())
        '        If ds.Tables(0).Rows.Count > 0 Then
        '            Me.DropDownListState.DataSource = ds.Tables(0)
        '            Me.DropDownListState.DataMember = "State Type"
        '            Me.DropDownListState.DataTextField = "Description"
        '            Me.DropDownListState.DataValueField = "chvcodevalue"
        '            Me.DropDownListState.DataBind()
        '            Me.DropDownListState.Items.Insert(0, "-Select State-")
        '            Me.DropDownListState.Items(0).Value = ""
        '            If Not ViewState("SelectedState") Is Nothing Then
        '                Me.DropDownListState.SelectedValue = CType(ViewState("SelectedState"), String)
        '                ViewState("SelectedState") = Nothing
        '            End If
        '        Else
        '            ClearDropDownListState()
        '        End If

        '    End If
        'End Sub
        'Sub ClearDropDownListState()
        '       DropDownListState.DataSource = Nothing
        '       DropDownListState.Items.Clear()
        '       Me.DropDownListState.Items.Insert(0, "-Select State-")
        '       Me.DropDownListState.Items(0).Value = ""
        '       Me.DropDownListState.DataBind()
        '       DropDownListState.SelectedValue = ""
        '   End Sub

        'YMCARET.YmcaBusinessObject.YMCAAddressBOClass.LookUpAddressStateType()

        'Me.DropDownListState.DataSource = ds
        'Me.DropDownListState.DataMember = "State Type"
        'Me.DropDownListState.DataTextField = "chvShortDescription"
        'Me.DropDownListState.DataValueField = "chvCodeValue"
        'Me.DropDownListState.DataBind()
        'Me.DropDownListState.Items.Insert(0, "-Select State-")
        'Me.DropDownListState.Items(0).Value = ""
    End Sub
#End Region
    'Start:Anudeep:14.06.2013 BT-1501:YRS 5.0-1745:Capture Beneficiary addresses 
    'Public Function SaveAddress(ByVal dataset_AddressInfo As DataSet) As String
    '    Try


    '        Dim ds_service_data As New DataSet("ServiceData")
    '        Dim dt_Address As New DataTable("Address")
    '        Dim dr_address As DataRow
    '        dt_Address.Columns.Add("guiEntityID")
    '        dt_Address.Columns.Add("entityCode")
    '        dt_Address.Columns.Add("effectiveDate")
    '        dt_Address.Columns.Add("addrCode")
    '        dt_Address.Columns.Add("addr1")
    '        dt_Address.Columns.Add("addr2")
    '        dt_Address.Columns.Add("addr3")
    '        dt_Address.Columns.Add("city")
    '        dt_Address.Columns.Add("state")
    '        dt_Address.Columns.Add("zipCode")
    '        dt_Address.Columns.Add("country")
    '        dt_Address.Columns.Add("isPrimary")
    '        dt_Address.Columns.Add("isBadAddress")

    '        dr_address = dt_Address.NewRow()
    '        dr_address("guiEntityID") = dataset_AddressInfo.Tables("Address").Rows(0)("guiEntityID").ToString()
    '        dr_address("entityCode") = "PERSON"
    '        dr_address("effectiveDate") = Convert.ToDateTime(dataset_AddressInfo.Tables("Address").Rows(0)("effectiveDate").ToString()).ToString("MM/dd/yyyy")
    '        dr_address("addrCode") = dataset_AddressInfo.Tables("Address").Rows(0)("addrCode").ToString()
    '        dr_address("addr1") = dataset_AddressInfo.Tables("Address").Rows(0)("addr1").ToString()
    '        dr_address("addr2") = dataset_AddressInfo.Tables("Address").Rows(0)("addr2").ToString()
    '        dr_address("addr3") = dataset_AddressInfo.Tables("Address").Rows(0)("addr3").ToString()
    '        dr_address("city") = dataset_AddressInfo.Tables("Address").Rows(0)("city").ToString().Replace(",", "")
    '        dr_address("state") = dataset_AddressInfo.Tables("Address").Rows(0)("state").ToString()
    '        dr_address("zipCode") = dataset_AddressInfo.Tables("Address").Rows(0)("zipCode").ToString().Replace(",", "")
    '        dr_address("country") = dataset_AddressInfo.Tables("Address").Rows(0)("country").ToString()
    '        dr_address("isPrimary") = dataset_AddressInfo.Tables("Address").Rows(0)("isPrimary").ToString().ToLower()
    '        dr_address("isBadAddress") = dataset_AddressInfo.Tables("Address").Rows(0)("isBadAddress").ToString().ToLower()
    '        dt_Address.Rows.Add(dr_address)
    '        ds_service_data.Tables.Add(dt_Address)
    '        Dim str As String
    '        str = ds_service_data.GetXml()
    '        str = str.Replace("<Address>", "<InputInformation>" + vbNewLine + "<Address>")
    '        str = str.Replace("</Address>", "</Address>" + vbNewLine + "</InputInformation>")
    '        objWebServicereturn = objWebService.SaveUserAddressForWebsite(str)
    '        If objWebServicereturn.ReturnStatus <> YRSBeneficiaryService.Status.Success Then
    '            Return objWebServicereturn.Message
    '            Exit Function
    '        Else
    '            Return ""
    '        End If
    '    Catch ex As Exception

    '    End Try
    'End Function
    'End:Anudeep:14.06.2013 BT-1501:YRS 5.0-1745:Capture Beneficiary addresses 
End Class

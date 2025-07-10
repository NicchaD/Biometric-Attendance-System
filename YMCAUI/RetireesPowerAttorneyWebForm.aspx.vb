'*******************************************************************************
' Copyright YMCA Retirement Fund All Rights Reserved. 
'
' Project Name		:	YMCA-YRS
' FileName			:	RetireesPowerAttorneyWebForm.aspx.vb
' Author Name		:	Shefali Bharti
' Employee ID		:	33488
' Email				:	shefali.bharti@3i-infotech.com
' Contact No		:	8740
' Creation Time		:	5/17/2005 5:12:45 PM
' Program Specification Name	:	Doc 3.1.2
' Unit Test Plan Name			:	
' Description					:	This is a Retirees Power of Attorney pop up window of General Tab
' Cache-Session                 : Hafiz 04Feb06
'*******************************************************************************
'Hafiz 04Feb06 Cache-Session
'Imports Microsoft.Practices.EnterpriseLibrary.Caching
'Hafiz 03Feb06 Cache-Session
'Changed By         :   Rahul Nasa
'Dated              :   14 Feb 06
'Description         :  Country was always  Andorra 
'********************************************************************************************************************************
'Modification History
'********************************************************************************************************************************
'Modified By        Date            Description
'********************************************************************************************************************************
'Ashutosh Patil     16-Mar-2007	    YREN-3028, YREN- 3029 
'Swopna Valappil    28-Dec-2007     On clicking ButtonAdd1 button,no item in datagrid DataGridRetireesAttorney should be selected
'Swopna             19-May-2008     BT-432
'Neeraj Singh                   12/Nov/2009         Added form name for security issue YRS 5.0-940 
'Imran              2010.12.18      Comment address validation on page level and call validation from address user control
'BS                 2011.07.27     'BT:905 - Assign current dsPOA to Session("dsPOA") after Insert & update POA, so any changes on termination date or any field will affect while select poa from grid
'Anudeep            2013.07.29      Bt-1683-YRS 5.0-1862:Add notes record when user enters address in any module.
'Anudeep            2013.10.14      BT:2236-After modifying address save button gets disabled
'Anudeep            2013.11.07      BT:2190-YRS 5.0-2199:Create view mode for Power of Attorney display 
'Anudeep            2013.11.07      BT:2269-YRS 5.0-2234:Change labelling of Power of Attorney / POW 
'Anudeep            2013.12.16      Code refactoring
'Anudeep            2014.06.11      BT:2554:YRS 5.0-2375 : Notes display if you have spaces in name.
'Manthan Rajguru    2015.09.16      YRS-AT-2550: YRS data cleanup: copyright comments and namespace/reference (Changed the Namespace/reference from 'Infotech' to 'YMCARET')
'Bala               2015.11.20      YRS-AT-2534: POA Last name issue
'********************************************************************************************************************************
Public Class RetireesPowerAttorneyWebForm
    Inherits System.Web.UI.Page
    'below line is Added by Neeraj for issue id YRS 5.0-940 
    Dim strFormName As String = New String("RetireesPowerAttorneyWebForm.aspx")
    'End issue id YRS 5.0-940
    Dim drFirst As DataRow
    'Protected WithEvents RequiredFieldValidator1 As System.Web.UI.WebControls.RequiredFieldValidator
    'Protected WithEvents RequiredFieldValidator3 As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents DropDownCountry As System.Web.UI.WebControls.DropDownList
    Protected WithEvents LabelCountry As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxZip As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelZip As System.Web.UI.WebControls.Label
    Protected WithEvents DropDownState As System.Web.UI.WebControls.DropDownList
    Protected WithEvents LabelState As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxCity As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelCity As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxAddress3 As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelAddress3 As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxAddress2 As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelAddtress2 As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxAddress1 As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelAddress1 As System.Web.UI.WebControls.Label
    'Protected WithEvents RequiredFieldValidator2 As System.Web.UI.WebControls.RequiredFieldValidator
    'Protected WithEvents RFVEffDate As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents LabelEffective As System.Web.UI.WebControls.Label
    Protected WithEvents LabelTermination As System.Web.UI.WebControls.Label
    'Start: Bala: YRS-AT-2534 POA Last Name issue 11/20/2015
    'Protected WithEvents TextBoxTermination As YMCAUI.DateUserControl
    Protected WithEvents TextBoxTermination As CustomControls.CalenderTextBox
    'End: Bala: YRS-AT-2534 POA Last Name issue 11/20/2015
    Protected WithEvents PlaceHolder1 As System.Web.UI.WebControls.PlaceHolder
    Dim l_string_PersId As String

#Region " Web Form Designer Generated Code "
    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    'Start: Bala: YRS-AT-2534 POA Last Name issue 11/20/2015
    'Protected WithEvents TextBoxEffective As YMCAUI.DateUserControl
    Protected WithEvents TextBoxEffective As CustomControls.CalenderTextBox
    'End: Bala: YRS-AT-2534 POA Last Name issue 11/20/2015
    Protected WithEvents TextBoxFirstName As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxLastName As System.Web.UI.WebControls.TextBox
    Protected WithEvents ButtonSave As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonCancel As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonAdd1 As System.Web.UI.WebControls.Button
    Protected WithEvents DropDownZip As System.Web.UI.WebControls.DropDownList
    Protected WithEvents DataGridRetireesAttorney As System.Web.UI.WebControls.DataGrid
    Protected WithEvents LabelPowerOfAttorney As System.Web.UI.WebControls.Label
    Protected WithEvents LabelFirstName As System.Web.UI.WebControls.Label
    Protected WithEvents LabelLastNAme As System.Web.UI.WebControls.Label
    Protected WithEvents LabelComments As System.Web.UI.WebControls.Label
    Protected WithEvents TextboxComments As System.Web.UI.WebControls.TextBox
    Protected WithEvents ButtonEdit As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonOk As System.Web.UI.WebControls.Button
    Protected WithEvents AddressWebUserControl1 As YMCAUI.AddressUserControlNew
    Protected WithEvents DropDownPoaCategory As System.Web.UI.WebControls.DropDownList

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
        SetAssociatedControlIDs()
        If Session("LoggedUserKey") Is Nothing Then
            Response.Redirect("Login.aspx", False)
        End If
        Try
            'Start: Bala: YRS-AT-2534 POA Last Name issue 11/18/2015
            'Me.TextBoxEffective.RequiredDate = True
            'End: Bala: YRS-AT-2534 POA Last Name issue 11/18/2015
            l_string_PersId = Session("PersId")
            
            Dim dsPOA As New DataSet
            Dim dsPOAAddr As New DataSet
            'Start:AA: 16.12.2013 - Code refactoring
            SetAttributes()
            If Not IsPostBack Then
                'Start:Anudeep:07.11.2013:BT:2190-YRS 5.0-2199: Added below code to set the controls to read only and hid the save , cancel , add buttons

                Dim strPOARights As String
                strPOARights = Session("POA_Rights")

                If Not strPOARights Is Nothing Then
                    ViewState("POA_Rights") = Session("POA_Rights")
                    If Not strPOARights = "Read-Only" And Not strPOARights = "True" Then
                        MessageBox.Show(PlaceHolder1, "YMCA-YRS", "You have no rights to view.", MessageBoxButtons.Stop, True)
                        Exit Sub
                    ElseIf strPOARights = "Read-Only" Then
                        Setreadonlytocontrols()
                    End If
                    Session("POA_Rights") = Nothing
                End If
                'End:Anudeep:07.11.2013:BT:2190-YRS 5.0-2199: Added below code to set the controls to read only and hid the save , cancel , add buttons
                Session("Datarow") = Nothing
                dsPOA = YMCARET.YmcaBusinessObject.RetireesInformationBOClass.getPOADetails(l_string_PersId)
                Session("dsPOA") = dsPOA
                HelperFunctions.BindGrid(DataGridRetireesAttorney, dsPOA, True)
                InitializePoaCategoryTypesDropDownList()
                If DataGridRetireesAttorney.Items.Count > 0 Then
                    dsPOAAddr = Address.GetAddressByEntity(DataGridRetireesAttorney.Items(0).Cells(2).Text, EnumEntityCode.PERSON)
                End If
                ChangeStatusForControls(False)
                SetAddressControlInitialProperties()
                'END:AA: 16.12.2013 - Code refactoring
            End If
        Catch ex As Exception
            HelperFunctions.LogException(ex.Message, ex)
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message)
        End Try
    End Sub
    Private Sub DataGridRetireesAttorney_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DataGridRetireesAttorney.SelectedIndexChanged
        Try
            Dim dsPOA As New DataSet
            Dim dsPOAAddr As New DataSet
            'BS:2011.07.27:BT:905 - Variable not being used
            'Dim index As Integer
            'Dim i As Integer
            Dim l_button_select As ImageButton
            Dim l_dgitem As DataGridItem
            For Each l_dgitem In Me.DataGridRetireesAttorney.Items
                l_button_select = l_dgitem.FindControl("Imagebutton6")
                If l_dgitem.ItemIndex = Me.DataGridRetireesAttorney.SelectedIndex Then
                    l_button_select.ImageUrl = "images\selected.gif"
                Else
                    l_button_select.ImageUrl = "images\select.gif"
                End If
            Next
            dsPOA = CType(Session("dsPOA"), DataSet)
            'dsPOAAddr = CType(cache("POAAddr"), DataSet)
			'Anudeep:01.06.2013 BT-1501:YRS 5.0-1745:Capture Beneficiary addresses 
            'dsPOAAddr = YMCARET.YmcaBusinessObject.RetireesInformationBOClass.getPOAAddrDtls(DataGridRetireesAttorney.SelectedItem.Cells(2).Text)
            dsPOAAddr = Address.GetAddressByEntity(DataGridRetireesAttorney.SelectedItem.Cells(3).Text, EnumEntityCode.PERSON)
            'index = DataGridRetireesAttorney.SelectedIndex 'BS:2011.07.27:BT:905 - Variable not being used
            If Me.DataGridRetireesAttorney.Items.Count <> 0 Then
                ShowPOADtls(dsPOA, dsPOAAddr, DataGridRetireesAttorney.SelectedItem.Cells(2).Text, DataGridRetireesAttorney.SelectedItem.Cells(3).Text)
				'Anudeep:01.06.2013 BT-1501:YRS 5.0-1745:Capture Beneficiary addresses 
                If Session("Datarow") Is Nothing Then
                    AddressWebUserControl1.LoadAddressDetail(dsPOAAddr.Tables(0).Select("UniqueID='" & DataGridRetireesAttorney.SelectedItem.Cells(2).Text & "'"))
                Else
                    AddressWebUserControl1.LoadAddressDetail(Session("Datarow"))
                End If
            End If
            'BS:2011.07.27:BT:905 - moving textbox and btn control into ChangeStatusForControls()
            'Anudeep:07.11.2013:BT:2190-YRS 5.0-2199: added below control to keep "ok" Button enable
            If ViewState("POA_Rights") Is Nothing OrElse ViewState("POA_Rights") = "True" Then
                ChangeStatusForControls(True)
                AddressWebUserControl1.EnableControls = True
            End If
            Session("blnAddPOA") = False
            'Anudeep:01.06.2013 BT-1501:YRS 5.0-1745:Capture Beneficiary addresses 
            AddressWebUserControl1.guiEntityId = DataGridRetireesAttorney.SelectedItem.Cells(3).Text
            AddressWebUserControl1.IsFromBenificarySettlement = False
            'AddressWebUserControl1.EnableControls = True
            'AddressWebUserControl1.IsPrimary = 1
            AddressWebUserControl1.FromWebAddr = True
            'AddressWebUserControl1.ShowDataForAddress()
        Catch ex As Exception
            HelperFunctions.LogException(ex.Message, ex)
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message)

        End Try
    End Sub

    Private Sub ButtonAdd1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAdd1.Click
        Dim i As Integer
        'Dim l_DataSet_StateNames As DataSet'BS:2011.07.27:BT:905 - Variable not being used
        'Added by Swopna in response to bug id 316,on 28-Dec-2007
        '**************
        Dim l_dgitem As DataGridItem
        Dim l_button_select As ImageButton
        Session("blnAddPOA") = True
        TextBoxEffective.Text = ""
        TextBoxTermination.Text = ""
        TextBoxFirstName.Text = ""
        TextBoxLastName.Text = ""
        TextboxComments.Text = ""
        DropDownPoaCategory.SelectedValue = "-- Select --"
        'BS:2011.07.27:BT:905 - moving textbox and btn control into ChangeStatusForControls()
        ChangeStatusForControls(True)
        'BS:2011.07.27:BT:905 - Variable not being used
        'l_DataSet_StateNames = YMCARET.YmcaBusinessObject.BeneficiarySettlement.LookUp_BS_StateNames(DropDownCountry.SelectedValue.ToString().Trim())
        'Added by Swopna in response to bug id 316,on 28-Dec-2007
        '**************
        For Each l_dgitem In Me.DataGridRetireesAttorney.Items
            l_button_select = CType(l_dgitem.FindControl("Imagebutton6"), ImageButton)
            l_button_select.ImageUrl = "images\select.gif"
        Next
        '**************
        Me.AddressWebUserControl1.EnableControls = True

        Me.AddressWebUserControl1.ClearControls = ""
        'Made changes in adddress user control
        ' Me.AddressWebUserControl1.SetValidationsForAddress()
        ''''''Changed By Rahul on 14 Feb 06
    End Sub
    Private Sub ButtonCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonCancel.Click
        Try
            Dim dsPOA As New DataSet
            Dim dsPOAAddr As New DataSet
            'BS:2011.07.27:BT:905  - Variable not being used
            'Dim index As Integer
            'dsPOA = CType(Cache("POA"), DataSet)
            'dsPOAAddr = CType(Cache("POAAddr"), DataSet)
            dsPOA = DirectCast(Session("dsPOA"), DataSet)
            dsPOAAddr = DirectCast(Session("POAAddr"), DataSet)
            'BS:2011.07.27:BT:905  - Variable not being used
            'index = DataGridRetireesAttorney.SelectedIndex 
            'If Me.DataGridRetireesAttorney.Items.Count > 0 Then
            '    'ShowPOADtls(dsPOA, dsPOAAddr, DataGridRetireesAttorney.Items(0).Cells(2).Text, DataGridRetireesAttorney.Items(0).Cells(3).Text)
            'End If
            DataGridRetireesAttorney.SelectedIndex = -1
            Session("blnAddPOA") = False
            'BS:2011.07.27:BT:905 - moving textbox and btn control into ChangeStatusForControls()
            Me.AddressWebUserControl1.EnableControls = False
            'Removed to display Address address
            'AddressWebUserControl1.ClearControls = ""
            ChangeStatusForControls(False)
        Catch ex As Exception
            HelperFunctions.LogException(ex.Message, ex)
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message)
        End Try
    End Sub

    Private Sub ButtonSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonSave.Click
        Try
			'Anudeep:01.06.2013 BT-1501:YRS 5.0-1745:Capture Beneficiary addresses 
            Dim dsPOAdtls As New DataSet
            Dim dsPOAAddr As New DataSet
            Dim dtPOAAddr As New DataTable
            Dim drPOA As DataRow
            Dim dtPOA As New DataTable
            Dim dsPOA As New DataSet
            'BS:2011.07.27:BT:905  - Variable not being used
            'Dim drRows As DataRow()
            Dim l_str_Pri_Address1 As String
            Dim l_str_Pri_Address2 As String
            Dim l_str_Pri_Address3 As String
            Dim l_str_Pri_City As String
            Dim l_str_Pri_Zip As String
            Dim l_str_Pri_CountryValue As String
            Dim l_str_Pri_StateValue As String
            Dim l_str_Pri_CountryText As String
            Dim l_str_Pri_StateText As String
            Dim l_string_Message As String

            If TextBoxEffective.Text.ToString().Trim() <> "" Then
                If TextBoxTermination.Text.ToString().Trim() <> "" Then
                    Dim l_datediff As Integer
                    l_datediff = DateDiff(DateInterval.Day, Convert.ToDateTime(TextBoxEffective.Text.ToString().Trim()), Convert.ToDateTime(TextBoxTermination.Text.ToString().Trim()))
                    If l_datediff < 0 Then
                        MessageBox.Show(PlaceHolder1, "YMCA", "Termination date cannot be earlier than effective date.", MessageBoxButtons.Stop)
                        Exit Sub
                    End If
                End If
            End If
            'Anudeep:01.06.2013 BT-1501:YRS 5.0-1745:Capture Beneficiary addresses 
            l_string_Message = AddressWebUserControl1.ValidateAddress()
            If l_string_Message <> "" Then
                MessageBox.Show(PlaceHolder1, "YMCA", l_string_Message, MessageBoxButtons.Stop)
                Exit Sub
            End If

            dsPOAdtls = DirectCast(Session("dsPOA"), DataSet)
            dsPOAAddr = DirectCast(Session("POAAddr"), DataSet)

            Me.AddressWebUserControl1.IsPrimary = 1
            Me.AddressWebUserControl1.EntityCode = EnumEntityCode.PERSON.ToString()
            Me.AddressWebUserControl1.AddrCode = "HOME"
            'Anudeep:29.07.2013:Bt-1683-YRS 5.0-1862:Add notes record when user enters address in any module.
            If Not AddressWebUserControl1.Notes Is String.Empty Then
                'Anudeep:07.11.2013-BT:2269-YRS 5.0-2234:Commented below line and added a new line for adding suffix dynamically on which category selected from drop-down when add/ update poa address.
                'Me.AddressWebUserControl1.Notes = "POA " + TextBoxFirstName.Text + " " + TextBoxLastName.Text + " " + AddressWebUserControl1.Notes
                Me.AddressWebUserControl1.Notes = DropDownPoaCategory.SelectedItem.Text + " " + TextBoxFirstName.Text.Trim + " " + TextBoxLastName.Text.Trim + " " + AddressWebUserControl1.Notes 'AA:11.06.2014 BT:2554 - YRS 5.0-2375 - Added to trim the name while adding to the notes
            End If
            'l_str_Pri_Address1 = AddressWebUserControl1.Address1
            'l_str_Pri_Address2 = AddressWebUserControl1.Address2
            'l_str_Pri_Address3 = AddressWebUserControl1.Address3
            'l_str_Pri_City = AddressWebUserControl1.City
            'l_str_Pri_Zip = AddressWebUserControl1.ZipCode
            'l_str_Pri_CountryValue = AddressWebUserControl1.DropDownListCountryValue
            'If l_str_Pri_CountryValue = "-Select Country-" Then
            '    l_str_Pri_CountryValue = ""
            'End If
            'l_str_Pri_StateValue = AddressWebUserControl1.DropDownListStateValue
            'If l_str_Pri_StateValue = "-Select State-" Then
            '    l_str_Pri_StateValue = ""
            'End If
            'l_str_Pri_CountryText = AddressWebUserControl1.DropDownListCountryText
            'l_str_Pri_StateText = AddressWebUserControl1.DropDownListStateText

            dtPOAAddr = AddressWebUserControl1.GetAddressTable()

            'If Trim(l_str_Pri_Address1) = "" Then
            '    MessageBox.Show(PlaceHolder1, "YMCA", "Please enter Address1.", MessageBoxButtons.Stop)
            '    Exit Sub
            'End If

            'If Trim(l_str_Pri_City) = "" Then
            '    MessageBox.Show(PlaceHolder1, "YMCA", "Please enter City.", MessageBoxButtons.Stop)
            '    Exit Sub
            'End If

            'If Trim(l_str_Pri_Address1) <> "" Then
            '    If l_str_Pri_CountryText = "-Select Country-" And l_str_Pri_StateText = "-Select State-" Then
            '        MessageBox.Show(PlaceHolder1, "YMCA", "Please select either State or Country.", MessageBoxButtons.Stop)
            '        Exit Sub
            '    End If
            'End If

            'If (l_str_Pri_CountryText = "-Select Country-") Then
            '    If l_str_Pri_StateText = "-Select State-" Then
            '        MessageBox.Show(PlaceHolder1, "YMCA", "Please select either State or Country.", MessageBoxButtons.Stop)
            '        Exit Sub
            '    End If
            'End If

            'l_string_Message = ValidateCountrySelStateZip(l_str_Pri_CountryValue, l_str_Pri_StateValue, l_str_Pri_Zip)
            'If l_string_Message <> "" Then
            '    MessageBox.Show(PlaceHolder1, "YMCA", l_string_Message, MessageBoxButtons.Stop)
            '    Exit Sub
            'End If

            'If l_str_Pri_CountryValue = "CA" And (System.Text.RegularExpressions.Regex.IsMatch(l_str_Pri_Zip, "^([a-zA-Z])([0-9])([a-zA-Z]) ([0-9])([a-zA-Z])([0-9])")) = False Then
            '    TextBoxZip.Text = ""
            '    MessageBox.Show(PlaceHolder1, "YMCA", "Invalid Zip Code format For Country Canada", MessageBoxButtons.Stop)
            '    Exit Sub
            'End If
            dtPOA.Columns.Add("PersId")
            dtPOA.Columns.Add("EffectiveDate")
            dtPOA.Columns.Add("TerminationDate")
            dtPOA.Columns.Add("Name1")
            dtPOA.Columns.Add("Name2")
            dtPOA.Columns.Add("Comments")
            dtPOA.Columns.Add("POACategory")
            dtPOA.Columns.Add("POAUniqueID")
            'AA:14.10.2013:BT:2236-Added addrs UniqueId to populate the addrs uniqueid if addrs has not modified also
            dtPOA.Columns.Add("AddrsUniqueID")
            If Session("blnAddPOA") = True Then
                drPOA = dsPOAdtls.Tables(0).NewRow
                drPOA("dtmeffdate") = TextBoxEffective.Text
                drPOA("dtmterminationdate") = IIf(TextBoxTermination.Text = "", System.DBNull.Value, TextBoxTermination.Text)
                drPOA("chvpoaname1") = TextBoxFirstName.Text.Trim ' AA:11.06.2014 BT:2554 - YRS 5.0-2375 - Added to trim the name while adding to the notes
                drPOA("chvpoaname2") = TextBoxLastName.Text.Trim ' AA:11.06.2014 BT:2554 - YRS 5.0-2375 - Added to trim the name while adding to the notes
                drPOA("txtComments") = TextboxComments.Text
                drPOA("chvPoaCategory") = DropDownPoaCategory.Text
                'drPOAAddr("chvcountry") = DropDownCountry.SelectedValue
                dsPOAdtls.Tables(0).Rows.Add(drPOA)
                drPOA = dtPOA.NewRow
                drPOA("PersId") = Session("PersId")
                drPOA("EffectiveDate") = TextBoxEffective.Text
                drPOA("TerminationDate") = IIf(TextBoxTermination.Text = "", System.DBNull.Value, TextBoxTermination.Text)
                drPOA("Name1") = TextBoxFirstName.Text.Trim ' AA:11.06.2014 BT:2554 - YRS 5.0-2375 - Added to trim the name while adding to the notes
                drPOA("Name2") = TextBoxLastName.Text.Trim ' AA:11.06.2014 BT:2554 - YRS 5.0-2375 - Added to trim the name while adding to the notes
                drPOA("Comments") = TextboxComments.Text
                drPOA("POACategory") = DropDownPoaCategory.Text
                dtPOA.Rows.Add(drPOA)
                dsPOA.Tables.Add(dtPOA)
                dsPOA.Tables.Add(dtPOAAddr)
				'Anudeep:01.06.2013 BT-1501:YRS 5.0-1745:Capture Beneficiary addresses 
                'Session("POAName") = TextBoxFirstName.Text  'BS:2011.07.27 - Variable not being used
                'BS:2011.07.27:BT:905 -  'BS:2011.07.27:BT:905 - moving textbox and btn control into ChangeStatusForControls() and call this method after end of this block
                'Me.TextboxComments.Enabled = True
                'Me.TextBoxEffective.Enabled = True
                'Me.TextBoxFirstName.Enabled = True
                'Me.TextBoxLastName.Enabled = True
                'Me.TextBoxTermination.Enabled = True
                'Me.ButtonSave.Enabled = False
                'Me.ButtonCancel.Enabled = False
                'YMCARET.YmcaBusinessObject.RetireesInformationBOClass.InsertPOADetails(Session("PersId"), TextBoxEffective.Text, TextBoxTermination.Text, TextBoxFirstName.Text, TextBoxLastName.Text, TextboxComments.Text, l_str_Pri_Address1, l_str_Pri_Address2, l_str_Pri_Address3, l_str_Pri_City, l_str_Pri_CountryValue, l_str_Pri_StateValue, Replace(l_str_Pri_Zip, " ", ""), DropDownPoaCategory.SelectedValue)
				'Anudeep:01.06.2013 BT-1501:YRS 5.0-1745:Capture Beneficiary addresses 
                YMCARET.YmcaBusinessObject.RetireesInformationBOClass.InsertPOA(dsPOA)
                'dsPOA = YMCARET.YmcaBusinessObject.RetireesInformationBOClass.getPOADetails(l_string_PersId)  'BS:2011.07.27 - moving this kind of code outside this statement.
                Session("blnAddPOA") = False
            Else
				'Anudeep:01.06.2013 BT-1501:YRS 5.0-1745:Capture Beneficiary addresses 
                drPOA = dtPOA.NewRow
                drPOA("PersId") = Session("PersId")
                drPOA("EffectiveDate") = TextBoxEffective.Text
                drPOA("TerminationDate") = IIf(TextBoxTermination.Text = "", System.DBNull.Value, TextBoxTermination.Text)
                drPOA("Name1") = TextBoxFirstName.Text.Trim ' AA:11.06.2014 BT:2554 - YRS 5.0-2375 - Added to trim the name while adding to the notes
                drPOA("Name2") = TextBoxLastName.Text.Trim ' AA:11.06.2014 BT:2554 - YRS 5.0-2375 - Added to trim the name while adding to the notes
                drPOA("Comments") = TextboxComments.Text
                drPOA("POACategory") = DropDownPoaCategory.Text
                drPOA("POAUniqueID") = DataGridRetireesAttorney.SelectedItem.Cells(3).Text
                'AA:14.10.2013:BT:2236-Added addrs UniqueId to populate the addrs uniqueid if addrs has not modified also
                drPOA("AddrsUniqueID") = AddressWebUserControl1.UniqueId
                dtPOA.Rows.Add(drPOA)
                dsPOA.Tables.Add(dtPOA)
                dsPOA.Tables.Add(dtPOAAddr)
                'YMCARET.YmcaBusinessObject.RetireesInformationBOClass.UpdatePOADetails(Session("PersId"), TextBoxEffective.Text, TextBoxTermination.Text, TextBoxFirstName.Text, TextBoxLastName.Text, TextboxComments.Text, l_str_Pri_Address1, l_str_Pri_Address2, l_str_Pri_Address3, l_str_Pri_City, l_str_Pri_CountryValue, l_str_Pri_StateValue, Replace(l_str_Pri_Zip, " ", ""), DataGridRetireesAttorney.SelectedItem.Cells(3).Text, DropDownPoaCategory.SelectedValue)

                YMCARET.YmcaBusinessObject.RetireesInformationBOClass.UpdatePOA(dsPOA)

                'drRows = dsPOA.Tables(0).Select("guiuniqueid='" & DataGridRetireesAttorney.SelectedItem.Cells(2).Text & "'")

                'drRows(0)("dtmeffdate") = TextBoxEffective.Text
                'drRows(0)("dtmterminationdate") = TextBoxTermination.Text
                'drRows(0)("chvpoaname1") = TextBoxFirstName.Text
                'drRows(0)("chvpoaname2") = TextBoxLastName.Text
                'drRows(0)("txtComments") = TextboxComments.Text
            End If
            'dsPOA.AcceptChanges()
			'Anudeep:01.06.2013 BT-1501:YRS 5.0-1745:Capture Beneficiary addresses 
            dsPOAdtls = YMCARET.YmcaBusinessObject.RetireesInformationBOClass.getPOADetails(l_string_PersId)
            'BS:2011.07.27:BT:905 - Assign dsPOA to Session
            Session("dsPOA") = dsPOAdtls
            'End
            DataGridRetireesAttorney.DataSource = dsPOAdtls
            DataGridRetireesAttorney.DataBind()
            'BS:2011.07.27:BT:905 - moving textbox and btn control into ChangeStatusForControls()
            ChangeStatusForControls(False)
            AddressWebUserControl1.EnableControls = False
            'AddressWebUserControl1.ClearControls = ""
            'Start : Bala: YRS-AT-2534: clearing the address on the click of save button 11/20/2015
            AddressWebUserControl1.ClearControls = ""
            'End: Bala: YRS-AT-2534: clearing the address on the click of save button 11/20/2015
        Catch ex As Exception
            HelperFunctions.LogException(ex.Message, ex)
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message)
        Finally
            'Session("blnAddPOA") = False
        End Try
    End Sub

    Public Sub ShowPOADtls(ByVal dsPOA As DataSet, ByVal dsPOAAddr As DataSet, ByVal guiAddruniqueId As String, ByVal uniqueid As String)
        Try
            Dim drRowsPOA As DataRow()
            Dim drRowsPOAAdr As DataRow()
            drRowsPOA = dsPOA.Tables(0).Select("guiUniqueID='" & uniqueid & "'")
            'drRowsPOAAdr = dsPOAAddr.Tables(0).Select("guiEntityID='" & uniqueid & "'")
            drRowsPOAAdr = dsPOAAddr.Tables(0).Select("UniqueID='" & guiAddruniqueId & "'")
            If drRowsPOA.Length <= 0 Then Exit Sub
            drFirst = drRowsPOA(0)
            TextBoxEffective.Text = drFirst.Item("dtmeffdate")
            TextBoxTermination.Text = IIf(IsDBNull(drFirst.Item("dtmterminationdate")), "", drFirst.Item("dtmterminationdate"))
            TextBoxFirstName.Text = drFirst.Item("chvpoaname1")
            TextBoxLastName.Text = drFirst.Item("chvpoaname2")

            If Not drFirst.Item("chvpoacategory") Is Nothing Then
                If Not String.IsNullOrEmpty(drFirst.Item("chvpoacategory")) Then
                    DropDownPoaCategory.SelectedValue = drFirst.Item("chvpoacategory")
                Else
                    DropDownPoaCategory.SelectedValue = "-- Select --"
                End If
           
            End If
			'Anudeep:01.06.2013 BT-1501:YRS 5.0-1745:Capture Beneficiary addresses 
            If Not drRowsPOAAdr Is Nothing Then
                If drRowsPOAAdr.Length > 0 Then
                    'BS:2011.07.27:BT:905  - Variable not being used
                    'If drFirstAddr Is Nothing Then
                    'drFirstAddr = dsPOAAddr.Tables(0).Rows(index)
                    'drFirstAddr = drRowsPOAAdr(0)
                    Session("Datarow") = drRowsPOAAdr 'drFirstAddr
                    'End If
                Else
                    'BS:2011.07.27:BT:905 - Clear Session
                    Session("Datarow") = Nothing
                End If
            Else
                'BS:2011.07.27:BT:905 - Clear Session
                Session("Datarow") = Nothing
            End If

            TextboxComments.Text = IIf(IsDBNull(drFirst.Item("txtcomments")), "", drFirst.Item("txtcomments"))
        Catch ex As Exception
            HelperFunctions.LogException(ex.Message, ex)
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message)
        End Try
    End Sub
    Private Sub ButtonOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonOk.Click
        'Session("POAName") = TextBoxFirstName.Text  'BS:2011.07.27 - Variable not being used
        Dim msg As String
        msg = msg + "<Script Language='JavaScript'>"
        'Vipul 25Nov05 This code was forcing the parent form Load event to Fire & therby the parent form screen control inputs were lost. Hence Fixed.
        msg = msg + "window.opener.document.forms(0).submit();"
        'msg = msg + "window.opener.location.href=window.opener.location.href;"
        msg = msg + "self.close();"
        msg = msg + "</Script>"
        Response.Write(msg)
    End Sub
    Public Sub ChangeStatusForControls(ByVal blnEnabled As Boolean)
        'Start: Bala: YRS-AT-2534 POA Last Name issue 11/18/2015
        If blnEnabled = False Then
            TextBoxEffective.Text = ""
            TextBoxTermination.Text = ""
            TextBoxFirstName.Text = ""
            TextBoxLastName.Text = ""
            TextboxComments.Text = ""
            DropDownPoaCategory.SelectedValue = "-- Select --"
            AddressWebUserControl1.ClearControls = ""
        End If
        'End: Bala: YRS-AT-2534 POA Last Name issue 11/18/2015
        ButtonSave.Enabled = blnEnabled
        ButtonCancel.Enabled = blnEnabled
        'BS:2011.07.27:BT:905 - move textboxs and button ctrl here from events and method
        TextBoxEffective.Enabled = blnEnabled
        TextBoxTermination.Enabled = blnEnabled
        TextBoxFirstName.Enabled = blnEnabled
        TextBoxLastName.Enabled = blnEnabled
        TextboxComments.Enabled = blnEnabled
        ButtonAdd1.Enabled = Not blnEnabled
        ButtonOk.Enabled = Not blnEnabled
        DropDownPoaCategory.Enabled = blnEnabled
    End Sub
    ''Private Sub DropDownState_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DropDownState.SelectedIndexChanged
    ''    Try
    ''        If Me.DropDownState.SelectedItem.Text <> "" Then
    ''            Me.DropDownCountry.SelectedIndex = -1
    ''            Me.DropDownCountry.SelectedValue = "US"
    ''            Me.DropDownCountry.Enabled = False
    ''        Else
    ''            Me.DropDownCountry.SelectedIndex = -1
    ''            Me.DropDownCountry.Items.FindByText("").Selected = True
    ''            Me.DropDownCountry.Enabled = True
    ''        End If
    ''    Catch ex As Exception
    ''        Dim l_String_Exception_Message As String
    ''        l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
    ''        Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message)
    ''    End Try
    ''End Sub
    Private Sub ButtonAdd1_Command(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs) Handles ButtonAdd1.Command
    End Sub
    Private Function ValidateCountrySelStateZip(ByVal CountryValue As String, ByVal StateValue As String, ByVal str_Pri_Zip As String) As String
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
        Dim l_Str_Msg As String = ""
        Try
            If (CountryValue = "US" Or CountryValue = "CA") And StateValue = "" Then
                l_Str_Msg = "Please select state"
            ElseIf (CountryValue = "US" Or CountryValue = "CA") And str_Pri_Zip = "" Then
                l_Str_Msg = "Please enter Zip Code"
            ElseIf CountryValue = "US" And (Len(str_Pri_Zip) <> 5 And Len(str_Pri_Zip) <> 9) Then
                l_Str_Msg = "Invalid Zip Code format"
            ElseIf CountryValue = "CA" And (Len(str_Pri_Zip) <> 7) Then
                l_Str_Msg = "Invalid Zip Code format"
            End If
            If CountryValue = "US" And l_Str_Msg <> "" Then
                l_Str_Msg = l_Str_Msg & " for United States"
            ElseIf CountryValue = "CA" And l_Str_Msg <> "" Then
                l_Str_Msg = l_Str_Msg & " for Canada"
            End If
            ValidateCountrySelStateZip = l_Str_Msg
        Catch ex As Exception
            HelperFunctions.LogException(ex.Message, ex)
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
        End Try
    End Function
    Private Sub ButtonSave_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonSave.Load
    End Sub

    Private Sub InitializePoaCategoryTypesDropDownList()
        'added by hafiz on 2-May-2007 for YREN-3112
        'Start : added by Dilip yadav on 2009.09.08 for YRS 5.0-852
        Dim l_dataset_PoaCategoryTypes As DataSet
        l_dataset_PoaCategoryTypes = YMCARET.YmcaBusinessObject.RetireesInformationBOClass.POACategoryTypes()
        If HelperFunctions.isNonEmpty(l_dataset_PoaCategoryTypes) Then
            Me.DropDownPoaCategory.DataSource = l_dataset_PoaCategoryTypes.Tables(0)
            Me.DropDownPoaCategory.DataTextField = "Description"
            Me.DropDownPoaCategory.DataValueField = "Code"
            Me.DropDownPoaCategory.DataBind()
            Me.DropDownPoaCategory.Items.Insert(0, "-- Select --")
        End If
        'End :  by Dilip yadav on 2009.09.08 for YRS 5.0-852
    End Sub
    'Start:AA: 16.12.2013 - Code refactoring
    Public Sub Setreadonlytocontrols()
        AddressWebUserControl1.EnableControls = False
        'TextBoxEffective.rReadOnly = True
        'Start: Bala: YRS-AT-2534 POA Last Name issue 11/20/2015
        TextBoxEffective.ReadOnly = True
        'End: Bala: YRS-AT-2534 POA Last Name issue 11/20/2015
        TextboxComments.ReadOnly = True
        TextBoxFirstName.ReadOnly = True
        TextBoxLastName.ReadOnly = True
        'TextBoxTermination.rReadOnly = True
        'Start: Bala: YRS-AT-2534 POA Last Name issue 11/20/2015
        TextBoxTermination.ReadOnly = True
        'End: Bala: YRS-AT-2534 POA Last Name issue 11/20/2015
        DropDownPoaCategory.Enabled = False
        ButtonSave.Visible = False
        ButtonCancel.Visible = False
        ButtonAdd1.Visible = False
    End Sub
    Private Sub SetAssociatedControlIDs()
        Me.LabelFirstName.AssociatedControlID = Me.TextBoxFirstName.ID
        Me.LabelLastNAme.AssociatedControlID = Me.TextBoxLastName.ID
        Me.LabelComments.AssociatedControlID = Me.TextboxComments.ID
    End Sub
    Private Sub SetAttributes()
        TextBoxEffective.Attributes.Add("onchange", "funcOnChangeText();")
        TextBoxTermination.Attributes.Add("onchange", "funcOnChangeText();")
        TextBoxFirstName.Attributes.Add("onchange", "funcOnChangeText();")
        TextBoxLastName.Attributes.Add("onchange", "funcOnChangeText();")
        TextboxComments.Attributes.Add("onchange", "funcOnChangeText();")
    End Sub
    Private Sub SetAddressControlInitialProperties()
        Me.AddressWebUserControl1.IsPrimary = 1
        Me.AddressWebUserControl1.FromWebAddr = True
        Me.AddressWebUserControl1.EnableControls = False
        Me.AddressWebUserControl1.LoadAddressDetail(Nothing)
    End Sub
    'ENd:AA: 16.12.2013 - Code refactoring
End Class

' Cache-Session         : Hafiz 04Feb06
'Hafiz 04Feb06 Cache-Session
'Imports Microsoft.Practices.EnterpriseLibrary.Caching
'Hafiz 04Feb06 Cache-Session
'*********************************************************************************************************************
'Modification History
'*********************************************************************************************************************
'Modified By        Date            Description
'*********************************************************************************************************************
'Ashutosh Patil   02-Apr-2007       YREN-3256 
'Ashutosh Patil   25-May-2007       Modifications related to IE7 MessageBox Issue, Dropdownlist issue and changes in
'                                   validations related to Additional Amount TextBox.    
'Neeraj Singh     12/Nov/2009       Added form name for security issue YRS 5.0-940 
'Imran            2010.06.21        Enhancement changes(CType to DirectCast)
'Shashi Shekhar   27-Dec-2010       For YRS 5.0-450, BT-643 : Replace SSN with Fund Id in title on all screens.
'Shashi           04 Mar. 2011      Replacing Header formating with user control (YRS 5.0-450 )
'Anudeep A        2012-09-22        BT-1126/Additional change YRS 5.0-1246 : : Handling DLIN records
'Manthan Rajguru  2015.09.16        YRS-AT-2550: YRS data cleanup: copyright comments and namespace/reference (Changed the Namespace/reference from 'Infotech' to 'YMCARET')
'Pooja Kumkar     2019.10.01        YRS-AT-4606 -  YRS enh:State Withholding Project - Update "General Withholding" tab under Annuity purchase screen
'                                   YRS-AT-4607 -  YRS enh:State Withholding Project - Update "Other W/h tab" tab under Retiree screen 
'*********************************************************************************************************************
Public Class UpdateGenHoldings
    Inherits System.Web.UI.Page
    'below line is Added by Neeraj for issue id YRS 5.0-940 
    Dim strFormName As String = New String("UpdateGenHoldings.aspx")
    'End issue id YRS 5.0-940

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents lblWithHolding As System.Web.UI.WebControls.Label
    Protected WithEvents lblAddAmount As System.Web.UI.WebControls.Label
    Protected WithEvents lblEndDate As System.Web.UI.WebControls.Label
    Protected WithEvents lblStartDate As System.Web.UI.WebControls.Label
    Protected WithEvents PopcalendarRecDate As RJS.Web.WebControl.PopCalendar
    Protected WithEvents txtEndDate As YMCAUI.DateUserControl
    Protected WithEvents txtStartDate As YMCAUI.DateUserControl
    Protected WithEvents Popcalendar1 As RJS.Web.WebControl.PopCalendar
    Protected WithEvents cmbWithHoldingType As System.Web.UI.WebControls.DropDownList
    Protected WithEvents txtAddAmount As System.Web.UI.WebControls.TextBox
    Protected WithEvents ButtonOK As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonCancel As System.Web.UI.WebControls.Button
    'Protected WithEvents RequiredFieldValidator4 As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents PlaceHolderUpadteGenWithold As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents RequiredFieldValidator1 As System.Web.UI.WebControls.RequiredFieldValidator
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
        If Session("LoggedUserKey") Is Nothing Then
            Response.Redirect("Login.aspx", False)
        End If
        Me.txtStartDate.RequiredDate = True
        'Me.txtStartDate.Text = Convert.ToDateTime(Now).ToString("MM/dd/yyyy")
        '------------------------------------------------------------------
        'Shashi: 04 Mar. 2011: Replacing Header formating with user control (YRS 5.0-450 )
        If Not Session("FundNo") Is Nothing Then
            Headercontrol.PageTitle = "Retiree Information - Add/Update General Withholding"
            Headercontrol.FundNo = Session("FundNo").ToString().Trim()
        End If
        '-------------------------------------------------------------------
        'Commented By Ashutosh Patil as on 25-May-2007
        'For avoiding runtime javascript error "; is missing" 
        'Me.ButtonCancel.Attributes.Add("onclick", "'javascript:Cancel_OnClick()'")

        Try
            If Not IsPostBack Then
                Me.txtStartDate.Text = Convert.ToDateTime(Now).ToString("MM/dd/yyyy")
                Dim l_string_PersId As String
                l_string_PersId = Session("PersId")
                Dim l_dataset_GenWith As New DataSet
                Dim l_dataset_GenCodes As New DataSet
                'START |PK |2019.10.01 | YRS-AT-4606/YRS-AT-4607 | variables declared 
                Dim dt_WithHoldCodes As DataTable
                Dim drResult As DataRow()
                'END |PK |2019.10.01 | YRS-AT-4606/YRS-AT-4607| variables declared
                l_dataset_GenWith = YMCARET.YmcaBusinessObject.RetireesInformationBOClass.LookUpGenWithDrawals(l_string_PersId)
                l_dataset_GenCodes = YMCARET.YmcaBusinessObject.RetireesInformationBOClass.getGenCodes()

                'Modified By Ashutosh Patil as on 25-May-2007
                'For avoiding javascript alret. Validation on KeyPress
                'Start Ashutosh Patil
                'Me.txtAddAmount.Attributes.Add("OnBlur", "_OnBlur_TextBoxAmount();")
                Me.txtAddAmount.Attributes.Add("OnKeyPress", "javascript:ValidateNumeric();")
                Me.txtAddAmount.Attributes.Add("onkeypress", "Javascript:return HandleAmountFiltering(this);")
                Me.txtAddAmount.Attributes.Add("onchange", "javascript:FormatAmtControl(this);")
                Me.txtAddAmount.Attributes.Add("OnPaste", "javascript:ValidateNumeric();")
                'End Ashutosh Patil
                'ButtonOK.Attributes.Add("onClick", "funcOnChangeText();")

                'START |PK |2019.10.01 | YRS-AT-4606/YRS-AT-4607 | Code to show only bitActive =true rows in dropdown
                'cmbWithHoldingType.DataSource = l_dataset_GenCodes
                If (HelperFunctions.isNonEmpty(l_dataset_GenCodes)) Then
                    drResult = l_dataset_GenCodes.Tables("WithHoldCodes").Select("Active = 'True'")
                    If drResult.Count > 0 Then
                        dt_WithHoldCodes = drResult.CopyToDataTable()
                    End If
                End If
                cmbWithHoldingType.DataSource = dt_WithHoldCodes  'Binding specific rows in datatable
                'END |PK |2019.10.01 | YRS-AT-4606/YRS-AT-4607 | Code to show only bitActive =true rows in dropdown
                cmbWithHoldingType.DataBind()
                'If l_dataset_GenWith.Tables(0).Rows.Count > 0 Then
                '    txtAddAmount.Text = IIf(IsDBNull(l_dataset_GenWith.Tables(0).Rows(0).Item("Add'l Amount")), "", l_dataset_GenWith.Tables(0).Rows(0).Item("Add'l Amount"))
                '    txtStartDate.Text = IIf(IsDBNull(l_dataset_GenWith.Tables(0).Rows(0).Item("Start Date")), "", l_dataset_GenWith.Tables(0).Rows(0).Item("Start Date"))
                '    txtEndDate.Text = IIf(IsDBNull(l_dataset_GenWith.Tables(0).Rows(0).Item("End Date")), "", l_dataset_GenWith.Tables(0).Rows(0).Item("End Date"))
                'End If

                If Not Request.QueryString("UniqueID") Is Nothing Or Not Request.QueryString("Index") Is Nothing Then
                    txtAddAmount.Text = Session("txtAddAmount")
                    txtStartDate.Text = Session("txtStartDate")
                    If CType(Session("txtEndDate"), String).Trim = "&nbsp;" Then
                        txtEndDate.Text = ""
                    Else
                        txtEndDate.Text = Session("txtEndDate")
                    End If

                    cmbWithHoldingType.SelectedValue = Session("cmbWithHoldingType")
                End If
            End If

        Catch ex As Exception
            Throw

        End Try
    End Sub

    Private Sub ButtonOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonOK.Click
        Try
            Dim l_bool As Boolean = False

            If Request.QueryString("UniqueID") Is Nothing And Request.QueryString("Index") Is Nothing Then
                Session("blnAddGenWithHoldings") = True
            End If

            'End Add
            Dim msg As String

            Dim GenWithDrawals As New DataSet
            Dim drRows() As DataRow

            Dim drUpdated As DataRow

            'Hafiz 04Feb06 Cache-Session
            'Dim l_CacheManager As CacheManager
            'l_CacheManager = CacheFactory.GetCacheManager
            'Hafiz 04Feb06 Cache-Session

            Session("cmbWithHoldingType") = cmbWithHoldingType.SelectedValue
            Session("txtAddAmount") = txtAddAmount.Text
            Session("txtStartDate") = txtStartDate.Text

            Session("txtEndDate") = txtEndDate.Text
            If txtEndDate.Text.ToString.Trim <> "" And txtEndDate.Text.ToString.Trim <> "&nbsp;" Then
                If Convert.ToDateTime(Me.txtEndDate.Text) < Convert.ToDateTime(Me.txtStartDate.Text) Then
                    'MessageBox.Show(200, 130, PlaceHolderUpadteGenWithold, "YMCA-YRS", "The End Date has to be greater than the Start Date.", MessageBoxButtons.OK, True)
                    'Commented for BT-1126
                    'MessageBox.Show(PlaceHolderUpadteGenWithold, "YMCA-YRS", "The End Date has to be greater than the Start Date.", MessageBoxButtons.OK)
                    MessageBox.Show(PlaceHolderUpadteGenWithold, "YMCA-YRS", Resources.withholding.MESSAGE_GENERAL_WITHOLDING_ENDDATE_GREATERTHAN_STARTDATE, MessageBoxButtons.OK)
                    l_bool = True
                End If
            End If


            If l_bool = False Then
                If Not Request.QueryString("UniqueID") Is Nothing Then

                    'Hafiz 04Feb06 Cache-Session
                    'GenWithDrawals = CType(l_CacheManager.GetData("GenWithDrawals"), DataSet)
                    GenWithDrawals = DirectCast(Session("GenWithDrawals"), DataSet)
                    'Hafiz 04Feb06 Cache-Session

                    If Not IsNothing(GenWithDrawals) Then
                        drRows = GenWithDrawals.Tables(0).Select("GenWithdrawalID='" & Request.QueryString("UniqueID") & "'")
                        drUpdated = drRows(0)
                        drUpdated("Type") = cmbWithHoldingType.SelectedValue
                        drUpdated("Add'l Amount") = txtAddAmount.Text
                        drUpdated("Start Date") = txtStartDate.Text
                        drUpdated("End Date") = txtEndDate.Text

                        Session("EnableSaveCancel") = True
                        Session("blnUpdateGenWithDrawals") = True
                        'Hafiz 04Feb06 Cache-Session
                        'l_CacheManager.Add("GenWithDrawals", GenWithDrawals)
                        Session("GenWithDrawals") = GenWithDrawals
                        'Hafiz 04Feb06 Cache-Session
                    End If
                End If

                If Not Request.QueryString("Index") Is Nothing Then

                    'Hafiz 04Feb06 Cache-Session
                    'GenWithDrawals = CType(l_CacheManager.GetData("GenWithDrawals"), DataSet)
                    GenWithDrawals = DirectCast(Session("GenWithDrawals"), DataSet)
                    'Hafiz 04Feb06 Cache-Session

                    If Not IsNothing(GenWithDrawals) Then

                        drUpdated = GenWithDrawals.Tables(0).Rows(Request.QueryString("Index"))

                        drUpdated("Type") = cmbWithHoldingType.SelectedValue
                        drUpdated("Add'l Amount") = txtAddAmount.Text
                        drUpdated("Start Date") = txtStartDate.Text
                        drUpdated("End Date") = txtEndDate.Text

                        Session("EnableSaveCancel") = True
                        Session("blnUpdateGenWithDrawals") = True
                        'Hafiz 04Feb06 Cache-Session
                        'l_CacheManager.Add("GenWithDrawals", GenWithDrawals)
                        Session("GenWithDrawals") = GenWithDrawals
                        'Hafiz 04Feb06 Cache-Session
                    End If
                End If

                'Session("Flag") = "Called"
                Session("Flag") = ""

                msg = msg + "<Script Language='JavaScript'>"


                ''commented By Vartika
                ''msg = msg + "window.opener.location.href=window.opener.location.href;"

                ''added By Vartika
                msg = msg + "window.opener.document.forms(0).submit();"

                msg = msg + "self.close();"

                msg = msg + "</Script>"

                Response.Write(msg)
            End If

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub ButtonCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonCancel.Click
        Dim msg As String
        Session("blnUpdateGenWithDrawals") = True

        Session("blnAddGenWithHoldings") = False

        msg = msg + "<Script Language='JavaScript'>"

        msg = msg + "self.close();"

        msg = msg + "</Script>"

        Response.Write(msg)
    End Sub
End Class

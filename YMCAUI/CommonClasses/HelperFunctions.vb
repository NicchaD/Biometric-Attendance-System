'*******************************************************************************
' Copyright YMCA Retirement Fund All Rights Reserved. 
'
' Project Name		:	YMCA-YRS
' FileName			:	HelperFunctions.vb
' Author Name		:	Priya Jawale    
' Employee ID		:	33488
' Email				:	Priya.jawale@3i-infotech.com
' Contact No		:	8642
' Creation Time		:	20-April-2009
' Program Specification Name	: YMCA_PS_Maintenance_YMCA
'*******************************************************************************
'Modification History
'************************************************************************************
'Modified By        Date            Description
'*********************************************************************************************************************
'NP/PP/SR           2009.05.18      Optimizing the YMCA Screen
'Shashi Singh        2009.10.06      Adding code to provide ability to parse a CSV file into a Data table
'Shashi Singh        2009.10.10      Adding Regular expression which handle empty value in CSV File
'Shashi Singh        2009.12.23      define global variable
'Ashish Srivastava  2010.03.22      Added exception log method
'Nikunj Patel		2010.06.09		Added method that sets the image of the selected item in a datagrid, also added code that helps bind data to a datagrid
'Shashi singh		2011.02.25		Added method to validate guid format through reg expression.
'Nikunj Patel		2011.08.02		Added method to Log generic exception message.
'BhavnaS            2012.01.27      Added method to sanitize values for javascript
'Shashank Patel     2013.09.25		BT:618/YRS 5.0-842 : Need ability to pay one person on a transmittal
'Sanjay R.          2013.10.15		YRS 5.0-1328 : Need new Void and Transfer annuity check process
'Anudeep            2013-12-16      BT:2311-13.3.0 Observations
'Anudeep            25.03.2014      BT:957: YRS 5.0-1484 -Termination watcher changes Re-Work (YRS 5.0-1484)
'Shashank           18.08.2014      BT-2344\YRS 5.0-2279 : Add in Administration Screen ability to change messages for YRS or Web site
'Anudeep A          26.08.2014      BT:2625 :YRS 5.0-2405-Consistent screen header sections
'Anudeep A          2015.02.10      BT:2738:YRS 5.0-2456:YERDI3I-2319: YERDI YMCA Officer Information Update function - YRS Changes
'B.Jagadeesh        2015.06.01      BT:2882: Remove all warning message from YRS project.
'Manthan Rajguru    2015.09.16      YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
'Anudeep A          2016.03.21      YRS-AT-2594 - YRS enh: Utility for Unearned Interest Transmittals (UEIN) emails (review/select//send
'Chandra sekar      2016.08.22      YRS-AT-3081 - YRS Enh: request to QDRO function (Retired ) to allow different amt or percent (TrackIT 23098)
'Pramod P. Pokale   2017.01.23      YRS-AT-3299 - YRS enh:improve usability of QDRO split screens(Retired) (TrackIT 28050) 
'Manthan Rajguru    2017.09.19      YRS-AT-3665 - YRS enh: Data Corrections Tool - Admin screen option to create a manual credit 
'Sanjay Singh       2017.11.27      YRS-AT-3742 - YRS Bug:(HOT FIX NEEDED) -participants with RMDs more than non-taxable amount are being forced to take excessive amt (TrackIT 31764). 
'Sanjay GS Rawat    2018.04.10      YRS-AT-3101 - YRS enh: EFT: Loans ( FIRST EFT PROJECT) - updates for "Payment Manager" (TrackIT 33024) 
'Chandra sekar      2018.05.23      YRS-AT-3270 - YRS enh-email notifications for updates made Contacts tab (TrackIT 27727)
'Pramod P. Pokale   2018.10.03      YRS-AT-4017 - YRS enh: Loan EFT Project: "new" Loan Admin webpages (approve/decline/process)
'Sanjay GS Rawat    2018.10.16		YRS-AT-3101 - YRS enh: EFT: Loans ( FIRST EFT PROJECT) - updates for "Payment Manager" (TrackIT 33024) 
'Megha Lad			2019.11.28 		YRS-AT-4598 - YRS enh: State Withholding Project - Annuity Payroll Processing.
'*********************************************************************************************************************
Imports System.Data
Imports System.IO
Imports System.Text.RegularExpressions
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports Microsoft.Practices.EnterpriseLibrary.Logging
Imports YMCAObjects
Imports YMCARET.YmcaBusinessObject
'START : ML | 2019.11.28 | 20.7.0 | YRS-AT-4598 | This Function Compare property and property Value of two objects.
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.Runtime.Serialization
'END : ML | 2019.11.28 | 20.7.0 | YRS-AT-4598 | This Function Compare property and property Value of two objects.

Public Class HelperFunctions
    Private Sub New()
    End Sub
    'Added By Shashi Shekhar:2009-12-23: Define variable to use globally on page whereever CheckAccess()javascript function is called from code behind
    Public Shared SecurityCheckString As String = "javascript:if(CheckAccess('{0}')==false)return false;"

#Region "General Utility Functions"
#Region "IsNonEmpty Functions"
    Public Shared Function isNonEmpty(ByRef ds As DataSet) As Boolean
        If ds Is Nothing Then Return False
        If ds.Tables.Count = 0 Then Return False
        If ds.Tables(0).Rows.Count = 0 Then Return False
        Return True
    End Function
    Public Shared Function isNonEmpty(ByRef dv As DataView) As Boolean
        If dv Is Nothing Then Return False
        If dv.Count = 0 Then Return False
        Return True
    End Function
    Public Shared Function isNonEmpty(ByRef dt As DataTable) As Boolean
        If dt Is Nothing Then Return False
        If dt.Rows.Count = 0 Then Return False
        Return True
    End Function
    Public Shared Function isNonEmpty(ByRef obj As Object) As Boolean
        If obj Is Nothing Then Return False
        If TypeOf (obj) Is DataSet Then
            Return isNonEmpty(CType(obj, DataSet))
        End If
        If Convert.ToString(obj).Trim = String.Empty Then Return False
        Return True
    End Function
#End Region
#Region "IsEmpty Functions"
    Public Shared Function isEmpty(ByRef obj As Object)
        Return Not isNonEmpty(obj)
    End Function
    Public Shared Function isEmpty(ByRef dt As DataTable)
        Return Not isNonEmpty(dt)
    End Function

    Public Shared Function isEmpty(ByRef ds As DataSet) As Boolean
        Return Not isNonEmpty(ds)
    End Function
    Public Shared Function isEmpty(ByRef dv As DataView) As Boolean
        Return Not isNonEmpty(dv)
    End Function
#End Region
    Public Shared Function GetRowForUpdation(ByVal p_datatable As DataTable, ByVal p_string_name As String, ByVal p_string_key As String) As DataRow
        Dim l_datarows As DataRow()
        Dim l_datarow As DataRow = Nothing 'B.Jagadeesh 2015.06.01 BT:2882: Remove all warning message from YRS project.
        Try
            l_datarows = p_datatable.Select(p_string_name + "='" + p_string_key + "'")
            If l_datarows.Length > 0 Then
                l_datarow = l_datarows(0)
            End If
            Return l_datarow
        Catch
            Throw
        End Try
    End Function
#End Region

#Region "ParseCSV - Convert CSV file to Data Table"
    'function that parses any CSV input string into a DataTable
    Public Shared Function ParseCSV(ByVal inputString As String) As DataTable

        Try

            Dim dt As New DataTable

            ' declare the Regular Expression that will match versus the input string 
            'Regular expression which handle empty value in CSV File
            Dim re As New Regex("(?<field>,)|((?<field>[^"",\r\n]+)|""(?<field>([^""]|"""")+)"")(,|(?<rowbreak>\r\n|\n|$))")

            'Dim re As New Regex("((?<field>[^"",\r\n]+)|""(?<field>([^""]|"""")+)"")(,|(?<rowbreak>\r\n|\n|$))")

            Dim colArray As New ArrayList
            Dim rowArray As New ArrayList

            Dim colCount As Integer = 0
            Dim maxColCount As Integer = 0
            Dim rowbreak As String = ""
            Dim field As String = ""

            Dim mc As MatchCollection = re.Matches(inputString)

            For Each m As Match In mc

                ' retrieve the field and replace two double-quotes with a single double-quote 
                field = m.Result("${field}").Replace("""""", """").Trim

                If field = "," Then
                    field = " "
                End If

                rowbreak = m.Result("${rowbreak}")

                If field.Length > 0 Then
                    colArray.Add(field)
                    colCount += 1
                End If

                If rowbreak.Length > 0 Then

                    ' add the column array to the row Array List 
                    rowArray.Add(colArray.ToArray())

                    ' create a new Array List to hold the field values 
                    colArray = New ArrayList

                    If colCount > maxColCount Then
                        maxColCount = colCount
                    End If

                    colCount = 0
                End If
            Next

            If rowbreak.Length = 0 Then
                ' this is executed when the last line doesn't 
                ' end with a line break 
                rowArray.Add(colArray.ToArray())
                If colCount > maxColCount Then
                    maxColCount = colCount
                End If
            End If

            ' create the columns for the table 
            For i As Integer = 0 To maxColCount - 1
                dt.Columns.Add([String].Format("col{0:000}", i))
            Next

            ' convert the row Array List into an Array object for easier access 
            Dim ra As Array = rowArray.ToArray()
            For i As Integer = 0 To ra.Length - 1

                ' create a new DataRow 
                Dim dr As DataRow = dt.NewRow()

                ' convert the column Array List into an Array object for easier access 
                Dim ca As Array = DirectCast((ra.GetValue(i)), Array)

                ' add each field into the new DataRow 
                For j As Integer = 0 To ca.Length - 1
                    dr(j) = ca.GetValue(j).ToString().Trim
                Next

                ' add the new DataRow to the DataTable 
                dt.Rows.Add(dr)
            Next

            ' in case no data was parsed, create a single column 
            If dt.Columns.Count = 0 Then
                dt.Columns.Add("NoData")
            End If
            Return dt
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Shared Function ParseCSVFile(ByVal path As String) As DataTable
        Try
            Dim inputString As String = ""

            ' check that the file exists before opening it 
            If File.Exists(path) Then

                Dim sr As New StreamReader(path)
                inputString = sr.ReadToEnd()

                sr.Close()
            End If

            Return ParseCSV(inputString.Trim)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
#End Region
#Region "Logger methods"
    'B.Jagadeesh 2015.06.01 BT:2882: Remove all warning message from YRS project.
    Public Shared Sub LogException(ByVal paraMessage As String, ByVal paraException As Exception)
        Dim newExp As Exception
        Try
            newExp = New Exception(paraMessage, paraException)
            ExceptionPolicy.HandleException(newExp, "Exception Policy")
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Shared Function LogMessage(ByVal paraMessage As String) As Boolean
        Try
            Logger.Write(paraMessage, "Application", 0, 0, TraceEventType.Information)
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function
#End Region

#Region "BindGrid Functions"
    'dg = The datagrid to bind data to
    'ds = The dataset which contains the data
    'forceVisible = Whether the datagrid should be displayed if it does not contain any data
    Public Shared Sub BindGrid(ByRef dg As DataGrid, ByRef ds As DataSet, Optional ByVal forceVisible As Boolean = False)
        If ds Is Nothing OrElse ds.Tables.Count = 0 OrElse ds.Tables(0).Rows.Count = 0 Then
            dg.DataSource = Nothing
            dg.DataBind()
            dg.Visible = forceVisible
            Exit Sub
        Else
            dg.DataSource = ds.Tables(0)
            dg.DataBind()
            dg.Visible = True
        End If
    End Sub
    Public Shared Sub BindGrid(ByRef dg As DataGrid, ByRef dv As DataView, Optional ByVal forceVisible As Boolean = False)
        If dv Is Nothing OrElse dv.Count = 0 Then
            dg.DataSource = Nothing
            dg.DataBind()
            dg.Visible = forceVisible
            Exit Sub
        Else
            dg.DataSource = dv
            dg.DataBind()
            dg.Visible = True
        End If
    End Sub
    'SR:2013.10.15 - added common binding function for Grid view		
    Public Shared Sub BindGrid(ByRef dg As GridView, ByRef ds As DataSet, Optional ByVal forceVisible As Boolean = False)
        If ds Is Nothing OrElse ds.Tables.Count = 0 OrElse ds.Tables(0).Rows.Count = 0 Then
            dg.DataSource = Nothing
            dg.DataBind()
            dg.Visible = forceVisible
            Exit Sub
        Else
            dg.DataSource = ds.Tables(0)
            dg.DataBind()
            dg.Visible = True
        End If
    End Sub

    Public Shared Sub BindGrid(ByRef dg As GridView, ByRef dv As DataView, Optional ByVal forceVisible As Boolean = False)
        If dv Is Nothing OrElse dv.Count = 0 Then
            dg.DataSource = Nothing
            dg.DataBind()
            dg.Visible = forceVisible
            Exit Sub
        Else
            dg.DataSource = dv
            dg.DataBind()
            dg.Visible = True
        End If
    End Sub
    'End, SR:2013.10.15 - added common binding function for Grid view		
#End Region

#Region "DataTable - Set selected image of selected row"
    Public Shared Sub SetSelectedImageOfDataGrid(ByVal sender As System.Object, ByVal e As System.EventArgs, ByVal RadioButtonName As String)
        Dim i As Integer
        Dim dg As DataGrid = DirectCast(sender, DataGrid)

        For i = 0 To dg.Items.Count - 1
            If dg.Items(i).ItemType = ListItemType.AlternatingItem OrElse dg.Items(i).ItemType = ListItemType.Item OrElse dg.Items(i).ItemType = ListItemType.SelectedItem Then
                Dim l_button_Select As ImageButton

                'l_button_Select = DirectCast(DataGridYMCAContact.Items(i).FindControl(RadioButtonName), ImageButton)
                l_button_Select = DirectCast(dg.Items(i).FindControl(RadioButtonName), ImageButton)
                If Not l_button_Select Is Nothing Then
                    If i = dg.SelectedIndex Then
                        l_button_Select.ImageUrl = "images\selected.gif"
                    Else
                        l_button_Select.ImageUrl = "images\select.gif"
                    End If
                End If
            End If
        Next
    End Sub
#End Region

    'Shashi Singh: 24 feb 2011: Function to validate Guid
    Private Shared isGuid As New Regex("^(\{){0,1}[0-9a-fA-F]{8}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{12}(\}){0,1}$")
    Public Shared Function IsValidGuid(ByVal value As String) As Boolean
        Dim isValid As Boolean = False
        If Not [String].IsNullOrEmpty(value) Then
            If IsGuid.IsMatch(value) Then
                isValid = True
            End If
        End If
        Return isValid
    End Function

    'Shashi Singh: 24 feb 2011: Function to validate SSN
    Private Shared isSSN As New Regex("[0-9]{9}")
    Public Shared Function IsValidSSN(ByVal value As String) As Boolean
        Dim isValid As Boolean = False
        If Not [String].IsNullOrEmpty(value) Then
            If isSSN.IsMatch(value) Then
                isValid = True
            End If
        End If
        Return isValid
    End Function

    'Shashi Singh: 24 feb 2011: Function to validate FundNo
    Private Shared isFundNo As New Regex("[0-9]")
    Public Shared Function IsValidFundNo(ByVal value As String) As Boolean
        Dim isValid As Boolean = False
        If Not [String].IsNullOrEmpty(value) Then
            If isFundNo.IsMatch(value) Then
                isValid = True
            End If
        End If
        Return isValid
    End Function

    'Shashi Singh: 24 feb 2011: Function to validate YMCANo
    Private Shared isYMCANo As New Regex("[0-9]{6}")
    Public Shared Function IsValidYMCANo(ByVal value As String) As Boolean
        Dim isValid As Boolean = False
        If Not [String].IsNullOrEmpty(value) Then
            If isYMCANo.IsMatch(value) Then
                isValid = True
            End If
        End If
        Return isValid
    End Function
    'BS:2012.01.27:Add common function to sanitize values for javascript
    Public Shared Function SanitizeValueForJS(ByVal s As String) As String
        Return s.Replace("\", "\\").Replace("'", "\'").Replace("""", "\""")
    End Function

    'SP : 2013.09.25 : BT:618/YRS 5.0-842 : Need ability to pay one person on a transmittal
    Public Shared Sub ShowMessageToUser(ByVal message As String, ByVal messageType As EnumMessageTypes, Optional ByVal div As HtmlGenericControl = Nothing)
        Dim objList As List(Of Dictionary(Of String, Object)) = DirectCast(HttpContext.Current.Items("ErrorMessages"), List(Of Dictionary(Of String, Object)))
        If objList Is Nothing Then
            objList = New List(Of Dictionary(Of String, Object))()
            HttpContext.Current.Items("ErrorMessages") = objList
        End If
        Dim d As New Dictionary(Of String, Object)()
        d.Add("message", message)
        d.Add("div", div)
        d.Add("messageType", messageType)
        objList.Add(d)
    End Sub
    'Start SP 2014.08.18  BT-2344\YRS 5.0-2279 : Add in Administration Screen ability to change messages for YRS or Web site
    Public Shared Sub ShowMessageToUser(ByVal iMessageNumber As Int32, Optional ByVal div As HtmlGenericControl = Nothing, Optional dictParameter As Dictionary(Of String, String) = Nothing)

        Dim metaMessage As MetaMessage

        Dim objList As List(Of Dictionary(Of String, Object)) = DirectCast(HttpContext.Current.Items("ErrorMessages"), List(Of Dictionary(Of String, Object)))
        If objList Is Nothing Then
            objList = New List(Of Dictionary(Of String, Object))()
            HttpContext.Current.Items("ErrorMessages") = objList
        End If

        Dim d As New Dictionary(Of String, Object)()

        metaMessage = MetaMessageBO.GetMessageByMessageNo(iMessageNumber, dictParameter)

        d.Add("message", metaMessage.DisplayText)
        d.Add("div", div)
        d.Add("messageType", metaMessage.MessageType)
        objList.Add(d)
    End Sub

    'End SP 2014.08.18  BT-2344\YRS 5.0-2279 : Add in Administration Screen ability to change messages for YRS or Web site

    'Start:BT:2625 :YRS 5.0-2405- added to a function to get the message and add a dynamic message in the message
    Public Shared Sub ShowMessageToUser(ByVal iMessageNumber As Int32, dictParameter As Dictionary(Of String, String))
        Try
            ShowMessageToUser(iMessageNumber, Nothing, dictParameter)
        Catch ex As Exception

        End Try
    End Sub
    'End:BT:2625 :YRS 5.0-2405- added to a function to get the message and add a dynamic message in the message
    'AA:16.12.2013 - BT:2311 Added to sort the gridviews
    ''' <summary>
    ''' Sets the sort expression 
    ''' </summary>
    ''' <param name="sortstate"></param>
    ''' <param name="sortexpression"></param>
    ''' <param name="dv"></param>
    ''' <exception cref="Exception"> Exceptions throw </exception>
    ''' <remarks> Setting the sorting</remarks>
    Public Shared Sub gvSorting(ByRef sortstate As GridViewCustomSort, ByVal sortexpression As String, ByVal dv As DataView)
        Dim oldsortexpression As String = String.Empty
        Try
            If sortstate IsNot Nothing Then
                oldsortexpression = sortstate.SortExpression
            Else
                sortstate = New GridViewCustomSort
            End If

            If sortexpression = oldsortexpression Then
                If sortstate.SortDirection.ToUpper() = "ASC" Then
                    sortstate.SortDirection = "DESC"
                Else
                    sortstate.SortDirection = "ASC"
                End If
            Else
                sortstate.SortDirection = "ASC"
            End If

            sortstate.SortExpression = sortexpression

            sortexpression = sortstate.SortExpression + " " + sortstate.SortDirection
            If dv IsNot Nothing Then
                dv.Sort() = sortexpression
            Else
                Throw New Exception("Dataview cannot be blank")
            End If

        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    'AA:16.12.2013 - BT:2311 Added to set the Arrows on sorting

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sorting"></param>
    ''' <param name="gv"></param>
    ''' <remarks></remarks>
    Public Shared Sub SetSortingArrows(ByVal sorting As GridViewCustomSort, ByVal gv As System.Web.UI.WebControls.GridViewRowEventArgs)
        Dim lnk As LinkButton
        Dim img As New System.Web.UI.WebControls.Image()
        Dim tc As TableCell
        Try
            If gv.Row.RowType = DataControlRowType.Header And sorting IsNot Nothing Then
                For Each tc In gv.Row.Cells
                    If tc.HasControls() Then    ' search for the header link    
                        'AA:25.03.2014 - BT:957: YRS 5.0-1484 - Changed for not occuring error
                        If tc.Controls(0).GetType().ToString() = "System.Web.UI.WebControls.DataControlLinkButton" Then
                            lnk = DirectCast(tc.Controls(0), LinkButton)
                            If lnk IsNot Nothing AndAlso sorting.SortExpression = lnk.CommandArgument Then     ' inizialize a new image   
                                img.ImageUrl = "~/images/" & (If(sorting.SortDirection.ToUpper() = "ASC", "asc", "desc")) & ".gif"     ' adding a space and the image to the header link    
                                tc.Controls.Add(New LiteralControl(" "))
                                tc.Controls.Add(img)
                            End If
                        End If
                    End If
                Next
            End If
        Catch
            Throw
        End Try
    End Sub

    'Start:AA:2015.02.10 :BT:2738:YRS 5.0-2456:YERDI3I-2319: Added to concatinate name with all the three fields of name
    Public Shared Function GetFullName(ByVal strFirstName As String, ByVal strMiddleName As String, ByVal strLastName As String) As String
        Try
            Dim strName As String = ""

            If strFirstName.Trim <> "" Then
                strName += strFirstName.Trim
            End If
            If strMiddleName.Trim <> "" Then
                If strName.Trim <> "" Then
                    strName += " " + strMiddleName.Trim
                Else
                    strName += strMiddleName.Trim
                End If
            End If
            If strLastName.Trim <> "" Then
                If strName.Trim <> "" Then
                    strName += " " + strLastName.Trim
                Else
                    strName += strLastName.Trim
                End If
            End If
            Return strName
        Catch
            Throw
        End Try
    End Function
    'End:AA:2015.02.10 :BT:2738:YRS 5.0-2456:YERDI3I-2319: Added to concatinate name with all the three fields of name
    'Start:BT:2882: Remove all warning message from YRS project.
    Public Shared Function GetConfigurationSetting(strKey As String) As String
        Return ConfigurationManager.AppSettings.Get(strKey)
    End Function
    'End:BT:2882: Remove all warning message from YRS project.
    'Start: AA:03.21.2016 YRS_AT_2594 Added below code to get bytes from string
    Public Shared Function GetBytes(str As String) As Byte()
        Dim bytes As Byte() = New Byte(str.Length * 2 - 1) {}
        System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length)
        Return bytes
    End Function
    'End: AA:03.21.2016 YRS_AT_2594 Added below code to get bytes from string
    'START - Chandra sekar -2016.08.22- YRS-AT-3081 - YRS Enh: request to QDRO function (Retired ) to allow different amt or percent (TrackIT 23098)
    'Generating the html table for recipiant Annuities grid
    'Getting distinct Split Type in the recipient Annuities details based on the split type the recipient annuities literation will works
    'For Grouping filtering the SplitType based in the recipient Person's
    Public Shared Function GenerateHTMLTableForRecipientAnnuities(ByVal dtRecptAccount As DataTable) As String
        Dim sb As System.Text.StringBuilder
        Dim sw As StringWriter
        Dim htmlTextWriter As HtmlTextWriter
        Dim table As HtmlTable
        Dim row As HtmlTableRow
        Dim cell As HtmlTableCell
        Dim strSplitTypeHeading As String
        Dim IsPercentageOrAmt As String
        Dim drArryRecipeientAnn As DataRow()
        Dim recordscount As Integer
        Dim strArrayTableHeadling() As String = {"Annuity Source Code", "Plan Type", "Purchase Date", "Annuity Type", "Current Payment", "Emp PreTax Current Payment", "Emp PostTax Current Payment",
                                            "Ymca PreTax Current Payment", "Emp PreTax Remaining Reserves", "Emp PostTax Remaining Reserves", "Ymca PreTax Remaining Reserves", "SS Levling Amount",
                                            "SS Reduction Amount", "SS Reduction Effective Date"}
        Try
            'Start - Creating Heading for the recipient Grid
            table = New System.Web.UI.HtmlControls.HtmlTable()
            row = New System.Web.UI.HtmlControls.HtmlTableRow()
            For i As Integer = 0 To strArrayTableHeadling.Length - 1
                cell = New System.Web.UI.HtmlControls.HtmlTableCell("th")
                cell.InnerText = strArrayTableHeadling(i)
                row.Cells.Add(cell)
            Next
            row.Attributes.Add("class", "DataGrid_HeaderStyle")
            table.Rows.Add(row)

            'End - Creating Heading for the recipient Grid
            ' Dim dtRecptAccount As DataTable = GetRecipientAnnutiesDetailsForHtmlTable(Me.Session_Datatable_DtRecptAccount, RecipientPersonID)
            'Getting distinct Split Type in the recipient Annuities details based on the split type the recipient annuities literation will works
            'For Grouping filtering the SplitType based in the recipient Person's

            Dim strdTobeDistinct As String() = {"splitType", "IsSplitPercentage", "SplitAmount"}
            Dim dtDistinct As DataTable = GetDistinctRecords(dtRecptAccount, strdTobeDistinct)
            For i As Integer = 0 To dtDistinct.Rows.Count - 1
                If (dtDistinct.Rows(i)("IsSplitPercentage").ToString().ToLower = "false") Then
                    strSplitTypeHeading = String.Format("Plan : {0} (Split: ${1})", dtDistinct.Rows(i)("splitType"), dtDistinct.Rows(i)("SplitAmount"))
                Else
                    strSplitTypeHeading = String.Format("Plan : {0} (Split: {1}%)", dtDistinct.Rows(i)("splitType"), dtDistinct.Rows(i)("SplitAmount"))
                End If
                row = New System.Web.UI.HtmlControls.HtmlTableRow()
                ' Split Type Heading
                cell = New System.Web.UI.HtmlControls.HtmlTableCell()
                cell.ColSpan = 14
                cell.InnerText = strSplitTypeHeading
                row.Cells.Add(cell)
                row.Attributes.Add("class", "DataGrid_HeaderStyle")
                table.Rows.Add(row)
                recordscount = 0


                drArryRecipeientAnn = dtRecptAccount.Select("splitType ='" + dtDistinct.Rows(i)("splitType").ToString() + "'")
                For Each drRecipientAnnuitiesData As DataRow In drArryRecipeientAnn

                    row = New System.Web.UI.HtmlControls.HtmlTableRow()

                    cell = New System.Web.UI.HtmlControls.HtmlTableCell()
                    cell.InnerText = String.Format("{0}", drRecipientAnnuitiesData("AnnuitySourceCode"))
                    row.Cells.Add(cell)

                    cell = New System.Web.UI.HtmlControls.HtmlTableCell()
                    cell.InnerText = String.Format("{0}", drRecipientAnnuitiesData("PlanType"))
                    row.Cells.Add(cell)

                    cell = New System.Web.UI.HtmlControls.HtmlTableCell()
                    cell.InnerText = String.Format("{0:MM/dd/yyyy}", drRecipientAnnuitiesData("PurchaseDate"))
                    row.Cells.Add(cell)


                    cell = New System.Web.UI.HtmlControls.HtmlTableCell()
                    cell.InnerText = String.Format("{0}", drRecipientAnnuitiesData("AnnuityType"))
                    row.Cells.Add(cell)

                    cell = New System.Web.UI.HtmlControls.HtmlTableCell()
                    cell.Attributes.Add("style", "text-align:right") 'PPP | 01/23/2017 | YRS-AT-3299 | Numeric columns alignment brings to right
                    cell.InnerText = String.Format("{0:F2}", drRecipientAnnuitiesData("CurrentPayment"))
                    row.Cells.Add(cell)

                    cell = New System.Web.UI.HtmlControls.HtmlTableCell()
                    cell.Attributes.Add("style", "text-align:right") 'PPP | 01/23/2017 | YRS-AT-3299 | Numeric columns alignment brings to right
                    cell.InnerText = String.Format("{0:F2}", drRecipientAnnuitiesData("EmpPreTaxCurrentPayment"))
                    row.Cells.Add(cell)

                    cell = New System.Web.UI.HtmlControls.HtmlTableCell()
                    cell.Attributes.Add("style", "text-align:right") 'PPP | 01/23/2017 | YRS-AT-3299 | Numeric columns alignment brings to right
                    cell.InnerText = String.Format("{0:F2}", drRecipientAnnuitiesData("EmpPostTaxCurrentPayment"))
                    row.Cells.Add(cell)

                    cell = New System.Web.UI.HtmlControls.HtmlTableCell()
                    cell.Attributes.Add("style", "text-align:right") 'PPP | 01/23/2017 | YRS-AT-3299 | Numeric columns alignment brings to right
                    cell.InnerText = String.Format("{0:F2}", drRecipientAnnuitiesData("YmcaPreTaxCurrentPayment"))
                    row.Cells.Add(cell)

                    cell = New System.Web.UI.HtmlControls.HtmlTableCell()
                    cell.Attributes.Add("style", "text-align:right") 'PPP | 01/23/2017 | YRS-AT-3299 | Numeric columns alignment brings to right
                    cell.InnerText = String.Format("{0:F2}", drRecipientAnnuitiesData("EmpPreTaxRemainingReserves"))
                    row.Cells.Add(cell)

                    cell = New System.Web.UI.HtmlControls.HtmlTableCell()
                    cell.Attributes.Add("style", "text-align:right") 'PPP | 01/23/2017 | YRS-AT-3299 | Numeric columns alignment brings to right
                    cell.InnerText = String.Format("{0:F2}", drRecipientAnnuitiesData("EmpPostTaxRemainingReserves"))
                    row.Cells.Add(cell)

                    cell = New System.Web.UI.HtmlControls.HtmlTableCell()
                    cell.Attributes.Add("style", "text-align:right") 'PPP | 01/23/2017 | YRS-AT-3299 | Numeric columns alignment brings to right
                    cell.InnerText = String.Format("{0:F2}", drRecipientAnnuitiesData("YmcaPreTaxRemainingReserves"))
                    row.Cells.Add(cell)

                    cell = New System.Web.UI.HtmlControls.HtmlTableCell()
                    cell.Attributes.Add("style", "text-align:right") 'PPP | 01/23/2017 | YRS-AT-3299 | Numeric columns alignment brings to right
                    cell.InnerText = String.Format("{0:F2}", drRecipientAnnuitiesData("SSLevelingAmt"))
                    row.Cells.Add(cell)

                    cell = New System.Web.UI.HtmlControls.HtmlTableCell()
                    cell.Attributes.Add("style", "text-align:right") 'PPP | 01/23/2017 | YRS-AT-3299 | Numeric columns alignment brings to right
                    cell.InnerText = String.Format("{0:F2}", drRecipientAnnuitiesData("SSReductionAmt"))
                    row.Cells.Add(cell)

                    cell = New System.Web.UI.HtmlControls.HtmlTableCell()
                    cell.InnerHtml = IIf(Convert.IsDBNull(drRecipientAnnuitiesData("SSReductionEftDate")), "<span>&nbsp;</span>", String.Format("{0:MM/dd/yyyy}", drRecipientAnnuitiesData("SSReductionEftDate")))
                    row.Cells.Add(cell)
                    ' To Display the style of rows 
                    If recordscount Mod 2 = 0 Then
                        row.Attributes.Add("class", "DataGrid_NormalStyle") 'Even Rows
                    Else
                        row.Attributes.Add("class", "DataGrid_AlternateStyle") 'Odd Rows
                    End If

                    recordscount = 1 + recordscount

                    table.Rows.Add(row)

                Next
            Next
            table.Attributes.Add("class", "DataGrid_Grid")
            table.Attributes.Add("cellspacing", "0")
            table.Attributes.Add("rules", "all")
            table.Attributes.Add("border", "1")
            table.Attributes.Add("style", "width:100%;border-collapse:collapse;")
            sb = New System.Text.StringBuilder()
            sw = New StringWriter(sb)
            htmlTextWriter = New System.Web.UI.HtmlTextWriter(sw)
            table.RenderControl(htmlTextWriter)
            Return sb.ToString()

        Catch
            Throw
        Finally
            cell = Nothing
            row = Nothing
            table = Nothing
            htmlTextWriter = Nothing
            sw = Nothing
            sb = Nothing
        End Try
    End Function
    Public Shared Function GetDistinctRecords(dt As DataTable, Columns As String()) As DataTable
        Dim dtUniqRecords As New DataTable()
        Try
            dtUniqRecords = dt.DefaultView.ToTable(True, Columns)
            Return dtUniqRecords
        Catch
            Throw
        End Try
    End Function
    'END - Chandra sekar-2016.08.22-YRS-AT-3081 - YRS Enh: request to QDRO function (Retired ) to allow different amt or percent (TrackIT 23098)

    'START: PPP | 03/14/2017 | YRS-AT-2625 | Following function will help to convert data table into XML
    Public Shared Function ConvertDataTableToXML(ByVal table As DataTable) As String
        Dim result As String = String.Empty
        If isNonEmpty(table) Then
            Using sw As New System.IO.StringWriter()
                table.WriteXml(sw)
                result = sw.ToString()
            End Using
        End If
        Return result
    End Function
    'END: PPP | 03/14/2017 | YRS-AT-2625 | Following function will help to convert data table into XML

    'START: MMR |  2017.09.19 | YRS-AT-3665 | Display success messsage on next screen after succcessfull activity on existing screen
    Public Shared Sub ShowMessageOnNextPage(ByVal iMessageNumber As Integer, Optional ByVal div As HtmlGenericControl = Nothing, Optional dictParameter As Dictionary(Of String, String) = Nothing)
        Dim metaMessage As MetaMessage
        Dim objList As List(Of Dictionary(Of String, Object))
        Dim d As New Dictionary(Of String, Object)()

        If Not HttpContext.Current.Session("NextPageMessages") Is Nothing Then
            objList = TryCast(HttpContext.Current.Session("NextPageMessages"), List(Of Dictionary(Of String, Object)))
        Else
            objList = New List(Of Dictionary(Of String, Object))()
            HttpContext.Current.Session("NextPageMessages") = objList
        End If

        metaMessage = MetaMessageBO.GetMessageByMessageNo(iMessageNumber, dictParameter)

        d.Add("message", metaMessage.DisplayText)
        d.Add("div", div)
        d.Add("messageType", metaMessage.MessageType)
        objList.Add(d)
    End Sub
    'END: MMR |  2017.09.19 | YRS-AT-3665 | Display success messsage on next screen after succcessfull activity on existing screen

    ' START | SR | 2017.11.27 | YRS-AT-3742 - YRS Bug:(HOT FIX NEEDED) -participants with RMDs more than non-taxable amount are being forced to take excessive amt (TrackIT 31764) 
    ''' <summary>
    ''' Provides a copy of given <typeparamref name="T"/> (Its not a shallow copy)
    ''' </summary>
    ''' <typeparam name="T">Object</typeparam>
    ''' <param name="item"></param>
    ''' <returns>Copy of <typeparamref name="T"/></returns>
    Public Shared Function DeepCopy(Of T)(item As T) As T
        Dim formatter As New System.Runtime.Serialization.Formatters.Binary.BinaryFormatter()
        Dim stream As New System.IO.MemoryStream()
        formatter.Serialize(stream, item)
        stream.Seek(0, System.IO.SeekOrigin.Begin)
        Dim result As T = DirectCast(formatter.Deserialize(stream), T)
        stream.Close()
        Return result
    End Function
    ' END | SR | 2017.11.27 | YRS-AT-3742 - YRS Bug:(HOT FIX NEEDED) -participants with RMDs more than non-taxable amount are being forced to take excessive amt (TrackIT 31764) 


    ' START | SR | 2018.04.19 | YRS-AT-3101 -  common method to add column into datatable
    ''' <summary>
    ''' add column into datatable
    ''' </summary>
    ''' <typeparam name="T">Object</typeparam>
    ''' <param name="name"></param>
    ''' <param name="dataType"></param>
    ''' <param name="defaultValue"></param>
    ''' <returns>DataColumn<typeparamref name="T"/></returns>
    Public Shared Function CreateDataTableColumn(ByVal name As String, ByVal dataType As String, ByVal defaultValue As String) As DataColumn
        Dim column As DataColumn = New DataColumn(name, System.Type.[GetType](dataType))
        column.DefaultValue = defaultValue
        Return column
    End Function
    ' END | SR | 2018.04.19 | YRS-AT-3101 - common method to add column into datatable

    'START:Chandra sekar | 2018.05.23 | YRS-AT-3270 |Displaying a telephone in the Format(111-111-1111) in the email content while sending notification for new officer/LPA added
    Public Shared Function TelephoneInFormat(ByVal phoneNo As String) As String
        Dim telephoneNo As Double
        Try
            If phoneNo.Length = 10 Then
                If Double.TryParse(phoneNo, telephoneNo) Then
                    phoneNo = String.Format("{0:###-###-####}", telephoneNo)
                End If
            End If

            Return phoneNo
        Catch
            Throw
        End Try
    End Function
    'END:Chandra sekar | 2018.05.23 | YRS-AT-3270 |Displaying a telephone in the Format(111-111-1111) in the email content while sending notification for new officer/LPA added

    'START: PPP | 10/03/2018 | 20.6.0 | YRS-AT-4017 | Following function is present at 3 RMD pages but was not available at common location for other pages to use it.
    '                                                 So coping it here and then need to update RMD pages to call this one
    Public Shared Function ConvertToXML(ByVal objCl As Object) As String
        Dim objXml As New System.Xml.Serialization.XmlSerializer(objCl.GetType())

        Dim objSW As New StringWriter
        objXml.Serialize(objSW, objCl)

        Dim xmlDoc As New System.Xml.XmlDocument
        xmlDoc.LoadXml(objSW.ToString())

        Dim XmlStr As String = ""
        XmlStr += "<" & xmlDoc.DocumentElement.Name & ">"
        XmlStr += xmlDoc.DocumentElement.InnerXml
        XmlStr += "</" & xmlDoc.DocumentElement.Name & ">"

        Return XmlStr
    End Function
    'END: PPP | 10/03/2018 | 20.6.0 | YRS-AT-4017 | Following function is present at 3 RMD pages but was not available at common location for other pages to use it.
    'START : SR | 10/09/2018 | 20.6.0 | YRS-AT-3101 | Following Methods will be used to dipaly messages in pages who are not having master pages.
    Public Shared Sub DisplayMessagesInNonMasterPages()
        Dim lsObjList As List(Of Dictionary(Of String, Object))
        Dim htDiv As HtmlGenericControl
        Dim strMessage As String
        Dim messageType As EnumMessageTypes
        lsObjList = DirectCast(System.Web.HttpContext.Current.Items("ErrorMessages"), List(Of Dictionary(Of String, Object)))
        If lsObjList IsNot Nothing Then
            For Each dict As Dictionary(Of String, Object) In lsObjList
                strMessage = Convert.ToString(dict("message"))
                htDiv = DirectCast(dict("div"), HtmlGenericControl)
                messageType = DirectCast(dict("messageType"), EnumMessageTypes)
                If Not String.IsNullOrEmpty(strMessage) AndAlso Not htDiv Is Nothing Then
                    AddMessageToDiv(messageType, htDiv, strMessage)
                End If
            Next
        End If

    End Sub

    Private Shared Sub AddMessageToDiv(ByVal messageType As EnumMessageTypes, ByVal htDivTemp As HtmlGenericControl, ByVal strMessage As String)
        Dim htDiv As HtmlGenericControl
        Try
            If Not String.IsNullOrEmpty(strMessage) Then


                Select Case messageType
                    Case YMCAObjects.EnumMessageTypes.Information
                        htDiv = DirectCast(htDivTemp.FindControl("info-msg"), HtmlGenericControl)
                        If htDiv Is Nothing Then
                            htDiv = New HtmlGenericControl("div")
                            htDiv.ID = "info-msg"
                            htDiv.Attributes.Add("class", "info-msg")
                            htDivTemp.Controls.Add(htDiv)
                        End If
                        htDiv.InnerHtml += Convert.ToString(strMessage) & "<br/>"
                        Exit Select
                    Case YMCAObjects.EnumMessageTypes.Error
                        htDiv = DirectCast(htDivTemp.FindControl("error-msg"), HtmlGenericControl)
                        If htDiv Is Nothing Then
                            htDiv = New HtmlGenericControl("div")
                            htDiv.ID = "error-msg"
                            htDiv.Attributes.Add("class", "error-msg")
                            htDivTemp.Controls.Add(htDiv)
                        End If
                        htDiv.InnerHtml += Convert.ToString(strMessage) & "<br/>"
                        Exit Select
                    Case YMCAObjects.EnumMessageTypes.Success
                        htDiv = DirectCast(htDivTemp.FindControl("success-msg"), HtmlGenericControl)
                        If htDiv Is Nothing Then
                            htDiv = New HtmlGenericControl("div")
                            htDiv.ID = "success-msg"
                            htDiv.Attributes.Add("class", "success-msg")
                            htDivTemp.Controls.Add(htDiv)
                        End If
                        htDiv.InnerHtml += Convert.ToString(strMessage) & "<br/>"
                        Exit Select
                    Case YMCAObjects.EnumMessageTypes.Warning
                        htDiv = DirectCast(htDivTemp.FindControl("warning-msg"), HtmlGenericControl)
                        If htDiv Is Nothing Then
                            htDiv = New HtmlGenericControl("div")
                            htDiv.ID = "warning-msg"
                            htDiv.Attributes.Add("class", "warning-msg")
                            htDivTemp.Controls.Add(htDiv)
                        End If
                        htDiv.InnerHtml += Convert.ToString(strMessage) & "<br/>"
                        Exit Select
                    Case Else
                        Exit Select
                End Select
            End If
        Catch ex As Exception

            Throw
        Finally
            htDiv = Nothing

        End Try
    End Sub
    'END : SR | 10/09/2018 | 20.6.0 | YRS-AT-3101 | Following Methods will be used to dipaly messages in pages who are not having master pages.
    'START : ML | 2019.11.28 | 20.7.0 | YRS-AT-4598 | This Function Compare property and property Value of two objects.
    ' If two objects match then return True Else False
    Public Shared Function CompareObjects(ByVal obj As Object, ByVal obj2 As Object) As Boolean
        If obj Is Nothing OrElse obj2.[GetType]() <> obj.[GetType]() Then
            Return False
        End If

        Using memStream As MemoryStream = New MemoryStream()
            Dim binaryFormatter As BinaryFormatter = New BinaryFormatter(Nothing, New StreamingContext(StreamingContextStates.Clone))
            binaryFormatter.Serialize(memStream, obj2)
            Dim b1 As Byte() = memStream.ToArray()
            memStream.SetLength(0)
            binaryFormatter.Serialize(memStream, obj)
            Dim b2 As Byte() = memStream.ToArray()
            If b1.Length <> b2.Length Then Return False

            For i As Integer = 0 To b1.Length - 1
                If b1(i) <> b2(i) Then Return False
            Next
        End Using

        Return True
    End Function
    'END : ML | 2019.11.28 | 20.7.0 | YRS-AT-4598 | This Function Compare property and property Value of two objects.
End Class



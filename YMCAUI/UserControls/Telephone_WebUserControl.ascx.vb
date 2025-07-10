'*******************************************************************************
' Copyright YMCA Retirement Fund All Rights Reserved. 
'
' Project Name		:	YMCA_YRS
' FileName			:	Telephone_WebUserControl.ascx.vb
' Author Name		:	Anudeep Adusumilli
' Employee ID		:	56556
' Email				:	anudeep.adusumilli@3i-infotech.com
' Contact No		:	
' Creation Time		:	3/13/2013 12:40:52 PM
' Description		:   Displays Participants Telephone Information 
'
'********************************************************************************************************************************
'Modification History
'********************************************************************************************************************************
'Modified By        Date            Description
'********************************************************************************************************************************
'Anudeep            22.08.2013      YRS 5.0-1862:Add notes record when user enters address in any module.
'Manthan Rajguru    2015.09.16      YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
'********************************************************************************************************************************
Public Class Telephone_WebUserControl
    Inherits System.Web.UI.UserControl
    Dim m_strPerssId As String
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("LoggedUserKey") Is Nothing Then
            Response.Redirect("Login.aspx", False)
            Exit Sub
        End If

    End Sub
    
    Public Sub RefreshTelephoneDetail(ByVal dtContacts As DataTable)
        dvContacts.InnerHtml = ""
        If dtContacts IsNot Nothing Then
            If dtContacts.Rows.Count > 0 Then
                Dim SbTableContacts As New StringBuilder()
                Dim strNumber As String = String.Empty
                Dim l_array_string_Phonetype(dtContacts.Rows.Count) As String ' To get the Phonetypes in an array

                'Extract the Phone type and respective phone numbers and display in div in table Format

                SbTableContacts.Append("<table style='vertical-align:top;'>")

                For iCount As Integer = 0 To dtContacts.Rows.Count - 1
                    'Checks already Phonetype exists in array if exists or Phone number is null or empty continue the loop
                    If l_array_string_Phonetype.Contains(dtContacts.Rows(iCount).Item("PhoneType").ToString()) Or String.IsNullOrEmpty(dtContacts.Rows(iCount).Item("PhoneNumber").ToString()) Then
                        Continue For
                    Else
                        'Collects the Phone type from dtatable and inserts in table row
                        l_array_string_Phonetype(iCount) = dtContacts.Rows(iCount).Item("PhoneType").ToString()

                        SbTableContacts.Append("<tr valign='top'><td align='left' class='Label_Small' valign='top'> ")
                        SbTableContacts.Append(l_array_string_Phonetype(iCount))
                        SbTableContacts.Append("</td>")
                        SbTableContacts.Append("<td align='left' valign='top'>")
                        strNumber = String.Empty
                        'Collects the phone number of above Phone type and extension to display in next cell of same row 
                        For Each l_datarow In dtContacts.Rows
                            If (l_datarow.IsNull("PhoneType") = False AndAlso l_datarow("PhoneType").ToString() <> "" And l_datarow.IsNull("PhoneNumber") = False AndAlso l_datarow("PhoneNumber").ToString() <> "") Then
                                Select Case l_datarow("PhoneType").ToString.ToUpper.Trim()
                                    Case l_array_string_Phonetype(iCount).ToUpper().Trim()
                                        'If extension exists then display in curve braces else display only number
                                        'If String.IsNullOrEmpty(l_datarow("Ext").ToString()) Then
                                        strNumber += l_datarow("PhoneNumber").ToString().Trim '+ ", "
                                        Exit For
                                        'Else
                                        'strNumber += l_datarow("PhoneNumber").ToString().Trim + "(" + CType(l_datarow("Ext"), String).Trim + "), "
                                        ' End If
                                End Select
                            End If
                        Next
                        'remove "," from the number and add in the table row cell
                        If strNumber.Length > 0 Then
                            'strNumber = strNumber.Substring(0, strNumber.Length - 2)
                            LabelNoContacts.Visible = False
                        Else
                            LabelNoContacts.Visible = True
                        End If
                        SbTableContacts.Append(strNumber)
                        SbTableContacts.Append("</td></tr>")
                    End If
                Next
                'Finally complete the table
                SbTableContacts.Append("</table>")
                dvContacts.InnerHtml = SbTableContacts.ToString()
                'Anudeep:22.08.2013-YRS 5.0-1862:Add notes record when user enters address in any module.
                If dtContacts.Select("ISNULL(PhoneNumber,'') <> '' ").Length = 0 Then
                    LabelNoContacts.Visible = True
                End If
            Else
                LabelNoContacts.Visible = True
            End If
        Else
            LabelNoContacts.Visible = True
        End If
        'EditPrimaryContact_Enabled = False
        'EditSecondaryContact_Enabled = False

    End Sub
    'To update telephone
    Public Sub UpdateTelphoneDetail(ByVal g_datatable_dsTelephoneInformation As DataTable, ByVal bitPrimary As Boolean)
        Dim l_dataset As DataSet
        If HelperFunctions.isNonEmpty(g_datatable_dsTelephoneInformation) Then
            For Each dr As DataRow In g_datatable_dsTelephoneInformation.Rows
                dr("bitPrimary") = bitPrimary
            Next
            l_dataset = New DataSet()
            l_dataset.Tables.Add(g_datatable_dsTelephoneInformation.Copy())
            YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.UpdateTelephone(l_dataset)
        End If
    End Sub
    'To Update telephone
End Class
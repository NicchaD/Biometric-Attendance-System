'****************************************************
'Modification History
'****************************************************
'Modified by          Date          Description
'****************************************************
'Manthan Rajguru      2015.09.16    YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
'Bala                 2015.12.14    YRS-AT-2642: Written a common method to send loan defaulted mail.
'Manthan Rajguru      2016.02.18    YRS-AT-2603: YRS enh: Withdrawals email - add participants name to eliminate 'forwarded' email confusion
'Anudeep A            2016.04.12    YRS-AT-2830 - YRS enh: automate 'Close' of fully paid Loans (TrackIT 25242)
'Anudeep A            2016.07.01    YRS-AT-2830 - YRS enh: automate 'Close' of fully paid Loans (TrackIT 25242)
'Pramod P. Pokale     2016.07.27    YRS-AT-3058 - YRS bug: timing issue Loan Utility - defaulting a Loan and then transmittal w/payment, Auto closed as successfully paid off? (trackIt 26945) 
'Santosh Bura         2018.09.03    YRS-AT-4081 -  YRS enh: YRS enhancement-generate email to LPA when loan is closed.Track it 34779
'Pramod P. Pokale     2019.01.17    YRS-AT-4281 -  YRS bug:"loan closed" email sent to wrong email address (TrackIT 36751) 
'Megha Lad            2019.01.17    YRS-AT-3157 - YRS enh-audit trail db table to track Loan freeze and unfreeze activity and copy emails to IDM (TrackIT 27438)
'*****************************************************
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
Imports System.Globalization
Imports System.Web.UI.HtmlControls.HtmlGenericControl
Imports Microsoft.Practices.EnterpriseLibrary.Caching
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling

Public Class RefundClass
    Public Sub New()
        MyBase.New()
    End Sub
    Public g_dataset_dsDisbursements As DataSet
    Dim g_dataset_dsDisbursementsByPersId As DataSet
    Public Function GetDisbursementByPersid(ByVal strPersId As String, ByVal strActivity As String) As DataSet
        'Dim obj As VoidUserControl
        Dim l_string_PersId As String
        Dim l_integer_GridIndex As Integer
        'Dim l_string_Activity As String
        Dim l_integer_AnnuityOnly As Integer
        Dim l_datarow_CurrentRow As DataRow
        'Aparna Samala -22/09/2006
        Dim l_searchExpr As String
        Dim l_dr_CurrentRow() As DataRow
        'Aparna Samala -22/09/2006
        Try
            'g_dataset_dsDisbursements = Session("dsDisbursements")
            'l_string_Activity = Request.QueryString.Get("Activity")

            If (strActivity = "Void") Or (strActivity = "VReplace") Then
                l_integer_AnnuityOnly = 1
            ElseIf (strActivity = "LOANREVERSE") Then
                l_integer_AnnuityOnly = 2
            ElseIf (strActivity = "LOANREPLACE") Then
                l_integer_AnnuityOnly = 21
            Else
                l_integer_AnnuityOnly = 0
            End If

            'l_integer_GridIndex = Session("DisbursementIndex") 'commented by priya on 12/15/2008 for YRS 5.0-626

            'l_datarow_CurrentRow = g_dataset_dsDisbursements.Tables("Disbursements").Rows(l_integer_GridIndex)
            'l_string_PersId = CType(l_datarow_CurrentRow("PersId"), String)

            'If obj.Action = "ANNUITY" Then
            '    l_integer_AnnuityOnly = 1
            'ElseIf obj.Action = "REFUND" Then
            '    l_integer_AnnuityOnly = 0
            'ElseIf obj.Action = "TDLOAN" Then
            '    l_integer_AnnuityOnly = 21
            'End If

            g_dataset_dsDisbursementsByPersId = YMCARET.YmcaBusinessObject.VoidDisbursementVRManagerBOClass.GetDisbursementsByPersId(strPersId, l_integer_AnnuityOnly)

            'Session("dsDisbursementsByPersId") = g_dataset_dsDisbursementsByPersId
            'If (g_dataset_dsDisbursementsByPersId.Tables("DisbursementsByPersId").Rows.Count = 0) Then
            '    Me.LabelNoRecordFound.Visible = True
            '    Me.TextBoxAddress.Text = ""
            '    Me.TextBoxAccountNo.Text = ""
            '    Me.TextBoxEntityType.Text = ""
            '    Me.TextBoxBankInfo.Text = ""
            '    Me.TextBoxEntityAddress.Text = ""
            '    Me.TextBoxLegalEntity.Text = ""
            'Else
            'Priya 06-May-2009
            If Not IsNothing(g_dataset_dsDisbursementsByPersId) Then
                If Not IsNothing(g_dataset_dsDisbursementsByPersId.Tables(0)) Then
                    If g_dataset_dsDisbursementsByPersId.Tables(0).Rows.Count = 0 Then
                        Return Nothing
                    Else
                        Dim l_disbursementHeading As DataTable
                        l_disbursementHeading = g_dataset_dsDisbursementsByPersId.Tables(0).Clone()
                        l_disbursementHeading.TableName = "l_disbursementHeading"
                        l_disbursementHeading.Rows.Clear()

                        Dim i As Integer
                        For i = 0 To g_dataset_dsDisbursementsByPersId.Tables(0).Rows.Count - 1
                            Dim l_row As DataRow
                            l_row = l_disbursementHeading.NewRow()
                            Dim totalAmount As String
                            If i <> 0 Then
                                If Convert.ToString(g_dataset_dsDisbursementsByPersId.Tables(0).Rows(i)("RequestId")).Trim <> Convert.ToString(g_dataset_dsDisbursementsByPersId.Tables(0).Rows(i - 1)("RequestId")).Trim Then
                                    totalAmount = 0
                                    l_row = g_dataset_dsDisbursementsByPersId.Tables(0).Rows(i)
                                    l_disbursementHeading.ImportRow(l_row)
                                    'added condition  by imran
                                    If g_dataset_dsDisbursementsByPersId.Tables(0).Rows(i)("RequestId").ToString.Trim <> "" Then

                                        totalAmount = g_dataset_dsDisbursementsByPersId.Tables(0).Compute("SUM(GrossAmount)", "RequestId = '" & g_dataset_dsDisbursementsByPersId.Tables(0).Rows(i)("RequestId").ToString.Trim & "'")
                                        l_disbursementHeading.Rows(l_disbursementHeading.Rows.Count - 1)("GrossAmount") = totalAmount

                                    End If

                                End If
                            Else
                                l_row = g_dataset_dsDisbursementsByPersId.Tables(0).Rows(i)
                                l_disbursementHeading.ImportRow(l_row) '  .Rows.Add(l_row)

                                'added condition  by imran
                                If g_dataset_dsDisbursementsByPersId.Tables(0).Rows(i)("RequestId").ToString.Trim <> "" Then
                                    totalAmount = g_dataset_dsDisbursementsByPersId.Tables(0).Compute("SUM(GrossAmount)", "RequestId = '" & g_dataset_dsDisbursementsByPersId.Tables(0).Rows(i)("RequestId").ToString.Trim & "'")
                                    l_disbursementHeading.Rows(l_disbursementHeading.Rows.Count - 1)("GrossAmount") = totalAmount
                                End If

                            End If
                        Next
                        g_dataset_dsDisbursementsByPersId.Tables.Add(l_disbursementHeading)
                        g_dataset_dsDisbursementsByPersId.AcceptChanges()
                        '.Tables.Add(l_disbursementHeading)
                        Return g_dataset_dsDisbursementsByPersId
                        'Datalist1.DataSource = l_disbursementHeading
                        ' Datalist1.DataBind()

                        'End priya 6-May-2009

                        'If Request.QueryString("Action") = "ANNUITY" Then
                        '    'Annuity
                        '    Dim l_disbursementAnnuityHeading As DataSet
                        '    l_disbursementAnnuityHeading = g_dataset_dsDisbursementsByPersId.Clone()
                        '    l_disbursementAnnuityHeading.Tables(0).Rows.Clear()
                        '    Dim j As Integer
                        '    'For j = 0 To g_dataset_dsDisbursementsByPersId.Tables(0).Rows.Count - 1
                        '    Dim l_row As DataRow
                        '    l_row = l_disbursementAnnuityHeading.Tables(0).NewRow()
                        '    Dim totalAmount As String

                        '    l_row = g_dataset_dsDisbursementsByPersId.Tables(0).Rows(0)
                        '    l_disbursementAnnuityHeading.Tables(0).ImportRow(l_row) '  .Rows.Add(l_row)
                        '    totalAmount = g_dataset_dsDisbursementsByPersId.Tables(0).Compute("SUM(Amount)", "")
                        '    l_disbursementAnnuityHeading.Tables(0).Rows(l_disbursementAnnuityHeading.Tables(0).Rows.Count - 1)("Amount") = totalAmount

                        '    Datalist2.DataSource = l_disbursementAnnuityHeading
                        '    Datalist2.DataBind()


                        'End If
                        'If Request.QueryString("Action") = "TDLOAN" Then
                        '    Dim l_disbursementLoanHeading As DataSet
                        '    l_disbursementLoanHeading = g_dataset_dsDisbursementsByPersId.Clone()
                        '    l_disbursementLoanHeading.Tables(0).Rows.Clear()
                        '    Dim j As Integer
                        '    For j = 0 To g_dataset_dsDisbursementsByPersId.Tables(0).Rows.Count - 1
                        '        Dim l_row As DataRow
                        '        l_row = l_disbursementLoanHeading.Tables(0).NewRow()
                        '        Dim totalAmount As String
                        '        If j <> 0 Then
                        '            If Convert.ToString(g_dataset_dsDisbursementsByPersId.Tables(0).Rows(j)("Number")).Trim <> Convert.ToString(g_dataset_dsDisbursementsByPersId.Tables(0).Rows(j - 1)("Number")).Trim Then
                        '                totalAmount = 0
                        '                l_row = g_dataset_dsDisbursementsByPersId.Tables(0).Rows(j)
                        '                l_disbursementLoanHeading.Tables(0).ImportRow(l_row)
                        '                totalAmount = g_dataset_dsDisbursementsByPersId.Tables(0).Compute("SUM(Amount)", "Number = '" & g_dataset_dsDisbursementsByPersId.Tables(0).Rows(j)("Number").ToString.Trim & "'")
                        '                l_disbursementLoanHeading.Tables(0).Rows(l_disbursementLoanHeading.Tables(0).Rows.Count - 1)("Amount") = totalAmount
                        '            End If
                        '        Else
                        '            l_row = g_dataset_dsDisbursementsByPersId.Tables(0).Rows(0)
                        '            l_disbursementLoanHeading.Tables(0).ImportRow(l_row) '  .Rows.Add(l_row)
                        '            totalAmount = g_dataset_dsDisbursementsByPersId.Tables(0).Compute("SUM(Amount)", "Number = '" & g_dataset_dsDisbursementsByPersId.Tables(0).Rows(j)("Number").ToString.Trim & "'")
                        '            l_disbursementLoanHeading.Tables(0).Rows(l_disbursementLoanHeading.Tables(0).Rows.Count - 1)("Amount") = totalAmount
                        '        End If
                        '    Next
                        '    Datalist3.DataSource = l_disbursementLoanHeading
                        '    Datalist3.DataBind()
                        'End If

                        'If Not (Session("DisbursementByPersIdIndex") > 0) Then
                        '    Session("DisbursementByPersIdIndex") = 0
                        'End If

                        ''PopulateDisbursementTextBoxes()
                        'Me.LabelNoRecordFound.Visible = False
                        'Me.LabelPayeeSSN.Text = l_datarow_CurrentRow("SSN").ToString + "-" + l_datarow_CurrentRow("FirstName").ToString + " " + l_datarow_CurrentRow("MiddleName").ToString + " " + l_datarow_CurrentRow("LastName").ToString
                        ''End If

                    End If


                End If
            End If
            'Catch sqlEx As System.Data.SqlClient.SqlException
            'If sqlEx.Number = 60006 Then
            'Me.LabelNoRecordFound.Visible = True
            'Else
            'Throw
            'End If
        Catch
            Throw
        End Try
    End Function
    Public Sub ReverseDisbursement(ByVal strDisbursementId As String)
        Dim strOutPut As String
        'Void("", "", "")
        Dim strNewDisbursementID As String
        strNewDisbursementID = Void(strDisbursementId, "REVERSE", "")

        strOutPut = YMCARET.YmcaBusinessObject.VoidDisbursementVRManagerBOClass.DoReversal(strDisbursementId, strNewDisbursementID)

    End Sub
    Public Sub ReplaceDisbursement()
    End Sub
    Public Sub ReissueDisbursement(ByVal strDisbursementId As String)
        Dim strOutPut As String
        Dim strNewDisbursementID As String
        strNewDisbursementID = Void(strDisbursementId, "REISUUE", "")

        strOutPut = YMCARET.YmcaBusinessObject.VoidDisbursementVRManagerBOClass.DoReissue(strDisbursementId, strNewDisbursementID)

    End Sub
    Public Function Void(ByVal strDisbursementID As String, ByVal strActivity As String, ByVal strNotes As String) As String
        Dim strOutput As String
        strOutput = YMCARET.YmcaBusinessObject.VoidDisbursementVRManagerBOClass.ChangeDisbursementStatus(strDisbursementID, strActivity, strNotes)
        If strOutput = "1" Then
            Throw New Exception("Unable to insert new row in atsDisbursement Table.")
            Exit Function
            'ElseIf strOutput = "1" Then
            'ElseIf strOutput = "2" Then
            '    Throw New Exception("Unsuccessful attempt to update row in funding table.& in insert row in disbursements table.")
            '    'Throw New Exception("Unsuccessful attempt to ")
            'ElseIf strOutput = "4" Then
            '    Throw New Exception("nsuccessful attempt to insert row in WithHoldings table.")
        ElseIf strOutput = "5" Then
            Throw New Exception("Unsuccessful attempt to insert row in DisbursementDetails table.")
            Exit Function
            'ElseIf strOutput = "6" Then
            '    Throw New Exception("Unsuccessful attempt to insert row in funding table.")
            'ElseIf strOutput = "7" Then
            '    Throw New Exception("Unsuccessful attempt to add row in DisbursementRefunds table.")
        End If
        Return strOutput
    End Function
End Class

Public Class LoanClass
    Public Sub New()
        MyBase.New()
    End Sub
    Public g_dataset_dsDisbursements As DataSet
    Public g_dataset_dsDisbursementsByPersId As DataSet
    'Commented by imran on  29/10/2009 for UnGrouping Disbursement list

    'Public Function GetLoanDisbursementByPersid(ByVal strPersId As String, ByVal strActivity As String) As DataSet
    '    Dim l_string_PersId As String
    '    Dim l_integer_GridIndex As Integer
    '    'Dim l_string_Activity As String
    '    Dim l_integer_AnnuityOnly As Integer
    '    Dim l_datarow_CurrentRow As DataRow
    '    'Aparna Samala -22/09/2006
    '    Dim l_searchExpr As String
    '    Dim l_dr_CurrentRow() As DataRow
    '    Try
    '        If (strActivity = "Void") Or (strActivity = "VReplace") Then
    '            l_integer_AnnuityOnly = 1
    '        ElseIf (strActivity = "LOANREVERSE") Then
    '            l_integer_AnnuityOnly = 2
    '        ElseIf (strActivity = "LOANREPLACE") Then
    '            l_integer_AnnuityOnly = 21
    '        Else
    '            l_integer_AnnuityOnly = 0
    '        End If

    '        g_dataset_dsDisbursementsByPersId = YMCARET.YmcaBusinessObject.VoidDisbursementVRManagerBOClass.GetDisbursementsByPersId(strPersId, l_integer_AnnuityOnly)
    '        If Not IsNothing(g_dataset_dsDisbursementsByPersId) Then
    '            If Not IsNothing(g_dataset_dsDisbursementsByPersId.Tables(0)) Then
    '                If g_dataset_dsDisbursementsByPersId.Tables(0).Rows.Count = 0 Then
    '                    Return Nothing
    '                Else
    '                    Dim l_disbursementLoanHeading As DataTable
    '                    l_disbursementLoanHeading = g_dataset_dsDisbursementsByPersId.Tables(0).Clone()
    '                    l_disbursementLoanHeading.TableName = "l_disbursementHeading"
    '                    l_disbursementLoanHeading.Rows.Clear()

    '                    Dim j As Integer
    '                    For j = 0 To g_dataset_dsDisbursementsByPersId.Tables(0).Rows.Count - 1
    '                        Dim l_row As DataRow
    '                        l_row = l_disbursementLoanHeading.NewRow()
    '                        Dim totalAmount As String
    '                        If j <> 0 Then
    '                            If Convert.ToString(g_dataset_dsDisbursementsByPersId.Tables(0).Rows(j)("Number")).Trim <> Convert.ToString(g_dataset_dsDisbursementsByPersId.Tables(0).Rows(j - 1)("Number")).Trim Then
    '                                totalAmount = 0
    '                                l_row = g_dataset_dsDisbursementsByPersId.Tables(0).Rows(j)
    '                                l_disbursementLoanHeading.ImportRow(l_row)
    '                                totalAmount = g_dataset_dsDisbursementsByPersId.Tables(0).Compute("SUM(Amount)", "Number = '" & g_dataset_dsDisbursementsByPersId.Tables(0).Rows(j)("Number").ToString.Trim & "'")
    '                                l_disbursementLoanHeading.Rows(l_disbursementLoanHeading.Rows.Count - 1)("Amount") = totalAmount
    '                            End If
    '                        Else
    '                            l_row = g_dataset_dsDisbursementsByPersId.Tables(0).Rows(0)
    '                            l_disbursementLoanHeading.ImportRow(l_row) '  .Rows.Add(l_row)
    '                            totalAmount = g_dataset_dsDisbursementsByPersId.Tables(0).Compute("SUM(Amount)", "Number = '" & g_dataset_dsDisbursementsByPersId.Tables(0).Rows(j)("Number").ToString.Trim & "'")
    '                            l_disbursementLoanHeading.Rows(l_disbursementLoanHeading.Rows.Count - 1)("Amount") = totalAmount
    '                        End If
    '                    Next
    '                    g_dataset_dsDisbursementsByPersId.Tables.Add(l_disbursementLoanHeading)
    '                    g_dataset_dsDisbursementsByPersId.AcceptChanges()
    '                    Return g_dataset_dsDisbursementsByPersId
    '                End If
    '            End If
    '        End If
    '    Catch ex As Exception
    '        Throw (ex)
    '    End Try

    '    'Dim l_disbursementLoanHeading As DataSet
    '    'l_disbursementLoanHeading = g_dataset_dsDisbursementsByPersId.Clone()
    '    'l_disbursementLoanHeading.Tables(0).Rows.Clear()

    '    'Dim j As Integer
    '    'For j = 0 To g_dataset_dsDisbursementsByPersId.Tables(0).Rows.Count - 1
    '    '    Dim l_row As DataRow
    '    '    l_row = l_disbursementLoanHeading.Tables(0).NewRow()
    '    '    Dim totalAmount As String
    '    '    If j <> 0 Then
    '    '        If Convert.ToString(g_dataset_dsDisbursementsByPersId.Tables(0).Rows(j)("Number")).Trim <> Convert.ToString(g_dataset_dsDisbursementsByPersId.Tables(0).Rows(j - 1)("Number")).Trim Then
    '    '            totalAmount = 0
    '    '            l_row = g_dataset_dsDisbursementsByPersId.Tables(0).Rows(j)
    '    '            l_disbursementLoanHeading.Tables(0).ImportRow(l_row)
    '    '            totalAmount = g_dataset_dsDisbursementsByPersId.Tables(0).Compute("SUM(Amount)", "Number = '" & g_dataset_dsDisbursementsByPersId.Tables(0).Rows(j)("Number").ToString.Trim & "'")
    '    '            l_disbursementLoanHeading.Tables(0).Rows(l_disbursementLoanHeading.Tables(0).Rows.Count - 1)("Amount") = totalAmount
    '    '        End If
    '    '    Else
    '    '        l_row = g_dataset_dsDisbursementsByPersId.Tables(0).Rows(0)
    '    '        l_disbursementLoanHeading.Tables(0).ImportRow(l_row) '  .Rows.Add(l_row)
    '    '        totalAmount = g_dataset_dsDisbursementsByPersId.Tables(0).Compute("SUM(Amount)", "Number = '" & g_dataset_dsDisbursementsByPersId.Tables(0).Rows(j)("Number").ToString.Trim & "'")
    '    '        l_disbursementLoanHeading.Tables(0).Rows(l_disbursementLoanHeading.Tables(0).Rows.Count - 1)("Amount") = totalAmount
    '    '    End If
    '    'Next
    '    'Datalist3.DataSource = l_disbursementLoanHeading
    '    'Datalist3.DataBind()
    '    'End If

    '    'If Not (Session("DisbursementByPersIdIndex") > 0) Then
    '    'Session("DisbursementByPersIdIndex") = 0
    '    'End If

    '    'PopulateDisbursementTextBoxes()
    '    'Me.LabelNoRecordFound.Visible = False
    '    'Me.LabelPayeeSSN.Text = l_datarow_CurrentRow("SSN").ToString + "-" + l_datarow_CurrentRow("FirstName").ToString + " " + l_datarow_CurrentRow("MiddleName").ToString + " " + l_datarow_CurrentRow("LastName").ToString
    'End Function
    Public Function GetLoanDisbursementByPersid(ByVal strPersId As String, ByVal strActivity As String) As DataSet
        Dim l_string_PersId As String
        Dim l_integer_GridIndex As Integer
        'Dim l_string_Activity As String
        Dim l_integer_AnnuityOnly As Integer
        Dim l_datarow_CurrentRow As DataRow
        'Aparna Samala -22/09/2006
        Dim l_searchExpr As String
        Dim l_dr_CurrentRow() As DataRow
        Try
            If (strActivity = "Void") Or (strActivity = "VReplace") Then
                l_integer_AnnuityOnly = 1
            ElseIf (strActivity = "LOANREVERSE") Then
                l_integer_AnnuityOnly = 2
            ElseIf (strActivity = "LOANREPLACE") Then
                l_integer_AnnuityOnly = 21
            Else
                l_integer_AnnuityOnly = 0
            End If

            g_dataset_dsDisbursementsByPersId = YMCARET.YmcaBusinessObject.VoidDisbursementVRManagerBOClass.GetDisbursementsByPersId(strPersId, l_integer_AnnuityOnly)
            If Not IsNothing(g_dataset_dsDisbursementsByPersId) Then
                If Not IsNothing(g_dataset_dsDisbursementsByPersId.Tables(0)) Then
                    If g_dataset_dsDisbursementsByPersId.Tables(0).Rows.Count = 0 Then
                        Return Nothing
                    Else
                        Dim l_disbursementLoanHeading As DataTable
                        l_disbursementLoanHeading = g_dataset_dsDisbursementsByPersId.Tables(0).Clone()
                        l_disbursementLoanHeading.TableName = "l_disbursementHeading"
                        l_disbursementLoanHeading.Rows.Clear()

                        Dim j As Integer
                        For j = 0 To g_dataset_dsDisbursementsByPersId.Tables(0).Rows.Count - 1
                            Dim l_row As DataRow
                            l_row = l_disbursementLoanHeading.NewRow()
                            Dim totalAmount As String
                            If j <> 0 Then
                                If Convert.ToString(g_dataset_dsDisbursementsByPersId.Tables(0).Rows(j)("Disbursementid")).Trim <> Convert.ToString(g_dataset_dsDisbursementsByPersId.Tables(0).Rows(j - 1)("Disbursementid")).Trim Then
                                    totalAmount = 0
                                    l_row = g_dataset_dsDisbursementsByPersId.Tables(0).Rows(j)
                                    l_disbursementLoanHeading.ImportRow(l_row)
                                    totalAmount = g_dataset_dsDisbursementsByPersId.Tables(0).Compute("SUM(Amount)", "Disbursementid = '" & g_dataset_dsDisbursementsByPersId.Tables(0).Rows(j)("Disbursementid").ToString.Trim & "'")
                                    l_disbursementLoanHeading.Rows(l_disbursementLoanHeading.Rows.Count - 1)("Amount") = totalAmount
                                End If
                            Else
                                l_row = g_dataset_dsDisbursementsByPersId.Tables(0).Rows(0)
                                l_disbursementLoanHeading.ImportRow(l_row) '  .Rows.Add(l_row)
                                totalAmount = g_dataset_dsDisbursementsByPersId.Tables(0).Compute("SUM(Amount)", "Disbursementid = '" & g_dataset_dsDisbursementsByPersId.Tables(0).Rows(j)("Disbursementid").ToString.Trim & "'")
                                l_disbursementLoanHeading.Rows(l_disbursementLoanHeading.Rows.Count - 1)("Amount") = totalAmount
                            End If
                        Next
                        g_dataset_dsDisbursementsByPersId.Tables.Add(l_disbursementLoanHeading)
                        g_dataset_dsDisbursementsByPersId.AcceptChanges()
                        Return g_dataset_dsDisbursementsByPersId
                    End If
                End If
            End If
        Catch ex As Exception
            Throw (ex)
        End Try


    End Function

    Public Shared Function FreezeUnfreeze(ByRef intLoanDetailsId As Integer, Optional ByRef decFrozenAmount As Decimal = 0, Optional ByRef strFrozendate As String = Nothing)
        Try
            Dim strToEmailId As String
            Dim strThresholdDays As String
            'Start -- Manthan Rajguru | 2016.02.18 | YRS-AT-2603 | Declared variable to store first and last name
            Dim strFirstName As String
            Dim strLastName As String
            'End -- Manthan Rajguru | 2016.02.18 | YRS-AT-2603 | Declared variable to store first and last name

            'START - ML |2019.01.17 | YRS-AT-3157 | Declared variable to store loanno, paymentcode and attachment
            Dim loanNo As String
            Dim paymentCode As String
            Dim result As YMCAObjects.ReturnObject(Of Boolean)
            Dim mailClient As New YMCARET.YmcaBusinessObject.MailUtil

            YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("Loanclass_FreezeUnfreeze", "UPDATE LOANFREEZEUNFREEZE START")
            '  Dim dictParameter As New Dictionary(Of String, String)  ' ML |2019.01.17 | YRS-AT-3157 |Commented- Not in used due to new Sendemail function implement
            'END - ML |2019.01.17 | YRS-AT-3157 | Declared variable to store loanno, paymentcode and attachment

            ''Start -- Manthan Rajguru | 2016.02.18 | YRS-AT-2603 | Added parameter to method to get first and last name and assigning values to dictParameter
            'YMCARET.YmcaBusinessObject.LoanInformationBOClass.UpdateLoanFreezeUnFreeze(intLoanDetailsId, strFrozendate, decFrozenAmount, strToEmailId, strThresholdDays, strFirstName, strLastName)
            YMCARET.YmcaBusinessObject.LoanInformationBOClass.UpdateLoanFreezeUnFreeze(intLoanDetailsId, strFrozendate, decFrozenAmount, loanNo, paymentCode)

            'If String.IsNullOrEmpty(strThresholdDays) Then
            '    Throw New Exception("'LOAN_THRESHOLD_DAYS' key not defined in atsMetaConfiguration")
            'ElseIf Not String.IsNullOrEmpty(strThresholdDays) Then
            '    dictParameter.Add("ThresholdDays", strThresholdDays)
            'End If
            'If Not String.IsNullOrEmpty(strFirstName) Then
            '    dictParameter.Add("FirstName", strFirstName)
            'End If
            'If Not String.IsNullOrEmpty(strLastName) Then
            '    dictParameter.Add("LastName", strLastName)
            'End If
            ''End -- Manthan Rajguru | 2016.02.18 | YRS-AT-2603 | Added parameter to method to get first and last name and assigning values to dictParameter

            If String.IsNullOrEmpty(loanNo) Then
                loanNo = "0"
            End If
            If String.IsNullOrEmpty(paymentCode) Then
                paymentCode = String.Empty
            End If
            'END: ML |2019.01.17 | YRS-AT-3157 | Declared variable to store loanno, paymentcode and attachment

            If decFrozenAmount = 0.0 Then
                Try
                    'START: ML |2019.01.17 | YRS-AT-3157 | Sending UNFREEZED Loan email and copying it to IDM
                    'MailUtil.SendMail(EnumEmailTemplateTypes.LOAN_UNFREEZE_PROCESS, "", strToEmailId, "", "", dictParameter, "", Nothing, Web.Mail.MailFormat.Html)
                    YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("Loanclass_FreezeUnfreeze", "SENDEMAILUNFREZZE START")
                    result = mailClient.SendLoanEmail(YMCAObjects.LoanStatus.UNFREEZE, loanNo, paymentCode, Nothing)
                    'END: ML |2019.01.17 | YRS-AT-3157 | Sending UNFREEZED Loan email and copying it to IDM
                Catch ex As Exception
                    HelperFunctions.LogException("Loanclass_FreezeUnfreeze", ex)
                    Return "UNFREEZED_EMAIL_ERROR"
                    'START: ML |2019.01.17 | YRS-AT-3157 | Finally added to save log 
                Finally
                    YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("Loanclass_FreezeUnfreeze", "SENDEMAILUNFREZZE END")
                    YMCARET.CommonUtilities.WebPerformanceTracer.EndTrace()
                    'END: ML |2019.01.17 | YRS-AT-3157 | Finally added to save log 
                End Try
                Return "UNFREEZED"
            Else
                'dictParameter.Add("Frozenamount", decFrozenAmount.ToString())
                Try
                    'START: ML |2019.01.17 | YRS-AT-3157 | Sending FREEZED Loan email and copying it to IDM
                    'MailUtil.SendMail(EnumEmailTemplateTypes.LOAN_FREEZE_PROCESS, "", strToEmailId, "", "", dictParameter, "", Nothing, Web.Mail.MailFormat.Html)  ' ML |2019.01.17 | YRS-AT-3157 |Commented- Not in used due to new Sendemail function implement
                    YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("Loanclass_FreezeUnfreeze", "SENDEMAILFREEZE START")
                    result = mailClient.SendLoanEmail(YMCAObjects.LoanStatus.FREEZE, loanNo, paymentCode, Nothing)
                    'END: ML |2019.01.17 | YRS-AT-3157 | Sending FREEZED Loan email and copying it to IDM
                Catch ex As Exception
                    HelperFunctions.LogException("Loanclass_FreezeUnfreeze", ex)
                    Return "FREEZED_EMAIL_ERROR"
                    'START: ML |2019.01.17 | YRS-AT-3157 | Finally added to save log 
                Finally
                    YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("Loanclass_FreezeUnfreeze", "SENDEMAILFREEZE END")
                    YMCARET.CommonUtilities.WebPerformanceTracer.EndTrace()
                    'END: ML |2019.01.17 | YRS-AT-3157 | Finally added to save log 
                End Try
                Return "FREEZED"
            End If

        Catch ex As Exception
            HelperFunctions.LogException("Loanclass_FreezeUnfreeze", ex)  'ML |2019.01.17 | YRS-AT-3157 | Log error
            Throw ex
            'START: ML |2019.01.17 | YRS-AT-3157 | Finally added to save log 
        Finally
            YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("Loanclass_FreezeUnfreeze", "UPDATE LOANFREEZEUNFREEZE END")
            YMCARET.CommonUtilities.WebPerformanceTracer.EndTrace()
            'END: ML |2019.01.17 | YRS-AT-3157 | Finally added to save log 
        End Try
    End Function
    Public Sub ReverseLoan()

    End Sub
    Public Sub ReplaceLoan()

    End Sub

    'Start: Bala: YRS-AT-2642: Created a common defaulted email sending function
    ''' <summary>
    ''' Sends the loan defaulted email.
    ''' </summary>
    ''' <param name="dtPersondetails"></param>
    ''' <remarks></remarks>
    'START: PPP | 07/27/2016 | YRS-AT-3058 | Need to send only email, IDM copy is not required
    'Public Sub SendLoanDefaultedClosedMail(ByVal dtPersondetails As DataTable, ByRef dtIDMFTList As DataTable) 'AA:04.12.2016 YRS-At-2830:Added to copy the file
    Public Sub SendLoanDefaultedClosedMail(ByVal dtPersondetails As DataTable)
        'Dim IDMAll As IDMforAll 'AA:04.12.2016 YRS-At-2830:Added to copy the file
        'END: PPP | 07/27/2016 | YRS-AT-3058 | Need to send only email, IDM copy is not required
        Dim objYMCAActionEntry As YMCAObjects.YMCAActionEntry 'AA:06.28.2016 YRS-AT-2830 Created an variable to add an entry in actuvuty log for every participant who are getting closed
        Try
            'If (HelperFunctions.isNonEmpty(dtPersondetails)) Then 'AA:04.12.2016 YRS-At-2830:removed because duplicate of code
            If (HelperFunctions.isNonEmpty(dtPersondetails)) Then
                'Start AA:04.12.2016 YRS-AT-2830 Added below code to close the default loan and send an email
                For Each drPersondetails As DataRow In dtPersondetails.Rows
                    If drPersondetails.Table.Columns.Contains("loanrequestid") AndAlso Not String.IsNullOrEmpty(drPersondetails("loanrequestid").ToString()) Then
                        CloseLoan(drPersondetails("loanrequestid").ToString())
                    End If
                    'Start: AA:06.28.2016 YRS-AT-2830 To Insert a record in AtsYractivty log we need to create an object of YMCAActionEntry and pass it to YMCAbussenessobject to insert in DB
                    objYMCAActionEntry = New YMCAObjects.YMCAActionEntry
                    objYMCAActionEntry.Action = "LOAN CLOSED JOB"
                    objYMCAActionEntry.ActionBy = HttpContext.Current.Session("LoginId")
                    objYMCAActionEntry.Data = ""
                    objYMCAActionEntry.EntityId = drPersondetails("loanrequestid").ToString()
                    objYMCAActionEntry.EntityType = YMCAObjects.EntityTypes.LOAN
                    objYMCAActionEntry.Module = "TDLoans"
                    objYMCAActionEntry.SuccessStatus = True
                    YMCARET.YmcaBusinessObject.LoggerBO.WriteLogDB(objYMCAActionEntry)
                    'End: AA:06.28.2016 YRS-AT-2830 To Insert a record in AtsYractivty log we need to create an object of YMCAActionEntry and pass it to YMCAbussenessobject to insert in DB

                    'START: PPP | 07/27/2016 | YRS-AT-3058 | Need to send only email, IDM copy is not required
                    'IDMAll = New IDMforAll()

                    'SendEmailToParticpantAndCopyPDFIntoIDM(drPersondetails, EnumEmailTemplateTypes.LOAN_DEFAULT_CLOSED_PERSONS, IDMAll)
                    SendEmailToParticpant(drPersondetails, EnumEmailTemplateTypes.LOAN_DEFAULT_CLOSED_PERSONS)

                    'If HelperFunctions.isNonEmpty(dtIDMFTList) Then
                    '    dtIDMFTList.ImportRow(IDMAll.SetdtFileList.Rows(0))
                    '    dtIDMFTList.ImportRow(IDMAll.SetdtFileList.Rows(1))
                    'Else
                    '    dtIDMFTList = IDMAll.SetdtFileList
                    'End If
                    'END: PPP | 07/27/2016 | YRS-AT-3058 | Need to send only email, IDM copy is not required

                Next
                'AA:04.12.2016 YRS-AT-2830 commented below lines as this email is no longer required
                'Dim dsPersondetails As DataSet = New DataSet
                'Dim dtCopyPersondetails = dtPersondetails.Copy()
                'dtCopyPersondetails.TableName = "LoanDefaultedPersonDetails"
                'dsPersondetails.Tables.Add(dtCopyPersondetails)
                'Try
                '    MailUtil.SendMail(EnumEmailTemplateTypes.LOAN_DEFAULTED_PERSONS, "", "", "", "", Nothing, "", dsPersondetails, Mail.MailFormat.Html)
                'Catch ex As Exception
                '    HelperFunctions.LogException("SendLoanPaidClosedMail_LOAN_DEFAULTED_PERSONS", ex)
                'End Try
                'End AA:04.12.2016 YRS-AT-2830 Added below code to close the default loan and send an email
            End If
            'End If
        Catch
            Throw
        End Try
    End Sub
    'End: Bala: YRS-AT-2642: Created a common defaulted email sending function

    'Start: AA : 04.12.2016 YRS-AT-2830 - Added below code to use common function 
    'To close the paid loans which all amorization records are funded 
    'send an email to lpa if particpant is active in ymca on which particpant took loan
    'copy a pdf into idm for participant
    'START: PPP | 07/27/2016 | YRS-AT-3058 | Need to send only email, IDM copy is not required
    'Public Sub SendLoanPaidClosedMail(ByVal dtPersondetails As DataTable, ByRef dtIDMFTList As DataTable)
    Public Sub SendLoanPaidClosedMail(ByVal dtPersondetails As DataTable)
        'END: PPP | 07/27/2016 | YRS-AT-3058 | Need to send only email, IDM copy is not required
        Dim strYmcaEmailAddrs As String
        'Dim IDMAll As IDMforAll 'PPP | 07/27/2016 | YRS-AT-3058 | Need to send only email, IDM copy is not required
        Dim strMessage As String
        Dim strPersId, strYmcaID As String
        Dim objYMCAActionEntry As YMCAObjects.YMCAActionEntry 'AA:06.28.2016 YRS-AT-2830 Created a variable to add an entry in YRSActivity log for every participant who are getting closed

        'START: SB | 09/05/2018 | YRS-AT-4081 | Send email to LPA when loan gets closed due to funding of last amortization 
        Dim dictParameters As Dictionary(Of String, String)  ' Declared dictonary to hold parameters required for email template
        Dim objMailUtil As MailUtil       'Creating object of MailUtil to access sendmailmessage method
        'END: SB | 09/05/2018 | YRS-AT-4081 | Send email to LPA when loan gets closed due to funding of last amortization 

        Try
            If (HelperFunctions.isNonEmpty(dtPersondetails)) Then

                For Each drPersondetails As DataRow In dtPersondetails.Rows
                    If drPersondetails.Table.Columns.Contains("loanrequestid") AndAlso Not String.IsNullOrEmpty(drPersondetails("loanrequestid").ToString()) Then
                        CloseLoan(drPersondetails("loanrequestid").ToString())
                    End If

                    'IDMAll = New IDMforAll() 'PPP | 07/27/2016 | YRS-AT-3058 | Need to send only email, IDM copy is not required

                    'START: PPP | 01/17/2019 | YRS-AT-4281 | Resetting Person and Ymca ID values, so that if new loan row values are not exists/NULL then they will not carry old loan values
                    strPersId = String.Empty
                    strYmcaID = String.Empty
                    'END: PPP | 01/17/2019 | YRS-AT-4281 | Resetting Person and Ymca ID values, so that if new loan row values are not exists/NULL then they will not carry old loan values

                    If dtPersondetails.Columns.Contains("guiPersid") AndAlso Not String.IsNullOrEmpty(drPersondetails("guiPersid").ToString()) Then
                        strPersId = drPersondetails("guiPersid").ToString()
                    End If

                    If dtPersondetails.Columns.Contains("guiYmcaid") AndAlso Not String.IsNullOrEmpty(drPersondetails("guiYmcaid").ToString()) Then
                        strYmcaID = drPersondetails("guiYmcaid").ToString()
                    End If

                    'Start: AA:06.28.2016 YRS-AT-2830 To Insert a record in YRSActivity log we need to create an object of YMCAActionEntry and pass it to YMCAbussenessobject to insert in DB
                    objYMCAActionEntry = New YMCAObjects.YMCAActionEntry
                    objYMCAActionEntry.Action = "LOAN CLOSED JOB"
                    objYMCAActionEntry.ActionBy = HttpContext.Current.Session("LoginId")
                    objYMCAActionEntry.Data = IIf(Not String.IsNullOrEmpty(strYmcaID), "YMCALetterGenerated = Yes", "YMCALetterGenerated = No")
                    objYMCAActionEntry.EntityId = drPersondetails("loanrequestid").ToString()
                    objYMCAActionEntry.EntityType = YMCAObjects.EntityTypes.LOAN
                    objYMCAActionEntry.Module = "TDLoans"
                    objYMCAActionEntry.SuccessStatus = True
                    YMCARET.YmcaBusinessObject.LoggerBO.WriteLogDB(objYMCAActionEntry)
                    'End: AA:06.28.2016 YRS-AT-2830 To Insert a record in YRSActivity log we need to create an object of YMCAActionEntry and pass it to YMCAbussenessobject to insert in DB

                    'START: PPP | 07/27/2016 | YRS-AT-3058 | Need to send only email, IDM copy is not required
                    'SendEmailToParticpantAndCopyPDFIntoIDM(drPersondetails, EnumEmailTemplateTypes.LOAN_PAID_CLOSED_PERSONS, IDMAll)
                    SendEmailToParticpant(drPersondetails, EnumEmailTemplateTypes.LOAN_PAID_CLOSED_PERSONS)

                    'If HelperFunctions.isNonEmpty(dtIDMFTList) Then
                    '    dtIDMFTList.ImportRow(IDMAll.SetdtFileList.Rows(0))
                    '    dtIDMFTList.ImportRow(IDMAll.SetdtFileList.Rows(1))
                    'Else
                    '    dtIDMFTList = IDMAll.SetdtFileList
                    'End If
                    'END: PPP | 07/27/2016 | YRS-AT-3058 | Need to send only email, IDM copy is not required

                    If Not String.IsNullOrEmpty(strYmcaID) Then
                        'START: PPP | 07/27/2016 | YRS-AT-3058 | Need to send only email, IDM copy is not required
                        'IDMAll = New IDMforAll()
                        'Call SetProperties(IDMAll, "Loan Satisfied-PA.rpt", "LNLETTR6", strYmcaID, strPersId)
                        'If IDMAll.DatatableFileList(False) = False Then
                        '    Dim ex As New Exception("Unable to Process Loan Letters, Could not create dependent table")
                        '    HelperFunctions.LogException("Unable to Process Loan Letters, Could not create dependent table", ex)
                        '    Throw ex
                        'End If
                        'Try
                        '    strMessage = IDMAll.ExportToPDF
                        'Catch ex As Exception
                        '    HelperFunctions.LogException("SendLoanPaidClosedMail_Loan Satisfied-PA", ex)
                        'End Try
                        'If HelperFunctions.isNonEmpty(dtIDMFTList) Then
                        '    dtIDMFTList.ImportRow(IDMAll.SetdtFileList.Rows(0))
                        '    dtIDMFTList.ImportRow(IDMAll.SetdtFileList.Rows(1))
                        'Else
                        '    dtIDMFTList = IDMAll.SetdtFileList
                        'End If
                        'END: PPP | 07/27/2016 | YRS-AT-3058 | Need to send only email, IDM copy is not required

                        'Sending an email to LPA with attachment
                        strYmcaEmailAddrs = YMCARET.YmcaBusinessObject.LoanInformationBOClass.GetYMCAEmailId(strYmcaID)
                        If Not String.IsNullOrEmpty(strYmcaEmailAddrs) AndAlso String.IsNullOrEmpty(strMessage) Then
                            Try
                                'START: SB | 09/03/2018 | YRS-AT-4081 | Preparing paramters to be replaced in email template and sending email to LPA and then to IDM
                                '  MailUtil.SendMail(EnumEmailTemplateTypes.LOAN_CLOSED_INTIMATION, "", strYmcaEmailAddrs, "", "", Nothing, HttpContext.Current.Session("StringDestFilePath"), Nothing, Mail.MailFormat.Html)
                                If drPersondetails.Table.Columns.Contains("ToEmail") AndAlso Not String.IsNullOrEmpty(drPersondetails("ToEmail").ToString()) Then
                                    dictParameters = New Dictionary(Of String, String)

                                    If drPersondetails.Table.Columns.Contains("Last Name") AndAlso Not String.IsNullOrEmpty(drPersondetails("Last Name").ToString()) Then
                                        dictParameters.Add("ParticipantLastName", drPersondetails("Last Name"))
                                    End If
                                    If drPersondetails.Table.Columns.Contains("First Name") AndAlso Not String.IsNullOrEmpty(drPersondetails("First Name").ToString()) Then
                                        dictParameters.Add("ParticipantFirstName", drPersondetails("First Name"))
                                    End If
                                    If drPersondetails.Table.Columns.Contains("chvLPALastName") AndAlso Not String.IsNullOrEmpty(drPersondetails("chvLPALastName").ToString()) Then
                                        dictParameters.Add("LPALastName", drPersondetails("chvLPALastName"))
                                    End If
                                    If drPersondetails.Table.Columns.Contains("chvLPAFirstName") AndAlso Not String.IsNullOrEmpty(drPersondetails("chvLPAFirstName").ToString()) Then
                                        dictParameters.Add("LPAFirstName", drPersondetails("chvLPAFirstName"))
                                    End If
                                    If drPersondetails.Table.Columns.Contains("LastPaymentDate") AndAlso Not String.IsNullOrEmpty(drPersondetails("LastPaymentDate").ToString()) Then
                                        dictParameters.Add("LastPaymentDate", drPersondetails("LastPaymentDate"))
                                    End If
                                End If
                                objMailUtil = New MailUtil
                                objMailUtil.SendMailMessage(EnumEmailTemplateTypes.LOAN_CLOSED_INTIMATION, "", strYmcaEmailAddrs, "", "", dictParameters, Nothing, Nothing, Mail.MailFormat.Html)
                                'Check mail message is not empty before copying into IDM
                                If Not String.IsNullOrEmpty(objMailUtil.MailMessage) Then
                                    ServiceManager.DMSService.SendEmailCopyToIDM(strYmcaID, objMailUtil.MailMessage)
                                End If
                                'END: SB | 09/03/2018 | YRS-AT-4081 | Preparing paramters to be replaced in email template and sending email to LPA and then to IDM
                            Catch ex As Exception
                                HelperFunctions.LogException("SendLoanPaidClosedMail_LOAN_CLOSED_INTIMATION", ex)
                            End Try
                        End If
                    End If
                Next
            End If

        Catch
            Throw
        End Try
    End Sub

    Public Sub CloseLoan(ByVal strLoanRequestId As String)
        Try
            YMCARET.YmcaBusinessObject.LoanInformationBOClass.CancelLoanUpdateAmortization("CLOSED", "PAYOFF", strLoanRequestId, " ", " ", " ", Nothing, 0, 0)
        Catch
            Throw
        End Try
    End Sub

    'START: PPP | 07/27/2016 | YRS-AT-3058 | Need to send only email, IDM copy is not required. So changing function name also.
    'Public Sub SendEmailToParticpantAndCopyPDFIntoIDM(ByVal drPersondetails As DataRow, ByVal EnumEmailTemplate As EnumEmailTemplateTypes, ByRef IDMAll As IDMforAll)
    Public Sub SendEmailToParticpant(ByVal drPersondetails As DataRow, ByVal EnumEmailTemplate As EnumEmailTemplateTypes)
        'END: PPP | 07/27/2016 | YRS-AT-3058 | Need to send only email, IDM copy is not required. So changing function name also.
        Dim dictParameters As Dictionary(Of String, String)
        Dim strPersId, strLoanNumber As String
        Try
            If drPersondetails.Table.Columns.Contains("ToEmail") AndAlso Not String.IsNullOrEmpty(drPersondetails("ToEmail").ToString()) Then
                dictParameters = New Dictionary(Of String, String)

                If drPersondetails.Table.Columns.Contains("Last Name") AndAlso Not String.IsNullOrEmpty(drPersondetails("Last Name").ToString()) Then
                    dictParameters.Add("LastName", drPersondetails("Last Name"))
                End If
                If drPersondetails.Table.Columns.Contains("First Name") AndAlso Not String.IsNullOrEmpty(drPersondetails("First Name").ToString()) Then
                    dictParameters.Add("FirstName", drPersondetails("First Name"))
                End If
                Try
                    MailUtil.SendMail(EnumEmailTemplate, "", drPersondetails("ToEmail"), "", "", dictParameters, "", Nothing, Mail.MailFormat.Html)
                Catch ex As Exception
                    HelperFunctions.LogException("SendLoanPaidClosedMail_LOAN_DEFAULT_CLOSED_PERSONS", ex)
                End Try
            End If

            If drPersondetails.Table.Columns.Contains("guiPersid") AndAlso Not String.IsNullOrEmpty(drPersondetails("guiPersid").ToString()) Then
                strPersId = drPersondetails("guiPersid").ToString()
            End If

            If drPersondetails.Table.Columns.Contains("chvLoanNumber") AndAlso Not String.IsNullOrEmpty(drPersondetails("chvLoanNumber").ToString()) Then
                strLoanNumber = drPersondetails("chvLoanNumber").ToString()
            End If

            'START: PPP | 07/27/2016 | YRS-AT-3058 | Not saving letter into IDM.
            'If Not String.IsNullOrEmpty(strPersId) Then
            '    Call SetProperties(IDMAll, "Loan Satisfied-part.rpt", "LNLETTR5", strPersId, strPersId, strLoanNumber)
            '    If IDMAll.DatatableFileList(True) = False Then
            '        Dim ex As New Exception("Unable to Process Loan Letters, Could not create dependent table")
            '        HelperFunctions.LogException("Unable to Process Loan Letters, Could not create dependent table", ex)
            '        Throw ex
            '    End If
            '    Try
            '        IDMAll.ExportToPDF()
            '    Catch ex As Exception
            '        HelperFunctions.LogException("SendLoanPaidClosedMail_Loan Satisfied-part", ex)
            '    End Try
            'End If
            'END: PPP | 07/27/2016 | YRS-AT-3058 | Not saving letter into IDM.

        Catch
            Throw
        End Try
    End Sub

    'START: PPP | 07/27/2016 | YRS-AT-3058 | Not saving letter into IDM. So IDM property setting function not required.
    'Private Function SetProperties(ByVal IDMAll As IDMforAll, ByVal strReportname As String, ByVal strlnletter As String, ByVal strEntityID As String, ByVal strPersID As String, Optional ByRef strLoanNumber As String = "") As String
    '    'Mofifed By Ashutosh Patil as on 18-Apr-2007
    '    'For Loan Report
    '    Dim par_Arrylist As ArrayList
    '    Try
    '        par_Arrylist = New ArrayList
    '        IDMAll.PreviewReport = True
    '        IDMAll.LogonToDb = True
    '        IDMAll.CreatePDF = True
    '        IDMAll.CreateIDX = True
    '        IDMAll.CopyFilesToIDM = True

    '        IDMAll.ReportName = strReportname.ToString
    '        IDMAll.DocTypeCode = strlnletter.ToString
    '        IDMAll.OutputFileType = strlnletter.ToString

    '        par_Arrylist.Add(CType(strPersID, String).ToString.Trim)

    '        If strlnletter.ToString = "LNLETTR5" Then
    '            IDMAll.PersId = strEntityID
    '            IDMAll.AppType = "P"
    '            par_Arrylist.Add(CType(strLoanNumber, String).ToString.Trim)
    '        Else
    '            IDMAll.AppType = "A"
    '            IDMAll.YMCAID = strEntityID
    '        End If

    '        IDMAll.ReportParameters = par_Arrylist
    '    Catch ex As Exception
    '        HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
    '        HelperFunctions.LogException("LoanOptions_" + "SetProperties", ex)
    '    End Try
    'End Function
    'END: PPP | 07/27/2016 | YRS-AT-3058 | Not saving letter into IDM. So IDM property setting function not required.
End Class
'End: AA : 04.12.2016 YRS-AT-2830 
Public Class AnnuityClass
    Public Sub New()
        MyBase.New()
    End Sub
    Public g_dataset_dsDisbursements As DataSet
    Dim g_dataset_dsDisbursementsByPersId As DataSet
    'Commented by imran on  29/10/2009 for UnGrouping Disbursement list

    'Public Function GetLoanDisbursementByPersid(ByVal strPersId As String, ByVal strActivity As String) As DataSet
    '    Dim l_string_PersId As String
    '    Dim l_integer_GridIndex As Integer
    '    'Dim l_string_Activity As String
    '    Dim l_integer_AnnuityOnly As Integer
    '    Dim l_datarow_CurrentRow As DataRow
    '    'Aparna Samala -22/09/2006
    '    Dim l_searchExpr As String
    '    Dim l_dr_CurrentRow() As DataRow
    '    Try
    '        If (strActivity = "Void") Or (strActivity = "VReplace") Then
    '            l_integer_AnnuityOnly = 1
    '        ElseIf (strActivity = "LOANREVERSE") Then
    '            l_integer_AnnuityOnly = 2
    '        ElseIf (strActivity = "LOANREPLACE") Then
    '            l_integer_AnnuityOnly = 21
    '        Else
    '            l_integer_AnnuityOnly = 0
    '        End If

    '        g_dataset_dsDisbursementsByPersId = YMCARET.YmcaBusinessObject.VoidDisbursementVRManagerBOClass.GetDisbursementsByPersId(strPersId, l_integer_AnnuityOnly)
    '        If Not IsNothing(g_dataset_dsDisbursementsByPersId) Then
    '            If Not IsNothing(g_dataset_dsDisbursementsByPersId.Tables(0)) Then
    '                If g_dataset_dsDisbursementsByPersId.Tables(0).Rows.Count = 0 Then
    '                    Return Nothing
    '                Else
    '                    Dim l_disbursementLoanHeading As DataTable
    '                    l_disbursementLoanHeading = g_dataset_dsDisbursementsByPersId.Tables(0).Clone()
    '                    l_disbursementLoanHeading.TableName = "l_disbursementHeading"
    '                    l_disbursementLoanHeading.Rows.Clear()

    '                    Dim j As Integer
    '                    For j = 0 To g_dataset_dsDisbursementsByPersId.Tables(0).Rows.Count - 1
    '                        Dim l_row As DataRow
    '                        l_row = l_disbursementLoanHeading.NewRow()
    '                        Dim totalAmount As String
    '                        If j <> 0 Then
    '                            If Convert.ToString(g_dataset_dsDisbursementsByPersId.Tables(0).Rows(j)("Number")).Trim <> Convert.ToString(g_dataset_dsDisbursementsByPersId.Tables(0).Rows(j - 1)("Number")).Trim Then
    '                                totalAmount = 0
    '                                l_row = g_dataset_dsDisbursementsByPersId.Tables(0).Rows(j)
    '                                l_disbursementLoanHeading.ImportRow(l_row)
    '                                totalAmount = g_dataset_dsDisbursementsByPersId.Tables(0).Compute("SUM(Amount)", "Number = '" & g_dataset_dsDisbursementsByPersId.Tables(0).Rows(j)("Number").ToString.Trim & "'")
    '                                l_disbursementLoanHeading.Rows(l_disbursementLoanHeading.Rows.Count - 1)("Amount") = totalAmount
    '                            End If
    '                        Else
    '                            l_row = g_dataset_dsDisbursementsByPersId.Tables(0).Rows(0)
    '                            l_disbursementLoanHeading.ImportRow(l_row) '  .Rows.Add(l_row)
    '                            totalAmount = g_dataset_dsDisbursementsByPersId.Tables(0).Compute("SUM(Amount)", "Number = '" & g_dataset_dsDisbursementsByPersId.Tables(0).Rows(j)("Number").ToString.Trim & "'")
    '                            l_disbursementLoanHeading.Rows(l_disbursementLoanHeading.Rows.Count - 1)("Amount") = totalAmount
    '                        End If
    '                    Next
    '                    g_dataset_dsDisbursementsByPersId.Tables.Add(l_disbursementLoanHeading)
    '                    g_dataset_dsDisbursementsByPersId.AcceptChanges()
    '                    Return g_dataset_dsDisbursementsByPersId
    '                End If
    '            End If
    '        End If
    '    Catch ex As Exception
    '        Throw (ex)
    '    End Try

    '    'Dim l_disbursementLoanHeading As DataSet
    '    'l_disbursementLoanHeading = g_dataset_dsDisbursementsByPersId.Clone()
    '    'l_disbursementLoanHeading.Tables(0).Rows.Clear()

    '    'Dim j As Integer
    '    'For j = 0 To g_dataset_dsDisbursementsByPersId.Tables(0).Rows.Count - 1
    '    '    Dim l_row As DataRow
    '    '    l_row = l_disbursementLoanHeading.Tables(0).NewRow()
    '    '    Dim totalAmount As String
    '    '    If j <> 0 Then
    '    '        If Convert.ToString(g_dataset_dsDisbursementsByPersId.Tables(0).Rows(j)("Number")).Trim <> Convert.ToString(g_dataset_dsDisbursementsByPersId.Tables(0).Rows(j - 1)("Number")).Trim Then
    '    '            totalAmount = 0
    '    '            l_row = g_dataset_dsDisbursementsByPersId.Tables(0).Rows(j)
    '    '            l_disbursementLoanHeading.Tables(0).ImportRow(l_row)
    '    '            totalAmount = g_dataset_dsDisbursementsByPersId.Tables(0).Compute("SUM(Amount)", "Number = '" & g_dataset_dsDisbursementsByPersId.Tables(0).Rows(j)("Number").ToString.Trim & "'")
    '    '            l_disbursementLoanHeading.Tables(0).Rows(l_disbursementLoanHeading.Tables(0).Rows.Count - 1)("Amount") = totalAmount
    '    '        End If
    '    '    Else
    '    '        l_row = g_dataset_dsDisbursementsByPersId.Tables(0).Rows(0)
    '    '        l_disbursementLoanHeading.Tables(0).ImportRow(l_row) '  .Rows.Add(l_row)
    '    '        totalAmount = g_dataset_dsDisbursementsByPersId.Tables(0).Compute("SUM(Amount)", "Number = '" & g_dataset_dsDisbursementsByPersId.Tables(0).Rows(j)("Number").ToString.Trim & "'")
    '    '        l_disbursementLoanHeading.Tables(0).Rows(l_disbursementLoanHeading.Tables(0).Rows.Count - 1)("Amount") = totalAmount
    '    '    End If
    '    'Next
    '    'Datalist3.DataSource = l_disbursementLoanHeading
    '    'Datalist3.DataBind()
    '    'End If

    '    'If Not (Session("DisbursementByPersIdIndex") > 0) Then
    '    'Session("DisbursementByPersIdIndex") = 0
    '    'End If

    '    'PopulateDisbursementTextBoxes()
    '    'Me.LabelNoRecordFound.Visible = False
    '    'Me.LabelPayeeSSN.Text = l_datarow_CurrentRow("SSN").ToString + "-" + l_datarow_CurrentRow("FirstName").ToString + " " + l_datarow_CurrentRow("MiddleName").ToString + " " + l_datarow_CurrentRow("LastName").ToString
    'End Function
    'Public Function GetAnnuityDisbursementByPersid(ByVal strPersId As String, ByVal strActivity As String) As DataSet
    '    Dim l_string_PersId As String
    '    Dim l_integer_GridIndex As Integer

    '    Dim l_integer_AnnuityOnly As Integer
    '    Dim l_datarow_CurrentRow As DataRow

    '    Dim l_searchExpr As String
    '    Dim l_dr_CurrentRow() As DataRow

    '    Try


    '        If (strActivity = "Void") Or (strActivity = "VReplace") Then
    '            l_integer_AnnuityOnly = 1
    '        ElseIf (strActivity = "LOANREVERSE") Then
    '            l_integer_AnnuityOnly = 2
    '        ElseIf (strActivity = "LOANREPLACE") Then
    '            l_integer_AnnuityOnly = 21
    '        ElseIf (strActivity = "VREVERSE") Then
    '            l_integer_AnnuityOnly = 1
    '        Else
    '            l_integer_AnnuityOnly = 0
    '        End If

    '        g_dataset_dsDisbursementsByPersId = YMCARET.YmcaBusinessObject.VoidDisbursementVRManagerBOClass.GetDisbursementsByPersId(strPersId, l_integer_AnnuityOnly)
    '        If Not IsNothing(g_dataset_dsDisbursementsByPersId) Then
    '            If Not IsNothing(g_dataset_dsDisbursementsByPersId.Tables(0)) Then
    '                If g_dataset_dsDisbursementsByPersId.Tables(0).Rows.Count = 0 Then
    '                    Return Nothing
    '                Else
    '                    Dim l_disbursementHeading As DataTable
    '                    l_disbursementHeading = g_dataset_dsDisbursementsByPersId.Tables(0).Clone()
    '                    l_disbursementHeading.TableName = "l_disbursementHeading"
    '                    l_disbursementHeading.Rows.Clear()

    '                    Dim i As Integer
    '                    For i = 0 To g_dataset_dsDisbursementsByPersId.Tables(0).Rows.Count - 1
    '                        Dim l_row As DataRow
    '                        l_row = l_disbursementHeading.NewRow()
    '                        Dim totalAmount As String
    '                        If i <> 0 Then
    '                            If Convert.ToString(g_dataset_dsDisbursementsByPersId.Tables(0).Rows(i)("RequestId")).Trim <> Convert.ToString(g_dataset_dsDisbursementsByPersId.Tables(0).Rows(i - 1)("RequestId")).Trim Then
    '                                totalAmount = 0
    '                                l_row = g_dataset_dsDisbursementsByPersId.Tables(0).Rows(i)
    '                                l_disbursementHeading.ImportRow(l_row)
    '                                'added condition  by imran
    '                                If g_dataset_dsDisbursementsByPersId.Tables(0).Rows(i)("RequestId").ToString.Trim <> "" Then

    '                                    totalAmount = g_dataset_dsDisbursementsByPersId.Tables(0).Compute("SUM(Amount)", "RequestId = '" & g_dataset_dsDisbursementsByPersId.Tables(0).Rows(i)("RequestId").ToString.Trim & "'")
    '                                    l_disbursementHeading.Rows(l_disbursementHeading.Rows.Count - 1)("Amount") = totalAmount

    '                                End If

    '                            End If
    '                        Else
    '                            l_row = g_dataset_dsDisbursementsByPersId.Tables(0).Rows(i)
    '                            l_disbursementHeading.ImportRow(l_row) '  .Rows.Add(l_row)

    '                            'added condition  by imran
    '                            If g_dataset_dsDisbursementsByPersId.Tables(0).Rows(i)("RequestId").ToString.Trim <> "" Then
    '                                totalAmount = g_dataset_dsDisbursementsByPersId.Tables(0).Compute("SUM(Amount)", "RequestId = '" & g_dataset_dsDisbursementsByPersId.Tables(0).Rows(i)("RequestId").ToString.Trim & "'")
    '                                l_disbursementHeading.Rows(l_disbursementHeading.Rows.Count - 1)("Amount") = totalAmount
    '                            End If

    '                        End If
    '                    Next
    '                    g_dataset_dsDisbursementsByPersId.Tables.Add(l_disbursementHeading)
    '                    g_dataset_dsDisbursementsByPersId.AcceptChanges()
    '                    '.Tables.Add(l_disbursementHeading)
    '                    Return g_dataset_dsDisbursementsByPersId

    '                End If


    '            End If
    '        End If

    '    Catch
    '        Throw
    '    End Try
    'End Function

    Public Function GetAnnuityDisbursementByPersid(ByVal strPersId As String, ByVal strActivity As String) As DataSet
        Dim l_string_PersId As String
        Dim l_integer_GridIndex As Integer

        Dim l_integer_AnnuityOnly As Integer
        Dim l_datarow_CurrentRow As DataRow

        Dim l_searchExpr As String
        Dim l_dr_CurrentRow() As DataRow

        Try


            If (strActivity = "Void") Or (strActivity = "VReplace") Then
                l_integer_AnnuityOnly = 1
            ElseIf (strActivity = "LOANREVERSE") Then
                l_integer_AnnuityOnly = 2
            ElseIf (strActivity = "LOANREPLACE") Then
                l_integer_AnnuityOnly = 21
            ElseIf (strActivity = "VREVERSE") Then
                l_integer_AnnuityOnly = 1
            Else
                l_integer_AnnuityOnly = 0
            End If

            g_dataset_dsDisbursementsByPersId = YMCARET.YmcaBusinessObject.VoidDisbursementVRManagerBOClass.GetDisbursementsByPersId(strPersId, l_integer_AnnuityOnly)
            If Not IsNothing(g_dataset_dsDisbursementsByPersId) Then
                If Not IsNothing(g_dataset_dsDisbursementsByPersId.Tables(0)) Then
                    If g_dataset_dsDisbursementsByPersId.Tables(0).Rows.Count = 0 Then
                        Return Nothing
                    Else
                        Dim l_disbursementHeading As DataTable
                        l_disbursementHeading = g_dataset_dsDisbursementsByPersId.Tables(0).Clone()
                        l_disbursementHeading.TableName = "l_disbursementHeading"
                        l_disbursementHeading.Rows.Clear()

                        Dim i As Integer
                        For i = 0 To g_dataset_dsDisbursementsByPersId.Tables(0).Rows.Count - 1
                            Dim l_row As DataRow
                            l_row = l_disbursementHeading.NewRow()
                            Dim totalAmount As String
                            If i <> 0 Then
                                If Convert.ToString(g_dataset_dsDisbursementsByPersId.Tables(0).Rows(i)("Disbursementid")).Trim <> Convert.ToString(g_dataset_dsDisbursementsByPersId.Tables(0).Rows(i - 1)("Disbursementid")).Trim Then
                                    totalAmount = 0
                                    l_row = g_dataset_dsDisbursementsByPersId.Tables(0).Rows(i)
                                    l_disbursementHeading.ImportRow(l_row)
                                    'added condition  by imran
                                    If g_dataset_dsDisbursementsByPersId.Tables(0).Rows(i)("Disbursementid").ToString.Trim <> "" Then

                                        'totalAmount = g_dataset_dsDisbursementsByPersId.Tables(0).Compute("SUM(Amount)", "RequestId = '" & g_dataset_dsDisbursementsByPersId.Tables(0).Rows(i)("RequestId").ToString.Trim & "'")
                                        totalAmount = g_dataset_dsDisbursementsByPersId.Tables(0).Compute("SUM(Amount)", "Disbursementid = '" & g_dataset_dsDisbursementsByPersId.Tables(0).Rows(i)("Disbursementid").ToString.Trim & "'")
                                        l_disbursementHeading.Rows(l_disbursementHeading.Rows.Count - 1)("Amount") = totalAmount

                                    End If

                                End If
                            Else
                                l_row = g_dataset_dsDisbursementsByPersId.Tables(0).Rows(i)
                                l_disbursementHeading.ImportRow(l_row) '  .Rows.Add(l_row)

                                'added condition  by imran
                                If g_dataset_dsDisbursementsByPersId.Tables(0).Rows(i)("Disbursementid").ToString.Trim <> "" Then
                                    totalAmount = g_dataset_dsDisbursementsByPersId.Tables(0).Compute("SUM(Amount)", "Disbursementid = '" & g_dataset_dsDisbursementsByPersId.Tables(0).Rows(i)("Disbursementid").ToString.Trim & "'")
                                    l_disbursementHeading.Rows(l_disbursementHeading.Rows.Count - 1)("Amount") = totalAmount
                                End If

                            End If
                        Next
                        g_dataset_dsDisbursementsByPersId.Tables.Add(l_disbursementHeading)
                        g_dataset_dsDisbursementsByPersId.AcceptChanges()
                        '.Tables.Add(l_disbursementHeading)
                        Return g_dataset_dsDisbursementsByPersId

                    End If


                End If
            End If

        Catch
            Throw
        End Try
    End Function


End Class

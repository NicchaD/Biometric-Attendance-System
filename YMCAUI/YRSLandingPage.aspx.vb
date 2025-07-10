'*******************************************************************************
' Copyright YMCA Retirement Fund All Rights Reserved. 
'
' Project Name		:	YMCA_YRS
' FileName			:	YRSLandingPage.aspx.vb
' Author Name		:	Deven
' Creation Time		:	13 Aug 2010
' Description		:	Implements logic to login user seamlessly from other web site. 
'
' Changed Hisorty
'------------   ----------------    ---------------------------------------------------
'Author         Date               Description
'------------   ----------------    ---------------------------------------------------
'Shashi Shekhar 14- Feb 2011     Integrate and make some changes in code or done validation as per new PS requirement.
'Sanjay R        2013.07.10       BT-2117/YRS 5.0-2152 : The link from the Employment tab to YMCA Maintenance is broken 
'Anudeep A       2016.02.17      YRS-AT-2640 - YRS enh: Withdrawals Phase2:Sprint2: allow AdminTool link to launch a prepopulated Yrs withdrawal page
'*******************************************************************************
Imports System
Imports System.Collections
Imports System.ComponentModel
Imports System.Data
Imports System.Security
Imports System.Web
Imports System.Web.SessionState
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.HtmlControls
Imports System.Data.DataRow
Imports System.Security.Permissions
Imports YMCAObjects
Imports YMCAObjects.MetaMessageList

Public Class YRSLandingPage
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try

            If Session("LoggedUserKey") Is Nothing Then
                Dim l_stLoginPage As String = "Login.aspx?" & Convert.ToString(Request.QueryString)
                Response.Redirect(l_stLoginPage, False)
            ElseIf Not Request.QueryString("PageType") Is Nothing _
                And Not Request.QueryString("DataType") Is Nothing _
                And Not Request.QueryString("Value") Is Nothing Then
                Dim l_stPageType As String = Request.QueryString("PageType")
                Dim l_stDataType As String = Request.QueryString("DataType")
                Dim l_stValue As String = Request.QueryString("Value")

                'SS:Validating guid format
                If ((l_stDataType.Trim.ToLower = "persid") Or (l_stDataType.Trim.ToLower = "fundeventid") Or (l_stDataType.Trim.ToLower = "ymcaid") Or (l_stDataType.Trim.ToLower = "refrequestid")) Then
                    If HelperFunctions.IsValidGuid(l_stValue.Trim) = False Then
                        'Start: AA:02.17.2016 YRS-AT-2640 Changed the method to accept error message from outside Private Sub RedirectToErrorPage()
                        'RedirectToErrorPage()
                        RedirectToErrorPage(MESSAGE_GEN_LANDINGPAGE_INVALID_DATATYPE_VALUE, New Dictionary(Of String, String)() From {{"Datatype", l_stDataType}})
                        'End: AA:02.17.2016 YRS-AT-2640 Changed the method to accept error message from outside Private Sub RedirectToErrorPage()
                        Exit Sub
                    End If
                End If

                'SS:Validating SSN
                If l_stDataType.Trim.ToLower = "ssno" Then
                    If HelperFunctions.IsValidSSN(l_stValue.Trim) = False Then
                        'Start: AA:02.17.2016 YRS-AT-2640 Changed the method to accept error message from outside Private Sub RedirectToErrorPage()
                        'RedirectToErrorPage()
                        RedirectToErrorPage(MESSAGE_GEN_LANDINGPAGE_INVALID_DATATYPE_VALUE, New Dictionary(Of String, String)() From {{"Datatype", l_stDataType}})
                        'End: AA:02.17.2016 YRS-AT-2640 Changed the method to accept error message from outside Private Sub RedirectToErrorPage()
                        Exit Sub
                    End If
                End If

                'SS:Validating Fund No
                If l_stDataType.Trim.ToLower = "fundno" Then
                    If HelperFunctions.IsValidFundNo(l_stValue.Trim) = False Then
                        'Start: AA:02.17.2016 YRS-AT-2640 Changed the method to accept error message from outside Private Sub RedirectToErrorPage()
                        'RedirectToErrorPage()
                        RedirectToErrorPage(MESSAGE_GEN_LANDINGPAGE_INVALID_DATATYPE_VALUE, New Dictionary(Of String, String)() From {{"Datatype", l_stDataType}})
                        'End: AA:02.17.2016 YRS-AT-2640 Changed the method to accept error message from outside Private Sub RedirectToErrorPage()
                        Exit Sub
                    End If
                End If

                'SS:Validating YMCA No
                If l_stDataType.Trim.ToLower = "ymcano" Then
                    'If l_stValue.Trim.Length < 6 Then
                    If HelperFunctions.IsValidFundNo(l_stValue.Trim) = False Then
                        'Start: AA:02.17.2016 YRS-AT-2640 Changed the method to accept error message from outside Private Sub RedirectToErrorPage()
                        'RedirectToErrorPage()
                        RedirectToErrorPage(MESSAGE_GEN_LANDINGPAGE_INVALID_DATATYPE_VALUE, New Dictionary(Of String, String)() From {{"Datatype", l_stDataType}})
                        'End: AA:02.17.2016 YRS-AT-2640 Changed the method to accept error message from outside Private Sub RedirectToErrorPage()
                        Exit Sub
                    End If
                End If




                If Not String.IsNullOrEmpty(l_stPageType) And Not String.IsNullOrEmpty(l_stDataType) And Not String.IsNullOrEmpty(l_stValue) Then
                    l_stDataType = l_stDataType.ToLower()
                    Dim l_stArrDataType() As String = l_stDataType.Split(",")
                    Dim l_stArrValue() As String = l_stValue.Split(",")
                    If l_stArrDataType.Length = l_stArrValue.Length Then
                        Dim l_stlstDataInfo As List(Of String) = l_stArrDataType.ToList()
                        Dim l_stlstValue As List(Of String) = l_stArrValue.ToList()
                        ' Following If statement works for Person maintenance 
                        If (l_stPageType.ToLower() = "personmaintenance") Then
                            Session("Seamless_From") = "personmaintenance"

                            If l_stlstDataInfo.IndexOf(l_stDataType.ToLower.Trim) <> -1 And l_stDataType.ToLower.Trim = "ssno" Then
                                Session("Seamless_SSN") = l_stlstValue(l_stlstDataInfo.IndexOf("ssno"))

                            ElseIf l_stlstDataInfo.IndexOf(l_stDataType.ToLower.Trim) <> -1 And l_stDataType.ToLower.Trim = "fundno" Then
                                Session("Seamless_Fund") = l_stlstValue(l_stlstDataInfo.IndexOf("fundno"))

                            ElseIf l_stlstDataInfo.IndexOf(l_stDataType.ToLower.Trim) <> -1 And l_stDataType.ToLower.Trim = "persid" Then
                                Session("Seamless_GuiPersId") = l_stlstValue(l_stlstDataInfo.IndexOf("persid"))

                            ElseIf l_stlstDataInfo.IndexOf(l_stDataType.ToLower.Trim) <> -1 And l_stDataType.ToLower.Trim = "fundeventid" Then
                                Session("Seamless_guiFundEventID") = l_stlstValue(l_stlstDataInfo.IndexOf("fundeventid"))
                            Else
                                ' No Data Type matched
                                'Start: AA:02.17.2016 YRS-AT-2640 Changed the method to accept error message from outside Private Sub RedirectToErrorPage()
                                'RedirectToErrorPage()
                                RedirectToErrorPage(MESSAGE_GEN_LANDINGPAGE_INVALID_DATATYPE_FOR_PERSONMAINTENANCE)
                                'End: AA:02.17.2016 YRS-AT-2640 Changed the method to accept error message from outside Private Sub RedirectToErrorPage()
                                Exit Sub
                            End If

                            Response.Redirect("SecurityCheck.aspx?Form=FindInfo.aspx?Name=Person", False)

                            ' Following If statement works for YMCA maintenance 
                        ElseIf l_stPageType.ToLower() = "ymcamaintenance" Then
                            Session("Seamless_FromLandingPage") = "True"  'SR:2013.07.10 - BT-2117/YRS 5.0-2152 : The link from the Employment tab to YMCA Maintenance is broken 
                            Session("Seamless_From") = "ymcamaintenance"
                            If l_stlstDataInfo.IndexOf(l_stDataType.ToLower.Trim) <> -1 And l_stDataType.ToLower.Trim = "ymcano" Then
                                Session("Seamless_YMCANo") = l_stlstValue(l_stlstDataInfo.IndexOf("ymcano"))

                            ElseIf l_stlstDataInfo.IndexOf(l_stDataType.ToLower.Trim) <> -1 And l_stDataType.ToLower.Trim = "ymcaid" Then
                                Session("Seamless_guiYMCAID") = l_stlstValue(l_stlstDataInfo.IndexOf("ymcaid"))
                            Else
                                ' No Data Type matched
                                'Start: AA:02.17.2016 YRS-AT-2640 Changed the method to accept error message from outside Private Sub RedirectToErrorPage()
                                'RedirectToErrorPage()
                                RedirectToErrorPage(MESSAGE_GEN_LANDINGPAGE_INVALID_DATATYPE_FOR_YMCAMAINTENANCE)
                                'End: AA:02.17.2016 YRS-AT-2640 Changed the method to accept error message from outside Private Sub RedirectToErrorPage()
                                Exit Sub
                            End If
                            Response.Redirect("SecurityCheck.aspx?Form=YMCAWebForm.aspx", False)
                            'Start: AA:02.17.2016 YRS-AT-2640 Added a new if condition to open withdrawal processing page
                        ElseIf l_stPageType.ToLower() = "withdrawals" Then
                            Session("Seamless_From") = "Withdrawals"

                            If l_stlstDataInfo.IndexOf(l_stDataType.ToLower.Trim) <> -1 And l_stDataType.ToLower.Trim = "refrequestid" Then
                                Dim strFundEventID As String
                                strFundEventID = YMCARET.YmcaBusinessObject.RefundRequest.GetFundEventIDFromRefrequestID(l_stlstValue(l_stlstDataInfo.IndexOf("refrequestid")))
                                If strFundEventID = "0" Then
                                    RedirectToErrorPage(MESSAGE_GEN_LANDINGPAGE_INVALID_REFREQUESTID_VALUE)
                                    Exit Sub

                                ElseIf strFundEventID = "-1" Then
                                    RedirectToErrorPage(MESSAGE_GEN_LANDINGPAGE_ERROR)
                                    Exit Sub
                                Else
                                    Session("Seamless_guiFundEventID") = strFundEventID
                                    Session("Seamless_refrequestid") = l_stlstValue(l_stlstDataInfo.IndexOf("refrequestid"))
                                    Response.Redirect("SecurityCheck.aspx?Form=FindInfo.aspx?Name=Refund", False)
                                End If
                            Else
                                RedirectToErrorPage(MESSAGE_GEN_LANDINGPAGE_INVALID_DATATYPE_FOR_WITHDRAWALPROCESSSING)
                                Exit Sub
                            End If
                            'End: AA:02.17.2016 YRS-AT-2640 Added a new if condition to open withdrawal processing page
                        Else
                            ' No Page Type matched
                            'Start: AA:02.17.2016 YRS-AT-2640 Changed the method to accept error message from outside Private Sub RedirectToErrorPage()
                            'RedirectToErrorPage()
                            RedirectToErrorPage(MESSAGE_GEN_LANDINGPAGE_INVALID_PAGETYPE)
                            'End: AA:02.17.2016 YRS-AT-2640 Changed the method to accept error message from outside Private Sub RedirectToErrorPage()
                        End If
                    Else
                        ' Length of parameters in QueryString DataType and Value is not same.
                        'Start: AA:02.17.2016 YRS-AT-2640 Changed the method to accept error message from outside Private Sub RedirectToErrorPage()
                        'RedirectToErrorPage()
                        RedirectToErrorPage(MESSAGE_GEN_LANDINGPAGE_PARAMETER_FORMAT)
                        'End: AA:02.17.2016 YRS-AT-2640 Changed the method to accept error message from outside Private Sub RedirectToErrorPage()
                    End If
                Else
                    ' One or more QueryString parameters from PageType, DataType or Value are blank
                    'Start: AA:02.17.2016 YRS-AT-2640 Changed the method to accept error message from outside Private Sub RedirectToErrorPage()
                    'RedirectToErrorPage()
                    RedirectToErrorPage(MESSAGE_GEN_LANDINGPAGE_PARAMETER_FORMAT)
                    'End: AA:02.17.2016 YRS-AT-2640 Changed the method to accept error message from outside Private Sub RedirectToErrorPage()
                End If
            Else
                ' One or more QueryString parameters from PageType, DataType or Value has null value
                'Start: AA:02.17.2016 YRS-AT-2640 Changed the method to accept error message from outside Private Sub RedirectToErrorPage()
                'RedirectToErrorPage()
                RedirectToErrorPage(MESSAGE_GEN_LANDINGPAGE_PARAMETER_FORMAT)
                'End: AA:02.17.2016 YRS-AT-2640 Changed the method to accept error message from outside Private Sub RedirectToErrorPage()
            End If
        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
        End Try

    End Sub
    'Start:AA:02.17.2016 YRS-AT-2640 Changed the method to accept error message from outside Private Sub RedirectToErrorPage()
    'Private Sub RedirectToErrorPage()
    Private Sub RedirectToErrorPage(ByVal intErrorMessageNo As Integer, Optional ByVal dictParameters As Dictionary(Of String, String) = Nothing)
        Dim strErrorMessage As String
        strErrorMessage = YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByTextMessageNo(intErrorMessageNo, dictParameters)
        Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(strErrorMessage), False)
    End Sub
    'End:AA:02.17.2016 YRS-AT-2640 Changed the method to accept error message from outside Private Sub RedirectToErrorPage()
End Class
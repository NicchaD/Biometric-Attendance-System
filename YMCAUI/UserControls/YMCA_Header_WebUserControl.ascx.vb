'*******************************************************************************
' Copyright YMCA Retirement Fund All Rights Reserved. 
'
' Project Name		:	YMCA_YRS
' FileName			:	YMCA_Header_WebUserControl.aspx.vb
' Author Name		:	Shashi Shekhar Singh
' Employee ID		:	51426
' Email				:	shashi.singh@3i-infotech.com
' Contact No		:	
' Creation Date		:	23-Feb-2011
'Description        :   To customize and set the header label.
'********************************************************************************************************************************
'Modification History
'********************************************************************************************************************************
'Modified By        Date            Description
'********************************************************************************************************************************
'Nikunj Patel		2011.02.28		Updated control to make it more generic
'Shashi Shekhar     2011.03.08      Update control to make it's input more generic.
'Manthan Rajguru    2015.09.16      YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
'Bala               2016.01.12      YRS-AT-1718 -  Adding Notes - YMCA Maintenance
'********************************************************************************************************************************

Imports System.Data
Imports System.Data.SqlClient
Imports System.Web
Imports System.Web.SessionState
Imports Microsoft.Practices.EnterpriseLibrary.Data
Public Class YMCA_Header_WebUserControl
    Inherits System.Web.UI.UserControl


#Region "Property Declarations"
    Dim m_strssno As String
    Public Property SSNo() As String
        Get
            If (Not m_strssno Is String.Empty And Not m_strssno = Nothing) Then
                Return m_strssno
            Else
                Return String.Empty
            End If
        End Get
        Set(ByVal Value As String)
            m_strssno = Value
        End Set
    End Property

    Dim l_FundNo As String
    Public Property FundNo() As String
        Get
            If (Not l_FundNo Is String.Empty And Not l_FundNo = Nothing) Then
                Return DirectCast(l_FundNo, String)
            Else
                Return String.Empty
            End If
        End Get

        Set(ByVal value As String)
            l_FundNo = value
        End Set
    End Property

    Dim m_strPerssId As String
    Public Property guiPerssId() As String
        Get
            If (Not m_strPerssId Is String.Empty And Not m_strPerssId = Nothing) Then
                Return m_strPerssId
            Else
                Return String.Empty
            End If
        End Get
        Set(ByVal Value As String)
            m_strPerssId = Value
        End Set
    End Property
    Dim m_TitleFormat As String
    Public Property TitleFormat() As String
        Get
            If m_TitleFormat IsNot Nothing Then
                Return m_TitleFormat
            Else
                Return String.Empty
            End If
        End Get
        Set(ByVal Value As String)
            m_TitleFormat = Value
        End Set
    End Property

    Dim m_PageTitle As String

    Public Property PageTitle() As String
        Get
            If m_PageTitle IsNot Nothing Then
                Return m_PageTitle
            Else
                Return String.Empty
            End If
        End Get
        Set(ByVal Value As String)
            m_PageTitle = Value
        End Set
    End Property

    Dim m_CustomTitle As String

    'Start: Bala: 01/12/2016:  YRS-AT-1718: Added new property
    Public Property CustomTitle() As String
        Get
            If m_CustomTitle IsNot Nothing Then
                Return m_CustomTitle
            Else
                Return String.Empty
            End If
        End Get
        Set(ByVal Value As String)
            m_CustomTitle = Value
        End Set
    End Property
    'End: Bala: 01/12/2016:  YRS-AT-1718: Added new property
#End Region

    Private Sub PrepareHeader()
        Try
            Dim l_ds As DataSet
            Dim dr As DataRow

            Dim strTitleFormat As String = TitleFormat
            If (SSNo().ToString.Trim = String.Empty AndAlso FundNo.ToString.Trim = String.Empty And guiPerssId = String.Empty And CustomTitle.ToString.Trim = String.Empty) Then 'Added one more condition
                Exit Sub
            End If

            Dim l_paramId As String = String.Empty
            Dim l_paramType As String = String.Empty

            If Not SSNo.ToString.Trim = String.Empty Then
                l_paramId = SSNo.ToString.Trim
                l_paramType = "chrSSNo"
            ElseIf Not FundNo.ToString.Trim = String.Empty Then
                l_paramId = FundNo.ToString.Trim
                l_paramType = "intFundIdNo"
            ElseIf Not guiPerssId.ToString.Trim = String.Empty Then
                l_paramId = guiPerssId.ToString.Trim
                l_paramType = "guiUniqueID"
            End If

            If strTitleFormat = "" Then
                strTitleFormat = "{pagetitle} - Fund Id  {fundno} - {lastname}, {firstname}"
            End If

            l_ds = YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.GetHeaderDetails(l_paramId.Trim, l_paramType.Trim)

            If HelperFunctions.isNonEmpty(l_ds) Then
                dr = l_ds.Tables("GetHeaderDetails").Rows(0)
                If (PageTitle.Trim.ToLower = "participant information" Or PageTitle.Trim.ToLower = "retirees information") Then
                    If (dr.IsNull("isarchived") = False AndAlso dr.Item("isarchived").ToString() <> String.Empty) _
                       AndAlso (dr.Item("isarchived").ToString.Trim.ToLower = True = "true") Then
                        strTitleFormat = strTitleFormat & " (Archived)"
                    End If
                End If
            End If

            Dim cachedTitleFormat As String = Cache(strTitleFormat)
            If cachedTitleFormat Is Nothing OrElse cachedTitleFormat = String.Empty Then
                cachedTitleFormat = strTitleFormat.Replace("{fundno}", "{0}")
                cachedTitleFormat = cachedTitleFormat.Replace("{firstname}", "{1}")
                cachedTitleFormat = cachedTitleFormat.Replace("{lastname}", "{2}")
                cachedTitleFormat = cachedTitleFormat.Replace("{archived}", "{3}")
                cachedTitleFormat = cachedTitleFormat.Replace("{pagetitle}", "{4}")
                Cache.Add(strTitleFormat, cachedTitleFormat, Nothing, DateTime.MaxValue, TimeSpan.FromSeconds(1000), CacheItemPriority.Default, Nothing)
            End If

            If HelperFunctions.isNonEmpty(l_ds) Then
                lblFormatHeader.Text = String.Format(cachedTitleFormat, dr("fundno"), dr("firstname"), dr("lastname"), dr("isArchived"), PageTitle)
            Else
                lblFormatHeader.Text = String.Format(cachedTitleFormat, String.Empty, String.Empty, String.Empty, String.Empty, PageTitle)
            End If

            'Start: Bala: 01/12/2016:  YRS-AT-1718: Custom Title 
            If Not CustomTitle Is String.Empty Then
                lblFormatHeader.Text = CustomTitle
            End If
            'End: Bala: 01/12/2016:  YRS-AT-1718: Custom Title 
        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim().ToString()), False)
        End Try
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        PrepareHeader()
    End Sub
End Class
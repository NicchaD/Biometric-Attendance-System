'*******************************************************************************
' Copyright YMCA Retirement Fund All Rights Reserved. 
'
' Project Name		:	YMCA - YRS
' FileName			:	MessageBoxClass.vb
' Author Name		:	SrimuruganG
' Employee ID		:	32365
' Email				:	srimurugan.ag@icici-infotech.com
' Contact No		:	8744
' Creation Time		:	7/30/2005 4:35:49 PM
' Program Specification Name	:	
' Unit Test Plan Name			:	
' Description					:	<<Please put the brief description here...>>
'*******************************************************************************
'2007.10.11 Nikunj Patel        Changed code to fix issue with IE 6 where dropdown lists 
'                               were being shown on top of the message box. Also added JS 
'                               to resize the frame to the size of the message box.
'2007.10.12 Nikunj Patel        Changed code to make use of script from an external file. Also made message box draggable.
'10-07-2012 Priya				BT:1038: QDRO Settlement process confirmation message. Added new show function overload it to set width and height of message box.
'12.10.2012 Anudeep             BT-655 /YRS 5.0-441 : File not creating when you click twice
'14.01.2016 chandrasekar.c      YRS-AT-2524: Give Customer Service the ability to unexpire an expired withdrawal request
'2015.11.19 Chandra sekar.c     YRS-AT-2689 - YRS enh- For participants with Fund status QDRO, block J annuities with spouse as survivor
Imports System
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.HtmlControls

Imports System.IO
Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.Drawing.Drawing2D

Public Enum MessageBoxButtons
    OK
    OKCancel
    YesNo
    YesNoCancel
    AbortRetryCancel
    ContinueCancel
    ProceedCancel 'Chandra sekar.c  2015.11.19  YRS-AT-2689 - YRS enh- For participants with Fund status QDRO, block J annuities with spouse as survivor
    [Stop]
End Enum

Public Enum MessageBoxIcon
    Warning
    [Stop]
    [Error]
    Question
    Asterik
    Information
End Enum

Public Class MessageBox

    Private l_MessageBoxClass As MessageBoxClass

    Private Sub MessageBox()

    End Sub

    Public Shared Sub Show(ByVal paramPlaceHolder As PlaceHolder, ByVal paramTitle As String, ByVal paramMessage As String, ByVal paramMessageButton As MessageBoxButtons, Optional ByVal paramIsFromPopup As Boolean = False)

        Dim l_messageBox As New MessageBoxClass

        If paramIsFromPopup = True Then
            'NP:PS:2007.10.11 - Commenting this section to ignore paramIsFromPopup value.
            'l_messageBox.MessageBoxTop = 170
            'l_messageBox.MessageBoxLeft = 160
        Else
            l_messageBox.MessageBoxTop = 170
            l_messageBox.MessageBoxLeft = 350
        End If


        'START: chandrasekar.c | 14.01.2016 | YRS-AT-2524: Give Customer Service the ability to unexpire an expired withdrawal request
        'l_messageBox.MessageBoxWidth = 350
        'l_messageBox.MessageBoxHeight = 120
        l_messageBox.MessageBoxWidth = 370
        l_messageBox.MessageBoxHeight = 150
        'END: chandrasekar.c | 14.01.2016 | YRS-AT-2524: Give Customer Service the ability to unexpire an expired withdrawal request
        l_messageBox.MessageBoxButtonWidth = 70


        l_messageBox.MessageBoxTitle = paramTitle
        l_messageBox.MessageBoxMessage = paramMessage
        l_messageBox.BorderColor = Color.DarkBlue
        l_messageBox.BorderWidth = New Unit(2)


        Select Case paramMessageButton
            Case MessageBoxButtons.OK
                l_messageBox.MessageBoxButton = 1

                l_messageBox.MessageBoxButtonYesText = "OK"
                l_messageBox.MessageBoxIDYes = "OK"

                l_messageBox.MessageBoxImage = "images/OK48.JPG"

            Case MessageBoxButtons.OKCancel
                l_messageBox.MessageBoxButton = 2

                l_messageBox.MessageBoxButtonYesText = "OK"
                l_messageBox.MessageBoxIDYes = "OK"

                l_messageBox.MessageBoxButtonCancelText = "Cancel"
                l_messageBox.MessageBoxIDCancel = "Cancel"

                l_messageBox.MessageBoxImage = "images/help48.JPG"

            Case MessageBoxButtons.YesNo
                l_messageBox.MessageBoxButton = 2

                l_messageBox.MessageBoxButtonYesText = "Yes"
                l_messageBox.MessageBoxIDYes = "Yes"

                l_messageBox.MessageBoxButtonNoText = "No"
                l_messageBox.MessageBoxIDNo = "No"

                l_messageBox.MessageBoxImage = "images/help48.JPG"

            Case MessageBoxButtons.ContinueCancel
                l_messageBox.MessageBoxButton = 2

                l_messageBox.MessageBoxButtonYesText = "Continue"
                l_messageBox.MessageBoxIDYes = "Continue"

                l_messageBox.MessageBoxButtonCancelText = "Cancel"
                l_messageBox.MessageBoxIDCancel = "Cancel"

                l_messageBox.MessageBoxImage = "images/help48.JPG"

            Case MessageBoxButtons.YesNoCancel
                l_messageBox.MessageBoxButton = 3

                l_messageBox.MessageBoxButtonYesText = "Yes"
                l_messageBox.MessageBoxIDYes = "Yes"

                l_messageBox.MessageBoxButtonNoText = "No"
                l_messageBox.MessageBoxIDNo = "No"

                l_messageBox.MessageBoxButtonCancelText = "Cancel"
                l_messageBox.MessageBoxIDCancel = "Cancel"

                l_messageBox.MessageBoxImage = "images/help48.JPG"

            Case MessageBoxButtons.AbortRetryCancel
                l_messageBox.MessageBoxButton = 3

                l_messageBox.MessageBoxButtonYesText = "Abort"
                l_messageBox.MessageBoxIDYes = "Abort"

                l_messageBox.MessageBoxButtonNoText = "Retry"
                l_messageBox.MessageBoxIDNo = "Retry"

                l_messageBox.MessageBoxButtonCancelText = "Cancel"
                l_messageBox.MessageBoxIDCancel = "Cancel"

                l_messageBox.MessageBoxImage = "images/help48.JPG"
                'Start-Chandra sekar.c  2015.11.19  YRS-AT-2689 - YRS enh- For participants with Fund status QDRO, block J annuities with spouse as survivor
            Case MessageBoxButtons.ProceedCancel
                l_messageBox.MessageBoxButton = 2

                l_messageBox.MessageBoxButtonYesText = "Proceed"
                l_messageBox.MessageBoxIDYes = "Proceed"

                l_messageBox.MessageBoxButtonNoText = "Cancel"
                l_messageBox.MessageBoxIDNo = "Cancel"

                l_messageBox.MessageBoxImage = "images/help48.JPG"
                'End-Chandra sekar.c  2015.11.19  YRS-AT-2689 - YRS enh- For participants with Fund status QDRO, block J annuities with spouse as survivor
            Case MessageBoxButtons.Stop

                l_messageBox.MessageBoxButton = 1

                l_messageBox.MessageBoxButtonYesText = "OK"
                l_messageBox.MessageBoxIDYes = "OK"

                l_messageBox.MessageBoxImage = "images/error.gif"


            Case Else
                l_messageBox.MessageBoxButton = 1

                l_messageBox.MessageBoxButtonYesText = "OK"
                l_messageBox.MessageBoxIDYes = "OK"

                l_messageBox.MessageBoxImage = "images/OK48.JPG"

        End Select

        Dim l_Iframe As New HtmlIFrameControl
        l_Iframe.IFrameLeft = l_messageBox.MessageBoxLeft
        l_Iframe.IFrameTop = l_messageBox.MessageBoxTop
        l_Iframe.IFrameWidth = l_messageBox.MessageBoxWidth
        l_Iframe.IFrameHeight = l_messageBox.MessageBoxHeight

        paramPlaceHolder.Controls.Add(l_Iframe)
        paramPlaceHolder.Controls.Add(l_messageBox)
        paramPlaceHolder.Controls.Add(New JavascriptScriptControl)

    End Sub

    Public Shared Sub Show(ByVal Top As Integer, ByVal Left As Integer, ByVal paramPlaceHolder As PlaceHolder, ByVal paramTitle As String, ByVal paramMessage As String, ByVal paramMessageButton As MessageBoxButtons, Optional ByVal paramIsFromPopup As Boolean = False)

        Dim l_messageBox As New MessageBoxClass

        l_messageBox.MessageBoxTop = Top
        l_messageBox.MessageBoxLeft = Left

        l_messageBox.MessageBoxWidth = 350
        l_messageBox.MessageBoxHeight = 120
        l_messageBox.MessageBoxButtonWidth = 70


        l_messageBox.MessageBoxTitle = paramTitle
        l_messageBox.MessageBoxMessage = paramMessage
        l_messageBox.BorderColor = Color.DarkBlue
        l_messageBox.BorderWidth = New Unit(2)


        Select Case paramMessageButton
            Case MessageBoxButtons.OK
                l_messageBox.MessageBoxButton = 1

                l_messageBox.MessageBoxButtonYesText = "OK"
                l_messageBox.MessageBoxIDYes = "OK"

                l_messageBox.MessageBoxImage = "images/OK48.JPG"

            Case MessageBoxButtons.OKCancel
                l_messageBox.MessageBoxButton = 2

                l_messageBox.MessageBoxButtonYesText = "OK"
                l_messageBox.MessageBoxIDYes = "OK"

                l_messageBox.MessageBoxButtonCancelText = "Cancel"
                l_messageBox.MessageBoxIDCancel = "Cancel"

                l_messageBox.MessageBoxImage = "images/help48.JPG"

            Case MessageBoxButtons.YesNo
                l_messageBox.MessageBoxButton = 2

                l_messageBox.MessageBoxButtonYesText = "Yes"
                l_messageBox.MessageBoxIDYes = "Yes"

                l_messageBox.MessageBoxButtonNoText = "No"
                l_messageBox.MessageBoxIDNo = "No"

                l_messageBox.MessageBoxImage = "images/help48.JPG"

            Case MessageBoxButtons.ContinueCancel
                l_messageBox.MessageBoxButton = 2

                l_messageBox.MessageBoxButtonYesText = "Continue"
                l_messageBox.MessageBoxIDYes = "Continue"

                l_messageBox.MessageBoxButtonCancelText = "Cancel"
                l_messageBox.MessageBoxIDCancel = "Cancel"

                l_messageBox.MessageBoxImage = "images/help48.JPG"

            Case MessageBoxButtons.YesNoCancel
                l_messageBox.MessageBoxButton = 3

                l_messageBox.MessageBoxButtonYesText = "Yes"
                l_messageBox.MessageBoxIDYes = "Yes"

                l_messageBox.MessageBoxButtonNoText = "No"
                l_messageBox.MessageBoxIDNo = "No"

                l_messageBox.MessageBoxButtonCancelText = "Cancel"
                l_messageBox.MessageBoxIDCancel = "Cancel"

                l_messageBox.MessageBoxImage = "images/help48.JPG"

            Case MessageBoxButtons.AbortRetryCancel
                l_messageBox.MessageBoxButton = 3

                l_messageBox.MessageBoxButtonYesText = "Abort"
                l_messageBox.MessageBoxIDYes = "Abort"

                l_messageBox.MessageBoxButtonNoText = "Retry"
                l_messageBox.MessageBoxIDNo = "Retry"

                l_messageBox.MessageBoxButtonCancelText = "Cancel"
                l_messageBox.MessageBoxIDCancel = "Cancel"

                l_messageBox.MessageBoxImage = "images/help48.JPG"

            Case MessageBoxButtons.Stop

                l_messageBox.MessageBoxButton = 1

                l_messageBox.MessageBoxButtonYesText = "OK"
                l_messageBox.MessageBoxIDYes = "OK"

                l_messageBox.MessageBoxImage = "images/error.gif"


            Case Else
                l_messageBox.MessageBoxButton = 1

                l_messageBox.MessageBoxButtonYesText = "OK"
                l_messageBox.MessageBoxIDYes = "OK"

                l_messageBox.MessageBoxImage = "images/OK48.JPG"

        End Select

        Dim l_Iframe As New HtmlIFrameControl
        l_Iframe.IFrameLeft = l_messageBox.MessageBoxLeft
        l_Iframe.IFrameTop = l_messageBox.MessageBoxTop
        l_Iframe.IFrameWidth = l_messageBox.MessageBoxWidth
        l_Iframe.IFrameHeight = l_messageBox.MessageBoxHeight

        paramPlaceHolder.Controls.Add(l_Iframe)
        paramPlaceHolder.Controls.Add(l_messageBox)
        paramPlaceHolder.Controls.Add(New JavascriptScriptControl)

    End Sub
    'Priya 10-07-2012 BT:1038: QDRO Settlement process confirmation message.
    'Added new show function overload it to set width and height of message box.
    Public Shared Sub Show(ByVal Top As Integer, ByVal Left As Integer, ByVal Width As Integer, ByVal Height As Integer, ByVal paramPlaceHolder As PlaceHolder, ByVal paramTitle As String, ByVal paramMessage As String, ByVal paramMessageButton As MessageBoxButtons, Optional ByVal paramIsFromPopup As Boolean = False)

        Dim l_messageBox As New MessageBoxClass
        'Priya 11-07-2012 BT-1030:QDRO Settlement process confirmation message.
        'set message box height and width.
        l_messageBox.MessageBoxTop = Top
        l_messageBox.MessageBoxLeft = Left

        l_messageBox.MessageBoxHeight = Height
        l_messageBox.MessageBoxWidth = Width
        'END Priya 11-07-2012 BT-1030:QDRO Settlement process confirmation message.

        'l_messageBox.MessageBoxWidth = 350
        'l_messageBox.MessageBoxHeight = 120
        l_messageBox.MessageBoxButtonWidth = 70


        l_messageBox.MessageBoxTitle = paramTitle
        l_messageBox.MessageBoxMessage = paramMessage
        l_messageBox.BorderColor = Color.DarkBlue
        l_messageBox.BorderWidth = New Unit(2)


        Select Case paramMessageButton
            Case MessageBoxButtons.OK
                l_messageBox.MessageBoxButton = 1

                l_messageBox.MessageBoxButtonYesText = "OK"
                l_messageBox.MessageBoxIDYes = "OK"

                l_messageBox.MessageBoxImage = "images/OK48.JPG"

            Case MessageBoxButtons.OKCancel
                l_messageBox.MessageBoxButton = 2

                l_messageBox.MessageBoxButtonYesText = "OK"
                l_messageBox.MessageBoxIDYes = "OK"

                l_messageBox.MessageBoxButtonCancelText = "Cancel"
                l_messageBox.MessageBoxIDCancel = "Cancel"

                l_messageBox.MessageBoxImage = "images/help48.JPG"

            Case MessageBoxButtons.YesNo
                l_messageBox.MessageBoxButton = 2

                l_messageBox.MessageBoxButtonYesText = "Yes"
                l_messageBox.MessageBoxIDYes = "Yes"

                l_messageBox.MessageBoxButtonNoText = "No"
                l_messageBox.MessageBoxIDNo = "No"

                l_messageBox.MessageBoxImage = "images/help48.JPG"

            Case MessageBoxButtons.ContinueCancel
                l_messageBox.MessageBoxButton = 2

                l_messageBox.MessageBoxButtonYesText = "Continue"
                l_messageBox.MessageBoxIDYes = "Continue"

                l_messageBox.MessageBoxButtonCancelText = "Cancel"
                l_messageBox.MessageBoxIDCancel = "Cancel"

                l_messageBox.MessageBoxImage = "images/help48.JPG"

            Case MessageBoxButtons.YesNoCancel
                l_messageBox.MessageBoxButton = 3

                l_messageBox.MessageBoxButtonYesText = "Yes"
                l_messageBox.MessageBoxIDYes = "Yes"

                l_messageBox.MessageBoxButtonNoText = "No"
                l_messageBox.MessageBoxIDNo = "No"

                l_messageBox.MessageBoxButtonCancelText = "Cancel"
                l_messageBox.MessageBoxIDCancel = "Cancel"

                l_messageBox.MessageBoxImage = "images/help48.JPG"

            Case MessageBoxButtons.AbortRetryCancel
                l_messageBox.MessageBoxButton = 3

                l_messageBox.MessageBoxButtonYesText = "Abort"
                l_messageBox.MessageBoxIDYes = "Abort"

                l_messageBox.MessageBoxButtonNoText = "Retry"
                l_messageBox.MessageBoxIDNo = "Retry"

                l_messageBox.MessageBoxButtonCancelText = "Cancel"
                l_messageBox.MessageBoxIDCancel = "Cancel"

                l_messageBox.MessageBoxImage = "images/help48.JPG"

            Case MessageBoxButtons.Stop

                l_messageBox.MessageBoxButton = 1

                l_messageBox.MessageBoxButtonYesText = "OK"
                l_messageBox.MessageBoxIDYes = "OK"

                l_messageBox.MessageBoxImage = "images/error.gif"


            Case Else
                l_messageBox.MessageBoxButton = 1

                l_messageBox.MessageBoxButtonYesText = "OK"
                l_messageBox.MessageBoxIDYes = "OK"

                l_messageBox.MessageBoxImage = "images/OK48.JPG"

        End Select

        Dim l_Iframe As New HtmlIFrameControl
        l_Iframe.IFrameLeft = l_messageBox.MessageBoxLeft
        l_Iframe.IFrameTop = l_messageBox.MessageBoxTop
        l_Iframe.IFrameWidth = l_messageBox.MessageBoxWidth
        l_Iframe.IFrameHeight = l_messageBox.MessageBoxHeight

        paramPlaceHolder.Controls.Add(l_Iframe)
        paramPlaceHolder.Controls.Add(l_messageBox)
        paramPlaceHolder.Controls.Add(New JavascriptScriptControl)

    End Sub

End Class

Public Class HtmlIFrameControl
    Inherits WebControl

    Private strLeft As String
    Private strTop As String
    Private intIFrameWidth As Integer
    Private intIFrameHeight As Integer

    '   Message box left position
    Public Property IFrameLeft() As String
        Get
            Return strLeft
        End Get

        Set(ByVal Value As String)
            strLeft = Value
        End Set

    End Property
    '   Message box top position
    Public Property IFrameTop() As String

        Get
            Return strTop
        End Get

        Set(ByVal Value As String)
            strTop = Value
        End Set

    End Property
    '   Message box width position
    Public Property IFrameWidth() As Integer
        Get
            Return intIFrameWidth
        End Get
        Set(ByVal Value As Integer)
            intIFrameWidth = Value
        End Set

    End Property
    '   Message box height position
    Public Property IFrameHeight() As Integer
        Get
            Return intIFrameHeight
        End Get
        Set(ByVal Value As Integer)
            intIFrameHeight = Value
        End Set

    End Property

    Sub New()
        MyBase.New(HtmlTextWriterTag.Iframe)
    End Sub

    Protected Overrides Sub AddAttributesToRender(ByVal writer As System.Web.UI.HtmlTextWriter)
        MyBase.AddAttributesToRender(writer)
        writer.AddAttribute("id", "showimage_iframe")
        writer.AddAttribute("frameborder", "0")
        writer.AddAttribute("scrolling", "no")
        writer.AddAttribute(HtmlTextWriterAttribute.Style, "Z-INDEX: 9999; LEFT:" + strLeft + "px; WIDTH:" + intIFrameWidth.ToString() + "px; POSITION: absolute; TOP: " + strTop + "px; HEIGHT: " + intIFrameHeight.ToString() + "px;")
    End Sub

    Protected Overrides Sub OnInit(ByVal e As System.EventArgs)
        MyBase.OnInit(e)

        '   Default properties settings for message box control
        If strLeft Is Nothing Then strLeft = "250"
        If strTop Is Nothing Then strTop = "250"
        If intIFrameWidth < 10 Then intIFrameWidth = 250
        If intIFrameHeight < 1 Then intIFrameHeight = 8

    End Sub

End Class
Public Class JavascriptScriptControl
    Inherits WebControl

    Private strScript As String
    Public Property Script() As String
        Get
            Return strScript
        End Get
        Set(ByVal Value As String)
            strScript = Value
        End Set
    End Property

    Sub New()
        MyBase.New(HtmlTextWriterTag.Script)
    End Sub

    Protected Overrides Sub AddAttributesToRender(ByVal writer As System.Web.UI.HtmlTextWriter)
        writer.AddAttribute("language", "JavaScript")
    End Sub

    Protected Overrides Sub RenderChildren(ByVal writer As System.Web.UI.HtmlTextWriter)
        MyBase.RenderChildren(writer)
        writer.WriteLine(strScript)
    End Sub

    Protected Overrides Sub OnInit(ByVal e As System.EventArgs)
        strScript = HttpContext.Current.Cache.Item("dom-drag.js Script")
        If strScript Is Nothing Then
            Dim fin As IO.StreamReader
            Try
                fin = New StreamReader(HttpContext.Current.Server.MapPath("~/JS/dom-drag.js"))
                strScript = fin.ReadToEnd()
            Catch ex As Exception

            Finally
                If Not fin Is Nothing Then fin.Close()
            End Try
            'Add the contents to cache to avoid reading it from disk again. Also add a dependency to the file.
            HttpContext.Current.Cache.Add("dom-drag.js Script", strScript, _
                                New Caching.CacheDependency(HttpContext.Current.Server.MapPath("~/JS/dom-drag.js")), _
                                DateTime.MaxValue, TimeSpan.FromMinutes(60), Caching.CacheItemPriority.AboveNormal, Nothing)
        End If
    End Sub
End Class

Public Class MessageBoxClass
    Inherits WebControl

#Region "MessageBox Class"

    Private strLeft As String
    Private strTop As String
    Private intButton As Integer
    Private strMessage As String
    Private strTitle As String
    Private strImage As String
    Private strCss As String
    Private strCssTitle As String
    Private strCssMessage As String
    Private strButtonYes As String
    Private strButtonNo As String
    Private strButtonCancel As String
    Private strButtonWidth As Integer
    Private strMessageBoxIDYes As String
    Private strMessageBoxIDNo As String
    Private strMessageBoxIDCancel As String
    Private intMessageBoxWidth As Integer
    Private intMessageBoxHeight As Integer
    Private intMessageBoxImageWidth As Integer
    Private intMessageBoxImageHeight As Integer

    Dim homedirectory As String

    '   Message box left position

    Public Property MessageBoxLeft() As String
        Get
            Return strLeft
        End Get

        Set(ByVal Value As String)
            strLeft = Value
        End Set

    End Property

    '   Message box top position

    Public Property MessageBoxTop() As String

        Get
            Return strTop
        End Get

        Set(ByVal Value As String)
            strTop = Value
        End Set

    End Property

    '   Number of buttons you want to display in the message box

    Public Property MessageBoxButton() As Integer
        Get
            Return intButton
        End Get

        Set(ByVal Value As Integer)
            intButton = Value
        End Set

    End Property

    '   Customize message you want to display in the message box

    Public Property MessageBoxMessage() As String
        Get
            Return strMessage
        End Get

        Set(ByVal Value As String)
            strMessage = Value
        End Set

    End Property

    '   Title you want to display in the message box

    Public Property MessageBoxTitle() As String

        Get
            Return strTitle
        End Get

        Set(ByVal Value As String)
            strTitle = Value
        End Set

    End Property

    '   Image you want to display in the message box (like information / warning)

    Public Property MessageBoxImage() As String

        Get
            Return strImage
        End Get

        Set(ByVal Value As String)
            strImage = Value
        End Set

    End Property

    '   Message box ID for Yes button

    Public Property MessageBoxIDYes() As String

        Get
            Return strMessageBoxIDYes
        End Get

        Set(ByVal Value As String)
            strMessageBoxIDYes = Value
        End Set

    End Property

    '   Message box ID for No button

    Public Property MessageBoxIDNo() As String

        Get
            Return strMessageBoxIDNo
        End Get

        Set(ByVal Value As String)
            strMessageBoxIDNo = Value
        End Set

    End Property

    '   Message box ID for Cancel button

    Public Property MessageBoxIDCancel() As String

        Get
            Return strMessageBoxIDCancel
        End Get

        Set(ByVal Value As String)
            strMessageBoxIDCancel = Value
        End Set

    End Property

    '   Style you want to incorporate for message box

    Public Property MessageBoxCss() As String

        Get
            Return strCss
        End Get

        Set(ByVal Value As String)
            strCss = Value
        End Set

    End Property

    Public Property MessageBoxCssTitle() As String

        Get
            Return strCssTitle
        End Get

        Set(ByVal Value As String)
            strCssTitle = Value
        End Set

    End Property

    Public Property MessageBoxCssMessage() As String

        Get
            Return strCssMessage
        End Get

        Set(ByVal Value As String)
            strCssMessage = Value
        End Set

    End Property

    '   Message box Text for Yes button

    Public Property MessageBoxButtonYesText() As String

        Get
            Return strButtonYes
        End Get

        Set(ByVal Value As String)
            strButtonYes = Value
        End Set

    End Property

    '   Message box Text for No button

    Public Property MessageBoxButtonNoText() As String

        Get
            Return strButtonNo
        End Get

        Set(ByVal Value As String)
            strButtonNo = Value
        End Set

    End Property

    '   Message box Text for Cancel button

    Public Property MessageBoxButtonCancelText() As String

        Get
            Return strButtonCancel
        End Get

        Set(ByVal Value As String)
            strButtonCancel = Value
        End Set

    End Property

    '   Message box buttons width

    Public Property MessageBoxButtonWidth() As Integer

        Get
            Return strButtonWidth
        End Get

        Set(ByVal Value As Integer)
            strButtonWidth = Value
        End Set

    End Property

    '   Message box width

    Public Property MessageBoxWidth() As Integer

        Get
            Return intMessageBoxWidth
        End Get

        Set(ByVal Value As Integer)
            intMessageBoxWidth = Value
        End Set

    End Property

    '   Message box height

    Public Property MessageBoxHeight() As Integer

        Get
            Return intMessageBoxHeight
        End Get

        Set(ByVal Value As Integer)
            intMessageBoxHeight = Value
        End Set

    End Property

    '   Message box image width

    Public Property MessageBoxImageWidth() As Integer

        Get
            Return intMessageBoxImageWidth
        End Get

        Set(ByVal Value As Integer)
            intMessageBoxImageWidth = Value
        End Set

    End Property

    '   Message box image height

    Public Property MessageBoxImageHeight() As Integer

        Get
            Return intMessageBoxImageHeight
        End Get

        Set(ByVal Value As Integer)
            intMessageBoxImageHeight = Value
        End Set

    End Property


    Protected Friend layer As HtmlGenericControl
    Protected Friend ilayer As HtmlGenericControl
    Protected Friend img As HtmlGenericControl
    Protected Friend div As HtmlGenericControl
    Protected Friend ButtonOK As Button
    Protected Friend ButtonYes As Button
    Protected Friend ButtonNo As Button
    Protected Friend ButtonCancel As Button

    Protected Alertimage As System.Web.UI.WebControls.Image

    Public Sub New()
        MyBase.New("div")
    End Sub

    Protected Overrides Sub OnInit(ByVal e As EventArgs)

        MyBase.OnInit(e)

        '   Default properties settings for message box control

        If strLeft Is Nothing Then
            strLeft = "250"
        End If

        If strTop Is Nothing Then
            strTop = "250"
        End If

        If strTitle Is Nothing Then
            strTitle = "MessageBox"
        End If

        If intButton < 0 Then
            intButton = 1
        End If

        If strMessageBoxIDYes Is Nothing Then
            strMessageBoxIDYes = "MessageBoxIDYes"
        End If

        If strMessageBoxIDNo Is Nothing Then
            strMessageBoxIDNo = "MessageBoxIDNo"
        End If

        If strMessageBoxIDCancel Is Nothing Then
            strMessageBoxIDCancel = "MessageBoxIDCancel"
        End If

        If strCss Is Nothing Then
            strCss = ""
        End If

        If strCssMessage Is Nothing Then
            strCssMessage = ""
        End If

        If strCssTitle Is Nothing Then
            strCssTitle = ""
        End If

        If strMessage Is Nothing Then
            strMessage = "No message to display here."
        End If

        If intButton = 1 Or intButton > 3 Or intButton < 1 Then
            If strButtonYes Is Nothing Then
                strButtonYes = "OK"
            End If

        ElseIf intButton > 1 And intButton < 4 Then

            If strButtonYes Is Nothing Then
                strButtonYes = "Approve"
            End If

            If strButtonNo Is Nothing Then
                strButtonNo = "Cancel"
            End If

            If strButtonCancel Is Nothing Then
                strButtonCancel = "Ignore"
            End If

        End If

        If strButtonWidth < 5 Then
            strButtonWidth = 70
        End If

        If intMessageBoxWidth < 10 Then
            intMessageBoxWidth = 250
        End If

        If intMessageBoxHeight < 1 Then
            intMessageBoxHeight = 8
        End If

        If intMessageBoxImageWidth < 5 Then
            intMessageBoxImageWidth = 48
        End If

        If intMessageBoxImageHeight < 5 Then
            intMessageBoxImageHeight = 48
        End If

        If homedirectory Is Nothing Then
            homedirectory = Me.Page.Request.PhysicalApplicationPath
        End If


    End Sub

    Protected Overrides Sub createChildControls()

        '   Creating message box

        Dim myRow As TableRow
        Dim myCell As TableCell

        Dim myTable As Table = New Table
        myTable.BorderWidth = New Unit(0)

        myTable.CellSpacing = 0

        myTable.Width = New Unit(intMessageBoxWidth)
        myTable.Height = New Unit(intMessageBoxHeight)

        Controls.Add(myTable)


        myRow = New TableRow

        myRow.BorderWidth = New Unit(0)
        myTable.Rows.Add(myRow)


        myCell = New TableCell

        Dim NewLabel As Label = New Label

        NewLabel.Text = strTitle
        NewLabel.CssClass = strCssTitle

        myCell.Controls.Add(NewLabel)


        myCell.ID = "dragbar"
        myCell.ColumnSpan = 5


        myCell.CssClass = strCssTitle

        If strCssTitle = "" Then

            myCell.ForeColor = System.Drawing.Color.White
            myCell.BackColor = System.Drawing.Color.DarkBlue

            myCell.Font.Name = "Verdana"
            myCell.Font.Bold = True

            myCell.HorizontalAlign = HorizontalAlign.Left

            myCell.Font.Size = New FontUnit(8)
            myCell.Style.Add("CURSOR", "hand")

            myCell.Height = New Unit(20)    'NP:PS:2007.10.12 - Setting the minimum height of the row as 20px
        End If

        myRow.Cells.Add(myCell)

        myRow = New TableRow
        myRow.BorderWidth = New Unit(0)

        myTable.Rows.Add(myRow)

        myCell = New TableCell
        myCell.ColumnSpan = 5

        myCell.CssClass = strCssMessage

        If strCssMessage = "" Then
            'myCell.BackColor = System.Drawing.Color.LightGray
            myCell.BackColor = System.Drawing.Color.White
        End If

        myRow.Cells.Add(myCell)

        Dim myRow1 As TableRow
        Dim myCell1 As TableCell
        Dim myTable1 As Table = New Table

        myTable1.BorderWidth = New Unit(0)

        myTable1.CellSpacing = 0
        myCell.Controls.Add(myTable1)

        myRow1 = New TableRow
        myRow1.BorderWidth = New Unit(0)

        myTable1.Rows.Add(myRow1)

        myCell1 = New TableCell
        myCell1.CssClass = strCssMessage

        myCell1.BorderWidth = New Unit(0)
        'myCell1.Width = New Unit(36)
        myCell1.Width = New Unit(58)


        Dim Alertimage As System.Web.UI.WebControls.Image = New System.Web.UI.WebControls.Image

        'Alertimage.Height = New Unit(intMessageBoxImageHeight)
        'Alertimage.Width = New Unit(intMessageBoxImageWidth)

        Alertimage.BorderWidth = New Unit(0)

        Alertimage.ImageUrl = strImage
        myCell1.Controls.Add(Alertimage)


        myRow1.Cells.Add(myCell1)

        myCell1 = New TableCell

        myCell1.CssClass = strCssMessage
        myCell1.BorderWidth = New Unit(0)

        myCell1.CssClass = strCssMessage

        If strCssMessage = "" Then

            'myCell1.HorizontalAlign = HorizontalAlign.Center
            'myCell1.ForeColor = System.Drawing.Color.Black
            'myCell1.BackColor = System.Drawing.Color.LightGray
            'myCell1.BorderColor = System.Drawing.Color.LightGray

            myCell1.HorizontalAlign = HorizontalAlign.Left
            myCell1.ForeColor = System.Drawing.Color.Black
            myCell1.BackColor = System.Drawing.Color.White
            myCell1.BorderColor = System.Drawing.Color.White

            myCell1.Font.Name = "Verdana"
            myCell1.Font.Bold = True
            myCell1.Font.Size = New FontUnit(8)

        End If


        Dim NewLabel1 As Label = New Label

        NewLabel1.Text = strMessage
        myCell1.Controls.Add(NewLabel1)

        myRow1.Cells.Add(myCell1)

        myRow = New TableRow
        myRow.BorderWidth = New Unit(0)
        myTable.Rows.Add(myRow)
        'Added Anudeep:12.10.2012 for BT-655 /YRS 5.0-441 : File not creating when you click twice, start
        Dim strScript As String

        strScript = "<script language='Javascript'>" +
                        "function DoProcess () {" +
                             "var div = document.getElementById('showimage');" +
                                     "div.style.display = 'none';" +
                             "var frm = document.getElementById('showimage_iframe');" +
                                     "frm.style.display = 'none';" +
                     "} </script>"

        If (Not Page.IsStartupScriptRegistered("MessageBox")) Then
            Page.RegisterStartupScript("MessageBox", strScript)
        End If
        Page.RegisterStartupScript("MessageBox", strScript)
        'Added Anudeep:12.10.2012 for BT-655 /YRS 5.0-441 : File not creating when you click twice, end
        If intButton = 1 Or intButton > 3 Or intButton < 1 Then

            myCell = New TableCell
            myCell.ColumnSpan = 5
            myCell.BorderWidth = New Unit(0)

            myCell.CssClass = strCssMessage
            myCell.HorizontalAlign = HorizontalAlign.Center

            If strCssMessage = "" Then

                myCell.ForeColor = System.Drawing.Color.Black
                'myCell.BackColor = System.Drawing.Color.LightGray
                myCell.BackColor = System.Drawing.Color.White

                myCell.Font.Name = "Verdana"
                myCell.Font.Bold = True
                myCell.Font.Size = New FontUnit(8)

            End If


            ButtonOK = New Button

            ButtonOK.ID = strMessageBoxIDYes
            ButtonOK.Text = strButtonYes
            ButtonOK.Width = New Unit(strButtonWidth)
            ButtonOK.Style.Add("CURSOR", "hand")

            ' Added By Anudeep:12.10.2012 For Bt-654 / YRS 5.0-441 
            ButtonOK.OnClientClick = "DoProcess()"


            ButtonOK.Font.Name = "Verdana"
            ButtonOK.Font.Bold = True
            ButtonOK.Font.Size = New FontUnit(8)

            'To avoid firing validation
            ButtonOK.CausesValidation = False

            myCell.Controls.Add(ButtonOK)

            myRow.Cells.Add(myCell)

        End If


        If intButton > 1 And intButton < 4 Then

            myCell = New TableCell
            myCell.CssClass = strCssMessage
            myCell.BorderWidth = New Unit(0)
            myCell.HorizontalAlign = HorizontalAlign.Right


            If strCssMessage = "" Then
                myCell.ForeColor = System.Drawing.Color.Black
                'myCell.BackColor = System.Drawing.Color.LightGray
                myCell.BackColor = System.Drawing.Color.White
                myCell.Font.Name = "Verdana"


                myCell.Font.Bold = True
                myCell.Font.Size = New FontUnit(8)
            End If


            ButtonYes = New Button
            ButtonYes.ID = strMessageBoxIDYes
            ButtonYes.Text = strButtonYes
            ButtonYes.Width = New Unit(strButtonWidth)
            ButtonYes.Style.Add("CURSOR", "hand")

            ' Added By Anudeep:12.10.2012 For Bt-654 / YRS 5.0-441 
            ButtonYes.OnClientClick = "DoProcess()"

            ButtonYes.Font.Name = "Verdana"
            ButtonYes.Font.Bold = True
            ButtonYes.Font.Size = New FontUnit(8)

            ButtonYes.CausesValidation = False

            myCell.Controls.Add(ButtonYes)

            myRow.Cells.Add(myCell)
            myCell = New TableCell

            myCell.Width = New Unit(20)
            myCell.BorderWidth = New Unit(0)

            myCell.CssClass = strCssMessage

            If strCssMessage = "" Then
                ' myCell.BackColor = System.Drawing.Color.LightGray
                myCell.BackColor = System.Drawing.Color.White
            End If

            myRow.Cells.Add(myCell)

            myCell = New TableCell
            myCell.CssClass = strCssMessage
            myCell.BorderWidth = New Unit(0)

            If strCssMessage = "" Then

                myCell.ForeColor = System.Drawing.Color.Black
                'myCell.BackColor = System.Drawing.Color.LightGray
                myCell.BackColor = System.Drawing.Color.White
                myCell.Font.Name = "Verdana"
                myCell.Font.Bold = True
                myCell.Font.Size = New FontUnit(8)

            End If


            If intButton = 2 Then
                myCell.HorizontalAlign = HorizontalAlign.Left
            ElseIf intButton = 3 Then
                myCell.HorizontalAlign = HorizontalAlign.Center
            End If


            ButtonNo = New Button
            ButtonNo.ID = strMessageBoxIDNo
            ButtonNo.Text = strButtonNo
            ButtonNo.Width = New Unit(strButtonWidth)

            ButtonNo.Attributes("WIDTH") = strButtonWidth.ToString()
            ButtonNo.Attributes("HEIGHT") = strButtonWidth.ToString()

            ' Added By Anudeep:12.10.2012 For Bt-654 / YRS 5.0-441 
            ButtonNo.OnClientClick = "DoProcess(); "

            ButtonNo.Style.Add("CURSOR", "hand")
            ButtonNo.CausesValidation = False

            ButtonNo.Font.Name = "Verdana"
            ButtonNo.Font.Bold = True
            ButtonNo.Font.Size = New FontUnit(8)

            ButtonNo.CausesValidation = False

            myCell.Controls.Add(ButtonNo)

            myRow.Cells.Add(myCell)


            If intButton = 3 Then
                myCell = New TableCell
                myCell.Width = New Unit(10)

                myCell.BorderWidth = New Unit(0)
                myCell.CssClass = strCssMessage

                If strCssMessage = "" Then
                    'myCell.BackColor = System.Drawing.Color.LightGray
                    myCell.BackColor = System.Drawing.Color.White
                End If


                myRow.Cells.Add(myCell)

                myCell = New TableCell
                myCell.CssClass = strCssMessage
                myCell.BorderWidth = New Unit(0)
                myCell.HorizontalAlign = HorizontalAlign.Left

                If strCssMessage = "" Then

                    myCell.ForeColor = System.Drawing.Color.Black
                    'myCell.BackColor = System.Drawing.Color.LightGray
                    myCell.BackColor = System.Drawing.Color.White
                    myCell.Font.Name = "Verdana"
                    myCell.Font.Bold = True
                    myCell.Font.Size = New FontUnit(8)

                End If


                ButtonCancel = New Button
                ButtonCancel.ID = strMessageBoxIDCancel
                ButtonCancel.Text = strButtonCancel
                ButtonCancel.Width = New Unit(strButtonWidth)
                ButtonCancel.Style.Add("CURSOR", "hand")

                ' Added By Anudeep:12.10.2012 For Bt-654 / YRS 5.0-441 
                ButtonCancel.OnClientClick = "DoProcess()"

                ButtonCancel.Font.Name = "Verdana"
                ButtonCancel.Font.Bold = True
                ButtonCancel.Font.Size = New FontUnit(8)

                ButtonCancel.CausesValidation = False

                myCell.Controls.Add(ButtonCancel)

                myRow.Cells.Add(myCell)

            End If

        End If

    End Sub

    Protected Overrides Sub AddAttributesToRender(ByVal writer As HtmlTextWriter)
        ' Rendering message box control to the browser
        MyBase.AddAttributesToRender(writer)
        writer.AddAttribute(HtmlTextWriterAttribute.Id, "showimage")
        writer.AddAttribute(HtmlTextWriterAttribute.Style, "Z-INDEX: 9999; LEFT:" + strLeft + "px; WIDTH:" + intMessageBoxWidth.ToString() + "px; POSITION: absolute; TOP: " + strTop + "px; HEIGHT: " + intMessageBoxHeight.ToString() + "px; filter:progid:DXImageTransform.Microsoft.Shadow(color='dimgray', direction='135,' strength='3;'")
    End Sub

#End Region

End Class
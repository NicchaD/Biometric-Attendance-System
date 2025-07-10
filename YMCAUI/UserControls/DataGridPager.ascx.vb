

Public MustInherit Class DataGridPager
    Inherits System.Web.UI.UserControl
    Protected WithEvents lnkFirstbutton As System.Web.UI.WebControls.LinkButton

    Protected WithEvents lnkNewPageList As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkprevbutton As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnknextbutton As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnklastbutton As System.Web.UI.WebControls.LinkButton

    Protected WithEvents lnkPrevPageList As System.Web.UI.WebControls.LinkButton

    Public Event PageChanged(ByVal PgNumber As Integer)
    
    Protected WithEvents lnkpg1 As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkpg2 As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkpg3 As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkpg4 As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkpg5 As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkpg6 As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkpg7 As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkpg8 As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkpg9 As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkpg10 As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkpg11 As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkpg12 As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkpg13 As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkpg14 As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkpg15 As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkpg16 As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkpg17 As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkpg18 As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkpg19 As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkpg20 As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lblPageCount As System.Web.UI.WebControls.Label



    Private dt As System.Web.UI.WebControls.DataGrid

    'default to full display, next/prev,first/last, page list
    Private m_blnShowNextPrev As Boolean = True
    Private m_blnShowFirstLast As Boolean = True
    Private m_blnShowPages As Boolean = True

    'default text for each button
    Private m_strNextPageText = ">"
    Private m_strPrevPageText = "<"
    Private m_strFirstPageText = "<<"
    Private m_strLastPageText = ">>"

    'Number of pages to list in page list
    Private m_intPagesToDisplay = 20 'default

    '[start] - [end] of current page display
    Private m_intCurrentPageStart As Integer
    Private m_intCurrentPageEnd As Integer
    Private m_intTotalRecords As Integer



#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If viewstate("PageDisplayStart") = "" Then viewstate("PageDisplayStart") = "1"
        If Not Page.IsPostBack Then RenderPageList()
        lnkNewPageList.Text = "Next " & PagesToDisplay.ToString
        lnkPrevPageList.Text = "Prev " & PagesToDisplay.ToString

    End Sub
    Private Sub RenderPageList()
        Dim iCtr, iCtr2, iTotalPages, iStart, iEnd, iResolve As Integer
        Dim intPageCount As Integer = dt.PageSize 'number of records per page
        Dim intPagesToDisplay = PagesToDisplay 'number of pages on page bar
        Try
            iStart = CInt(viewstate("PageDisplayStart"))
            iStart = (Int(iStart / intPageCount) * intPageCount) + 1 'page to start on
            'iStart = (Int(iStart / PagesToDisplay) * PagesToDisplay) + 1 'page to start on
            iEnd = iStart + (intPageCount - 1) 'page to end on

            Dim lnkPageNumber As System.Web.UI.WebControls.LinkButton

            iTotalPages = dt.PageCount

            For iCtr = 1 To intPagesToDisplay
                lnkPageNumber = Me.FindControl("lnkpg" & iCtr)

                iResolve = (Int(iStart / intPageCount) * intPageCount) + iCtr

                If iResolve = dt.CurrentPageIndex + 1 Then
                    lnkPageNumber.Attributes.Add("style", "font-weight:bold;padding-right:2px")
                Else
                    lnkPageNumber.Attributes.Add("style", "font-weight:normal;padding-right:2px")
                End If
                lnkPageNumber.Text = iResolve.ToString
                lnkPageNumber.Visible = ShowPages


                If iResolve = iTotalPages Then
                    lnkNewPageList.Visible = False
                    For iCtr2 = iCtr + 1 To intPagesToDisplay
                        lnkPageNumber = Me.FindControl("lnkpg" & iCtr2)
                        lnkPageNumber.Visible = False
                    Next
                    Exit For
                Else
                    lnkNewPageList.Visible = ShowPages
                End If


            Next

            lnkPrevPageList.Visible = iStart > intPageCount And ShowPages

            lnkFirstbutton.Text = FirstPageText
            lnklastbutton.Text = LastPageText
            lnknextbutton.Text = NextPageText
            lnkprevbutton.Text = PrevPageText


            lnkFirstbutton.Visible = ShowFirstLast
            lnklastbutton.Visible = ShowFirstLast
            lnkprevbutton.Visible = ShowNextPrev
            lnknextbutton.Visible = ShowNextPrev
            If TypeOf (dt.DataSource) Is DataTable Then
                m_intTotalRecords = dt.DataSource.rows.count

                m_intCurrentPageStart = (dt.PageSize * (dt.CurrentPageIndex)) + 1
                m_intCurrentPageEnd = m_intCurrentPageStart + dt.PageSize - 1
                If m_intCurrentPageEnd > m_intTotalRecords Then m_intCurrentPageEnd = m_intTotalRecords
            End If
            RaiseEvent PageChanged(dt.CurrentPageIndex)

        Catch up As Exception
            Throw up
        End Try
    End Sub
    Public Property Grid() As DataGrid
        'the grid to use
        'set this property first
        Get
            Return dt
        End Get
        Set(ByVal TheGrid As DataGrid)

            dt = TheGrid

        End Set

    End Property

    Protected Sub ChangePage(ByVal sender As Object, ByVal e As System.EventArgs)
        'Change to next, prev, first, last, or specified page
        Dim lbLink As LinkButton = CType(sender, LinkButton)
        Dim NewPage As String = lbLink.CommandArgument
        'Dim NewPage As String = sender.commandargument.ToString

        Dim iCurrentViewPage As Integer

        Dim iResolvePage As Integer
        Dim intPageSize As Integer
        Dim intPagesToDisplay = PagesToDisplay

        Try

            iCurrentViewPage = CInt(ViewState("PageDisplayStart"))
            intPageSize = dt.PageSize
            Select Case NewPage
                Case "next"
                    If (dt.CurrentPageIndex < (dt.PageCount - 1)) Then
                        dt.CurrentPageIndex += 1
                    End If

                Case "prev"
                    If (dt.CurrentPageIndex > 0) Then
                        dt.CurrentPageIndex -= 1
                    End If

                Case "last"
                    dt.CurrentPageIndex = (dt.PageCount - 1)

                Case ".." 'Next group
                    ViewState("PageDisplayStart") = (CInt(ViewState("PageDisplayStart")) + intPagesToDisplay).ToString
                    dt.CurrentPageIndex = CInt(ViewState("PageDisplayStart")) - 1
                    RenderPageList()
                Case "..." 'previous group
                    ViewState("PageDisplayStart") = (CInt(ViewState("PageDisplayStart")) - intPagesToDisplay).ToString
                    dt.CurrentPageIndex = CInt(ViewState("PageDisplayStart")) - 1
                    RenderPageList()
                Case Else
                    iResolvePage = Convert.ToInt32(NewPage)
                    iResolvePage = iCurrentViewPage + iResolvePage - 1
                    dt.CurrentPageIndex = Convert.ToInt32(iResolvePage)
            End Select

            dt.DataBind()
            ResolvePager()


        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Property ShowNextPrev() As Boolean
        Get
            Return m_blnShowNextPrev
        End Get
        Set(ByVal Value As Boolean)
            m_blnShowNextPrev = Value

        End Set
    End Property


    Public Property ShowFirstLast() As Boolean
        Get
            Return m_blnShowFirstLast
        End Get
        Set(ByVal Value As Boolean)
            m_blnShowFirstLast = Value
        End Set
    End Property


    Public Property ShowPages() As Boolean
        Get
            Return m_blnShowPages
        End Get
        Set(ByVal Value As Boolean)
            m_blnShowPages = Value
        End Set
    End Property

    Public Property NextPageText() As String
        Get
            Return m_strNextPageText
        End Get
        Set(ByVal Value As String)
            m_strNextPageText = Value
        End Set
    End Property

    Public Property PrevPageText() As String
        Get
            Return m_strPrevPageText
        End Get
        Set(ByVal Value As String)
            m_strPrevPageText = Value
        End Set
    End Property


    Public Property FirstPageText() As String
        Get
            Return m_strFirstPageText
        End Get
        Set(ByVal Value As String)
            m_strFirstPageText = Value
        End Set
    End Property
    Public Property LastPageText() As String
        Get
            Return m_strLastPageText
        End Get
        Set(ByVal Value As String)
            m_strLastPageText = Value
        End Set
    End Property

    Public Function GoToPage(ByVal PgNumber As Integer)
        '1 based unlike datagrid itself
        If PgNumber < 1 Or PgNumber > dt.PageCount Then Exit Function
        dt.CurrentPageIndex = PgNumber - 1
        dt.DataBind()
        ResolvePager()
        RaiseEvent PageChanged(dt.CurrentPageIndex)

    End Function
    Private Function ResolvePager()
        Dim iStartPage As Integer
        Dim iEndPage As Integer
        Dim iShouldStartAt As Integer
        Dim intPageSize As Integer
        Dim intPagesToDisplay As Integer = PagesToDisplay
        Dim iCurrentViewPage As Integer = CInt(ViewState("PageDisplayStart"))

        intPageSize = dt.PageSize

        iStartPage = dt.CurrentPageIndex + 1
        iEndPage = iStartPage + intPageSize - 1
        If iEndPage > dt.PageCount Then iEndPage = dt.PageCount

        iShouldStartAt = (Int(iStartPage / intPagesToDisplay) * intPagesToDisplay) + 1
        If iStartPage Mod intPagesToDisplay = 0 Then iShouldStartAt -= intPagesToDisplay
        If iShouldStartAt <> iCurrentViewPage Then
            ViewState("PageDisplayStart") = iShouldStartAt.ToString



        End If
        RenderPageList()
   
    End Function
    Public Property PagesToDisplay() As Integer
        'number of pages to display in page bar (1-20 valid values)
        Get
            Return m_intPagesToDisplay
        End Get
        Set(ByVal Value As Integer)
            If Value >= 1 And Value <= 20 Then m_intPagesToDisplay = Value
        End Set
    End Property

    Public ReadOnly Property CurrentPageStart() As Integer
        Get
            Return m_intCurrentPageStart

        End Get
    End Property

    Public ReadOnly Property CurrentPageEnd() As Integer
        Get
            Return m_intCurrentPageEnd
        End Get
    End Property

    Public ReadOnly Property TotalRecords()
        Get
            Return m_inttotalrecords

        End Get

    End Property

    Public ReadOnly Property CurrentPage()
        Get
            Return dt.CurrentPageIndex + 1
        End Get
    End Property
    Public ReadOnly Property Totalpages()
        Get
            Return dt.PageCount

        End Get

    End Property

    Protected Overrides Sub Finalize()
        MyBase.Finalize()

    End Sub


End Class

' Cache-Session                 : Hafiz 04Feb06
'Neeraj Singh                   12/Nov/2009         Added form name for security issue YRS 5.0-940 

Public Class PopUpCaller
    Inherits System.Web.UI.Page
    'below line is Added by Neeraj for issue id YRS 5.0-940 
    Dim strFormName As String = New String("PopUpCaller.aspx")
    'End issue id YRS 5.0-940

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

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
        '    Dim popupScript As String = "<script language='javascript'>" & _
        '"LeftPosition=(screen.width)?(screen.width-700)/2:100;TopPosition=(screen.height)?(screen.height-500)/2:100;window.open('DisbursementsBreakDown.aspx', 'CustomPopUp', " & _
        '"'width=700, height=500, menubar=no, Resizable=Yes,top='+TopPosition+',left='+LeftPosition +', scrollbars=yes')" & _
        '"</script>"

        '    Page.RegisterStartupScript("PopupScript", popupScript)


        Dim popupScript As String = "<script language='javascript'>" & _
                           "LeftPosition=(screen.width)?(screen.width-700)/2:100;alert(LeftPosition);TopPosition=(screen.height)?(screen.height-500)/2:100;window.open('DisbursementsBreakDown.aspx?PersId=" + "88" + "&DisbId =" + "99" + " & FundId =" + "11" + " & WHAmount = " + "22" + " & Gross = " + "22" + " & Status = " + "REPLACE" + "', 'CustomPopUp', " & _
                           "'width=700, height=500, menubar=no, Resizable=Yes,top=200,left=200, scrollbars=yes')" & _
                           "</script>"

        Page.RegisterStartupScript("PopupScript", popupScript)


    End Sub

End Class

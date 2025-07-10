<%@ Page Language="vb" AutoEventWireup="false" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="YMCAUI" %>
<%@ Import Namespace="Microsoft.Practices.EnterpriseLibrary.Common" %>
<%@ Import Namespace="Microsoft.Practices.EnterpriseLibrary.Data" %>
<%@ Import Namespace="System.Data.Common" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript" src="JS/jquery-1.2.6.pack.js"></script>
    <link href="CSS/CustomStyleSheet.css" type="text/css" rel="stylesheet" />
    <script language="javascript" src="JS/YMCA_JScript.js"></script>
    <style type="text/css">
        #timeout
        {
            position: absolute; 
            top: 0;
            background: red;
            text-align: center;
            color: #fff;
            font-weight: bold;
            width: 100%;
            padding: 5px;
            display: none;
        }
    </style>
    <script language="javascript" type="text/javascript">
        var to;
        $().ready(function() {
            to = setTimeout("TimeOut()",10000);
        });

        function TimeOut()
        {
               CalAjax();
        }

         function UpdateScreen(t) {
        	document.getElementById('output').innerHTML = t;
            alert(t);
        }

        function CalAjax()
        {
         $.ajax({
                type: "POST",
                url: "CashOutIDMWrite.aspx/getpageStatus",
                data: "{}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (returndata) {
                    to = setTimeout("TimeOut()", 10000);
                //$('#output').text(returndata.d);
                },
                 failure: function () {
                    return false;
                }
            });
         }      

         function CallWindow()
         {
            window.open('ProcessCashOut.aspx?count=10', 'ReportPopUp1','width=1024,height=768, menubar=no, resizable=yes,top=0,left=0, scrollbars=yes, status=yes');
         }
         


    </script>
    <script runat="server">
        Protected timeout As Int32
        
        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            timeout = 10000
            If Not Page.IsPostBack Then
                
            End If
        End Sub

        Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSubmit.Click
            Session("pageStatus") = Nothing
            BindDataGrid()
        End Sub

        Private Function GetUnProcessdata() As DataSet
            Dim dsUnProcessIDM As New DataSet
            Try
                If Not String.IsNullOrEmpty(txtBatchId.Text.Trim) Then
                    Session("strBatchId") = txtBatchId.Text.Trim
                    dsUnProcessIDM = GetUnProcessIDM(txtBatchId.Text.Trim)
                    Session("UnprocessData") = dsUnProcessIDM
                    btnProcess.Visible = True
                    btnProcess.Enabled = True
                    trMessage.Visible = False
                    Session("RowCount") = dsUnProcessIDM.Tables(0).Rows.Count
                Else
                    lblMessage.Text = "Please enter batchid"
                    dsUnProcessIDM = Nothing
                    btnProcess.Visible = False
                    btnProcess.Enabled = False
                    trMessage.Visible = True
                End If
            Catch ex As Exception
                Response.Redirect("~/ErrorForSession.aspx?Message=" + Server.UrlEncode("Unprocess IDM"), False)
            End Try
            Return dsUnProcessIDM
        End Function
        Private Sub BindDataGrid()
            Dim drUnProcessData As New DataSet
            Try
                drUnProcessData = GetUnProcessdata()
                If Not HelperFunctions.isEmpty(drUnProcessData) Then
                    gvUnprocessIDM.DataSource = drUnProcessData.Tables(0)
                    gvUnprocessIDM.DataBind()
                    
                    grdBatchSummary.DataSource = drUnProcessData.Tables(1)
                    grdBatchSummary.DataBind()
                    btnProcess.Enabled = True
                Else
                    gvUnprocessIDM.DataSource = Nothing
                    gvUnprocessIDM.DataBind()
                    
                    grdBatchSummary.DataSource = Nothing
                    grdBatchSummary.DataBind()
                    btnProcess.Enabled = False
                End If
            Catch ex As Exception
                Response.Redirect("~/ErrorForSession.aspx?Message=" + Server.UrlEncode("Unprocess IDM") + ex.Message, False)
            End Try
        End Sub
        Private Sub btnProcess_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnProcess.Click
            Session("UnprocessData") = GetUnProcessdata()
            
            Dim popupScript3 As String = "<" & "script language='javascript'>" & _
                    "window.open('ProcessCashOut.aspx', 'ProcessIDM', " & _
                    "'width=600,height=400, menubar=no, resizable=yes,top=0,left=0, scrollbars=yes, status=yes') </" & "script" & ">"
                    
            If (Not Me.IsStartupScriptRegistered("PopupScript2")) Then
                Page.RegisterStartupScript("PopupScript2", popupScript3)
            End If
            btnProcess.Enabled = False
            'HelperFunctions.dtTracking = Nothing
        End Sub

        <System.Web.Services.WebMethod()> _
        Public Shared Function getpageStatus() As String
            Return Convert.ToString(HttpContext.Current.Session("pageStatus"))
        End Function
        
        Protected Sub gvUnprocessIDM_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs)
            Dim dtSortTable As New DataTable
            dtSortTable = CType(Session("UnprocessData"), DataSet).Tables(0)
            If Not dtSortTable Is Nothing Then
                Dim dvSortView As DataView
                dvSortView = New DataView(dtSortTable)
                dvSortView.Sort = e.SortExpression + " " + getSortDirection(e.SortDirection)
                gvUnprocessIDM.DataSource = dvSortView
                gvUnprocessIDM.DataBind()
            End If
        End Sub
        
        Private Function getSortDirection(ByVal sortDirection As SortDirection) As String
            Dim strSortDirection As String = String.Empty
            If sortDirection = sortDirection.Ascending Then
                strSortDirection = "ASC"
            Else
                strSortDirection = "DESC"
            End If
            Return strSortDirection
        End Function
        
        
        Public Function GetUnProcessIDM(ByVal strBatchID As String) As DataSet
            Dim dsGetUnprocessIDM As DataSet
            Dim db As Database = Nothing
            Dim getCommandWrapper As DbCommand = Nothing
            Try
                db = DatabaseFactory.CreateDatabase("YRS")
                
                If db Is Nothing Then
                    Return Nothing
                End If
                getCommandWrapper = db.GetStoredProcCommand("yrs_usp_GetUnprocessIDM")
                getCommandWrapper.CommandTimeout = ConfigurationManager.AppSettings("LargeConnectionTimeOut").ToString()
                db.AddInParameter(getCommandWrapper, "@chvBatchId", DbType.String, strBatchID)
                If getCommandWrapper Is Nothing Then
                    Return Nothing
                End If
                dsGetUnprocessIDM = New DataSet
                db.LoadDataSet(getCommandWrapper, dsGetUnprocessIDM, "UnProcessIDM")
                Return dsGetUnprocessIDM
            Catch ex As Exception
                Throw
            End Try
        End Function
        
        
        Private Sub CopyXMlToDB()
            Dim db As Database = Nothing
            Dim strConnection As String = ""
            Try
                db = DatabaseFactory.CreateDatabase("YRS")
                strConnection = db.ConnectionString
                Dim dsXmlData As New DataSet
                dsXmlData.ReadXml("D:\\ProcessTracking.xml")
                Dim sqlBulckCopy As New SqlBulkCopy(strConnection)
                sqlBulckCopy.DestinationTableName = "XMLDATA"
                sqlBulckCopy.ColumnMappings.Add("FunctionName", "FunctionName")
                sqlBulckCopy.ColumnMappings.Add("Start", "StartDate")
                sqlBulckCopy.ColumnMappings.Add("End", "EndDate")
                sqlBulckCopy.ColumnMappings.Add("Message", "Message")
                sqlBulckCopy.ColumnMappings.Add("PersID", "PersID")
                sqlBulckCopy.WriteToServer(dsXmlData.Tables(0))
            Catch ex As Exception
                Throw
            End Try
        End Sub
        
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div>
        <div>
            <div id='output'>
            </div>
        </div>
        <table class="Table_WithBorder" width="100%">
            <tr id="trMessage" runat="server" visible="false">
                <td bgcolor="#FFCC66">
                    <asp:Label runat="server" ID="lblMessage" Font-Size="Small" ForeColor="#FF6600"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <table width="100%" class="Table_WithBorder">
                        <tr>
                            <td class="Label_Small" width="8%">
                                Batch ID:
                            </td>
                            <td align="left" width="8%">
                                <asp:TextBox runat="server" ID="txtBatchId" CssClass="TextBox_Normal Warn"></asp:TextBox>
                            </td>
                            <td align="left">
                                <asp:Button runat="server" ID="btnSubmit" Text="Submit" CssClass="Button_Normal" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <table width="100%" class="Table_WithBorder">
                        <tr>
                            <td class="Label_Small" width="12%" rowspan="2">
                                Batch Summary:
                            </td>
                            <td>
                                <asp:GridView runat="server" ID="grdBatchSummary" CssClass="DataGrid_Grid" BackColor="White"
                                    BorderColor="#E7E7FF" RowStyle-Width="50px" BorderStyle="None" EmptyDataText="No records found"
                                    BorderWidth="2px" CellPadding="3" AutoGenerateColumns="false" Width="100%">
                                    <RowStyle Width="50px" BackColor="#E7E7FF" ForeColor="#4A3C8C"></RowStyle>
                                    <HeaderStyle BackColor="#4A3C8C" Font-Bold="True" ForeColor="#F7F7F7"></HeaderStyle>
                                    <AlternatingRowStyle BackColor="#F7F7F7" />
                                    <FooterStyle BackColor="#B5C7DE" ForeColor="#4A3C8C" />
                                    <PagerStyle BackColor="#E7E7FF" ForeColor="#4A3C8C" HorizontalAlign="Right" />
                                    <Columns>
                                        <asp:BoundField DataField="chvBatchId" HeaderText="BatchId" />
                                        <asp:BoundField DataField="IncludedInBatch" HeaderText="Included In Batch" />
                                        <asp:BoundField DataField="DocCreated" HeaderText="Doc Created" />
                                        <asp:BoundField DataField="DocCopied" HeaderText="Doc Copied" />
                                        <asp:BoundField DataField="DocNotCopied" HeaderText="Doc Not Copied" />
                                        <asp:BoundField DataField="DocVerifiedInIDM" HeaderText="Doc Verified In IDM" />
                                        <asp:BoundField DataField="DocNotCopiedButVerifiedByIDM" HeaderText="Doc Not Copied But Verified By IDM" />
                                    </Columns>
                                </asp:GridView>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="2" class="Label_Small" width="12%">
                    IDM Details:
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <div style="overflow: auto; height: 400px" class="Table_WithBorder">
                        <asp:GridView runat="server" ID="gvUnprocessIDM" CssClass="DataGrid_Grid" BackColor="White"
                            BorderColor="#E7E7FF" RowStyle-Width="50px" BorderStyle="None" EmptyDataText="No records found"
                            BorderWidth="2px" CellPadding="3" AutoGenerateColumns="false" Width="100%" AllowSorting="true"
                            OnSorting="gvUnprocessIDM_Sorting">
                            <RowStyle Width="50px" BackColor="#E7E7FF" ForeColor="#4A3C8C"></RowStyle>
                            <HeaderStyle BackColor="#4A3C8C" Font-Bold="True" ForeColor="#F7F7F7"></HeaderStyle>
                            <AlternatingRowStyle BackColor="#F7F7F7" />
                            <FooterStyle BackColor="#B5C7DE" ForeColor="#4A3C8C" />
                            <PagerStyle BackColor="#E7E7FF" ForeColor="#4A3C8C" HorizontalAlign="Right" />
                            <Columns>
                                <asp:BoundField DataField="chrSSNo" SortExpression="chrSSNo" HeaderText="SSNo" />
                                <asp:BoundField DataField="FundNo" SortExpression="FundNo" HeaderText="FundNo" />
                                <asp:BoundField DataField="Name" SortExpression="Name" HeaderText="Name" />
                                <asp:BoundField DataField="chvBatchId" SortExpression="chvBatchId" HeaderText="BatchID" />
                                <asp:BoundField DataField="Amount" SortExpression="Amount" HeaderText="Amount" />
                            </Columns>
                        </asp:GridView>
                    </div>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:Button runat="server" ID="btnProcess" Text="Process" CssClass="Button_Normal"
                        Visible="false" />
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>

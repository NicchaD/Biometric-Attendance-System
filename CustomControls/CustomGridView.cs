// REVISION HISTORY:
// ------------------------------------------------------------------------------------------------------
// Developer Name | Date       | Change
// ------------------------------------------------------------------------------------------------------
// Shashank		 | 02/13/2013  | BT-1719 /YERDI3I-1889:Grid sort order indicator	
// Deven         | 07/24/2014  | YERDI3I-2327: Financial screen display alphabetical
// Deven         | 08/08/2014  | Next and Last buttons on pager remains visible when last page in paging sequence 
//               |             | contains records equal to grids page size. (Solved under YERDI3I-2327)
// Rakesh V      | 06/01/2015  | BT-2886: Remove compiler/build warnings
// ------------------------------------------------------------------------------------------------------
// ***************************************
using System;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.Web.UI;
using System.Collections;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace CustomControls
{
    [ToolboxData("<{0}:CustomGridView runat=server></{0}:CustomGridView>")]
    public class CustomGridView : GridView, IPageableItemContainer
    {
        private DataPager pager;
        HtmlGenericControl divPagination, innerDiv, innerAlphabetPagingDiv, divAlphabetHolder; // DS:07/24/2014: Declared innerAlphanetPaginDiv and divAlphabetHolder for YERDI3I-2327
        NextPreviousPagerField f, l;
        NumericPagerField n;

        private SortDirection sortDirection = SortDirection.Ascending;
        private string sortExpression;
        // Start: DS:07/24/2014: For YERDI3I-2327
        private bool bAllowAlphabetPaging;
        private string stCurrentAlphabet;
        // End: DS:07/24/2014: For YERDI3I-2327
        public new SortDirection SortDirection  // Rakesh V | 06/01/2015 | BT-2886: Added new keyword to avoid warning message
        {
            get { return sortDirection; }
            set { sortDirection = value; }
        }
        public new string SortExpression    // Rakesh V | 06/01/2015 | BT-2886: Added new keyword to avoid warning message
        {
            get { return sortExpression; }
            set { sortExpression = value; }
        }
        // Start: DS:07/24/2014: For YERDI3I-2327
        [DefaultValue(false)]
        [Category("Alphabet Paging")]
        [Description("Whether to turn on Alphabet paging functionality in the GridView")]
        public bool AllowAlphabetPaging
        {
            get { return bAllowAlphabetPaging; }
            set { bAllowAlphabetPaging = value; }
        }

        [DefaultValue("ALL")]
        [Category("Alphabet Paging")]
        [Description("Alphabet currently selected from the alphabet list")]
        public string CurrentAlphabet
        {
            get { return stCurrentAlphabet == null ? "ALL" : stCurrentAlphabet; }
            set { stCurrentAlphabet = value; }
        }

        [Category("Alphabet Paging")]
        [TypeConverter(typeof(StringArrayConverter))]
        [Description("The list of alphabets which needs to be shown")]
        public string[] AlphabetsToShow
        {
            get;
            set;
        }

        public delegate void GridViewAlphabetPageEventHandler(object sender, GridViewAlphabetPageEventArgs e);
        [Category("Action")]
        [Description("Fires when the current alphabet of the GridView is changing")]
        public event GridViewAlphabetPageEventHandler AlphabetChanging;
        public void OnAlphabetChanging(GridViewAlphabetPageEventArgs e)
        {
            if (AlphabetChanging != null)
                AlphabetChanging(this, e);
            else
                throw new HttpException("There is no handler for the CustomGridView.AlphabetChanging event.");
        }
        // End: DS:07/24/2014: For YERDI3I-2327
        protected override void OnInit(EventArgs e)  // Rakesh V | 06/01/2015 | BT-2886: Added override keyword to avoid warning message
        {
            this.Page.RegisterRequiresControlState(this);
            base.OnInit(e);
        }

        protected override void LoadControlState(object savedState)
        {
            object[] ctlState = (object[])savedState;
            base.LoadControlState(ctlState[0]);
            this.SortDirection = (SortDirection)ctlState[1];
            this.SortExpression = (string)ctlState[2];
            // Start: DS:07/24/2014: For YERDI3I-2327
            this.AllowAlphabetPaging = Convert.ToBoolean(ctlState[3]);
            this.CurrentAlphabet = Convert.ToString(ctlState[4]);
            // End: DS:07/24/2014: For YERDI3I-2327
        }

        protected override object SaveControlState()
        {
            object[] ctlState = new object[5]; // DS:07/24/2014: Increased object size from 3 to 5 for YERDI3I-2327
            ctlState[0] = base.SaveControlState();
            ctlState[1] = this.SortDirection;
            ctlState[2] = this.SortExpression;
            // Start: DS:07/24/2014: For YERDI3I-2327
            ctlState[3] = this.AllowAlphabetPaging;
            ctlState[4] = this.CurrentAlphabet;
            // End: DS:07/24/2014: For YERDI3I-2327
            return ctlState;
        }
        public CustomGridView()
        {
            pager = new DataPager();
            pager.ID = "pg";
            pager.PagedControlID = this.ID;
            f = new NextPreviousPagerField();
            f.ButtonType = ButtonType.Link;
            f.ShowNextPageButton = false;
            f.ShowFirstPageButton = true;
            f.PreviousPageText = "<< Previous";
            f.ShowPreviousPageButton = true;
                 

            n = new NumericPagerField();
            n.ButtonType = ButtonType.Link;
            n.ButtonCount = 10;
            n.CurrentPageLabelCssClass = "current";

            l = new NextPreviousPagerField();
            l.ButtonType = ButtonType.Link;
            l.ShowNextPageButton = true;
            l.ShowLastPageButton = true;
            l.NextPageText = "Next >>";
            l.ShowPreviousPageButton = false;
            pager.Fields.Add(f);
            pager.Fields.Add(n);
            pager.Fields.Add(l);

            innerDiv = new HtmlGenericControl("div");
            innerDiv.Attributes["class"] = "right-float";
            innerDiv.Controls.Add(pager);

            divPagination = new HtmlGenericControl("div");
            divPagination.ID = "divPagination";
            divPagination.Attributes["class"] = "pagination";
            divPagination.Controls.Add(innerDiv);

            // Start: DS:07/24/2014: For YERDI3I-2327
            divAlphabetHolder = new HtmlGenericControl("div");
            divAlphabetHolder.ID = "divAlphabetHolder";
            SetAlphabetDiv();
            // End: DS:07/24/2014: For YERDI3I-2327
        }
        int IPageableItemContainer.MaximumRows
        {
            get { return this.PageSize; }
        }
        int IPageableItemContainer.StartRowIndex
        {
            get { return this.PageSize * this.PageIndex; }
        }
		
        void IPageableItemContainer.SetPageProperties(int startRowIndex, int maximumRows, bool databind)
        {
            if (maximumRows != this.PageSize)
            {
                maximumRows = this.PageSize;
            }
            int newPageIndex = (startRowIndex / maximumRows);
            this.PageSize = maximumRows;
            if (this.PageIndex != newPageIndex)
            {
                bool isCanceled = false;
                if (databind)
                {
                    //  create the event arguments and raise the event
                    GridViewPageEventArgs args = new GridViewPageEventArgs(newPageIndex);
                    this.OnPageIndexChanging(args);
                    isCanceled = args.Cancel;
                    newPageIndex = args.NewPageIndex;

                    //  if the event wasn't cancelled change the paging values
                    if (!isCanceled)
                    {
                        this.PageIndex = newPageIndex;
                        if (databind)
                            this.OnPageIndexChanged(EventArgs.Empty);
                    }
                    if (databind)
                        this.RequiresDataBinding = true;
                }
                
            }
        }

        protected override int CreateChildControls(IEnumerable dataSource, bool dataBinding)
        {
            LinkButton PreviousLink, NextLink, FirstLink, LastLink;
            int rows = base.CreateChildControls(dataSource, dataBinding);
            //  if the paging feature is enabled, determine the total number of rows in the datasource
            if (this.AllowPaging)
            {
                //  if we are databinding, use the number of rows that were created, otherwise cast the datasource to an Collection and use that as the count
                int totalRowCount = dataBinding ? rows : ((ICollection)dataSource).Count;

                //If there are no records and if records are less than PageSize then hide pagination block.
                if (totalRowCount > this.PageSize)
                {
                    this.Controls.Add(divPagination);
                    //  raise the row count available event
                    IPageableItemContainer pageableItemContainer = this as IPageableItemContainer;
                    this.OnTotalRowCountAvailable(new PageEventArgs(pageableItemContainer.StartRowIndex, pageableItemContainer.MaximumRows, totalRowCount));
                    //  make sure the top and bottom pager rows are not visible
                    if (this.TopPagerRow != null)
                        this.TopPagerRow.Visible = false;

                    if (this.BottomPagerRow != null)
                        this.BottomPagerRow.Visible = false;

                    FirstLink = this.pager.Controls[0].Controls[0] as LinkButton;
                    PreviousLink = this.pager.Controls[0].Controls[2] as LinkButton;
                    NextLink = this.pager.Controls[2].Controls[0] as LinkButton;
                    LastLink = this.pager.Controls[2].Controls[2] as LinkButton;
                    if (this.pager.StartRowIndex >= (this.pager.TotalRowCount - this.PageSize)) // DS: 08/08/2014: Changed the if condition from > to >= to hide Next and Last buttons on pager, when last page contains records equal to page size
                    {
                        NextLink.Visible = false;
                        LastLink.Visible = false;
                    }
                    if (this.pager.StartRowIndex == 0)
                    {
                        PreviousLink.Visible = false;
                        FirstLink.Visible = false;
                    }
                    // Start: DS:07/24/2014: For YERDI3I-2327
                    if (AllowAlphabetPaging)
                    {
                        this.Controls.Add(divAlphabetHolder);
                        SetCurrentAlphabet(true);
                    }
                    // End: DS:07/24/2014: For YERDI3I-2327
                } // Start: DS:07/24/2014: For YERDI3I-2327
                else if (AllowAlphabetPaging)
                {
                    if (totalRowCount == 0)
                    {
                        int iGridViewIndex = this.Parent.Controls.IndexOf(this);
                        this.Parent.Controls.AddAt(iGridViewIndex, divAlphabetHolder);
                        SetCurrentAlphabet(false);
                    }
                    else
                    {
                        this.Controls.Add(divAlphabetHolder);
                        SetCurrentAlphabet(true);
                    }
                }
                // End: DS:07/24/2014: For YERDI3I-2327
            }
            return rows;
        }

        // Start: DS:07/24/2014: For YERDI3I-2327
        private void SetAlphabetDiv()
        {
            innerAlphabetPagingDiv = new HtmlGenericControl("div");
            AddAlphabet("ALL");
            for (int i = 65; i <= 90; i++)
            {
                string stAlphabet = Char.ConvertFromUtf32(i);
                AddAlphabet(stAlphabet);
            }
            innerAlphabetPagingDiv.Attributes["class"] = "pagination AlphabetPagination";
            divAlphabetHolder.Controls.Add(innerAlphabetPagingDiv);
        }

        private void SetCurrentAlphabet(bool bRecordExists)
        {
            HtmlGenericControl divAlphabetHolder;
            if (bRecordExists)
                divAlphabetHolder = (HtmlGenericControl)this.FindControl("divAlphabetHolder");
            else
                divAlphabetHolder = (HtmlGenericControl)this.Parent.FindControl("divAlphabetHolder");

            if (this.AlphabetsToShow == null || this.AlphabetsToShow.Length <= 0)
            {
                if (divAlphabetHolder != null)
                {
                    divAlphabetHolder.Style.Add("display", "none");
                    return;
                }
            }
            else
            {
                if (divAlphabetHolder != null)
                {
                    divAlphabetHolder.Style.Add("display", "block");
                }
            }

            SearchAlphabetAndSetCss(bRecordExists, "ALL");
            for (int i = 65; i <= 90; i++)
            {
                SearchAlphabetAndSetCss(bRecordExists, Char.ConvertFromUtf32(i));
            }
        }

        private void SearchAlphabetAndSetCss(bool bRecordExists, string stAlphabet)
        {
            LinkButton lbtnAlphabet;
            HtmlGenericControl spanSpacer;
            if (bRecordExists)
            {
                lbtnAlphabet = (LinkButton)this.FindControl("Alpha_" + stAlphabet);
                spanSpacer = (HtmlGenericControl)this.FindControl("Span_" + stAlphabet);
            }
            else
            {
                lbtnAlphabet = (LinkButton)this.Parent.FindControl("Alpha_" + stAlphabet);
                spanSpacer = (HtmlGenericControl)this.Parent.FindControl("Span_" + stAlphabet);
            }

            if (lbtnAlphabet.Text == CurrentAlphabet)
            {
                lbtnAlphabet.Enabled = false;
                lbtnAlphabet.CssClass = "current";
            }
            else
            {
                lbtnAlphabet.Enabled = true;
                lbtnAlphabet.CssClass = "";
            }

            if ((this.AlphabetsToShow != null && !this.AlphabetsToShow.Contains(stAlphabet) && stAlphabet != "ALL")
                || (this.AlphabetsToShow == null || this.AlphabetsToShow.Length <= 0))
            {
                lbtnAlphabet.Visible = false;
                spanSpacer.Visible = false;
            }
            else
            {
                lbtnAlphabet.Visible = true;
                spanSpacer.Visible = true;
            }
        }

        private void AddAlphabet(string stAlphabet)
        {
            HtmlGenericControl spanSpacer;
            LinkButton lbtnAlphabet;
            lbtnAlphabet = new LinkButton();
            lbtnAlphabet.ID = "Alpha_" + stAlphabet;
            lbtnAlphabet.Text = stAlphabet;
            lbtnAlphabet.CommandArgument = stAlphabet;
            lbtnAlphabet.CommandName = "AlphabetPaging";
            lbtnAlphabet.Command += new CommandEventHandler(RaiseAlphabetChangingEvent);
            if (stAlphabet == CurrentAlphabet)
            {
                lbtnAlphabet.Enabled = false;
                lbtnAlphabet.CssClass = "current";
            }
            innerAlphabetPagingDiv.Controls.Add(lbtnAlphabet);

            spanSpacer = new HtmlGenericControl("span");
            spanSpacer.ID = "Span_"+stAlphabet;
            spanSpacer.InnerHtml = "&nbsp;";
            innerAlphabetPagingDiv.Controls.Add(spanSpacer);
        }

        private void RaiseAlphabetChangingEvent(object sender, CommandEventArgs e)
        {
            if (e.CommandName == "AlphabetPaging")
            {
                CurrentAlphabet = Convert.ToString(e.CommandArgument);
                GridViewAlphabetPageEventArgs objGridViewAlphabetPageEventArgs = new GridViewAlphabetPageEventArgs(CurrentAlphabet);
                OnAlphabetChanging(objGridViewAlphabetPageEventArgs);
            }
        }
        // End: DS:07/24/2014: For YERDI3I-2327
        event EventHandler<PageEventArgs> IPageableItemContainer.TotalRowCountAvailable
        {
            add { base.Events.AddHandler(CustomGridView.EventTotalRowCountAvailable, value); }
            remove { base.Events.RemoveHandler(CustomGridView.EventTotalRowCountAvailable, value); }
        }

        protected virtual void OnTotalRowCountAvailable(PageEventArgs e)
        {
            EventHandler<PageEventArgs> handler = (EventHandler<PageEventArgs>)base.Events[CustomGridView.EventTotalRowCountAvailable];
            if (handler != null)
            {
                handler(this, e);
            }
        }
        private static readonly object EventTotalRowCountAvailable = new object();

        protected override void OnRowDataBound(GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                Image image = new Image();
			   if(!string.IsNullOrEmpty(SortExpression))
                {
                    foreach (TableCell tc in e.Row.Cells)
                    {
                        if (tc.HasControls())
                        {
                            if (tc.Controls[0] is LinkButton)
                            {
                                LinkButton headerLink = (LinkButton)tc.Controls[0];

                                if (SortExpression == headerLink.CommandArgument)
                                {

                                    if (SortDirection == SortDirection.Descending)
                                    {
                                        image.ImageUrl = "~/resources/images/desc.gif";
                                        image.ToolTip = "Descending";
                                    }
                                    else
                                    {
                                        image.ImageUrl = "~/resources/images/asc.gif";
                                        image.ToolTip = "Ascending";
                                    }
                                    tc.Controls.Add(image);
                                    break;

                                }
                            }
                        }
                    }
                }
            }
            base.OnRowDataBound(e);
        }
    }
    // Start: DS:07/24/2014: For YERDI3I-2327
    public class GridViewAlphabetPageEventArgs : CancelEventArgs
    {
        public GridViewAlphabetPageEventArgs(string stNewAlphabet)
        {
            NewAlphabet = stNewAlphabet;
        }
        public string NewAlphabet
        {
            get;
            set;
        }
    }
    // End: DS:07/24/2014: For YERDI3I-2327
}

//*******************************************************************************
// Copyright 3i Infotech Ltd. All Rights Reserved. 
// Project Name		:	YMCA-YRS
// FileName			:	HelperFunctions.cs
// Author Name		:	Dinesh Kanojia
// Employee ID		:	56257
// Email			:	dinesh.kanojia@3i-infotech.com
// Contact No		:	9876543210
// Creation Time	:	23-Dec-2014
//*******************************************************************************
//************************************************************************************
//Modified By          Date            Description
//*********************************************************************************************************************
//*********************************************************************************************************************

using System;
using System.Text.RegularExpressions;
using System.Data;
using System.Collections;
using System.IO;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using System.Diagnostics;
using System.Web.UI.WebControls;
using System.Web.UI;

namespace CommonUtilities
{
    public class HelperFunctions
    {
        //Added By Shashi Shekhar:2009-12-23: Define variable to use globally on page whereever CheckAccess()javascript function is called from code behind
        public static String SecurityCheckString = "javascript:if(CheckAccess('{0}')==false)return false;";
        private static Regex isGuid = new Regex("^(\\{){0,1}[0-9a-fA-F]{8}\\-[0-9a-fA-F]{4}\\-[0-9a-fA-F]{4}\\-[0-9a-fA-F]{4}\\-[0-9a-fA-F]{12}(\\}){0,1}$");
        private static Regex isSSN = new Regex("[0-9]{9}");
        private static Regex isFundNo = new Regex("[0-9]");
        private static Regex isYMCANo = new Regex("[0-9]{6}");

        #region IsNonEmpty Functions
        public static bool isNonEmpty(ref DataSet ds)
        {
            if (ds == null)
                return false;
            if (ds.Tables.Count == 0)
                return false;
            if (ds.Tables[0].Rows.Count == 0)
                return false;
            return true;
        }
        public static bool isNonEmpty(ref DataView dv)
        {
            if (dv == null)
                return false;
            if (dv.Count == 0)
                return false;
            return true;
        }
        public static bool isNonEmpty(ref DataTable dt)
        {
            if (dt == null)
                return false;
            if (dt.Rows.Count == 0)
                return false;
            return true;
        }
        public static bool isNonEmpty(ref object obj)
        {
            if (obj == null)
                return false;
            if ((obj) is DataSet)
            {
                return isNonEmpty(ref obj);
            }
            if (Convert.ToString(obj).Trim() == string.Empty)
                return false;
            return true;
        }
        #endregion

        #region IsEmpty Functions
        public static object isEmpty(ref object obj)
        {
            return !isNonEmpty(ref obj);
        }
        public static object isEmpty(ref DataTable dt)
        {
            return !isNonEmpty(ref dt);
        }
        public static bool isEmpty(ref DataSet ds)
        {
            return !isNonEmpty(ref ds);
        }
        public static bool isEmpty(ref DataView dv)
        {
            return !isNonEmpty(ref dv);
        }
        #endregion

        public static DataRow GetRowForUpdation(DataTable p_datatable, string p_string_name, string p_string_key)
        {
            DataRow[] l_datarows = null;
            DataRow l_datarow = null;
            try
            {
                l_datarows = p_datatable.Select(p_string_name + "='" + p_string_key + "'");
                if (l_datarows.Length > 0)
                {
                    l_datarow = l_datarows[0];
                }
                return l_datarow;
            }
            catch
            {
                throw;
            }
        }

        #region ParseCSV - Convert CSV file to Data Table

        //function that parses any CSV input string into a DataTable
        public static DataTable ParseCSV(string inputString)
        {
            try
            {
                DataTable dt = new DataTable();
                // declare the Regular Expression that will match versus the input string 
                //Regular expression which handle empty value in CSV File
                Regex re = new Regex("(?<field>,)|((?<field>[^\",\\r\\n]+)|\"(?<field>([^\"]|\"\")+)\")(,|(?<rowbreak>\\r\\n|\\n|$))");
                ArrayList colArray = new ArrayList();
                ArrayList rowArray = new ArrayList();
                int colCount = 0;
                int maxColCount = 0;
                string rowbreak = "";
                string field = "";
                MatchCollection mc = re.Matches(inputString);
                foreach (Match m in mc)
                {
                    // retrieve the field and replace two double-quotes with a single double-quote 
                    field = m.Result("${field}").Replace("\"\"", "\"").Trim();

                    if (field == ",")
                    {
                        field = " ";
                    }

                    rowbreak = m.Result("${rowbreak}");

                    if (field.Length > 0)
                    {
                        colArray.Add(field);
                        colCount += 1;
                    }


                    if (rowbreak.Length > 0)
                    {
                        // add the column array to the row Array List 
                        rowArray.Add(colArray.ToArray());

                        // create a new Array List to hold the field values 
                        colArray = new ArrayList();

                        if (colCount > maxColCount)
                        {
                            maxColCount = colCount;
                        }

                        colCount = 0;
                    }
                }

                if (rowbreak.Length == 0)
                {
                    // this is executed when the last line doesn't 
                    // end with a line break 
                    rowArray.Add(colArray.ToArray());
                    if (colCount > maxColCount)
                    {
                        maxColCount = colCount;
                    }
                }

                // create the columns for the table 
                for (int i = 0; i <= maxColCount - 1; i++)
                {
                    dt.Columns.Add(String.Format("col{0:000}", i));
                }

                // convert the row Array List into an Array object for easier access 
                Array ra = rowArray.ToArray();

                for (int i = 0; i <= ra.Length - 1; i++)
                {
                    // create a new DataRow 
                    DataRow dr = dt.NewRow();

                    // convert the column Array List into an Array object for easier access 
                    Array ca = (Array)(ra.GetValue(i));

                    // add each field into the new DataRow 
                    for (int j = 0; j <= ca.Length - 1; j++)
                    {
                        dr[j] = ca.GetValue(j).ToString().Trim();
                    }

                    // add the new DataRow to the DataTable 
                    dt.Rows.Add(dr);
                }

                // in case no data was parsed, create a single column 
                if (dt.Columns.Count == 0)
                {
                    dt.Columns.Add("NoData");
                }
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataTable ParseCSVFile(string path)
        {
            try
            {
                string inputString = "";

                // check that the file exists before opening it 

                if (File.Exists(path))
                {
                    StreamReader sr = new StreamReader(path);
                    inputString = sr.ReadToEnd();

                    sr.Close();
                }

                return ParseCSV(inputString.Trim());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Logger methods
        public static object LogException(string paraMessage, Exception paraException)
        {
            Exception newExp = null;
            try
            {
                newExp = new Exception(paraMessage, paraException);
                return ExceptionPolicy.HandleException(newExp, "Exception Policy");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool LogMessage(string paraMessage)
        {
            try
            {
                Logger.Write(paraMessage, "Application", 0, 0, TraceEventType.Information);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion

        #region BindGrid Functions
        public static void BindGrid(ref DataGrid dg, ref DataSet ds, bool forceVisible = false)
        {
            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
            {
                dg.DataSource = null;
                dg.DataBind();
                dg.Visible = forceVisible;
                return;
            }
            else
            {
                dg.DataSource = ds.Tables[0];
                dg.DataBind();
                dg.Visible = true;
            }
        }
        public static void BindGrid(ref DataGrid dg, ref DataView dv, bool forceVisible = false)
        {
            if (dv == null || dv.Count == 0)
            {
                dg.DataSource = null;
                dg.DataBind();
                dg.Visible = forceVisible;
                return;
            }
            else
            {
                dg.DataSource = dv;
                dg.DataBind();
                dg.Visible = true;
            }
        }
        //SR:2013.10.15 - added common binding function for Grid view		
        public static void BindGrid(ref GridView dg, ref DataSet ds, bool forceVisible = false)
        {
            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
            {
                dg.DataSource = null;
                dg.DataBind();
                dg.Visible = forceVisible;
                return;
            }
            else
            {
                dg.DataSource = ds.Tables[0];
                dg.DataBind();
                dg.Visible = true;
            }
        }
        public static void BindGrid(ref GridView dg, ref DataView dv, bool forceVisible = false)
        {
            if (dv == null || dv.Count == 0)
            {
                dg.DataSource = null;
                dg.DataBind();
                dg.Visible = forceVisible;
                return;
            }
            else
            {
                dg.DataSource = dv;
                dg.DataBind();
                dg.Visible = true;
            }
        }
        #endregion

        #region DataTable - Set selected image of selected row
        public static void SetSelectedImageOfDataGrid(System.Object sender, System.EventArgs e, string RadioButtonName)
        {
            int i = 0;
            DataGrid dg = (DataGrid)sender;

            for (i = 0; i <= dg.Items.Count - 1; i++)
            {
                if (dg.Items[i].ItemType == ListItemType.AlternatingItem || dg.Items[i].ItemType == ListItemType.Item || dg.Items[i].ItemType == ListItemType.SelectedItem)
                {
                    ImageButton l_button_Select = default(ImageButton);

                    //l_button_Select = DirectCast(DataGridYMCAContact.Items(i).FindControl(RadioButtonName), ImageButton)
                    l_button_Select = (ImageButton)dg.Items[i].FindControl(RadioButtonName);
                    if ((l_button_Select != null))
                    {
                        if (i == dg.SelectedIndex)
                        {
                            l_button_Select.ImageUrl = "images\\selected.gif";
                        }
                        else
                        {
                            l_button_Select.ImageUrl = "images\\select.gif";
                        }
                    }
                }
            }
        }
        #endregion

        public static bool IsValidGuid(string value)
        {
            bool isValid = false;
            if (!String.IsNullOrEmpty(value))
            {
                if (isGuid.IsMatch(value))
                {
                    isValid = true;
                }
            }
            return isValid;
        }

        //Shashi Singh: 24 feb 2011: Function to validate SSN

        public static bool IsValidSSN(string value)
        {
            bool isValid = false;
            if (!String.IsNullOrEmpty(value))
            {
                if (isSSN.IsMatch(value))
                {
                    isValid = true;
                }
            }
            return isValid;
        }

        ////Shashi Singh: 24 feb 2011: Function to validate FundNo
        
        public static bool IsValidFundNo(string value)
        {
            bool isValid = false;
            if (!String.IsNullOrEmpty(value))
            {
                if (isFundNo.IsMatch(value))
                {
                    isValid = true;
                }
            }
            return isValid;
        }

        ////Shashi Singh: 24 feb 2011: Function to validate YMCANo
        
        public static bool IsValidYMCANo(string value)
        {
            bool isValid = false;
            if (!String.IsNullOrEmpty(value))
            {
                if (isYMCANo.IsMatch(value))
                {
                    isValid = true;
                }
            }
            return isValid;
        }
        ////BS:2012.01.27:Add common function to sanitize values for javascript
        public static string SanitizeValueForJS(string s)
        {
            return s.Replace("\\", "\\\\").Replace("'", "\\'").Replace("\"", "\\\"");
        }


        ////AA:16.12.2013 - BT:2311 Added to sort the gridviews
        ///// <summary>
        ///// Sets the sort expression 
        ///// </summary>
        ///// <param name="sortstate"></param>
        ///// <param name="sortexpression"></param>
        ///// <param name="dv"></param>
        ///// <exception cref="Exception"> Exceptions throw </exception>
        ///// <remarks> Setting the sorting</remarks>
        public static void gvSorting(ref GridViewCustomSort sortstate, string sortexpression, DataView dv)
        {
            string oldsortexpression = string.Empty;
            try
            {
                if (sortstate != null)
                {
                    oldsortexpression = sortstate.SortExpression;
                }
                else
                {
                    sortstate = new GridViewCustomSort();
                }

                if (sortexpression == oldsortexpression)
                {
                    if (sortstate.SortDirection.ToUpper() == "ASC")
                    {
                        sortstate.SortDirection = "DESC";
                    }
                    else
                    {
                        sortstate.SortDirection = "ASC";
                    }
                }
                else
                {
                    sortstate.SortDirection = "ASC";
                }

                sortstate.SortExpression = sortexpression;

                sortexpression = sortstate.SortExpression + " " + sortstate.SortDirection;
                if (dv != null)
                {
                    dv.Sort = sortexpression;
                }
                else
                {
                    throw new Exception("Dataview cannot be blank");
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        ////AA:16.12.2013 - BT:2311 Added to set the Arrows on sorting

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="sorting"></param>
        ///// <param name="gv"></param>
        ///// <remarks></remarks>
        public static void SetSortingArrows(GridViewCustomSort sorting, System.Web.UI.WebControls.GridViewRowEventArgs gv)
        {
            LinkButton lnk = new LinkButton();
            System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
            //TableCell tc = new TableCell();
            try
            {
                if (gv.Row.RowType == DataControlRowType.Header & sorting != null)
                {
                    foreach (TableCell tc in gv.Row.Cells)
                    {
                        // search for the header link    
                        if (tc.HasControls())
                        {
                            //AA:25.03.2014 - BT:957: YRS 5.0-1484 - Changed for not occuring error
                            if (tc.Controls[0].GetType().ToString() == "System.Web.UI.WebControls.DataControlLinkButton")
                            {
                                lnk = (LinkButton)tc.Controls[0];
                                // inizialize a new image   
                                if (lnk != null && sorting.SortExpression == lnk.CommandArgument)
                                {
                                    img.ImageUrl = "~/images/" + (sorting.SortDirection.ToUpper() == "ASC" ? "asc" : "desc") + ".gif";
                                    // adding a space and the image to the header link    
                                    tc.Controls.Add(new LiteralControl(" "));
                                    tc.Controls.Add(img);
                                }
                            }
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
        }
    }
}

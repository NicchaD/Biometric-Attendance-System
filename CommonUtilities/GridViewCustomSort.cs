//****************************************************
//Modification History
//****************************************************
//Modified by          Date          Description
//****************************************************
//Manthan Rajguru      2015.09.16    YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Added 'YMCARET' Namespace)
//*****************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace YMCARET.CommonUtilities
{
    [Serializable()]
    public class GridViewCustomSort
    {

        string strSortExpression;
        string strSortDirection;
        public string SortExpression
        {
            get { return strSortExpression; }
            set { strSortExpression = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <exception cref="Test"/>
        /// <remarks></remarks>
        public string SortDirection
        {
            get { return strSortDirection; }
            set { strSortDirection = value; }
        }
    }
}

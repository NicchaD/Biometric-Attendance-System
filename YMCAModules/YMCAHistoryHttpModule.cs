//****************************************************
//Created by    Date          Description
//-------------------------------------------------
//Bhavna		23.06.2011     Creating HttpModule for maintain history of all menuitems  
//Bhavna		26.08.2011     Changes on  findDisplayTextForNode()
//Manthan       2015.09.16     YRS-AT-2550: YRS data cleanup: copyright comments and namespace/reference (Changed the Namespace/reference from 'Infotech' to 'YMCARET')
//*****************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using System.Configuration;
using System.Data;
using System.Xml;
using System.Web.Caching;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace YMCAHttpModules
{
	/// <summary>
	/// Summary description for YMCAHistoryHttpModule
	/// </summary>
	/// 
	public class YMCAHistoryHttpModule : IHttpModule, IReadOnlySessionState, IRequiresSessionState
	{

		string str = String.Empty;
		public YMCAHistoryHttpModule()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		// IHttpModule Members  

		public void Dispose()
		{
			//Implement Dispose if required.  
		}
		public void Init(HttpApplication context)
		{
			//Register the events of interest in Init method. 
			context.AcquireRequestState += new EventHandler(context_AcquireRequestState);
			Logger.Write("Adding YMCAHistoryHttpModule into the pipeline", "Application", 1, 1, System.Diagnostics.TraceEventType.Information);
		}

		void context_AcquireRequestState(object sender, EventArgs e)
		{
			HttpApplication httpApp = (HttpApplication)sender;
			HttpContext ctx = HttpContext.Current;

			int vUserId = 0;
			string vPath = String.Empty;
			string v_Url = String.Empty;
			string v_Display = String.Empty;
			string v_Result = String.Empty;
			try
			{
				DataTable dt;
				if (HttpContext.Current.Session != null && HttpContext.Current.Session["LoggedUserKey"] != null && HttpContext.Current.Session["LoggedUserKey"].ToString().Length != 0)
				{
					vUserId = Convert.ToInt16(ctx.Session["LoggedUserKey"]);
					vPath = ctx.Request.RawUrl;
					dt = (DataTable)ctx.Application["SimpleXmlCache"];
					if (dt == null)
					{
						//here simpleXML.xml convert into datatable
						dt = MapXml();
						ctx.Application["SimpleXmlCache"] = dt;

					}
					foreach (DataRow row in dt.Rows)
					{
						v_Display = row["Display"].ToString();
						v_Url = row["Url"].ToString();

						//here raw url path convert into actual url which will come from database
						if (vPath.Substring(vPath.LastIndexOf("/"), (vPath.Length - vPath.LastIndexOf("/"))) == "/" + v_Url)
						{
							v_Result = YMCARET.YmcaBusinessObject.YMCAMenuHistoryBOClass.Insert_MenuHistory(vUserId, v_Url, v_Display).ToString();
						}

					}


				}
			}
			catch (Exception ex)
			{
				Logger.Write(ex.Message, "Application", 1, 1, System.Diagnostics.TraceEventType.Information);
			}

		}
		private DataTable MapXml()
		{
			DataTable dtable = new DataTable("MenuItemTable");
			dtable.Columns.Add(new DataColumn("Display", typeof(string)));
			dtable.Columns.Add(new DataColumn("Url", typeof(string)));
			// here we recreate  SimpleXML.xml
			XmlTextReader xtr = new XmlTextReader(HttpContext.Current.Server.MapPath("~/SimpleXML.xml"));
			xtr.WhitespaceHandling = WhitespaceHandling.None;
			XmlDocument xd = new XmlDocument();
			xd.Load(xtr);
			XmlNode xnodRoot = xd.DocumentElement;
			XmlNode xnodWorking = default(XmlNode);
			if (xnodRoot.HasChildNodes)
			{
				xnodWorking = xnodRoot.FirstChild;
				while ((xnodWorking != null))
				{
					ProcessChildren(xnodWorking, dtable);
					xnodWorking = xnodWorking.NextSibling;
				}
			}
			return dtable;
		}
		private void ProcessChildren(XmlNode xnod, DataTable dt)
		{
			//we're only going to process Text and Element nodes
			if ((xnod.NodeType == XmlNodeType.Element) || (xnod.NodeType == XmlNodeType.Text))
			{
				if (xnod.Name == "menuItem")
				{
					string url = findUrlForNode(xnod);
					string displayText = findDisplayTextForNode(xnod).Substring(3);
					dt.Rows.Add(new object[] { displayText, url });
				}
				//recursively process the children of this node
				XmlNode xnodworking = default(XmlNode);
				if (xnod.HasChildNodes)
				{
					xnodworking = xnod.FirstChild;
					while ((xnodworking != null))
					{
						ProcessChildren(xnodworking, dt);
						xnodworking = xnodworking.NextSibling;
					}
				}
			}

		}
		private string findUrlForNode(XmlNode p)
		{
			foreach (XmlNode c in p.ChildNodes)
			{
				if (c.Name == "url") return c.InnerText;
			}
			return null;
		}
		private string findDisplayTextForNode(XmlNode x)
		{
			if (x.Name == "subMenu") x = x.ParentNode;
			foreach (XmlNode n in x.ChildNodes)
			{
				if (n.Name == "text") return findDisplayTextForNode(x.ParentNode) + " > " + n.InnerText.Replace("|","").Trim();
			}
			return null;
		}

	}
}
//****************************************************
// Date			Created by		Description
//****************************************************
// 2011.11.10	Nikunj Patel	Updated code to handle YRS 5.0-1480
//
//*****************************************************
using System;
using System.IO;
using System.Web;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace YMCAHttpModules
{

	public class YMCAStringReplacementHttpModule : IHttpModule
	{

		const string CONTEXT_KEY = "YMCAStringReplacementHttpModule";

		public void Init(HttpApplication context)
		{
			//context.BeginRequest += new EventHandler(BeginRequest);
			context.ReleaseRequestState += new EventHandler(HandleOpportunityToInstall);
			//// have to sink PreSendRequestHeaders too to handle calls to Flush
			//// comment this next line out and hit Flusher.ashx to see it fail
			context.PreSendRequestHeaders += new EventHandler(HandleOpportunityToInstall);
			Logger.Write("Adding YMCAStringReplacementHttpModule into the pipeline", "Application", 1, 1, System.Diagnostics.TraceEventType.Information);
		}

		public void Dispose() { }

		private void HandleOpportunityToInstall(object sender, EventArgs e)
		{
			HttpApplication app = sender as HttpApplication;
			if (app.Request.CurrentExecutionFilePathExtension.ToLower() == ".aspx")
			{
				if (!app.Context.Items.Contains(CONTEXT_KEY))
				{
					if (app.Response.ContentType == "text/html")
					{
						app.Response.Filter = new YMCAStringReplacementFilter(app.Response.Filter);
					}

					app.Context.Items.Add(CONTEXT_KEY, new object());
				}
			}
		}

	}

	public class YMCAStringReplacementFilter : HttpFilterBase
	{

		public YMCAStringReplacementFilter(Stream originalFilter)
			: base(originalFilter)
		{
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
			Byte[] data = new Byte[count];
			Buffer.BlockCopy(buffer, offset, data, 0, count);
			String html = System.Text.Encoding.Default.GetString(buffer);
			if (html.Contains(@"__doPostBack=function(t,a){bobj.event.publish('saveViewState');__CRYSTALREPORTVIEWERONSUBMIT12(t,a);}function WebForm_OnSubmit()")) {
				html = html.Replace(@"__doPostBack=function(t,a){bobj.event.publish('saveViewState');__CRYSTALREPORTVIEWERONSUBMIT12(t,a);}function WebForm_OnSubmit()", 
									@"__doPostBack=function(t,a){bobj.event.publish('saveViewState');__CRYSTALREPORTVIEWERONSUBMIT12(t,a);};function WebForm_OnSubmit()"); 
			}
			//NP:2011.11.10:YRS 5.0-1480 - Added a hack to make Crystal Report Viewer to use the default printer settings of the user.
			if (html.Contains(@",'driverName':'Microsoft XPS Document Writer',")) {
				html = html.Replace(@",'driverName':'Microsoft XPS Document Writer',", @",'driverName':'DISPLAY',");
			}
			if (html.Contains(@",'useDefPrinter':false,")) {
				html = html.Replace(@",'useDefPrinter':false,", @",'useDefPrinter':true,");
			}
			//This particular setting may also be of interest - html = html.Replace(@",'useDefPrinterSettings':false,", @",'useDefPrinterSettings':true,");
			Byte[] outdata = System.Text.Encoding.Default.GetBytes(html);
			OriginalFilter.Write(outdata, 0, outdata.GetLength(0));
		}

	}
	public abstract class HttpFilterBase : Stream
	{
		#region Protected Properties

		protected Stream OriginalFilter { get; private set; }

		#endregion

		#region Constructors

		protected HttpFilterBase(Stream originalFilter)
		{
			OriginalFilter = originalFilter;
		}

		#endregion

		#region Public Overrides Properites

		public override bool CanRead { get { return true; } }
		public override bool CanSeek { get { return true; } }
		public override bool CanWrite { get { return true; } }
		public override long Length { get { return 0; } }
		public override long Position { get; set; }

		#endregion

		#region Public Overrides Methods

		public override void Flush()
		{
			OriginalFilter.Flush();
		}
		public override int Read(byte[] buffer, int offset, int count)
		{
			return OriginalFilter.Read(buffer, offset, count);
		}
		public override long Seek(long offset, SeekOrigin origin)
		{
			return OriginalFilter.Seek(offset, origin);
		}
		public override void SetLength(long value)
		{
			OriginalFilter.SetLength(value);
		}
		public override void Close()
		{
			OriginalFilter.Close();
		}

		#endregion
	}
}

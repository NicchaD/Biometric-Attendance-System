//****************************************************
//Modification History
//****************************************************
//Modified by          Date          Description
//****************************************************
//Manthan Rajguru      2015.09.16    YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
//*****************************************************
using System;
using System.Data ;
using YMCARET.YmcaDataAccessObject;

namespace YMCARET.YmcaBusinessObject
{
	/// <summary>
	/// Summary description for AnnuityPayrollOutsourceBOClass.
	/// </summary>
	public class AnnuityPayrollOutsourceBOClass
	{
		public AnnuityPayrollOutsourceBOClass()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public static DataSet GetLatestPayroll()
		{	
			
			try
			{
				return (YMCARET.YmcaDataAccessObject.AnnuityPayrollOutSourceDAClass.GetLatestPayroll()) ;
			}
			catch 
			{
				throw;
			}

		}
		private  DataTable GetEDISegment()
		{
			DataTable dtSegment=new DataTable("Segment");
			DataRow drNew;
			DataColumn dcNew=new DataColumn("SegID",typeof(string));
			dtSegment.Columns.Add(dcNew);
			 dcNew=new DataColumn("Name",typeof(string));
			dtSegment.Columns.Add(dcNew);
			 dcNew=new DataColumn("ReqSeg",typeof(string));
			dtSegment.Columns.Add(dcNew);
			 dcNew=new DataColumn("MaxUse",typeof(int));
			dtSegment.Columns.Add(dcNew);
			dcNew=new DataColumn("Purpose",typeof(int));
			dtSegment.Columns.Add(dcNew);
			//Filling the DataTable
			drNew=dtSegment.NewRow();
			drNew["SegID"]="ISA";drNew["Name"]="Interchange and Functional Group Information Segment";
			drNew["ReqSeg"]="M";drNew["MaxUse"]=1;dtSegment.Rows.Add(drNew);
			drNew=dtSegment.NewRow();
			drNew["SegID"]="GS";drNew["Name"]="Functional Group";
			drNew["ReqSeg"]="M";drNew["MaxUse"]=1;dtSegment.Rows.Add(drNew);
			drNew=dtSegment.NewRow();
			drNew["SegID"]="ST";drNew["Name"]="Transaction Set Header";
			drNew["ReqSeg"]="M";drNew["MaxUse"]=1;dtSegment.Rows.Add(drNew);
			drNew=dtSegment.NewRow();
			drNew["SegID"]="BPR";drNew["Name"]="Beginning Segment for Payment Order/Remittance Advice";
			drNew["ReqSeg"]="M";drNew["MaxUse"]=1;dtSegment.Rows.Add(drNew);
			drNew=dtSegment.NewRow();
			drNew["SegID"]="TRN";drNew["Name"]="Trace";
			drNew["ReqSeg"]="N";drNew["MaxUse"]=1;dtSegment.Rows.Add(drNew);
			drNew=dtSegment.NewRow();
			drNew["SegID"]="REF";drNew["Name"]="Reference Numbers";
			drNew["ReqSeg"]="M";drNew["MaxUse"]=2;drNew["Purpose"]="To uniquely identify a transaction to an application";dtSegment.Rows.Add(drNew);
			drNew=dtSegment.NewRow();
			drNew["SegID"]="N1";drNew["Name"]="Name";
			drNew["ReqSeg"]="N";drNew["MaxUse"]=1;dtSegment.Rows.Add(drNew);
			drNew=dtSegment.NewRow();
			drNew["SegID"]="N2";drNew["Name"]="Additional Name Information";
			drNew["ReqSeg"]="N";drNew["MaxUse"]=2;dtSegment.Rows.Add(drNew);
			drNew=dtSegment.NewRow();
			drNew["SegID"]="N3";drNew["Name"]="Address Information";
			drNew["ReqSeg"]="N";drNew["MaxUse"]=2;dtSegment.Rows.Add(drNew);
			drNew=dtSegment.NewRow();
			drNew["SegID"]="N4";drNew["Name"]="Geographic Location";
			drNew["ReqSeg"]="N";drNew["MaxUse"]=1;dtSegment.Rows.Add(drNew);
			drNew=dtSegment.NewRow();
			drNew["SegID"]="SE";drNew["Name"]="Transaction Set Trailer";
			drNew["ReqSeg"]="M";drNew["MaxUse"]=1;dtSegment.Rows.Add(drNew);
			drNew=dtSegment.NewRow();
			drNew["SegID"]="N4";drNew["Name"]="Geographic Location";
			drNew["ReqSeg"]="N";drNew["MaxUse"]=1;dtSegment.Rows.Add(drNew);
			drNew=dtSegment.NewRow();
			drNew["SegID"]="N4";drNew["Name"]="Geographic Location";
			drNew["ReqSeg"]="N";drNew["MaxUse"]=1;dtSegment.Rows.Add(drNew);
			drNew=dtSegment.NewRow();
			
			return dtSegment;
		}
	}
}

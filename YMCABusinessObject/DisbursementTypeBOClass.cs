//*******************************************************************************
// Copyright YMCA Retirement Fund All Rights Reserved. 
//
// Project Name			:	YMCA - YRS
// FileName				:	DisbursementTypeBOClass.cs
// Author Name			:	SrimuruganG
// Employee ID			:	32365
// Email				:	srimurugan.ag@icici-infotech.com
// Contact No			:	8744
// Creation Time		:	7/25/2005 5:39:20 PM
// Program Specification Name	:	
// Unit Test Plan Name			:	
// Description					:	<<Please put the brief description here...>>
//*******************************************************************************
//Modification History
//*******************************************************************************
//Modified by          Date          Description
//*******************************************************************************
//Manthan Rajguru      2015.09.16    YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
//*******************************************************************************
using System;
using System.Data;
using YMCARET.YmcaDataAccessObject;

namespace YMCARET.YmcaBusinessObject
{
	/// <summary>
	/// Summary description for DisbursementTypeBusinessClass.
	/// </summary>
	public sealed class DisbursementTypeBOClass
	{
		private DisbursementTypeBOClass()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public static DataSet LookupDisbursementTypes ()
		{
			return (YMCARET.YmcaDataAccessObject.DisbursementTypeDAClass.LookupDisbursementTypes ());
		}

		public static void UpdateDataSet (DataSet disbursementDataSet)
		{
			YMCARET.YmcaDataAccessObject.DisbursementTypeDAClass.UpdateDataSet (disbursementDataSet);
		}

		public static DataSet LookupDisbursementTypes (string searchDisbursementType)
		{
			return (YMCARET.YmcaDataAccessObject.DisbursementTypeDAClass.LookupDisbursementTypes(searchDisbursementType));
		}


	}
}

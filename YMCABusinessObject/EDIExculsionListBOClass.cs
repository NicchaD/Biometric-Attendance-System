/******************************************************************************************************************
 Copyright YMCA Retirement Fund All Rights Reserved. 
 
 Project Name		:	YMCABusinessObject
 BOClassName		:	EDIExculsionListBOClass.cs
 Author Name		:	Ashutosh Patil
 Employee ID		:	36307
 Email				:	ashutosh.patil@3i-infotech.com
 Contact No			:	8568
 Creation Date		:	30-Apr-2007
******************************************************************************************************************** 
Modification History
********************************************************************************************************************
Modified By        Date            Description
********************************************************************************************************************
Manthan Rajguru    2015.09.16      YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
********************************************************************************************************************/
using System;
using System.Collections;
using System.Data;
using YMCARET.YmcaDataAccessObject;

namespace YMCARET.YmcaBusinessObject
{
	/// <summary>
	/// Summary description for EDIExculsionListBOClass.
	/// </summary>
	public class EDIExculsionListBOClass
	{
		public EDIExculsionListBOClass()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		public static DataSet GetEDIExlusionList()
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.EDIExculsionListDAClass.GetEDIExlusionList();
			}
			catch
			{
				throw;
			}
		} 
		public static void  InsertParticipantsintoList(DataSet DataSetEDIExlusionlist)
		{
			try
			{
				 YMCARET.YmcaDataAccessObject.EDIExculsionListDAClass.InsertParticipantsintoList(DataSetEDIExlusionlist);
			}
			catch
			{
				throw;
			}
		} 
	}
}

//****************************************************
//Modification History
//****************************************************
//Modified by          Date          Description
//****************************************************
//Manthan Rajguru      2015.09.16    YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
//*****************************************************
using System;
using System.Data;
using YMCARET.YmcaDataAccessObject;

namespace YMCARET.YmcaBusinessObject
{
	/// <summary>
	/// Summary description for GroupMembersBOClass.
	/// </summary>
	public sealed class GroupMembersBOClass
	{
		public GroupMembersBOClass()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		public static DataSet SearchMembersForGroup(string parameterGroupId)
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.GroupMembersDAClass.SearchMembersForGroup(parameterGroupId);
			}
			catch
			{
				throw;
			}
		}
		public static void UpdateGroupMembers(int parameterGroupId, string parameterUserList)
		{
			try
			{
				YMCARET.YmcaDataAccessObject.GroupMembersDAClass.UpdateGroupMembers(parameterGroupId,parameterUserList);
			}
			catch
			{
				throw;
			}
		}
	}
}

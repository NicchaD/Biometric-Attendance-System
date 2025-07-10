//****************************************************
//Modification History
//****************************************************
//Modified by          Date          Description
//****************************************************
//Manthan Rajguru      2015.09.16    YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
//*****************************************************
using System;
using System.Data;
using System.Security.Permissions;

using YMCARET.YmcaDataAccessObject;


namespace YMCARET.YmcaBusinessObject
{
	/// <summary>
	/// Summary description for CommonLookUpTablesBOClass.
	/// </summary>
	public sealed class CommonLookUpTablesBOClass
	{
		private CommonLookUpTablesBOClass()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public static DataSet LookupTables()
					{
						try
						{
							

							return (YMCARET.YmcaDataAccessObject.CommonLookUpTablesDAClass.LookUpTables());

							
						
						}
						catch 
						{
							throw;
						}
					}
	}
}

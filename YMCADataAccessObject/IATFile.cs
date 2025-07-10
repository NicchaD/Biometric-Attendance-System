//*******************************************************************************
// Copyright YMCA Retirement Fund All Rights Reserved. 
//
// Project Name		:	YMCA-YRS
// FileName			:	IATFile.cs
// Author Name		:	Prasad Jadhav
// Employee ID		:	55509	
// Email			:	prasad.jadhav@3i-infotech.com
// Contact No		:	
// Creation Time	:	7/12/2011 11:55:28 AM
// Program Specification Name	:	
// Unit Test Plan Name			:	
// Description					:	BT:855- YRS 5.0-1344 : Create new IAT output file
//*******************************************************************************
//'Modification History
//'*************************************************************************************************************************************************************************************
//'Modified By			           Date		                		  Description
//'*************************************************************************************************************************************************************************************
//prasad Jadhav                    7/27/2011                          Record Number Added
//prasad Jadhav                    08/16/2011                         Remove method Added to remove decimal amount and time changed to 24hr clock  
//prasad Jadhav                    08/17/2011                         Payroll date added in the record number 5 and block count logic of record nunmer 9 changed Record no.715 and 716 changed 
//Prasad Jadhav					   08/18/2011						  Updating code to not truncate the name in the local variable and to accept the check date instead of the payroll date. Also passing boolProof into the create method as it may be needed in the future.
//Prasad Jadhav                    10/04/2011                         For BT-645,YRS 5.0-632 : Test database output files need word "test" in them.
//Prasad Jadhav                    10/11/2011                         For BT-645,YRS 5.0-632 : Test database output files need word "test" in them. 
//Manthan Rajguru                  2015.09.16                         YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
//Sanjay GS Rawat                  2018.09.14                         YRS-AT-4080 -  YRS enh : Need enhancement to IAT file for payroll (TrackIT 34812) 
//*************************************************************************************************************************************************************************************

using System;
using System.Data;
using System.IO;
using System.Globalization;

namespace YMCARET.YmcaDataAccessObject
{

	public class IATFile
	{
		#region "Constants"
		const string C_BATCHCOUNT = "000001";
		const string C_COMPANYID = "1135562401";
		const string C_ORIGINATINGDFIID = "07100015";
		const string C_SERVICECLASSCODE = "200";

		#endregion "Constants"
		#region Global Vriable
        string String_PersID,
                String_City,
                String_State,
                String_ZIP,
                String_Country,
                String_DBAddress2,
                String_DBAddress3,
                String_Address1,
                String_Address2,
                String_Address3,
                String_FundID,
                String_Name,
                String_ReceivingDFEAccount,
                String_EftTypeCode,
                String_BankABANumberNinthDigit,
                String_PaymentMethodCode,
                String_BankAbaNumber,
                String_DBFirstName,
                String_DBMiddleName,
                String_DBLastName,
                String_BankName,
            //prasad Jadhav:4-oct-2011:for YRS 5.0-632 : Test database output files need word "test" in them.
                String_Test_Production;
		DateTime DateTime_currentDateTime,DateTime_Payroll;
		double double_NetPayMonthTotal, TotalMonthlyNet;


		private DataTable l_datatable_FileList;
		StreamWriter StreamOutputFileIAT, StreamOutputFileIAT_Back;
		string String_OutputFileIATGuid;//ANN
		string String_DisbursementType = "ANN";

		string C_BATCHNUMBER = string.Empty;
        int BatchRecordNumber = 0;
		double double_OutputFilesIATDetailSum;
		double double_OutputFileIATDetailCount;
		double int32_OutputFileIATDetailHash = 0;


		#endregion
		//GeneratedFiles
		public DataTable GeneratedFiles
		{
			get { return l_datatable_FileList; }
		}
		public IATFile()
		{

		}

		public bool CreateIATFile(DataSet ParameterDatasetPayroll,int parameterBatchNumber, DateTime parameterDateTimePayroll,bool parameterBoolProof,string parameterTestOrProduction)
		{
			DataRow[] foundRows;
			DataTable foundDataTable = null;
            DateTime_Payroll = parameterDateTimePayroll;
            String_Test_Production = parameterTestOrProduction;
			try
			{
				if (ParameterDatasetPayroll.Tables["PayRoll"] == null) { return false; }
				foundDataTable = ParameterDatasetPayroll.Tables["PayRoll"].Clone();
				C_BATCHNUMBER = parameterBatchNumber.ToString("0000000");
				string l_String_expr = "PaymentMethodCode = 'IAT' ";
				foundRows = ParameterDatasetPayroll.Tables["PayRoll"].Select(l_String_expr);
				if (foundRows.Length == 0) return false;
				foreach (DataRow row in foundRows)
				{
					object[] dr = row.ItemArray;
					foundDataTable.Rows.Add(dr);
				}

				if (CreateFileName() == false) return false;
				ProduceOutputFilesCreateHeader();
				CreateIATRecordsForEachRow(foundDataTable);
				ProduceOutputFilesCreateFooter();
				return true;
			}
			catch(Exception ex)
			{
                throw ex;
			}
			finally
			{
				CloseOutputStreamFiles();
			}
		}

		private bool CreateFileName()
		{
			this.DateTime_currentDateTime = DateTime.Now;
			this.l_datatable_FileList = new DataTable();
			DataColumn SourceFolder = new DataColumn("SourceFolder", typeof(String));
			DataColumn SourceFile = new DataColumn("SourceFile", typeof(String));
			DataColumn DestFolder = new DataColumn("DestFolder", typeof(String));
			DataColumn DestFile = new DataColumn("DestFile", typeof(String));
			this.l_datatable_FileList.Columns.Add(SourceFolder);
			this.l_datatable_FileList.Columns.Add(SourceFile);
			this.l_datatable_FileList.Columns.Add(DestFolder);
			this.l_datatable_FileList.Columns.Add(DestFile);
			string lcFileName, lcFilenameSuffix, lcOutPutFileNameBak, lcDateSuffix, lcDateTimeSuffix, lcOutputFileName, lcMetaOutputFileType;
            lcOutputFileName = string.Empty;
			DateTime ld_date = this.DateTime_currentDateTime;
			DataSet l_dataset = new DataSet();
			DataSet dsNewGuid;
			object[] rowVals = new object[4];
			DataRow l_datarow_FileList;
			//Needs to clarify ,Is it reuire Proof file for IAT?
			//if (this.ProofReport) lcFilenameSuffix = "Proof";
			//else lcFilenameSuffix = "";
			lcFilenameSuffix = "";
			l_dataset = YMCARET.YmcaDataAccessObject.MonthlyPayrollDAClass.MetaOutputChkFileType();
			lcFileName = "";
			lcOutPutFileNameBak = "";
			lcMetaOutputFileType = "";
			if (l_dataset == null) return false;
			ld_date = this.DateTime_currentDateTime;
			lcDateSuffix = ld_date.ToString("MMddyy").Trim();
			lcDateTimeSuffix = ld_date.ToString("yyyyMMdd").Trim();
			lcDateTimeSuffix += "_" + ld_date.ToString("hhmmss").Trim();
			foreach (DataRow oRow in l_dataset.Tables[0].Rows)
			{
				lcFileName = oRow["FilenamePrefix"].ToString().Trim();
				lcMetaOutputFileType = oRow["OutputFileType"].ToString().Trim();
				if (Convert.ToBoolean(oRow["DateSuffix"]) == true)
				{
					lcFileName += lcDateSuffix;
				}
				if (Convert.ToBoolean(oRow["PaymentManagerSuffix"]) == true)
				{
					lcFileName += lcFilenameSuffix;
				}
				if (oRow.IsNull("OutputDirectory") || oRow["OutputDirectory"].ToString().Trim() == "") return false;

				if (lcMetaOutputFileType == "IAT")
				{
					l_datarow_FileList = this.l_datatable_FileList.NewRow();
					l_datarow_FileList["SourceFolder"] = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["IAT"]);
                    //Added by prasad:10/11/2011 For BT-645,YRS 5.0-632 : Test database output files need word "test" in them. 
                    if (String_Test_Production == "TEST")
                    {
                        l_datarow_FileList["SourceFile"] = l_datarow_FileList["SourceFolder"].ToString() + "\\" + lcFileName.Trim()+"_TEST" + "." + oRow["FilenameExtension"].ToString().Trim();
                    }
                    else
                    if (String_Test_Production == "PRODUCTION")
                    {
                        l_datarow_FileList["SourceFile"] = l_datarow_FileList["SourceFolder"].ToString() + "\\" + lcFileName.Trim() + "." + oRow["FilenameExtension"].ToString().Trim();
                    }
					l_datarow_FileList["DestFolder"] = oRow["OutputDirectory"].ToString().Trim();
                    if (String_Test_Production == "TEST")
                    {
                        l_datarow_FileList["DestFile"] = oRow["OutputDirectory"].ToString().Trim() + "\\" + lcFileName.Trim()+"_TEST" + "." + oRow["FilenameExtension"].ToString().Trim();
                    }
                    else
                    if (String_Test_Production == "PRODUCTION")
                    {
                        l_datarow_FileList["DestFile"] = oRow["OutputDirectory"].ToString().Trim() + "\\" + lcFileName.Trim() + "." + oRow["FilenameExtension"].ToString().Trim();
                    }
					
					if (!Directory.Exists(l_datarow_FileList["SourceFolder"].ToString()))
					{
						//Throw an exception.
						throw new Exception("Error! Output Directory for IAT does not exist. \nFolder: " + l_datarow_FileList["SourceFolder"].ToString() + " Not Found");
					}
					else
					{
						this.l_datatable_FileList.Rows.Add(l_datarow_FileList);
					}

					l_datarow_FileList = this.l_datatable_FileList.NewRow();
					l_datarow_FileList["SourceFolder"] = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["IAT"]);
                    if (String_Test_Production == "TEST")
                    {
                        l_datarow_FileList["SourceFile"] = l_datarow_FileList["SourceFolder"].ToString() + "\\" + lcFileName.Trim() + "_" + lcDateTimeSuffix.Trim() + "_TEST" + "." + oRow["FilenameExtension"].ToString().Trim();
                    }
                    if (String_Test_Production == "PRODUCTION")
                    {
                        l_datarow_FileList["SourceFile"] = l_datarow_FileList["SourceFolder"].ToString() + "\\" + lcFileName.Trim() + "_" + lcDateTimeSuffix.Trim() + "." + oRow["FilenameExtension"].ToString().Trim();
                    }
					
					l_datarow_FileList["DestFolder"] = oRow["OutputDirectory"].ToString().Trim();
                    if (String_Test_Production == "TEST")
                    {
                        l_datarow_FileList["DestFile"] = oRow["OutputDirectory"].ToString().Trim() + "\\" + lcFileName.Trim() + "_" + lcDateTimeSuffix.Trim() + "_TEST" + "." + oRow["FilenameExtension"].ToString().Trim();
                    }
                    if (String_Test_Production == "PRODUCTION")
                    {
                        l_datarow_FileList["DestFile"] = oRow["OutputDirectory"].ToString().Trim() + "\\" + lcFileName.Trim() + "_" + lcDateTimeSuffix.Trim() + "." + oRow["FilenameExtension"].ToString().Trim();
                    }
					if (!Directory.Exists(l_datarow_FileList["SourceFolder"].ToString()))
					{
						// Throw an exception.
						throw new Exception("Error! Output Directory for IAT file does not exist. \nFolder: " + l_datarow_FileList["SourceFolder"].ToString() + " Not Found");
					}
					else
					{
						this.l_datatable_FileList.Rows.Add(l_datarow_FileList);
					}

                    //prasad Jadhav:4-oct-2011:for YRS 5.0-632 : Test database output files need word "test" in them.
                    if (String_Test_Production == "TEST")
                    {
                        lcOutputFileName = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["IAT"]) + "\\" + lcFileName.Trim() +"_TEST" +"." + oRow["FilenameExtension"].ToString().Trim(); ;
                        lcOutPutFileNameBak = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["IAT"]) + "\\" + lcFileName.Trim() + "_" + lcDateTimeSuffix.Trim() +"_TEST"+ "." + oRow["FilenameExtension"].ToString().Trim();
                    }
                    else if (String_Test_Production == "PRODUCTION")
                        {
                            lcOutputFileName = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["IAT"]) + "\\" + lcFileName.Trim() + "." + oRow["FilenameExtension"].ToString().Trim(); ;
                            lcOutPutFileNameBak = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["IAT"]) + "\\" + lcFileName.Trim() + "_" + lcDateTimeSuffix.Trim() + "." + oRow["FilenameExtension"].ToString().Trim();
                        }

					this.StreamOutputFileIAT = File.CreateText(lcOutputFileName.Trim());
					this.StreamOutputFileIAT_Back = File.CreateText(lcOutPutFileNameBak.Trim());

					dsNewGuid = YMCARET.YmcaDataAccessObject.MonthlyPayrollDAClass.GetNewGuid();

					if (dsNewGuid != null && dsNewGuid.Tables[0] != null && dsNewGuid.Tables[0].Rows.Count > 0)
					{
						this.String_OutputFileIATGuid = dsNewGuid.Tables[0].Rows[0]["Newguid"].ToString().Trim();
					}
				}
			}
			if (this.StreamOutputFileIAT == null) throw new Exception("Configuration entry not defined for IAT file in the database");
			return true;

		}

		private void ProduceOutputFilesCreateHeader()
		{
			string l_String_Output;
			DateTime ld_date;
            ld_date = this.DateTime_currentDateTime;
            //Added by prasad to make 24hr clock written HHmm 08/16/2011
            //Added by prasad YRS 5.0-632 : Test database output files need word "test" in them.
            if (String_Test_Production == "TEST")
            {
                l_String_Output = "TYPE=TESTACH  REP=SKD";
                WriteOutputFilesDataLineIntoStream(l_String_Output);
            }
            
            l_String_Output = "101 0710001521135562401" + ld_date.ToString("yyMMdd") + ld_date.ToString("HHmm") + "A094101Northern Trust Company YMCA Retirement Fund           ";
			WriteOutputFilesDataLineIntoStream(l_String_Output);
			//Insert record 5
            //Payroll date added 08/17/2011
            //prasad Jadhav:4-oct-2011:for YRS 5.0-632 : Test database output files need word "test" in them.
            if (String_Test_Production == "TEST")
            {
                //Added by prasad:For BT-645,YRS 5.0-632 : Test database output files need word "test" in them. (reopen).
                l_String_Output = "5200         1234567890       FF3               US1135562401IATANNUITY_PYUSDUSD" + DateTime_Payroll.ToString("yyMMdd") + "   " + "107100015" + C_BATCHNUMBER;
                l_String_Output = l_String_Output.Substring(0, 94);
            }
            else if (String_Test_Production == "PRODUCTION")
            {
                l_String_Output = "5200                FF3               US1135562401IATANNUITY_PYUSDUSD" + DateTime_Payroll.ToString("yyMMdd") + "   " + "107100015" + C_BATCHNUMBER;
            }
			WriteOutputFilesDataLineIntoStream(l_String_Output);
		}
		private void CreateIATRecordsForEachRow(DataTable IATDetails)
		{
            BatchRecordNumber = 0;
			foreach (DataRow oRow in IATDetails.Rows)
			{
                InitializeVariables(oRow);
				WriteOutputFilesDataLineIntoStream(CreateDetails6());
				WriteOutputFilesDataLineIntoStream(CreateDetails710());
				WriteOutputFilesDataLineIntoStream(CreateDetails711());
				WriteOutputFilesDataLineIntoStream(CreateDetails712());
				WriteOutputFilesDataLineIntoStream(CreateDetails713());
				WriteOutputFilesDataLineIntoStream(CreateDetails714());
				WriteOutputFilesDataLineIntoStream(CreateDetails715());
				WriteOutputFilesDataLineIntoStream(CreateDetails716());
			}
		}
		private void ProduceOutputFilesCreateFooter()
		{
			NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
			nfi.CurrencyGroupSeparator = "";
			nfi.CurrencySymbol = "";
			nfi.CurrencyDecimalDigits = 2;
			WriteOutputFilesDataLineIntoStream(CreateDetails8(nfi));
			WriteOutputFilesDataLineIntoStream(CreateDetails9(nfi));
		}

		private void InitializeVariables(DataRow oRow)
		{
            //Increment record number within batch
            BatchRecordNumber++;
			//Clear all variables being used 
			double ln_tmp, ln_tmp1, ln_tmp2;
			this.String_PersID = this.String_Address1 = this.String_Address2 = this.String_Address3 = "";
			this.String_FundID = "";
			this.String_Name = this.String_ReceivingDFEAccount = "";
			this.String_EftTypeCode = this.String_BankABANumberNinthDigit = "";
			this.double_NetPayMonthTotal = this.TotalMonthlyNet = 0;
			this.String_City = this.String_State = this.String_ZIP = this.String_Country = string.Empty;
			this.String_DBAddress2 = this.String_DBAddress3 = string.Empty;
			this.String_DBFirstName = this.String_DBMiddleName = this.String_DBLastName = string.Empty;
			//Initialize all variables from the Data row for use 
			this.String_PersID = oRow["PersID"].ToString();
			this.String_Address1 = oRow["Addr1"].ToString().Trim();
			if (!oRow.IsNull("City")) this.String_City = oRow["City"].ToString().Trim();
			if (!oRow.IsNull("StateType")) this.String_State = oRow["StateType"].ToString().Trim();
			if (!oRow.IsNull("Zip")) this.String_ZIP = oRow["Zip"].ToString().Trim();
			if (!oRow.IsNull("Country")) this.String_Country = oRow["Country"].ToString().Trim();
			if (!oRow.IsNull("Addr2")) this.String_DBAddress2 = oRow["Addr2"].ToString();
			this.String_FundID = Convert.ToString(oRow["FundIDNo"]);
			if (!oRow.IsNull("FirstName")) this.String_DBFirstName = oRow["FirstName"].ToString();
			if (!oRow.IsNull("MiddleName")) this.String_DBMiddleName = oRow["MiddleName"].ToString();
			if (!oRow.IsNull("LastName")) this.String_DBLastName = oRow["LastName"].ToString();
			if (oRow["MiddleName"].ToString().Trim() == "Guardian of")
			{
				this.String_Name = oRow["FirstName"].ToString();
			}
			else
			{
				this.String_Name = oRow["FirstName"].ToString().Trim() + " ";
				if (oRow["MiddleName"].ToString().Trim() != "")
				{
					this.String_Name = this.String_Name + oRow["MiddleName"].ToString().Trim() + " ";
				}
				this.String_Name = this.String_Name + oRow["Lastname"].ToString().Trim();
			}

			// Monthly Withholding data.
			ln_tmp = ln_tmp1 = ln_tmp2 = 0;
			if (!oRow.IsNull("CurrentPayment"))
			{
				ln_tmp = Convert.ToDouble(oRow["CurrentPayment"]);
			}
			if (!oRow.IsNull("MonthlyWithhold"))
			{
				ln_tmp1 = Convert.ToDouble(oRow["MonthlyWithhold"]);
			}
			if (!oRow.IsNull("SocialSecurityAdjPayment"))
			{
				ln_tmp2 = Convert.ToDouble(oRow["SocialSecurityAdjPayment"]);
			}
			// CurrentPayment - lnTotalMonthlyWithholdings + SocialSecurityAdjPayment

			this.double_NetPayMonthTotal = ln_tmp - ln_tmp1 + ln_tmp2;

			if (this.double_NetPayMonthTotal < 0) this.double_NetPayMonthTotal = 0;

			this.String_ReceivingDFEAccount = oRow["BankAcctNumber"].ToString();
			this.TotalMonthlyNet = this.double_NetPayMonthTotal;


			if (!oRow.IsNull("PaymentMethodCode"))
				this.String_PaymentMethodCode = oRow["PaymentMethodCode"].ToString().Trim();
			else this.String_PaymentMethodCode = "";


			if (!oRow.IsNull("EftTypeCode"))
				this.String_EftTypeCode = oRow["EftTypeCode"].ToString();
			else this.String_EftTypeCode = "";

			if (!oRow.IsNull("EftTypeCode"))
				this.String_BankAbaNumber = oRow["BankAbaNumber"].ToString().Substring(0, 8);
			else this.String_BankAbaNumber = "";


			if (String_BankAbaNumber.Trim().Length != 9)
			{
				if (oRow["EftTypeCode"].GetType().ToString() != "System.DBNull") this.String_BankABANumberNinthDigit = oRow["BankAbaNumber"].ToString().Trim().ToCharArray(8, 1).GetValue(0).ToString();
				else this.String_BankABANumberNinthDigit = "";
			}
			else
			{
				this.String_BankABANumberNinthDigit = "";
			}

			if (!oRow.IsNull("BankName"))
				this.String_BankName = oRow["BankName"].ToString();
			else this.String_BankName = "";
		}
		private string CreateDetails6()
		{

			string l_String_Output = string.Empty;

			string l_String_double_NetPayMonthTotal = string.Empty;
			l_String_Output = "6";
			l_String_Output += this.String_EftTypeCode.Trim();
			l_String_Output += this.String_BankAbaNumber.Trim().PadRight(8, ' ');
			l_String_Output += this.String_BankABANumberNinthDigit;
			l_String_Output += "0007".PadRight(17, ' ');
            //Added by prasad .Remove mehod to remove decimal point 08/16/2011
			//l_String_Output += this.double_NetPayMonthTotal.ToString().Trim().PadLeft(11, '0').Remove(8,1);
            l_String_Output += (this.double_NetPayMonthTotal * 100).ToString("0000000000");
            //l_String_Output += this.double_NetPayMonthTotal.ToString("0.00").Replace(".","").PadLeft(10, '0');
			l_String_Output += this.String_ReceivingDFEAccount;
			l_String_Output = (l_String_Output.Length > 78 ? l_String_Output.Substring(0, 78) : l_String_Output);

			l_String_Output = l_String_Output.PadRight(78, ' ');
			l_String_Output += "1";
            l_String_Output += this.BatchRecordNumber.ToString().PadLeft(15, '0');

			//This code is written before WriteOutputFilesDataLineIntoStream(l_String_Output);
			this.double_OutputFileIATDetailCount += 1;
			if (this.String_DisbursementType.ToString() == "ANN")
			{
				this.double_OutputFilesIATDetailSum += this.double_NetPayMonthTotal;
			}
			else
			{
				l_String_double_NetPayMonthTotal = this.double_NetPayMonthTotal.ToString().Trim();
				this.double_OutputFilesIATDetailSum += Convert.ToDouble(l_String_double_NetPayMonthTotal);
			}

			if (this.String_BankAbaNumber.Trim().Length > 8)
			{
				this.int32_OutputFileIATDetailHash += Int32.Parse(this.String_BankAbaNumber.Trim().Substring(0, 8));
			}
			else
			{
				this.int32_OutputFileIATDetailHash += Int32.Parse(this.String_BankAbaNumber.Trim());
			}

			return l_String_Output;

		}
		private string CreateDetails710()
		{
			string l_String_Output = string.Empty;
			l_String_Output += "710ANN";
            //Added by prasad .Remove mehod to remove decimal point 08/16/2011
			//l_String_Output += this.double_NetPayMonthTotal.ToString().Trim().PadLeft(19, '0').Remove(16,1);
            l_String_Output += (this.double_NetPayMonthTotal * 100).ToString("000000000000000000");
            //l_String_Output += this.double_NetPayMonthTotal.ToString("0.00").Replace(".","").PadLeft(18, '0');
			l_String_Output = l_String_Output.PadRight(46, ' ');
			l_String_Output += String_Name;
			//l_String_Output = (l_String_Output.Length > 87 ? l_String_Output.Substring(0, 87) : l_String_Output);
            //name size is 35 character including space 08/17/2011
            l_String_Output = (l_String_Output.Length > 81 ? l_String_Output.Substring(0, 81) : l_String_Output);
			l_String_Output = l_String_Output.PadRight(87, ' ');
            l_String_Output += this.BatchRecordNumber.ToString().PadLeft(7, '0');

			return l_String_Output;
		}
		private string CreateDetails711()
		{
            //START: SR | 2018.08.29 | YRS-AT-4080 | Changed the address
            //return "711YMCA Retirement Fund               140 Broadway 27th Floor                          " + this.BatchRecordNumber.ToString().PadLeft(7, '0');
            return "711YMCA Retirement Fund               120 Broadway 19th Floor                          " + this.BatchRecordNumber.ToString().PadLeft(7, '0');
            //END: SR | 2018.08.29 | YRS-AT-4080 | Changed the address
		}
		private string CreateDetails712()
		{
            //START: SR | 2018.08.29 | YRS-AT-4080 | Changed the zip code 
            //return "712NEW YORK*NEW YORK\\                 US*10005\\                                        " + this.BatchRecordNumber.ToString().PadLeft(7, '0');
            return "712NEW YORK*NEW YORK\\                 US*10271\\                                        " + this.BatchRecordNumber.ToString().PadLeft(7, '0');
            //END: SR | 2018.08.29 | YRS-AT-4080 | Changed the zip code 
		}
		private string CreateDetails713()
		{
            return "713THE NORTHERN TRUST                 01071000152                         US           " + this.BatchRecordNumber.ToString().PadLeft(7, '0');
		}
		private string CreateDetails714()
		{
			string l_String_Output = string.Empty;

			l_String_Output += "714";

			l_String_Output += String_BankName;

			if (l_String_Output.Length > 38)
			{
				l_String_Output = l_String_Output.Substring(0, 38);
			}
			else
			{
				l_String_Output = l_String_Output.PadRight(38, ' ');
			}
			l_String_Output += "01";
			l_String_Output += String_BankAbaNumber.Trim();
			//l_String_Output += this.String_BankABANumberNinthDigit.Trim().Substring(this.String_BankABANumberNinthDigit.Trim().Length - 1, 1);
			l_String_Output += this.String_BankABANumberNinthDigit;
			l_String_Output = l_String_Output.PadRight(74, ' ');
			l_String_Output += "US";
			l_String_Output = l_String_Output.PadRight(87, ' ');
            l_String_Output += this.BatchRecordNumber.ToString().PadLeft(7, '0');
			return l_String_Output;
		}
		private string CreateDetails715()
		{
			string l_String_Output = string.Empty;
			l_String_Output += "715";
			l_String_Output += String_FundID;
            l_String_Output = l_String_Output.PadRight(18, ' ');
			l_String_Output += String_Address1;
            //To leave blank
			//l_String_Output = (l_String_Output.Length > 87 ? l_String_Output.Substring(0, 87) : l_String_Output);
            l_String_Output = (l_String_Output.Length > 53 ? l_String_Output.Substring(0, 53) : l_String_Output);
			l_String_Output = l_String_Output.PadRight(87, ' ');
            l_String_Output += this.BatchRecordNumber.ToString().PadLeft(7, '0');
			return l_String_Output;
		}
		private string CreateDetails716()
		{
			string l_String_Output = string.Empty;
			l_String_Output += "716";
			l_String_Output += String_City;
			if (String_State.Length != 0)
			{
				l_String_Output += "*";
				l_String_Output += String_State;
			}
			if (l_String_Output.Length > 37)
			{
				l_String_Output = l_String_Output.Substring(0, 37);
			}
			l_String_Output += @"\";
			l_String_Output = l_String_Output.PadRight(38, ' ');
			l_String_Output += String_Country;
			if (String_ZIP.Length != 0)
			{
				l_String_Output += "*";
				l_String_Output += String_ZIP;
			}
            //To leave blank
			//if (l_String_Output.Length > 86) 
            if (l_String_Output.Length > 72)
			{
				//l_String_Output = l_String_Output.Substring(0, 86);
                l_String_Output = l_String_Output.Substring(0, 72);
			}
			l_String_Output += @"\";
			l_String_Output = l_String_Output.PadRight(87, ' ');
            l_String_Output += this.BatchRecordNumber.ToString().PadLeft(7, '0');
			return l_String_Output;
		}
		private string CreateDetails8(NumberFormatInfo nfi)
		{
			string l_String_temp, l_String_Output;
			l_String_Output = "";
			double_OutputFileIATDetailCount = double_OutputFileIATDetailCount * 8;
			l_String_Output = "8" + C_SERVICECLASSCODE;
			l_String_Output += this.double_OutputFileIATDetailCount.ToString().Trim().PadLeft(6, '0');
			l_String_temp = this.int32_OutputFileIATDetailHash.ToString().Trim().PadLeft(20, '0');
			l_String_Output += l_String_temp.Substring(10, 10).ToString();
			l_String_temp = "";
			l_String_Output += l_String_temp.PadRight(12, '0');
			l_String_Output += this.double_OutputFilesIATDetailSum.ToString("C", nfi).PadLeft(13, '0').Remove(10, 1);
			l_String_Output += C_COMPANYID;
			l_String_Output += l_String_temp.PadRight(25, ' ');
			l_String_Output += C_ORIGINATINGDFIID + C_BATCHNUMBER;
			return l_String_Output;
		}
		private string CreateDetails9(NumberFormatInfo nfi)
		{
			int l_long_Blocks;
			string l_String_temp, l_String_Output;
			l_String_Output = "9" + C_BATCHCOUNT;
            l_long_Blocks = (int)this.double_OutputFileIATDetailCount + 4;
            //New logic of block count implemented 08/17/2011
            l_long_Blocks = (int)Math.Ceiling((double)l_long_Blocks / 10);
			l_String_Output += l_long_Blocks.ToString().Trim().PadLeft(6, '0');
			l_String_Output += this.double_OutputFileIATDetailCount.ToString().Trim().PadLeft(8, '0');
			l_String_temp = "";
			l_String_temp = this.int32_OutputFileIATDetailHash.ToString().Trim().PadLeft(20, '0');
			l_String_Output += l_String_temp.Substring(10, 10).ToString();
			l_String_temp = "";
			l_String_Output += l_String_temp.PadRight(12, '0');
			l_String_Output += this.double_OutputFilesIATDetailSum.ToString("C", nfi).Trim().PadLeft(13, '0').Remove(10, 1);
			l_String_temp = "";
			l_String_Output += l_String_temp.PadRight(39, ' ');
			return l_String_Output;
		}

		private void WriteOutputFilesDataLineIntoStream(string parameterStringOutput)
		{
			string l_String_Output = parameterStringOutput;
			try
			{
				StreamOutputFileIAT.WriteLine(l_String_Output);
				StreamOutputFileIAT_Back.WriteLine(l_String_Output);
			}
			catch
			{
				CloseOutputStreamFiles();
				throw;
			}
		}
		private void CloseOutputStreamFiles()
		{
			if (StreamOutputFileIAT != null)
			{
				StreamOutputFileIAT.Close();
				StreamOutputFileIAT.Dispose();
				StreamOutputFileIAT = null;
			}
			if (StreamOutputFileIAT_Back != null)
			{
				StreamOutputFileIAT_Back.Close();
				StreamOutputFileIAT_Back.Dispose();
				StreamOutputFileIAT_Back = null;
			}
		}

	}

}

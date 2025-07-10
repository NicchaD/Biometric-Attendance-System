//****************************************************
//Modification History
//****************************************************
//Modified by          Date          Description
//****************************************************
//Manthan Rajguru      2015.09.16    YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
//*****************************************************
using System;
using System.IO;
using System.Data;
using System.Security.Permissions;
using System.Collections; 
using System.Globalization;

namespace YMCARET.YmcaBusinessObject
{
	/// <summary>
	/// Summary description for EDIBOClass.
	/// </summary>
	/// 
	public class EDIBOClass
	{
		DataTable l_datatable_FileList;

		public EDIBOClass()
		{
			//
			// TODO: Add constructor logic here
			//
		
		}

		public DataTable datatable_FileList
		{
			get{return l_datatable_FileList;}
		}

		public static DataSet GetEDIOutSourceList(string ParameterDisbursementType)
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.EDIDAClass.GetEDIOutSourceList(ParameterDisbursementType);

			}
			catch
			{
				throw;
			}
		}	

//		public static void UpdateEDIProcessList(string ParameterDisbursementType,string ParameterProcessId, DataSet Payrolllist,int ParameterPayrollProcessed,int ParameterBatchNo)
//		{
//			try
//			{
//				YMCARET.YmcaDataAccessObject.EDIDAClass.UpdateEDIProcessList(ParameterDisbursementType,ParameterProcessId,Payrolllist,ParameterPayrollProcessed,ParameterBatchNo);
//
//			}
//			catch
//			{
//				throw;
//			}
//		}

	
		public  bool ProcessEDIChecks(DataTable parameterEDIDatatable,DateTime parameterDateCheckDate, string ParameterDisbursementType, int ParameterBatchNo, DataSet ParameterEDIDataSet,string ParameterProcessId)
		{
			DataSet l_dataset_EDIProcessData = null;
			string l_string_EDIOutputFileId ="";
			try
			{
				l_dataset_EDIProcessData = YMCARET.YmcaDataAccessObject.EDIDAClass.GetEDIProcessData(ParameterDisbursementType);
				YMCARET.YmcaBusinessObject.EDI_USPayroll EDICheckProcess = new YMCARET.YmcaBusinessObject.EDI_USPayroll();

				//get batch no
				EDICheckProcess.EDIBatchNO =Convert.ToString(ParameterBatchNo);
				
				if (EDICheckProcess.PreparOutPutFileData(parameterEDIDatatable,l_dataset_EDIProcessData,parameterDateCheckDate,ParameterDisbursementType))
				{
					// Insert  the output files 
					YMCARET.YmcaDataAccessObject.EDIDAClass.InsertOutPutFileTypes(l_dataset_EDIProcessData);
					// Update AtsEDILogs to show report
					// Insert Recors in AtsEDIPayments 
					l_string_EDIOutputFileId= EDICheckProcess.EDIOutputFileID;
					YMCARET.YmcaDataAccessObject.EDIDAClass.UpdateEDIProcessData(ParameterEDIDataSet,ParameterDisbursementType,ParameterProcessId,ParameterBatchNo,l_string_EDIOutputFileId);
					

					l_datatable_FileList = EDICheckProcess.datatable_FileList;
					return true;
				}
				else
				{				
					return false;
				}
                    				
			}
			catch
			{
				throw;
			}
		}


	}



	public class EDI_USPayroll
	{

		// Payee data varaibles
		#region "Constants"
		// Constants fro OUTPUT.H file.
		const string 	C_BATCHCOUNT		=	"000001";
	//	const string	C_BATCHNUMBER		=	"0000001";
		const string	C_BLOCKINGFACTOR	=	"10";
		const string	C_COMPANYENTRYDESC	=	"ANNUITY_PY";
		const string	C_COMPANYENTRYDESC_SPDIVIDEND	=	"SpDividend";
		const string	C_COMPANYID			=	"1135562401";
		const string	C_COMPANYNAME		=	"YMCA Retirement Fund             ";
		const string	C_FILEFORMATCODE	=	"1";
		const string	C_FILEIDMODIFIER	=	"A";
		const string	C_IMMEDIATEDEST		=	" 071000152";
		const string	C_IMMEDIATEDESTNAME	=	"Northern Trust Company ";
		const string	C_IMMEDIATEORIGIN	=	"1135562401";
		const string	C_IMMEDORIGINNAME	=	"YMCA Retirement Fund   ";
		const string	C_ORIGINATINGDFIID	=	"07100015";
		const string	C_ORIGINATORSTATUSCODE	= "1";
		const string	C_PRIORITYCODE		=	"01";
		const string	C_RECORDSIZE		=	"094";
		const string	C_ROWCODE1			=	"1";
		const string	C_ROWCODE5			=	"5";
		const string	C_ROWCODE8			=	"8";
		const string	C_ROWCODE9			=	"9";
		const string	C_ROWCODEH			=	"H";
		const string	C_SERVICECLASSCODE	=	"200";
		const string	C_STANDARDECCODE	=	"PPD";
		const string	C_COMPANYNAMESHORT	=	"YMCA Retire Fund";
		const string	C_ADDENDAINDICATOR	=	"0";
		const string	C_ROWCODE6			=	"6";
		const string	C_ROWCODEA			=	"A";
		const string	C_ROWCODEB			=	"B";
		const string	C_ROWCODER			=	"R";
		const string	C_ROWCODET			=	"T";
		const string	C_TRACENUMBER		=	"000000000000000";
		const string	C_TRANSACTIONCODE	=	"SA";
		const string	C_YMCABANKACCOUNT	=	"0030362081";
		const string	C_ROWCODE7_ADDENDA	=	"7";
		const string	C_ADDENDATYPE_ADDENDA =	"5";
		const string	C_DEDTEXT			=	"Ded       ";
		//const string	C_EXPDIVTEXT		=	"Experience Dividend - Taxable      ";
		const string	C_EXPDIVTEXT		=	"Special Dividend:                  ";
		const string	C_GROSSTEXT			=	"Gross     ";
		const string	C_NETTEXT			=	"Net       ";
		const string	C_NONTAXTEXT		=	"Non-Taxable     ";
		const string	C_REGALLOWTAXTEXT	=	"Regular Allowance: Taxable         ";
		const string	C_ROWCODEE			=	"E";
		const string	C_YTDDEDTEXT		=	"YTD Ded   ";
		const string	C_YTDGROSSTEXT		=	"YTD Gross ";
		const string	C_YTDNETTEXT		=	"YTD Net   ";
		const string	C_ROWCODED			=	"D";
		const string	C_ROWCODEC			=	"C";
		const string	C_ROWCODEF			=	"F";
		const string	C_ROWCODEG			=	"G";
		const string    C_POSPAY_EFT_LINE1	=	"$$ADD ID=YMCRE1B BID='9902827 A";
		const string    C_POSPAY_LINE1_SUFFIX =	"RPI'";
		const string    C_EFT_LINE1_SUFFIX	=	"CHI'";

		const string EDI_END_OF_LINE = "[";
		const string EDI_FIELD_DELIMITER = "^";
		const string EDI_PAYOR_BANK_ABA_NUMBER = "071923828";
		const string EDI_PAYOR_BANK_ACCOUNT_NUMBER = "30362081";
		//by Aparna 05/07/07
		//const string EDI_CONFIGURATION_KEY_MODE = "EDI_MODE";
		const string EDI_CONFIGURATION_CATEGORY_CODE = "EDIISA";

		// For the ISA Line
		const string EDI_INTERCHANGE_CONTROL_NUMBER_FORMAT = "000000000";
		// From the First GS Line
		const string EDI_APPLICATION_SENDER_CODE = "901234572000"; 
		const string EDI_APPLICATION_RECEIVER_CODE = "908887732000";
		// For the First ST/SE Line
		const string EDI_ST_LINE_TRANSACTION_NUMBER_FORMAT = "0000";
		// For the Second ST/SE Line
		const string EDI_ST_LINE_TRANSACTION_NUMBER_LONG_FORMAT = "000000000";
		// For the BPR Line
		const string EDI_NETPAY_CURRENCY_FORMAT = "0.00";
		const string EDI_CHECK_DATE_FORMAT = "yyyyMMdd";
		// For the RMR Lines
		const string EDI_CURRENCY_FORMAT = "0.00";

		// Constants for identifying the check detail file.
		const int CSDetail  = 1;
		const int PPDetail  = 2;
		const int EFTDetail = 3;

		#endregion "Constants"

		string 	C_BATCHNUMBER		=	"0000001";

		public string  EDIBatchNO		
		{
			get
			{
				return C_BATCHNUMBER;
			}
			set
			{
				C_BATCHNUMBER = value;
				C_BATCHNUMBER = C_BATCHNUMBER.Trim().PadLeft(7,'0');
			}

		}

		string string_EDIOutputFileID="";
		public string EDIOutputFileID
		{
			get
			{
				return string_EDIOutputFileID;
			}
			set
			{
				string_EDIOutputFileID=value;
			}
		}

		#region "Members"
		string String_PersID,
			String_Address1,
			String_Address2,
			String_Address3, 
			String_Address4,
			String_Address5,  
			String_CompanyData,
			String_Description, 
			String_DisbursementID,
			String_DisbursementNumber,
			String_FilingStatusExemptions,
			String_FundID,
			String_IndividualID,
			String_Name22,
			String_Name60,  
			//String_EftTypeCode,
			String_DisbursementType,
			String_UsCanadaBankCode,
			String_PaymentMethodCode;


		//For use with EDI generation
		string String_City, String_State, String_ZIP, String_Country, 
			String_DBAddress2, String_DBAddress3,
			String_DBFirstName, String_DBMiddleName, String_DBLastName			;
		DateTime DateTime_currentDateTime;
		TruncateFormatter myFormatter = new TruncateFormatter();
		//DataSet DataSet_ExclusionList = null; 
		bool bool_ExcludeThisPersonFromEDI = false;

		// PaymentData_Addenda, EntryDetailSeqNum_Addenda, AddendaSeqNum_Addenda,
		DateTime CheckDate,DisbursementDate,dateTimePayrollMonth;

		double double_NetPayMonthTotal;
		//long RegisterCount;
		//int ExpDivMonth, ExpDivYTD; 

		// PayeePayroll Variables
		double double_GrossPayMonthNontaxable,
			double_GrossPayMonthTaxable,
			double_GrossPayMonthTotal, 
			double_GrossPayYTDNontaxable, 
			double_GrossPayYTDTaxable, 
			double_GrossPayYTDTotal, 
			double_NetPayYTDTotal, 
			double_WithholdingMonthTotal, 
			double_WithholdingYTDTotal, 
			double_WithholdingMonthAddl,
			double_GrossPayDividentMonthTotal, 
			double_GrossPayDividentYTDTaxable;


				
		//string 	String_OutputFileCSCanadianGuid,String_OutputFileCSUSGuid,String_OutputFilePosPayGuid,String_OutputFileEFT_NorTrustGuid;
				
		//PayeePayrollWithholding varables
	        		
		double double_WithholdingMonthDetail,double_WithholdingYTDDetail;
		string String_WithholdingDetailDesc; 

		string String_OutputFileType="";

		double double_Exemptions;
		//Int32 OutputFileFormatCurrency,	OutputFilesPPDetailSum,lcOutputCursor,;

		bool bool_OutputFilePayRoll = true;
		bool bool_OutputFileEDI_US;

		//OutputFileOutputFileRefund,OutputFileRefund,
			    
	//	StreamWriter StreamOutputFileCSCanadian,StreamOutputFileCSUS,StreamOutputFilePosPay,StreamOutputFileEFT_NorTrust;
		StreamWriter StreamOutputFileEDI_US; //NP added to support EDI_US
	//	StreamWriter StreamOutputFileCSCanadianBack,StreamOutputFileCSUSBack,StreamOutputFilePosPayBack,StreamOutputFileEFT_NorTrustBack;
		StreamWriter StreamOutputFileEDI_USBack; //NP added to support EDI_US
		
			
		private DataTable l_datatable_FileList;

		//by Aparna 18/06/2007
		//private  DataSet l_dataset_EDIProcessData;


		#region "EDI counters"
		// Variables to keep track of Transaction numbers and other stuff related to EDI
		//private double double_checkTotal = 0;		//This variable is no longer used to keep running totals	// Net amount of credits and debits for all the detail blocks. This figure is sent out in the summary block
		private int i_numberOfFunctionalBlocks = 0;	// Number of functional detail blocks. These equal to 2 in our case. One is the Details block and the other is the summary block.
		private int i_numberOfDetailBlocks = 0;		// Total number of detail blocks. There is one detail block per record that is outputted. This figure is used in the summary block.
		private int i_numberOfSegmentsInDetailBlock = 0; // Number of segments in a detail block. This is needed in the trailing SE of each transaction.
		private double double_checkTotal = 0;		// Net of all the payments made in this EDI document
		//	We are not keeping running totals for these variables. Instead they are 
		//	being printed directly from their base variables
		//		private double double_currentWithholdingTotal = 0; // Net amount of withholdings for all people
		//		private double double_ytdWithholdingTotal = 0; // YTD amount of withholdings for all people
		//		private double double_currentGrossAllowance = 0; // Current Gross allowance for all people
		//		private double double_ytdGrossAllowance = 0;	// YTD amount of Gross allowance for all people
		//		private double double_currentNetPaymentAmount = 0; // Current amount of Net payment for all people
		//		private double double_ytdNetPaymentAmount = 0;	// YTD amount of Net payment for all people
		#endregion "EDI counters"

		#endregion "Members"

		// Here we initialize variables that need to be reset for each participant 
		// or payroll entry that is retrieved from the database.
		private void initializeEDIVariables() 
		{
			//NP - Added code to initialize the values used in EDI generation
			i_numberOfSegmentsInDetailBlock = 0;
			this.String_City = this.String_State = this.String_ZIP = this.String_Country = string.Empty;
			this.String_DBAddress2 = this.String_DBAddress3 = string.Empty;
			this.String_DBFirstName = this.String_DBMiddleName = this.String_DBLastName = string.Empty;
		}

		public EDI_USPayroll()
		{
			//
			// TODO: Add constructor logic here
			//
//			this.String_OutputFileCSCanadianGuid = "";
//			this.String_OutputFileCSUSGuid = "";
//			this.String_OutputFilePosPayGuid = "";
//			this.String_OutputFileEFT_NorTrustGuid = "";
			
			this.l_datatable_FileList = new DataTable();
			DataColumn SourceFolder = new DataColumn ("SourceFolder",typeof(String));
			DataColumn SourceFile = new DataColumn ("SourceFile", typeof(String));
			DataColumn DestFolder = new DataColumn ("DestFolder", typeof(String));
			DataColumn DestFile = new DataColumn ("DestFile", typeof(String));

			this.l_datatable_FileList.Columns.Add(SourceFolder);
			this.l_datatable_FileList.Columns.Add(SourceFile);
			this.l_datatable_FileList.Columns.Add(DestFolder);
			this.l_datatable_FileList.Columns.Add(DestFile);
			//by Aparna 18/06/2007
			//this.l_dataset_EDIProcessData = new DataSet();

		}


	





		private void ProduceOutputFilesCreateFooters()
		{
			//string lcOutputPPBatchDate,lcCompanyDate;
			
			//string l_String_temp1;
			string l_String_Output;
			NumberFormatInfo nfi = new CultureInfo( "en-US", false ).NumberFormat;
			nfi.CurrencyGroupSeparator ="";
			nfi.CurrencySymbol=""; 
			nfi.CurrencyDecimalDigits = 2;

			try
			{
			
				// Gets a NumberFormatInfo associated with the en-US culture.
				l_String_Output ="";
		
				if (this.bool_OutputFileEDI_US)
				{
					l_String_Output = this.writeGEForDetailFunctionalGroup();
					//Write to Output
					WriteOutputFilesDataLineIntoStreamEDI(l_String_Output);
					l_String_Output = this.writeGSForSummaryFunctionalGroup(this.DateTime_currentDateTime);
					//Write to Output
					WriteOutputFilesDataLineIntoStreamEDI(l_String_Output);
					l_String_Output = this.writeSTLineForSummary(1);
					//Write to Output
					WriteOutputFilesDataLineIntoStreamEDI(l_String_Output);
					//ASK_C: What is the reference information here
					l_String_Output = this.writeBGNLineForSummary("0012345", this.DateTime_currentDateTime);
					//Write to Output
					WriteOutputFilesDataLineIntoStreamEDI(l_String_Output);
					l_String_Output = this.writeN9LineForSummary(C_BATCHNUMBER);
					//Write to Output
					WriteOutputFilesDataLineIntoStreamEDI(l_String_Output);
					//Removing to comply with the new specification for AMT
					l_String_Output = this.writeAMTLineForSummary(this.double_checkTotal);
					//					l_String_Output = this.writeAMTLineForSummary(this.double_currentNetPaymentAmount, this.double_ytdNetPaymentAmount,
					//																	this.double_currentGrossAllowance, this.double_ytdGrossAllowance,
					//																	this.double_currentWithholdingTotal, this.double_ytdWithholdingTotal);
					//Write to Output
					WriteOutputFilesDataLineIntoStreamEDI(l_String_Output);
					l_String_Output = this.writeQTYLineForSummary(this.i_numberOfDetailBlocks);
					//Write to Output
					WriteOutputFilesDataLineIntoStreamEDI(l_String_Output);
					l_String_Output = this.writeSELineForSummary(this.i_numberOfSegmentsInDetailBlock, 1);
					//Write to Output
					WriteOutputFilesDataLineIntoStreamEDI(l_String_Output);
					l_String_Output = this.writeGELineForSummary();
					//Write to Output
					WriteOutputFilesDataLineIntoStreamEDI(l_String_Output);
					l_String_Output = this.writeIEALineForSummary(this.i_numberOfFunctionalBlocks, 1); //1 is the interchange control number. Since it is the first and only interchange in the document
					//Write to Output
					WriteOutputFilesDataLineIntoStreamEDI(l_String_Output);
					
				}

			}
			catch
			{
				throw;
			}

							
		}

		private void ProduceOutputFilesCreateHeader()
		{
			string l_String_Output;
			
			try
			{		
				if (this.bool_OutputFileEDI_US) 
				{
					// We have to print ISA and GS Lines in the header
					l_String_Output = writeISALine(this.DateTime_currentDateTime, 1, "053439083test  ") ; // 1 = Interchange control number. This is first and only one
					//Write to output stream
					WriteOutputFilesDataLineIntoStreamEDI(l_String_Output);
					l_String_Output = writeGSLineForDetailFunctionalGroup(this.DateTime_currentDateTime);
					//Write to output stream
					WriteOutputFilesDataLineIntoStreamEDI(l_String_Output);
				}

			}
			catch
			{
				throw;
			}
	            
		}

		#region EDI Printing helpers
		//Make sure that any data that comes in as a parameter fits the field size 
		//specifications of the corresponding EDI parameter

		//		Write the ISA line for the EDI document
		private string writeISALine(DateTime dt, int interchangeControlNumber, string interchangeReceiverId) 
		{
			//by Aparna 05/07/07
			DataSet ds;  	
			string stringISA07 ,stringISA08,stringISA15;
			stringISA07 = string.Empty;
			stringISA08 = string.Empty;
			stringISA15 = string.Empty;

			try
			{
				ds  = YMCARET.YmcaDataAccessObject.EDIDAClass.getEDIModeFromDatabase();			

				DataRow[] dr = ds.Tables[0].Select("[Key]=" + "'EDI_ISA_15'");
				if (dr.Length == 0 || dr[0].IsNull("Value") ) 
				{
					throw new Exception("The value for the key EDI_ISA_15 needs to be defined in the Configuration table");
				}
				switch (dr[0]["Value"].ToString().ToUpper()) 
				{
					case "TEST": 
						stringISA15=  "T";
						break;
					case "PRODUCTION": 
						stringISA15 = "P";
						break;
					default: 
						throw new Exception("The value for the key EDI_ISA_15 is not a valid value.");
						
				}

				dr = ds.Tables[0].Select("[Key]=" + "'EDI_ISA_07'");
				if (dr.Length == 0 || dr[0].IsNull("Value") ) 
				{
					throw new Exception("The value for the key EDI_ISA_07 needs to be defined in the Configuration table");
				}
				else
				{
					stringISA07 = dr[0]["Value"].ToString();
				}

				dr = ds.Tables[0].Select("[Key]=" + "'EDI_ISA_08'");
				if (dr.Length == 0 || dr[0].IsNull("Value") ) 
				{
					throw new Exception("The value for the key EDI_ISA_08 needs to be defined in the Configuration table");
				}
				else
				{
					stringISA08 = dr[0]["Value"].ToString();
				}
				//by Aparna 05/07/07

			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			sb.Append("ISA"); sb.Append(EDI_FIELD_DELIMITER); // Control Type ISA - Length 3
			sb.Append("00"); sb.Append(EDI_FIELD_DELIMITER);  // Authorization Information Qualifier - Length 2
			sb.Append("          ".PadRight(10, ' ')); sb.Append(EDI_FIELD_DELIMITER); // Authorization Information - Length 10
			sb.Append("00"); sb.Append(EDI_FIELD_DELIMITER);  // Security Information Qualifier - Length 2
			sb.Append("          ".PadRight(10, ' ')); sb.Append(EDI_FIELD_DELIMITER); // Security Information Qualifier - Length 10
			sb.Append("01"); sb.Append(EDI_FIELD_DELIMITER);  // Interchange ID Qualifier - Length 2
			sb.Append("YMCA RETIREMENT".PadRight(15, ' ')); sb.Append(EDI_FIELD_DELIMITER); // Interchange Sender ID - Length 15
			//Aparna 05/07/07
			//sb.Append("16"); sb.Append(EDI_FIELD_DELIMITER);  // Interchange ID Qualifier - Length 2
			sb.Append(stringISA07.Trim()); sb.Append(EDI_FIELD_DELIMITER);  // Interchange ID Qualifier - Length 2
			//sb.Append("053439083test  ".PadRight(15, ' ')); sb.Append(EDI_FIELD_DELIMITER); // Interchange Receiver ID - Length 15
			sb.Append(stringISA08.Trim().PadRight(15, ' ')); sb.Append(EDI_FIELD_DELIMITER); // Interchange Receiver ID - Length 15
			//Aparna 05/07/07
			sb.Append(dt.ToString("yyMMdd")); sb.Append(EDI_FIELD_DELIMITER);// Interchange Date - Format YYMMDD - Length 6
			sb.Append(dt.ToString("HHmm")); sb.Append(EDI_FIELD_DELIMITER);  // Interchange Time - Format HHMM - Length 4
			sb.Append("U");	sb.Append(EDI_FIELD_DELIMITER);     // Interchange Control Standards Identifier - Length 1
			sb.Append("00200"); sb.Append(EDI_FIELD_DELIMITER); // Interchange Control Version Number - Length 5
			sb.Append(interchangeControlNumber.ToString(EDI_INTERCHANGE_CONTROL_NUMBER_FORMAT));	sb.Append(EDI_FIELD_DELIMITER); // Interchange Control Number - Length 9
			sb.Append("0");	sb.Append(EDI_FIELD_DELIMITER);		// Acknowledgement requested - Length 1
			//NP: 2007.04.26 - This value should come from the atsMetaConfiguration table with a key value of 'EDI_MODE'
			//aparna 18/06/07
			//sb.Append(getEDIModeFromDatabase());	sb.Append(EDI_FIELD_DELIMITER);		// Usage indicator - Length 1 - T or P indicating whether to be used in Test mode or Production mode
			//by Aparna 05/07/07
			//sb.Append(YMCARET.YmcaDataAccessObject.EDIDAClass.getEDIModeFromDatabase());	sb.Append(EDI_FIELD_DELIMITER);		// Usage indicator - Length 1 - T or P indicating whether to be used in Test mode or Production mode
			sb.Append(stringISA15.Trim());	sb.Append(EDI_FIELD_DELIMITER);		// Usage indicator - Length 1 - T or P indicating whether to be used in Test mode or Production mode
			sb.Append("~");	sb.Append(EDI_END_OF_LINE);			// Component Element separator - Length 1
			return sb.ToString();
			//by Aparna 05/07/07
			}
			catch
			{ 
				throw;
				
			}
		}

		//		Write the GS line for the Details Functional Group
		private string writeGSLineForDetailFunctionalGroup(DateTime dt) 
		{ 
			this.i_numberOfFunctionalBlocks++;	// Increment the count for number of Functional Blocks
			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			sb.Append("GS"); sb.Append(EDI_FIELD_DELIMITER); 
			sb.Append("RA"); sb.Append(EDI_FIELD_DELIMITER); 
			sb.Append(EDI_APPLICATION_SENDER_CODE); sb.Append(EDI_FIELD_DELIMITER); 
			sb.Append(EDI_APPLICATION_RECEIVER_CODE); sb.Append(EDI_FIELD_DELIMITER); 
			sb.Append(dt.ToString("yyyyMMdd")); sb.Append(EDI_FIELD_DELIMITER);
			sb.Append(dt.ToString("HHmm")); sb.Append(EDI_FIELD_DELIMITER); 
			sb.Append("1"); sb.Append(EDI_FIELD_DELIMITER); 
			sb.Append("T"); sb.Append(EDI_FIELD_DELIMITER); 
			sb.Append("004010"); sb.Append(EDI_END_OF_LINE);
			return sb.ToString();
		}
						
						
		string _cache_STLine = string.Empty;
		private string writeSTLine(int transactionNumber) 
		{ 
			if (_cache_STLine == string.Empty) 
			{
				_cache_STLine += "ST" + EDI_FIELD_DELIMITER;
				_cache_STLine += "820" + EDI_FIELD_DELIMITER;
				_cache_STLine += "{0}" + EDI_END_OF_LINE;
			}
			string data = string.Empty ;
			this.i_numberOfSegmentsInDetailBlock = 1;	// Set number of Lines printed in current detail block to 1 as this is the first line in the current detail block
			data = string.Format(_cache_STLine, transactionNumber.ToString(EDI_ST_LINE_TRANSACTION_NUMBER_FORMAT));
			return data;
		}
		//		Write the BPR record
		private string writeBPRLine(double netPayMonthTotal, DateTime checkDate ) 
		{
			this.i_numberOfSegmentsInDetailBlock++;	// Increment number of Lines printed in current detail block printed
			this.double_checkTotal += double.Parse(netPayMonthTotal.ToString("0.00")); 	//Update the running count for the checks issued
			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			sb.Append("BPR"); sb.Append(EDI_FIELD_DELIMITER);
			sb.Append("D"); sb.Append(EDI_FIELD_DELIMITER); 
			sb.Append(netPayMonthTotal.ToString(EDI_NETPAY_CURRENCY_FORMAT)); sb.Append(EDI_FIELD_DELIMITER); 
			sb.Append("C"); sb.Append(EDI_FIELD_DELIMITER); 
			sb.Append("YMC"); sb.Append(EDI_FIELD_DELIMITER); 
			sb.Append("PBC"); sb.Append(EDI_FIELD_DELIMITER); 
			sb.Append("01"); sb.Append(EDI_FIELD_DELIMITER); 
			sb.Append(EDI_PAYOR_BANK_ABA_NUMBER); sb.Append(EDI_FIELD_DELIMITER); 
			sb.Append("DA"); sb.Append(EDI_FIELD_DELIMITER); 
			sb.Append(EDI_PAYOR_BANK_ACCOUNT_NUMBER); sb.Append(EDI_FIELD_DELIMITER); 
			sb.Append(""); sb.Append(EDI_FIELD_DELIMITER); 
			sb.Append(""); sb.Append(EDI_FIELD_DELIMITER); 
			sb.Append(""); sb.Append(EDI_FIELD_DELIMITER); 
			sb.Append(""); sb.Append(EDI_FIELD_DELIMITER); 
			sb.Append(""); sb.Append(EDI_FIELD_DELIMITER); 
			sb.Append(""); sb.Append(EDI_FIELD_DELIMITER); 
			sb.Append(checkDate.ToString(EDI_CHECK_DATE_FORMAT)); sb.Append(EDI_END_OF_LINE); // Format YYYYMMDD
			return sb.ToString();
		}
		//		Write the TRN record
		string _cache_TRNLine = string.Empty;
		private string writeTRNLine(string chequeNumber) 
		{ 
			if (_cache_TRNLine == string.Empty) 
			{
				_cache_TRNLine += "TRN" + EDI_FIELD_DELIMITER;
				_cache_TRNLine += "1" + EDI_FIELD_DELIMITER;
				_cache_TRNLine += "{0:T30}" + EDI_END_OF_LINE;
			}
			this.i_numberOfSegmentsInDetailBlock++;	// Increment number of Lines printed in current detail block printed
			string data = string.Format(this.myFormatter, _cache_TRNLine, chequeNumber.Trim());
			return data;
		}
		//		Write the REF record for Batch Number
		string _cache_REFBatchNumberLine = string.Empty;
		private string writeREFBatchNumberLine(string batchNumber) 
		{ 
			if (_cache_REFBatchNumberLine==string.Empty) 
			{
				_cache_REFBatchNumberLine += "REF" + EDI_FIELD_DELIMITER;
				_cache_REFBatchNumberLine += "BT" + EDI_FIELD_DELIMITER;
				_cache_REFBatchNumberLine += "{0:T30}" + EDI_END_OF_LINE;
			}
			this.i_numberOfSegmentsInDetailBlock++;	// Increment number of Lines printed in current detail block printed
			string data = string.Format(this.myFormatter, _cache_REFBatchNumberLine, batchNumber );
			return data;
		}
		//		Write the REF record for Mailing information
		string _cache_REFMailingInformationLines = string.Empty;
		//NP: Updated to include mail handling information FCM
		private string writeREFMailingInformationLines() 
		{ 
			if (_cache_REFMailingInformationLines == string.Empty) 
			{
				_cache_REFMailingInformationLines += "REF" + EDI_FIELD_DELIMITER;
				_cache_REFMailingInformationLines += "W9" + EDI_FIELD_DELIMITER;
				//NP: Adding the constant mail handling code FCM to the output
				_cache_REFMailingInformationLines += "FCM" + EDI_FIELD_DELIMITER;
				_cache_REFMailingInformationLines += "000" + EDI_END_OF_LINE;
			}
			this.i_numberOfSegmentsInDetailBlock++;	// Increment number of Lines printed in current detail block printed
			string data = string.Format(_cache_REFMailingInformationLines);
			return data;
		}
		//		Write the REF record for Fund Identification number
		string _cache_REFFundId = string.Empty;
		private string writeREFFundIdentificationInformationLine(string fundIdNo) 
		{ 
			this.i_numberOfSegmentsInDetailBlock++;	// Increment number of Lines printed in current detail block printed
			if (_cache_REFFundId == string.Empty) 
			{
				_cache_REFFundId  += "REF" + EDI_FIELD_DELIMITER;
				_cache_REFFundId += "FU" + EDI_FIELD_DELIMITER;
				_cache_REFFundId += "{0:T30}" + EDI_END_OF_LINE;
			}
			string data = string.Format(this.myFormatter, _cache_REFFundId, fundIdNo ) ;
			return data;
		}
		//		Write the REF record for Accounting status
		string _cache_REFAccountStatus = string.Empty;
		private string writeREFAccountingStatusLine(string filingStatusExemptions) 
		{ 
			if (_cache_REFAccountStatus == string.Empty) 
			{
				_cache_REFAccountStatus += "REF" + EDI_FIELD_DELIMITER;
				_cache_REFAccountStatus += "ACC" + EDI_FIELD_DELIMITER;
				_cache_REFAccountStatus += "{0:T30}" + EDI_END_OF_LINE;
			}
			this.i_numberOfSegmentsInDetailBlock++;	// Increment number of Lines printed in current detail block printed
			string data = string.Format(this.myFormatter, _cache_REFAccountStatus , filingStatusExemptions );
			return data;
		}
		//		Write the REF record for Taxing Authority Identification number
		string _cache_REFTaxingAuthority = string.Empty;
		private string writeREFTaxingAuthorityIdentificationNumberLine(double amount) 
		{ 
			if (_cache_REFTaxingAuthority == string.Empty) 
			{
				_cache_REFTaxingAuthority += "REF" + EDI_FIELD_DELIMITER;
				_cache_REFTaxingAuthority += "61" + EDI_FIELD_DELIMITER;
				_cache_REFTaxingAuthority += "${0}" + EDI_END_OF_LINE;
			}
			this.i_numberOfSegmentsInDetailBlock++;	// Increment number of Lines printed in current detail block printed
			string data = string.Format(_cache_REFTaxingAuthority, amount.ToString(EDI_CURRENCY_FORMAT));
			return data;
		}
		//		Write the N1-N4 Entries record for the Payor
		string _cache_PayorInformation = string.Empty;
		private string writeN1toN4PayorInformationLines() 
		{ 
			if (_cache_PayorInformation == string.Empty) 
			{
				System.Text.StringBuilder sb = new System.Text.StringBuilder();
				sb.Append("N1"); sb.Append(EDI_FIELD_DELIMITER); 
				sb.Append("PR"); sb.Append(EDI_FIELD_DELIMITER); 
				sb.Append("YMCA RETIREMENT FUND"); sb.Append(EDI_END_OF_LINE);
				sb.Append("\r\n");
				sb.Append("N3"); sb.Append(EDI_FIELD_DELIMITER); 
				sb.Append("140 Broadway"); sb.Append(EDI_END_OF_LINE);
				sb.Append("\r\n");
				sb.Append("N4"); sb.Append(EDI_FIELD_DELIMITER); 
				sb.Append("NEW YORK"); sb.Append(EDI_FIELD_DELIMITER); 
				sb.Append("NY"); sb.Append(EDI_FIELD_DELIMITER); 
				sb.Append("100051197"); sb.Append(EDI_END_OF_LINE);
				_cache_PayorInformation = sb.ToString();
			}
			this.i_numberOfSegmentsInDetailBlock += 3;	// Increment number of Lines printed in current detail block printed
			return string.Format(_cache_PayorInformation);
		}
		//		Write the N1-N4 Entries record for the Payee
		private string writeN1toN4PayeeInformationLines(string firstName, string middleName, string lastName, string addressLine1, string addressLine2, string addressLine3, string city, string state, string zip, string country) 
		{ 
			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			sb.Append("N1"); sb.Append(EDI_FIELD_DELIMITER); 
			sb.Append("PE"); sb.Append(EDI_FIELD_DELIMITER);

			string fullName = firstName.Trim() + ((firstName!=string.Empty)?" ":string.Empty) 
				+ middleName.Trim() + ((middleName!=string.Empty)?" ":string.Empty  )
				+ lastName.Trim();

			// If the fullname length is greater than 45 (usually this person 
			// is a guardian) then we need to make sure that we truncate the 
			// data and put it on a separate line if it exceeds 45 characters.
			if (fullName.Length > 45) 
			{
				string format = "{0:T45}" + EDI_END_OF_LINE + "\r\n" + 
					"N2" + EDI_FIELD_DELIMITER + "{1:T45}"; // The second section is optional -  + EDI_FIELD_DELIMITER;
				string tmp = string.Empty;
				// tmp will contain Guardian Name + the string "Guardian Of"
				tmp += firstName; if (firstName != string.Empty) tmp +=" ";
				tmp += middleName;
				// lastname contains the name of the entity who requires a guardian
				sb.Append(String.Format(this.myFormatter, format, tmp, lastName));
				this.i_numberOfSegmentsInDetailBlock++; // Increment number of Lines printed in current detail block printed
			} 
			else 
			{	// Truncate the full name of the person if needed and print it out to sb
				sb.Append(String.Format(this.myFormatter, "{0:T45}", fullName));
			}
 
			sb.Append(EDI_END_OF_LINE); sb.Append("\r\n");
			this.i_numberOfSegmentsInDetailBlock++;	// Increment number of Lines printed in current detail block printed
			if (addressLine1.Trim() != "") 
			{
				sb.Append("N3"); sb.Append(EDI_FIELD_DELIMITER); 
				sb.Append(String.Format(this.myFormatter, "{0:T45}", addressLine1.Trim())); 
				sb.Append(EDI_FIELD_DELIMITER);
				if (addressLine2.Trim() != "") sb.Append(String.Format(this.myFormatter, "{0:T45}", addressLine2.Trim()));
				sb.Append(EDI_END_OF_LINE);
				sb.Append("\r\n");
				this.i_numberOfSegmentsInDetailBlock++;	// Increment number of Lines printed in current detail block printed
			}
			if (addressLine3.Trim() != "") 
			{
				sb.Append("N3"); sb.Append(EDI_FIELD_DELIMITER); 
				sb.Append(String.Format(this.myFormatter, "{0:T45}", addressLine3.Trim())); 
				sb.Append(EDI_FIELD_DELIMITER); 
				sb.Append(EDI_END_OF_LINE);
				sb.Append("\r\n");
				this.i_numberOfSegmentsInDetailBlock++;	// Increment number of Lines printed in current detail block printed
			}
			sb.Append("N4"); sb.Append(EDI_FIELD_DELIMITER); 
			sb.Append(String.Format(this.myFormatter, "{0:T30}", city.Trim())); 
			sb.Append(EDI_FIELD_DELIMITER); 
			sb.Append(String.Format(this.myFormatter, "{0:T2}", state.Trim())); 
			sb.Append(EDI_FIELD_DELIMITER); 
			sb.Append(String.Format(this.myFormatter, "{0:T15}", zip.Trim())); 
			if (!(country == null || country.Trim().ToUpper() == "USA" || country.Trim().ToUpper() == "US")) 
			{
				//	Country code is not required if printing for US cheques and the country is assumed to be US
				//	In other cases the country code should be printed as per Mark's request received orally through Ragesh on 2007.05.09
				sb.Append(EDI_FIELD_DELIMITER);
				sb.Append(String.Format(this.myFormatter, "{0:T3}", country.Trim())); 
			}
			sb.Append(EDI_END_OF_LINE);
			this.i_numberOfSegmentsInDetailBlock++;	// Increment number of Lines printed in current detail block printed
			return sb.ToString();
		}
		//		Write the ENT record
		string _cache_ENTLine = string.Empty;
		private string writeENTLine(string param) 
		{ 
			if (_cache_ENTLine==string.Empty) 
			{
				_cache_ENTLine += "ENT" + EDI_FIELD_DELIMITER;
				_cache_ENTLine += "{0:T6}" + EDI_END_OF_LINE;
			}
			this.i_numberOfSegmentsInDetailBlock++;	// Increment number of Lines printed in current detail block printed
			string data = string.Format(this.myFormatter, _cache_ENTLine, param);
			return data;
		}


		#region "Commented out to support the newer specification of EDI for RMR lines"
		//		//		Write the RMR record for Current Payments/Regular Allowance
		//		private string writeRMRCurrentPaymentLine(double netPayMonthTotal, double grossPayMonthTotal, 
		//			double withholdingMonthTotal, int billingCode, double grossPayMonthTaxable) 
		//		{ 
		//			this.i_numberOfSegmentsInDetailBlock++;	// Increment number of Lines printed in current detail block printed
		//			System.Text.StringBuilder sb = new System.Text.StringBuilder();
		//			sb.Append("RMR"); sb.Append(EDI_FIELD_DELIMITER); 
		//			sb.Append("AB"); sb.Append(EDI_FIELD_DELIMITER); 
		//			sb.Append("Regular Allowance: Taxable"); sb.Append(EDI_FIELD_DELIMITER); 
		//			sb.Append(""); sb.Append(EDI_FIELD_DELIMITER); 
		//			sb.Append(netPayMonthTotal.ToString(EDI_CURRENCY_FORMAT)); sb.Append(EDI_FIELD_DELIMITER); 
		//			sb.Append(grossPayMonthTotal.ToString(EDI_CURRENCY_FORMAT)); sb.Append(EDI_FIELD_DELIMITER); 
		//			sb.Append(withholdingMonthTotal.ToString(EDI_CURRENCY_FORMAT)); sb.Append(EDI_FIELD_DELIMITER); 
		//			sb.Append(billingCode.ToString("00")); sb.Append(EDI_FIELD_DELIMITER); 
		//			sb.Append(grossPayMonthTaxable.ToString(EDI_CURRENCY_FORMAT)); sb.Append(EDI_END_OF_LINE);
		//			return sb.ToString();
		//		}
		//		//		Write the RMR record for YTD Payments/YTD Allowance
		//		private string writeRMRYTDPaymentLine(double netPayYTDTotal, double grossPayYTDTotal, 
		//			double withholdingYTDTotal, int billingCode, double grossPayYTDTaxable) 
		//		{ 
		//			this.i_numberOfSegmentsInDetailBlock++;	// Increment number of Lines printed in current detail block printed
		//			System.Text.StringBuilder sb = new System.Text.StringBuilder();
		//			sb.Append("RMR"); sb.Append(EDI_FIELD_DELIMITER); 
		//			sb.Append("BT"); sb.Append(EDI_FIELD_DELIMITER); 
		//			sb.Append("Regular Allowance:Taxable YTD"); sb.Append(EDI_FIELD_DELIMITER); 
		//			sb.Append(""); sb.Append(EDI_FIELD_DELIMITER); 
		//			sb.Append(netPayYTDTotal.ToString(EDI_CURRENCY_FORMAT)); sb.Append(EDI_FIELD_DELIMITER); 
		//			sb.Append(grossPayYTDTotal.ToString(EDI_CURRENCY_FORMAT)); sb.Append(EDI_FIELD_DELIMITER); 
		//			sb.Append(withholdingYTDTotal.ToString(EDI_CURRENCY_FORMAT)); sb.Append(EDI_FIELD_DELIMITER); 
		//			sb.Append(billingCode.ToString("00")); sb.Append(EDI_FIELD_DELIMITER); 
		//			sb.Append(grossPayYTDTaxable.ToString(EDI_CURRENCY_FORMAT)); sb.Append(EDI_END_OF_LINE);
		//			return sb.ToString();
		//		}
		//		//		Write the NTE*ALL record for special notes/Non-taxable income entries
		//		private string writeNTELines(double grossPayMonthNonTaxable, double grossPayYTDNonTaxable ) 
		//		{ 
		//			// If both values are zero then no need to print this line item
		//			if (grossPayMonthNonTaxable == 0 && grossPayYTDNonTaxable ==0) return string.Empty ;
		//			this.i_numberOfSegmentsInDetailBlock++;	// Increment number of Lines printed in current detail block printed
		//			System.Text.StringBuilder sb = new System.Text.StringBuilder();
		//			sb.Append("NTE"); sb.Append(EDI_FIELD_DELIMITER);
		//			sb.Append("ALL"); sb.Append(EDI_FIELD_DELIMITER); 
		//			sb.Append("Non-Taxable                   ".PadRight(30, ' '));
		//			sb.Append((grossPayMonthNonTaxable * 100).ToString().PadLeft(10, '0')); 
		//			sb.Append((grossPayYTDNonTaxable * 100).ToString().PadLeft(10, '0')); // This field must not be larget than 10 characters
		//			sb.Append(EDI_END_OF_LINE);
		//			return sb.ToString();
		//		}
		#endregion

		//		Write the RMR record for Current Taxable Payment
		private string writeRMR_IV_CurrentTaxablePaymentLine(double grossPayMonthTaxable, double grossPayYTDTaxable) 
		{ 
			this.i_numberOfSegmentsInDetailBlock++;	// Increment number of Lines printed in current detail block printed
			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			sb.Append("RMR"); sb.Append(EDI_FIELD_DELIMITER); 
			sb.Append("IV"); sb.Append(EDI_FIELD_DELIMITER); 
			sb.Append("Taxable"); sb.Append(EDI_FIELD_DELIMITER); 
			sb.Append(""); sb.Append(EDI_FIELD_DELIMITER); 
			sb.Append(grossPayMonthTaxable.ToString(EDI_CURRENCY_FORMAT)); sb.Append(EDI_FIELD_DELIMITER); 
			sb.Append(grossPayYTDTaxable.ToString(EDI_CURRENCY_FORMAT)); sb.Append(EDI_END_OF_LINE);
			return sb.ToString();
		}
		//		Write the RMR record for Current Non-Taxable Payment
		private string writeRMR_IK_CurrentNonTaxablePaymentLine(double grossPayMonthNonTaxable, double grossPayYTDNonTaxable) 
		{ 
			// This line is not printed if both current and ytd amounts are 0. As per Mark's email dt. 2007.04.30
			if (grossPayMonthNonTaxable == 0 && grossPayYTDNonTaxable == 0) return string.Empty;
			this.i_numberOfSegmentsInDetailBlock++;	// Increment number of Lines printed in current detail block printed
			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			sb.Append("RMR"); sb.Append(EDI_FIELD_DELIMITER); 
			sb.Append("IK"); sb.Append(EDI_FIELD_DELIMITER); 
			sb.Append("Non-Taxable"); sb.Append(EDI_FIELD_DELIMITER); 
			sb.Append(""); sb.Append(EDI_FIELD_DELIMITER); 
			sb.Append(grossPayMonthNonTaxable.ToString(EDI_CURRENCY_FORMAT)); sb.Append(EDI_FIELD_DELIMITER); 
			sb.Append(grossPayYTDNonTaxable.ToString(EDI_CURRENCY_FORMAT)); sb.Append(EDI_END_OF_LINE);
			return sb.ToString();
		}
		//		Write the RMR record for Special Dividend Payment
		private string writeRMR_2G_SpecialDividendPaymentLine(double grossDividendMonth, double grossDividendYTD )
		{ 
			// This line is not printed if both current and ytd amounts are 0. As per Mark's email dt. 2007.04.30
			if (grossDividendMonth == 0 && grossDividendYTD == 0) return string.Empty;
			this.i_numberOfSegmentsInDetailBlock++;	// Increment number of Lines printed in current detail block printed
			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			sb.Append("RMR"); sb.Append(EDI_FIELD_DELIMITER); 
			sb.Append("2G"); sb.Append(EDI_FIELD_DELIMITER); 
			sb.Append("Not Used"); sb.Append(EDI_FIELD_DELIMITER); 
			sb.Append(""); sb.Append(EDI_FIELD_DELIMITER); 
			sb.Append(grossDividendMonth.ToString(EDI_CURRENCY_FORMAT)); sb.Append(EDI_FIELD_DELIMITER); 
			sb.Append(grossDividendYTD.ToString(EDI_CURRENCY_FORMAT)); sb.Append(EDI_END_OF_LINE);
			return sb.ToString();
		}


		#region "Commented out to support the newer specifications of EDI for REF lines"
		//		//		Write the NTE*ADD record for special notes/Special Dividends entry
		//		private string writeNTELinesSpecialDividend(double grossDividendMonth, double grossDividendYTD ) 
		//		{
		//			// If both values are zero then no need to print this line item
		//			if (grossDividendMonth == 0 && grossDividendYTD == 0) return string.Empty ;
		//			this.i_numberOfSegmentsInDetailBlock++;	// Increment number of Lines printed in current detail block printed
		//			System.Text.StringBuilder sb = new System.Text.StringBuilder();
		//			sb.Append("NTE"); sb.Append(EDI_FIELD_DELIMITER);
		//			sb.Append("ADD"); sb.Append(EDI_FIELD_DELIMITER); 
		//			sb.Append("Special Dividend".PadRight(30, ' '));
		//			sb.Append((grossDividendMonth * 100).ToString().PadLeft(10, '0')); 
		//			sb.Append((grossDividendYTD * 100).ToString().PadLeft(10, '0')); // This field must not be larget than 10 characters
		//			sb.Append(EDI_END_OF_LINE);
		//			return sb.ToString();
		//		}
		//
		
		//		//		Write the REF record for Deductions from current pay
		//		private string writeREFDeductionsLines(string description, double amount, double yTDAmount) 
		//		{ 
		//			// No need to filter this. This line needs to be printed especially for the Federal Withholding part
		//			// if (amount == 0 && yTDAmount == 0) return string.Empty ; // If amount and yTDAmount are both zero then no need to print this line item
		//			string format = "REF" + EDI_FIELD_DELIMITER + "CM" + EDI_FIELD_DELIMITER 
		//				+ EDI_FIELD_DELIMITER + "{0}{1}{2}" + EDI_END_OF_LINE;
		//			this.i_numberOfSegmentsInDetailBlock++;	// Increment number of Lines printed in current detail block printed
		//			return String.Format(format, description.PadRight(30, ' '), 
		//				((amount * 100)).ToString().PadLeft(10,'0'), 
		//				((yTDAmount * 100)).ToString().PadLeft(10, '0'));	// The last field must not exceed 10 characters. The money values are without the decimal point.
		//		}
		#endregion

		//		Write the DED record for Deductions from current pay
		private string writeDEDDeductionsLines(string description, double amount, double ytdAmount, DateTime d) 
		{ 
			// No need to filter this. This line needs to be printed especially for the Federal Withholding part
			// if (amount == 0 && yTDAmount == 0) return string.Empty ; // If amount and yTDAmount are both zero then no need to print this line item
			string format = "DED" + EDI_FIELD_DELIMITER + "CM" + EDI_FIELD_DELIMITER 
				+ "IGNORE" + EDI_FIELD_DELIMITER + "{0}" + EDI_FIELD_DELIMITER + "{1}"
				+ EDI_FIELD_DELIMITER + "{2}" + EDI_FIELD_DELIMITER + "N" 
				+ EDI_FIELD_DELIMITER + "{3:T60}" + EDI_END_OF_LINE;
			this.i_numberOfSegmentsInDetailBlock++;	// Increment number of Lines printed in current detail block printed
			return String.Format(this.myFormatter, format, d.ToString("yyyyMMdd"), (amount * 100).ToString(),
				(ytdAmount * 100).ToString().PadLeft(25, '0'), description);
		}
		//		Write the N9 record for each detail block
		string _cache_N9Line = string.Empty;
		private string writeN9Line() 
		{
			if (_cache_N9Line == string.Empty) 
			{
				_cache_N9Line += "N9" + EDI_FIELD_DELIMITER;
				_cache_N9Line += "ZZ" + EDI_FIELD_DELIMITER;
				_cache_N9Line += "NOT USED" + EDI_END_OF_LINE;
			}
			this.i_numberOfSegmentsInDetailBlock++;	// Increment number of Lines printed in current detail block printed
			string data = string.Format(_cache_N9Line);
			return data;
		}
		//		Write the AMT record for current payee
		private string writeAMTLine(double currentNetPayAmount, double ytdNetPayAmount, 
			double currentGrossPayAmount, double ytdGrossPayAmount,
			double currentWithholdingAmount, double ytdWithholdingAmount) 
		{ 
			System.Text.StringBuilder data = new System.Text.StringBuilder();
			this.i_numberOfSegmentsInDetailBlock++;	// Increment number of Lines printed in current detail block printed
			data.Append("AMT"); data.Append(EDI_FIELD_DELIMITER);
			data.Append("TA"); data.Append(EDI_FIELD_DELIMITER);
			data.Append(currentNetPayAmount.ToString(EDI_CURRENCY_FORMAT)); data.Append(EDI_END_OF_LINE);
			data.Append("\r\n");
			this.i_numberOfSegmentsInDetailBlock++;	// Increment number of Lines printed in current detail block printed
			data.Append("AMT"); data.Append(EDI_FIELD_DELIMITER);
			data.Append("TD"); data.Append(EDI_FIELD_DELIMITER);
			data.Append(currentWithholdingAmount.ToString(EDI_CURRENCY_FORMAT)); data.Append(EDI_END_OF_LINE);
			data.Append("\r\n");
			this.i_numberOfSegmentsInDetailBlock++;	// Increment number of Lines printed in current detail block printed
			data.Append("AMT"); data.Append(EDI_FIELD_DELIMITER);
			data.Append("TG"); data.Append(EDI_FIELD_DELIMITER);
			data.Append(currentGrossPayAmount.ToString(EDI_CURRENCY_FORMAT)); data.Append(EDI_END_OF_LINE);
			data.Append("\r\n");
			this.i_numberOfSegmentsInDetailBlock++;	// Increment number of Lines printed in current detail block printed
			data.Append("AMT"); data.Append(EDI_FIELD_DELIMITER);
			data.Append("YTA"); data.Append(EDI_FIELD_DELIMITER);
			data.Append(ytdNetPayAmount.ToString(EDI_CURRENCY_FORMAT)); data.Append(EDI_END_OF_LINE);
			data.Append("\r\n");
			this.i_numberOfSegmentsInDetailBlock++;	// Increment number of Lines printed in current detail block printed
			data.Append("AMT"); data.Append(EDI_FIELD_DELIMITER);
			data.Append("YTD"); data.Append(EDI_FIELD_DELIMITER);
			data.Append(ytdWithholdingAmount.ToString(EDI_CURRENCY_FORMAT)); data.Append(EDI_END_OF_LINE);
			data.Append("\r\n");
			this.i_numberOfSegmentsInDetailBlock++;	// Increment number of Lines printed in current detail block printed
			data.Append("AMT"); data.Append(EDI_FIELD_DELIMITER);
			data.Append("YTG"); data.Append(EDI_FIELD_DELIMITER);
			data.Append(ytdGrossPayAmount.ToString(EDI_CURRENCY_FORMAT)); data.Append(EDI_END_OF_LINE);
			return data.ToString();
		}
		//		Write the Trailing SE record
		string _cache_SELine = string.Empty;
		private string writeSELine(int lc_int_numberOfSegments, int transactionNumber) 
		{ 
			if (_cache_SELine == string.Empty) 
			{
				_cache_SELine  += "SE" + EDI_FIELD_DELIMITER;
				_cache_SELine  += "{0}" + EDI_FIELD_DELIMITER;
				_cache_SELine  += "{1:" + EDI_ST_LINE_TRANSACTION_NUMBER_FORMAT + "}";
				_cache_SELine  += EDI_END_OF_LINE;
			}
			this.i_numberOfSegmentsInDetailBlock++;	// Increment number of Lines printed in current detail block printed
			lc_int_numberOfSegments++; // Number of segments we are incrementing to include this one 
			string data = string.Format(_cache_SELine, lc_int_numberOfSegments, transactionNumber);
			this.i_numberOfSegmentsInDetailBlock = 0;	// Set number of Lines printed in current detail block to zero as we finished using the value
			return data;
		}
						
		//		Write the Trailing GE for the Details record
		private string writeGEForDetailFunctionalGroup() 
		{ 
			string data = string.Empty;
			data += "GE" + EDI_FIELD_DELIMITER;
			data += "104" + EDI_FIELD_DELIMITER; 
			data += "1" + EDI_END_OF_LINE;
			return data;
		}
		//		Write the opening GS record for summaries
		private string writeGSForSummaryFunctionalGroup(DateTime dt) 
		{ 
			this.i_numberOfFunctionalBlocks++;	// Increment the number of Functional Blocks being printed
			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			sb.Append("GS"); sb.Append(EDI_FIELD_DELIMITER); 
			sb.Append("CT"); sb.Append(EDI_FIELD_DELIMITER); 
			sb.Append(EDI_APPLICATION_SENDER_CODE); sb.Append(EDI_FIELD_DELIMITER); 
			sb.Append(EDI_APPLICATION_RECEIVER_CODE); sb.Append(EDI_FIELD_DELIMITER); 
			sb.Append(dt.ToString("yyyyMMdd")); sb.Append(EDI_FIELD_DELIMITER);
			sb.Append(dt.ToString("HHmm")); sb.Append(EDI_FIELD_DELIMITER); 
			sb.Append("1"); sb.Append(EDI_FIELD_DELIMITER); 
			sb.Append("T"); sb.Append(EDI_FIELD_DELIMITER); 
			sb.Append("004010"); sb.Append(EDI_END_OF_LINE);
			return sb.ToString();
		}
		//		Write the ST record for Summary
		private string writeSTLineForSummary(int transactionNumber) 
		{ 
			this.i_numberOfSegmentsInDetailBlock = 1;	// Set number of Lines printed in current detail block as this is the first line to be printed
			string data = string.Empty;
			data += "ST" + EDI_FIELD_DELIMITER;
			data += "831" + EDI_FIELD_DELIMITER;
			data += transactionNumber.ToString(EDI_ST_LINE_TRANSACTION_NUMBER_LONG_FORMAT);
			data += EDI_END_OF_LINE;
			return data;
		}
		//		Write the BGN record for the summaries
		private string writeBGNLineForSummary(string referenceInformation, DateTime dt) 
		{ 
			this.i_numberOfSegmentsInDetailBlock++;	// Increment number of Lines printed in current detail block printed
			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			sb.Append("BGN"); sb.Append(EDI_FIELD_DELIMITER);
			sb.Append("27"); sb.Append(EDI_FIELD_DELIMITER); 
			sb.Append(referenceInformation); sb.Append(EDI_FIELD_DELIMITER);
			sb.Append(dt.ToString("yyyyMMdd")); sb.Append(EDI_FIELD_DELIMITER); // Format yyyyMMdd
			sb.Append(dt.ToString("HHmmssff")); sb.Append(EDI_END_OF_LINE);	// Format HHmmssff
			return sb.ToString();
		}
		//		Write the N9 record to specify the Batch number
		private string writeN9LineForSummary(string batchNumber) 
		{ 
			this.i_numberOfSegmentsInDetailBlock++;	// Increment number of Lines printed in current detail block printed
			string data = string.Empty;
			data += "N9" + EDI_FIELD_DELIMITER;
			data += "BT" + EDI_FIELD_DELIMITER;
			data += String.Format(this.myFormatter, "{0:T30}", batchNumber) + EDI_END_OF_LINE;
			return data;
		}

		//		Write the AMT record for net of all Credit/Debit operations
		private string writeAMTLineForSummary(double checkTotal) 
		{ 
			this.i_numberOfSegmentsInDetailBlock++;	// Increment number of Lines printed in current detail block printed
			string data = string.Empty;
			data += "AMT" + EDI_FIELD_DELIMITER;
			data += "OP" + EDI_FIELD_DELIMITER;
			data += checkTotal.ToString(EDI_CURRENCY_FORMAT)  + EDI_END_OF_LINE;
			return data;
		}

		//		Write the QTY record to indicate the number of records written
		private string writeQTYLineForSummary(int numberOfDetailBlocks) 
		{ 
			this.i_numberOfSegmentsInDetailBlock++;	// Increment number of Lines printed in current detail block printed
			string data = string.Empty;
			data += "QTY" + EDI_FIELD_DELIMITER;
			data += "46" + EDI_FIELD_DELIMITER;
			data += numberOfDetailBlocks + EDI_END_OF_LINE;
			return data;
		}
		//		Write the trailing SE record to mark close of summaries
		private string writeSELineForSummary(int lc_int_numberOfSegments, int transactionNumber) 
		{ 
			string data = string.Empty;
			data += "SE" + EDI_FIELD_DELIMITER;
			this.i_numberOfSegmentsInDetailBlock++;	// Increment number of Lines printed in current detail block printed
			lc_int_numberOfSegments++;	// Increment count to include the lines for current segment
			data += lc_int_numberOfSegments.ToString() + EDI_FIELD_DELIMITER;
			data += transactionNumber.ToString(EDI_ST_LINE_TRANSACTION_NUMBER_LONG_FORMAT);
			data += EDI_END_OF_LINE;
			this.i_numberOfSegmentsInDetailBlock = 0;	// Set number of Lines printed in current detail block to zero since this is the end of the current block
			return data;
		}
		//		Write the trailing GE record to mark close of summaries
		private string writeGELineForSummary() 
		{ 
			string data = string.Empty;
			data += "GE" + EDI_FIELD_DELIMITER;
			data += "1" + EDI_FIELD_DELIMITER; 
			data += "1" + EDI_END_OF_LINE;
			return data;
		}
		//		Write the IEA record to mark the end of the EDI document
		private string writeIEALineForSummary(int numberOfFunctionalBlocks, int interchangeControlNumber) 
		{ 
			string data = string.Empty;
			data += "IEA" + EDI_FIELD_DELIMITER;
			data += numberOfFunctionalBlocks + EDI_FIELD_DELIMITER; 
			data += interchangeControlNumber.ToString(EDI_INTERCHANGE_CONTROL_NUMBER_FORMAT) + EDI_END_OF_LINE;
			return data;
		}

		#endregion
		private void ProduceOutputFilesCreateDetail(int index,int intPhase )
		{
				
			string l_String_Output;
			// Number Formats.

			try
			{
				NumberFormatInfo nfi = new CultureInfo( "en-US", false ).NumberFormat;
				nfi.CurrencyGroupSeparator=""; 
				nfi.CurrencySymbol=""; 
				nfi.CurrencyDecimalDigits = 2;
		
				
				switch(index)
				{
					case CSDetail:
						if (this.String_UsCanadaBankCode =="1") 
						{
							//this.lcOutputCursor = "curCSUS";
							this.String_OutputFileType = "CHKSCU";
						}
			
						if (intPhase == 1)
						{			
							if (this.bool_OutputFilePayRoll)
							{
	   					
								//NP - Added to support printing of EDI document - Here we are writing the first part of the EDI details
								if (this.bool_OutputFileEDI_US && this.String_UsCanadaBankCode=="1" && !this.bool_ExcludeThisPersonFromEDI) 
								{
									//Reset count for the numberOfSegments since we are starting a new block
									this.i_numberOfSegmentsInDetailBlock = 0;
									//Write the ST*820, BPR*D, TRN*1, REF*BT, REF*W9, REF*FU lines
									this.i_numberOfDetailBlocks++;	// Increment number of records/detail blocks being printed
									l_String_Output = this.writeSTLine(this.i_numberOfDetailBlocks);
									// write to output
									WriteOutputFilesDataLineIntoStreamEDI(l_String_Output);

									l_String_Output = this.writeBPRLine(this.double_NetPayMonthTotal,
										this.CheckDate);
									//Write to Output
									WriteOutputFilesDataLineIntoStreamEDI(l_String_Output);
									l_String_Output = this.writeTRNLine(this.String_DisbursementNumber);
									//Write to Output
									WriteOutputFilesDataLineIntoStreamEDI(l_String_Output);
									l_String_Output = this.writeREFBatchNumberLine(C_BATCHNUMBER);
									//Write to Output
									WriteOutputFilesDataLineIntoStreamEDI(l_String_Output);
									l_String_Output = this.writeREFMailingInformationLines();
									//Write to Output
									WriteOutputFilesDataLineIntoStreamEDI(l_String_Output);
									l_String_Output = this.writeREFFundIdentificationInformationLine(this.String_FundID);
									//Write to Output
									WriteOutputFilesDataLineIntoStreamEDI(l_String_Output);
									l_String_Output = this.writeREFAccountingStatusLine(this.String_FilingStatusExemptions);
									//Write to Output
									WriteOutputFilesDataLineIntoStreamEDI(l_String_Output);
									l_String_Output = this.writeREFTaxingAuthorityIdentificationNumberLine(double_WithholdingMonthAddl);
									//Write to Output
									WriteOutputFilesDataLineIntoStreamEDI(l_String_Output);
									//					N1-N4 Payor Lines no longer need to be printed as per Mark/Clarence's email.
									//									l_String_Output = this.writeN1toN4PayorInformationLines();
									//									//Write to Output
									//									WriteOutputFilesDataLineIntoStreamEDI(l_String_Output);
									l_String_Output = this.writeN1toN4PayeeInformationLines(this.String_DBFirstName, this.String_DBMiddleName, this.String_DBLastName, 
										this.String_Address1, this.String_DBAddress2, this.String_DBAddress3, 
										this.String_City, this.String_State, this.String_ZIP, this.String_Country);
									//Write to Output
									WriteOutputFilesDataLineIntoStreamEDI(l_String_Output);
									l_String_Output = this.writeENTLine("1");
									//Write to Output
									WriteOutputFilesDataLineIntoStreamEDI(l_String_Output);

									//									l_String_Output = this.writeRMRCurrentPaymentLine(this.double_NetPayMonthTotal, this.double_GrossPayMonthTotal,
									//										this.double_WithholdingMonthTotal, 1, this.double_GrossPayMonthTaxable);
									//									l_String_Output = this.writeRMRYTDPaymentLine(this.double_NetPayYTDTotal, this.double_GrossPayYTDTotal, 
									//										this.double_WithholdingYTDTotal, 1, this.double_GrossPayYTDTaxable);
									//									// Write Non-taxable line to EDI
									//									l_String_Output = this.writeNTELines(this.double_GrossPayMonthNontaxable, this.double_GrossPayYTDNontaxable);
									
									//NP: 2007.04.26 We want to print the following lines only if we are 
									//	performing regular Payroll processing. - as per Mark's email
									if (this.String_DisbursementType.ToString() != "EXP") 
									{
										l_String_Output = this.writeRMR_IV_CurrentTaxablePaymentLine(this.double_GrossPayMonthTaxable, this.double_GrossPayYTDTaxable);
										//Write to Output
										WriteOutputFilesDataLineIntoStreamEDI(l_String_Output);
										l_String_Output = this.writeRMR_IK_CurrentNonTaxablePaymentLine(this.double_GrossPayMonthNontaxable, 
											this.double_GrossPayYTDNontaxable);
										//Write to Output
										WriteOutputFilesDataLineIntoStreamEDI(l_String_Output);
									}

									// Write Special Dividend line to EDI. The value is stored in different places depending on the 
									// value in this.String_DisbursementType. (either ANN or something else)
									//									l_String_Output = this.writeNTELinesSpecialDividend(
									//										this.String_DisbursementType == "ANN" ? this.double_GrossPayDividentMonthTotal : this.double_GrossPayMonthTaxable, 
									//										this.String_DisbursementType == "ANN" ? this.double_GrossPayDividentYTDTaxable : this.double_GrossPayYTDTaxable
									//										);
									l_String_Output = this.writeRMR_2G_SpecialDividendPaymentLine(
										this.String_DisbursementType == "ANN" ? this.double_GrossPayDividentMonthTotal : this.double_GrossPayMonthTaxable, 
										this.String_DisbursementType == "ANN" ? this.double_GrossPayDividentYTDTaxable : this.double_GrossPayYTDTaxable
										);
									//Write to Output
									WriteOutputFilesDataLineIntoStreamEDI(l_String_Output);


									// Insert
									// Record - (Other Deductions) - Can be multiple records
								}
							}
						}
						else
						{
							if (this.bool_OutputFilePayRoll)
							{
								//NP - Added to support printing of EDI document - Here we are printing the N9, AMT and SE the EDI detail section
								if (this.bool_OutputFileEDI_US && this.String_UsCanadaBankCode == "1" && !this.bool_ExcludeThisPersonFromEDI) 
								{
									//Write the N9 Line for the detail segment
									l_String_Output = this.writeN9Line();
									//Write to Output
									WriteOutputFilesDataLineIntoStreamEDI(l_String_Output);

									//Write AMT Lines for person details
									l_String_Output = this.writeAMTLine(this.double_NetPayMonthTotal, this.double_NetPayYTDTotal,
										this.double_GrossPayMonthTotal, this.double_GrossPayYTDTotal,
										this.double_WithholdingMonthTotal, this.double_WithholdingYTDTotal);
									//Write to Output
									WriteOutputFilesDataLineIntoStreamEDI(l_String_Output);

									//Write the closing SE line for the Detail section
									l_String_Output = this.writeSELine(this.i_numberOfSegmentsInDetailBlock, this.i_numberOfDetailBlocks);
									//Write to Output
									WriteOutputFilesDataLineIntoStreamEDI(l_String_Output);
								}

							}
							
						}

						break;
				
					default:
						break;
				}


			}
			catch
			{
			}

		}

		private void ProduceOutputFilesCreateDetailMonthlyHolding()
		{
			string l_String_Output;
			string l_String_temp;
			
			// define Number Formats
			try
			{
				NumberFormatInfo nfi = new CultureInfo( "en-US", false ).NumberFormat;
				nfi.CurrencyGroupSeparator=""; 
				nfi.CurrencySymbol=""; 
				nfi.CurrencyDecimalDigits = 2;

				l_String_Output = C_ROWCODED;
				l_String_temp = this.String_WithholdingDetailDesc.Trim().PadRight(29,' ');
				if (l_String_temp.Length > 29) l_String_temp = l_String_temp.Substring(0,28);
						
				//start of comment by hafiz on 13Apr2006
				//l_String_Output = l_String_temp;
				//end of comment by hafiz on 13Apr2006
						
				//start of code add by hafiz on 13Apr2006
				l_String_Output += l_String_temp;
				//end of code add by hafiz on 13Apr2006

				//NP - Added to support printing of EDI document - Here we are printing the withholding details
				if (this.bool_OutputFileEDI_US && this.String_UsCanadaBankCode == "1" && !this.bool_ExcludeThisPersonFromEDI) 
				{
					
					//					//Write to EDI_US
					l_String_Output = this.writeDEDDeductionsLines(this.String_WithholdingDetailDesc.Trim(), this.double_WithholdingMonthDetail, 
						this.double_WithholdingYTDDetail, this.DateTime_currentDateTime);
					//Write to Output
					WriteOutputFilesDataLineIntoStreamEDI(l_String_Output);
				}
			}
			catch
			{


			}

			// Insert
	            
		}

		public DataTable datatable_FileList
		{
			get{return l_datatable_FileList;}
		}
		
		public  bool PreparOutPutFileData(DataTable parameterEDIDatatable,DataSet parameter_dataset_EDIProcessData,DateTime parameterDateCheckDate,string parameterDibursementType)
		{
			DataSet ldataset = parameter_dataset_EDIProcessData;
			//string lstr_tmp;
			string lcMaritalStatusCode,lcWithholdingType;
			string l_String_expr;//,l_String_Guid;
			DataRow[] foundWithholdingsRows,foundYTDWithholdingsRows,foundFedTaxWithholdingRORows;
			//	String_WithholdingDetailDesc,str_WithholdingTypeCode,
			
			this.dateTimePayrollMonth = parameterDateCheckDate;

			//DataRow[] EDISelectedRows = null;
			DataRow[] foundRows = null;

	//		DataRowCollection rc; 
			DataRow oRow ;
					
			// aparna 19/06/2007
			
			//l_String_Guid ="";
			object[] rowVals = new object[3];
			//by Aparna 18/06/2007

				    
			//Hashtable HTwithHoldings = new Hashtable();
			//start of comment by hafiz on 12April2006
			//this.ProofReport = false;
			//end of comment by hafiz on 12April2006

			//NP - Added code here to populate the Exclusion List for EDI
			//by aparna 18th june 2007

			try
			{ 
				//DataTable oDataTable = ldataset.Tables["PayRoll"];
				//DataTable oDataTable = parameterEDIDataset.Tables["EDIUS List"];				
				DataTable oDataTable = parameterEDIDatatable;
				DataTable oDataTableWithholdings = ldataset.Tables["Withholdings"];
				DataTable oDataTableYTDWithholdings = ldataset.Tables["YTDWithholdings"];
				//DataTable oDataTableDisbursementFiles = ldataset.Tables["DisbursementFiles"];
				DataTable oDataTableOutPutfilePath = ldataset.Tables["OutPutfilePath"];
				DataTable oDataTableFedTaxWithholdingRO = ldataset.Tables["FedTaxWithholdingRO"];
//
//				if (oDataTable.Rows.Count > 0) 
//				{
//					l_String_expr = "bitExcluded = 0 "  ;
//					EDISelectedRows = oDataTable.Select(l_String_expr);
//				}

				//rc = oDataTableDisbursementFiles.Rows;
								
				this.bool_OutputFileEDI_US  = true;
				this.String_PaymentMethodCode ="CHECK";
				
				// Initialize the global dateTime value that is to be used at various 
				// locations in the EDI and Batch process. This value is initialized  
				// to the DateTime when the process is being executed.
				this.DateTime_currentDateTime = DateTime.Now;

				if (!ProduceOutputFilesOutput(oDataTableOutPutfilePath)) return false;
				// Add rows for the Header Information.
						
				this.String_DisbursementType = parameterDibursementType.ToString().Trim();

				ProduceOutputFilesCreateHeader();
					

				if (oDataTable == null) {}
	    
				//foreach (DataRow oRow in oDataTable.Rows)
				//foreach (DataRow dRow in EDISelectedRows)
				foreach (DataRow dRow in oDataTable.Rows)
				{

					//by aparna 20/06/2007				
					this.String_PersID = dRow["PersID"].ToString() ;
					l_String_expr = "PersID = '" + this.String_PersID + "' " ;
					foundRows = ldataset.Tables["EDIUS List"].Select(l_String_expr);
					oRow=foundRows[0];
					//by aparna 20/06/2007	


					InitializseMembers();

					this.String_PersID = oRow["PersID"].ToString() ;
					this.String_Address1 = oRow["Addr1"].ToString().Trim() ;

					//NP - Added to support printing of EDI documents
					if (!oRow.IsNull("City")) this.String_City = oRow["City"].ToString();
					if (!oRow.IsNull("StateType")) this.String_State = oRow["StateType"].ToString();
					if (!oRow.IsNull("Zip") ) this.String_ZIP = oRow["Zip"].ToString();
					if (!oRow.IsNull("Country")) this.String_Country = oRow["Country"].ToString();
					if (!oRow.IsNull("Addr2") ) this.String_DBAddress2 = oRow["Addr2"].ToString();
					this.String_DBAddress3 = string.Empty;
					//NP - We decide whether this person needs to be excluded from EDI generation or not
					//DataRow[] dr = oDataTableEDI_Exclusions.Select("guiPersId='" + this.String_PersID + "'");
					//this.bool_ExcludeThisPersonFromEDI = (dr.Length > 0) ? true : false;

					if (oRow["Addr2"].GetType().ToString() =="System.DBNull"  || oRow["Addr2"].ToString() =="")
					{

						this.String_Address2 = "";
						if (oRow["City"].GetType().ToString()!="System.DBNull")
						{
							this.String_Address2 = oRow["City"].ToString().Trim();
						}

						this.String_Address2 = this.String_Address2 + ",";

						if (oRow["StateType"].GetType().ToString() !="System.DBNull")
						{
							this.String_Address2 = this.String_Address2 + oRow["StateType"].ToString().Trim();
						}

						this.String_Address2 = this.String_Address2 + " ";

						if (oRow["Zip"].GetType().ToString()  !="System.DBNull")
						{
							this.String_Address2 = this.String_Address2 + oRow["Zip"].ToString().Trim();
						}
					}
					else
					{
						this.String_Address2 =  oRow["Addr2"].ToString().Trim();

						this.String_Address3 = "";

						if (oRow["City"].GetType().ToString()!="System.DBNull")
						{
							this.String_Address3 = oRow["City"].ToString().Trim();
						}

						this.String_Address3 = this.String_Address3 + ",";

						if (oRow["StateType"].GetType().ToString() !="System.DBNull")
						{
							this.String_Address3 = this.String_Address3 + oRow["StateType"].ToString().Trim();
						}

						this.String_Address3 = this.String_Address3 + " ";

						if (oRow["Zip"].GetType().ToString()  !="System.DBNull")
						{
							this.String_Address3 = this.String_Address3 + oRow["Zip"].ToString().Trim();
						}
					}
					this.String_Address4 = "";
					this.String_Address5 ="";
					// Check Date should be assigned.
					
					this.CheckDate =parameterDateCheckDate;
					this.String_CompanyData = "  ";
					this.String_Description =" ";
					this.String_Description = this.String_Description.PadLeft(39,' '); 
					this.DisbursementDate = this.CheckDate; 
					this.String_DisbursementID = Convert.ToString(oRow["DisbursementID"]);
					this.String_DisbursementNumber = Convert.ToString(oRow["DisbursementNumber"]);
							
					this.String_DisbursementType = parameterDibursementType.ToString().Trim();

					this.String_FundID = Convert.ToString(oRow["FundIDNo"]);
					//aparna -12/02/2007 -YREN-3074
					//this.String_IndividualID = (string) oRow["SSNo"];
					this.String_IndividualID = this.String_FundID;
					if (!oRow.IsNull("FirstName")) this.String_DBFirstName = oRow["FirstName"].ToString();
					if (!oRow.IsNull("MiddleName")) this.String_DBMiddleName = oRow["MiddleName"].ToString();
					if (!oRow.IsNull("LastName")) this.String_DBLastName = oRow["LastName"].ToString();
					if (oRow["MiddleName"].ToString().Trim()=="Guardian of") 
					{
						this.String_Name22 = oRow["FirstName"].ToString();
						this.String_Name60 = oRow["FirstName"].ToString() + " " + oRow["MiddleName"].ToString() + " "								+ oRow["LastName"].ToString();
					}
					else
					{	
						this.String_Name22  = oRow["FirstName"].ToString().Trim() + " ";  
						if (oRow["MiddleName"].ToString().Trim() !="") 
						{
							this.String_Name22 = this.String_Name22 +  oRow["MiddleName"].ToString().Trim() + " " ;
	                        
						}
						this.String_Name22 = this.String_Name22 +  oRow["Lastname"].ToString().Trim();

						if (this.String_Name22.Length  > 60) this.String_Name60 = this.String_Name22.Substring(0,60); 
						else this.String_Name60 = this.String_Name22;
						if (this.String_Name22.Length  > 22)  this.String_Name22 = this.String_Name22.Substring(0,22); 
					
					}
					
					this.double_NetPayMonthTotal=Convert.ToDouble(oRow["NetAmount"]);

					if (this.double_NetPayMonthTotal < 0 ) this.double_NetPayMonthTotal = 0;

				//	this.String_ReceivingDFEAccount = oRow["BankAcctNumber"].ToString() ;
					
					this.String_UsCanadaBankCode = "1";
					this.String_FilingStatusExemptions = "?-??";

					l_String_expr = "guiPersID = '" + this.String_PersID + "' and chrTaxEntityType = 'IRS'" ;

					foundFedTaxWithholdingRORows = oDataTableFedTaxWithholdingRO.Select(l_String_expr);

					foreach (DataRow qRow in foundFedTaxWithholdingRORows)
					{
						if (qRow["mnyAmount"].GetType().ToString() !="System.DBNull") this.double_WithholdingMonthAddl  = Convert.ToDouble(qRow["mnyAmount"]); 
						else this.double_WithholdingMonthAddl  = 0;

						this.String_FilingStatusExemptions = "?-??";
						lcMaritalStatusCode ="";

						if (qRow["chrWithholdingType"].GetType().ToString() !="System.DBNull" || qRow["chrWithholdingType"].ToString().Trim()!="")
						{
							lcWithholdingType = qRow["chrWithholdingType"].ToString(); 
							if (qRow["chrWithholdingType"].GetType().ToString() !="System.DBNull") lcMaritalStatusCode   = qRow["chvMaritalStatusCode"].ToString() ; 
							if (lcMaritalStatusCode.Trim()    == "") lcMaritalStatusCode = "U";

							if (qRow["intExemptions"].GetType().ToString() !="System.DBNull") this.double_Exemptions  = Convert.ToDouble(qRow["intExemptions"]); 
							else double_Exemptions  = 0;

							// DSFILINGSTATUSEXEMPTIONSOUTPUT
							switch (lcWithholdingType.Trim() )
							{
								case "FORMUL":
									this.String_FilingStatusExemptions = lcMaritalStatusCode.Trim()+ "-" + this.double_Exemptions.ToString().Trim().PadLeft(2,'0');   
									break;
								case "FLAT":
									this.String_FilingStatusExemptions = "9-99";
									break;
								default:
									this.String_FilingStatusExemptions = "?-??";
									break;
							}
	                
						}

						break;
					}
					

													
					if (oRow["TaxableAmount"].ToString() !="System.DBNull") this.double_GrossPayMonthTaxable= Convert.ToDouble(oRow["TaxableAmount"]);
					else this.double_GrossPayMonthTaxable =0;

					if (oRow["NonTaxableAmount"].ToString() !="System.DBNull") this.double_GrossPayMonthNontaxable= Convert.ToDouble(oRow["NonTaxableAmount"]);
					else this.double_GrossPayMonthNontaxable =0;

					this.double_GrossPayMonthTotal = this.double_GrossPayMonthTaxable + this.double_GrossPayMonthNontaxable;

					if (oRow["YTDPayNonTaxable"].GetType().ToString() !="System.DBNull") this.double_GrossPayYTDNontaxable    = Convert.ToDouble(oRow["YTDPayNonTaxable"]); 
					else this.double_GrossPayYTDNontaxable    = 0;
					
					if (oRow["YTDPayTaxable"].GetType().ToString() !="System.DBNull") this.double_GrossPayYTDTaxable  = Convert.ToDouble(oRow["YTDPayTaxable"]); 
					else this.double_GrossPayYTDTaxable    = 0;
					
					if (oRow["YTDPayNonTaxable"].GetType().ToString() !="System.DBNull") this.double_GrossPayYTDTotal  = Convert.ToDouble(oRow["YTDPayNonTaxable"]); 
					else this.double_GrossPayYTDTotal    = 0;
					if (oRow["YTDPayTaxable"].GetType().ToString() !="System.DBNull") this.double_GrossPayYTDTotal  += Convert.ToDouble(oRow["YTDPayTaxable"]); 
					else this.double_GrossPayYTDTotal    += 0;
							
					//populate Divident Values
					this.double_GrossPayDividentMonthTotal = 0.00;

					if (oRow["YTDPayTaxableDivident"].GetType().ToString() !="System.DBNull") this.double_GrossPayDividentYTDTaxable  = Convert.ToDouble(oRow["YTDPayTaxableDivident"]); 
					else this.double_GrossPayDividentYTDTaxable     = 0;

					this.double_GrossPayYTDTotal +=  double_GrossPayDividentYTDTaxable;

					//Get Year to Date (YTD) WithHoldings

					if (oRow["YTDPayTaxable"].GetType().ToString() !="System.DBNull") this.double_NetPayYTDTotal  = Convert.ToDouble(oRow["YTDPayTaxable"]); 
					else this.double_NetPayYTDTotal     = 0;
					if (oRow["YTDPayNonTaxable"].GetType().ToString() !="System.DBNull") this.double_NetPayYTDTotal  += Convert.ToDouble(oRow["YTDPayNonTaxable"]); 
					else this.double_NetPayYTDTotal     += 0;
					if (oRow["YTDWithholding"].GetType().ToString() !="System.DBNull") this.double_NetPayYTDTotal  -= Convert.ToDouble(oRow["YTDWithholding"]); 
					else this.double_NetPayYTDTotal     -= 0;
					                    
					this.double_NetPayYTDTotal += this.double_GrossPayDividentYTDTaxable;

					if (oRow["YTDWithholding"].GetType().ToString() !="System.DBNull") this.double_WithholdingYTDTotal  = Convert.ToDouble(oRow["YTDWithholding"]); 
					else this.double_WithholdingYTDTotal     = 0;
					
					if (oRow["MonthlyWithhold"].GetType().ToString() !="System.DBNull") this.double_WithholdingMonthTotal  = Convert.ToDouble(oRow["MonthlyWithhold"]); 
					else this.double_WithholdingMonthTotal     = 0;
						
				
					//Withholdings
				
					this.String_WithholdingDetailDesc = "";
					this.double_WithholdingMonthDetail =0;
					this.double_WithholdingYTDDetail = 0;

					l_String_expr = "PersID = '" + this.String_PersID   + "'";
					foundWithholdingsRows = oDataTableWithholdings.Select(l_String_expr);

					if (this.String_PaymentMethodCode.Trim()  =="CHECK")	
					{
						ProduceOutputFilesCreateDetail(CSDetail,1);
							
						foreach (DataRow lRow in foundWithholdingsRows)
						{
							if (lRow["Description"].GetType().ToString() !="System.DBNull") this.String_WithholdingDetailDesc  = lRow["Description"].ToString() ; 
							else String_WithholdingDetailDesc= "";

							if (lRow["Amount"].GetType().ToString() !="System.DBNull") this.double_WithholdingMonthDetail  = Convert.ToDouble(lRow["Amount"]); 
							else double_WithholdingMonthDetail= 0;

							double_WithholdingYTDDetail = 0;

							l_String_expr = "PersID = '" + this.String_PersID + "' and WithholdingTypeCode = '" + lRow["WithholdingTypeCode"].ToString().Trim()+ "'" ;

							foundYTDWithholdingsRows = oDataTableYTDWithholdings.Select(l_String_expr);

							foreach (DataRow pRow in foundYTDWithholdingsRows)
							{
								if (pRow["YTDWithholding"].GetType().ToString() !="System.DBNull") this.double_WithholdingYTDDetail  += Convert.ToDouble(pRow["YTDWithholding"]); 
								else double_WithholdingYTDDetail = 0;	
								break;
							}

							//HTwithHoldings.Add(String_WithholdingDetailDesc,double_WithholdingMonthDetail,double_WithholdingYTDDetail);
								
							ProduceOutputFilesCreateDetailMonthlyHolding();
						}
						ProduceOutputFilesCreateDetail(CSDetail,2);
					}
						
				}

				ProduceOutputFilesCreateFooters();
				CloseOutputStreamFiles();
				
						
				return true;
			}
			catch 
			{
				throw;
			}

		}
		private void InitializseMembers()
		{
			try
			{
				this.String_PersID	= this.String_Address1 = this.String_Address2 = this.String_Address3 ="";
				this.String_Address4 = this.String_Address5 = String_CompanyData = String_Description = String_DisbursementID = "";
				this.String_DisbursementNumber = this.String_FilingStatusExemptions = this.String_FundID = "";
				this.String_IndividualID = this.String_Name22 = this.String_Name60 =  "";
				//this.String_EftTypeCode = "";
				this.String_DisbursementType = this.String_UsCanadaBankCode ="";
				this.double_NetPayMonthTotal=0;
				
				// PayeePayroll Variables
				this.double_GrossPayMonthNontaxable = this.double_GrossPayMonthTaxable = this.double_GrossPayMonthTotal= this.double_GrossPayYTDNontaxable = 0; 
				this.double_GrossPayYTDTaxable = this.double_GrossPayYTDTotal = this.double_NetPayYTDTotal = this.double_WithholdingMonthTotal = 0; 
				this.double_WithholdingYTDTotal = this.double_WithholdingMonthAddl = 0;

				this.double_WithholdingMonthDetail = this.double_WithholdingYTDDetail = 0;

				this.double_GrossPayDividentMonthTotal = 0;
				this.double_GrossPayDividentYTDTaxable = 0;

				this.String_WithholdingDetailDesc=""; 

				this.double_Exemptions = 0;

				initializeEDIVariables();
			}
			catch
			{
				throw;
			}

		}

		
		
		private bool ProduceOutputFilesOutput(DataTable  oDataTableOutPutfilePath)
		{
			//StreamOutputFileCSCanadian,StreamOutputFileCSUS,StreamOutputFilePosPay,StreamOutputFileEFT_NorTrust;
			string lcFileName,lcFilenameSuffix,lcOutPutFileNameBak,lcDateSuffix,lcDateTimeSuffix,lcOutputFileName,lcMetaOutputFileType; 

			DateTime ld_date = this.DateTime_currentDateTime;
			DataSet l_dataset= new DataSet(); 
				
			DataRowCollection rc; 
			DataRow DfNewRow;
			DataSet dsNewGuid;
			object[] rowVals = new object[4];
					
			DataRow l_datarow_FileList;
			try
			{

				rc = oDataTableOutPutfilePath.Rows;

				
				lcFilenameSuffix = "";
							
				l_dataset = YMCARET.YmcaDataAccessObject.YMCACommonDAClass.MetaOutputFileType("EDI_US");  
		            
				lcFileName = "";
				lcOutPutFileNameBak="";
				lcMetaOutputFileType = "";

				if (l_dataset == null) return false; 
				ld_date = this.DateTime_currentDateTime;
				lcDateSuffix = ld_date.ToString("MMddyy").Trim() ;
				lcDateTimeSuffix = ld_date.ToString("yyyyMMdd").Trim();
				lcDateTimeSuffix += "_" + ld_date.ToString("hhmmss").Trim();
		 
				foreach (DataRow oRow in l_dataset.Tables[0].Rows)  
				{
					lcFileName = oRow["FilenamePrefix"].ToString().Trim(); 

					lcMetaOutputFileType = oRow["OutputFileType"].ToString().Trim(); 

					if (Convert.ToBoolean(oRow["DateSuffix"])== true) 
					{
						lcFileName += lcDateSuffix;
					}

					if (Convert.ToBoolean(oRow["PaymentManagerSuffix"])== true) 
					{
						lcFileName += lcFilenameSuffix;

					}

					if (oRow["OutputDirectory"].GetType().ToString()=="System.DBNull" || oRow["OutputDirectory"].ToString().Trim()=="") return false;
				

					//NP Added to support creation of the output streams for EDI
					if (this.bool_OutputFileEDI_US && lcMetaOutputFileType == "EDI_US") 
					{
						l_datarow_FileList = l_datatable_FileList.NewRow();

						l_datarow_FileList["SourceFolder"] = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["EDI_US"]);
						l_datarow_FileList["SourceFile"] = l_datarow_FileList[0] + "\\" + lcFileName.Trim() + "." + oRow["FilenameExtension"].ToString().Trim(); 
							
						l_datarow_FileList["DestFolder"] = oRow["OutputDirectory"].ToString().Trim();
						l_datarow_FileList["DestFile"] = oRow["OutputDirectory"].ToString().Trim()+ "\\" + lcFileName.Trim() + "." + oRow["FilenameExtension"].ToString().Trim();  

						if (!Directory.Exists((string)l_datarow_FileList[0])) 
						{
							// Throw an exception.
							throw new Exception("Error! Output Directory does not exists. \nFolder: " + (string)l_datarow_FileList["SourceFolder"] + " Not Found"); 																				
						}
						else 
						{
							this.l_datatable_FileList.Rows.Add(l_datarow_FileList);
						}

						l_datarow_FileList = l_datatable_FileList.NewRow();

						l_datarow_FileList["SourceFolder"] = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["EDI_US"]);
						l_datarow_FileList["SourceFile"] = l_datarow_FileList[0] + "\\" + lcFileName.Trim() + "_" + lcDateTimeSuffix.Trim() + "." + oRow["FilenameExtension"].ToString().Trim(); 
							
						l_datarow_FileList["DestFolder"] = oRow["OutputDirectory"].ToString().Trim();
						l_datarow_FileList["DestFile"] = oRow["OutputDirectory"].ToString().Trim()+ "\\" + lcFileName.Trim() + "_" + lcDateTimeSuffix.Trim() + "." + oRow["FilenameExtension"].ToString().Trim();  

						if (!Directory.Exists((string)l_datarow_FileList[0])) 
						{
							// Throw an exception.
							throw new Exception("Error! Output Directory does not exists. \nFolder: " + (string)l_datarow_FileList["SourceFolder"] + " Not Found"); 																				
						}
						else 
						{
							this.l_datatable_FileList.Rows.Add(l_datarow_FileList);
						}

						lcOutputFileName = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["EDI_US"])+ "\\" + lcFileName.Trim() + "." + oRow["FilenameExtension"].ToString().Trim();  ;
						lcOutPutFileNameBak = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["EDI_US"])+ "\\" + lcFileName.Trim() + "_" + lcDateTimeSuffix.Trim() + "." + oRow["FilenameExtension"].ToString().Trim();  

						this.StreamOutputFileEDI_US = File.CreateText(lcOutputFileName);  
						this.StreamOutputFileEDI_USBack = File.CreateText(lcOutPutFileNameBak);  

						dsNewGuid = YMCARET.YmcaDataAccessObject.MonthlyPayrollDAClass.GetNewGuid();

						if (dsNewGuid != null) 
					
						{

					
							if (dsNewGuid.Tables[0] != null) 
							{
								rowVals[0] = dsNewGuid.Tables[0].Rows[0]["Newguid"].ToString().Trim(); 
								//used to maintain Record in AtsEDIPayments Table
 							   this.EDIOutputFileID=dsNewGuid.Tables[0].Rows[0]["Newguid"].ToString().Trim(); 
								rowVals[1] = oRow["OutputDirectory"].ToString().Trim();
								//rowVals[1] = l_datarow_FileList["SourceFolder"].ToString().Trim();
								rowVals[2] = lcFileName.Trim() + "." + oRow["FilenameExtension"].ToString().Trim();
								rowVals[3] = lcMetaOutputFileType;
								DfNewRow = rc.Add(rowVals);

								//this.String_OutputFileCSUSGuid = dsNewGuid.Tables[0].Rows[0]["Newguid"].ToString().Trim();  

							}
						}
					}
				}


				//NP -	Code checks here if the configuration entries exist in the 
				//		AtsMetaOutputFileTypes table for all files we need to create. If not then 
				//		we need to throw an exception which is caught in the main process which 
				//		eventually aborts the process.
				if (this.bool_OutputFileEDI_US && this.StreamOutputFileEDI_US == null) throw new Exception("Configuration entry not defined for Electronic Data Interchange for US cheques (EDI_US) output file in the database.");
			
				return true;
			}
			catch (Exception ex)
			{
				throw ex;
			}

		}

//		//NP - Added to support writing to the output stream for EDI documents
		private void WriteOutputFilesDataLineIntoStreamEDI(string parameterStringOutput) 
		{
			try 
			{
				if (parameterStringOutput != string.Empty && parameterStringOutput != "\r\n") 
				{
					if (StreamOutputFileEDI_US!=null) StreamOutputFileEDI_US.WriteLine(parameterStringOutput);
					if (StreamOutputFileEDI_USBack!=null) StreamOutputFileEDI_USBack.WriteLine(parameterStringOutput);
				}
			} 
			catch 
			{
				throw;
			}
		}


		//NP: 2007.04.26 - Returns the characters 'T' or 'P' depending on the value 
		//		of EDI_MODE key in the atsMetaConfiguration table. 
		//		'T' = Test mode and 'P' = Production mode
		//		EXCEPTIONS:
		//		Throws an exception if the EDI_MODE key is not defined. 
		//		Throws an exception if the value for the key is not a valid value.
		


		
		private void CloseOutputStreamFiles()
		{
			try
			{
				//NP - Added to support printing of EDI document - Here we are closing the output stream
				if (this.bool_OutputFileEDI_US)
				{
					this.StreamOutputFileEDI_US.Close();
					this.StreamOutputFileEDI_USBack.Close();
				}

			}
			catch
			{
				throw;

			}
		}

		

		
	}



}

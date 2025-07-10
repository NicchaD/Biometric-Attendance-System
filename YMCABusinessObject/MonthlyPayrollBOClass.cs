//****************************************************
//Modification History
//****************************************************
//Modified by          Date          Description
//****************************************************
//Manthan Rajguru      2015.09.16    YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
//Manthan Rajguru         2017.01.26    YRS-AT-3288 -  YRS bug: bitActive and bitPrimary validation for Annuity Processing
//Megha Lad			   2019.12.26 	YRS-AT-4602  State Withholding Project - Export file Annuity Payroll
//Megha Lad			   2019.12.26 	YRS-AT-4677  State Withholding - Validations for exporting "Change File" (Monthly Payroll)
//*****************************************************
using System;
using YMCARET.YmcaDataAccessObject;
using System.Data;
using System.Collections;
namespace YMCARET.YmcaBusinessObject
{
	/// <summary>
	/// Summary description for MonthlyPayroll.
	/// </summary>
	public class MonthlyPayroll
	{
		DataTable l_datatable_FileList;
        DataTable ExceptionLogForNTPYRL;//ML | 2019.12.19 | YRS-AT-4677 | Declare Variable
		public MonthlyPayroll()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public static DataSet getCurrentCheckSeries()
		{
			try
			{
				return (YMCARET.YmcaDataAccessObject.MonthlyPayrollDAClass.CurrentCheckSeries());  
			}
			catch
			{
				throw;
			}

		}
	
		public static DataSet getPayrollLast()
		{
			try
			{
				return (YMCARET.YmcaDataAccessObject.MonthlyPayrollDAClass.PayRollLast()); 
			}
			catch
			{
				throw;
			}

		}

		public static DataSet getnextBusinessDay(System.DateTime ParameternextBusinessDate)
		{
			try
			{
				return (YMCARET.YmcaDataAccessObject.MonthlyPayrollDAClass.GetNextBusinessDay(ParameternextBusinessDate));
			}
			catch
			{
				throw;
			}

		}
		
		public DataTable datatable_FileList
		{
			get{return l_datatable_FileList;}
		}
        //START : ML | 2019.12.19 | YRS-AT-4677 | Declare Variable
        public DataTable dtExceptionLogForNTPYRL
        {
            get { return ExceptionLogForNTPYRL; }
        }
        //END : ML | 2019.12.19 | YRS-AT-4677 | Declare Variable
		//public static bool ProcessPayrolldata(System.DateTime parameterdateTimePayrollDate, bool parameterboolProof,long parameterFirstCheckCanada,long parameterFirstCheckUS,DateTime parameterDateCheckDate )
		public bool ProcessPayrolldata(System.DateTime parameterdateTimePayrollDate, bool parameterboolProof,long parameterFirstCheckCanada,long parameterFirstCheckUS,DateTime parameterDateCheckDate)
		{
			bool bool_Return = false;
			try
			{
				YMCARET.YmcaDataAccessObject.MonthlyPayrollDAClass objMonthlyPayrollDAClass;
				objMonthlyPayrollDAClass = new YMCARET.YmcaDataAccessObject.MonthlyPayrollDAClass();
				bool_Return = objMonthlyPayrollDAClass.getPayRollData(parameterdateTimePayrollDate,parameterboolProof,parameterFirstCheckCanada,parameterFirstCheckUS,parameterDateCheckDate);
				if (bool_Return==true)
				{
					l_datatable_FileList = objMonthlyPayrollDAClass.datatable_FileList;
				}
			    //START : ML | 2019.12.19 | YRS-AT-4677 | Declare Variable
                if (parameterboolProof == true)
                {
                    ExceptionLogForNTPYRL = objMonthlyPayrollDAClass.dtExceptionLogForNTPYRL;
                    //if (HelperFunctions.isNonEmpty(ExceptionLogForNTPYRL))
                    //    return false;
                    //else
                    //    return true;
                }
			    //END : ML | 2019.12.19 | YRS-AT-4677 | Declare Variable
				return bool_Return;
				//return (YMCARET.YmcaDataAccessObject.MonthlyPayrollDAClass.getPayRollData(parameterdateTimePayrollDate,parameterboolProof,parameterFirstCheckCanada,parameterFirstCheckUS,parameterDateCheckDate,datatable_FileList)); 				
			}
			catch
			{
				throw;
			}
		}
		
		public static bool UpdateCheckSeriesCurrentValues(DataSet parameter_DatasetCheckSeries)
		{
			try
			{
				return (YMCARET.YmcaDataAccessObject.MonthlyPayrollDAClass.UpdateCheckSeriesCurrentValues(parameter_DatasetCheckSeries)); 
				
			}
			catch
			{
				throw;
			}

		}
		// For EXP Dividends 
		public bool ProcessXPDividentData(System.DateTime parameterdateTimePayrollDate, bool parameterboolProof,long parameterFirstCheckCanada,long parameterFirstCheckUS,DateTime parameterDateCheckDate,string parameterCurrentExpUniqueiId)
		{
			bool bool_Return = false;
			try
			{
				YMCARET.YmcaDataAccessObject.MonthlyPayrollDAClass objMonthlyPayrollDAClass;
				objMonthlyPayrollDAClass = new YMCARET.YmcaDataAccessObject.MonthlyPayrollDAClass();
				bool_Return = objMonthlyPayrollDAClass.getEXPDividentData(parameterdateTimePayrollDate,parameterboolProof,parameterFirstCheckCanada,parameterFirstCheckUS,parameterDateCheckDate,parameterCurrentExpUniqueiId);
				if (bool_Return==true)
				{
					l_datatable_FileList = objMonthlyPayrollDAClass.datatable_FileList;
				}
				return bool_Return;
				//return (YMCARET.YmcaDataAccessObject.MonthlyPayrollDAClass.getPayRollData(parameterdateTimePayrollDate,parameterboolProof,parameterFirstCheckCanada,parameterFirstCheckUS,parameterDateCheckDate,datatable_FileList)); 				
			}
			catch
			{
				throw;
			}
		}
		public static DataSet getnextPayrollDayteForExp()
		{
			try
			{
				return (YMCARET.YmcaDataAccessObject.MonthlyPayrollDAClass.getnextPayrollDayteForExp());
			}
			catch
			{
				throw;
			}

		}

		public static int ValidateLastPayroll(DateTime p_ldate_SpecialDvidentDate)
		{
			try
			{
				return (YMCARET.YmcaDataAccessObject.MonthlyPayrollDAClass.ValidateLastPayroll(p_ldate_SpecialDvidentDate));
			}
			catch
			{
				throw;
			}

		}

        //START: MMR | 2017.01.25 | YRS-AT-3288 | Added to get list of persons with missing and duplicate Address
        public static DataSet GetDuplicateMissingAddressInfo(DateTime checkDate)
        {
            try
            {
                return (YMCARET.YmcaDataAccessObject.MonthlyPayrollDAClass.GetDuplicateMissingAddressInfo(checkDate));
            }
            catch
            {
                throw;
            }

        }
        //END: MMR | 2017.01.25 | YRS-AT-3288 | Added to get list of persons with missing and duplicate Address


        //START: SR | 2020.01.08 | YRS-AT-4602 | Get Payment outsourcing key value from databse
        public static bool SetPaymentOutsourcingKey(DateTime CheckDate)
        {
            try
            {
                return (YMCARET.YmcaDataAccessObject.MonthlyPayrollDAClass.SetPaymentOutsourcingKey(CheckDate));
            }
            catch
            {
                throw;
            }

        }
        //END: SR | 2020.01.08 | YRS-AT-4602 | Get Payment outsourcing key value from databse
        

	}
}

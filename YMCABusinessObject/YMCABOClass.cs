//****************************************************
//Modification History
//****************************************************
//Modified by           Date                Description
//****************************************************
// Paramesh K.			31-July-2008		Added method GetParticipantsHavingLoans() for sending email alert after merging YCMA
// NP/PP/SR       		2009.05.18      	Optimizing the YMCA Screen
// Priya				17-March-2010		YRS 5.0-1017:Display withdrawal date as merged date for branches
// Deven                02-Sep-2010         Added function GetYMCANoByGuiID to get the YMCA No by its Gui ID
//Priya					24-01-2011			BT-697 YRS 5.0-1235 : Allow termination of YMCA from active (skipping withdrawal)
//BhavnaS               2011.08.05          YRS 5.0-1380:BT:910 - here compute termination date to sort the  Resolution tab on SearchYMCAResolution() 
//Manthan Rajguru       2015.09.16          YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')s
//Manthan Rajguru       2016.02.03          YRS-AT-2334: Enhancement to YRS YMCA Maintenance-add a suspend participation option
//Chandra sekar         2016.07.12          YRS-AT-2772 - YRS enh: Waiver of Participation Tracking (TrackIT 24931) (dependency YERDI)
//****************************************************

using System;
using System.Data;
using YMCARET.YmcaDataAccessObject;

  /// <summary>
        /// To Get the Ymca relation managers record from master table
        /// </summary>
        /// <returns></returns>
//*******
namespace YMCARET.YmcaBusinessObject
{
	/// <summary>
	/// Summary description for YMCABusinessClass.
	/// </summary>
	public sealed class YMCABOClass
	{
		private YMCABOClass()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		//calling YMCADAClass function for populating List tab grid
		public static DataSet SearchYMCAList(string parameterSearchYMCANo,string parameterSearchYMCAName,string parametersearchYMCACity,string parameterSearchYMCAState)
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.YMCADAClass.SearchYMCAList(parameterSearchYMCANo,parameterSearchYMCAName,parametersearchYMCACity,parameterSearchYMCAState);
			}
			catch
			{
				throw;
			}

		}

		//Calling YMCADAClass Function for populating Controls of General Tab
		public static DataSet SearchYMCAGeneral(string parameterSearchYMCAGeneralGuiUniqueId)
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.YMCADAClass.SearchYMCAGeneral(parameterSearchYMCAGeneralGuiUniqueId);
			}
			catch
			{
				throw;
			}

		}

		//Calling YMCADAClass Function for populating Controls of Officer Tab
		public static DataSet SearchYMCAOfficer(string parameterSearchYMCAOfficerGuiUniqueId)
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.YMCADAClass.SearchYMCAOfficer(parameterSearchYMCAOfficerGuiUniqueId);
			}
			catch
			{
				throw;
			}

		}


		//Added by Dilip : 08-July-09
		//Calling YMCADAClass Function for populating dates at  WMT Tab
		public static DataSet SearchYMCAPayrolldate(string parameterSearchYMCAPayrolldateGuiUniqueId)
		{
			try
			{
			   return YMCARET.YmcaDataAccessObject.YMCADAClass.SearchYMCAPayrolldate(parameterSearchYMCAPayrolldateGuiUniqueId);
			}
			catch(Exception ex)
			{
				string a = ex.Message;
				throw ;
			}

		}

     
		//Calling YMCADAClass Function for populating Controls of Contact Tab
		public static DataSet SearchYMCAContact(string parameterSearchYMCAContactGuiUniqueId)
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.YMCADAClass.SearchYMCAContact(parameterSearchYMCAContactGuiUniqueId);
			}
			catch
			{
				throw;
			}

		}

		//Calling YMCADAClass Function for populating Controls of Resolution Tab
		public static DataSet SearchYMCAResolution(string parameterSearchYMCAResolutionGuiUniqueId)
		{
			try
			{
                //BS:2011.08.05:YRS 5.0-1380:BT:910 - here compute termination date to sort the  Resolutions tab
                DataSet ds = YMCARET.YmcaDataAccessObject.YMCADAClass.SearchYMCAResolution(parameterSearchYMCAResolutionGuiUniqueId);
                DataColumn dc = new DataColumn("EffectiveTerminationDate", typeof(DateTime));
                dc.Expression = "ISNULL([Term. Date],#"+Convert.ToString(DateTime.MaxValue)+"# )";
                ds.Tables[0].Columns.Add(dc);
                ds.AcceptChanges();
                return ds;
			}
			catch
			{
				throw;
			}

		}


		//Calling YMCADAClass Function for populating Controls of Branch Tab
		public static DataSet SearchYMCABranch(string parameterSearchYMCABranchGuiUniqueId)
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.YMCADAClass.SearchYMCABranch(parameterSearchYMCABranchGuiUniqueId);
			}
			catch
			{
				throw;
			}

		}

		//Calling YMCADAClass Function for populating Controls of BankInfo Tab
		public static DataSet SearchYMCABankInfo(string parameterSearchYMCABAnkInfoGuiUniqueId)
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.YMCADAClass.SearchYMCABankInfo(parameterSearchYMCABAnkInfoGuiUniqueId);
			}
			catch
			{
				throw;
			}

		}


		//Calling YMCADAClass Function for populating Controls of Note Tab
		public static DataSet SearchYMCANote(string parameterSearchYMCANoteGuiUniqueId)
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.YMCADAClass.SearchYMCANotes(parameterSearchYMCANoteGuiUniqueId);
			}
			catch
			{
				throw;
			}

		}

		public static void SaveYMCAGeneral(DataSet parameterInsertYMCAGeneral)
		{
			try
			{
				YMCARET.YmcaDataAccessObject.YMCADAClass.SaveYMCAGeneral(parameterInsertYMCAGeneral);
			}
			catch
			{
				throw;
			}
		}


		//****Method Added by ashutosh on 09-june-06****
		public static string GetNewGuid()
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.InterestProcessingDAClass.GetNewGuidString();
			}
			catch 
			{
				throw;
			}
		}
		//*************************************
		public static void InsertYMCAResolution(DataSet parameterInsertYMCAResolution)
		{
			try
			{
				YMCARET.YmcaDataAccessObject.YMCADAClass.InsertYMCAResolution(parameterInsertYMCAResolution);
			}
			catch
			{
				throw;
			}
		}
		

		public static void InsertYMCABankInfo(DataSet parameterInsertYMCABankInfo)
		{
			try
			{
				YMCARET.YmcaDataAccessObject.YMCADAClass.InsertYMCABankInfo(parameterInsertYMCABankInfo);
			}
			catch
			{
				throw;
			}
		}


		public static void SaveYMCAOfficers(DataSet parameterInsertYMCAOfficer)
		{
			try
			{
				YMCARET.YmcaDataAccessObject.YMCADAClass.SaveYMCAOfficers(parameterInsertYMCAOfficer);
			}
			catch
			{
				throw;
			}
		}


		public static void SaveYMCAContacts(DataSet parameterInsertYMCAContact)
		{
			try
			{
				YMCARET.YmcaDataAccessObject.YMCADAClass.SaveYMCAContacts(parameterInsertYMCAContact);
			}
			catch
			{
				throw;
			}
		}
		
		public static DataSet LookUpGeneralPaymentMethod()
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.YMCADAClass.LookUpGeneralPaymentMethod();
			}
			catch
			{
				throw;
			}
		}


		public static DataSet LookUpGeneralHubType()
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.YMCADAClass.LookUpGeneralHubType();
			}
			catch
			{
				throw;
			}
		}

		

		public static void UpdateOnTerminationOfYmca(DateTime YMCATerminationDate,string GuiYmcaId,int g_integer_update,char bitAllowYerdiAccess)//Swopna Phase IV
		{
			try
			{
				YMCARET.YmcaDataAccessObject.YMCADAClass.UpdateOnTerminationOfYmca(YMCATerminationDate,GuiYmcaId,g_integer_update,bitAllowYerdiAccess);
			}
			catch
			{
				throw;
			}
		}

		public static DataSet GetStatus(string parameterUniqueId)//Swopna
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.YMCADAClass.GetStatus(parameterUniqueId);
			}
			catch
			{
				throw;
			}
		}
		public static DataSet GetTotalActiveParticipants(string parameterUniqueId)//Swopna
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.YMCADAClass.GetTotalActiveParticipants(parameterUniqueId);
			}
			catch
			{
				throw;
			}
		}
		//'Added---Swopna 20June,2008-------Start
		public static DataSet GetParticipantStatusOnWithdrawal(string GuiYmcaId,DateTime YMCAWithdrawalDate,DateTime updatedDate)
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.YMCADAClass.GetParticipantStatusOnWithdrawal(GuiYmcaId,YMCAWithdrawalDate,updatedDate); // Manthan Rajguru | 2016.02.15 | YRS-AT-2334 & 1686 | Parameter passed to get update date
			}
			catch
			{
				throw;
			}
		}
		//'Added---Swopna 20June,2008-------End
		
		//Priya:17-March-2010:YRS 5.0-1017:Display withdrawal date as merged date for branches
		//Added parameter to pass metro id to get details of metroymca
    
		public static DataSet GetMetroYMCAs(string strMetroYMCAID)//Swopna
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.YMCADAClass.GetMetroYMCAs(strMetroYMCAID);
			}
			catch
			{
				throw;
			}
		}

		
		/// <summary>
		/// Created By : Paramesh K.
		/// Created On : July 31st 2008
		/// This Method will return details of the participants for sending mails after Merging YMCA
		/// </summary>
		/// <returns></returns>
		public static DataSet GetParticipantsHavingOpenLoans(string GuiYmcaId,string GuiYmcaMetroID)
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.YMCADAClass.GetParticipantsHavingOpenLoans(GuiYmcaId,GuiYmcaMetroID);
			}
			catch
			{
				throw;
			}
		}

        //Start - Manthan Rajguru | 2016.02.03 | YRS-AT-2334 | Added new parameter for Effective Merge Date
		public static void UpdateOnMergingYMCA(DateTime YMCAMergeDate,string GuiYmcaId,string GuiYmcaMetroID, DateTime YMCAEffectiveMergeDate)//Swopna Phase IV
		{
			try
			{
				YMCARET.YmcaDataAccessObject.YMCADAClass.UpdateOnMergingYMCA(YMCAMergeDate,GuiYmcaId,GuiYmcaMetroID, YMCAEffectiveMergeDate);
			}
			catch
			{
				throw;
			}
		}

        //Start - Manthan Rajguru | 2016.02.03 | YRS-AT-2334 | Calling YMCADAClass Function for suspendingYMCA and added parameter to check activity performed at WTSM Tab
        //public static void UpdateOnSuspendingYMCA(DateTime YMCAWithdrawalDate, string GuiYmcaId, char bitAllowYerdiAccess, char type, DateTime YMCASuspendEffectiveDate)//Manthan Rajguru | 2016.02.03 | YRS-AT-2334 | 
        public static DateTime? UpdateOnSuspendingYMCA(DateTime YMCAWithdrawalDate, string GuiYmcaId, char bitAllowYerdiAccess, char type, DateTime YMCASuspendEffectiveDate)//Manthan Rajguru | 2016.02.03 | YRS-AT-2334 & 1686 | 
        {
            try
            {
                //YMCARET.YmcaDataAccessObject.YMCADAClass.UpdateOnSuspendingYMCA(YMCAWithdrawalDate,GuiYmcaId, bitAllowYerdiAccess,type,YMCASuspendEffectiveDate);
                return YMCARET.YmcaDataAccessObject.YMCADAClass.UpdateOnSuspendingYMCA(YMCAWithdrawalDate, GuiYmcaId, bitAllowYerdiAccess, type, YMCASuspendEffectiveDate); //Manthan Rajguru | 2016.02.03 | YRS-AT-2334 & 1686
            }
            catch
            {
                throw;
            }

        }
         //End - Manthan Rajguru | 2016.02.03 | YRS-AT-2334 | Calling YMCADAClass Function for suspendingYMCA and added parameter to check activity performed at WTSM Tab

        //public static void UpdateOnWithdrawingYMCA(DateTime YMCAWithdrawalDate, string GuiYmcaId, char bitAllowYerdiAccess, char type, DateTime YMCAWithdrawalEffectiveDate)//Swopna Phase IV--//Manthan Rajguru | 2016.02.03 | YRS-AT-2334 | Added parameter to pass activity performed
        public static DateTime? UpdateOnWithdrawingYMCA(DateTime YMCAWithdrawalDate, string GuiYmcaId, char bitAllowYerdiAccess, char type, DateTime YMCAWithdrawalEffectiveDate)//Manthan Rajguru | 2016.02.03 | YRS-AT-2334 & 1686
		{
			try
			{
				//YMCARET.YmcaDataAccessObject.YMCADAClass.UpdateOnWithdrawingYMCA(YMCAWithdrawalDate,GuiYmcaId,bitAllowYerdiAccess,type,YMCAWithdrawalEffectiveDate);
                return YMCARET.YmcaDataAccessObject.YMCADAClass.UpdateOnWithdrawingYMCA(YMCAWithdrawalDate, GuiYmcaId, bitAllowYerdiAccess, type, YMCAWithdrawalEffectiveDate); //Manthan Rajguru | 2016.02.03 | YRS-AT-2334 & 1686
            }
			catch
			{
				throw;
			}
		}
		//calling YMCADAClass function for to check presence of transmittals before withdrawal/termination process(26May08 Swopna)
		public static DataSet SearchTransmittals(string parameterYMCAId,DateTime YMCAWithdrawalDate)
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.YMCADAClass.SearchTransmittals(parameterYMCAId,YMCAWithdrawalDate);
			}
			catch
			{
				throw;
			}

		}
		public static void UpdateOnReActivatingYMCA(DateTime YMCAReactivationDate,string DefaultPaymentCode,string BillingMethodCode,string GuiYmcaId,int YMCA_Status,DateTime YMCAWithdrawalDate)//Swopna Phase IV
		{
			try
			{
				YMCARET.YmcaDataAccessObject.YMCADAClass.UpdateOnReActivatingYMCA(YMCAReactivationDate,DefaultPaymentCode,BillingMethodCode,GuiYmcaId,YMCA_Status,YMCAWithdrawalDate);
			}
			catch
			{
				throw;
			}
		}


		public static void SaveYMCAGeneralAddressInformation(DataSet ds) {
			YMCARET.YmcaDataAccessObject.YMCADAClass.SaveYMCAGeneralAddressInformation(ds);
		}

		public static void SaveYMCAGeneralTelephoneInformation(DataSet ds) 
		{
			YMCARET.YmcaDataAccessObject.YMCADAClass.SaveYMCAGeneralTelephoneInformation(ds);
		}
		public static void SaveYMCANotes(DataSet ds)
		{
			YMCARET.YmcaDataAccessObject.YMCADAClass.SaveYMCANotes(ds);
		}
		public static void SaveYMCAGeneralEmailInformation(DataSet ds) 
		{
			YMCARET.YmcaDataAccessObject.YMCADAClass.SaveYMCAGeneralEmailInformation(ds);
		}
		//Added by sanjay
		public static string SaveYMCAOutputFileIDMTrackingLogs(DataSet ds) 
		{
			return YMCARET.YmcaDataAccessObject.YMCADAClass.SaveYMCAOutputFileIDMTrackingLogs(ds);
		}

		//Added by sanjay
		public static void UpdateYMCAOutputFileIDMTrackingLogs( int pTrackingId ,bool pIdxFileCreated ,bool  pPdfFilecreated ,string IdxFileName,bool IdxCopyInitiated,bool IdxCopied,bool idxDeletedafterCopy,string PdfFileName,bool PdfCopyInitiated,bool PdfCopied,bool pdfDeletedafterCopy,char FileType,string ErrorMsg)
		{
			YMCARET.YmcaDataAccessObject.YMCADAClass.UpdateYMCAOutputFileIDMTrackingLogs(pTrackingId,pIdxFileCreated,pPdfFilecreated,IdxFileName,IdxCopyInitiated,IdxCopied,idxDeletedafterCopy,PdfFileName,PdfCopyInitiated,PdfCopied,pdfDeletedafterCopy,FileType,ErrorMsg);
		}

        // Deven 02-Sep-2010
        public static DataTable GetYMCANoByGuiID(string parameterguiYMCAID)
        {
            try
            {
                return (YMCADAClass.GetYMCANoByGuiID(parameterguiYMCAID));
            }
            catch
            {
                throw;
            }
        }
		////Priya 24-01-2011: BT-697 YRS 5.0-1235 : Allow termination of YMCA from active (skipping withdrawal)
		public static DataSet GetTotalBeforeTerminateParticipants(string parameterUniqueId)//Swopna
			{
			try
				{
				return YMCARET.YmcaDataAccessObject.YMCADAClass.GetTotalBeforeTerminateParticipants(parameterUniqueId);
				}
			catch
				{
				throw;
				}
			}

		public static DataSet GetParticipantStatusOnTermination(string GuiYmcaId, DateTime YMCAWithdrawalDate, DateTime YMCAWithdrawnDate)
			{
			try
				{
				return YMCARET.YmcaDataAccessObject.YMCADAClass.GetParticipantStatusOnTermination(GuiYmcaId, YMCAWithdrawalDate,YMCAWithdrawnDate);
				}
			catch
				{
				throw;
				}
			}
	//End //Priya 24-01-2011: BT-697 YRS 5.0-1235 : Allow termination of YMCA from active (skipping withdrawal)
		//Showing summery grid for termination 


        /// <summary>
        /// To Get the Ymca relation managers record from master table
        /// </summary>
        /// <returns></returns>
        public static DataSet GetYMCARelationManagers()
        {
            try
            {
                return YMCARET.YmcaDataAccessObject.YMCADAClass.GetYMCARelationManagers();
            }
            catch
            {
                throw;
            }

        }
        
        //START- Chandra sekar - 2016.07.12 - YRS-AT-2772 - YRS enh: Waiver of Participation Tracking (TrackIT 24931) (dependency YERDI)
        public static DataSet GetListOfWaivedParticipants(string guiYmcaID)
        {
            try
            {
                return YMCARET.YmcaDataAccessObject.YMCADAClass.GetListOfWaivedParticipants(guiYmcaID);
            }
            catch
            {
                throw;
            }
        }
        //END- Chandra sekar - 2016.07.12 - YRS-AT-2772 - YRS enh: Waiver of Participation Tracking (TrackIT 24931) (dependency YERDI)
	}
}

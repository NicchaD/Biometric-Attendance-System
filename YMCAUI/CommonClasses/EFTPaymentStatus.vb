'************************************************************************************************************************/
' Author:                   : Manthan Rajguru
' Created on:               : 04/18/2018
' Summary of Functionality  : Provides available EFT disbursement payment statuses 
' Declared in Version       : 20.6.0 | YRS-AT-3101 - YRS enh: EFT: Loans ( FIRST EFT PROJECT) - updates for "Payment Manager" (TrackIT 33024) 
'
'************************************************************************************************************************/
' REVISION HISTORY:
' ------------------------------------------------------------------------------------------------------
' Developer Name              | Date         | Version No      | Ticket
' ------------------------------------------------------------------------------------------------------
' 			                  | 	         |		           | 
' ------------------------------------------------------------------------------------------------------
'************************************************************************************************************************/

''' <summary>
''' Added structure type for EFT payment status which can be used in required modules for checking EFT payment status instead of hardcoded.
''' </summary>
''' <remarks></remarks>
Public Structure EFTPaymentStatus
    Public Shared PENDING As String = "PENDING" 'Initial stage not picked for processing
    Public Shared PROOF As String = "PROOF" 'EFT upload file is prepared 
    Public Shared APPROVED As String = "APPROVED" 'Bank approved the disbursement (bank details are correct)
    Public Shared REJECTED As String = "REJECTED" 'Bank rejected the disbursement (bank details are incorrect)
End Structure
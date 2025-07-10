'************************************************************************************************************************/
' Author:                   : Manthan Rajguru
' Created on:               : 04/11/2018
' Summary of Functionality  : Provides Loan Statuses (WEB/YRS)
' Declared in Version       : 20.6.0 | YRS-AT-4017 -  YRS enh: Loan EFT Project: "new" Loan Admin webpages (approve/decline/process) 
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
''' Structure type for loan statuses which can be used in loan modules for checking loan status instead of hardcoded
''' </summary>
''' <remarks></remarks>
Public Structure LoanStatus
    Public Shared ACCEPTED As String = "ACCEPTED"
    Public Shared APPROVED As String = "APPROVED"
    Public Shared CANCEL As String = "CANCEL"
    Public Shared DECLINED As String = "DECLINED"
    Public Shared EXP As String = "EXP"
    Public Shared PAID As String = "PAID"
    Public Shared PEND As String = "PEND"
    Public Shared REJECTED As String = "REJECTED"
    Public Shared WITHDRAWN As String = "WITHDRAWN"
End Structure

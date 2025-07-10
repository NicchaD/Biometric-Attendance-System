'************************************************************************************************************************/
' Author                    : Pramod Prakash Pokale
' Created on                : 03/14/2017
' Summary of Functionality  : Contains functions which are commn in between Retirement Estimation and Processing
' Declared in Version       : 20.0.3 | YRS-AT-2625 -  YRS enh: change in disability Annuity calculations (TrackIT 24012) 
'
'************************************************************************************************************************/
' REVISION HISTORY:
' ------------------------------------------------------------------------------------------------------
' Developer Name              | Date         | Version No      | Ticket
' ------------------------------------------------------------------------------------------------------
' 			                  |              |		           | 
'------------------------------------------------------------------------------------------------------

Public Class Retirement

    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub LogDetails(ByVal action As String, ByVal FundNo As String, ByVal source As YMCAObjects.EntityTypes, ByVal moduleName As String, ByVal logData As String)
        Dim logEntry As YMCAObjects.YMCAActionEntry
        Try
            logEntry = New YMCAObjects.YMCAActionEntry
            logEntry.Action = action
            logEntry.ActionBy = HttpContext.Current.Session("LoginId")
            logEntry.Data = logData
            logEntry.EntityId = FundNo
            logEntry.EntityType = source
            logEntry.Module = moduleName
            logEntry.SuccessStatus = True
            YMCARET.YmcaBusinessObject.LoggerBO.WriteLogDB(logEntry)
        Catch
            Throw
        Finally
            logEntry = Nothing
        End Try
    End Sub
End Class

'*******************************************************************************
' Copyright YMCA Retirement Fund All Rights Reserved. 
' Project Name		:	YMCA-YRS
' FileName			:	Helper.aspx.vb
' Author Name		:	Anudeep  
' Creation Date		:	05/27/2014
' Description		:	This form is used to common aspx pages to call any webmethods
'*******************************************************************************
'Modification History
'********************************************************************************************************************************
'Modified By        Date            Description
'********************************************************************************************************************************
'Shashank           09/01/2014      To implement enterprise library caching
'Manthan Rajguru    2015.09.16      YRS-AT-2550: YRS data cleanup: copyright comments and namespace/reference (Changed the Namespace/reference from 'Infotech' to 'YMCARET')
'*******************************************************************************

Imports System.Web.Services
Imports YMCAObjects

Public Class Helper
    Inherits System.Web.UI.Page

    'To get the institutions list with name and id from atsrolloverintstitutions
    <WebMethod()>
    Public Shared Function GetInstitutions(ByVal name As String) As String
        Dim dsRolloverInstitutions As DataSet
        Dim lstInstitutions As New List(Of Dictionary(Of String, String))
        Dim js As New Script.Serialization.JavaScriptSerializer()
        Dim strJson As String
        Dim drRolloverInstitutions As DataRow()
        Try
            'dsRolloverInstitutions = ApplicationCache.GetCacheValue("RolloverInstitutions") 'SP 2014.09.01 
            dsRolloverInstitutions = CacheManager.GetDataFromCache("RolloverInstitutions") 'SP 2014.09.01 
            If HelperFunctions.isEmpty(dsRolloverInstitutions) Then
                dsRolloverInstitutions = YMCARET.YmcaBusinessObject.RolloversBOClass.GetRolloverInstitutions()
                'ApplicationCache.AddCache("RolloverInstitutions", dsRolloverInstitutions, ApplicationCache.CachePeriod.Short)'SP 2014.09.01 
                CacheManager.AddCache("RolloverInstitutions", dsRolloverInstitutions, CacheManager.CachePeriod.Short) 'SP 2014.09.01 
            End If
            If HelperFunctions.isNonEmpty(dsRolloverInstitutions) Then
                drRolloverInstitutions = dsRolloverInstitutions.Tables(0).Select("InstitutionName LIKE '" + name + "%'")
                For Each dr As DataRow In drRolloverInstitutions
                    lstInstitutions.Add(New Dictionary(Of String, String)() From {{"instName", dr("InstitutionName").ToString().Trim}})
                Next
            End If
            strJson = js.Serialize(lstInstitutions)
            Return strJson
        Catch ex As Exception
            Throw ex
        End Try
    End Function
End Class
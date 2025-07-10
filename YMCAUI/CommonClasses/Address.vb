'*******************************************************************************
' Copyright YMCA Retirement Fund All Rights Reserved. 
'
' Project Name		:	YMCA-YRS
' FileName			:	AddressFunction.vb
' Author Name		:	Anudeep Adusumilli
' Employee ID		:	56556
' Email				:	anudeep.adusmilli@3i-infotech.com
' Contact No		:	-
' Creation Time		:	30-May-2013
'*******************************************************************************

'************************************************************************************
'Modified By          Date            Description
'*********************************************************************************************************************
''Anudeep            10-Jul-2013     BT-1501:YRS 5.0-1745:Capture Beneficiary addresses 
''Anudeep            25-Sep-2013     BT-1501:YRS 5.0-1745:Capture Beneficiary addresses 
''Anudeep            04.Jul.2014     BT-1051:YRS 5.0-1618 :Enhancements to Roll In process
''Manthan Rajguru    2015.09.16      YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
'*********************************************************************************************************************
Imports System.Data
Imports System.IO
Imports System.Text.RegularExpressions
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports Microsoft.Practices.EnterpriseLibrary.Logging

Public Class Address
    'To get the address with respect to entityId 
    Public Shared Function GetAddressByEntity(ByVal strEntityID As String, ByVal enumEntityCode As EnumEntityCode) As DataSet
        Dim dsAddress As New DataSet
        Try
            HelperFunctions.LogMessage("Address - Get Address by entity for entityID = '" + strEntityID + "' started")
            dsAddress = YMCARET.YmcaBusinessObject.AddressBOClass.GetAddressByEntity(strEntityID, enumEntityCode.ToString())
            HelperFunctions.LogMessage("Address - Get Address by entity for entityID = '" + strEntityID + "' completed")
            Return dsAddress
        Catch ex As Exception
            Throw
        End Try
    End Function
    'To save the address 
    Public Shared Function SaveAddress(ByVal dtAddress As DataTable) As String
        Dim dsAddress As New DataSet
        Dim intReturn As Integer
        Dim strNewAddressID As String
        Try
            HelperFunctions.LogMessage("Address - Save address started")
            dsAddress.Tables.Add(dtAddress.Copy)
            dsAddress.Tables(0).TableName = "Address"
            If Not dsAddress.GetChanges Is Nothing Then
                strNewAddressID = YMCARET.YmcaBusinessObject.AddressBOClass.SaveAddress(dsAddress)
                dsAddress.AcceptChanges()
            End If
            Return strNewAddressID
            HelperFunctions.LogMessage("Address - Save address completed")
        Catch ex As Exception
            Throw
        End Try
    End Function
    'To Get the address of beneficiaries For a participant
    Public Shared Function GetAddressOfBeneficiariesByPerson(ByVal strPersID As String, ByVal enumEntityCode As EnumEntityCode) As DataSet
        Dim dsAddress As New DataSet
        Try
            HelperFunctions.LogMessage("Address - Get Address for Beneficiaries started")
            dsAddress = YMCARET.YmcaBusinessObject.AddressBOClass.GetAddessForBeneficiariesOfPerson(strPersID, enumEntityCode.ToString())
            HelperFunctions.LogMessage("Address - Get Address for Beneficiaries completed")
            Return dsAddress
        Catch ex As Exception
            Throw
        End Try
    End Function
    Public Shared Function GetBeneficiariesAddress(ByVal strPersID As String, ByVal strSSNO As String, ByVal strFirstname As String, ByVal strLastname As String) As DataSet
        Dim dsAddress As New DataSet
        Try
            HelperFunctions.LogMessage("Get beneficiary address ")
            dsAddress = YMCARET.YmcaBusinessObject.AddressBOClass.GetBeneficiariesAddress(strPersID, strSSNO, strFirstname, strLastname)
            Return dsAddress
        Catch ex As Exception
            Throw
        End Try
    End Function
    Public Shared Function SaveAddressOfBeneficiariesByPerson(ByVal dtAddress As DataTable)
        Dim dsAddress As New DataSet
        Try
            HelperFunctions.LogMessage("Address - Save Address for Beneficiaries started")
            dsAddress.Tables.Add(dtAddress.Copy)
            dsAddress.Tables(0).TableName = "Address"
            If Not dsAddress.GetChanges Is Nothing Then
                YMCARET.YmcaBusinessObject.AddressBOClass.SaveAddressOfBeneficiariesByPerson(dsAddress)
                dsAddress.AcceptChanges()
            End If
            HelperFunctions.LogMessage("Address - Save Address for Beneficiaries completed")
        Catch ex As Exception
            Throw
        End Try
    End Function
    'An empty datatable 
    Public Shared Function CreateAddressDatatable() As DataTable
        Dim dtAddress As New DataTable("Address")
        dtAddress.Columns.Add("UniqueId")
        dtAddress.Columns.Add("guiEntityId")
        dtAddress.Columns.Add("addr1")
        dtAddress.Columns.Add("addr2")
        dtAddress.Columns.Add("addr3")
        dtAddress.Columns.Add("city")
        dtAddress.Columns.Add("state")
        dtAddress.Columns.Add("CountryName")
        dtAddress.Columns.Add("StateName")
        dtAddress.Columns.Add("zipCode")
        dtAddress.Columns.Add("country")
        dtAddress.Columns.Add("isActive")
        dtAddress.Columns.Add("isPrimary")
        dtAddress.Columns.Add("effectiveDate")
        dtAddress.Columns.Add("isBadAddress")
        dtAddress.Columns.Add("addrCode")
        dtAddress.Columns.Add("entityCode")
        dtAddress.Columns.Add("Note")
        dtAddress.Columns.Add("bitImportant")
        dtAddress.Columns.Add("BenSSNo")
        dtAddress.Columns.Add("PersID")
        Return dtAddress
    End Function

    Public Shared Function GetAddressForYMCA(ByVal guiYmcaID As String) As DataSet
        Try
            HelperFunctions.LogMessage("Address - Get Address for Ymca for ymcaID = '" + guiYmcaID + "'")
            Return YMCARET.YmcaBusinessObject.AddressBOClass.GetAddressForYMCA(guiYmcaID)
        Catch ex As Exception
            Throw
        End Try

    End Function

    Public Shared Function GetCountryList() As DataSet
        Try
            HelperFunctions.LogMessage("Address - Get Country lists")
            Return YMCARET.YmcaBusinessObject.AddressBOClass.GetCountryList()
        Catch ex As Exception
            Throw
        End Try

    End Function

    Public Shared Function GetState() As DataSet
        Try
            Return YMCARET.YmcaBusinessObject.AddressBOClass.GetState()
        Catch ex As Exception
            Throw
        End Try

    End Function
    'AA:2013.09.24:BT-1501:Handling the getting reason from address class and getting reasons for beneficiary address change
    Public Shared Function GetAddressNotesReasonSource(ByVal stringNotesSourceReason As String, ByVal stringNotesSourceFor As String) As DataSet
        Try
            Return YMCARET.YmcaBusinessObject.AddressBOClass.GetAddressNotesReasonSource(stringNotesSourceReason, stringNotesSourceFor)
        Catch ex As Exception
            Throw
        End Try

    End Function
    Public Shared Function SaveAddressForYMCA(ByVal ds As DataSet) As DataSet
        Try
            YMCARET.YmcaBusinessObject.AddressBOClass.SaveAddressForYMCA(ds)
        Catch
            Throw
        End Try
    End Function

    'Start: Anudeep:04.07.2014: BT-1051:YRS 5.0-1618 : Added to save the institution address 
    Public Shared Function SaveInstitutionAddress(ByVal dtAddress As DataTable) As String
        Dim dsAddress As New DataSet
        Dim intReturn As Integer
        Dim strNewAddressID As String
        Try
            HelperFunctions.LogMessage("Address - Save address started")
            dsAddress.Tables.Add(dtAddress.Copy)
            dsAddress.Tables(0).TableName = "Address"
            If Not dsAddress.GetChanges Is Nothing Then
                strNewAddressID = YMCARET.YmcaBusinessObject.AddressBOClass.SaveInstitutionAddress(dsAddress)
                dsAddress.AcceptChanges()
            End If
            Return strNewAddressID
            HelperFunctions.LogMessage("Address - Save address completed")
        Catch ex As Exception
            Throw
        End Try
    End Function
    'End: Anudeep:04.07.2014: BT-1051:YRS 5.0-1618 : Added to save the institution address 
End Class

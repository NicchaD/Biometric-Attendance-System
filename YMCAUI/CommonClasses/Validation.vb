'*******************************************************************************************************************************
' Copyright YMCA Retirement Fund All Rights Reserved. 
'
' Project Name		:	YMCA-YRS
' FileName			:	Validation.vb
' Author Name		:	Pramod Prakash Pokale
' Employee ID		:	
' Email				:	
' Extn      		:	
' Creation Date		:	10/10/2015
' Purpose           :   Validates various objects

'********************************************************************************************************************************
'Modification History
'********************************************************************************************************************************
'Modified By        Date            Desription
'********************************************************************************************************************************
'Chandra C          2016.02.18      YRS-AT-1483: YRS enh: Withdrawals Phase2: sprint 2 - Move web methods for address updates into YRSwebService, (GetAddressPhone, UpdateAddressPhone add new Lock Address validation)
'Manthan Rajguru    2016.07.27      YRS-AT-2560 -  YRS enhancement-make SSN for beneficiaries an optional field. 
'Santosh Bura       2017.07.28      YRS-AT-3324 -  YRS enh: Restrict withdrawals and death for 70 1/2 & older if RMDs not yet generated(TrackIT 28857) MORE DETAILS NEEDED
'Santosh Bura       2018.01.11      YRS-AT-3324 -  YRS enh: Restrict withdrawals and death for 70 1/2 & older if RMDs not yet generated(TrackIT 28857) MORE DETAILS NEEDED
'********************************************************************************************************************************

''' <summary>
''' Validates various objects
''' </summary>
''' <remarks></remarks>
Public Class Validation

    ''' <summary>
    ''' Validates telephone number
    ''' </summary>
    ''' <param name="number">Telephone number</param>
    ''' <param name="type">Type of telephone number (Home, Work..)</param>
    ''' <returns>List of errors if telephone number is invalid</returns>
    ''' <remarks></remarks>
    Public Shared Function Telephone(ByVal number As String, ByVal type As YMCAObjects.TelephoneType) As String
        Dim bIsValid As Boolean
        Dim stErrorMessage As String
        Try
            stErrorMessage = String.Empty
            bIsValid = True

            'START: CC | 2016.02.18 | YRS-AT-1483 | Changed the namespace
            'bIsValid = CommonUtilities.Validation.Telephone(number)
            bIsValid = YMCARET.CommonUtilities.Validation.Telephone(number)
            'END: CC | 2016.02.18 | YRS-AT-1483 | Changed the namespace

            If (Not bIsValid) Then
                Select Case type
                    Case YMCAObjects.TelephoneType.Office
                        stErrorMessage = YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.MESSAGE_GEN_OFFICE_TELEPHONE_ERROR).DisplayText
                    Case YMCAObjects.TelephoneType.Home
                        stErrorMessage = YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.MESSAGE_GEN_HOME_TELEPHONE_ERROR).DisplayText
                    Case YMCAObjects.TelephoneType.Mobile
                        stErrorMessage = YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.MESSAGE_GEN_MOBILE_TELEPHONE_ERROR).DisplayText
                    Case YMCAObjects.TelephoneType.Fax
                        stErrorMessage = YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.MESSAGE_GEN_FAX_TELEPHONE_ERROR).DisplayText
                    Case YMCAObjects.TelephoneType.Work
                        stErrorMessage = YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.MESSAGE_GEN_WORK_TELEPHONE_ERROR).DisplayText
                    Case YMCAObjects.TelephoneType.Telephone
                        stErrorMessage = YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.MESSAGE_GEN_TELEPHONE_ERROR).DisplayText
                    Case YMCAObjects.TelephoneType.PhoneNumber
                        stErrorMessage = YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.MESSAGE_GEN_PHONENUMBER_ERROR).DisplayText
                    Case Else

                End Select
            End If
            Return stErrorMessage
        Catch ex As Exception
            Throw
        Finally
            stErrorMessage = Nothing
        End Try
    End Function
    'Start - Manthan Rajguru | 2016.07.27 | YRS-AT-2560 | Added method to validate existing phony SSN
    Public Shared Function IsValidPhonySSN(ByVal strPhonySSN As String, ByVal strBeneficiaryStatus As String, ByVal strBeneficiaryId As String, ByVal strBeneficiaryType As String) As Boolean
        Dim bIsValidPhonySSN As Boolean
        Try
            bIsValidPhonySSN = YMCARET.YmcaBusinessObject.LookupsMaintenanceBOClass.IsValidPhonySSN(strPhonySSN, strBeneficiaryStatus, strBeneficiaryId, strBeneficiaryType)
            Return bIsValidPhonySSN
        Catch ex As Exception
            Throw
        End Try
    End Function
    'End - Manthan Rajguru | 2016.07.27 | YRS-AT-2560 | Added method to validate existing phony SSN

    'START: SB | 2017.07.28 | YRS-AT-3324 | Added function to check if RMD need's to be generate/regenerate when processing withdrawal, death notification and death settlement
    'START: SB | 2018.01.09 | YRS-AT-3324 | 20.4.3 | Added additional parameter to function for checking deceased participants mrd records uptill deceased year
    'Public Shared Function IsRMDExist(ByVal moduleName As String, ByVal fundEventID As String, ByRef reasonForRestriction As String) As Boolean
    Public Shared Function IsRMDExist(ByVal moduleName As String, ByVal fundEventID As String, ByRef reasonForRestriction As String, Optional ByVal deceasedDate As String = Nothing) As Boolean
        'END: SB | 2018.01.09 | YRS-AT-3324 | 20.4.3 | Added additional parameter to function for checking deceased participants mrd records uptill deceased year
        Dim isAllowedToProceed As Boolean
        Dim dsErrorNos As DataSet
        Dim message As String
        Dim datarow As DataRow
        Try
            isAllowedToProceed = True
            reasonForRestriction = String.Empty
            'START: SB | 2018.01.03 | YRS-AT-3324 | 20.4.3 | Added additional parameter to function for checking deceased participants mrd records uptill deceased year 
            'dsErrorNos = YMCARET.YmcaBusinessObject.MRDBO.IsRMDGenerationRequired(moduleName, fundEventID)
            dsErrorNos = YMCARET.YmcaBusinessObject.MRDBO.IsRMDGenerationRequired(moduleName, fundEventID, deceasedDate)
            'END: SB | 2018.01.03 | YRS-AT-3324 | 20.4.3 | Added additional parameter to function for checking deceased participants mrd records uptill deceased year 

            If HelperFunctions.isNonEmpty(dsErrorNos) Then
                If (dsErrorNos.Tables(0).Rows.Count = 1) Then
                    If Not Convert.IsDBNull(dsErrorNos.Tables(0).Rows(0)("MessageNo")) Then
                        reasonForRestriction = YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(Convert.ToInt32(dsErrorNos.Tables(0).Rows(0)("MessageNo"))).DisplayText
                    End If

                    If Not String.IsNullOrEmpty(reasonForRestriction) Then
                        isAllowedToProceed = False
                    End If
                Else
                    'If restriction is due to more than one reason then concate each reason one after the another
                    reasonForRestriction = String.Format("{0} cannot be performed due to following  ", moduleName)
                    For Each datarow In dsErrorNos.Tables(0).Rows
                        message = String.Empty
                        If Not Convert.IsDBNull(datarow("MessageNo")) Then
                            message = YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(Convert.ToInt32(datarow("MessageNo"))).DisplayText
                        End If

                        If Not String.IsNullOrEmpty(message) Then
                            reasonForRestriction = String.Format("{0}<br />-{1}", reasonForRestriction, message)
                        End If

                    Next
                    isAllowedToProceed = False
                End If
            End If
            Return isAllowedToProceed
        Catch
            Throw
        Finally
            message = Nothing
            datarow = Nothing
            dsErrorNos = Nothing
        End Try
    End Function
    'END: SB | 2017.07.28 | YRS-AT-3324 | Added function to check if RMD need's to be generate/regenerate when processing withdrawal, death notification and death settlement


End Class

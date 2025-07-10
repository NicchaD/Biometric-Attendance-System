'************************************************************************************************************************
' Author                    : Pramod Prakash Pokale
' Created on                : 04/28/2017
' Summary of Functionality  : Helps to encrypt provided dictionary
' Declared in Version       : 20.2.0 | YRS-AT-3356 -  YRS enh: due MAY 2017 - RMD Print Letters screen-Changes to Initial & Follow-up screen. (1 of 3 tickets) (TrackIT 29186) 
'
'************************************************************************************************************************
' REVISION HISTORY:
'------------------------------------------------------------------------------------------------------
' Developer Name                | Date      | Version No    | Ticket
'------------------------------------------------------------------------------------------------------
' 			                    | 	        |		        | 
'------------------------------------------------------------------------------------------------------
'************************************************************************************************************************

Imports System.Security.Cryptography

Public Class EncryptedQueryString
    Inherits System.Collections.Specialized.StringDictionary

    Public Sub New()
        ' Empty constructor
    End Sub

    Public Sub New(ByVal encryptedData As String)
        Dim rawData As Byte()
        Dim stringData As String
        Dim index As Integer
        Dim splittedData As String()
        Try
            If (Not String.IsNullOrEmpty(encryptedData)) Then
                rawData = HexEncodingGetBytes(encryptedData)
                stringData = DecryptInputString(rawData)
                splittedData = stringData.Split(New Char() {"&"})
                For Each singleData As String In splittedData
                    index = singleData.IndexOf("=")
                    MyBase.Add(
                        HttpUtility.UrlDecode(singleData.Substring(0, index)),
                        HttpUtility.UrlDecode(singleData.Substring(index + 1))
                        )
                Next
            End If
        Catch
            Throw
        Finally
            splittedData = Nothing
            stringData = Nothing
            rawData = Nothing
        End Try
    End Sub

    Public Overrides Function ToString() As String
        Dim content As StringBuilder = New StringBuilder
        Dim encryptedData As Byte()
        ' HEX-encoded string
        ' Go through the contents and build a
        ' typical query string

        For Each key As String In Me.Keys
            content.Append(HttpUtility.UrlEncode(key))
            content.Append("=")
            content.Append(HttpUtility.UrlEncode(Me(key)))
            content.Append("&")
        Next
        ' Remove the last '&'
        content.Remove(content.Length - 1, 1)
        encryptedData = EncryptInputString(content.ToString())
        Return HexEncodingGetString(encryptedData)
    End Function

#Region "DESCryptoServiceProvider"
    Private Shared Function EncryptInputString(ByVal plainText As String) As Byte()
        Dim des As DESCryptoServiceProvider
        Dim ms As System.IO.MemoryStream
        Dim cs As CryptoStream
        Dim key, inputByteArray As Byte()
        Dim iv As Byte() = {18, 52, 86, 120, 144, 171, 205, 239}
        Try
            key = Encoding.UTF8.GetBytes("12345678")
            des = New DESCryptoServiceProvider()
            inputByteArray = Encoding.UTF8.GetBytes(plainText)
            ms = New IO.MemoryStream()
            cs = New CryptoStream(ms, des.CreateEncryptor(key, iv), CryptoStreamMode.Write)
            cs.Write(inputByteArray, 0, inputByteArray.Length)
            cs.FlushFinalBlock()

            Return ms.ToArray()
        Catch
            Throw
        Finally
            iv = Nothing
            key = Nothing
            inputByteArray = Nothing
            cs = Nothing
            ms = Nothing
            des = Nothing
        End Try
    End Function

    Private Shared Function DecryptInputString(ByVal encryptedBytes As Byte()) As String
        Dim des As DESCryptoServiceProvider
        Dim ms As System.IO.MemoryStream
        Dim cs As CryptoStream
        Dim key As Byte()
        Dim iv As Byte() = {18, 52, 86, 120, 144, 171, 205, 239}
        Dim en As Encoding
        Try
            key = Encoding.UTF8.GetBytes("12345678")
            des = New DESCryptoServiceProvider()
            ms = New IO.MemoryStream()
            cs = New CryptoStream(ms, des.CreateDecryptor(key, iv), CryptoStreamMode.Write)
            cs.Write(encryptedBytes, 0, encryptedBytes.Length)
            cs.FlushFinalBlock()

            en = Encoding.UTF8
            Return en.GetString(ms.ToArray())
        Catch
            Throw
        Finally
            en = Nothing
            iv = Nothing
            key = Nothing
            cs = Nothing
            ms = Nothing
            des = Nothing
        End Try
    End Function
#End Region

    Private Shared Function HexEncodingGetString(ByVal data As Byte()) As String
        Dim result As StringBuilder = New StringBuilder
        For Each b As Byte In data
            result.Append(b.ToString("X2"))
        Next
        Return result.ToString()
    End Function

    Private Shared Function HexEncodingGetBytes(ByVal data As String) As Byte()
        'GetString encodes the hex numbers with two digits
        Dim result As Byte() = New Byte((data.Length / 2) - 1) {}
        For i As Integer = 0 To data.Length - 1 Step 2
            result(i / 2) = Convert.ToByte(data.Substring(i, 2), 16)
        Next
        Return result
    End Function

    Public Function GetEncryptedURL(ByVal input As String) As String
        Dim query, outputValue As String
        Dim valueSplit, paramSplit As String()
        Try
            outputValue = String.Empty
            If (input.IndexOf("?") > -1) Then
                query = input.Substring(input.IndexOf("?"))
                If (Not String.IsNullOrEmpty(query) AndAlso query.Length > 0) Then
                    query = query.Substring(1)
                    valueSplit = query.Split("&")
                    For Each v As String In valueSplit
                        paramSplit = v.Split("=")
                        MyBase.Add(paramSplit(0), v.Replace(String.Concat(paramSplit(0), "="), ""))
                    Next
                End If
                outputValue = String.Concat(input.Replace(query, ""), "id=", Me.ToString())
            End If
            Return outputValue
        Catch
            Throw
        End Try
    End Function
End Class

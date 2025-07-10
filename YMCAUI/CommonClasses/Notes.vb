'*******************************************************************************
' Copyright YMCA Retirement Fund All Rights Reserved. 
'
' Project Name		:	YMCA-YRS
' FileName			:	Notes.vb
' Author Name		:	Bala
' Creation Time		:	01/12/2016
'*******************************************************************************

'************************************************************************************
'Modified By          Date            Description
'*********************************************************************************************************************
'*********************************************************************************************************************
Public Class NotesManagement

    ''' <summary>
    ''' Creates note at data source
    ''' </summary>
    ''' <param name="strEntityID"></param>
    ''' <param name="strNotes"></param>
    ''' <param name="bitImportant"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function InsertNotes(ByVal strEntityID As String, ByVal strNotes As String, ByVal bitImportant As Boolean)
        Try
            HelperFunctions.LogMessage("Notes - Insert notes started")
            If Not strEntityID Is Nothing Then
                YMCARET.YmcaBusinessObject.NotesBOClass.InsertNotes(strEntityID, strNotes, bitImportant)
            End If
            HelperFunctions.LogMessage("Notes - Insert notes completed")
        Catch ex As Exception
            Throw
        End Try
    End Function

    ''' <summary>
    ''' Deletes note from data source
    ''' </summary>
    ''' <param name="strUniqueID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function DeleteNotes(ByVal strUniqueID As String)
        Try
            HelperFunctions.LogMessage("Notes - Delete notes started")
            If Not strUniqueID Is Nothing Then
                YMCARET.YmcaBusinessObject.NotesBOClass.DeleteNotes(strUniqueID)
            End If
            HelperFunctions.LogMessage("Notes - Delete notes completed")
        Catch ex As Exception
            Throw
        End Try
    End Function

End Class

'*******************************************************************************
' Copyright YMCA Retirement Fund All Rights Reserved. 
'
' Project Name		:	YMCA-YRS
' FileName			:	HelperFunctions.vb
' Author Name		:	Anudeep Adusumilli    
' Employee ID		:	56556
' Creation Time		:	17-december-2013
'*******************************************************************************

'************************************************************************************
'Modified By        Date            Description
'*********************************************************************************************************************
'Anudeep            25.03.2014      BT:957: YRS 5.0-1484 -Termination watcher changes Re-Work (YRS 5.0-1484)
'*********************************************************************************************************************
'AA:25.03.2014 - Added to not get error for termination watcher while storing object in viewstate
<Serializable()>
Public Class GridViewCustomSort

    Dim strSortExpression As String
    Dim strSortDirection As String
    Public Property SortExpression As String
        Get
            Return strSortExpression
        End Get
        Set(ByVal value As String)
            strSortExpression = value
        End Set
    End Property

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <exception cref="Test"/>
    ''' <remarks></remarks>
    Public Property SortDirection As String
        Get
            Return strSortDirection
        End Get
        Set(ByVal value As String)
            strSortDirection = value
        End Set
    End Property
End Class

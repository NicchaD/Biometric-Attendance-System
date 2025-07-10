'*******************************************************************************
' Copyright YMCA Retirement Fund All Rights Reserved. 
' Project Name		:	YMCA-YRS
' FileName			:	RolloverInstitution.ascx.vb
' Author Name		:	Anudeep  
' Creation Date		:	06/06/2014
' Description		:	User Control is used to get the list of institutions in AtsRollovers
'*******************************************************************************
'Modification History
'********************************************************************************************************************************
'Modified By        Date            Description
'********************************************************************************************************************************
'Sanjay R.      2014.07.25      BT 2615/YRS 5.0-2404:Second Beneficiary get settled automatically after settling first Beneficiary
'*******************************************************************************
Public Class RolloverInstitution
    Inherits System.Web.UI.UserControl
    Dim strEvent As String
    Public Property OnSelectEvent() As String
        Get
            Return strEvent
        End Get
        Set(value As String)
            strEvent = value
        End Set
    End Property
    Public Property Text() As String
        Get
            Return TextBoxInstitution.Text
        End Get
        Set(value As String)
            TextBoxInstitution.Text = value
        End Set
    End Property
    'Start:SR:2014.08.19 :BT 2632/YRS 5.0-2404 - Intelli-sense to select rollover institution name.
    Public Property Enabled() As Boolean
        Get
            Return TextBoxInstitution.Enabled
        End Get
        Set(value As Boolean)
            TextBoxInstitution.Enabled = value
        End Set
    End Property
    Public Property rReadOnly() As Boolean
        Get
            Return TextBoxInstitution.ReadOnly
        End Get
        Set(value As Boolean)
            TextBoxInstitution.ReadOnly = value
        End Set
    End Property
    'End:SR:2014.08.19 :BT 2632/YRS 5.0-2404 - Intelli-sense to select rollover institution name.

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

End Class
'************************************************************************************************************************
' Author                    : Pramod Prakash Pokale
' Created on                : 04/21/2017
' Summary of Functionality  : Hosts user control methods and event handlers
' Declared in Version       : 20.2.0 | YRS-AT-3356 -  YRS enh: due MAY 2017 - RMD Print Letters screen-Changes to Initial & Follow-up screen. (1 of 3 tickets) (TrackIT 29186) 
'
'************************************************************************************************************************
' REVISION HISTORY:
' ------------------------------------------------------------------------------------------------------
' Developer Name                | Date          | Version No    | Ticket
' ------------------------------------------------------------------------------------------------------
' Manthan Rajguru			    | 2017.05.04    |	20.2.0	    | YRS-AT-3205 -  YRS enh: needed by DECEMBER 2017 - RMD Print letters function for QD participants with first RMD due in December (TrackIT 27977) 
' ------------------------------------------------------------------------------------------------------
'************************************************************************************************************************

Public Class BatchProcessProgressControl
    Inherits System.Web.UI.UserControl

    Public Event HandlePUCCloseButtonClick As EventHandler

#Region "Properties"

    Public Property DialogTitle() As String
        Get
            If Not ViewState("PUCDialogTitle") Is Nothing Then
                Return Convert.ToString(ViewState("PUCDialogTitle"))
            Else
                Return String.Empty
            End If
        End Get
        Set(ByVal value As String)
            ViewState("PUCDialogTitle") = value
        End Set
    End Property

    Public ReadOnly Property OpenConfirmDialogMethodName() As String
        Get
            Return "OpenConfirmDialog"
        End Get
    End Property

    Public ReadOnly Property StartProcessMethodName() As String
        Get
            Return "CallProcess"
        End Get
    End Property

    Public Property BatchID() As String
        Get
            If Not HttpContext.Current.Session("PUCBatchID") Is Nothing Then
                Return Convert.ToString(HttpContext.Current.Session("PUCBatchID"))
            Else
                Return String.Empty
            End If
        End Get
        Set(ByVal value As String)
            HttpContext.Current.Session("PUCBatchID") = value
        End Set
    End Property

    Public Property ModuleName() As String
        Get
            If Not HttpContext.Current.Session("PUCModuleName") Is Nothing Then
                Return Convert.ToString(HttpContext.Current.Session("PUCModuleName"))
            Else
                Return String.Empty
            End If
        End Get
        Set(ByVal value As String)
            HttpContext.Current.Session("PUCModuleName") = value
        End Set
    End Property
    'START: MMR | YRS-AT-3205 | Declared property to set reprint status
    Public Shared Property AllowReprint() As Boolean
        Get
            If Not HttpContext.Current.Session("AllowReprint") Is Nothing Then
                Return Convert.ToBoolean(HttpContext.Current.Session("AllowReprint"))
            Else
                Return False
            End If
        End Get
        Set(ByVal value As Boolean)
            HttpContext.Current.Session("AllowReprint") = value
        End Set
    End Property
    'END: MMR | YRS-AT-3205 | Declared property to set reprint status

    'START: MMR | YRS-AT-3205 | Declared property to set folder name module wise for reprint letters
    Public Shared Property FolderForReprint() As String
        Get
            If Not HttpContext.Current.Session("FolderForReprint") Is Nothing Then
                Return Convert.ToString(HttpContext.Current.Session("FolderForReprint"))
            Else
                Return String.Empty
            End If
        End Get
        Set(ByVal value As String)
            HttpContext.Current.Session("FolderForReprint") = value
        End Set
    End Property
    'END: MMR | YRS-AT-3205 | Declared property to set folder name module wise for reprint letters
#End Region

    Protected Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        RaiseEvent HandlePUCCloseButtonClick(sender, e)
    End Sub
End Class
Imports System.Drawing

'********************************************************************************************************************************
'Modification History
'********************************************************************************************************************************
'Modified By        Date                Description
'Ashutosh Patil     24-May-2007         Height of PanelDate changed from 16px to 20px so that the Calendar control can be
'                                       displayed properly.
'********************************************************************************************************************************
<ValidationPropertyAttribute("Message")> Public Class DateUserControl
    Inherits System.Web.UI.UserControl

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents valCustomDOB As System.Web.UI.WebControls.CustomValidator
    Protected WithEvents TextBoxUCDate As System.Web.UI.WebControls.TextBox
    Protected WithEvents PanelDate As System.Web.UI.WebControls.Panel
    Protected WithEvents PopcalendarDate As RJS.Web.WebControl.PopCalendar
    Protected WithEvents RequiredFieldValidator1 As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents RangeValidatorUCDate As System.Web.UI.WebControls.RangeValidator

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Public Property Text()
        Get
            Return Me.TextBoxUCDate.Text
        End Get
        Set(ByVal Value)
            Me.TextBoxUCDate.Text = Value
        End Set
    End Property
	Public Property cssInput() As String
		Get
			Return Me.TextBoxUCDate.CssClass
		End Get
		Set(ByVal Value As String)
			Me.TextBoxUCDate.CssClass = Value
		End Set
	End Property
    Public Property Enabled()
        Get
            Return Me.TextBoxUCDate.Enabled
        End Get
        Set(ByVal Value)
            Me.TextBoxUCDate.Enabled = Value
            Me.PopcalendarDate.Enabled = Value
        End Set
    End Property
    Public Property RequiredDate() As Boolean
        Get

        End Get
        Set(ByVal Value As Boolean)
            If Value = True Then
                Me.RequiredFieldValidator1.Enabled = True
            Else
                Me.RequiredFieldValidator1.Enabled = False
            End If
        End Set
    End Property
    Public Property rReadOnly() As Boolean
        Get
            Return CType(Me.TextBoxUCDate.ReadOnly, Boolean)
        End Get
        Set(ByVal Value As Boolean)
            If Value = True Then
                Me.TextBoxUCDate.ReadOnly = True
            Else
                Me.TextBoxUCDate.ReadOnly = False
            End If
        End Set
    End Property
    Public Property BackColor() As Color
        Get
            Return CType(Me.TextBoxUCDate.BackColor, Color)
        End Get
        Set(ByVal Value As Color)
            Me.TextBoxUCDate.BackColor = Value
        End Set
    End Property
    'Start - Added by hafiz on 14-Dec-2006
    Public Property RequiredValidatorErrorMessage() As String
        Get
            Return Me.RequiredFieldValidator1.ErrorMessage
        End Get
        Set(ByVal Value As String)
            Me.RequiredFieldValidator1.ErrorMessage = Value
        End Set
    End Property
    Public Property RequiredValidatorErrorMessage1() As String
        Get
            Return Me.PopcalendarDate.RequiredDateMessage
        End Get
        Set(ByVal Value As String)
            Me.PopcalendarDate.RequiredDateMessage = Value
        End Set
    End Property
    Public Property RequiredValidatorText() As String
        Get
            Return Me.RequiredFieldValidator1.Text
        End Get
        Set(ByVal Value As String)
            Me.RequiredFieldValidator1.Text = Value
        End Set
    End Property
    Public Property RequiredValidatorText1() As String
        Get
            Return Me.PopcalendarDate.TextMessage
        End Get
        Set(ByVal Value As String)
            Me.PopcalendarDate.TextMessage = Value
        End Set
    End Property
    Public Property FormatValidatorErrorMessage() As String
        Get
            Return Me.valCustomDOB.ErrorMessage
        End Get
        Set(ByVal Value As String)
            Me.valCustomDOB.ErrorMessage = Value
        End Set
    End Property
    Public Property FormatValidatorErrorMessage1() As String
        Get
            Return Me.PopcalendarDate.InvalidDateMessage
        End Get
        Set(ByVal Value As String)
            Me.PopcalendarDate.InvalidDateMessage = Value
        End Set
    End Property
    Public Property FormatValidatorText() As String
        Get
            Return Me.valCustomDOB.Text
        End Get
        Set(ByVal Value As String)
            Me.valCustomDOB.Text = Value
        End Set
    End Property
    Public Property FormatValidatorText1() As String
        Get
            Return Me.PopcalendarDate.TextMessage
        End Get
        Set(ByVal Value As String)
            Me.PopcalendarDate.TextMessage = Value
        End Set
    End Property
    Public Property AutoPostBack() As Boolean
        Get
            Return Me.TextBoxUCDate.AutoPostBack
        End Get
        Set(ByVal Value As Boolean)
            Me.TextBoxUCDate.AutoPostBack = Value
        End Set
    End Property
    'End - Added by hafiz on 14-Dec-2006

    'Public Overrides Property ID() As String
    '    Get
    '        Return CType(Me.TextBoxUCDate.ID, String)
    '    End Get
    '    Set(ByVal Value As String)
    '        Me.TextBoxUCDate.ID = Value
    '    End Set
    'End Property
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        'Me.RequiredFieldValidator1.Enabled = True
        Me.rReadOnly = False
    End Sub

    Private Sub TextBoxUCDate_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBoxUCDate.TextChanged

    End Sub
    Private _message As Integer = 0

    Public Property Message() As Integer
        Get
            Return _message
        End Get
        Set(ByVal Value As Integer)
            _message = Value
        End Set
    End Property
    'Start- Added By Ashish 13 May 2008
    Public Property EnabledDateRangeValidator() As Boolean
        Get
            Return Me.RangeValidatorUCDate.Enabled
        End Get
        Set(ByVal Value As Boolean)
            Me.RangeValidatorUCDate.Enabled = Value
        End Set
    End Property
    Public Property RangeValidatorMinValue() As String
        Get
            Return Me.RangeValidatorUCDate.MinimumValue
        End Get
        Set(ByVal Value As String)
            Me.RangeValidatorUCDate.MinimumValue = Value
        End Set
    End Property
    Public Property RangeValidatorMaxValue() As String
        Get
            Return Me.RangeValidatorUCDate.MaximumValue
        End Get
        Set(ByVal Value As String)
            Me.RangeValidatorUCDate.MaximumValue = Value
        End Set
    End Property
    Public Property RangeValidatorErrorMassege() As String
        Get
            Return Me.RangeValidatorUCDate.ErrorMessage
        End Get
        Set(ByVal Value As String)
            Me.RangeValidatorUCDate.ErrorMessage = Value
        End Set
    End Property
    'End- Added By Ashish 13 May 2008
End Class

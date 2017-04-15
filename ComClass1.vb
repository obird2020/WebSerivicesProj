<ComClass(ComClass1.ClassId, ComClass1.InterfaceId, ComClass1.EventsId)> _
Public Class ComClass1

#Region "COM GUIDs"
    ' These  GUIDs provide the COM identity for this class 
    ' and its COM interfaces. If you change them, existing 
    ' clients will no longer be able to access the class.
    Public Const ClassId As String = "c11a7d54-4169-404a-b0ff-a1fd9afca5ae"
    Public Const InterfaceId As String = "aa80c46f-a7fd-4296-a8ca-806195ff14c6"
    Public Const EventsId As String = "ecfcecbb-603a-4f09-bb46-25f7372183b7"
#End Region

    ' A creatable COM class must have a Public Sub New() 
    ' with no parameters, otherwise, the class will not be 
    ' registered in the COM registry and cannot be created 
    ' via CreateObject.
    Public Sub New()
        MyBase.New()
    End Sub

End Class



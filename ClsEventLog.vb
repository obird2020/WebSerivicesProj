Imports System.Security.Permissions
Public Class ClsEventLog
    'Friend Shared Sub WriteError(ByVal errMsg As String)

    '    Dim AppName As String = "Email_Library"
    '    Dim LogName As String = "Application"

    '    Dim log As New EventLog()

    '    If Not EventLog.SourceExists(AppName) Then
    '        EventLog.CreateEventSource(AppName, LogName)
    '    End If

    '    log.Source = AppName

    '    'WriteEntry is overloaded; this is one of 10 ways to call it
    '    Dim msg As String = "Application Unhandled exception has occurred." & vbCrLf & vbCrLf & _
    '        errMsg

    '    log.WriteEntry(msg, System.Diagnostics.EventLogEntryType.Error)

    'End Sub
    Sub giveaccess()
        Dim strHKLMPath As String = "HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\EventLog\Security"
        Dim strHKCUPath As String = "HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\EventLog\Security"

        Dim fp As New RegistryPermission(RegistryPermissionAccess.AllAccess, strHKLMPath)
        fp.Assert()
        ' Do your read/write here
        'Security.CodeAccessPermission.RevertAssert()
        fp = Nothing

   End Sub

    Public Function CreateLog(strLogName As String) As Boolean
        Dim Reasult As Boolean = False

        Try
            System.Diagnostics.EventLog.CreateEventSource(strLogName, strLogName)
            Dim SQLEventLog As New System.Diagnostics.EventLog()

            SQLEventLog.Source = strLogName
            SQLEventLog.Log = strLogName

            SQLEventLog.Source = strLogName
            SQLEventLog.WriteEntry("The " & strLogName & " was successfully initialize component.", EventLogEntryType.Information)

            Reasult = True
        Catch
            Reasult = False
        End Try


        Return Reasult
   End Function

    Public Sub WriteToEventLog(strLogName As String, strSource As String, strErrDetail As String, ByVal status As Boolean)
        Dim SQLEventLog As New System.Diagnostics.EventLog()

        'giveaccess()
        Try
            If Not System.Diagnostics.EventLog.SourceExists(strLogName) Then
                Me.CreateLog(strLogName)
            End If


            SQLEventLog.Source = strLogName
            If status = True Then
                SQLEventLog.WriteEntry(Convert.ToString(strSource) + Convert.ToString(strErrDetail), EventLogEntryType.Information)
            Else
                SQLEventLog.WriteEntry(Convert.ToString(strSource) + Convert.ToString(strErrDetail), EventLogEntryType.Error)
            End If

        Catch ex As Exception
            SQLEventLog.Source = strLogName

         SQLEventLog.WriteEntry(Convert.ToString("INFORMATION: ") + Convert.ToString(ex.Message), EventLogEntryType.Error)
        Finally
            SQLEventLog.Dispose()

            SQLEventLog = Nothing
        End Try

    End Sub

End Class

Imports System.IO
Imports System.Text.Json
Imports System.Windows
Imports System.Windows.Controls

Class MainWindow
    Private Sub ServiceContract_Click(sender As Object, e As RoutedEventArgs)
        Dim btn = TryCast(sender, Button)
        If btn Is Nothing Then Return
        Dim key = TryCast(btn.Tag, String)
        If String.IsNullOrWhiteSpace(key) Then Return
        Dim detail = New ServiceDetailWindow(key)
        detail.Owner = Me
        detail.ShowDialog()
    End Sub

    Private Sub Button_Click(sender As Object, e As RoutedEventArgs)

    End Sub
End Class

Imports System.IO
Imports System.Text.Json
Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Media

Class MainWindow
    Private _clientsBorder As Border
    Private _servicesBorder As Border
    Private _aboutBorder As Border

    Private Sub ServiceContract_Click(sender As Object, e As RoutedEventArgs)
        Dim btn = TryCast(sender, Button)
        If btn Is Nothing Then Return
        Dim key = TryCast(btn.Tag, String)
        If String.IsNullOrWhiteSpace(key) Then Return
        Dim detail = New ServiceDetailWindow(key)
        detail.Owner = Me
        detail.ShowDialog()
    End Sub

    Private Sub Salir_Click(sender As Object, e As RoutedEventArgs)
        Application.Current.Shutdown()
    End Sub

    Private Sub Nosotros_Click(sender As Object, e As RoutedEventArgs)
        EnsurePanelsRefs()
        ShowPanel(_aboutBorder)
    End Sub

    Private Sub Clientes_Click(sender As Object, e As RoutedEventArgs)
        EnsurePanelsRefs()
        ShowPanel(_clientsBorder)
    End Sub

    Private Sub Servicios_Click(sender As Object, e As RoutedEventArgs)
        EnsurePanelsRefs()
        ShowPanel(_servicesBorder)
    End Sub

    Private Sub ShowPanel(target As Border)
        If target Is Nothing Then Return
        If _servicesBorder IsNot Nothing Then _servicesBorder.Visibility = If(target Is _servicesBorder, Visibility.Visible, Visibility.Collapsed)
        If _clientsBorder IsNot Nothing Then _clientsBorder.Visibility = If(target Is _clientsBorder, Visibility.Visible, Visibility.Collapsed)
        If _aboutBorder IsNot Nothing Then _aboutBorder.Visibility = If(target Is _aboutBorder, Visibility.Visible, Visibility.Collapsed)
    End Sub

    Private Sub EnsurePanelsRefs()
        If _servicesBorder Is Nothing OrElse _clientsBorder Is Nothing OrElse _aboutBorder Is Nothing Then
            Dim rootGrid = TryCast(Me.Content, Grid)
            If rootGrid Is Nothing Then Return
            For Each child In rootGrid.Children
                Dim b = TryCast(child, Border)
                If b Is Nothing Then Continue For
                Dim name = b.Name
                Select Case name
                    Case "ServicesPanel" : _servicesBorder = b
                    Case "ClientsPanel" : _clientsBorder = b
                    Case "AboutPanel" : _aboutBorder = b
                End Select
            Next
        End If
    End Sub

    ' Handler referenced in XAML for first contratar button (kept empty; dynamic hooking used)
    Private Sub Button_Click(sender As Object, e As RoutedEventArgs)
        ' Intentionally left blank; dynamic hooking assigns proper handler.
    End Sub

    Private Sub MainWindow_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        HookServiceButtons()
        CachePanels()
        EnsureClientsPanel()
        HookMenuButtons()
    End Sub

    Private Sub HookMenuButtons()
        Try
            Dim allButtons = FindButtons(Me)
            For Each b In allButtons
                Dim txt = TryCast(b.Content, String)
                If txt Is Nothing Then Continue For
                If String.Equals(txt, "Clientes", StringComparison.OrdinalIgnoreCase) Then
                    RemoveHandler b.Click, AddressOf Clientes_Click
                    AddHandler b.Click, AddressOf Clientes_Click
                ElseIf String.Equals(txt, "Servicios", StringComparison.OrdinalIgnoreCase) Then
                    RemoveHandler b.Click, AddressOf Servicios_Click
                    AddHandler b.Click, AddressOf Servicios_Click
                End If
            Next
        Catch ex As Exception
            Console.WriteLine(ex.Message)
        End Try
    End Sub

    Private Sub HookServiceButtons()
        Try
            Dim serviceMap As New Dictionary(Of String, String)(StringComparer.OrdinalIgnoreCase)
            serviceMap("Consultoría y Auditoría") = "Consultoria"
            serviceMap("Consultoria y Auditoria") = "Consultoria"
            serviceMap("Capacitaciones Técnicas") = "Capacitaciones"
            serviceMap("Capacitaciones Tecnicas") = "Capacitaciones"
            serviceMap("Herramientas / Suites") = "Herramientas"

            Dim allButtons = FindButtons(Me)
            For Each b In allButtons
                If String.Equals(Convert.ToString(b.Content), "Contratar", StringComparison.OrdinalIgnoreCase) Then
                    Dim parentStack = TryCast(b.Parent, StackPanel)
                    If parentStack IsNot Nothing Then
                        For Each child In parentStack.Children
                            Dim tb = TryCast(child, TextBlock)
                            If tb IsNot Nothing AndAlso serviceMap.ContainsKey(tb.Text) Then
                                b.Tag = serviceMap(tb.Text)
                                RemoveHandler b.Click, AddressOf ServiceContract_Click
                                AddHandler b.Click, AddressOf ServiceContract_Click
                                Exit For
                            End If
                        Next
                    End If
                End If
            Next
        Catch ex As Exception
            Console.WriteLine(ex.Message)
        End Try
    End Sub

    Private Sub CachePanels()
        Dim rootGrid = TryCast(Me.Content, Grid)
        If rootGrid Is Nothing Then Return
        For Each child In rootGrid.Children
            Dim b = TryCast(child, Border)
            If b IsNot Nothing AndAlso Grid.GetColumn(b) = 1 AndAlso Grid.GetRow(b) = 1 Then
                Dim sv = TryCast(b.Child, ScrollViewer)
                If sv IsNot Nothing Then
                    Dim sp = TryCast(sv.Content, StackPanel)
                    If sp IsNot Nothing Then
                        For Each c In sp.Children
                            Dim tb = TryCast(c, TextBlock)
                            If tb IsNot Nothing AndAlso tb.Text.Contains("Servicios") Then
                                _servicesBorder = b
                                Exit For
                            End If
                        Next
                    End If
                End If
            End If
        Next
    End Sub

    Private Sub EnsureClientsPanel()
        If _clientsBorder IsNot Nothing Then Return
        Dim rootGrid = TryCast(Me.Content, Grid)
        If rootGrid Is Nothing Then Return
        _clientsBorder = BuildClientsBorder()
        Grid.SetRow(_clientsBorder, 1)
        Grid.SetColumn(_clientsBorder, 1)
        _clientsBorder.Visibility = Visibility.Collapsed
        rootGrid.Children.Add(_clientsBorder)
    End Sub

    Private Function BuildClientsBorder() As Border
        Dim wrap = New WrapPanel()
        Dim clients() As String = {"Cemex", "Lenovo", "Epicor", "Softtek", "Accenture", "Neoris"}
        For Each clientName As String In clients
            wrap.Children.Add(BuildClientCard(clientName, 4.9))
        Next
        Dim stack = New StackPanel()
        stack.Children.Add(New TextBlock With {.Text = "Clientes", .FontSize = 22, .Foreground = Brushes.White, .FontWeight = FontWeights.SemiBold, .Margin = New Thickness(0, 0, 0, 14)})
        stack.Children.Add(wrap)
        Dim sv = New ScrollViewer With {.VerticalScrollBarVisibility = ScrollBarVisibility.Auto, .Content = stack}
        Return New Border With {
            .Background = New SolidColorBrush(Color.FromRgb(&H1F, &H1F, &H1F)),
            .CornerRadius = New CornerRadius(10),
            .Padding = New Thickness(22),
            .Margin = New Thickness(16),
            .Child = sv
        }
    End Function

    Private Function BuildClientCard(name As String, rating As Double) As Border
        Dim stars = "★★★★★"
        Dim outer = New Border With {
            .Background = New SolidColorBrush(Color.FromRgb(&H23, &H23, &H23)),
            .CornerRadius = New CornerRadius(8),
            .Padding = New Thickness(16),
            .Margin = New Thickness(0, 0, 18, 18),
            .Width = 260
        }
        Dim sp = New StackPanel()
        sp.Children.Add(New TextBlock With {.Text = name, .FontSize = 16, .FontWeight = FontWeights.Bold, .Foreground = Brushes.White})
        Dim starsPanel = New StackPanel With {.Orientation = Orientation.Horizontal, .Margin = New Thickness(0, 6, 0, 0)}
        starsPanel.Children.Add(New TextBlock With {.Text = stars, .Foreground = New SolidColorBrush(Color.FromRgb(&HFF, &HC1, &H7)), .FontSize = 14})
        starsPanel.Children.Add(New TextBlock With {.Text = rating.ToString("0.0"), .Foreground = New SolidColorBrush(Color.FromRgb(&HCC, &HCC, &HCC)), .FontSize = 14, .Margin = New Thickness(8, 0, 0, 0)})
        sp.Children.Add(starsPanel)
        outer.Child = sp
        Return outer
    End Function

    Private Function FindButtons(root As DependencyObject) As List(Of Button)
        Dim list As New List(Of Button)
        If root Is Nothing Then Return list
        Dim count = VisualTreeHelper.GetChildrenCount(root)
        For i = 0 To count - 1
            Dim child = VisualTreeHelper.GetChild(root, i)
            If TypeOf child Is Button Then list.Add(TryCast(child, Button))
            list.AddRange(FindButtons(child))
        Next
        Return list
    End Function
End Class

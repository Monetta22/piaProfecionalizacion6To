Imports System.IO
Imports System.Text.Json
Imports System.Windows
Imports System.Windows.Controls

Public Class ServiceDetailWindow
    Private _serviceKey As String

    Public Sub New(serviceKey As String)
        InitializeComponent()
        _serviceKey = serviceKey
        LoadService()
        AddHandlers()
        LoadFeatures()
    End Sub

    Private Sub LoadService()
        Dim info = ServiceInfoRepository.Get(_serviceKey)
        TitleText.Text = info.Name
        PriceText.Text = info.Price
        SummaryText.Text = info.Summary
        DeliverablesText.Text = "• " & String.Join(Environment.NewLine & "• ", info.Deliverables)
        BenefitsText.Text = "• " & String.Join(Environment.NewLine & "• ", info.Benefits)
        DurationText.Text = info.Duration
        SlaText.Text = info.SLA
        EquipoText.Text = info.Team
        CasosText.Text = "• " & String.Join(Environment.NewLine & "• ", info.CaseStudies)
        Me.Title = "Detalle: " & info.Name
    End Sub

    Private Sub LoadFeatures()
        Dim baseFeatures As New List(Of String)
        Select Case _serviceKey.ToLowerInvariant()
            Case "consultoria"
                baseFeatures.AddRange(New String() {
                    "Cobertura OWASP Top10 + CIS Benchmarks",
                    "Escaneo autenticado y no autenticado",
                    "Revisión de políticas y procedimientos críticos",
                    "Sesión ejecutiva de hallazgos prioritarios",
                    "Roadmap de madurez de seguridad a12 meses"
                })
            Case "capacitaciones"
                baseFeatures.AddRange(New String() {
                    "Laboratorio aislado para ejercicios de ataque/defensa",
                    "Simulaciones de incidentes basadas en MITRE ATT&CK",
                    "Material actualizado trimestralmente",
                    "Panel de progreso individual y por equipo",
                    "Acceso a microlearning post curso durante6 meses"
                })
            Case "herramientas"
                baseFeatures.AddRange(New String() {
                    "Arquitectura de referencia multi-cloud",
                    "Integración con Active Directory / Entra ID",
                    "Playbooks iniciales para respuesta automatizada",
                    "Dashboards ejecutivos KPI + métricas técnicas",
                    "Capacitación operativa de tuning y mantenimiento"
                })
            Case Else
                baseFeatures.AddRange(New String() {
                    "Soporte estándar",
                    "Documentación básica",
                    "Reporte resumido de avance"
                })
        End Select
        Dim ic = TryCast(Me.FindName("FeaturesList"), ItemsControl)
        If ic IsNot Nothing Then
            ic.ItemsSource = baseFeatures
        End If
    End Sub

    Private Sub AddHandlers()
        Dim solicitar = TryCast(Me.FindName("BtnSolicitar"), Button)
        Dim enviar = TryCast(Me.FindName("BtnEnviar"), Button)
        Dim cancelar = TryCast(Me.FindName("BtnCancelarForm"), Button)
        If solicitar IsNot Nothing Then AddHandler solicitar.Click, AddressOf ShowForm
        If enviar IsNot Nothing Then AddHandler enviar.Click, AddressOf SendForm
        If cancelar IsNot Nothing Then AddHandler cancelar.Click, AddressOf CancelForm
    End Sub

    Private Sub ShowForm(sender As Object, e As RoutedEventArgs)
        Dim formBorder = TryCast(Me.FindName("FormBorder"), Border)
        If formBorder IsNot Nothing Then formBorder.Visibility = Visibility.Visible
    End Sub

    Private Sub CancelForm(sender As Object, e As RoutedEventArgs)
        Dim formBorder = TryCast(Me.FindName("FormBorder"), Border)
        If formBorder IsNot Nothing Then formBorder.Visibility = Visibility.Collapsed
        Dim status = TryCast(Me.FindName("StatusText"), TextBlock)
        If status IsNot Nothing Then status.Text = String.Empty
    End Sub

    Private Sub SendForm(sender As Object, e As RoutedEventArgs)
        Dim empresa = TryCast(Me.FindName("EmpresaText"), TextBox)
        Dim correo = TryCast(Me.FindName("CorreoText"), TextBox)
        Dim telefono = TryCast(Me.FindName("TelefonoText"), TextBox)
        Dim representante = TryCast(Me.FindName("RepresentanteText"), TextBox)
        Dim mensaje = TryCast(Me.FindName("MensajeText"), TextBox)
        Dim status = TryCast(Me.FindName("StatusText"), TextBlock)

        If empresa Is Nothing OrElse correo Is Nothing OrElse telefono Is Nothing OrElse representante Is Nothing Then
            Return
        End If

        Dim solicitud = New With {
            .Servicio = _serviceKey,
            .Empresa = empresa.Text,
            .Correo = correo.Text,
            .Telefono = telefono.Text,
            .Representante = representante.Text,
            .Mensaje = If(mensaje?.Text, String.Empty),
            .Fecha = Date.Now.ToString("s")
        }

        Try
            Dim folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "SolicitudesSeguridad")
            Directory.CreateDirectory(folder)
            Dim filePath = Path.Combine(folder, "solicitudes.json")
            Dim list As New List(Of Object)
            If File.Exists(filePath) Then
                Dim existing = File.ReadAllText(filePath)
                If Not String.IsNullOrWhiteSpace(existing) Then
                    Dim arr = JsonSerializer.Deserialize(Of List(Of Object))(existing)
                    If arr IsNot Nothing Then list.AddRange(arr)
                End If
            End If
            list.Add(solicitud)
            Dim json = JsonSerializer.Serialize(list, New JsonSerializerOptions With {.WriteIndented = True})
            File.WriteAllText(filePath, json)
            If status IsNot Nothing Then
                status.Text = "Solicitud guardada correctamente."
            Else
                MessageBox.Show("Guardado")
            End If
        Catch ex As Exception
            If status IsNot Nothing Then status.Text = "Error: " & ex.Message
        End Try
    End Sub
End Class
Public Class ServiceInfo
    Public Property Key As String
    Public Property Name As String
    Public Property Price As String
    Public Property Summary As String
    Public Property Deliverables As List(Of String)
    Public Property Benefits As List(Of String)
    Public Property Duration As String
    Public Property SLA As String
    Public Property Team As String
    Public Property CaseStudies As List(Of String)
End Class

Public Module ServiceInfoRepository
    Private _items As Dictionary(Of String, ServiceInfo)

    Public Function [Get](key As String) As ServiceInfo
        Ensure()
        If _items.ContainsKey(key) Then
            Return _items(key)
        End If
        Return New ServiceInfo With {
   .Key = key,
  .Name = key,
      .Price = "N/D",
      .Summary = "No disponible",
 .Deliverables = New List(Of String),
  .Benefits = New List(Of String),
        .Duration = "N/D",
   .SLA = "N/D",
   .Team = "N/D",
   .CaseStudies = New List(Of String)
   }
    End Function

    Private Sub Ensure()
        If _items IsNot Nothing Then Return
        _items = New Dictionary(Of String, ServiceInfo)(StringComparer.OrdinalIgnoreCase) From {
          {"Consultoria", New ServiceInfo With {
     .Key = "Consultoria",
           .Name = "Consultoría y Auditoría",
          .Price = "Desde $1,500",
     .Summary = "Servicio integral de evaluación y mejora de la postura de seguridad corporativa.",
           .Deliverables = New List(Of String) From {
         "Informe de vulnerabilidades priorizado",
         "Mapa de riesgos y matriz de impacto",
         "Plan de remediación a 90 días",
         "Checklist de hardening para servidores y endpoints"
         },
          .Benefits = New List(Of String) From {
         "Reducción de superficie de ataque",
         "Cumplimiento normativo (ISO 27001 / PCI-DSS)",
     "Mayor visibilidad de riesgos críticos",
         "Optimización de inversiones en herramientas"
           },
         .Duration = "2 a 4 semanas según alcance",
     .SLA = "Primer informe preliminar en 5 días hábiles",
         .Team = "1 Líder auditor, 1 analista técnico, 1 especialista de cumplimiento",
           .CaseStudies = New List(Of String) From {
         "Retail: reducción 45% findings críticos",
     "Finanzas: alineación a PCI-DSS en 3 semanas"
     }
       }},
       {"Capacitaciones", New ServiceInfo With {
           .Key = "Capacitaciones",
           .Name = "Capacitaciones Técnicas",
          .Price = "Desde $900",
         .Summary = "Programas prácticos para fortalecer capacidades internas frente a amenazas modernas.",
     .Deliverables = New List(Of String) From {
     "Material digital y laboratorio virtual",
         "Evaluación de conocimientos inicial y final",
     "Certificado interno de aprovechamiento",
         "Acceso a repositorio de scripts y playbooks"
     },
     .Benefits = New List(Of String) From {
     "Equipos más preparados ante incidentes",
         "Estandarización de procedimientos",
         "Mejora en tiempos de respuesta",
         "Retención de talento técnico"
           },
         .Duration = "Bootcamps de 16 a 40 horas",
         .SLA = "Acceso a plataforma en 24h tras contratación",
         .Team = "Instructor senior + mentor de laboratorio",
           .CaseStudies = New List(Of String) From {
         "Manufactura: reducción del tiempo de contención 30%",
     "Fintech: adopción de playbooks SOAR"
         }
          }},
      {"Herramientas", New ServiceInfo With {
         .Key = "Herramientas",
     .Name = "Herramientas / Suites",
          .Price = "Desde $2,300",
          .Summary = "Implementación y tuning de plataformas de monitoreo y detección avanzada.",
          .Deliverables = New List(Of String) From {
         "Deploy de SIEM / EDR / Scanner",
    "Reglas de correlación personalizadas",
         "Paneles ejecutivos y técnicos",
         "Documento de operación y transferencia de conocimiento"
     },
           .Benefits = New List(Of String) From {
         "Mayor detección temprana de amenazas",
         "Visibilidad centralizada",
         "Optimización de licenciamiento",
     "Integración con procesos de respuesta"
     },
     .Duration = "3 a 6 semanas según stack",
     .SLA = "Incidentes críticos escalados < 15 minutos (post go-live)",
          .Team = "Arquitecto de plataforma, Ingeniero de seguridad, Especialista de integración",
     .CaseStudies = New List(Of String) From {
         "E-Commerce: reducción de falsos positivos 55%",
    "Energía: integración OT/IT en SIEM"
     }
      }}
      }
    End Sub
End Module
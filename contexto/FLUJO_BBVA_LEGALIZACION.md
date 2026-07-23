# Flujo BBVA Legalización — Resumen de Procesos

> Extraído del diagrama `BBVA_Legalizacion.drawio`
> HU asociadas desde la carpeta `contexto/hu_texto/`

---

## Historias de Usuario del Subproceso de Escrituración y Garantías

| HU | Actividad | Estado |
|---|---|---|
| BBV-86 | Firmar Escritura Cliente | ✅ Implementada |
| BBV-130 | Revisar EP Abogado | Pendiente |
| BBV-91 | Firmar Rep. Legal | ✅ Implementada |
| BBV-92 | Realizar Entrega EP Firmada | ✅ Implementada |
| BBV-90 | Realizar Devolución EP | Pendiente |
| BBV-93 | Realizar Recepción Boleta (OCR) | Pendiente |
| BBV-94 | Realizar Excepción Desembolso | Pendiente |
| BBV-95 | Realizar EP Registradas | Pendiente |
| BBV-96 | Realizar VB Final Abogado | Pendiente |
| BBV-97 | Realizar Gestión Comercial | Pendiente |
| BBV-98 | Preformalizar | Pendiente |
| BBV-99 | Validar Condiciones Desembolso | Pendiente |
| BBV-100 | Realizar Desembolso | Pendiente |
| BBV-101 | Registro y Control de Cheques | Pendiente |
| BBV-102 | Gestionar Control de Garantías | Pendiente |
| BBV-103 | Enviar a Custodio | Pendiente |
| BBV-141 | Realizar VoBo Gerencia COH | Pendiente |

## Otras HU (fuera de Escrituración)

| HU | Actividad | Subproceso |
|---|---|---|
| BBV-104 | Validar Cumplimiento de Políticas | Cumplimiento |
| BBV-105 | Realizar Devolución Pendiente VB Comercial | Cumplimiento |
| BBV-80 | Definir Inmueble | Cumplimiento |

---

## Flujo de Escrituración y Garantías (secuencia de actividades)

```
Firmar Escritura Cliente (BBV-86)
  │
  ├─ SI Escalamiento Comercial → Realizar Gestión Comercial (BBV-97)
  │
  ├─ SI CXI → Realizar VB Prorrata (paralela)
  │
  ├─ SI Leasing + Causar → Realizar Causación (paralela)
  │
  └─ NO Escalamiento → Revisar EP Abogado (BBV-130)
                           │
                           ├─ Concepto Favorable → Firmar Rep. Legal (BBV-91)
                           │                        │
                           │                        ├─ Firmada Conforme → Realizar Entrega EP Firmada (BBV-92)
                           │                        │                      │
                           │                        │                      ├─ Siempre → Realizar Recepción Boleta (BBV-93)
                           │                        │                      │              → Realizar EP Registradas (BBV-95)
                           │                        │                      │                → Realizar VB Final Abogado (BBV-96)
                           │                        │                      │                  → Preformalizar (BBV-98)
                           │                        │                      │                    → Validar Condiciones Desembolso (BBV-99)
                           │                        │                      │                      → Realizar Desembolso (BBV-100)
                           │                        │                      │                        → Registro y Control Cheques (BBV-101)
                           │                        │                      │                          → Gestionar Control Garantías (BBV-102)
                           │                        │                      │                            → Enviar a Custodio (BBV-103)
                           │                        │                      │
                           │                        │                      └─ Si aplica excepción → Realizar Excepción Desembolso (BBV-94) (paralela)
                           │                        │
                           │                        └─ NO Firmada → Realizar Devolución EP (BBV-90)
                           │
                           └─ Concepto Desfavorable → Realizar Devolución EP (BBV-90)

Realizar VoBo Gerencia COH (BBV-141) — actividad transversal de autorización
```

---

## 1. Proceso Principal — Presto Legalización

### HU asociadas

| HU | Actividad | Subproceso |
|---|---|---|
| BBV-39 | Orquestación y Trazabilidad End-to-End del Workflow | Plataforma |
| BBV-40 | Paramétricas Maestras | Plataforma |
| BBV-41 | Cargar Créditos Aprobados | Presto Legalización |
| BBV-42 | Implementación Carta de Aprobación | Presto Legalización |
| BBV-76 | Radicar Crédito | Presto Legalización |
| BBV-77 | Cargar Documentos del Cliente Transversal | Presto Legalización |
| BBV-78 | Cargar Documentos del Comercial Transversal | Presto Legalización |
| BBV-140 | Módulo Radicación Manual (Comercial) | Presto Legalización |

### HU Transversales / Plataforma

| HU | Funcionalidad |
|---|---|
| BBV-54 | Expediente Digital — Consultar Parametrización Documental |
| BBV-55 | Expediente Digital — Extracción Automatizada de Datos OCR |
| BBV-56 | Motor de Asignación Automática |
| BBV-82 | Expediente Digital — Administrar Configuración Documental |
| BBV-83 | Expediente Digital — Workflow Documental Dinámico |
| BBV-84 | Expediente Digital — Restricciones y Validaciones Documentales |
| BBV-85 | Expediente Digital — Roles y Permisos Documentales |
| BBV-106 | Gestión de Perfiles y Permisos por Módulo |
| BBV-108 | Gestión Documental — Expediente Digital |
| BBV-109 | Funcionalidad Registro de Contacto Transversal |
| BBV-112 | Funcionalidad Sistema Nhia (Composición Documental) |
| BBV-113 | Asignación de Actividades en Balanceo |
| BBV-115 | Gestión de Estados |
| BBV-131 | Componente de Resultados de Asignación |
| BBV-132 | Reasignación Manual (Asignador) |
| BBV-133 | Módulo de Parametrización Tarifas por Roles |
| BBV-134 | Cargue de Archivos Planos |
| BBV-135 | Reportes y Auditoría Asignador |
| BBV-136 | Módulo Usuarios — Gestión y Directorio |
| BBV-139 | Historial de Seguimiento por Áreas |
| BBV-142 | Gestión de Reportes [Área Seguros] |
| BBV-143 | Gestión de Reportes [Gestión Folios Activos] |
| BBV-144 | Gestión de Reportes [Gestión EETT] |
| BBV-165 | Registro Seguimiento de Garantía |
| BBV-166 | Módulo Consulta de Folios |
| BBV-167 | Módulo Bandeja Asignadas |
| BBV-168 | Vista Ventana de Consulta Transversal |
| BBV-169 | Módulo Reportes |
| BBV-170 | Módulo Paramétricas |
| BBV-171 | Módulo Soporte |
| BBV-172 | Cálculo Automático del Porcentaje de Financiación |
| BBV-173 | Generación Automática de Correo de VB Garantías Cumplidas |

### Roles / Lanes

| Lane | Rol |
|---|---|
| 1 | Comercial |
| 2 | Sistema |
| 3 | Analista de Vivienda |
| 4 | Cliente |
| 5 | Comercial (devolución) |
| 6 | Analista de Vivienda (cumplimiento/escrituración) |

### Flujo

```
[Inicio]
  → ¿Requiere Cargue Manual?
      ├─ SÍ → Radicar Crédito y Carta de Aprobación (Comercial)
      │        → ¿Aplica Fast Track?
      │            ├─ SÍ → Fast Track (subproceso) → [Fin]
      │            └─ NO → (continúa al gateway +)
      └─ NO → Cargar Créditos Aprobados (Sistema)
               → Generar Carta de Aprobación (Sistema)
               → (gateway ×) → (gateway +)

[Gateway + paralelo] — Se abren hasta 3 caminos:
  ├─ Contacto Cliente (subproceso) → ¿Continua Caso?
  │     ├─ NO → [Fin Terminal]
  │     └─ SÍ → (gateway ×) → Gestión de Subprocesos
  │
  ├─ Cargar Documentos Cliente (Cliente) → [Fin]
  │
  └─ Cargar Documentos Comercial (Comercial) → [Fin]

Gestión de Subprocesos → ¿Continua Caso?
  ├─ NO → [Fin Terminal]  
  │        + señal "Definir Inmueble Asignador"
  └─ SÍ → Cumplimiento (subproceso) → ¿Continua Caso?
              ├─ NO → [Fin]
              │        + señal "Definir Inmueble Asignador"
              └─ SÍ → Escrituración y Garantías (subproceso) → [Fin Terminal]
```

---

## 2. Gestión de Subprocesos

### HU asociadas — Contacto Cliente

| HU | Actividad |
|---|---|
| BBV-43 | Validar Información |
| BBV-44 | Definir Inmueble |
| BBV-45 | Cargar Documentos Constructora |
| BBV-46 | Cargar Documentos Cliente |
| BBV-47 | Revisar Documentos Inmueble |
| BBV-48 | Asignar Firmas, Peritos y Abogados (OCR) |
| BBV-49 | Cargar Soportes de Pago |
| BBV-50 | Gestionar Firma |
| BBV-51 | Gestionar Firma Física |
| BBV-52 | Validar Integración de Documentos |
| BBV-53 | Realizar Devolución Pendiente VB Comercial |

### HU asociadas — Gestión de Subprocesos

| HU | Actividad |
|---|---|
| BBV-57 | Gestionar Escalamientos |
| BBV-58 | Realizar Devolución Pendiente VB Comercial |
| BBV-79 | Definir Inmueble (Gestión de Subprocesos) |
| BBV-81 | Asignar Firmas, Peritos y Abogados Comercial (Fast Track / OCR) |
| BBV-116 | Realizar VoBo Constructor |

### HU asociadas — Subprocesos hijos

| HU | Actividad | Subproceso |
|---|---|---|
| BBV-59 | Validar Seguros | Gestión de Seguros |
| BBV-60 | Realizar Exámenes Médicos | Gestión de Seguros |
| BBV-61 | Elaborar EETT | Gestión de EETT |
| BBV-62 | Programar Visita | Gestión de Avalúos |
| BBV-63 | Generar Informe | Gestión de Avalúos |
| BBV-65 | Realizar Consulta de Favorabilidad | Gestión de Avalúos |
| BBV-110 | Asignar Perito — Firmas Valuadoras | Gestión de Avalúos |
| BBV-111 | Generar Informe — Firmas | Gestión de Avalúos |
| BBV-66 | Emitir VB Inmuebles | Gestión de Activos |
| BBV-69 | Validar Oferta Vinculante | Gestión Compra de Cartera |
| BBV-70 | Radicar Oferta Vinculante | Gestión Compra de Cartera |
| BBV-71 | Esperar Respuesta de Oferta | Gestión Compra de Cartera |
| BBV-72 | Esperar Respuesta de Oferta (Vivienda) | Gestión Compra de Cartera |
| BBV-74 | Esperar Respuesta de Oferta (alt) | Gestión Compra de Cartera |
| BBV-75 | Marcar Cobertura | Gestión de Coberturas |
| BBV-107 | Revisar Marcación de Cobertura | Gestión de Coberturas |
| BBV-73 | Generar Carta de Compromiso | Gestión Cartas de Compromiso |

### Roles / Lanes

| Lane | Rol |
|---|---|
| 1 | Analista de Vivienda |
| 2 | Gestor Constructor |
| 3 | Gestión de Subprocesos (subprocesos hijos) |
| 4 | Comercial |

### Flujo

```
[Inicio]
  → (gateway ×) ← recibe señal "Gestionar Escalamientos"
  → ¿Aplica Fast Track?
      ├─ SÍ → Fast Track (subproceso) → [Fin]
      └─ NO → Gestionar Escalamientos
               → ¿Requiere Escalamiento?
                   ├─ SÍ → (gateway ○ paralelo — abre subprocesos)
                   └─ NO → ¿Inmueble Definido?
                              ├─ SÍ → ¿Requiere Escalamiento Comercial?
                              │         ├─ NO → [Fin]
                              │         └─ SÍ → señal "Devolución VB Comercial"
                              └─ NO → Definir Inmueble → [Fin]

[Gateway ○ paralelo — Subprocesos simultáneos]:
  ├─ Gestión Seguros
  ├─ Gestión EETT
  ├─ Gestión Avalúos
  ├─ Gestión de Activos
  ├─ Gestión Leasing
  ├─ Gestión Compra de Cartera
  ├─ Gestión de Remodelaciones
  ├─ Gestión de Cobertura
  └─ Realizar VoBo Constructor (Gestor Constructor, solo CXI)

Todos convergen en → señal "Gestionar Escalamientos" (retorno)

### Comercial (lane 4)
señal "Devolución VB Comercial"
  → Realizar Devolución Pendiente VB Comercial
    → ¿Cliente Continua?
        ├─ SÍ → señal "Gestionar Escalamientos" (retorno)
        └─ NO → [Fin Terminal]
```

---

## 3. Gestión Leasing

### HU asociadas

| HU | Actividad |
|---|---|
| BBV-67 | Realizar Causación |
| BBV-68 | Realizar Orden de Giro |

### HU asociadas — Gestión CXI / Causar

| HU | Actividad |
|---|---|
| BBV-87 | Realizar VB Prorrata |
| BBV-88 | Realizar Causar |
| BBV-89 | Revisar EP Abogado (Gestión CXI) |
| BBV-129 | Gestionar Crédito CXI |

### Roles / Lanes

| Lane | Rol |
|---|---|
| 1 | Analista Leasing |
| 2 | Control Leasing |

### Flujo

```
[Inicio]
  → ¿Es un proceso Condicionado con Orden realizada?
      ├─ SÍ → (gateway × merge)
      └─ NO → ¿Realiza Orden de Giro?
                ├─ SÍ → (gateway × merge)
                └─ NO → Realizar Causación
                          → ¿Requiere Devolución?
                              ├─ SÍ → [Fin] (retorno)
                              └─ NO → (gateway × merge)

(gateway × merge)
  → Realizar Orden de Giro (Control Leasing)
    → ¿Requiere Devolución?
        ├─ NO → [Fin]
        └─ SÍ → Realizar Causación (loop)
```

---

## 4. Gestión de Cobertura

### HU asociadas

| HU | Actividad |
|---|---|
| BBV-75 | Marcar Cobertura |
| BBV-107 | Revisar Marcación de Cobertura |

### Roles / Lanes

| Lane | Rol |
|---|---|
| 1 | Analista Cobertura |

### Flujo

```
[Inicio]
  → (gateway ×) ← recibe retorno de "espera de tiempos"
  → Reservar Cobertura
    → ¿Requiere Devolución?
        ├─ SÍ → [Fin Terminal]
        └─ NO → Marcar Cobertura
                  → ¿Requiere espera de tiempos?
                      ├─ SÍ → (loop: vuelve al gateway ×)
                      └─ NO → [Fin]
```

**Nota:** Aplica únicamente para Gestión de Subprocesos.

---

## 5. Gestión Cartas de Compromiso

### HU asociadas

| HU | Actividad |
|---|---|
| BBV-73 | Generar Carta de Compromiso |

### Roles / Lanes

| Lane | Rol |
|---|---|
| 1 | Analista de Vivienda |

### Flujo

```
[Inicio] — Se habilita cuando la constructora solicita la carta
  → Generar Carta de Compromiso
    → [Fin]
```

---

## Notas de negocio (del diagrama)

- Si es CXI no aplica Gestión de Avalúos.
- Si aplica remodelación, se hace verificación manual para el avalúo.
- Aplica Fast Track únicamente para ciertos tipos de crédito.
- Gestión de Activos: aplica minuta de transferencia (aumenta documentos).
- Gestión Seguros: se envía notificación al cliente cuando aplica exámenes médicos.
- VoBo Constructor: aplica únicamente posterior a Validar Condiciones Desembolso.
- Gestión de Cobertura: aplica MiCasaYa, Reduce Tu Cuota.
- MCY requiere: Formulario de Vinculación, Carta Compensación, Form. Solicitud MCY.
- MCY: Carta firmada de No Concurrencia cuando no requiere caja de compensación.

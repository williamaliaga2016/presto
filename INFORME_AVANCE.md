# Informe de Avance — Escrituración y Garantías BBVA Legalización

**Fecha:** 25 de julio de 2026
**Proyecto:** Presto Legalización — BBVA Colombia
**Subproceso:** Escrituración y Garantías

---

## Resumen Ejecutivo

De las 12 HU del subproceso de Escrituración y Garantías, **9 están implementadas** (backend + frontend + SQL) y **3 tienen receta lista** pendiente de implementación.

---

## HU Implementadas (Código en repositorio)

| # | HU | Actividad | Rol | Backend | Frontend | SQL | Observaciones |
|---|---|---|---|---|---|---|---|
| 1 | BBV-86 | Firmar Escritura Cliente | Analista Vivienda | ✅ | ✅ | ✅ | Implementación previa. Se corrigió bug de `.valor` → `.code` en esta sesión. |
| 2 | BBV-90 | Realizar Devolución EP / VB Comercial | Comercial | ✅ | ✅ | ✅ | Implementada por compañero. Tabla `devolucion_vb_comercial`. |
| 3 | BBV-91 | Firmar Rep. Legal | Rep. Legal | ✅ | ✅ | ✅ | Datos heredados de BBV-86 conectados en esta sesión. |
| 4 | BBV-92 | Realizar Entrega EP Firmada | Analista Vivienda | ✅ | ✅ | ✅ | Incluye lógica de excepción paralela + datos heredados. |
| 5 | BBV-93 | Realizar Recepción Boleta (OCR) | Analista Vivienda | ✅ | ✅ | ✅ | Placeholder VUR (endpoint listo, integración RPA pendiente). Oficinas L45 reales cargadas. |
| 6 | BBV-95 | Realizar EP Registradas | Analista Vivienda | ✅ | ✅ | ✅ | Flujo lineal. L53 removida del alcance. |
| 7 | BBV-96 | Realizar VB Final Abogado | Abogado | ✅ | ✅ | ✅ | Enrutamiento triple + banderas backend. Datos heredados con notaría + boleta. |
| 8 | BBV-99 | Validar Condiciones Desembolso | Analista Vivienda | ✅ | ✅ | ✅ | Suspensión condicional + conteo caídas + datos heredados. |
| 9 | BBV-130 | Revisar EP Abogado | Abogado | ✅ | ✅ | ✅ | Implementación previa (compañero). |

---

## HU con Receta lista (pendientes de implementación)

| # | HU | Actividad | Rol | Receta | Complejidad | Notas |
|---|---|---|---|---|---|---|
| 1 | BBV-94 | Realizar Excepción Desembolso | Comercial | ❌ Sin receta | Media | Falta HU en `hu_texto/`. Destino de BBV-92 y BBV-93. |
| 2 | BBV-97 | Realizar Gestión Comercial | Comercial | ✅ Receta lista | Alta | Retorno a origen + Fin Terminal + modal confirmación + soft-delete. |
| 3 | BBV-141 | Realizar Vobo Gerencia COH | Gerencia | ❌ Sin receta | Baja | No tiene receta ni implementación. |

---

## Detalle por HU implementada

### BBV-86 — Firmar Escritura Cliente
- **Archivos backend:** Entity, Repository, Application, Controller (preexistentes)
- **Corrección en sesión:** Bug `t.valor` → `t.code` en `EsTipoLeasing`/`EsTipoCXI`
- **Tiempo estimado:** 30 min (solo fix)

### BBV-91 — Firmar Rep. Legal
- **Archivos backend:** 8 archivos (Entity, EntityConfig, Repository, Domain, Application, Controller)
- **Frontend:** 9 archivos (models, api, hooks x4, components x2, page)
- **Conexión datos heredados:** Hook `useDatosHeredadosFirmarRepLegal` que consume endpoint de BBV-86
- **Tiempo estimado:** 2h

### BBV-92 — Realizar Entrega EP Firmada
- **Archivos backend:** 8 archivos completos
- **Frontend:** 9 archivos + route
- **Lógica especial:** Cálculo `aplica_excepcion` por tipo_credito + enrutamiento paralelo condicional
- **Datos heredados:** Concepto firma Rep. Legal + datos notaría (endpoint compuesto)
- **Tiempo estimado:** 2.5h

### BBV-93 — Realizar Recepción Boleta (OCR)
- **Archivos backend:** 8 archivos + endpoint `/ejecutar-vur`
- **Frontend:** 11 archivos (incluye hook VUR + VurSection con botón estilizado)
- **Lógica especial:** Placeholder VUR con reintento 3x + cálculo excepción + enrutamiento paralelo
- **Catálogos:** L44 (Tipo Boleta, 3 valores) + L45 (Oficinas Registrales, 195 valores reales)
- **UI:** Botón VUR con ícono robot + Dropdown con buscador para oficinas
- **Faltante menor:** Integración real con servicio RPA (queda como TODO)
- **Tiempo estimado:** 3h

### BBV-95 — Realizar EP Registradas
- **Archivos backend:** 8 archivos
- **Frontend:** 10 archivos
- **Lógica:** Flujo lineal (único destino). Toggle confirmación EP + campos fecha/causal.
- **Datos heredados:** Cliente + Notaría + Boleta (3 repositorios) con resolución de descripciones de catálogos
- **L53 removida:** Tipologías Garantías eliminada del alcance por indicación del negocio
- **Tiempo estimado:** 2h

### BBV-96 — Realizar VB Final Abogado
- **Archivos backend:** 8 archivos
- **Frontend:** 10 archivos
- **Lógica compleja:** Enrutamiento triple condicional (Devolución / Validar Desembolso / Control Garantías)
- **Banderas backend:** `origen_tramite` + `bandera_excepcion` calculadas con `IsCompleteActivity`
- **Datos heredados:** Notaría + Boleta + EP Registradas (dinámico según origen)
- **Catálogos:** Reutiliza L39/L40 (Tipología/Casuística — listas independientes, no dependientes)
- **Tiempo estimado:** 3h

### BBV-99 — Validar Condiciones Desembolso
- **Archivos backend:** 8 archivos + endpoint `/suspender`
- **Frontend:** 9 archivos (incluye hook suspender)
- **Lógica especial:** Suspensión condicional por actividades paralelas + conteo caídas + herencia multivía (4 orígenes)
- **UI:** Botón "Suspender" condicionado + Badge caídas + alerta suspensión + toggle plan pagos + toggle escalamiento
- **Datos heredados:** Concepto jurídico del VB Final Abogado + procedencia
- **Tiempo estimado:** 2.5h

---

## Archivos modificados transversalmente (en cada HU)

| Archivo | Modificaciones acumuladas |
|---|---|
| `Constants.cs` | +15 actividades, +12 transiciones, +3 catálogos |
| `IoCRegisterMultibanca.cs` | +7 Applications, +7 Repositories |
| `AutoMapperProfileMultibanca.cs` | +7 mappings |
| `MultibancaDBContext.cs` | +7 DbSets, +7 EntityConfigs |
| `Routes.tsx` | +6 rutas nuevas |

---

## Pendientes técnicos identificados

| Item | HU | Severidad | Detalle |
|---|---|---|---|
| Integración VUR real | BBV-93 | Media | El endpoint `/ejecutar-vur` es placeholder. Falta contrato del servicio RPA. |
| Validación documento Expediente Digital | BBV-93, BBV-95, BBV-96 | Baja | CA07 de varias HU pide validar documentos adjuntos. No existe servicio para consultar. |
| Conteo caídas dinámico | BBV-99 | Baja | Se almacena en tabla; se podría calcular contando en `actividades`. |
| Tipo documento muestra ID | BBV-93/95 | Baja | En expediente 202 pruebas, `tipo_id_t1 = "1"` no matchea catálogo real. Dato de prueba incorrecto. |

---

## Tiempo total estimado de la sesión

| Concepto | Tiempo |
|---|---|
| Debugging BBV-86 (bug `.valor`) | 30 min |
| Implementación BBV-91 + datos heredados | 2h |
| Implementación BBV-92 | 2.5h |
| Implementación BBV-93 (más compleja) | 3h |
| Implementación BBV-95 | 2h |
| Implementación BBV-96 | 3h |
| Implementación BBV-99 | 2.5h |
| Recetas (BBV-93, 95, 96, 97, 99) | 2h |
| Soporte pruebas (scripts workflow, fixes) | 1.5h |
| **Total estimado** | **~19h** |

---

## Próximos pasos sugeridos

1. Ejecutar scripts SQL pendientes (BBV-95, 96, 99) en BD de pruebas
2. Ejecutar scripts de workflow para transiciones de BBV-95 y BBV-96
3. Probar flujo completo: BBV-92 → BBV-93 → BBV-95 → BBV-96 → BBV-99
4. Implementar BBV-97 (Gestión Comercial) — receta lista
5. Definir alcance de BBV-94 (Excepción Desembolso) y BBV-141 (Vobo Gerencia COH)

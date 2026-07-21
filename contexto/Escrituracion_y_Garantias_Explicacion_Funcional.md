# Escrituración y Garantías — Explicación Funcional

## 1. ¿Qué actividad da origen a Escrituración y Garantías?

En el proceso general **Presto Legalización**, la secuencia anterior es:

```text
Gestión de Subprocesos
        ↓
¿Continúa caso?
        ↓ Sí
Cumplimiento
        ↓
¿Continúa caso?
        ├─ No → Fin
        └─ Sí → Escrituración y Garantías
```

Por tanto, **Escrituración y Garantías no nace directamente desde Gestión de Subprocesos**. Antes debe pasar por **Cumplimiento**.

La transición exacta que lo activa en el proceso principal es:

```text
Cumplimiento
      ↓
¿Continúa caso?
      ↓ Sí
Escrituración y Garantías
```

### Evento de negocio recomendado para implementarlo

Aunque en Bizagi aparece como una compuerta llamada **¿Continúa caso?**, para la implementación conviene definir un evento más explícito:

> **Caso habilitado para Escrituración y Garantías**

Este evento debería generarse cuando:

- Terminó la validación de cumplimiento.
- No existe un desistimiento definitivo.
- El cliente continúa con la operación.
- Se resolvieron los escalamientos obligatorios.
- Se cuenta con la información mínima del inmueble.
- Los subprocesos exigidos alcanzaron el estado requerido para continuar.

En términos de integración:

```text
Proceso Cumplimiento
        ↓
Resultado: APROBADO / CONTINÚA
        ↓
Evento: CASO_HABILITADO_PARA_ESCRITURACION
        ↓
Inicia Escrituración y Garantías
```

---

## 2. ¿Qué ocurre justo antes, dentro de Cumplimiento?

El proceso de **Cumplimiento** comienza validando las políticas:

```text
Validar Cumplimiento de Políticas
                 ↓
¿Requiere escalamiento?
```

Si no requiere escalamiento, verifica:

```text
¿Inmueble definido?
```

Si el inmueble está definido, revisa:

```text
¿Requiere escalamiento comercial?
```

Cuando no se requiere escalamiento comercial, el subproceso de Cumplimiento llega a **Fin** y retorna al proceso principal.

El proceso principal recibe ese resultado y pregunta:

```text
¿Continúa caso?
```

Si la respuesta es **Sí**, se invoca **Escrituración y Garantías**.

Por ello, el origen lógico completo es:

```text
Validación de políticas satisfactoria
              ↓
Inmueble definido o gestionado
              ↓
Escalamientos resueltos
              ↓
Fin de Cumplimiento
              ↓
¿Continúa caso? = Sí
              ↓
Inicio de Escrituración y Garantías
```

---

## 3. Inicio interno de Escrituración y Garantías

Una vez invocado el proceso, aparece un evento de inicio sin nombre. Su primera validación es:

```text
Inicio de Escrituración
          ↓
¿Inmueble de Barranquilla / Mi Techo Propio?
```

Esta decisión parece determinar si antes de continuar se debe revisar la cobertura.

### Ruta especial

Cuando el inmueble corresponde a esa condición:

```text
¿Inmueble de Barranquilla / Mi Techo Propio?
                   ↓ Sí
Revisar Marcación de Cobertura
                   ↓
Validar Condiciones de Desembolso
```

También puede existir una derivación a **Gestión de Cobertura** cuando la cobertura todavía debe gestionarse o corregirse.

### Ruta general

En la ruta normal se evalúa:

```text
¿Requiere escrituración?
```

Y se divide en dos escenarios:

```text
¿Requiere escrituración?
        ├─ Sí → Firmar Escritura Cliente
        └─ No → Preformalizar
```

Esto significa que **no todas las operaciones necesariamente pasan por firma de escritura**. Algunas pueden dirigirse a preformalización, según el producto o condición del crédito.

---

## 4. Flujo principal cuando sí requiere escrituración

La ruta principal es:

```text
Firmar Escritura Cliente
          ↓
¿Es un CXI?
```

### Si es CXI

```text
Firmar Escritura Cliente
          ↓
¿Es un CXI? = Sí
          ↓
Gestión CXI - Causar
          ↓
¿EP conforme?
```

CXI tiene su propio subproceso, donde pueden intervenir:

- Gestión del crédito CXI.
- Visto bueno de prorrata.
- Causación.
- Gestión Leasing.
- Revisión de Escritura Pública.

### Si no es CXI

La ruta continúa verificando si se requiere gestión comercial:

```text
¿Es un CXI? = No
        ↓
¿Requiere escalamiento comercial?
```

Si no requiere escalamiento:

```text
Revisar EP Abogado
        ↓
¿EP conforme?
```

EP se interpreta en el modelo como **Escritura Pública**.

---

## 5. Revisión de la Escritura Pública

El abogado revisa el documento:

```text
Revisar EP Abogado
        ↓
¿EP conforme?
```

### Si la escritura está conforme

```text
¿EP conforme? = Sí
        ↓
Firmar Representante Legal
```

Después se evalúa:

```text
¿EP firmada?
```

### Si la escritura no está conforme

Puede pasar a:

```text
¿EP conforme? = No
        ↓
Realizar Devolución EP
```

La devolución no necesariamente termina el proceso. Significa que la escritura debe corregirse y luego regresar a revisión.

Dependiendo de la observación, puede evaluarse:

```text
¿Requiere escalamiento comercial?
```

Si requiere escalamiento:

```text
Realizar Gestión Comercial
          ↓
¿Continúa caso?
```

Las alternativas que aparecen en el modelo son:

- Volver a **Firmar Escritura**.
- Volver a **Realizar Devolución EP**.
- Ir a **Validar Condiciones de Desembolso**.
- Terminar en **Fin Terminal** si el caso no continúa.

---

## 6. Firma del representante legal

Cuando la escritura es conforme:

```text
Firmar Representante Legal
             ↓
¿EP firmada?
```

### Si la EP no fue firmada

```text
¿EP firmada? = No
        ↓
Realizar Devolución EP
```

### Si la EP fue firmada

```text
¿EP firmada? = Sí
        ↓
¿Aplica excepción de desembolso?
```

También aparece una salida hacia **Preformalizar**, que representa un traspaso hacia otro momento o etapa del expediente según la condición de la operación.

---

## 7. Excepción de desembolso

Después de la firma se evalúa:

```text
¿Aplica excepción de desembolso?
```

### Cuando no se necesita excepción

El proceso puede continuar con:

```text
Realizar Entrega EP Firmada
           ↓
Realizar Recepción de Boleta
```

### Cuando sí aplica excepción

Se ejecuta:

```text
Realizar Excepción de Desembolso
                 ↓
¿Requiere visto bueno de Gerencia?
```

Si necesita visto bueno:

```text
Realizar Visto Bueno Gerencia COH
```

Luego la ruta se reincorpora al flujo de registro o validación correspondiente.

Esto debería modelarse como una autorización excepcional, con:

- Motivo de la excepción.
- Usuario solicitante.
- Nivel de aprobación.
- Responsable de aprobación.
- Fecha y hora.
- Evidencias adjuntas.
- Resultado aprobado o rechazado.

---

## 8. Entrega y registro de la escritura

Una vez firmada la escritura:

```text
Realizar Entrega EP Firmada
           ↓
Realizar Recepción Boleta
           ↓
¿Aplica excepción de desembolso?
           ↓
Realizar EP Registradas
```

Después:

```text
Realizar EP Registradas
          ↓
Realizar VB Final Abogado
```

Aquí se confirma que el documento ha sido registrado y que jurídicamente puede continuar.

El flujo sería:

```text
Escritura firmada
       ↓
Entrega de escritura
       ↓
Recepción de boleta
       ↓
Registro de Escritura Pública
       ↓
Visto bueno final del abogado
```

---

## 9. Visto bueno final y posibles devoluciones

Después del registro:

```text
Realizar VB Final Abogado
          ↓
¿Requiere devolución?
```

### Si requiere devolución

```text
¿Requiere devolución? = Sí
          ↓
Realizar Devolución EP
```

La operación vuelve al punto que debe subsanar la observación.

### Si no requiere devolución

El caso puede continuar hacia:

- Validación de condiciones de desembolso.
- Gestión de control de garantías.

En el modelo existen varias rutas que convergen hacia estas actividades porque depende de si el crédito ya fue desembolsado, si tiene excepción o si todavía falta una validación.

---

## 10. Validación de condiciones de desembolso

La actividad central es:

```text
Validar Condiciones de Desembolso
                ↓
¿Requiere escalamiento comercial?
```

Si hay observaciones, el caso puede pasar a gestión comercial.

Cuando el caso vuelve de Gestión Comercial, se evalúa:

```text
¿Continúa caso?
```

Las respuestas pueden conducir a:

- Firmar escritura nuevamente.
- Corregir la Escritura Pública.
- Revalidar las condiciones de desembolso.
- Terminar el expediente.

---

## 11. Gestión de Subprocesos desde Escrituración

Dentro de Escrituración existe una llamada nuevamente a:

```text
Gestión de Subprocesos
```

Esto sucede cuando, durante las validaciones finales, se detecta que todavía falta resolver algún requisito externo.

La ruta modelada es:

```text
¿Requiere escalamiento comercial? = No
                  ↓
Gestión de Subprocesos
                  ↓
¿Continúa caso?
```

Es importante porque demuestra que el proceso **no es estrictamente lineal**. Desde Escrituración puede regresar a procesos anteriores, por ejemplo:

- Avalúos.
- Cobertura.
- Gestión comercial.
- Correcciones documentales.
- Preformalización.

Para la implementación, estos retornos deberían tratarse como **estados y reingresos controlados**, no como el inicio de un expediente nuevo.

---

## 12. Desembolso

La ruta principal de desembolso es:

```text
Validar Condiciones de Desembolso
               ↓
Realizar Desembolso
               ↓
¿Cumple desembolso?
```

### Si cumple

El flujo se distribuye hacia:

- Registro y control de cheques.
- Gestión de control de garantías.

### Si no cumple

```text
¿Cumple desembolso? = No
          ↓
Preformalizar
```

También existe una decisión:

```text
¿Continúa caso?
```

- **Sí:** intenta o ejecuta el desembolso.
- **No:** termina como **Fin Terminal**.

Para implementarlo, el desembolso debería considerarse exitoso únicamente cuando se cuente con una confirmación del sistema financiero, no solamente cuando se envíe la solicitud.

Estados recomendados:

```text
PENDIENTE_VALIDACION
APTO_PARA_DESEMBOLSO
DESEMBOLSO_SOLICITADO
DESEMBOLSO_CONFIRMADO
DESEMBOLSO_RECHAZADO
DESEMBOLSO_EN_CORRECCION
```

---

## 13. Registro y control de cheques

Cuando el desembolso lo requiere:

```text
Realizar Desembolso
         ↓
Registro y Control de Cheques
```

Después, la ruta se une con el control de garantías.

Esta actividad debe controlar, como mínimo:

- Número o identificación del cheque.
- Beneficiario.
- Importe.
- Fecha de emisión.
- Estado.
- Responsable.
- Confirmación de entrega o aplicación.
- Evidencia documental.

---

## 14. Gestión de Control de Garantías

Después del desembolso o del registro de la escritura aparece:

```text
Gestionar Control de Garantías
             ↓
¿Requiere corrección de registro?
```

### Si requiere corrección

```text
¿Requiere corrección de registro? = Sí
                 ↓
Realizar VB Final
                 ↓
Realizar VB Final Abogado
```

Es decir, la corrección vuelve a ser revisada jurídicamente.

### Si no requiere corrección

Se evalúa:

```text
¿Requiere gestión de avalúo?
```

#### Si requiere avalúo

```text
Gestión Avalúos
       ↓
Gestionar Control de Garantías
```

Después de resolver el avalúo vuelve nuevamente al control de garantías.

#### Si no requiere avalúo

La operación continúa hacia la convergencia final:

```text
Enviar a Custodio
```

---

## 15. ¿Cuál es el final real de Escrituración y Garantías?

El **final operativo exitoso** del proceso interno es:

```text
Gestionar Control de Garantías
             ↓
Validaciones o correcciones resueltas
             ↓
Enviar a Custodio
             ↓
Fin
```

Por tanto, la última actividad de negocio es:

> **Enviar a Custodio**

Y el resultado final debería ser:

> **Documentación y garantías enviadas a custodia satisfactoriamente.**

Ese es el verdadero cierre funcional de Escrituración y Garantías.

### Evento final recomendado

```text
GARANTIAS_ENVIADAS_A_CUSTODIA
```

O un nombre más general:

```text
ESCRITURACION_Y_GARANTIAS_COMPLETADA
```

El evento debería generarse cuando:

- La Escritura Pública está firmada, cuando corresponda.
- El registro está confirmado.
- Se obtuvo el visto bueno jurídico final.
- El desembolso se realizó o quedó correctamente confirmado.
- Las garantías están completas.
- No existen correcciones pendientes.
- Los documentos fueron remitidos al custodio.
- Existe evidencia de recepción o radicación en custodia.

---

## 16. ¿Qué significa el Fin Terminal posterior en Presto Legalización?

En el proceso padre aparece:

```text
Escrituración y Garantías
            ↓
Fin Terminal
```

Por la posición en el diagrama, ese **Fin Terminal** no parece significar necesariamente una cancelación. Representa que al terminar Escrituración y Garantías ya no queda otro subproceso dentro de **Presto Legalización**.

Es decir:

```text
Fin interno de Escrituración:
Enviar a Custodio → Fin

Fin del proceso general:
Escrituración y Garantías completada → Fin Terminal de Presto Legalización
```

Sin embargo, el nombre **Fin Terminal** es ambiguo, porque también se utiliza en algunas rutas de desistimiento.

Para la implementación sería mejor diferenciar:

| Estado final | Significado |
|---|---|
| `COMPLETADO` | Legalización finalizada correctamente |
| `DESISTIDO` | Cliente o negocio decidió no continuar |
| `CANCELADO` | Expediente cancelado por una decisión formal |
| `RECHAZADO` | No cumplió las condiciones |
| `EN_CUSTODIA` | Garantías entregadas al custodio |
| `PENDIENTE_CORRECCION` | Existe una devolución por resolver |

No conviene utilizar el mismo estado **Fin Terminal** para una operación completada y para una operación desistida.

---

## 17. Flujo integral resumido

```text
CUMPLIMIENTO
      ↓
Cumplimiento finalizado
      ↓
¿Continúa caso?
      ├─ No → Fin del expediente
      └─ Sí
           ↓
ESCRITURACIÓN Y GARANTÍAS
           ↓
Validaciones iniciales
           ↓
¿Requiere escrituración?
      ┌────┴────┐
      │         │
     Sí         No
      │         │
Firmar EP   Preformalizar
      ↓
¿Es CXI?
      ├─ Sí → Gestión CXI - Causar
      └─ No → Revisión normal
                    ↓
            Revisar EP Abogado
                    ↓
              ¿EP conforme?
            ┌───────┴────────┐
            │                │
           No                Sí
            │                │
 Devolver/corregir     Firmar Rep. Legal
                             ↓
                      ¿EP firmada?
                             ↓
                Excepción de desembolso
                             ↓
                   Entrega EP firmada
                             ↓
                   Recepción de boleta
                             ↓
                    Registrar escritura
                             ↓
                  VB final del abogado
                             ↓
             Validar condiciones desembolso
                             ↓
                    Realizar desembolso
                             ↓
               Registro/control de cheques
                             ↓
                Control de garantías
                             ↓
        Correcciones o avalúos, si corresponden
                             ↓
                    Enviar a custodio
                             ↓
                            FIN
```

---

## 18. Definición precisa para la implementación

### Inicio

> El proceso se inicia cuando Cumplimiento concluye satisfactoriamente y el proceso principal determina que el caso continúa.

### Entrada

```text
CASO_HABILITADO_PARA_ESCRITURACION
```

### Última actividad

```text
Enviar a Custodio
```

### Resultado exitoso

```text
ESCRITURACION_Y_GARANTIAS_COMPLETADA
```

### Final funcional

> Escritura, desembolso y garantías procesados; expediente documental enviado a custodia.

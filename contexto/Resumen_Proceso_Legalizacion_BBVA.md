# Resumen del proceso de Legalización BBVA

La confusión viene de que el modelo de Bizagi contiene **16 diagramas**, pero no significa que los 16 se ejecuten uno después de otro. Existe un **proceso principal** y varios **subprocesos opcionales** que se activan según las características del crédito.

---

## 1. ¿Dónde inicia todo?

El proceso principal es la página:

### **Presto Legalización**

Todo comienza con la llegada de un **crédito aprobado**.

El sistema evalúa:

```text
Crédito aprobado
        ↓
¿Requiere cargue manual?
```

- **No requiere cargue manual:** se cargan automáticamente los créditos aprobados y se genera la carta de aprobación.
- **Sí requiere cargue manual:** el comercial radica el crédito y la carta de aprobación.

Después se evalúa si el crédito puede usar la modalidad **Fast Track**.

---

## 2. Flujo principal completo

Visto de manera simplificada, el proceso es:

```text
1. Crédito aprobado
        ↓
2. Generación o radicación de la carta de aprobación
        ↓
3. Contacto con el cliente
        ↓
4. Gestión de subprocesos necesarios
        ↓
5. Validación de cumplimiento
        ↓
6. Escrituración y garantías
        ↓
7. Desembolso y custodia
        ↓
8. Fin
```

Ese es el hilo principal que debes seguir para comprender todo el modelo.

---

## 3. Primera etapa: carga y carta de aprobación

Dentro de **Presto Legalización**, primero se registra el crédito.

Dependiendo del origen:

- El sistema carga el crédito aprobado.
- O el comercial realiza una radicación manual.
- Se genera la carta de aprobación.
- Se determina si aplica **Fast Track**.
- Luego el caso pasa a **Contacto Cliente**.

También aparecen tareas separadas para:

- Cargar documentos del cliente.
- Cargar documentos del comercial.
- Definir el inmueble mediante el asignador.

Estas actividades pueden producirse durante la preparación inicial del expediente.

---

## 4. Segunda etapa: Contacto Cliente

Esta es una de las páginas más importantes.

El objetivo es confirmar que el cliente y el inmueble cuentan con toda la información necesaria.

El flujo general es:

```text
Contacto con cliente
        ↓
Validar información
        ↓
¿Tiene inmueble definido?
```

### Cuando el inmueble ya está definido

Se revisan:

- Documentos del inmueble.
- Documentos del cliente.
- Documentos de la constructora.
- Soportes de pago.
- Información entregada por prescriptores o brokers.

Luego se verifica:

```text
¿Documentos correctos?
```

Si están correctos:

- Se valida la integración de los documentos.
- Se asignan las firmas valuadoras, peritos y abogados.
- Se define si la firma será física o mediante el mecanismo correspondiente.
- El caso continúa.

Si los documentos presentan observaciones:

- Se genera una devolución.
- El cliente o la constructora corrigen la información.
- El expediente vuelve a validarse.

### Cuando el cliente no tiene inmueble definido

El proceso pasa por:

- Definir inmueble.
- Asignador.
- Motor de asignación/OCR.
- Revisión de información del inmueble.

Después vuelve al flujo de contacto y validación documental.

### Decisión de continuidad

Al final se pregunta:

```text
¿Cliente continúa?
```

- **Sí:** continúa hacia los subprocesos.
- **No:** el caso termina como **Fin Terminal**.

---

## 5. Tercera etapa: Gestión de Subprocesos

Esta página funciona como un **distribuidor de actividades**.

No necesariamente se ejecutan todos los subprocesos. Se activan solamente los que correspondan al crédito.

Los posibles subprocesos son:

| Subproceso | Para qué sirve |
|---|---|
| Gestión de Seguros | Validar seguros y posibles exámenes médicos |
| Gestión de EETT | Elaborar el estudio de títulos |
| Gestión de Activos | Validar inmuebles, activos, cesiones o recolocaciones |
| Gestión Avalúos | Realizar la valoración comercial del inmueble |
| Gestión Leasing | Gestionar causación y orden de giro |
| Compra de Cartera | Validar y radicar ofertas de compra de cartera |
| Remodelaciones | Emitir el visto bueno para remodelaciones |
| Gestión de Cobertura | Reservar y marcar cobertura |
| Fast Track | Ejecutar avalúo y estudio de títulos de manera acelerada |

Por eso esta página parece especialmente enredada: representa varias rutas posibles.

### ¿Cómo se combinan?

Conceptualmente ocurre así:

```text
                 ┌─ Seguros
                 ├─ Estudio de títulos
                 ├─ Avalúo
Expediente ──────┼─ Leasing
                 ├─ Compra de cartera
                 ├─ Cobertura
                 ├─ Activos
                 └─ Remodelaciones
                         ↓
               Reunir resultados
                         ↓
             Gestionar escalamientos
```

Una operación puede requerir solo avalúo y estudio de títulos, mientras otra puede necesitar leasing, seguros, cobertura y otros controles.

---

## 6. ¿Qué hace cada subproceso?

### 6.1 Gestión Avalúos

```text
Asignar perito
    ↓
Programar visita o actualizar avalúo
    ↓
¿Visita completada?
    ↓
Generar informe
    ↓
¿Requiere gestión de riesgos?
```

Si necesita gestión de riesgos, se realiza una consulta de favorabilidad y se genera el informe correspondiente.

También existe una decisión de **reconsideración**, por ejemplo, cuando un resultado debe revisarse nuevamente.

---

### 6.2 Gestión de EETT

Es el proceso más directo:

```text
Inicio
  ↓
Elaborar EETT
  ↓
Retornar al proceso principal
```

EETT se refiere al **Estudio de Títulos**, mediante el cual el abogado revisa la situación jurídica del inmueble.

---

### 6.3 Gestión de Seguros

```text
Validar seguros
      ↓
¿Aplican exámenes médicos?
```

- **No:** termina la validación.
- **Sí:** se solicitan o realizan los exámenes médicos y se espera el resultado.

---

### 6.4 Gestión de Cobertura

```text
Reservar cobertura
       ↓
¿Requiere devolución?
       ↓
Marcar cobertura
       ↓
¿Requiere espera de tiempos?
```

Cuando la gestión necesita más tiempo, el caso queda temporalmente en espera antes de continuar.

---

### 6.5 Gestión Leasing

Incluye dos actividades principales:

- Realizar causación.
- Realizar orden de giro.

El recorrido depende de decisiones como:

```text
¿Es un proceso condicionado?
¿Ya tiene orden realizada?
¿Se debe realizar orden de giro?
¿Requiere devolución?
```

Si hay una devolución, el expediente vuelve a causación o a corrección, según el momento en que se encuentre.

---

### 6.6 Gestión Compra de Cartera

```text
Validar oferta vinculante
        ↓
¿Documentos correctos?
        ↓
Radicar oferta vinculante
        ↓
Esperar respuesta
        ↓
Fin
```

Cuando los documentos no son correctos, deben corregirse antes de radicar la oferta.

---

### 6.7 Gestión de Activos

Se revisan situaciones como:

- Cesiones.
- Recolocaciones.
- Transferencias.
- Cancelación de leasing.
- Remodelaciones.
- Documentos de traspaso.

El resultado central es:

```text
Emitir visto bueno del inmueble
```

---

### 6.8 Gestión de Remodelaciones

Es un proceso breve:

```text
Realizar visto bueno de remodelaciones
                  ↓
                 Fin
```

---

### 6.9 Gestión Fast Track

Fast Track acelera principalmente dos frentes:

```text
Asignar firmas, peritos y abogados
               ↓
        ┌──────┴──────┐
        ↓             ↓
     Avalúo      Estudio de títulos
        └──────┬──────┘
               ↓
             Fin
```

Es decir, avalúo y estudio de títulos pueden avanzar de forma paralela o coordinada.

---

### 6.10 Gestión Cartas de Compromiso

Es un proceso complementario que se habilita cuando la constructora solicita una carta:

```text
Generar carta de compromiso
             ↓
            Fin
```

No forma parte obligatoria del flujo principal.

---

## 7. Cuarta etapa: Cumplimiento

Después de ejecutar los subprocesos requeridos, el caso pasa por una revisión integral.

El objetivo es comprobar que el crédito cumple las políticas y que los resultados de los subprocesos son aceptables.

```text
Validar cumplimiento de políticas
               ↓
¿Requiere escalamiento?
```

### Si no requiere escalamiento

Se verifica si el inmueble está definido y el caso continúa.

### Si requiere escalamiento

Puede pasar a revisión comercial.

Si el comercial o el cliente debe corregir algo:

```text
Desistir o devolver para VB comercial
              ↓
Realizar corrección pendiente
              ↓
¿Cliente continúa?
```

- **Sí:** se vuelve a validar cumplimiento.
- **No:** el caso termina.

Una vez aprobada esta etapa, el expediente pasa a **Escrituración y Garantías**.

---

## 8. Quinta etapa: Escrituración y Garantías

Esta es la página más grande porque abarca desde la preparación de la escritura hasta el desembolso y la custodia.

El flujo principal puede resumirse así:

```text
Validaciones iniciales
        ↓
¿Requiere escrituración?
        ↓
Firmar escritura cliente
        ↓
Revisión de EP por abogado
        ↓
Firma del representante legal
        ↓
Registro de la escritura
        ↓
Validación final
        ↓
Desembolso
        ↓
Control de garantías
        ↓
Enviar a custodio
        ↓
Fin
```

EP parece referirse a la **Escritura Pública**.

---

### 8.1 Validaciones previas

Se revisa:

- Si el inmueble se encuentra en Barranquilla o aplica a Mi Techo Propio.
- Si la cobertura está correctamente marcada.
- Si se necesita un nuevo avalúo.
- Si existen observaciones comerciales.
- Si se requiere escrituración.
- Si el crédito corresponde a un CXI.

Si falta algún requisito, el expediente puede retornar a:

- Gestión de Cobertura.
- Gestión Avalúos.
- Gestión de Subprocesos.
- Gestión Comercial.

---

### 8.2 Firma de escritura

Cuando sí requiere escritura:

```text
Firmar escritura cliente
         ↓
¿Es un CXI?
```

- **Sí:** ingresa a **Gestión CXI – Causar**.
- **No:** continúa con la revisión de la escritura pública.

Luego:

```text
Revisar EP abogado
        ↓
¿EP conforme?
```

Si no está conforme:

- Se devuelve la escritura.
- Se solicita corrección.
- Puede escalarse al comercial.
- Luego vuelve a revisión.

Si está conforme:

- Firma el representante legal.
- Se verifica si la escritura quedó firmada.

---

### 8.3 Registro de escritura

Después de la firma:

- Se entrega la escritura firmada.
- Se recibe la boleta.
- Se realizan las escrituras públicas registradas.
- El abogado realiza el visto bueno final.

Si existe una excepción de desembolso, se gestiona antes de continuar.

---

### 8.4 Condiciones de desembolso

```text
Validar condiciones de desembolso
               ↓
¿Cumple condiciones?
```

- **Sí:** se realiza el desembolso.
- **No:** se devuelve para preformalización, corrección o gestión comercial.

También pueden requerirse:

- Excepción de desembolso.
- Visto bueno de Gerencia COH.
- Validación adicional de cobertura.
- Registro y control de cheques.

---

### 8.5 Control de garantías

Después del desembolso o registro:

```text
Gestionar control de garantías
              ↓
¿Requiere corrección de registro?
```

- Si requiere corrección, se realiza un nuevo visto bueno.
- Si necesita un avalúo adicional, vuelve a Gestión Avalúos.
- Finalmente, los documentos se envían al custodio.

Ahí concluye la legalización.

---

## 9. Subproceso CXI – Causar

Este proceso se activa únicamente cuando la operación se identifica como **CXI**.

De manera resumida:

```text
Gestionar crédito CXI
       ↓
Realizar VB de prorrata
       ↓
¿Requiere causación?
       ↓
Gestión de causación
       ↓
Revisión de EP por abogado
       ↓
¿Dictamen final?
       ↓
Fin o retorno a escrituración
```

También puede interactuar con Gestión Leasing.

---

## 10. Las devoluciones y escalamientos

En casi todas las páginas aparecen dos mecanismos recurrentes.

### Devolución

Se utiliza cuando:

- Falta un documento.
- Un documento es incorrecto.
- La escritura tiene observaciones.
- La cobertura no está lista.
- El avalúo requiere corrección.
- Las condiciones del desembolso no se cumplen.

La devolución normalmente no termina el proceso. Hace que el caso retroceda a la persona responsable para corregirlo.

### Escalamiento comercial

Se utiliza cuando el área operativa no puede resolver directamente una situación.

```text
Observación
    ↓
¿Requiere escalamiento comercial?
    ├─ No → continuar
    └─ Sí → gestión comercial
                  ↓
         ¿Cliente continúa?
```

Si el cliente continúa, el expediente vuelve al punto correspondiente. Si no continúa, termina como **Fin Terminal**.

---

## 11. Diferencia entre “Fin” y “Fin Terminal”

En el modelo parecen representar cosas distintas:

- **Fin:** concluye satisfactoriamente una etapa o subproceso.
- **Fin Terminal:** el expediente completo se detiene, por ejemplo, porque el cliente no continúa o porque el caso fue desistido.

Por eso puedes encontrar varios “Fin” dentro de las páginas sin que necesariamente haya terminado toda la legalización.

---

## 12. Mapa final resumido

```text
CRÉDITO APROBADO
       ↓
Cargar o radicar crédito
       ↓
Generar carta de aprobación
       ↓
¿Aplica Fast Track?
       ↓
CONTACTO CLIENTE
       ↓
Validar cliente, inmueble y documentos
       ↓
¿Cliente continúa?
       ├── NO → Fin Terminal
       └── SÍ
             ↓
GESTIÓN DE SUBPROCESOS
             ├─ Avalúo
             ├─ Estudio de títulos
             ├─ Seguros
             ├─ Cobertura
             ├─ Leasing
             ├─ Compra de cartera
             ├─ Activos
             └─ Remodelaciones
             ↓
CUMPLIMIENTO
       ↓
¿Cumple políticas?
       ├── NO → Devolución o escalamiento
       └── SÍ
             ↓
ESCRITURACIÓN Y GARANTÍAS
       ↓
Firma cliente
       ↓
Firma representante legal
       ↓
Registro de escritura
       ↓
Validar condiciones de desembolso
       ↓
Realizar desembolso
       ↓
Control de garantías
       ↓
Enviar documentos a custodio
       ↓
FIN
```

---

## 13. Orden recomendado para revisar el Draw.io

La forma más fácil de revisar el archivo Draw.io es comenzar por la página **“Presto Legalización”**.

Desde ahí, revisar en este orden:

1. **Presto Legalización**
2. **Contacto Cliente**
3. **Gestión de Subprocesos**
4. **Cumplimiento**
5. **Escrituración y Garantías**

Las demás páginas corresponden a rutas auxiliares o subprocesos que se invocan desde esos cinco procesos principales.

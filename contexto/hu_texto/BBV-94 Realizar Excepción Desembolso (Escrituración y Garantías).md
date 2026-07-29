# Descripción

**Yo como:** Comercial.

**Deseo:** Acceder al acordeón **"Realizar Excepción Desembolso"** para visualizar la información del crédito, confirmar mediante un check de validación que se continúa con la excepción de desembolso, y definir si la operación requiere una aprobación superior por parte de la Gerencia.

**Para:** Autorizar formalmente el salto del proceso registral y avanzar el flujo directamente hacia la validación de desembolsos, o, en caso de requerirse, escalar el trámite hacia la actividad de **"Realizar Vobo Gerencia COH"** para obtener el aval del gerente.

# Alcance

Esta funcionalidad corresponde a una pantalla transaccional resolutiva operada por el área Comercial. Se activa cuando el Analista de Vivienda marcó que el crédito aplicaba para una excepción previa al registro. Su objetivo principal es asegurar la trazabilidad de la autorización comercial mediante un check explícito de confirmación. Con la nueva actualización, esta pantalla se convierte en un nodo de enrutamiento condicionado: evalúa si la excepción puede ser aprobada directamente por el Comercial (pasando a Desembolsos) o si por políticas de atribución requiere ser escalada al Gerente COH para un visto bueno adicional.

# Criterios de Aceptación - Reglas de Negocio

## CA01 (Criterio Global Transversal)

1. El sistema debe renderizar y mantener la estructura visual de los grupos de datos exactamente igual a como venían de la actividad anterior (heredando la vista de Escrituración y Garantías).
2. El sistema debe renderizar en la cabecera la **Información General** (solo lectura).
3. Incorporar **Funciones Transversales**: Expediente Digital y Trazabilidad/Bitácora.
4. Botones: **Guardar** y **Transición / Avanzar**.
5. Al ejecutar **Avanzar** registrar: Fecha, Actividad, Usuario, Decisión de Enrutamiento y Observaciones.

## CA02 (Acordeón "Realizar Excepción Desembolso" y Herencia Dinámica): 
El sistema debe desplegar un acordeón central nombrado "Realizar Excepción Desembolso", Este bloque mostrará en estricto modo de solo lectura la información del caso, adaptándose dinámicamente según su origien:
    - Si proviene de "Firmar Rep. Legal": Mostrará los datos básicos del cliente, la notaría y el concepto de firma exitosa.
    - Si proviene de "Realizar Recepción Boleta": Mostrará adicional a lo anterior, todos los datos de radicación en la oficina de registro (Fecha de ingreso, Radicado, Tipos de boleta, Oficina, etc.).


## CA03 (Registro Informativo de Autorización):
Dentro de la sección operativa, el sistema habilitará un nuevo campo obligatorio denominado "Excepción autorizada" con los valores "Sí"/ "No".
 - Regla de comportamiento: Este campo será de carácter únicamente informativo; no ejecutará ninguna acción de sistema, no habilitará otras pestañas y no afectará el enrutamiento del lfujo. Sirve exclusivamente como constancia de la gestión comercial.

## CA04 (Confirmación de Excepción Comercial):
Adicional al cmapo anterior, el sistema debe habilitar obligatoriamente un control tipo checkbox (o lista desplegable) con el enunciado: "Confirmar continuación con excepción de desembolso". El usuario Comercial deberá marcar obligatoriamente este check para poder accionar el avance de la etapa.

## CA05 (Compuerta de Escalamiento a Gerencia):
El sistema habilitará un nuevo campo de decisión obligatorio denominado "Requiere VoBo Gerencia?" con los valores "Sí"/"No". Este campo determinará la ruta de salida de la activdad.

## CA06 (Lógica de Enrutamiento Condicionado):
Una vez diligenciado los campos, confirmado el check y accionado el botón "Avanzar", el flujo evaluará la respuesta del CA-05.
    - Si se selecciona "SI" en ¿REquiere Vobo gerencia? : El flujo se desvía a una nueva actividad denominada "Realizar VoBo gerencia COH" y el sistema asignará la tarea al rol de Gerente COH.
    - Si selecciona "NO" en ¿Requiere VoBo gerencia?: El flujo saldrá del actividad actual e ingresará directamente a la actividad "Validar Condiciones Desembolso" asignando la tarea al rol "Analista Vivienda"

## CA07 (Bloqueo de avance y origen transparente):
El sistema impedirá que el comercial transite la actividad si no ha activado explicitamente el check de continuación de excepción (CA-04). Adicionalmente, la interfaz no requerirá  que el comercial le indique de dónde viene el caso; el sistema identificará automáticamente la procedencia y precargará los bloques de datos correspondientes.


## CA08 Lógica de escalamiento

La compuerta **¿Requiere Vobo Gerencia?** define el destino del trámite. Es de marcado estricto y obligatorio

## CA09 Continuidad del trámite:
El check de "Confirmar continuación con excepción de desembolso" sigue siendo la garantía legal y comercial de que la liberación de recursos sin registro procede, por lo que es ineludible, independientemente de si el caso escala al gerente o va directo al analista de vivienda.

## CA10 - Mensaje de avance:
Al momento de realizar el avance en el sistema muestra la alerta de Modal que indique "Certifico que el trámite cuenta con: Aprobación del Área de Riesgos de la Operación en los términos indicados (Nacar) Estudio de titulos sin observaciones, Pagaré firmado por el deudor y seguros avalúo, dictamen favorable, sin observaciones, boleta de ingreso a registro con folio previo formato de autorización de desembolso con boleta firmado por el cliente. ¿estás seguro de avanzar?, muestra cancelar y aceptar, aceptar avanza hacia el enrutamiento necesario.

# Modelado de Datos

| Campo | Tipo de Dato | Editable | Obligatorio | Reglas |
|---|---|---|---|---|
| Datos Cliente y Notaría | Varios | No | Sí | Solo lectura. |
| Datos Boleta de Registro | Varios | No | Condicionado | Solo si proviene de Recepción Boleta. |
| Excepción Autorizada | Lista | Sí | Sí | Informativo. |
| Confirmar continuación con excepción de desembolso | Checkbox | Sí | Sí | Obligatorio. |
| ¿Requiere Vobo Gerencia? | Lista | Sí | Sí | Define el enrutamiento. |
| Observaciones de Excepción | Área de texto | Sí | No | Justificación del comercial. |

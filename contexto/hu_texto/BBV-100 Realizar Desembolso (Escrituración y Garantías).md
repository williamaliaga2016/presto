# BBV-100 Realizar Desembolso (Escrituración y Garantías)

> Documento convertido desde PDF para edición y análisis en Kiro.

\n\n--- PAGE page-1.png ---\n
6/7/26, 15:25 [#BBV-100] HU - Actividad Realizar Desembolso (Escrituración y Garantías)
Epica - Presto Escrituración y Garantías BBVA Legalización (sev-122)
‘+ [BBV-100] HU - Actividad Realizar Desembolso (Escrituración y Garantías) creada: 02jun/26 Actualizada: 19/jun/26
Estado: Tareas por hacer
Proyecto: BBVA - Colombia
Componentes: Ninguno
Versiones afectadas: Ninguno
Versiones corregidas: Ninguno
Principal: Épica - Presto Escrituración y Garantías BBVA Legalización
Tipo: Historia Prioridad: Media
Informador: Jorge Andres Garzon Paez Persona asignada: Jorge Andres Garzon Paez
Resolución: Sin resolver Votos: 0
Etiquetas: BBVA_LEGALIZACION
Trabajo restante estimado: Desconocido
Tiempo Trabajado: Desconocido
Estimación original: Desconocido

## Descripción

**Yo como:** Analista de Desembolso / Comercial (Radicación Manual).
**Deseo:** Acceder a la pantalla central de "Realizar Desembolso" para visualizar la información heredada, confirmar si el crédito cumple las condiciones
**Para:** desembolsar, diligenciar los datos financieros (ajustando montos si es necesario), y avanzar el flujo aplicando las reglas de excepciones, tipos de
producto y subsidios correspondientes.
**Para:** Liberar los recursos del crédito adecuadamente, enrutar las devoluciones hacia la preformalización si no cumple, o distribuir sistemicamente las
gestiones operativas finales (cheques, control de garantías o envío de notificaciones).
Esta funcionalidad corresponde a la pantalla transaccional "Core" del subproceso de Desembolso. Actúa como la compuerta principal que consolida la
herencia de datos limpios. Su diseño se basa en un comportamiento dinámico: si el desembolso NO cumple, oculta los campos de captura y devuelve
el flujo; si SÍ cumple, despliega las variables financieras (con reglas de tope máximo en el valor) y, al avanzar, el motor de procesos (backend)
orquesta automáticamente qué actividades paralelas se encienden o apagan basándose en tres variables críticas: la existencia de excepciones, el tipo
de producto (ej. Compra de Cartera) y la aplicación de subsidios (MCY).

## Criterios de aceptación

+ CA01 (Criterio Global Transversal):
1. El sistema debe renderizar y mantener la estructura visual de los grupos de datos exactamente igual a como venían de la actividad anterior
(heredando la vista de Escrituración y Garantías).
2. El sistema debe renderizar en la cabecera la "Información General" (estrictamente de solo lectura).
3. El sistema debe incorporar el contenedor para "Funciones Transversales", dividido en Expediente Digital y Trazabilidad/Bitácora.
4. Funcionalidad de Botones de Acción: Opciones de "Guardado" y "Transición / Avanzar".
5. Trazabilidad: Al ejecutar "Avanzar", se registrará en la bitácora: Fecha, Actividad (Realizar Desembolso), Usuario Ejecutor (Analista de
Desembolso), Decisión de Desembolso y Observaciones.
+ CA02 (Acordeón Principal y Herencia de Datos): El sistema debe desplegar un acordeón principal denominado "Realizar Desembolso”. Este
bloque mostrará en modo de solo lectura la información consolidada proveniente de "Gestionar Escalamiento" o "Preformalizar".
+ CA03 (Compuerta de Desembolso y Ruta Negativa): En la pestaña principal, el sistema habilitará el campo obligatorio "¿Cumple Desembolso?"
(Valores: SÍ / NO).
o Si se selecciona "NO": El sistema oculta automáticamente cualquier campo transaccional adicional. Al accionar "Avanzar", el flujo retorna y
se ingresa directamente a la actividad "Preformalizar".
\n\n--- PAGE page-2.png ---\n
6/7/26, 15:25 [#BBV-100] HU - Actividad Realizar Desembolso (Escrituración y Garantías)

+ CA04 (Despliegue de Campos Transaccionales - Ruta Positiva): Si en el campo "¿Cumple Desembolso?" se selecciona "SÍ", el sistema
desplegará inmediatamente los siguientes campos obligatorios para su diligenciamiento:

o Valor Desembolso: Campo numérico/moneda. Regla especial: Permitirá edición, pero el sistema validará que el monto ingresado pueda ser
inferior, pero nunca superior al monto original aprobado para el crédito.

o Fecha de Desembolso: Selector de calendario.

o Forma de Abono de Recursos Desembolso: Lista de selección múltiple (permite adicionar varias opciones simultáneas) alimentada por la
**Para:** metría L47.

o Tipo de Desembolso: Lista desplegable alimentada por la parametría L48.

+ CA05 (Reglas de Enrutamiento y Avance Condicionado): Una vez diligenciados los campos de la ruta positiva y accionado el botón "Avanzar", el
motor de workflow evaluará las siguientes reglas para habilitar actividades o notificaciones paralelas:

o Regla 1 (Origen y Producto sin Garantía): Si la tarea proviene de la actividad Preformalizar y el tipo de crédito es "Compra de Cartera -
Recolocaciones y Remodelaciones con Garantía Constituida", el sistema NO habilitará la actividad de Gestionar Control de Garantías.
o Regla 2 (Notificación Subsidio MCY): Si se confirma el desembolso y el crédito cuenta con un subsidio MCY (Mi Casa Ya), el sistema
emitirá automáticamente un correo electrónico con la información de la operación dirigido al área de Garantías.
o Regla 3 (Excepciones y VoBo Final): El sistema evaluará si el crédito tuvo una "Excepción Autorizada" en etapas previas:
= Siexistió excepción: Se habilita únicamente la actividad de "Registro y Control de Cheques".
= Si NO existió excepción: El sistema enruta a la vía estándar pero queda en estado de espera hasta obtener el VoBo Final del Abogado
(actividad "Realizar VB Final Abogado").

+ CAD6 - Validación del Tope de Desembolso: El Front-End no permitirá guardar ni avanzar la actividad si el usuario ingresa en el campo "Valor
Desembolso" una cifra que supere el límite máximo aprobado en la estructuración del crédito. Deberá mostrar una alerta indicando: "El valor del
desembolso no puede ser superior al aprobado".

+ CAO7 - Obligatoriedad Condicionada: Los campos L47, L48, Valor y Fecha son estrictamente obligatorios y exigibles únicamente si el usuario
marca "SÍ" en ¿Cumple Desembolso?.

+ CA08 - Obligatoriedad de flujo: El sistema debe de identificar si el proceso fue por Requiere Escrituración Sl y el Analista de Desembolso ya
realizo la Preformalización exitosa, en caso de que no presente la información de Validar Condiciones de Desembolso por el Analista de Vivienda
esta actividad NO debe de activarse y debe esperar la respuesta de ambas áreas (Preformalización y Validar Condiciones de Desembolso) para
ingresar a la actividad.

+ CA09 - Asignación de Rol: El sistema debe de identificar el origen del caso si fue una radicación manual y en caso de que si debe de caer al
Comercial que venía trabajando el caso.


## Modelo de datos


Campo Tipo de Dato Editable Obligatorio Reglas de Negocio / Origen
Bandera: Excepción Booleano No Sí (Backend). Define si se habilita solo Cheques o se espera
VoBo Final.
Bandera: Origen y Tipo Producto Booleano/Texto No Si (Backend). Identifica si es Compra de Cartera desde
Preformalizar.
Bandera: Subsidio MCY Booleano No Sí (Backend). Gatilla el envío de correo al área de Garantías.
¿Cumple Desembolso? Lista (SÍ/NO) Si Si Decision central. Muestra/Oculta los siguientes campos.
o o Visible si Cumple = SÍ. Editable inferior, no superior al
Valor Desembolso Moneda Sí Condicionado
aprobado.
Fecha de Desembolso Fecha Sí Condicionado Visible si Cumple = SÍ.
Forma de Abono de Recursos Selección ;
“ pn Si Condicionado Visible si Cumple = SÍ. Alimentado por Parametria L47.
Desembolso Múltiple
Tipo de Desembolso Lista Despl. Si Condicionado Visible si Cumple = SÍ. Alimentado por Parametria L48.
Observaciones Texto (Área) Sí No Campo libre para asentar consideraciones del desembolso.
rev:c5fe927afdde415031546c632fc491f4848e02e5.

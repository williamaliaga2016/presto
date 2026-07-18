# BBV-102 Gestionar Control de Garantías (Escrituración y Garantías)

> Documento convertido desde PDF para edición y análisis en Kiro.

- -- PAGE page-1.png ---

6/7/26, 15:26 [#BBV-102] HU - Actividad Gestionar Control de Garantías (Escrituración y Garantías)

Epica - Presto Escrituración y Garantías BBVA Legalización (eev122)

+ [BBV-102] HU - Actividad Gestionar Control de Garantías (Escrituración y Garantías) creada: 02/jun/26 Actualizada:

Estado: Tareas por hacer

Proyecto: BBVA - Colombia

Componentes: Ninguno

Versiones afectadas: Ninguno

Versiones corregidas: Ninguno

Principal: Épica - Presto Escrituración y Garantías BBVA Legalización

Tipo: Historia Prioridad: Media

Informador: Jorge Andres Garzon Paez Persona asignada: Jorge Andres Garzon Paez

Resolución: Sin resolver Votos: o

Etiquetas: BBVA_LEGALIZACION

Trabajo restante estimado: Desconocido

Tiempo Trabajado: Desconocido

Estimación original: Desconocido


## Descripción


**Yo como:** Analista de Garantías.

**Deseo:** Acceder al acordeón "Gestionar Control de Garantías” para visualizar la información heredada (desde Realizar Desembolso y/o Realizar VB

Final Abogado), revisar la sábana de datos operativos/financieros del crédito, y definir si existen errores de registro o si se requiere habilitar una

gestión de avalúo.

**Para:** Solicitar correcciones legales al Abogado, disparar el subproceso de Avalúos, o avanzar el flujo exitosamente emitiendo un token que me habilite

la etapa final de "Enviar a Custodio".


## Alcance


Esta funcionalidad corresponde a una pantalla transaccional resolutiva y de validación operada por el Analista de Garantías. Su naturaleza es de

origen dinámico, ya que consolida tareas que provienen de "Realizar Desembolso" o que retornan de "Realizar VB Final Abogado". La pantalla suple

el vacío de información integrando más de 20 variables operativas (CE, Canal, COH, datos de oficina, etc.) bajo una estricta regla de "solo lectura",
permitiendo editar únicamente el Wizard, el estado pendiente y las observaciones. Aplica compuertas de decisión secuenciales para desviar el flujo
hacia áreas legales o técnicas (Avalúos), y culmina generando un token directo para iniciar el empaquetado hacia custodia.


## Criterios de aceptación y reglas de negocio


+ CA01 (Criterio Global Transversal):

1. El sistema debe renderizar y mantener la estructura visual de los grupos de datos exactamente igual a como venían de la actividad anterior
(heredando la vista de Escrituración y Garantías).

2. El sistema debe renderizar en la cabecera la "Información General" (estrictamente de solo lectura).

3. El sistema debe incorporar el contenedor para "Funciones Transversales", dividido en Expediente Digital y Trazabilidad/Bitácora.

4. Funcionalidad de Botones de Acción: Opciones de "Guardado" y "Transición / Avanzar",

5. Trazabilidad: Al ejecutar "Avanzar", se registrará en la bitácora: Fecha, Actividad (Gestionar Control de Garantías), Usuario Ejecutor
(Analista de Garantías), Decisión de control y Observaciones (Nota técnica: Ajustado del original para mantener coherencia de rol en el log
del sistema).

+ CA02 (Acordeón "Gestionar Control de Garantías" y Datos Heredados/Complementarios): El sistema debe desplegar un acordeón nombrado
"Gestionar Control de Garantías”. Este bloque debe heredar el contexto dinámico desde Realizar Desembolso o Realizar VB Final Abogado.
Adicionalmente, deberá cargar la sábana de datos operativos.


- -- PAGE page-2.png ---

6/7/26, 15:26 [ABBV-102] HU - Actividad Gestionar Control de Garantías (Escrituración y Garantías)

o Regla de Inmutabilidad: A excepción del N° Wizard, el estado Pendiente y las Observaciones, ningún otro campo listado en la pantalla
podrá ser modificado por el analista

+ CA03 (Compuerta de Corrección de Registro - Evaluador Primario): Dentro de la sección operativa, se habilitará el campo obligatorio
"¿Requiere Corrección de Registro?" (Valores: SÍ/ NO).

+ Sise selecciona "SÍ" y se acciona "Avanzar": El flujo se redirige a la actividad "Realizar VB Final Abogado”, asignando la tarea al Abogado

+ Se debe desplegar las tipologías relacionadas hacia el abogado siendo la paramétrica L52 Pendiente Garantías

o Si se selecciona "NO": El sistema ocultará las opciones de abogado y desplegará el campo del CA-04.

+ CA04 (Compuerta de Gestión de Avalúo - Evaluador Secundario): Si en el campo anterior se seleccionó "NO", el sistema mostrará
inmediatamente el campo obligatorio "¿Requiere Gestión de Avalúo?” (Valores: SÍ / NO)

o Sise selecciona "SÍ" y se acciona "Avanzar": El sistema habilita y dispara el subproceso paralelo de "Gestión de Avalúos".

o Si se selecciona "NO" y se acciona "Avanzar": El flujo continúa su ciclo regular hacia la custodia (Ver CA-05).

+ CAO5 (Enrutamiento por Token hacia Custodia): Cuando el Analista de Garantías responde "NO" en ambas compuertas y acciona "Avanzar", la
actividad generará automáticamente un Token de Avance. Este token transitará el flujo e ingresará a la actividad "Enviar a Custodio",
manteniendo la tarea asignada al mismo rol de Analista de Garantías.

+ CA06 - Lógica en Cascada: La pregunta "¿Requiere Gestión de Avalúo?" solo debe ser visible si el analista garantizó previamente ("NO") que no
existen problemas de registro.

+ CAQ7 - Carga de Datos y Consistencia: Todos los datos inmutables (CE, Canal, COH, Folio, ele.) deben ser precargados por el Backend desde las
bases maestras del banco. El analista no debe digitarlos

+ CA08 - Funcionalidades de Gestión de Bandejas:

o Gestión y Asignación desde Bandeja General: El sistema consolida todos los casos nuevos en una bandeja global denominada "General",
accesible para todos los analistas de garantías, Al utlizar un botón de asignación sobre un folio seleccionado, el sistema retira
automáticamente el registro de la bandeja "General" y lo traslada de forma exclusiva a la bandeja "Asignadas" del usuario logueado.

o Guardado Parcial de Trámites: El sistema proporciona un botón "Guardar Parcialmente" dentro de la vista de gestión de la bandeja
“Asignadas". Al accionarlo, el sistema almacena todos los avances registrados por el usuario sin dar por finalizado el trámite y mueve
automáticamente el folio a una bandeja global denominada "Parcial".

o Consulta y Reasignación de Avances: El sistema expone la bandeja "Parcial" a todos los analistas de garantías para que puedan consultar
los casos que ya cuentan con avance. Mediante un botón de asignación, el sistema permite que cualquier analista seleccione un folio de esta
bandeja y lo transfiera a su propia bandeja "Asignadas" para continuar la gestión

o Visibilidad Estructural de Seguimiento: El sistema mantiene habilitadas y diferenciadas tres vistas de trabajo para organizar el flujo:
"General" (acceso global para iniciar casos), "Parcial" (acceso global para retomar casos en pausa) y "Asignadas" (acceso personal para la
gestión activa del usuario),

o Restricción de uso compartido asignado: El sistema debe de identificar cuando un Folio fue asignado por la bandeja de General o Parcial y
no debe de permitir que algún usuario pueda sobre escribir o tomar ese caso a menos de que exista una reasignación por el administrador

Tipo de Editable (Sí- . . 7 .
Campo Dato No) Obligatorio Reglas de Negocio / Origen

Folio Presto Alfanumérico NO si Identificador principal del crédito, Solo lectura.
N° Wizard Alfanumérico SÍ Si Editable. Identificador de radicación externa.
CE/CANAL / COH/ CENTRO Alfanumérico NO sí Códigos comerciales y de operación. Solo lectura.
SCORING Numérico NO si Puntaje de evaluación. Solo lectura
FECHA DE DESEMBOLSO / VALOR Varios NO sí Datos financieros de liberación de fondos. Solo lectura.
TITULAR / CC_TITULAR Alfanumérico NO si Datos del cliente. Solo lectura.
CO_OFICINA / NOMBRE_OFICINA Alfanumérico NO Si Radicacion original. Solo lectura.
ZONA / TERRITORIAL Alfanumérico NO sí Geolocalización comercial. Solo lectura.
TIPO_DE_CREDITO /

=—_ Alf é N i Ti te lo lect
TIPO_DE DESEMBOLSO fanumérico NO si ipo de producto y abono. Solo lectura
PROYECTO / CONSTRUCTORA Alfanumérico NO Cond. Visible si aplica (ej. CXI). Solo lectura
DIRECTOR / EJECUTIVO /

Alf é Ni Í Mi if édito. ti
RESPONSABILIDAD \fanumérico NO si lapeo comercial del crédito. Solo lectura
¿Requiere Corrección de Registro? — Lista Despl. — SÍ Sí Valores: "SÍ", "NO". Determina si retorna al Abogado.
en 7 . Si se indica que Requiere Corrección de Registro = SI,
Tipología Devolución Abogado List Despl. SI Condicionado. que Rea! orrecel a
muestra el campo. Parametría L52.


- -- PAGE page-3.png ---

6/7/26, 15:26 [*BBV-102] HU - Actividad Gestionar Control de Garantías (Escrituración y Garantías)
a Visible solo si C ón de Registro = "NO". Si "SÍ"
¿Requiere Gestión de Avalúo? Lista Despl. Si Cond. isible Solo Si Gorrección de Registro men
habilita subproceso Avalúos.
PENDIENTE Lista Despl. — SÍ No Editable. Alimentado por la Parametría (L52).
Editable. C: liby t t
OBSERVACIONES Texto (Área) SÍ No O e Pa sena anotaciones
operativas de la garantía y control.
rev:c5fe927afdde415031546c632fc49114848002e5,

# BBV-91 Firmar Rep. Legal (Escrituración y Garantías)

> Documento convertido desde PDF para edición y análisis en Kiro.

<!-- --- PAGE 1 --- -->


07726, 15:28 {#8BV-91] HU - Actividad Fimar Rep. Legal (Escrituración y Garantias)
Epica - Presto Escrituración y Garantías BBVA Legalización (ssv.12
1, [BBV-91] HU - Actividad Firmar Rep. Legal (Escrituración y Garantías) creada: 02jun26 Actualizada: 17Jun/26
Proyecto: BBVA - Colombia
Principal: Épica - Presto Escrituración y Garantías BBVA Legalización
Tipo Historia Prioridad. Media
Etiquetas: BBVA_LEGALIZACION
Tiempo Trabajado: Desconocido

## Descripción

**Yo como:** Representante Legal.
**Para:** Avanzar el flujo hacia la entrega de la escritura firmada, o enrutar adecuadamente las devoluciones hacia la preformalización y Realizar Entrega
EP Firmada ola revisión del abogado, según coresponda,

## Alcance

Esta funcionalidad corresponde a la pantalla transaccional de formalización donde el Representante Legal interactúa. La interfaz consolida la
dela firma mediante un campo de parametizacién específica y aplicar una compuerta lógica de enrutamiento: ss firma, elfjo avanza hacia

## Criterios de aceptación y reglas de negocio

+ CAO1 (Criterio Global Transversal):
anteriores (heredando la vista de Escrituración y Garantías).
2. El sistema debe renderizar en la cabecera la "información General" (estctamente de solo lectura con los datos más recientes)
3. El sistema debe incorporar el contenedor para "Funciones Transversales”, dividido en Expediente Digital (para visualizar la minuta/escritura)
y Trazabilidad/Bitácora
4. Funcionalidad de Botones de Acción: Opciones de "Guardado" y “Transición / Avanzar"
5. Trazablidad: Al ejecutar "Avanzar, se registrará en a bicora: Fecha, Actividad (Fimar Rep. Lega), Usuario Ejecutor (Representante
Legal), Concepto de Firma y Observaciones
+ CA02 (Acordeón "Firmar Rep. Legal” y Datos Heredados): El sistema debe desplegar un acordeón central nombrado "Firmar Rep. Legal”. En
(Datos del Cliente, Datos dela Notaría, VoBo Prorata, Liquidación Leasing y Concepto de Revisión EP del Abogado)
+ CA03 (Campos Transaccionales de Firma): El acordeón habitar los siguientes campos operativas para el Representante Lega
o Concepto: Lista desplegable alimentada por la Parametría(L41) on las opciones estrictas: “Escntura fmada Conforme"y "Escritura NO
firmada”.
hitpsliergeston tam atassia.neisiiraisuevievsissue-ht/BEV.91/B8V-91 i ve


<!-- --- PAGE 2 --- -->


877726, 15:23 [#8BV.91] HU - Actividad Firmar Rep. Legal (Escrituración y Garantias)
© Despliegue de Novedades: Si el concepto seleccionado es "Escritura NO firmada", el sistema debe renderizar dinámicamente tres campos
obligatorios
+ Tipología: Lista desplegable almentada por la Parametría(L42)
+ Casulstica: Lista desplegable alimentada pora Parametía(L43)
+ Observaciones: Campo de texto libre.
+ CA04 (Enrutamiento por Dovolución / Escritura NO firmada): Al accionar "Avanzar" con el concepto “Escritura NO firmada”, el sistema realiza
+ CAOS (Enrutamiento de Avance / Escritura Firmada): Al accionar "Avanzar" con el concepto "Escritura firmada Conforme”, el sistema transitará el
Entrega EP Firmada" cargo del analista de vivienda,
+ CAOS - Obligatoriedad de Justificación: No será posible avanzar una tarea con concepto "Escritura NO firmada? si as stas de tipología y

## Modelo de datos

Editable (Si- | Obligatorio (Si ,
m Tipo de 0: Reglas de Negocio / Origen
Campo ipo de Dato No) No) teglas de Negocio / Origer
Datos Heredados ' Precargados de las revisiones de Abogado, Leasing y Prrrata
Varios No sí
Consolidados Solo lectura
concept (Firma) Lista si S Parametra (L41). Valores: "Escritura frmada Conforme",
'oncepto (Firma) Desplegable "Escritura NO firmada".
poto Lista : concicionado _ Obligatorio si Concepto ="Escrtura NO firmada”. Parametra
ipologia Demas |! ondiionado ORNS
. Lista Obligatorio si Concepto = "Escritura NO firmada". Parametía
Casuística Desplegable sí Condicionado. Les)
Observaciones Texto (Área) SÍ Condicionado — Obligatorio si Concepto = "Escritura NO firmada” para detalla
la novedad.
Generado alas Mon Jul 06 20:23:04 UTC 2026 por Michael Pulido usando JIRA 1001.0.0-SNAPSHOT100292-
rev.c6te927afdde4 1503 16460632049 148480265,
hitps:ebergestionJtamatlassian netsijiaissueviews:ssue-himl/BBV-91/BBV-91 html ze

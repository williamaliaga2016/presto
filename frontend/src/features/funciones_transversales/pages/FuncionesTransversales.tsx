import { useState } from "react";
import { TabView, TabPanel } from "primereact/tabview";
import BitacoraPage from "@/features/funciones_transversales/components/bitacora/pages/Bitacora";
import HistorialExpedientePage from "@/features/funciones_transversales/components/historial_expediente/pages/HistorialExpediente";
import ExpedienteDigitalPage from "@/features/funciones_transversales/components/expediente_digital/pages/ExpedienteDigital";
import CartaAprobacionBbvaPage from "@/features/funciones_transversales/components/carta_aprobacion_bbva/pages/CartaAprobacionBbva";
import CartaCompromisoPage from "@/features/funciones_transversales/components/carta_compromiso/pages/CartaCompromiso";
import RegistroContactoPage from "@/features/funciones_transversales/components/registro_contacto/pages/RegistroContacto";

type Props = {
  idExpediente: number;
  idActividad: string;
  filter_by_activity?: boolean;
  show_bitacora?: boolean;
  show_carta_aprobacion?: boolean;
  show_carta_compromiso?: boolean;
  locked_categoria_id?: number;
  locked_documento_id?: number;
  onDocumentUploaded?: () => void;
  onDocumentsLoaded?: (docs: { id_tipo_documento: number; estado: string }[]) => void;
};

export default function FuncionesTransversales({
  idExpediente,
  idActividad,
  filter_by_activity = false,
  show_bitacora = true,
  show_carta_aprobacion = true,
  show_carta_compromiso = true,
  locked_categoria_id,
  locked_documento_id,
  onDocumentUploaded,
  onDocumentsLoaded,
}: Props) {

  const [activeIndex, setActiveIndex] = useState(0);

  return (
    <TabView className="mi-tabview"
      activeIndex={activeIndex}
      onTabChange={(e) => setActiveIndex(e.index)}
      scrollable
    >

      <TabPanel header="Expediente Digital" leftIcon="pi pi-list">
        <ExpedienteDigitalPage
          id_expediente={idExpediente}
          activity_id={idActividad}
          filter_by_activity={filter_by_activity}
          locked_categoria_id={locked_categoria_id}
          locked_documento_id={locked_documento_id}
          onDocumentUploaded={onDocumentUploaded}
          onDocumentsLoaded={onDocumentsLoaded}
        />
      </TabPanel>

      {show_bitacora && (
        <TabPanel header="Bitácora" leftIcon="pi pi-list">
          <BitacoraPage
            id_expediente={idExpediente}
            id_actividad={idActividad}
          />
        </TabPanel>
      )}

      <TabPanel header="Historial de Expedientes" leftIcon="pi pi-history">
        <HistorialExpedientePage
          id_expediente={idExpediente}
        />
      </TabPanel>

      <TabPanel header="Registro Contacto" leftIcon="pi pi-phone">
        <RegistroContactoPage
          id_expediente={idExpediente}
          id_actividad={idActividad}
        />
      </TabPanel>

      {show_carta_aprobacion && (
        <TabPanel header="Carta de Aprobación" leftIcon="pi pi-file-edit">
          <CartaAprobacionBbvaPage
            id_expediente={idExpediente}
          />
        </TabPanel>
      )}

      {show_carta_compromiso && (
        <TabPanel header="Carta de Compromiso" leftIcon="pi pi-users">
          <CartaCompromisoPage
            id_expediente={idExpediente}
          />
        </TabPanel>
      )}

    </TabView>
  );
}

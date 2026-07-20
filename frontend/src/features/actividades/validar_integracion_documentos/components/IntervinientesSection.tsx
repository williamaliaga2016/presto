import { useState } from "react";
import { Button } from "primereact/button";
import { Dialog } from "primereact/dialog";
import { InputText } from "primereact/inputtext";
import { DataTable } from "primereact/datatable";
import { Column } from "primereact/column";

import type { Interviniente } from "../models/interviniente";
import { ValidarIntegracionCatalogos } from "../models/catalogo";
import { Dropdown } from "primereact/dropdown";

interface Props {
  data: Interviniente[];
  disabled?: boolean;
  onSave: (data: Interviniente) => void;
  controles: ValidarIntegracionCatalogos
}

const emptyForm: Interviniente = {
  nombre_completo: "",
  tipo_identificacion: "",
  numero_identificacion: "",
  telefono: "",
  correo_electronico: ""
};

export default function IntervinientesSection({
  data,
  disabled = false,
  onSave,
  controles
}: Props) {

  const [visible, setVisible] = useState(false);

  const [form, setForm] = useState<Interviniente>(emptyForm);

  const updateField = (
    field: keyof Interviniente,
    value: string
  ) => {
    setForm(prev => ({
      ...prev,
      [field]: value
    }));
  };


  const handleSave = () => {
    onSave(form);

    setForm(emptyForm);
    setVisible(false);
  };


  return (
    <div className="flex flex-col gap-4">

      <div className="flex justify-between items-center">
        <h4 className="text-blue-800 font-bold tracking-wider border-b pb-1">
          Intervinientes / Apoderados
        </h4>

        { !disabled && 
          <Button
            label="Adicionar Interviniente / Apoderado"
            icon="pi pi-user-plus"
            disabled={disabled}
            onClick={() => setVisible(true)}
          />
        }
        
      </div>


      <DataTable
        value={data}
        emptyMessage="No existen intervinientes registrados."
        paginator={data.length > 10}
        rows={10}
      >

        <Column
          field="nombre_completo"
          header="Nombre Completo"
        />

        <Column
          field="tipo_identificacion_descripcion"
          header="Tipo Identificación"
        />

        <Column
          field="numero_identificacion"
          header="Número Identificación"
        />

        <Column
          field="telefono"
          header="Teléfono"
        />

        <Column
          field="correo_electronico"
          header="Correo Electrónico"
        />

      </DataTable>


      <Dialog
        header="Adicionar Interviniente / Apoderado"
        visible={visible}
        modal
        style={{ width: "450px" }}
        appendTo={document.body}
        onHide={() => setVisible(false)}
        footer={
          <div className="flex justify-end gap-2">

            <Button
              label="Cancelar"
              icon="pi pi-times"
              className="p-button-text"
              onClick={() => {
                setForm(emptyForm);
                setVisible(false);
              }}
            />

            <Button
              label="Guardar"
              icon="pi pi-check"
              severity="success"
              onClick={handleSave}
            />

          </div>
        }
      >
        <div className="flex flex-col gap-3">

          <div className="flex flex-col gap-1">
            <label>
              Nombre Completo *
            </label>

            <InputText
              value={form.nombre_completo}
              className="form-input-presto"
              onChange={(e) =>
                updateField(
                  "nombre_completo",
                  e.target.value
                )
              }
            />
          </div>


          <div className="flex flex-col gap-1">
            <label className="text-xs text-gray-500">Tipo Documento</label>
            <Dropdown
              value={form.tipo_identificacion}
              options={controles.tipo_documento_id}
              optionLabel="description"
              optionValue="code"
              className="form-dropdown-presto"
              onChange={(e) =>
                updateField(
                  "tipo_identificacion",
                  e.target.value
                )
              }
              placeholder="Seleccionar..."
            />
          </div>


          <div className="flex flex-col gap-1">
            <label>
              Número Identificación *
            </label>

            <InputText
              value={form.numero_identificacion}
              className="form-input-presto"
              onChange={(e) =>
                updateField(
                  "numero_identificacion",
                  e.target.value
                )
              }
            />
          </div>


          <div className="flex flex-col gap-1">
            <label>
              Teléfono
            </label>

            <InputText
              value={form.telefono ?? ""}
              className="form-input-presto"
              onChange={(e) =>
                updateField(
                  "telefono",
                  e.target.value
                )
              }
            />
          </div>


          <div className="flex flex-col gap-1">
            <label>
              Correo Electrónico
            </label>

            <InputText
              value={form.correo_electronico ?? ""}
              className="form-input-presto"
              onChange={(e) =>
                updateField(
                  "correo_electronico",
                  e.target.value
                )
              }
            />
          </div>

        </div>

      </Dialog>

    </div>
  );
}
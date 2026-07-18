import { FormEvent } from 'react';
import { useNavigate } from 'react-router-dom';
import { Button } from 'primereact/button';
import { useReasignacionDesestimacion } from '../hooks/useReasignacionDesestimacion';
import type { ControlBaseDTO } from '../models/ControlBaseDTO';

const SUMMARY_MAX_CHARACTERS_OBS = 500;

const getControlId = (item: ControlBaseDTO): number => {
  return Number(item.id ?? 0);
};

const getControlIdBig = (item: ControlBaseDTO): number => {
  return Number(item.idBig ?? 0);
};

const getControlCode = (item: ControlBaseDTO): string => {
  return String(item.code ?? '').trim();
};

const getControlDescription = (item: ControlBaseDTO): string => {
  return String(item.description ?? '');
};

export default function ReasignacionDesestimacionPage() {
  const navigate = useNavigate();

  const {
    form,
    acciones,
    actividades,
    usuariosActividad,
    isEditing,
    isReassign,
    message,
    setIsEditing,
    updateField,
    validateRequestNumber,
    changeAccion,
    changeActividad,
    save,
    reset,
  } = useReasignacionDesestimacion();

  const disabled = !isEditing;
  const cantidadComentarios = form.observaciones.length;

  const handleGuardar = async (event?: FormEvent<HTMLFormElement>) => {
    event?.preventDefault();
    await save();
  };

  const handleEditar = () => {
    setIsEditing(true);
  };

  const handleSalir = () => {
    navigate('/home/bandeja');
  };

  return (
    <div className="p-4">
      <div className="flex items-center gap-2 mb-4 text-sm">
        <span className="font-semibold text-xl text-gray-800">
          🛠️ Utilidades
        </span>
        <span className="text-blue-500 font-bold text-lg">›</span>
        <span className="text-gray-700 text-base">
          Reasignar y Desestimar Actividades
        </span>
      </div>

      <form
        onSubmit={handleGuardar}
        className="bg-white border border-gray-200 rounded-lg shadow-sm"
      >
        <div className="p-4 md:p-6">
          <div className="mb-5 rounded-md border border-blue-200 bg-blue-50 px-4 py-3 text-sm text-gray-800">
            <span className="mr-2">⚠️</span>
            <strong>¡Importante!:</strong> Ingresar el Nro de Solicitud,
            después seleccione la acción que desee realizar sobre la solicitud.
          </div>

          {message && (
            <div
              className={`mb-4 rounded-md border px-4 py-3 text-sm ${
                message.type === 'success'
                  ? 'border-green-200 bg-green-50 text-green-700'
                  : message.type === 'error'
                    ? 'border-red-200 bg-red-50 text-red-700'
                    : 'border-blue-200 bg-blue-50 text-blue-700'
              }`}
            >
              {message.text}
            </div>
          )}

          <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4 mb-4">
            <div>
              <label
                htmlFor="id_expediente"
                className="block text-sm font-semibold text-gray-700 mb-1"
              >
                Número de Solicitud <span className="text-red-500">*</span>
              </label>

              <input
                id="id_expediente"
                name="id_expediente"
                type="text"
                inputMode="numeric"
                className="w-full rounded-md border border-gray-300 px-3 py-2 text-sm focus:outline-none focus:ring-1 focus:ring-blue-500 disabled:bg-gray-100 disabled:text-gray-500"
                disabled={disabled}
                value={form.id_expediente}
                onChange={(event) =>
                  updateField('id_expediente', event.target.value)
                }
                onBlur={validateRequestNumber}
                placeholder="Número de Solicitud"
              />
            </div>

            <div>
              <label
                htmlFor="accion_id"
                className="block text-sm font-semibold text-gray-700 mb-1"
              >
                Acción <span className="text-red-500">*</span>
              </label>

              <select
                id="accion_id"
                name="accion_id"
                className="w-full rounded-md border border-gray-300 px-3 py-2 text-sm focus:outline-none focus:ring-1 focus:ring-blue-500 disabled:bg-gray-100 disabled:text-gray-500"
                disabled={disabled}
                value={form.accion_id}
                onChange={(event) => changeAccion(Number(event.target.value))}
              >
                <option value={0}>Seleccione...</option>

                {acciones.map((accion) => {
                  const id = getControlId(accion);
                  const code = getControlCode(accion);
                  const description = getControlDescription(accion);
                  const accionId = Number(code);

                  return (
                    <option key={id || code} value={accionId}>
                      {description}
                    </option>
                  );
                })}
              </select>
            </div>
          </div>

          {isReassign && (
            <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4 mb-4">
              <div>
                <label
                  htmlFor="actividad_id"
                  className="block text-sm font-semibold text-gray-700 mb-1"
                >
                  Actividad Actual <span className="text-red-500">*</span>
                </label>

                <select
                  id="actividad_id"
                  name="actividad_id"
                  className="w-full rounded-md border border-gray-300 px-3 py-2 text-sm focus:outline-none focus:ring-1 focus:ring-blue-500 disabled:bg-gray-100 disabled:text-gray-500"
                  disabled={disabled}
                  value={form.actividad_id}
                  onChange={(event) =>
                    changeActividad(Number(event.target.value))
                  }
                >
                  <option value={0}>Seleccione...</option>

                  {actividades.map((actividad) => {
                    const id = getControlId(actividad);
                    const idBig = getControlIdBig(actividad);
                    const description = getControlDescription(actividad);
                    const actividadValue = idBig || id;

                    return (
                      <option key={actividadValue} value={actividadValue}>
                        {description}
                      </option>
                    );
                  })}
                </select>
              </div>

              <div>
                <label
                  htmlFor="actividad_usuario_id"
                  className="block text-sm font-semibold text-gray-700 mb-1"
                >
                  Usuario Destino <span className="text-red-500">*</span>
                </label>

                <select
                  id="actividad_usuario_id"
                  name="actividad_usuario_id"
                  className="w-full rounded-md border border-gray-300 px-3 py-2 text-sm focus:outline-none focus:ring-1 focus:ring-blue-500 disabled:bg-gray-100 disabled:text-gray-500"
                  disabled={disabled || !form.actividad_id}
                  value={form.actividad_usuario_id}
                  onChange={(event) =>
                    updateField(
                      'actividad_usuario_id',
                      Number(event.target.value),
                    )
                  }
                >
                  <option value={0}>Seleccione...</option>

                  {usuariosActividad.map((usuario) => {
                    const id = getControlId(usuario);
                    const description = getControlDescription(usuario);

                    return (
                      <option key={id} value={id}>
                        {description}
                      </option>
                    );
                  })}
                </select>
              </div>
            </div>
          )}

          <div className="mb-2">
            <label
              htmlFor="observaciones"
              className="block text-sm font-semibold text-gray-700 mb-1"
            >
              Comentarios <span className="text-red-500">*</span>
            </label>

            <textarea
              id="observaciones"
              name="observaciones"
              className="w-full rounded-md border border-gray-300 px-3 py-2 text-sm focus:outline-none focus:ring-1 focus:ring-blue-500 disabled:bg-gray-100 disabled:text-gray-500"
              disabled={disabled}
              placeholder="Escribe comentario..."
              rows={5}
              maxLength={SUMMARY_MAX_CHARACTERS_OBS}
              value={form.observaciones}
              onChange={(event) =>
                updateField('observaciones', event.target.value)
              }
            />

            <div className="mt-1 text-right text-xs text-gray-500">
              {cantidadComentarios}/{SUMMARY_MAX_CHARACTERS_OBS} caracteres
            </div>
          </div>
        </div>

        <div className="border-t border-gray-200 bg-gray-50 px-4 py-4 md:px-6">
          <div className="form-actions">
            <Button
              type="button"
              label="Editar"
              icon="pi pi-pencil"
              severity="info"
              outlined
              onClick={handleEditar}
              disabled={isEditing}
              className="btn-responsive"
            />

            <Button
              type="submit"
              label="Guardar"
              icon="pi pi-save"
              severity="success"
              disabled={!isEditing}
              className="btn-responsive"
            />

            <Button
              type="button"
              label="Limpiar"
              icon="pi pi-eraser"
              severity="warning"
              outlined
              onClick={reset}
              className="btn-responsive"
            />

            <Button
              type="button"
              label="Salir"
              icon="pi pi-sign-out"
              severity="secondary"
              outlined
              onClick={handleSalir}
              className="btn-responsive"
            />
          </div>
        </div>
      </form>
    </div>
  );
}
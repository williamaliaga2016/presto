import { useEffect, useRef, useState } from 'react';

import { Button } from 'primereact/button';
import { Card } from 'primereact/card';
import { Column } from 'primereact/column';
import { ConfirmDialog, confirmDialog } from 'primereact/confirmdialog';
import { DataTable } from 'primereact/datatable';
import { Dialog } from 'primereact/dialog';
import { InputSwitch } from 'primereact/inputswitch';
import { InputText } from 'primereact/inputtext';
import { Toast } from 'primereact/toast';

import type { administracion_rol } from '../models/administracion_roles_dto';

import { use_administracion_roles } from '../hooks/use_administracion_roles';
import { use_create_administracion_rol } from '../hooks/use_create_administracion_rol';
import { use_update_administracion_rol } from '../hooks/use_update_administracion_rol';
import { use_delete_administracion_rol } from '../hooks/use_delete_administracion_rol';

const now = () => new Date().toISOString();

const build_initial_state = (): administracion_rol => ({
  role_id: 0,
  code: generate_role_code(),
  name: '',
  is_active: true,
  row_status: true,
  created_by: 0,
  created_date: now(),
  modified_by: null,
  modified_date: null,
});

const ROLE_CODE_LENGTH = 23;
const ROLE_CODE_CHARS =
  'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789_-';

const generate_role_code = () => {
  const values = new Uint32Array(ROLE_CODE_LENGTH - 1);
  window.crypto.getRandomValues(values);

  const suffix = Array.from(values)
    .map((value) => ROLE_CODE_CHARS[value % ROLE_CODE_CHARS.length])
    .join('');

  return `_${suffix}`;
};

export default function AdministracionRolesPage() {
  const toast = useRef<Toast>(null);

  const [visible, set_visible] = useState(false);

  const [form, set_form] = useState<administracion_rol>(
    build_initial_state(),
  );

  const {
    roles,
    loading: loading_roles,
    reload_roles,
  } = use_administracion_roles();

  const {
    create_role,
    loading: creating_role,
  } = use_create_administracion_rol();

  const {
    update_role,
    loading: updating_role,
  } = use_update_administracion_rol();

  const {
    delete_role,
    loading: deleting_role,
  } = use_delete_administracion_rol();

  const roles_visibles = roles?.filter(
    (role) => role.row_status === true,
  );

  const is_busy =
    creating_role ||
    updating_role ||
    deleting_role;

  useEffect(() => {
    load_roles();
  }, []);

  const load_roles = async () => {
    try {
      await reload_roles();
    } catch (error) {
      console.error(error);

      toast.current?.show({
        severity: 'error',
        summary: 'Error',
        detail: 'No se pudieron cargar los roles.',
        life: 3000,
      });
    }
  };

  const update_field = <
    K extends keyof administracion_rol,
  >(
    field: K,
    value: administracion_rol[K],
  ) => {
    set_form((prev) => ({
      ...prev,
      [field]: value,
    }));
  };

  const validate_form = () => {
    const errors: string[] = [];
    const code = form.code?.trim() ?? '';

    if (!form.name?.trim()) {
      errors.push('Debe ingresar el nombre del rol.');
    }

    if (!code) {
      errors.push('Debe ingresar el código del rol.');
    }

    if (code && !/^[a-zA-Z0-9_-]+$/.test(code)) {
      errors.push(
        'El código del rol solo permite letras, números, guion bajo (_) y guion medio (-).',
      );
    }

    if (code.length > 50) {
      errors.push(
        'El código del rol no debe superar los 50 caracteres.',
      );
    }

    return errors;
  };

  const handle_nuevo = () => {
    set_form(build_initial_state());
    set_visible(true);
  };

  const handle_editar = (role: administracion_rol) => {
    set_form({
      ...role,
      code: role.code ?? '',
    });

    set_visible(true);
  };

  const handle_eliminar = async (role_id: number) => {
    try {
      const response = await delete_role(role_id);

      if (!response.status) {
        throw new Error(
          response.message ||
            'No se pudo eliminar el rol.',
        );
      }

      toast.current?.show({
        severity: 'success',
        summary: 'Éxito',
        detail:
          response.message ||
          'Rol eliminado correctamente.',
        life: 3000,
      });

      await load_roles();
    } catch (error: any) {
      console.error(error);

      toast.current?.show({
        severity: 'error',
        summary: 'Error',
        detail:
          error.message ||
          'Ocurrió un error.',
        life: 3000,
      });
    }
  };

  const confirmar_eliminar = (role_id: number) => {
    confirmDialog({
      message: '¿Deseas eliminar este rol?',
      header: 'Confirmación',
      icon: 'pi pi-exclamation-triangle',

      acceptLabel: 'Sí',
      rejectLabel: 'No',

      acceptClassName: 'ml-3',
      rejectClassName: 'mr-3',

      accept: () => {
        (document.activeElement as HTMLElement)?.blur();
        handle_eliminar(role_id);
      },

      reject: () => {
        (document.activeElement as HTMLElement)?.blur();
      },
    });
  };

  const handle_guardar = async () => {
    const validation_errors = validate_form();

    if (validation_errors.length > 0) {
      toast.current?.show({
        severity: 'warn',
        summary: 'Validación',
        detail: validation_errors.join('\n'),
        life: 3000,
      });

      return;
    }

    try {
      const payload: administracion_rol = {
        ...form,
        name: form.name.trim(),
        code: (form.code ?? '').trim(),
        row_status: true,
        created_date: form.created_date || now(),
        modified_date:
          form.role_id === 0
            ? null
            : now(),
      };

      const response =
        payload.role_id === 0
          ? await create_role(payload)
          : await update_role(payload);

      if (!response.status) {
        throw new Error(
          response.message ||
            'No se pudo guardar el rol.',
        );
      }

      toast.current?.show({
        severity: 'success',
        summary: 'Éxito',
        detail:
          response.message ||
          'Rol guardado correctamente.',
        life: 3000,
      });

      set_visible(false);

      await load_roles();
    } catch (error: any) {
      console.error(error);

      toast.current?.show({
        severity: 'error',
        summary: 'Error',
        detail:
          error.message ||
          'Ocurrió un error.',
        life: 3000,
      });
    }
  };

  return (
    <>
      <ConfirmDialog />
      <Toast ref={toast} />

      <h2 className="text-2xl font-bold text-gray-900 mb-2">
        Administración de Roles
      </h2>

      <Card className="w-full shadow-md card-presto-form">
        <div className="flex justify-end mb-5">
          <Button
            label="Nuevo Rol"
            icon="pi pi-plus"
            severity="success"
            onClick={handle_nuevo}
          />
        </div>

        <DataTable
          value={roles_visibles}
          paginator
          rows={10}
          loading={loading_roles}
          responsiveLayout="scroll"
          size="small"
        className="text-sm datatable-presto"
          emptyMessage="No existen roles registrados"
        >
          <Column
            field="role_id"
            header="ID"
          />

          <Column
            field="name"
            header="Nombre"
          />

          <Column
            header="Estado"
            body={(row_data) =>
              row_data.is_active
                ? 'Activo'
                : 'Inactivo'
            }
          />

          <Column
            header="Acciones"
            body={(row_data) => (
              <div className="flex gap-2">
                <Button
                  icon="pi pi-pencil"
                  severity="info"
                  rounded
                  outlined
                  onClick={(e) => {
                    handle_editar(row_data);
                    (
                      e.currentTarget as HTMLButtonElement
                    ).blur();
                  }}
                />

                <Button
                  icon="pi pi-trash"
                  severity="danger"
                  rounded
                  outlined
                  loading={deleting_role}
                  onClick={(e) => {
                    (
                      e.currentTarget as HTMLButtonElement
                    ).blur();
                    confirmar_eliminar(row_data.role_id);
                  }}
                />
              </div>
            )}
          />
        </DataTable>
      </Card>

      <Dialog
        header={
          form.role_id === 0
            ? 'Nuevo Rol'
            : 'Editar Rol'
        }
        visible={visible}
        style={{ width: '45vw' }}
        modal
        draggable={false}
        resizable={false}
        onHide={() => set_visible(false)}
      >
        <div className="grid grid-cols-1 md:grid-cols-2 gap-5">
          <div className="flex flex-col gap-1">
            <label className="font-semibold text-sm">
              Nombre del Rol *
            </label>

            <InputText
              value={form.name ?? ''}
              onChange={(e) =>
                update_field('name', e.target.value)
              }
              className="form-input-presto w-full"
              disabled={is_busy}
              placeholder="Ingrese nombre del rol"
            />
          </div>

          <div className="flex flex-col gap-1">
            <label className="font-semibold text-sm">
              Código *
            </label>

            <InputText
              value={form.code ?? ''}
              className="form-input-presto w-full"
              disabled
              placeholder="Código autogenerado"
              maxLength={50}
            />
          </div>

          <div className="flex flex-col gap-2">
            <label className="font-semibold text-sm">
              Activo
            </label>

            <InputSwitch
              checked={form.is_active}
              disabled={is_busy}
              onChange={(e) =>
                update_field(
                  'is_active',
                  e.value,
                )
              }
            />
          </div>
        </div>

        <div className="flex justify-end gap-3 mt-8">
          <Button
            label="Cancelar"
            icon="pi pi-times"
            severity="secondary"
            outlined
            onClick={() => set_visible(false)}
          />

          <Button
            label="Guardar"
            icon="pi pi-save"
            severity="success"
            loading={is_busy}
            onClick={handle_guardar}
          />
        </div>
      </Dialog>
    </>
  );
}

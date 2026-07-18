import { useEffect, useRef, useState } from 'react';
import { useNavigate } from 'react-router-dom';

import { Button } from 'primereact/button';
import { Card } from 'primereact/card';
import { Column } from 'primereact/column';
import { DataTable } from 'primereact/datatable';
import { Dialog } from 'primereact/dialog';
import { Dropdown } from 'primereact/dropdown';
import { InputNumber } from 'primereact/inputnumber';
import { InputSwitch } from 'primereact/inputswitch';
import { InputText } from 'primereact/inputtext';
import { Toast } from 'primereact/toast';
import { ConfirmDialog, confirmDialog } from 'primereact/confirmdialog';
import type { AdministracionUsuario } from '../models/AdministracionUsuariosDTO';


import { useAdministracionUsuarios } from '../hooks/useAdministracionUsuarios';
import { useCreateAdministracionUsuario } from '../hooks/useCreateAdministracionUsuario';
import { useUpdateAdministracionUsuario } from '../hooks/useUpdateAdministracionUsuario';
import { useDeleteAdministracionUsuario } from '../hooks/useDeleteAdministracionUsuario';
import { useRoles } from '../hooks/useRoles';

const now = () => new Date().toISOString();

const buildInitialState = (): AdministracionUsuario => ({
  user_id: 0,
  role_id: 0,

  name: '',

  last_name_first: '',
  last_name_second: '',

  id_document_type: 0,

  nro_document: '',
  user_name: '',
  password: '',
  email: '',

  is_first_access: true,

  remaining_attempts: 3,

  is_active: true,
  row_status: true,

  created_by: 0,
  created_date: now(),

  modified_by: null,
  modified_date: null,
});

export default function UsuarioPage() {
  const toast = useRef<Toast>(null);

  const navigate = useNavigate();
   const { roles } = useRoles();

  const [visible, setVisible] = useState(false);

  const [form, setForm] =
    useState<AdministracionUsuario>(
      buildInitialState(),
    );

  const [isDisabled, setIsDisabled] =
    useState(false);

  const roleOptions = roles.map((x) => ({
  label: x.description,
  value: x.id,
}));

  const documentTypeOptions = [
    { label: 'Cedula Ciudadania', value: 1 },
    { label: 'Cedula Extranjeria', value: 2 },
    { label: 'Pasaporte', value: 3 },
  ];
  /* =========================
     HOOKS
  ========================= */

  const {
  users: usuarios,
  loading: loadingUsers,
  reloadUsers,
} = useAdministracionUsuarios();
const usuariosActivos = usuarios?.filter(
  (u) => u.row_status === true
);
  const {
    createUser,
    loading: creatingUser,
  } = useCreateAdministracionUsuario();

  const {
    updateUser,
    loading: updatingUser,
  } = useUpdateAdministracionUsuario();

  const {
    deleteUser,
    loading: deletingUser,
  } = useDeleteAdministracionUsuario();



  const isBusy =
    creatingUser ||
    updatingUser ||
    deletingUser;

  /* =========================
     INIT
  ========================= */

  useEffect(() => {
    loadUsuarios();
  }, []);

  const loadUsuarios = async () => {
    try {
      await reloadUsers();
    } catch (error) {
      console.error(error);

      toast.current?.show({
        severity: 'error',
        summary: 'Error',
        detail:
          'No se pudieron cargar los usuarios.',
        life: 3000,
      });
    }
  };

  /* =========================
     HELPERS
  ========================= */

  const updateField = <
    K extends keyof AdministracionUsuario,
  >(
    field: K,
    value: AdministracionUsuario[K],
  ) => {
    setForm((prev) => ({
      ...prev,
      [field]: value,
    }));
  };

  const validateForm = () => {
  const errors: string[] = [];

  if (!form.name?.trim()) {
    errors.push('Debe ingresar el nombre.');
  }

  if (!form.role_id) {
    errors.push('Debe seleccionar un rol.');
  }

  if (!form.id_document_type) {
    errors.push('Debe seleccionar tipo documento.');
  }

  if (!form.nro_document?.trim()) {
    errors.push('Debe ingresar número documento.');
  }

  if (!form.user_name?.trim()) {
    errors.push('Debe ingresar usuario.');
  }

  if (!form.email?.trim()) {
    errors.push('Debe ingresar email.');
  } else {
    const emailRegex =
      /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;

    if (!emailRegex.test(form.email.trim())) {
      errors.push('Debe ingresar un email válido.');
    }
  }

  if (form.user_id === 0 && !form.password?.trim()) {
    errors.push('Debe ingresar password.');
  }

  return errors;
};

  /* =========================
     EVENTS
  ========================= */

  const handleNuevo = () => {
    setForm(buildInitialState());
    setIsDisabled(false);
    setVisible(true);
  };

  const handleEditarUsuario = (
    usuario: AdministracionUsuario,
  ) => {
    setForm({
      ...usuario,
      password: '',
    });

    setIsDisabled(false);
    setVisible(true);
  };

  const handleEliminarUsuario = async (
    user_id: number,
  ) => {
    try {
      const response = await deleteUser(
        user_id,
      );

      if (!response.status) {
        throw new Error(
          response.message ||
            'No se pudo eliminar.',
        );
      }

      toast.current?.show({
        severity: 'success',
        summary: 'Éxito',
        detail:
          response.message ||
          'Usuario eliminado correctamente.',
        life: 3000,
      });

      await loadUsuarios();
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

  const confirmarEliminarUsuario = (user_id: number) => {
  confirmDialog({
  message: '¿Deseas eliminar este usuario?',
  header: 'Confirmación',
  icon: 'pi pi-exclamation-triangle',

  acceptLabel: 'Sí',
  rejectLabel: 'No',

  acceptClassName: 'ml-3', // 👈 separa visualmente
  rejectClassName: 'mr-3',

 accept: () => {
  (document.activeElement as HTMLElement)?.blur();
  handleEliminarUsuario(user_id);
},

reject: () => {
  (document.activeElement as HTMLElement)?.blur();
},
});
};

  const handleGuardar = async () => {
    const validationErrors = validateForm();

if (validationErrors.length > 0) {
  toast.current?.show({
    severity: 'warn',
    summary: 'Validación',
    detail: validationErrors.join('\n'),
    life: 3000,
  });

  return;
}

    try {
      let response;

      if (form.user_id === 0) {
        response = await createUser(form);
      } else {

        const payload = {
          ...form,
        };

        // SI NO CAMBIÓ PASSWORD
        // NO ENVIAR STRING VACÍO
        if (!payload.password?.trim()) {
          delete payload.password;
        }

        response = await updateUser(payload as AdministracionUsuario);
      }

      if (!response.status) {
        throw new Error(
          response.message ||
            'No se pudo guardar.',
        );
      }

      toast.current?.show({
        severity: 'success',
        summary: 'Éxito',
        detail:
          response.message ||
          'Usuario guardado correctamente.',
        life: 3000,
      });

      setVisible(false);

      await loadUsuarios();
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
        Administración de Usuarios
      </h2>

      <Card className="w-full shadow-md card-presto-form">

        <div className="flex justify-end mb-5">
          <Button
            label="Nuevo Usuario"
            icon="pi pi-plus"
            severity="success"
            onClick={handleNuevo}
          />
        </div>

        <DataTable
          value={usuariosActivos}
          paginator
          rows={10}
          loading={loadingUsers}
          responsiveLayout="scroll"
          emptyMessage="No existen usuarios registrados"
          size="small"
        className="text-sm datatable-presto"
        >

          <Column
            field="user_id"
            header="ID"
          />

          <Column
            header="Nombre Completo"
            body={(rowData) =>
              `${rowData.name} ${
                rowData.last_name_first ?? ''
              } ${
                rowData.last_name_second ?? ''
              }`
            }
          />

          <Column
            field="user_name"
            header="Usuario"
          />

          <Column
            field="email"
            header="Email"
          />

          <Column
            header="Estado"
            body={(rowData) =>
              rowData.is_active
                ? 'Activo'
                : 'Inactivo'
            }
          />

          <Column
            header="Acciones"
            body={(rowData) => (
              <div className="flex gap-2">

                <Button
                  icon="pi pi-pencil"
                  severity="info"
                  rounded
                  outlined
                  onClick={(e) => {
                    handleEditarUsuario(rowData);

                    // quitar focus del botón
                    (e.currentTarget as HTMLButtonElement).blur();
                  }}
                />

                <Button
                  icon="pi pi-trash"
                  severity="danger"
                  rounded
                  outlined
                  loading={deletingUser}
                  onClick={(e) => {
                    (e.currentTarget as HTMLButtonElement).blur();
                    confirmarEliminarUsuario(rowData.user_id);
                  }}
                />

              </div>
            )}
          />

        </DataTable>

      </Card>

      <Dialog
        header={
          form.user_id === 0
            ? 'Nuevo Usuario'
            : 'Editar Usuario'
        }
        visible={visible}
        style={{ width: '80vw' }}
        modal
        draggable={false}
        resizable={false}
        onHide={() => setVisible(false)}
      >

        <div className="grid grid-cols-1 md:grid-cols-3 gap-5">

          {/* NOMBRE */}
          <div className="flex flex-col gap-1">
            <label className="font-semibold text-sm">
              Nombre *
            </label>

            <InputText
              value={form.name ?? ''}
              onChange={(e) =>
                updateField(
                  'name',
                  e.target.value,
                )
              }
              className="form-input-presto w-full"
              disabled={
                isDisabled || isBusy
              }
              placeholder="Ingrese nombre"
            />
          </div>

          {/* APELLIDO PATERNO */}
          <div className="flex flex-col gap-1">
            <label className="font-semibold text-sm">
              Apellido Paterno
            </label>

            <InputText
              value={
                form.last_name_first ?? ''
              }
              onChange={(e) =>
                updateField(
                  'last_name_first',
                  e.target.value,
                )
              }
              className="form-input-presto w-full"
              disabled={
                isDisabled || isBusy
              }
              placeholder="Ingrese apellido paterno"
            />
          </div>

          {/* APELLIDO MATERNO */}
          <div className="flex flex-col gap-1">
            <label className="font-semibold text-sm">
              Apellido Materno
            </label>

            <InputText
              value={
                form.last_name_second ?? ''
              }
              onChange={(e) =>
                updateField(
                  'last_name_second',
                  e.target.value,
                )
              }
              className="form-input-presto w-full"
              disabled={
                isDisabled || isBusy
              }
              placeholder="Ingrese apellido materno"
            />
          </div>

          {/* ROL */}
          <div className="flex flex-col gap-1">
            <label className="font-semibold text-sm">
              Rol *
            </label>

            <Dropdown
              value={form.role_id ?? null}
              options={roleOptions}
              optionLabel="label"
              optionValue="value"
              onChange={(e) =>
                updateField(
                  'role_id',
                  e.value ?? 0,
                )
              }
              className="form-dropdown-presto w-full"
              disabled={
                isDisabled || isBusy
              }
              placeholder="Seleccione"
              emptyMessage="Sin resultados"
              showClear
              filter
            />
          </div>

          {/* TIPO DOCUMENTO */}
          <div className="flex flex-col gap-1">
            <label className="font-semibold text-sm">
              Tipo Documento *
            </label>

            <Dropdown
              value={
                form.id_document_type ??
                null
              }
              options={
                documentTypeOptions
              }
              optionLabel="label"
              optionValue="value"
              onChange={(e) =>
                updateField(
                  'id_document_type',
                  e.value ?? 0,
                )
              }
              className="form-dropdown-presto w-full"
              disabled={
                isDisabled || isBusy
              }
              placeholder="Seleccione"
              emptyMessage="Sin resultados"
              showClear
              filter
            />
          </div>

          {/* NUMERO DOCUMENTO */}
          <div className="flex flex-col gap-1">
            <label className="font-semibold text-sm">
              Número Documento *
            </label>

            <InputText
              value={
                form.nro_document ?? ''
              }
              onChange={(e) =>
                updateField(
                  'nro_document',
                  e.target.value,
                )
              }
              className="form-input-presto w-full"
              disabled={
                isDisabled || isBusy
              }
              placeholder="Ingrese número documento"
            />
          </div>

          {/* USUARIO */}
          <div className="flex flex-col gap-1">
            <label className="font-semibold text-sm">
              Usuario *
            </label>

            <InputText
              value={
                form.user_name ?? ''
              }
              onChange={(e) =>
                updateField(
                  'user_name',
                  e.target.value,
                )
              }
              className="form-input-presto w-full"
              disabled={
                isDisabled || isBusy
              }
              placeholder="Ingrese usuario"
            />
          </div>

          {/* PASSWORD */}
          <div className="flex flex-col gap-1">
            <label className="font-semibold text-sm">
              {form.user_id === 0
                ? 'Password *'
                : 'Nueva Password'}
            </label>

            <InputText
              type="password"
              value={
                form.password ?? ''
              }
              onChange={(e) =>
                updateField(
                  'password',
                  e.target.value,
                )
              }
              className="form-input-presto w-full"
              disabled={
                isDisabled || isBusy
              }
              placeholder={
                form.user_id === 0
                  ? 'Ingrese password'
                  : 'Dejar vacío para mantener'
              }
            />
          </div>

          {/* EMAIL */}
          <div className="flex flex-col gap-1">
            <label className="font-semibold text-sm">
              Email *
            </label>

            <InputText
              value={form.email ?? ''}
              onChange={(e) =>
                updateField(
                  'email',
                  e.target.value,
                )
              }
              className="form-input-presto w-full"
              disabled={
                isDisabled || isBusy
              }
              placeholder="Ingrese email"
            />
          </div>

          {/* ACTIVO */}
          <div className="flex flex-col gap-2">
            <label className="font-semibold text-sm">
              Activo
            </label>

            <InputSwitch
              checked={form.is_active}
              disabled={
                isDisabled || isBusy
              }
              onChange={(e) =>
                updateField(
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
            onClick={() =>
              setVisible(false)
            }
          />

          <Button
            label="Guardar"
            icon="pi pi-save"
            severity="success"
            loading={isBusy}
            onClick={handleGuardar}
          />

        </div>

      </Dialog>
    </>
  );
}
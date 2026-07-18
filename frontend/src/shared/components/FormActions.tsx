import { Button } from "primereact/button";

interface FormActionsProps {
  isDisabled: boolean;
  canAdvance: boolean;
  isBusy: boolean;
  saveIsPending: boolean;
  avanzarIsPending: boolean;
  onEditar: () => void;
  onGuardar: () => void;
  onAvanzar: () => void;
  onSalir: () => void;
}

export default function FormActions({
  isDisabled,
  canAdvance,
  isBusy,
  saveIsPending,
  avanzarIsPending,
  onEditar,
  onGuardar,
  onAvanzar,
  onSalir,
}: FormActionsProps) {
  return (
    /* Agregamos la clase 'form-actions' aquí. Esto garantiza que los estilos de PrimeReact 
      (como el color de la letra o los bordes de los botones con severity) se hereden 
      exactamente igual que antes.
    */
    <div className="form-actions flex gap-2 flex-wrap">
      <Button 
        type="button" 
        label="Editar" 
        icon="pi pi-pencil" 
        severity="info" 
        outlined 
        onClick={onEditar} 
        disabled={isBusy || !isDisabled} 
        className="btn-responsive" 
      />
      <Button 
        type="button" 
        label={saveIsPending ? "Guardando..." : "Guardar"} 
        icon="pi pi-save" 
        severity="success" 
        onClick={onGuardar} 
        disabled={isBusy || isDisabled} 
        className="btn-responsive" 
      />
      <Button 
        type="button" 
        label={avanzarIsPending ? "Avanzando..." : "Avanzar"} 
        icon="pi pi-arrow-right" 
        severity="warning" 
        onClick={onAvanzar} 
        disabled={isBusy || !canAdvance} 
        className="btn-responsive" 
      />
      <Button 
        type="button" 
        label="Salir" 
        icon="pi pi-sign-out" 
        severity="secondary" 
        outlined 
        onClick={onSalir} 
        disabled={isBusy} 
        className="btn-responsive" 
      />
    </div>
  );
}
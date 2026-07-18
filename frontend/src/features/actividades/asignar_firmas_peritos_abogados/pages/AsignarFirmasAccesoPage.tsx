import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { Button } from 'primereact/button';
import { Card } from 'primereact/card';
import { InputNumber } from 'primereact/inputnumber';

export default function AsignarFirmasAccesoPage() {
  const navigate = useNavigate();
  const [idExpediente, setIdExpediente] = useState<number | null>(null);

  const abrir = () => {
    if (idExpediente && idExpediente > 0) {
      navigate(`/home/asignar_firmas/${idExpediente}`);
    }
  };

  return (
    <div className="flex justify-center pt-10">
      <Card title="Asignar Firmas, Peritos y Abogados" className="w-full max-w-xl">
        <p className="mb-5 text-sm text-gray-600">
          Ingresa el número del expediente. También puedes abrir la actividad desde la bandeja.
        </p>
        <div className="flex flex-col gap-2">
          <label htmlFor="expediente-asignar-firmas" className="text-sm font-medium">
            Número de expediente
          </label>
          <InputNumber
            inputId="expediente-asignar-firmas"
            value={idExpediente}
            onValueChange={(event) => setIdExpediente(event.value ?? null)}
            useGrouping={false}
            min={1}
            placeholder="Ej. 12345"
            onKeyDown={(event) => { if (event.key === 'Enter') abrir(); }}
          />
        </div>
        <div className="flex justify-end gap-3 mt-6">
          <Button label="Ir a la bandeja" severity="secondary" outlined onClick={() => navigate('/home/bandeja')} />
          <Button label="Abrir actividad" icon="pi pi-arrow-right" disabled={!idExpediente || idExpediente <= 0} onClick={abrir} />
        </div>
      </Card>
    </div>
  );
}

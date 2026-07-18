import { useMutation } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import { expedienteDigitalService } from '../api/expedienteDigitalService';

type UploadExpedienteDigitalFileParams = {
  file: File;
  id_expediente: number;
  activity_id?: string;
  doc_name?: string;
  work_flow_process_id?: string;
};

export function useUploadExpedienteDigitalFile() {
  return useMutation<ApiResponse<boolean>, unknown, UploadExpedienteDigitalFileParams>({
    mutationFn: (params) => expedienteDigitalService.uploadFile(params),
  });
}

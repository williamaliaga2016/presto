import { axiosClient } from './axiosClient';

export class BaseApiService<T> {
  constructor(protected resource: string) {}

  async getAll(): Promise<T[]> {
    const response = await axiosClient.get<T[]>(this.resource);
    return response.data;
  }

  async getById(id: string | number): Promise<T> {
    const response = await axiosClient.get<T>(`${this.resource}/${id}`);
    return response.data;
  }

  async create(payload: Partial<T>): Promise<T> {
    const response = await axiosClient.post<T>(this.resource, payload);
    return response.data;
  }

  async update(id: string | number, payload: Partial<T>): Promise<T> {
    const response = await axiosClient.put<T>(
      `${this.resource}/${id}`,
      payload,
    );
    return response.data;
  }

  async delete(id: string | number): Promise<void> {
    await axiosClient.delete(`${this.resource}/${id}`);
  }
}

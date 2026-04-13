import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Warehouse, WarehouseDetail } from '../models/warehouse.model';

@Injectable({ providedIn: 'root' })
export class WarehouseService {
  private readonly apiUrl = 'http://localhost:5182/api/warehouses';

  constructor(private http: HttpClient) {}

  getAll(): Observable<Warehouse[]> {
    return this.http.get<Warehouse[]>(this.apiUrl);
  }

  getAllWithSites(): Observable<WarehouseDetail[]> {
    return this.http.get<WarehouseDetail[]>(`${this.apiUrl}/with-sites`);
  }

  getById(id: number): Observable<WarehouseDetail> {
    return this.http.get<WarehouseDetail>(`${this.apiUrl}/${id}`);
  }

  create(warehouse: Partial<Warehouse>): Observable<number> {
    return this.http.post<number>(this.apiUrl, warehouse);
  }

  update(id: number, warehouse: Partial<Warehouse>): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, warehouse);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}

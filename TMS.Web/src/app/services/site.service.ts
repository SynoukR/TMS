import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Site, SiteDetail } from '../models/site.model';

@Injectable({ providedIn: 'root' })
export class SiteService {
  private readonly apiUrl = 'http://localhost:5182/api/sites';

  constructor(private http: HttpClient) {}

  getAll(): Observable<Site[]> {
    return this.http.get<Site[]>(this.apiUrl);
  }

  getById(id: number): Observable<SiteDetail> {
    return this.http.get<SiteDetail>(`${this.apiUrl}/${id}`);
  }

  create(site: Partial<Site>): Observable<number> {
    return this.http.post<number>(this.apiUrl, site);
  }

  update(id: number, site: Partial<Site>): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, site);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}

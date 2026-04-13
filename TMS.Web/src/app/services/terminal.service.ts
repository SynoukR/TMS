import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Terminal } from '../models/terminal.model';

@Injectable({ providedIn: 'root' })
export class TerminalService {
  private readonly apiUrl = 'http://localhost:5182/api/terminals';

  constructor(private http: HttpClient) {}

  getAll(): Observable<Terminal[]> {
    return this.http.get<Terminal[]>(this.apiUrl);
  }

  getById(id: number): Observable<Terminal> {
    return this.http.get<Terminal>(`${this.apiUrl}/${id}`);
  }

  create(terminal: Partial<Terminal>): Observable<Terminal> {
    return this.http.post<Terminal>(this.apiUrl, terminal);
  }

  update(id: number, terminal: Partial<Terminal>): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, terminal);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}

// ClientApp/src/app/api.service.ts
import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({ providedIn: 'root' })
export class ApiService {
  private http = inject(HttpClient);
  diagnostics(code: string) { return this.http.post<any>('/api/scripts/diagnostics', { code }); }
  run(code: string)         { return this.http.post<any>('/api/scripts/run', { code }); }
}

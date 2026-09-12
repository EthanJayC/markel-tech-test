import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Claim, Company, UpdateClaimRequest } from './models';

@Injectable({ providedIn: 'root' })
export class ClaimsApiService {
  constructor(private readonly http: HttpClient) {}

  getCompanies() {
    return this.http.get<Company[]>('/api/companies');
  }

  getCompany(id: number) {
    return this.http.get<Company>(`/api/companies/${id}`);
  }

  getClaims(companyId: number) {
    return this.http.get<Claim[]>(`/api/companies/${companyId}/claims`);
  }

  getClaim(ucr: string) {
    return this.http.get<Claim>(`/api/claims/${encodeURIComponent(ucr)}`);
  }

  updateClaim(ucr: string, body: UpdateClaimRequest) {
    return this.http.put<Claim>(`/api/claims/${encodeURIComponent(ucr)}`, body);
  }
}

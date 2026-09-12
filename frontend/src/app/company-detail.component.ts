import { DatePipe } from '@angular/common';
import { Component, OnInit, inject } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { forkJoin } from 'rxjs';
import { ClaimsApiService } from './claims-api.service';
import { JsonDownloadService } from './json-download.service';
import { Claim, Company } from './models';

@Component({
  selector: 'app-company-detail',
  imports: [RouterLink, DatePipe],
  template: `
    <p><a routerLink="/">Back to companies</a></p>
    @if (error) {
      <p>{{ error }}</p>
    } @else if (!company || !claims) {
      <p>Loading...</p>
    } @else {
      <h1>{{ company.name }}</h1>
      <p><button type="button" (click)="saveCompanyJson()">Save company JSON</button></p>
      <p>Active: {{ company.active ? 'yes' : 'no' }}</p>
      <p>Has active insurance policy: {{ company.hasActiveInsurancePolicy ? 'yes' : 'no' }}</p>
      <p>Insurance end date: {{ company.insuranceEndDate | date: 'yyyy-MM-dd' }}</p>
      <p>{{ company.address1 }} {{ company.address2 }} {{ company.address3 }}</p>
      <p>{{ company.postcode }} {{ company.country }}</p>

      <h2>Claims</h2>
      <p><button type="button" (click)="saveClaimsJson()">Save claims JSON</button></p>
      @if (claims.length === 0) {
        <p>No claims for this company.</p>
      } @else {
        <ul>
          @for (claim of claims; track claim.ucr) {
            <li>
              <a [routerLink]="['/claims', claim.ucr]">{{ claim.ucr }}</a>
              — {{ claim.assuredName }} — {{ claim.closed ? 'closed' : 'open' }}
            </li>
          }
        </ul>
      }
    }
  `
})
export class CompanyDetailComponent implements OnInit {
  private readonly api = inject(ClaimsApiService);
  private readonly jsonDownload = inject(JsonDownloadService);
  private readonly route = inject(ActivatedRoute);

  company: Company | null = null;
  claims: Claim[] | null = null;
  error = '';

  ngOnInit(): void {
    const id = Number(this.route.snapshot.paramMap.get('id'));
    forkJoin({
      company: this.api.getCompany(id),
      claims: this.api.getClaims(id)
    }).subscribe({
      next: result => {
        this.company = result.company;
        this.claims = result.claims;
      },
      error: err => (this.error = err.error?.detail ?? 'Failed to load company')
    });
  }

  saveCompanyJson(): void {
    if (this.company) {
      this.jsonDownload.save(`company-${this.company.id}.json`, this.company);
    }
  }

  saveClaimsJson(): void {
    if (this.company && this.claims) {
      this.jsonDownload.save(`company-${this.company.id}-claims.json`, this.claims);
    }
  }
}

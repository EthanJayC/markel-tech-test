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
    <a routerLink="/" class="nav-link">Back to companies</a>

    @if (error) {
      <p class="alert">{{ error }}</p>
    } @else if (!company || !claims) {
      <p class="muted">Loading company…</p>
    } @else {
      <div class="page-header">
        <h1>{{ company.name }}</h1>
        <div class="toolbar">
          <button type="button" class="primary" (click)="saveCompanyJson()">
            Save company JSON
          </button>
          <button type="button" (click)="saveClaimsJson()">
            Save claims JSON
          </button>
        </div>
      </div>

      <section class="card">
        <h2>Company</h2>
        <dl class="facts">
          <dt>Active</dt>
          <dd>{{ company.active ? 'Yes' : 'No' }}</dd>
          <dt>Insurance policy</dt>
          <dd>
            <span class="badge" [class.ok]="company.hasActiveInsurancePolicy">
              {{ company.hasActiveInsurancePolicy ? 'Active' : 'Inactive' }}
            </span>
          </dd>
          <dt>Insurance end date</dt>
          <dd>{{ company.insuranceEndDate | date: 'yyyy-MM-dd' }}</dd>
          <dt>Address</dt>
          <dd>{{ address }}</dd>
        </dl>
      </section>

      <section class="card">
        <h2>Claims</h2>
        @if (claims.length === 0) {
          <p class="muted">No claims for this company.</p>
        } @else {
          <ul class="row-list">
            @for (claim of claims; track claim.ucr) {
              <li>
                <a [routerLink]="['/claims', claim.ucr]">
                  <span>{{ claim.ucr }} — {{ claim.assuredName }}</span>
                  <span class="row-meta">
                    <span class="badge" [class.ok]="!claim.closed">
                      {{ claim.closed ? 'Closed' : 'Open' }}
                    </span>
                  </span>
                </a>
              </li>
            }
          </ul>
        }
      </section>
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

  get address(): string {
    if (!this.company) {
      return '';
    }

    return [
      this.company.address1,
      this.company.address2,
      this.company.address3,
      this.company.postcode,
      this.company.country
    ]
      .filter(part => !!part)
      .join(', ');
  }

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

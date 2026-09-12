import { Component, OnInit, inject } from '@angular/core';
import { RouterLink } from '@angular/router';
import { ClaimsApiService } from './claims-api.service';
import { JsonDownloadService } from './json-download.service';
import { Company } from './models';

@Component({
  selector: 'app-company-list',
  imports: [RouterLink],
  template: `
    <div class="page-header">
      <h1>Companies</h1>
      @if (companies) {
        <div class="toolbar">
          <button type="button" class="primary" (click)="saveJson()">
            Save companies JSON
          </button>
        </div>
      }
    </div>

    @if (error) {
      <p class="alert">{{ error }}</p>
    } @else if (!companies) {
      <p class="muted">Loading companies…</p>
    } @else {
      <ul class="row-list">
        @for (company of companies; track company.id) {
          <li>
            <a [routerLink]="['/companies', company.id]">
              <span>{{ company.name }}</span>
              <span class="row-meta">
                <span
                  class="badge"
                  [class.ok]="company.hasActiveInsurancePolicy"
                >
                  {{ company.hasActiveInsurancePolicy ? 'Active policy' : 'No active policy' }}
                </span>
              </span>
            </a>
          </li>
        }
      </ul>
    }
  `
})
export class CompanyListComponent implements OnInit {
  private readonly api = inject(ClaimsApiService);
  private readonly jsonDownload = inject(JsonDownloadService);

  companies: Company[] | null = null;
  error = '';

  ngOnInit(): void {
    this.api.getCompanies().subscribe({
      next: companies => (this.companies = companies),
      error: err => (this.error = err.error?.detail ?? 'Failed to load companies')
    });
  }

  saveJson(): void {
    if (this.companies) {
      this.jsonDownload.save('companies.json', this.companies);
    }
  }
}

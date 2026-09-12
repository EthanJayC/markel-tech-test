import { Component, OnInit, inject } from '@angular/core';
import { RouterLink } from '@angular/router';
import { ClaimsApiService } from './claims-api.service';
import { JsonDownloadService } from './json-download.service';
import { Company } from './models';

@Component({
  selector: 'app-company-list',
  imports: [RouterLink],
  template: `
    <h1>Companies</h1>
    @if (error) {
      <p>{{ error }}</p>
    } @else if (!companies) {
      <p>Loading...</p>
    } @else {
      <p><button type="button" (click)="saveJson()">Save JSON</button></p>
      <ul>
        @for (company of companies; track company.id) {
          <li>
            <a [routerLink]="['/companies', company.id]">{{ company.name }}</a>
            — {{ company.hasActiveInsurancePolicy ? 'active policy' : 'no active policy' }}
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

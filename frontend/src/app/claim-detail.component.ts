import { CurrencyPipe, DatePipe } from '@angular/common';
import { Component, OnInit, inject } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { ClaimsApiService } from './claims-api.service';
import { JsonDownloadService } from './json-download.service';
import { Claim } from './models';

@Component({
  selector: 'app-claim-detail',
  imports: [RouterLink, DatePipe, CurrencyPipe],
  template: `
    @if (error) {
      <p class="alert">{{ error }}</p>
    } @else if (!claim) {
      <p class="muted">Loading claim…</p>
    } @else {
      <a [routerLink]="['/companies', claim.companyId]" class="nav-link">Back to company</a>

      <div class="page-header">
        <h1>Claim {{ claim.ucr }}</h1>
        <div class="toolbar">
          <button type="button" class="primary" (click)="saveJson()">
            Save claim JSON
          </button>
          <a class="button" [routerLink]="['/claims', claim.ucr, 'edit']">Edit claim</a>
        </div>
      </div>

      <section class="card">
        <h2>Claim</h2>
        <dl class="facts">
          <dt>Assured name</dt>
          <dd>{{ claim.assuredName }}</dd>
          <dt>Claim date</dt>
          <dd>{{ claim.claimDate | date: 'yyyy-MM-dd' }}</dd>
          <dt>Loss date</dt>
          <dd>{{ claim.lossDate | date: 'yyyy-MM-dd' }}</dd>
          <dt>Age in days</dt>
          <dd>{{ claim.ageInDays }}</dd>
          <dt>Incurred loss</dt>
          <dd>{{ claim.incurredLoss | currency: 'GBP' }}</dd>
          <dt>Status</dt>
          <dd>
            <span class="badge" [class.ok]="!claim.closed">
              {{ claim.closed ? 'Closed' : 'Open' }}
            </span>
          </dd>
        </dl>
      </section>
    }
  `
})
export class ClaimDetailComponent implements OnInit {
  private readonly api = inject(ClaimsApiService);
  private readonly jsonDownload = inject(JsonDownloadService);
  private readonly route = inject(ActivatedRoute);

  claim: Claim | null = null;
  error = '';

  ngOnInit(): void {
    const ucr = this.route.snapshot.paramMap.get('ucr') ?? '';
    this.api.getClaim(ucr).subscribe({
      next: claim => (this.claim = claim),
      error: err => (this.error = err.error?.detail ?? 'Failed to load claim')
    });
  }

  saveJson(): void {
    if (this.claim) {
      this.jsonDownload.save(`claim-${this.claim.ucr}.json`, this.claim);
    }
  }
}

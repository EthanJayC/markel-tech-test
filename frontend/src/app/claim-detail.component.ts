import { DatePipe } from '@angular/common';
import { Component, OnInit, inject } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { ClaimsApiService } from './claims-api.service';
import { JsonDownloadService } from './json-download.service';
import { Claim } from './models';

@Component({
  selector: 'app-claim-detail',
  imports: [RouterLink, DatePipe],
  template: `
    @if (error) {
      <p>{{ error }}</p>
    } @else if (!claim) {
      <p>Loading...</p>
    } @else {
      <p><a [routerLink]="['/companies', claim.companyId]">Back to company</a></p>
      <h1>Claim {{ claim.ucr }}</h1>
      <p>
        <button type="button" (click)="saveJson()">Save JSON</button>
        <a [routerLink]="['/claims', claim.ucr, 'edit']">Edit</a>
      </p>
      <p>Assured name: {{ claim.assuredName }}</p>
      <p>Claim date: {{ claim.claimDate | date: 'yyyy-MM-dd' }}</p>
      <p>Loss date: {{ claim.lossDate | date: 'yyyy-MM-dd' }}</p>
      <p>Age in days: {{ claim.ageInDays }}</p>
      <p>Incurred loss: {{ claim.incurredLoss }}</p>
      <p>Closed: {{ claim.closed ? 'yes' : 'no' }}</p>
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

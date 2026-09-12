import { Component, OnInit, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { ClaimsApiService } from './claims-api.service';
import { Claim } from './models';

@Component({
  selector: 'app-claim-edit',
  imports: [FormsModule, RouterLink],
  template: `
    @if (error) {
      <p class="alert">{{ error }}</p>
    } @else if (!claim) {
      <p class="muted">Loading claim…</p>
    } @else {
      <a [routerLink]="['/claims', claim.ucr]" class="nav-link">Cancel</a>

      <div class="page-header">
        <h1>Edit claim {{ claim.ucr }}</h1>
      </div>

      @if (saveError) {
        <p class="alert">{{ saveError }}</p>
      }

      <form class="card form" (ngSubmit)="save()">
        <label class="field">
          Claim date
          <input name="claimDate" type="date" [(ngModel)]="claimDate" />
        </label>
        <label class="field">
          Loss date
          <input name="lossDate" type="date" [(ngModel)]="lossDate" />
        </label>
        <label class="field">
          Assured name
          <input name="assuredName" type="text" [(ngModel)]="assuredName" />
        </label>
        <label class="field">
          Incurred loss
          <input name="incurredLoss" type="number" step="0.01" [(ngModel)]="incurredLoss" />
        </label>
        <label class="switch">
          <input name="closed" type="checkbox" [(ngModel)]="closed" />
          Closed
        </label>
        <div class="toolbar">
          <button type="submit" class="primary">Save claim</button>
          <a class="button" [routerLink]="['/claims', claim.ucr]">Cancel</a>
        </div>
      </form>
    }
  `
})
export class ClaimEditComponent implements OnInit {
  private readonly api = inject(ClaimsApiService);
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);

  claim: Claim | null = null;
  error = '';
  saveError = '';
  claimDate = '';
  lossDate = '';
  assuredName = '';
  incurredLoss = 0;
  closed = false;

  ngOnInit(): void {
    const ucr = this.route.snapshot.paramMap.get('ucr') ?? '';
    this.api.getClaim(ucr).subscribe({
      next: claim => {
        this.claim = claim;
        this.claimDate = toDateInput(claim.claimDate);
        this.lossDate = toDateInput(claim.lossDate);
        this.assuredName = claim.assuredName;
        this.incurredLoss = claim.incurredLoss;
        this.closed = claim.closed;
      },
      error: err => (this.error = err.error?.detail ?? 'Failed to load claim')
    });
  }

  save(): void {
    if (!this.claim) {
      return;
    }

    this.saveError = '';
    this.api
      .updateClaim(this.claim.ucr, {
        claimDate: this.claimDate,
        lossDate: this.lossDate,
        assuredName: this.assuredName,
        incurredLoss: Number(this.incurredLoss),
        closed: this.closed
      })
      .subscribe({
        next: () => void this.router.navigate(['/claims', this.claim!.ucr]),
        error: err => (this.saveError = err.error?.detail ?? 'Failed to update claim')
      });
  }
}

function toDateInput(value: string): string {
  return value.slice(0, 10);
}

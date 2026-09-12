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
      <p>{{ error }}</p>
    } @else if (!claim) {
      <p>Loading...</p>
    } @else {
      <p><a [routerLink]="['/claims', claim.ucr]">Cancel</a></p>
      <h1>Edit claim {{ claim.ucr }}</h1>
      @if (saveError) {
        <p>{{ saveError }}</p>
      }
      <form (ngSubmit)="save()">
        <p>
          <label>
            Claim date
            <input name="claimDate" type="date" [(ngModel)]="claimDate" />
          </label>
        </p>
        <p>
          <label>
            Loss date
            <input name="lossDate" type="date" [(ngModel)]="lossDate" />
          </label>
        </p>
        <p>
          <label>
            Assured name
            <input name="assuredName" [(ngModel)]="assuredName" />
          </label>
        </p>
        <p>
          <label>
            Incurred loss
            <input name="incurredLoss" type="number" step="0.01" [(ngModel)]="incurredLoss" />
          </label>
        </p>
        <p>
          <label>
            Closed
            <input name="closed" type="checkbox" [(ngModel)]="closed" />
          </label>
        </p>
        <button type="submit">Save</button>
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

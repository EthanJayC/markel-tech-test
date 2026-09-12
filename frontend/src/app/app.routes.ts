import { Routes } from '@angular/router';
import { ClaimDetailComponent } from './claim-detail.component';
import { ClaimEditComponent } from './claim-edit.component';
import { CompanyDetailComponent } from './company-detail.component';
import { CompanyListComponent } from './company-list.component';

export const routes: Routes = [
  { path: '', component: CompanyListComponent },
  { path: 'companies/:id', component: CompanyDetailComponent },
  { path: 'claims/:ucr/edit', component: ClaimEditComponent },
  { path: 'claims/:ucr', component: ClaimDetailComponent },
  { path: '**', redirectTo: '' }
];

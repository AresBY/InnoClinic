import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { ConfirmEmailComponent } from './features/auth/confirm-email/confirm-email.component';

import { HomeComponent } from './pages/home/home.component';
import { OfficesComponent } from './pages/offices/offices.component';
import { OfficeDetailsComponent } from './pages/offices/office-details.component';

const routes: Routes = [
  { path: '', component: HomeComponent },

  { path: 'doctor', component: HomeComponent },
  { path: 'offices', component: OfficesComponent },
  { path: 'offices/:id', component: OfficeDetailsComponent },

  { path: 'confirm-email', component: ConfirmEmailComponent },
  { path: '**', redirectTo: '' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {}

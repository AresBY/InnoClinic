import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { ConfirmEmailComponent } from './features/auth/confirm-email/confirm-email.component';
import { HomePatientComponent } from './pages/home-patient/home.patient.component';

import { HomeDoctorComponent } from './pages/home-doctor/home.doctor.component';

const routes: Routes = [
  { path: '', component: HomeDoctorComponent },

  // Patient routes
  { path: 'patient', component: HomePatientComponent },

  // Doctor routes
  { path: 'doctor', component: HomeDoctorComponent },

  { path: 'confirm-email', component: ConfirmEmailComponent },
  { path: '**', redirectTo: '' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {}

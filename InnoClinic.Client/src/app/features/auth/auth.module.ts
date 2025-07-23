import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RegisterPatientComponent } from './register-patient/register.patient.component';
import { SignInPatientComponent } from './sign-in-patient/sign-in.patient.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { AuthInterceptor } from '@auth/auth.interceptor';
import { SignInDoctorComponent } from './sign-in-doctor/sign-in.doctor.component';

@NgModule({
  declarations: [RegisterPatientComponent, SignInPatientComponent, SignInDoctorComponent],
  imports: [CommonModule, FormsModule, ReactiveFormsModule, RouterModule, HttpClientModule],
  exports: [RegisterPatientComponent, SignInPatientComponent],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true
    }
  ]
})
export class AuthModule {}

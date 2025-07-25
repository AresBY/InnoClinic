import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RegisterPComponent as RegisterComponent } from './register/register.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { AuthInterceptor } from '@auth/auth.interceptor';
import { SignInComponent } from './sign-in/sign-in.component';

@NgModule({
  declarations: [RegisterComponent, SignInComponent],
  imports: [CommonModule, FormsModule, ReactiveFormsModule, RouterModule, HttpClientModule],
  exports: [RegisterComponent, SignInComponent],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true
    }
  ]
})
export class AuthModule {}

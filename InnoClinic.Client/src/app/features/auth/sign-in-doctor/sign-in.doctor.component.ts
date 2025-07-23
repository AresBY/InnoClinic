import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators, AbstractControl } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { AuthService } from '@services/auth.service';

@Component({
  selector: 'app-sign-in',
  templateUrl: './sign-in.doctor.component.html'
})
export class SignInDoctorComponent implements OnInit {
  @Output() switchToSignUp = new EventEmitter<void>();

  signInForm!: FormGroup;
  submitted = false;
  errorMessage: string | null = null;
  isSubmitting = false;

  constructor(
    private dialogRef: MatDialogRef<SignInDoctorComponent>,
    private fb: FormBuilder,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    this.signInForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6), Validators.maxLength(15)]],
      role: [2]
    });
  }

  get email(): AbstractControl | null {
    return this.signInForm.get('email');
  }

  get password(): AbstractControl | null {
    return this.signInForm.get('password');
  }

  onSubmit() {
    this.submitted = true;
    this.errorMessage = null;

    if (this.signInForm.invalid) {
      return;
    }

    this.isSubmitting = true;

    this.authService.signIn$(this.signInForm.value, true).subscribe({
      next: (response: any) => {
        this.isSubmitting = false;
        this.authService.setAccessToken(response.accessToken);
        this.dialogRef.close();
      },
      error: (error) => {
        this.isSubmitting = false;
        if (error.status === 400 && error.error?.errors) {
          const errors = error.error.errors;

          Object.keys(errors).forEach((serverField) => {
            const formField = serverField.charAt(0).toLowerCase() + serverField.slice(1);
            const control = this.signInForm.get(formField);
            if (control) {
              control.setErrors({ server: errors[serverField][0] });
              control.markAsTouched();
            }
          });
        } else {
          this.errorMessage = error.error?.message || 'Server error during sign in.';
        }
      }
    });
  }

  close() {
    this.dialogRef.close();
  }
}

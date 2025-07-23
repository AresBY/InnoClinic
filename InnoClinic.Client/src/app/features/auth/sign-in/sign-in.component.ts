import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators, AbstractControl } from '@angular/forms';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { RegisterComponent } from '@auth/register/register.component';
import { AuthService } from '@services/auth.service';

@Component({
  selector: 'app-sign-in',
  templateUrl: './sign-in.component.html'
})
export class SignInComponent implements OnInit {
  @Output() switchToSignUp = new EventEmitter<void>();

  signInForm!: FormGroup;
  submitted = false;
  errorMessage: string | null = null;
  isSubmitting = false;

  constructor(
    private dialog: MatDialog,
    private dialogRef: MatDialogRef<SignInComponent>,
    private fb: FormBuilder,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    this.signInForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6), Validators.maxLength(15)]],
      role: [1]
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

    this.authService.signIn$(this.signInForm.value).subscribe({
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

  openRegisterModal(event: Event) {
    event.preventDefault();

    this.dialogRef.close();

    this.dialog.open(RegisterComponent, {
      width: '600px'
    });
  }

  close() {
    this.dialogRef.close();
  }
}

import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators, AbstractControl } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { AuthService } from '@services/auth.service';

@Component({
  selector: 'app-sign-in',
  templateUrl: './sign-in.component.html'
})
export class SignInComponent implements OnInit {
  @Output() public switchToSignUp = new EventEmitter<void>();

  public signInForm!: FormGroup;
  public submitted = false;
  public errorMessage: string | null = null;
  public isSubmitting = false;

  public constructor(
    private dialogRef: MatDialogRef<SignInComponent>,
    private fb: FormBuilder,
    private authService: AuthService
  ) {}

  public ngOnInit(): void {
    this.signInForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6), Validators.maxLength(15)]],
      role: [2]
    });
  }

  public get email(): AbstractControl | null {
    return this.signInForm.get('email');
  }

  public get password(): AbstractControl | null {
    return this.signInForm.get('password');
  }

  public onSubmit() {
    this.submitted = true;
    this.errorMessage = null;

    if (this.signInForm.invalid) {
      return;
    }

    this.isSubmitting = true;

    this.authService.signIn$(this.signInForm.value).subscribe({
      // eslint-disable-next-line @typescript-eslint/no-explicit-any
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

  public close() {
    this.dialogRef.close();
  }
}

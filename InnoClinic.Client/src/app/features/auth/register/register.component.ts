import { Component, EventEmitter, Output, OnInit, Inject } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  Validators,
  AbstractControl,
  ValidationErrors
} from '@angular/forms';
import { AuthService } from '@services/auth.service';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { SignInComponent } from '../sign-in/sign-in.component';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html'
})
export class RegisterComponent implements OnInit {
  @Output() switchToSignIn = new EventEmitter<void>();

  registerForm!: FormGroup;
  submitted = false;
  errorMessage: string | null = null;
  successMessage: string | null = null;
  isSubmitting = false;

  constructor(
    private dialog: MatDialog,
    public dialogRef: MatDialogRef<RegisterComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private fb: FormBuilder,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    console.log('RegisterComponent opened');
    this.registerForm = this.fb.group(
      {
        email: ['', [Validators.required, Validators.email]],
        password: ['', [Validators.required, Validators.minLength(6), Validators.maxLength(15)]],
        reEnteredPassword: ['', [Validators.required]]
      },
      { validators: this.passwordsMatchValidator }
    );
  }

  get email(): AbstractControl | null {
    return this.registerForm.get('email');
  }

  get password(): AbstractControl | null {
    return this.registerForm.get('password');
  }

  get reEnteredPassword(): AbstractControl | null {
    return this.registerForm.get('reEnteredPassword');
  }

  public passwordsMatchValidator(group: AbstractControl): ValidationErrors | null {
    const pass = group.get('password')?.value;
    const rePass = group.get('reEnteredPassword')?.value;
    return pass === rePass ? null : { passwordsMismatch: true };
  }

  public onSubmit() {
    this.submitted = true;
    this.errorMessage = null;
    this.successMessage = null;

    if (this.registerForm.invalid) {
      return;
    }

    this.isSubmitting = true;

    this.authService.register$(this.registerForm.value).subscribe({
      next: () => {
        this.successMessage = 'Registration successful!';
        this.isSubmitting = false;
      },
      error: (error) => {
        this.isSubmitting = false;

        if (error.status === 400 && error.error?.errors) {
          const errors = error.error.errors;
          Object.keys(errors).forEach((field) => {
            const formField = field.charAt(0).toLowerCase() + field.slice(1);
            const control = this.registerForm.get(formField);
            if (control) {
              control.setErrors({ server: errors[field][0] });
              control.markAsTouched();
            }
          });
        } else {
          this.errorMessage = error.error?.message || 'Something went wrong.';
        }
      }
    });
  }

  public checkEmailExists() {
    const emailControl = this.email;
    if (!emailControl || emailControl.invalid) {
      return;
    }

    const emailValue = emailControl.value.toLowerCase();

    this.authService.checkEmailExists$(emailValue).subscribe({
      next: (exists: boolean) => {
        if (exists) {
          emailControl.setErrors({ server: 'User with this email already exists' });
        } else {
          if (emailControl.hasError('server')) {
            const errors = { ...emailControl.errors };
            delete errors['server'];

            if (Object.keys(errors).length === 0) {
              emailControl.setErrors(null);
            } else {
              emailControl.setErrors(errors);
            }
          }
        }
      },
      error: (error) => {
        if (error.status === 409 && error.error?.message) {
          emailControl.setErrors({ server: error.error.message });
        }
      }
    });
  }

  public openSignInModal(event: Event) {
    event.preventDefault();

    this.dialog.open(SignInComponent, {
      width: '600px'
    });

    this.dialogRef.close();
  }

  close() {
    this.dialogRef.close();
  }
}

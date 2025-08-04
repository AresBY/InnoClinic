import { Component } from '@angular/core';
import {
  FormBuilder,
  Validators,
  AbstractControl,
  ValidationErrors,
  ValidatorFn
} from '@angular/forms';
import { MatDialogRef, MatDialog } from '@angular/material/dialog';
import { OfficeService } from '@services/office.service';
import { HttpErrorResponse } from '@angular/common/http';
import { CreateOffice } from '@models/create-office.model';
import { CancelConfirmDialogComponent } from '@shared/dialogs/cancel-confirm-dialog/cancel-confirm-dialog.component';

@Component({
  selector: 'app-create-office-dialog',
  templateUrl: './create-office-dialog.component.html'
})
export class CreateOfficeDialogComponent {
  public serverErrors: Record<string, string> = {};

  public officeForm = this.fb.group({
    photo: [null],
    city: ['', Validators.required],
    street: ['', Validators.required],
    houseNumber: ['', Validators.required],
    officeNumber: [''],
    registryPhoneNumber: ['', [Validators.required, this.phoneValidator()]],
    status: ['Active', Validators.required]
  });

  public constructor(
    private fb: FormBuilder,
    private dialogRef: MatDialogRef<CreateOfficeDialogComponent>,
    private dialog: MatDialog,
    private officeService: OfficeService
  ) {}

  public phoneValidator(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      const val = control.value;
      if (!val) return null;

      const phoneRegex = /^[0-9]+$/;
      return phoneRegex.test(val) ? null : { invalidPhone: true };
    };
  }

  public onPhotoChange(event: Event): void {
    const input = event.target as HTMLInputElement;
    this.officeForm.patchValue({ photo: input.files?.[0] ?? null });
  }

  public openCancelConfirmDialog(): void {
    CancelConfirmDialogComponent.open(this.dialog, {
      disableClose: false
    })
      .afterClosed()
      .subscribe((result) => {
        if (result === true) {
          this.dialogRef.close();
        }
      });
  }

  public onCancel(): void {
    this.openCancelConfirmDialog();
  }

  public onConfirm(): void {
    this.serverErrors = {};
    Object.keys(this.officeForm.controls).forEach((field) => {
      const control = this.officeForm.get(field);
      if (control?.hasError('serverError')) {
        control.setErrors(null);
      }
    });

    if (this.officeForm.invalid) {
      this.officeForm.markAllAsTouched();
      return;
    }

    const formValue = this.officeForm.value;

    const officeDto: CreateOffice = {
      city: formValue.city!,
      street: formValue.street!,
      houseNumber: formValue.houseNumber!,
      officeNumber: formValue.officeNumber || undefined,
      registryPhoneNumber: '+' + formValue.registryPhoneNumber!,
      status: formValue.status === 'Active'
    };

    this.officeService.createOffice(officeDto).subscribe({
      next: (createdOffice) => this.dialogRef.close(createdOffice),
      error: (error: HttpErrorResponse) => {
        if (error.status === 400 && error.error && typeof error.error === 'object') {
          this.serverErrors = error.error;
          Object.entries(this.serverErrors).forEach(([field, message]) => {
            const control = this.officeForm.get(field);
            if (control) {
              control.setErrors({ serverError: message });
              control.markAsTouched();
            }
          });
        } else {
          alert(error.message);
        }
      }
    });
  }
}

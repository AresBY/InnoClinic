import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { OfficeService } from '../../services/office.service';
import { OfficeDetails } from '../../models/office-details.model';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { CancelConfirmDialogComponent } from '@shared/dialogs/cancel-confirm-dialog/cancel-confirm-dialog.component';

@Component({
  selector: 'app-office-details',
  templateUrl: './office-details.component.html'
})
export class OfficeDetailsComponent implements OnInit {
  officeId!: string;
  office!: OfficeDetails;
  isLoading = true;
  isEditing = false;

  officeForm!: FormGroup;

  constructor(
    private route: ActivatedRoute,
    private officeService: OfficeService,
    private fb: FormBuilder,
    private dialog: MatDialog,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.officeId = this.route.snapshot.paramMap.get('id')!;
    this.officeService.getOfficeById(this.officeId).subscribe({
      next: (data) => {
        this.office = data;
        this.buildForm(data);
        this.isLoading = false;
        this.officeForm.disable();
      },
      error: (err) => {
        console.error(err);
        this.isLoading = false;
      }
    });
  }

  buildForm(data: OfficeDetails): void {
    this.officeForm = this.fb.group({
      photoUrl: [data.photoUrl],
      city: [data.city, Validators.required],
      street: [data.street, Validators.required],
      houseNumber: [data.houseNumber, Validators.required],
      officeNumber: [data.officeNumber],
      registryPhoneNumber: [
        data.registryPhoneNumber,
        [Validators.required, Validators.pattern(/^\d+$/)]
      ],
      status: [data.status, Validators.required]
    });
  }

  onEdit(): void {
    this.isEditing = true;
    this.officeForm.enable();
  }

  onEditCancel(): void {
    const dialogRef = this.dialog.open(CancelConfirmDialogComponent);

    dialogRef.afterClosed().subscribe((result: boolean) => {
      if (result) {
        this.officeForm.patchValue(this.office);
        this.officeForm.markAsPristine();
        this.officeForm.disable();
        this.isEditing = false;
      }
    });
  }

  onViewCancel(): void {
    this.router.navigate(['/offices']);
  }

  onSave(): void {
    if (this.officeForm.invalid) return;

    const updated: OfficeDetails = {
      ...this.office,
      ...this.officeForm.value
    };

    this.officeService.updateOffice(this.officeId, updated).subscribe({
      next: () => {
        this.office = updated;
        this.officeForm.markAsPristine();
        this.officeForm.disable();
        this.router.navigate(['/offices']);
        this.isEditing = false;
      },
      error: (err) => console.error(err)
    });
  }
}

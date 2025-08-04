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
  public isLoading = true;
  public isEditing = false;

  public officeId!: string;
  public office!: OfficeDetails;
  public officeForm!: FormGroup;

  public constructor(
    private route: ActivatedRoute,
    private officeService: OfficeService,
    private fb: FormBuilder,
    private dialog: MatDialog,
    private router: Router
  ) {}

  public ngOnInit(): void {
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

  public buildForm(data: OfficeDetails): void {
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

  public onEdit(): void {
    this.isEditing = true;
    this.officeForm.enable();
  }

  public onEditCancel(): void {
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

  public onViewCancel(): void {
    this.router.navigate(['/offices']);
  }

  public onSave(): void {
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

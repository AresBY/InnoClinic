import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { CreateOfficeDialogComponent } from './create-office-dialog/create-office-dialog.component';
import { OfficeService } from '@services/office.service';
import { Office } from '@models/office.model';

@Component({
  selector: 'app-offices',
  templateUrl: './offices.component.html'
})
export class OfficesComponent implements OnInit {
  offices: Office[] = [];
  loading = false;
  error = '';

  constructor(
    private dialog: MatDialog,
    private officeService: OfficeService
  ) {}

  ngOnInit(): void {
    this.loadOffices();
  }

  public loadOffices(): void {
    this.loading = true;
    this.officeService.getAllOffices().subscribe({
      next: (data) => {
        this.offices = data;
        this.loading = false;
      },
      error: () => {
        this.loading = false;
      }
    });
  }

  public onCreateOffice() {
    const dialogRef = this.dialog.open(CreateOfficeDialogComponent, {
      width: '600px',
      disableClose: true
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        this.loadOffices();
      }
    });
  }
}

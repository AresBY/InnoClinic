import { Component } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { CreateOfficeDialogComponent } from './create-office-dialog/create-office-dialog.component';

@Component({
  selector: 'app-offices',
  templateUrl: './offices.component.html'
})
export class OfficesComponent {
  constructor(private dialog: MatDialog) {}

  public onCreateOffice() {
    const dialogRef = this.dialog.open(CreateOfficeDialogComponent, {
      width: '600px',
      disableClose: true
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        alert(`Office created with ID: ${result}`);
      }
    });
  }
}

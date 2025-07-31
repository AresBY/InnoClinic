import { Component } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-cancel-confirm-dialog',
  templateUrl: './cancel-confirm-dialog.component.html'
})
export class CancelConfirmDialogComponent {
  constructor(private dialogRef: MatDialogRef<CancelConfirmDialogComponent>) {}

  onYes() {
    this.dialogRef.close(true);
  }

  onNo() {
    this.dialogRef.close(false);
  }
}

import { Component } from '@angular/core';
import { MatDialog, MatDialogRef, MatDialogConfig } from '@angular/material/dialog';

@Component({
  selector: 'app-cancel-confirm-dialog',
  templateUrl: './cancel-confirm-dialog.component.html'
})
export class CancelConfirmDialogComponent {
  public constructor(private dialogRef: MatDialogRef<CancelConfirmDialogComponent>) {}

  public onYes() {
    this.dialogRef.close(true);
  }

  public onNo() {
    this.dialogRef.close(false);
  }

  public static open(
    dialog: MatDialog,
    config?: MatDialogConfig
  ): MatDialogRef<CancelConfirmDialogComponent> {
    const defaultConfig: MatDialogConfig = {
      width: '360px',
      disableClose: true,
      data: {
        title: 'Are you sure?',
        message: 'Do you really want to cancel?',
        confirmButtonText: 'Yes',
        cancelButtonText: 'No'
      }
    };

    return dialog.open(CancelConfirmDialogComponent, {
      ...defaultConfig,
      ...config,
      data: { ...defaultConfig.data, ...(config?.data || {}) }
    });
  }
}

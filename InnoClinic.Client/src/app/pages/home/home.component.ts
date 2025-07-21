import { Component } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { SignInComponent } from '@auth/sign-in/sign-in.component';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html'
})
export class HomeComponent {
  constructor(private dialog: MatDialog) {}

  openSignInModal() {
    const dialogRef = this.dialog.open(SignInComponent, {
      width: '600px'
    });

    /* dialogRef.afterClosed().subscribe((result) => {
      if (result?.success) {
        console.log('User signed in');
        // возможно, обновление состояния, навигация и т.п.
      }
    }); */
  }
}

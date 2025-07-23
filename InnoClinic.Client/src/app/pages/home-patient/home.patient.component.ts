import { Component, OnDestroy } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { SignInPatientComponent } from '@features/auth/sign-in-patient/sign-in.patient.component';
import { AuthService } from '@services/auth.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-home',
  templateUrl: './home.patient.component.html'
})
export class HomePatientComponent implements OnDestroy {
  isAuthenticated = false;
  private sub!: Subscription;

  constructor(
    private dialog: MatDialog,
    private authService: AuthService
  ) {
    this.sub = this.authService.isAuthenticated$.subscribe((auth) => (this.isAuthenticated = auth));
  }

  public openSignInModal() {
    const dialogRef = this.dialog.open(SignInPatientComponent, {
      width: '600px'
    });
  }

  public logout() {
    this.authService.logout();
  }

  public onAuthButtonClick() {
    if (this.isAuthenticated) {
      this.logout();
    } else {
      this.openSignInModal();
    }
  }

  ngOnDestroy() {
    this.sub.unsubscribe();
  }
}

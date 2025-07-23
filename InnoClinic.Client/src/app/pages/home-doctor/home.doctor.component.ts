import { Component, OnDestroy, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { SignInDoctorComponent } from '@features/auth/sign-in-doctor/sign-in.doctor.component';
import { AuthService } from '@services/auth.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-home',
  templateUrl: './home.doctor.component.html'
})
export class HomeDoctorComponent implements OnInit, OnDestroy {
  isAuthenticated = false;
  private sub!: Subscription;

  constructor(
    private dialog: MatDialog,
    private authService: AuthService
  ) {}

  ngOnInit() {
    this.sub = this.authService.isAuthenticated$.subscribe((auth) => {
      this.isAuthenticated = auth;

      // Если пользователь не авторизован, сразу показываем окно входа
      if (!auth) {
        this.openSignInModal();
      }
    });
  }

  public openSignInModal() {
    this.dialog.open(SignInDoctorComponent, {
      width: '600px',
      disableClose: true
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

import { Component, OnDestroy, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { SignInComponent } from '@features/auth/sign-in/sign-in.component';
import { AuthService } from '@services/auth.service';
import { Subscription } from 'rxjs';
import { Router } from '@angular/router';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html'
})
export class HomeComponent implements OnInit, OnDestroy {
  public isAuthenticated = false;
  private sub!: Subscription;

  public constructor(
    private dialog: MatDialog,
    private authService: AuthService,
    private router: Router
  ) {}

  public ngOnInit() {
    this.sub = this.authService.isAuthenticated$.subscribe((auth) => {
      this.isAuthenticated = auth;

      if (!auth) {
        this.openSignInModal();
        console.log('!');
      }
    });
  }

  public openSignInModal() {
    this.dialog.open(SignInComponent, {
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

  public goToOffices() {
    this.router.navigate(['/offices']);
  }

  public ngOnDestroy() {
    this.sub.unsubscribe();
  }
}

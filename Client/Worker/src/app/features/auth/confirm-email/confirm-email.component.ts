import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { AuthService } from '@services/auth.service';

@Component({
  selector: 'app-confirm-email',
  templateUrl: './confirm-email.component.html'
})
export class ConfirmEmailComponent implements OnInit {
  public confirmationResult: string | null = null;

  public constructor(
    private route: ActivatedRoute,
    private http: HttpClient,
    private authService: AuthService
  ) {}

  public ngOnInit(): void {
    const userId = this.route.snapshot.queryParamMap.get('userId');

    if (userId) {
      this.authService.confirmEmail$(userId).subscribe({
        next: () => {
          console.log('Email confirmation successful');
          this.confirmationResult = 'Email успешно подтверждён.';
        },
        error: (error) => {
          console.error('Error confirming email:', error);
          this.confirmationResult = 'Ошибка подтверждения email.';
        }
      });
    } else {
      this.confirmationResult = 'Некорректная ссылка.';
    }
  }
}

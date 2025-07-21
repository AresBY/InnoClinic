import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-confirm-email',
  templateUrl: './confirm-email.component.html'
})
export class ConfirmEmailComponent implements OnInit {
  confirmationResult: string | null = null;

  constructor(
    private route: ActivatedRoute,
    private http: HttpClient
  ) {}

  ngOnInit(): void {
    const userId = this.route.snapshot.queryParamMap.get('userId');
    console.log('UserId from query:', userId);

    if (userId) {
      this.http
        .post(
          'http://localhost:5179/api/Auth/ConfirmEmail',
          { userId },
          { headers: { 'Content-Type': 'application/json' } }
        )
        .subscribe({
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

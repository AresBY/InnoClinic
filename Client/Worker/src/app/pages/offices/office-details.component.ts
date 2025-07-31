import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { OfficeService } from '../../services/office.service';
import { OfficeDetails } from '../../models/office-details.model';

@Component({
  selector: 'app-office-details',
  templateUrl: './office-details.component.html'
})
export class OfficeDetailsComponent implements OnInit {
  officeId!: string;
  office!: OfficeDetails;
  isLoading = true;

  constructor(
    private route: ActivatedRoute,
    private officeService: OfficeService
  ) {}

  ngOnInit(): void {
    this.officeId = this.route.snapshot.paramMap.get('id')!;
    this.officeService.getOfficeById(this.officeId).subscribe({
      next: (data) => {
        this.office = data;
        this.isLoading = false;
      },
      error: (err) => {
        console.error(err);
        this.isLoading = false;
      }
    });
  }
}

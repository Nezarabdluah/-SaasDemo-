import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SiteSettingsService } from '../../services/site-settings.service';
import { SiteSettingsDto } from '../../services/site-settings.service';

@Component({
  selector: 'app-dynamic-footer',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './dynamic-footer.component.html',
  styleUrls: ['./dynamic-footer.component.scss']
})
export class DynamicFooterComponent implements OnInit {
  currentYear: number = new Date().getFullYear();
  settings: SiteSettingsDto | null = null;
  isLoading = true;

  constructor(private siteSettingsService: SiteSettingsService) {}

  ngOnInit(): void {
    this.siteSettingsService.get().subscribe({
      next: (data) => {
        this.settings = data;
        this.isLoading = false;
      },
      error: () => {
        this.isLoading = false;
      }
    });
  }
}

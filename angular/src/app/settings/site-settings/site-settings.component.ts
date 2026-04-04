import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ToasterService } from '@abp/ng.theme.shared';
import { SiteSettingsService } from '../../shared/services/site-settings.service';
import { EnvironmentService } from '@abp/ng.core';
import { MediaPickerModalComponent } from '../../shared/components/media-picker-modal/media-picker-modal.component';

@Component({
  selector: 'app-site-settings',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, MediaPickerModalComponent],
  templateUrl: './site-settings.component.html',
  styleUrls: ['./site-settings.component.scss']
})
export class SiteSettingsComponent implements OnInit {
  form: FormGroup;
  isSaving = false;
  isMediaPickerOpen = false;
  
  logoPreviewUrl = '';

  constructor(
    private fb: FormBuilder,
    private siteSettingsService: SiteSettingsService,
    private toaster: ToasterService,
    private env: EnvironmentService
  ) {}

  ngOnInit() {
    this.buildForm();
    this.loadData();
  }

  buildForm() {
    this.form = this.fb.group({
      siteName: ['', [Validators.required, Validators.maxLength(128)]],
      primaryColor: ['#007bff', [Validators.required, Validators.maxLength(32)]],
      secondaryColor: ['#6c757d', [Validators.required, Validators.maxLength(32)]],
      logoMediaId: [null],
      
      facebookUrl: ['', Validators.maxLength(256)],
      twitterUrl: ['', Validators.maxLength(256)],
      instagramUrl: ['', Validators.maxLength(256)],
      linkedInUrl: ['', Validators.maxLength(256)],
      
      smtpHost: ['', Validators.maxLength(256)],
      smtpPort: [null],
      smtpUserName: ['', Validators.maxLength(256)],
      smtpPassword: ['', Validators.maxLength(256)],
      senderEmail: ['', Validators.maxLength(256)],
      senderName: ['', Validators.maxLength(128)],
    });
  }

  loadData() {
    this.siteSettingsService.get().subscribe((data) => {
      this.form.patchValue({
        siteName: data.siteName,
        primaryColor: data.primaryColor,
        secondaryColor: data.secondaryColor,
        logoMediaId: data.logoMediaId,
        facebookUrl: data.facebookUrl,
        twitterUrl: data.twitterUrl,
        instagramUrl: data.instagramUrl,
        linkedInUrl: data.linkedInUrl,
        smtpHost: data.smtpHost,
        smtpPort: data.smtpPort,
        smtpUserName: data.smtpUserName,
        senderEmail: data.senderEmail,
        senderName: data.senderName
      });
      
      if (data.logoMediaId) {
        const apiUrl = this.env.getEnvironment()?.apis?.default?.url || '';
        this.logoPreviewUrl = `${apiUrl}/api/app/media/${data.logoMediaId}/content`;
      }
    });
  }

  save() {
    if (this.form.invalid) return;

    this.isSaving = true;
    this.siteSettingsService.update(this.form.value).subscribe({
      next: () => {
        this.isSaving = false;
        this.toaster.success('تم الحفظ بنجاح', 'إعدادات الموقع');
      },
      error: () => {
        this.isSaving = false;
        this.toaster.error('حدث خطأ أثناء الحفظ', 'خطأ');
      }
    });
  }
  
  openMediaPicker() {
    this.isMediaPickerOpen = true;
  }

  onImageSelected(url: string) {
    this.logoPreviewUrl = url;
    const urlParts = url.split('/');
    if (urlParts.length >= 2) {
      const id = urlParts[urlParts.length - 2]; 
      this.form.controls['logoMediaId'].setValue(id);
    }
  }
}

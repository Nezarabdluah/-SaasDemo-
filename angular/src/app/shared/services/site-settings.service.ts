import { Injectable } from '@angular/core';
import { RestService } from '@abp/ng.core';
import { Observable } from 'rxjs';

export interface SiteSettingsDto {
  siteName: string;
  primaryColor: string;
  secondaryColor: string;
  logoMediaId?: string;
  facebookUrl?: string;
  twitterUrl?: string;
  instagramUrl?: string;
  linkedInUrl?: string;
  smtpHost?: string;
  smtpPort?: number;
  smtpUserName?: string;
  hasSmtpPassword?: boolean;
  senderEmail?: string;
  senderName?: string;
}

export interface UpdateSiteSettingsDto {
  siteName: string;
  primaryColor: string;
  secondaryColor: string;
  logoMediaId?: string;
  facebookUrl?: string;
  twitterUrl?: string;
  instagramUrl?: string;
  linkedInUrl?: string;
  smtpHost?: string;
  smtpPort?: number;
  smtpUserName?: string;
  smtpPassword?: string;
  senderEmail?: string;
  senderName?: string;
}

@Injectable({
  providedIn: 'root'
})
export class SiteSettingsService {
  constructor(private restService: RestService) {}

  get(): Observable<SiteSettingsDto> {
    return this.restService.request<void, SiteSettingsDto>({
      method: 'GET',
      url: '/api/app/site-settings'
    }, { apiName: 'Default' });
  }

  update(input: UpdateSiteSettingsDto): Observable<SiteSettingsDto> {
    return this.restService.request<UpdateSiteSettingsDto, SiteSettingsDto>({
      method: 'PUT',
      url: '/api/app/site-settings',
      body: input
    }, { apiName: 'Default' });
  }
}

import { Injectable } from '@angular/core';
import { RestService } from '@abp/ng.core';
import { Observable } from 'rxjs';

export interface MediaFileDto {
  id: string;
  fileName: string;
  contentType: string;
  fileSize: number;
  blobName: string;
  folderPath?: string;
  altText?: string;
  width?: number;
  height?: number;
  creationTime: string;
}

export interface PagedResultDto<T> {
  items: T[];
  totalCount: number;
}

export interface MediaFileGetListInput {
  filter?: string;
  folderPath?: string;
  skipCount?: number;
  maxResultCount?: number;
  sorting?: string;
}

@Injectable({
  providedIn: 'root'
})
export class MediaService {
  constructor(private restService: RestService) { }

  getList(input: MediaFileGetListInput): Observable<PagedResultDto<MediaFileDto>> {
    return this.restService.request<MediaFileGetListInput, PagedResultDto<MediaFileDto>>({
      method: 'GET',
      url: '/api/app/media-file',
      params: {
        filter: input.filter,
        folderPath: input.folderPath,
        skipCount: input.skipCount,
        maxResultCount: input.maxResultCount,
        sorting: input.sorting
      }
    }, { apiName: 'Default' });
  }

  // Upload requires FormData because we send a file (byte[])
  upload(file: File, folderPath?: string, altText?: string): Observable<MediaFileDto> {
    const formData = new FormData();
    formData.append('content', file); // Maps to IFormFile content
    
    if (folderPath) {
      formData.append('folderPath', folderPath);
    }
    if (altText) {
      formData.append('altText', altText);
    }

    return this.restService.request<FormData, MediaFileDto>({
      method: 'POST',
      url: '/api/app/media/upload',
      body: formData
    }, { apiName: 'Default' });
  }

  delete(id: string): Observable<void> {
    return this.restService.request<void, void>({
      method: 'DELETE',
      url: `/api/app/media-file/${id}`
    }, { apiName: 'Default' });
  }

  updateMetadata(id: string, altText?: string, folderPath?: string): Observable<MediaFileDto> {
    return this.restService.request<any, MediaFileDto>({
      method: 'PUT',
      url: `/api/app/media-file/${id}/metadata`,
      params: { altText, folderPath }
    }, { apiName: 'Default' });
  }
}

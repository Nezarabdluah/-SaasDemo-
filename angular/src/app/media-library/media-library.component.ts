import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { EnvironmentService } from '@abp/ng.core';
import { MediaService, MediaFileDto, MediaFileGetListInput } from '../shared/services/media.service';

@Component({
  selector: 'app-media-library',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './media-library.component.html',
  styleUrls: ['./media-library.component.scss']
})
export class MediaLibraryComponent implements OnInit {
  mediaFiles: MediaFileDto[] = [];
  totalCount = 0;
  
  filter = '';
  folderPath = '';
  
  isUploading = false;
  selectedFile: File | null = null;
  uploadFolder = '';
  uploadAltText = '';

  copiedId: string | null = null; // For "Copied!" visual feedback

  private apiUrl = '';

  constructor(
    private mediaService: MediaService,
    private environment: EnvironmentService
  ) {
    this.apiUrl = this.environment.getEnvironment()?.apis?.default?.url || '';
  }

  ngOnInit(): void {
    this.loadMedia();
  }

  loadMedia() {
    const input: MediaFileGetListInput = {
      filter: this.filter,
      folderPath: this.folderPath,
      maxResultCount: 50
    };
    this.mediaService.getList(input).subscribe(result => {
      this.mediaFiles = result.items;
      this.totalCount = result.totalCount;
    });
  }

  onFileSelected(event: any) {
    this.selectedFile = event.target.files[0] as File;
  }

  uploadFile() {
    if (!this.selectedFile) return;

    this.isUploading = true;
    this.mediaService.upload(this.selectedFile, this.uploadFolder, this.uploadAltText).subscribe({
      next: () => {
        this.loadMedia();
        this.isUploading = false;
        this.selectedFile = null;
        this.uploadFolder = '';
        this.uploadAltText = '';
      },
      error: (err) => {
        console.error('Upload failed', err);
        this.isUploading = false;
      }
    });
  }

  deleteFile(id: string) {
    if (confirm('هل أنت متأكد من حذف هذا الملف؟')) {
      this.mediaService.delete(id).subscribe(() => {
        this.loadMedia();
      });
    }
  }

  getMediaUrl(id: string) {
    return `${this.apiUrl}/api/app/media/${id}/content`;
  }

  copyUrl(id: string) {
    const url = this.getMediaUrl(id);
    navigator.clipboard.writeText(url).then(() => {
      this.copiedId = id;
      setTimeout(() => this.copiedId = null, 2000);
    });
  }
}

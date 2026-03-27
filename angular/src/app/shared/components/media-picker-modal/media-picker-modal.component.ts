import { Component, EventEmitter, Input, Output, OnChanges, SimpleChanges } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { EnvironmentService } from '@abp/ng.core';
import { MediaService, MediaFileDto, MediaFileGetListInput } from '../../services/media.service';
import { SharedModule } from '../../shared.module';

@Component({
  selector: 'app-media-picker-modal',
  standalone: true,
  imports: [CommonModule, FormsModule, SharedModule],
  templateUrl: './media-picker-modal.component.html',
  styleUrls: ['./media-picker-modal.component.scss']
})
export class MediaPickerModalComponent implements OnChanges {
  @Input() isOpen = false;
  @Output() closed = new EventEmitter<void>();
  @Output() imageSelected = new EventEmitter<string>();

  mediaFiles: MediaFileDto[] = [];
  totalCount = 0;
  filter = '';

  isUploading = false;
  selectedFile: File | null = null;

  private apiUrl = '';

  constructor(
    private mediaService: MediaService,
    private environment: EnvironmentService
  ) {
    this.apiUrl = this.environment.getEnvironment()?.apis?.default?.url || '';
  }

  ngOnChanges(changes: SimpleChanges) {
    if (changes['isOpen'] && this.isOpen) {
      this.loadMedia();
    }
  }

  loadMedia() {
    const input: MediaFileGetListInput = {
      filter: this.filter,
      maxResultCount: 50
    };
    this.mediaService.getList(input).subscribe(result => {
      this.mediaFiles = result.items;
      this.totalCount = result.totalCount;
    });
  }

  getMediaUrl(id: string): string {
    return `${this.apiUrl}/api/app/media/${id}/content`;
  }

  selectImage(file: MediaFileDto) {
    const url = this.getMediaUrl(file.id);
    this.imageSelected.emit(url);
    this.close();
  }

  onFileSelected(event: any) {
    this.selectedFile = event.target.files[0] as File;
  }

  onFileDropped(files: FileList) {
    if (files && files.length > 0) {
      this.selectedFile = files[0];
    }
  }

  uploadAndSelect() {
    if (!this.selectedFile) return;
    this.isUploading = true;
    this.mediaService.upload(this.selectedFile).subscribe({
      next: (result) => {
        this.isUploading = false;
        this.selectedFile = null;
        // Auto-select the newly uploaded image
        const url = this.getMediaUrl(result.id);
        this.imageSelected.emit(url);
        this.close();
      },
      error: () => {
        this.isUploading = false;
      }
    });
  }

  close() {
    this.isOpen = false;
    this.closed.emit();
  }
}

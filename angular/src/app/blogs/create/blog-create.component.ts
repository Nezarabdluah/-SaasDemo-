import { Component, OnInit, ViewChild, inject } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { RestService } from '@abp/ng.core';
import { CommonModule } from '@angular/common';
import { PageModule } from '@abp/ng.components/page';
import { QuillModule, QuillEditorComponent } from 'ngx-quill';
import { MediaPickerModalComponent } from '../../shared/components/media-picker-modal/media-picker-modal.component';

@Component({
  selector: 'app-blog-create',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule, PageModule, QuillModule, MediaPickerModalComponent],
  templateUrl: './blog-create.component.html',
})
export class BlogCreateComponent implements OnInit {
  private fb = inject(FormBuilder);
  private restService = inject(RestService);
  private router = inject(Router);

  @ViewChild(QuillEditorComponent) quillEditor!: QuillEditorComponent;

  form: FormGroup;
  isSaving = false;

  categories: any[] = [];
  tags: any[] = [];

  // Media Picker state
  showCoverPicker = false;
  showQuillPicker = false;

  // إعدادات شريط أدوات Quill
  quillModules: any;

  constructor() {
    const self = this;

    this.quillModules = {
      toolbar: {
        container: [
          [{ header: [1, 2, 3, 4, false] }],
          ['bold', 'italic', 'underline', 'strike'],
          [{ color: [] }, { background: [] }],
          [{ list: 'ordered' }, { list: 'bullet' }],
          [{ align: [] }],
          ['blockquote', 'code-block'],
          ['link', 'image'],
          ['clean']
        ],
        handlers: {
          image: function () {
            // Override default image handler to open our Media Library picker
            self.showQuillPicker = true;
          }
        }
      }
    };

    this.form = this.fb.group({
      title: ['', [Validators.required, Validators.maxLength(256)]],
      slug: ['', [Validators.required, Validators.maxLength(256)]],
      content: ['', [Validators.required]],
      shortDescription: ['', [Validators.maxLength(512)]],
      status: [0],
      publishedAt: [null],
      featuredImageUrl: [null],
      categoryIds: [[]],
      tagIds: [[]]
    });
  }

  ngOnInit() {
    this.restService.request<any, any>({
      method: 'GET',
      url: '/api/app/blog-category?maxResultCount=1000'
    }).subscribe(response => {
      this.categories = response.items || [];
    });

    this.restService.request<any, any>({
      method: 'GET',
      url: '/api/app/blog-tag?maxResultCount=1000'
    }).subscribe(response => {
      this.tags = response.items || [];
    });
  }

  // Cover Image Picker
  onCoverImageSelected(url: string) {
    this.form.patchValue({ featuredImageUrl: url });
    this.showCoverPicker = false;
  }

  // Quill Image Insert
  onQuillImageSelected(url: string) {
    this.showQuillPicker = false;
    const quill = this.quillEditor?.quillEditor;
    if (quill) {
      const range = quill.getSelection(true);
      quill.insertEmbed(range.index, 'image', url);
      quill.setSelection(range.index + 1);
    }
  }

  save() {
    if (this.form.invalid) {
      return;
    }

    this.isSaving = true;

    this.restService.request<any, any>({
      method: 'POST',
      url: '/api/app/blog-post',
      body: this.form.value,
    }).subscribe({
      next: () => {
        this.isSaving = false;
        this.router.navigate(['/blogs']);
      },
      error: (err) => {
        this.isSaving = false;
        console.error(err);
      }
    });
  }
}

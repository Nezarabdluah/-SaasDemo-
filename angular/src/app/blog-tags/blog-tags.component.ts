import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { RestService } from '@abp/ng.core';
import { PageModule } from '@abp/ng.components/page';
import { ThemeSharedModule } from '@abp/ng.theme.shared';
import { BlogTagDto } from './models/blog-tag.model';

@Component({
  selector: 'app-blog-tags',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, PageModule, ThemeSharedModule],
  templateUrl: './blog-tags.component.html'
})
export class BlogTagsComponent implements OnInit {
  Math = Math; // Exposed for template
  private restService = inject(RestService);
  private fb = inject(FormBuilder);
  
  items: BlogTagDto[] = [];
  totalCount = 0;
  
  skipCount = 0;
  maxResultCount = 10;
  
  isModalOpen = false;
  selectedId: string | null = null;
  form: FormGroup;
  
  ngOnInit() {
    this.buildForm();
    this.loadTags();
  }
  
  buildForm() {
    this.form = this.fb.group({
      name: ['', Validators.required]
    });
  }
  
  loadTags() {
    const request = {
      method: 'GET',
      url: '/api/app/blog-tag',
      params: { skipCount: this.skipCount, maxResultCount: this.maxResultCount }
    };
    
    this.restService.request<any, { items: BlogTagDto[], totalCount: number }>(request).subscribe({
      next: (response) => {
        this.items = response.items;
        this.totalCount = response.totalCount;
      },
      error: (err) => console.error(err)
    });
  }
  
  openModal() {
    this.selectedId = null;
    this.form.reset();
    this.isModalOpen = true;
  }
  
  edit(id: string) {
    this.selectedId = id;
    this.form.reset();
    this.restService.request<any, BlogTagDto>({
      method: 'GET',
      url: `/api/app/blog-tag/${id}`
    }).subscribe(tag => {
      this.form.patchValue(tag);
      this.isModalOpen = true;
    });
  }
  
  save() {
    if (this.form.invalid) return;
    
    const request = {
      method: this.selectedId ? 'PUT' : 'POST',
      url: this.selectedId ? `/api/app/blog-tag/${this.selectedId}` : '/api/app/blog-tag',
      body: this.form.value
    };
    
    this.restService.request<any, void>(request).subscribe(() => {
      this.isModalOpen = false;
      this.loadTags();
    });
  }
  
  delete(id: string) {
    if (confirm('Are you sure you want to delete this blog tag?')) {
      this.restService.request<void, void>({
        method: 'DELETE',
        url: `/api/app/blog-tag/${id}`
      }).subscribe(() => {
        this.loadTags();
      });
    }
  }

  nextPage() {
    if (this.skipCount + this.maxResultCount < this.totalCount) {
      this.skipCount += this.maxResultCount;
      this.loadTags();
    }
  }

  prevPage() {
    if (this.skipCount > 0) {
      this.skipCount -= this.maxResultCount;
      this.loadTags();
    }
  }
}

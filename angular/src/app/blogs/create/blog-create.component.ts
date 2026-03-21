import { Component, OnInit, inject } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { RestService } from '@abp/ng.core';
import { CommonModule } from '@angular/common';
import { PageModule } from '@abp/ng.components/page';

@Component({
  selector: 'app-blog-create',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule, PageModule],
  templateUrl: './blog-create.component.html',
})
export class BlogCreateComponent implements OnInit {
  private fb = inject(FormBuilder);
  private restService = inject(RestService);
  private router = inject(Router);

  form: FormGroup;
  isSaving = false;

  categories: any[] = [];

  constructor() {
    this.form = this.fb.group({
      title: ['', [Validators.required, Validators.maxLength(256)]],
      slug: ['', [Validators.required, Validators.maxLength(256)]],
      content: ['', [Validators.required]],
      shortDescription: ['', [Validators.maxLength(512)]],
      status: [0],
      publishedAt: [null],
      featuredImageUrl: [null],
      categoryIds: [[]]
    });
  }

  ngOnInit() {
    this.restService.request<any, any>({
      method: 'GET',
      url: '/api/app/blog-category?maxResultCount=1000'
    }).subscribe(response => {
      this.categories = response.items || [];
    });
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

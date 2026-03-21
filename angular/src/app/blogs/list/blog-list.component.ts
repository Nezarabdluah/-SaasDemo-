import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { RestService } from '@abp/ng.core';
import { BlogPostDto } from '../models/blog-post.dto';
import { PageModule } from '@abp/ng.components/page';

@Component({
  selector: 'app-blog-list',
  standalone: true,
  imports: [CommonModule, RouterModule, PageModule],
  templateUrl: './blog-list.component.html',
})
export class BlogListComponent implements OnInit {
  private restService = inject(RestService);
  
  items: BlogPostDto[] = [];
  totalCount = 0;
  
  // Basic pagination state
  skipCount = 0;
  maxResultCount = 10;
  
  ngOnInit(): void {
    this.loadPosts();
  }

  loadPosts() {
    const request = {
      method: 'GET',
      url: '/api/app/blog-post',
      params: { skipCount: this.skipCount, maxResultCount: this.maxResultCount },
    };

    this.restService.request<any, { items: BlogPostDto[], totalCount: number }>(request).subscribe({
      next: (response) => {
        this.items = response.items;
        this.totalCount = response.totalCount;
      },
      error: (err) => console.error(err)
    });
  }

  delete(id: string) {
    if(confirm('Are you sure you want to delete this blog post?')) {
      this.restService.request<void, void>({
        method: 'DELETE',
        url: `/api/app/blog-post/${id}`,
      }).subscribe(() => {
        this.loadPosts();
      });
    }
  }

  nextPage() {
    if (this.skipCount + this.maxResultCount < this.totalCount) {
      this.skipCount += this.maxResultCount;
      this.loadPosts();
    }
  }

  prevPage() {
    if (this.skipCount > 0) {
      this.skipCount -= this.maxResultCount;
      this.loadPosts();
    }
  }
}

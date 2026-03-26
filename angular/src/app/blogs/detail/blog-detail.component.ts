import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { RestService } from '@abp/ng.core';
import { BlogPostDto } from '../models/blog-post.dto';
import { PageModule } from '@abp/ng.components/page';
import { SharedModule } from '../../shared/shared.module';

@Component({
  selector: 'app-blog-detail',
  standalone: true,
  imports: [CommonModule, RouterModule, PageModule, SharedModule],
  templateUrl: './blog-detail.component.html',
})
export class BlogDetailComponent implements OnInit {
  private route = inject(ActivatedRoute);
  private restService = inject(RestService);

  post?: BlogPostDto;

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.loadPost(id);
    }
  }

  loadPost(id: string) {
    this.restService.request<any, BlogPostDto>({
      method: 'GET',
      url: `/api/app/blog-post/${id}`,
    }).subscribe({
      next: (data) => {
        this.post = data;
      },
      error: (err) => console.error(err)
    });
  }
}

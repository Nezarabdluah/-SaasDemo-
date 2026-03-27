import { Component, OnInit, inject } from '@angular/core';
import { Title, Meta } from '@angular/platform-browser';
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
  private titleService = inject(Title);
  private metaService = inject(Meta);

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
        this.updateSeoTags(this.post);
      },
      error: (err) => console.error(err)
    });
  }

  updateSeoTags(post: BlogPostDto) {
    // 1. Title
    this.titleService.setTitle(`${post.title} | SaasDemo Blog`);
    
    // 2. Meta Description
    if (post.shortDescription) {
      this.metaService.updateTag({ name: 'description', content: post.shortDescription });
      this.metaService.updateTag({ property: 'og:description', content: post.shortDescription });
    }
    
    // 3. Open Graph Metadata
    this.metaService.updateTag({ property: 'og:title', content: post.title || '' });
    this.metaService.updateTag({ property: 'og:type', content: 'article' });
    
    if (post.featuredImageUrl) {
      // Assuming featuredImageUrl is a complete relative URL like /api/app/media/...
      this.metaService.updateTag({ property: 'og:image', content: post.featuredImageUrl });
    }
  }
}

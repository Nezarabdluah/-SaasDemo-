---
description: How to add dynamic SEO Meta Tags to Angular components for Googlebot indexing
---

# SEO Meta Tags in Angular (Without SSR)

## When to Use
When you need SEO for pages in an Angular SPA that can't use SSR (e.g., ABP Lepton-X projects).

## Steps

### 1. Import Services
```typescript
import { Title, Meta } from '@angular/platform-browser';
import { inject } from '@angular/core';
```

### 2. Inject in Component
```typescript
private titleService = inject(Title);
private metaService = inject(Meta);
```

### 3. Update Tags After Data Loads
```typescript
updateSeoTags(data: any) {
  // Page Title (appears in browser tab + Google results)
  this.titleService.setTitle(`${data.title} | YourSiteName`);

  // Standard Meta Description
  this.metaService.updateTag({ name: 'description', content: data.description });

  // Open Graph (Facebook, LinkedIn, WhatsApp)
  this.metaService.updateTag({ property: 'og:title', content: data.title });
  this.metaService.updateTag({ property: 'og:description', content: data.description });
  this.metaService.updateTag({ property: 'og:type', content: 'article' });
  this.metaService.updateTag({ property: 'og:image', content: data.imageUrl });

  // Twitter Cards (optional)
  this.metaService.updateTag({ name: 'twitter:card', content: 'summary_large_image' });
  this.metaService.updateTag({ name: 'twitter:title', content: data.title });
}
```

## Why This Works Without SSR
- **Googlebot 2024+** renders JavaScript fully before indexing
- **Social crawlers** (Facebook, Twitter) also execute JS in most cases
- Only very old/simple crawlers need static HTML — these are increasingly rare

## ABP Project Notes
- Use `shortDescription` (not `summary`) for BlogPostDto
- Use `featuredImageUrl` (not `coverImageId`) 
- Fields must exist in the DTO interface before referencing them

import { Routes } from '@angular/router';
import { BlogListComponent } from './list/blog-list.component';
import { BlogCreateComponent } from './create/blog-create.component';
import { BlogDetailComponent } from './detail/blog-detail.component';

export const blogRoutes: Routes = [
  { path: '', component: BlogListComponent },
  { path: 'new', component: BlogCreateComponent },
  { path: ':id', component: BlogDetailComponent }
];

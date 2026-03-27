import { CoreModule } from '@abp/ng.core';
import { NgbDropdownModule } from '@ng-bootstrap/ng-bootstrap';
import { NgModule } from '@angular/core';
import { ThemeSharedModule } from '@abp/ng.theme.shared';
import { NgxValidateCoreModule } from '@ngx-validate/core';

import { CommentListComponent } from './components/comments/comment-list/comment-list.component';
import { CommentItemComponent } from './components/comments/comment-item/comment-item.component';
import { CommentFormComponent } from './components/comments/comment-form/comment-form.component';
import { DragDropDirective } from './directives/drag-drop.directive';

@NgModule({
  declarations: [
    CommentListComponent,
    CommentItemComponent,
    CommentFormComponent,
    DragDropDirective
  ],
  imports: [
    CoreModule,
    ThemeSharedModule,
    NgbDropdownModule,
    NgxValidateCoreModule
  ],
  exports: [
    CoreModule,
    ThemeSharedModule,
    NgbDropdownModule,
    NgxValidateCoreModule,
    CommentListComponent,
    CommentItemComponent,
    CommentFormComponent,
    DragDropDirective
  ],
  providers: []
})
export class SharedModule {}

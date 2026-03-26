import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CommentDto, CommentService } from '../../../services/comment.service';
import { ConfigStateService } from '@abp/ng.core';

export interface CommentNode extends CommentDto {
  replies: CommentNode[];
}

@Component({
  selector: 'app-comment-item',
  standalone: false,
  templateUrl: './comment-item.component.html',
  styleUrls: ['./comment-item.component.scss']
})
export class CommentItemComponent {
  @Input() comment!: CommentNode;
  @Input() entityType!: string;
  @Input() entityId!: string;

  @Output() commentChanged = new EventEmitter<void>();

  showReplyForm = false;
  isEditing = false;

  constructor(
    private commentService: CommentService,
    private config: ConfigStateService
  ) {}

  get currentUserId(): string | undefined {
    return this.config.getOne('currentUser')?.id;
  }

  get isMyComment(): boolean {
    return this.currentUserId === this.comment.creatorId;
  }

  toggleReplyForm(): void {
    this.showReplyForm = !this.showReplyForm;
    this.isEditing = false;
  }

  toggleEditForm(): void {
    this.isEditing = !this.isEditing;
    this.showReplyForm = false;
  }

  deleteComment(): void {
    if (!confirm('هل أنت متأكد من حذف هذا التعليق؟')) return;

    this.commentService.delete(this.comment.id).subscribe(() => {
      this.commentChanged.emit();
    });
  }

  onCommentSaved(): void {
    this.showReplyForm = false;
    this.isEditing = false;
    this.commentChanged.emit();
  }
}

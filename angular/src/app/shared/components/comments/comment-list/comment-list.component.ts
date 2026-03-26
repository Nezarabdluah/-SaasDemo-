import { Component, Input, OnInit } from '@angular/core';
import { CommentService, CommentDto } from '../../../services/comment.service';
import { CommentNode } from '../comment-item/comment-item.component';
import { ConfigStateService } from '@abp/ng.core';

@Component({
  selector: 'app-comment-list',
  standalone: false,
  templateUrl: './comment-list.component.html',
  styleUrls: ['./comment-list.component.scss']
})
export class CommentListComponent implements OnInit {
  @Input() entityType!: string;
  @Input() entityId!: string;

  commentsData: CommentDto[] = [];
  commentTree: CommentNode[] = [];
  totalCount = 0;
  isLoading = false;

  constructor(
    private commentService: CommentService,
    private config: ConfigStateService
  ) {}

  get isAuthenticated(): boolean {
    return this.config.getOne('currentUser')?.isAuthenticated || false;
  }

  ngOnInit(): void {
    this.loadComments();
  }

  loadComments(): void {
    if (!this.entityType || !this.entityId) return;

    this.isLoading = true;
    this.commentService.getList(this.entityType, this.entityId).subscribe({
      next: (result) => {
        this.commentsData = result.items;
        this.totalCount = result.totalCount;
        this.buildCommentTree();
        this.isLoading = false;
      },
      error: () => {
        this.isLoading = false;
      }
    });
  }

  buildCommentTree(): void {
    this.commentTree = [];
    const map = new Map<string, CommentNode>();

    // Create a new array of full node objects
    this.commentsData.forEach(item => {
      map.set(item.id, { ...item, replies: [] });
    });

    // Link children to their parents
    this.commentsData.forEach(item => {
      const node = map.get(item.id);
      if (item.repliedCommentId) {
        const parent = map.get(item.repliedCommentId);
        if (parent && node) {
          parent.replies.push(node);
        }
      } else {
        // It's a root comment
        if (node) {
          this.commentTree.push(node);
        }
      }
    });

    // Sort by creation time (newest or oldest depending on UX preference)
    this.commentTree.sort((a, b) => new Date(b.creationTime).getTime() - new Date(a.creationTime).getTime());
  }
}

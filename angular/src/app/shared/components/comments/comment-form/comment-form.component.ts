import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { CommentService } from '../../../services/comment.service';

@Component({
  selector: 'app-comment-form',
  standalone: false,
  templateUrl: './comment-form.component.html'
})
export class CommentFormComponent implements OnInit {
  @Input() entityType!: string;
  @Input() entityId!: string;
  @Input() repliedCommentId?: string;
  @Input() editCommentId?: string;
  @Input() initialText: string = '';

  @Output() commentSaved = new EventEmitter<void>();
  @Output() cancel = new EventEmitter<void>();

  form!: FormGroup;
  isSaving = false;

  constructor(
    private fb: FormBuilder,
    private commentService: CommentService
  ) {}

  ngOnInit(): void {
    this.form = this.fb.group({
      text: [this.initialText, [Validators.required, Validators.maxLength(1000)]]
    });
  }

  submit(): void {
    if (this.form.invalid) return;

    this.isSaving = true;
    const text = this.form.value.text;

    const request = this.editCommentId
      ? this.commentService.update(this.editCommentId, { text })
      : this.commentService.create(this.entityType, this.entityId, {
          entityType: this.entityType,
          entityId: this.entityId,
          text,
          repliedCommentId: this.repliedCommentId
        });

    request.subscribe({
      next: () => {
        this.isSaving = false;
        this.form.reset();
        this.commentSaved.emit();
      },
      error: () => {
        this.isSaving = false;
      }
    });
  }
}

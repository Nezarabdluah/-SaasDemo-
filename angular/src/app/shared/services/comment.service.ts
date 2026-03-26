import { Injectable } from '@angular/core';
import { RestService } from '@abp/ng.core';
import { Observable } from 'rxjs';

export interface CommentDto {
  id: string;
  entityType: string;
  entityId: string;
  text: string;
  repliedCommentId?: string;
  creatorId: string;
  creationTime: string;
  author?: {
    name: string;
    surname: string;
    userName: string;
  };
}

export interface CommentListResultDto {
  items: CommentDto[];
  totalCount: number;
}

export interface CreateCommentInput {
  entityType: string;
  entityId: string;
  text: string;
  repliedCommentId?: string;
  idempotencyToken: string;
}

export interface UpdateCommentInput {
  text: string;
}

@Injectable({
  providedIn: 'root'
})
export class CommentService {
  constructor(private restService: RestService) {}

  getList(entityType: string, entityId: string): Observable<CommentListResultDto> {
    return this.restService.request<void, CommentListResultDto>({
      method: 'GET',
      url: `/api/cms-kit-public/comments/${entityType}/${entityId}`
    }, { apiName: 'CmsKitPublic' });
  }

  create(entityType: string, entityId: string, input: CreateCommentInput): Observable<CommentDto> {
    // Auto-generate idempotencyToken if not provided
    if (!input.idempotencyToken) {
      input.idempotencyToken = crypto.randomUUID();
    }
    return this.restService.request<CreateCommentInput, CommentDto>({
      method: 'POST',
      url: `/api/cms-kit-public/comments/${entityType}/${entityId}`,
      body: input
    }, { apiName: 'CmsKitPublic' });
  }

  update(id: string, input: UpdateCommentInput): Observable<CommentDto> {
    return this.restService.request<UpdateCommentInput, CommentDto>({
      method: 'PUT',
      url: `/api/cms-kit-public/comments/${id}`,
      body: input
    }, { apiName: 'CmsKitPublic' });
  }

  delete(id: string): Observable<void> {
    return this.restService.request<void, void>({
      method: 'DELETE',
      url: `/api/cms-kit-public/comments/${id}`
    }, { apiName: 'CmsKitPublic' });
  }
}

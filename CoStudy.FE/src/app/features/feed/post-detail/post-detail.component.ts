import {
  ChangeDetectionStrategy, Component, OnInit, inject, signal, ElementRef, ViewChild
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { PostService } from '../../../core/services/post.service';
import { CommentService } from '../../../core/services/comment.service';
import { Post, PostImage } from '../../../core/models/post.model';
import { AvatarComponent } from '../../../shared/components/avatar/avatar.component';

/** Raw CommentViewModel from the server */
interface CommentVM {
  oid: string;
  post_id: string;
  content: string;
  author_id: string;
  author_name: string;
  author_avatar: string;
  created_date: string;
  upvote_count: number;
  downvote_count: number;
  replies_count: number;
  is_vote_by_current?: boolean;
  is_downvote_by_current?: boolean;
}

@Component({
  selector: 'app-post-detail',
  standalone: true,
  imports: [CommonModule, RouterLink, FormsModule, AvatarComponent],
  templateUrl: './post-detail.component.html',
  styleUrls: ['./post-detail.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class PostDetailComponent implements OnInit {
  private readonly route = inject(ActivatedRoute);
  private readonly postService = inject(PostService);
  private readonly commentService = inject(CommentService);

  @ViewChild('commentInput') commentInputRef!: ElementRef<HTMLTextAreaElement>;

  post = signal<Post | null>(null);
  comments = signal<CommentVM[]>([]);
  loadingPost = signal(true);
  loadingComments = signal(false);
  submittingComment = signal(false);
  commentText = signal('');
  postError = signal('');
  commentError = signal('');

  // Lightbox
  lightboxUrl = signal<string | null>(null);

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id') ?? '';
    this.loadPost(id);
  }

  private loadPost(id: string): void {
    this.loadingPost.set(true);
    this.postService.getById(id).subscribe({
      next: (res: any) => {
        const raw = res?.result ?? res;
        this.post.set(this.mapPost(raw));
        this.loadingPost.set(false);
        this.loadComments(id);
      },
      error: () => {
        this.postError.set('Không thể tải bài viết.');
        this.loadingPost.set(false);
      }
    });
  }

  private loadComments(postId: string): void {
    this.loadingComments.set(true);
    this.commentService.getByPostId({ postId, skip: 0, count: 50 }).subscribe({
      next: (res: any) => {
        const raw: CommentVM[] = res?.result ?? [];
        this.comments.set(raw);
        this.loadingComments.set(false);
      },
      error: () => this.loadingComments.set(false)
    });
  }

  submitComment(): void {
    const text = this.commentText().trim();
    if (!text) return;
    const p = this.post();
    if (!p) return;

    this.submittingComment.set(true);
    this.commentError.set('');
    this.commentService.addComment({ postId: p.id, content: text }).subscribe({
      next: (res: any) => {
        const raw: CommentVM = res?.result ?? res;
        this.comments.update(list => [raw, ...list]);
        this.commentText.set('');
        this.submittingComment.set(false);
      },
      error: () => {
        this.commentError.set('Không thể gửi bình luận.');
        this.submittingComment.set(false);
      }
    });
  }

  onUpvote(): void {
    const p = this.post();
    if (!p) return;
    this.postService.upvote(p.id).subscribe();
    this.post.update(prev => prev ? { ...prev, isUpvoted: !prev.isUpvoted, upvoteCount: (prev.upvoteCount ?? 0) + (prev.isUpvoted ? -1 : 1) } : prev);
  }

  onDownvote(): void {
    const p = this.post();
    if (!p) return;
    this.postService.downvote(p.id).subscribe();
    this.post.update(prev => prev ? { ...prev, isDownvoted: !prev.isDownvoted, downvoteCount: (prev.downvoteCount ?? 0) + (prev.isDownvoted ? -1 : 1) } : prev);
  }

  onSave(): void {
    const p = this.post();
    if (!p) return;
    this.postService.savePost(p.id).subscribe();
    this.post.update(prev => prev ? { ...prev, isSaved: !prev.isSaved } : prev);
  }

  openLightbox(url: string): void {
    this.lightboxUrl.set(url);
  }

  closeLightbox(): void {
    this.lightboxUrl.set(null);
  }

  get postImages(): string[] {
    const imgs = this.post()?.image_contents;
    if (imgs?.length) return imgs.map((img: PostImage) => img.image_hash).filter(Boolean);
    return [];
  }

  timeAgo(date: string | undefined): string {
    if (!date) return '';
    const diff = Date.now() - new Date(date).getTime();
    const mins = Math.floor(diff / 60000);
    if (mins < 1) return 'Vừa xong';
    if (mins < 60) return `${mins} phút trước`;
    const hours = Math.floor(mins / 60);
    if (hours < 24) return `${hours} giờ trước`;
    const days = Math.floor(hours / 24);
    if (days < 30) return `${days} ngày trước`;
    return new Date(date).toLocaleDateString('vi-VN');
  }

  private mapPost(raw: any): Post {
    return {
      id: raw.oid,
      objectId: raw.oid,
      oid: raw.oid,
      title: raw.title,
      content: (raw.string_contents as any[])?.[0]?.content ?? '',
      image_contents: raw.image_contents ?? [],
      authorId: raw.author_id,
      author_name: raw.author_name,
      author_avatar: raw.author_avatar,
      author_email: raw.author_email,
      postType: raw.post_type,
      post_type_name: raw.post_type_name,
      upvoteCount: raw.upvote ?? 0,
      downvoteCount: raw.downvote ?? 0,
      commentCount: raw.comments_count ?? 0,
      isUpvoted: raw.is_vote_by_current ?? false,
      isDownvoted: raw.is_downvote_by_current ?? false,
      isSaved: raw.is_save_by_current ?? false,
      createdAt: raw.created_date,
      updatedAt: undefined
    };
  }
}

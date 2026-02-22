import { Component, inject, input, output, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { Post } from '../../../core/models/post.model';
import { AvatarComponent } from '../avatar/avatar.component';
import { PostService } from '../../../core/services/post.service';

@Component({
  selector: 'app-post-card',
  standalone: true,
  imports: [CommonModule, RouterLink, AvatarComponent],
  templateUrl: './post-card.component.html',
  styleUrls: ['./post-card.component.scss']
})
export class PostCardComponent {
  private postService = inject(PostService);
  Math = Math;
  post = input.required<Post>();

  // Local optimistic state
  localUpvotes   = signal<number | null>(null);
  localDownvotes = signal<number | null>(null);
  localSaved     = signal<boolean | null>(null);
  localUpvoted   = signal<boolean | null>(null);
  localDownvoted = signal<boolean | null>(null);

  // Keep output for backward compatibility (parent can still listen)
  upvote = output<string>();
  downvote = output<string>();
  save = output<string>();
  share = output<string>();

  get displayUpvotes(): number   { return this.localUpvotes()   ?? this.post().upvoteCount   ?? 0; }
  get displayDownvotes(): number { return this.localDownvotes() ?? this.post().downvoteCount ?? 0; }
  get isSavedNow(): boolean      { return this.localSaved()     ?? this.post().isSaved       ?? false; }
  get isUpvotedNow(): boolean    { return this.localUpvoted()   ?? this.post().isUpvoted     ?? false; }
  get isDownvotedNow(): boolean  { return this.localDownvoted() ?? this.post().isDownvoted   ?? false; }

  get authorName(): string {
    const p = this.post();
    // Prefer flat fields from API response
    if (p.author_name) return p.author_name;
    const author = p.author;
    if (!author) return 'Ẩn danh';
    return `${author.firstName ?? ''} ${author.lastName ?? ''}`.trim() || author.email || 'Ẩn danh';
  }

  get authorAvatar(): string {
    const p = this.post();
    return p.author_avatar ?? p.author?.avatarHash ?? '';
  }

  get authorInitial(): string {
    return (this.authorName[0] ?? '?').toUpperCase();
  }

  get isQuestion(): boolean {
    const p = this.post();
    const name = (p.post_type_name ?? '').toLowerCase();
    return p.postType === 0 || name.includes('question') || name.includes('hỏi');
  }

  get postTypeBadgeClass(): string {
    return this.isQuestion
      ? 'inline-flex items-center px-2 py-0.5 rounded-full text-[10px] font-semibold bg-violet-500/20 text-violet-300 border border-violet-500/30'
      : 'inline-flex items-center px-2 py-0.5 rounded-full text-[10px] font-semibold bg-indigo-500/20 text-indigo-300 border border-indigo-500/30';
  }

  get postTypeLabel(): string {
    const name = this.post().post_type_name;
    if (name) return name;
    return this.isQuestion ? 'Câu hỏi' : 'Chia sẻ';
  }

  get timeAgo(): string {
    const date = this.post().createdAt;
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

  /** Unified list of image URLs: backend stores the URL in image_hash */
  get postImages(): string[] {
    const imageContents = this.post().image_contents;
    if (imageContents?.length) {
      return imageContents.map(img => img.image_hash).filter(Boolean);
    }
    return this.post().images ?? [];
  }

  onUpvote(): void {
    const next = !this.isUpvotedNow;
    this.localUpvoted.set(next);
    this.localUpvotes.set(this.displayUpvotes + (next ? 1 : -1));
    this.postService.upvote(this.post().id).subscribe();
    this.upvote.emit(this.post().id);
  }

  onDownvote(): void {
    const next = !this.isDownvotedNow;
    this.localDownvoted.set(next);
    this.localDownvotes.set(this.displayDownvotes + (next ? 1 : -1));
    this.postService.downvote(this.post().id).subscribe();
    this.downvote.emit(this.post().id);
  }

  onSave(): void {
    const next = !this.isSavedNow;
    this.localSaved.set(next);
    this.postService.savePost(this.post().id).subscribe();
    this.save.emit(this.post().id);
  }

  onShare(): void {
    this.share.emit(this.post().id);
  }
}

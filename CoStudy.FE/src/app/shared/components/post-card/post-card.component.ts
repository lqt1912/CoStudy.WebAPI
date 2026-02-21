import { Component, input, output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { Post } from '../../../core/models/post.model';
import { AvatarComponent } from '../avatar/avatar.component';

@Component({
  selector: 'app-post-card',
  standalone: true,
  imports: [CommonModule, RouterLink, AvatarComponent],
  templateUrl: './post-card.component.html',
  styleUrls: ['./post-card.component.scss']
})
export class PostCardComponent {
  Math = Math;
  post = input.required<Post>();

  upvote = output<string>();
  downvote = output<string>();
  save = output<string>();
  share = output<string>();

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

  onUpvote(): void {
    this.upvote.emit(this.post().id);
  }

  onDownvote(): void {
    this.downvote.emit(this.post().id);
  }

  onSave(): void {
    this.save.emit(this.post().id);
  }

  onShare(): void {
    this.share.emit(this.post().id);
  }
}

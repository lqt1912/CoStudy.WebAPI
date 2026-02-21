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
    const author = this.post().author;
    if (!author) return 'Ẩn danh';
    return `${author.firstName ?? ''} ${author.lastName ?? ''}`.trim() || author.email || 'Ẩn danh';
  }

  get authorInitial(): string {
    return (this.authorName[0] ?? '?').toUpperCase();
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

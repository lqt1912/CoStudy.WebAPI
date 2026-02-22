import { Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { PostService } from '../../../core/services/post.service';
import { Post } from '../../../core/models/post.model';
import { PostCardComponent } from '../../../shared/components/post-card/post-card.component';

@Component({
  selector: 'app-saved-posts',
  standalone: true,
  imports: [CommonModule, RouterLink, PostCardComponent],
  templateUrl: './saved-posts.component.html',
  styleUrls: ['./saved-posts.component.scss']
})
export class SavedPostsComponent implements OnInit {
  private postService = inject(PostService);

  posts = signal<Post[]>([]);
  loading = signal(true);
  error = signal('');

  private skip = 0;
  private readonly count = 20;
  hasMore = signal(true);
  loadingMore = signal(false);

  ngOnInit(): void {
    this.loadPosts();
  }

  private loadPosts(): void {
    this.loading.set(true);
    this.postService.getSavedPosts({ skip: this.skip, count: this.count }).subscribe({
      next: (res: any) => {
        const raw: any[] = res?.result ?? [];
        const mapped = raw.map(p => this.mapPost(p));
        this.posts.set(mapped);
        this.hasMore.set(raw.length === this.count);
        this.loading.set(false);
      },
      error: () => {
        this.error.set('Không thể tải bài viết đã lưu.');
        this.loading.set(false);
      }
    });
  }

  loadMore(): void {
    if (this.loadingMore() || !this.hasMore()) return;
    this.loadingMore.set(true);
    this.skip += this.count;
    this.postService.getSavedPosts({ skip: this.skip, count: this.count }).subscribe({
      next: (res: any) => {
        const raw: any[] = res?.result ?? [];
        this.posts.update(prev => [...prev, ...raw.map(p => this.mapPost(p))]);
        this.hasMore.set(raw.length === this.count);
        this.loadingMore.set(false);
      },
      error: () => this.loadingMore.set(false)
    });
  }

  unsave(postId: string): void {
    this.postService.savePost(postId).subscribe({
      next: () => this.posts.update(prev => prev.filter(p => p.id !== postId))
    });
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
      isSaved: true,
      createdAt: raw.created_date,
      updatedAt: undefined
    };
  }
}

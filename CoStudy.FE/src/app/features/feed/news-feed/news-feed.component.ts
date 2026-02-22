import { ChangeDetectionStrategy, Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PostService } from '../../../core/services/post.service';
import { Post } from '../../../core/models/post.model';
import { PostCardComponent } from '../../../shared/components/post-card/post-card.component';
import { LoaderComponent } from '../../../shared/components/loader/loader.component';
import { CreatePostComponent } from '../../../shared/components/create-post/create-post.component';

@Component({
  selector: 'app-news-feed',
  standalone: true,
  imports: [CommonModule, PostCardComponent, LoaderComponent, CreatePostComponent],
  templateUrl: './news-feed.component.html',
  styleUrls: ['./news-feed.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class NewsFeedComponent implements OnInit {
  private readonly postService = inject(PostService);

  posts = signal<Post[]>([]);
  loading = signal(true);
  page = 0;
  pageSize = 10;

  ngOnInit(): void {
    this.loadPosts();
  }

  loadPosts(): void {
    this.loading.set(true);
    this.postService.getNewsFeed({ skip: this.page * this.pageSize, count: this.pageSize }).subscribe({
      next: (res: any) => {
        const raw: any[] = res?.result ?? res?.data ?? [];
        this.posts.update(prev => [...prev, ...raw.map(p => this.mapPost(p))]);
        this.loading.set(false);
      },
      error: () => this.loading.set(false)
    });
  }

  /** Normalize raw PostViewModel (System.Text.Json keys) into the frontend Post shape */
  private mapPost(raw: any): Post {
    return {
      id: raw.oid,
      objectId: raw.oid,
      oid: raw.oid,
      title: raw.title,
      // string_contents is List<PostContent>; each item has { contentType, content }
      content: (raw.string_contents as any[])?.[0]?.content ?? '',
      // image_contents is List<Image>; each item has { image_url, image_hash, discription }
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

  loadMore(): void {
    this.page++;
    this.loadPosts();
  }

  onUpvote(postId: string): void {
    this.postService.upvote(postId).subscribe();
  }

  onDownvote(postId: string): void {
    this.postService.downvote(postId).subscribe();
  }

  onSave(postId: string): void {
    this.postService.savePost(postId).subscribe();
  }

  onShare(postId: string): void {
    // TODO: Implement share modal
  }

  onPostCreated(post: any): void {
    if (post) this.posts.update(list => [this.mapPost(post), ...list]);
  }
}

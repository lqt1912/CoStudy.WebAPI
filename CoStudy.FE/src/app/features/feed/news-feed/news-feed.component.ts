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
  private postService = inject(PostService);

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
        const newPosts = res?.result ?? res?.data ?? [];
        this.posts.update(prev => [...prev, ...newPosts]);
        this.loading.set(false);
      },
      error: () => this.loading.set(false)
    });
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
    if (post) this.posts.update(list => [post, ...list]);
  }
}

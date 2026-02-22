import { Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { UserService } from '../../../core/services/user.service';
import { AuthService } from '../../../core/services/auth.service';
import { PostService } from '../../../core/services/post.service';
import { User } from '../../../core/models/user.model';
import { Post, PostImage } from '../../../core/models/post.model';
import { AvatarComponent } from '../../../shared/components/avatar/avatar.component';
import { PostCardComponent } from '../../../shared/components/post-card/post-card.component';

@Component({
  selector: 'app-user-profile',
  standalone: true,
  imports: [CommonModule, RouterLink, AvatarComponent, PostCardComponent],
  templateUrl: './user-profile.component.html',
  styleUrls: ['./user-profile.component.scss']
})
export class UserProfileComponent implements OnInit {
  private route = inject(ActivatedRoute);
  private userService = inject(UserService);
  private authService = inject(AuthService);
  private postService = inject(PostService);

  user = signal<User | null>(null);
  posts = signal<Post[]>([]);
  isLoading = signal(true);
  loadingPosts = signal(false);
  errorMessage = signal('');
  currentUserId = '';
  activeTab = signal<'posts' | 'info'>('posts');

  ngOnInit(): void {
    this.currentUserId = this.authService.currentAuth?.id || '';
    this.route.params.subscribe(params => {
      const userId = params['id'];
      if (userId) {
        this.loadUser(userId);
      }
    });
  }

  loadUser(userId: string): void {
    this.isLoading.set(true);
    this.userService.getUserById(userId).subscribe({
      next: (res: any) => {
        const raw = res?.result ?? res;
        this.user.set(this.mapUser(raw));
        this.isLoading.set(false);
        this.loadPosts(raw.oid ?? userId);
      },
      error: () => {
        this.errorMessage.set('Không tìm thấy người dùng');
        this.isLoading.set(false);
      }
    });
  }

  private loadPosts(userId: string): void {
    this.loadingPosts.set(true);
    this.postService.getByUserId({ userId, skip: 0, count: 20 }).subscribe({
      next: (res: any) => {
        const raw: any[] = res?.result ?? [];
        this.posts.set(raw.map(p => this.mapPost(p)));
        this.loadingPosts.set(false);
      },
      error: () => this.loadingPosts.set(false)
    });
  }

  toggleFollow(): void {
    const u = this.user();
    if (!u) return;
    if (u.isFollowing) {
      this.userService.unfollow(u.oid).subscribe({
        next: () => this.user.update(prev => prev ? {
          ...prev,
          isFollowing: false,
          followerCount: (prev.followerCount ?? 0) - 1
        } : prev)
      });
    } else {
      this.userService.follow({ followingId: u.oid }).subscribe({
        next: () => this.user.update(prev => prev ? {
          ...prev,
          isFollowing: true,
          followerCount: (prev.followerCount ?? 0) + 1
        } : prev)
      });
    }
  }

  get isOwnProfile(): boolean {
    return this.user()?.oid === this.currentUserId;
  }

  get displayName(): string {
    const u = this.user();
    if (!u) return '';
    return `${u.first_name ?? ''} ${u.last_name ?? ''}`.trim() || 'Người dùng';
  }

  get addressLine(): string {
    const addr = this.user()?.address;
    return [addr?.district, addr?.city].filter(v => !!v).join(', ');
  }

  get initials(): string {
    const f = this.user()?.first_name?.[0] ?? '';
    const l = this.user()?.last_name?.[0] ?? '';
    return (f + l).toUpperCase() || '?';
  }

  private mapUser(raw: any): User {
    return {
      oid: raw.oid,
      id: raw.oid,
      email: raw.email ?? '',
      first_name: raw.first_name ?? '',
      last_name: raw.last_name ?? '',
      avatar_hash: raw.avatar_hash ?? raw.avatar?.image_hash ?? '',
      bio: raw.bio ?? '',
      phone_number: raw.phone_number,
      date_of_birth: raw.date_of_birth,
      gender: raw.gender,
      address: raw.address,
      additional_infos: raw.additional_infos ?? [],
      followerCount: raw.followers ?? 0,
      followingCount: raw.followings ?? 0,
      isFollowing: false,
      fields: raw.fields ?? [],
      created_date: raw.created_date,
      status: raw.status
    };
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

import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { UserService } from '../../../core/services/user.service';
import { AuthService } from '../../../core/services/auth.service';
import { User } from '../../../core/models/user.model';
import { AvatarComponent } from '../../../shared/components/avatar/avatar.component';

@Component({
  selector: 'app-user-profile',
  standalone: true,
  imports: [
    CommonModule,
    RouterLink,
    AvatarComponent
  ],
  templateUrl: './user-profile.component.html',
  styleUrls: ['./user-profile.component.scss']
})
export class UserProfileComponent implements OnInit {
  private route = inject(ActivatedRoute);
  private userService = inject(UserService);
  private authService = inject(AuthService);

  isLoading = true;
  user: User | null = null;
  errorMessage = '';
  currentUserId = '';

  ngOnInit() {
    this.currentUserId = this.authService.currentAuth?.id || '';
    this.route.params.subscribe(params => {
      const userId = params['id'];
      if (userId) {
        this.loadUser(userId);
      }
    });
  }

  loadUser(userId: string) {
    this.isLoading = true;
    this.userService.getUserById(userId).subscribe({
      next: (res) => {
        this.user = res?.result ?? res;
        this.isLoading = false;
      },
      error: () => {
        this.errorMessage = 'Không tìm thấy người dùng';
        this.isLoading = false;
      }
    });
  }

  toggleFollow() {
    if (!this.user) return;

    if (this.user.isFollowing) {
      this.userService.unfollow(this.user.oid).subscribe({
        next: () => {
          if (this.user) {
            this.user.isFollowing = false;
            this.user.followerCount = (this.user.followerCount || 0) - 1;
          }
        }
      });
    } else {
      this.userService.follow({ followingId: this.user.oid }).subscribe({
        next: () => {
          if (this.user) {
            this.user.isFollowing = true;
            this.user.followerCount = (this.user.followerCount || 0) + 1;
          }
        }
      });
    }
  }

  get isOwnProfile(): boolean {
    return this.user?.oid === this.currentUserId;
  }

  get displayName(): string {
    if (!this.user) return '';
    return `${this.user.first_name ?? ''} ${this.user.last_name ?? ''}`.trim() || 'Người dùng';
  }

  get initials(): string {
    const f = this.user?.first_name?.[0] ?? '';
    const l = this.user?.last_name?.[0] ?? '';
    return (f + l).toUpperCase() || '?';
  }
}

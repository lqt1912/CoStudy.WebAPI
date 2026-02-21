import { Routes } from '@angular/router';
import { authGuard, adminGuard, guestGuard } from './core/guards/auth.guard';

export const routes: Routes = [
  { path: '', redirectTo: 'feed', pathMatch: 'full' },

  // ── Auth (guest only) ───────────────────────────────────────────────────────
  {
    path: 'login',
    canActivate: [guestGuard],
    loadComponent: () => import('./features/auth/login/login.component').then(m => m.LoginComponent)
  },
  {
    path: 'register',
    canActivate: [guestGuard],
    loadComponent: () => import('./features/auth/register/register.component').then(m => m.RegisterComponent)
  },
  {
    path: 'forgot-password',
    canActivate: [guestGuard],
    loadComponent: () => import('./features/auth/forgot-password/forgot-password.component').then(m => m.ForgotPasswordComponent)
  },
  {
    path: 'reset-password',
    loadComponent: () => import('./features/auth/reset-password/reset-password.component').then(m => m.ResetPasswordComponent)
  },
  {
    path: 'verify-email',
    loadComponent: () => import('./features/auth/verify-email/verify-email.component').then(m => m.VerifyEmailComponent)
  },

  // ── Main app (requires login) ──────────────────────────────────────────────
  {
    path: '',
    canActivate: [authGuard],
    loadComponent: () => import('./shared/layout/layout.component').then(m => m.LayoutComponent),
    children: [
      // Feed
      {
        path: 'feed',
        loadComponent: () => import('./features/feed/news-feed/news-feed.component').then(m => m.NewsFeedComponent)
      },
      {
        path: 'timeline',
        loadComponent: () => import('./features/feed/timeline/timeline.component').then(m => m.TimelineComponent)
      },
      {
        path: 'post/:id',
        loadComponent: () => import('./features/feed/post-detail/post-detail.component').then(m => m.PostDetailComponent)
      },
      {
        path: 'saved-posts',
        loadComponent: () => import('./features/feed/saved-posts/saved-posts.component').then(m => m.SavedPostsComponent)
      },

      // Profile
      {
        path: 'profile',
        loadComponent: () => import('./features/profile/my-profile/my-profile.component').then(m => m.MyProfileComponent)
      },
      {
        path: 'profile/:id',
        loadComponent: () => import('./features/profile/user-profile/user-profile.component').then(m => m.UserProfileComponent)
      },

      // Search
      {
        path: 'search',
        loadComponent: () => import('./features/search/search.component').then(m => m.SearchComponent)
      },
      {
        path: 'nearby',
        loadComponent: () => import('./features/search/nearby/nearby.component').then(m => m.NearbyComponent)
      },

      // Messaging
      {
        path: 'messages',
        loadComponent: () => import('./features/messaging/conversation-list/conversation-list.component').then(m => m.ConversationListComponent)
      },
      {
        path: 'messages/:id',
        loadComponent: () => import('./features/messaging/chat-window/chat-window.component').then(m => m.ChatWindowComponent)
      },

      // Notifications
      {
        path: 'notifications',
        loadComponent: () => import('./features/notifications/notifications.component').then(m => m.NotificationsComponent)
      },

      // Leaderboard
      {
        path: 'leaderboard',
        loadComponent: () => import('./features/leaderboard/leaderboard.component').then(m => m.LeaderboardComponent)
      },

      // Admin
      {
        path: 'admin',
        canActivate: [adminGuard],
        loadComponent: () => import('./features/admin/admin-layout/admin-layout.component').then(m => m.AdminLayoutComponent),
        children: [
          { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
          {
            path: 'dashboard',
            loadComponent: () => import('./features/admin/dashboard/dashboard.component').then(m => m.DashboardComponent)
          },
          {
            path: 'users',
            loadComponent: () => import('./features/admin/users/admin-users.component').then(m => m.AdminUsersComponent)
          },
          {
            path: 'reports',
            loadComponent: () => import('./features/admin/reports/admin-reports.component').then(m => m.AdminReportsComponent)
          },
          {
            path: 'master-data',
            loadComponent: () => import('./features/admin/master-data/master-data.component').then(m => m.MasterDataComponent)
          }
        ]
      }
    ]
  },

  // Fallback
  { path: '**', redirectTo: 'feed' }
];

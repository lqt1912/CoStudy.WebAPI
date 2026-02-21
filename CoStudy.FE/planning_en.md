# CoStudy Frontend – Angular Development Plan

## 1. Project Overview

**CoStudy** is a social learning network that allows users to share posts, interact (upvote/downvote/comment), direct message, follow each other, and find nearby study partners by field of study. The backend is fully built with REST API (ASP.NET Core), Firebase FCM for push notifications, and SignalR/Queue for real-time features.

---

## 2. API Analysis from Controllers

| Controller | Endpoint Prefix | Main Features |
|---|---|---|
| `AccountsController` | `/api/accounts` | Login (JWT + Google OAuth), refresh token, forgot password, reset password, CRUD accounts (Admin) |
| `UserController` | `/api/user` | Registration, profile update, avatar, address, follow/unfollow, filter users, find nearby users, view history |
| `PostController` | `/api/post` | CRUD posts, news feed, timeline, upvote/downvote, save posts, filter, share, find matching users by field |
| `CommentController` | `/api/comment` | CRUD comments, reply comments, upvote/downvote comments & replies |
| `ConversationController` | `/api/conversation` | Create conversations, view list, delete, add members |
| `MessageController` | `/api/message` | Send/receive/delete/edit messages by conversation |
| `NofticationController` | `/api/noftication` | View, mark as read, delete notifications |
| `FieldController` | `/api/field` | CRUD study fields |
| `FieldGroupController` | `/api/fieldgroup` | Field groups |
| `LevelController` | `/api/level` | Level management, experience points, leaderboard |
| `LocationController` | `/api/location` | Provinces/Districts/Wards (Vietnam) |
| `FileController` | `/api/file` | File upload (Firebase Storage) |
| `ReportController` | `/api/report` | Report posts/comments/replies (user & admin) |
| `ReportReasonController` | `/api/reportreason` | Report reasons |
| `FcmController` | `/api/fcm` | Register/unregister Firebase FCM token, FCM groups |
| `MasterDataController` | `/api/masterdata` | Admin management: Field, Level, FieldGroup |
| `CmsController` | `/api/cms` | CMS admin |
| `ViolenceWordController` | `/api/violenceword` | Violence keyword management (admin) |

---

## 3. Angular Architecture

### Technology Stack
- **Angular 17+** (Standalone Components, Signals)
- **Angular Router** (lazy loading)
- **RxJS** + Angular `HttpClient`
- **NgRx** or **Angular Signals** for state management
- **Socket.IO / SignalR client** for real-time messaging
- **Firebase SDK** for FCM push notifications
- **Angular Material** + custom CSS or **PrimeNG** for UI components
- **Google Sign-In** SDK for OAuth

### Folder Structure

```
costudy-fe/
├── src/
│   ├── app/
│   │   ├── core/                     # Singleton services, guards, interceptors
│   │   │   ├── auth/
│   │   │   │   ├── auth.service.ts
│   │   │   │   ├── auth.guard.ts
│   │   │   │   └── token.interceptor.ts
│   │   │   ├── services/             # API services (1 service per controller)
│   │   │   │   ├── account.service.ts
│   │   │   │   ├── user.service.ts
│   │   │   │   ├── post.service.ts
│   │   │   │   ├── comment.service.ts
│   │   │   │   ├── conversation.service.ts
│   │   │   │   ├── message.service.ts
│   │   │   │   ├── notification.service.ts
│   │   │   │   ├── field.service.ts
│   │   │   │   ├── level.service.ts
│   │   │   │   ├── location.service.ts
│   │   │   │   ├── file.service.ts
│   │   │   │   ├── report.service.ts
│   │   │   │   └── fcm.service.ts
│   │   │   └── models/               # TypeScript interfaces/types
│   │   │       ├── user.model.ts
│   │   │       ├── post.model.ts
│   │   │       ├── comment.model.ts
│   │   │       ├── message.model.ts
│   │   │       └── ...
│   │   ├── shared/                   # Shared components, pipes, directives
│   │   │   ├── components/
│   │   │   │   ├── navbar/
│   │   │   │   ├── avatar/
│   │   │   │   ├── post-card/
│   │   │   │   ├── comment-box/
│   │   │   │   ├── notification-bell/
│   │   │   │   ├── user-card/
│   │   │   │   └── loader/
│   │   │   ├── pipes/
│   │   │   └── directives/
│   │   ├── features/                 # Feature modules (lazy loaded)
│   │   │   ├── auth/
│   │   │   │   ├── login/
│   │   │   │   ├── register/
│   │   │   │   ├── forgot-password/
│   │   │   │   └── reset-password/
│   │   │   ├── feed/
│   │   │   │   ├── news-feed/
│   │   │   │   └── post-detail/
│   │   │   ├── profile/
│   │   │   │   ├── my-profile/
│   │   │   │   ├── user-profile/
│   │   │   │   ├── followers/
│   │   │   │   └── following/
│   │   │   ├── search/
│   │   │   │   └── search-user/
│   │   │   ├── messaging/
│   │   │   │   ├── conversation-list/
│   │   │   │   └── chat-window/
│   │   │   ├── notifications/
│   │   │   ├── leaderboard/
│   │   │   ├── nearby/
│   │   │   └── admin/
│   │   │       ├── dashboard/
│   │   │       ├── users/
│   │   │       ├── posts/
│   │   │       ├── reports/
│   │   │       ├── master-data/
│   │   │       └── violence-words/
│   │   ├── app.routes.ts
│   │   ├── app.config.ts
│   │   └── app.component.ts
│   ├── environments/
│   │   ├── environment.ts
│   │   └── environment.prod.ts
│   └── styles/
│       ├── _variables.scss
│       ├── _typography.scss
│       └── styles.scss
```

---

## 4. Screens & Features

### 4.1 Authentication
| Page | Route | Description |
|---|---|---|
| Login | `/login` | Email/password + Google Sign-In. Store JWT + refresh token |
| Register | `/register` | Registration form, send verification email |
| Forgot Password | `/forgot-password` | Enter email, receive reset link |
| Reset Password | `/reset-password?token=...` | Reset password form |
| Email Verification | `/verify-email?token=...` | Automatically call verify API |

### 4.2 Feed & Posts
| Page | Route | Description |
|---|---|---|
| News Feed | `/feed` | Posts from followed users, paginated |
| Timeline | `/timeline` | Public posts, trending |
| Post Detail | `/post/:id` | Content + comment section |
| Create Post | Modal/page | Compose text, attach images (file upload), select field |
| Edit Post | `/post/:id/edit` | Edit own post |
| Saved Posts | `/saved-posts` | List of bookmarked posts |
| Find Matching Users | `/post/:id/match` | Users matching the post's field of study |

### 4.3 User Profile
| Page | Route | Description |
|---|---|---|
| My Profile | `/profile` | View/edit profile info, avatar, address, fields |
| Other User Profile | `/profile/:id` | View other user's info, follow button |
| Follower List | `/profile/:id/followers` | |
| Following List | `/profile/:id/following` | |
| My Fields | `/profile/fields` | Select and update study fields, levels |

### 4.4 Messaging
| Page | Route | Description |
|---|---|---|
| Conversation List | `/messages` | Show all conversations |
| Chat Window | `/messages/:conversationId` | Real-time chat (SignalR), send/delete/edit messages |

### 4.5 Search & Discover
| Page | Route | Description |
|---|---|---|
| User Search | `/search` | Filter by name, field |
| Nearby Users | `/nearby` | Geographically nearby users |

### 4.6 Notifications
| Page | Route | Description |
|---|---|---|
| Notification List | `/notifications` | View, mark as read, delete |

### 4.7 Leaderboard
| Page | Route | Description |
|---|---|---|
| Leaderboard | `/leaderboard` | Rankings by points, field |

### 4.8 Admin Panel (role: Admin)
| Page | Route | Description |
|---|---|---|
| Dashboard | `/admin` | Overview |
| User Management | `/admin/users` | List, block/unblock accounts |
| Post Management | `/admin/posts` | View, hide/show violating posts |
| Report Handling | `/admin/reports` | Approve/reject violation reports |
| Master Data Management | `/admin/master-data` | CRUD Field, Level, FieldGroup |
| Violence Keywords | `/admin/violence-words` | Manage word filter |

---

## 5. Auth & Security

- **JWT Bearer Token**: Stored in `localStorage` or Memory. HTTP Interceptor automatically attaches `Authorization: Bearer <token>`
- **Refresh Token**: On receiving 401, automatically call `/api/accounts/refresh-token` and retry the original request
- **Google OAuth**: Use `@abacritt/angularx-social-login` or Firebase Auth for Google sign-in popup, then call `/api/accounts/google-login`
- **Route Guards**: `AuthGuard` protects routes requiring login; `AdminGuard` for `/admin`

---

## 6. Real-time

- **Messaging**: Connect to SignalR hub (or Socket.IO if backend uses it) for real-time messaging
- **Notifications**: Firebase Cloud Messaging (FCM) — register token via `/api/fcm/add` after login

---

## 7. File Upload

- Call `POST /api/file/file-upload` with `multipart/form-data`
- Receive the returned image URL, save to post or avatar

---

## 8. Implementation Roadmap (by Phase)

### Phase 1 – Foundation (1–2 weeks)
- [x] Initialize Angular project (standalone, SCSS, routing)
- [x] Set up environment (API base URL, Firebase config)
- [x] Core module: `AuthService`, `TokenInterceptor`, guards
- [x] Shared: Navbar, Avatar, Loader, PostCard components
- [x] Models/interfaces for all main entities

### Phase 2 – Auth & Profile (1 week)
- [x] Login page (email/password + Google OAuth)
- [x] Register page + email verification
- [x] Forgot/Reset password
- [ ] My Profile + edit profile, avatar, address
- [ ] Public User Profile + Follow/Unfollow

### Phase 3 – Feed & Posts (1–2 weeks)
- [ ] News Feed + Timeline (pagination)
- [ ] Create/Edit posts (image upload, field selection)
- [ ] Post detail (upvote/downvote, shared posts)
- [ ] Comment section (CRUD, reply, upvote/downvote comments)
- [ ] Saved posts

### Phase 4 – Messaging & Notifications (1 week)
- [ ] Conversation list
- [ ] Real-time chat window (SignalR)
- [ ] FCM push notification registration
- [ ] Notification panel (bell icon)

### Phase 5 – Search & Leaderboard (0.5 weeks)
- [ ] User search & filter
- [ ] Nearby users (location-based)
- [ ] Leaderboard by points/field

### Phase 6 – Admin Panel (1 week)
- [ ] Overview dashboard
- [ ] User, post, report management
- [ ] Master data (Field, Level, FieldGroup)
- [ ] Violence keywords

### Phase 7 – Polish & Deploy (0.5 weeks)
- [ ] Mobile responsive
- [ ] Dark mode
- [ ] Global error handling
- [ ] Build & deploy (Nginx/Vercel/Firebase Hosting)

---

## 9. API Service Implementation Priority

1. `AuthService` → `/api/accounts`
2. `UserService` → `/api/user`
3. `PostService` → `/api/post`
4. `CommentService` → `/api/comment`
5. `ConversationService` + `MessageService` → `/api/conversation`, `/api/message`
6. `NotificationService` → `/api/noftication`
7. `FieldService` + `LevelService` → `/api/field`, `/api/level`
8. `FileService` → `/api/file`
9. `LocationService` → `/api/location`
10. `ReportService` → `/api/report`
11. `FcmService` → `/api/fcm`
12. `MasterDataService` → `/api/masterdata` (Admin only)

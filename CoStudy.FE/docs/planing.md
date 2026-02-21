# CoStudy Frontend – Kế hoạch phát triển Angular

## 1. Tổng quan dự án

**CoStudy** là một mạng xã hội học tập cho phép người dùng chia sẻ bài viết, tương tác (upvote/downvote/comment), nhắn tin riêng, theo dõi nhau và tìm kiếm bạn học gần đây theo lĩnh vực. Backend đã hoàn chỉnh với REST API (ASP.NET Core), Firebase FCM cho push notification, và SignalR/Queue cho real-time.

---

## 2. Phân tích API từ các Controllers

| Controller | Endpoint prefix | Chức năng chính |
|---|---|---|
| `AccountsController` | `/api/accounts` | Login (JWT + Google OAuth), refresh token, quên mật khẩu, reset mật khẩu, CRUD tài khoản (Admin) |
| `UserController` | `/api/user` | Đăng ký, cập nhật hồ sơ, avatar, địa chỉ, follow/unfollow, lọc user, tìm user gần đây, lịch sử xem |
| `PostController` | `/api/post` | CRUD bài viết, news feed, timeline, upvote/downvote, lưu bài, lọc, share, tìm user match theo lĩnh vực |
| `CommentController` | `/api/comment` | CRUD comment, reply comment, upvote/downvote comment & reply |
| `ConversationController` | `/api/conversation` | Tạo cuộc trò chuyện, xem danh sách, xóa, thêm thành viên |
| `MessageController` | `/api/message` | Gửi/nhận/xóa/sửa tin nhắn theo conversation |
| `NofticationController` | `/api/noftication` | Xem, đánh dấu đã đọc, xóa thông báo |
| `FieldController` | `/api/field` | CRUD lĩnh vực học tập |
| `FieldGroupController` | `/api/fieldgroup` | Nhóm lĩnh vực |
| `LevelController` | `/api/level` | Quản lý cấp độ, điểm kinh nghiệm, leaderboard |
| `LocationController` | `/api/location` | Tỉnh/Huyện/Xã (Việt Nam) |
| `FileController` | `/api/file` | Upload file (Firebase Storage) |
| `ReportController` | `/api/report` | Báo cáo bài viết/bình luận/reply (người dùng & admin) |
| `ReportReasonController` | `/api/reportreason` | Lý do báo cáo |
| `FcmController` | `/api/fcm` | Đăng ký/hủy Firebase FCM token, nhóm FCM |
| `MasterDataController` | `/api/masterdata` | Quản trị admin: Field, Level, FieldGroup |
| `CmsController` | `/api/cms` | CMS admin |
| `ViolenceWordController` | `/api/violenceword` | Quản lý từ khóa bạo lực (admin) |

---

## 3. Kiến trúc Angular

### Công nghệ sử dụng
- **Angular 17+** (Standalone Components, Signals)
- **Angular Router** (lazy loading)
- **RxJS** + Angular `HttpClient`
- **NgRx** hoặc **Angular Signals** cho state management
- **Socket.IO / SignalR client** cho real-time messaging
- **Firebase SDK** cho FCM push notification
- **Angular Material** + custom CSS hoặc **PrimeNG** cho UI components
- **Google Sign-In** SDK cho OAuth

### Cấu trúc thư mục

```
costudy-fe/
├── src/
│   ├── app/
│   │   ├── core/                     # Singleton services, guards, interceptors
│   │   │   ├── auth/
│   │   │   │   ├── auth.service.ts
│   │   │   │   ├── auth.guard.ts
│   │   │   │   └── token.interceptor.ts
│   │   │   ├── services/             # API services (1 service/controller)
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

## 4. Màn hình & tính năng

### 4.1 Authentication
| Trang | Route | Mô tả |
|---|---|---|
| Login | `/login` | Email/password + Google Sign-In. Lưu JWT + refresh token |
| Đăng ký | `/register` | Form đăng ký, gửi email xác thực |
| Quên mật khẩu | `/forgot-password` | Nhập email, nhận link reset |
| Reset mật khẩu | `/reset-password?token=...` | Form đặt lại mật khẩu |
| Xác thực email | `/verify-email?token=...` | Tự động gọi API verify |

### 4.2 Feed & Bài viết
| Trang | Route | Mô tả |
|---|---|---|
| News Feed | `/feed` | Bài viết từ người mình follow, phân trang |
| Timeline | `/timeline` | Bài viết công khai, trending |
| Chi tiết bài viết | `/post/:id` | Nội dung + comment section |
| Tạo bài viết | Modal/page | Soạn văn bản, đính kèm ảnh (upload file), chọn lĩnh vực |
| Chỉnh sửa bài viết | `/post/:id/edit` | Sửa bài viết của mình |
| Bài viết đã lưu | `/saved-posts` | Danh sách bài đã bookmark |
| Tìm user match | `/post/:id/match` | Người dùng phù hợp với lĩnh vực bài viết |

### 4.3 Hồ sơ người dùng
| Trang | Route | Mô tả |
|---|---|---|
| Hồ sơ của tôi | `/profile` | Xem/chỉnh sửa thông tin, avatar, địa chỉ, lĩnh vực |
| Hồ sơ người khác | `/profile/:id` | Xem thông tin người dùng khác, nút follow |
| Danh sách follower | `/profile/:id/followers` | |
| Danh sách following | `/profile/:id/following` | |
| Lĩnh vực của tôi | `/profile/fields` | Chọn và cập nhật lĩnh vực học tập, cấp độ |

### 4.4 Nhắn tin
| Trang | Route | Mô tả |
|---|---|---|
| Danh sách hội thoại | `/messages` | Hiện toàn bộ conversation |
| Chat window | `/messages/:conversationId` | Realtime chat (SignalR), gửi/xóa/sửa tin nhắn |

### 4.5 Tìm kiếm & Khám phá
| Trang | Route | Mô tả |
|---|---|---|
| Tìm kiếm người dùng | `/search` | Lọc theo tên, lĩnh vực |
| Người dùng nearby | `/nearby` | Người dùng gần đây theo địa lý |

### 4.6 Thông báo
| Trang | Route | Mô tả |
|---|---|---|
| Danh sách thông báo | `/notifications` | Xem, đánh dấu đọc, xóa |

### 4.7 Leaderboard
| Trang | Route | Mô tả |
|---|---|---|
| Bảng xếp hạng | `/leaderboard` | Xếp hạng theo điểm, lĩnh vực |

### 4.8 Admin Panel (role: Admin)
| Trang | Route | Mô tả |
|---|---|---|
| Dashboard | `/admin` | Tổng quan |
| Quản lý người dùng | `/admin/users` | Danh sách, khóa/mở tài khoản |
| Quản lý bài viết | `/admin/posts` | Xem, ẩn/hiện bài viết vi phạm |
| Xử lý báo cáo | `/admin/reports` | Duyệt/từ chối báo cáo vi phạm |
| Quản lý Master Data | `/admin/master-data` | CRUD Field, Level, FieldGroup |
| Từ khóa bạo lực | `/admin/violence-words` | Quản lý bộ lọc từ ngữ |

---

## 5. Auth & Security

- **JWT Bearer Token**: Lưu trong `localStorage` hoặc Memory. HTTP Interceptor tự động đính kèm `Authorization: Bearer <token>`
- **Refresh Token**: Khi nhận 401, tự động gọi `/api/accounts/refresh-token` và retry request gốc
- **Google OAuth**: Dùng `@abacritt/angularx-social-login` hoặc Firebase Auth để popup Google sign-in, sau đó gọi `/api/accounts/google-login`
- **Route Guards**: `AuthGuard` bảo vệ các route cần đăng nhập; `AdminGuard` cho `/admin`

---

## 6. Real-time

- **Nhắn tin**: Kết nối SignalR hub (hoặc Socket.IO nếu backend dùng) cho real-time message
- **Thông báo**: Firebase Cloud Messaging (FCM) — đăng ký token qua `/api/fcm/add` sau khi login

---

## 7. Upload file

- Gọi `POST /api/file/file-upload` với `multipart/form-data`
- Nhận URL ảnh trả về, lưu vào bài viết hoặc avatar

---

## 8. Kế hoạch triển khai (theo phase)

### Phase 1 – Nền tảng (1–2 tuần)
- [x] Khởi tạo dự án Angular (standalone, SCSS, routing)
- [x] Thiết lập environment (API base URL, Firebase config)
- [x] Core module: `AuthService`, `TokenInterceptor`, guards
- [x] Shared: Navbar, Avatar, Loader, PostCard component
- [x] Models/interfaces cho tất cả entity chính

### Phase 2 – Auth & Profile (1 tuần)
- [x] Trang Login (email/password + Google OAuth)
- [x] Trang Register + xác thực email
- [x] Forgot/Reset password
- [ ] My Profile + chỉnh sửa hồ sơ, avatar, địa chỉ
- [ ] Public User Profile + Follow/Unfollow

### Phase 3 – Feed & Bài viết (1–2 tuần)
- [ ] News Feed + Timeline (phân trang)
- [ ] Tạo/Chỉnh sửa bài viết (upload ảnh, chọn lĩnh vực)
- [ ] Chi tiết bài viết (upvote/downvote, shared post)
- [ ] Comment section (CRUD, reply, upvote/downvote comment)
- [ ] Bài viết đã lưu

### Phase 4 – Messaging & Notification (1 tuần)
- [ ] Danh sách conversation
- [ ] Real-time chat window (SignalR)
- [ ] FCM push notification đăng ký
- [ ] Notification panel (bell icon)

### Phase 5 – Tìm kiếm & Leaderboard (0.5 tuần)
- [ ] Tìm kiếm & lọc người dùng
- [ ] Nearby users (dựa vị trí)
- [ ] Leaderboard theo điểm/lĩnh vực

### Phase 6 – Admin Panel (1 tuần)
- [ ] Dashboard tổng quan
- [ ] Quản lý users, posts, báo cáo
- [ ] Master data (Field, Level, FieldGroup)
- [ ] Từ khóa bạo lực

### Phase 7 – Polish & Deploy (0.5 tuần)
- [ ] Responsive mobile
- [ ] Dark mode
- [ ] Error handling toàn cục
- [ ] Build & deploy (Nginx/Vercel/Firebase Hosting)

---

## 9. Thứ tự ưu tiên API services cần viết

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

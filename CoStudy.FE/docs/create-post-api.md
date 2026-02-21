# Create Post API – Research Notes

## Endpoint

```
POST /api/post
Authorization: Bearer <JWT>
Content-Type: application/json
```

---

## Request Body (`AddPostRequest`)

**Source:** `CoStudy.API.Infrastructure.Shared/Models/Request/PostRequest/AddPostRequest.cs`

```json
{
  "title": "string",
  "string_contents": [
    {
      "content_type": 0,    // 0 = Code, 1 = Text
      "content": "string"
    }
  ],
  "image_contents": [
    {
      "discription": "string",   // Note: typo in backend, NOT "description"
      "image_url": "string",
      "image_hash": "string",
      "media_type": 0            // 0 = Image, 1 = Video, 2 = File
    }
  ],
  "fields": [
    {
      "field_id": "string",      // ID of the study field
      "level_id": "string"       // ID of the level within that field
    }
  ],
  "post_type": 0                 // 0 = Question, 1 = Sharing
}
```

> **Important:** All JSON property names use **snake_case** (e.g. `string_contents`, `image_contents`, `post_type`, `field_id`, `level_id`).

---

## Sub-Entity Details

### `PostContent`
**Source:** `CoStudy.API.Domain/Entities/Application/PostContent.cs`

| JSON Property  | Type              | Description                    |
|----------------|-------------------|--------------------------------|
| `content_type` | `PostContentType` | Enum: `0` = Code, `1` = Text  |
| `content`      | `string`          | The actual text/code content   |

### `Image` (used for media attachments)
**Source:** `CoStudy.API.Domain/Entities/Application/Image.cs`

| JSON Property  | Type         | Description                              |
|----------------|--------------|------------------------------------------|
| `discription`  | `string`     | Caption/description (**note the typo**)  |
| `image_url`    | `string`     | URL of the uploaded image/media          |
| `image_hash`   | `string`     | Hash of the image (Firebase storage key) |
| `media_type`   | `MediaType?` | Enum: `0` = Image, `1` = Video, `2` = File |

> **Note:** When creating a post via `PostAdapter.FromRequest()`, only `image_hash` and `discription` are copied from the request to the saved entity. The `image_url` is NOT copied. This means images must be uploaded separately first via `POST /api/file/file-upload`, and then the returned URL should be passed as `image_hash` (not `image_url`).

### `ObjectLevel` (study field tagging)
**Source:** `CoStudy.API.Domain/Entities/Application/ObjectLevel.cs`

| JSON Property  | Type       | Description                              |
|----------------|------------|------------------------------------------|
| `object_id`    | `string`   | Set automatically to the post's OId      |
| `field_id`     | `string`   | ID of the study field                    |
| `level_id`     | `string`   | ID of the proficiency level              |
| `point`        | `int?`     | Points (default: 0)                      |
| `is_active`    | `bool`     | Active flag (default: true)              |

> When creating a post, only `field_id` and `level_id` need to be provided in the request. The `object_id` is automatically set to the new post's OId by the service.

### `PostType` Enum
**Source:** `Common/Constants.cs`

| Value | Name       | Description          |
|-------|------------|----------------------|
| `0`   | `Question` | Asking a question    |
| `1`   | `Sharing`  | Sharing knowledge    |

---

## Service Logic Flow

**Source:** `CoStudy.API.Infrastructure.Shared/Services/PostServices/PostService.cs` → `AddPost()`

```
1. Get current authenticated user
2. Convert AddPostRequest → Post entity via PostAdapter.FromRequest()
   - Sets: Title, StringContents, MediaContents, PostType
   - Auto-sets: AuthorId, CreatedDate, ModifiedDate, Status = Active
3. Save Post to MongoDB
4. Create FCM notification group for the post (GroupName = post.OId)
5. Set object_id on each ObjectLevel field → post.OId
6. Save ObjectLevel entries (study field tagging)
7. Find users whose study fields match the post's fields
8. For each matching user who follows the author:
   → Send FCM push notification ("new related post" notification)
9. Map Post → PostViewModel and return
```

---

## Response (`PostViewModel`)

**Source:** `CoStudy.API.Infrastructure.Shared/ViewModels/PostViewModel.cs`

Wrapped in `ApiOkResponse`: `{ "result": <PostViewModel> }`

```json
{
  "oid": "string",
  "title": "string",
  "author_id": "string",
  "author_name": "string",
  "author_avatar": "string",
  "author_email": "string",
  "upvote": 0,
  "downvote": 0,
  "created_date": "2024-01-01T00:00:00",
  "string_contents": [ /* PostContent[] */ ],
  "image_contents": [ /* Image[] */ ],
  "media_type": 0,
  "comments_count": 0,
  "post_type": 0,
  "post_type_name": "Question",
  "field": [ /* ObjectLevel[] */ ],
  "is_vote_by_current": false,
  "is_downvote_by_current": false,
  "is_save_by_current": false,
  "is_notify_by_current": false
}
```

> **Note:** `author_name`, `author_avatar`, `author_email`, `upvote`, `downvote`, `comments_count`, `is_vote_by_current`, `is_downvote_by_current`, `is_save_by_current`, `is_notify_by_current`, and `field` are populated by `PostConvertAction` (AutoMapper AfterMap), not directly from the Post entity.

---

## Frontend Implementation Considerations

### Workflow for Creating a Post with Images
1. Upload images first: `POST /api/file/file-upload` (multipart/form-data)
2. Collect returned URLs → use as `image_hash` in the `image_contents` array
3. Submit post: `POST /api/post` with the full request body

### TypeScript Interfaces Needed

```typescript
interface PostContent {
  content_type: PostContentType;
  content: string;
}

enum PostContentType {
  Code = 0,
  Text = 1
}

interface ImageContent {
  discription?: string;    // Note: typo matches backend
  image_url?: string;
  image_hash?: string;
  media_type?: MediaType;
}

enum MediaType {
  Image = 0,
  Video = 1,
  File = 2
}

interface ObjectLevelField {
  field_id: string;
  level_id: string;
}

enum PostType {
  Question = 0,
  Sharing = 1
}

interface AddPostRequest {
  title: string;
  string_contents: PostContent[];
  image_contents: ImageContent[];
  fields: ObjectLevelField[];
  post_type: PostType;
}
```

### Key Gotchas
1. **snake_case JSON names** — All request/response fields use snake_case, not camelCase
2. **`discription` typo** — The backend uses `discription` (missing "e"), the frontend must match
3. **Image upload flow** — Images must be uploaded separately first; `PostAdapter` only copies `image_hash` and `discription`, NOT `image_url`
4. **`comments_count` inconsistency** — `JsonProperty` says `comment_count` but `JsonPropertyName` says `comments_count`. System.Text.Json (used by ASP.NET Core) will serialize as `comments_count`
5. **Fields auto-set** — `object_id` in fields is auto-set to the post's OId; only send `field_id` and `level_id`

export interface Post {
  id: string;
  objectId: string;
  oid?: string;
  title?: string;
  content: string;
  images?: string[];
  authorId: string;
  post_type_name?: string;
  postType?: number;
  author_name?: string;
  author_avatar?: string;
  author_email?: string;
  author?: any;
  upvoteCount?: number;
  downvoteCount?: number;
  commentCount?: number;
  shareCount?: number;
  isUpvoted?: boolean;
  isDownvoted?: boolean;
  isSaved?: boolean;
  fieldIds?: string[];
  status?: string;
  createdAt?: string;
  updatedAt?: string;
  image_contents?: PostImage[];
}


// POST type: 0 = Question, 1 = Sharing
export type PostType = 0 | 1;

// Content type: 0 = Text, 1 = Code
export type PostContentType = 0 | 1;

export interface PostImage {
  /** URL returned by the file-upload API (Google Cloud) */
  image_url: string;
  /** Same as image_url — backend requires both fields */
  image_hash: string;
  discription?: string;
}

export interface PostContentItem {
  contentType: PostContentType;
  content: string;
}

export interface ObjectLevelField {
  fieldId: string;
  levelId: string;
}

export interface AddPostRequest {
  title: string;
  stringContents: PostContentItem[];
  imageContents?: PostImage[];
  fields?: ObjectLevelField[];
  postType: PostType;
}

export interface UpdatePostRequest {
  id: string;
  title?: string;
  content?: string;
  images?: string[];
  fieldIds?: string[];
}

export interface GetPostByUserRequest {
  userId?: string;
  skip?: number;
  count?: number;
}

export interface NewsFeedRequest {
  skip?: number;
  count?: number;
}

export interface GetSavedPostRequest {
  skip?: number;
  count?: number;
}

export interface FilterRequest {
  keyword?: string;
  fieldIds?: string[];
  skip?: number;
  count?: number;
}

export interface SharePostRequest {
  postId: string;
  content?: string;
}

export interface ModifedPostStatusRequest {
  postId: string;
  status: string;
}

export interface UpdatePostLevelRequest {
  postId: string;
  fieldId: string;
}

export interface BaseGetAllRequest {
  skip?: number;
  count?: number;
}

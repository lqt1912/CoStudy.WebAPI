export interface Post {
  id: string;
  objectId: string;
  title?: string;
  content: string;
  images?: string[];
  authorId: string;
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
}

export interface AddPostRequest {
  title?: string;
  content: string;
  images?: string[];
  fieldIds?: string[];
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

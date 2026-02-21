export interface Comment {
  id: string;
  objectId: string;
  postId: string;
  authorId: string;
  author?: any;
  content: string;
  upvoteCount?: number;
  downvoteCount?: number;
  replyCount?: number;
  status?: string;
  createdAt?: string;
}

export interface Reply {
  id: string;
  objectId: string;
  commentId: string;
  authorId: string;
  author?: any;
  content: string;
  upvoteCount?: number;
  downvoteCount?: number;
  status?: string;
  createdAt?: string;
}

export interface AddCommentRequest {
  postId: string;
  content: string;
}

export interface ReplyCommentRequest {
  commentId: string;
  content: string;
}

export interface UpdateCommentRequest {
  id: string;
  content: string;
}

export interface UpdateReplyRequest {
  id: string;
  content: string;
}

export interface CommentFilterRequest {
  postId: string;
  skip?: number;
  count?: number;
}

export interface ModifiedCommentStatusRequest {
  id: string;
  status: string;
}

export interface Notification {
  id: string;
  objectId: string;
  receiverId: string;
  senderId?: string;
  sender?: any;
  content: string;
  type?: string;
  isRead: boolean;
  objectIdentifier?: string;
  createdAt?: string;
}

export interface AddNofticationRequest {
  receiverId: string;
  content: string;
  type?: string;
  objectIdentifier?: string;
}

export interface Field {
  id: string;
  objectId: string;
  name: string;
  groupId?: string;
  description?: string;
}

export interface FieldGroup {
  id: string;
  objectId: string;
  name: string;
  fields?: Field[];
}

export interface Level {
  id: string;
  objectId: string;
  name: string;
  minPoint: number;
  maxPoint: number;
  badge?: string;
}

export interface ObjectLevel {
  objectId: string;
  levelId: string;
  point: number;
  fieldId: string;
}

export interface UserAddFieldRequest {
  userId: string;
  fieldIds: string[];
}

export interface AddPointRequest {
  objectId: string;
  fieldId: string;
  point: number;
}

export interface LeaderBoardRequest {
  fieldId?: string;
  skip?: number;
  count?: number;
}

export interface Province {
  code: string;
  name: string;
}

export interface District {
  code: string;
  name: string;
  provinceCode: string;
}

export interface Ward {
  code: string;
  name: string;
  districtCode: string;
}

export interface Report {
  id: string;
  objectId: string;
  reporterId: string;
  targetId: string;
  targetType: string;
  reason?: string;
  status?: string;
  createdAt?: string;
}

export interface CreatePostReportRequest {
  postId: string;
  reason?: string;
  reportReasonId?: string;
}

export interface CreateCommentReportRequest {
  commentId: string;
  reason?: string;
  reportReasonId?: string;
}

export interface CreateReplyReportRequest {
  replyId: string;
  reason?: string;
  reportReasonId?: string;
}

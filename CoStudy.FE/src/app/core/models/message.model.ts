export interface Conversation {
  id: string;
  objectId: string;
  name?: string;
  members?: string[];
  lastMessage?: Message;
  createdAt?: string;
}

export interface Message {
  id: string;
  objectId: string;
  conversationId: string;
  senderId: string;
  sender?: any;
  content: string;
  type?: string;
  status?: string;
  createdAt?: string;
}

export interface AddConversationRequest {
  memberIds: string[];
  name?: string;
}

export interface AddMemberRequest {
  conversationId: string;
  memberIds: string[];
}

export interface AddMessageRequest {
  conversationId: string;
  content: string;
  type?: string;
}

export interface UpdateMessageRequest {
  id: string;
  content: string;
}

export interface GetMessageByConversationIdRequest {
  conversationId: string;
  skip?: number;
  count?: number;
}

export interface User {
  oid: string;
  id?: string;          // alias for oid
  email: string;
  first_name: string;
  last_name: string;
  avatar_hash?: string;
  avatar?: UserAvatar;
  bio?: string;
  phone_number?: string;
  date_of_birth?: string;
  gender?: string;
  address?: Address;
  additional_infos?: AdditionalInfomation[];
  followerCount?: number;
  followingCount?: number;
  isFollowing?: boolean;
  fields?: string[];
  notification_off?: string[];
  call_id?: string;
  created_date?: string;
  modified_date?: string;
  status?: number;
}

export interface UserAvatar {
  description?: string;
  image_url?: string;
  image_hash?: string;
  media_type?: string;
}

export interface Address {
  district?: string;
  city?: string;
  detail?: string;
  longtitude?: string;
  latitude?: string;
}

export interface AdditionalInfomation {
  key: string;
  value: string;
}

export interface AddUserRequest {
  email: string;
  password: string;
  confirmPassword: string;
  acceptTerms: boolean;
  firstName?: string;
  lastName?: string;
  isExternalRegister?: boolean;
}

export interface UpdateUserRequest {
  firstName?: string;
  lastName?: string;
  bio?: string;
  phone?: string;
  dateOfBirth?: string;
  gender?: string;
}

export interface AddAvatarRequest {
  avatarHash: string;
  description?: string;
}

export interface AddFollowerRequest {
  followingId: string;
}

export interface FilterUserRequest {
  keyword?: string;
  fieldId?: string;
  skip?: number;
  count?: number;
}

export interface FollowFilterRequest {
  userId: string;
  skip?: number;
  count?: number;
}

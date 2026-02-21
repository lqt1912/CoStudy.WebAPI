export interface AuthenticateRequest {
  email: string;
  password: string;
}

export interface AuthenticateResponse {
  id: string;
  email: string;
  role: string;
  jwtToken: string;
  refreshToken: string;
}

export interface GoogleAuthenticationRequest {
  idToken: string;
}

export interface ForgotPasswordRequest {
  email: string;
}

export interface ResetPasswordRequest {
  token: string;
  password: string;
  confirmPassword: string;
}

export interface ValidateResetTokenRequest {
  token: string;
}

export interface RegisterRequest {
  email: string;
  password: string;
  confirmPassword: string;
  acceptTerms: boolean;
}

export interface ApiResponse<T = any> {
  succeeded: boolean;
  message?: string;
  errors?: string[];
  data?: T;
  statusCode?: number;
}

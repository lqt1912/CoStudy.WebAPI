import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { SocialAuthService, GoogleSigninButtonModule, SocialUser } from '@abacritt/angularx-social-login';
import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    RouterLink,
    GoogleSigninButtonModule
  ],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent {
  private fb = inject(FormBuilder);
  private authService = inject(AuthService);
  private socialAuthService = inject(SocialAuthService);
  private router = inject(Router);

  isLoading = false;
  hidePassword = true;
  errorMessage = '';

  loginForm = this.fb.group({
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required]]
  });

  constructor() {
    // this.socialAuthService.authState.subscribe((user: SocialUser) => {
    //   if (user?.idToken) {
    //     this.handleGoogleLogin(user.idToken);
    //   }
    // });
  }

  onSubmit() {
    if (this.loginForm.invalid) return;

    this.isLoading = true;
    this.errorMessage = '';
    const { email, password } = this.loginForm.value;

    if (!email || !password) return;

    this.authService.login({ email, password }).subscribe({
      next: () => {
        this.router.navigate(['/']);
      },
      error: (err) => {
        this.isLoading = false;
        this.errorMessage = err.error?.message || 'Login failed. Please try again.';
      }
    });
  }

  handleGoogleLogin(idToken: string) {
    this.isLoading = true;
    this.errorMessage = '';
    
    this.authService.googleLogin({ idToken }).subscribe({
      next: () => {
        this.router.navigate(['/']);
      },
      error: (err) => {
        this.isLoading = false;
        this.errorMessage = err.error?.message || 'Google Login failed.';
      }
    });
  }
}

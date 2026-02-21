import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-forgot-password',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    RouterLink
  ],
  templateUrl: './forgot-password.component.html',
  styleUrls: ['./forgot-password.component.scss']
})
export class ForgotPasswordComponent {
  private fb = inject(FormBuilder);
  private authService = inject(AuthService);

  isLoading = false;
  successMessage = '';
  errorMessage = '';

  forgotPasswordForm = this.fb.group({
    email: ['', [Validators.required, Validators.email]]
  });

  onSubmit() {
    if (this.forgotPasswordForm.invalid) return;

    this.isLoading = true;
    this.errorMessage = '';
    this.successMessage = '';
    const { email } = this.forgotPasswordForm.value;

    this.authService.forgotPassword({ email: email! }).subscribe({
      next: () => {
        this.isLoading = false;
        this.successMessage = 'If an account exists with this email, you will receive a password reset link.';
        this.forgotPasswordForm.reset();
      },
      error: (err) => {
        this.isLoading = false;
        // For security reasons, often better not to reveal if email doesn't exist, 
        // but for now we follow standard error handling or show generic message
        this.errorMessage = err.error?.message || 'Something went wrong. Please try again.';
      }
    });
  }
}

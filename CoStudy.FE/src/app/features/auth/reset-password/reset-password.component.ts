import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators, AbstractControl, ValidationErrors } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-reset-password',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    RouterLink
  ],
  templateUrl: './reset-password.component.html',
  styleUrls: ['./reset-password.component.scss']
})
export class ResetPasswordComponent implements OnInit {
  private fb = inject(FormBuilder);
  private authService = inject(AuthService);
  private route = inject(ActivatedRoute);
  private router = inject(Router);

  isLoading = false;
  hidePassword = true;
  hideConfirmPassword = true;
  successMessage = '';
  errorMessage = '';
  token = '';

  resetPasswordForm = this.fb.group({
    password: ['', [Validators.required, Validators.minLength(6)]],
    confirmPassword: ['', [Validators.required]]
  }, { validators: this.passwordMatchValidator });

  ngOnInit() {
    this.token = this.route.snapshot.queryParams['token'];
    if (!this.token) {
      this.errorMessage = 'Invalid or missing reset token.';
    }
  }

  onSubmit() {
    if (this.resetPasswordForm.invalid || !this.token) return;

    this.isLoading = true;
    this.errorMessage = '';
    this.successMessage = '';
    
    const { password, confirmPassword } = this.resetPasswordForm.value;

    this.authService.resetPassword({ 
      token: this.token, 
      password: password!, 
      confirmPassword: confirmPassword! 
    }).subscribe({
      next: () => {
        this.isLoading = false;
        this.successMessage = 'Password has been reset successfully.';
        setTimeout(() => this.router.navigate(['/login']), 3000);
      },
      error: (err) => {
        this.isLoading = false;
        this.errorMessage = err.error?.message || 'Failed to reset password. Token may be expired.';
      }
    });
  }

  passwordMatchValidator(control: AbstractControl): ValidationErrors | null {
    const password = control.get('password');
    const confirmPassword = control.get('confirmPassword');

    if (password && confirmPassword && password.value !== confirmPassword.value) {
      confirmPassword.setErrors({ mismatch: true });
      return { mismatch: true };
    }
    return null;
  }
}

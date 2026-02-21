import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-verify-email',
  standalone: true,
  imports: [
    CommonModule,
    RouterLink
  ],
  templateUrl: './verify-email.component.html',
  styleUrls: ['./verify-email.component.scss']
})
export class VerifyEmailComponent implements OnInit {
  private route = inject(ActivatedRoute);
  private authService = inject(AuthService);

  isLoading = true;
  isSuccess = false;
  message = 'Verifying your email...';

  ngOnInit() {
    const token = this.route.snapshot.queryParams['token'];
    
    if (token) {
      this.authService.verifyEmail(token).subscribe({
        next: () => {
          this.isLoading = false;
          this.isSuccess = true;
          this.message = 'Email verified successfully! You can now login.';
        },
        error: (err) => {
          this.isLoading = false;
          this.isSuccess = false;
          this.message = err.error?.message || 'Verification failed. The link may be invalid or expired.';
        }
      });
    } else {
      this.isLoading = false;
      this.isSuccess = false;
      this.message = 'Invalid verification link. Token is missing.';
    }
  }
}

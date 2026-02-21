import { Component, OnInit, signal } from '@angular/core';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../core/services/auth.service';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [CommonModule, RouterLink, RouterLinkActive],
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss']
})
export class NavbarComponent implements OnInit {
  menuOpen = signal(false);
  unreadCount = signal(0);
  currentInitial = signal('U');
  isAdmin = signal(false);

  constructor(
    private authService: AuthService,
    private router: Router
  ) {}

  ngOnInit(): void {
    const auth = this.authService.currentAuth;
    if (auth) {
      this.currentInitial.set((auth.email?.[0] ?? 'U').toUpperCase());
      this.isAdmin.set(auth.role === 'Admin');
    }
  }

  toggleMenu(): void {
    this.menuOpen.update(v => !v);
  }

  logout(): void {
    this.authService.logout();
    this.router.navigate(['/login']);
    this.menuOpen.set(false);
  }
}

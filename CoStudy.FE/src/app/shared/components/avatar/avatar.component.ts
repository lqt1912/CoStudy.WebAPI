import { Component, input } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-avatar',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './avatar.component.html',
  styleUrls: ['./avatar.component.scss']
})
export class AvatarComponent {
  src = input<string>('');
  alt = input<string>('Avatar');
  size = input<'sm' | 'md' | 'lg' | 'xl'>('md');
  initial = input<string>('?');
  showOnline = input<boolean>(false);

  onImageError(event: Event): void {
    (event.target as HTMLImageElement).style.display = 'none';
  }
}

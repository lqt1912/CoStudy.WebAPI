import { Component, inject, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { UserService } from '../../../core/services/user.service';
import { User, UpdateUserRequest } from '../../../core/models/user.model';
import { FileService } from '../../../core/services/file.service';
import { AvatarComponent } from '../../../shared/components/avatar/avatar.component';

@Component({
  selector: 'app-my-profile',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    AvatarComponent
  ],
  templateUrl: './my-profile.component.html',
  styleUrls: ['./my-profile.component.scss']
})
export class MyProfileComponent implements OnInit {
  private fb = inject(FormBuilder);
  private userService = inject(UserService);
  private fileService = inject(FileService);
  private cdr = inject(ChangeDetectorRef);

  isLoading = true;
  isSaving = false;
  isEditMode = false;
  currentUser: User | null = null;
  errorMessage = '';
  successMessage = '';

  profileForm = this.fb.group({
    firstName: ['', Validators.required],
    lastName: ['', Validators.required],
    bio: [''],
    phone: [''],
    gender: [''],
    dateOfBirth: ['']
  });

  addressForm = this.fb.group({
    city: [''],
    district: [''],
    detail: ['']
  });

  ngOnInit() {
    this.loadProfile();
  }

  loadProfile() {
    this.isLoading = true;
    this.userService.getCurrentUser().subscribe({
      next: (res) => {
        this.currentUser = res?.result ?? res;
        this.patchFormValues();
        this.isLoading = false;
        this.cdr.markForCheck();
      },
      error: () => {
        this.errorMessage = 'Không thể tải thông tin hồ sơ';
        this.isLoading = false;
        this.cdr.markForCheck();
      }
    });
  }

  patchFormValues() {
    if (!this.currentUser) return;

    this.profileForm.patchValue({
      firstName: this.currentUser.first_name ?? '',
      lastName: this.currentUser.last_name ?? '',
      bio: this.currentUser.bio ?? '',
      phone: this.currentUser.phone_number ?? '',
      gender: this.currentUser.gender ?? '',
      dateOfBirth: this.currentUser.date_of_birth ?? ''
    });

    if (this.currentUser.address) {
      this.addressForm.patchValue({
        city: this.currentUser.address.city ?? '',
        district: this.currentUser.address.district ?? '',
        detail: this.currentUser.address.detail ?? ''
      });
    }
  }

  toggleEditMode() {
    this.isEditMode = !this.isEditMode;
    if (!this.isEditMode) {
      this.patchFormValues();
      this.errorMessage = '';
      this.successMessage = '';
    }
  }

  onSave() {
    if (this.profileForm.invalid) return;

    this.isSaving = true;
    this.errorMessage = '';
    this.successMessage = '';

    const updateRequest: UpdateUserRequest = {
      firstName: this.profileForm.value.firstName ?? undefined,
      lastName: this.profileForm.value.lastName ?? undefined,
      bio: this.profileForm.value.bio ?? undefined,
      phone: this.profileForm.value.phone ?? undefined,
      gender: this.profileForm.value.gender ?? undefined,
      dateOfBirth: this.profileForm.value.dateOfBirth ?? undefined
    };

    this.userService.updateUser(updateRequest).subscribe({
      next: () => {
        this.userService.updateAddress(this.addressForm.value).subscribe({
          next: () => {
            this.isSaving = false;
            this.isEditMode = false;
            this.successMessage = 'Cập nhật hồ sơ thành công!';
            this.loadProfile();
            this.cdr.markForCheck();
          },
          error: () => {
            this.isSaving = false;
            this.errorMessage = 'Không thể cập nhật địa chỉ';
            this.cdr.markForCheck();
          }
        });
      },
      error: (err) => {
        this.isSaving = false;
        this.errorMessage = err.error?.message || 'Không thể cập nhật hồ sơ';
        this.cdr.markForCheck();
      }
    });
  }

  // Avatar upload state
  isUploadingAvatar = false;
  avatarPreview: string | null = null;

  onFileSelected(event: Event) {
    const input = event.target as HTMLInputElement;
    const file = input?.files?.[0];
    if (!file) return;

    // Show local preview immediately
    const reader = new FileReader();
    reader.onload = () => this.avatarPreview = reader.result as string;
    reader.readAsDataURL(file);

    this.isUploadingAvatar = true;
    this.errorMessage = '';

    this.fileService.uploadFiles('avatars', [file]).subscribe({
      next: (res: any) => {
        const imageUrl = res?.result?.[0];
        if (imageUrl) {
          this.userService.uploadAvatar({ avatarHash: imageUrl }).subscribe({
            next: () => {
              this.isUploadingAvatar = false;
              this.avatarPreview = null;
              this.successMessage = 'Cập nhật ảnh đại diện thành công!';
              this.loadProfile();
              this.cdr.markForCheck();
            },
            error: () => {
              this.isUploadingAvatar = false;
              this.avatarPreview = null;
              this.errorMessage = 'Không thể cập nhật ảnh đại diện';
              this.cdr.markForCheck();
            }
          });
        } else {
          this.isUploadingAvatar = false;
          this.avatarPreview = null;
          this.errorMessage = 'Phản hồi không hợp lệ từ dịch vụ tải lên';
          this.cdr.markForCheck();
        }
      },
      error: () => {
        this.isUploadingAvatar = false;
        this.avatarPreview = null;
        this.errorMessage = 'Không thể tải ảnh lên';
        this.cdr.markForCheck();
      }
    });

    // Reset input so re-selecting the same file triggers change
    input.value = '';
  }

  get displayName(): string {
    if (!this.currentUser) return '';
    return `${this.currentUser.first_name ?? ''} ${this.currentUser.last_name ?? ''}`.trim() || 'Người dùng';
  }

  get initials(): string {
    const f = this.currentUser?.first_name?.[0] ?? '';
    const l = this.currentUser?.last_name?.[0] ?? '';
    return (f + l).toUpperCase() || '?';
  }

  get formattedAddress(): string {
    const a = this.currentUser?.address;
    if (!a) return 'Chưa cập nhật';
    const parts = [a.detail, a.district, a.city].filter(Boolean);
    return parts.length ? parts.join(', ') : 'Chưa cập nhật';
  }
}

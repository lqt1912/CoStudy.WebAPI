import {
  ChangeDetectionStrategy,
  Component,
  OnInit,
  computed,
  inject,
  signal
} from '@angular/core';

const ICON_OPTIONS = [
  '🌱','🌿','🍀','🌾','🌸','🌺','🌻','🌟',
  '⭐','💫','✨','🔥','💥','⚡','🎯','🏹',
  '💎','👑','🏆','🥇','🥈','🥉','🎖️','🏅',
  '🚀','🛸','🌙','☀️','🌈','❄️','🌊','⛰️',
  '📚','📖','🔬','🔭','🧠','💡','🎓','📝',
  '⚔️','🛡️','🗡️','🪄','🔮','🧿','🃏','🎲',
];
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { FieldService } from '../../../core/services/field.service';
import { LevelService } from '../../../core/services/level.service';
import type { Field, FieldGroup } from '../../../core/models/field.model';

export interface Level {
  oid: string;
  name: string;
  description?: string;
  order?: number;
  point?: number;
  icon?: string;
}

const PAGE_SIZE = 10;

@Component({
  selector: 'app-master-data',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './master-data.component.html',
  styleUrls: ['./master-data.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class MasterDataComponent implements OnInit {
  private fieldService = inject(FieldService);
  private levelService = inject(LevelService);

  // ─── Tab ─────────────────────────────────────────────────────────────────
  activeTab = signal<'fields' | 'groups' | 'levels'>('fields');

  // ─── Fields ──────────────────────────────────────────────────────────────
  fields = signal<Field[]>([]);
  fieldPage = signal(0);
  fieldTotal = signal(0);
  fieldLoading = signal(false);
  fieldError = signal('');
  newFieldValue = signal('');
  addingField = signal(false);
  fieldPageCount = computed(() => Math.ceil(this.fieldTotal() / PAGE_SIZE));

  // ─── Field Groups ─────────────────────────────────────────────────────────
  groups = signal<FieldGroup[]>([]);
  groupPage = signal(0);
  groupTotal = signal(0);
  groupLoading = signal(false);
  groupError = signal('');
  expandedGroupId = signal<string | null>(null);
  groupPageCount = computed(() => Math.ceil(this.groupTotal() / PAGE_SIZE));

  showGroupModal = signal(false);
  modalMode = signal<'create' | 'edit'>('create');
  modalGroupId = signal('');
  modalGroupName = signal('');
  modalSelectedFieldIds = signal<string[]>([]);
  modalSaving = signal(false);
  modalError = signal('');

  // ─── Levels ───────────────────────────────────────────────────────────────
  levels = signal<Level[]>([]);
  levelPage = signal(0);
  levelTotal = signal(0);
  levelLoading = signal(false);
  levelError = signal('');
  levelPageCount = computed(() => Math.ceil(this.levelTotal() / PAGE_SIZE));

  showLevelModal = signal(false);
  levelModalName = signal('');
  levelModalDescription = signal('');
  levelModalPoint = signal(0);
  levelModalOrder = signal(0);
  levelModalIcon = signal('⭐');
  levelModalSaving = signal(false);
  levelModalError = signal('');

  // ─── Lifecycle ─────────────────────────────────────────────────────────────
  ngOnInit(): void {
    this.loadFields();
    this.loadGroups();
    this.loadLevels();
  }

  // ─── Fields logic (unchanged) ──────────────────────────────────────────────
  loadFields(): void {
    this.fieldLoading.set(true);
    this.fieldError.set('');
    this.fieldService.getAllFields({ skip: this.fieldPage() * PAGE_SIZE, count: PAGE_SIZE }).subscribe({
      next: res => {
        this.fields.set(res.result ?? []);
        this.fieldTotal.set(res.result?.length === PAGE_SIZE
          ? (this.fieldPage() + 2) * PAGE_SIZE
          : this.fieldPage() * PAGE_SIZE + (res.result?.length ?? 0));
        this.fieldLoading.set(false);
      },
      error: () => { this.fieldError.set('Không thể tải danh sách lĩnh vực.'); this.fieldLoading.set(false); }
    });
  }

  addField(): void {
    const value = this.newFieldValue().trim();
    if (!value) return;
    this.addingField.set(true);
    this.fieldService.addField({ value }).subscribe({
      next: res => { this.fields.update(list => [res.result, ...list]); this.newFieldValue.set(''); this.addingField.set(false); },
      error: () => { this.fieldError.set('Thêm lĩnh vực thất bại.'); this.addingField.set(false); }
    });
  }

  deleteField(id: string): void {
    if (!confirm('Bạn có chắc muốn xóa lĩnh vực này?')) return;
    this.fieldService.deleteField(id).subscribe({
      next: () => this.fields.update(list => list.filter(f => f.oid !== id)),
      error: () => this.fieldError.set('Xóa lĩnh vực thất bại.')
    });
  }

  goFieldPage(page: number): void { this.fieldPage.set(page); this.loadFields(); }
  onFieldKeydown(event: KeyboardEvent): void { if (event.key === 'Enter') this.addField(); }

  // ─── Groups logic (unchanged) ──────────────────────────────────────────────
  loadGroups(): void {
    this.groupLoading.set(true);
    this.fieldService.getAllFieldGroups({ skip: this.groupPage() * PAGE_SIZE, count: PAGE_SIZE }).subscribe({
      next: res => { this.groups.set(res.result ?? []); this.groupLoading.set(false); },
      error: () => { this.groupError.set('Không thể tải danh sách nhóm.'); this.groupLoading.set(false); }
    });
  }

  toggleGroup(id: string): void { this.expandedGroupId.update(cur => cur === id ? null : id); }
  goGroupPage(page: number): void { this.groupPage.set(page); this.loadGroups(); }

  openCreateGroupModal(): void {
    this.modalMode.set('create'); this.modalGroupId.set(''); this.modalGroupName.set('');
    this.modalSelectedFieldIds.set([]); this.modalError.set(''); this.showGroupModal.set(true);
  }

  openEditGroupModal(group: FieldGroup): void {
    this.modalMode.set('edit'); this.modalGroupId.set(group.oid); this.modalGroupName.set(group.group_name);
    this.modalSelectedFieldIds.set(group.fields?.map(f => f.oid) ?? []); this.modalError.set('');
    this.showGroupModal.set(true);
  }

  closeModal(): void { this.showGroupModal.set(false); }
  isFieldSelected(id: string): boolean { return this.modalSelectedFieldIds().includes(id); }
  toggleFieldInModal(id: string): void {
    this.modalSelectedFieldIds.update(ids => ids.includes(id) ? ids.filter(x => x !== id) : [...ids, id]);
  }

  saveModal(): void {
    const name = this.modalGroupName().trim();
    if (!name) { this.modalError.set('Tên nhóm không được để trống.'); return; }
    this.modalSaving.set(true); this.modalError.set('');
    if (this.modalMode() === 'create') {
      this.fieldService.addFieldGroup({ group_name: name, field_id: this.modalSelectedFieldIds() }).subscribe({
        next: res => { this.groups.update(list => [res.result, ...list]); this.modalSaving.set(false); this.showGroupModal.set(false); },
        error: () => { this.modalError.set('Tạo nhóm thất bại.'); this.modalSaving.set(false); }
      });
    } else {
      this.fieldService.addFieldToGroup({ group_id: this.modalGroupId(), group_name: name, field_ids: this.modalSelectedFieldIds() }).subscribe({
        next: res => { this.groups.update(list => list.map(g => g.oid === res.result.oid ? res.result : g)); this.modalSaving.set(false); this.showGroupModal.set(false); },
        error: () => { this.modalError.set('Cập nhật nhóm thất bại.'); this.modalSaving.set(false); }
      });
    }
  }

  // ─── Levels logic ──────────────────────────────────────────────────────────
  loadLevels(): void {
    this.levelLoading.set(true);
    this.levelError.set('');
    this.levelService.getAllLevels({ skip: this.levelPage() * PAGE_SIZE, count: PAGE_SIZE }).subscribe({
      next: res => {
        const list: Level[] = (res.result ?? res.data ?? []);
        this.levels.set(list.sort((a, b) => (a.order ?? 0) - (b.order ?? 0)));
        this.levelTotal.set(list.length === PAGE_SIZE
          ? (this.levelPage() + 2) * PAGE_SIZE
          : this.levelPage() * PAGE_SIZE + list.length);
        this.levelLoading.set(false);
      },
      error: () => { this.levelError.set('Không thể tải danh sách cấp độ.'); this.levelLoading.set(false); }
    });
  }

  readonly iconOptions = ICON_OPTIONS;

  openLevelModal(): void {
    const nextOrder = this.levels().length;
    this.levelModalName.set('');
    this.levelModalDescription.set('');
    this.levelModalPoint.set(nextOrder === 0 ? 0 : nextOrder === 1 ? 100 : nextOrder === 2 ? 300 : nextOrder === 3 ? 600 : nextOrder === 4 ? 1000 : 1500);
    this.levelModalOrder.set(nextOrder);
    this.levelModalIcon.set('⭐');
    this.levelModalError.set('');
    this.showLevelModal.set(true);
  }

  closeLevelModal(): void { this.showLevelModal.set(false); }

  saveLevelModal(): void {
    const name = this.levelModalName().trim();
    if (!name) { this.levelModalError.set('Tên cấp độ không được để trống.'); return; }
    this.levelModalSaving.set(true);
    this.levelModalError.set('');

    const payload = [{
      name,
      description: this.levelModalDescription().trim(),
      point: this.levelModalPoint(),
      order: this.levelModalOrder(),
      icon: this.levelModalIcon().trim() || '⭐'
    }];

    this.levelService.addLevel(payload).subscribe({
      next: res => {
        const added = res.result ?? res.data ?? res;
        const newLevel: Level = Array.isArray(added) ? added[0] : added;
        if (newLevel) this.levels.update(list => [...list, newLevel].sort((a, b) => (a.order ?? 0) - (b.order ?? 0)));
        this.levelModalSaving.set(false);
        this.showLevelModal.set(false);
      },
      error: () => { this.levelModalError.set('Tạo cấp độ thất bại.'); this.levelModalSaving.set(false); }
    });
  }

  goLevelPage(page: number): void { this.levelPage.set(page); this.loadLevels(); }

  // ─── Shared helpers ────────────────────────────────────────────────────────
  get pages(): number[] {
    const count = this.activeTab() === 'fields' ? this.fieldPageCount()
      : this.activeTab() === 'groups' ? this.groupPageCount()
      : this.levelPageCount();
    return Array.from({ length: count }, (_, i) => i);
  }
}

import {
  ChangeDetectionStrategy,
  Component,
  ElementRef,
  OnInit,
  ViewChild,
  inject,
  output,
  signal,
  computed
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { PostService } from '../../../core/services/post.service';
import { FieldService } from '../../../core/services/field.service';
import { LevelService } from '../../../core/services/level.service';
import type { Field } from '../../../core/models/field.model';
import type { AddPostRequest, PostType } from '../../../core/models/post.model';

export interface Level {
  oid: string;
  name: string;
  order?: number;
  point?: number;
  icon?: string;
}

@Component({
  selector: 'app-create-post',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './create-post.component.html',
  styleUrls: ['./create-post.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class CreatePostComponent implements OnInit {
  private postService = inject(PostService);
  private fieldService = inject(FieldService);
  private levelService = inject(LevelService);

  /** Emits the newly created post so the parent can prepend it to the list */
  postCreated = output<any>();

  @ViewChild('editor') editorRef!: ElementRef<HTMLDivElement>;

  // ─── State ───────────────────────────────────────────────────────────────
  title = signal('');
  postType = signal<PostType>(1); // default: Sharing
  fields = signal<Field[]>([]);
  levels = signal<Level[]>([]);

  /** Map of fieldId → levelId chosen by user */
  fieldLevelMap = signal<Record<string, string>>({});

  submitting = signal(false);
  error = signal('');
  expanded = signal(false);

  // ─── Toolbar format state ──────────────────────────────────────────────
  isBold = signal(false);
  isItalic = signal(false);
  isUnderline = signal(false);

  /** Fields the user has toggled on */
  selectedFields = computed(() =>
    this.fields().filter(f => this.fieldLevelMap()[f.oid] !== undefined)
  );

  ngOnInit(): void {
    this.fieldService.getAllFields({ skip: 0, count: 100 }).subscribe({
      next: res => this.fields.set(res.result ?? [])
    });
    this.levelService.getAllLevels({ skip: 0, count: 50 }).subscribe({
      next: res => this.levels.set(
        (res.result ?? res.data ?? []).sort((a: Level, b: Level) => (a.order ?? 0) - (b.order ?? 0))
      )
    });
  }

  // ─── Field / level helpers ────────────────────────────────────────────
  isFieldSelected(fieldId: string): boolean {
    return this.fieldLevelMap()[fieldId] !== undefined;
  }

  toggleField(fieldId: string): void {
    this.fieldLevelMap.update(map => {
      const next = { ...map };
      if (next[fieldId] !== undefined) {
        delete next[fieldId];
      } else {
        // default to first (easiest) level
        next[fieldId] = this.levels()[0]?.oid ?? '';
      }
      return next;
    });
  }

  setLevel(fieldId: string, levelId: string): void {
    this.fieldLevelMap.update(map => ({ ...map, [fieldId]: levelId }));
  }

  getLevelForField(fieldId: string): string {
    return this.fieldLevelMap()[fieldId] ?? '';
  }

  // ─── Editor helpers ────────────────────────────────────────────────────
  execFormat(cmd: string): void {
    document.execCommand(cmd, false);
    this.updateFormatState();
    this.editorRef.nativeElement.focus();
  }

  execBlock(tag: string): void {
    document.execCommand('formatBlock', false, tag);
    this.editorRef.nativeElement.focus();
  }

  insertCode(): void {
    const sel = window.getSelection();
    if (!sel || sel.rangeCount === 0) return;
    const range = sel.getRangeAt(0);
    const selectedText = range.toString() || 'code here';
    const code = document.createElement('code');
    code.className = 'inline-code';
    code.textContent = selectedText;
    range.deleteContents();
    range.insertNode(code);
    this.editorRef.nativeElement.focus();
  }

  updateFormatState(): void {
    this.isBold.set(document.queryCommandState('bold'));
    this.isItalic.set(document.queryCommandState('italic'));
    this.isUnderline.set(document.queryCommandState('underline'));
  }

  onEditorFocus(): void {
    this.expanded.set(true);
  }

  // ─── Submit ───────────────────────────────────────────────────────────
  submit(): void {
    const htmlContent = this.editorRef?.nativeElement.innerHTML.trim() ?? '';
    const titleVal = this.title().trim();

    if (!titleVal) { this.error.set('Vui lòng nhập tiêu đề.'); return; }
    if (!htmlContent || htmlContent === '<br>') { this.error.set('Vui lòng nhập nội dung.'); return; }

    this.error.set('');
    this.submitting.set(true);

    const map = this.fieldLevelMap();
    const fields = Object.entries(map).map(([fieldId, levelId]) => ({ fieldId, levelId }));

    const req: AddPostRequest = {
      title: titleVal,
      postType: this.postType(),
      stringContents: [{ contentType: 0, content: htmlContent }],
      fields: fields.length > 0 ? fields : undefined
    };

    this.postService.addPost(req).subscribe({
      next: res => {
        this.postCreated.emit(res?.result ?? res);
        this.reset();
        this.submitting.set(false);
      },
      error: () => {
        this.error.set('Đăng bài thất bại. Vui lòng thử lại.');
        this.submitting.set(false);
      }
    });
  }

  cancel(): void {
    this.reset();
  }

  private reset(): void {
    this.title.set('');
    this.fieldLevelMap.set({});
    this.postType.set(1);
    this.error.set('');
    this.expanded.set(false);
    if (this.editorRef) this.editorRef.nativeElement.innerHTML = '';
  }
}

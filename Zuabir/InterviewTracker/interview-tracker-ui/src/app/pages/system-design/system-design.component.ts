import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ApiService, SystemDesignTopic } from '../../services/api.service';

@Component({
  selector: 'app-system-design',
  standalone: true,
  imports: [CommonModule, FormsModule],
  template: `
    <div>
      <div style="display: flex; justify-content: space-between; align-items: center; margin-bottom: 2rem;">
        <h1 style="font-size: 1.75rem; font-weight: 700;">System Design Topics</h1>
        <div style="display: flex; gap: 0.5rem;">
          @if (topics.length === 0) {
            <button class="btn btn-success" (click)="seedTopics()">Load 70+ Topics</button>
          }
          <button class="btn btn-primary" (click)="openModal()">+ Add Topic</button>
        </div>
      </div>

      <!-- Filters -->
      <div class="card" style="margin-bottom: 1.5rem;">
        <div style="display: flex; gap: 1rem; flex-wrap: wrap; align-items: center;">
          <select class="form-select" style="width: auto;" [(ngModel)]="filters.category" (change)="loadTopics()">
            <option value="">All Categories</option>
            @for (cat of allCategories; track cat) {
              <option [value]="cat">{{ cat }}</option>
            }
          </select>
          <select class="form-select" style="width: auto;" [(ngModel)]="filters.status" (change)="loadTopics()">
            <option value="">All Status</option>
            <option value="NotStarted">Not Started</option>
            <option value="Learning">Learning</option>
            <option value="Understood">Understood</option>
            <option value="Mastered">Mastered</option>
          </select>
          <label style="display: flex; align-items: center; gap: 0.5rem; cursor: pointer; margin-left: auto;">
            <input type="checkbox" [(ngModel)]="showFavoritesOnly" (change)="loadTopics()">
            <span style="color: #fbbf24;">&#9733;</span> Favorites Only
          </label>
        </div>
      </div>

      <!-- Stats Bar -->
      <div class="grid grid-4" style="margin-bottom: 1.5rem;">
        <div style="background: var(--bg-card); padding: 1rem; border-radius: 8px; text-align: center;">
          <div style="font-size: 1.5rem; font-weight: 700;">{{ allTopics.length }}</div>
          <div style="font-size: 0.75rem; color: var(--text-secondary);">Total</div>
        </div>
        <div style="background: var(--bg-card); padding: 1rem; border-radius: 8px; text-align: center;">
          <div style="font-size: 1.5rem; font-weight: 700; color: #10b981;">{{ getMasteredCount() }}</div>
          <div style="font-size: 0.75rem; color: var(--text-secondary);">Mastered</div>
        </div>
        <div style="background: var(--bg-card); padding: 1rem; border-radius: 8px; text-align: center;">
          <div style="font-size: 1.5rem; font-weight: 700; color: #fbbf24;">{{ getFavoriteCount() }}</div>
          <div style="font-size: 0.75rem; color: var(--text-secondary);">Favorites</div>
        </div>
        <div style="background: var(--bg-card); padding: 1rem; border-radius: 8px; text-align: center;">
          <div style="font-size: 1.5rem; font-weight: 700; color: #f59e0b;">{{ getLearningCount() }}</div>
          <div style="font-size: 0.75rem; color: var(--text-secondary);">Learning</div>
        </div>
      </div>

      <!-- Topics Grid -->
      @if (topics.length === 0) {
        <div class="card">
          <div class="empty-state">
            <div class="empty-state-icon">&#127959;</div>
            <div>No topics found. Click "Load 70+ Topics" to get started with comprehensive system design topics!</div>
          </div>
        </div>
      } @else {
        <div class="grid grid-3">
          @for (topic of topics; track topic.id) {
            <div class="card">
              <div style="display: flex; justify-content: space-between; align-items: start; margin-bottom: 1rem;">
                <div style="flex: 1;">
                  <div style="display: flex; align-items: center; gap: 0.5rem;">
                    <button style="background: none; border: none; cursor: pointer; font-size: 1.25rem; color: {{ topic.isFavorite ? '#fbbf24' : '#475569' }}; padding: 0;" (click)="toggleFavorite(topic)">
                      {{ topic.isFavorite ? '&#9733;' : '&#9734;' }}
                    </button>
                    <h3 style="font-weight: 600; margin-bottom: 0;">{{ topic.title }}</h3>
                  </div>
                  <span style="font-size: 0.875rem; color: var(--text-secondary);">{{ topic.category }}</span>
                </div>
                <span class="badge" [class.badge-solved]="topic.status === 'Mastered'" [class.badge-average]="topic.status === 'Understood'" [class.badge-pending]="topic.status === 'Learning'">
                  {{ topic.status }}
                </span>
              </div>

              <div style="margin-bottom: 1rem;">
                <div style="font-size: 0.75rem; color: var(--text-secondary); margin-bottom: 0.25rem;">Confidence Level</div>
                <div style="display: flex; gap: 0.25rem;">
                  @for (i of [1,2,3,4,5]; track i) {
                    <div style="width: 24px; height: 8px; border-radius: 4px;" [style.background]="i <= topic.confidenceLevel ? '#6366f1' : 'var(--bg-dark)'"></div>
                  }
                </div>
              </div>

              @if (topic.keyConcepts) {
                <div style="font-size: 0.875rem; color: var(--text-secondary); margin-bottom: 1rem;">
                  {{ topic.keyConcepts | slice:0:100 }}{{ topic.keyConcepts.length > 100 ? '...' : '' }}
                </div>
              }

              <div style="display: flex; gap: 0.5rem; flex-wrap: wrap;">
                @if (topic.lesson) {
                  <button class="btn btn-success" style="flex: 1;" (click)="openLessonModal(topic)">📖 Lesson</button>
                }
                <button class="btn btn-secondary" style="flex: 1;" (click)="editTopic(topic)">✏️ Edit</button>
                <button class="btn btn-primary" style="flex: 1;" (click)="openReviewModal(topic)">📝 Review</button>
                <button class="btn btn-danger" style="padding: 0.625rem;" (click)="deleteTopic(topic.id!)">🗑️</button>
              </div>
            </div>
          }
        </div>
      }

      <!-- Add/Edit Modal -->
      @if (showModal) {
        <div class="modal-backdrop" (click)="closeModal()">
          <div class="modal" (click)="$event.stopPropagation()">
            <h2 class="modal-title">{{ editingTopic ? 'Edit' : 'Add' }} System Design Topic</h2>
            <form (ngSubmit)="saveTopic()">
              <div class="form-group">
                <label class="form-label">Title *</label>
                <input type="text" class="form-input" [(ngModel)]="formData.title" name="title" required placeholder="e.g., Design Twitter">
              </div>
              <div class="grid grid-2">
                <div class="form-group">
                  <label class="form-label">Category *</label>
                  <select class="form-select" [(ngModel)]="formData.category" name="category" required>
                    <option value="">Select Category</option>
                    @for (cat of allCategories; track cat) {
                      <option [value]="cat">{{ cat }}</option>
                    }
                  </select>
                </div>
                <div class="form-group">
                  <label class="form-label">Difficulty</label>
                  <select class="form-select" [(ngModel)]="formData.difficulty" name="difficulty">
                    <option value="Easy">Easy</option>
                    <option value="Medium">Medium</option>
                    <option value="Hard">Hard</option>
                  </select>
                </div>
              </div>
              <div class="grid grid-2">
                <div class="form-group">
                  <label class="form-label">Status</label>
                  <select class="form-select" [(ngModel)]="formData.status" name="status">
                    <option value="NotStarted">Not Started</option>
                    <option value="Learning">Learning</option>
                    <option value="Understood">Understood</option>
                    <option value="Mastered">Mastered</option>
                  </select>
                </div>
                <div class="form-group">
                  <label class="form-label">Confidence (1-5)</label>
                  <select class="form-select" [(ngModel)]="formData.confidenceLevel" name="confidence">
                    <option [value]="1">1 - Very Low</option>
                    <option [value]="2">2 - Low</option>
                    <option [value]="3">3 - Medium</option>
                    <option [value]="4">4 - High</option>
                    <option [value]="5">5 - Very High</option>
                  </select>
                </div>
              </div>
              <div class="form-group">
                <label class="form-label">Key Concepts</label>
                <textarea class="form-textarea" [(ngModel)]="formData.keyConcepts" name="keyConcepts" rows="3" placeholder="List the key concepts to remember..."></textarea>
              </div>
              <div class="form-group">
                <label class="form-label">Resources</label>
                <textarea class="form-textarea" [(ngModel)]="formData.resources" name="resources" rows="2" placeholder="Links to articles, videos, etc."></textarea>
              </div>
              <div class="form-group">
                <label class="form-label">Notes</label>
                <textarea class="form-textarea" [(ngModel)]="formData.notes" name="notes" rows="2"></textarea>
              </div>
              <div class="form-group">
                <label style="display: flex; align-items: center; gap: 0.5rem; cursor: pointer;">
                  <input type="checkbox" [(ngModel)]="formData.isFavorite" name="favorite">
                  <span style="color: #fbbf24;">&#9733;</span> Mark as Favorite
                </label>
              </div>
              <div style="display: flex; gap: 1rem; justify-content: flex-end; margin-top: 1.5rem;">
                <button type="button" class="btn btn-secondary" (click)="closeModal()">Cancel</button>
                <button type="submit" class="btn btn-primary">{{ editingTopic ? 'Update' : 'Add' }} Topic</button>
              </div>
            </form>
          </div>
        </div>
      }

      <!-- Review Modal -->
      @if (showReviewModal) {
        <div class="modal-backdrop" (click)="closeReviewModal()">
          <div class="modal" (click)="$event.stopPropagation()">
            <h2 class="modal-title">Review: {{ selectedTopic?.title }}</h2>
            <form (ngSubmit)="saveReview()">
              <div class="form-group">
                <label class="form-label">New Status</label>
                <select class="form-select" [(ngModel)]="reviewData.status" name="status">
                  <option value="Learning">Still Learning</option>
                  <option value="Understood">Understood</option>
                  <option value="Mastered">Mastered</option>
                </select>
              </div>
              <div class="form-group">
                <label class="form-label">Confidence Level (1-5)</label>
                <div style="display: flex; gap: 0.5rem;">
                  @for (i of [1,2,3,4,5]; track i) {
                    <button type="button" class="btn" [class.btn-primary]="reviewData.confidenceLevel === i" [class.btn-secondary]="reviewData.confidenceLevel !== i" (click)="reviewData.confidenceLevel = i">
                      {{ i }}
                    </button>
                  }
                </div>
              </div>
              <div class="form-group">
                <label class="form-label">Review Notes</label>
                <textarea class="form-textarea" [(ngModel)]="reviewData.notes" name="notes" rows="3" placeholder="What did you review? Any new insights?"></textarea>
              </div>
              <div style="display: flex; gap: 1rem; justify-content: flex-end; margin-top: 1.5rem;">
                <button type="button" class="btn btn-secondary" (click)="closeReviewModal()">Cancel</button>
                <button type="submit" class="btn btn-primary">Save Review</button>
              </div>
            </form>
          </div>
        </div>
      }

      <!-- Lesson Modal -->
      @if (showLessonModal && lessonTopic) {
        <div class="modal-backdrop" (click)="closeLessonModal()">
          <div class="modal lesson-modal" (click)="$event.stopPropagation()" style="max-width: 900px; max-height: 90vh; overflow: hidden; display: flex; flex-direction: column;">
            <div style="display: flex; justify-content: space-between; align-items: center; padding-bottom: 1rem; border-bottom: 1px solid var(--border-color);">
              <div>
                <h2 class="modal-title" style="margin: 0;">{{ lessonTopic.title }}</h2>
                <div style="display: flex; gap: 0.5rem; margin-top: 0.5rem;">
                  <span class="badge">{{ lessonTopic.category }}</span>
                  <span class="badge" [class.badge-solved]="lessonTopic.difficulty === 'Easy'" [class.badge-average]="lessonTopic.difficulty === 'Medium'" [class.badge-pending]="lessonTopic.difficulty === 'Hard'">{{ lessonTopic.difficulty }}</span>
                </div>
              </div>
              <button class="btn btn-secondary" (click)="closeLessonModal()" style="padding: 0.5rem 1rem;">Close</button>
            </div>
            <div style="overflow-y: auto; flex: 1; padding: 1rem 0;">
              <div class="lesson-content" [innerHTML]="formatLesson(lessonTopic.lesson)"></div>
              @if (lessonTopic.resources) {
                <div style="margin-top: 2rem;">
                  <h3 style="color: #8b5cf6; margin-bottom: 1rem;">📚 Resources</h3>
                  <div style="background: #1e293b; padding: 1rem; border-radius: 8px;">{{ lessonTopic.resources }}</div>
                </div>
              }
            </div>
          </div>
        </div>
      }
    </div>
  `,
  styles: [`
    .lesson-content { font-size: 0.95rem; line-height: 1.8; }
    .lesson-content h1 { font-size: 1.75rem; font-weight: 700; margin: 1.5rem 0 1rem; color: #a78bfa; }
    .lesson-content h2 { font-size: 1.4rem; font-weight: 600; margin: 1.5rem 0 0.75rem; color: #c4b5fd; }
    .lesson-content h3 { font-size: 1.15rem; font-weight: 600; margin: 1.25rem 0 0.5rem; color: #ddd6fe; }
    .lesson-content strong { color: #fbbf24; }
    .lesson-content ul, .lesson-content ol { margin-left: 1.5rem; margin-bottom: 1rem; }
    .lesson-content li { margin-bottom: 0.5rem; }
    .lesson-content pre { background: #1e293b; padding: 1rem; border-radius: 8px; overflow-x: auto; margin: 1rem 0; }
    .lesson-content code { font-family: 'Consolas', 'Monaco', monospace; font-size: 0.875rem; }
    .lesson-content table { border-collapse: collapse; width: 100%; margin: 1rem 0; }
    .lesson-content th, .lesson-content td { border: 1px solid #374151; padding: 0.75rem; text-align: left; }
    .lesson-content th { background: #1e293b; font-weight: 600; }
  `]
})
export class SystemDesignComponent implements OnInit {
  topics: SystemDesignTopic[] = [];
  allTopics: SystemDesignTopic[] = [];
  showModal = false;
  showReviewModal = false;
  showLessonModal = false;
  editingTopic: SystemDesignTopic | null = null;
  selectedTopic: SystemDesignTopic | null = null;
  lessonTopic: SystemDesignTopic | null = null;
  showFavoritesOnly = false;

  filters = { category: '', status: '' };

  allCategories = [
    'Fundamentals', 'Load Balancing', 'Caching', 'Database', 'Message Queues',
    'Microservices', 'API Design', 'Security', 'Monitoring', 'System Design Problems',
    'Infrastructure', 'Data Engineering', 'Real-time'
  ];

  formData: Partial<SystemDesignTopic> = this.getEmptyForm();
  reviewData = { status: 'Understood', confidenceLevel: 3, notes: '' };

  constructor(private api: ApiService) {}

  ngOnInit() {
    this.loadTopics();
  }

  loadTopics() {
    this.api.getSystemDesignTopics(this.filters).subscribe(topics => {
      this.allTopics = topics;
      if (this.showFavoritesOnly) {
        this.topics = topics.filter(t => t.isFavorite);
      } else {
        this.topics = topics;
      }
    });
  }

  getMasteredCount(): number {
    return this.allTopics.filter(t => t.status === 'Mastered').length;
  }

  getFavoriteCount(): number {
    return this.allTopics.filter(t => t.isFavorite).length;
  }

  getLearningCount(): number {
    return this.allTopics.filter(t => t.status === 'Learning').length;
  }

  getEmptyForm(): Partial<SystemDesignTopic> {
    return {
      title: '', category: '', difficulty: 'Medium', status: 'NotStarted',
      confidenceLevel: 1, notes: '', keyConcepts: '', resources: '', tags: [], isFavorite: false
    };
  }

  openModal() {
    this.editingTopic = null;
    this.formData = this.getEmptyForm();
    this.showModal = true;
  }

  editTopic(topic: SystemDesignTopic) {
    this.editingTopic = topic;
    this.formData = { ...topic };
    this.showModal = true;
  }

  closeModal() {
    this.showModal = false;
    this.editingTopic = null;
  }

  saveTopic() {
    if (!this.formData.title || !this.formData.category) return;

    const topic = this.formData as SystemDesignTopic;
    if (this.editingTopic) {
      this.api.updateSystemDesignTopic(this.editingTopic.id!, topic).subscribe(() => {
        this.loadTopics();
        this.closeModal();
      });
    } else {
      this.api.createSystemDesignTopic(topic).subscribe(() => {
        this.loadTopics();
        this.closeModal();
      });
    }
  }

  deleteTopic(id: number) {
    if (confirm('Are you sure you want to delete this topic?')) {
      this.api.deleteSystemDesignTopic(id).subscribe(() => this.loadTopics());
    }
  }

  toggleFavorite(topic: SystemDesignTopic) {
    this.api.toggleSystemDesignFavorite(topic.id!).subscribe(updated => {
      topic.isFavorite = updated.isFavorite;
      if (this.showFavoritesOnly && !updated.isFavorite) {
        this.topics = this.topics.filter(t => t.id !== topic.id);
      }
    });
  }

  seedTopics() {
    this.api.seedSystemDesignTopics().subscribe({
      next: (res) => {
        alert(res.message);
        this.loadTopics();
      },
      error: (err) => {
        alert(err.error || 'Failed to seed topics');
      }
    });
  }

  openReviewModal(topic: SystemDesignTopic) {
    this.selectedTopic = topic;
    this.reviewData = { status: topic.status, confidenceLevel: topic.confidenceLevel, notes: '' };
    this.showReviewModal = true;
  }

  closeReviewModal() {
    this.showReviewModal = false;
    this.selectedTopic = null;
  }

  saveReview() {
    if (!this.selectedTopic) return;
    const updated = { ...this.selectedTopic, ...this.reviewData };
    this.api.updateSystemDesignTopic(this.selectedTopic.id!, updated).subscribe(() => {
      this.loadTopics();
      this.closeReviewModal();
    });
  }

  openLessonModal(topic: SystemDesignTopic) {
    this.lessonTopic = topic;
    this.showLessonModal = true;
  }

  closeLessonModal() {
    this.showLessonModal = false;
    this.lessonTopic = null;
  }

  formatLesson(lesson: string | undefined): string {
    if (!lesson) return '';
    return lesson
      .replace(/^### (.*$)/gm, '<h3>$1</h3>')
      .replace(/^## (.*$)/gm, '<h2>$1</h2>')
      .replace(/^# (.*$)/gm, '<h1>$1</h1>')
      .replace(/\*\*(.*?)\*\*/g, '<strong>$1</strong>')
      .replace(/\*(.*?)\*/g, '<em>$1</em>')
      .replace(/```(\w+)?\n([\s\S]*?)```/g, '<pre><code>$2</code></pre>')
      .replace(/`([^`]+)`/g, '<code style="background: #334155; padding: 0.125rem 0.25rem; border-radius: 4px;">$1</code>')
      .replace(/^\- (.*$)/gm, '<li>$1</li>')
      .replace(/\|(.+)\|/g, (match) => {
        const cells = match.split('|').filter(c => c.trim());
        const isHeader = lesson.indexOf(match) > 0 && lesson.charAt(lesson.indexOf(match) - 1) === '\n';
        return '<tr>' + cells.map(c => isHeader ? `<th>${c.trim()}</th>` : `<td>${c.trim()}</td>`).join('') + '</tr>';
      })
      .replace(/(<tr>.*<\/tr>)/g, '<table>$1</table>')
      .replace(/\n\n/g, '</p><p>')
      .replace(/\n/g, '<br>');
  }
}

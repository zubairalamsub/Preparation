import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ApiService, EntityFrameworkTopic } from '../../services/api.service';

@Component({
  selector: 'app-entity-framework',
  standalone: true,
  imports: [CommonModule, FormsModule],
  template: `
    <div>
      <div class="page-header">
        <h1>Entity Framework Core</h1>
        <div class="page-actions">
          @if (topics.length === 0) { <button class="btn btn-success" (click)="seedTopics()">Load Topics</button> }
          <button class="btn btn-primary" (click)="openModal()">+ Add</button>
        </div>
      </div>

      <div class="card filters-card">
        <div class="filters-container">
          <select class="form-select" [(ngModel)]="filters.category" (change)="loadTopics()">
            <option value="">All Categories</option>
            @for (cat of allCategories; track cat) { <option [value]="cat">{{ cat }}</option> }
          </select>
          <select class="form-select" [(ngModel)]="filters.status" (change)="loadTopics()">
            <option value="">All Status</option>
            <option value="NotStarted">Not Started</option><option value="Learning">Learning</option><option value="Understood">Understood</option><option value="Mastered">Mastered</option>
          </select>
          <label class="favorites-toggle"><input type="checkbox" [(ngModel)]="showFavoritesOnly" (change)="loadTopics()"><span style="color: #fbbf24;">&#9733;</span> Favorites</label>
        </div>
      </div>

      <div class="stats-grid">
        <div class="mini-stat"><div class="mini-stat-value">{{ allTopics.length }}</div><div class="mini-stat-label">Total</div></div>
        <div class="mini-stat"><div class="mini-stat-value" style="color: #10b981;">{{ getMasteredCount() }}</div><div class="mini-stat-label">Mastered</div></div>
        <div class="mini-stat"><div class="mini-stat-value" style="color: #fbbf24;">{{ getFavoriteCount() }}</div><div class="mini-stat-label">Favorites</div></div>
        <div class="mini-stat"><div class="mini-stat-value" style="color: #f59e0b;">{{ getLearningCount() }}</div><div class="mini-stat-label">Learning</div></div>
      </div>

      @if (topics.length === 0) {
        <div class="card"><div class="empty-state"><div class="empty-state-icon">&#128190;</div><div>No topics. Click "Load Topics" to start!</div></div></div>
      } @else {
        <div class="topics-grid">
          @for (topic of topics; track topic.id) {
            <div class="card topic-card">
              <div class="topic-card-header">
                <div class="topic-info">
                  <div class="topic-title-row">
                    <button class="fav-btn" [style.color]="topic.isFavorite ? '#fbbf24' : '#475569'" (click)="toggleFavorite(topic)">{{ topic.isFavorite ? '&#9733;' : '&#9734;' }}</button>
                    <h3 class="topic-title">{{ topic.title }}</h3>
                  </div>
                  <div class="topic-meta">
                    <span class="topic-category">{{ topic.category }}</span>
                    @if (topic.efVersion) { <span class="topic-version">EF {{ topic.efVersion }}</span> }
                  </div>
                </div>
                <span class="badge" [class.badge-solved]="topic.status === 'Mastered'" [class.badge-average]="topic.status === 'Understood'" [class.badge-pending]="topic.status === 'Learning'">{{ topic.status }}</span>
              </div>
              <div class="confidence-section"><div class="confidence-label">Confidence</div><div class="confidence-bar">@for (i of [1,2,3,4,5]; track i) { <div class="confidence-dot" [style.background]="i <= topic.confidenceLevel ? '#8b5cf6' : 'var(--bg-dark)'"></div> }</div></div>
              @if (topic.keyConcepts) { <div class="topic-concepts">{{ topic.keyConcepts | slice:0:100 }}{{ topic.keyConcepts.length > 100 ? '...' : '' }}</div> }
              @if (topic.problemScenario) {
                <div class="problem-scenario">
                  <strong>Real Problem:</strong> {{ topic.problemScenario | slice:0:120 }}{{ topic.problemScenario.length > 120 ? '...' : '' }}
                </div>
              }
              <div class="topic-card-actions">
                @if (topic.lesson) { <button class="btn btn-success" (click)="openLessonModal(topic)">&#128218; Lesson</button> }
                <button class="btn btn-secondary" (click)="editTopic(topic)">&#9998; Edit</button>
                <button class="btn btn-primary" (click)="openReviewModal(topic)">&#128221; Review</button>
                <button class="btn btn-danger btn-icon" (click)="deleteTopic(topic.id!)">&#128465;</button>
              </div>
            </div>
          }
        </div>
      }

      @if (showModal) {
        <div class="modal-backdrop" (click)="closeModal()"><div class="modal" (click)="$event.stopPropagation()">
          <h2 class="modal-title">{{ editingTopic ? 'Edit' : 'Add' }} EF Core Topic</h2>
          <form (ngSubmit)="saveTopic()">
            <div class="form-group"><label class="form-label">Title *</label><input type="text" class="form-input" [(ngModel)]="formData.title" name="title" required></div>
            <div class="grid grid-2">
              <div class="form-group"><label class="form-label">Category *</label><select class="form-select" [(ngModel)]="formData.category" name="category" required><option value="">Select</option>@for (cat of allCategories; track cat) { <option [value]="cat">{{ cat }}</option> }</select></div>
              <div class="form-group"><label class="form-label">EF Version</label><input type="text" class="form-input" [(ngModel)]="formData.efVersion" name="efVersion" placeholder="e.g., 6.0+"></div>
            </div>
            <div class="form-group"><label class="form-label">Key Concepts</label><textarea class="form-textarea" [(ngModel)]="formData.keyConcepts" name="keyConcepts" rows="3"></textarea></div>
            <div class="form-group"><label class="form-label">Problem Scenario</label><textarea class="form-textarea" [(ngModel)]="formData.problemScenario" name="problemScenario" rows="2" placeholder="Real-world problem this topic solves"></textarea></div>
            <div class="form-group"><label class="form-label">Code Example</label><textarea class="form-textarea" [(ngModel)]="formData.codeExample" name="codeExample" rows="3" style="font-family: monospace;"></textarea></div>
            <div style="display: flex; gap: 1rem; justify-content: flex-end; margin-top: 1.5rem;"><button type="button" class="btn btn-secondary" (click)="closeModal()">Cancel</button><button type="submit" class="btn btn-primary">{{ editingTopic ? 'Update' : 'Add' }}</button></div>
          </form>
        </div></div>
      }

      @if (showReviewModal) {
        <div class="modal-backdrop" (click)="closeReviewModal()"><div class="modal" (click)="$event.stopPropagation()">
          <h2 class="modal-title">Review: {{ selectedTopic?.title }}</h2>
          <form (ngSubmit)="saveReview()">
            <div class="form-group"><label class="form-label">Status</label><select class="form-select" [(ngModel)]="reviewData.status" name="status"><option value="Learning">Learning</option><option value="Understood">Understood</option><option value="Mastered">Mastered</option></select></div>
            <div class="form-group"><label class="form-label">Confidence</label><div style="display: flex; gap: 0.5rem;">@for (i of [1,2,3,4,5]; track i) { <button type="button" class="btn" [class.btn-primary]="reviewData.confidenceLevel === i" [class.btn-secondary]="reviewData.confidenceLevel !== i" (click)="reviewData.confidenceLevel = i">{{ i }}</button> }</div></div>
            <div style="display: flex; gap: 1rem; justify-content: flex-end; margin-top: 1.5rem;"><button type="button" class="btn btn-secondary" (click)="closeReviewModal()">Cancel</button><button type="submit" class="btn btn-primary">Save</button></div>
          </form>
        </div></div>
      }

      @if (showLessonModal && lessonTopic) {
        <div class="modal-backdrop" (click)="closeLessonModal()">
          <div class="modal lesson-modal" (click)="$event.stopPropagation()" style="max-width: 900px; max-height: 90vh; overflow: hidden; display: flex; flex-direction: column;">
            <div style="display: flex; justify-content: space-between; align-items: center; padding-bottom: 1rem; border-bottom: 1px solid var(--border-color);">
              <h2 class="modal-title" style="margin: 0;">{{ lessonTopic.title }}</h2>
              <button class="btn btn-secondary" (click)="closeLessonModal()" style="padding: 0.5rem 1rem;">Close</button>
            </div>
            <div style="overflow-y: auto; flex: 1; padding: 1rem 0;">
              <div class="lesson-content" [innerHTML]="formatLesson(lessonTopic.lesson)"></div>
              @if (lessonTopic.codeExample) {
                <div style="margin-top: 2rem;">
                  <h3 style="color: #8b5cf6; margin-bottom: 1rem;">Code Example</h3>
                  <pre style="background: #1e293b; padding: 1rem; border-radius: 8px; overflow-x: auto; font-size: 0.875rem; line-height: 1.5;"><code>{{ lessonTopic.codeExample }}</code></pre>
                </div>
              }
              @if (lessonTopic.problemScenario) {
                <div style="margin-top: 2rem;">
                  <h3 style="color: #f59e0b; margin-bottom: 1rem;">Real-World Problem</h3>
                  <div style="background: #1e293b; padding: 1rem; border-radius: 8px; border-left: 4px solid #f59e0b;">{{ lessonTopic.problemScenario }}</div>
                </div>
              }
            </div>
          </div>
        </div>
      }
    </div>
  `,
  styles: [`
    /* Page Header */
    .page-header { display: flex; justify-content: space-between; align-items: center; margin-bottom: 1.5rem; flex-wrap: wrap; gap: 1rem; }
    .page-header h1 { font-size: 1.75rem; font-weight: 700; margin: 0; }
    .page-actions { display: flex; gap: 0.5rem; flex-wrap: wrap; }

    /* Filters */
    .filters-card { margin-bottom: 1rem; }
    .filters-container { display: flex; gap: 1rem; flex-wrap: wrap; align-items: center; }
    .filters-container .form-select { width: auto; min-width: 140px; }
    .favorites-toggle { display: flex; align-items: center; gap: 0.5rem; cursor: pointer; margin-left: auto; }

    /* Stats Grid */
    .stats-grid { display: grid; grid-template-columns: repeat(4, 1fr); gap: 0.75rem; margin-bottom: 1.5rem; }
    .mini-stat { background: var(--bg-card); padding: 0.875rem; border-radius: 8px; text-align: center; border: 1px solid var(--border); }
    .mini-stat-value { font-size: 1.5rem; font-weight: 700; }
    .mini-stat-label { font-size: 0.75rem; color: var(--text-secondary); }

    /* Topics Grid */
    .topics-grid { display: grid; grid-template-columns: repeat(3, 1fr); gap: 1rem; }

    /* Topic Card */
    .topic-card { display: flex; flex-direction: column; }
    .topic-card-header { display: flex; justify-content: space-between; align-items: flex-start; margin-bottom: 0.75rem; gap: 0.5rem; }
    .topic-info { flex: 1; min-width: 0; }
    .topic-title-row { display: flex; align-items: center; gap: 0.5rem; }
    .topic-title { font-weight: 600; font-size: 1rem; margin: 0; word-break: break-word; }
    .topic-meta { display: flex; flex-wrap: wrap; gap: 0.5rem; align-items: center; margin-top: 0.25rem; }
    .topic-category { font-size: 0.875rem; color: var(--text-secondary); }
    .topic-version { font-size: 0.75rem; color: #8b5cf6; }
    .fav-btn { background: none; border: none; cursor: pointer; font-size: 1.25rem; padding: 0; line-height: 1; }

    /* Confidence */
    .confidence-section { margin-bottom: 0.75rem; }
    .confidence-label { font-size: 0.75rem; color: var(--text-secondary); margin-bottom: 0.25rem; }
    .confidence-bar { display: flex; gap: 0.25rem; }
    .confidence-dot { width: 24px; height: 8px; border-radius: 4px; }

    /* Concepts & Problem */
    .topic-concepts { font-size: 0.875rem; color: var(--text-secondary); margin-bottom: 0.75rem; line-height: 1.4; }
    .problem-scenario { font-size: 0.875rem; color: #f59e0b; margin-bottom: 0.75rem; line-height: 1.4; background: rgba(245, 158, 11, 0.1); padding: 0.5rem; border-radius: 4px; }
    .problem-scenario strong { color: #fbbf24; }

    /* Card Actions */
    .topic-card-actions { display: flex; gap: 0.5rem; flex-wrap: wrap; margin-top: auto; }
    .topic-card-actions .btn { flex: 1; min-width: 70px; }
    .topic-card-actions .btn-icon { flex: 0 0 auto; padding: 0.625rem; }

    /* Lesson Modal */
    .lesson-modal { width: 90%; }
    .lesson-content { font-size: 0.95rem; line-height: 1.8; }
    .lesson-content h1 { font-size: 1.75rem; font-weight: 700; margin: 1.5rem 0 1rem; color: #a78bfa; }
    .lesson-content h2 { font-size: 1.4rem; font-weight: 600; margin: 1.5rem 0 0.75rem; color: #c4b5fd; }
    .lesson-content h3 { font-size: 1.15rem; font-weight: 600; margin: 1.25rem 0 0.5rem; color: #ddd6fe; }
    .lesson-content pre { background: #1e293b; padding: 1rem; border-radius: 8px; overflow-x: auto; margin: 1rem 0; }
    .lesson-content code { font-family: 'Consolas', 'Monaco', monospace; font-size: 0.875rem; }
    .lesson-content strong { color: #fbbf24; }

    /* Mobile Responsive */
    @media (max-width: 1024px) {
      .topics-grid { grid-template-columns: repeat(2, 1fr); }
      .stats-grid { grid-template-columns: repeat(4, 1fr); }
    }

    @media (max-width: 768px) {
      .page-header { flex-direction: column; align-items: flex-start; }
      .page-header h1 { font-size: 1.4rem; }
      .page-actions { width: 100%; }
      .page-actions .btn { flex: 1; }

      .filters-container { flex-direction: column; align-items: stretch; }
      .filters-container .form-select { width: 100%; }
      .favorites-toggle { margin-left: 0; justify-content: flex-start; }

      .stats-grid { grid-template-columns: repeat(2, 1fr); }
      .mini-stat-value { font-size: 1.25rem; }

      .topics-grid { grid-template-columns: 1fr; }

      .topic-card-header { flex-direction: column; }
      .topic-card-actions .btn { flex: 1 1 45%; font-size: 0.8rem; }

      .lesson-modal { max-width: none !important; width: 95% !important; }
      .lesson-content { font-size: 0.9rem !important; line-height: 1.6 !important; }
      .lesson-content h1 { font-size: 1.3rem !important; }
      .lesson-content h2 { font-size: 1.15rem !important; }
      .lesson-content h3 { font-size: 1rem !important; }
      .lesson-content pre { font-size: 0.75rem; padding: 0.75rem; }
    }

    @media (max-width: 480px) {
      .stats-grid { grid-template-columns: repeat(2, 1fr); gap: 0.5rem; }
      .mini-stat { padding: 0.625rem; }
      .mini-stat-value { font-size: 1.1rem; }
      .topic-card-actions .btn { padding: 0.5rem; }
    }
  `]
})
export class EntityFrameworkComponent implements OnInit {
  topics: EntityFrameworkTopic[] = []; allTopics: EntityFrameworkTopic[] = []; showModal = false; showReviewModal = false; showLessonModal = false;
  editingTopic: EntityFrameworkTopic | null = null; selectedTopic: EntityFrameworkTopic | null = null; lessonTopic: EntityFrameworkTopic | null = null; showFavoritesOnly = false;
  filters = { category: '', status: '' };
  allCategories = ['Fundamentals', 'Relationships', 'Querying', 'Migrations', 'Performance', 'Advanced Features'];
  formData: Partial<EntityFrameworkTopic> = this.getEmptyForm(); reviewData = { status: 'Understood', confidenceLevel: 3 };

  constructor(private api: ApiService) {}
  ngOnInit() { this.loadTopics(); }
  loadTopics() { this.api.getEntityFrameworkTopics(this.filters).subscribe(t => { this.allTopics = t; this.topics = this.showFavoritesOnly ? t.filter(x => x.isFavorite) : t; }); }
  getMasteredCount() { return this.allTopics.filter(t => t.status === 'Mastered').length; }
  getFavoriteCount() { return this.allTopics.filter(t => t.isFavorite).length; }
  getLearningCount() { return this.allTopics.filter(t => t.status === 'Learning').length; }
  getEmptyForm(): Partial<EntityFrameworkTopic> { return { title: '', category: '', difficulty: 'Medium', status: 'NotStarted', confidenceLevel: 1, keyConcepts: '', codeExample: '', efVersion: '', problemScenario: '', tags: [], isFavorite: false }; }

  openModal() { this.editingTopic = null; this.formData = this.getEmptyForm(); this.showModal = true; }
  editTopic(t: EntityFrameworkTopic) { this.editingTopic = t; this.formData = { ...t }; this.showModal = true; }
  closeModal() { this.showModal = false; }
  saveTopic() { if (!this.formData.title || !this.formData.category) return; const t = this.formData as EntityFrameworkTopic; if (this.editingTopic) this.api.updateEntityFrameworkTopic(this.editingTopic.id!, t).subscribe(() => { this.loadTopics(); this.closeModal(); }); else this.api.createEntityFrameworkTopic(t).subscribe(() => { this.loadTopics(); this.closeModal(); }); }
  deleteTopic(id: number) { if (confirm('Delete?')) this.api.deleteEntityFrameworkTopic(id).subscribe(() => this.loadTopics()); }
  toggleFavorite(t: EntityFrameworkTopic) { this.api.toggleEntityFrameworkFavorite(t.id!).subscribe(u => { t.isFavorite = u.isFavorite; if (this.showFavoritesOnly && !u.isFavorite) this.topics = this.topics.filter(x => x.id !== t.id); }); }
  seedTopics() { this.api.seedEntityFrameworkTopics().subscribe({ next: r => { alert(r.message); this.loadTopics(); }, error: e => alert(e.error || 'Failed') }); }
  openReviewModal(t: EntityFrameworkTopic) { this.selectedTopic = t; this.reviewData = { status: t.status, confidenceLevel: t.confidenceLevel }; this.showReviewModal = true; }
  closeReviewModal() { this.showReviewModal = false; }
  saveReview() { if (!this.selectedTopic) return; this.api.updateEntityFrameworkTopic(this.selectedTopic.id!, { ...this.selectedTopic, ...this.reviewData }).subscribe(() => { this.loadTopics(); this.closeReviewModal(); }); }

  openLessonModal(t: EntityFrameworkTopic) { this.lessonTopic = t; this.showLessonModal = true; }
  closeLessonModal() { this.showLessonModal = false; this.lessonTopic = null; }

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
      .replace(/\n\n/g, '</p><p>')
      .replace(/\n/g, '<br>');
  }
}

import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ApiService, DesignPatternTopic } from '../../services/api.service';

@Component({
  selector: 'app-design-patterns',
  standalone: true,
  imports: [CommonModule, FormsModule],
  template: `
    <div>
      <div style="display: flex; justify-content: space-between; align-items: center; margin-bottom: 2rem;">
        <h1 style="font-size: 1.75rem; font-weight: 700;">Design Patterns</h1>
        <div style="display: flex; gap: 0.5rem;">
          @if (topics.length === 0) { <button class="btn btn-success" (click)="seedTopics()">Load Design Patterns with Lessons</button> }
          <button class="btn btn-primary" (click)="openModal()">+ Add Pattern</button>
        </div>
      </div>

      <div class="card" style="margin-bottom: 1.5rem;">
        <div style="display: flex; gap: 1rem; flex-wrap: wrap; align-items: center;">
          <select class="form-select" style="width: auto;" [(ngModel)]="filters.category" (change)="loadTopics()">
            <option value="">All Categories</option>
            @for (cat of allCategories; track cat) { <option [value]="cat">{{ cat }}</option> }
          </select>
          <select class="form-select" style="width: auto;" [(ngModel)]="filters.status" (change)="loadTopics()">
            <option value="">All Status</option>
            <option value="NotStarted">Not Started</option><option value="Learning">Learning</option><option value="Understood">Understood</option><option value="Mastered">Mastered</option>
          </select>
          <label style="display: flex; align-items: center; gap: 0.5rem; cursor: pointer; margin-left: auto;"><input type="checkbox" [(ngModel)]="showFavoritesOnly" (change)="loadTopics()"><span style="color: #fbbf24;">&#9733;</span> Favorites</label>
        </div>
      </div>

      <div class="grid grid-4" style="margin-bottom: 1.5rem;">
        <div style="background: var(--bg-card); padding: 1rem; border-radius: 8px; text-align: center;"><div style="font-size: 1.5rem; font-weight: 700;">{{ allTopics.length }}</div><div style="font-size: 0.75rem; color: var(--text-secondary);">Total</div></div>
        <div style="background: var(--bg-card); padding: 1rem; border-radius: 8px; text-align: center;"><div style="font-size: 1.5rem; font-weight: 700; color: #10b981;">{{ getMasteredCount() }}</div><div style="font-size: 0.75rem; color: var(--text-secondary);">Mastered</div></div>
        <div style="background: var(--bg-card); padding: 1rem; border-radius: 8px; text-align: center;"><div style="font-size: 1.5rem; font-weight: 700; color: #fbbf24;">{{ getFavoriteCount() }}</div><div style="font-size: 0.75rem; color: var(--text-secondary);">Favorites</div></div>
        <div style="background: var(--bg-card); padding: 1rem; border-radius: 8px; text-align: center;"><div style="font-size: 1.5rem; font-weight: 700; color: #f59e0b;">{{ getLearningCount() }}</div><div style="font-size: 0.75rem; color: var(--text-secondary);">Learning</div></div>
      </div>

      @if (topics.length === 0) {
        <div class="card"><div class="empty-state"><div class="empty-state-icon">&#128208;</div><div>No patterns. Click "Load Design Patterns with Lessons" to start!</div></div></div>
      } @else {
        <div class="grid grid-3">
          @for (topic of topics; track topic.id) {
            <div class="card">
              <div style="display: flex; justify-content: space-between; align-items: start; margin-bottom: 1rem;">
                <div style="flex: 1;">
                  <div style="display: flex; align-items: center; gap: 0.5rem;">
                    <button style="background: none; border: none; cursor: pointer; font-size: 1.25rem; color: {{ topic.isFavorite ? '#fbbf24' : '#475569' }}; padding: 0;" (click)="toggleFavorite(topic)">{{ topic.isFavorite ? '&#9733;' : '&#9734;' }}</button>
                    <h3 style="font-weight: 600; margin-bottom: 0;">{{ topic.title }}</h3>
                  </div>
                  <span style="font-size: 0.875rem; padding: 0.125rem 0.5rem; border-radius: 4px;" [style.background]="getCategoryColor(topic.category)" [style.color]="'white'">{{ topic.category }}</span>
                </div>
                <span class="badge" [class.badge-solved]="topic.status === 'Mastered'" [class.badge-average]="topic.status === 'Understood'" [class.badge-pending]="topic.status === 'Learning'">{{ topic.status }}</span>
              </div>
              <div style="margin-bottom: 1rem;"><div style="font-size: 0.75rem; color: var(--text-secondary); margin-bottom: 0.25rem;">Confidence</div><div style="display: flex; gap: 0.25rem;">@for (i of [1,2,3,4,5]; track i) { <div style="width: 24px; height: 8px; border-radius: 4px;" [style.background]="i <= topic.confidenceLevel ? '#059669' : 'var(--bg-dark)'"></div> }</div></div>
              @if (topic.keyConcepts) { <div style="font-size: 0.875rem; color: var(--text-secondary); margin-bottom: 0.5rem;">{{ topic.keyConcepts | slice:0:80 }}{{ topic.keyConcepts.length > 80 ? '...' : '' }}</div> }
              @if (topic.useCases) { <div style="font-size: 0.75rem; color: #6366f1; margin-bottom: 1rem;"><strong>Use:</strong> {{ topic.useCases | slice:0:60 }}{{ topic.useCases.length > 60 ? '...' : '' }}</div> }
              <div style="display: flex; gap: 0.5rem; flex-wrap: wrap;">
                @if (topic.lesson) { <button class="btn btn-success" style="flex: 1;" (click)="openLessonModal(topic)">View Lesson</button> }
                <button class="btn btn-secondary" style="flex: 1;" (click)="editTopic(topic)">Edit</button>
                <button class="btn btn-primary" style="flex: 1;" (click)="openReviewModal(topic)">Review</button>
                <button class="btn btn-danger" style="padding: 0.625rem;" (click)="deleteTopic(topic.id!)">&#128465;</button>
              </div>
            </div>
          }
        </div>
      }

      @if (showModal) {
        <div class="modal-backdrop" (click)="closeModal()"><div class="modal" (click)="$event.stopPropagation()">
          <h2 class="modal-title">{{ editingTopic ? 'Edit' : 'Add' }} Design Pattern</h2>
          <form (ngSubmit)="saveTopic()">
            <div class="form-group"><label class="form-label">Title *</label><input type="text" class="form-input" [(ngModel)]="formData.title" name="title" required></div>
            <div class="grid grid-2">
              <div class="form-group"><label class="form-label">Category *</label><select class="form-select" [(ngModel)]="formData.category" name="category" required><option value="">Select</option>@for (cat of allCategories; track cat) { <option [value]="cat">{{ cat }}</option> }</select></div>
              <div class="form-group"><label class="form-label">Difficulty</label><select class="form-select" [(ngModel)]="formData.difficulty" name="difficulty"><option value="Easy">Easy</option><option value="Medium">Medium</option><option value="Hard">Hard</option></select></div>
            </div>
            <div class="form-group"><label class="form-label">Key Concepts</label><textarea class="form-textarea" [(ngModel)]="formData.keyConcepts" name="keyConcepts" rows="3"></textarea></div>
            <div class="form-group"><label class="form-label">Use Cases</label><textarea class="form-textarea" [(ngModel)]="formData.useCases" name="useCases" rows="2"></textarea></div>
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
                  <h3 style="color: #059669; margin-bottom: 1rem;">Code Example</h3>
                  <pre style="background: #1e293b; padding: 1rem; border-radius: 8px; overflow-x: auto; font-size: 0.875rem; line-height: 1.5;"><code>{{ lessonTopic.codeExample }}</code></pre>
                </div>
              }
            </div>
          </div>
        </div>
      }
    </div>
  `,
  styles: [`
    .lesson-modal { width: 90%; }
    .lesson-content { font-size: 0.95rem; line-height: 1.8; }
    .lesson-content h1 { font-size: 1.75rem; font-weight: 700; margin: 1.5rem 0 1rem; color: #34d399; }
    .lesson-content h2 { font-size: 1.4rem; font-weight: 600; margin: 1.5rem 0 0.75rem; color: #6ee7b7; }
    .lesson-content h3 { font-size: 1.15rem; font-weight: 600; margin: 1.25rem 0 0.5rem; color: #a7f3d0; }
    .lesson-content pre { background: #1e293b; padding: 1rem; border-radius: 8px; overflow-x: auto; margin: 1rem 0; }
    .lesson-content code { font-family: 'Consolas', 'Monaco', monospace; font-size: 0.875rem; }
    .lesson-content strong { color: #fbbf24; }
  `]
})
export class DesignPatternsComponent implements OnInit {
  topics: DesignPatternTopic[] = []; allTopics: DesignPatternTopic[] = []; showModal = false; showReviewModal = false; showLessonModal = false;
  editingTopic: DesignPatternTopic | null = null; selectedTopic: DesignPatternTopic | null = null; lessonTopic: DesignPatternTopic | null = null; showFavoritesOnly = false;
  filters = { category: '', status: '' };
  allCategories = ['Creational', 'Structural', 'Behavioral', 'Architectural'];
  formData: Partial<DesignPatternTopic> = this.getEmptyForm(); reviewData = { status: 'Understood', confidenceLevel: 3 };

  constructor(private api: ApiService) {}
  ngOnInit() { this.loadTopics(); }
  loadTopics() { this.api.getDesignPatternTopics(this.filters).subscribe(t => { this.allTopics = t; this.topics = this.showFavoritesOnly ? t.filter(x => x.isFavorite) : t; }); }
  getMasteredCount() { return this.allTopics.filter(t => t.status === 'Mastered').length; }
  getFavoriteCount() { return this.allTopics.filter(t => t.isFavorite).length; }
  getLearningCount() { return this.allTopics.filter(t => t.status === 'Learning').length; }
  getEmptyForm(): Partial<DesignPatternTopic> { return { title: '', category: '', difficulty: 'Medium', status: 'NotStarted', confidenceLevel: 1, keyConcepts: '', codeExample: '', useCases: '', tags: [], isFavorite: false }; }

  getCategoryColor(cat: string): string {
    switch(cat) { case 'Creational': return '#2563eb'; case 'Structural': return '#7c3aed'; case 'Behavioral': return '#059669'; case 'Architectural': return '#dc2626'; default: return '#6b7280'; }
  }

  openModal() { this.editingTopic = null; this.formData = this.getEmptyForm(); this.showModal = true; }
  editTopic(t: DesignPatternTopic) { this.editingTopic = t; this.formData = { ...t }; this.showModal = true; }
  closeModal() { this.showModal = false; }
  saveTopic() { if (!this.formData.title || !this.formData.category) return; const t = this.formData as DesignPatternTopic; if (this.editingTopic) this.api.updateDesignPatternTopic(this.editingTopic.id!, t).subscribe(() => { this.loadTopics(); this.closeModal(); }); else this.api.createDesignPatternTopic(t).subscribe(() => { this.loadTopics(); this.closeModal(); }); }
  deleteTopic(id: number) { if (confirm('Delete?')) this.api.deleteDesignPatternTopic(id).subscribe(() => this.loadTopics()); }
  toggleFavorite(t: DesignPatternTopic) { this.api.toggleDesignPatternFavorite(t.id!).subscribe(u => { t.isFavorite = u.isFavorite; if (this.showFavoritesOnly && !u.isFavorite) this.topics = this.topics.filter(x => x.id !== t.id); }); }
  seedTopics() { this.api.seedDesignPatternTopics().subscribe({ next: r => { alert(r.message); this.loadTopics(); }, error: e => alert(e.error || 'Failed') }); }
  openReviewModal(t: DesignPatternTopic) { this.selectedTopic = t; this.reviewData = { status: t.status, confidenceLevel: t.confidenceLevel }; this.showReviewModal = true; }
  closeReviewModal() { this.showReviewModal = false; }
  saveReview() { if (!this.selectedTopic) return; this.api.updateDesignPatternTopic(this.selectedTopic.id!, { ...this.selectedTopic, ...this.reviewData }).subscribe(() => { this.loadTopics(); this.closeReviewModal(); }); }

  openLessonModal(t: DesignPatternTopic) { this.lessonTopic = t; this.showLessonModal = true; }
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

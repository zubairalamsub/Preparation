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

              <div style="display: flex; gap: 0.5rem;">
                <button class="btn btn-secondary" style="flex: 1;" (click)="editTopic(topic)">Edit</button>
                <button class="btn btn-primary" style="flex: 1;" (click)="openReviewModal(topic)">Review</button>
                <button class="btn btn-danger" style="padding: 0.625rem;" (click)="deleteTopic(topic.id!)">&#128465;</button>
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
    </div>
  `
})
export class SystemDesignComponent implements OnInit {
  topics: SystemDesignTopic[] = [];
  allTopics: SystemDesignTopic[] = [];
  showModal = false;
  showReviewModal = false;
  editingTopic: SystemDesignTopic | null = null;
  selectedTopic: SystemDesignTopic | null = null;
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
}

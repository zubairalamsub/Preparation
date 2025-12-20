import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ApiService, DSAProblem } from '../../services/api.service';

@Component({
  selector: 'app-dsa',
  standalone: true,
  imports: [CommonModule, FormsModule],
  template: `
    <div>
      <div style="display: flex; justify-content: space-between; align-items: center; margin-bottom: 2rem;">
        <h1 style="font-size: 1.75rem; font-weight: 700;">DSA Problems</h1>
        <div style="display: flex; gap: 0.5rem;">
          @if (problems.length === 0) {
            <button class="btn btn-success" (click)="seedProblems()">Load 75+ Problems</button>
          }
          <button class="btn btn-primary" (click)="openModal()">+ Add Problem</button>
        </div>
      </div>

      <!-- Filters -->
      <div class="card" style="margin-bottom: 1.5rem;">
        <div style="display: flex; gap: 1rem; flex-wrap: wrap; align-items: center;">
          <select class="form-select" style="width: auto;" [(ngModel)]="filters.category" (change)="loadProblems()">
            <option value="">All Categories</option>
            @for (cat of categories; track cat) {
              <option [value]="cat">{{ cat }}</option>
            }
          </select>
          <select class="form-select" style="width: auto;" [(ngModel)]="filters.difficulty" (change)="loadProblems()">
            <option value="">All Difficulties</option>
            <option value="Easy">Easy</option>
            <option value="Medium">Medium</option>
            <option value="Hard">Hard</option>
          </select>
          <select class="form-select" style="width: auto;" [(ngModel)]="filters.status" (change)="loadProblems()">
            <option value="">All Status</option>
            <option value="NotStarted">Not Started</option>
            <option value="InProgress">In Progress</option>
            <option value="Solved">Solved</option>
            <option value="NeedsReview">Needs Review</option>
          </select>
          <label style="display: flex; align-items: center; gap: 0.5rem; cursor: pointer; margin-left: auto;">
            <input type="checkbox" [(ngModel)]="showFavoritesOnly" (change)="loadProblems()">
            <span style="color: #fbbf24;">&#9733;</span> Favorites Only
          </label>
        </div>
      </div>

      <!-- Stats Bar -->
      <div class="grid grid-4" style="margin-bottom: 1.5rem;">
        <div style="background: var(--bg-card); padding: 1rem; border-radius: 8px; text-align: center;">
          <div style="font-size: 1.5rem; font-weight: 700;">{{ problems.length }}</div>
          <div style="font-size: 0.75rem; color: var(--text-secondary);">Total</div>
        </div>
        <div style="background: var(--bg-card); padding: 1rem; border-radius: 8px; text-align: center;">
          <div style="font-size: 1.5rem; font-weight: 700; color: #10b981;">{{ getSolvedCount() }}</div>
          <div style="font-size: 0.75rem; color: var(--text-secondary);">Solved</div>
        </div>
        <div style="background: var(--bg-card); padding: 1rem; border-radius: 8px; text-align: center;">
          <div style="font-size: 1.5rem; font-weight: 700; color: #fbbf24;">{{ getFavoriteCount() }}</div>
          <div style="font-size: 0.75rem; color: var(--text-secondary);">Favorites</div>
        </div>
        <div style="background: var(--bg-card); padding: 1rem; border-radius: 8px; text-align: center;">
          <div style="font-size: 1.5rem; font-weight: 700; color: #f59e0b;">{{ getNeedsReviewCount() }}</div>
          <div style="font-size: 0.75rem; color: var(--text-secondary);">Needs Review</div>
        </div>
      </div>

      <!-- Problems Table -->
      <div class="card">
        @if (problems.length === 0) {
          <div class="empty-state">
            <div class="empty-state-icon">&#128218;</div>
            <div>No problems found. Click "Load 75+ Problems" to get started with popular LeetCode problems!</div>
          </div>
        } @else {
          <table class="table">
            <thead>
              <tr>
                <th style="width: 40px;"></th>
                <th>Title</th>
                <th>Category</th>
                <th>Difficulty</th>
                <th>Status</th>
                <th>Time</th>
                <th>Attempts</th>
                <th>Actions</th>
              </tr>
            </thead>
            <tbody>
              @for (problem of problems; track problem.id) {
                <tr>
                  <td>
                    <button style="background: none; border: none; cursor: pointer; font-size: 1.25rem; color: {{ problem.isFavorite ? '#fbbf24' : '#475569' }};" (click)="toggleFavorite(problem)">
                      {{ problem.isFavorite ? '&#9733;' : '&#9734;' }}
                    </button>
                  </td>
                  <td>
                    <div style="font-weight: 500;">
                      @if (problem.leetCodeNumber) {
                        <span style="color: var(--text-secondary);">#{{ problem.leetCodeNumber }}</span>
                      }
                      {{ problem.title }}
                    </div>
                    @if (problem.problemUrl) {
                      <a [href]="problem.problemUrl" target="_blank" style="font-size: 0.75rem; color: var(--primary);">View Problem</a>
                    }
                  </td>
                  <td>{{ problem.category }}</td>
                  <td>
                    <span class="badge" [class.badge-easy]="problem.difficulty === 'Easy'" [class.badge-medium]="problem.difficulty === 'Medium'" [class.badge-hard]="problem.difficulty === 'Hard'">
                      {{ problem.difficulty }}
                    </span>
                  </td>
                  <td>
                    <span class="badge" [class.badge-solved]="problem.status === 'Solved'" [class.badge-pending]="problem.status === 'InProgress' || problem.status === 'NeedsReview'">
                      {{ problem.status }}
                    </span>
                  </td>
                  <td>{{ problem.timeTakenMinutes }} min</td>
                  <td>{{ problem.attemptCount }}</td>
                  <td>
                    <div style="display: flex; gap: 0.5rem;">
                      <button class="btn btn-secondary" style="padding: 0.375rem 0.75rem;" (click)="openAttemptModal(problem)">Attempt</button>
                      <button class="btn btn-secondary" style="padding: 0.375rem 0.75rem;" (click)="editProblem(problem)">Edit</button>
                      <button class="btn btn-danger" style="padding: 0.375rem 0.75rem;" (click)="deleteProblem(problem.id!)">&#128465;</button>
                    </div>
                  </td>
                </tr>
              }
            </tbody>
          </table>
        }
      </div>

      <!-- Add/Edit Modal -->
      @if (showModal) {
        <div class="modal-backdrop" (click)="closeModal()">
          <div class="modal" (click)="$event.stopPropagation()">
            <h2 class="modal-title">{{ editingProblem ? 'Edit' : 'Add' }} DSA Problem</h2>
            <form (ngSubmit)="saveProblem()">
              <div class="form-group">
                <label class="form-label">Title *</label>
                <input type="text" class="form-input" [(ngModel)]="formData.title" name="title" required>
              </div>
              <div class="grid grid-2">
                <div class="form-group">
                  <label class="form-label">Category *</label>
                  <select class="form-select" [(ngModel)]="formData.category" name="category" required>
                    <option value="">Select Category</option>
                    <option value="Array">Array</option>
                    <option value="String">String</option>
                    <option value="Linked List">Linked List</option>
                    <option value="Tree">Tree</option>
                    <option value="Graph">Graph</option>
                    <option value="Dynamic Programming">Dynamic Programming</option>
                    <option value="Backtracking">Backtracking</option>
                    <option value="Binary Search">Binary Search</option>
                    <option value="Stack">Stack</option>
                    <option value="Heap">Heap</option>
                    <option value="Trie">Trie</option>
                    <option value="Intervals">Intervals</option>
                    <option value="Bit Manipulation">Bit Manipulation</option>
                    <option value="Math">Math</option>
                  </select>
                </div>
                <div class="form-group">
                  <label class="form-label">Difficulty *</label>
                  <select class="form-select" [(ngModel)]="formData.difficulty" name="difficulty" required>
                    <option value="">Select Difficulty</option>
                    <option value="Easy">Easy</option>
                    <option value="Medium">Medium</option>
                    <option value="Hard">Hard</option>
                  </select>
                </div>
              </div>
              <div class="grid grid-2">
                <div class="form-group">
                  <label class="form-label">Platform</label>
                  <select class="form-select" [(ngModel)]="formData.platform" name="platform">
                    <option value="">Select Platform</option>
                    <option value="LeetCode">LeetCode</option>
                    <option value="HackerRank">HackerRank</option>
                    <option value="CodeForces">CodeForces</option>
                    <option value="GeeksForGeeks">GeeksForGeeks</option>
                    <option value="Other">Other</option>
                  </select>
                </div>
                <div class="form-group">
                  <label class="form-label">Status</label>
                  <select class="form-select" [(ngModel)]="formData.status" name="status">
                    <option value="NotStarted">Not Started</option>
                    <option value="InProgress">In Progress</option>
                    <option value="Solved">Solved</option>
                    <option value="NeedsReview">Needs Review</option>
                  </select>
                </div>
              </div>
              <div class="form-group">
                <label class="form-label">Problem URL</label>
                <input type="url" class="form-input" [(ngModel)]="formData.problemUrl" name="problemUrl">
              </div>
              <div class="grid grid-2">
                <div class="form-group">
                  <label class="form-label">Time Complexity</label>
                  <input type="text" class="form-input" [(ngModel)]="formData.timeComplexity" name="timeComplexity" placeholder="e.g., O(n)">
                </div>
                <div class="form-group">
                  <label class="form-label">Space Complexity</label>
                  <input type="text" class="form-input" [(ngModel)]="formData.spaceComplexity" name="spaceComplexity" placeholder="e.g., O(1)">
                </div>
              </div>
              <div class="form-group">
                <label class="form-label">Solution Approach</label>
                <textarea class="form-textarea" [(ngModel)]="formData.solutionApproach" name="solutionApproach" rows="2"></textarea>
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
                <button type="submit" class="btn btn-primary">{{ editingProblem ? 'Update' : 'Add' }} Problem</button>
              </div>
            </form>
          </div>
        </div>
      }

      <!-- Attempt Modal -->
      @if (showAttemptModal) {
        <div class="modal-backdrop" (click)="closeAttemptModal()">
          <div class="modal" (click)="$event.stopPropagation()">
            <h2 class="modal-title">Record Attempt: {{ selectedProblem?.title }}</h2>
            <form (ngSubmit)="saveAttempt()">
              <div class="form-group">
                <label class="form-label">Time Taken (minutes) *</label>
                <input type="number" class="form-input" [(ngModel)]="attemptData.timeTakenMinutes" name="time" required min="1">
              </div>
              <div class="form-group">
                <label class="form-label">Status *</label>
                <select class="form-select" [(ngModel)]="attemptData.status" name="status" required>
                  <option value="Solved">Solved</option>
                  <option value="InProgress">Still Working</option>
                  <option value="NeedsReview">Need to Review</option>
                </select>
              </div>
              <div class="form-group">
                <label style="display: flex; align-items: center; gap: 0.5rem; cursor: pointer;">
                  <input type="checkbox" [(ngModel)]="attemptData.solvedOptimally" name="optimal">
                  <span>Solved with optimal solution</span>
                </label>
              </div>
              <div class="form-group">
                <label class="form-label">Notes</label>
                <textarea class="form-textarea" [(ngModel)]="attemptData.notes" name="notes" rows="3" placeholder="What did you learn? Any tricks to remember?"></textarea>
              </div>
              <div style="display: flex; gap: 1rem; justify-content: flex-end; margin-top: 1.5rem;">
                <button type="button" class="btn btn-secondary" (click)="closeAttemptModal()">Cancel</button>
                <button type="submit" class="btn btn-primary">Save Attempt</button>
              </div>
            </form>
          </div>
        </div>
      }
    </div>
  `
})
export class DsaComponent implements OnInit {
  problems: DSAProblem[] = [];
  allProblems: DSAProblem[] = [];
  categories: string[] = [];
  showModal = false;
  showAttemptModal = false;
  editingProblem: DSAProblem | null = null;
  selectedProblem: DSAProblem | null = null;
  showFavoritesOnly = false;

  filters = { category: '', difficulty: '', status: '' };

  formData: Partial<DSAProblem> = this.getEmptyForm();
  attemptData = { timeTakenMinutes: 30, solvedOptimally: false, status: 'Solved', notes: '' };

  constructor(private api: ApiService) {}

  ngOnInit() {
    this.loadProblems();
    this.loadCategories();
  }

  loadProblems() {
    this.api.getDSAProblems(this.filters).subscribe(problems => {
      this.allProblems = problems;
      if (this.showFavoritesOnly) {
        this.problems = problems.filter(p => p.isFavorite);
      } else {
        this.problems = problems;
      }
    });
  }

  loadCategories() {
    this.api.getDSACategories().subscribe(cats => this.categories = cats);
  }

  getSolvedCount(): number {
    return this.allProblems.filter(p => p.status === 'Solved').length;
  }

  getFavoriteCount(): number {
    return this.allProblems.filter(p => p.isFavorite).length;
  }

  getNeedsReviewCount(): number {
    return this.allProblems.filter(p => p.status === 'NeedsReview').length;
  }

  getEmptyForm(): Partial<DSAProblem> {
    return {
      title: '', category: '', difficulty: '', platform: '', problemUrl: '',
      status: 'NotStarted', timeTakenMinutes: 0, solvedOptimally: false,
      notes: '', solutionApproach: '', timeComplexity: '', spaceComplexity: '',
      attemptCount: 1, tags: [], isFavorite: false
    };
  }

  openModal() {
    this.editingProblem = null;
    this.formData = this.getEmptyForm();
    this.showModal = true;
  }

  editProblem(problem: DSAProblem) {
    this.editingProblem = problem;
    this.formData = { ...problem };
    this.showModal = true;
  }

  closeModal() {
    this.showModal = false;
    this.editingProblem = null;
  }

  saveProblem() {
    if (!this.formData.title || !this.formData.category || !this.formData.difficulty) return;

    const problem = this.formData as DSAProblem;
    if (this.editingProblem) {
      this.api.updateDSAProblem(this.editingProblem.id!, problem).subscribe(() => {
        this.loadProblems();
        this.closeModal();
      });
    } else {
      this.api.createDSAProblem(problem).subscribe(() => {
        this.loadProblems();
        this.loadCategories();
        this.closeModal();
      });
    }
  }

  deleteProblem(id: number) {
    if (confirm('Are you sure you want to delete this problem?')) {
      this.api.deleteDSAProblem(id).subscribe(() => this.loadProblems());
    }
  }

  toggleFavorite(problem: DSAProblem) {
    this.api.toggleDSAFavorite(problem.id!).subscribe(updated => {
      problem.isFavorite = updated.isFavorite;
      if (this.showFavoritesOnly && !updated.isFavorite) {
        this.problems = this.problems.filter(p => p.id !== problem.id);
      }
    });
  }

  seedProblems() {
    this.api.seedDSAProblems().subscribe({
      next: (res) => {
        alert(res.message);
        this.loadProblems();
        this.loadCategories();
      },
      error: (err) => {
        alert(err.error || 'Failed to seed problems');
      }
    });
  }

  openAttemptModal(problem: DSAProblem) {
    this.selectedProblem = problem;
    this.attemptData = { timeTakenMinutes: 30, solvedOptimally: false, status: 'Solved', notes: '' };
    this.showAttemptModal = true;
  }

  closeAttemptModal() {
    this.showAttemptModal = false;
    this.selectedProblem = null;
  }

  saveAttempt() {
    if (!this.selectedProblem) return;
    this.api.recordAttempt(this.selectedProblem.id!, this.attemptData).subscribe(() => {
      this.loadProblems();
      this.closeAttemptModal();
    });
  }
}

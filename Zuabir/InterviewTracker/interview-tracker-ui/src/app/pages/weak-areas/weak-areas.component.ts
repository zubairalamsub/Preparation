import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ApiService, WeakArea } from '../../services/api.service';

@Component({
  selector: 'app-weak-areas',
  standalone: true,
  imports: [CommonModule, FormsModule],
  template: `
    <div>
      <div style="display: flex; justify-content: space-between; align-items: center; margin-bottom: 2rem;">
        <h1 style="font-size: 1.75rem; font-weight: 700;">Weak Areas</h1>
        <button class="btn btn-primary" (click)="openModal()">+ Add Weak Area</button>
      </div>

      <!-- Tabs -->
      <div style="display: flex; gap: 0.5rem; margin-bottom: 1.5rem;">
        <button class="btn" [class.btn-primary]="showResolved === false" [class.btn-secondary]="showResolved !== false" (click)="showResolved = false; loadWeakAreas()">
          Active ({{ activeCount }})
        </button>
        <button class="btn" [class.btn-primary]="showResolved === true" [class.btn-secondary]="showResolved !== true" (click)="showResolved = true; loadWeakAreas()">
          Resolved ({{ resolvedCount }})
        </button>
      </div>

      <!-- Weak Areas List -->
      @if (weakAreas.length === 0) {
        <div class="card">
          <div class="empty-state">
            <div class="empty-state-icon">{{ showResolved ? '&#9989;' : '&#128161;' }}</div>
            <div>{{ showResolved ? 'No resolved weak areas yet' : 'No active weak areas. Great job!' }}</div>
          </div>
        </div>
      } @else {
        <div style="display: flex; flex-direction: column; gap: 1rem;">
          @for (area of weakAreas; track area.id) {
            <div class="card" [style.opacity]="area.isResolved ? 0.7 : 1">
              <div style="display: flex; justify-content: space-between; align-items: start;">
                <div style="flex: 1;">
                  <div style="display: flex; align-items: center; gap: 1rem; margin-bottom: 0.5rem;">
                    <h3 style="font-weight: 600;">{{ area.area }}</h3>
                    <span class="badge" [class.badge-weak]="area.severity === 'High'" [class.badge-average]="area.severity === 'Medium'" [class.badge-strong]="area.severity === 'Low'">
                      {{ area.severity }}
                    </span>
                    <span class="badge" style="background: var(--bg-hover);">{{ area.category }}</span>
                  </div>

                  @if (area.description) {
                    <div style="font-size: 0.875rem; color: var(--text-secondary); margin-bottom: 0.75rem;">
                      {{ area.description }}
                    </div>
                  }

                  @if (area.improvementPlan) {
                    <div style="margin-bottom: 0.75rem;">
                      <div style="font-size: 0.75rem; font-weight: 600; color: var(--primary); margin-bottom: 0.25rem;">Improvement Plan</div>
                      <div style="font-size: 0.875rem;">{{ area.improvementPlan }}</div>
                    </div>
                  }

                  <div style="font-size: 0.75rem; color: var(--text-secondary);">
                    @if (area.isResolved) {
                      Resolved on {{ area.resolvedAt | date:'mediumDate' }}
                    } @else {
                      Identified {{ getDaysAgo(area.identifiedAt!) }} days ago
                    }
                  </div>
                </div>

                <div style="display: flex; gap: 0.5rem;">
                  @if (!area.isResolved) {
                    <button class="btn btn-success" (click)="resolveArea(area.id!)">Mark Resolved</button>
                  }
                  <button class="btn btn-secondary" (click)="editArea(area)">Edit</button>
                  <button class="btn btn-danger" (click)="deleteArea(area.id!)">Delete</button>
                </div>
              </div>
            </div>
          }
        </div>
      }

      <!-- Add/Edit Modal -->
      @if (showModal) {
        <div class="modal-backdrop" (click)="closeModal()">
          <div class="modal" (click)="$event.stopPropagation()">
            <h2 class="modal-title">{{ editingArea ? 'Edit' : 'Add' }} Weak Area</h2>
            <form (ngSubmit)="saveArea()">
              <div class="form-group">
                <label class="form-label">Area *</label>
                <input type="text" class="form-input" [(ngModel)]="formData.area" name="area" required placeholder="e.g., Dynamic Programming, System Design Basics">
              </div>
              <div class="grid grid-2">
                <div class="form-group">
                  <label class="form-label">Category *</label>
                  <select class="form-select" [(ngModel)]="formData.category" name="category" required>
                    <option value="">Select Category</option>
                    <option value="DSA">DSA</option>
                    <option value="SystemDesign">System Design</option>
                    <option value="Behavioral">Behavioral</option>
                    <option value="Communication">Communication</option>
                    <option value="Other">Other</option>
                  </select>
                </div>
                <div class="form-group">
                  <label class="form-label">Severity *</label>
                  <select class="form-select" [(ngModel)]="formData.severity" name="severity" required>
                    <option value="Low">Low - Minor improvement needed</option>
                    <option value="Medium">Medium - Needs attention</option>
                    <option value="High">High - Critical to address</option>
                  </select>
                </div>
              </div>
              <div class="form-group">
                <label class="form-label">Description</label>
                <textarea class="form-textarea" [(ngModel)]="formData.description" name="description" rows="2" placeholder="Describe the weakness in detail..."></textarea>
              </div>
              <div class="form-group">
                <label class="form-label">Improvement Plan</label>
                <textarea class="form-textarea" [(ngModel)]="formData.improvementPlan" name="plan" rows="3" placeholder="What steps will you take to improve?"></textarea>
              </div>
              <div style="display: flex; gap: 1rem; justify-content: flex-end; margin-top: 1.5rem;">
                <button type="button" class="btn btn-secondary" (click)="closeModal()">Cancel</button>
                <button type="submit" class="btn btn-primary">{{ editingArea ? 'Update' : 'Add' }} Weak Area</button>
              </div>
            </form>
          </div>
        </div>
      }
    </div>
  `
})
export class WeakAreasComponent implements OnInit {
  weakAreas: WeakArea[] = [];
  showModal = false;
  editingArea: WeakArea | null = null;
  showResolved = false;
  activeCount = 0;
  resolvedCount = 0;

  formData: Partial<WeakArea> = this.getEmptyForm();

  constructor(private api: ApiService) {}

  ngOnInit() {
    this.loadWeakAreas();
    this.loadCounts();
  }

  loadWeakAreas() {
    this.api.getWeakAreas(this.showResolved).subscribe(areas => this.weakAreas = areas);
  }

  loadCounts() {
    this.api.getWeakAreas(false).subscribe(areas => this.activeCount = areas.length);
    this.api.getWeakAreas(true).subscribe(areas => this.resolvedCount = areas.length);
  }

  getEmptyForm(): Partial<WeakArea> {
    return {
      area: '', category: '', severity: 'Medium',
      description: '', improvementPlan: '', isResolved: false
    };
  }

  getDaysAgo(date: string): number {
    return Math.floor((Date.now() - new Date(date).getTime()) / (1000 * 60 * 60 * 24));
  }

  openModal() {
    this.editingArea = null;
    this.formData = this.getEmptyForm();
    this.showModal = true;
  }

  editArea(area: WeakArea) {
    this.editingArea = area;
    this.formData = { ...area };
    this.showModal = true;
  }

  closeModal() {
    this.showModal = false;
    this.editingArea = null;
  }

  saveArea() {
    if (!this.formData.area || !this.formData.category || !this.formData.severity) return;

    const area = this.formData as WeakArea;
    if (this.editingArea) {
      // For edit, we'd need an update endpoint - using create for now
      this.api.createWeakArea(area).subscribe(() => {
        this.loadWeakAreas();
        this.loadCounts();
        this.closeModal();
      });
    } else {
      this.api.createWeakArea(area).subscribe(() => {
        this.loadWeakAreas();
        this.loadCounts();
        this.closeModal();
      });
    }
  }

  resolveArea(id: number) {
    this.api.resolveWeakArea(id).subscribe(() => {
      this.loadWeakAreas();
      this.loadCounts();
    });
  }

  deleteArea(id: number) {
    if (confirm('Are you sure you want to delete this weak area?')) {
      this.api.deleteWeakArea(id).subscribe(() => {
        this.loadWeakAreas();
        this.loadCounts();
      });
    }
  }
}

import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ApiService, MockInterview } from '../../services/api.service';

@Component({
  selector: 'app-interviews',
  standalone: true,
  imports: [CommonModule, FormsModule],
  template: `
    <div>
      <div style="display: flex; justify-content: space-between; align-items: center; margin-bottom: 2rem;">
        <h1 style="font-size: 1.75rem; font-weight: 700;">Mock Interviews</h1>
        <button class="btn btn-primary" (click)="openModal()">+ Add Interview</button>
      </div>

      <!-- Filters -->
      <div class="card" style="margin-bottom: 1.5rem;">
        <div style="display: flex; gap: 1rem; flex-wrap: wrap;">
          <select class="form-select" style="width: auto;" [(ngModel)]="filters.type" (change)="loadInterviews()">
            <option value="">All Types</option>
            <option value="DSA">DSA</option>
            <option value="SystemDesign">System Design</option>
            <option value="Behavioral">Behavioral</option>
          </select>
          <input type="text" class="form-input" style="width: auto;" placeholder="Search company..." [(ngModel)]="filters.company" (input)="loadInterviews()">
        </div>
      </div>

      <!-- Interviews List -->
      @if (interviews.length === 0) {
        <div class="card">
          <div class="empty-state">
            <div class="empty-state-icon">&#127942;</div>
            <div>No interviews recorded yet. Add your first mock interview!</div>
          </div>
        </div>
      } @else {
        <div style="display: flex; flex-direction: column; gap: 1rem;">
          @for (interview of interviews; track interview.id) {
            <div class="card">
              <div style="display: flex; justify-content: space-between; align-items: start;">
                <div>
                  <div style="display: flex; align-items: center; gap: 1rem; margin-bottom: 0.5rem;">
                    <h3 style="font-weight: 600;">{{ interview.company }}</h3>
                    <span class="badge" [class.badge-solved]="interview.type === 'DSA'" [class.badge-average]="interview.type === 'SystemDesign'" [class.badge-pending]="interview.type === 'Behavioral'">
                      {{ interview.type }}
                    </span>
                    @if (interview.passed) {
                      <span class="badge badge-solved">Passed</span>
                    } @else {
                      <span class="badge badge-weak">Failed</span>
                    }
                  </div>
                  <div style="font-size: 0.875rem; color: var(--text-secondary);">
                    {{ interview.interviewDate | date:'mediumDate' }} &bull; {{ interview.durationMinutes }} minutes
                  </div>
                </div>
                <div style="text-align: right;">
                  <div style="font-size: 2rem; font-weight: 700; color: var(--primary);">{{ interview.overallScore }}/10</div>
                  <div style="font-size: 0.75rem; color: var(--text-secondary);">Overall Score</div>
                </div>
              </div>

              <div class="grid grid-3" style="margin-top: 1rem; margin-bottom: 1rem;">
                <div style="text-align: center; padding: 0.75rem; background: var(--bg-dark); border-radius: 8px;">
                  <div style="font-size: 1.25rem; font-weight: 600;">{{ interview.communicationScore }}/10</div>
                  <div style="font-size: 0.75rem; color: var(--text-secondary);">Communication</div>
                </div>
                <div style="text-align: center; padding: 0.75rem; background: var(--bg-dark); border-radius: 8px;">
                  <div style="font-size: 1.25rem; font-weight: 600;">{{ interview.problemSolvingScore }}/10</div>
                  <div style="font-size: 0.75rem; color: var(--text-secondary);">Problem Solving</div>
                </div>
                <div style="text-align: center; padding: 0.75rem; background: var(--bg-dark); border-radius: 8px;">
                  <div style="font-size: 1.25rem; font-weight: 600;">{{ interview.technicalScore }}/10</div>
                  <div style="font-size: 0.75rem; color: var(--text-secondary);">Technical</div>
                </div>
              </div>

              @if (interview.feedback) {
                <div style="margin-bottom: 1rem;">
                  <div style="font-size: 0.75rem; font-weight: 600; color: var(--text-secondary); margin-bottom: 0.25rem;">Feedback</div>
                  <div style="font-size: 0.875rem;">{{ interview.feedback }}</div>
                </div>
              }

              <div class="grid grid-2" style="margin-bottom: 1rem;">
                @if (interview.strengths) {
                  <div>
                    <div style="font-size: 0.75rem; font-weight: 600; color: #10b981; margin-bottom: 0.25rem;">Strengths</div>
                    <div style="font-size: 0.875rem;">{{ interview.strengths }}</div>
                  </div>
                }
                @if (interview.areasToImprove) {
                  <div>
                    <div style="font-size: 0.75rem; font-weight: 600; color: #f59e0b; margin-bottom: 0.25rem;">Areas to Improve</div>
                    <div style="font-size: 0.875rem;">{{ interview.areasToImprove }}</div>
                  </div>
                }
              </div>

              <div style="display: flex; gap: 0.5rem; justify-content: flex-end;">
                <button class="btn btn-secondary" (click)="editInterview(interview)">Edit</button>
                <button class="btn btn-danger" (click)="deleteInterview(interview.id!)">Delete</button>
              </div>
            </div>
          }
        </div>
      }

      <!-- Add/Edit Modal -->
      @if (showModal) {
        <div class="modal-backdrop" (click)="closeModal()">
          <div class="modal" style="max-width: 600px;" (click)="$event.stopPropagation()">
            <h2 class="modal-title">{{ editingInterview ? 'Edit' : 'Add' }} Mock Interview</h2>
            <form (ngSubmit)="saveInterview()">
              <div class="grid grid-2">
                <div class="form-group">
                  <label class="form-label">Company *</label>
                  <input type="text" class="form-input" [(ngModel)]="formData.company" name="company" required placeholder="e.g., Google, Mock Practice">
                </div>
                <div class="form-group">
                  <label class="form-label">Type *</label>
                  <select class="form-select" [(ngModel)]="formData.type" name="type" required>
                    <option value="">Select Type</option>
                    <option value="DSA">DSA</option>
                    <option value="SystemDesign">System Design</option>
                    <option value="Behavioral">Behavioral</option>
                  </select>
                </div>
              </div>
              <div class="grid grid-2">
                <div class="form-group">
                  <label class="form-label">Interview Date *</label>
                  <input type="date" class="form-input" [(ngModel)]="formData.interviewDate" name="date" required>
                </div>
                <div class="form-group">
                  <label class="form-label">Duration (minutes)</label>
                  <input type="number" class="form-input" [(ngModel)]="formData.durationMinutes" name="duration" min="15">
                </div>
              </div>

              <div style="margin-bottom: 1rem;">
                <label class="form-label">Scores (1-10)</label>
                <div class="grid grid-4" style="gap: 0.75rem;">
                  <div class="form-group" style="margin-bottom: 0;">
                    <label style="font-size: 0.75rem; color: var(--text-secondary);">Overall *</label>
                    <input type="number" class="form-input" [(ngModel)]="formData.overallScore" name="overall" min="1" max="10" required>
                  </div>
                  <div class="form-group" style="margin-bottom: 0;">
                    <label style="font-size: 0.75rem; color: var(--text-secondary);">Communication</label>
                    <input type="number" class="form-input" [(ngModel)]="formData.communicationScore" name="comm" min="1" max="10">
                  </div>
                  <div class="form-group" style="margin-bottom: 0;">
                    <label style="font-size: 0.75rem; color: var(--text-secondary);">Problem Solving</label>
                    <input type="number" class="form-input" [(ngModel)]="formData.problemSolvingScore" name="problem" min="1" max="10">
                  </div>
                  <div class="form-group" style="margin-bottom: 0;">
                    <label style="font-size: 0.75rem; color: var(--text-secondary);">Technical</label>
                    <input type="number" class="form-input" [(ngModel)]="formData.technicalScore" name="tech" min="1" max="10">
                  </div>
                </div>
              </div>

              <div class="form-group">
                <label style="display: flex; align-items: center; gap: 0.5rem; cursor: pointer;">
                  <input type="checkbox" [(ngModel)]="formData.passed" name="passed">
                  <span>Passed this interview</span>
                </label>
              </div>

              <div class="form-group">
                <label class="form-label">Questions Asked</label>
                <textarea class="form-textarea" [(ngModel)]="formData.questionsAsked" name="questions" rows="2" placeholder="List the questions you were asked..."></textarea>
              </div>
              <div class="form-group">
                <label class="form-label">Feedback</label>
                <textarea class="form-textarea" [(ngModel)]="formData.feedback" name="feedback" rows="2"></textarea>
              </div>
              <div class="grid grid-2">
                <div class="form-group">
                  <label class="form-label">Strengths</label>
                  <textarea class="form-textarea" [(ngModel)]="formData.strengths" name="strengths" rows="2"></textarea>
                </div>
                <div class="form-group">
                  <label class="form-label">Areas to Improve</label>
                  <textarea class="form-textarea" [(ngModel)]="formData.areasToImprove" name="improve" rows="2"></textarea>
                </div>
              </div>

              <div style="display: flex; gap: 1rem; justify-content: flex-end; margin-top: 1.5rem;">
                <button type="button" class="btn btn-secondary" (click)="closeModal()">Cancel</button>
                <button type="submit" class="btn btn-primary">{{ editingInterview ? 'Update' : 'Add' }} Interview</button>
              </div>
            </form>
          </div>
        </div>
      }
    </div>
  `
})
export class InterviewsComponent implements OnInit {
  interviews: MockInterview[] = [];
  showModal = false;
  editingInterview: MockInterview | null = null;

  filters = { type: '', company: '' };

  formData: Partial<MockInterview> = this.getEmptyForm();

  constructor(private api: ApiService) {}

  ngOnInit() {
    this.loadInterviews();
  }

  loadInterviews() {
    this.api.getInterviews(this.filters).subscribe(interviews => this.interviews = interviews);
  }

  getEmptyForm(): Partial<MockInterview> {
    return {
      type: '', company: '', interviewDate: new Date().toISOString().split('T')[0],
      durationMinutes: 60, overallScore: 5, communicationScore: 5,
      problemSolvingScore: 5, technicalScore: 5, feedback: '',
      strengths: '', areasToImprove: '', questionsAsked: '', passed: false
    };
  }

  openModal() {
    this.editingInterview = null;
    this.formData = this.getEmptyForm();
    this.showModal = true;
  }

  editInterview(interview: MockInterview) {
    this.editingInterview = interview;
    this.formData = {
      ...interview,
      interviewDate: interview.interviewDate.split('T')[0]
    };
    this.showModal = true;
  }

  closeModal() {
    this.showModal = false;
    this.editingInterview = null;
  }

  saveInterview() {
    if (!this.formData.company || !this.formData.type || !this.formData.interviewDate) return;

    const interview = this.formData as MockInterview;
    if (this.editingInterview) {
      this.api.updateInterview(this.editingInterview.id!, interview).subscribe(() => {
        this.loadInterviews();
        this.closeModal();
      });
    } else {
      this.api.createInterview(interview).subscribe(() => {
        this.loadInterviews();
        this.closeModal();
      });
    }
  }

  deleteInterview(id: number) {
    if (confirm('Are you sure you want to delete this interview record?')) {
      this.api.deleteInterview(id).subscribe(() => this.loadInterviews());
    }
  }
}

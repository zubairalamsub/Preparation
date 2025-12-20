import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { ApiService, DashboardStats, DSAProblem, WeakArea } from '../../services/api.service';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, RouterLink],
  template: `
    <div class="dashboard">
      <h1 style="font-size: 1.75rem; font-weight: 700; margin-bottom: 2rem;">Dashboard</h1>

      <!-- Stats Grid -->
      <div class="grid grid-4" style="margin-bottom: 2rem;">
        <div class="stat-card">
          <div style="display: flex; justify-content: space-between; align-items: start;">
            <div>
              <div class="stat-value">{{ stats?.solvedDSAProblems || 0 }}/{{ stats?.totalDSAProblems || 0 }}</div>
              <div class="stat-label">DSA Problems Solved</div>
            </div>
            <div class="stat-icon" style="background: rgba(99, 102, 241, 0.2); color: #818cf8;">&#128187;</div>
          </div>
          <div class="progress-bar" style="margin-top: 1rem;">
            <div class="progress-fill" [style.width.%]="stats?.dsaCompletionRate || 0"></div>
          </div>
        </div>

        <div class="stat-card">
          <div style="display: flex; justify-content: space-between; align-items: start;">
            <div>
              <div class="stat-value">{{ stats?.masteredTopics || 0 }}/{{ stats?.totalSystemDesignTopics || 0 }}</div>
              <div class="stat-label">System Design Mastered</div>
            </div>
            <div class="stat-icon" style="background: rgba(16, 185, 129, 0.2); color: #34d399;">&#127959;</div>
          </div>
          <div class="progress-bar" style="margin-top: 1rem;">
            <div class="progress-fill" [style.width.%]="stats?.systemDesignProgress || 0" style="background: #10b981;"></div>
          </div>
        </div>

        <div class="stat-card">
          <div style="display: flex; justify-content: space-between; align-items: start;">
            <div>
              <div class="stat-value">{{ stats?.passedInterviews || 0 }}/{{ stats?.totalMockInterviews || 0 }}</div>
              <div class="stat-label">Mock Interviews Passed</div>
            </div>
            <div class="stat-icon" style="background: rgba(245, 158, 11, 0.2); color: #fbbf24;">&#127942;</div>
          </div>
          <div style="margin-top: 0.5rem; font-size: 0.875rem; color: var(--text-secondary);">
            Avg Score: {{ (stats?.averageInterviewScore || 0).toFixed(1) }}/10
          </div>
        </div>

        <div class="stat-card">
          <div style="display: flex; justify-content: space-between; align-items: start;">
            <div>
              <div class="stat-value">{{ stats?.activeWeakAreas || 0 }}</div>
              <div class="stat-label">Active Weak Areas</div>
            </div>
            <div class="stat-icon" style="background: rgba(239, 68, 68, 0.2); color: #f87171;">&#9888;</div>
          </div>
          <div style="margin-top: 0.5rem; font-size: 0.875rem; color: var(--text-secondary);">
            {{ stats?.totalStudyHours || 0 }} hours studied
          </div>
        </div>
      </div>

      <div class="grid grid-2">
        <!-- Needs Review -->
        <div class="card">
          <div class="card-header">
            <h2 class="card-title">Needs Review</h2>
            <a routerLink="/dsa" class="btn btn-secondary">View All</a>
          </div>
          @if (needsReview.length === 0) {
            <div class="empty-state">
              <div>No problems need review right now</div>
            </div>
          } @else {
            <div style="display: flex; flex-direction: column; gap: 0.75rem;">
              @for (problem of needsReview.slice(0, 5); track problem.id) {
                <div style="display: flex; justify-content: space-between; align-items: center; padding: 0.75rem; background: var(--bg-dark); border-radius: 8px;">
                  <div>
                    <div style="font-weight: 500;">{{ problem.title }}</div>
                    <div style="font-size: 0.875rem; color: var(--text-secondary);">{{ problem.category }}</div>
                  </div>
                  <span class="badge badge-pending">Review Due</span>
                </div>
              }
            </div>
          }
        </div>

        <!-- Active Weak Areas -->
        <div class="card">
          <div class="card-header">
            <h2 class="card-title">Active Weak Areas</h2>
            <a routerLink="/weak-areas" class="btn btn-secondary">View All</a>
          </div>
          @if (weakAreas.length === 0) {
            <div class="empty-state">
              <div>No weak areas identified</div>
            </div>
          } @else {
            <div style="display: flex; flex-direction: column; gap: 0.75rem;">
              @for (area of weakAreas.slice(0, 5); track area.id) {
                <div style="display: flex; justify-content: space-between; align-items: center; padding: 0.75rem; background: var(--bg-dark); border-radius: 8px;">
                  <div>
                    <div style="font-weight: 500;">{{ area.area }}</div>
                    <div style="font-size: 0.875rem; color: var(--text-secondary);">{{ area.category }}</div>
                  </div>
                  <span class="badge" [class.badge-weak]="area.severity === 'High'" [class.badge-average]="area.severity === 'Medium'" [class.badge-strong]="area.severity === 'Low'">
                    {{ area.severity }}
                  </span>
                </div>
              }
            </div>
          }
        </div>
      </div>
    </div>
  `
})
export class DashboardComponent implements OnInit {
  stats: DashboardStats | null = null;
  needsReview: DSAProblem[] = [];
  weakAreas: WeakArea[] = [];

  constructor(private api: ApiService) {}

  ngOnInit() {
    this.loadData();
  }

  loadData() {
    this.api.getDashboardStats().subscribe(stats => this.stats = stats);
    this.api.getNeedsReview().subscribe(problems => this.needsReview = problems);
    this.api.getWeakAreas(false).subscribe(areas => this.weakAreas = areas);
  }
}

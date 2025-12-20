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
      <h1 class="page-title">📊 Dashboard</h1>

      <!-- Stats Grid -->
      <div class="stats-grid">
        <div class="stat-card">
          <div class="stat-row">
            <div class="stat-info">
              <div class="stat-value">{{ stats?.solvedDSAProblems || 0 }}/{{ stats?.totalDSAProblems || 0 }}</div>
              <div class="stat-label">DSA Solved</div>
            </div>
            <div class="stat-icon icon-primary">🧮</div>
          </div>
          <div class="progress-bar"><div class="progress-fill" [style.width.%]="stats?.dsaCompletionRate || 0"></div></div>
        </div>

        <div class="stat-card">
          <div class="stat-row">
            <div class="stat-info">
              <div class="stat-value">{{ stats?.masteredTopics || 0 }}/{{ stats?.totalSystemDesignTopics || 0 }}</div>
              <div class="stat-label">System Design</div>
            </div>
            <div class="stat-icon icon-success">🏗️</div>
          </div>
          <div class="progress-bar"><div class="progress-fill fill-success" [style.width.%]="stats?.systemDesignProgress || 0"></div></div>
        </div>

        <div class="stat-card">
          <div class="stat-row">
            <div class="stat-info">
              <div class="stat-value">{{ stats?.passedInterviews || 0 }}/{{ stats?.totalMockInterviews || 0 }}</div>
              <div class="stat-label">Interviews</div>
            </div>
            <div class="stat-icon icon-warning">🎤</div>
          </div>
          <div class="stat-extra">Avg: {{ (stats?.averageInterviewScore || 0).toFixed(1) }}/10</div>
        </div>

        <div class="stat-card">
          <div class="stat-row">
            <div class="stat-info">
              <div class="stat-value">{{ stats?.activeWeakAreas || 0 }}</div>
              <div class="stat-label">Weak Areas</div>
            </div>
            <div class="stat-icon icon-danger">⚠️</div>
          </div>
          <div class="stat-extra">{{ stats?.totalStudyHours || 0 }}h studied</div>
        </div>
      </div>

      <div class="cards-grid">
        <!-- Needs Review -->
        <div class="card">
          <div class="card-header">
            <h2 class="card-title">📝 Needs Review</h2>
            <a routerLink="/dsa" class="btn btn-secondary btn-sm">View All</a>
          </div>
          @if (needsReview.length === 0) {
            <div class="empty-state"><div>No problems need review</div></div>
          } @else {
            <div class="list-container">
              @for (problem of needsReview.slice(0, 5); track problem.id) {
                <div class="list-item">
                  <div class="list-item-info">
                    <div class="list-item-title">{{ problem.title }}</div>
                    <div class="list-item-subtitle">{{ problem.category }}</div>
                  </div>
                  <span class="badge badge-pending">Review</span>
                </div>
              }
            </div>
          }
        </div>

        <!-- Active Weak Areas -->
        <div class="card">
          <div class="card-header">
            <h2 class="card-title">⚠️ Weak Areas</h2>
            <a routerLink="/weak-areas" class="btn btn-secondary btn-sm">View All</a>
          </div>
          @if (weakAreas.length === 0) {
            <div class="empty-state"><div>No weak areas identified</div></div>
          } @else {
            <div class="list-container">
              @for (area of weakAreas.slice(0, 5); track area.id) {
                <div class="list-item">
                  <div class="list-item-info">
                    <div class="list-item-title">{{ area.area }}</div>
                    <div class="list-item-subtitle">{{ area.category }}</div>
                  </div>
                  <span class="badge" [class.badge-weak]="area.severity === 'High'" [class.badge-average]="area.severity === 'Medium'" [class.badge-strong]="area.severity === 'Low'">{{ area.severity }}</span>
                </div>
              }
            </div>
          }
        </div>
      </div>
    </div>
  `,
  styles: [`
    .page-title { font-size: 1.75rem; font-weight: 700; margin-bottom: 1.5rem; }

    .stats-grid { display: grid; grid-template-columns: repeat(4, 1fr); gap: 1rem; margin-bottom: 1.5rem; }
    .stat-card { background: var(--bg-card); border-radius: 12px; padding: 1.25rem; border: 1px solid var(--border); }
    .stat-row { display: flex; justify-content: space-between; align-items: flex-start; margin-bottom: 0.75rem; }
    .stat-info { flex: 1; }
    .stat-value { font-size: 1.75rem; font-weight: 700; line-height: 1.2; }
    .stat-label { font-size: 0.8rem; color: var(--text-secondary); margin-top: 0.25rem; }
    .stat-icon { width: 44px; height: 44px; border-radius: 10px; display: flex; align-items: center; justify-content: center; font-size: 1.25rem; }
    .icon-primary { background: rgba(99, 102, 241, 0.2); }
    .icon-success { background: rgba(16, 185, 129, 0.2); }
    .icon-warning { background: rgba(245, 158, 11, 0.2); }
    .icon-danger { background: rgba(239, 68, 68, 0.2); }
    .stat-extra { font-size: 0.8rem; color: var(--text-secondary); }
    .fill-success { background: #10b981; }

    .cards-grid { display: grid; grid-template-columns: repeat(2, 1fr); gap: 1rem; }
    .card-header { display: flex; justify-content: space-between; align-items: center; margin-bottom: 1rem; flex-wrap: wrap; gap: 0.5rem; }
    .card-title { font-size: 1rem; font-weight: 600; margin: 0; }
    .btn-sm { padding: 0.4rem 0.75rem; font-size: 0.75rem; }

    .list-container { display: flex; flex-direction: column; gap: 0.625rem; }
    .list-item { display: flex; justify-content: space-between; align-items: center; padding: 0.75rem; background: var(--bg-dark); border-radius: 8px; gap: 0.75rem; }
    .list-item-info { flex: 1; min-width: 0; }
    .list-item-title { font-weight: 500; font-size: 0.9rem; white-space: nowrap; overflow: hidden; text-overflow: ellipsis; }
    .list-item-subtitle { font-size: 0.8rem; color: var(--text-secondary); }

    /* Tablet */
    @media (max-width: 1024px) {
      .stats-grid { grid-template-columns: repeat(2, 1fr); }
      .stat-value { font-size: 1.5rem; }
    }

    /* Mobile */
    @media (max-width: 768px) {
      .page-title { font-size: 1.4rem; margin-bottom: 1rem; }
      .stats-grid { gap: 0.75rem; }
      .stat-card { padding: 1rem; }
      .stat-value { font-size: 1.35rem; }
      .stat-icon { width: 40px; height: 40px; font-size: 1.1rem; }

      .cards-grid { grid-template-columns: 1fr; }
      .list-item { padding: 0.625rem; }
      .list-item-title { font-size: 0.85rem; }
    }

    /* Small Mobile */
    @media (max-width: 480px) {
      .stats-grid { grid-template-columns: repeat(2, 1fr); gap: 0.5rem; }
      .stat-card { padding: 0.875rem; }
      .stat-value { font-size: 1.2rem; }
      .stat-label { font-size: 0.7rem; }
      .stat-icon { width: 36px; height: 36px; font-size: 1rem; }
    }
  `]
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

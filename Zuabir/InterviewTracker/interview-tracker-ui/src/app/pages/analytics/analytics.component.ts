import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ApiService, DSAAnalytics, InterviewAnalytics, WeakAreaAnalytics } from '../../services/api.service';

@Component({
  selector: 'app-analytics',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div>
      <h1 style="font-size: 1.75rem; font-weight: 700; margin-bottom: 2rem;">Analytics & Insights</h1>

      <!-- DSA Analytics -->
      <div class="card" style="margin-bottom: 1.5rem;">
        <h2 class="card-title" style="margin-bottom: 1.5rem;">DSA Performance</h2>

        @if (dsaAnalytics) {
          <div class="grid grid-4" style="margin-bottom: 1.5rem;">
            <div style="text-align: center; padding: 1rem; background: var(--bg-dark); border-radius: 8px;">
              <div style="font-size: 1.5rem; font-weight: 700;">{{ dsaAnalytics.averageTimePerProblem.toFixed(0) }}</div>
              <div style="font-size: 0.75rem; color: var(--text-secondary);">Avg Minutes/Problem</div>
            </div>
            <div style="text-align: center; padding: 1rem; background: var(--bg-dark); border-radius: 8px;">
              <div style="font-size: 1.5rem; font-weight: 700;">{{ dsaAnalytics.optimalSolutionRate.toFixed(1) }}%</div>
              <div style="font-size: 0.75rem; color: var(--text-secondary);">Optimal Solutions</div>
            </div>
            <div style="text-align: center; padding: 1rem; background: var(--bg-dark); border-radius: 8px;">
              <div style="font-size: 1.5rem; font-weight: 700;">{{ dsaAnalytics.needsReview.length }}</div>
              <div style="font-size: 0.75rem; color: var(--text-secondary);">Need Review</div>
            </div>
            <div style="text-align: center; padding: 1rem; background: var(--bg-dark); border-radius: 8px;">
              <div style="font-size: 1.5rem; font-weight: 700;">{{ getTotalProblems() }}</div>
              <div style="font-size: 0.75rem; color: var(--text-secondary);">Total Problems</div>
            </div>
          </div>

          <!-- Category Performance -->
          <h3 style="font-weight: 600; margin-bottom: 1rem;">Performance by Category</h3>
          <div style="display: flex; flex-direction: column; gap: 0.75rem;">
            @for (cat of dsaAnalytics.categoryPerformance; track cat.category) {
              <div style="display: flex; align-items: center; gap: 1rem;">
                <div style="width: 150px; font-weight: 500;">{{ cat.category }}</div>
                <div style="flex: 1;">
                  <div class="progress-bar" style="height: 24px; position: relative;">
                    <div class="progress-fill" [style.width.%]="cat.successRate"
                         [style.background]="cat.strengthLevel === 'Strong' ? '#10b981' : cat.strengthLevel === 'Average' ? '#f59e0b' : '#ef4444'">
                    </div>
                    <span style="position: absolute; left: 50%; top: 50%; transform: translate(-50%, -50%); font-size: 0.75rem; font-weight: 600;">
                      {{ cat.solved }}/{{ cat.totalProblems }} ({{ cat.successRate.toFixed(0) }}%)
                    </span>
                  </div>
                </div>
                <span class="badge" [class.badge-strong]="cat.strengthLevel === 'Strong'" [class.badge-average]="cat.strengthLevel === 'Average'" [class.badge-weak]="cat.strengthLevel === 'Weak'">
                  {{ cat.strengthLevel }}
                </span>
              </div>
            }
          </div>

          <!-- Problems by Difficulty -->
          <div class="grid grid-3" style="margin-top: 1.5rem;">
            @for (item of getDifficultyData(); track item.name) {
              <div style="padding: 1rem; background: var(--bg-dark); border-radius: 8px;">
                <div style="display: flex; justify-content: space-between; align-items: center; margin-bottom: 0.5rem;">
                  <span class="badge" [class.badge-easy]="item.name === 'Easy'" [class.badge-medium]="item.name === 'Medium'" [class.badge-hard]="item.name === 'Hard'">
                    {{ item.name }}
                  </span>
                  <span style="font-weight: 600;">{{ item.count }}</span>
                </div>
                <div class="progress-bar">
                  <div class="progress-fill" [style.width.%]="(item.count / getTotalProblems()) * 100"></div>
                </div>
              </div>
            }
          </div>
        } @else {
          <div class="empty-state">Loading DSA analytics...</div>
        }
      </div>

      <!-- Interview Analytics -->
      <div class="card" style="margin-bottom: 1.5rem;">
        <h2 class="card-title" style="margin-bottom: 1.5rem;">Interview Performance</h2>

        @if (interviewAnalytics) {
          <div class="grid grid-4" style="margin-bottom: 1.5rem;">
            <div style="text-align: center; padding: 1rem; background: var(--bg-dark); border-radius: 8px;">
              <div style="font-size: 1.5rem; font-weight: 700; color: #10b981;">{{ interviewAnalytics.overallPassRate.toFixed(0) }}%</div>
              <div style="font-size: 0.75rem; color: var(--text-secondary);">Pass Rate</div>
            </div>
            <div style="text-align: center; padding: 1rem; background: var(--bg-dark); border-radius: 8px;">
              <div style="font-size: 1.5rem; font-weight: 700;">{{ interviewAnalytics.averageCommunicationScore.toFixed(1) }}</div>
              <div style="font-size: 0.75rem; color: var(--text-secondary);">Avg Communication</div>
            </div>
            <div style="text-align: center; padding: 1rem; background: var(--bg-dark); border-radius: 8px;">
              <div style="font-size: 1.5rem; font-weight: 700;">{{ interviewAnalytics.averageProblemSolvingScore.toFixed(1) }}</div>
              <div style="font-size: 0.75rem; color: var(--text-secondary);">Avg Problem Solving</div>
            </div>
            <div style="text-align: center; padding: 1rem; background: var(--bg-dark); border-radius: 8px;">
              <div style="font-size: 1.5rem; font-weight: 700;">{{ interviewAnalytics.averageTechnicalScore.toFixed(1) }}</div>
              <div style="font-size: 0.75rem; color: var(--text-secondary);">Avg Technical</div>
            </div>
          </div>

          <!-- Scores by Type -->
          @if (getInterviewTypes().length > 0) {
            <h3 style="font-weight: 600; margin-bottom: 1rem;">Average Scores by Type</h3>
            <div class="grid grid-3" style="margin-bottom: 1.5rem;">
              @for (type of getInterviewTypes(); track type.type) {
                <div style="padding: 1rem; background: var(--bg-dark); border-radius: 8px; text-align: center;">
                  <div style="font-size: 2rem; font-weight: 700; color: var(--primary);">{{ type.score.toFixed(1) }}</div>
                  <div style="font-size: 0.875rem; color: var(--text-secondary);">{{ type.type }}</div>
                </div>
              }
            </div>
          }

          <!-- Common Weaknesses -->
          @if (interviewAnalytics.commonWeaknesses.length > 0) {
            <h3 style="font-weight: 600; margin-bottom: 1rem;">Common Weaknesses</h3>
            <div style="display: flex; gap: 0.5rem; flex-wrap: wrap;">
              @for (weakness of interviewAnalytics.commonWeaknesses; track weakness) {
                <span class="badge badge-weak">{{ weakness }}</span>
              }
            </div>
          }
        } @else {
          <div class="empty-state">Loading interview analytics...</div>
        }
      </div>

      <!-- Weak Areas Analytics -->
      <div class="card">
        <h2 class="card-title" style="margin-bottom: 1.5rem;">Weak Areas Analysis</h2>

        @if (weakAreaAnalytics) {
          <div class="grid grid-2" style="margin-bottom: 1.5rem;">
            <div style="padding: 1.5rem; background: var(--bg-dark); border-radius: 8px;">
              <div style="font-size: 0.875rem; color: var(--text-secondary); margin-bottom: 0.5rem;">Resolved This Month</div>
              <div style="font-size: 2rem; font-weight: 700; color: #10b981;">{{ weakAreaAnalytics.resolvedThisMonth }}</div>
            </div>
            <div style="padding: 1.5rem; background: var(--bg-dark); border-radius: 8px;">
              <div style="font-size: 0.875rem; color: var(--text-secondary); margin-bottom: 0.5rem;">Active Weak Areas</div>
              <div style="font-size: 2rem; font-weight: 700; color: #ef4444;">{{ weakAreaAnalytics.activeWeakAreas.length }}</div>
            </div>
          </div>

          <!-- Recommended Focus Areas -->
          @if (weakAreaAnalytics.recommendedFocusAreas.length > 0) {
            <div style="margin-bottom: 1.5rem;">
              <h3 style="font-weight: 600; margin-bottom: 1rem;">Recommended Focus Areas</h3>
              <div style="display: flex; gap: 0.5rem; flex-wrap: wrap;">
                @for (area of weakAreaAnalytics.recommendedFocusAreas; track area) {
                  <span style="padding: 0.5rem 1rem; background: rgba(99, 102, 241, 0.2); color: #818cf8; border-radius: 8px; font-weight: 500;">
                    {{ area }}
                  </span>
                }
              </div>
            </div>
          }

          <!-- Active Weak Areas List -->
          @if (weakAreaAnalytics.activeWeakAreas.length > 0) {
            <h3 style="font-weight: 600; margin-bottom: 1rem;">Active Weak Areas</h3>
            <table class="table">
              <thead>
                <tr>
                  <th>Area</th>
                  <th>Category</th>
                  <th>Severity</th>
                  <th>Days Identified</th>
                </tr>
              </thead>
              <tbody>
                @for (area of weakAreaAnalytics.activeWeakAreas; track area.area) {
                  <tr>
                    <td style="font-weight: 500;">{{ area.area }}</td>
                    <td>{{ area.category }}</td>
                    <td>
                      <span class="badge" [class.badge-weak]="area.severity === 'High'" [class.badge-average]="area.severity === 'Medium'" [class.badge-strong]="area.severity === 'Low'">
                        {{ area.severity }}
                      </span>
                    </td>
                    <td>{{ area.daysIdentified }} days</td>
                  </tr>
                }
              </tbody>
            </table>
          }
        } @else {
          <div class="empty-state">Loading weak areas analytics...</div>
        }
      </div>
    </div>
  `
})
export class AnalyticsComponent implements OnInit {
  dsaAnalytics: DSAAnalytics | null = null;
  interviewAnalytics: InterviewAnalytics | null = null;
  weakAreaAnalytics: WeakAreaAnalytics | null = null;

  constructor(private api: ApiService) {}

  ngOnInit() {
    this.loadAnalytics();
  }

  loadAnalytics() {
    this.api.getDSAAnalytics().subscribe(data => this.dsaAnalytics = data);
    this.api.getInterviewAnalytics().subscribe(data => this.interviewAnalytics = data);
    this.api.getWeakAreaAnalytics().subscribe(data => this.weakAreaAnalytics = data);
  }

  getTotalProblems(): number {
    if (!this.dsaAnalytics) return 0;
    return Object.values(this.dsaAnalytics.problemsByDifficulty).reduce((a, b) => a + b, 0);
  }

  getDifficultyData(): { name: string; count: number }[] {
    if (!this.dsaAnalytics) return [];
    return Object.entries(this.dsaAnalytics.problemsByDifficulty).map(([name, count]) => ({ name, count }));
  }

  getInterviewTypes(): { type: string; score: number }[] {
    if (!this.interviewAnalytics) return [];
    return Object.entries(this.interviewAnalytics.averageScoresByType).map(([type, score]) => ({ type, score }));
  }
}

import { Injectable, isDevMode } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

// Use environment variable for production, fallback to localhost for development
const API_URL = isDevMode()
  ? 'http://localhost:5000/api'
  : (window as any).__env?.API_URL || '/api';

export interface DSAProblem {
  id?: number;
  title: string;
  category: string;
  difficulty: string;
  platform: string;
  problemUrl?: string;
  status: string;
  timeTakenMinutes: number;
  solvedOptimally: boolean;
  notes?: string;
  solutionApproach?: string;
  timeComplexity?: string;
  spaceComplexity?: string;
  attemptCount: number;
  createdAt?: string;
  lastAttemptedAt?: string;
  nextReviewDate?: string;
  tags: string[];
  isFavorite: boolean;
  leetCodeNumber?: number;
}

export interface SystemDesignTopic {
  id?: number;
  title: string;
  category: string;
  difficulty: string;
  status: string;
  confidenceLevel: number;
  notes?: string;
  keyConcepts?: string;
  resources?: string;
  createdAt?: string;
  lastReviewedAt?: string;
  tags: string[];
  isFavorite: boolean;
}

export interface MockInterview {
  id?: number;
  type: string;
  company: string;
  interviewDate: string;
  durationMinutes: number;
  overallScore: number;
  communicationScore: number;
  problemSolvingScore: number;
  technicalScore: number;
  feedback?: string;
  strengths?: string;
  areasToImprove?: string;
  questionsAsked?: string;
  passed: boolean;
  createdAt?: string;
}

export interface WeakArea {
  id?: number;
  area: string;
  category: string;
  severity: string;
  description?: string;
  improvementPlan?: string;
  isResolved: boolean;
  identifiedAt?: string;
  resolvedAt?: string;
}

export interface StudySession {
  id?: number;
  type: string;
  topic: string;
  durationMinutes: number;
  productivityScore: number;
  notes?: string;
  sessionDate: string;
}

export interface DashboardStats {
  totalDSAProblems: number;
  solvedDSAProblems: number;
  totalSystemDesignTopics: number;
  masteredTopics: number;
  totalMockInterviews: number;
  passedInterviews: number;
  activeWeakAreas: number;
  totalStudyHours: number;
  averageInterviewScore: number;
  dsaCompletionRate: number;
  systemDesignProgress: number;
}

export interface DSAAnalytics {
  problemsByCategory: Record<string, number>;
  problemsByDifficulty: Record<string, number>;
  problemsByStatus: Record<string, number>;
  categoryPerformance: CategoryPerformance[];
  needsReview: { id: number; title: string; category: string; nextReviewDate: string }[];
  averageTimePerProblem: number;
  optimalSolutionRate: number;
}

export interface CategoryPerformance {
  category: string;
  totalProblems: number;
  solved: number;
  successRate: number;
  averageTime: number;
  strengthLevel: string;
}

export interface InterviewAnalytics {
  averageScoresByType: Record<string, number>;
  scoreTrends: { date: string; score: number; type: string }[];
  overallPassRate: number;
  averageCommunicationScore: number;
  averageProblemSolvingScore: number;
  averageTechnicalScore: number;
  commonWeaknesses: string[];
}

export interface WeakAreaAnalytics {
  activeWeakAreas: { area: string; category: string; severity: string; daysIdentified: number }[];
  weakAreasByCategory: Record<string, number>;
  resolvedThisMonth: number;
  recommendedFocusAreas: string[];
}

// New Interfaces for Additional Learning Sections
export interface AzureTopic {
  id?: number;
  title: string;
  category: string;
  difficulty: string;
  status: string;
  confidenceLevel: number;
  notes?: string;
  keyConcepts?: string;
  lesson?: string;
  codeExample?: string;
  resources?: string;
  azureService?: string;
  createdAt?: string;
  lastReviewedAt?: string;
  tags: string[];
  isFavorite: boolean;
}

export interface OOPTopic {
  id?: number;
  title: string;
  category: string;
  difficulty: string;
  status: string;
  confidenceLevel: number;
  notes?: string;
  keyConcepts?: string;
  lesson?: string;
  codeExample?: string;
  resources?: string;
  createdAt?: string;
  lastReviewedAt?: string;
  tags: string[];
  isFavorite: boolean;
}

export interface CSharpTopic {
  id?: number;
  title: string;
  category: string;
  difficulty: string;
  status: string;
  confidenceLevel: number;
  notes?: string;
  keyConcepts?: string;
  lesson?: string;
  codeExample?: string;
  resources?: string;
  dotNetVersion?: string;
  createdAt?: string;
  lastReviewedAt?: string;
  tags: string[];
  isFavorite: boolean;
}

export interface AspNetCoreTopic {
  id?: number;
  title: string;
  category: string;
  difficulty: string;
  status: string;
  confidenceLevel: number;
  notes?: string;
  keyConcepts?: string;
  lesson?: string;
  codeExample?: string;
  resources?: string;
  createdAt?: string;
  lastReviewedAt?: string;
  tags: string[];
  isFavorite: boolean;
}

export interface SqlServerTopic {
  id?: number;
  title: string;
  category: string;
  difficulty: string;
  status: string;
  confidenceLevel: number;
  notes?: string;
  keyConcepts?: string;
  lesson?: string;
  sqlExample?: string;
  resources?: string;
  createdAt?: string;
  lastReviewedAt?: string;
  tags: string[];
  isFavorite: boolean;
}

export interface DesignPatternTopic {
  id?: number;
  title: string;
  category: string;
  difficulty: string;
  status: string;
  confidenceLevel: number;
  notes?: string;
  keyConcepts?: string;
  lesson?: string;
  codeExample?: string;
  useCases?: string;
  resources?: string;
  createdAt?: string;
  lastReviewedAt?: string;
  tags: string[];
  isFavorite: boolean;
}

@Injectable({ providedIn: 'root' })
export class ApiService {
  constructor(private http: HttpClient) {}

  // DSA Problems
  getDSAProblems(filters?: { category?: string; difficulty?: string; status?: string }): Observable<DSAProblem[]> {
    const params = new URLSearchParams();
    if (filters?.category) params.set('category', filters.category);
    if (filters?.difficulty) params.set('difficulty', filters.difficulty);
    if (filters?.status) params.set('status', filters.status);
    return this.http.get<DSAProblem[]>(`${API_URL}/dsa?${params.toString()}`);
  }

  createDSAProblem(problem: DSAProblem): Observable<DSAProblem> {
    return this.http.post<DSAProblem>(`${API_URL}/dsa`, problem);
  }

  updateDSAProblem(id: number, problem: DSAProblem): Observable<void> {
    return this.http.put<void>(`${API_URL}/dsa/${id}`, problem);
  }

  deleteDSAProblem(id: number): Observable<void> {
    return this.http.delete<void>(`${API_URL}/dsa/${id}`);
  }

  recordAttempt(id: number, attempt: { timeTakenMinutes: number; solvedOptimally: boolean; status: string; notes?: string }): Observable<DSAProblem> {
    return this.http.post<DSAProblem>(`${API_URL}/dsa/${id}/attempt`, attempt);
  }

  getDSACategories(): Observable<string[]> {
    return this.http.get<string[]>(`${API_URL}/dsa/categories`);
  }

  getNeedsReview(): Observable<DSAProblem[]> {
    return this.http.get<DSAProblem[]>(`${API_URL}/dsa/needs-review`);
  }

  toggleDSAFavorite(id: number): Observable<DSAProblem> {
    return this.http.post<DSAProblem>(`${API_URL}/dsa/${id}/favorite`, {});
  }

  seedDSAProblems(): Observable<{ message: string }> {
    return this.http.post<{ message: string }>(`${API_URL}/dsa/seed`, {});
  }

  getDSAFavorites(): Observable<DSAProblem[]> {
    return this.http.get<DSAProblem[]>(`${API_URL}/dsa/favorites`);
  }

  // System Design
  getSystemDesignTopics(filters?: { category?: string; status?: string }): Observable<SystemDesignTopic[]> {
    const params = new URLSearchParams();
    if (filters?.category) params.set('category', filters.category);
    if (filters?.status) params.set('status', filters.status);
    return this.http.get<SystemDesignTopic[]>(`${API_URL}/systemdesign?${params.toString()}`);
  }

  createSystemDesignTopic(topic: SystemDesignTopic): Observable<SystemDesignTopic> {
    return this.http.post<SystemDesignTopic>(`${API_URL}/systemdesign`, topic);
  }

  updateSystemDesignTopic(id: number, topic: SystemDesignTopic): Observable<void> {
    return this.http.put<void>(`${API_URL}/systemdesign/${id}`, topic);
  }

  deleteSystemDesignTopic(id: number): Observable<void> {
    return this.http.delete<void>(`${API_URL}/systemdesign/${id}`);
  }

  toggleSystemDesignFavorite(id: number): Observable<SystemDesignTopic> {
    return this.http.post<SystemDesignTopic>(`${API_URL}/systemdesign/${id}/favorite`, {});
  }

  seedSystemDesignTopics(): Observable<{ message: string }> {
    return this.http.post<{ message: string }>(`${API_URL}/systemdesign/seed`, {});
  }

  getSystemDesignFavorites(): Observable<SystemDesignTopic[]> {
    return this.http.get<SystemDesignTopic[]>(`${API_URL}/systemdesign/favorites`);
  }

  // Interviews
  getInterviews(filters?: { type?: string; company?: string }): Observable<MockInterview[]> {
    const params = new URLSearchParams();
    if (filters?.type) params.set('type', filters.type);
    if (filters?.company) params.set('company', filters.company);
    return this.http.get<MockInterview[]>(`${API_URL}/interview?${params.toString()}`);
  }

  createInterview(interview: MockInterview): Observable<MockInterview> {
    return this.http.post<MockInterview>(`${API_URL}/interview`, interview);
  }

  updateInterview(id: number, interview: MockInterview): Observable<void> {
    return this.http.put<void>(`${API_URL}/interview/${id}`, interview);
  }

  deleteInterview(id: number): Observable<void> {
    return this.http.delete<void>(`${API_URL}/interview/${id}`);
  }

  // Weak Areas
  getWeakAreas(resolved?: boolean): Observable<WeakArea[]> {
    const params = resolved !== undefined ? `?resolved=${resolved}` : '';
    return this.http.get<WeakArea[]>(`${API_URL}/weakarea${params}`);
  }

  createWeakArea(weakArea: WeakArea): Observable<WeakArea> {
    return this.http.post<WeakArea>(`${API_URL}/weakarea`, weakArea);
  }

  resolveWeakArea(id: number): Observable<WeakArea> {
    return this.http.post<WeakArea>(`${API_URL}/weakarea/${id}/resolve`, {});
  }

  deleteWeakArea(id: number): Observable<void> {
    return this.http.delete<void>(`${API_URL}/weakarea/${id}`);
  }

  // Study Sessions
  getStudySessions(filters?: { type?: string; from?: string; to?: string }): Observable<StudySession[]> {
    const params = new URLSearchParams();
    if (filters?.type) params.set('type', filters.type);
    if (filters?.from) params.set('from', filters.from);
    if (filters?.to) params.set('to', filters.to);
    return this.http.get<StudySession[]>(`${API_URL}/studysession?${params.toString()}`);
  }

  createStudySession(session: StudySession): Observable<StudySession> {
    return this.http.post<StudySession>(`${API_URL}/studysession`, session);
  }

  // Analytics
  getDashboardStats(): Observable<DashboardStats> {
    return this.http.get<DashboardStats>(`${API_URL}/analytics/dashboard`);
  }

  getDSAAnalytics(): Observable<DSAAnalytics> {
    return this.http.get<DSAAnalytics>(`${API_URL}/analytics/dsa`);
  }

  getInterviewAnalytics(): Observable<InterviewAnalytics> {
    return this.http.get<InterviewAnalytics>(`${API_URL}/analytics/interviews`);
  }

  getWeakAreaAnalytics(): Observable<WeakAreaAnalytics> {
    return this.http.get<WeakAreaAnalytics>(`${API_URL}/analytics/weak-areas`);
  }

  // Azure Topics
  getAzureTopics(filters?: { category?: string; status?: string }): Observable<AzureTopic[]> {
    const params = new URLSearchParams();
    if (filters?.category) params.set('category', filters.category);
    if (filters?.status) params.set('status', filters.status);
    return this.http.get<AzureTopic[]>(`${API_URL}/azure?${params.toString()}`);
  }

  createAzureTopic(topic: AzureTopic): Observable<AzureTopic> {
    return this.http.post<AzureTopic>(`${API_URL}/azure`, topic);
  }

  updateAzureTopic(id: number, topic: AzureTopic): Observable<void> {
    return this.http.put<void>(`${API_URL}/azure/${id}`, topic);
  }

  deleteAzureTopic(id: number): Observable<void> {
    return this.http.delete<void>(`${API_URL}/azure/${id}`);
  }

  toggleAzureFavorite(id: number): Observable<AzureTopic> {
    return this.http.post<AzureTopic>(`${API_URL}/azure/${id}/favorite`, {});
  }

  seedAzureTopics(): Observable<{ message: string }> {
    return this.http.post<{ message: string }>(`${API_URL}/azure/seed`, {});
  }

  // OOP Topics
  getOOPTopics(filters?: { category?: string; status?: string }): Observable<OOPTopic[]> {
    const params = new URLSearchParams();
    if (filters?.category) params.set('category', filters.category);
    if (filters?.status) params.set('status', filters.status);
    return this.http.get<OOPTopic[]>(`${API_URL}/oop?${params.toString()}`);
  }

  createOOPTopic(topic: OOPTopic): Observable<OOPTopic> {
    return this.http.post<OOPTopic>(`${API_URL}/oop`, topic);
  }

  updateOOPTopic(id: number, topic: OOPTopic): Observable<void> {
    return this.http.put<void>(`${API_URL}/oop/${id}`, topic);
  }

  deleteOOPTopic(id: number): Observable<void> {
    return this.http.delete<void>(`${API_URL}/oop/${id}`);
  }

  toggleOOPFavorite(id: number): Observable<OOPTopic> {
    return this.http.post<OOPTopic>(`${API_URL}/oop/${id}/favorite`, {});
  }

  seedOOPTopics(): Observable<{ message: string }> {
    return this.http.post<{ message: string }>(`${API_URL}/oop/seed`, {});
  }

  // C# Topics
  getCSharpTopics(filters?: { category?: string; status?: string }): Observable<CSharpTopic[]> {
    const params = new URLSearchParams();
    if (filters?.category) params.set('category', filters.category);
    if (filters?.status) params.set('status', filters.status);
    return this.http.get<CSharpTopic[]>(`${API_URL}/csharp?${params.toString()}`);
  }

  createCSharpTopic(topic: CSharpTopic): Observable<CSharpTopic> {
    return this.http.post<CSharpTopic>(`${API_URL}/csharp`, topic);
  }

  updateCSharpTopic(id: number, topic: CSharpTopic): Observable<void> {
    return this.http.put<void>(`${API_URL}/csharp/${id}`, topic);
  }

  deleteCSharpTopic(id: number): Observable<void> {
    return this.http.delete<void>(`${API_URL}/csharp/${id}`);
  }

  toggleCSharpFavorite(id: number): Observable<CSharpTopic> {
    return this.http.post<CSharpTopic>(`${API_URL}/csharp/${id}/favorite`, {});
  }

  seedCSharpTopics(): Observable<{ message: string }> {
    return this.http.post<{ message: string }>(`${API_URL}/csharp/seed`, {});
  }

  // ASP.NET Core Topics
  getAspNetCoreTopics(filters?: { category?: string; status?: string }): Observable<AspNetCoreTopic[]> {
    const params = new URLSearchParams();
    if (filters?.category) params.set('category', filters.category);
    if (filters?.status) params.set('status', filters.status);
    return this.http.get<AspNetCoreTopic[]>(`${API_URL}/aspnetcore?${params.toString()}`);
  }

  createAspNetCoreTopic(topic: AspNetCoreTopic): Observable<AspNetCoreTopic> {
    return this.http.post<AspNetCoreTopic>(`${API_URL}/aspnetcore`, topic);
  }

  updateAspNetCoreTopic(id: number, topic: AspNetCoreTopic): Observable<void> {
    return this.http.put<void>(`${API_URL}/aspnetcore/${id}`, topic);
  }

  deleteAspNetCoreTopic(id: number): Observable<void> {
    return this.http.delete<void>(`${API_URL}/aspnetcore/${id}`);
  }

  toggleAspNetCoreFavorite(id: number): Observable<AspNetCoreTopic> {
    return this.http.post<AspNetCoreTopic>(`${API_URL}/aspnetcore/${id}/favorite`, {});
  }

  seedAspNetCoreTopics(): Observable<{ message: string }> {
    return this.http.post<{ message: string }>(`${API_URL}/aspnetcore/seed`, {});
  }

  // SQL Server Topics
  getSqlServerTopics(filters?: { category?: string; status?: string }): Observable<SqlServerTopic[]> {
    const params = new URLSearchParams();
    if (filters?.category) params.set('category', filters.category);
    if (filters?.status) params.set('status', filters.status);
    return this.http.get<SqlServerTopic[]>(`${API_URL}/sqlserver?${params.toString()}`);
  }

  createSqlServerTopic(topic: SqlServerTopic): Observable<SqlServerTopic> {
    return this.http.post<SqlServerTopic>(`${API_URL}/sqlserver`, topic);
  }

  updateSqlServerTopic(id: number, topic: SqlServerTopic): Observable<void> {
    return this.http.put<void>(`${API_URL}/sqlserver/${id}`, topic);
  }

  deleteSqlServerTopic(id: number): Observable<void> {
    return this.http.delete<void>(`${API_URL}/sqlserver/${id}`);
  }

  toggleSqlServerFavorite(id: number): Observable<SqlServerTopic> {
    return this.http.post<SqlServerTopic>(`${API_URL}/sqlserver/${id}/favorite`, {});
  }

  seedSqlServerTopics(): Observable<{ message: string }> {
    return this.http.post<{ message: string }>(`${API_URL}/sqlserver/seed`, {});
  }

  // Design Pattern Topics
  getDesignPatternTopics(filters?: { category?: string; status?: string }): Observable<DesignPatternTopic[]> {
    const params = new URLSearchParams();
    if (filters?.category) params.set('category', filters.category);
    if (filters?.status) params.set('status', filters.status);
    return this.http.get<DesignPatternTopic[]>(`${API_URL}/designpattern?${params.toString()}`);
  }

  createDesignPatternTopic(topic: DesignPatternTopic): Observable<DesignPatternTopic> {
    return this.http.post<DesignPatternTopic>(`${API_URL}/designpattern`, topic);
  }

  updateDesignPatternTopic(id: number, topic: DesignPatternTopic): Observable<void> {
    return this.http.put<void>(`${API_URL}/designpattern/${id}`, topic);
  }

  deleteDesignPatternTopic(id: number): Observable<void> {
    return this.http.delete<void>(`${API_URL}/designpattern/${id}`);
  }

  toggleDesignPatternFavorite(id: number): Observable<DesignPatternTopic> {
    return this.http.post<DesignPatternTopic>(`${API_URL}/designpattern/${id}/favorite`, {});
  }

  seedDesignPatternTopics(): Observable<{ message: string }> {
    return this.http.post<{ message: string }>(`${API_URL}/designpattern/seed`, {});
  }
}

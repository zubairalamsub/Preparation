import { Routes } from '@angular/router';

export const routes: Routes = [
  { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
  {
    path: 'dashboard',
    loadComponent: () => import('./pages/dashboard/dashboard.component').then(m => m.DashboardComponent)
  },
  {
    path: 'dsa',
    loadComponent: () => import('./pages/dsa/dsa.component').then(m => m.DsaComponent)
  },
  {
    path: 'system-design',
    loadComponent: () => import('./pages/system-design/system-design.component').then(m => m.SystemDesignComponent)
  },
  {
    path: 'interviews',
    loadComponent: () => import('./pages/interviews/interviews.component').then(m => m.InterviewsComponent)
  },
  {
    path: 'weak-areas',
    loadComponent: () => import('./pages/weak-areas/weak-areas.component').then(m => m.WeakAreasComponent)
  },
  {
    path: 'analytics',
    loadComponent: () => import('./pages/analytics/analytics.component').then(m => m.AnalyticsComponent)
  },
  {
    path: 'azure',
    loadComponent: () => import('./pages/azure/azure.component').then(m => m.AzureComponent)
  },
  {
    path: 'oop',
    loadComponent: () => import('./pages/oop/oop.component').then(m => m.OopComponent)
  },
  {
    path: 'csharp',
    loadComponent: () => import('./pages/csharp/csharp.component').then(m => m.CsharpComponent)
  },
  {
    path: 'aspnetcore',
    loadComponent: () => import('./pages/aspnetcore/aspnetcore.component').then(m => m.AspnetcoreComponent)
  },
  {
    path: 'sqlserver',
    loadComponent: () => import('./pages/sqlserver/sqlserver.component').then(m => m.SqlserverComponent)
  },
  {
    path: 'design-patterns',
    loadComponent: () => import('./pages/design-patterns/design-patterns.component').then(m => m.DesignPatternsComponent)
  },
  {
    path: 'entity-framework',
    loadComponent: () => import('./pages/entity-framework/entity-framework.component').then(m => m.EntityFrameworkComponent)
  }
];

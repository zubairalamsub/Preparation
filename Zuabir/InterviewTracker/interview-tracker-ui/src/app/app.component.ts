import { Component } from '@angular/core';
import { RouterOutlet, RouterLink, RouterLinkActive } from '@angular/router';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, RouterLink, RouterLinkActive],
  template: `
    <nav class="nav">
      <div class="container nav-content">
        <span class="nav-brand">Interview Tracker</span>
        <div class="nav-links">
          <a routerLink="/dashboard" routerLinkActive="active" class="nav-link">Dashboard</a>
          <a routerLink="/dsa" routerLinkActive="active" class="nav-link">DSA</a>
          <a routerLink="/system-design" routerLinkActive="active" class="nav-link">System Design</a>
          <a routerLink="/csharp" routerLinkActive="active" class="nav-link">C#</a>
          <a routerLink="/aspnetcore" routerLinkActive="active" class="nav-link">ASP.NET Core</a>
          <a routerLink="/sqlserver" routerLinkActive="active" class="nav-link">SQL Server</a>
          <a routerLink="/oop" routerLinkActive="active" class="nav-link">OOP</a>
          <a routerLink="/design-patterns" routerLinkActive="active" class="nav-link">Patterns</a>
          <a routerLink="/azure" routerLinkActive="active" class="nav-link">Azure</a>
          <a routerLink="/interviews" routerLinkActive="active" class="nav-link">Interviews</a>
          <a routerLink="/weak-areas" routerLinkActive="active" class="nav-link">Weak Areas</a>
          <a routerLink="/analytics" routerLinkActive="active" class="nav-link">Analytics</a>
        </div>
      </div>
    </nav>
    <main class="container" style="padding-top: 2rem; padding-bottom: 2rem;">
      <router-outlet></router-outlet>
    </main>
  `
})
export class AppComponent {}

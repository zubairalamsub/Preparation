import { Component } from '@angular/core';
import { RouterOutlet, RouterLink, RouterLinkActive } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, RouterLink, RouterLinkActive, CommonModule],
  template: `
    <nav class="nav">
      <div class="container nav-content">
        <span class="nav-brand">📚 Interview Tracker</span>
        <button class="nav-toggle" [class.active]="menuOpen" (click)="toggleMenu()">
          <span class="nav-toggle-icon"></span>
        </button>
        <div class="nav-links" [class.show]="menuOpen">
          <a routerLink="/dashboard" routerLinkActive="active" class="nav-link" (click)="closeMenu()">📊 Dashboard</a>
          <a routerLink="/dsa" routerLinkActive="active" class="nav-link" (click)="closeMenu()">🧮 DSA</a>
          <a routerLink="/system-design" routerLinkActive="active" class="nav-link" (click)="closeMenu()">🏗️ System Design</a>
          <a routerLink="/csharp" routerLinkActive="active" class="nav-link" (click)="closeMenu()">💻 C#</a>
          <a routerLink="/aspnetcore" routerLinkActive="active" class="nav-link" (click)="closeMenu()">🌐 ASP.NET</a>
          <a routerLink="/sqlserver" routerLinkActive="active" class="nav-link" (click)="closeMenu()">🗄️ SQL</a>
          <a routerLink="/oop" routerLinkActive="active" class="nav-link" (click)="closeMenu()">🎯 OOP</a>
          <a routerLink="/design-patterns" routerLinkActive="active" class="nav-link" (click)="closeMenu()">🔷 Patterns</a>
          <a routerLink="/azure" routerLinkActive="active" class="nav-link" (click)="closeMenu()">☁️ Azure</a>
          <a routerLink="/interviews" routerLinkActive="active" class="nav-link" (click)="closeMenu()">🎤 Interviews</a>
          <a routerLink="/weak-areas" routerLinkActive="active" class="nav-link" (click)="closeMenu()">⚠️ Weak Areas</a>
          <a routerLink="/analytics" routerLinkActive="active" class="nav-link" (click)="closeMenu()">📈 Analytics</a>
        </div>
      </div>
    </nav>
    <main class="container" style="padding-top: 2rem; padding-bottom: 2rem;">
      <router-outlet></router-outlet>
    </main>
  `
})
export class AppComponent {
  menuOpen = false;

  toggleMenu() {
    this.menuOpen = !this.menuOpen;
  }

  closeMenu() {
    this.menuOpen = false;
  }
}

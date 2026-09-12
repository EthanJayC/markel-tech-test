import { Component } from '@angular/core';
import { RouterLink, RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, RouterLink],
  template: `
    <header class="app-header">
      <div class="app-header-inner">
        <a routerLink="/" class="brand">
          <img src="/markelLogo.png" alt="Markel" />
          Markel claims
        </a>
      </div>
    </header>
    <main class="page">
      <router-outlet />
    </main>
  `
})
export class AppComponent {}

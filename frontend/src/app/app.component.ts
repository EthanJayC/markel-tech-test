import { Component } from '@angular/core';
import { RouterLink, RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, RouterLink],
  template: `
    <header>
      <a routerLink="/">Markel claims</a>
    </header>
    <main>
      <router-outlet />
    </main>
  `
})
export class AppComponent {}

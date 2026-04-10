import { Component } from '@angular/core';
import { RouterOutlet, RouterLink, RouterLinkActive } from '@angular/router';
import { TitleCasePipe } from '@angular/common';

@Component({
  selector: 'app-layout',
  imports: [RouterOutlet, RouterLink, RouterLinkActive, TitleCasePipe],
  templateUrl: './layout.html',
  styleUrl: './layout.scss'
})
export class Layout {
  activeSection = 'configuration';

  navItems = [
    { label: 'Configuration', key: 'configuration' },
    { label: 'Import', key: 'import' },
    { label: 'Administration', key: 'administration' }
  ];

  sidebarMenus: Record<string, { label: string; icon: string; route: string }[]> = {
    configuration: [
      { label: 'Dashboard', icon: '📊', route: '/dashboard' },
      { label: 'Équipements', icon: '🖥️', route: '/equipements' },
      { label: 'Sites', icon: '📍', route: '/sites' },
      { label: 'Versions', icon: '🔖', route: '/versions' },
      { label: 'Alertes', icon: '🔔', route: '/alertes' },
      { label: 'Suivi', icon: '📈', route: '/suivi' }
    ],
    import: [],
    administration: []
  };

  get currentMenu() {
    return this.sidebarMenus[this.activeSection] ?? [];
  }

  setSection(key: string) {
    this.activeSection = key;
  }

  logout() {
    // À implémenter
  }
}

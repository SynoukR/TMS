import { Component, OnInit, computed, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TerminalService } from '../../services/terminal.service';
import { Terminal, TerminalStatus } from '../../models/terminal.model';

@Component({
  selector: 'app-equipements',
  imports: [CommonModule],
  templateUrl: './equipements.html',
  styleUrl: './equipements.scss'
})
export class Equipements implements OnInit {
  readonly PAGE_SIZE = 20;
  readonly TerminalStatus = TerminalStatus;

  terminals = signal<Terminal[]>([]);
  loading = signal(true);
  error = signal<string | null>(null);
  currentPage = signal(1);

  totalCount = computed(() => this.terminals().length);
  onlineCount = computed(() => this.terminals().filter(t => t.status === TerminalStatus.Online).length);
  offlineCount = computed(() => this.terminals().filter(t => t.status === TerminalStatus.Offline).length);
  maintenanceCount = computed(() => this.terminals().filter(t => t.status === TerminalStatus.Maintenance).length);

  totalPages = computed(() => Math.ceil(this.terminals().length / this.PAGE_SIZE));

  pagedTerminals = computed(() => {
    const start = (this.currentPage() - 1) * this.PAGE_SIZE;
    return this.terminals().slice(start, start + this.PAGE_SIZE);
  });

  pages = computed(() =>
    Array.from({ length: this.totalPages() }, (_, i) => i + 1)
  );

  constructor(private terminalService: TerminalService) {}

  ngOnInit() {
    this.terminalService.getAll().subscribe({
      next: data => { this.terminals.set(data); this.loading.set(false); },
      error: () => { this.error.set('Impossible de charger les équipements.'); this.loading.set(false); }
    });
  }

  goToPage(page: number) {
    if (page >= 1 && page <= this.totalPages()) {
      this.currentPage.set(page);
    }
  }

  statusLabel(status: TerminalStatus): string {
    return { [TerminalStatus.Online]: 'En ligne', [TerminalStatus.Offline]: 'Hors ligne',
      [TerminalStatus.Maintenance]: 'Maintenance', [TerminalStatus.Unknown]: 'Inconnu' }[status] ?? 'Inconnu';
  }

  statusClass(status: TerminalStatus): string {
    return { [TerminalStatus.Online]: 'online', [TerminalStatus.Offline]: 'offline',
      [TerminalStatus.Maintenance]: 'maintenance', [TerminalStatus.Unknown]: 'unknown' }[status] ?? 'unknown';
  }

  formatDate(date?: string): string {
    if (!date) return '—';
    return new Date(date).toLocaleString('fr-FR', { day: '2-digit', month: '2-digit', year: 'numeric', hour: '2-digit', minute: '2-digit' });
  }

  viewDetail(terminal: Terminal) {
    // À implémenter
  }

  createNew() {
    // À implémenter
  }

  showOnMap() {
    // À implémenter
  }
}

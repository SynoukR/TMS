import { Component, OnInit, computed, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink, ActivatedRoute } from '@angular/router';
import { SiteService } from '../../../services/site.service';
import { SiteDetail as SiteDetailModel } from '../../../models/site.model';
import { TerminalStatus } from '../../../models/terminal.model';

@Component({
  selector: 'app-site-detail',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './site-detail.html',
  styleUrl: './site-detail.scss'
})
export class SiteDetail implements OnInit {
  readonly PAGE_SIZE = 20;
  readonly TerminalStatus = TerminalStatus;

  site = signal<SiteDetailModel | null>(null);
  loading = signal(true);
  error = signal<string | null>(null);
  currentPage = signal(1);

  totalPages = computed(() =>
    Math.ceil((this.site()?.terminals.length ?? 0) / this.PAGE_SIZE)
  );

  pagedTerminals = computed(() => {
    const terminals = this.site()?.terminals ?? [];
    const start = (this.currentPage() - 1) * this.PAGE_SIZE;
    return terminals.slice(start, start + this.PAGE_SIZE);
  });

  pages = computed(() =>
    Array.from({ length: this.totalPages() }, (_, i) => i + 1)
  );

  constructor(
    private route: ActivatedRoute,
    private siteService: SiteService
  ) {}

  ngOnInit(): void {
    const id = Number(this.route.snapshot.paramMap.get('id'));
    this.siteService.getById(id).subscribe({
      next: (data: SiteDetailModel) => {
        this.site.set(data);
        this.loading.set(false);
      },
      error: () => {
        this.error.set('Erreur lors du chargement du site.');
        this.loading.set(false);
      }
    });
  }

  goToPage(page: number): void {
    const total = this.totalPages();
    if (page >= 1 && page <= total) {
      this.currentPage.set(page);
    }
  }

  statusLabel(status: TerminalStatus): string {
    const labels: Record<TerminalStatus, string> = {
      [TerminalStatus.Online]: 'En ligne',
      [TerminalStatus.Offline]: 'Hors ligne',
      [TerminalStatus.Maintenance]: 'Maintenance',
      [TerminalStatus.Unknown]: 'Inconnu'
    };
    return labels[status] ?? 'Inconnu';
  }

  statusClass(status: TerminalStatus): string {
    const classes: Record<TerminalStatus, string> = {
      [TerminalStatus.Online]: 'online',
      [TerminalStatus.Offline]: 'offline',
      [TerminalStatus.Maintenance]: 'maintenance',
      [TerminalStatus.Unknown]: 'unknown'
    };
    return classes[status] ?? 'unknown';
  }

  formatDate(date?: string): string {
    if (!date) return '—';
    return new Date(date).toLocaleString('fr-FR', {
      day: '2-digit', month: '2-digit', year: 'numeric',
      hour: '2-digit', minute: '2-digit'
    });
  }
}

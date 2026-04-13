import { Component, OnInit, computed, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { WarehouseService } from '../../services/warehouse.service';
import { WarehouseDetail } from '../../models/warehouse.model';

@Component({
  selector: 'app-sites',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './sites.html',
  styleUrl: './sites.scss'
})
export class Sites implements OnInit {
  warehouses = signal<WarehouseDetail[]>([]);
  loading = signal(true);
  error = signal<string | null>(null);
  expandedWarehouses = signal<Set<number>>(new Set());

  warehouseCount = computed(() => this.warehouses().length);
  siteCount = computed(() => this.warehouses().reduce((acc, w) => acc + w.sites.length, 0));
  activeSiteCount = computed(() => this.warehouses().flatMap(w => w.sites).filter(s => s.isActive).length);
  inactiveSiteCount = computed(() => this.siteCount() - this.activeSiteCount());

  constructor(private warehouseService: WarehouseService) {}

  ngOnInit(): void {
    this.warehouseService.getAllWithSites().subscribe({
      next: (data) => {
        this.warehouses.set(data);
        this.loading.set(false);
      },
      error: () => {
        this.error.set('Erreur lors du chargement des sites et warehouses.');
        this.loading.set(false);
      }
    });
  }

  toggleWarehouse(id: number): void {
    this.expandedWarehouses.update(set => {
      const newSet = new Set(set);
      if (newSet.has(id)) {
        newSet.delete(id);
      } else {
        newSet.add(id);
      }
      return newSet;
    });
  }

  isExpanded(id: number): boolean {
    return this.expandedWarehouses().has(id);
  }
}

import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink, ActivatedRoute } from '@angular/router';
import { WarehouseService } from '../../../services/warehouse.service';
import { WarehouseDetail as WarehouseDetailModel } from '../../../models/warehouse.model';
import { EquipmentStatus } from '../../../models/terminal.model';

@Component({
  selector: 'app-warehouse-detail',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './warehouse-detail.html',
  styleUrl: './warehouse-detail.scss'
})
export class WarehouseDetail implements OnInit {
  warehouse = signal<WarehouseDetailModel | null>(null);
  loading = signal(true);
  error = signal<string | null>(null);

  constructor(
    private route: ActivatedRoute,
    private warehouseService: WarehouseService
  ) {}

  ngOnInit(): void {
    const id = Number(this.route.snapshot.paramMap.get('id'));
    this.warehouseService.getById(id).subscribe({
      next: (data: WarehouseDetailModel) => {
        this.warehouse.set(data);
        this.loading.set(false);
      },
      error: () => {
        this.error.set('Erreur lors du chargement de la warehouse.');
        this.loading.set(false);
      }
    });
  }

  equipmentStatusLabel(status: EquipmentStatus): string {
    const labels: Record<EquipmentStatus, string> = {
      [EquipmentStatus.Available]: 'Disponible',
      [EquipmentStatus.Returned]: 'En retour',
      [EquipmentStatus.InTransfer]: 'En transfert'
    };
    return labels[status] ?? 'Inconnu';
  }

  equipmentStatusClass(status: EquipmentStatus): string {
    const classes: Record<EquipmentStatus, string> = {
      [EquipmentStatus.Available]: 'available',
      [EquipmentStatus.Returned]: 'returned',
      [EquipmentStatus.InTransfer]: 'in-transfer'
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

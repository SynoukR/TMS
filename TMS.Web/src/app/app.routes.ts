import { Routes } from '@angular/router';
import { Layout } from './layout/layout';

export const routes: Routes = [
  {
    path: '',
    component: Layout,
    children: [
      { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
      { path: 'dashboard', loadComponent: () => import('./pages/dashboard/dashboard').then(m => m.Dashboard) },
      { path: 'equipements', loadComponent: () => import('./pages/equipements/equipements').then(m => m.Equipements) },
      { path: 'sites', loadComponent: () => import('./pages/sites/sites').then(m => m.Sites) },
      // La route warehouses/:id DOIT être avant sites/:id pour éviter le conflit de pattern
      { path: 'sites/warehouses/:id', loadComponent: () => import('./pages/sites/warehouse-detail/warehouse-detail').then(m => m.WarehouseDetail) },
      { path: 'sites/:id', loadComponent: () => import('./pages/sites/site-detail/site-detail').then(m => m.SiteDetail) },
      { path: 'versions', loadComponent: () => import('./pages/dashboard/dashboard').then(m => m.Dashboard) },
      { path: 'alertes', loadComponent: () => import('./pages/dashboard/dashboard').then(m => m.Dashboard) },
      { path: 'suivi', loadComponent: () => import('./pages/dashboard/dashboard').then(m => m.Dashboard) },
    ]
  }
];

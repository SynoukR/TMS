import { TerminalSummary } from './terminal.model';
import { Site } from './site.model';

export interface Warehouse {
  id: number;
  name: string;
  address?: string;
  isActive: boolean;
  siteCount: number;
  spareCount: number;
  createdAt: string;
}

export interface WarehouseDetail {
  id: number;
  name: string;
  address?: string;
  isActive: boolean;
  createdAt: string;
  sites: Site[];
  spareTerminals: TerminalSummary[];
}

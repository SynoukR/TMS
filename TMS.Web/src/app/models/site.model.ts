import { TerminalSummary } from './terminal.model';

export interface Site {
  id: number;
  name: string;
  address?: string;
  isActive: boolean;
  warehouseId: number;
  warehouseName: string;
  terminalCount: number;
  createdAt: string;
}

export interface SiteDetail extends Site {
  terminals: TerminalSummary[];
}

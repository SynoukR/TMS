export enum TerminalStatus {
  Unknown = 0,
  Online = 1,
  Offline = 2,
  Maintenance = 3
}

export enum EquipmentStatus {
  Available = 0,
  Returned = 1,
  InTransfer = 2
}

export interface Terminal {
  id: number;
  name: string;
  serialNumber: string;
  model: string;
  ipAddress?: string;
  location?: string;
  status: TerminalStatus;
  equipmentStatus: EquipmentStatus;
  lastSeen?: string;
  createdAt: string;
  siteId?: number;
  warehouseId?: number;
}

export interface TerminalSummary {
  id: number;
  name: string;
  serialNumber: string;
  model: string;
  ipAddress?: string;
  status: TerminalStatus;
  equipmentStatus: EquipmentStatus;
  lastSeen?: string;
}

using TMS.Core.Entities;
using TMS.Core.Enums;

namespace TMS.Infrastructure.Data;

public static class DbSeeder
{
    public static void Seed(AppDbContext context)
    {
        if (context.Warehouses.Any()) return;

        // ── Entrepôts ──────────────────────────────────────────────────────────
        var warehouseParis = new Warehouse
        {
            Name = "Entrepôt Paris-Nord",
            Address = "14 Rue de la Logistique, 93200 Saint-Denis",
            IsActive = true,
            CreatedAt = new DateTime(2025, 1, 10, 9, 0, 0, DateTimeKind.Utc)
        };
        var warehouseLyon = new Warehouse
        {
            Name = "Entrepôt Lyon-Est",
            Address = "8 Avenue des Transporteurs, 69003 Lyon",
            IsActive = true,
            CreatedAt = new DateTime(2025, 2, 5, 10, 0, 0, DateTimeKind.Utc)
        };
        var warehouseBordeaux = new Warehouse
        {
            Name = "Entrepôt Bordeaux-Lac",
            Address = "3 Quai du Commerce, 33000 Bordeaux",
            IsActive = false,
            CreatedAt = new DateTime(2025, 3, 20, 8, 0, 0, DateTimeKind.Utc)
        };

        context.Warehouses.AddRange(warehouseParis, warehouseLyon, warehouseBordeaux);
        context.SaveChanges();

        // ── Sites ──────────────────────────────────────────────────────────────
        var sites = new List<Site>
        {
            new() { Name = "Hypermarché Carrefour Rosny",   Address = "Av. du Général de Gaulle, 93110 Rosny-sous-Bois", IsActive = true,  WarehouseId = warehouseParis.Id,    CreatedAt = new DateTime(2025, 1, 15, 0, 0, 0, DateTimeKind.Utc) },
            new() { Name = "Leclerc Saint-Denis Centre",    Address = "12 Rue Victor Hugo, 93200 Saint-Denis",           IsActive = true,  WarehouseId = warehouseParis.Id,    CreatedAt = new DateTime(2025, 1, 20, 0, 0, 0, DateTimeKind.Utc) },
            new() { Name = "Auchan Villepinte",              Address = "ZAC Paris-Nord II, 95500 Villepinte",             IsActive = true,  WarehouseId = warehouseParis.Id,    CreatedAt = new DateTime(2025, 2, 1, 0, 0, 0, DateTimeKind.Utc) },
            new() { Name = "Intermarché Bobigny",            Address = "23 Bd Lénine, 93000 Bobigny",                    IsActive = false, WarehouseId = warehouseParis.Id,    CreatedAt = new DateTime(2025, 2, 10, 0, 0, 0, DateTimeKind.Utc) },
            new() { Name = "Carrefour Part-Dieu",            Address = "17 Rue du Docteur Bouchut, 69003 Lyon",          IsActive = true,  WarehouseId = warehouseLyon.Id,     CreatedAt = new DateTime(2025, 2, 8, 0, 0, 0, DateTimeKind.Utc) },
            new() { Name = "Leclerc Vénissieux",             Address = "2 Av. de la République, 69200 Vénissieux",       IsActive = true,  WarehouseId = warehouseLyon.Id,     CreatedAt = new DateTime(2025, 2, 15, 0, 0, 0, DateTimeKind.Utc) },
            new() { Name = "Auchan Mermoz",                  Address = "47 Av. Jean Mermoz, 69008 Lyon",                 IsActive = true,  WarehouseId = warehouseLyon.Id,     CreatedAt = new DateTime(2025, 3, 1, 0, 0, 0, DateTimeKind.Utc) },
            new() { Name = "Carrefour Mériadeck",            Address = "Place Charles Gruet, 33000 Bordeaux",            IsActive = false, WarehouseId = warehouseBordeaux.Id, CreatedAt = new DateTime(2025, 3, 25, 0, 0, 0, DateTimeKind.Utc) },
        };

        context.Sites.AddRange(sites);
        context.SaveChanges();

        var siteRosny      = sites[0];
        var siteSaintDenis = sites[1];
        var siteVillepinte = sites[2];
        var siteBobigny    = sites[3];
        var sitePartDieu   = sites[4];
        var siteVenissieux = sites[5];
        var siteMermoz     = sites[6];
        var siteMeriadeck  = sites[7];

        // ── Terminaux ──────────────────────────────────────────────────────────
        var terminals = new List<Terminal>
        {
            // -- Rosny (5 terminaux) --
            new() { Name = "TRM-ROS-01", SerialNumber = "SN-2024-001", Model = "Zebra TC57",    IpAddress = "192.168.1.11", Location = "Caisse 1",      Status = TerminalStatus.Online,      EquipmentStatus = EquipmentStatus.Available, SiteId = siteRosny.Id,      LastSeen = DateTime.UtcNow.AddMinutes(-5),   CreatedAt = new DateTime(2025, 1, 16, 0, 0, 0, DateTimeKind.Utc) },
            new() { Name = "TRM-ROS-02", SerialNumber = "SN-2024-002", Model = "Zebra TC57",    IpAddress = "192.168.1.12", Location = "Caisse 2",      Status = TerminalStatus.Online,      EquipmentStatus = EquipmentStatus.Available, SiteId = siteRosny.Id,      LastSeen = DateTime.UtcNow.AddMinutes(-3),   CreatedAt = new DateTime(2025, 1, 16, 0, 0, 0, DateTimeKind.Utc) },
            new() { Name = "TRM-ROS-03", SerialNumber = "SN-2024-003", Model = "Honeywell CT40",IpAddress = "192.168.1.13", Location = "Réception",     Status = TerminalStatus.Offline,     EquipmentStatus = EquipmentStatus.Available, SiteId = siteRosny.Id,      LastSeen = DateTime.UtcNow.AddHours(-6),     CreatedAt = new DateTime(2025, 1, 17, 0, 0, 0, DateTimeKind.Utc) },
            new() { Name = "TRM-ROS-04", SerialNumber = "SN-2024-004", Model = "Honeywell CT40",IpAddress = "192.168.1.14", Location = "Rayon frais",   Status = TerminalStatus.Maintenance, EquipmentStatus = EquipmentStatus.Available, SiteId = siteRosny.Id,      LastSeen = DateTime.UtcNow.AddDays(-2),      CreatedAt = new DateTime(2025, 1, 18, 0, 0, 0, DateTimeKind.Utc) },
            new() { Name = "TRM-ROS-05", SerialNumber = "SN-2024-005", Model = "Zebra MC9300",  IpAddress = "192.168.1.15", Location = "Entrepôt",      Status = TerminalStatus.Online,      EquipmentStatus = EquipmentStatus.Available, SiteId = siteRosny.Id,      LastSeen = DateTime.UtcNow.AddMinutes(-12),  CreatedAt = new DateTime(2025, 1, 19, 0, 0, 0, DateTimeKind.Utc) },

            // -- Saint-Denis (4 terminaux) --
            new() { Name = "TRM-SDN-01", SerialNumber = "SN-2024-011", Model = "Zebra TC57",    IpAddress = "192.168.2.11", Location = "Accueil",       Status = TerminalStatus.Online,      EquipmentStatus = EquipmentStatus.Available, SiteId = siteSaintDenis.Id, LastSeen = DateTime.UtcNow.AddMinutes(-8),   CreatedAt = new DateTime(2025, 1, 21, 0, 0, 0, DateTimeKind.Utc) },
            new() { Name = "TRM-SDN-02", SerialNumber = "SN-2024-012", Model = "Zebra TC57",    IpAddress = "192.168.2.12", Location = "Caisse 1",      Status = TerminalStatus.Online,      EquipmentStatus = EquipmentStatus.Available, SiteId = siteSaintDenis.Id, LastSeen = DateTime.UtcNow.AddMinutes(-2),   CreatedAt = new DateTime(2025, 1, 21, 0, 0, 0, DateTimeKind.Utc) },
            new() { Name = "TRM-SDN-03", SerialNumber = "SN-2024-013", Model = "Honeywell CT40",IpAddress = "192.168.2.13", Location = "Galerie",       Status = TerminalStatus.Offline,     EquipmentStatus = EquipmentStatus.Available, SiteId = siteSaintDenis.Id, LastSeen = DateTime.UtcNow.AddHours(-18),    CreatedAt = new DateTime(2025, 1, 22, 0, 0, 0, DateTimeKind.Utc) },
            new() { Name = "TRM-SDN-04", SerialNumber = "SN-2024-014", Model = "Zebra MC9300",  IpAddress = "192.168.2.14", Location = "Réserve",       Status = TerminalStatus.Online,      EquipmentStatus = EquipmentStatus.Available, SiteId = siteSaintDenis.Id, LastSeen = DateTime.UtcNow.AddMinutes(-25),  CreatedAt = new DateTime(2025, 1, 22, 0, 0, 0, DateTimeKind.Utc) },

            // -- Villepinte (3 terminaux) --
            new() { Name = "TRM-VPT-01", SerialNumber = "SN-2024-021", Model = "Zebra TC57",    IpAddress = "192.168.3.11", Location = "Entrée",        Status = TerminalStatus.Online,      EquipmentStatus = EquipmentStatus.Available, SiteId = siteVillepinte.Id, LastSeen = DateTime.UtcNow.AddMinutes(-1),   CreatedAt = new DateTime(2025, 2, 2, 0, 0, 0, DateTimeKind.Utc) },
            new() { Name = "TRM-VPT-02", SerialNumber = "SN-2024-022", Model = "Honeywell CT40",IpAddress = "192.168.3.12", Location = "Drive",         Status = TerminalStatus.Online,      EquipmentStatus = EquipmentStatus.Available, SiteId = siteVillepinte.Id, LastSeen = DateTime.UtcNow.AddMinutes(-4),   CreatedAt = new DateTime(2025, 2, 2, 0, 0, 0, DateTimeKind.Utc) },
            new() { Name = "TRM-VPT-03", SerialNumber = "SN-2024-023", Model = "Zebra MC9300",  IpAddress = "192.168.3.13", Location = "Réserve",       Status = TerminalStatus.Maintenance, EquipmentStatus = EquipmentStatus.Available, SiteId = siteVillepinte.Id, LastSeen = DateTime.UtcNow.AddDays(-5),      CreatedAt = new DateTime(2025, 2, 3, 0, 0, 0, DateTimeKind.Utc) },

            // -- Bobigny (2 terminaux, site inactif) --
            new() { Name = "TRM-BOB-01", SerialNumber = "SN-2024-031", Model = "Zebra TC57",    IpAddress = "192.168.4.11", Location = "Caisse 1",      Status = TerminalStatus.Offline,     EquipmentStatus = EquipmentStatus.Available, SiteId = siteBobigny.Id,    LastSeen = DateTime.UtcNow.AddDays(-10),     CreatedAt = new DateTime(2025, 2, 11, 0, 0, 0, DateTimeKind.Utc) },
            new() { Name = "TRM-BOB-02", SerialNumber = "SN-2024-032", Model = "Honeywell CT40",IpAddress = "192.168.4.12", Location = "Réception",     Status = TerminalStatus.Offline,     EquipmentStatus = EquipmentStatus.Available, SiteId = siteBobigny.Id,    LastSeen = DateTime.UtcNow.AddDays(-10),     CreatedAt = new DateTime(2025, 2, 11, 0, 0, 0, DateTimeKind.Utc) },

            // -- Part-Dieu (4 terminaux) --
            new() { Name = "TRM-PDI-01", SerialNumber = "SN-2024-041", Model = "Zebra TC57",    IpAddress = "10.0.1.11",    Location = "Caisse 1",      Status = TerminalStatus.Online,      EquipmentStatus = EquipmentStatus.Available, SiteId = sitePartDieu.Id,   LastSeen = DateTime.UtcNow.AddMinutes(-6),   CreatedAt = new DateTime(2025, 2, 9, 0, 0, 0, DateTimeKind.Utc) },
            new() { Name = "TRM-PDI-02", SerialNumber = "SN-2024-042", Model = "Zebra TC57",    IpAddress = "10.0.1.12",    Location = "Caisse 2",      Status = TerminalStatus.Online,      EquipmentStatus = EquipmentStatus.Available, SiteId = sitePartDieu.Id,   LastSeen = DateTime.UtcNow.AddMinutes(-9),   CreatedAt = new DateTime(2025, 2, 9, 0, 0, 0, DateTimeKind.Utc) },
            new() { Name = "TRM-PDI-03", SerialNumber = "SN-2024-043", Model = "Honeywell CT40",IpAddress = "10.0.1.13",    Location = "Self-scan",     Status = TerminalStatus.Online,      EquipmentStatus = EquipmentStatus.Available, SiteId = sitePartDieu.Id,   LastSeen = DateTime.UtcNow.AddMinutes(-15),  CreatedAt = new DateTime(2025, 2, 10, 0, 0, 0, DateTimeKind.Utc) },
            new() { Name = "TRM-PDI-04", SerialNumber = "SN-2024-044", Model = "Zebra MC9300",  IpAddress = "10.0.1.14",    Location = "Entrepôt",      Status = TerminalStatus.Offline,     EquipmentStatus = EquipmentStatus.Available, SiteId = sitePartDieu.Id,   LastSeen = DateTime.UtcNow.AddHours(-3),     CreatedAt = new DateTime(2025, 2, 10, 0, 0, 0, DateTimeKind.Utc) },

            // -- Vénissieux (3 terminaux) --
            new() { Name = "TRM-VEN-01", SerialNumber = "SN-2024-051", Model = "Zebra TC57",    IpAddress = "10.0.2.11",    Location = "Accueil",       Status = TerminalStatus.Online,      EquipmentStatus = EquipmentStatus.Available, SiteId = siteVenissieux.Id, LastSeen = DateTime.UtcNow.AddMinutes(-7),   CreatedAt = new DateTime(2025, 2, 16, 0, 0, 0, DateTimeKind.Utc) },
            new() { Name = "TRM-VEN-02", SerialNumber = "SN-2024-052", Model = "Honeywell CT40",IpAddress = "10.0.2.12",    Location = "Drive",         Status = TerminalStatus.Online,      EquipmentStatus = EquipmentStatus.Available, SiteId = siteVenissieux.Id, LastSeen = DateTime.UtcNow.AddMinutes(-3),   CreatedAt = new DateTime(2025, 2, 16, 0, 0, 0, DateTimeKind.Utc) },
            new() { Name = "TRM-VEN-03", SerialNumber = "SN-2024-053", Model = "Zebra TC57",    IpAddress = "10.0.2.13",    Location = "Rayon SEC",     Status = TerminalStatus.Maintenance, EquipmentStatus = EquipmentStatus.Available, SiteId = siteVenissieux.Id, LastSeen = DateTime.UtcNow.AddDays(-3),      CreatedAt = new DateTime(2025, 2, 17, 0, 0, 0, DateTimeKind.Utc) },

            // -- Mermoz (2 terminaux) --
            new() { Name = "TRM-MER-01", SerialNumber = "SN-2024-061", Model = "Zebra TC57",    IpAddress = "10.0.3.11",    Location = "Entrée",        Status = TerminalStatus.Online,      EquipmentStatus = EquipmentStatus.Available, SiteId = siteMermoz.Id,     LastSeen = DateTime.UtcNow.AddMinutes(-20),  CreatedAt = new DateTime(2025, 3, 2, 0, 0, 0, DateTimeKind.Utc) },
            new() { Name = "TRM-MER-02", SerialNumber = "SN-2024-062", Model = "Honeywell CT40",IpAddress = "10.0.3.12",    Location = "Caisse unique", Status = TerminalStatus.Online,      EquipmentStatus = EquipmentStatus.Available, SiteId = siteMermoz.Id,     LastSeen = DateTime.UtcNow.AddMinutes(-11),  CreatedAt = new DateTime(2025, 3, 2, 0, 0, 0, DateTimeKind.Utc) },

            // -- Mériadeck (2 terminaux, site inactif) --
            new() { Name = "TRM-MBX-01", SerialNumber = "SN-2024-071", Model = "Zebra TC57",    IpAddress = "172.16.1.11",  Location = "Caisse 1",      Status = TerminalStatus.Offline,     EquipmentStatus = EquipmentStatus.Available, SiteId = siteMeriadeck.Id,  LastSeen = DateTime.UtcNow.AddDays(-15),     CreatedAt = new DateTime(2025, 3, 26, 0, 0, 0, DateTimeKind.Utc) },
            new() { Name = "TRM-MBX-02", SerialNumber = "SN-2024-072", Model = "Zebra MC9300",  IpAddress = "172.16.1.12",  Location = "Réserve",       Status = TerminalStatus.Offline,     EquipmentStatus = EquipmentStatus.Available, SiteId = siteMeriadeck.Id,  LastSeen = DateTime.UtcNow.AddDays(-15),     CreatedAt = new DateTime(2025, 3, 26, 0, 0, 0, DateTimeKind.Utc) },

            // -- Spare Paris-Nord (4 terminaux en stock) --
            new() { Name = "TRM-SPARE-P01", SerialNumber = "SN-2024-101", Model = "Zebra TC57",    IpAddress = null, Location = null, Status = TerminalStatus.Unknown, EquipmentStatus = EquipmentStatus.Available,  WarehouseId = warehouseParis.Id, LastSeen = null, CreatedAt = new DateTime(2025, 3, 10, 0, 0, 0, DateTimeKind.Utc) },
            new() { Name = "TRM-SPARE-P02", SerialNumber = "SN-2024-102", Model = "Honeywell CT40", IpAddress = null, Location = null, Status = TerminalStatus.Unknown, EquipmentStatus = EquipmentStatus.InTransfer,  WarehouseId = warehouseParis.Id, LastSeen = null, CreatedAt = new DateTime(2025, 3, 12, 0, 0, 0, DateTimeKind.Utc) },
            new() { Name = "TRM-SPARE-P03", SerialNumber = "SN-2024-103", Model = "Zebra MC9300",   IpAddress = null, Location = null, Status = TerminalStatus.Unknown, EquipmentStatus = EquipmentStatus.Returned,    WarehouseId = warehouseParis.Id, LastSeen = null, CreatedAt = new DateTime(2025, 3, 14, 0, 0, 0, DateTimeKind.Utc) },
            new() { Name = "TRM-SPARE-P04", SerialNumber = "SN-2024-104", Model = "Zebra TC57",    IpAddress = null, Location = null, Status = TerminalStatus.Unknown, EquipmentStatus = EquipmentStatus.Available,  WarehouseId = warehouseParis.Id, LastSeen = null, CreatedAt = new DateTime(2025, 3, 15, 0, 0, 0, DateTimeKind.Utc) },

            // -- Spare Lyon-Est (3 terminaux en stock) --
            new() { Name = "TRM-SPARE-L01", SerialNumber = "SN-2024-111", Model = "Zebra TC57",    IpAddress = null, Location = null, Status = TerminalStatus.Unknown, EquipmentStatus = EquipmentStatus.Available,  WarehouseId = warehouseLyon.Id, LastSeen = null, CreatedAt = new DateTime(2025, 2, 20, 0, 0, 0, DateTimeKind.Utc) },
            new() { Name = "TRM-SPARE-L02", SerialNumber = "SN-2024-112", Model = "Honeywell CT40", IpAddress = null, Location = null, Status = TerminalStatus.Unknown, EquipmentStatus = EquipmentStatus.Returned,    WarehouseId = warehouseLyon.Id, LastSeen = null, CreatedAt = new DateTime(2025, 2, 22, 0, 0, 0, DateTimeKind.Utc) },
            new() { Name = "TRM-SPARE-L03", SerialNumber = "SN-2024-113", Model = "Zebra MC9300",   IpAddress = null, Location = null, Status = TerminalStatus.Unknown, EquipmentStatus = EquipmentStatus.InTransfer,  WarehouseId = warehouseLyon.Id, LastSeen = null, CreatedAt = new DateTime(2025, 2, 25, 0, 0, 0, DateTimeKind.Utc) },
        };

        context.Terminals.AddRange(terminals);
        context.SaveChanges();
    }
}

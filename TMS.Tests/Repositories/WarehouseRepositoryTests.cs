using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using TMS.Core.Entities;
using TMS.Core.Enums;
using TMS.Infrastructure.Data;
using TMS.Infrastructure.Repositories;

namespace TMS.Tests.Repositories;

public class WarehouseRepositoryTests : IDisposable
{
    private readonly AppDbContext _context;
    private readonly WarehouseRepository _repo;

    public WarehouseRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        _context = new AppDbContext(options);
        _repo = new WarehouseRepository(_context);
    }

    public void Dispose() => _context.Dispose();

    // --- Helpers ---

    private async Task<Warehouse> AddWarehouseAsync(string name = "WH Default")
    {
        var wh = new Warehouse { Name = name };
        _context.Warehouses.Add(wh);
        await _context.SaveChangesAsync();
        return wh;
    }

    private async Task<Site> AddSiteAsync(int warehouseId, string name = "Site A")
    {
        var site = new Site { Name = name, WarehouseId = warehouseId };
        _context.Sites.Add(site);
        await _context.SaveChangesAsync();
        return site;
    }

    private async Task<Terminal> AddTerminalOnSiteAsync(int siteId, string serial)
    {
        var t = new Terminal
        {
            Name = $"Terminal {serial}",
            SerialNumber = serial,
            Model = "Model X",
            SiteId = siteId
        };
        _context.Terminals.Add(t);
        await _context.SaveChangesAsync();
        return t;
    }

    private async Task<Terminal> AddSpareTerminalAsync(int warehouseId, string serial)
    {
        var t = new Terminal
        {
            Name = $"Spare {serial}",
            SerialNumber = serial,
            Model = "Model Y",
            WarehouseId = warehouseId,
            EquipmentStatus = EquipmentStatus.Available
        };
        _context.Terminals.Add(t);
        await _context.SaveChangesAsync();
        return t;
    }

    // ────────────────────────────────────────────────
    // GetAllWithSitesAsync
    // ────────────────────────────────────────────────

    [Fact]
    public async Task GetAllWithSitesAsync_ReturnsSitesAndSpareTerminals()
    {
        // Arrange
        var wh = await AddWarehouseAsync("WH-With-Data");
        var site = await AddSiteAsync(wh.Id, "Site-1");
        await AddTerminalOnSiteAsync(site.Id, "SN-SITE-001");
        await AddSpareTerminalAsync(wh.Id, "SN-SPARE-001");

        // Act
        var result = (await _repo.GetAllWithSitesAsync()).ToList();

        // Assert
        result.Should().HaveCount(1);
        var fetched = result[0];
        fetched.Name.Should().Be("WH-With-Data");
        fetched.Sites.Should().HaveCount(1);
        fetched.Sites.First().Terminals.Should().HaveCount(1);
        fetched.SpareTerminals.Should().HaveCount(1);
        fetched.SpareTerminals.First().SerialNumber.Should().Be("SN-SPARE-001");
    }

    [Fact]
    public async Task GetAllWithSitesAsync_ReturnsEmpty_WhenNoWarehouses()
    {
        // Act
        var result = await _repo.GetAllWithSitesAsync();

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetAllWithSitesAsync_ReturnsMultipleWarehouses()
    {
        // Arrange
        await AddWarehouseAsync("WH-Alpha");
        await AddWarehouseAsync("WH-Beta");
        await AddWarehouseAsync("WH-Gamma");

        // Act
        var result = (await _repo.GetAllWithSitesAsync()).ToList();

        // Assert
        result.Should().HaveCount(3);
    }

    [Fact]
    public async Task GetAllWithSitesAsync_ReturnsWarehousesOrderedByName()
    {
        // Arrange
        await AddWarehouseAsync("Zebra WH");
        await AddWarehouseAsync("Alpha WH");
        await AddWarehouseAsync("Metro WH");

        // Act
        var result = (await _repo.GetAllWithSitesAsync()).ToList();

        // Assert
        result.Select(w => w.Name).Should().BeInAscendingOrder();
    }

    [Fact]
    public async Task GetAllWithSitesAsync_WarehouseWithNoSitesHasEmptyCollections()
    {
        // Arrange
        await AddWarehouseAsync("Empty WH");

        // Act
        var result = (await _repo.GetAllWithSitesAsync()).ToList();

        // Assert
        result.Should().HaveCount(1);
        result[0].Sites.Should().BeEmpty();
        result[0].SpareTerminals.Should().BeEmpty();
    }

    [Fact]
    public async Task GetAllWithSitesAsync_DoesNotIncludeSiteTerminalsAsSpares()
    {
        // Arrange
        var wh = await AddWarehouseAsync("WH-Mixed");
        var site = await AddSiteAsync(wh.Id);
        // This terminal belongs to the site, NOT to the warehouse as spare
        await AddTerminalOnSiteAsync(site.Id, "SN-SITE-ONLY");

        // Act
        var result = (await _repo.GetAllWithSitesAsync()).First();

        // Assert
        result.SpareTerminals.Should().BeEmpty();
        result.Sites.First().Terminals.Should().HaveCount(1);
    }

    // ────────────────────────────────────────────────
    // GetWithSitesAndTerminalsAsync
    // ────────────────────────────────────────────────

    [Fact]
    public async Task GetWithSitesAndTerminalsAsync_ReturnsFullGraph()
    {
        // Arrange
        var wh = await AddWarehouseAsync("Full WH");
        var site1 = await AddSiteAsync(wh.Id, "Site-1");
        var site2 = await AddSiteAsync(wh.Id, "Site-2");
        await AddTerminalOnSiteAsync(site1.Id, "SN-S1-001");
        await AddTerminalOnSiteAsync(site1.Id, "SN-S1-002");
        await AddTerminalOnSiteAsync(site2.Id, "SN-S2-001");
        await AddSpareTerminalAsync(wh.Id, "SN-SPARE-001");
        await AddSpareTerminalAsync(wh.Id, "SN-SPARE-002");

        // Act
        var result = await _repo.GetWithSitesAndTerminalsAsync(wh.Id);

        // Assert
        result.Should().NotBeNull();
        result!.Name.Should().Be("Full WH");
        result.Sites.Should().HaveCount(2);
        result.Sites.First(s => s.Name == "Site-1").Terminals.Should().HaveCount(2);
        result.Sites.First(s => s.Name == "Site-2").Terminals.Should().HaveCount(1);
        result.SpareTerminals.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetWithSitesAndTerminalsAsync_ReturnsNull_WhenNotFound()
    {
        // Act
        var result = await _repo.GetWithSitesAndTerminalsAsync(99999);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetWithSitesAndTerminalsAsync_ReturnsEmptyCollections_WhenNoSitesOrSpares()
    {
        // Arrange
        var wh = await AddWarehouseAsync("Lonely WH");

        // Act
        var result = await _repo.GetWithSitesAndTerminalsAsync(wh.Id);

        // Assert
        result.Should().NotBeNull();
        result!.Sites.Should().BeEmpty();
        result.SpareTerminals.Should().BeEmpty();
    }

    [Fact]
    public async Task GetWithSitesAndTerminalsAsync_ReturnsCorrectWarehouse_WhenMultipleExist()
    {
        // Arrange
        var wh1 = await AddWarehouseAsync("WH-One");
        var wh2 = await AddWarehouseAsync("WH-Two");
        await AddSiteAsync(wh2.Id, "Only-In-WH2");

        // Act
        var result = await _repo.GetWithSitesAndTerminalsAsync(wh1.Id);

        // Assert
        result.Should().NotBeNull();
        result!.Name.Should().Be("WH-One");
        result.Sites.Should().BeEmpty();
    }

    // ────────────────────────────────────────────────
    // Base CRUD (inherited from Repository<T>)
    // ────────────────────────────────────────────────

    [Fact]
    public async Task AddAsync_CreatesWarehouse_WithCorrectFields()
    {
        // Arrange
        var wh = new Warehouse
        {
            Name = "Brand New WH",
            Address = "99 Rue de la Paix",
            IsActive = true
        };

        // Act
        var created = await _repo.AddAsync(wh);

        // Assert
        created.Id.Should().BeGreaterThan(0);
        created.Name.Should().Be("Brand New WH");
        created.IsActive.Should().BeTrue();
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsCorrectWarehouse()
    {
        // Arrange
        var wh = await AddWarehouseAsync("Find Me");

        // Act
        var result = await _repo.GetByIdAsync(wh.Id);

        // Assert
        result.Should().NotBeNull();
        result!.Name.Should().Be("Find Me");
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsNull_WhenNotFound()
    {
        // Act
        var result = await _repo.GetByIdAsync(99999);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task UpdateAsync_ChangesWarehouseFields()
    {
        // Arrange
        var wh = await AddWarehouseAsync("Original");

        // Act
        wh.Name = "Updated";
        wh.IsActive = false;
        await _repo.UpdateAsync(wh);
        var result = await _repo.GetByIdAsync(wh.Id);

        // Assert
        result!.Name.Should().Be("Updated");
        result.IsActive.Should().BeFalse();
    }

    [Fact]
    public async Task DeleteAsync_RemovesWarehouse()
    {
        // Arrange
        var wh = await AddWarehouseAsync("Delete Me");

        // Act
        await _repo.DeleteAsync(wh);
        var result = await _repo.GetByIdAsync(wh.Id);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllWarehouses()
    {
        // Arrange
        await AddWarehouseAsync("WH-1");
        await AddWarehouseAsync("WH-2");

        // Act
        var result = (await _repo.GetAllAsync()).ToList();

        // Assert
        result.Should().HaveCount(2);
    }
}

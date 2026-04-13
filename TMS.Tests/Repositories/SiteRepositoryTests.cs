using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using TMS.Core.Entities;
using TMS.Core.Enums;
using TMS.Infrastructure.Data;
using TMS.Infrastructure.Repositories;

namespace TMS.Tests.Repositories;

public class SiteRepositoryTests : IDisposable
{
    private readonly AppDbContext _context;
    private readonly SiteRepository _repo;

    public SiteRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        _context = new AppDbContext(options);
        _repo = new SiteRepository(_context);
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

    private async Task<Terminal> AddTerminalAsync(int siteId, string serial)
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

    // ────────────────────────────────────────────────
    // GetByWarehouseIdAsync
    // ────────────────────────────────────────────────

    [Fact]
    public async Task GetByWarehouseIdAsync_ReturnsOnlySitesOfWarehouse()
    {
        // Arrange
        var wh1 = await AddWarehouseAsync("WH1");
        var wh2 = await AddWarehouseAsync("WH2");
        await AddSiteAsync(wh1.Id, "Site-A1");
        await AddSiteAsync(wh1.Id, "Site-A2");
        await AddSiteAsync(wh2.Id, "Site-B1");

        // Act
        var result = (await _repo.GetByWarehouseIdAsync(wh1.Id)).ToList();

        // Assert
        result.Should().HaveCount(2);
        result.Should().OnlyContain(s => s.WarehouseId == wh1.Id);
        result.Select(s => s.Name).Should().Contain(["Site-A1", "Site-A2"]);
    }

    [Fact]
    public async Task GetByWarehouseIdAsync_ReturnsEmpty_WhenNoSitesInWarehouse()
    {
        // Arrange
        var wh = await AddWarehouseAsync();

        // Act
        var result = await _repo.GetByWarehouseIdAsync(wh.Id);

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetByWarehouseIdAsync_ReturnsEmpty_WhenWarehouseDoesNotExist()
    {
        // Act
        var result = await _repo.GetByWarehouseIdAsync(99999);

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetByWarehouseIdAsync_IncludesTerminals()
    {
        // Arrange
        var wh = await AddWarehouseAsync();
        var site = await AddSiteAsync(wh.Id);
        await AddTerminalAsync(site.Id, "SN-001");
        await AddTerminalAsync(site.Id, "SN-002");

        // Act
        var result = (await _repo.GetByWarehouseIdAsync(wh.Id)).ToList();

        // Assert
        result.Should().HaveCount(1);
        result[0].Terminals.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetByWarehouseIdAsync_ReturnsSitesOrderedByName()
    {
        // Arrange
        var wh = await AddWarehouseAsync();
        await AddSiteAsync(wh.Id, "Zebra");
        await AddSiteAsync(wh.Id, "Alpha");
        await AddSiteAsync(wh.Id, "Metro");

        // Act
        var result = (await _repo.GetByWarehouseIdAsync(wh.Id)).ToList();

        // Assert
        result.Select(s => s.Name).Should().BeInAscendingOrder();
    }

    // ────────────────────────────────────────────────
    // GetWithTerminalsAsync
    // ────────────────────────────────────────────────

    [Fact]
    public async Task GetWithTerminalsAsync_IncludesWarehouseAndTerminals()
    {
        // Arrange
        var wh = await AddWarehouseAsync("My Warehouse");
        var site = await AddSiteAsync(wh.Id, "Site-X");
        await AddTerminalAsync(site.Id, "SN-AAA");

        // Act
        var result = await _repo.GetWithTerminalsAsync(site.Id);

        // Assert
        result.Should().NotBeNull();
        result!.Name.Should().Be("Site-X");
        result.Warehouse.Should().NotBeNull();
        result.Warehouse.Name.Should().Be("My Warehouse");
        result.Terminals.Should().HaveCount(1);
        result.Terminals.First().SerialNumber.Should().Be("SN-AAA");
    }

    [Fact]
    public async Task GetWithTerminalsAsync_ReturnsNull_WhenNotFound()
    {
        // Act
        var result = await _repo.GetWithTerminalsAsync(99999);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetWithTerminalsAsync_ReturnsEmptyTerminals_WhenNoTerminalsOnSite()
    {
        // Arrange
        var wh = await AddWarehouseAsync();
        var site = await AddSiteAsync(wh.Id, "Empty Site");

        // Act
        var result = await _repo.GetWithTerminalsAsync(site.Id);

        // Assert
        result.Should().NotBeNull();
        result!.Terminals.Should().BeEmpty();
    }

    [Fact]
    public async Task GetWithTerminalsAsync_IncludesMultipleTerminals()
    {
        // Arrange
        var wh = await AddWarehouseAsync();
        var site = await AddSiteAsync(wh.Id);
        await AddTerminalAsync(site.Id, "SN-001");
        await AddTerminalAsync(site.Id, "SN-002");
        await AddTerminalAsync(site.Id, "SN-003");

        // Act
        var result = await _repo.GetWithTerminalsAsync(site.Id);

        // Assert
        result!.Terminals.Should().HaveCount(3);
    }

    // ────────────────────────────────────────────────
    // Base CRUD (inherited from Repository<T>)
    // ────────────────────────────────────────────────

    [Fact]
    public async Task AddAsync_CreatesSite_WithCorrectFields()
    {
        // Arrange
        var wh = await AddWarehouseAsync();
        var site = new Site
        {
            Name = "New Site",
            Address = "1 Rue Test",
            IsActive = true,
            WarehouseId = wh.Id
        };

        // Act
        var created = await _repo.AddAsync(site);

        // Assert
        created.Id.Should().BeGreaterThan(0);
        created.Name.Should().Be("New Site");
        created.IsActive.Should().BeTrue();
        created.WarehouseId.Should().Be(wh.Id);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsCorrectSite()
    {
        // Arrange
        var wh = await AddWarehouseAsync();
        var site = await AddSiteAsync(wh.Id, "Target Site");

        // Act
        var result = await _repo.GetByIdAsync(site.Id);

        // Assert
        result.Should().NotBeNull();
        result!.Name.Should().Be("Target Site");
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
    public async Task DeleteAsync_RemovesSite()
    {
        // Arrange
        var wh = await AddWarehouseAsync();
        var site = await AddSiteAsync(wh.Id, "To Delete");

        // Act
        await _repo.DeleteAsync(site);
        var result = await _repo.GetByIdAsync(site.Id);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task UpdateAsync_ChangesSiteFields()
    {
        // Arrange
        var wh = await AddWarehouseAsync();
        var site = await AddSiteAsync(wh.Id, "Original Name");

        // Act
        site.Name = "Updated Name";
        site.IsActive = false;
        await _repo.UpdateAsync(site);
        var result = await _repo.GetByIdAsync(site.Id);

        // Assert
        result!.Name.Should().Be("Updated Name");
        result.IsActive.Should().BeFalse();
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllSites()
    {
        // Arrange
        var wh = await AddWarehouseAsync();
        await AddSiteAsync(wh.Id, "Site 1");
        await AddSiteAsync(wh.Id, "Site 2");
        await AddSiteAsync(wh.Id, "Site 3");

        // Act
        var result = (await _repo.GetAllAsync()).ToList();

        // Assert
        result.Should().HaveCount(3);
    }
}

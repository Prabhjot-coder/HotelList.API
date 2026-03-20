using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using WebApplication4.Data;
using WebApplication4.Models;
using WebApplication4.Repositories;
using Xunit;

namespace HotelList.API.Tests.Repositories;

public class GenericRepositoryTests
{
    private static SchoolContext CreateContext(string db)
    {
        var opts = new DbContextOptionsBuilder<SchoolContext>()
            .UseInMemoryDatabase(db).Options;
        return new SchoolContext(opts);
    }

    [Fact]
    public async Task AddAsync_ShouldPersistEntityAndReturnWithId()
    {
        using var ctx = CreateContext(nameof(AddAsync_ShouldPersistEntityAndReturnWithId));
        var repo = new GenericRepository<Department>(ctx);
        var dept = new Department { Name = "Computer Science", IsActive = true };

        var result = await repo.AddAsync(dept);

        result.DepartmentId.Should().BeGreaterThan(0);
        result.Name.Should().Be("Computer Science");
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsCorrectEntity()
    {
        using var ctx = CreateContext(nameof(GetByIdAsync_ReturnsCorrectEntity));
        var repo = new GenericRepository<Department>(ctx);
        var added = await repo.AddAsync(new Department { Name = "Physics", IsActive = true });

        var found = await repo.GetByIdAsync(added.DepartmentId);

        found.Should().NotBeNull();
        found!.Name.Should().Be("Physics");
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsNull_WhenNotFound()
    {
        using var ctx = CreateContext(nameof(GetByIdAsync_ReturnsNull_WhenNotFound));
        var repo = new GenericRepository<Department>(ctx);

        var result = await repo.GetByIdAsync(9999);

        result.Should().BeNull();
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllEntities()
    {
        using var ctx = CreateContext(nameof(GetAllAsync_ReturnsAllEntities));
        var repo = new GenericRepository<Department>(ctx);
        await repo.AddAsync(new Department { Name = "Math", IsActive = true });
        await repo.AddAsync(new Department { Name = "Biology", IsActive = true });

        var result = await repo.GetAllAsync();

        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task UpdateAsync_ModifiesEntity()
    {
        using var ctx = CreateContext(nameof(UpdateAsync_ModifiesEntity));
        var repo = new GenericRepository<Department>(ctx);
        var dept = await repo.AddAsync(new Department { Name = "Old Name", IsActive = true });

        dept.Name = "New Name";
        await repo.UpdateAsync(dept);

        var updated = await repo.GetByIdAsync(dept.DepartmentId);
        updated!.Name.Should().Be("New Name");
    }

    [Fact]
    public async Task DeleteAsync_RemovesEntity()
    {
        using var ctx = CreateContext(nameof(DeleteAsync_RemovesEntity));
        var repo = new GenericRepository<Department>(ctx);
        var dept = await repo.AddAsync(new Department { Name = "ToDelete", IsActive = true });

        await repo.DeleteAsync(dept.DepartmentId);

        (await repo.GetByIdAsync(dept.DepartmentId)).Should().BeNull();
    }

    [Fact]
    public async Task ExistsAsync_ReturnsTrue_WhenExists()
    {
        using var ctx = CreateContext(nameof(ExistsAsync_ReturnsTrue_WhenExists));
        var repo = new GenericRepository<Department>(ctx);
        var dept = await repo.AddAsync(new Department { Name = "Exists", IsActive = true });

        (await repo.ExistsAsync(dept.DepartmentId)).Should().BeTrue();
    }

    [Fact]
    public async Task ExistsAsync_ReturnsFalse_WhenNotFound()
    {
        using var ctx = CreateContext(nameof(ExistsAsync_ReturnsFalse_WhenNotFound));
        var repo = new GenericRepository<Department>(ctx);

        (await repo.ExistsAsync(9999)).Should().BeFalse();
    }

    [Fact]
    public async Task CountAsync_ReturnsCorrectCount()
    {
        using var ctx = CreateContext(nameof(CountAsync_ReturnsCorrectCount));
        var repo = new GenericRepository<Department>(ctx);
        await repo.AddAsync(new Department { Name = "A", IsActive = true });
        await repo.AddAsync(new Department { Name = "B", IsActive = true });
        await repo.AddAsync(new Department { Name = "C", IsActive = true });

        (await repo.CountAsync()).Should().Be(3);
    }

    [Fact]
    public async Task FindAsync_ReturnsMatchingEntities()
    {
        using var ctx = CreateContext(nameof(FindAsync_ReturnsMatchingEntities));
        var repo = new GenericRepository<Department>(ctx);
        await repo.AddAsync(new Department { Name = "Active1", IsActive = true });
        await repo.AddAsync(new Department { Name = "Inactive", IsActive = false });
        await repo.AddAsync(new Department { Name = "Active2", IsActive = true });

        var active = await repo.FindAsync(d => d.IsActive == true);

        active.Should().HaveCount(2);
        active.All(d => d.IsActive == true).Should().BeTrue();
    }
}

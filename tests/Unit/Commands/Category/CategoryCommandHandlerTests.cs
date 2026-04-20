using Moq;
using Shop.Application.Commands.Category.CreateCategory;
using Shop.Application.Commands.Category.DeleteCategory;
using Shop.Application.Commands.Category.RestoreCategory;
using Shop.Application.Commands.Category.UpdateCategoryName;
using Shop.Application.Exceptions;
using Shop.Domain.Exceptions;
using Shop.Domain.Interfaces.IFactories;
using Shop.Domain.Interfaces.IRepositories;
using Shop.Presentation.RequestDTOs;
using Shop.Tests.Helpers;
using Xunit;

namespace Shop.Tests.Unit.Commands.Category;

public class CategoryCommandHandlerTests
{
    private readonly Mock<ICategoryRepository> _repo = new();
    private readonly Mock<ICategoryFactory> _factory = new();

    [Fact]
    public async Task Create_Valid_CallsFactoryAndSaves()
    {
        var category = DomainObjectBuilder.CreateCategory("Electronics");
        _factory.Setup(f => f.CreateAsync("Electronics")).ReturnsAsync(category);
        _repo.Setup(r => r.SaveChangesAsync(default)).Returns(Task.CompletedTask);

        var handler = new CreateCategoryCommandHandler(_repo.Object, _factory.Object);
        await handler.Handle(new CreateCategoryCommand(new CategoryRequestDto("Electronics")), default);

        _repo.Verify(r => r.AddAsync(category, default), Times.Once);
        _repo.Verify(r => r.SaveChangesAsync(default), Times.Once);
    }

    [Fact]
    public async Task Create_DuplicateName_FactoryThrowsPropagates()
    {
        _factory.Setup(f => f.CreateAsync("Duplicate"))
                .ThrowsAsync(new DomainValidationException("Категорія з вказаним ім'ям вже існує."));

        var handler = new CreateCategoryCommandHandler(_repo.Object, _factory.Object);
        await Assert.ThrowsAsync<DomainValidationException>(
            () => handler.Handle(new CreateCategoryCommand(new CategoryRequestDto("Duplicate")), default));
    }

    [Fact]
    public async Task UpdateName_CategoryNotFound_ThrowsNotFoundException()
    {
        _repo.Setup(r => r.GetByIdAsync(99, default))
             .ReturnsAsync((Shop.Domain.Models.Category?)null);

        var handler = new UpdateCategoryNameCommandHandler(_repo.Object);
        await Assert.ThrowsAsync<NotFoundException>(
            () => handler.Handle(new UpdateCategoryNameCommand(99, "NewName"), default));
    }

    [Fact]
    public async Task UpdateName_NameAlreadyTaken_ThrowsDomainValidationException()
    {
        var category = DomainObjectBuilder.CreateCategory("OldName");
        var existing = DomainObjectBuilder.CreateCategory("TakenName");

        _repo.Setup(r => r.GetByIdAsync(1, default)).ReturnsAsync(category);
        _repo.Setup(r => r.GetByNameAsync("TakenName", default)).ReturnsAsync(existing);

        var handler = new UpdateCategoryNameCommandHandler(_repo.Object);
        await Assert.ThrowsAsync<DomainValidationException>(
            () => handler.Handle(new UpdateCategoryNameCommand(1, "TakenName"), default));
    }

    [Fact]
    public async Task UpdateName_ValidNewName_UpdatesAndSaves()
    {
        var category = DomainObjectBuilder.CreateCategory("OldName");
        _repo.Setup(r => r.GetByIdAsync(1, default)).ReturnsAsync(category);
        _repo.Setup(r => r.GetByNameAsync("NewName", default))
             .ReturnsAsync((Shop.Domain.Models.Category?)null);
        _repo.Setup(r => r.SaveChangesAsync(default)).Returns(Task.CompletedTask);

        var handler = new UpdateCategoryNameCommandHandler(_repo.Object);
        await handler.Handle(new UpdateCategoryNameCommand(1, "NewName"), default);

        Assert.Equal("NewName", category.CategoryName);
        _repo.Verify(r => r.SaveChangesAsync(default), Times.Once);
    }

    [Fact]
    public async Task Delete_CategoryNotFound_ThrowsNotFoundException()
    {
        _repo.Setup(r => r.GetByIdAsync(99, default))
             .ReturnsAsync((Shop.Domain.Models.Category?)null);

        var handler = new DeleteCategoryCommandHandler(_repo.Object);
        await Assert.ThrowsAsync<NotFoundException>(
            () => handler.Handle(new DeleteCategoryCommand(99), default));
    }

    [Fact]
    public async Task Delete_ExistingCategory_MarksDeleted()
    {
        var category = DomainObjectBuilder.CreateCategory();
        _repo.Setup(r => r.GetByIdAsync(1, default)).ReturnsAsync(category);
        _repo.Setup(r => r.SaveChangesAsync(default)).Returns(Task.CompletedTask);

        var handler = new DeleteCategoryCommandHandler(_repo.Object);
        await handler.Handle(new DeleteCategoryCommand(1), default);

        Assert.True(category.IsDeleted);
    }

    [Fact]
    public async Task Restore_DeletedCategory_RestoresAndSaves()
    {
        var category = DomainObjectBuilder.CreateCategory();
        category.MarkAsDeleted();
        _repo.Setup(r => r.GetByIdAsync(1, default)).ReturnsAsync(category);
        _repo.Setup(r => r.SaveChangesAsync(default)).Returns(Task.CompletedTask);

        var handler = new RestoreCategoryCommandHandler(_repo.Object);
        await handler.Handle(new RestoreCategoryCommand(1), default);

        Assert.False(category.IsDeleted);
    }
}
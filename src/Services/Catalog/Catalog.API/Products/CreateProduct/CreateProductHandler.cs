﻿namespace Catalog.API.Products.CreateProduct;

public record CreateProductCommand(
    string Name,
    List<string> Category,
    string Description,
    string ImageFile,
    decimal Price)
    : ICommand<CreateProductResault>;
public record CreateProductResault(Guid Id);

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required.");
        RuleFor(x => x.Category).NotEmpty().WithMessage("Category is required.");
        RuleFor(x => x.ImageFile).NotEmpty().WithMessage("ImageFile is required.");
        RuleFor(x => x.Price).GreaterThan(  0).WithMessage("Price must be greater than 0.");
    }
}

internal class CreateProductCommandHandler(IDocumentSession session)
    : ICommandHandler<CreateProductCommand, CreateProductResault>
{
    public async Task<CreateProductResault> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
        // Create product entity from command object.
        var product = new Product
        {
            Name = command.Name,
            Category = command.Category,
            Description = command.Description,
            ImageFile = command.ImageFile,
            Price = command.Price,
        };

        // Save to database.
        session.Store(product);
        await session.SaveChangesAsync(cancellationToken);

        // Return CreateProductResault result.
        return new CreateProductResault(product.Id);
    }
}

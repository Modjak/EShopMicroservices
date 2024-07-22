namespace Catalog.API.Products.CreateProduct;


public record CreateProductCommand(string Name, List<string> Category, string Description, string ImageFile, decimal Price) 
    : ICommand<CreateProductResult>;
public record CreateProductResult(Guid Id);


// Vertical slcie architecture
//cqrs library to execute the commands or queries
//with mediator receives a request and process it, encapsulates business logic

internal class CreateProductCommandHandler(IDocumentSession session) 
    : ICommandHandler<CreateProductCommand, CreateProductResult>
{
    public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
        //Business logic to create a product

        //create product entity froma command object
        //save to db
        //return CreateProductResult result

        var product = new Product
        {
            Name = command.Name,
            Category = command.Category,
            Description = command.Description,
            ImageFile = command.ImageFile,
            Price = command.Price
        };

        // TODO save to db   

        session.Store(product);
        await session.SaveChangesAsync(cancellationToken);

        //return result

        return new CreateProductResult(product.Id);

    }
}

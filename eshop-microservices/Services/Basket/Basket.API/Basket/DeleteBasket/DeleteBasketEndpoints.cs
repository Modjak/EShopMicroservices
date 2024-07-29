namespace Basket.API.Basket.DeleteBasket;

//public record DeleteBasketRequest(string UserName);

public record DeleteBasketResponse(bool IsSuccess);
public class DeleteBasketEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/basket/{userName}", async (string userName, ISender Sender) =>
        {
            var command = new DeleteBasketCommand(userName);

            var result = await Sender.Send(command);

            var response = result.Adapt<DeleteBasketResponse>();

            return Results.Ok(response);
        }).WithName("DeleteProduct")
        .Produces<DeleteBasketResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Delete Product")
        .WithDescription("Delete Product");


    }
}

namespace Basket.API.Basket.StoreBasket;

public record StoreBasketRequest(ShoppingCart Cart);
public record StoreBasketEndpointResponse(string UserName);

public class StoreBasketEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/basket", async (ISender sender, StoreBasketRequest request) =>
        {
            var command = request.Adapt<StoreBasketCommand>();

            var result = sender.Send(command);

            var response = result.Adapt<StoreBasketEndpointResponse>();

            return Results.Created($"/basket/{response.UserName}", response);
        }).WithName("Store Product in Basket")
        .Produces<StoreBasketEndpointResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Store Product in Basket")
        .WithDescription("Store Product in Basket");
    }
}

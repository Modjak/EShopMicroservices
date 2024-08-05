using Discount.Grpc;
using Grpc.Net.Client;

namespace Basket.API.Basket.StoreBasket;

public record StoreBasketCommand(ShoppingCart Cart) : ICommand<StoreBasketResult>;

public record StoreBasketResult(string UserName);

public class StoreBasketCommandValidator : AbstractValidator<StoreBasketCommand>
{
    public StoreBasketCommandValidator()
    {
        RuleFor(x => x.Cart).NotNull().WithMessage("Cart can not be null");
        RuleFor(x => x.Cart.UserName).NotEmpty().WithMessage("UserName is required");
    }
}
public class StoreBasketCommandHandler 
    //(IBasketRepository repository, DiscountProtoService.DiscountProtoServiceClient discountProto)
    : ICommandHandler<StoreBasketCommand, StoreBasketResult>
{
    private readonly IBasketRepository _repository;
    private readonly DiscountProtoService.DiscountProtoServiceClient _discountProto;

    public StoreBasketCommandHandler(IBasketRepository repository)
    {
        _repository = repository;

        // Create a custom HttpClientHandler that disables SSL certificate validation
        var handler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
        };

        // Create the gRPC channel using the custom handler
        var channel = GrpcChannel.ForAddress("https://localhost:5052", new GrpcChannelOptions
        {
            HttpHandler = handler
        });

        // Create the discountProto client using the custom channel
        _discountProto = new DiscountProtoService.DiscountProtoServiceClient(channel);
    }

    public async Task<StoreBasketResult> Handle(StoreBasketCommand command, CancellationToken cancellationToken)
    {

        await DeductDiscount(command.Cart, cancellationToken);

        await _repository.StoreBasket(command.Cart, cancellationToken);

        return new StoreBasketResult(command.Cart.UserName);
    }

    private async Task DeductDiscount(ShoppingCart cart, CancellationToken cancellationToken)
    {

        foreach (var item in cart.Items)
        {
            var coupon = await _discountProto.GetDiscountAsync(new GetDiscountRequest
            {
                ProductName = item.ProductName
            }, cancellationToken: cancellationToken);

            item.Price -= coupon.Amount;
        }
    }
}

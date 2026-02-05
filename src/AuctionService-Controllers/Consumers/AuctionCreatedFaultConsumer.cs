using Contracts;
using MassTransit;

namespace AuctionService_Controllers.Consumers
{
    public class AuctionCreatedFaultConsumer : IConsumer<Fault<AuctionCreated>>
    {
        public async Task Consume(ConsumeContext<Fault<AuctionCreated>> context)
        {
            System.Console.WriteLine("Fault received for AuctionCreated event:");

            var exception = context.Message.Exceptions.First();
            if (exception.ExceptionType == "System.ArgumentException")
            {
                context.Message.Message.Model = "FooBar";
                await context.Publish<AuctionCreated>(context.Message.Message);
                System.Console.WriteLine("--> Retried AuctionCreated event with Model set to 'FooBar'");
            }
            else
            {
                System.Console.WriteLine($"--> Exception Type: {exception.ExceptionType}");
                System.Console.WriteLine($"--> Message: {exception.Message}");
            }

        }

    }
}
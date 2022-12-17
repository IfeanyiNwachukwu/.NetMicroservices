using MassTransit;
using MessagingInterfacesConstants.Commands;
using System.Threading.Tasks;

namespace OrderAPI.Messages.Consumers
{
    public class RegisterOrderCommandConsumer : IConsumer<IRegisterOrderCommand>
    {
        public Task Consume(ConsumeContext<IRegisterOrderCommand> context)
        {
            throw new System.NotImplementedException();
        }
    }
}

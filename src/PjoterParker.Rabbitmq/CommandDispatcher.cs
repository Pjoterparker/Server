using System;
using System.Threading.Tasks;
using Autofac;
using FluentValidation;
using MassTransit;
using PjoterParker.Core.Commands;
using PjoterParker.Core.Validation;

namespace PjoterParker.Rabbitmq
{
    public class CommandDispatcher : ICommandDispatcher
    {
        private readonly IBusControl _bus;
        private readonly IComponentContext _context;

        public CommandDispatcher(
            IComponentContext context,
            IBusControl bus)
        {
            _context = context;
            _bus = bus;
        }

        public async Task DispatchAsync<TCommand>(TCommand command) where TCommand : class, ICommand
        {
            if (_context.TryResolve(out AppAbstractValidation<TCommand> validator))
            {
                var validationResult = validator.Validate(command);
                if (!validationResult.IsValid)
                {
                    throw new ValidationException(validationResult.Errors);
                }
            }

            var endpoint = await _bus.GetSendEndpoint(new Uri($"rabbitmq://{_bus.Address.Host}/commands"));
            await endpoint.Send(command, ctx => { ctx.CorrelationId = Guid.NewGuid(); });
        }
    }
}
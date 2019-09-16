using System.Threading.Tasks;
using Autofac;
using FluentValidation;
using PjoterParker.Core.Aggregates;
using PjoterParker.Core.Events;
using PjoterParker.Core.Validation;

namespace PjoterParker.Core.Commands
{
    public class CommandDispatcher : ICommandDispatcher
    {
        private readonly ICommandFactory _commandFactory;

        private readonly IComponentContext _context;

        private readonly IEventFactory _eventFactory;

        private readonly IAggregateStore _aggregateStore;

        public CommandDispatcher(
            IComponentContext context,
            ICommandFactory commandFactory,
            IEventFactory eventFactory,
            IAggregateStore aggregateStore)
        {
            _context = context;
            _commandFactory = commandFactory;
            _eventFactory = eventFactory;
            _aggregateStore = aggregateStore;
        }

        public async Task DispatchAsync<TCommand>(TCommand command) where TCommand : ICommand
        {
            CommandComposite commandComposite = _commandFactory.Make(command);

            if (_context.TryResolve(out AppAbstractValidation<TCommand> validator))
            {
                var validationResult = validator.Validate(command);
                if (!validationResult.IsValid)
                {
                    throw new ValidationException(validationResult.Errors);
                }
            }

            var handler = _context.Resolve<ICommandHandlerAsync<TCommand>>();
            IAggregateRoot aggregate = await handler.ExecuteAsync(command).ConfigureAwait(false);

            _eventFactory.Make(commandComposite, aggregate.Events);
            await _aggregateStore.SaveAsync(aggregate);
        }
    }
}
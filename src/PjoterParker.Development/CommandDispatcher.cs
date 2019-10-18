using System.Collections.Generic;
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

        private readonly IEventDispatcher _eventDispatcher;

        private readonly IAggregateStore _aggregateStore;

        private readonly IEventFactory _eventFactory;

        public CommandDispatcher(
            IComponentContext context,
            ICommandFactory commandFactory,
            IEventFactory eventFactory,
            IEventDispatcher eventDispatcher,
            IAggregateStore aggregateStore)
        {
            _context = context;
            _commandFactory = commandFactory;
            _eventFactory = eventFactory;
            _eventDispatcher = eventDispatcher;
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
            IAggregateRoot aggregateRoot = await handler.ExecuteAsync(command); 

            _eventFactory.Make(commandComposite, aggregateRoot.Events);
            await aggregateRoot.CommitAsync(_aggregateStore);
            foreach (var @event in aggregateRoot.Events)
            {
                await _eventDispatcher.DispatchAsync(@event.Event);
            }
        }
    }
}
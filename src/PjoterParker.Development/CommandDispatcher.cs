using System.Collections.Generic;
using System.Threading.Tasks;
using Autofac;
using FluentValidation;
using PjoterParker.Core.Events;
using PjoterParker.Core.Validation;

namespace PjoterParker.Core.Commands
{
    public class CommandDispatcher : ICommandDispatcher
    {
        private readonly ICommandFactory _commandFactory;

        private readonly IComponentContext _context;

        private readonly IEventDispatcher _eventDispatcher;

        private readonly IEventFactory _eventFactory;

        public CommandDispatcher(
            IComponentContext context,
            ICommandFactory commandFactory,
            IEventFactory eventFactory,
            IEventDispatcher eventDispatcher)
        {
            _context = context;
            _commandFactory = commandFactory;
            _eventFactory = eventFactory;
            _eventDispatcher = eventDispatcher;
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
            IEnumerable<EventComposite> events = await handler.ExecuteAsync(command).ConfigureAwait(false);

            _eventFactory.Make(commandComposite, events);
            foreach (var @event in events)
            {
                await _eventDispatcher.DispatchAsync(@event.Event);
            }
        }
    }
}
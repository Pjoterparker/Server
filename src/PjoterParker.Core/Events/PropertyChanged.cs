using System;
using System.Collections.Generic;
using System.Text;

namespace PjoterParker.Core.Events
{
    public class PropertyChanged<TEntity> : IEvent
    {
        public PropertyChanged(Guid aggregateId, string aggregateName, string typeName, string propertyName, string oldValue, string newValue)
        {
            AggregateId = aggregateId;
            AggregateName = aggregateName;
            TypeName = typeName;
            PropertyName = propertyName;
            OldValue = oldValue;
            NewValue = newValue;
        }

        public Guid AggregateId { get; set; }

        public string AggregateName { get; set; }

        public Guid CommandId { get; set; }

        public Guid Id { get; set; }

        public string NewValue { get; set; }

        public string OldValue { get; set; }

        public string PropertyName { get; set; }

        public string TypeName { get; set; }
    }
}

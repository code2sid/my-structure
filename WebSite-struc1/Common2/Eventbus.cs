using System.Reactive.Subjects;


namespace Common2
{
    public class EventBus
    {
        public Subject<object> MappingItemsChanged { get; } = new Subject<object>();
        public Subject<int> RequestCreated { get; } = new Subject<int>();
        public Subject<int> DueDateUpdated { get; } = new Subject<int>();
    }
}

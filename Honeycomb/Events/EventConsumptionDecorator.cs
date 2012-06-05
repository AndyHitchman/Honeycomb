namespace Honeycomb.Events
{
    using System;
    using System.ComponentModel;

    /// <summary>
    /// Provides a wrapper that intercepts events before and after consumption.
    /// </summary>
    public interface EventConsumptionDecorator<TConsumer,TEvent> where TConsumer : ConsumesEvent<TEvent> where TEvent : Event
    {
        /// <summary>
        /// Called for each consumer of the event, before consumption. Cancelling will prevent the consumer from running.
        /// </summary>
        /// <remarks>
        /// If the consumer is cancelled, then <see cref="AfterConsumption"/> is not called.</remarks>
        /// <param name="uniqueEvent"></param>
        /// <param name="consumer"></param>
        /// <param name="eventArgs"></param>
        void BeforeConsumption(
            UniqueEvent<TEvent> uniqueEvent,
            ConsumesEvent<TEvent> consumer,
            CancelEventArgs eventArgs);

        /// <summary>
        /// Called for each consumer of the event, after consumption. 
        /// </summary>
        /// <remarks>
        /// If an unhandled exception is propagated from the consumer, <see cref="AfterConsumption"/> will be called instead of this method.</remarks>
        /// <param name="uniqueEvent"></param>
        /// <param name="consumer"></param>
        void AfterConsumption(UniqueEvent<TEvent> uniqueEvent, ConsumesEvent<TEvent> consumer);

        /// <summary>
        /// Called for each consumer of the event, after consumption that propagates an exception. 
        /// </summary>
        /// <param name="uniqueEvent"></param>
        /// <param name="consumer"></param>
        /// <param name="eventArgs"></param>
        void AfterFailedConsumption(UniqueEvent<TEvent> uniqueEvent, ConsumesEvent<TEvent> consumer, UnhandledExceptionEventArgs eventArgs);
    }
}
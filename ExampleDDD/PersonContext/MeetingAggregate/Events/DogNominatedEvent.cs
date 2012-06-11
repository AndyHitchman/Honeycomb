using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Honeycomb.Events;

namespace RacingContext.MeetingAggregate.Events
{
    public class DogNominatedEvent : Event
    {
        public DogNominatedEvent(Guid dogId, int trackDistance)
        {
            DogId = dogId;
            TrackDistance = trackDistance;
        }

        public DogNominatedEvent(Guid dogId, Guid advertisedEventId)
        {
            DogId = dogId;
            AdvertisedEventId = advertisedEventId;
        }

        public Guid DogId { get; private set; }

        public int? TrackDistance { get; private set; }

        public Guid? AdvertisedEventId { get; private set; }
    }
}

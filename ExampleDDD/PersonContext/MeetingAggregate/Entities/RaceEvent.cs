using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DDD;
using RacingContext.AdvertisedEventAggregate;
using RacingContext.RaceTypeAggregate;

namespace RacingContext.MeetingAggregate.Entities
{
    internal class RaceEvent : Entity<Meeting>
    {
        protected AdvertisedEvent advertisedEvent;
        protected RaceType raceType;
        protected int distanceInMetres;

        public RaceEvent(Meeting aggregateRoot, Guid id, RaceType raceType, int distanceInMetres) : base(aggregateRoot, id)
        {
            this.raceType = raceType;
            this.distanceInMetres = distanceInMetres;
        }

        public RaceEvent(Meeting aggregateRoot, Guid id, AdvertisedEvent advertisedEvent)
            : base(aggregateRoot, id)
        {
            this.advertisedEvent = advertisedEvent;
        }
    }
}

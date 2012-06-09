using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DDD;
using RacingContext.DogAggregate;
using RacingContext.PersonAggregate.Entities;

namespace RacingContext.MeetingAggregate.Entities
{
    internal class Nomination : Entity<Meeting>
    {
        protected Dog dog;
        protected RaceEvent raceEvent;

        public Nomination(Meeting aggregateRoot, Guid id, Dog dog, RaceEvent raceEvent) : base(aggregateRoot, id)
        {
            this.dog = dog;
            this.raceEvent = raceEvent;
        }
    }
}

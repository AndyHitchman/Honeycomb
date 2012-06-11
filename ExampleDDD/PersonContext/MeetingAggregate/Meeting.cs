using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DDD;
using Honeycomb;
using Microsoft.Practices.ServiceLocation;
using RacingContext.AdvertisedEventAggregate;
using RacingContext.DogAggregate;
using RacingContext.MeetingAggregate.Entities;
using RacingContext.RaceTypeAggregate;
using RacingContext.TrackAggregate;

namespace RacingContext.MeetingAggregate
{
    internal class Meeting : AggregateRoot
    {
        private readonly Track track;

        protected ICollection<Nomination> nominations;
        private IdentityGenerator identityGenerator;

        public Meeting(Track track)
        {
            this.track = track;
            identityGenerator = ServiceLocator.Current.GetInstance<IdentityGenerator>();
        }

        public bool AddNomination(Dog dog, RaceType raceType, int distanceInMetres)
        {
            if (!isValidDistanceForTrack())
            {
                return false;
            }

            var raceEvent = GetExistingRaceEvent(raceType, distanceInMetres) ??
                            new RaceEvent(this, identityGenerator.NewId(), raceType, distanceInMetres);

            nominations.Add(new Nomination(this, identityGenerator.NewId(), dog, raceEvent));
            return true;
        }

        public bool AddNomination(Dog dog, AdvertisedEvent advertisedEvent)
        {
            if (!isValidDistanceForTrack())
            {
                return false;
            }

            var raceEvent = GetExistingRaceEvent(advertisedEvent) ??
                            new RaceEvent(this, identityGenerator.NewId(), advertisedEvent);

            nominations.Add(new Nomination(this, identityGenerator.NewId(), dog, raceEvent));
            return true;
        }

        private bool isValidDistanceForTrack()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This will retrieve an existing race event with the same raceType, distanceInMetres and share an aggregate root.        
        /// </summary>
        /// <param name="raceType"></param>
        /// <param name="distanceInMetres"></param>
        /// <returns>The race event if it exists, otherwise null</returns>
        public RaceEvent GetExistingRaceEvent(RaceType raceType, int distanceInMetres)
        {            
            throw new NotImplementedException();
        }

        /// <summary>
        /// This will retrieve an existing race event with the same Advertised Event and share an aggregate root.        
        /// </summary>
        /// <param name="advertisedEvent"></param>
        /// <returns>The race event if it exists, otherwise null</returns>
        public RaceEvent GetExistingRaceEvent(AdvertisedEvent advertisedEvent)
        {
            throw new NotImplementedException();
        }
    }
}

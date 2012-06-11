using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Honeycomb.Commands;

namespace ApplicationLayer.Meeting.Commands
{
    public class NominateDogCommand : Command
    {        
        public Guid MeetingId { get; set; }

        public Guid TrainerId { get; set; }

        public ICollection<NominateDogOption> NominationOptions { get; set; }

        public class NominateDogOption
        {
            public NominateDogOption(int distanceInMetres)
            {
                DistanceInMetres = distanceInMetres;
            }

            public NominateDogOption(Guid advertisedEventId)
            {
                AdvertisedEventId = advertisedEventId;
            }

            public Guid DogId { get; set; }

            public int? DistanceInMetres { get; private set; }

            public Guid? AdvertisedEventId { get; private set; }
        }
    }
}

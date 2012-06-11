using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Honeycomb.Projections;

namespace ApplicationLayer.Meeting.Projections
{
    public class NominateDogsProjection : Projection
    {
        public static string ProjectionIdentifier(Guid meetingId, Guid trainerId)
        {
            return typeof(NominateDogsProjection).Name + meetingId.ToString() + trainerId;
        }

        public string Identifier()
        {
            return ProjectionIdentifier(MeetingId, TrainerId);
        }

        public Guid TrainerId { get; set; }

        public Guid MeetingId { get; set; }

        public ICollection<EligibleDogProjection> EligibleDogs { get; set; }

        public class EligibleDogProjection
        {
            public string DogName { get; set; }

            public Guid DogId { get; set; }

            public IEnumerable<int> EligibleTrackDistances { get; set; }

            public IEnumerable<AdvertisedEventProjection> EligibleAdvertisedEvents { get; set; } 

            public class AdvertisedEventProjection
            {
                public Guid AdvertisedEventId { get; set; }

                public string EventName { get; set; }
            }
        }
    }
}

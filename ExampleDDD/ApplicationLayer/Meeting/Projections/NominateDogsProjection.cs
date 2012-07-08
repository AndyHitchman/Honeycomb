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

        public NominateDogsProjection(MeetingEligibilityProjection meetingEligibilityProjection, TrainersDogsEligibilityProjection trainersDogsEligibilityProjection)
        {
            TrainerId = trainersDogsEligibilityProjection.TrainerId;
            MeetingId = meetingEligibilityProjection.MeetingId;

            foreach (var dog in trainersDogsEligibilityProjection.DogsForTrainer)
            {
                var newEligibleDog = new EligibleDogProjection();

                newEligibleDog.EligibleTrackDistances = meetingEligibilityProjection.Distances;
                newEligibleDog.DogId = dog.DogId;
                newEligibleDog.DogName = dog.DogName;
                newEligibleDog.EligibleAdvertisedEvents = new List<EligibleDogProjection.AdvertisedEventProjection>();
                
                foreach (var advertisedEventDetailse in meetingEligibilityProjection.AdvertisedEventsAtMeeting)
                {
                    var isEligible =
                        (advertisedEventDetailse.MaximumTotalPrize.HasValue && dog.TotalPrizeMoney <= advertisedEventDetailse.MaximumTotalPrize) &&
                        (advertisedEventDetailse.MinimumTotalPrize.HasValue && dog.TotalPrizeMoney >= advertisedEventDetailse.MinimumTotalPrize) &&
                        (!advertisedEventDetailse.IsBitchOnly || !dog.IsMale) && 
                        (!advertisedEventDetailse.IsDogOnly || dog.IsMale) &&
                        (dog.GradeAtTrackDistances.Any(
                            _ =>
                            _.Distance == advertisedEventDetailse.DistanceInMetres &&
                            advertisedEventDetailse.EligibleGrades.Contains(_.DogGrade)));

                    if (isEligible)
                    {
                        newEligibleDog.EligibleAdvertisedEvents.Add(new EligibleDogProjection.AdvertisedEventProjection(){AdvertisedEventId = advertisedEventDetailse.AdvertisedEventId, EventName = advertisedEventDetailse.EventName});
                    }
                }

                EligibleDogs.Add(newEligibleDog);
            }
        }

        public Guid TrainerId { get; set; }

        public Guid MeetingId { get; set; }

        public ICollection<EligibleDogProjection> EligibleDogs { get; set; }

        public class EligibleDogProjection
        {
            public string DogName { get; set; }

            public Guid DogId { get; set; }

            public IEnumerable<int> EligibleTrackDistances { get; set; }

            public ICollection<AdvertisedEventProjection> EligibleAdvertisedEvents { get; set; } 

            public class AdvertisedEventProjection
            {
                public Guid AdvertisedEventId { get; set; }

                public string EventName { get; set; }
            }
        }
    }
}

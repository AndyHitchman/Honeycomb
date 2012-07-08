using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Honeycomb.Projections;

namespace ApplicationLayer.Meeting.Projections
{
    public class TrainersDogsEligibilityProjection : PersistentProjection
    {
        public static string ProjectionIdentifier(Guid trainerId)
        {
            return typeof (TrainersDogsEligibilityProjection).Name + trainerId;
        }

        public string Identifier()
        {
            return ProjectionIdentifier(TrainerId);
        }

        public Guid TrainerId { get; set; }

        public IEnumerable<DogDetails> DogsForTrainer { get; set; }

        public class DogDetails
        {
            public Guid DogId { get; set; }

            public string DogName { get; set; }

            /// Various properties that are needed for advertised event eligibility
            public int TotalPrizeMoney { get; set; }

            public bool IsMale { get; set; }

            public IEnumerable<GradeAtTrackDistance> GradeAtTrackDistances { get; set; }

            public class GradeAtTrackDistance
            {
                public Guid TrackId { get; set; }

                public int Distance { get; set; }

                public int DogGrade { get; set; }
            }    
        }
    }
}

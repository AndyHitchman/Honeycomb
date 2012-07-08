using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Honeycomb.Projections;

namespace ApplicationLayer.Meeting.Projections
{
    public class MeetingEligibilityProjection : PersistentProjection
    {
        public static string ProjectionIdentifier(Guid meetingId)
        {
            return typeof (MeetingEligibilityProjection).Name + meetingId;
        }

        public string Identifier()
        {
            return ProjectionIdentifier(MeetingId);
        }

        public Guid MeetingId { get; set; }

        public Guid TrackId { get; set; }

        public IEnumerable<int> Distances { get; set; }

        public IEnumerable<AdvertisedEventDetails> AdvertisedEventsAtMeeting { get; set; }

        public class AdvertisedEventDetails
        {
            public Guid AdvertisedEventId { get; set; }

            public string EventName { get; set; }

            public bool IsDogOnly { get; set; }

            public bool IsBitchOnly { get; set; }

            public int? MinimumTotalPrize { get; set; }

            public int? MaximumTotalPrize { get; set; }

            public int DistanceInMetres { get; set; }

            public IEnumerable<int> EligibleGrades { get; set; }            
        }
    }
}

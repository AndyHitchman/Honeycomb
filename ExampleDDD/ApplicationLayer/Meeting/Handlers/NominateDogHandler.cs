using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ApplicationLayer.Meeting.Commands;
using ApplicationLayer.Meeting.Projections;
using Honeycomb.Commands;
using Honeycomb.Events;
using Honeycomb.Projections;
using RacingContext.MeetingAggregate.Events;

namespace ApplicationLayer.Meeting.Handlers
{
    public class NominateDogHandler : HandlesCommand<NominateDogCommand>
    {
        private readonly ProjectionStore projectionStore;

        public NominateDogHandler(ProjectionStore projectionStore)
        {
            this.projectionStore = projectionStore;
        }

        public CommandApplication TryApply(NominateDogCommand command)
        {
            // Get the projection and validate 
            if (!validateCommand(command))
            {
                return CommandApplication.Rejected;
            }

            foreach (var nominateDogOption in command.NominationOptions)
            {
                var @event = nominateDogOption.AdvertisedEventId.HasValue
                                 ? new DogNominatedEvent(nominateDogOption.DogId, nominateDogOption.AdvertisedEventId.Value)
                                 : new DogNominatedEvent(nominateDogOption.DogId, nominateDogOption.DistanceInMetres.Value);
                @event.Raise();
            }

            return CommandApplication.Accepted;
        }

        private bool validateCommand(NominateDogCommand command)
        {
            var nominationProjection =
                projectionStore.RetrieveProjection<NominateDogsProjection>(
                    NominateDogsProjection.ProjectionIdentifier(command.MeetingId, command.TrainerId));

            foreach (var nominationOption in command.NominationOptions)
            {
                var isValidNomination =
                    nominationProjection.EligibleDogs.Any(elligibleDog =>
                                                          elligibleDog.DogId == nominationOption.DogId &&
                                                          ((nominationOption.AdvertisedEventId.HasValue &&
                                                            elligibleDog.EligibleAdvertisedEvents.Any(eae =>
                                                                                                      eae.AdvertisedEventId ==
                                                                                                      nominationOption.
                                                                                                          AdvertisedEventId)) ||
                                                           (nominationOption.DistanceInMetres.HasValue &&
                                                            elligibleDog.EligibleTrackDistances.Contains(
                                                                nominationOption.DistanceInMetres.Value))));
                if (!isValidNomination)
                {
                    return false;
                }
            }

            return true;
        }
    }
}

using SampleApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleApi.DAL
{
    public interface IItineraryDAO
    {
        Itinerary GetItinerary(int id);

        List<Itinerary> GetAllItinerary();

        bool CreateItinerary(Itinerary itinerary);

        bool DeleteItinerary(int id);

        bool UpdateItinerary(Itinerary itinerary);
    }
}

using SampleApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleApi.DAL
{
    public interface ILandmarkDAO
    {
        Landmark GetLandmark(int id);

        bool AddLandmark(Landmark landmark);

        IList<Landmark> GetAllLandmarks();

        IList<Landmark> GetUnapprovedLandmarks();

        IList<Landmark> GetApprovedLandmarks();

        bool DeleteLandmark(int id);

        bool UpdateLandmark(Landmark landmark);
    }
}

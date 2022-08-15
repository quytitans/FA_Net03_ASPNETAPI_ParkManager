﻿using Parky2API.Model;
using System.Diagnostics;
using Parky2API.Model.Dtos;

namespace Parky2API.Repository.IRepository
{
    public interface ITrailRepository
    {
        ICollection<Trail> GetTrails();
        ICollection<Trail> GetTrailsInNationalPark(int id);
        Trail GetTrail(int id);
        bool TrailExists(string trailName);
        bool TrailExists(int id);
        bool CreateTrail(Trail trail);
        bool UpdateTrail(Trail trail);
        bool DeleteTrail(Trail trail);
        bool Save();
    }
}

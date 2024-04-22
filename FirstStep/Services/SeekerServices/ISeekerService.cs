﻿using FirstStep.Models;
using FirstStep.Models.DTOs;

namespace FirstStep.Services
{
    public interface ISeekerService
    {
        public Task<IEnumerable<Seeker>> GetAll();

        public Task<Seeker> GetById(int id);

        public Task<SeekerApplicationDto> GetSeekerDetails(int seekerID);

        public Task Create(AddSeekerDto newSeeker);

        public Task Update(int seekerID, Seeker seeker);

        public Task Delete(int id);
    }
}

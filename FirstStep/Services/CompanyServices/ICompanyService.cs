﻿using FirstStep.Models;
using FirstStep.Models.DTOs;

namespace FirstStep.Services
{
    public interface ICompanyService
    {
        public Task<IEnumerable<Company>> GetAll();


        public Task<IEnumerable<Company>> GetAllUnregisteredCompanies();

        public Task<IEnumerable<Company>> GetAllRegisteredCompanies();

        public Task<Company> FindByID(int id);

        public Task<CompanyProfileDetailsDto> GetById(int id);
        
        //get comapny list for system Admin
        public Task<IEnumerable<ViewCompanyListDto>> GetAllCompanyList();
        //get company application
        public Task<CompanyApplicationDto> GetCompanyApplicationById(int companyID);
        
        public Task<CompanyProfileDto> GetCompanyProfile(int companyID);

        public Task Create(AddCompanyDto company);

        public Task RegisterCompany(int companyID, AddDetailsCompanyDto company);

        public Task UpdateCompanyVerification(int companyID, CompanyRegInfoDto companyRegInfo);

        public Task UpdateUnregisteredCompany(int companyID, UpdateUnRegCompanyDto company);

        public Task UpdateRegisteredCompany(int companyID, UpdateCompanyDto company);

        public Task Delete(int id);

        public Task<bool> IsRegistered(int companyID);
    }
}

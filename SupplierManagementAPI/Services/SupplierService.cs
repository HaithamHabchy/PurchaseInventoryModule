using AutoMapper;
using Common.Shared.CustomExceptions;
using Common.Shared.DTOs;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories.IRepositories;
using SupplierManagementAPI.Services.IServices;

namespace SupplierManagementAPI.Services
{
    // Service class responsible for handling operations related to supplier.
    public class SupplierService : ISupplierService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public SupplierService(IMapper mapper,
                               IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        // Method to get a supplier by ID.
        public async Task<SupplierDTO> GetSupplierByIdAsync(int id)
        {
            ValidateSupplierId(id);

            Supplier supplier = await FetchSupplierFromDb(id);

            return _mapper.Map<SupplierDTO>(supplier);
        }

        // Method to create a new supplier.
        public async Task<SupplierDTO> CreateSupplierAsync(SupplierCreateDTO createDTO)
        {
            await CheckSupplierData(createDTO);

            Supplier supplier = _mapper.Map<Supplier>(createDTO);

            await _unitOfWork.Supplier.CreateAsync(supplier);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<SupplierDTO>(supplier);
        }

        // Method to delete a supplier by ID.
        public async Task DeleteSupplierAsync(int id)
        {
            ValidateSupplierId(id);

            Supplier supplier = await FetchSupplierFromDb(id);

            await _unitOfWork.Supplier.DeleteAsync(supplier);
            await _unitOfWork.SaveAsync();
        }

        // Method to update a supplier's information.
        public async Task UpdateSupplierAsync(int id, SupplierUpdateDTO updateDTO)
        {
            ValidateSupplierId(id);

            Supplier supplier = await FetchSupplierFromDb(id);

            await CheckSupplierDataForUpdate(id, updateDTO);

            supplier.Name = updateDTO.Name ?? supplier.Name;
            supplier.Email = updateDTO.Email ?? supplier.Email;
            supplier.PhoneNumber = updateDTO.PhoneNumber ?? supplier.PhoneNumber;
            supplier.Address = updateDTO.Address ?? supplier.Address;

            await _unitOfWork.Supplier.UpdateAsync(supplier);
            await _unitOfWork.SaveAsync();
        }

        // Method to check if supplier data is valid when creating a new supplier.
        private async Task CheckSupplierData(SupplierCreateDTO createDTO)
        {
            List<string> errorMessages = new List<string>();

            bool emailExists = await _unitOfWork.Supplier.GetAsync(u => u.Email.ToLower() == createDTO.Email.ToLower()) != null;
            if (emailExists)
            {
                errorMessages.Add("Email already registered");
            }

            bool phoneExists = await _unitOfWork.Supplier.GetAsync(u => u.PhoneNumber.ToLower() == createDTO.PhoneNumber.ToLower()) != null;
            if (phoneExists)
            {
                errorMessages.Add("Phone number already registered");
            }

            if (errorMessages.Count > 0)
            {
                throw new ApiBadRequestException(errorMessages);
            }
        }

        // Method to check supplier data for updates.
        private async Task CheckSupplierDataForUpdate(int id, SupplierUpdateDTO updateDTO)
        {
            List<string> errorMessages = new List<string>();

            if (!string.IsNullOrEmpty(updateDTO.Email))
            {
                bool emailExists = await _unitOfWork.Supplier.GetAsync(u => u.Email.ToLower() == updateDTO.Email.ToLower() && u.Id != id) != null;
                if (emailExists)
                {
                    errorMessages.Add("Email already registered");
                }
            }

            if (!string.IsNullOrEmpty(updateDTO.PhoneNumber))
            {
                bool phoneExists = await _unitOfWork.Supplier.GetAsync(u => u.PhoneNumber.ToLower() == updateDTO.PhoneNumber.ToLower() && u.Id != id) != null;
                if (phoneExists)
                {
                    errorMessages.Add("Phone number already registered");
                }
            }

            if (errorMessages.Count > 0)
            {
                throw new ApiBadRequestException(errorMessages);
            }
        }

        // Method to fetch a supplier from the database.
        private async Task<Supplier> FetchSupplierFromDb(int id)
        {
            Supplier supplier = await _unitOfWork.Supplier.GetAsync(u => u.Id == id);
            if (supplier == null)
            {
                throw new ApiNotFoundException(new List<string> { "Supplier ID not found." });
            }

            return supplier;
        }

        // Method to validate that the supplier ID is valid.
        private static void ValidateSupplierId(int id)
        {
            if (id <= 0)
            {
                throw new ApiBadRequestException(new List<string> { "Invalid supplier ID." });
            }
        }
    }
}

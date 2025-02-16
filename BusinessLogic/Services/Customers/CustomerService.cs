using BusinessLogic.Interfaces;
using BusinessLogic.Models;
using CommerceEntities.Entities;
using DataAccess.Interfaces;
using log4net;

namespace BusinessLogic.Services.Customers
{
    public class CustomerService : ICustomerService
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(CustomerService));
        private readonly ICustomerRepository _customerRepository;

        public CustomerService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<IEnumerable<CustomerModel>> GetAllCustomersAsync()
        {
            _logger.Info("Fetching all customers from service.");
            var customers = await _customerRepository.GetAllCustomersAsync();

            // Convert Entities → Business Models
            return customers.Select(c => new CustomerModel
            {
                Id = c.Id,
                FirstName = c.FirstName,
                LastName = c.LastName,
                Email = c.Email,
                CreatedAt = c.CreatedAt
            }).ToList();
        }

        public async Task<CustomerModel> GetCustomerByIdAsync(int id)
        {
            _logger.Info($"Fetching customer with ID: {id} from service.");
            var customer = await _customerRepository.GetCustomerByIdAsync(id);

            return customer == null ? null : new CustomerModel
            {
                Id = customer.Id,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Email = customer.Email,
                CreatedAt = customer.CreatedAt
            };
        }

        public async Task AddCustomerAsync(CustomerModel customerModel)
        {
            _logger.Info("Adding new customer from service.");

            // Convert Business Model → Entity
            var customerEntity = new Customer
            {
                FirstName = customerModel.FirstName,
                LastName = customerModel.LastName,
                Email = customerModel.Email,
                CreatedAt = customerModel.CreatedAt
            };

            await _customerRepository.AddCustomerAsync(customerEntity);

            // Assign the generated ID to the model
            customerModel.Id = customerEntity.Id;
        }

        public async Task UpdateCustomerAsync(CustomerModel customerModel)
        {
            _logger.Info($"Updating customer with ID: {customerModel.Id} from service.");

            // Convert Business Model → Entity
            var customerEntity = new Customer
            {
                Id = customerModel.Id,
                FirstName = customerModel.FirstName,
                LastName = customerModel.LastName,
                Email = customerModel.Email,
                CreatedAt = customerModel.CreatedAt
            };

            await _customerRepository.UpdateCustomerAsync(customerEntity);
        }

        public async Task DeleteCustomerAsync(int id)
        {
            _logger.Info($"Deleting customer with ID: {id} from service.");
            await _customerRepository.DeleteCustomerAsync(id);
        }
    }
}

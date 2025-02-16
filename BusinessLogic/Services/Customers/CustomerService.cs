using BusinessLogic.Interfaces;
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

        public async Task<IEnumerable<Customer>> GetAllCustomersAsync()
        {
            _logger.Info("Fetching all customers from service.");
            return await _customerRepository.GetAllCustomersAsync();
        }

        public async Task<Customer> GetCustomerByIdAsync(int id)
        {
            _logger.Info($"Fetching customer with ID: {id} from service.");
            return await _customerRepository.GetCustomerByIdAsync(id);
        }

        public async Task AddCustomerAsync(Customer customer)
        {
            _logger.Info("Adding new customer from service.");
            await _customerRepository.AddCustomerAsync(customer);
        }

        public async Task UpdateCustomerAsync(Customer customer)
        {
            _logger.Info($"Updating customer with ID: {customer.Id} from service.");
            await _customerRepository.UpdateCustomerAsync(customer);
        }

        public async Task DeleteCustomerAsync(int id)
        {
            _logger.Info($"Deleting customer with ID: {id} from service.");
            await _customerRepository.DeleteCustomerAsync(id);
        }
    }
}

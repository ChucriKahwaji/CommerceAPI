using CommerceEntities.Entities;
using DataAccess.Interfaces;
using log4net;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories.Customers
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly CommerceContext _context;

        private static readonly ILog _logger = LogManager.GetLogger(typeof(CustomerRepository));

        public CustomerRepository(CommerceContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Customer>> GetAllCustomersAsync()
        {
            try
            {
                _logger.Info("Fetching all customers from the database.");
                return await _context.Customers.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.Error("An error occurred while fetching all customers.", ex);
                throw;
            }
        }

        public async Task<Customer> GetCustomerByIdAsync(int id)
        {
            try
            {
                _logger.Info($"Fetching customer with ID: {id}");
                var customer = await _context.Customers.FindAsync(id);
                if (customer == null)
                {
                    _logger.Warn($"Customer with ID: {id} not found.");
                }
                return customer;
            }
            catch (Exception ex)
            {
                _logger.Error($"An error occurred while fetching customer with ID: {id}", ex);
                throw;
            }
        }

        public async Task AddCustomerAsync(Customer customer)
        {
            try
            {
                _logger.Info("Adding a new customer to the database.");
                await _context.Customers.AddAsync(customer);
                await _context.SaveChangesAsync();
                _logger.Info("Customer added successfully.");
            }
            catch (Exception ex)
            {
                _logger.Error("An error occurred while adding a new customer.", ex);
                throw;
            }
        }

        public async Task UpdateCustomerAsync(Customer customer)
        {
            try
            {
                _logger.Info($"Updating customer with ID: {customer.Id}");
                _context.Customers.Update(customer);
                await _context.SaveChangesAsync();
                _logger.Info("Customer updated successfully.");
            }
            catch (Exception ex)
            {
                _logger.Error($"An error occurred while updating customer with ID: {customer.Id}", ex);
                throw;
            }
        }

        public async Task DeleteCustomerAsync(int id)
        {
            try
            {
                _logger.Info($"Deleting customer with ID: {id}");
                var customer = await _context.Customers.FindAsync(id);
                if (customer == null)
                {
                    _logger.Warn($"Customer with ID: {id} not found.");
                    return;
                }

                _context.Customers.Remove(customer);
                await _context.SaveChangesAsync();
                _logger.Info("Customer deleted successfully.");
            }
            catch (Exception ex)
            {
                _logger.Error($"An error occurred while deleting customer with ID: {id}", ex);
                throw;
            }
        }
    }
}

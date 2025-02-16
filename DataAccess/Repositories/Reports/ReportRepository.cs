using CommerceEntities.Entities;
using Dapper;
using DataAccess.Interfaces;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace DataAccess.Repositories.Reports
{
    public class ReportRepository : IReportRepository
    {
        private readonly IDbConnection _dbConnection;

        public ReportRepository(CommerceContext context)
        {
            _dbConnection = context.Database.GetDbConnection();
        }

        public async Task<List<CustomerPurchaseSummary>> GetCustomerPurchaseSummaryAsync()
        {
            string sql = @"SELECT 
                            c.Id AS CustomerId, 
                            c.FirstName AS CustomerName, 
                            COUNT(o.Id) AS TotalOrders
                        FROM Customers c
                        LEFT JOIN Orders o ON c.Id = o.CustomerId
                        GROUP BY c.Id, c.FirstName;";


            try
            {
                var result = await _dbConnection.QueryAsync<CustomerPurchaseSummary>(sql);
                return result.ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"SQL Execution Error: {ex.Message}");
                throw;
            }
        }

    }
}

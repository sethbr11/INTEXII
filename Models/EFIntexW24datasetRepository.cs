using Microsoft.EntityFrameworkCore;

namespace INTEXII.Models
{
    public class EFIntexW24datasetRepository : IIntexW24datasetRepository
    {
        private IntexW24datasetContext _repo;

        // Constructor
        public EFIntexW24datasetRepository(IntexW24datasetContext temp) => _repo = temp;

        public IQueryable<Customer> Customers => _repo.Customers;
        public IQueryable<LineItem> LineItems => _repo.LineItems;
        public IQueryable<Order> Orders => _repo.Orders;
        public void AddOrder(Order order) {
            _repo.Orders.Add(order);
            _repo.SaveChanges();
        }

        public IQueryable<Product> Products => _repo.Products;
        public void AddProduct(Product product)
        {
            _repo.Products.Add(product);
            _repo.SaveChanges();
        }
        public void UpdateProduct(Product product)
        {
            _repo.Update(product);
            _repo.SaveChanges();
        }

        public void UpdateCustomer(Customer customer)
        {
            _repo.Update(customer);
            _repo.SaveChanges();
        }
        //public void RemoveProduct(Product product)
        //{
        //_repo.Remove(product);
        //_repo.SaveChanges();

        //}

        public void DeleteProduct(Product product)
        {
            _repo.Remove(product);
            _repo.SaveChanges();
        }
        public void DeleteCustomer(Customer customer)
        {
            _repo.Remove(customer);
            _repo.SaveChanges();
        }

        public IQueryable<Recommendation> Recommendations => _repo.Recommendations;


    }
}

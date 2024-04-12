namespace INTEXII.Models {
    public interface IIntexW24datasetRepository {
        public IQueryable<Customer> Customers { get; }
        public IQueryable<LineItem> LineItems { get; }
        public IQueryable<Order> Orders { get; }
        public void AddOrder(Order order);
        public IQueryable<Product> Products { get; }
        public void AddProduct(Product product);
        public void UpdateProduct(Product product);
        public void UpdateCustomer(Customer customer);

        public void DeleteProduct(Product product);
        public void DeleteCustomer(Customer customer);

        public IQueryable<Recommendation> Recommendations { get; }

    }
}

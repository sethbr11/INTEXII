namespace INTEXII.Models {
    public interface IIntexW24datasetRepository {
        public IQueryable<Customer> Customers { get; }
        public IQueryable<LineItem> LineItems { get; }
        public IQueryable<Order> Orders { get; }
        public IQueryable<Product> Products { get; }
        public IQueryable<Recommendation> Recommendations { get; }
    }
}

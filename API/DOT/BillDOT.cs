namespace API.DOT
{
    public class BillDOT
    {
        public int BillID { get; set; }
        public string Customer { get; set; }
        public string Staff { get; set; }
        public string ProductName { get; set; }
        public string CusAddress { get; set; }
        public string Phone { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Total { get; set; }
        public int Status { get; set; }
    }
    public class BillDOTDetail
    {
        public int BillDetailID { get; set; }
        public string CustomerName { get; set; }
        public string StaffName { get; set; }
        public string ProductName { get; set; }
        public string CusAddress { get; set; }
        public string Phone { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Total { get; set; }
        public int Status { get; set; }
        public string Size { get; set; }
        public string Color { get; set; }
        public string Material { get; set; }
    }
    public class HomeDOT
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public string Image { get; set; }
    }
}

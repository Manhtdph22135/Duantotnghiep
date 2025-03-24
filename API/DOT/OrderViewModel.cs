namespace API.DOT
{
    public class OrderViewModel
    {
        public int? StaffId { get; set; }
        public string? StaffName { get; set; } // Người dùng nhập tên nhân viên
        public int CustomerId { get; set; }
        public int TransportId { get; set; }
        public string? TransportMethod { get; set; }
        public decimal TotalAmount { get; set; }
    }
}

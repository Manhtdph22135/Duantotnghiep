using API.Context;
using API.DOT;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers.Bill
{
    [Route("api/[controller]")]
    [ApiController]
    public class BillsController : ControllerBase
    {
        private readonly DbContextShop _contextShop;
        private readonly DLBase _dLBase;

        public BillsController(DbContextShop contextShop)
        {
            _contextShop = contextShop;
            _dLBase = new DLBase();
        }

        // GET: api/Bills
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Models.Bill>>> GetBills()
        {
            return await _contextShop.Bills.ToListAsync();
        }

        [HttpGet("hoadon")]
        public async Task<ActionResult<IEnumerable<BillDOT>>> GetBillsBy()
        {
            var query = from bd in _contextShop.BillDetails
                        join b in _contextShop.Bills on bd.BillId equals b.BillId
                        join c in _contextShop.Customers on b.CustomerId equals c.CustomerId
                        join s in _contextShop.Staffs on b.StaffId equals s.StaffId
                        join pd in _contextShop.ProductDetails on bd.ProductDetailId equals pd.ProductDetailId
                        join p in _contextShop.Products on pd.ProductId equals p.ProductId
                        join sz in _contextShop.Sizes on pd.SizeId equals sz.SizeId
                        join clr in _contextShop.Colors on pd.ColorId equals clr.ColorId
                        join mat in _contextShop.Materials on pd.MaterialId equals mat.MaterialId
                        select new BillDOT
                        {
                            BillID = b.BillId,
                            Customer = c.FullName,
                            Staff = s.FullName,
                            ProductName = p.ProductName,
                            CusAddress = c.Address,
                            Phone = c.Phone,
                            Quantity = bd.Quantity,
                            UnitPrice = bd.UnitPrice,
                            Total = (decimal)bd.Total,
                            Status = b.Status == false ? 0 : 1
                        };
            return await query.ToListAsync();
        }


        // GET: api/Bills/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Models.Bill>> GetBill(int id)
        {
            var bill = await _contextShop.Bills.FindAsync(id);

            if (bill == null)
            {
                return NotFound();
            }

            return bill;
        }

        // PUT: api/Bills/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBill(int id, Models.Bill bill)
        {
            if (id != bill.BillId)
            {
                return BadRequest();
            }

            _contextShop.Entry(bill).State = EntityState.Modified;

            try
            {
                await _contextShop.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BillExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Bills
        [HttpPost]
        public async Task<ActionResult<Models.Bill>> PostBill(Models.Bill bill)
        {
            _contextShop.Bills.Add(bill);
            await _contextShop.SaveChangesAsync();

            return CreatedAtAction("GetBill", new { id = bill.BillId }, bill);
        }
        [HttpPost("create-bill")]
        public async Task<IActionResult> CreateBill([FromBody] OrderViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Tìm StaffId nếu nhập StaffName
            if (!string.IsNullOrEmpty(model.StaffName))
            {
                var staff = await _contextShop.Staffs
                    .Where(s => EF.Functions.Like(s.FullName, $"%{model.StaffName}%"))
                    .FirstOrDefaultAsync();

                model.StaffId = staff?.StaffId; // Nếu không tìm thấy, StaffId sẽ là null
            }

            // Tìm TransportId nếu nhập TransportMethod
            if (!string.IsNullOrEmpty(model.TransportMethod))
            {
                var transport = await _contextShop.Transports
                    .Where(t => EF.Functions.Like(t.TransportMethod, $"%{model.TransportMethod}%"))
                    .FirstOrDefaultAsync();

                model.TransportId = Convert.ToInt32(transport?.TransportId); // Nếu không tìm thấy, TransportId sẽ là null
            }
            else
            {
                model.TransportId = 1; // Nếu không nhập, mặc định là 1
            }

            // Tính TotalAmount = Tổng Total trong BillDetail + Cost trong Transport
            decimal totalBillDetail = await _contextShop.BillDetails
                .Where(bd => bd.BillId == model.CustomerId) // Giả sử CustomerId chính là BillId
                .SumAsync(bd => bd.Total ?? 0); // Nếu Total là null, lấy 0

            decimal transportCost = model.TransportId != null
                ? await _contextShop.Transports
                    .Where(t => t.TransportId == model.TransportId)
                    .Select(t => t.Cost)
                    .FirstOrDefaultAsync()
                : totalBillDetail;

            model.TotalAmount = totalBillDetail + transportCost;

            // Tạo hóa đơn mới
            var bill = new Models.Bill
            {
                StaffId = model.StaffId,
                CustomerId = model.CustomerId,
                TransportId = model.TransportId,
                CreateAt = DateTime.Now,
                Status = false,
                TotalAmount = model.TotalAmount
            };

            _contextShop.Bills.Add(bill);
            await _contextShop.SaveChangesAsync();

            return Ok(bill);
        }


        // DELETE: api/Bills/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBill(int id)
        {
            var bill = await _contextShop.Bills.FindAsync(id);
            if (bill == null)
            {
                return NotFound();
            }

            _contextShop.Bills.Remove(bill);
            await _contextShop.SaveChangesAsync();

            return NoContent();
        }

        private bool BillExists(int id)
        {
            return _contextShop.Bills.Any(e => e.BillId == id);
        }
    }
}

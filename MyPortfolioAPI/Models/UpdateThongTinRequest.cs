using Microsoft.AspNetCore.Http;

namespace MyPortfolioAPI.Models
{
    public class UpdateThongTinRequest
    {
        public string? HoTen { get; set; }
        public string? Lop { get; set; }
        public string? NhomHP { get; set; }
        public string? Email { get; set; }

        public IFormFile? AnhDaiDien { get; set; }
    }
}
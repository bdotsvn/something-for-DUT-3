using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using MyPortfolioAPI.Models;
using MyPortfolioAPI.Services;
using System.Threading.Tasks;

namespace MyPortfolioAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ThongTinCaNhanController : ControllerBase
    {
        private readonly string? _connectionString;
        private readonly ICloudinaryService _cloudinaryService;

        public ThongTinCaNhanController(IConfiguration config, ICloudinaryService cloudinaryService)
        {
            _connectionString = config.GetConnectionString("DefaultConnection");
            _cloudinaryService = cloudinaryService;
        }

        [HttpGet("{maSV}")]
        public IActionResult GetThongTin(string maSV)
        {
            ThongTinCaNhan? thongTin = null;

            if (string.IsNullOrEmpty(_connectionString))
            {
                return StatusCode(500, new { message = "Connection string is not configured properly." });
            }

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM ThongTinCaNhan WHERE MaSV = @MaSV";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MaSV", maSV);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            thongTin = new ThongTinCaNhan
                            {
                                MaSV = reader["MaSV"].ToString(),
                                HoTen = reader["HoTen"].ToString(),
                                Lop = reader["Lop"].ToString(),
                                NhomHP = reader["NhomHP"].ToString(),
                                Email = reader["Email"].ToString(),
                                AnhDaiDienUrl = reader["AnhDaiDienUrl"].ToString()
                            };
                        }
                    }
                }
            }

            if (thongTin == null) return NotFound(new { message = "Không tìm thấy sinh viên" });

            return Ok(thongTin);
        }

        [HttpPut("{maSV}")]
        public async Task<IActionResult> UpdateThongTin(string maSV, [FromForm] UpdateThongTinRequest request)
        {
            string? newImageUrl = null;

            if (request.AnhDaiDien != null)
            {
                newImageUrl = await _cloudinaryService.UploadImageAsync(request.AnhDaiDien);
            }

            if (string.IsNullOrEmpty(_connectionString))
            {
                return StatusCode(500, new { message = "Connection string is not configured properly." });
            }

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                string query = @"
                    UPDATE ThongTinCaNhan
                    SET HoTen = @HoTen,
                        Lop = @Lop,
                        NhomHP = @NhomHP,
                        Email = @Email
                        " + (newImageUrl != null ? ", AnhDaiDienUrl = @AnhDaiDienUrl" : "") + @"
                    WHERE MaSV = @MaSV";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@HoTen", (object?)request.HoTen ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Lop", (object?)request.Lop ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@NhomHP", (object?)request.NhomHP ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Email", (object?)request.Email ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@MaSV", maSV);

                    if (newImageUrl != null)
                    {
                        cmd.Parameters.AddWithValue("@AnhDaiDienUrl", newImageUrl);
                    }

                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected == 0) return NotFound(new { message = "Không tìm thấy sinh viên để cập nhật" });
                }
            }

            return Ok(new { message = "Cập nhật thành công!", newImageUrl });
        }
    }
}
CREATE DATABASE DoAn_CaNhan;
GO

USE DoAn_CaNhan;
GO

CREATE TABLE ThongTinCaNhan (
    MaSV NVARCHAR(20) PRIMARY KEY,
    HoTen NVARCHAR(100) NOT NULL,
    Lop NVARCHAR(50),
    NhomHP NVARCHAR(50),
    Email NVARCHAR(100),
    AnhDaiDienUrl NVARCHAR(500)
);
GO

-- Chèn 1 dòng dữ liệu mẫu ban đầu của bạn vào DB
INSERT INTO ThongTinCaNhan (MaSV, HoTen, Lop, NhomHP, Email, AnhDaiDienUrl)
VALUES ('SV001', N'Nguyễn Văn A', N'K65CNTTA', N'01', 'nguyenvana@gmail.com', '');
GO

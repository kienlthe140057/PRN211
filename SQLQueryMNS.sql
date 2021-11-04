create database QLVatTu
go
use QLVatTu
go
create table NguoiDung(
	TaiKhoan char(50) primary key,
	MatKhau char(50)
)
go
create table NCC(
	MaNCC char(10) primary key,
	TenNCC nvarchar(50),
	SDT char(10),
	DiaChi nvarchar(50)
)
go
create table VatTu(
	MaVT char(10),
	MaNCC char(10),
	TenVT nvarchar(50),
	SoLuong int,
	DonGia int,
	primary key(MaVT),
	foreign key(MaNCC) references NCC(MaNCC)
)
go
create table KhachHang(
	MaKH char(10) primary key,
	TenKH nvarchar(50),
	DiaChi nvarchar(50),
	SDT char(10)
)
go
create table HoaDon(
	MaHD char(10) primary key,
	NgayTao datetime,
	ThanhTien int,
	MaKH char(10),
	foreign key(MaKH) references KhachHang(MaKH)
)
go
create table CTHoaDon(
	MaHD char(10),
	MaVT char(10),
	DonGia int,
	SoLuong int,
	ThanhTien int,
	primary key(MaHD,MaVT),
	foreign key(MaHD) references HoaDon(MaHD),
	foreign key(MaVT) references VatTu(MaVT)
)
go
create table PhieuNhap(
	MaPN char(10) primary key,
	NgayNhap datetime,
	ThanhTien int
)
go
create table CTPhieuNhap(
	MaPN char(10),
	MaVT char(10),
	DonGia int,
	SoLuong int,
	ThanhTien int,
	primary key(MaPN,MaVT),
	foreign key(MaPN) references PhieuNhap(MaPN),
	foreign key(MaVT) references VatTu(MaVT)
)
go

select * from PhieuNhap where Cast(NgayNhap as date) = convert(varchar,'01/07/2020',3)

select KhachHang.MaKH, KhachHang.TenKH, KhachHang.SDT, KhachHang.DiaChi, Sum(HoaDon.ThanhTien) as ThanhToan
from KhachHang, HoaDon
group by HoaDon.MaKH, KhachHang.MaKH, KhachHang.TenKH, KhachHang.SDT, KhachHang.DiaChi

select HoaDon.MaKH,KhachHang.TenKH,  KhachHang.SDT, KhachHang.DiaChi, Sum(HoaDon.ThanhTien) as ThanhToan
from HoaDon, KhachHang
where KhachHang.MaKH = HoaDon.MaKH
group by HoaDon.MaKH, KhachHang.TenKH,  KhachHang.SDT, KhachHang.DiaChi
order by ThanhToan DESC

select ThanhTien 
from HoaDon
where NgayTao between '2020-01-1' and '2020-08-31'


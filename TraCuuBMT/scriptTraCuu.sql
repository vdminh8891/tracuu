USE [master]
GO
/****** Object:  Database [TraCuuBMT]    Script Date: 28/03/2022 20:53:18 ******/
CREATE DATABASE [TraCuuBMT]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'TraCuuBMT', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL13.MSSQLSERVER\MSSQL\DATA\TraCuuBMT.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'TraCuuBMT_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL13.MSSQLSERVER\MSSQL\DATA\TraCuuBMT_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
ALTER DATABASE [TraCuuBMT] SET COMPATIBILITY_LEVEL = 130
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [TraCuuBMT].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [TraCuuBMT] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [TraCuuBMT] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [TraCuuBMT] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [TraCuuBMT] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [TraCuuBMT] SET ARITHABORT OFF 
GO
ALTER DATABASE [TraCuuBMT] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [TraCuuBMT] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [TraCuuBMT] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [TraCuuBMT] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [TraCuuBMT] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [TraCuuBMT] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [TraCuuBMT] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [TraCuuBMT] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [TraCuuBMT] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [TraCuuBMT] SET  DISABLE_BROKER 
GO
ALTER DATABASE [TraCuuBMT] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [TraCuuBMT] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [TraCuuBMT] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [TraCuuBMT] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [TraCuuBMT] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [TraCuuBMT] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [TraCuuBMT] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [TraCuuBMT] SET RECOVERY FULL 
GO
ALTER DATABASE [TraCuuBMT] SET  MULTI_USER 
GO
ALTER DATABASE [TraCuuBMT] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [TraCuuBMT] SET DB_CHAINING OFF 
GO
ALTER DATABASE [TraCuuBMT] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [TraCuuBMT] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [TraCuuBMT] SET DELAYED_DURABILITY = DISABLED 
GO
EXEC sys.sp_db_vardecimal_storage_format N'TraCuuBMT', N'ON'
GO
ALTER DATABASE [TraCuuBMT] SET QUERY_STORE = OFF
GO
USE [TraCuuBMT]
GO
ALTER DATABASE SCOPED CONFIGURATION SET LEGACY_CARDINALITY_ESTIMATION = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION SET MAXDOP = 0;
GO
ALTER DATABASE SCOPED CONFIGURATION SET PARAMETER_SNIFFING = ON;
GO
ALTER DATABASE SCOPED CONFIGURATION SET QUERY_OPTIMIZER_HOTFIXES = OFF;
GO
USE [TraCuuBMT]
GO
/****** Object:  Table [dbo].[BieuThue]    Script Date: 28/03/2022 20:53:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BieuThue](
	[ID] [nvarchar](50) NOT NULL,
	[createDate] [datetime] NOT NULL,
	[status] [int] NOT NULL,
	[creator] [nvarchar](50) NULL,
	[lastEditDate] [datetime] NULL,
	[lastEditor] [nvarchar](50) NULL,
	[note] [nvarchar](max) NULL,
	[description] [nvarchar](max) NULL,
	[HS_CODE] [nvarchar](50) NOT NULL,
	[DVT_SL2] [nvarchar](50) NOT NULL,
	[link_file_vn] [nvarchar](max) NULL,
	[link_file_en] [nvarchar](max) NULL,
	[tenBieuThue] [nvarchar](200) NOT NULL,
	[Ten_Hanghoa_VN] [nvarchar](200) NOT NULL,
	[Ten_Hanghoa_EN] [nvarchar](200) NOT NULL,
	[THUE_EXPORT] [float] NULL,
	[THUE_BVMT] [float] NULL,
	[THUE_TTDB] [float] NULL,
	[THUE_TVCBPG] [float] NULL,
	[THUE_PBDX] [float] NULL,
	[B01] [float] NULL,
	[B02] [float] NULL,
	[B03] [float] NULL,
	[B04] [float] NULL,
	[B05] [float] NULL,
	[B06] [float] NULL,
	[B07] [float] NULL,
	[B08] [float] NULL,
	[B09] [float] NULL,
	[B10] [float] NULL,
	[B11] [float] NULL,
	[B12] [float] NULL,
	[B13] [float] NULL,
	[B14] [float] NULL,
	[B15] [float] NULL,
	[B16] [float] NULL,
	[B17] [float] NULL,
	[B18] [float] NULL,
	[B19] [float] NULL,
	[B20] [float] NULL,
	[B21] [float] NULL,
	[B22] [float] NULL,
	[B23] [float] NULL,
	[B24] [float] NULL,
	[B25] [float] NULL,
	[B30] [float] NULL,
	[B61] [float] NULL,
	[B99] [float] NULL,
 CONSTRAINT [PK_BieuThueXNK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[KetQuaPhanTichPhanLoai]    Script Date: 28/03/2022 20:53:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[KetQuaPhanTichPhanLoai](
	[ID] [nvarchar](50) NOT NULL,
	[createDate] [datetime] NOT NULL,
	[status] [int] NOT NULL,
	[creator] [nvarchar](50) NOT NULL,
	[lastEditDate] [datetime] NULL,
	[lastEditor] [nvarchar](50) NULL,
	[hsCode] [nvarchar](50) NOT NULL,
	[Mota_Dnkhaibao] [nvarchar](max) NOT NULL,
	[link_file_vn] [nvarchar](200) NOT NULL,
	[link_file_en] [nvarchar](200) NULL,
	[unit] [nvarchar](50) NOT NULL,
	[Mota_KQPTPL] [nvarchar](max) NOT NULL,
	[So_Vanban] [nvarchar](50) NOT NULL,
	[Ngay_Vanban] [datetime] NOT NULL,
	[Ngay_Vanban_HH] [datetime] NULL,
	[CoQuan_PhatHanh_PTPL] [nvarchar](200) NOT NULL,
	[CoQuan_TiepNhan_YC_PTPL] [nvarchar](200) NULL,
	[DonVi_XNK] [nvarchar](200) NOT NULL,
	[ToKhai_HQ] [nvarchar](200) NOT NULL,
	[Loai_Hinh] [nvarchar](200) NOT NULL,
	[Co_Quan_YC_PTPL] [nvarchar](200) NOT NULL,
	[Hieu_Luc_Vanban] [nvarchar](200) NULL,
	[description] [nvarchar](max) NULL,
 CONSTRAINT [PK_KetQuaPhanTichPhanLoai] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Package]    Script Date: 28/03/2022 20:53:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Package](
	[ID] [nvarchar](50) NOT NULL,
	[packageName] [nvarchar](200) NOT NULL,
	[description] [nvarchar](max) NULL,
	[createDate] [datetime] NOT NULL,
	[status] [int] NOT NULL,
	[type] [int] NOT NULL,
	[price] [float] NOT NULL,
	[startDate] [datetime] NULL,
	[endDate] [datetime] NULL,
	[creator] [nvarchar](50) NULL,
	[lastEditDate] [datetime] NULL,
	[lastEditor] [nvarchar](50) NULL,
	[amount] [int] NOT NULL,
 CONSTRAINT [PK_Package] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ThueVAT]    Script Date: 28/03/2022 20:53:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ThueVAT](
	[ID] [nvarchar](50) NOT NULL,
	[status] [int] NOT NULL,
	[mota] [nvarchar](max) NOT NULL,
	[value] [float] NOT NULL,
	[note] [nvarchar](max) NULL,
	[name] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_ThueVAT] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Transaction]    Script Date: 28/03/2022 20:53:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Transaction](
	[ID] [nvarchar](50) NOT NULL,
	[createDate] [datetime] NOT NULL,
	[status] [int] NOT NULL,
	[type] [int] NOT NULL,
	[currentPrice] [float] NOT NULL,
	[packageId] [nvarchar](50) NOT NULL,
	[userId] [nvarchar](50) NOT NULL,
	[creator] [nvarchar](50) NOT NULL,
	[lastEditDate] [datetime] NULL,
	[lastEditor] [nvarchar](50) NULL,
	[note] [nvarchar](max) NULL,
	[amount] [int] NOT NULL,
 CONSTRAINT [PK_Transaction] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[User]    Script Date: 28/03/2022 20:53:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User](
	[ID] [nvarchar](50) NOT NULL,
	[username] [nvarchar](200) NOT NULL,
	[password] [nvarchar](max) NOT NULL,
	[email] [nvarchar](200) NOT NULL,
	[phone] [nvarchar](50) NOT NULL,
	[status] [int] NOT NULL,
	[createDate] [datetime] NOT NULL,
	[type] [int] NOT NULL,
	[role] [int] NOT NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
INSERT [dbo].[BieuThue] ([ID], [createDate], [status], [creator], [lastEditDate], [lastEditor], [note], [description], [HS_CODE], [DVT_SL2], [link_file_vn], [link_file_en], [tenBieuThue], [Ten_Hanghoa_VN], [Ten_Hanghoa_EN], [THUE_EXPORT], [THUE_BVMT], [THUE_TTDB], [THUE_TVCBPG], [THUE_PBDX], [B01], [B02], [B03], [B04], [B05], [B06], [B07], [B08], [B09], [B10], [B11], [B12], [B13], [B14], [B15], [B16], [B17], [B18], [B19], [B20], [B21], [B22], [B23], [B24], [B25], [B30], [B61], [B99]) VALUES (N'bieuThue_1624285628_821059', CAST(N'2021-06-21T21:27:08.927' AS DateTime), 1, N'', NULL, NULL, N'Ghi chú:asdasdasdasd', N'mô tả abc', N'1012100', N'Tấn', N'', NULL, N'Tên abc', N'Tên hàng hóa (Việt)(*):asdasd', N'Tên hàng hóa (Anh)(*):asdasd', NULL, NULL, NULL, NULL, NULL, 1.2, 5.5, 7.8, -99, -99, -99, -99, -99, -99, -99, -99, -99, -99, -99, -99, -99, -99, -99, -99, -99, -99, -99, -99, -99, -99, -99, -99, -99)
INSERT [dbo].[BieuThue] ([ID], [createDate], [status], [creator], [lastEditDate], [lastEditor], [note], [description], [HS_CODE], [DVT_SL2], [link_file_vn], [link_file_en], [tenBieuThue], [Ten_Hanghoa_VN], [Ten_Hanghoa_EN], [THUE_EXPORT], [THUE_BVMT], [THUE_TTDB], [THUE_TVCBPG], [THUE_PBDX], [B01], [B02], [B03], [B04], [B05], [B06], [B07], [B08], [B09], [B10], [B11], [B12], [B13], [B14], [B15], [B16], [B17], [B18], [B19], [B20], [B21], [B22], [B23], [B24], [B25], [B30], [B61], [B99]) VALUES (N'bieuThue_1624293010_145140', CAST(N'2021-06-21T23:30:10.607' AS DateTime), 1, N'', NULL, NULL, N'', N'mota', N'123', N'kg', N'', NULL, N'tên abc', N'tên', N'name', NULL, NULL, NULL, NULL, NULL, -99, -99, -99, -99, -99, -99, -99, -99, -99, 7.8, -99, -99, 2.6, -99, -99, -99, -99, -99, -99, -99, -99, -99, -99, -99, -99, -99, -99, -99)
INSERT [dbo].[BieuThue] ([ID], [createDate], [status], [creator], [lastEditDate], [lastEditor], [note], [description], [HS_CODE], [DVT_SL2], [link_file_vn], [link_file_en], [tenBieuThue], [Ten_Hanghoa_VN], [Ten_Hanghoa_EN], [THUE_EXPORT], [THUE_BVMT], [THUE_TTDB], [THUE_TVCBPG], [THUE_PBDX], [B01], [B02], [B03], [B04], [B05], [B06], [B07], [B08], [B09], [B10], [B11], [B12], [B13], [B14], [B15], [B16], [B17], [B18], [B19], [B20], [B21], [B22], [B23], [B24], [B25], [B30], [B61], [B99]) VALUES (N'bieuThue_1638717205_366036', CAST(N'2021-12-05T22:13:25.653' AS DateTime), 1, N'', NULL, NULL, N'', N'', N'5555', N'21', N'', NULL, N'', N'asd', N'asd', 11, 22, 33, 44, 55, -99, -99, -99, -99, -99, -99, -99, -99, -99, -99, -99, -99, -99, -99, -99, -99, -99, -99, -99, -99, -99, -99, -99, -99, -99, -99, -99, -99)
GO
INSERT [dbo].[KetQuaPhanTichPhanLoai] ([ID], [createDate], [status], [creator], [lastEditDate], [lastEditor], [hsCode], [Mota_Dnkhaibao], [link_file_vn], [link_file_en], [unit], [Mota_KQPTPL], [So_Vanban], [Ngay_Vanban], [Ngay_Vanban_HH], [CoQuan_PhatHanh_PTPL], [CoQuan_TiepNhan_YC_PTPL], [DonVi_XNK], [ToKhai_HQ], [Loai_Hinh], [Co_Quan_YC_PTPL], [Hieu_Luc_Vanban], [description]) VALUES (N'KQPTPL_1622393396_231664', CAST(N'2021-05-30T23:49:56.060' AS DateTime), 1, N'', NULL, NULL, N'123', N'123', N'KQPTPL_1638725757.pdf', NULL, N'123', N'123', N'123', CAST(N'2021-05-11T00:00:00.000' AS DateTime), CAST(N'2021-05-06T00:00:00.000' AS DateTime), N'123', NULL, N'123', N'123', N'123', N'123', NULL, NULL)
INSERT [dbo].[KetQuaPhanTichPhanLoai] ([ID], [createDate], [status], [creator], [lastEditDate], [lastEditor], [hsCode], [Mota_Dnkhaibao], [link_file_vn], [link_file_en], [unit], [Mota_KQPTPL], [So_Vanban], [Ngay_Vanban], [Ngay_Vanban_HH], [CoQuan_PhatHanh_PTPL], [CoQuan_TiepNhan_YC_PTPL], [DonVi_XNK], [ToKhai_HQ], [Loai_Hinh], [Co_Quan_YC_PTPL], [Hieu_Luc_Vanban], [description]) VALUES (N'KQPTPL_1622476906_730715', CAST(N'2021-05-31T23:01:46.530' AS DateTime), 1, N'', NULL, NULL, N'123', N'123', N'KQPTPL_1638725749.pdf', NULL, N'123', N'123', N'123', CAST(N'2021-05-19T00:00:00.000' AS DateTime), CAST(N'2021-05-19T00:00:00.000' AS DateTime), N'123', NULL, N'qweqwe', N'123', N'123', N'qweqwe', NULL, NULL)
INSERT [dbo].[KetQuaPhanTichPhanLoai] ([ID], [createDate], [status], [creator], [lastEditDate], [lastEditor], [hsCode], [Mota_Dnkhaibao], [link_file_vn], [link_file_en], [unit], [Mota_KQPTPL], [So_Vanban], [Ngay_Vanban], [Ngay_Vanban_HH], [CoQuan_PhatHanh_PTPL], [CoQuan_TiepNhan_YC_PTPL], [DonVi_XNK], [ToKhai_HQ], [Loai_Hinh], [Co_Quan_YC_PTPL], [Hieu_Luc_Vanban], [description]) VALUES (N'KQPTPL_1638695696_667403', CAST(N'2021-12-05T16:14:56.887' AS DateTime), 1, N'', NULL, NULL, N'123123', N'', N'KQPTPL_1638725649.pdf', NULL, N'123', N'', N'', CAST(N'2021-12-15T00:00:00.000' AS DateTime), NULL, N'', NULL, N'', N'', N'', N'', NULL, NULL)
GO
INSERT [dbo].[Package] ([ID], [packageName], [description], [createDate], [status], [type], [price], [startDate], [endDate], [creator], [lastEditDate], [lastEditor], [amount]) VALUES (N'package_1638709198_988171', N'asdasd22', N'asdasd11', CAST(N'2021-12-05T19:59:58.127' AS DateTime), 1, 1, 12000012, NULL, NULL, N'admin@gmail.com', CAST(N'2021-12-05T21:04:08.547' AS DateTime), N'admin@gmail.com', 1211)
GO
INSERT [dbo].[ThueVAT] ([ID], [status], [mota], [value], [note], [name]) VALUES (N'ThueVAT_001', 1, N'Hàng hóa thuộc đối tượng chịu thuế GTGT với mức thuế suất 0%', 0, N'', N'V')
INSERT [dbo].[ThueVAT] ([ID], [status], [mota], [value], [note], [name]) VALUES (N'ThueVAT_002', 1, N'Nước sạch phục vụ sản xuất và sinh hoạt', 5, N'', N'VB015')
INSERT [dbo].[ThueVAT] ([ID], [status], [mota], [value], [note], [name]) VALUES (N'ThueVAT_003', 1, N'Quặng để sản xuất phân bón', 5, N'', N'VB025')
INSERT [dbo].[ThueVAT] ([ID], [status], [mota], [value], [note], [name]) VALUES (N'ThueVAT_004', 1, N'Thuốc phòng trừ sâu bệnh  và chất kích thích tăng trưởng vật nuôi, cây trồng', 5, N'', N'VB035')
INSERT [dbo].[ThueVAT] ([ID], [status], [mota], [value], [note], [name]) VALUES (N'ThueVAT_005', 1, N'Mủ cao su sơ chế', 5, N'', N'VB055')
INSERT [dbo].[ThueVAT] ([ID], [status], [mota], [value], [note], [name]) VALUES (N'ThueVAT_006', 1, N'Nhựa thông sơ chế', 5, N'', N'VB065')
INSERT [dbo].[ThueVAT] ([ID], [status], [mota], [value], [note], [name]) VALUES (N'ThueVAT_007', 1, N'Lưới, dây giềng và sợi để đan lưới đánh cá', 5, N'', N'VB075')
INSERT [dbo].[ThueVAT] ([ID], [status], [mota], [value], [note], [name]) VALUES (N'ThueVAT_008', 1, N'Thực phẩm tươi sống; lâm sản chưa qua chế biến, trừ gỗ, măng và sản phẩm quy định tại khoản 1 Điều 5 Luật thuế GTGT số 13/2008/QH12', 5, N'', N'VB085')
INSERT [dbo].[ThueVAT] ([ID], [status], [mota], [value], [note], [name]) VALUES (N'ThueVAT_009', 1, N'Đường; phụ phẩm trong sản xuất đường, bao gồm gỉ đường, bã mía, bã bùn', 5, N'', N'VB095')
INSERT [dbo].[ThueVAT] ([ID], [status], [mota], [value], [note], [name]) VALUES (N'ThueVAT_010', 1, N'Sản phẩm bằng đay, cói, tre, nứa, lá, rơm, vỏ dừa, sọ dừa, bèo tây và các sản phẩm thủ công khác sản xuất bằng nguyên liệu tận dụng từ nông nghiệp', 5, N'', N'VB105')
INSERT [dbo].[ThueVAT] ([ID], [status], [mota], [value], [note], [name]) VALUES (N'ThueVAT_011', 1, N'Bông sơ chế', 5, N'', N'VB115')
INSERT [dbo].[ThueVAT] ([ID], [status], [mota], [value], [note], [name]) VALUES (N'ThueVAT_012', 1, N'Giấy in báo', 5, N'', N'VB125')
INSERT [dbo].[ThueVAT] ([ID], [status], [mota], [value], [note], [name]) VALUES (N'ThueVAT_013', 1, N'Thiết bị, dụng cụ y tế; bông, băng vệ sinh y tế', 5, N'', N'VB145')
INSERT [dbo].[ThueVAT] ([ID], [status], [mota], [value], [note], [name]) VALUES (N'ThueVAT_014', 1, N'Thuốc phòng bệnh, chữa bệnh; sản phẩm hóa dược, dược liệu là nguyên liệu sản xuất thuốc chữa bệnh, thuốc phòng bệnh', 5, N'', N'VB155')
INSERT [dbo].[ThueVAT] ([ID], [status], [mota], [value], [note], [name]) VALUES (N'ThueVAT_015', 1, N'Giáo cụ dùng để giảng dạy và học tập, bao gồm các loại mô hình, hình vẽ, bảng, phấn, thước kẻ, com-pa và các loại thiết bị, dụng cụ chuyên dùng cho giảng dạy, nghiên cứu, thí nghiệm khoa học', 5, N'', N'VB165')
INSERT [dbo].[ThueVAT] ([ID], [status], [mota], [value], [note], [name]) VALUES (N'ThueVAT_016', 1, N'Phim nhập khẩu', 5, N'', N'VB175')
INSERT [dbo].[ThueVAT] ([ID], [status], [mota], [value], [note], [name]) VALUES (N'ThueVAT_017', 1, N'Đồ chơi cho trẻ em; sách các loại, trừ sách quy định tại khoản 15 Điều 5 Luật thuế GTGT số 13/2008/QH12', 5, N'', N'VB185')
INSERT [dbo].[ThueVAT] ([ID], [status], [mota], [value], [note], [name]) VALUES (N'ThueVAT_018', 1, N'Hàng hóa thuộc đối tượng chịu thuế GTGT với mức thuế suất 10%', 10, N'', N'VB901')
GO
INSERT [dbo].[Transaction] ([ID], [createDate], [status], [type], [currentPrice], [packageId], [userId], [creator], [lastEditDate], [lastEditor], [note], [amount]) VALUES (N'transaction_1638721394_178231', CAST(N'2021-12-05T23:23:14.607' AS DateTime), 3, 2, 12000012, N'package_1638709198_988171', N'user_1621762539_425072', N'vdminh8891@gmail.com', NULL, NULL, N'', 1211)
INSERT [dbo].[Transaction] ([ID], [createDate], [status], [type], [currentPrice], [packageId], [userId], [creator], [lastEditDate], [lastEditor], [note], [amount]) VALUES (N'transaction_1638726132_628529', CAST(N'2021-12-06T00:42:12.843' AS DateTime), 1, 2, 0, N'', N'user_1621762539_425072', N'vdminh8891@gmail.com', NULL, NULL, NULL, -1)
INSERT [dbo].[Transaction] ([ID], [createDate], [status], [type], [currentPrice], [packageId], [userId], [creator], [lastEditDate], [lastEditor], [note], [amount]) VALUES (N'transaction_1638726373_660056', CAST(N'2021-12-06T00:46:13.930' AS DateTime), 1, 2, 0, N'', N'user_1621762539_425072', N'vdminh8891@gmail.com', NULL, NULL, NULL, -1)
INSERT [dbo].[Transaction] ([ID], [createDate], [status], [type], [currentPrice], [packageId], [userId], [creator], [lastEditDate], [lastEditor], [note], [amount]) VALUES (N'transaction_1638726960_657196', CAST(N'2021-12-06T00:56:00.280' AS DateTime), 1, 2, 0, N'', N'user_1621762539_425072', N'vdminh8891@gmail.com', NULL, NULL, NULL, -1)
INSERT [dbo].[Transaction] ([ID], [createDate], [status], [type], [currentPrice], [packageId], [userId], [creator], [lastEditDate], [lastEditor], [note], [amount]) VALUES (N'transaction_1638726973_824328', CAST(N'2021-12-06T00:56:13.240' AS DateTime), 1, 2, 0, N'', N'user_1621762539_425072', N'vdminh8891@gmail.com', NULL, NULL, NULL, -1)
GO
INSERT [dbo].[User] ([ID], [username], [password], [email], [phone], [status], [createDate], [type], [role]) VALUES (N'rootUserId', N'normal', N'normal', N'normal@email', N'0918360111', 1, CAST(N'2021-04-25T00:00:00.000' AS DateTime), 1, 1)
INSERT [dbo].[User] ([ID], [username], [password], [email], [phone], [status], [createDate], [type], [role]) VALUES (N'user_1620005250_849160', N'email', N'202CB962AC59075B964B07152D234B70', N'email', N'0999999999', 2, CAST(N'2021-05-03T08:27:30.273' AS DateTime), 1, 1)
INSERT [dbo].[User] ([ID], [username], [password], [email], [phone], [status], [createDate], [type], [role]) VALUES (N'user_1620020252_810717', N'123', N'123', N'123', N'123', 2, CAST(N'2021-05-03T12:37:32.203' AS DateTime), 1, 1)
INSERT [dbo].[User] ([ID], [username], [password], [email], [phone], [status], [createDate], [type], [role]) VALUES (N'user_1620020333_825475', N'asd', N'asd', N'asd', N'asd', 1, CAST(N'2021-05-03T12:38:56.917' AS DateTime), 1, 1)
INSERT [dbo].[User] ([ID], [username], [password], [email], [phone], [status], [createDate], [type], [role]) VALUES (N'user_1620143656_125819', N'email', N'827CCB0EEA8A706C4C34A16891F84E7B', N'email', N'0988888777', 1, CAST(N'2021-05-04T22:54:16.343' AS DateTime), 1, 1)
INSERT [dbo].[User] ([ID], [username], [password], [email], [phone], [status], [createDate], [type], [role]) VALUES (N'user_1620143843_265205', N'email', N'202CB962AC59075B964B07152D234B70', N'email', N'123456789', 1, CAST(N'2021-05-04T22:57:23.777' AS DateTime), 1, 1)
INSERT [dbo].[User] ([ID], [username], [password], [email], [phone], [status], [createDate], [type], [role]) VALUES (N'user_1620144143_40477', N'email', N'202CB962AC59075B964B07152D234B70', N'admin@gmail.com', N'123123', 1, CAST(N'2021-05-04T23:02:23.023' AS DateTime), 1, 2)
INSERT [dbo].[User] ([ID], [username], [password], [email], [phone], [status], [createDate], [type], [role]) VALUES (N'user_1621762539_425072', N'vdminh8891@gmail.com', N'FEA087517C26FADD409BD4B9DC642555', N'vdminh8891@gmail.com', N'0918222222', 1, CAST(N'2021-05-23T16:35:39.233' AS DateTime), 1, 1)
INSERT [dbo].[User] ([ID], [username], [password], [email], [phone], [status], [createDate], [type], [role]) VALUES (N'user_1638721090_89735', N'abc@gmail.com', N'202CB962AC59075B964B07152D234B70', N'abc@gmail.com', N'444', 2, CAST(N'2021-12-05T23:18:10.750' AS DateTime), 1, 2)
GO
USE [master]
GO
ALTER DATABASE [TraCuuBMT] SET  READ_WRITE 
GO

USE [master]
GO
/****** Object:  Database [ShoppingCartCmc.Trading]    Script Date: 12/06/2022 9:19:51 PM ******/
CREATE DATABASE [ShoppingCartCmc.Trading]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'ShoppingCartCmc.Trading', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.SQLEXPRESS\MSSQL\DATA\ShoppingCartCmc.Trading.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'ShoppingCartCmc.Trading_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.SQLEXPRESS\MSSQL\DATA\ShoppingCartCmc.Trading_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
ALTER DATABASE [ShoppingCartCmc.Trading] SET COMPATIBILITY_LEVEL = 140
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [ShoppingCartCmc.Trading].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [ShoppingCartCmc.Trading] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [ShoppingCartCmc.Trading] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [ShoppingCartCmc.Trading] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [ShoppingCartCmc.Trading] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [ShoppingCartCmc.Trading] SET ARITHABORT OFF 
GO
ALTER DATABASE [ShoppingCartCmc.Trading] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [ShoppingCartCmc.Trading] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [ShoppingCartCmc.Trading] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [ShoppingCartCmc.Trading] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [ShoppingCartCmc.Trading] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [ShoppingCartCmc.Trading] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [ShoppingCartCmc.Trading] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [ShoppingCartCmc.Trading] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [ShoppingCartCmc.Trading] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [ShoppingCartCmc.Trading] SET  DISABLE_BROKER 
GO
ALTER DATABASE [ShoppingCartCmc.Trading] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [ShoppingCartCmc.Trading] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [ShoppingCartCmc.Trading] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [ShoppingCartCmc.Trading] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [ShoppingCartCmc.Trading] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [ShoppingCartCmc.Trading] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [ShoppingCartCmc.Trading] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [ShoppingCartCmc.Trading] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [ShoppingCartCmc.Trading] SET  MULTI_USER 
GO
ALTER DATABASE [ShoppingCartCmc.Trading] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [ShoppingCartCmc.Trading] SET DB_CHAINING OFF 
GO
ALTER DATABASE [ShoppingCartCmc.Trading] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [ShoppingCartCmc.Trading] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [ShoppingCartCmc.Trading] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [ShoppingCartCmc.Trading] SET QUERY_STORE = OFF
GO
USE [ShoppingCartCmc.Trading]
GO
/****** Object:  Table [dbo].[Billing]    Script Date: 12/06/2022 9:19:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Billing](
	[Id] [int] IDENTITY(100,1) NOT NULL,
	[Key] [nchar](50) NOT NULL,
	[Subtotal] [decimal](18, 2) NULL,
	[ShippingFee] [decimal](18, 2) NULL,
	[Total] [decimal](18, 2) NULL,
	[FirstName] [nvarchar](50) NULL,
	[LastName] [nvarchar](50) NULL,
	[EmailId] [nvarchar](250) NULL,
	[Address1] [nvarchar](500) NULL,
	[Address2] [nvarchar](500) NULL,
	[Country] [nvarchar](500) NULL,
	[State] [nvarchar](50) NULL,
	[Zip] [nvarchar](50) NULL,
 CONSTRAINT [PK_Billing_1] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BillingProduct]    Script Date: 12/06/2022 9:19:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BillingProduct](
	[BillingKey] [nchar](50) NULL,
	[ProductKey] [nchar](50) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Product]    Script Date: 12/06/2022 9:19:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Product](
	[Key] [nchar](50) NOT NULL,
	[Id] [int] NULL,
	[Name] [nvarchar](250) NULL,
	[Category] [nvarchar](250) NULL,
	[Price] [decimal](18, 2) NULL,
	[Description] [nchar](2000) NULL,
	[ImageUrl] [nvarchar](500) NULL,
	[TimeTickAdded] [int] NULL,
	[Quantity] [int] NULL,
	[Rating] [decimal](18, 2) NULL,
	[Favourite] [bit] NULL,
	[Seller] [nvarchar](250) NULL,
	[Currency] [varchar](10) NULL,
 CONSTRAINT [PK_Product] PRIMARY KEY CLUSTERED 
(
	[Key] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Billing] ON 

INSERT [dbo].[Billing] ([Id], [Key], [Subtotal], [ShippingFee], [Total], [FirstName], [LastName], [EmailId], [Address1], [Address2], [Country], [State], [Zip]) VALUES (100, N'ENdYdxKbqvcv                                      ', CAST(0.00 AS Decimal(18, 2)), CAST(20.00 AS Decimal(18, 2)), CAST(4020.00 AS Decimal(18, 2)), N'Patrick', N'Wei', N'patrickw@myemail.com', N'100 Queen St', N'NA', N'New Zealand', N'Auckland', N'1001      ')
INSERT [dbo].[Billing] ([Id], [Key], [Subtotal], [ShippingFee], [Total], [FirstName], [LastName], [EmailId], [Address1], [Address2], [Country], [State], [Zip]) VALUES (101, N'ICTxzmAxWJpY                                      ', CAST(0.00 AS Decimal(18, 2)), CAST(20.00 AS Decimal(18, 2)), CAST(4020.00 AS Decimal(18, 2)), N'Patrick', N'Wei', N'patrickw@myemail.com', N'100 Queen St', N'NA', N'New Zealand', N'Auckland', N'1001      ')
INSERT [dbo].[Billing] ([Id], [Key], [Subtotal], [ShippingFee], [Total], [FirstName], [LastName], [EmailId], [Address1], [Address2], [Country], [State], [Zip]) VALUES (102, N'qrlqhyCdUhAFS                                     ', CAST(0.00 AS Decimal(18, 2)), CAST(20.00 AS Decimal(18, 2)), CAST(4020.00 AS Decimal(18, 2)), N'Patrick', N'Wei', N'patrickw@myemail.com', N'100 Queen St', N'NA', N'New Zealand', N'Auckland', N'1001')
INSERT [dbo].[Billing] ([Id], [Key], [Subtotal], [ShippingFee], [Total], [FirstName], [LastName], [EmailId], [Address1], [Address2], [Country], [State], [Zip]) VALUES (103, N'CXbQPUDpTlh                                       ', CAST(0.00 AS Decimal(18, 2)), CAST(20.00 AS Decimal(18, 2)), CAST(4020.00 AS Decimal(18, 2)), N'Patrick', N'Wei', N'patrickw@myemail.com', N'100 Queen St', N'NA', N'New Zealand', N'Auckland', N'1001')
SET IDENTITY_INSERT [dbo].[Billing] OFF
INSERT [dbo].[Product] ([Key], [Id], [Name], [Category], [Price], [Description], [ImageUrl], [TimeTickAdded], [Quantity], [Rating], [Favourite], [Seller], [Currency]) VALUES (N'L1HnndxVc2-KaJ10Skc                               ', 10001, N'Apple iPhone X 64,GB', N'Smartphone', CAST(8000.00 AS Decimal(18, 2)), N'The iPhone X models have a 5.8\" (diagonal) widescreen LED-backlit True Tone, wide color (P3) \"Super Retina\" with 3D Touch and a 2436x1125 native resolution at 458 ppi                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                       ', N'https://i.ibb.co/ZVGQZsb/iphone-x-in-hand.jpg', 1654414949, 99, CAST(5.00 AS Decimal(18, 2)), 1, N'Apple', N'AUD')
INSERT [dbo].[Product] ([Key], [Id], [Name], [Category], [Price], [Description], [ImageUrl], [TimeTickAdded], [Quantity], [Rating], [Favourite], [Seller], [Currency]) VALUES (N'LG0nDkGo2w9ey5JLhaf-KaJ10Skc                      ', 10002, N'RealMe 1 (Silver)', N'Smartphone', CAST(4000.00 AS Decimal(18, 2)), N'13MP primary camera with beautify, filter, HDR, panorama, ultra HD and 8MP front facing camera 15.2 centimeters (6-inch) 1080p FHD capacitive touchscreen with 2160 x 1080 pixels resolution and 403 ppi pixel density Android v8.1 Oreo operating system with 2GHz MTK Helio P60 AI octa core processor with 8-cores CPU, 4GB RAM, 64GB internal memory expandable up to 256GB and dual SIM(nano+nano) dual-standby(4G+4G) 3410mAH lithium-ion battery providing talk-time of 30 hours and standby time of 380 hours 1 year manufacturer warranty for device and 6 months manufacturer warranty for in-box accessories including batteries from the date of purchase                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           ', N'https://i.ibb.co/5jnXDv7/realme1.jpg', 1654414950, 99, CAST(4.00 AS Decimal(18, 2)), 0, N'Realme', N'AUD')
INSERT [dbo].[Product] ([Key], [Id], [Name], [Category], [Price], [Description], [ImageUrl], [TimeTickAdded], [Quantity], [Rating], [Favourite], [Seller], [Currency]) VALUES (N'LG0ndKVM8VGYxVUH7qC                               ', 10003, N'Moto G6 (Indigo Black)', N'Smartphone', CAST(4500.00 AS Decimal(18, 2)), N'12+5MP dual rear cameras (f/1.8, single LED flash) with creative camera system; 16MP front facing camera with low light mode and LED flash Unlock your phone by simply letting the camera see your face. It knows who you are thanks to the face recognition software, so you don’t need to enter your password 14.5cm(5.7) FHD + 18:9 Max Vision display with 1080 * 2160 pixels resolution; Premium 3D glass black 4GB RAM and 64GB internal memory expandable up to 256GB; Android v8.0 Oreo operating system with Snapdragon 450 1.8GHz Octa-core processor 3000mAh battery with 15W TurboPowerTM charging 1 year manufacturer warranty for device and 6 months manufacturer warranty for in-box accessories including battery from the date of purchase                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                    ', N'https://i.ibb.co/d5QB740/moto-g6-2.jpg', 1654414951, 89, CAST(3.00 AS Decimal(18, 2)), 0, N'Motorolla', N'AUD')
INSERT [dbo].[Product] ([Key], [Id], [Name], [Category], [Price], [Description], [ImageUrl], [TimeTickAdded], [Quantity], [Rating], [Favourite], [Seller], [Currency]) VALUES (N'LIj11ZwXMhvnB6K8fae                               ', 10004, N'Realme 1 (Diamond)', N'Smartphone', CAST(4000.00 AS Decimal(18, 2)), N'13MP primary Camera with beautify, filter                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                       ', N'https://i.ibb.co/5jnXDv7/realme2.jpg', 1654414952, 89, CAST(4.00 AS Decimal(18, 2)), 0, N'Oppo', N'AUD')
INSERT [dbo].[Product] ([Key], [Id], [Name], [Category], [Price], [Description], [ImageUrl], [TimeTickAdded], [Quantity], [Rating], [Favourite], [Seller], [Currency]) VALUES (N'LXsQ2ImqAdLlHJFAwor                               ', 10005, N'Nokia 8.1', N'Smartphone', CAST(69.00 AS Decimal(18, 2)), N'4 GB RAM | 64 GB ROM | 15.7 cm (6.18 inch) Full HD+ Display 12MP + 13MP | 20MP Front Camera 3500 mAh Battery Qualcomm Snapdragon 710 Processor                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                  ', N'https://i.ibb.co/g9Vk9jc/nokia8-1.jpg', 1654414953, 69, CAST(2.00 AS Decimal(18, 2)), 0, N'Nokia', N'AUD')
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Billing]    Script Date: 12/06/2022 9:19:51 PM ******/
CREATE NONCLUSTERED INDEX [IX_Billing] ON [dbo].[Billing]
(
	[Key] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
USE [master]
GO
ALTER DATABASE [ShoppingCartCmc.Trading] SET  READ_WRITE 
GO

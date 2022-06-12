USE [master]
GO
/****** Object:  Database [ShoppingCartCmc.Trading]    Script Date: 12/06/2022 12:15:53 PM ******/
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
/****** Object:  Table [dbo].[Billing]    Script Date: 12/06/2022 12:15:53 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Billing](
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
	[Zip] [int] NULL,
 CONSTRAINT [PK_Billing] PRIMARY KEY CLUSTERED 
(
	[Key] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BillingProduct]    Script Date: 12/06/2022 12:15:53 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BillingProduct](
	[BillingKey] [nchar](50) NULL,
	[ProductKey] [nchar](50) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Product]    Script Date: 12/06/2022 12:15:53 PM ******/
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
USE [master]
GO
ALTER DATABASE [ShoppingCartCmc.Trading] SET  READ_WRITE 
GO

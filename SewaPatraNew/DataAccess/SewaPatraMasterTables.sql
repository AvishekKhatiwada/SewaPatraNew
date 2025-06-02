-------------------------- Master Tables --------------------------
-------AREA MASTER TABLE START-------------------------------
CREATE TABLE [dbo].[Area_Master](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Area_name] [varchar](25) NULL,
	[Area_type] [varchar](15) NULL,
	[Under] [varchar](15) NULL,
	[CreatedBy] [varchar](30) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
-------AREA MASTER TABLE END-------------------------------

-------Coordinator MASTER TABLE START-------------------------------
CREATE TABLE [dbo].[Coordinator_master](
    [ID] [int] IDENTITY(1,1) NOT NULL,
    [Name] [varchar](125) NOT NULL,
    [Mobile_No] [varchar](15) NOT NULL,
    [Alternate_Mobile_No] [varchar](15) NULL,
    [Address] [varchar](125) NULL,
    [City] [varchar](25) NULL,
    [Email] [varchar](255) NULL,
    [Area_Under] [varchar](25) NULL,
    [Active] [bit] NULL,
    [CreatedAt] [datetime] DEFAULT (getdate()) NULL, -- Default constraint added here
    [Report_to] [varchar](25) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[Email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
-------Coordinator MASTER TABLE END-------------------------------

-------Donor MASTER TABLE START-------------------------------
CREATE TABLE [dbo].[Donor_master](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Mobile_no] [varchar](15) NOT NULL,
	[Name] [varchar](125) NOT NULL,
	[Address] [varchar](125) NULL,
	[City] [varchar](25) NULL,
	[Mobile_no2] [varchar](15) NULL,
	[Email] [varchar](255) NULL,
	[Area] [int] NULL,
	[Coordinator] [int] NULL,
	[Location] [varchar](MAX) NULL,
	[Active] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Donor_master]  WITH CHECK ADD FOREIGN KEY([Coordinator])
REFERENCES [dbo].[Coordinator_master] ([ID])
GO

ALTER TABLE [dbo].[Donor_master]  WITH CHECK ADD FOREIGN KEY([Area])
REFERENCES [dbo].[Area_Master] ([ID])
-------Donor MASTER TABLE END-------------------------------

-------DonationBox MASTER TABLE START-------------------------------
CREATE TABLE [dbo].[DonationBox](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Box_Number] [varchar](100) NULL,
	[Active] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
-------DonationBox MASTER TABLE END-------------------------------

-------User MASTER TABLE START-------------------------------
CREATE TABLE [dbo].[Users](
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [FullName] [nvarchar](100) NOT NULL,
    [Number] [nvarchar](15) NOT NULL,
    [Email] [nvarchar](100) NOT NULL,
    [Password] [nvarchar](max) NOT NULL, -- Remember to hash passwords!
    [Role] [nvarchar](50) NOT NULL,
    [CreatedAt] [datetime2](7) DEFAULT (GETDATE()) NULL, -- Default added
PRIMARY KEY CLUSTERED 
(
    [Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
CONSTRAINT [UQ_Users_Email] UNIQUE NONCLUSTERED 
(
    [Email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
CONSTRAINT [CHK_Users_Number] CHECK (len([Number]) >= 10 AND len([Number]) <= 15 AND NOT [Number] LIKE '%[^0-9]%'),
CONSTRAINT [CHK_Users_Password] CHECK (len([Password]) >= 8)
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY];
-------User MASTER TABLE END-------------------------------

-------------------------- Entry Tables --------------------------
------------------------SewaPatra Issue Table Start ---------------
CREATE TABLE [dbo].[SewaPatraIssue](
	[TranId] [varchar](100) NOT NULL,
	[Entered_Date] [datetime] NOT NULL,
	[Donor] [int] NOT NULL,
	[Coordinator] [int] NOT NULL,
	[DonationBox] [int] NOT NULL,
	[Issue_Date] [datetime] NOT NULL,
	[Recurring] [varchar](255) NULL,
	[Due_Date] [datetime] NULL,
	[Remarks] [varchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[TranId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
------------------------SewaPatra Issue Table END ---------------

------------------------SewaPatra Receipt Table Start ---------------
CREATE TABLE [dbo].[SewaPatraReceipt](
	[SPR_TranId] [varchar](100) NOT NULL,
	[Date] [datetime] NOT NULL,
	[Donor] [int] NOT NULL,
	[Coordinator] [int] NOT NULL,
	[DonationBox] [int] NOT NULL,
	[Receive_Date] [datetime] NOT NULL,
	[Remarks] [varchar](max) NULL,
	[SPI_Id] [varchar](100) NOT NULL
PRIMARY KEY CLUSTERED 
(
	[SPR_TranId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
------------------------SewaPatra Receipt Table END ---------------

------------------------PaymentVoucher Table Start ---------------
CREATE TABLE [dbo].[PaymentVoucher](
	[P_TranId] [varchar](255) NOT NULL,
	[Date] [varchar](255) NULL,
	[Ledger_Name] [varchar](255) NULL,
	[Coordinator] [int] NULL,
	[Amount] [varchar](100) NULL,
	[Remarks] [varchar](255) NULL,
 CONSTRAINT [PK__PaymentV__245437C9B303FD03] PRIMARY KEY CLUSTERED 
(
	[P_TranId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
------------------------PaymentVoucher Table END ---------------

------------------------ReceiptVoucher Table Start -------------
CREATE TABLE [dbo].[ReceiptVoucher](
	[R_TranId] [varchar](255) NOT NULL,
	[Date] [varchar](255) NULL,
	[Sewapatra_No] [int] NOT NULL,
	[Donor] [int] NOT NULL,
	[Coordinator] [int] NOT NULL,
	[Amount] [varchar](255) NULL,
	[Next_DueDate] [varchar](255) NULL,
	[Remarks] [varchar](255) NULL,
	[SPI_Id] [varchar](100) NOT NULL
PRIMARY KEY CLUSTERED 
(
	[R_TranId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
------------------------ReceiptVoucher Table END ---------------

-------------------------- Report Queries ----------------------
--Coordinator Listing 
Select ROW_NUMBER() OVER (ORDER BY CM.id) AS SNo,CM.id as [Coordinator ID],Name as [Name],Mobile_No As [Contact],Alternate_Mobile_No As [Alt.Contact],Address AS [Address],City As [City],Email,
Area_Under As [Area Code],Am.Area_name As [Area],AM.Area_type As [Area Type],AM.Under AreaUnder
From Coordinator_master CM
Inner Join Area_Master AM On AM.Id=CM.Area_Under
Where 1=1 ORDER BY CM.Name ASC


--Donor Listing
Select ROW_NUMBER() OVER (ORDER BY DM.id) AS SNo,DM.Id As [DonorId],DM.Mobile_no As [Contact],DM.Name As [name],DM.Address As [Address],Dm.City As [City],DM.Mobile_no2 as [Alt.Contact],
DM.Email As [Email],DM.Area As [Area Code],CM.Name AS [Coordinator Name],AM.Area_name As [Area]
from Donor_master DM 
Inner Join Area_Master AM On Am.ID=Dm.Area
Inner Join Coordinator_master CM ON CM.ID=DM.Coordinator

--DonationBox Listing
Select ROW_NUMBER() OVER (ORDER BY ID) AS Sno,ID BoxID,Box_Number [SewaPatra No],
CASE Active WHEN Active THEN 'Active' ELSE 'InActive' END AS Status FROM DonationBox

--Payment Register
Select ROW_NUMBER() OVER (ORDER BY P_TranId) AS Sno,P_TranId, CONVERT(DATE,[Date],103) As [Date],Ledger_Name As [Bank Name],
Amount,Cm.Name AS Coordinator,AM.Area_name Area,Remarks
from PaymentVoucher PV
Inner Join Coordinator_master CM On CM.Id=PV.Coordinator
INNER JOIN Area_Master AM ON AM.ID=CM.Area_Under
Where 1=1 ORDER BY PV.[Date] ASC

--Receipt Voucher Register
Select ROW_NUMBER() OVER (ORDER BY R_TranId) As Sno,R_TranId, [Date],db.Box_Number As [Donation Box],DM.Name As [Donor],AM1.Area_name DonorArea,
Amount,Cm.Name As [Coordinator],AM.Area_name CoordinatorArea,Next_DueDate As [Next Due],Remarks
from ReceiptVoucher RV
LEFT OUTER Join Coordinator_master CM On CM.Id=RV.Coordinator
LEFT OUTER JOIN Area_Master AM ON AM.ID=CM.Area_Under
LEFT OUTER join DonationBox DB ON Db.Id=RV.Sewapatra_No
LEFT OUTER Join Donor_master DM ON DM.Id=RV.Donor
LEFT OUTER JOIN Area_Master AM1 ON AM1.ID=DM.Area
Where 1=1 ORDER BY RV.[Date] ASC

--SewaPatra Issue Register
SELECT TranId,Entered_Date As [Date], SPI.Donor As DonorId,DM.Name As DonorName,DMA.Area_name As DonorArea, SPI.Coordinator As CoordinatorId
,CM.Name As CoordinatorName,CMA.Area_name As CoordinatorArea,DonationBox As BoxId,Db.Box_Number As DonationBox,Issue_Date As IssueDate, Recurring, Due_Date As DueDate, Remarks 
FROM SewaPatraIssue SPI
INNER JOIN Donor_master DM ON DM.Id=SPI.Donor
INNER Join Area_Master DMA ON DM.Area=DMA.Id
INNER JOIN Coordinator_master CM ON CM.Id=SPI.Coordinator
INNER JOIN Area_Master CMA ON CM.Area_Under=CMA.Id 
INNER JOIN DonationBox DB ON Db.Id=SPI.DonationBox
WHERE 1=1 ORDER BY Entered_Date

--SewaPatra Receipt Register
SELECT SPR_TranId,Date As [Date], SPR.Donor As DonorId,DM.Name As DonorName,DMA.Area_name As DonorArea, SPR.Coordinator As CoordinatorId
,CM.Name As CoordinatorName,CMA.Area_name As CoordinatorArea,DonationBox As BoxId,Db.Box_Number As DonationBox,Receive_Date As ReceiveDate, Remarks 
FROM SewaPatraReceipt SPR
INNER JOIN Donor_master DM ON DM.Id=SPR.Donor
INNER Join Area_Master DMA ON DM.Area=DMA.Id
INNER JOIN Coordinator_master CM ON CM.Id=SPR.Coordinator
INNER JOIN Area_Master CMA ON CM.Area_Under=CMA.Id 
INNER JOIN DonationBox DB ON Db.Id=SPR.DonationBox
WHERE 1=1 ORDER BY [Date]

--Sewapatra Due Report
Select ROW_NUMBER() OVER (ORDER BY DonationBox) AS SNo,Db.Box_Number,SPI.Issue_Date As IssueDate,SPI.Due_Date As DueDate,DM.Name As Donor,
AM.Area_name As DonorArea,CM.Name As Coordinator
FROM SewaPatraIssue SPI
INNER JOIN DonationBox DB ON Db.Id=SPI.DonationBox
INNER JOIN Donor_master DM ON Dm.Id=SPI.Donor
INNER JOIN Coordinator_master CM ON CM.ID=SPI.Coordinator
INNER JOIN Area_Master AM ON Am.ID=Dm.Area
ORDER BY Due_Date DESC

--Collection Analysis
Select ROW_NUMBER() OVER (ORDER BY R_TranId) AS Sno, R_TranId, Date, RV.Sewapatra_No SewaPatraID, DB.Box_Number Sewapatra, RV.Donor DonorId, DM.Name DonorName,
RV.Coordinator As CoordinatorId,CM.Name AS Coordinator,Amount, Next_DueDate, Remarks,AM.Area_name
FROM ReceiptVoucher RV
INNER JOIN Coordinator_master CM ON CM.ID= RV.Coordinator
INNER JOIN Donor_master DM ON DM.Id=RV.Donor
INNER JOIN Area_Master AM ON AM.ID=Cm.Area_Under
INNER JOIN DonationBox DB ON Db.Id=RV.Donor
Order By RV.Date

----------------------------------SELECT QUERIES------------------------------

SELECT * FROM [dbo].[Area_Master]
SELECT * FROM [dbo].[Coordinator_master]
SELECT * FROM [dbo].[DonationBox]
SELECT * FROM [dbo].[Donor_master]
SELECT * FROM [dbo].[PaymentVoucher]
SELECT * FROM [dbo].[ReceiptVoucher]
SELECT * FROM [dbo].[SewaPatraIssue]
SELECT * FROM [dbo].[SewaPatraReceipt]
SELECT * FROM [dbo].[Users]

----------------------------------SELECT QUERIES------------------------------

----------------------------------DELETE QUERIES------------------------------

--DELETE FROM [dbo].[PaymentVoucher]
--DELETE FROM [dbo].[ReceiptVoucher]
--DELETE FROM [dbo].[SewaPatraIssue]
--DELETE FROM [dbo].[SewaPatraReceipt]
--DELETE FROM [dbo].[Area_Master]
--DELETE FROM [dbo].[Coordinator_master]
--DELETE FROM [dbo].[DonationBox]
--DELETE FROM [dbo].[Donor_master]
--DELETE FROM [dbo].[Users]

----------------------------------DELETE QUERIES------------------------------

USE [PatientRecordData]
GO

/****** Object:  Table [dbo].[Dosages]    Script Date: 11/19/2024 7:22:21 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Dosages](
	[DosageID] [int] IDENTITY(1,1) NOT NULL,
	[MedicationID] [int] NOT NULL,
	[Dosage] [nvarchar](100) NOT NULL,
	[Frequency] [nvarchar](100) NOT NULL,
	[StartDate] [date] NOT NULL,
	[EndDate] [date] NULL,
	[RouteOfAdministration] [nvarchar](100) NULL,
	[Instructions] [text] NULL,
	[DosageAmount] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK__Dosages__FD7BEAA25F0D76D6] PRIMARY KEY CLUSTERED 
(
	[DosageID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[Dosages]  WITH CHECK ADD  CONSTRAINT [FK__Dosages__Medicat__6FE99F9F] FOREIGN KEY([MedicationID])
REFERENCES [dbo].[Medications] ([MedicationID])
GO

ALTER TABLE [dbo].[Dosages] CHECK CONSTRAINT [FK__Dosages__Medicat__6FE99F9F]
GO



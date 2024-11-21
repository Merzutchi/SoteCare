USE [PatientRecordData]
GO

/****** Object:  Table [dbo].[MedicationLists]    Script Date: 11/20/2024 3:16:29 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[MedicationLists](
	[MedicationListID] [int] NOT NULL,
	[MedicationName] [varchar](255) NULL,
	[MedicationType] [varchar](255) NULL,
	[Description] [text] NULL,
PRIMARY KEY CLUSTERED 
(
	[MedicationListID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO


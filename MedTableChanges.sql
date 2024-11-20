USE [PatientRecordData]
GO

/****** Object:  Table [dbo].[Medications]    Script Date: 11/19/2024 8:38:53 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Medications](
	[MedicationID] [int] IDENTITY(1,1) NOT NULL,
	[PatientID] [int] NOT NULL,
	[MedicationName] [nvarchar](255) NOT NULL,
	[DoctorID] [int] NULL,
	[RefillStatus] [nvarchar](20) NULL,
	[MedicationStatus] [nvarchar](50) NULL,
	[Allergies] [text] NULL,
	[Comments] [text] NULL,
 CONSTRAINT [PK__Medicati__62EC1ADAD962DDB1] PRIMARY KEY CLUSTERED 
(
	[MedicationID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[Medications]  WITH CHECK ADD  CONSTRAINT [FK__Medicatio__Patie__4222D4EF] FOREIGN KEY([PatientID])
REFERENCES [dbo].[Patients] ([PatientID])
GO

ALTER TABLE [dbo].[Medications] CHECK CONSTRAINT [FK__Medicatio__Patie__4222D4EF]
GO



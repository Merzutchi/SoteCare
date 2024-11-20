USE [PatientRecordData]
GO

/****** Object:  Table [dbo].[PatientMedications]    Script Date: 11/20/2024 3:21:13 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PatientMedications](
	[PatientMedicationID] [int] IDENTITY(1,1) NOT NULL,
	[PatientID] [int] NULL,
	[MedicationID] [int] NULL,
	[StartDate] [date] NULL,
	[EndDate] [date] NULL,
	[MedicationListID] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[PatientMedicationID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[PatientMedications]  WITH CHECK ADD  CONSTRAINT [FK__PatientMe__Medic__09A971A2] FOREIGN KEY([MedicationID])
REFERENCES [dbo].[Medications] ([MedicationID])
GO

ALTER TABLE [dbo].[PatientMedications] CHECK CONSTRAINT [FK__PatientMe__Medic__09A971A2]
GO

ALTER TABLE [dbo].[PatientMedications]  WITH CHECK ADD FOREIGN KEY([PatientID])
REFERENCES [dbo].[Patients] ([PatientID])
GO

ALTER TABLE [dbo].[PatientMedications]  WITH CHECK ADD  CONSTRAINT [FK_PatientMedications_MedicationLists] FOREIGN KEY([MedicationListID])
REFERENCES [dbo].[MedicationLists] ([MedicationListID])
GO

ALTER TABLE [dbo].[PatientMedications] CHECK CONSTRAINT [FK_PatientMedications_MedicationLists]
GO


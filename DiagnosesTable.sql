/****** Object:  Table [dbo].[Diagnoses]    Script Date: 11/20/2024 3:03:04 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Diagnoses](
	[DiagnosisID] [int] IDENTITY(1,1) NOT NULL,
	[PatientID] [int] NULL,
	[DiagnosisName] [varchar](255) NULL,
	[DiagnosisDate] [date] NULL,
	[Notes] [varchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[DiagnosisID] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Diagnoses]  WITH CHECK ADD FOREIGN KEY([PatientID])
REFERENCES [dbo].[Patients] ([PatientID])
GO


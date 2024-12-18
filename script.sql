USE [PatientRecordData]
GO
/****** Object:  Table [dbo].[Doctors]    Script Date: 11/11/2024 8:25:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Doctors](
	[DoctorID] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [nvarchar](100) NULL,
	[LastName] [nvarchar](100) NULL,
	[Specialization] [nvarchar](100) NULL,
	[PhoneNumber] [nvarchar](20) NULL,
	[Email] [nvarchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[DoctorID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Medications]    Script Date: 11/11/2024 8:25:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Medications](
	[MedicationID] [int] IDENTITY(1,1) NOT NULL,
	[PatientID] [int] NOT NULL,
	[MedicationName] [nvarchar](255) NOT NULL,
	[Dosage] [nvarchar](100) NOT NULL,
	[StartDate] [date] NULL,
	[EndDate] [date] NULL,
	[Frequency] [nvarchar](100) NULL,
	[RouteOfAdministration] [nvarchar](100) NULL,
	[Instructions] [text] NULL,
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
/****** Object:  Table [dbo].[Nurses]    Script Date: 11/11/2024 8:25:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Nurses](
	[NurseID] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [nvarchar](100) NULL,
	[LastName] [nvarchar](100) NULL,
	[Department] [nvarchar](100) NULL,
	[PhoneNumber] [nvarchar](20) NULL,
	[Email] [nvarchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[NurseID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PatientHistory]    Script Date: 11/11/2024 8:25:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PatientHistory](
	[HistoryID] [int] IDENTITY(1,1) NOT NULL,
	[PatientID] [int] NOT NULL,
	[ConditionName] [nvarchar](255) NULL,
	[DiagnosisDate] [date] NULL,
	[TreatmentDetails] [nvarchar](255) NULL,
	[SurgeryDate] [date] NULL,
	[Notes] [nvarchar](255) NULL,
 CONSTRAINT [PK__PatientH__4D7B4ADD8EAE5B58] PRIMARY KEY CLUSTERED 
(
	[HistoryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Patients]    Script Date: 11/11/2024 8:25:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Patients](
	[PatientID] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [nvarchar](100) NOT NULL,
	[LastName] [nvarchar](100) NOT NULL,
	[DateOfBirth] [date] NOT NULL,
	[Gender] [nvarchar](10) NOT NULL,
	[Address] [nvarchar](255) NULL,
	[PhoneNumber] [nvarchar](20) NULL,
	[Email] [nvarchar](100) NULL,
	[EmergencyContactName] [nvarchar](100) NULL,
	[EmergencyContactPhone] [nvarchar](20) NULL,
 CONSTRAINT [PK__Patients__970EC3463632D6DA] PRIMARY KEY CLUSTERED 
(
	[PatientID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Treatment]    Script Date: 11/11/2024 8:25:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Treatment](
	[TreatmentID] [int] IDENTITY(1,1) NOT NULL,
	[PatientID] [int] NOT NULL,
	[MedicationID] [int] NULL,
	[StartDate] [date] NULL,
	[EndDate] [date] NULL,
	[Dosage] [nvarchar](100) NULL,
	[TreatmentType] [nvarchar](100) NULL,
 CONSTRAINT [PK__Treatmen__1A57B7111972126E] PRIMARY KEY CLUSTERED 
(
	[TreatmentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TreatmentMedications]    Script Date: 11/11/2024 8:25:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TreatmentMedications](
	[TreatmentID] [int] NOT NULL,
	[MedicationID] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[TreatmentID] ASC,
	[MedicationID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 11/11/2024 8:25:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[UserID] [int] IDENTITY(1,1) NOT NULL,
	[Username] [nvarchar](100) NOT NULL,
	[Password] [nvarchar](255) NOT NULL,
	[Role] [nvarchar](50) NOT NULL,
	[FullName] [nvarchar](255) NOT NULL,
	[Email] [nvarchar](100) NOT NULL,
	[PhoneNumber] [nvarchar](20) NULL,
	[DateOfBirth] [date] NULL,
	[IsActive] [bit] NULL,
 CONSTRAINT [PK__Users__1788CCAC92296CB7] PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UQ__Users__536C85E4FCAFB08A] UNIQUE NONCLUSTERED 
(
	[Username] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[VitalFunctions]    Script Date: 11/11/2024 8:25:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[VitalFunctions](
	[VitalFunctionID] [int] IDENTITY(1,1) NOT NULL,
	[PatientID] [int] NOT NULL,
	[DateTime] [datetime] NOT NULL,
	[HeartRate] [int] NULL,
	[SystolicBloodPressure] [int] NULL,
	[DiastolicBloodPressure] [int] NULL,
	[RespiratoryRate] [int] NULL,
	[Temperature] [decimal](5, 2) NULL,
	[OxygenSaturation] [decimal](5, 2) NULL,
 CONSTRAINT [PK__VitalFun__321D9280BCAC7743] PRIMARY KEY CLUSTERED 
(
	[VitalFunctionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Medications]  WITH CHECK ADD  CONSTRAINT [FK__Medicatio__Patie__4222D4EF] FOREIGN KEY([PatientID])
REFERENCES [dbo].[Patients] ([PatientID])
GO
ALTER TABLE [dbo].[Medications] CHECK CONSTRAINT [FK__Medicatio__Patie__4222D4EF]
GO
ALTER TABLE [dbo].[PatientHistory]  WITH CHECK ADD  CONSTRAINT [FK__PatientHi__Patie__3F466844] FOREIGN KEY([PatientID])
REFERENCES [dbo].[Patients] ([PatientID])
GO
ALTER TABLE [dbo].[PatientHistory] CHECK CONSTRAINT [FK__PatientHi__Patie__3F466844]
GO
ALTER TABLE [dbo].[Treatment]  WITH CHECK ADD  CONSTRAINT [FK__Treatment__Patie__3C69FB99] FOREIGN KEY([PatientID])
REFERENCES [dbo].[Patients] ([PatientID])
GO
ALTER TABLE [dbo].[Treatment] CHECK CONSTRAINT [FK__Treatment__Patie__3C69FB99]
GO
ALTER TABLE [dbo].[TreatmentMedications]  WITH CHECK ADD  CONSTRAINT [FK__Treatment__Medic__45F365D3] FOREIGN KEY([MedicationID])
REFERENCES [dbo].[Medications] ([MedicationID])
GO
ALTER TABLE [dbo].[TreatmentMedications] CHECK CONSTRAINT [FK__Treatment__Medic__45F365D3]
GO
ALTER TABLE [dbo].[TreatmentMedications]  WITH CHECK ADD  CONSTRAINT [FK__Treatment__Treat__44FF419A] FOREIGN KEY([TreatmentID])
REFERENCES [dbo].[Treatment] ([TreatmentID])
GO
ALTER TABLE [dbo].[TreatmentMedications] CHECK CONSTRAINT [FK__Treatment__Treat__44FF419A]
GO
ALTER TABLE [dbo].[VitalFunctions]  WITH CHECK ADD  CONSTRAINT [FK__VitalFunc__Patie__398D8EEE] FOREIGN KEY([PatientID])
REFERENCES [dbo].[Patients] ([PatientID])
GO
ALTER TABLE [dbo].[VitalFunctions] CHECK CONSTRAINT [FK__VitalFunc__Patie__398D8EEE]
GO

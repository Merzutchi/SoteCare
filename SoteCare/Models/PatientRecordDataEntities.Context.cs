﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SoteCare.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class PatientRecordDataEntities : DbContext
    {
        public PatientRecordDataEntities()
            : base("name=PatientRecordDataEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Diagnoses> Diagnoses { get; set; }
        public virtual DbSet<Doctors> Doctors { get; set; }
        public virtual DbSet<Dosages> Dosages { get; set; }
        public virtual DbSet<MedicationLists> MedicationLists { get; set; }
        public virtual DbSet<Medications> Medications { get; set; }
        public virtual DbSet<Nurses> Nurses { get; set; }
        public virtual DbSet<PatientHistory> PatientHistory { get; set; }
        public virtual DbSet<PatientMedications> PatientMedications { get; set; }
        public virtual DbSet<Patients> Patients { get; set; }
        public virtual DbSet<Treatment> Treatment { get; set; }
        public virtual DbSet<TreatmentDetails> TreatmentDetails { get; set; }
        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<VitalFunctions> VitalFunctions { get; set; }
    }
}
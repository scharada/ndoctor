/*
    This file is part of NDoctor.

    NDoctor is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    NDoctor is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with NDoctor.  If not, see <http://www.gnu.org/licenses/>.
*/
namespace Probel.NDoctor.Domain.DAL.Entities
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Represents a patient
    /// </summary>
    public class Patient : Person
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Patient"/> class.
        /// </summary>
        public Patient()
        {
            this.Address = new Address();
            this.BmiHistory = new List<Bmi>();
            this.MedicalRecords = new List<MedicalRecord>();
            this.Pictures = new List<Picture>();
            this.IllnessHistory = new List<IllnessPeriod>();
            this.PrescriptionDocuments = new List<PrescriptionDocument>();
            this.Appointments = new List<Appointment>();
            this.Doctors = new List<Doctor>();
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets the age of the patient.
        /// This is a calculated property based on the Birthdate
        /// This is ignored by nHibernate mapping
        /// </summary>
        public virtual int Age
        {
            get
            {
                DateTime today = DateTime.Today;
                int age = today.Year - this.BirthDate.Year;
                if (BirthDate > today.AddYears(-age)) age--;
                return age;
            }
        }

        /// <summary>
        /// Gets or sets the meetings of the patients.
        /// </summary>
        /// <value>
        /// The meetings.
        /// </value>
        public virtual IList<Appointment> Appointments
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the birth date.
        /// </summary>
        /// <value>
        /// The birth date.
        /// </value>
        public virtual DateTime BirthDate
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the bmi history.
        /// </summary>
        /// <value>
        /// The bmi history.
        /// </value>
        public virtual IList<Bmi> BmiHistory
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the doctors linked to this patient.
        /// </summary>
        /// <value>
        /// The doctors.
        /// </value>
        public virtual IList<Doctor> Doctors
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the father of this patient.
        /// </summary>
        /// <value>
        /// The father.
        /// </value>
        public virtual Patient Father
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the fee.
        /// </summary>
        /// <value>
        /// The fee.
        /// </value>
        public virtual decimal Fee
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the height.
        /// </summary>
        /// <value>
        /// The height.
        /// </value>
        public virtual long Height
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the illness history that's the list of of periods when the patient
        /// was ill.
        /// </summary>
        /// <value>
        /// The illness history.
        /// </value>
        public virtual IList<IllnessPeriod> IllnessHistory
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the inscription date.
        /// </summary>
        /// <value>
        /// The inscription date.
        /// </value>
        public virtual DateTime InscriptionDate
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the insurance.
        /// </summary>
        /// <value>
        /// The insurance.
        /// </value>
        public virtual Insurance Insurance
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this patient is deactivated.
        /// Deactivated means that this patient is logically deleted but still in
        /// the database
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this patient is deactivated; otherwise, <c>false</c>.
        /// </value>
        public virtual bool IsDeactivated
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the medical records of the patient.
        /// </summary>
        /// <value>
        /// The medical records.
        /// </value>
        public virtual IList<MedicalRecord> MedicalRecords
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the mother of this patient.
        /// </summary>
        /// <value>
        /// The mother.
        /// </value>
        public virtual Patient Mother
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the pictures of the patient.
        /// </summary>
        /// <value>
        /// The pictures.
        /// </value>
        public virtual IList<Picture> Pictures
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the place of birth.
        /// </summary>
        /// <value>
        /// The place of birth.
        /// </value>
        public virtual string PlaceOfBirth
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the practice.
        /// </summary>
        /// <value>
        /// The practice.
        /// </value>
        public virtual Practice Practice
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the prescriptions documents of this patient.
        /// </summary>
        /// <value>
        /// The prescriptions.
        /// </value>
        public virtual IList<PrescriptionDocument> PrescriptionDocuments
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the private mail.
        /// </summary>
        /// <value>
        /// The private mail.
        /// </value>
        public virtual string PrivateMail
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the private mobile.
        /// </summary>
        /// <value>
        /// The private mobile.
        /// </value>
        public virtual string PrivateMobile
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the private phone.
        /// </summary>
        /// <value>
        /// The private phone.
        /// </value>
        public virtual string PrivatePhone
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the profession.
        /// </summary>
        /// <value>
        /// The profession.
        /// </value>
        public virtual Profession Profession
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the reason.
        /// </summary>
        /// <value>
        /// The reason.
        /// </value>
        public virtual string Reason
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the reputation.
        /// </summary>
        /// <value>
        /// The reputation.
        /// </value>
        public virtual Reputation Reputation
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the list of tags used for search.
        /// </summary>
        /// <value>
        /// The search tags.
        /// </value>
        public virtual IList<SearchTag> SearchTags
        {
            get; set;
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("Patient: {0} {1}", this.FirstName, this.LastName);
        }

        #endregion Methods
    }
}
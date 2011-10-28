namespace Probel.NDoctor.Plugins.FamilyManager.ViewModel
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    using AutoMapper;

    using Probel.Helpers.Conversions;
    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.Plugins.FamilyManager.Properties;
    using Probel.NDoctor.View.Core.ViewModel;
    using Probel.NDoctor.View.Plugins.Helpers;

    using StructureMap;

    public class RemoveFamilyViewModel : BaseViewModel
    {
        #region Fields

        private IFamilyComponent component = ObjectFactory.GetInstance<IFamilyComponent>();
        private LightPatientViewModel selectedPatient;

        #endregion Fields

        #region Constructors

        public RemoveFamilyViewModel()
            : base()
        {
            this.FamilyMembers = new ObservableCollection<LightPatientViewModel>();
        }

        #endregion Constructors

        #region Properties

        public ObservableCollection<LightPatientViewModel> FamilyMembers
        {
            get;
            private set;
        }

        public LightPatientViewModel SelectedPatient
        {
            get { return this.selectedPatient; }
            set
            {
                this.selectedPatient = value;
                this.OnPropertyChanged("SelectedPatient");
            }
        }

        #endregion Properties

        #region Methods

        public void Refresh()
        {
            using (this.component.UnitOfWork)
            {
                var result = this.component.GetAllFamilyMembers(this.Host.SelectedPatient);
                var mapped = Mapper.Map<IList<LightPatientDto>, IList<LightPatientViewModel>>(result);

                for (int i = 0; i < mapped.Count; i++)
                {
                    mapped[i].SessionPatient = this.Host.SelectedPatient;
                    mapped[i].Refreshed += (sender, e) =>
                    {
                        this.Refresh();
                        if (e.Data == State.Added) { this.Host.WriteStatus(StatusType.Info, Messages.Msg_RelationAdded); }
                        else if (e.Data == State.Removed) { this.Host.WriteStatus(StatusType.Info, Messages.Msg_RelationRemoved); }
                    };
                }
                this.FamilyMembers.Refill(mapped);
                this.Host.WriteStatusReady();
            }
        }

        #endregion Methods
    }
}
namespace Probel.NDoctor.Plugins.MedicalRecord.Helpers
{
    using System;

    public static class Notifyer
    {
        #region Events

        /// <summary>
        /// Occurs when refreshed the plugin needs a refresh of all the data.
        /// </summary>
        public static event EventHandler Refreshed;

        #endregion Events

        #region Methods

        /// <summary>
        /// Called to trigger the <see cref="Refreshed"/> event 
        /// </summary>
        public static void OnRefreshed()
        {
            if (Refreshed != null)
                Refreshed(new Object(), EventArgs.Empty);
        }

        #endregion Methods
    }
}
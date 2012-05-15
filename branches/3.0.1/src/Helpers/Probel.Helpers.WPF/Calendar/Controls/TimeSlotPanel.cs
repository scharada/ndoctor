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
namespace Probel.Helpers.WPF.Calendar.Controls
{
    using System;
    using System.Windows;
    using System.Windows.Controls;

    public class TimeSlotPanel : Panel
    {
        #region Fields

        /// <summary>
        /// EndTime Attached Dependency Property
        /// </summary>
        public static readonly DependencyProperty EndTimeProperty = 
            DependencyProperty.RegisterAttached("EndTime", typeof(DateTime), typeof(TimeSlotPanel),
                new FrameworkPropertyMetadata((DateTime)DateTime.Now));

        /// <summary>
        /// Indentation Attached Dependency Property
        /// </summary>
        public static readonly DependencyProperty IndentationProperty = 
            DependencyProperty.RegisterAttached("Indentation", typeof(uint), typeof(TimeSlotPanel),
            new FrameworkPropertyMetadata((uint)0));

        /// <summary>
        /// StartTime Attached Dependency Property
        /// </summary>
        public static readonly DependencyProperty StartTimeProperty = 
            DependencyProperty.RegisterAttached("StartTime", typeof(DateTime), typeof(TimeSlotPanel),
                new FrameworkPropertyMetadata((DateTime)DateTime.Now));

        #endregion Fields

        #region Methods

        /// <summary>
        /// Gets the EndTime property.  This dependency property 
        /// indicates ....
        /// </summary>
        public static DateTime GetEndTime(DependencyObject d)
        {
            return (DateTime)d.GetValue(EndTimeProperty);
        }

        /// <summary>
        /// Sets the Indentation property.  This dependency property 
        /// indicates ....
        /// </summary>
        public static uint GetIndentation(DependencyObject d)
        {
            return (uint)d.GetValue(IndentationProperty);
        }

        /// <summary>
        /// Gets the StartTime property.  This dependency property 
        /// indicates ....
        /// </summary>
        public static DateTime GetStartTime(DependencyObject d)
        {
            return (DateTime)d.GetValue(StartTimeProperty);
        }

        /// <summary>
        /// Sets the EndTime property.  This dependency property 
        /// indicates ....
        /// </summary>
        public static void SetEndTime(DependencyObject d, DateTime value)
        {
            d.SetValue(EndTimeProperty, value);
        }

        /// <summary>
        /// Sets the Indentation property.  This dependency property 
        /// indicates ....
        /// </summary>
        public static void SetIndentation(DependencyObject d, uint value)
        {
            d.SetValue(IndentationProperty, value);
        }

        /// <summary>
        /// Sets the StartTime property.  This dependency property 
        /// indicates ....
        /// </summary>
        public static void SetStartTime(DependencyObject d, DateTime value)
        {
            d.SetValue(StartTimeProperty, value);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            double top = 0;
            double left = 0;
            double width = 0;
            double height = 0;

            foreach (UIElement element in this.Children)
            {
                Nullable<DateTime> startTime = element.GetValue(TimeSlotPanel.StartTimeProperty) as Nullable<DateTime>;
                Nullable<DateTime> endTime = element.GetValue(TimeSlotPanel.EndTimeProperty) as Nullable<DateTime>;

                double start_minutes = (startTime.Value.Hour * 60) + startTime.Value.Minute;
                double end_minutes = (endTime.Value.Hour * 60) + endTime.Value.Minute;
                double start_offset = (finalSize.Height / (24 * 60)) * start_minutes;
                double end_offset = (finalSize.Height / (24 * 60)) * end_minutes;

                top = start_offset + 1;
                width = this.GetWidth(finalSize, element as CalendarAppointmentItem);
                height = (end_offset - start_offset) - 2;
                left = (element as CalendarAppointmentItem).Indentation * width;

                element.Arrange(new Rect(left, top, width, height));
            }

            return finalSize;
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            // Calculate size based on duration?
            Size size = new Size(double.PositiveInfinity, double.PositiveInfinity);

            foreach (UIElement element in this.Children)
            {
                element.Measure(size);
            }

            return new Size();
        }

        private double GetWidth(Size finalSize, CalendarAppointmentItem appointementItem)
        {
            double count = Overlapping.ItemCount(appointementItem, this.Children);

            return (double)(finalSize.Width / count);
        }

        #endregion Methods
    }
}
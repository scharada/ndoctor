﻿namespace Smith.WPF.HtmlEditor
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Documents;
    using System.Windows.Input;
    using System.Windows.Media;

    /// <summary>
    /// ColorPicker.xaml 的交互逻辑
    /// </summary>
    public partial class ColorPicker : UserControl
    {
        #region Fields

        public static readonly DependencyProperty SelectedColorProperty = 
            DependencyProperty.Register("SelectedColor", typeof(Color), typeof(ColorPicker),
                new UIPropertyMetadata(Colors.Transparent, new PropertyChangedCallback(OnSelectedColorPropertyChanged)));

        bool IsRaiseColorChangedEvent = true;

        #endregion Fields

        #region Constructors

        public ColorPicker()
        {
            InitializeComponent();
            InitColors();
        }

        #endregion Constructors

        #region Events

        public event EventHandler<PropertyChangedEventArgs<Color>> SelectedColorChanged;

        #endregion Events

        #region Properties

        public Color SelectedColor
        {
            get { return (Color)GetValue(SelectedColorProperty); }
            set { SetValue(SelectedColorProperty, value); }
        }

        #endregion Properties

        #region Methods

        public void Reset()
        {
            IsRaiseColorChangedEvent = false;
            StandardColors.SelectedItem = null;
            SelectedColor = Color.FromRgb(0x01, 0x01, 0x01);
            IsRaiseColorChangedEvent = true;
        }

        static void OnSelectedColorPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ColorPicker control = sender as ColorPicker;
            if (control != null && control.SelectedColorChanged != null && control.IsRaiseColorChangedEvent)
                control.SelectedColorChanged(control, PropertyChangedEventArgs<Color>.Create((Color)e.NewValue, (Color)e.OldValue));
        }

        void HandleSelect(object sender, MouseButtonEventArgs e)
        {
            ListBoxItem item = sender as ListBoxItem;
            if (item != null) SelectedColor = (Color)item.Content;
        }

        void InitColors()
        {
            List<Color> ls = new List<Color>()
            {
                Color.FromRgb(0x00, 0x00, 0x00), Color.FromRgb(0x99, 0x33, 0x00),
                Color.FromRgb(0x33, 0x33, 0x00), Color.FromRgb(0x00, 0x33, 0x00),
                Color.FromRgb(0x00, 0x33, 0x66), Color.FromRgb(0x00, 0x00, 0x80),
                Color.FromRgb(0x33, 0x33, 0x99), Color.FromRgb(0x33, 0x33, 0x33),

                Color.FromRgb(0x80, 0x00, 0x00), Color.FromRgb(0xff, 0x66, 0x00),
                Color.FromRgb(0x80, 0x80, 0x00), Color.FromRgb(0x00, 0x80, 0x00),
                Color.FromRgb(0x00, 0x80, 0x80), Color.FromRgb(0x00, 0x00, 0xff),
                Color.FromRgb(0x66, 0x66, 0x99), Color.FromRgb(0x80, 0x80, 0x80),

                Color.FromRgb(0xff, 0x00, 0x00), Color.FromRgb(0xff, 0x99, 0x00),
                Color.FromRgb(0x99, 0xcc, 0x00), Color.FromRgb(0x33, 0x99, 0x66),
                Color.FromRgb(0x33, 0xcc, 0xcc), Color.FromRgb(0x33, 0x66, 0xff),
                Color.FromRgb(0x80, 0x00, 0x80), Color.FromRgb(0x99, 0x99, 0x99),

                Color.FromRgb(0xff, 0x00, 0xff), Color.FromRgb(0xff, 0xcc, 0x00),
                Color.FromRgb(0xff, 0xff, 0x00), Color.FromRgb(0x00, 0xff, 0x00),
                Color.FromRgb(0x00, 0xff, 0xff), Color.FromRgb(0x00, 0xcc, 0xff),
                Color.FromRgb(0x99, 0x33, 0x66), Color.FromRgb(0xc0, 0xc0, 0xc0),

                Color.FromRgb(0xff, 0x99, 0xcc), Color.FromRgb(0xff, 0xcc, 0x99),
                Color.FromRgb(0xff, 0xff, 0x99), Color.FromRgb(0x00, 0xff, 0x00),
                Color.FromRgb(0xcc, 0xff, 0xcc), Color.FromRgb(0x99, 0xcc, 0xff),
                Color.FromRgb(0xcc, 0x99, 0xff), Color.FromRgb(0xff, 0xff, 0xff)
            };
            StandardColors.ItemsSource = new ReadOnlyCollection<Color>(ls);
        }

        #endregion Methods
    }

    public class PropertyChangedEventArgs<T> : EventArgs
    {
        #region Constructors

        private PropertyChangedEventArgs()
        {
        }

        #endregion Constructors

        #region Properties

        public T NewValue
        {
            get; private set;
        }

        public T OldValue
        {
            get; private set;
        }

        #endregion Properties

        #region Methods

        public static PropertyChangedEventArgs<T> Create(T newValue, T oldValue)
        {
            return new PropertyChangedEventArgs<T>() { NewValue = newValue, OldValue = oldValue };
        }

        #endregion Methods
    }
}
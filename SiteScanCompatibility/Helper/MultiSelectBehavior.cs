using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace SiteScanCompatibility
{
    /// <summary>
    /// It's just a helper for MVVM thing so it can do multiselect behavior
    /// </summary>
    public class MultiSelectBehavior : Behavior<DataGrid>
    {
        public static readonly DependencyProperty SelectCommandProperty = DependencyProperty.Register(
            "SelectCommand", typeof(ICommand), typeof(MultiSelectBehavior), new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the select command.
        /// </summary>
        public ICommand SelectCommand
        {
            get { return (ICommand)GetValue(SelectCommandProperty); }
            set { SetValue(SelectCommandProperty, value); }
        }

        protected override void OnAttached()
        {
            AssociatedObject.SelectionChanged += AssociatedObject_SelectionChanged;
            base.OnAttached();
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.SelectionChanged -= AssociatedObject_SelectionChanged;
        }

        void AssociatedObject_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SelectCommand != null)
                SelectCommand.Execute(AssociatedObject.SelectedItems);
        }
    }
}

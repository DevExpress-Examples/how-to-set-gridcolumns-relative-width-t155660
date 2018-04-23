using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using DevExpress.Xpf.Grid;
using DevExpress.Mvvm.UI.Interactivity;
using DevExpress.Xpf.Core.Native;

namespace GridDynamicColumnsWIdth
{
    public class DynamicColumnsWidth : Behavior<GridControl>
    {
        #region Static
        public static DependencyProperty WidthMultiplierProperty =
            DependencyProperty.RegisterAttached(
            "WidthMultiplier",
            typeof(double), typeof(DynamicColumnsWidth),
            new PropertyMetadata(1.0,
                new PropertyChangedCallback(GridWidthMultiplier))); 
        

        private static DependencyProperty AutoCalcWidthProperty =
            DependencyProperty.RegisterAttached(
            "AutoCalcWidth",
            typeof(bool), typeof(DynamicColumnsWidth),
            new PropertyMetadata(true));

        private static void SetAutoCalcWidth(DependencyObject d, bool val)
        {
            d.SetValue(AutoCalcWidthProperty, val);
        }

        private static bool GetAutoCalcWidth(DependencyObject d)
        {
            return (bool)d.GetValue(AutoCalcWidthProperty);
        }


        public static double GetWidthMultiplier(DependencyObject d)
        {
            return (double)d.GetValue(WidthMultiplierProperty);
        }

        public static void SetWidthMultiplier(DependencyObject d, double value)
        {
            
            d.SetValue(WidthMultiplierProperty, value);
        }

        public static void GridWidthMultiplier(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            GridControl grid = (d as GridColumn).Parent as GridControl;
            if (grid!=null)
            CalcColumnWidth(grid);
        }

        private static void CalcColumnWidth(GridControl grid)
        {
            FrameworkElement mainElem = LayoutHelper.FindElementByName(grid, "PART_ScrollablePartPanel");
            FrameworkElement elem = LayoutHelper.FindElementByName(grid, "PART_VerticalScrollBar");
            if (mainElem == null || elem==null) return;
            double totalWidth = mainElem.ActualWidth-elem.ActualWidth-1;
            List<int> indexes = new List<int>();
            double colCount = 0;
            foreach (var column in grid.Columns)
            {
                if (!double.IsNaN(column.Width))
                    SetAutoCalcWidth(column, false);

                if (GetAutoCalcWidth(column))
                {
                    indexes.Add(grid.Columns.IndexOf(column));
                    colCount+=GetWidthMultiplier(column);
                }
                else
                    totalWidth -= column.ActualHeaderWidth;
            }
            double defaultlength = totalWidth / colCount;
            double corrIndex = 1;
            foreach (var i in indexes)
            {
                corrIndex = GetWidthMultiplier(grid.Columns[i]);
                grid.Columns[i].Width = defaultlength;
                if (corrIndex * defaultlength <= totalWidth)
                    grid.Columns[i].Width *= corrIndex;
                totalWidth -= grid.Columns[i].Width;
                colCount-=GetWidthMultiplier(grid.Columns[i]);
                defaultlength = totalWidth / colCount;
            }
            double k = 0;
            foreach (var el in grid.Columns)
            {
                k += el.ActualHeaderWidth;
            }
        }
        #endregion

        protected override void OnAttached()
        {
            AssociatedObject.Loaded += AssociatedObject_Loaded;
            
        }

        void AssociatedObject_Loaded(object sender, RoutedEventArgs e)
        {
            CalcColumnWidth(sender as GridControl);
        }
    }
}

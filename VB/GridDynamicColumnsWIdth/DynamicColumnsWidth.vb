Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Threading.Tasks
Imports System.Windows
Imports DevExpress.Xpf.Grid
Imports DevExpress.Mvvm.UI.Interactivity
Imports DevExpress.Xpf.Core.Native

Namespace GridDynamicColumnsWIdth
    Public Class DynamicColumnsWidth
        Inherits Behavior(Of GridControl)

        #Region "Static"
        Public Shared WidthMultiplierProperty As DependencyProperty = DependencyProperty.RegisterAttached("WidthMultiplier", GetType(Double), GetType(DynamicColumnsWidth), New PropertyMetadata(1.0, New PropertyChangedCallback(AddressOf GridWidthMultiplier)))


        Private Shared AutoCalcWidthProperty As DependencyProperty = DependencyProperty.RegisterAttached("AutoCalcWidth", GetType(Boolean), GetType(DynamicColumnsWidth), New PropertyMetadata(True))

        Private Shared Sub SetAutoCalcWidth(ByVal d As DependencyObject, ByVal val As Boolean)
            d.SetValue(AutoCalcWidthProperty, val)
        End Sub

        Private Shared Function GetAutoCalcWidth(ByVal d As DependencyObject) As Boolean
            Return DirectCast(d.GetValue(AutoCalcWidthProperty), Boolean)
        End Function


        Public Shared Function GetWidthMultiplier(ByVal d As DependencyObject) As Double
            Return DirectCast(d.GetValue(WidthMultiplierProperty), Double)
        End Function

        Public Shared Sub SetWidthMultiplier(ByVal d As DependencyObject, ByVal value As Double)

            d.SetValue(WidthMultiplierProperty, value)
        End Sub

        Public Shared Sub GridWidthMultiplier(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
            Dim grid As GridControl = TryCast((TryCast(d, GridColumn)).Parent, GridControl)
            If grid IsNot Nothing Then
            CalcColumnWidth(grid)
            End If
        End Sub

        Private Shared Sub CalcColumnWidth(ByVal grid As GridControl)
            Dim mainElem As FrameworkElement = LayoutHelper.FindElementByName(grid, "PART_ScrollablePartPanel")
            Dim elem As FrameworkElement = LayoutHelper.FindElementByName(grid, "PART_VerticalScrollBar")
            If mainElem Is Nothing OrElse elem Is Nothing Then
                Return
            End If
            Dim totalWidth As Double = mainElem.ActualWidth-elem.ActualWidth-1
            Dim indexes As New List(Of Integer)()
            Dim colCount As Double = 0
            For Each column In grid.Columns
                If Not Double.IsNaN(column.Width) Then
                    SetAutoCalcWidth(column, False)
                End If

                If GetAutoCalcWidth(column) Then
                    indexes.Add(grid.Columns.IndexOf(column))
                    colCount+= GetWidthMultiplier(column)
                Else
                    totalWidth -= column.ActualHeaderWidth
                End If
            Next column
            Dim defaultlength As Double = totalWidth / colCount
            Dim corrIndex As Double = 1
            For Each i In indexes
                corrIndex = GetWidthMultiplier(grid.Columns(i))
                grid.Columns(i).Width = defaultlength
                If corrIndex * defaultlength <= totalWidth Then
                    grid.Columns(i).Width *= corrIndex
                End If
                totalWidth -= grid.Columns(i).Width
                colCount-= GetWidthMultiplier(grid.Columns(i))
                defaultlength = totalWidth / colCount
            Next i
            Dim k As Double = 0
            For Each el In grid.Columns
                k += el.ActualHeaderWidth
            Next el
        End Sub
        #End Region

        Protected Overrides Sub OnAttached()
            AddHandler AssociatedObject.Loaded, AddressOf AssociatedObject_Loaded

        End Sub

        Private Sub AssociatedObject_Loaded(ByVal sender As Object, ByVal e As RoutedEventArgs)
            CalcColumnWidth(TryCast(sender, GridControl))
        End Sub
    End Class
End Namespace

<!-- default file list -->
*Files to look at*:

* [MainWindow.xaml](./CS/GridDynamicColumnsWIdth/MainWindow.xaml) (VB: [MainWindow.xaml](./VB/GridDynamicColumnsWIdth/MainWindow.xaml))
* [MainWindow.xaml.cs](./CS/GridDynamicColumnsWIdth/MainWindow.xaml.cs) (VB: [MainWindow.xaml.vb](./VB/GridDynamicColumnsWIdth/MainWindow.xaml.vb))
<!-- default file list end -->
# How to set GridColumn’s relative width


<p>In versions <strong>16.1</strong> and newer, you can resize columns proportionally by setting their <strong>Width</strong> property in the following manner:</p>


```xaml
<dxg:GridColumn Width="2*" />
```


<p>To obtain more details on how to provide this functionality in previous versions, select an appropriate version and refer to the <strong>Show Implementation Details</strong> section.</p>


<h3>Description</h3>

<p>While resizing columns proportionally is not supported out of the box, this functionality can be enabled using the <strong>BandedViewBehavior</strong> class from the <strong>DevExpress.Xpf.Grid.vXX.X.Extensions</strong> assembly. To learn more how to use it, refer to the <a data-ticket="K18529">Advanced Banded View Layout with attached behavior for versions prior to 13.1</a> KB article.</p>

<br/>



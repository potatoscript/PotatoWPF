---

## Building a CRUD WPF Application with Dynamic UserControls - PotatoWPF Tutorial

---

**[Introduction: What is WPF?]**

"Hey everyone, welcome to this WPF tutorial! If you're new to WPF, let me give you a quick introduction. WPF, or Windows Presentation Foundation, is a UI framework from Microsoft used to create desktop applications. What makes WPF powerful is its ability to separate the user interface from the business logic using XAML (Extensible Application Markup Language). This allows you to create beautiful, scalable interfaces with advanced controls.

In this series, we'll be building a WPF project step-by-step that will fetch data from a database and perform CRUD operations—Create, Read, Update, and Delete. We'll display this data dynamically based on menu clicks, allowing users to interact with different views seamlessly.

Let me show you the final product of what we'll be building."

---

**[Section 1: The Final Product Overview]**

"The final project we're aiming to build will include a main window with a simple menu for navigation. The menu will have items like 'Home,' 'Users,' 'Products,' and 'Help.'

When we click on one of these items, a corresponding `UserControl` will load. For example, when we click on 'Users,' we’ll display a list of users fetched from the database. You’ll also be able to add, edit, and delete users using CRUD operations. The same will happen for other entities like 'Products.'

By the end of this tutorial series, you'll have a working WPF app that can manage and display data dynamically using a combination of XAML, C#, and a database."

---

**[Section 2: Setting Up the WPF Project]**

"Let's get started by setting up our WPF project. We'll call this project 'PotatoWPF.' If you're following along, make sure to create a new WPF project in Visual Studio.

Once that's done, we’ll start by setting up a simple layout with a menu bar at the top and a grid in the middle that will act as our content area. Here’s what the XAML for the main window will look like."

**Code (MainWindow.xaml):**

```xml
<Window x:Class="PotatoWPF.MainWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        Title="MainWindow" Height="450" Width="800">
    <DockPanel>
        <!-- Menu at the top -->
        <Menu DockPanel.Dock="Top">
            <MenuItem Header="Home" />
            <MenuItem Header="Users">
                <MenuItem Header="View Users" Click="ViewUsersMenuItem_Click"/>
            </MenuItem>
            <MenuItem Header="Products">
                <MenuItem Header="View Products" Click="ViewProductsMenuItem_Click"/>
            </MenuItem>
            <MenuItem Header="Help" />
        </Menu>

        <!-- Main content grid where different views will load -->
        <Grid Name="MainContentGrid">
            <TextBlock Text="Welcome to PotatoWPF Project"
                       HorizontalAlignment="Center" 
                       VerticalAlignment="Center"
                       FontSize="24" />
        </Grid>
    </DockPanel>
</Window>
```

"This is a basic layout. We have a menu with options like 'Home,' 'Users,' and 'Products.' The main content will go into the `MainContentGrid`. When a menu item is clicked, we'll clear the grid and load the relevant `UserControl`."

---

**[Section 3: Creating Dynamic UserControls]**

"Now, let's create our `UserControls` that will be displayed when menu items are clicked. For example, we will create a `UsersControl` that will list all users fetched from the database.

First, create a `UserControl` in Visual Studio by right-clicking on the project and selecting 'Add > UserControl.' We'll call this one 'UsersControl.' This control will hold the list of users.

Here’s the XAML for the `UsersControl`."

**Code (UsersControl.xaml):**

```xml
<UserControl x:Class="PotatoWPF.UsersControl"
             xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
             xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
             Height="300" Width="400">
    <Grid>
        <TextBlock Text="List of Users"
                   FontSize="20"
                   HorizontalAlignment="Center"
                   VerticalAlignment="Top" />
        <!-- Placeholder for user list (later will be bound to database data) -->
    </Grid>
</UserControl>
```

"This is just a placeholder. Later on, we’ll populate it with user data fetched from the database."

---

**[Section 4: Loading UserControls on Menu Clicks]**

"Now, let's go back to `MainWindow.xaml.cs` and write the code to dynamically load our `UsersControl` when the 'View Users' menu item is clicked."

**Code (MainWindow.xaml.cs):**

```csharp
private void ViewUsersMenuItem_Click(object sender, RoutedEventArgs e)
{
    // Clear current content
    MainContentGrid.Children.Clear();

    // Create an instance of the UsersControl
    UsersControl usersControl = new UsersControl();

    // Set horizontal alignment to Left and vertical alignment to Stretch
    usersControl.HorizontalAlignment = HorizontalAlignment.Left;
    usersControl.VerticalAlignment = VerticalAlignment.Stretch;

    // Add the UsersControl to the MainContentGrid
    MainContentGrid.Children.Add(usersControl);
}
```

"This code clears the current content of the grid and loads the `UsersControl` whenever we click 'View Users' from the menu."

---

**[Section 5: Setting Up the Database and CRUD Operations]**

"Next, we need to set up a database to store our users and products. We’ll use SQL Server or SQLite for simplicity. 

In our project, we’ll add a database and a model to represent our data, such as a `User` class. We will also write CRUD operations for adding, reading, updating, and deleting records from the database.

For now, let's create a simple `User` class and set up Entity Framework to manage database connections and CRUD operations."

**Code (User.cs):**

```csharp
public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
}
```

"Once we've created our models, we'll set up the database context and write methods to perform the CRUD operations, which we'll use to populate our `UsersControl`."

---

**[Section 6: Displaying Data in the UserControl]**

"After setting up the CRUD operations, we’ll bind the data to the `UserControl`. This will allow us to display a list of users in the UI. 

We’ll use data binding in WPF to connect the user list to the control. Here’s a quick example of how the binding will work."

**Code (UsersControl.xaml with Data Binding):**

```xml
<ListBox ItemsSource="{Binding Users}">
    <ListBox.ItemTemplate>
        <DataTemplate>
            <StackPanel>
                <TextBlock Text="{Binding Name}" />
                <TextBlock Text="{Binding Email}" />
            </StackPanel>
        </DataTemplate>
    </ListBox.ItemTemplate>
</ListBox>
```

# Contents

- [Image Uri](#image_uri)
- [Popup Window](#popup_window)
- [Template](#template)
- [Image Template](#image_template)
- [ComboBox](#combobox)
- [DBHelper](#dbhelper)

---

## Image_Uri
[Back to Contents](#contents)
1. Set the Uri in the app.xaml
```xaml
<ResourceDictionary>
    <sys:Uri x:Key="BaseImageUri">pack://application:,,,/Resources/Images/</sys:Uri>
</ResourceDictionary>
```
2. Usage
```c#
Uri baseUri = (Uri)Application.Current.Resources["BaseImageUri"];
```


## Popup_Window
[Back to Contents](#contents)

### 1. Create a New Window for Input

First, you need to create a new window that will serve as the input dialog. You can do this by adding a new WPF Window to your project.

1. **Right-click on your project** in the Solution Explorer.
2. **Select** `Add > Window...` and name it `InputDialog.xaml`.

### 2. Define the InputDialog Layout

Open the `InputDialog.xaml` file and design the user interface for the input dialog. Here’s a simple example:

```xml
<Window x:Class="Fw.InputDialog"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        Title="Input" Height="200" Width="300">
    <StackPanel Margin="10">
        <TextBlock Text="Enter your input:" />
        <TextBox x:Name="InputTextBox" Margin="0,5,0,10" />
        <StackPanel Orientation="Horizontal" HorizontalAlignment="Right">
            <Button Content="OK" Width="75" Click="OKButton_Click" />
            <Button Content="Cancel" Width="75" Click="CancelButton_Click" />
        </StackPanel>
    </StackPanel>
</Window>
```

### 3. Implement the Logic in the InputDialog Code-Behind

Open `InputDialog.xaml.cs` and implement the logic to handle the button clicks:

```csharp
using System.Windows;

namespace Fw
{
    public partial class InputDialog : Window
    {
        public string InputValue { get; private set; }

        public InputDialog()
        {
            InitializeComponent();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            InputValue = InputTextBox.Text; // Get the user input
            DialogResult = true; // Set the result to true
            Close(); // Close the dialog
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false; // Set the result to false
            Close(); // Close the dialog
        }
    }
}
```

### 4. Show the Input Dialog from Your MainWindow

Now, you need to modify the command associated with the `設定 > env_setup` menu item in your `MainWindow.xaml.cs`. When the menu item is clicked, you will create an instance of the `InputDialog` and display it.

First, ensure you have a property or method in your ViewModel that will execute when the menu item is clicked. Here’s a simple example of how to implement this:

```csharp
using System.Windows;

namespace Fw
{
    public class MainWindowViewModel
    {
        public ICommand EnvSetupCommand { get; }

        public MainWindowViewModel()
        {
            EnvSetupCommand = new RelayCommand(ShowEnvSetupDialog); // Use a RelayCommand
        }

        private void ShowEnvSetupDialog()
        {
            InputDialog inputDialog = new InputDialog();
            if (inputDialog.ShowDialog() == true) // Show dialog and check result
            {
                string userInput = inputDialog.InputValue;
                // Handle the input as needed (e.g., save it, use it in your application)
                MessageBox.Show($"You entered: {userInput}"); // For demonstration
            }
        }
    }
}
```

### 5. Bind the Command to the Menu Item

Finally, make sure the command you defined is bound to the `MenuItem` in `MainWindow.xaml`:

```xml
<MenuItem Header="{x:Static properties:Resources.env_setup}" Command="{Binding EnvSetupCommand}"/>
```

## Implementation for DynamicResource with Language Switching:

### 1. Modify `App.xaml` 

```xml
<Application x:Class="Fw.App"
             xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
             xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
             StartupUri="MainWindow.xaml">
    <Application.Resources>
        <ResourceDictionary>
            <ResourceDictionary.MergedDictionaries>
                <!-- Initially load the default language (e.g., English) -->
                <ResourceDictionary Source="Resources/Strings.xaml" />
            </ResourceDictionary.MergedDictionaries>
        </ResourceDictionary>
    </Application.Resources>
</Application>
```

### 2. Use `DynamicResource` in XAML Views

Now update your XAML views to use `DynamicResource` instead of `StaticResource`. For example:

```xml
<MenuItem Header="{DynamicResource str_kankyosette}" Command="{Binding EnvSetupCommand}" />
```

This way, when the `ResourceDictionary` is updated at runtime, the UI will automatically reflect the changes.

### 3. Create Multiple Resource Files

Create two separate `ResourceDictionary` files for each language, such as `Strings.xaml` for English and `Strings.ja-JP.xaml` for Japanese.

#### `Strings.xaml` (English)

```xml
<ResourceDictionary xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
                    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
                    xmlns:sys="clr-namespace:System;assembly=mscorlib">
    <sys:String x:Key="str_kankyosette">Environment Setup</sys:String>
</ResourceDictionary>
```

#### `Strings.ja-JP.xaml` (Japanese)

```xml
<ResourceDictionary xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
                    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
                    xmlns:sys="clr-namespace:System;assembly=mscorlib">
    <sys:String x:Key="str_kankyosette">環境設定</sys:String>
</ResourceDictionary>
```

### 4. Change Resource at Runtime

In your code-behind (e.g., `MainWindow.xaml.cs`), add a method to change the `ResourceDictionary` based on the user's language selection:

```csharp
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    // Method to change language
    private void ChangeLanguage(string language)
    {
        // Clear existing resource dictionaries
        Application.Current.Resources.MergedDictionaries.Clear();

        // Load the appropriate language resource
        var dictionary = new ResourceDictionary();
        switch (language)
        {
            case "ja-JP":
                dictionary.Source = new Uri("Resources/Strings.ja-JP.xaml", UriKind.Relative);
                break;
            default:
                dictionary.Source = new Uri("Resources/Strings.xaml", UriKind.Relative);
                break;
        }

        // Add the selected resource dictionary
        Application.Current.Resources.MergedDictionaries.Add(dictionary);
    }
}
```

### 5. Create a Language Selection Mechanism

You can create a dropdown, radio buttons, or a simple menu option to allow users to select the language. For example, a `ComboBox` in XAML could look like this:

```xml
<ComboBox SelectionChanged="LanguageSelectionChanged">
    <ComboBoxItem Content="English" Tag="en-US" />
    <ComboBoxItem Content="日本語" Tag="ja-JP" />
</ComboBox>
```

And in the code-behind, handle the selection:

```csharp
private void LanguageSelectionChanged(object sender, SelectionChangedEventArgs e)
{
    var comboBox = sender as ComboBox;
    var selectedLanguage = (comboBox.SelectedItem as ComboBoxItem)?.Tag.ToString();
    
    // Change the language based on user selection
    ChangeLanguage(selectedLanguage);
}
```

## MultiBinding Approach

   ```xml
   <TreeViewItem Foreground="White" Margin="0,0,0,5">
       <TreeViewItem.Header>
           <TextBlock>
               <TextBlock.Text>
                   <MultiBinding StringFormat="{}{0}{1}" FallbackValue="Fallback Text">
                       <Binding Source="{StaticResource str_kaidan}" />
                       <Binding Source="{StaticResource str_kaidan}" />
                   </MultiBinding>
               </TextBlock.Text>
           </TextBlock>
       </TreeViewItem.Header>
   </TreeViewItem>
   ```

## 
<DataGrid x:Name="KisoDataGrid" Grid.Row="2" Grid.ColumnSpan="2" AutoGenerateColumns="False" CanUserAddRows="False">
    <DataGrid.Columns>
        <DataGridTextColumn  Header="呼称" Binding="{Binding Name}" />
        <DataGridComboBoxColumn Header="積算要否" SelectedItemBinding="{Binding SekisanYouhi}" ItemsSource="{Binding SekisanYouhiOptions}" />
    </DataGrid.Columns>
</DataGrid>


If the `Data/Kutai.json` file is not being copied to the `bin/Debug` directory when you build your WPF application, 
you need to set the properties of the JSON file in Visual Studio to ensure it gets copied during the build process. 

### Step 1: Add the JSON File to Your Project

1. **Ensure the File is in Your Project:**
   - If you haven't already, make sure you add the `Kutai.json` file to your project. You can do this by right-clicking on your project in the Solution Explorer, selecting **Add**, and then **Existing Item...**. Browse to your `Data` directory and select `Kutai.json`.

2. **Create the Data Folder:**
   - If the `Data` folder doesn’t exist in your project structure, you can create it by right-clicking on your project, selecting **Add**, then **New Folder**, and naming it `Data`. Then, place your `Kutai.json` file inside this folder.

### Step 2: Set File Properties

1. **Select the JSON File:**
   - In the Solution Explorer, right-click on the `Kutai.json` file and select **Properties**.

2. **Modify the Properties:**
   - In the Properties window, set the following:
     - **Build Action:** Set this to `Content`. This ensures that the file is included in the build output.
     - **Copy to Output Directory:** Set this to `Copy if newer` or `Copy always`. This determines whether the file will be copied to the `bin/Debug` folder during the build process.

### Step 3: Verify the Output Directory

- After making these changes, build your project again (Build > Build Solution or press `Ctrl + Shift + B`). Check the `bin/Debug/Data` directory to ensure that `Kutai.json` is now present.

### Accessing the JSON File

Now that the file is properly set up to be copied to the output directory, your existing code for loading the JSON file using the relative path should work correctly:

```csharp
string jsonFilePath = Path.Combine(baseDirectory, "Data", "Kutai.json");
```

## bind data to combobox
<ComboBox SelectedItem="{Binding SekisanYouhi}"
          ItemsSource="{Binding Path=DataContext.SekisanYouhiOptions, RelativeSource={RelativeSource AncestorType=UserControl}}"
          IsEditable="False" />
		  
public DialogEnvSetup_Kutai()
{
    InitializeComponent();
    DataContext = this; // Set DataContext for the UserControl
    SekisanYouhiOptions = new List<string> { "必要", "不要" }; // Initialize options
    DataList = new List<Model>(); // Initialize DataList as an empty list
    KisoDataGrid.ItemsSource = DataList; // Bind the DataGrid to DataList
}

## Template
[Back to Contents](#contents)

- Overview of Templates in WPF
- Types of Templates: ControlTemplate, DataTemplate, and ItemTemplate
- Creating and applying Templates in WPF
- Template Binding and Triggers
- Example of creating a ControlTemplate for a custom control
- Basic setup xaml
  ```xaml
  <UserControl x:Class="Fw.StaticMenu.DynamicMenuItem.Views.DialogEnvSetupTeikeinobeniya"
             xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
             xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
             xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006" 
             xmlns:d="http://schemas.microsoft.com/expression/blend/2008" 
             mc:Ignorable="d" 
             xmlns:local="clr-namespace:Fw.Templates"
             d:DesignHeight="450" d:DesignWidth="800">

    <Grid>
        <local:TableTemplate x:Name="TableControl" MaxHeightOffset="200"/>
    </Grid>
</UserControl>
  ```
- Basic setup xaml.cs
```c#
using Fw.Data;
using Fw.StaticMenu.DynamicMenuItem.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace Fw.StaticMenu.DynamicMenuItem.Views
{
    public partial class DialogEnvSetupTeikeinobeniya : UserControl
    {
        private MessageBoxHelper _messageBoxHelper;
        private readonly Dictionary<string, string> localizedStrings;
        private readonly DialogEnvSetupKutaiViewModel viewModel;

        private Dictionary<string, string> LoadLocalizedStrings()
        {
            return new Dictionary<string, string>
            {
                { "str_paneru_teikeinobeniya", (string)Application.Current.Resources["str_paneru_teikeinobeniya"] },
                { "str_koshou", (string)Application.Current.Resources["str_koshou"] },
                { "str_gouhanhaba", (string)Application.Current.Resources["str_gouhanhaba"] },
                { "str_gouhantakasa", (string)Application.Current.Resources["str_gouhantakasa"] },
                { "str_sangihaichi", (string)Application.Current.Resources["str_sangihaichi"] },
                { "str_sangihaba", (string)Application.Current.Resources["str_sangihaba"] },
                { "str_sangitakasa", (string)Application.Current.Resources["str_sangitakasa"] },
                { "str_chukansangihonsu", (string)Application.Current.Resources["str_chukansangihonsu"] },
                { "str_chukansangisukima1", (string)Application.Current.Resources["str_chukansangisukima1"] },
                { "str_chukansangisukima2", (string)Application.Current.Resources["str_chukansangisukima2"] }
            };
        }
        public DialogEnvSetupTeikeinobeniya()
        {
            InitializeComponent();
            _messageBoxHelper = new MessageBoxHelper();
            localizedStrings = LoadLocalizedStrings();
            TableControl.TitleTextBlock.Content = localizedStrings["str_paneru_teikeinobeniya"];
            viewModel = new DialogEnvSetupKutaiViewModel();
            DataContext = viewModel;
            SetColumns();
            SubscribeToEvents();
            RefreshDataGrid();
        }
        private void RefreshDataGrid()
        {
            //viewModel.LoadAllData();
            TableControl.DataList = viewModel.DataList;
            TableControl.DataGrid.ItemsSource = viewModel.DataList;
            TableControl.editedRowIds.Clear();
            TableControl.deletedRowIds.Clear();
            TableControl.addedRowIds.Clear();
        }
        private void SetColumns()
        {
            var centeredTextStyle = new Style(typeof(TextBlock));
            centeredTextStyle.Setters.Add(new Setter(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Center));
            centeredTextStyle.Setters.Add(new Setter(TextBlock.HorizontalAlignmentProperty, HorizontalAlignment.Center));

            var centeredEditingTextStyle = new Style(typeof(TextBox));
            centeredEditingTextStyle.Setters.Add(new Setter(TextBox.VerticalContentAlignmentProperty, VerticalAlignment.Center));
            centeredEditingTextStyle.Setters.Add(new Setter(TextBox.HorizontalContentAlignmentProperty, HorizontalAlignment.Center));

            var columns = new List<DataGridColumn>
            {
                new DataGridTextColumn
                {
                    Header = "ID",
                    Binding = new System.Windows.Data.Binding("ID"),
                    Visibility = Visibility.Collapsed
                },
                new DataGridTextColumn
                {
                    Header = localizedStrings["str_koshou"],
                    Binding = new System.Windows.Data.Binding("Title"),
                    Width = new DataGridLength(1, DataGridLengthUnitType.Star),
                    ElementStyle = centeredTextStyle,
                    EditingElementStyle = centeredEditingTextStyle
                }

            };
            TableControl.SetColumns(columns);
        }
        private void SubscribeToEvents()
        {
            TableControl.ApplyClicked += TableControl_ApplyClicked;
            TableControl.CancelClicked += TableControl_CancelClicked;
            TableControl.AddNewClicked += TableControl_AddNewClicked;
            TableControl.EditDataGridCell += TableControl_EditDataGridCell;
        }
        private void TableControl_EditDataGridCell(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.Row.Item is DialogEnvSetupModel editedItem && !TableControl.editedRowIds.Contains(editedItem.ID.ToString()))
            {
                TableControl.editedRowIds.Add(editedItem.ID.ToString());  // Store the ID of the edited row
            }
        }
        private void TableControl_ApplyClicked(object sender, RoutedEventArgs e)
        {
            if (TableControl.editedRowIds.Count > 0 || TableControl.deletedRowIds.Count > 0 || TableControl.addedRowIds.Count > 0)
            {
                // Apply changes to the SQLite database
                viewModel.ApplyChanges(TableControl.editedRowIds, TableControl.deletedRowIds, TableControl.addedRowIds);

                RefreshDataGrid();

                _messageBoxHelper.ShowSuccessUpdated();
            }
            else
            {
                _messageBoxHelper.ShowNoRowToChange();
            }
        }
        private void TableControl_CancelClicked(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = _messageBoxHelper.ShowOnCancelClicked();
            if (result == MessageBoxResult.Yes)
            {
                RefreshDataGrid();
            }
        }
        private void TableControl_AddNewClicked(object sender, RoutedEventArgs e)
        {
            var newRow = new DialogEnvSetupModel
            {
                Title = "",
                Deleteable = 1
            };
            TableControl.addedRowIds.Add(newRow);
            viewModel.DataList.Add(newRow);
        }

    }
}

```

## Image_Template
[Back to Contents](#contents)
- Usage:
- xaml setting
 ```xaml
 <Grid x:Name="MainGrid" >
    <local:TableTemplate x:Name="TableControl" MaxHeightOffset="465"  Grid.Row="0"/>
</Grid>
 ```
- xaml.cs setting
 ```c#
 private Dictionary<string, DateTime> lastEditTimes = new Dictionary<string, DateTime>();

 Uri baseUri = (Uri)Application.Current.Resources["BaseImageUri"];
 
 TextBox Dim1TextBox;
 TextBox Dim2TextBox;
 TextBox Dim3TextBox;
 
 SetImageTextBox();
 private void SetImageTextBox()
 {
    Canvas canvas = new Canvas();

    Dim1TextBox = CreateTextBox("Dim1", 330, 238); 
    Dim2TextBox = CreateTextBox("Dim2", 155, 138);  
    Dim3TextBox = CreateTextBox("Dim3", 310, 28);  

    canvas.Children.Add(Dim1TextBox);
    canvas.Children.Add(Dim2TextBox);
    canvas.Children.Add(Dim3TextBox);

    TableControl.CanvasContents = canvas;
 }
 // TextBox Helper
 private TextBox CreateTextBox(string bindingProperty, double left, double top)
 {
    var textBox = new TextBox
    {
        Width = 80,
        Height = 25
    };

    // Bind the TextBox Text to the specified property (Dim1, Dim2, Dim3)
    textBox.SetBinding(TextBox.TextProperty, new Binding(bindingProperty)
    {
        UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
        Mode = BindingMode.TwoWay
    });

    // Set position of TextBox within the StackPanel
    Canvas.SetLeft(textBox, left);
    Canvas.SetTop(textBox, top);

    return textBox;
 }


 private void SubscribeToEvents()
 {
    TableControl.EditDataGridCell += TableControl_EditDataGridCell;
    TableControl.DataGridMouseLeftButtonUp += TableControl_DataGridMouseLeftButtonUp;
 }

 private void TableControl_EditDataGridCell(object sender, DataGridCellEditEndingEventArgs e)
 {
    if (e.Row.Item is DialogEnvSetupModel editedItem)
    {
        string itemId = editedItem.Id.ToString();
        DateTime currentTime = DateTime.Now;

        // Check if item has been edited recently
        if (!lastEditTimes.ContainsKey(itemId) || (currentTime - lastEditTimes[itemId]).TotalMilliseconds > 100)
        {
            // Update the last edit time for this item
            lastEditTimes[itemId] = currentTime;

            // Add the Id to editedRowIds if not already present
            if (!TableControl.editedRowIds.Contains(itemId))
            {
                TableControl.editedRowIds.Add(itemId);
            }

            // Force commit of the cell edit to ensure updated value is available to make sure the textBox get updated as well
            TableControl.DataGrid.CommitEdit(DataGridEditingUnit.Row, true);

        }
    }
 }
 
 // to let the image items get display the selected row's information
 private string type;
 private void TableControl_DataGridMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
 {
    if (TableControl.DataGrid.SelectedItem is DialogEnvSetupModel selectedItem)
    {
        type = selectedItem.Type;

        //ImageView.UpdateBeam(selectedItem);
        TableControl.ImageItems.Visibility = Visibility.Visible;
        TableControl.ImageTitle.Content = selectedItem.Title.ToString();

        // to let the textbox bind to the result in selectedItem
        Dim1TextBox.DataContext = selectedItem;
        Dim2TextBox.DataContext = selectedItem;
        Dim3TextBox.DataContext = selectedItem;


        TableControl.editedRowIds.Add(selectedItem.Id.ToString());

        TableControl.Image.Source = new BitmapImage(new Uri(baseUri, "tepa.png"));

    }
 }
 
 ```

## ComboBox
[Back to Contents](#contents)

- Basics of ComboBox in WPF
- Binding data to a ComboBox
- Customizing ComboBox items using ItemTemplate
- Handling ComboBox selection events
- Example of ComboBox with a dynamic list of items

## DBHelper
[Back to Contents](#contents)

- Purpose of a Database Helper class
- Steps to create a DBHelper class in WPF
- Common functions in DBHelper (Connect, Read, Write, Update, Delete)
- Using DBHelper with different databases (SQLite, PostgreSQL, etc.)
- Example of using DBHelper for CRUD operations
- Code:
The code defines a `DBHelper` class that handles SQLite database interactions within a WPF application. Below is a breakdown of each section of the code, explaining its purpose and how it works.

### 1. **Namespaces and Class Definition:**

```csharp
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
```

The `DBHelper` class uses:
- `System.Data.SQLite` for SQLite database interaction.
- `System.IO` for file and directory management (e.g., checking for file existence).
- `System.Windows` to access application resources (e.g., paths, URIs).
- `System.Collections.Generic` for working with collections like `Dictionary` and `List`.

```csharp
namespace Fw.Data
{
    public class DBHelper
    {
        private readonly string _connectionString;
        public string DbFilePath { get; }
        public SQLiteConnection Connection { get; private set; }
        private Dictionary<string, string> resources;
        Uri BaseImageUri { get; }
```

- **Class Declaration**: The `DBHelper` class is encapsulated in the `Fw.Data` namespace and provides methods to interact with a SQLite database. 
- **Private Fields**: 
  - `_connectionString`: The connection string used to connect to the SQLite database.
  - `DbFilePath`: The full path to the SQLite database file.
  - `Connection`: A property that holds the active SQLite connection.
  - `resources`: A dictionary to store resource strings (loaded from the application’s resources).
  - `BaseImageUri`: Holds the URI for images stored in the resources.

### 2. **Constructor (`DBHelper`)**:

```csharp
public DBHelper()
{
    string dbDirectory = Application.Current.Resources["dbDirectory"] as string;
    string dbName = Application.Current.Resources["dbName"] as string;
    DbFilePath = Path.Combine(dbDirectory, dbName);
    BaseImageUri = (Uri)Application.Current.Resources["BaseImageUri"];
    _connectionString = $"Data Source={DbFilePath};Version=3;";

    InitializeResources();
    InitializeDatabase();
}
```

- **Constructor**: It initializes the `DBHelper` class, reads database path and base image URI from the application resources, constructs the connection string, and calls methods to initialize resources and database.
  - **DbFilePath**: Combines `dbDirectory` and `dbName` (from the application's resources) to determine the full database file path.
  - **BaseImageUri**: Loads the base URI used for images.
  - **_connectionString**: Constructs the SQLite connection string using the `DbFilePath`.

### 3. **Resource Initialization (`InitializeResources`)**:

```csharp
private void InitializeResources()
{
    resources = new Dictionary<string, string>
    {
        { "strKiso", Application.Current.Resources["str_kiso"] as string },
        { "strHitsuyou", Application.Current.Resources["str_hitsuyou"] as string },
        ...
        { "strKukei", Application.Current.Resources["str_kukei"] as string }
    };
}
```

This method loads strings from the application's resources into the `resources` dictionary. These resource strings are used throughout the application, particularly when inserting data into the database.

### 4. **Database Initialization (`InitializeDatabase`)**:

```csharp
private void InitializeDatabase()
{
    if (!File.Exists(DbFilePath))
    {
        SQLiteConnection.CreateFile(DbFilePath);
    }

    using (Connection = new SQLiteConnection(_connectionString))
    {
        Connection.Open();
        CreateTables(Connection);
        CreateData();
    }
}
```

- **Checking Database Existence**: If the SQLite database file does not exist, it creates the file using `SQLiteConnection.CreateFile(DbFilePath)`.
- **Connecting to SQLite**: Creates a connection to the SQLite database using the connection string, then opens the connection.
- **CreateTables**: Calls the `CreateTables` method to create necessary tables in the database.
- **CreateData**: Calls the `CreateData` method to insert initial data into the tables.

### 5. **Creating Tables (`CreateTables`)**:

```csharp
private void CreateTables(SQLiteConnection connection)
{
    var createTableCommands = new List<string>
    {
        @"
        CREATE TABLE IF NOT EXISTS KutaiDBTable (
            ID INTEGER PRIMARY KEY AUTOINCREMENT,
            Type TEXT,
            Title TEXT,
            SekisanYouhi TEXT,
            Deleteable INTEGER
        );",
        ...
    };

    foreach (var command in createTableCommands)
    {
        ExecuteNonQuery(connection, command);
    }
}
```

- **SQL Commands**: This method creates a list of SQL commands that define the structure of the tables (e.g., `KutaiDBTable`, `KihonbuzaiDBTable`, etc.). Each table has columns with different data types such as `INTEGER`, `TEXT`, and `DOUBLE`.
- **Executing Commands**: The commands are executed via the `ExecuteNonQuery` method to ensure the tables are created if they do not already exist.

### 6. **Executing SQL (`ExecuteNonQuery`)**:

```csharp
private void ExecuteNonQuery(SQLiteConnection connection, string query)
{
    using (var command = new SQLiteCommand(query, connection))
    {
        command.ExecuteNonQuery();
    }
}
```

This method executes SQL commands that do not return data (e.g., `CREATE TABLE`, `INSERT`, `UPDATE`, and `DELETE`). It uses a `SQLiteCommand` to execute the query.

### 7. **Inserting Initial Data (`CreateData`)**:

```csharp
private void CreateData()
{
    InitializeResources();

    using (var connection = new SQLiteConnection(_connectionString))
    {
        connection.Open();

        InsertInitialDataIfEmpty(connection, "KutaiDBTable", GetKutaiData());
        InsertInitialDataIfEmpty(connection, "KihonbuzaiDBTable", GetKihonbuzaiData());
        ...
    }
}
```

- **Initializing Resources**: Calls the `InitializeResources` method to ensure that the resource strings are loaded.
- **Inserting Data**: It checks if the tables are empty and, if they are, it inserts initial data using the `InsertInitialDataIfEmpty` method. The data is obtained by methods like `GetKutaiData`, `GetKihonbuzaiData`, etc.

### 8. **Inserting Data (`InsertInitialDataIfEmpty`)**:

```csharp
private void InsertInitialDataIfEmpty(SQLiteConnection connection, string tableName, IEnumerable<IDictionary<string, object>> data)
{
    if (IsTableEmpty(connection, tableName))
    {
        InsertData(tableName, data);
    }
}
```

This method checks whether a table is empty by calling `IsTableEmpty`, and if the table is empty, it calls the `InsertData` method to insert the initial data.

### 9. **Checking if Table is Empty (`IsTableEmpty`)**:

```csharp
private bool IsTableEmpty(SQLiteConnection connection, string tableName)
{
    using (var command = new SQLiteCommand($"SELECT COUNT(*) FROM {tableName};", connection))
    {
        long count = (long)command.ExecuteScalar();
        return count == 0;
    }
}
```

- **Table Check**: Executes a query to count the number of records in a given table. If the count is 0, it returns `true`, indicating that the table is empty.

### 10. **Inserting Data into Table (`InsertData`)**:

```csharp
public void InsertData(string tableName, IEnumerable<IDictionary<string, object>> data)
{
    using (var connection = new SQLiteConnection(_connectionString))
    {
        connection.Open();
        if (!data.Any()) return;

        var keys = string.Join(", ", data.First().Keys);
        var parameters = string.Join(", ", data.First().Keys.Select(k => $"@{k}"));
        string insertQuery = $"INSERT INTO {tableName} ({keys}) VALUES ({parameters});";

        using (var insertCommand = new SQLiteCommand(insertQuery, connection))
        {
            foreach (var item in data)
            {
                insertCommand.Parameters.Clear();
                foreach (var kvp in item)
                {
                    insertCommand.Parameters.AddWithValue($"@{kvp.Key}", kvp.Value);
                }
                insertCommand.ExecuteNonQuery();
            }
        }
    }
}
```

- **Inserting Data**: Constructs an `INSERT` query dynamically based on the keys of the provided data. For each item in the `data` collection, it adds the parameters to the query and executes it.

### 11. **Reading Data (`ReadData`)**:

```csharp
public IEnumerable<Dictionary<string, object>> ReadData(string tableName)
{
    var result = new List<Dictionary<string, object>>();
    using (var connection = new SQLiteConnection(_connectionString))
    {
        connection.Open();
        string selectQuery = $"SELECT * FROM {tableName};";
        using (var selectCommand = new SQLiteCommand(selectQuery, connection))
        using (var reader = selectCommand.ExecuteReader())
        {
            while (reader.Read())
            {
                var row = new Dictionary<string, object>();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    row[reader.GetName(i)] = reader.GetValue(i);
                }
                result.Add(row);
            }
        }
    }
    return result;
}
```

- **Reading Data**: Executes a `SELECT` query to retrieve all data from the specified table. It reads each row and stores the data in a dictionary, returning the result as a collection of dictionaries.



### Conclusion:
This `DBHelper` class handles creating a SQLite database, defining tables, inserting initial data, and retrieving data. The methods are designed to ensure that the database is correctly initialized and that the necessary data is always available for use within the application.
- Usage:
To use the `DatabaseHelper` class in the `ReadData`, `InsertData`, `UpdateData`, and `DeleteData` methods, you'll first need to refactor these methods to call the helper class methods instead of directly using SQLite commands. Here’s how you can modify each of these methods to utilize `DatabaseHelper`:

1. **Initialize DatabaseHelper**  
   First, create an instance of `DatabaseHelper` with the connection string.

   ```csharp
   private DatabaseHelper _databaseHelper;

   public UserControlConstructor()
   {
       string connectionString = $"Data Source={DbFilePath};Version=3;";
       _databaseHelper = new DatabaseHelper(connectionString);
   }
   ```

2. **Refactor `LoadAllData`**  
   Modify `LoadAllData` to use the `ReadData` method from `DatabaseHelper`.

   ```csharp
   public void ReadData()
   {
       var data = _databaseHelper.ReadData("KoukanDBTable");

       foreach (var row in data)
       {
           var item = new DialogEnvSetupModel
           {
               ID = Convert.ToInt32(row["ID"]),
               Title = row["Title"].ToString(),
               Type = row["Type"].ToString(),
               Dim1 = Convert.ToDouble(row["Dim1"]),
               Dim2 = Convert.ToDouble(row["Dim2"]),
               Length = Convert.ToDouble(row["Length"]),
               ImageSource = row["ImageSource"].ToString(),
               Deleteable = Convert.ToInt32(row["Deleteable"])
           };
           DataList.Add(item);
       }
   }
   ```

3. **Refactor `InsertKoukanData`**  
   Modify `InsertKoukanData` to use the `InsertData` method from `DatabaseHelper`.

   ```csharp
   private void InsertData(string title, string type, double dim1, double dim2, double length)
   {
       var data = new List<Dictionary<string, object>>
       {
           new Dictionary<string, object>
           {
               { "Title", title },
               { "Type", type },
               { "Dim1", dim1 },
               { "Dim2", dim2 },
               { "Length", length },
               { "ImageSource", type },
               { "Deleteable", 1 }
           }
       };
       _databaseHelper.InsertData("KoukanDBTable", data);
   }
   ```

4. **Refactor `UpdateKoukanData`**  
   Modify `UpdateKoukanData` to use the `UpdateData` method from `DatabaseHelper`.

   ```csharp
   public void UpdateData(DialogEnvSetupModel target)
   {
       var updatedValues = new Dictionary<string, object>
       {
           { "Title", target.Title },
           { "Type", target.Type },
           { "Dim1", target.Dim1 },
           { "Dim2", target.Dim2 },
           { "Length", target.Length }
       };
       var conditions = new Dictionary<string, object> { { "ID", target.ID } };

       _databaseHelper.UpdateData("KoukanDBTable", updatedValues, conditions);
   }
   ```

5. **Refactor `DeleteKoukanData`**  
   Modify `DeletenData` to use the `DeleteData` method from `DatabaseHelper`.

   ```csharp
   private void DeleteKoukanData(string id)
   {
       var conditions = new Dictionary<string, object> { { "ID", id } };
       _databaseHelper.DeleteData("KoukanDBTable", conditions);
   }
   ```

Each method now leverages `DatabaseHelper`, making your code more modular and reusable. This setup also simplifies managing SQL commands, as `DatabaseHelper` encapsulates these details, allowing you to keep the CRUD operations consistent across different parts of your application.



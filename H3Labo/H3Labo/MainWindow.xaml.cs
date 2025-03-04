using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace H3Labo;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private List<string> _firstNames = new List<string>();
    private List<string> _lastNames = new List<string>();
    private string[] _fullNames = new string[100];

    private const string _fileName = @"subdir\personen.txt";

    FileStream fsw = null;
    StreamWriter sw = null;

    private void addButton_Click(object sender, RoutedEventArgs e)
    {
        if (!string.IsNullOrWhiteSpace(firstNameTextBox.Text) && !string.IsNullOrWhiteSpace(lastNameTextBox.Text))
        {
            _firstNames.Add(firstNameTextBox.Text);
            _lastNames.Add(lastNameTextBox.Text);

            firstNameTextBox.Clear();
            lastNameTextBox.Clear();

            firstNameListBox.Items.Clear();
            lastNameListBox.Items.Clear();

            foreach (string firstName in _firstNames)
            {
                ListBoxItem item = new ListBoxItem();
                item.Content = firstName;
                firstNameListBox.Items.Add(item);
            }

            foreach (string lastName in _lastNames)
            {
                lastNameListBox.Items.Add(lastName);
            }
        }
        else if (string.IsNullOrWhiteSpace(firstNameTextBox.Text) && string.IsNullOrWhiteSpace(lastNameTextBox.Text))
        {
            MessageBox.Show("Please enter a first name and a last name.");
            firstNameTextBox.Focus();
        }
        else if (string.IsNullOrWhiteSpace(firstNameTextBox.Text) && !string.IsNullOrWhiteSpace(lastNameTextBox.Text))
        {
            MessageBox.Show("Please enter a first name.");
            firstNameTextBox.Focus();
        }
        else if (!string.IsNullOrWhiteSpace(firstNameTextBox.Text) && string.IsNullOrWhiteSpace(lastNameTextBox.Text))
        {
            MessageBox.Show("Please enter a last name.");
            lastNameTextBox.Focus();
        }
    }

    private void saveFileButton_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            if (!Directory.Exists("subdir"))
            {
                Directory.CreateDirectory("subdir");
            }

            if (!File.Exists(_fileName))
            {
                fsw = new FileStream(_fileName, FileMode.Append, FileAccess.Write);
            }
            else
            {
                fsw = new FileStream(_fileName, FileMode.OpenOrCreate, FileAccess.Write);
            }

            sw = new StreamWriter(fsw);

            for (int i = 0; i < _firstNames.Count; i++)
            {
                sw.WriteLine($"{_firstNames[i]};{_lastNames[i]}");
            }
        }
        catch (FileNotFoundException ex)
        {
            MessageBox.Show(ex.Message, "Foutmelding", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Foutmelding", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
        finally
        {
            if (sw != null)
            {
                sw.Close();
            }

            if (fsw != null)
            {
                fsw.Close();
            }
        }
    }

    private void readFileButton_Click(object sender, RoutedEventArgs e)
    {
        Array.Clear(_fullNames, 0, _fullNames.Length);
        _firstNames.Clear();
        _lastNames.Clear();
        firstNameListBox.Items.Clear();
        lastNameListBox.Items.Clear(); 

        using (FileStream fsr = new FileStream(_fileName, FileMode.Open, FileAccess.Read))
        using (StreamReader sr = new StreamReader(fsr))
        {
            while (!sr.EndOfStream)
            {
                _fullNames = sr.ReadLine().Split(';');

                for (int i = 0; i < _fullNames.Length; i++)
                {
                    if (i % 2 == 0)
                    {
                        _firstNames.Add(_fullNames[i]);
                    }
                    else
                    {
                        _lastNames.Add(_fullNames[i]);
                    }
                }
            }
        }

        foreach (string firstname in _firstNames)
        {
            firstNameListBox.Items.Add(firstname);
        }

        foreach (string lastname in _lastNames)
        {
            lastNameListBox.Items.Add(lastname);
        }
    }
}
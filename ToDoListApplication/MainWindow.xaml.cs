using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ToDoListApplication
{
    public partial class MainWindow : Window
    {
        public ObservableCollection<TaskItem> Tasks { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            Tasks = new ObservableCollection<TaskItem>();
            TaskListBox.ItemsSource = Tasks;
        }

        private void AddTask_Click(object sender, RoutedEventArgs e)
        {
            string newTaskDescription = TaskTextBox.Text;
            if (!string.IsNullOrWhiteSpace(newTaskDescription))
            {
                Tasks.Add(new TaskItem { Description = newTaskDescription });
                TaskTextBox.Clear();
            }
            else
            {
                MessageBox.Show("Please enter task description.");
            }
        }

        private void RemoveSelected_Click(object sender, RoutedEventArgs e)
        {
            var itemsToRemove = new ObservableCollection<TaskItem>();

            foreach (var task in Tasks)
            {
                var listBoxItem = (ListBoxItem)TaskListBox.ItemContainerGenerator.ContainerFromItem(task);
                var checkBox = FindVisualChild<CheckBox>(listBoxItem);

                if (checkBox.IsChecked == true)
                {
                    itemsToRemove.Add(task);
                }
            }

            foreach (var itemToRemove in itemsToRemove)
            {
                Tasks.Remove(itemToRemove);
            }
        }

        private static T FindVisualChild<T>(DependencyObject parent) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(parent, i);
                if (child != null && child is T)
                {
                    return (T)child;
                }
                else
                {
                    T childOfChild = FindVisualChild<T>(child);
                    if (childOfChild != null)
                    {
                        return childOfChild;
                    }
                }
            }
            return null;
        }

        private void TaskTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }

    public class TaskItem : INotifyPropertyChanged
    {
        private string _description;
        public string Description
        {
            get { return _description; }
            set
            {
                if (_description != value)
                {
                    _description = value;
                    OnPropertyChanged("Description");
                }
            }
        }

        private bool _isCompleted;
        public bool IsCompleted
        {
            get { return _isCompleted; }
            set
            {
                if (_isCompleted != value)
                {
                    _isCompleted = value;
                    OnPropertyChanged("IsCompleted");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

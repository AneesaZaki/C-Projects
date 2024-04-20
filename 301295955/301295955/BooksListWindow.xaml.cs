using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace _301295955
{
    /// <summary>
    /// Interaction logic for BooksListWindow.xaml
    /// </summary>
    public partial class BooksListWindow : Window
    {
        MainWindow parent;
        BookLibrary bookLibrary;
        public string userName;

        public ObservableCollection<Book> BookTitles { get; set; }

        public BooksListWindow(MainWindow parent)
        {
            InitializeComponent();
            this.parent = parent;
            DataContext = this;
            userName = parent.UsernameTextBox.Text;
            userName_lbl.Content = userName;
            bookLibrary = new BookLibrary();
            BookTitles = new ObservableCollection<Book>();
            Loaded += BooksList_Loaded;
        }
        private async void BooksList_Loaded(object sender, RoutedEventArgs e)
        {
            await loadDataAsync(userName);
        }
        public async Task loadDataAsync(string userName)
        {
            try
            {
                var books = await bookLibrary.GetBooksByUserNameAsync(userName);

                if (books != null && books.Any())
                {
                    // Sort the books by bookmark time in descending order (most recent first)
                    var sortedBooks = books.OrderByDescending(b => b.BookmarkTime).ToList();

                    // Iterate through the sorted books and add to the ObservableCollection
                    int numberOfBooks = books.Count();
                    foreach (var book in sortedBooks)
                    {
                        Book bookToAdd = new Book
                        {
                            BookTitle = book.BookTitle,
                            Author = book.Author,
                            UserName = book.UserName,
                            ISBN = book.ISBN,
                            PdfTitle = book.PdfTitle
                        };

                        BookTitles.Add(bookToAdd);

                        if (BookTitles.Count >= numberOfBooks)
                        {
                            break;
                        }
                    }
                }
                else
                {
                    MessageBox.Show("No books found for the user.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }



        private void close_BookListWindow(object sender, EventArgs e)
        {
            parent.UsernameTextBox.Text = "";
            parent.PasswordBox.Password = string.Empty;
            Application.Current.MainWindow = this;
            this.Close();
            parent.Show();
        }



        private void BookTitle_btn_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            Book selectedBook = (sender as Button)?.DataContext as Book;

            if (selectedBook != null)
            {
                PDFViewer pdfViewer = new PDFViewer(this, selectedBook);
                Application.Current.MainWindow = pdfViewer;
                pdfViewer.Show();
                this.Hide();
            }
        }
    }


    public class ListToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is List<string> authors)
            {
                return string.Join(", ", authors);
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}


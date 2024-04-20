using System;
using System.Collections.Generic;
using System.IO;
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
    /// Interaction logic for PDFViewer.xaml
    /// </summary>
    public partial class PDFViewer : Window
    {
        private BooksListWindow bookListWindow;
        private Book selectedBook { get; set; }
        const string tableName = "Bookshelf";
        private BookLibrary bookLibrary;
        public PDFViewer(BooksListWindow booksListWindow, Book selectedBook)
        {
            InitializeComponent();
            this.bookListWindow = booksListWindow;
            this.selectedBook = selectedBook;
            bookLibrary = new BookLibrary();
            LoadPdfFromS3(selectedBook);

        }

        private async void LoadPdfFromS3(Book selectedBook)
        {
            Stream pdfStream = await bookLibrary.GetPdfStreamAsync(selectedBook);

            try
            {

                // Read the response stream into a memory stream
                MemoryStream _documentStream = new MemoryStream();
                pdfStream.CopyTo(_documentStream);

                // Set the position to the beginning of the memory stream
                _documentStream.Position = 1;

                // Load the PDF document from the memory stream
                pdfViewer.ItemSource = _documentStream;

                int lastPageNumber = bookLibrary.GetLastPageNumberForUser(selectedBook, tableName);

                // Set the PDF viewer's current page to the last page number
                pdfViewer.GotoPage(lastPageNumber);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading PDF: {ex.Message}");
            }
        }

        private async Task BookMark()
        {
            int pageNumber = pdfViewer.CurrentPageIndex;
            await bookLibrary.UpdateBookmarkAttributesAsync(selectedBook, pageNumber, tableName);
        }

        private async void BookMark_Click(object sender, RoutedEventArgs e)
        {
            await BookMark();
        }

        private async void Closed_PdfViewerWindow(object sender, EventArgs e)
        {
            await BookMark();
            bookListWindow.BookTitles.Clear();
            await bookListWindow.loadDataAsync(selectedBook.UserName);
            this.Close();
            bookListWindow.Show();
        }
    }
}


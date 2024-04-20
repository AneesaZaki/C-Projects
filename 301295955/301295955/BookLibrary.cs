using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.S3.Model;
using Amazon.S3;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Net;
using Amazon;

namespace _301295955
{
    public class BookLibrary
    {

        DynamoDBContext context = Connection.Context;
        public async Task<IEnumerable<Book>> GetBooksByUserNameAsync(string userName)
        {
            var scan = context.ScanAsync<Book>(new List<ScanCondition>
            {
                new ScanCondition("UserName", ScanOperator.Equal, userName)
            });

            var scanResponse = await scan.GetRemainingAsync();

            return scanResponse;
        }

        public int GetLastPageNumberForUser(Book book, String tableName)
        {
            try
            {
                // Initialize DynamoDB context and table
                Table table = Connection.ContentTableLoad(Connection.dynamoClient, tableName);
                // Define the query filter to retrieve the user data
                QueryFilter filter = new QueryFilter();
                filter.AddCondition("ISBN", QueryOperator.Equal, book.ISBN);
                filter.AddCondition("UserName", QueryOperator.Equal, book.UserName);

                // Execute the query to retrieve the user's data
                Search search = table.Query(filter);
                var user = search.GetNextSetAsync().Result.FirstOrDefault();

                if (user != null)
                {
                    // Check if the user document contains the "BookmarkPage" attribute
                    if (user.Contains("BookmarkPage"))
                    {
                        int lastPageNumber = Convert.ToInt32(user["BookmarkPage"]);
                        return lastPageNumber;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error retrieving last page number: {ex.Message}");
            }
            return 0;
        }

        public async Task UpdateBookmarkAttributesAsync(Book book, int lastPageNumber, string tableName)
        {
            try
            {
                // Load the existing item
                Book existingBook = await context.LoadAsync<Book>(book.ISBN, book.UserName);
                if (existingBook != null)
                {
                    // Update attributes
                    existingBook.BookmarkPage = lastPageNumber;
                    string bookmarkTimeStr = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
                    existingBook.BookmarkTime = bookmarkTimeStr;
                    // Save the updated item
                    await context.SaveAsync(existingBook);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating attributes: {ex.Message}");
            }
        }

        public async Task<Stream> GetPdfStreamAsync(Book selectedBook)
        {
            using (AmazonS3Client s3Client = new AmazonS3Client("AKIAWT6J3RGSA26XPIXN", "hjQXQoeTFVpshBQCPtX0h04dq7yiRs0ipBHieqma", RegionEndpoint.CACentral1))
                try
            {
                    string pdfTitle = selectedBook.PdfTitle;
                    GetObjectRequest request = new GetObjectRequest
                {
                    BucketName = "comp306lab2bucket",
                    Key = "Book2" 
                    };

                GetObjectResponse response = await Connection.s3Client.GetObjectAsync(request);

                Stream responseStream = response.ResponseStream;
                return responseStream;
            }
            catch (AmazonS3Exception ex)
            {
                Console.WriteLine($"Amazon S3 Error: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                throw;
            }
        }
    }
}



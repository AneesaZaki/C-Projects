using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using Amazon.DynamoDBv2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Amazon.DynamoDBv2.DocumentModel;
using Table = Amazon.DynamoDBv2.DocumentModel.Table;


namespace _301295955
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private User user;
        const string tableName = "Users";
        Table userTable;

        public MainWindow()
        {
            InitializeComponent();
        }


        private async void Load_MainWindows(object sender, RoutedEventArgs e)
        {
            user = new User();
            try
            {
                await CreateUserTable();
                await InsertUserCredentials();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private static async Task CreateUserTable()
        {
            List<string> currentTable = Connection.dynamoClient.ListTablesAsync().Result.TableNames;
            if (!currentTable.Contains(tableName))
            {
                Console.WriteLine("\n*** Creating table ***");
                var response = await Connection.dynamoClient.CreateTableAsync(new CreateTableRequest
                {
                    AttributeDefinitions = new List<AttributeDefinition>
                {
                new AttributeDefinition
                {
                    AttributeName = "UserName",
                    AttributeType = ScalarAttributeType.S
                }
                },
                    KeySchema = new List<KeySchemaElement>
                {
                new KeySchemaElement
                {
                    AttributeName = "UserName",
                    KeyType = KeyType.HASH // Partition key
                }
                },
                    ProvisionedThroughput = new ProvisionedThroughput
                    {
                        ReadCapacityUnits = 1,
                        WriteCapacityUnits = 1
                    },
                    TableName = tableName
                });

                var tableDescription = response.TableDescription;
                Console.WriteLine($"{tableDescription.TableName}: {tableDescription.TableStatus} \t " +
                                  $"ReadsPerSec: {tableDescription.ProvisionedThroughput.ReadCapacityUnits} \t " +
                                  $"WritesPerSec: {tableDescription.ProvisionedThroughput.WriteCapacityUnits}");

                Console.WriteLine($"{tableName} - {tableDescription.TableStatus}");

                await WaitUntilTableReady(tableName);
            }
        }


        private static async Task WaitUntilTableReady(string tableName)
        {
            string status = null;
            // Wait until table is created. Call DescribeTable.
            do
            {
                Thread.Sleep(3000); // Wait  seconds.
                try
                {
                    var res = await Connection.dynamoClient.DescribeTableAsync(new DescribeTableRequest
                    {
                        TableName = tableName
                    });

                    Console.WriteLine($"Table name: {res.Table.TableName}, status: {res.Table.TableStatus}");
                    status = res.Table.TableStatus;
                }
                catch (ResourceNotFoundException ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            } while (status != "ACTIVE");
        }
        private static async Task InsertUserCredentials()
        {
            var context = new DynamoDBContext(Connection.dynamoClient);

            // User credentials
            var users = new List<User>
    {
        new User
        {
            UserName = "aneesa@yahoo.com",
            Password = "121212"
        },
        new User
        {
            UserName = "ghutai@yahoo.com",
            Password = "121212"
        },
        new User
        {
            UserName = "lab2@yahoo.com",
            Password = "121212"
        }
    };

            foreach (var user in users)
            {
                try
                {
                    // Check if the user already exists
                    var existingUser = await context.LoadAsync<User>(user.UserName);
                    if (existingUser == null)
                    {
                        await context.SaveAsync(user);
                        Console.WriteLine($"User {user.UserName} inserted successfully.");
                    }
                    else
                    {
                        Console.WriteLine($"User {user.UserName} already exists in the table.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error inserting user {user.UserName}: {ex.Message}");
                }
            }
        }

        private async void Login_btn_Click(object sender, RoutedEventArgs e)
        {
            userTable = Connection.ContentTableLoad(Connection.dynamoClient, tableName);
            string userName = UsernameTextBox.Text.Trim();
            string password = PasswordBox.Password;
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Fields can't be empty!", "Alert", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else
            {
                Document doc = await userTable.GetItemAsync(userName);

                if (doc != null)
                {
                    string username = doc["UserName"].AsString();
                    string userPassword = doc["Password"].AsString();

                    if (password == userPassword)
                    {
                        BooksListWindow bookListWindow = new BooksListWindow(this);
                        Application.Current.MainWindow = bookListWindow;
                        MessageBox.Show("Successfully Logged In");
                        bookListWindow.Show();
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("Incorrect Email or Password entered!", "Alert", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    }
                }
                else
                {
                    MessageBox.Show("Email not found!", "Alert", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }

        }
    }
}

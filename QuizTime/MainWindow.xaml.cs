using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
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
using QuizTime.DataModel;
using System.IO;
using Newtonsoft.Json;
using JsonNetSerializer = Newtonsoft.Json.JsonSerializer;
using SystemTextJsonSerializer = System.Text.Json.JsonSerializer;
using System.Linq.Expressions;

namespace QuizTime
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Quiz quiz;
        private Window quizWindow;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void StartQuiz_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string sourceFilePath = @"C:\Users\Philip\source\repos\QuizTime\QuizTime\MyQuizGame.json"; // Ensure this points to the correct JSON file
                string destinationFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                string destinationFilePath = System.IO.Path.Combine(destinationFolderPath, "MyQuizGame.json");

                if (!File.Exists(destinationFilePath))
                {
                    Directory.CreateDirectory(destinationFolderPath); // Ensure folder exists
                    File.Copy(sourceFilePath, destinationFilePath);
                }

                string jsonData = File.ReadAllText(destinationFilePath);

                // Check if the JSON data is valid and not empty
                if (!string.IsNullOrWhiteSpace(jsonData))
                {
                    List<Question> questions = JsonConvert.DeserializeObject<List<Question>>(jsonData);

                    quiz = new Quiz();
                    foreach (var question in questions)
                    {
                        quiz.AddQuestion(question.Statement, question.CorrectAnswer, question.Option1, question.Option2, question.Option3);
                    }

                    PlayQuiz playQuiz = new PlayQuiz();
                    playQuiz.LoadQuiz(quiz);

                    quizWindow = new Window
                    {
                        Title = "PlayQuiz",
                        Content = playQuiz,
                        Width = 800,
                        Height = 450,
                        WindowStartupLocation = WindowStartupLocation.CenterScreen
                    };

                    quizWindow.Closed += QuizWindow_Closed;

                    Hide();
                    quizWindow.Show();
                }
                else
                {
                    MessageBox.Show("Quiz file is empty or contains invalid JSON data.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading quiz: " + ex.Message);
                Console.WriteLine("Error loading quiz: " + ex.Message);
            }
        }

        private void EditQuiz_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string sourceFilePath = @"C:\Users\Philip\source\repos\QuizTime\QuizTime\MyQuizGame.json";
                string destinationFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                string destinationFilePath = System.IO.Path.Combine(destinationFolderPath, "MyQuizGame.json");

                if (!File.Exists(destinationFilePath))
                {
                    Directory.CreateDirectory(destinationFolderPath); // Ensure folder exists
                    File.Copy(sourceFilePath, destinationFilePath);
                }

                string jsonData = File.ReadAllText(destinationFilePath);

                // Check if the JSON data is valid and not empty
                if (!string.IsNullOrWhiteSpace(jsonData))
                {
                    List<Question> questions = System.Text.Json.JsonSerializer.Deserialize<List<Question>>(jsonData);

                    quiz = new Quiz();
                    foreach (var question in questions)
                    {
                        quiz.AddQuestion(question.Statement, question.CorrectAnswer, question.Option1, question.Option2, question.Option3);
                    }

                    EditQuiz editQuiz = new EditQuiz();
                    editQuiz.LoadQuestions();

                    Window editWindow = new Window
                    {
                        Title = "Edit Quiz",
                        Content = editQuiz,
                        Width = 800,
                        Height = 450,
                        WindowStartupLocation = WindowStartupLocation.CenterScreen
                    };

                    editWindow.Closed += EditWindow_Closed;

                    Hide();
                    editWindow.Show();
                }
                else
                {
                    MessageBox.Show("Quiz file is empty or contains invalid JSON data.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading quiz for editing: " + ex.Message);
                Console.WriteLine("Error loading quiz for editing: " + ex.Message);
            }
        }


        private void QuizWindow_Closed(object sender, EventArgs e)
        {
            this.Show();
        }

        private void EditWindow_Closed(object sender, EventArgs e)
        {
            this.Show();
        }
    }
}
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
                string appName = "MyQuizGame.json";
                string appDataFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                string jsonFilePath = System.IO.Path.Combine(appDataFolderPath, appName);

                // Check if the JSON file doesn't exist in the local app data folder
                if (!File.Exists(jsonFilePath))
                {
                    // Specify the path of the source JSON file to copy
                    string sourceFilePath = @"C:\Users\Philip\source\repos\QuizTime\QuizTime\MyQuizGame.json";


                    // Copy the source file to the local app data folder
                    System.IO.File.Copy(sourceFilePath, jsonFilePath);

                    // Now the file should exist in the local app data folder
                }

                // Proceed with loading the quiz
                string jsonData = File.ReadAllText(jsonFilePath);
                List<Question> questions = System.Text.Json.JsonSerializer.Deserialize<List<Question>>(jsonData);

                QuizTime.DataModel.Quiz quiz = new QuizTime.DataModel.Quiz();
                foreach (var question in questions)
                {
                    quiz.AddQuestion(question.Statement, question.CorrectAnswer, question.Option1, question.Option2, question.Option3);
                }

                Console.WriteLine("Quiz loaded successfully!");

                PlayQuiz playQuiz = new PlayQuiz();
                playQuiz.LoadQuiz(quiz);

                Window quizWindow = new Window
                {
                    Title = "PlayQuiz",
                    Content = playQuiz,
                    Width = 800,
                    Height = 450,
                    WindowStartupLocation = WindowStartupLocation.CenterScreen
                };

                quizWindow.Closed += QuizWindow_Closed;

                this.Hide();
                quizWindow.Show();
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
                string appName = "MyQuizGame.json";
                string sourceFilePath = @"C:\Users\Philip\source\repos\QuizTime\QuizTime\" + appName;
                string destinationFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

                string destinationFilePath = System.IO.Path.Combine(destinationFolderPath, appName);

                if (!File.Exists(destinationFilePath))
                {
                    File.Copy(sourceFilePath, destinationFilePath);
                }

                string jsonData = File.ReadAllText(destinationFilePath);
                List<Question> questions = System.Text.Json.JsonSerializer.Deserialize<List<Question>>(jsonData);


                QuizTime.DataModel.Quiz quiz = new QuizTime.DataModel.Quiz();
                foreach (var question in questions)
                {
                    quiz.AddQuestion(question.Statement, question.CorrectAnswer, question.Option1, question.Option2, question.Option3);
                }

                Console.WriteLine("Quiz loaded successfully for editing!");

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
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using QuizTime.DataModel;

namespace QuizTime
{
    public partial class MainWindow : Window
    {
        private Quiz quiz;

        public MainWindow()
        {
            InitializeComponent();
        }

        //Starting the quiz and loading the questions.
        private void StartQuiz_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string appDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                string quizFilePath = Path.Combine(appDirectory, "MyQuizGame.json");

                string appDataFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                string destinationFilePath = Path.Combine(appDataFolderPath, "MyQuizGame.json");

                if (!File.Exists(destinationFilePath))
                {
                    File.Copy(quizFilePath, destinationFilePath);
                    MessageBox.Show("Quiz file created in AppData\\Local.");
                }
                else
                {
                    string jsonData = File.ReadAllText(destinationFilePath);
                    List<Question> questions = JsonSerializer.Deserialize<List<Question>>(jsonData);

                    Quiz quiz = new Quiz();
                    foreach (var question in questions)
                    {
                        quiz.AddQuestion(question.Statement, question.CorrectAnswer, question.Option1, question.Option2, question.Option3);
                    }

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

                    Hide();
                    quizWindow.Show();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        //Editing the quiz and loading the questions.
        private void EditQuiz_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string appDataFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                string destinationFilePath = Path.Combine(appDataFolderPath, "MyQuizGame.json");

                if (!File.Exists(destinationFilePath))
                {
                    string appDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                    string quizFilePath = Path.Combine(appDirectory, "MyQuizGame.json");

                    File.Copy(quizFilePath, destinationFilePath);
                    MessageBox.Show("Quiz file created in AppData\\Local.");
                }

                string jsonData = File.ReadAllText(destinationFilePath);
                List<Question> questions = JsonSerializer.Deserialize<List<Question>>(jsonData);

                Quiz quiz = new Quiz();
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
            catch (Exception ex)
            {
                MessageBox.Show("Error loading quiz for editing: " + ex.Message);
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

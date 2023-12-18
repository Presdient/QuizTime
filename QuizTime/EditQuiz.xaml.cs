using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
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
using Newtonsoft.Json;
using QuizTime.DataModel;

namespace QuizTime
{
    public partial class EditQuiz : UserControl
    {
        private List<Question> loadedQuestions;
        private int currentQuestionIndex = -1;

        public EditQuiz()
        {
            InitializeComponent();
            LoadQuestions(); 
        }

        //Saving changes on a question.
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (currentQuestionIndex >= 0 && currentQuestionIndex < loadedQuestions.Count)
                {
                    Question currentQuestion = loadedQuestions[currentQuestionIndex];
                    currentQuestion.Statement = txtStatement.Text;
                    currentQuestion.Option1 = txtOption1.Text;
                    currentQuestion.Option2 = txtOption2.Text;
                    currentQuestion.Option3 = txtOption3.Text;
                    currentQuestion.CorrectAnswer = txtCorrectAnswer.Text;

                    string json = JsonConvert.SerializeObject(loadedQuestions);
                    File.WriteAllText(GetFilePath(), json); 

                    MessageBox.Show("Question saved successfully!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving question: " + ex.Message);
            }
        }

        //Moving to the previous question.
        private void Previous_Click(object sender, RoutedEventArgs e)
        {
            if (currentQuestionIndex > 0)
            {
                currentQuestionIndex--;
                LoadAndDisplayCurrentQuestion();
            }
        }

        //Moving to the next question.
        private void Next_Click(object sender, RoutedEventArgs e)
        {
            if (currentQuestionIndex < loadedQuestions.Count - 1)
            {
                currentQuestionIndex++;
                LoadAndDisplayCurrentQuestion();
            }
        }

        //Loading the jsonfile questions.
        public void LoadQuestions()
        {
            try
            {
                string jsonData = File.ReadAllText(GetFilePath());
                loadedQuestions = JsonConvert.DeserializeObject<List<Question>>(jsonData);

                currentQuestionIndex = 0;
                LoadAndDisplayCurrentQuestion();
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("The file was not found!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading questions: " + ex.Message);
            }
        }

        //Displaying the questions.
        private void LoadAndDisplayCurrentQuestion()
        {
            if (currentQuestionIndex >= 0 && currentQuestionIndex < loadedQuestions.Count)
            {
                Question currentQuestion = loadedQuestions[currentQuestionIndex];

                txtStatement.Text = currentQuestion.Statement;
                txtOption1.Text = currentQuestion.Option1;
                txtOption2.Text = currentQuestion.Option2;
                txtOption3.Text = currentQuestion.Option3;
                txtCorrectAnswer.Text = currentQuestion.CorrectAnswer;

                txtQuestionNumber.Text = $"{currentQuestionIndex + 1}/{loadedQuestions.Count}";
            }
        }

        //Finding the file path where the file is located at.
        private string GetFilePath()
        {
            string appName = "MyQuizGame.json";
            return System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), appName);
        }

        //Quit button functions.
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Window parentWindow = Window.GetWindow(this);
            parentWindow.Close();
        }
    }
}





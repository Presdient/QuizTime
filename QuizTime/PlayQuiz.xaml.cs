using QuizTime.DataModel;
using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace QuizTime
{
    public partial class PlayQuiz : UserControl
    {
        private QuizTime.DataModel.Quiz currentQuiz;
        private List<int> questionIndices;
        private int currentQuestionIndex = 0;
        private int questionsPerSession = 10;
        private int correctAnswersCount = 0;
        private bool answerSubmitted = false;
        private int questionsAnswered = 0;
        private int questions = 0;
        public PlayQuiz()
        {
            InitializeComponent();
            btnNext.IsEnabled = false;  
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            if (!answerSubmitted)
            {
                if (currentQuiz != null && currentQuestionIndex < currentQuiz.Questions.Count())
                {
                    if (currentQuestionIndex >= 0 &&
                        currentQuestionIndex < questionIndices.Count &&
                        currentQuestionIndex < currentQuiz.Questions.Count())
                    {
                        var selectedOption = optionsPanel.Children.OfType<RadioButton>().FirstOrDefault(r => r.IsChecked == true);
                        if (selectedOption != null)
                        {
                            string selectedAnswer = selectedOption.Content.ToString();
                            var currentQuestion = currentQuiz.Questions.ElementAtOrDefault(questionIndices[currentQuestionIndex]);

                            if (selectedAnswer == currentQuestion.CorrectAnswer)
                            {
                                resultText.Text = "Correct!";
                                resultText.Foreground = Brushes.Green;
                                correctAnswersCount++;
                            }
                            else
                            {
                                resultText.Text = $"Incorrect! Correct answer: {currentQuestion.CorrectAnswer}";
                                resultText.Foreground = Brushes.Red;
                            }

                            questionsAnswered++;

                            foreach (var radioButton in optionsPanel.Children.OfType<RadioButton>())
                            {
                                radioButton.IsEnabled = false;
                            }

                            answerSubmitted = true;
                            btnNext.IsEnabled = true;
                        }
                        else
                        {
                            MessageBox.Show("Please select an option!");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Index out of range!");
                    }
                }
                else
                {
                    MessageBox.Show("No quiz loaded or no more questions!");
                }
            }
            else
            {
                MessageBox.Show("Answer already submitted!");
            }
        }



        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            if (currentQuiz != null && currentQuestionIndex < currentQuiz.Questions.Count() - 1)
            {
                currentQuestionIndex++;
                LoadQuestion(currentQuestionIndex);
                resultText.Text = "";
            }
            else if (currentQuiz != null && currentQuestionIndex == questionIndices.Count - 1)
            {
                btnFinishQuiz.Visibility = Visibility.Visible;
                btnNext.Visibility = Visibility.Collapsed;
                currentQuestionIndex++;
                LoadQuestion(currentQuestionIndex);
                resultText.Text = "Plesae click 'Finish Quiz' to continue";
            }
            else
            {
                MessageBox.Show("No more questions!");
            }

            if (currentQuestionIndex >= questionsPerSession - 1)
            {
                btnNext.IsEnabled = false;
                btnFinishQuiz.Visibility = Visibility.Visible;
            }

            answerSubmitted = false;
            foreach (var radioButton in optionsPanel.Children.OfType<RadioButton>())
            {
                radioButton.IsEnabled = true;
            }

            questions = questionsAnswered;

            quizInfo.Text = $"Question: {questions + 1}/{questionsPerSession}, Correct Answers: {correctAnswersCount}";
        }

        private void ShowQuizResult()
        {
            MessageBox.Show($"You got a score of {correctAnswersCount} out of {questionsAnswered}.", "Quiz Result");

            MessageBoxResult result = MessageBox.Show("Do you want to play again?", "Quiz ended", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                ResetQuiz();
            }
            else
            {
                Window.GetWindow(this).Close();
            }
        }

        public void LoadQuiz(QuizTime.DataModel.Quiz quiz)
        {
            currentQuiz = quiz;
            GenerateNewQuestions();
            currentQuestionIndex = 0;
            LoadQuestion(currentQuestionIndex);
        }

        public void LoadQuestion(int index)
        {
            if (currentQuiz != null && index >= 0 && index < questionIndices.Count())
            {
                int questionIndex = questionIndices[index];
                var question = currentQuiz.Questions.ElementAtOrDefault(questionIndex);
                if (question != null)
                {
                    txtStatement.Text = question.Statement;
                    option1.Content = question.Option1;
                    option2.Content = question.Option2;
                    option3.Content = question.Option3;
                    option1.IsChecked = true;

                    quizInfo.Text = $"Question: {index + 1}/{questionsPerSession}, Correct Answers: {correctAnswersCount}";
                    btnNext.IsEnabled = false;

                    if (index < questionsPerSession - 1)
                    {
                        btnNext.IsEnabled = false;
                    }

                    btnNext.Visibility = (index < questionsPerSession - 1) ? Visibility.Visible : Visibility.Collapsed;
                }
                else
                {
                    MessageBox.Show("Question not found!");
                }
            }
        }

        private void GenerateNewQuestions()
        {
            if (currentQuiz != null)
            {
                Random rand = new Random();
                List<int> shuffledIndices = Enumerable.Range(0, currentQuiz.Questions.Count()).OrderBy(r => rand.Next()).ToList();

                questionIndices = shuffledIndices.Take(questionsPerSession).ToList();
            }
        }


        private void ResetQuiz()
        {
            currentQuestionIndex = 0;
            correctAnswersCount = 0;
            questionsAnswered = 0;
            GenerateNewQuestions();
            LoadQuestion(currentQuestionIndex);
            resultText.Text = "";
            quizInfo.Text = $"Questions Answered: {questionsAnswered}/{questionsPerSession}, Correct Answers: {correctAnswersCount}";

            answerSubmitted = false;
        }

        private void ReturnToMainMenu()
        {
            MainWindow mainWindow = new MainWindow();
            Application.Current.MainWindow = mainWindow;    
            mainWindow.Show();
            Window.GetWindow(this).Close();
        }

        private void btnFinishQuiz_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure that you want to finish the quiz?", "Finish Quiz", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes) 
            {
                ShowQuizResult();
            }
            else if (result == MessageBoxResult.No)
            {
                Window.GetWindow(this).Close();
            }
        }

        private void btnExitQuiz_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure that you want to exit the quiz?", "Exit Quiz", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                Window.GetWindow(this)?.Close();
            }
        }
    }
}


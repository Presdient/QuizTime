using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizTime.DataModel
{
    public class Quiz
    {
        private List<Question> _questions;
        private string _title = string.Empty;
        public IEnumerable<Question> Questions => _questions;
        public string Title => _title;

        public Quiz()
        {
            _questions = new List<Question>();
        }

        public Question GetRandomQuestion()
        {
            Random random = new Random();
            int index = random.Next(_questions.Count);
            return _questions[index];
        }

        public void AddQuestion(string statement, string correctAnswer, string option1, string option2, string option3)
        {
            Question newQuestion = new Question()
            {
                Statement = statement,
                CorrectAnswer = correctAnswer,
                Option1 = option1,
                Option2 = option2,
                Option3 = option3
            };
            _questions.Add(newQuestion);
        }

        public void RemoveQuestion(int index)
        {
            if (index >= 0 && index < _questions.Count)
            {
                _questions.RemoveAt(index);
            }
        }
    }
}
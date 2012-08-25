using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NHibernateFetchProblems.Models
{
    public class Question
    {
        public virtual int QuestionId { get; set; }

        public virtual string QuestionText { get; set; }

        public virtual Person AskedBy { get; set; }
        public virtual Person QuestionModifiedBy { get; set; }

        public virtual IList<QuestionComment> QuestionComments { get; set; }
        public virtual IList<Answer> Answers { get; set; }
    }


    public class QuestionComment
    {
        public virtual Question Question { get; set; }

        public virtual int QuestionCommentId { get; set; }
        
        public virtual string QuestionCommentText { get; set; }

        public virtual Person QuestionCommentBy { get; set; }
    }


    public class Answer
    {
        public virtual Question Question { get; set; }

        public virtual int AnswerId { get; set; }

        public virtual string AnswerText { get; set; }
        
        public virtual Person AnsweredBy { get; set; }
        public virtual Person AnswerModifiedBy { get; set; }

        public virtual IList<AnswerComment> AnswerComments { get; set; }
    }


    public class AnswerComment
    {
        public virtual Answer Answer { get; set; }

        public virtual int AnswerCommentId { get; set; }

        public virtual string  AnswerCommentText { get; set; }

        public virtual Person AnswerCommentBy { get; set; }
    }


    public class Person
    {
        public virtual int PersonId { get; set; }
        public virtual string PersonName { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using NHibernate.Linq;

using NHibernateFetchProblems.DbMapping;
using NHibernateFetchProblems.Models;

namespace NHibernateFetchProblems
{
    class Program
    {
        static void Main(string[] args)
        {

            //TestEF();

            //TestNhProb();
                                    
            TestNhOk();

            
            

            
            Console.ReadKey();
        }

        static void TestEF()
        {
            using (var db = new EfMapping())
            {
                var q = db.Set<Question>()

                    .Include("AskedBy")
                    .Include("QuestionModifiedBy")

                    .Include("QuestionComments")

                    .Include("Answers")
                        .Include("Answers.AnswerComments")

                    .Where(x => x.QuestionId == 1).Single();

                Console.WriteLine("{0}", q.QuestionText);

                Console.ReadKey();
                Console.WriteLine("{0}", q.AskedBy.PersonId);
                Console.ReadKey();
                Console.WriteLine("{0}", q.AskedBy.PersonName);
                Console.ReadKey();
                Console.WriteLine("Question Comments Count should be 2: {0}", q.QuestionComments.Count());
                Console.ReadKey();
                Console.WriteLine("Answers Count should be 3: {0}", q.Answers.Count());
                Console.ReadKey();
                Console.WriteLine("Answer #1 Comments Count should be 5: {0}", q.Answers[0].AnswerComments.Count());
                Console.ReadKey();
                Console.WriteLine("Answer #2 Comments Count should be 4: {0}", q.Answers[1].AnswerComments.Count());

                Console.ReadLine();

            }
        }






        private static void TestNhOk()
        {
            using (var sess = NhMapping.GetSessionFactory().OpenSession())
            {
                int id = 1;

                var q1 = sess.Query<Question>().Where(x => x.QuestionId == id);


                q1
                .FetchMany(x => x.QuestionComments)
                .ToFuture();


                q1
                .Fetch(x => x.AskedBy)
                .Fetch(x => x.QuestionModifiedBy)
                .FetchMany(x => x.Answers)                    
                .ToFuture();


                sess.Query<Answer>()                    
                .Where(x => x.Question.QuestionId == id)
                .FetchMany(x => x.AnswerComments)
                .ToFuture();
                




                var q = q1.ToFuture().Single();

                Console.WriteLine("{0}", q.QuestionText);

                Console.ReadKey();
                Console.WriteLine("{0}", q.AskedBy.PersonId);
                Console.ReadKey();
                Console.WriteLine("{0}", q.AskedBy.PersonName);
                Console.ReadKey();
                Console.WriteLine("Question Comments Count should be 2: {0}", q.QuestionComments.Count());
                Console.ReadKey();
                Console.WriteLine("Answers Count should be 3: {0}", q.Answers.Count());
                Console.ReadKey();
                Console.WriteLine("Answer #1 Comments Count should be 5: {0}", q.Answers[0].AnswerComments.Count());
                Console.ReadKey();
                Console.WriteLine("Answer #2 Comments Count should be 4: {0}", q.Answers[1].AnswerComments.Count());
                



                Console.ReadLine();
            }
        }


      


        private static void TestNhProb()
        {
            using (var sess = NhMapping.GetSessionFactory().OpenSession())
            {

                var q1 = sess.Query<Question>().Where(x => x.QuestionId == 1);


                q1
                .FetchMany(x => x.QuestionComments)
                .ToFuture();


                q1
                .Fetch(x => x.AskedBy)
                .Fetch(x => x.QuestionModifiedBy)
                .FetchMany(x => x.Answers)
                    .ThenFetchMany(x => x.AnswerComments) // Doesn't work as advertised. This causes duplicate/cartesianed rows
                .ToFuture();


                

                var q = q1.ToFuture().Single();

                Console.WriteLine("{0}", q.QuestionText);

                Console.ReadKey();
                Console.WriteLine("{0}", q.AskedBy.PersonId);
                Console.ReadKey();
                Console.WriteLine("{0}", q.AskedBy.PersonName);
                Console.ReadKey();
                Console.WriteLine("Question Comments Count should be 2: {0}", q.QuestionComments.Count());
                Console.ReadKey();
                Console.WriteLine("Answers Count should be 3: {0}", q.Answers.Count());
                Console.ReadKey();
                Console.WriteLine("Answer #1 Comments Count should be 5: {0}", q.Answers[0].AnswerComments.Count());
                Console.ReadKey();
                Console.WriteLine("Answer #2 Comments Count should be 4: {0}", q.Answers[1].AnswerComments.Count());

            }
        }
    }//Program

}

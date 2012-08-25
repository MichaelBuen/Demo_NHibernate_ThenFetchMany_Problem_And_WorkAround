using System;
using System.Collections.Generic;
using System.Linq;

using NHibernate;

using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Automapping;

using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;

using NHibernateFetchProblems.Models;

namespace NHibernateFetchProblems.DbMapping
{
    public static class NhMapping
    {
        private static ISessionFactory _isf = null;
        public static ISessionFactory GetSessionFactory()
        {
            if (_isf != null) return _isf;

            var cfg = new StoreConfiguration();

            var sessionFactory = Fluently.Configure()
              .Database(MsSqlConfiguration.MsSql2008.ShowSql().ConnectionString(                
                  "Server=localhost; Database=NhFetch; Trusted_Connection=true; MultipleActiveResultSets=true"
                  ))
              .Mappings(m =>
                m.AutoMappings
                  .Add(AutoMap.AssemblyOf<Person>(cfg)
                  .Conventions.Add<ReferenceConvention>()
                  .Override<Question>(x => x.HasMany(y => y.QuestionComments).KeyColumn("Question_QuestionId").Cascade.AllDeleteOrphan().Inverse())
                  .Override<Question>(x => x.HasMany(y => y.Answers).KeyColumn("Question_QuestionId").Cascade.AllDeleteOrphan().Inverse())
                  .Override<Answer>(x => x.HasMany(y => y.AnswerComments).KeyColumn("Answer_AnswerId").Cascade.AllDeleteOrphan().Inverse())
                  )
                )
              .BuildSessionFactory();


            _isf = sessionFactory;

            return _isf;
        }
    }


    public class StoreConfiguration : DefaultAutomappingConfiguration
    {
        readonly IList<Type> _objectsToMap = new List<Type>()
        {
            // whitelisted objects to map
            typeof(Person), typeof(Question), typeof(QuestionComment), typeof(Answer), typeof(AnswerComment)
        };
        public override bool IsId(FluentNHibernate.Member member)
        {
            // return base.IsId(member);
            return member.Name == member.DeclaringType.Name + "Id";
        }
        public override bool ShouldMap(Type type) { return _objectsToMap.Any(x => x == type); }


    }

    public class ReferenceConvention : IReferenceConvention
    {
        public void Apply(IManyToOneInstance instance)
        {
            instance.Column(
                instance.Name + "_" + instance.Property.PropertyType.Name + "Id");
        }
    }

}
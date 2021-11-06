using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bogus;
using PIM.Api.Data.Context;

namespace PIM.Shared.Builders.Base
{
    public abstract class BaseBuilder<T>
    {
        protected ApplicationContext BuilderContext;
        protected Faker Faker;
        protected T Model;

        protected BaseBuilder(ApplicationContext builderContext)
        {
            BuilderContext = builderContext;
            Faker = new Faker();
            ResetModel();
        }

        public abstract void ResetModel();
        public abstract T CreateInMemory();
        public abstract Task<T> CreateInDataBase();
    }
}

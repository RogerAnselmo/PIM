using Bogus;
using Microsoft.AspNetCore.Mvc;

namespace PIM.UnitTest
{
    public class GlobalSetup
    {
        protected Faker Faker;

        public GlobalSetup() => Faker = new Faker();
        public ObjectResult Result;
    }
}

using Bogus;
using Microsoft.AspNetCore.Mvc;
using PIM.Api.TransferObjects.Responses.Base;

namespace PIM.UnitTest
{
    public class GlobalSetup
    {
        protected Faker Faker;

        public GlobalSetup() => Faker = new Faker();
        public ObjectResult HttpResultResult;
        public BaseResponse ResponseResult;
    }
}

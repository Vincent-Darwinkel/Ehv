using System;
using Hobby_Service.Models;

namespace UnitTest.HobbyService.TestModels
{
    public class TestHobbyDto
    {
        public readonly HobbyDto Hobby = new HobbyDto
        {
            Uuid = Guid.Parse("cd1811d9-492a-438f-af88-e4f49c3ecb52"),
            Name = "Test"
        };

        public readonly HobbyDto Empty = new HobbyDto
        {

        };
    }
}
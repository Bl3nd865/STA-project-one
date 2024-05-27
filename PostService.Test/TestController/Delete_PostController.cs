using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PostService.API.Controllers;
using PostService.API.DTO;
using PostService.API.Models;
using PostService.API.Services;
using PostService.Test.Dummy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostService.Test.TestController
{
    public class Delete_PostController
    {
        private Mock<IMapper> _mapper;
        private Mock<IPostServices> _postServices;

        private const string id = "FBFF1432-05BC-4686-A888-90B86A70D07C";

        public Delete_PostController()
        {
            _mapper = new Mock<IMapper>();
            _postServices = new Mock<IPostServices>();
        }

        [Fact]
        public async Task Delete_PostController_200Ok()
        {
            // Arrange: Set up mock behavior and controller
            var data = new PostDummyData();
            var returnObj = data.GetAllPost().FirstOrDefault(p => p.Id == id);

            _postServices.Setup(service => service.DeleteAsync(id).Result).Returns(true);

            var controller = new PostController(_mapper.Object, _postServices.Object);

            // Act: Call the Delete method
            var res = await controller.Delete(id);

            // Assert: Check if the response is 200 OK
            var okResult = Assert.IsType<OkResult>(res);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
        }

        [Fact]
        public async Task Delete_PostController_400BadRequest()
        {
            // Arrange: Set up mock behavior and controller
            var data = new PostDummyData();
            var returnObj = data.GetAllPost().FirstOrDefault(p => p.Id == "");

            _postServices.Setup(service => service.DeleteAsync("").Result).Returns(false);

            var controller = new PostController(_mapper.Object, _postServices.Object);

            // Act: Call the Delete method with an invalid ID
            var res = await controller.Delete("");

            // Assert: Check if the response is 400 Bad Request
            var badRequestResult = Assert.IsType<BadRequestResult>(res);
            Assert.Equal(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
        }
    }
}

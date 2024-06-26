﻿using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PostService.API.Controllers;
using PostService.API.DTO;
using PostService.API.Models;
using PostService.API.Services;
using PostService.Test.Dummy;

namespace PostService.Test.TestController
{
    public class GetAll_PostController
    {
        private Mock<IMapper> _mapper;
        private Mock<IPostServices> _postServices;

        public GetAll_PostController()
        {
            _mapper = new Mock<IMapper>();
            _postServices = new Mock<IPostServices>();
        }

        [Fact]
        public async Task GetAll_PostController_NotEmpty()
        {
            // Arrange: Setup data and mock services
            var data = new PostDummyData();
            _postServices.Setup(service => service.GetAllAsync().Result).Returns(data.GetAllPost);
            _mapper.Setup(mapper => mapper.Map<IEnumerable<ResponsePostDTO>>(It.IsAny<IEnumerable<Post>>()));

            var controller = new PostController(_mapper.Object, _postServices.Object);

            // Act: Call the Get method
            var res = await controller.Get();

            // Assert: Check for OK result and non-null model
            var okResult = Assert.IsType<OkObjectResult>(res);
            var model = Assert.IsAssignableFrom<IEnumerable<ResponsePostDTO>>(okResult.Value);
            Assert.NotNull(model);
        }

        [Fact]
        public async Task GetAll_PostController_204NoContent()
        {
            // Arrange: Setup data and mock services
            var data = new PostDummyData();
            _postServices.Setup(service => service.GetAllAsync().Result).Returns(data.GetAllPost().Where(p => p.Id == ""));
            _mapper.Setup(mapper => mapper.Map<IEnumerable<ResponsePostDTO>>(It.IsAny<IEnumerable<Post>>()));

            var controller = new PostController(_mapper.Object, _postServices.Object);

            // Act: Call the Get method
            var res = await controller.Get();

            // Assert: Check for NoContent result
            var noContentResult = Assert.IsType<NoContentResult>(res);
            Assert.Equal(StatusCodes.Status204NoContent, noContentResult.StatusCode);
        }

        [Fact]
        public async Task GetAll_PostController_200Ok()
        {
            // Arrange: Setup data and mock services
            var data = new PostDummyData();
            _postServices.Setup(service => service.GetAllAsync().Result).Returns(data.GetAllPost);
            _mapper.Setup(mapper => mapper.Map<IEnumerable<ResponsePostDTO>>(It.IsAny<IEnumerable<Post>>()));

            var controller = new PostController(_mapper.Object, _postServices.Object);

            // Act: Call the Get method
            var res = await controller.Get();

            // Assert: Check for OK result
            var OkResult = Assert.IsType<OkObjectResult>(res);
            Assert.Equal(StatusCodes.Status200OK, OkResult.StatusCode);
        }
    }
}

using MapsterMapper;
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
    public class GetById_PostController
    {
        private Mock<IMapper> _mapper;
        private Mock<IPostServices> _postServices;

        private const string id = "FBFF1432-05BC-4686-A888-90B86A70D07C";

        public GetById_PostController()
        {
            _mapper = new Mock<IMapper>();
            _postServices = new Mock<IPostServices>();
        }

        [Fact]
        public async Task GetById_PostController_NotEmpty()
        {
            // Arrange: Setup data and mock services
            var data = new PostDummyData();
            var returnObj = data.GetAllPost().FirstOrDefault(p => p.Id == id);

            var postResponseDTO = new ResponsePostDTO
            {
                Id = id,
                Title = returnObj.Title,
                CreatedAt = returnObj.CreatedAt,
                UserName = returnObj.UserName
            };

            _postServices.Setup(service => service.GetByIdAsync(id).Result).Returns(returnObj);
            _mapper.Setup(mapper => mapper.Map<ResponsePostDTO>(It.IsAny<Post>())).Returns(postResponseDTO);

            var controller = new PostController(_mapper.Object, _postServices.Object);

            // Act: Call the GetById method
            var res = await controller.GetById(id);

            // Assert: Check for OK result and non-null model
            var okResult = Assert.IsType<OkObjectResult>(res);
            var model = Assert.IsAssignableFrom<ResponsePostDTO>(okResult.Value);
            Assert.NotNull(model);
        }

        [Fact]
        public async Task GetById_PostController_404NotFound()
        {
            // Arrange: Setup data and mock services
            var data = new PostDummyData();
            _postServices.Setup(service => service.GetByIdAsync("").Result).Returns(data.GetAllPost().FirstOrDefault(p => p.Id == ""));
            _mapper.Setup(mapper => mapper.Map<ResponsePostDTO>(It.IsAny<Post>()));

            var controller = new PostController(_mapper.Object, _postServices.Object);

            // Act: Call the GetById method with an invalid ID
            var res = await controller.GetById("");

            // Assert: Check for NotFound result
            var notFoundResult = Assert.IsType<NotFoundResult>(res);
            Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
        }

        [Fact]
        public async Task GetById_PostController_200Ok()
        {
            // Arrange: Setup data and mock services
            var data = new PostDummyData();
            _postServices.Setup(service => service.GetByIdAsync(id).Result).Returns(data.GetAllPost().FirstOrDefault(p => p.Id == id));
            _mapper.Setup(mapper => mapper.Map<ResponsePostDTO>(It.IsAny<Post>()));

            var controller = new PostController(_mapper.Object, _postServices.Object);

            // Act: Call the GetById method
            var res = await controller.GetById(id);

            // Assert: Check for OK result
            var okResult = Assert.IsType<OkObjectResult>(res);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
        }
    }
}

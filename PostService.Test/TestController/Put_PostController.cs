using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PostService.API.Controllers;
using PostService.API.DTO;
using PostService.API.Models;
using PostService.API.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostService.Test.TestController
{
    public class Put_PostController
    {
        private Mock<IMapper> _mapper;
        private Mock<IPostServices> _postServices;
        private const string id = "FBFF1432-05BC-4686-A888-90B86A70D07C";

        public Put_PostController()
        {
            _mapper = new Mock<IMapper>();
            _postServices = new Mock<IPostServices>();
        }

        [Fact]
        public async Task Put_PostController_200Ok()
        {
            // Arrange: Create DTO and model for the test
            var updatePostDTO = new UpdatePostDTO
            {
                Id = id,
                Title = "test2",
                UserName = "test2",
                CreatedAt = DateTime.Now
            };

            var post = new Post
            {
                Id = updatePostDTO.Id,
                Title = updatePostDTO.Title,
                UserName = updatePostDTO.UserName,
                CreatedAt = updatePostDTO.CreatedAt
            };

            var responsePostDTO = new ResponsePostDTO
            {
                Id = post.Id,
                Title = post.Title,
                UserName = post.UserName,
                CreatedAt = post.CreatedAt
            };

            // Mock service and mapper behavior
            _postServices.Setup(service => service.EditAsync(post).Result).Returns(post);
            _mapper.Setup(mapper => mapper.Map<Post>(It.IsAny<UpdatePostDTO>())).Returns(post);
            _mapper.Setup(mapper => mapper.Map<ResponsePostDTO>(It.IsAny<Post>())).Returns(responsePostDTO);

            var controller = new PostController(_mapper.Object, _postServices.Object);

            // Act: Call the Put method
            var res = await controller.Put(updatePostDTO);

            // Assert: Check if the response is 200 OK
            var result = Assert.IsType<OkObjectResult>(res);
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        }

        [Fact]
        public async Task Put_PostController_400BadRequest()
        {
            // Arrange: Create DTO and model for the bad request test
            var updatePostDTO = new UpdatePostDTO
            {
                Id = id,
                Title = "test",
                UserName = "test",
                CreatedAt = DateTime.Now
            };
            var post = new Post
            {
                Id = updatePostDTO.Id,
                Title = updatePostDTO.Title,
                UserName = updatePostDTO.UserName,
                CreatedAt = updatePostDTO.CreatedAt
            };

            // Do not mock mapper to simulate failure
            var controller = new PostController(_mapper.Object, _postServices.Object);

            // Act: Call the Put method
            var res = await controller.Put(updatePostDTO);

            // Assert: Check if the response is 400 Bad Request
            var result = Assert.IsType<BadRequestResult>(res);
            Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);
        }

        [Fact]
        public void Put_PostController_Validation()
        {
            // Arrange: Create DTO with invalid data
            var updatePostDTO = new UpdatePostDTO
            {
                Id = id,
                Title = null,
                UserName = "test",
                CreatedAt = DateTime.Now
            };

            // Act: Validate the DTO
            var validationContext = new ValidationContext(updatePostDTO);
            var validationResult = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(updatePostDTO, validationContext, validationResult, true);

            // Assert: Check if the validation failed and contains the expected error message
            Assert.False(isValid);
            Assert.Contains(validationResult, vr => vr.ErrorMessage == "The Title field is required.");
        }
    }
}

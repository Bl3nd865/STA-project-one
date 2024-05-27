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
    public class Post_PostController
    {
        private Mock<IMapper> _mapper;
        private Mock<IPostServices> _postServices;

        public Post_PostController()
        {
            _mapper = new Mock<IMapper>();
            _postServices = new Mock<IPostServices>();
        }

        [Fact]
        public async Task Post_PostController_200Ok()
        {
            // Arrange: Create DTO and model for the test
            var createPostDTO = new CreatePostDTO
            {
                Id = Guid.NewGuid().ToString(),
                Title = "test",
                UserName = "test",
                CreatedAt = DateTime.Now
            };

            var post = new Post
            {
                Id = createPostDTO.Id,
                Title = createPostDTO.Title,
                UserName = createPostDTO.UserName,
                CreatedAt = createPostDTO.CreatedAt
            };

            var responsePostDTO = new ResponsePostDTO
            {
                Id = post.Id,
                Title = post.Title,
                UserName = post.UserName,
                CreatedAt = post.CreatedAt
            };

            // Mock service and mapper behavior
            _postServices.Setup(service => service.AddAsync(post).Result).Returns(post);
            _mapper.Setup(mapper => mapper.Map<Post>(It.IsAny<CreatePostDTO>())).Returns(post);
            _mapper.Setup(mapper => mapper.Map<ResponsePostDTO>(It.IsAny<Post>())).Returns(responsePostDTO);

            var controller = new PostController(_mapper.Object, _postServices.Object);

            // Act: Call the Post method
            var res = await controller.Post(createPostDTO);

            // Assert: Check if the response is 201 Created
            var result = Assert.IsType<CreatedAtActionResult>(res);
            Assert.Equal(StatusCodes.Status201Created, result.StatusCode);
        }

        [Fact]
        public async Task Post_PostController_400BadRequest()
        {
            // Arrange: Create DTO and model for the bad request test
            var createPostDTO = new CreatePostDTO
            {
                Id = Guid.NewGuid().ToString(),
                Title = "test",
                UserName = "test",
                CreatedAt = DateTime.Now
            };
            var post = new Post
            {
                Id = createPostDTO.Id,
                Title = createPostDTO.Title,
                UserName = createPostDTO.UserName,
                CreatedAt = createPostDTO.CreatedAt
            };

            // Do not mock mapper to simulate failure
            var controller = new PostController(_mapper.Object, _postServices.Object);

            // Act: Call the Post method
            var res = await controller.Post(createPostDTO);

            // Assert: Check if the response is 400 Bad Request
            var result = Assert.IsType<BadRequestResult>(res);
            Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);
        }

        [Fact]
        public void Post_PostController_Validation()
        {
            // Arrange: Create DTO with invalid data
            var createPostDTO = new CreatePostDTO
            {
                Id = Guid.NewGuid().ToString(),
                Title = null,
                UserName = "test",
                CreatedAt = DateTime.Now
            };

            // Act: Validate the DTO
            var validationContext = new ValidationContext(createPostDTO);
            var validationResult = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(createPostDTO, validationContext, validationResult, true);

            // Assert: Check if the validation failed and contains the expected error message
            Assert.False(isValid);
            Assert.Contains(validationResult, vr => vr.ErrorMessage == "The Title field is required.");
        }
    }
}

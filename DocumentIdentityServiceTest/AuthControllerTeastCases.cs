using DocumentIdentityService.Controllers;
using DocumentIdentityService.Models;
using DocumentIdentityService.Models.Dtos;
using DocumentIdentityService.Service;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentIdentityServiceTest
{
    public class AuthControllerTeastCases
    {
        private readonly Mock<IAuthService> _authService;
        private readonly AuthController _controller;
            
        public AuthControllerTeastCases()
        {
            _authService = new Mock<IAuthService>();     
            _controller = new AuthController(_authService.Object);
        }
        [Fact]
        public async void Login_User_Test_Case()
        {
            // Arrage
            var user = new LoginUser() { Email = "ganesh@gmail.com", Passward = "Ganesh@123", Role = "ADMIN" };
            _authService.Setup(u => u.LoginUser(user)).ReturnsAsync("sample-jwt-token");

            //Act
            var result = await _controller.LoginUser(user);

            //Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal("sample-jwt-token", okObjectResult.Value);
        }
        [Fact]
        public async void Login_User_Invalid_Test_Case()
        {
            var user = new LoginUser() { Email = "Test", Passward = "Test", Role = "Role" };
            _authService.Setup(u => u.LoginUser(user)).ReturnsAsync("Invalid Credentials!!!");

            var result = await _controller.LoginUser(user);

            var okObjectResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal("Invalid Credentials!!!", okObjectResult.Value);
        }

        [Fact]
        public async void Register_User_Test()
        {
            //arrange
            var user = new UserRegistration()
            {
                Email = "ganesh@gmail.com",
                Passward = "Ganesh@123",
                Role = "ADMIN"
            };
            _authService.Setup(u=> u.RegisterUser(user)).ReturnsAsync(true);

            // Act
            var result = await _controller.RegisterUser(user);
            //Assert
            var okObjecct = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(true, okObjecct.Value);
        }
        [Fact]
        public async void Register_User_Test_Where_NullorFail()
        {
            //Arrange
            var user = new UserRegistration()
            {
                Email = "Shakti@gmail.com",
                Passward = "Ganesh@123",
                Role = "ADMIN"
            };

            // Act

            _authService.Setup(u => u.RegisterUser(user)).ReturnsAsync(false);

            var result =await _controller.RegisterUser(user);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);

            Assert.Equal(false,okResult.Value);
        }
        [Fact]
        public async void Add_Memberas_Test_Success()
        {
            var user = new AddMember()
            {
                FirstName = "test_name",
                LastName = "test_lst",
                Email = "test@gmail.com", 
                Passward = "testpass",
                Phone = "11111111",
                Role = "TestRole"
            };

            _authService.Setup(u=>u.AddMember(user,"1")).ReturnsAsync(true);

            var result = await _controller.AddMemberas(user,"1");

            var okResult = Assert.IsType<OkObjectResult>(result.Result);

            Assert.Equal(true,okResult.Value);
        }
        [Fact]
        public async void Add_Memberas_Test_Failed()
        {
            // Arrange
            var user = new AddMember()
            {
                FirstName = "test_name",
                LastName = "test_lst",
                Email = "test@gmail.com",
                Passward = "testpass",
                Phone = "11111111",
                Role = "TestRole"
            };

            // Act
            _authService.Setup(u => u.AddMember(user, "10")).ReturnsAsync(false);

            var result = await _controller.AddMemberas(user, "10");

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(false, okResult.Value);
        }
        [Fact]
        public async void Get_Member_By_Email_Test_Success()
        {
            var email = "test@gmail.com";
            var user = new UserDto()
            {
                Email = email,
                Contact = "1234567890",
                FirstName = "Test Name",
                LastName = "Test Lst"
                
            };
            _authService.Setup(u => u.GetMemberByEmail(email)).ReturnsAsync(user);

            var result = await _controller.GetMemberByEmail(email);

            //var actionResult = Assert.IsType<ActionResult<UserDto>>(result);
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<UserDto>(okResult.Value);
            
            // Assert
            Assert.Equal(user.Email, returnValue.Email);
            Assert.Equal(user.FirstName, returnValue.FirstName);
            Assert.Equal(user.Contact, returnValue.Contact);
            Assert.Equal(user.LastName, returnValue.LastName);
        }
    }
}

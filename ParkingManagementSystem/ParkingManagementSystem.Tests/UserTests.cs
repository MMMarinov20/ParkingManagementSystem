using ParkingManagementSystem.DAL.Data;
using ParkingManagementSystem.DAL.Models;
using ParkingManagementSystem.DAL.Repositories;
using Microsoft.Data.SqlClient;
namespace ParkingManagementSystem.Tests
{
    [TestFixture]
    public class UserTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test, Order(1)]
        public void DatabaseConnectionOpensSuccessfuly()
        {
            DatabaseConnector databaseConnector = new DatabaseConnector();
            
            SqlConnection connection = databaseConnector.GetOpenConnection();

            Assert.IsNotNull(connection);
        }

        [Test, Order(2)]
        public async Task UserCreatesAccountSuccessfuly()
        {
            User user = new User();
            user.FirstName = "Test";
            user.LastName = "Test";
            user.Email = "test@email.com";
            user.PasswordHash = "Test123test";
            user.Phone = "123456789";

            DatabaseConnector databaseConnector = new DatabaseConnector();
            UserRepository userRepository = new UserRepository(databaseConnector);
            await userRepository.CreateUser(user);

            User userFromDatabase = await userRepository.GetUserByEmail(user.Email);

            Assert.IsNotNull(userFromDatabase);
        }

        [Test, Order(3)]
        public async Task GetsUserByIdSuccessfuly()
        {
            DatabaseConnector databaseConnector = new DatabaseConnector();
            UserRepository userRepository = new UserRepository(databaseConnector);

            User user = await userRepository.GetUserByIdAsync(1);

            Assert.IsNotNull(user);
        }

        [Test, Order(4)]
        public async Task GetsUserByEmailSuccessfuly()
        {
            DatabaseConnector databaseConnector = new DatabaseConnector();
            UserRepository userRepository = new UserRepository(databaseConnector);

            User user = await userRepository.GetUserByEmail("test@email.com");

            Assert.IsNotNull(user);
        }

        [Test, Order(5)]
        public async Task UserCanAuthenticateSuccessfuly()
        {
            DatabaseConnector databaseConnector = new DatabaseConnector();
            UserRepository userRepository = new UserRepository(databaseConnector);

            if (await userRepository.AuthenticateUser("test@email.com", "Test123test"))
            {
                Assert.Pass();
            }
            else Assert.Fail();
        }

        [Test, Order(6)]
        public async Task UserCanBeUpdatedSuccessfuly()
        {
            DatabaseConnector databaseConnector = new DatabaseConnector();
            UserRepository userRepository = new UserRepository(databaseConnector);
        
            User user = await userRepository.GetUserByEmail("test@email.com");
        
            user.LastName = "Test2";
        
            if (await userRepository.UpdateUser(user, "Test123test"))
            {
                Assert.Pass();
            }
            else Assert.Fail();
        }

        [Test, Order(7)]
        public async Task AllUsersCanBeFetchedSuccessfuly()
        {
            DatabaseConnector databaseConnector = new DatabaseConnector();
            UserRepository userRepository = new UserRepository(databaseConnector);

            List<User> users = await userRepository.GetAllUsers();

            Assert.IsNotNull(users);
        }

        [Test, Order(8)]
        public async Task UserCanBePromotedSuccessfuly()
        {
            DatabaseConnector databaseConnector = new DatabaseConnector();
            UserRepository userRepository = new UserRepository(databaseConnector);

            User user = await userRepository.GetUserByEmail("test@email.com");
            await userRepository.PromoteUser(user.UserID);

            User userFromDatabase = await userRepository.GetUserByIdAsync(user.UserID);

            Assert.That(userFromDatabase.IsAdmin == true);
        }

        //[Test]
        //public async Task UserCanBeDeletedSuccessfuly()
        //{
        //    DatabaseConnector databaseConnector = new DatabaseConnector();
        //    UserRepository userRepository = new UserRepository(databaseConnector);

        //    User user = await userRepository.GetUserByEmail("test@email.com");

        //    if (await userRepository.DeleteUser(user.UserID, "Test123test"))
        //    {
        //        Assert.Pass();
        //    }
        //    else Assert.Fail();
        //}
    }
}